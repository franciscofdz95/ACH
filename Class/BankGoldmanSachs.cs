using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using CommonUtility;
using System.Collections.ObjectModel;
using Infragistics.Win.UltraWinSchedule;
using PaymentXP.BusinessObjects.Zeus;
using Infragistics.Win.UltraWinScrollBar;

namespace AchSystem
{
    public class BankGoldmanSachs : Bank
    {
        private readonly ReadOnlyDictionary<string, string> directories;

        public string FileID { get; set; } = "A";

        public BankGoldmanSachs()
        {

        }
        public BankGoldmanSachs(string bank, int bankid)
            : base(bank, bankid)
        {
            directories = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>() {
                {"Outgoing", $@"{this.SettleFileDirectory}{bank}\Outgoing\{DateTime.Today:yyyy}\{$"{DateTime.Today:MMM}".ToUpper()}\"},
                {"Archive", $@"{this.SettleFileDirectory}{bank}\Archive\{DateTime.Today:yyyy}\{$"{DateTime.Today:MMM}".ToUpper()}\"}
            });            

            foreach (string directory in directories.Values)
            {
                if (!Directory.Exists(directory))
                {
                    try
                    {
                        Directory.CreateDirectory(directory);
                    }
                    catch
                    {
                        MessageBox.Show("Cannot create directory!\n "+ directory);
                    }
                }
            }
            this.SettlementFileName = $@"{directories["Outgoing"]}GSBACH_{DateTime.Today:yyyyMMdd}_{DateTime.Now:HHmmss}.ACH";

        }

        public override bool CreateSettlementFile(DateTime Dt)
        {
            //Ani: Start

            ArrayList prms = new ArrayList();
            DataAchProcess data = new DataAchProcess();
            prms.Add(new SqlParameter("@BankID", this.BankID));
            SqlDataReader dr = data.SelectLastDateSubmitted(prms);
            DateTime dateSubmitted = DateTime.Now.Date;

            while (dr.Read())
            {
                dateSubmitted = DataLayer.Field2Date(dr["DateSubmitted"]);
            }

            dr.Close();

            //DateTime Dt = DateTime.Now.Date;

            if (dateSubmitted.Date.Equals(Dt.Date))
            {
                return CreateSettlementFile("ALL", Dt, Dt.AddDays(-1));
            }
            else
            {
                while (Dt.Date > dateSubmitted.Date)
                {
                    CreateSettlementFile("ALL", Dt, Dt.AddDays(-1));
                    Dt = Dt.AddDays(-1);
                }
                return true;
            }
        }

        public bool CreateSettlementFile(string RecordType, DateTime processDate, DateTime cuttOffTime)
        {
            try
            {
                if (this.FileID == ":")  // On ASCII this is after "9"
                    throw new System.Exception("Exceeded the FileID limit for today.");

                ArrayList prms = new ArrayList();
                DataAchProcess data = new DataAchProcess();

                this.CurrentJobID = data.GetNextJobID($"Create Settlement File for BankID {this.BankID} ({(AchBankInfo)this.BankID}).");

                //Build settlment records
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@Date", DateTime.Today));
                //Ani:DM-6867
                prms.Add(new SqlParameter("@ProcessDate", processDate));
                prms.Add(new SqlParameter("@CuttOffTime", cuttOffTime));
                SqlDataReader dr = data.SelectSettlementData(prms);

                if (!dr.HasRows)
                {
                    FormHandler.DispalyInformationMessage("No data to write to file.");
                    return false;
                }
                

                NachaFile file = new SettlementFile(this);
                file.FileID = this.FileID;
                NachaBatch batch = null;
                NachaDetail detail = null;
                SettlementAddenda addenda = null;

                /* Get last tracenumber and batchid */
                int DetailCount = data.SelectLastTraceNumber(this.BankID);
                file.FileBatchNumber = data.SelectLastBatchID(this.BankID);

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

                //Ani: DM-6867
                int dbBankFileID = data.InsertBankFiles(this.BankID, Path.GetFileName(this.SettlementFileName), 0, 0, main.g_User.UserID, DateTime.Now, 0, DateTime.MaxValue);

                #region Load Data DB
                while (dr.Read())
                {
                    if (LastBatchID != DataLayer.Field2Int(dr["BatchID"]) || LastBatchFileID != DataLayer.Field2Int(dr["FileBatchID"]))
                    {
                        LastBatchID = DataLayer.Field2Int(dr["BatchID"]);
                        LastBatchFileID = DataLayer.Field2Int(dr["FileBatchID"]);

                        //Create new batch
                        batch = file.CreateBatch();
                        batch.AchID = DataLayer.Field2Int(dr["AchID"]);
                        batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
                        batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
                        batch.DiscretionaryData = "";
                        batch.CompanyIdentification = "GS00007356";
                        // Entry Detail Record – type 6
                        if ((DataLayer.Field2Str(dr["Secc"]) == "WEB" || DataLayer.Field2Str(dr["Secc"]) == "TEL") 
                            && DataLayer.Field2Str(dr["TransType"]).Substring(1, 1) == "2")
                            batch.Secc = "PPD"; //Need to change to a PPD batch because WEB batch cannot have credits
                        else
                            batch.Secc = DataLayer.Field2Str(dr["Secc"]);

                        batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
                        batch.CompanyDescriptiveDate = DataLayer.Field2Str(dr["DescDate"]);
                        batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        EntryDate = batch.EntyDate;
                        batch.OriginatingDFI = this.ImmediateDestination.Equals(string.Empty) ? string.Empty : this.ImmediateDestination.Trim().Substring(0, 8);

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
                    detail.RecurID = DataLayer.Field2Int(dr["Recur_ID"]);

                    //Create new addenda record
                    if (DataLayer.Field2Int(dr["Addenda_TransID"]) != 0)
                    {
                        addenda = (SettlementAddenda)detail.CreateAddenda();
                        addenda.PaymentInfo = DataLayer.Field2Str(dr["AddentInfo"]);
                        addenda.AddendaSeqNumber = DataLayer.Field2Int(dr["SequenceNo"]);
                        addenda.EntrySeqNumber = detail.TraceNumber;
                        detail.AddAddenda(addenda);
                    }

                    batch.AddDetail(detail);
                    //Ani: DM-6867
                    data.InsertTransactionsToBankFileDetail(dbBankFileID, (int)detail.TransID);
                }

                dr.Close();
                dr = null;

                //Build hold release and preauth dr records
                batch = null;
                detail = null;

                prms.Clear();
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@Date", DateTime.Today));
                prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                prms.Add(new SqlParameter("@JobID", this.CurrentJobID));

                dr = data.SelectPaymentData(prms);

                string Description = string.Empty;

                while (dr.Read())
                {
                    if (Description != DataLayer.Field2Str(dr["Description"]))
                    {
                        Description = DataLayer.Field2Str(dr["Description"]);

                        //Create new batch
                        batch = file.CreateBatch();
                        batch.AchID = DataLayer.Field2Int(dr["AchID"]);
                        batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
                        batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
                        batch.DiscretionaryData = "";
                        batch.CompanyIdentification = this.CompanyID;
                        batch.Secc = DataLayer.Field2Str(dr["Secc"]);
                        batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
                        batch.CompanyDescriptiveDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        EntryDate = batch.EntyDate;
                        batch.OriginatingDFI = this.ImmediateOrigin.Equals(string.Empty) ? string.Empty : this.ImmediateOrigin.Trim().Substring(0, 8);
                        batch.BatchNumber = 1;

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
                    detail.TraceNumber = ++DetailCount;

                    batch.AddDetail(detail);

                }

                dr.Close();
                dr = null;
                #endregion

                /* update last tracenumber and batchid */
                int rows = data.UpdateLastTraceNumber(this.BankID, DetailCount);
                if (rows == 0)
                    throw new System.Exception("Unable to update Last TraceNumber");

                rows = data.UpdateLastBatchID(this.BankID, file.FileBatchNumber);
                if (rows == 0)
                    throw new System.Exception("Unable to update Last BatchID");

                #region Write File
                
                //StreamWriter sw = new StreamWriter(SettlementFileName);
                using (StreamWriter sw = new StreamWriter(SettlementFileName))
                {
                    //Print file header
                    //File Header Record – Type 1
                    sw.WriteLine(file.GetFileHeader());

                    //Print batch offsets
                    foreach (SettlementBatch b in file.Batches)
                    {
                        // Batch Header Record – type 5
                        sw.WriteLine(b.GetBatchHeader());
                        foreach (SettlementDetail d in b.Details)
                        {
                            // Entry Detail Record – type 6
                            sw.WriteLine(d.GetFileDetail());
                            if (d.Addendas.Count > 0)
                                sw.WriteLine(d.Addendas[0].GetFileAddenda());// Addenda Record – type 7
                        }
                        // Batch Control Record – type 8
                        sw.WriteLine(b.GetBatchFooter());
                        //Ani: DM-6867 - To set Total Debit, Total Credit and IsSubmitted to TRUE after successful creation of the Batch
                        try
                        {

                            data.InsertBankFiles(0, Path.GetFileName(this.SettlementFileName), file.TotalDebit, file.TotalCredit, 0, DateTime.MaxValue, main.g_User.UserID, DateTime.Now);
                        }
                        catch (Exception aEx)
                        {

                            FormHandler.DispalyErrorMessage("Update for InsertBankFiles failed for BatchID {" + b.BatchID + "}");
                        }
                    }

                    //Print file footer Type 9
                    sw.WriteLine(file.GetFileFooter());

                    //sw.Close();                
                }
                
                if (file.FileID == "Z")
                    file.FileID = "/"; // On ASCII this is before Zero
                this.FileID = ((char)((file.FileID[0]) + 1)).ToString();
                
                #endregion

                #region Encrypt File 
                FileInfo SettlementFileInfo = new FileInfo(SettlementFileName);
                string PaysafePriv = ConfigurationManager.AppSettings["PaysafePriv"];
                string PaysafePub = ConfigurationManager.AppSettings["PaysafePub"];
                string publicKey_GS = ConfigurationManager.AppSettings["PublicKey_GS"];

                // Encrypt NACHA file for GS
                string errorMessage = string.Empty;
                var encryptedGS_result = PGPHandler.EncryptAndSign(SettlementFileName, $"{SettlementFileName}.pgp", publicKey_GS, PaysafePriv, ref errorMessage);
                if (encryptedGS_result)
                {                   
                    #region Upload File
                    //TODO: 

                    File.Move($"{SettlementFileName}.pgp", $"{directories["Archive"]}{SettlementFileInfo.Name}.pgp");
                    FormHandler.DispalyInformationMessage("Settlement file created successfully for bank " + this.BankName + ". File located at " + $"{directories["Archive"]}{SettlementFileInfo.Name}.pgp");
                    #endregion
                }
                else
                {
                    FormHandler.DispalyErrorMessage("Create encrypt settlement file failed. Please contact system administrator. \n" + errorMessage);
                }

                // Encrypt NACHA file to backup
                var encryptedPS_result = PGPHandler.EncryptAndSign(SettlementFileName, $"{directories["Archive"]}{SettlementFileInfo.Name}_PS.pgp", PaysafePub, PaysafePriv, ref errorMessage);
                if (encryptedPS_result)
                {
                    File.Delete(SettlementFileName);
                    FormHandler.DispalyInformationMessage("Settlement file backup created successfully for bank " + this.BankName + ". File located at " + $"{directories["Archive"]}{SettlementFileInfo.Name}_PS.pgp");
                }
                else
                {
                    FormHandler.DispalyErrorMessage("Create encrypt settlement file to backup failed. Please contact system administrator. \n" + errorMessage);
                }
                #endregion

                
                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create settlement file failed. Please contact system administrator.", exc);
                return false;
            }

        }
        public override bool CreateBatch(Merchant merchant)
        {
            return true;
        }//CreateBatch

        public override bool CreateJournal(Batch batch)
        {
            return true;
        }//CreateJournal

        public override bool JournalReleaseHold()
        {
            return true;
        }//JournalReleaseHold
    }
}
