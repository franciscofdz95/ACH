using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using Nmc.Ach.Dal;
using CommonUtility;
using System.Configuration;
using System.Linq;
using System.Collections;


namespace BatchFileLoader
{
    public class BatchFileNacha: BatchFile
    {
        private NachaFile _NachaFile = new SettlementFile(); 

        public BatchFileNacha(string filename)
            : base(filename)
        {
            this._FileExtension = ".NAC";
        }

        public BatchFileNacha(string fileName, bool encrypted, int zid)
            : base(fileName, encrypted, zid)
        {
            this._FileExtension = ".NAC";
        }


        public override bool ImportFile()
        {
            try
            {
                DateTime nextProcDate;

                int curBatch = 1;
                int curTrans = 1;

                if(this._NachaFile != null && this._NachaFile.Batches.Count == 0)
                {
                    BatchLog.Batch.ErrorFormat("No transactions to process");
                    throw new BatchFileException("No transactions to process", BatchFileErrorCode.NoTransactions);
                }

                foreach (SettlementBatch b in this._NachaFile.Batches)
                {
                    BatchLog.Batch.InfoFormat("Processing batch {0} of {1}......", curBatch, this._NachaFile.Batches.Count);

                    curTrans = 1;
                    //Ani: DM-6778 - PXP - ACH - Implement Validation for ACHBatchUploader
                    BatchLog.Batch.InfoFormat("Start validating batch: {0} with EffectiveEntryDate: {1}", curBatch, b.EffectiveEntryDate);
                    nextProcDate = DateTime.Now;

                    string EffectEntryDt = b.EffectiveEntryDate.Replace("/", string.Empty);
                   
                    DateTime modifiedNextProcDate = DateTime.Now;
                  
                     if (!string.IsNullOrWhiteSpace(EffectEntryDt))
                        {

                            if (DateTime.TryParseExact(b.EffectiveEntryDate, "yy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None, out nextProcDate))
                            {
                                nextProcDate = (nextProcDate.Date <= DateTime.Today.Date) ? DateTime.Now : nextProcDate.Date;
                            }
                        }

                    if (ConfigurationManager.AppSettings.AllKeys.Contains("OverrideDate"))
                    {
                        string modifiedDate = ConfigurationManager.AppSettings["OverrideDate"].ToString();
                        if (!string.IsNullOrEmpty(modifiedDate))
                        {
                            BatchLog.Batch.InfoFormat("Config Modified Date: {0}.", modifiedDate);
                            DateTime.TryParseExact(modifiedDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out modifiedNextProcDate);

                            nextProcDate = (nextProcDate.Date <= modifiedNextProcDate.Date) ? modifiedNextProcDate : nextProcDate;
                        }
                    }
                    //Ani: DM-7033 Only for Rvine internal billings and refunds ZID
                    // If File submitted even after cutoff time but before NACHA file generation keep the nextProcDate as today's date.
                    // If  Nacha file for today is already generated then update the nextProcDate so that the record will get picked up by tomorrow's file.

                    int[] internalBillingAchIds = { 10234, 11405, 10943, 246502 };
                    if (internalBillingAchIds.Contains(this.MerchantID))
                    {
                        ArrayList prms = new ArrayList();
                        DataAchProcess data = new DataAchProcess();
                        prms.Add(new SqlParameter("@BankID", 17));
                        SqlDataReader dr = data.SelectLastDateSubmitted(prms);
                        DateTime dateSubmitted = DateTime.Now.Date;

                        while (dr.Read())
                        {
                            dateSubmitted = DataLayer.Field2Date(dr["DateSubmitted"]);
                        }

                        dr.Close();


                        if (dateSubmitted.Date.Equals(nextProcDate.Date))
                            nextProcDate = DateTime.Now;
                        else
                            nextProcDate = nextProcDate.Date;

                        }

                    BatchLog.Batch.InfoFormat("After validating batch: {0} the EffectiveEntryDate: {1}", curBatch, nextProcDate);

                    foreach (SettlementDetail d in b.Details)
                        {
                            BatchLog.Batch.InfoFormat("Processing batch transaction {0} of {1}......", curTrans, b.Details.Count);

                            Transaction tran = new Transaction();
                            tran.MerchantID = this.MerchantID;
                            tran.Description = b.CompanyDescription;
                            tran.TransType = d.TransType;
                            tran.TransRoute = d.TransRoute;
                            tran.AccountNo = d.AccountNo;
                            tran.NameOnAccount = d.AccountName;
                            tran.RefID = d.IdentificationNumber;
                            tran.Amount = Math.Round(d.Amount, 2);
                            tran.Secc = b.Secc;
                            tran.CompanyName = b.CompanyName;
                            tran.UploadID = this.UploadID;
                            tran.NextProcDate = nextProcDate;

                            long transID = tran.SaveTransaction();

                            BatchLog.Batch.InfoFormat("Batch transaction [TransID={0}] successful", transID);
                            BatchLog.Batch.InfoFormat("Batch transaction {0} of {1} processed.", curTrans, b.Details.Count);

                            curTrans++;
                        }

                    BatchLog.Batch.InfoFormat("Batch {0} of {1} processed.", curBatch, this._NachaFile.Batches.Count);
                    
                    curBatch++;
                }

                BatchLog.Batch.InfoFormat("Nacha batch transaction processing complete.");

                return true;
            }
            catch (BatchFileException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                BatchLog.Batch.ErrorFormat("A {0} has occurred while processing batch file '{1}': {2}", ex.GetType().Name, this.FileName, ex.Message);
                BatchLog.Batch.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                throw new BatchFileException("Transaction processing failed", ex.InnerException, BatchFileErrorCode.ProcessingFailed);
            }

        }

        public override string ParseFile()
        {
            StringBuilder sb = new StringBuilder();
            FileInfo fi = null;
            NachaFile file = new SettlementFile();

            try
            {
                fi = new FileInfo(this.FileName);
                this.ParseNachaFile(ref file, fi);

                this.FileTotal = file.TotalDebit + file.TotalCredit;
                this.TotalTransCount = file.EntryAddendaCount;
            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg += "Batch file " + this.FileName + " failed to load. \n\r";
                msg += "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("Parse Failed - " + this.FileName, msg, Program.m_From, Program.m_To);
                CommonUtility.Logger.LogError(msg);

                sb.Append(exc.Message);
            }

            return sb.ToString();
        }

        private bool ParseNachaFile(ref NachaFile file, FileInfo fi)
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
                            break;
                        case "5":
                            batch = file.CreateBatch();
                            batch.CompanyName = line.Substring(4, 16).Trim();
                            batch.DiscretionaryData = line.Substring(20, 20).Trim();
                            batch.CompanyIdentification = line.Substring(40, 10).Trim();
                            batch.Secc = line.Substring(50, 3).Trim();
                            batch.CompanyDescription = line.Substring(53, 10).Trim();
                            batch.CompanyDescriptiveDate = line.Substring(63, 6).Trim();
                            batch.EffectiveEntryDate = line.Substring(69, 2) + "/" + line.Substring(71, 2) + "/" + line.Substring(73, 2);
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
                            detail.IdentificationNumber = line.Substring(39, 15).Trim();
                            detail.AccountName = line.Substring(54, 22).Trim();
                            detail.TraceNumber = Convert.ToInt64(line.Substring(79, 15));

                            batch.AddDetail(detail);
                            break;
                        case "7":
                            addenda = (SettlementAddenda)detail.CreateAddenda();
                            addenda.RecordType = line.Substring(0, 1).Trim();
                            addenda.TypeCode = line.Substring(1, 2).Trim();
                            addenda.PaymentInfo = line.Substring(3, 80).Trim();
                            addenda.AddendaSeqNumber = Convert.ToInt32(line.Substring(83, 4));
                            addenda.EntrySeqNumber = Convert.ToInt32(line.Substring(87, 7));

                            detail.AddAddenda(addenda);
                            break;
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
                string msg = string.Empty;
                msg += "Parse return file failed.   Please contact system administrator.";

                Email.SendEmail("Load Batch Failed - " + fi.Directory.Name, msg, Program.m_From, Program.m_To);
                CommonUtility.Logger.LogError(msg);

                return false;
            }
            finally
            {
                sr = null;
            }
        }

