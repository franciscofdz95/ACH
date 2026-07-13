using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Nmc.Ach.Dal;
using CommonUtility;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;


namespace BatchFileLoader
{
    public enum BatchFileErrorCode
    {
        GeneralError = 11000,
        DecryptionFailed = 11001,
        EmptyFile = 11002,
        ParseFailed = 11003,
        ParseErrors = 11004,
        LogBatchFailed = 11005,
        DuplicateCheckFailed = 11006,
        NoTransactions = 11007,
        ProcessingFailed = 11008,
        ArchiveFailed = 11009,
        DuplicateBatchFile = 11010,
    }

    public class BatchFileException : Exception
    {
        public BatchFileException()
        {
        }

        public BatchFileException(string message)
            : base(message)
        {
        }


        public BatchFileException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public BatchFileException(string message, BatchFileErrorCode errorCode)
            : base(message)
        {
            this._ErrorCode = errorCode;
        }

        public BatchFileException(string message, Exception inner, BatchFileErrorCode errorCode)
            : base(message, inner)
        {
            this._ErrorCode = errorCode;
        }

        private BatchFileErrorCode _ErrorCode = BatchFileErrorCode.GeneralError;

        public BatchFileErrorCode ErrorCode
        {
            get { return this._ErrorCode; }
        }
    }

    public abstract class BatchFile : iBatchFile 
    {
        protected string m_FileName;
        protected long m_UploadID;
        protected decimal m_FileTotal = 0;
        protected int m_TotalTransCount = 0;
        protected string m_Delimitor;
        protected int m_MerchantID;
        protected string m_MerchantAppUID;
        protected bool _Encrypted = false;
        protected List<Transaction> _Transactions = new List<Transaction>();
        protected MerchantApp _Merchant = null;
        protected string _FileExtension = ".ACH";

        public BatchFile()
        {

        }

        public BatchFile(string filename)
        {
            this.m_FileName = filename;

        }

        public BatchFile(string filename, bool encrypted, int zid)
        {
            this.m_FileName = filename;
            this.m_MerchantID = zid;
            this._Encrypted = encrypted;

            PaymentXP.DataObjects.DataMerchantApp data = PaymentXP.DataObjects.DataAccess.DataMerchantAppDao;
            this.Merchant = data.GetMerchantApp(this.MerchantID);
        }

        public int MerchantID
        {
            get { return m_MerchantID; }
            set { m_MerchantID = value; }
        }
        public string MerchantAppUID
        {
            get { return m_MerchantAppUID; }
            set { m_MerchantAppUID = value; }
        }

        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        public long UploadID
        {
            get
            {
                return m_UploadID;
            }

            set { m_UploadID = value; }
        }

        public bool Encrypted
        {
            get
            {
                return this._Encrypted;
            }
        }

        public decimal FileTotal
        {
            get { return m_FileTotal; }
            set { m_FileTotal = value; }
        }

        public int TotalTransCount
        {
            get { return m_TotalTransCount; }
            set { m_TotalTransCount = value; }
        }

        public MerchantApp Merchant
        {
            get
            {
                return this._Merchant;
            }
            set
            {
                this._Merchant = value;
            }
        }

        public string FileExtension
        {
            get
            {
                return this._FileExtension;
            }
        }

        public abstract bool ImportFile();

