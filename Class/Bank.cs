using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using Nmc.Ach.Dal;
using CommonUtility;
using PaymentXP.BusinessObjects.Logging;

namespace AchSystem
{
    public abstract class Bank
    {
        public ILogging BankLog = new Log();

        private string m_SettleFileDirectory;
        private string m_SettlementFileName;
        private string m_BankName;
        private int m_BankID;
        private string m_CompanyName;
        private string m_ImmediateDestination;
        private string m_ImmediateOrigin;
        private string m_Symbol;
        private string m_DestinationName;
        private string m_OriginName;
        private string m_OriginatingTransRoute;
        private string m_OriginatingAccountNo;
        private List<Merchant> m_Merchants;
        private int m_CurrentJobID = 0;
        private string m_CompanyID;

        public Bank()
        {
            m_Merchants = new List<Merchant>();
        }

        public Bank(string bank, int bankid)
        {
            this.m_Merchants = new List<Merchant>();

            this.m_BankName = bank;
            this.m_BankID = bankid;

            this.SettleFileDirectory = ConfigurationManager.AppSettings["SettlementFileDirectory"].ToString();
            this.SettlementFileName = SettleFileDirectory + DateTime.Today.ToString("yyyyMMdd") + "01" + ".ACH";
        }
        public override string ToString()
        {
            return m_BankName;
        }
        public int CurrentJobID
        {
            get { return m_CurrentJobID; }
            set { m_CurrentJobID = value; }
        }

        public string SettleFileDirectory
        {
            get { return m_SettleFileDirectory; }
            set { m_SettleFileDirectory = value; }
        }

        public string SettlementFileName
        {
            get { return m_SettlementFileName; }
            set { m_SettlementFileName = value; }
        }