        public override void ParseFile(string filePath)
        {
            try
            {
                int curLine = 1;

                using (StreamReader sr = File.OpenText(filePath))
                {
                    SettlementAddenda addenda = null;
                    NachaBatch batch = null;
                    NachaDetail detail = null;

                    string line = sr.ReadLine();

                    if (line == null)
                    {
                        BatchLog.Batch.ErrorFormat("Empty NAC file detected.");
                        throw new BatchFileException("Empty NAC file detected.", BatchFileErrorCode.EmptyFile);
                    }

                    while (line != null)
                    {
                        BatchLog.Batch.InfoFormat("Line {0} - Parsing line......", curLine);
                        BatchLog.Batch.InfoFormat("Line {0} - Record: {1}", curLine, line.Substring(0, 1));

                        switch (line.Substring(0, 1))
                        {
                            case "1":
                                BatchLog.Batch.WarnFormat("Line {0} - Ignoring line", curLine);
                                break;

                            case "5":
                                batch = this._NachaFile.CreateBatch();
                                batch.CompanyName = line.Substring(4, 16).Trim();
                                batch.DiscretionaryData = line.Substring(20, 20).Trim();
                                batch.CompanyIdentification = line.Substring(40, 10).Trim();
                                batch.Secc = line.Substring(50, 3).Trim();
                                batch.CompanyDescription = line.Substring(53, 10).Trim();
                                batch.CompanyDescriptiveDate = line.Substring(63, 6).Trim();
                                batch.EffectiveEntryDate = line.Substring(69, 2) + "/" + line.Substring(71, 2) + "/" + line.Substring(73, 2);
                                batch.SettlementDate = line.Substring(75, 3).Trim();
                                batch.OriginatingDFI = line.Substring(78, 8).Trim();
                                batch.BatchNumber = Convert.ToInt32(line.Substring(87, 7));

                                BatchLog.Batch.InfoFormat("Line {0} - Nacha batch parsed.", curLine);

                                break;

                            case "6":
                                detail = batch.CreateDetail();
                                detail.TransType = line.Substring(1, 2).Trim();
                                detail.TransRoute = line.Substring(3, 9).Trim();
                                detail.AccountNo = line.Substring(12, 17).Trim();
                                detail.Amount = Convert.ToDecimal(line.Substring(29, 10)) / 100;
                                detail.IdentificationNumber = line.Substring(39, 15).Trim();
                                detail.AccountName = line.Substring(54, 22).Trim();
                                detail.TraceNumber = Convert.ToInt64(line.Substring(79, 15));

                                batch.AddDetail(detail);

                                BatchLog.Batch.InfoFormat("Line {0} - Nacha transaction parsed.", curLine);

                                break;

                            case "7":
                                addenda = (SettlementAddenda)detail.CreateAddenda();
                                addenda.RecordType = line.Substring(0, 1).Trim();
                                addenda.TypeCode = line.Substring(1, 2).Trim();
                                addenda.PaymentInfo = line.Substring(3, 80).Trim();
                                addenda.AddendaSeqNumber = Convert.ToInt32(line.Substring(83, 4));
                                addenda.EntrySeqNumber = Convert.ToInt32(line.Substring(87, 7));

                                detail.AddAddenda(addenda);

                                BatchLog.Batch.InfoFormat("Line {0} - Nacha Addenda parsed.", curLine);

                                break;

                            case "8":
                                this._NachaFile.AddBatch(batch);

                                BatchLog.Batch.InfoFormat("Line {0} - Nacha end of batch parsed.", curLine);
                                break;

                            default:
                                BatchLog.Batch.WarnFormat("Line {0} - Ignoring line", curLine);
                                break;
                        }

                        line = sr.ReadLine();
                        curLine++;
                        
                    }

                    this.FileTotal = this._NachaFile.TotalDebit + this._NachaFile.TotalCredit;
                    this.TotalTransCount = this._NachaFile.EntryAddendaCount;

                    sr.Close();
                }
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


    }
}