        public virtual string ParseFile()
        {
            string strLine = string.Empty;
            StringBuilder sb = new StringBuilder();
            int line = 0;

            try
            {
                using (StreamReader sr = new StreamReader(this.FileName))
                {
                    //determine if there is a header
                    strLine = sr.ReadLine();
                    line++;

                    //if the first line read is null then we assume that the file is empty. 
                    //we'll go ahead and throw our own custom exception so we can exit out 
                    //of this function and let the caller handle the exception
                    if (strLine == null)
                    {
                        throw new BatchFileException("Empty ACH file detected.", BatchFileErrorCode.EmptyFile);
                    }

                    string[] fields = strLine.Split(new char[] { '\t' }, StringSplitOptions.None);

                    switch (fields.Length)
                    {
                        case 9:
                        case 45:
                            m_Delimitor = "\t";
                            break;
                        default:
                            fields = strLine.Split(new string[] { "\",\"" }, StringSplitOptions.None);

                            switch (fields.Length)
                            {
                                case 9:
                                case 45:
                                    m_Delimitor = "\",\"";
                                    break;
                                default:
                                    sb.Append("File format is invalid (Column Count Error). Line: " + line.ToString() + "<br>");
                                    break;
                            }
                            break;
                    }

                    if (!DataLayer.IsNumeric(fields[1]))
                    {
                        strLine = sr.ReadLine();
                        line++;
                    }
                    //Continue to read until you reach end of file
                    while (strLine != null)
                    {
                        fields = strLine.Split(new string[] { m_Delimitor }, StringSplitOptions.None);

                        switch (fields.Length)
                        {
                            case 9:
                            case 45:
                                break;
                            default:
                                sb.Append("File format is invalid (Column Count Error). Line: " + line.ToString() + "<br>");
                                break;
                        }


                        if (string.IsNullOrEmpty(fields[0]))
                            sb.Append("File format is invalid (Description Missing). Line: " + line.ToString() + "<br>");


                        if (fields.Length > 9 && !string.IsNullOrEmpty(fields[16]) && !DataLayer.IsNumeric(fields[16]))
                            sb.Append("File format is invalid (ClientID must be an integer). Line: " + line.ToString() + "<br>");

                        //string[] fields = strLine.Split(new string[] { '"' + "," + '"' }, StringSplitOptions.None);

                        this.FileTotal += Math.Round(decimal.Parse(fields[6].Replace("\"", "")), 2);
                        this.TotalTransCount++;

                        //Read the next line
                        strLine = sr.ReadLine();
                        line++;
                    }
                }
            }
            catch (BatchFileException ex)
            {
                throw ex;
            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg += "Batch file " + this.FileName + " failed to load. \n\r";
                msg += "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("Parse Failed - " + this.FileName, msg, Program.m_From, Program.m_To);
                CommonUtility.Logger.Log(exc);

                sb.Append(exc.Message + "<br>");
            }

            return sb.ToString();
        }

        public virtual void ParseFile(string filePath)
        {
            string strLine = string.Empty;
            StringBuilder sb = new StringBuilder();
            int line = 0;

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    //determine if there is a header
                    strLine = sr.ReadLine();
                    line++;

                    //if the first line read is null then we assume that the file is empty. 
                    //we'll go ahead and throw our own custom exception so we can exit out 
                    //of this function and let the caller handle the exception
                    if (strLine == null)
                    {
                        BatchLog.Batch.ErrorFormat("Empty ACH file detected.");

                        throw new BatchFileException("Empty ACH file detected.", BatchFileErrorCode.EmptyFile);
                    }

                    string[] fields = strLine.Split(new char[] { '\t' }, StringSplitOptions.None);

                    switch (fields.Length)
                    {
                        case 9:
                        case 45:
                            m_Delimitor = "\t";
                            break;
                        default:
                            fields = strLine.Split(new string[] { "\",\"" }, StringSplitOptions.None);

                            switch (fields.Length)
                            {
                                case 9:
                                case 45:
                                    m_Delimitor = "\",\"";

                                    break;
                                default:
                                    BatchLog.Batch.ErrorFormat("Line {0} - File format is invalid (Column Count Error)", line);
                                    sb.Append("File format is invalid (Column Count Error). Line: " + line.ToString() + "<br>");
                                    break;
                            }
                            break;
                    }

                    if (!DataLayer.IsNumeric(fields[1]))
                    {
                        //this means the file came with a header. this is a poor way checking if we have a column header
                        //but it's been working fine. we'll skip to the next line if a column header was detected
                        strLine = sr.ReadLine();
                        line++;
                    }

                    //Continue to read until you reach end of file
                    while (strLine != null)
                    {
                        try
                        {
                            BatchLog.Batch.InfoFormat("Line {0} - Parsing line......", line);

                            fields = strLine.Split(new string[] { m_Delimitor }, StringSplitOptions.None);

                            string errors = this.ValidateTransactionRow(fields, line);

                            if (string.IsNullOrWhiteSpace(errors))
                            {
                                //passed row validation
                                ParseAndLoad(fields);

                                this.FileTotal += Math.Round(decimal.Parse(fields[6].Replace("\"", "")), 2);
                                this.TotalTransCount++;

                                BatchLog.Batch.InfoFormat("Line {0} - ACH transaction parsed.", line);
                            }
                            else
                            {
                                sb.Append(errors);
                            }

                            //Read the next line
                            strLine = sr.ReadLine();
                            line++;
                        }
                        catch(Exception ex)
                        {
                            BatchLog.Batch.ErrorFormat("A {0} has occurred while parsing line '{1}': {2}", ex.GetType().Name, line, ex.Message);
                            BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                            sb.AppendFormat("Line [{0}]: {1}<br>", line, ex.Message);

                            //Read the next line
                            strLine = sr.ReadLine();
                            line++;
                            BatchLog.Batch.ErrorFormat("Continued with the next transaction from Line [{0}].", line);
                        }
                    }
                }