        public List<Merchant> Merchants
        {
            get { return m_Merchants; }
            set { m_Merchants = value; }
        }
        public int BankID
        {
            get { return m_BankID; }
            set { m_BankID = value; }
        }
        public string BankName
        {
            get { return m_BankName; }
            set { m_BankName = value; }
        }
        public string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value.PadLeft(7, '0'); }
        }
        public string ImmediateDestination
        {
            get { return m_ImmediateDestination; }
            set { m_ImmediateDestination = value; }
        }
        public string ImmediateOrigin
        {
            get { return m_ImmediateOrigin; }
            set { m_ImmediateOrigin = value; }
        }
        public string Symbol
        {
            get { return m_Symbol; }
            set { m_Symbol = value; }
        }
        public string DestinationName
        {
            get { return m_DestinationName; }
            set { m_DestinationName = value.PadRight(23, ' '); }
        }
        public string OriginName
        {
            get { return m_OriginName; }
            set { m_OriginName = value.PadRight(23, ' '); }
        }
        public string OriginatingTransRoute
        {
            get { return m_OriginatingTransRoute; }
            set { m_OriginatingTransRoute = value; }
        }
        public string OriginatingAccountNo
        {
            get { return m_OriginatingAccountNo; }
            set { m_OriginatingAccountNo = value; }
        }

        public string CompanyID
        {
            get { return m_CompanyID; }
            set { m_CompanyID = value; }
        }



        public string m_FTPCentral = ConfigurationManager.AppSettings["FTPCentral"].ToString();

        public List<Merchant> GetPendingBatchMerchants()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Get_Data";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
            SqlDataReader dr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());

            this.Merchants.Clear();

            while (dr.Read())
            {
                this.Merchants.Add(new Merchant(DataLayer.Field2Int(dr["AchID"]), DataLayer.Field2Str(dr["AchCoName"])));
            }

            dr.Close();

            return Merchants;
        }

        public List<Merchant> GetPendingJournalMerchants()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Journal_Get_Data";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
            SqlDataReader dr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());

            this.Merchants.Clear();

            Merchant LastMerchant = null;

            while (dr.Read())
            {
                if (LastMerchant == null || LastMerchant.MerchantID != DataLayer.Field2Int(dr["AchID"]))
                {
                    LastMerchant = new Merchant(DataLayer.Field2Int(dr["AchID"]), DataLayer.Field2Str(dr["AchCoName"]));
                    this.Merchants.Add(LastMerchant);
                }

                LastMerchant.Batches.Add(new Batch(DataLayer.Field2Int(dr["AchID"]),
                                                   DataLayer.Field2Int(dr["BatchID"]),
                                                   DataLayer.Field2Dbl(dr["Debit"]),
                                                   DataLayer.Field2Dbl(dr["Credit"]),
                                                   DataLayer.Field2Int(dr["DebitCount"]),
                                                   DataLayer.Field2Int(dr["CreditCount"]),
                                                   DataLayer.Field2Dbl(dr["OverDailyAmountLimitCount"]),
                                                   DataLayer.Field2Dbl(dr["OverItemAmount"])));

            }

            dr.Close();
            return Merchants;
        }
        public List<Merchant> GetPendingResponseFileMerchants()
        {
            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();

            prms.Add(new SqlParameter("@BankID", this.BankID));
            prms.Add(new SqlParameter("@CurrentDate", DateTime.Today));

            SqlDataReader dr = data.SelectResponseFileGetData(prms);

            this.Merchants.Clear();

            while (dr.Read())
            {
                this.Merchants.Add(new Merchant(DataLayer.Field2Int(dr["AchID"]), DataLayer.Field2Int(dr["MerchantID"]), DataLayer.Field2Str(dr["AchCoName"])));
            }

            dr.Close();

            data = null;
            return Merchants;
        }
        public List<Merchant> GetPendingReturnFileMerchants()
        {
            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();

            prms.Add(new SqlParameter("@BankID", this.BankID));

            SqlDataReader dr = data.SelectReturnFileGetData2(prms);

            this.Merchants.Clear();

            while (dr.Read())
            {
                this.Merchants.Add(new Merchant(DataLayer.Field2Int(dr["AchID"]), DataLayer.Field2Int(dr["MerchantID"]), DataLayer.Field2Str(dr["AchCoName"])));
            }
            dr.Close();
            data = null;
            return Merchants;
        }

        public List<Merchant> GetPendingReturnEmailMerchants()
        {
            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();

            prms.Add(new SqlParameter("@BankID", this.BankID));

            SqlDataReader dr = data.SelectReturnEmailGetData(prms);

            this.Merchants.Clear();

            while (dr.Read())
            {
                Merchant merchant = new Merchant(DataLayer.Field2Int(dr["AchID"]), DataLayer.Field2Int(dr["MerchantID"]), DataLayer.Field2Str(dr["AchCoName"]));
                merchant.Email = DataLayer.Field2Str(dr["Email"]);
                merchant.MerchantAppUID = DataLayer.Field2Str(dr["MerchantAppUID"]);
                this.Merchants.Add(merchant);
            }
            dr.Close();
            data = null;
            return Merchants;
        }
        public abstract bool CreateBatch(Merchant merchant);
        public abstract bool CreateJournal(Batch batch);

        //private void CreateSettlementBatch(ref NachaFile file, ref NachaBatch batch, ref NachaDetail detail, SqlDataReader dr, ref int LastBatchID, ref int LastBatchFileID)
        //{                
        //    LastBatchID = DataLayer.Field2Int(dr["BatchID"]);
        //    LastBatchFileID = DataLayer.Field2Int(dr["FileBatchID"]);

        //    //Create new batch
        //    batch = file.CreateBatch();
        //    batch.AchID = DataLayer.Field2Int(dr["AchID"]);
        //    batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
        //    batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
        //    batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
        //    batch.CompanyIdentification = "1954662869";

        //    if (DataLayer.Field2Str(dr["Secc"]) == "WEB" && DataLayer.Field2Str(dr["TransType"]).Substring(1, 1) == "2") //Credit cannot contain in WEB batches
        //        batch.Secc = "PPD"; //Need to change to a PPD batch because WEB batch cannot have credits
        //    else
        //        batch.Secc = DataLayer.Field2Str(dr["Secc"]);


        //    batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
        //    batch.CompanyDescriptiveDate = DataLayer.Field2Str(dr["DescDate"]);
        //    batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //    batch.OriginatingDFI = this.m_ImmediateDestination.Trim().Substring(1, 8);

        //    file.AddBatch(batch);
        //}

        public virtual bool CreateSettlementFile(DateTime dateSubmitted)
        {
            try
            {
                ArrayList prms = new ArrayList();
                DataAchProcess data = new DataAchProcess();

                this.CurrentJobID = data.GetNextJobID("Create File for BankID " + this.BankID.ToString());


                //Build settlment records
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@Date", dateSubmitted));

                SqlDataReader dr = data.SelectSettlementData(prms);

                if (!dr.HasRows)
                {
                    FormHandler.DispalyInformationMessage("No data to write to file.");
                    return false;
                }

                StreamWriter sw = new StreamWriter(this.SettlementFileName);

                NachaFile file = new SettlementFile(this);
                NachaBatch batch = null;
                NachaDetail detail = null;
                SettlementAddenda addenda = null;
                int DetailCount = 0;

                file.ImmediateDestination = this.ImmediateDestination;
                file.Immediate_Origin = this.ImmediateOrigin;
                file.FileCreationDate = DateTime.Now.ToString("yyMMdd");
                file.FileCreationTime = DateTime.Now.ToString("HHmm");
                file.DestinationName = this.DestinationName;
                file.OriginName = this.OriginName;
                file.ReferenceCode = "00000000";

                int LastBatchID = 0;
                int LastBatchFileID = 0;
                string EntryDate = string.Empty;

                while (dr.Read())
                {
                    if (LastBatchID != DataLayer.Field2Int(dr["BatchID"]) || LastBatchFileID != DataLayer.Field2Int(dr["FileBatchID"]))
                    {
                        if (LastBatchID != 0) //If this is not the first record
                        {

                            switch (batch.Secc)
                            {
                                case "TEL":
                                case "WEB":
                                    this.CreateBatchOffsetWEBTEL(file, batch, ref DetailCount);
                                    break;
                                default:
                                    this.CreateBatchOffsetDetailRecord(batch, ref DetailCount);
                                    break;
                            }
                        }
                        //this.CreateSettlementBatch(ref file, ref batch, ref detail, dr, ref LastBatchID, ref LastBatchFileID);

                        LastBatchID = DataLayer.Field2Int(dr["BatchID"]);
                        LastBatchFileID = DataLayer.Field2Int(dr["FileBatchID"]);

                        //Create new batch
                        batch = file.CreateBatch();
                        batch.AchID = DataLayer.Field2Int(dr["AchID"]);
                        batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
                        batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
                        batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
                        batch.CompanyIdentification = this.CompanyID; //1262437777

                        if (DataLayer.Field2Str(dr["Secc"]) == "WEB" && DataLayer.Field2Str(dr["TransType"]).Substring(1, 1) == "2")
                            batch.Secc = "PPD"; //Need to change to a PPD batch because WEB batch cannot have credits
                        else
                            batch.Secc = DataLayer.Field2Str(dr["Secc"]);


                        batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
                        batch.CompanyDescriptiveDate = DataLayer.Field2Str(dr["DescDate"]);
                        batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        EntryDate = batch.EntyDate;
                        batch.OriginatingDFI = this.m_ImmediateDestination.Trim().Substring(1, 8);

                        file.AddBatch(batch);

                    }

                    //Create new detail record
                    detail = batch.CreateDetail();
                    detail.TransID = DataLayer.Field2Int(dr["TransID"]);
                    detail.AccountName = DataLayer.Field2Str(dr["NameOnAccount"]);
                    detail.AccountNo = DataLayer.Field2Str(dr["AccountNo"]);
                    detail.TransRoute = DataLayer.Field2Str(dr["TransRoute"]);
                    detail.TransType = DataLayer.Field2Str(dr["TransType"]);
                    detail.Amount = DataLayer.Field2Dec(dr["Amount"]);
                    detail.IdentificationNumber = "N" + DataLayer.Field2Str(dr["TraceNumber"]);
                    detail.CheckNumber = DataLayer.Field2Str(dr["CheckNumber"]);
                    detail.TraceNumber = ++DetailCount;

                    //Create new addenda record
                    if (DataLayer.Field2Int(dr["Addenda_TransID"]) != 0)
                    {
                        addenda = (SettlementAddenda)detail.CreateAddenda();
                        addenda.PaymentInfo = DataLayer.Field2Str(dr["AddentInfo"]);
                        addenda.AddendaSeqNumber = DataLayer.Field2Int(dr["SequenceNo"]);
                        addenda.EntrySeqNumber = detail.TraceNumber;
                        detail.AddAddenda(addenda);
                    }

                    //int z = 0;
                    //if (detail.Amount == Convert.ToDecimal(497.58))
                    //    z = 0;

                    //if (batch.Secc == "WEB" && detail.TransType.Substring(1, 1) == "2") //Credit cannot contain in WEB batches
                    //{
                    //    this.CreateBatchOffsetWEB(file, batch, ref DetailCount);
                    //    this.CreateSettlementBatch(ref file, ref batch, ref detail, dr, ref LastBatchID, ref LastBatchFileID);
                    //} if (batch.Secc == "PPD" && (detail.TransType.Substring(1, 1) == "7" && detail.TransType.Substring(1, 1) == "8")) 


                    batch.AddDetail(detail);

                }
                if (batch != null)
                {
                    switch (batch.Secc)
                    {
                        case "TEL":
                        case "WEB":
                            this.CreateBatchOffsetWEBTEL(file, batch, ref DetailCount);
                            break;
                        default:
                            this.CreateBatchOffsetDetailRecord(batch, ref DetailCount);
                            break;
                    }

                }


                dr.Close();
                dr = null;

                //Build hold release and preauth dr records
                batch = null;
                detail = null;
                //Commenting out 
                //prms.Clear();
                //prms.Add(new SqlParameter("@BankID", this.BankID));
                //prms.Add(new SqlParameter("@Date", DateTime.Today));
                //prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                //prms.Add(new SqlParameter("@JobID", this.CurrentJobID));

                //dr = data.SelectPaymentData(prms);

                //string Description = string.Empty;

                //while (dr.Read())
                //{
                //    if (Description != DataLayer.Field2Str(dr["Description"]))
                //    {
                //        if (Description != string.Empty) //If this is not the first record
                //        {
                //            this.CreateBatchOffsetDetailRecord(batch, ref DetailCount);
                //        }

                //        Description = DataLayer.Field2Str(dr["Description"]);

                //        //Create new batch
                //        batch = file.CreateBatch();
                //        batch.AchID = DataLayer.Field2Int(dr["AchID"]);
                //        batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
                //        batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
                //        batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
                //        //batch.CompanyIdentification = "1954662869";
                //        batch.CompanyIdentification = this.CompanyID; //1262437777
                //        batch.Secc = DataLayer.Field2Str(dr["Secc"]);
                //        batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
                //        batch.CompanyDescriptiveDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                //        batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                //        EntryDate = batch.EntyDate;
                //        batch.OriginatingDFI = this.m_ImmediateDestination.Trim().Substring(1, 8);
                //        batch.BatchNumber = 1;

                //        file.AddBatch(batch);
                //    }

                //    //Create new detail record
                //    detail = batch.CreateDetail();
                //    detail.TransID = DataLayer.Field2Int(dr["TransID"]);
                //    detail.AccountName = DataLayer.Field2Str(dr["NameOnAccount"]);
                //    detail.AccountNo = DataLayer.Field2Str(dr["AccountNo"]);
                //    detail.TransRoute = DataLayer.Field2Str(dr["TransRoute"]);
                //    detail.TransType = DataLayer.Field2Str(dr["TransType"]);
                //    detail.Amount = DataLayer.Field2Dec(dr["Amount"]);
                //    detail.IdentificationNumber = "N" + DataLayer.Field2Str(dr["TraceNumber"]);
                //    detail.TraceNumber = ++DetailCount;

                //    batch.AddDetail(detail);

                //}

                //if (batch != null)
                //    this.CreateBatchOffsetDetailRecord(batch, ref DetailCount);

                //dr.Close();
                //dr = null;

                //Print file header
                sw.WriteLine(file.GetFileHeader());

                //Print batch offsets
                foreach (SettlementBatch b in file.Batches)
                {
                    sw.WriteLine(b.GetBatchHeader());
                    foreach (SettlementDetail d in b.Details)
                    {
                        sw.WriteLine(d.GetFileDetail());
                        if (d.Addendas.Count > 0)
                            sw.WriteLine(d.Addendas[0].GetFileAddenda());
                    }
                    sw.WriteLine(b.GetBatchFooter());
                }

                //Print file footer
                sw.WriteLine(file.GetFileFooter());

                //Print padded "9"
                int remainder = file.TotalLines % 10;

                if (remainder > 0)
                {
                    for (int i = 0; i < 10 - remainder; i++)
                    {
                        sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
                    }
                }

                sw.Close();

                FormHandler.DispalyInformationMessage("Settlement file created successfully for bank " + this.BankName + ". File located at " + this.SettlementFileName);
                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create settlement file failed.   Please contact system administrator.", exc);
                return false;
            }

        }
        public void CreateBatchOffsetWEBTEL(NachaFile file, NachaBatch batch, ref int DetailCount)
        {
            NachaBatch webbatch = null;

            //Create new batch for WEB
            webbatch = file.CreateBatch();
            webbatch.AchID = batch.AchID; // DataLayer.Field2Int(dr["AchID"]);
            webbatch.BatchID = batch.BatchID; // DataLayer.Field2Int(dr["BatchID"]);
            webbatch.CompanyName = batch.CompanyName; // DataLayer.Field2Str(dr["CompanyName"]);
            webbatch.DiscretionaryData = batch.DiscretionaryData; //DataLayer.Field2Str(dr["DiscretionaryData"]);
            webbatch.CompanyIdentification = batch.CompanyIdentification; //"1954662869";
            webbatch.Secc = "CCD"; //DataLayer.Field2Str(dr["Secc"]);
            webbatch.CompanyDescription = batch.CompanyDescription; //DataLayer.Field2Str(dr["Description"]);
            webbatch.CompanyDescriptiveDate = batch.CompanyDescriptiveDate; //DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
            webbatch.EntyDate = batch.EntyDate; //DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
            webbatch.OriginatingDFI = batch.OriginatingDFI; //this.m_ImmediateDestination.Trim().Substring(1, 8);
            webbatch.TotalCredit = batch.TotalCredit;
            webbatch.TotalDebit = batch.TotalDebit;

            this.CreateBatchOffsetDetailRecord(webbatch, ref DetailCount);

            decimal BatchNetAmount = batch.TotalDebit - batch.TotalCredit;
            if (BatchNetAmount > 0)
                webbatch.TotalDebit = 0;
            else
                webbatch.TotalCredit = 0;


            file.AddBatch(webbatch);


        }
        public virtual void CreateBatchOffsetDetailRecord(NachaBatch batch, ref int DetailCount)
        {
            decimal BatchNetAmount = batch.TotalDebit - batch.TotalCredit;

            if (BatchNetAmount == 0) //Offset is not required
                return;

            NachaDetail detail = null;

            detail = batch.CreateDetail();



            if (BatchNetAmount > 0)
                detail.TransType = "22";
            else if (BatchNetAmount < 0)
                detail.TransType = "27";
            else
                detail.TransType = "28";


            detail.AccountName = batch.CompanyDescription;
            detail.AccountNo = this.OriginatingAccountNo;
            detail.TransRoute = this.OriginatingTransRoute;
            detail.Amount = Math.Abs(BatchNetAmount);
            detail.TraceNumber = ++DetailCount;
            detail.IdentificationNumber = this.Symbol + " BATCH OFFSET";
            batch.AddDetail(detail);

        }

        public virtual NachaDetail GetBatchOffsetDetailRecord(NachaBatch batch, ref int DetailCount)
        {
            decimal BatchNetAmount = batch.TotalDebit - batch.TotalCredit;

            if (BatchNetAmount == 0) //Offset is not required
                return null;

            NachaDetail detail = null;

            detail = batch.CreateDetail();



            if (BatchNetAmount > 0)
                detail.TransType = "22";
            else if (BatchNetAmount < 0)
                detail.TransType = "27";
            else
                detail.TransType = "28";


            detail.AccountName = batch.CompanyDescription;
            detail.AccountNo = this.OriginatingAccountNo;
            detail.TransRoute = this.OriginatingTransRoute;
            detail.Amount = Math.Abs(BatchNetAmount);
            detail.TraceNumber = ++DetailCount;
            detail.IdentificationNumber = this.Symbol + " BATCH OFFSET";
            batch.AddDetail(detail);

            return detail;

        }

        //public virtual bool CreateSettlementFile(string FilePath)
        //{
        //    try
        //    {
        //        ArrayList prms = new ArrayList();
        //        DataAchProcess data = new DataAchProcess();

        //        this.CurrentJobID = data.GetNextJobID("Create File for BankID " + this.BankID.ToString());


        //        //Build settlment records
        //        prms.Add(new SqlParameter("@BankID", this.BankID));
        //        prms.Add(new SqlParameter("@Date", DateTime.Today));

        //        SqlDataReader dr = data.SelectSettlementData(prms);

        //        if (!dr.HasRows)
        //        {
        //            FormHandler.DispalyInformationMessage("No data to write to file.");
        //            return false;
        //        }

        //        StreamWriter sw = new StreamWriter(FilePath);

        //        NachaFile file = new SettlementFile(this);
        //        NachaBatch batch = null;
        //        NachaDetail detail = null;
        //        SettlementAddenda addenda = null;

        //        file.ImmediateDestination = this.ImmediateDestination;
        //        file.Immediate_Origin = this.ImmediateOrigin;
        //        file.FileCreationDate = DateTime.Now.ToString("yyMMdd");
        //        file.FileCreationTime = DateTime.Now.ToString("HHmm");
        //        file.DestinationName = this.DestinationName;
        //        file.OriginName = this.OriginName;
        //        file.ReferenceCode = this.ImmediateOrigin.Substring(1, 8);

        //        int LastBatchID = 0;
        //        int LastBatchFileID = 0;
        //        string EntryDate = string.Empty;

        //        while (dr.Read())
        //        {
        //            if (LastBatchID != DataLayer.Field2Int(dr["BatchID"]) || LastBatchFileID != DataLayer.Field2Int(dr["FileBatchID"]))
        //            {
        //                LastBatchID = DataLayer.Field2Int(dr["BatchID"]);
        //                LastBatchFileID = DataLayer.Field2Int(dr["FileBatchID"]);

        //                //Create new batch
        //                batch = file.CreateBatch();
        //                batch.AchID = DataLayer.Field2Int(dr["AchID"]);
        //                batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
        //                batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
        //                batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
        //                batch.CompanyIdentification = DataLayer.Field2Str(dr["CompanyIdentification"]);
        //                batch.Secc = DataLayer.Field2Str(dr["Secc"]);
        //                batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
        //                batch.CompanyDescriptiveDate = DataLayer.Field2Str(dr["DescDate"]);
        //                batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //                EntryDate = batch.EntyDate;
        //                batch.OriginatingDFI = this.ImmediateOrigin.Substring(1, 8);

        //                file.AddBatch(batch);
        //            }

        //            //Create new detail record
        //            detail = batch.CreateDetail();
        //            detail.TransID = DataLayer.Field2Int(dr["TransID"]);
        //            detail.AccountName = DataLayer.Field2Str(dr["NameOnAccount"]);
        //            detail.AccountNo = DataLayer.Field2Str(dr["AccountNo"]);
        //            detail.TransRoute = DataLayer.Field2Str(dr["TransRoute"]);
        //            detail.TransType = DataLayer.Field2Str(dr["TransType"]);
        //            detail.Amount = DataLayer.Field2Dec(dr["Amount"]);
        //            detail.IdentificationNumber = DataLayer.Field2Str(dr["IdenficationNumber"]);
        //            detail.CheckNumber = DataLayer.Field2Str(dr["CheckNumber"]);
        //            detail.TraceNumber = DataLayer.Field2Int(dr["TraceNumber"]);

        //            //Create new addenda record
        //            if (DataLayer.Field2Int(dr["Addenda_TransID"]) != 0)
        //            {
        //                addenda = (SettlementAddenda)detail.CreateAddenda();
        //                addenda.PaymentInfo = DataLayer.Field2Str(dr["AddentInfo"]);
        //                addenda.AddendaSeqNumber = DataLayer.Field2Int(dr["SequenceNo"]);
        //                addenda.EntrySeqNumber = detail.TraceNumber;
        //                detail.AddAddenda(addenda);
        //            }

        //            batch.AddDetail(detail);

        //        }

        //        dr.Close();
        //        dr = null;

        //        //Build hold release and preauth dr records
        //        prms.Clear();
        //        prms.Add(new SqlParameter("@BankID", this.BankID));
        //        prms.Add(new SqlParameter("@Date", DateTime.Today));
        //        prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
        //        prms.Add(new SqlParameter("@JobID", this.CurrentJobID));

        //        dr = data.SelectPaymentData(prms);

        //        string Description = string.Empty;

        //        while (dr.Read())
        //        {
        //            if (Description != DataLayer.Field2Str(dr["Description"]))
        //            {
        //                Description = DataLayer.Field2Str(dr["Description"]);

        //                //Create new batch
        //                batch = file.CreateBatch();
        //                batch.AchID = DataLayer.Field2Int(dr["AchID"]);
        //                batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
        //                batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
        //                batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
        //                batch.CompanyIdentification = DataLayer.Field2Str(dr["CompanyIdentification"]);
        //                batch.Secc = DataLayer.Field2Str(dr["Secc"]);
        //                batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
        //                batch.CompanyDescriptiveDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //                batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //                EntryDate = batch.EntyDate;
        //                batch.OriginatingDFI = this.ImmediateOrigin.Substring(1, 8);
        //                batch.BatchNumber = 1;

        //                file.AddBatch(batch);
        //            }

        //            //Create new detail record
        //            detail = batch.CreateDetail();
        //            detail.TransID = DataLayer.Field2Int(dr["TransID"]);
        //            detail.AccountName = DataLayer.Field2Str(dr["NameOnAccount"]);
        //            detail.AccountNo = DataLayer.Field2Str(dr["AccountNo"]);
        //            detail.TransRoute = DataLayer.Field2Str(dr["TransRoute"]);
        //            detail.TransType = DataLayer.Field2Str(dr["TransType"]);
        //            detail.Amount = DataLayer.Field2Dec(dr["Amount"]);
        //            detail.IdentificationNumber = DataLayer.Field2Str(dr["IdentificationNumber"]);
        //            detail.TraceNumber = DataLayer.Field2Int(dr["TraceNumber"]);

        //            batch.AddDetail(detail);

        //        }

        //        dr.Close();
        //        dr = null;

        //        //Build batch offset records

        //        long LastTraceNumber = detail.TraceNumber + 1;

        //        batch = file.CreateBatch();
        //        batch.CompanyName = "NATIONAL MERCHAN";
        //        batch.DiscretionaryData = "BATCH OFFSET LINE   ";
        //        batch.CompanyIdentification = "NMC9999999";
        //        batch.Secc = "CCD";
        //        batch.CompanyDescription = "BATCHOFFSE";
        //        batch.CompanyDescriptiveDate = "      ";
        //        batch.EntyDate = EntryDate;
        //        batch.OriginatingDFI = this.ImmediateOrigin.Substring(1, 8);

        //        foreach (SettlementBatch b in file.Batches)
        //        {
        //            detail = batch.CreateDetail();

        //            decimal BatchNetAmount = b.TotalDebit - b.TotalCredit;
        //            if (BatchNetAmount > 0)
        //                detail.TransType = "22";
        //            else
        //                detail.TransType = "27";

        //            detail.AccountName = b.CompanyName;
        //            detail.AccountNo = this.OriginatingAccountNo;
        //            detail.TransRoute = this.OriginatingTransRoute;
        //            detail.Amount = Math.Abs(BatchNetAmount);
        //            detail.TraceNumber = LastTraceNumber++;

        //            switch (b.CompanyDescription)
        //            {
        //                case "HOLD RELEA":
        //                case "OVER DRAFT":
        //                case "PREAUTH DR":
        //                    detail.IdentificationNumber = DataLayer.FilePadString(this.Symbol + b.CompanyDescription, 15);
        //                    break;
        //                default:
        //                    detail.IdentificationNumber = this.Symbol + DataLayer.FilePadNumber(b.AchID, 5) + "TRANS     ";
        //                    break;
        //            }

        //            batch.AddDetail(detail);
        //        }

        //        file.AddBatch(batch);


        //        //Print file header
        //        sw.WriteLine(file.GetFileHeader());

        //        //Print batch offsets
        //        foreach (SettlementBatch b in file.Batches)
        //        {
        //            sw.WriteLine(b.GetBatchHeader());
        //            foreach (SettlementDetail d in b.Details)
        //            {
        //                sw.WriteLine(d.GetFileDetail());
        //                if (d.Addendas.Count > 0)
        //                    sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //            }
        //            sw.WriteLine(b.GetBatchFooter());
        //        }

        //        //Print file footer
        //        sw.WriteLine(file.GetFileFooter());

        //        //Print padded "9"
        //        int remainder = file.TotalLines % 10;

        //        for (int i = 0; i < 10 - remainder; i++)
        //        {
        //            sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //        }
        //        StringBuilder sb = new StringBuilder();

        //        sw.Close();

        //        FormHandler.DispalyInformationMessage("Settlement file created successfully for bank " + this.BankName + ". File located at " + FilePath);
        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        FormHandler.DispalyErrorMessage("Create settlement file failed.   Please contact system administrator.", exc);
        //        return false;
        //    }

        //}

        //public virtual bool CreateSettlementFile(string FilePath)
        //{
        //    try
        //    {
        //        ArrayList prms = new ArrayList();
        //        DataAchProcess data = new DataAchProcess();

        //        this.CurrentJobID = data.GetNextJobID("Create File for BankID " + this.BankID.ToString());


        //        //Build settlment records
        //        prms.Add(new SqlParameter("@BankID", this.BankID));
        //        prms.Add(new SqlParameter("@Date", DateTime.Today));

        //        SqlDataReader dr = data.SelectSettlementData(prms);

        //        if (!dr.HasRows)
        //        {
        //            FormHandler.DispalyInformationMessage("No data to write to file.");
        //            return false;
        //        }

        //        StreamWriter sw = new StreamWriter(FilePath);

        //        NachaFile file = new SettlementFile(this);
        //        NachaBatch batch = null;
        //        NachaDetail detail = null;
        //        SettlementAddenda addenda = null;

        //        file.ImmediateDestination = this.ImmediateDestination;
        //        file.Immediate_Origin = this.ImmediateOrigin;
        //        file.FileCreationDate = DateTime.Now.ToString("yyMMdd");
        //        file.FileCreationTime = DateTime.Now.ToString("HHmm");
        //        file.DestinationName = this.DestinationName;
        //        file.OriginName = this.OriginName;
        //        file.ReferenceCode = this.ImmediateOrigin.Substring(1, 8);

        //        int LastBatchID = 0;
        //        int LastBatchFileID = 0;
        //        string EntryDate = string.Empty;
        //        int LineCount = 0;

        //        while (dr.Read())
        //        {
        //            if (LastBatchID != DataLayer.Field2Int(dr["BatchID"]) || LastBatchFileID != DataLayer.Field2Int(dr["FileBatchID"]))
        //            {
        //                LastBatchID = DataLayer.Field2Int(dr["BatchID"]);
        //                LastBatchFileID = DataLayer.Field2Int(dr["FileBatchID"]);

        //                if (m_IncludeBatchOffsetInSameBatch = true)
        //                {
        //                    //Create Batch Offset Record
        //                    if (batch != null)
        //                    {
        //                        detail = batch.CreateDetail();

        //                        decimal BatchNetAmount = batch.TotalDebit - batch.TotalCredit;
        //                        if (BatchNetAmount > 0)
        //                            detail.TransType = "22";
        //                        else
        //                            detail.TransType = "27";

        //                        detail.AccountName = batch.CompanyName;
        //                        detail.AccountNo = this.OriginatingAccountNo;
        //                        detail.TransRoute = this.OriginatingTransRoute;
        //                        detail.Amount = Math.Abs(BatchNetAmount);
        //                        detail.TraceNumber = LineCount++;

        //                        switch (batch.CompanyDescription)
        //                        {
        //                            case "HOLD RELEA":
        //                            case "OVER DRAFT":
        //                            case "PREAUTH DR":
        //                                detail.IdentificationNumber = DataLayer.FilePadString(this.Symbol + batch.CompanyDescription, 15);
        //                                break;
        //                            default:
        //                                detail.IdentificationNumber = this.Symbol + DataLayer.FilePadNumber(batch.AchID, 5) + "TRANS     ";
        //                                break;
        //                        }

        //                        batch.AddDetail(detail);
        //                    }
        //                }

        //                //Create new batch
        //                batch = file.CreateBatch();
        //                batch.AchID = DataLayer.Field2Int(dr["AchID"]);
        //                batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
        //                batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
        //                batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
        //                batch.CompanyIdentification = DataLayer.Field2Str(dr["CompanyIdentification"]);
        //                batch.Secc = DataLayer.Field2Str(dr["Secc"]);
        //                batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
        //                batch.CompanyDescriptiveDate = DataLayer.Field2Str(dr["DescDate"]);
        //                batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //                EntryDate = batch.EntyDate;
        //                batch.OriginatingDFI = this.ImmediateOrigin.Substring(1, 8);

        //                file.AddBatch(batch);
        //            }

        //            //Create new detail record
        //            detail = batch.CreateDetail();
        //            detail.TransID = DataLayer.Field2Int(dr["TransID"]);
        //            detail.AccountName = DataLayer.Field2Str(dr["NameOnAccount"]);
        //            detail.AccountNo = DataLayer.Field2Str(dr["AccountNo"]);
        //            detail.TransRoute = DataLayer.Field2Str(dr["TransRoute"]);
        //            detail.TransType = DataLayer.Field2Str(dr["TransType"]);
        //            detail.Amount = DataLayer.Field2Dec(dr["Amount"]);
        //            detail.IdentificationNumber = DataLayer.Field2Str(dr["IdenficationNumber"]);
        //            detail.CheckNumber = DataLayer.Field2Str(dr["CheckNumber"]);
        //            if (m_IncludeBatchOffsetInSameBatch = true)
        //                detail.TraceNumber = LineCount++;
        //            else
        //                detail.TraceNumber = DataLayer.Field2Int(dr["TraceNumber"]);

        //            //Create new addenda record
        //            if (DataLayer.Field2Int(dr["Addenda_TransID"]) != 0)
        //            {
        //                addenda = (SettlementAddenda)detail.CreateAddenda();
        //                addenda.PaymentInfo = DataLayer.Field2Str(dr["AddentInfo"]);
        //                addenda.AddendaSeqNumber = DataLayer.Field2Int(dr["SequenceNo"]);
        //                addenda.EntrySeqNumber = detail.TraceNumber;
        //                detail.AddAddenda(addenda);
        //            }

        //            batch.AddDetail(detail);

        //        }

        //        dr.Close();
        //        dr = null;

        //        //Build hold release and preauth dr records
        //        prms.Clear();
        //        prms.Add(new SqlParameter("@BankID", this.BankID));
        //        prms.Add(new SqlParameter("@Date", DateTime.Today));
        //        prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
        //        prms.Add(new SqlParameter("@JobID", this.CurrentJobID));

        //        dr = data.SelectPaymentData(prms);

        //        string Description = string.Empty;

        //        while (dr.Read())
        //        {
        //            if (Description != DataLayer.Field2Str(dr["Description"]))
        //            {
        //                Description = DataLayer.Field2Str(dr["Description"]);

        //                //Create new batch
        //                batch = file.CreateBatch();
        //                batch.AchID = DataLayer.Field2Int(dr["AchID"]);
        //                batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
        //                batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
        //                batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
        //                batch.CompanyIdentification = DataLayer.Field2Str(dr["CompanyIdentification"]);
        //                batch.Secc = DataLayer.Field2Str(dr["Secc"]);
        //                batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
        //                batch.CompanyDescriptiveDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //                batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
        //                EntryDate = batch.EntyDate;
        //                batch.OriginatingDFI = this.ImmediateOrigin.Substring(1, 8);
        //                batch.BatchNumber = 1;

        //                file.AddBatch(batch);
        //            }

        //            //Create new detail record
        //            detail = batch.CreateDetail();
        //            detail.TransID = DataLayer.Field2Int(dr["TransID"]);
        //            detail.AccountName = DataLayer.Field2Str(dr["NameOnAccount"]);
        //            detail.AccountNo = DataLayer.Field2Str(dr["AccountNo"]);
        //            detail.TransRoute = DataLayer.Field2Str(dr["TransRoute"]);
        //            detail.TransType = DataLayer.Field2Str(dr["TransType"]);
        //            detail.Amount = DataLayer.Field2Dec(dr["Amount"]);
        //            detail.IdentificationNumber = DataLayer.Field2Str(dr["IdentificationNumber"]);
        //            detail.TraceNumber = DataLayer.Field2Int(dr["TraceNumber"]);

        //            batch.AddDetail(detail);

        //        }

        //        dr.Close();
        //        dr = null;

        //        //Build batch offset records

        //        long LastTraceNumber = detail.TraceNumber + 1;

        //        if (m_IncludeBatchOffsetInSameBatch == false)
        //        {
        //            batch = file.CreateBatch();
        //            batch.CompanyName = "NATIONAL MERCHAN";
        //            batch.DiscretionaryData = "BATCH OFFSET LINE   ";
        //            batch.CompanyIdentification = "2339999999";
        //            batch.Secc = "CCD";
        //            batch.CompanyDescription = "BATCHOFFSE";
        //            batch.CompanyDescriptiveDate = "      ";
        //            batch.EntyDate = EntryDate;
        //            batch.OriginatingDFI = this.ImmediateOrigin.Substring(1, 8);

        //            foreach (SettlementBatch b in file.Batches)
        //            {
        //                detail = batch.CreateDetail();

        //                decimal BatchNetAmount = b.TotalDebit - b.TotalCredit;
        //                if (BatchNetAmount > 0)
        //                    detail.TransType = "22";
        //                else
        //                    detail.TransType = "27";

        //                detail.AccountName = b.CompanyName;
        //                detail.AccountNo = this.OriginatingAccountNo;
        //                detail.TransRoute = this.OriginatingTransRoute;
        //                detail.Amount = Math.Abs(BatchNetAmount);
        //                detail.TraceNumber = LastTraceNumber++;

        //                switch (b.CompanyDescription)
        //                {
        //                    case "HOLD RELEA":
        //                    case "OVER DRAFT":
        //                    case "PREAUTH DR":
        //                        detail.IdentificationNumber = DataLayer.FilePadString(this.Symbol + b.CompanyDescription, 15);
        //                        break;
        //                    default:
        //                        detail.IdentificationNumber = this.Symbol + DataLayer.FilePadNumber(b.AchID, 5) + "TRANS     ";
        //                        break;
        //                }

        //                batch.AddDetail(detail);
        //            }
        //        }

        //        file.AddBatch(batch);


        //        //Print file header
        //        sw.WriteLine(file.GetFileHeader());

        //        //Print batch offsets
        //        foreach (SettlementBatch b in file.Batches)
        //        {
        //            sw.WriteLine(b.GetBatchHeader());
        //            foreach (SettlementDetail d in b.Details)
        //            {
        //                sw.WriteLine(d.GetFileDetail());
        //                if (d.Addendas.Count > 0)
        //                    sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //            }
        //            sw.WriteLine(b.GetBatchFooter());
        //        }

        //        //Print file footer
        //        sw.WriteLine(file.GetFileFooter());

        //        //Print padded "9"
        //        int remainder = file.TotalLines % 10;

        //        for (int i = 0; i < 10 - remainder; i++)
        //        {
        //            sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //        }
        //        StringBuilder sb = new StringBuilder();

        //        sw.Close();

        //        FormHandler.DispalyInformationMessage("Settlement file created successfully for bank " + this.BankName + ". File located at " + FilePath);
        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        FormHandler.DispalyErrorMessage("Create settlement file failed.   Please contact system administrator.", exc);
        //        return false;
        //    }

        //}

        public bool ParseSettlementFile(ref NachaFile file, FileInfo fi)
        {
            StreamReader sr = File.OpenText(fi.FullName);
            NachaBatch batch = null;
            NachaDetail detail = null;
            SettlementAddenda addenda = null;

            try
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    switch (line.Substring(0, 1))
                    {
                        case "1":
                            file = new SettlementFile(this.BankLog);
                            break;
                        case "5":
                            batch = file.CreateBatch(this.BankLog);
                            batch.CompanyName = line.Substring(4, 16).Trim();
                            batch.DiscretionaryData = line.Substring(20, 20).Trim();
                            batch.CompanyIdentification = line.Substring(40, 10).Trim();
                            batch.Secc = line.Substring(50, 3).Trim();
                            batch.CompanyDescription = line.Substring(53, 10).Trim();
                            batch.CompanyDescriptiveDate = line.Substring(63, 6).Trim();
                            batch.EntyDate = line.Substring(71, 2) + "/" + line.Substring(73, 2) + "/" + line.Substring(69, 2);
                            batch.SettlementDate = line.Substring(75, 3).Trim();
                            batch.OriginatingDFI = line.Substring(78, 8).Trim();
                            batch.BatchNumber = Convert.ToInt32(line.Substring(87, 7));

                            if (file.FileBatchNumber == 0)
                                file.FileBatchNumber = batch.BatchNumber - 1;

                            break;
                        case "6":
                            detail = batch.CreateDetail();
                            detail.TransType = line.Substring(1, 2).Trim();
                            detail.TransRoute = line.Substring(3, 9).Trim();
                            detail.AccountNo = line.Substring(12, 17).Trim();
                            detail.Amount = Convert.ToDecimal(line.Substring(29, 10)) / 100;
                            detail.IdentificationNumber = line.Substring(39, 15).Trim().Replace("N", "");
                            detail.AccountName = line.Substring(54, 22).Trim();
                            detail.TraceNumber = Convert.ToInt64(line.Substring(79, 15));
                            detail.DiscretionaryData = line.Substring(77, 2).Trim();

                            //Do not include the OFFSET in calculating the file totals. This offset is anyways madeup on our system.
                            //If a merchant batch offset is a credit record (22), A turnkey split adds up twice. (Once on the batch offset and one more time on the Turnkey total offset.)
                            //This will make the file totals to mismatch as they are 
                            if (!(detail.IdentificationNumber.Contains("OFFSET") || detail.AccountName.Contains("OFFSET")))
                            {
                                batch.AddDetail(detail);
                            }
                            break;
                        //case "7":
                        //    addenda = (SettlementAddenda)detail.CreateAddenda();
                        //    addenda.ReturnReasonCode = line.Substring(3, 3).Trim();
                        //    addenda.OrigTraceNumber = Convert.ToInt64(line.Substring(6, 15));
                        //    addenda.DateOfDeath = line.Substring(21, 6).Trim();
                        //    addenda.OrigRDFI = line.Substring(27, 8).Trim();
                        //    addenda.AddendaInfo = line.Substring(35, 44).Trim();
                        //    addenda.TraceNumber = Convert.ToInt64(line.Substring(79, 15));

                        //    detail.AddAddenda(addenda);
                        //    break;
                        case "8":
                            file.AddBatch(batch);
                            break;
                    }
                    line = sr.ReadLine();
                }

                sr.Close();

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Parse return file failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                sr = null;
            }
        }

        public bool ParseReturnFile(ref NachaFile file, FileInfo fi)
        {
            StreamReader sr = File.OpenText(fi.FullName);
            NachaBatch batch = null;
            NachaDetail detail = null;
            ReturnAddenda addenda = null;

            try
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    switch (line.Substring(0, 1))
                    {
                        case "1":
                            file = new ReturnFile();
                            break;
                        case "5":
                            batch = file.CreateBatch();
                            batch.CompanyName = line.Substring(4, 16).Trim();
                            batch.DiscretionaryData = line.Substring(20, 20).Trim();
                            batch.CompanyIdentification = line.Substring(40, 10).Trim();
                            batch.Secc = line.Substring(50, 3).Trim();
                            batch.CompanyDescription = line.Substring(53, 10).Trim();
                            batch.CompanyDescriptiveDate = line.Substring(63, 6).Trim();
                            batch.EntyDate = line.Substring(71, 2) + "/" + line.Substring(73, 2) + "/" + line.Substring(69, 2);
                            batch.SettlementDate = line.Substring(75, 3).Trim();
                            batch.OriginatingDFI = line.Substring(78, 8).Trim();
                            batch.BatchNumber = Convert.ToInt32(line.Substring(87, 7));
                            break;
                        case "6":
                            detail = batch.CreateDetail();
                            detail.TransType = line.Substring(1, 2).Trim();
                            detail.TransRoute = line.Substring(3, 9).Trim();
                            detail.AccountNo = line.Substring(12, 17).Trim();
                            detail.Amount = Convert.ToDecimal(line.Substring(29, 10)) / 100;
                            detail.IdentificationNumber = line.Substring(39, 15).Trim().Replace("N", "");
                            detail.AccountName = line.Substring(54, 22).Trim();
                            detail.TraceNumber = Convert.ToInt64(line.Substring(79, 15));

                            batch.AddDetail(detail);
                            break;
                        case "7":
                            addenda = (ReturnAddenda)detail.CreateAddenda();
                            addenda.ReturnReasonCode = line.Substring(3, 3).Trim();
                            addenda.OrigTraceNumber = Convert.ToInt64(line.Substring(6, 15));
                            addenda.DateOfDeath = line.Substring(21, 6).Trim();
                            addenda.OrigRDFI = line.Substring(27, 8).Trim();
                            addenda.AddendaInfo = line.Substring(35, 44).Trim();
                            addenda.TraceNumber = Convert.ToInt64(line.Substring(79, 15));

                            detail.AddAddenda(addenda);
                            break;
                        case "8":
                            file.AddBatch(batch);
                            break;
                        case "9":
                            if (line.Substring(0, 7) != "9999999")
                            {
                                file.EntryAddendaCount = Convert.ToInt32(line.Substring(13, 8));
                            }

                            break;

                    }
                    line = sr.ReadLine();
                }

                sr.Close();

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Parse return file failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                sr = null;
            }
        }

        private bool InsertReturnsIntoStaging(NachaFile file, FileInfo fi)
        {
            DataAchProcess data = new DataAchProcess();
            SqlParameter prm = null;
            ArrayList prms = new ArrayList();

            try
            {

                foreach (ReturnBatch b in file.Batches)
                {
                    foreach (ReturnDetail d in b.Details)
                    {
                        ReturnAddenda a = d.Addendas[0];

                        prms.Clear();
                        prm = new SqlParameter("@ReturnID", -1);
                        prm.Direction = ParameterDirection.Output;
                        prms.Add(prm);
                        prms.Add(new SqlParameter("@PostedDate", DateTime.Now));
                        prms.Add(new SqlParameter("@TransType", d.TransType));
                        prms.Add(new SqlParameter("@TransRoute", d.TransRoute.Trim()));
                        prms.Add(new SqlParameter("@AccountNo", d.AccountNo.Trim()));
                        prms.Add(new SqlParameter("@NameOnAccount", d.AccountName.Trim()));
                        prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(d.Amount)));
                        prms.Add(new SqlParameter("@AddenCode", a.TypeCode));
                        prms.Add(new SqlParameter("@ReasonCode", a.ReturnReasonCode));
                        prms.Add(new SqlParameter("@AddenInfo", a.AddendaInfo.Trim()));
                        prms.Add(new SqlParameter("@OrigTrace", d.IdentificationNumber)); //TraceNumber in Ach_BatchDetail table is stored in the field
                        prms.Add(new SqlParameter("@OrigRDFI", a.OrigRDFI));
                        prms.Add(new SqlParameter("@Trace", d.TraceNumber.ToString()));
                        prms.Add(new SqlParameter("@FileName", fi.Name));
                        prms.Add(new SqlParameter("@Description", b.CompanyDescription));
                        prms.Add(new SqlParameter("@DescDate", b.CompanyDescriptiveDate));
                        prms.Add(new SqlParameter("@EntryDate", b.EntyDate));

                        data.InsertStagingReturn(prms);

                    }
                }

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Insert returns into staging failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                data = null;
                prm = null;
                prms = null;
            }
        }

        public bool CreateReturnsAndJournals()
        {
            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();

            try
            {

                prms.Clear();
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                prms.Add(new SqlParameter("@JobID", this.CurrentJobID));
                data.ScrubAndCreateReturns(prms);

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create returns and journals failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                data = null;
                prms = null;
            }
        }

        public bool ConfirmReturnCount(NachaFile file, string ReturnFileName)
        {
            ArrayList prms = new ArrayList();
            DataReturn data = new DataReturn();
            bool Perform = false;

            try
            {
                prms.Add(new SqlParameter("@BeginPostedDate", DateTime.Today));
                prms.Add(new SqlParameter("@EndPostedDate", DateTime.Today));
                prms.Add(new SqlParameter("@ReturnFilename", ReturnFileName));
                DataSet ds = data.SelectReturnTotals(prms);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    int returnCount = DataLayer.Field2Int(dr["ReturnCount"]);

                    int fileReturnCount = file.EntryAddendaCount / 2;

                    if (returnCount == fileReturnCount)
                    {
                        return true;
                    }


                    FormHandler.DispalyErrorMessage("File return count does not match database count.   Please contact system administrator.");

                }


            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Return Count Confirmation failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                data = null;
                prms = null;
            }

            return Perform;
        }

        public bool AutoResubmitNSF()
        {
            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();

            try
            {

                data.AutoResubmitNSF(prms);

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Auto Resubmit NSF failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                data = null;
                prms = null;
            }
        }

        public bool CreateAccountBlocks()
        {
            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();

            try
            {

                data.CreateAccountBlock(prms);

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create account blocks failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                data = null;
                prms = null;
            }
        }


        public virtual bool CreateReturns(FileInfo fi)
        {
            DataAchProcess data = new DataAchProcess();
            NachaFile file = null;
            bool perform = false;

            try
            {
                this.CurrentJobID = data.GetNextJobID("Create Returns for BankID " + this.BankID.ToString());

                if (this.CurrentJobID != -1)
                    perform = this.ParseReturnFile(ref file, fi);

                if (file != null)
                {
                    if (perform)
                        perform = data.TruncateStagingReturn();

                    if (perform)
                        perform = this.InsertReturnsIntoStaging(file, fi);
                }

                if (perform)
                    perform = this.CreateReturnsAndJournals();

                if (perform)
                {
                    perform = data.InsertIntoBatchFileLog(fi.Name, -1);
                    if (!perform)
                        FormHandler.DispalyErrorMessage("Log batch file failed.   Please contact system administrator.");
                }

                if (perform)
                    perform = this.ProcessCreateReturnEmails();

                //if (perform)
                //    perform = this.ProcessCreateReturnFiles();

                if (perform)
                    perform = this.UpdateReturnPrintedFlag();

                if (perform)
                    perform = this.CreateAccountBlocks();

                if (perform)
                    perform = this.AutoResubmitNSF();

                //validate return count against database count
                if (perform)
                {
                    perform = ConfirmReturnCount(file, fi.Name);
                }


                if (perform)
                    FormHandler.DispalyInformationMessage("Returns created successfully for bank " + this.BankName + ".");

                return perform;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create returns failed.   Please contact system administrator.", exc);
                return false;
            }
            finally
            {
                data = null;
                file = null;
            }

        }

        public bool ProcessCreateBatches()
        {
            //int ReamstownAchID = Convert.ToInt32(ConfigurationManager.AppSettings["ReamstownAchID"]);

            try
            {
                List<Merchant> merchants = this.GetPendingBatchMerchants();

                if (merchants.Count == 0)
                {
                    FormHandler.DispalyWarningMessage("No batches to process.");
                    return false;
                }
                else
                {
                    DataAchProcess data = new DataAchProcess();
                    this.CurrentJobID = data.GetNextJobID("Create Batches for BankID " + this.BankID.ToString());

                    foreach (Merchant merchant in merchants)
                    {
                        CreateBatchByUploadID(merchant);

                        //if (merchant.AchID == ReamstownAchID) //9728
                        //    this.CreateBatchReamstown(merchant);
                        //else
                        //    this.CreateBatch(merchant);
                    }
                }
                FormHandler.DispalyInformationMessage("Batches created successfully for bank " + this.BankName + ".");

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Batch process failed.  Please contact system administrator.", exc);
                return false;
            }

        }

        public bool CreateBatchByUploadID(Merchant merchant)
        {
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = "Ach_Batch_Get_UploadID";
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.Add(new SqlParameter("@AchID", merchant.AchID));

            using (SqlDataReader drUploadID = DataLayer.GetDataReader(cmd2, DataLayer.ConnectStringBuild()))
            {
                while (drUploadID.Read())
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Ach_Batch_Process_By_UploadID";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
                    cmd.Parameters.Add(new SqlParameter("@AchID", merchant.AchID));
                    cmd.Parameters.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                    cmd.Parameters.Add(new SqlParameter("@JobID", this.CurrentJobID));
                    cmd.Parameters.Add(new SqlParameter("@UploadID", DataLayer.Field2Int(drUploadID["UploadID"])));

                    using (SqlDataReader dr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
                    {

                        if (dr.Read())
                        {
                            merchant.Batches.Add(new Batch(DataLayer.Field2Int(dr["AchID"]),
                               DataLayer.Field2Int(dr["BatchID"]),
                               DataLayer.Field2Dbl(dr["Debit"]),
                               DataLayer.Field2Dbl(dr["Credit"]),
                               DataLayer.Field2Int(dr["DebitCount"]),
                               DataLayer.Field2Int(dr["CreditCount"]),
                               DataLayer.Field2Int(dr["OverDailyAmountLimitCount"]),
                               DataLayer.Field2Int(dr["OverItemAmount"])));
                        }

                        dr.Close();
                    }
                }

                drUploadID.Close();
            }

            return true;
        }//CreateBatch

        public bool ProcessCreateJournals()
        {
            try
            {
                List<Merchant> merchants = this.GetPendingJournalMerchants();
                //int ReamstownAchID = Convert.ToInt32(ConfigurationManager.AppSettings["ReamstownAchID"]);

                if (merchants.Count == 0)
                    FormHandler.DispalyInformationMessage("No journals to process.");
                else
                {
                    DataAchProcess data = new DataAchProcess();
                    this.CurrentJobID = data.GetNextJobID("Create Journal for BankID " + this.BankID.ToString());

                    foreach (Merchant merchant in merchants)
                    {
                        foreach (Batch batch in merchant.Batches)
                        {
                            this.CreateJournal(batch);

                            //release holds
                            this.JournalReleaseHold(batch.AchID);

                            //if (batch.AchID == ReamstownAchID) //9728
                            //{
                            //    JournalReleaseHold(batch.AchID);
                            //}
                        }
                    }
                }

                this.JournalReleaseHold();

                FormHandler.DispalyInformationMessage("Journals created successfully for bank " + this.BankName + ".");

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Journal process failed.   Please contact system administrator.", exc);
                return false;
            }
        }

        public bool JournalReleaseHold(int AchID)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Ach_Journal_Hold_Release";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
                cmd.Parameters.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                cmd.Parameters.Add(new SqlParameter("@JobID", this.CurrentJobID));
                cmd.Parameters.Add(new SqlParameter("@ManualHoldReleaseAchID", AchID));

                int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

                return true;
            }
            catch (Exception exc)
            {
                throw exc;
                //return false;
            }
        }//JournalReleaseHold 

        public bool ProcessCreateResponseFiles()
        {
            try
            {
                List<Merchant> merchants = this.GetPendingResponseFileMerchants();

                foreach (Merchant merchant in merchants)
                {
                    this.WriteResponseFile(merchant);
                }

                FormHandler.DispalyInformationMessage("Response files create successfully for bank " + this.BankName + ".");

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create response files failed.   Please contact system administrator.", exc);
                return false;
            }

        }

        public bool ProcessCreateReturnFiles()
        {
            try
            {
                List<Merchant> merchants = this.GetPendingReturnFileMerchants();

                foreach (Merchant merchant in merchants)
                {
                    this.WriteReturnFile(merchant);
                }

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create return files failed.   Please contact system administrator.", exc);
                return false;
            }

        }

        public bool UpdateReturnPrintedFlag()
        {
            try
            {

                DataAchProcess data = new DataAchProcess();
                ArrayList prms = new ArrayList();
                prms.Add(new SqlParameter("@BankID", this.BankID));
                data.UpdateReturnPrintedFlag(prms);

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Update return printed flag failed.   Please contact system administrator.", exc);
                return false;
            }

        }
        public bool ProcessCreateReturnEmails()
        {
            try
            {
                List<Merchant> merchants = this.GetPendingReturnEmailMerchants();

                foreach (Merchant merchant in merchants)
                {
                    this.SendReturnEmail(merchant);
                }

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create return email failed.   Please contact system administrator.", exc);
                return false;
            }

        }

        public bool WriteResponseFile(Merchant merchant)
        {
            string strFileSeqNumber = string.Empty;
            string strFile = string.Empty;
            string strLine = string.Empty;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AchID", merchant.AchID));
            prms.Add(new SqlParameter("@CurrentDate", DateTime.Today));

            SqlDataReader dr = data.SelectResponseFileProcess(prms);

            strFileSeqNumber = this.GetLastSequenceNumber(m_FTPCentral + merchant.MerchantID + @"\", ".rsp");
            strFile = m_FTPCentral + merchant.MerchantID + @"\" + DateTime.Today.ToString("yyMMdd") + strFileSeqNumber + ".rsp";
            StreamWriter sw = File.CreateText(strFile);

            while (dr.Read())
            {
                strLine = (char)34 + DataLayer.Field2Str(dr["RefID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Dec(dr["Amount"]).ToString() + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["Reason"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["AddenInfo"]) + (char)34;

                sw.WriteLine(strLine);
            }
            dr.Close();
            data = null;
            sw.Close();

            return true;
        }

        public string GetLastSequenceNumber(string strPath, string strFileType)
        {
            string strFile;
            string strFileSeqNumber;
            for (int loopCnt = 1; ; loopCnt++)
            {
                strFileSeqNumber = loopCnt.ToString().PadLeft(2, Char.Parse("0"));
                strFile = strPath + DateTime.Today.ToString("yyMMdd") + strFileSeqNumber + strFileType;
                if (!File.Exists(strFile))
                {
                    return strFileSeqNumber;
                }
            }
        }

        public bool WriteReturnFile(Merchant merchant)
        {
            string strFileSeqNumber = string.Empty;
            string strFile = string.Empty;
            string strLine = string.Empty;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AchID", merchant.AchID));
            prms.Add(new SqlParameter("@Date", DateTime.Today));

            //SqlDataReader dr = data.SelectReturnFileProcess(prms);
            SqlDataReader dr = data.SelectReturnFileProcessRecreate(prms);

            strFileSeqNumber = this.GetLastSequenceNumber(m_FTPCentral + merchant.MerchantID + @"\", ".rtn");
            strFile = m_FTPCentral + merchant.MerchantID + @"\" + DateTime.Today.ToString("yyMMdd") + strFileSeqNumber + ".rtn";
            StreamWriter sw = File.CreateText(strFile);

            while (dr.Read())
            {
                strLine = (char)34 + DataLayer.Field2Str(dr["RefID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Dec(dr["Amount"]).ToString() + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["ReasonCode"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["ReasonDesc"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["Type"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransType"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["AccountNo"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransRoute"]) + (char)34;

                sw.WriteLine(strLine);
            }
            dr.Close();
            data = null;
            sw.Close();

            return true;
        }

        public bool WriteReturnFile(int AchID, int MerchantID, DateTime date)
        {
            string strFileSeqNumber = string.Empty;
            string strFile = string.Empty;
            string strLine = string.Empty;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AchID", AchID));
            prms.Add(new SqlParameter("@Date", date));

            //SqlDataReader dr = data.SelectReturnFileProcess(prms);
            SqlDataReader dr = data.SelectReturnFileProcessRecreateManual(prms);

            strFileSeqNumber = this.GetLastSequenceNumber(m_FTPCentral + MerchantID.ToString() + @"\", ".rtn");
            strFile = m_FTPCentral + MerchantID.ToString() + @"\" + DateTime.Today.ToString("yyMMdd") + strFileSeqNumber + ".rtn";
            StreamWriter sw = File.CreateText(strFile);

            while (dr.Read())
            {
                strLine = (char)34 + DataLayer.Field2Str(dr["RefID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Dec(dr["Amount"]).ToString() + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["ReasonCode"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["ReasonDesc"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["Type"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransType"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["AccountNo"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransRoute"]) + (char)34;

                sw.WriteLine(strLine);
            }
            dr.Close();
            data = null;
            sw.Close();

            return true;
        }

        public bool SendReturnEmail(Merchant merchant)
        {
            string strMessage = string.Empty;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AchID", merchant.AchID));

            SqlDataReader dr = data.SelectReturnEmailProcess(prms);

            strMessage = "<h2><Strong>Daily ACH Return List Report</Strong></h2>";
            strMessage += "<table border=1>";
            strMessage += "<tr>";
            strMessage += "<td><Strong>MerchantID</Strong></td>";
            strMessage += "<td><Strong>ACHID</Strong></td>";
            strMessage += "<td><Strong>Merchant Name</Strong></td>";
            strMessage += "<td><Strong>RefID</Strong></td>";
            strMessage += "<td><Strong>TransID</Strong></td>";
            strMessage += "<td><Strong>Amount</Strong></td>";
            strMessage += "<td><Strong>Reason Code</Strong></td>";
            strMessage += "<td><Strong>Reason Desc.</Strong></td>";
            strMessage += "<td><Strong>Type</Strong></td>";
            strMessage += "<td><Strong>TransType</Strong></td>";
            strMessage += "<td><Strong>NameonAccount</Strong></td>";
            strMessage += "<td><Strong>Account No</Strong></td>";
            strMessage += "<td><Strong>TransRoute</Strong></td>";
            strMessage += "<td><Strong>Source</Strong></td>";
            strMessage += "<td><Strong>Transaction Date</Strong></td>";
            strMessage += "<td><Strong>Customer ID</Strong></td>";
            strMessage += "<td><Strong>Merchant ZID</Strong></td>";
            strMessage += "</tr>";
            while (dr.Read())
            {
                strMessage += "<tr>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["MerchantID"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["AchID"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["AchCoName"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["RefID"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["TransID"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Dec(dr["Amount"]).ToString() + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["ReasonCode"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["ReasonDesc"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["Type"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["TransType"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["NameonAccount"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["AccountNo"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["TransRoute"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["Source"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["TransDate"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["CustomerID"]) + " &nbsp;</td>";
                strMessage += "<td>" + DataLayer.Field2Str(dr["MerchantZID"]) + " &nbsp;</td>";
                strMessage += "</tr>";
            }
            strMessage += "</table>";
            strMessage += "<p>";
            strMessage += "Please print out this return list and keep it with your bank records.<br />";
            strMessage += "<Strong>NOTE:</Strong> Please do not attempt to respond to this message. We cannot accept electronic replies to this e-mail address.";
            strMessage += "</p>";

            dr.Close();

            DataAchProcess.InsertCommunication("RE: Daily ACH Return List Report", strMessage, strMessage, PaymentXP.BusinessObjects.Constants.DO_NOT_REPLY_EMAIL, merchant.Email, "", PaymentXP.BusinessObjects.Constants.MNGUYEN_MERITUSPAYMENT, merchant.MerchantAppUID);

            //Email.SendEmail("RE: Daily ACH Return List Report", strMessage, "Ach@merituspayment.com", merchant.Email, "mnguyen@merituspayment.com");

            data = null;

            return true;
        }

        public abstract bool JournalReleaseHold();

    }
}