                if (sb.Length > 0)
                {
                    //parsing had errors
                    throw new BatchFileException(sb.ToString(), BatchFileErrorCode.ParseErrors);
                }

                BatchLog.Batch.InfoFormat("Total transactions: {0}", this.TotalTransCount);
                BatchLog.Batch.InfoFormat("Total amount: {0}", this.FileTotal);
            }
            catch (BatchFileException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BatchLog.Batch.ErrorFormat("A {0} has occurred while parsing batch file '{1}': {2}", ex.GetType().Name, this.FileName, ex.Message);
                BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                throw new BatchFileException("Failed to parse batch file", ex.InnerException, BatchFileErrorCode.ParseFailed);
            }
        }

        protected virtual string ValidateTransactionRow(string[] rowColumns, int line)
        {
            StringBuilder sb = new StringBuilder();

            if (!(rowColumns.Length == 9 || rowColumns.Length == 45))
            {
                BatchLog.Batch.ErrorFormat("Line {0} - File format is invalid (Column Count Error)", line);
                sb.AppendFormat("File format is invalid (Column Count Error). Line: {0}<br>", line);
            }

            if (string.IsNullOrEmpty(rowColumns[0]))
            {
                BatchLog.Batch.ErrorFormat("Line {0} - File format is invalid (Description Missing)", line);
                sb.AppendFormat("File format is invalid (Description Missing). Line: {0}<br>", line);
            }


            if (rowColumns.Length > 9 && !string.IsNullOrEmpty(rowColumns[16]) && !DataLayer.IsNumeric(rowColumns[16]))
            {
                BatchLog.Batch.ErrorFormat("Line {0} - File format is invalid (ClientID must be an integer)", line);
                sb.AppendFormat("File format is invalid (ClientID must be an integer). Line: {0}<br>", line);
            }

            return sb.ToString();
        }

        protected virtual void ParseAndLoad(string[] rowColumns)
        {
            BatchLog.Batch.InfoFormat("Loading transaction.......");



            Transaction tran = new Transaction();
            tran.MerchantID = this.MerchantID;
            tran.MerchantAppUID = this.Merchant.MerchantAppUID;
            tran.Description = rowColumns[0].Replace("\"", "");
            tran.TransType = rowColumns[1].Replace("\"", "");
            tran.TransRoute = rowColumns[2].Replace("\"", "");
            tran.AccountNo = rowColumns[3].Replace("\"", "");
            tran.NameOnAccount = rowColumns[4].Replace("\"", "");
            tran.RefID = rowColumns[5].Replace("\"", "");
            tran.Amount = Math.Round(decimal.Parse(rowColumns[6].Replace("\"", "")), 2);
            tran.Secc = rowColumns[7].Replace("\"", "");
            tran.CompanyName = rowColumns[8].Replace("\"", "");

            if (rowColumns.Length > 9)
            {
                tran.BillingAddress = rowColumns[9].Replace("\"", "");
                tran.BillingCity = rowColumns[10].Replace("\"", "");
                tran.BillingState = rowColumns[11].Replace("\"", "");
                tran.BillingZip = rowColumns[12].Replace("\"", "");
                tran.BillingCountry = rowColumns[13].Replace("\"", "");
                tran.BillingEmail = rowColumns[14].Replace("\"", "");
                tran.BillingPhone = rowColumns[15].Replace("\"", "");
                tran.ClientID = rowColumns[16].Replace("\"", "");

                if (!string.IsNullOrEmpty(rowColumns[18].Replace("\"", "")))
                    tran.OriginalTransID = Convert.ToInt64(rowColumns[18].Replace("\"", ""));

                tran.ShippingAddress = rowColumns[19].Replace("\"", "");
                tran.ShippingAddress2 = rowColumns[20].Replace("\"", "");
                tran.ShippingCity = rowColumns[21].Replace("\"", "");
                tran.ShippingState = rowColumns[22].Replace("\"", "");
                tran.ShippingZip = rowColumns[23].Replace("\"", "");
                tran.ShippingCountry = rowColumns[24].Replace("\"", "");
                tran.CustomInfo1 = rowColumns[25].Replace("\"", "");
                tran.CustomInfo2 = rowColumns[26].Replace("\"", "");
                tran.CustomInfo3 = rowColumns[27].Replace("\"", "");
                tran.CustomInfo4 = rowColumns[28].Replace("\"", "");
                tran.CustomInfo5 = rowColumns[29].Replace("\"", "");
                tran.CustomInfo6 = rowColumns[30].Replace("\"", "");
                tran.CustomInfo7 = rowColumns[31].Replace("\"", "");
                tran.CustomInfo8 = rowColumns[32].Replace("\"", "");
                tran.CustomInfo9 = rowColumns[33].Replace("\"", "");
                tran.CustomInfo10 = rowColumns[34].Replace("\"", "");
                tran.CustomInfo11 = rowColumns[35].Replace("\"", "");
                tran.CustomInfo12 = rowColumns[36].Replace("\"", "");
                tran.CustomInfo13 = rowColumns[37].Replace("\"", "");
                tran.CustomInfo14 = rowColumns[38].Replace("\"", "");
                tran.CustomInfo15 = rowColumns[39].Replace("\"", "");
                tran.CustomInfo16 = rowColumns[40].Replace("\"", "");
                tran.CustomInfo17 = rowColumns[41].Replace("\"", "");
                tran.CustomInfo18 = rowColumns[42].Replace("\"", "");
                tran.CustomInfo19 = rowColumns[43].Replace("\"", "");
                tran.CustomInfo20 = rowColumns[44].Replace("\"", "");
            }

            this._Transactions.Add(tran);

            BatchLog.Batch.InfoFormat("Transaction loaded.");
        }

        private bool IsDuplicateFileName(FileInfo fi)
        {
            string strError = string.Empty;

            try
            {
                BatchLog.Batch.InfoFormat("Checking for duplicate file name '{0}'......", fi.Name);

                bool perform = false;
                DataBatchFileLog data = new DataBatchFileLog();
                ArrayList prms = new ArrayList();

                prms.Add(new SqlParameter("@FileName", fi.Name));
                prms.Add(new SqlParameter("@MerchantID", fi.Directory.Name));

                   using (SqlDataReader dr = data.Select(prms))
                {
                    if (dr.Read())
                        perform = true;

                    dr.Close();

                    BatchLog.Batch.InfoFormat("File name '{0}' is duplicate? {1}", fi.Name, perform);
                }

                return perform;
            }
            catch (Exception ex)
            {
                BatchLog.Batch.ErrorFormat("A {0} has occurred while checking for duplicate batch file '{1}': {2}", ex.GetType().Name, this.FileName, ex.Message);
                BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                throw new BatchFileException("Failed to verify duplicate batch file", ex.InnerException, BatchFileErrorCode.DuplicateCheckFailed);
            }
        }    

        private bool IsDuplicateAmountAndCount(FileInfo fi)
        {
            bool Perform = false;
            string strError = string.Empty;

            try
            {
                
                DataBatchFileLog data = new DataBatchFileLog();
                ArrayList prms = new ArrayList();               
                prms.Add(new SqlParameter("@MerchantID", fi.Directory.Name));
                prms.Add(new SqlParameter("@FileTotal", this.FileTotal));
                prms.Add(new SqlParameter("@TransactionCount", this.TotalTransCount));                           
                SqlDataReader dr = data.Select(prms);

                if (dr.Read())
                    Perform = true;

                dr.Close();
                dr = null;
                data = null;

                return Perform;
            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg += "Batch file " + this.FileName + " failed to load. \n\r";
                msg += "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("Check Duplicate Amount And Count Batch Failed - " + fi.Name, msg, Program.m_From, Program.m_To);
                CommonUtility.Logger.Log(exc);
                return false;
            }


        }

        public bool PassDuplicateFile(FileInfo fi)
        {
            string strError = string.Empty;
            // PXP-3237-Code modified for validate duplicate batch file basis on FileName,TotalAmount, tiotal transaction By Koshlendra on 06-11-2017 start
            
            AchTransactionFacade achTransFacade = new AchTransactionFacade();
            //PXP-4051 Code added for check duplicate file for same merchantID by Koshlendra
            if (achTransFacade.CheckforDuplicateBatchFile(fi.Name, this.FileTotal, this.TotalTransCount,this.MerchantID))
            {
                strError = "File has already been loaded.";
            }
            
            // PXP-3237-Code modified for validate duplicate batch file basis on FileName,TotalAmount, tiotal transaction By Koshlendra on 06-11-2017 End         
   
            if (!string.IsNullOrWhiteSpace(strError))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public long LogBatchFile(FileInfo fi)
        {
            long uploadId = 0;

            try
            {
                BatchLog.Batch.InfoFormat("Logging batch file.......");

                if (this.UploadID > 0)
                {
                    BatchLog.Batch.InfoFormat("Logging batch file [UploadID={0}] already logged", this.UploadID);
                }
                else
                {
                    DataBatchFileLog data = new DataBatchFileLog();
                    ArrayList prms = new ArrayList();

                    SqlParameter prm = new SqlParameter("@FileID", -1);
                    prm.Direction = ParameterDirection.Output;
                    prms.Add(prm);
                    prms.Add(new SqlParameter("@AchID", -1));
                    prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(fi.Directory.Name)));
                    prms.Add(new SqlParameter("@FileName", fi.Name));
                    prms.Add(new SqlParameter("@LoadDate", DateTime.Now));
                    prms.Add(new SqlParameter("@FileTotal", this.FileTotal));
                    prms.Add(new SqlParameter("@TransactionCount", this.TotalTransCount));

                    uploadId = data.Insert(prms);

                    this.m_UploadID = uploadId;

                    BatchLog.Batch.InfoFormat("Logging batch file [UploadID={0}] logged", uploadId);
                }
            }
            catch (Exception ex)
            {
                BatchLog.Batch.ErrorFormat("A {0} has occurred while logging batch file '{1}': {2}", ex.GetType().Name, this.FileName, ex.Message);
                BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                throw new BatchFileException("Failed to log batch file", ex.InnerException, BatchFileErrorCode.LogBatchFailed);
            }

            return uploadId;
        }

    }
}
