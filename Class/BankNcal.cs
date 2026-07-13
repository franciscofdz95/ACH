using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections;
using System.Text;
using Nmc.Ach.Dal;

namespace AchSystem
{
    /// <summary>
    /// Summary description for BankNcal.
    /// </summary>
    public class BankNcal : Bank
    {
        public BankNcal() { }

        public BankNcal(string bank, int bankid)
            : base(bank, bankid)
        {
            if (!Directory.Exists(this.SettleFileDirectory + bank))
            {
                try
                {
                    Directory.CreateDirectory(this.SettleFileDirectory + bank);

                }
                catch
                {
                    MessageBox.Show("Cannot create directory!");
                }
            }

            this.SettlementFileName = this.SettleFileDirectory + bank + @"\NCAL_" + DateTime.Today.ToString("yyyyMMdd") + "01" + ".ACH";

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
                ArrayList prms = new ArrayList();
                DataAchProcess data = new DataAchProcess();

                this.CurrentJobID = data.GetNextJobID("Create File for BankID " + this.BankID.ToString());


                //Build settlment records
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@Date", DateTime.Today));

                prms.Add(new SqlParameter("@ProcessDate", processDate));
                prms.Add(new SqlParameter("@CuttOffTime", cuttOffTime));
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

                        if ((DataLayer.Field2Str(dr["Secc"]) == "WEB" || DataLayer.Field2Str(dr["Secc"]) == "TEL") && DataLayer.Field2Str(dr["TransType"]).Substring(1, 1) == "2")
                            batch.Secc = "PPD"; //Need to change to a PPD batch because WEB batch cannot have credits
                        else
                            batch.Secc = DataLayer.Field2Str(dr["Secc"]);


                        batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
                        batch.CompanyDescriptiveDate = DataLayer.Field2Str(dr["DescDate"]);
                        batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        EntryDate = batch.EntyDate;
                        batch.OriginatingDFI = this.ImmediateDestination.Substring(1, 8);//this.ImmediateOrigin.Substring(1, 8);this.ImmediateOrigin.Substring(1, 8);

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

                    //int z = 0;
                    //if (detail.Amount == Convert.ToDecimal(497.58))
                    //    z = 0;

                    //if (batch.Secc == "WEB" && detail.TransType.Substring(1, 1) == "2") //Credit cannot contain in WEB batches
                    //{
                    //    this.CreateBatchOffsetWEB(file, batch, ref DetailCount);
                    //    this.CreateSettlementBatch(ref file, ref batch, ref detail, dr, ref LastBatchID, ref LastBatchFileID);
                    //} if (batch.Secc == "PPD" && (detail.TransType.Substring(1, 1) == "7" && detail.TransType.Substring(1, 1) == "8")) 


                    batch.AddDetail(detail);
                    //Ani: DM-6867
                    data.InsertTransactionsToBankFileDetail(dbBankFileID, (int)detail.TransID);
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
                        if (Description != string.Empty) //If this is not the first record
                        {
                            this.CreateBatchOffsetDetailRecord(batch, ref DetailCount);
                        }

                        Description = DataLayer.Field2Str(dr["Description"]);

                        //Create new batch
                        batch = file.CreateBatch();
                        batch.AchID = DataLayer.Field2Int(dr["AchID"]);
                        batch.BatchID = DataLayer.Field2Int(dr["BatchID"]);
                        batch.CompanyName = DataLayer.Field2Str(dr["CompanyName"]);
                        batch.DiscretionaryData = DataLayer.Field2Str(dr["DiscretionaryData"]);
                        batch.CompanyIdentification = this.CompanyID; //1262437777
                        batch.Secc = DataLayer.Field2Str(dr["Secc"]);
                        batch.CompanyDescription = DataLayer.Field2Str(dr["Description"]);
                        batch.CompanyDescriptiveDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        batch.EntyDate = DataLayer.Field2Date(dr["EntryDate"]).ToString("yyMMdd");
                        EntryDate = batch.EntyDate;
                        batch.OriginatingDFI = this.ImmediateDestination.Substring(1, 8);//this.ImmediateOrigin.Substring(1, 8);
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

                if (batch != null)
                    this.CreateBatchOffsetDetailRecord(batch, ref DetailCount);

                dr.Close();
                dr = null;


                /* update last tracenumber and batchid */
                int rows = data.UpdateLastTraceNumber(this.BankID, DetailCount);
                if (rows == 0)
                    throw new System.Exception("Unable to update Last TraceNumber");

                rows = data.UpdateLastBatchID(this.BankID, file.FileBatchNumber);
                if (rows == 0)
                    throw new System.Exception("Unable to update Last BatchID");

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

        public override bool CreateBatch(Merchant merchant)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Process";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
            cmd.Parameters.Add(new SqlParameter("@AchID", merchant.AchID));
            cmd.Parameters.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
            cmd.Parameters.Add(new SqlParameter("@JobID", this.CurrentJobID));

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
            return true;
        }//CreateBatch

        public override bool CreateJournal(Batch batch)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Ach_Journal_Process";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
                cmd.Parameters.Add(new SqlParameter("@AchID", batch.AchID));
                cmd.Parameters.Add(new SqlParameter("@BatchID", batch.BatchID));
                cmd.Parameters.Add(new SqlParameter("@Debit", batch.Debit));
                cmd.Parameters.Add(new SqlParameter("@Credit", batch.Credit));
                cmd.Parameters.Add(new SqlParameter("@DebitCount", batch.DebitCount));
                cmd.Parameters.Add(new SqlParameter("@CreditCount", batch.CreditCount));
                cmd.Parameters.Add(new SqlParameter("@OverDailyAmountLimitCount", batch.OverDailyAmountLimitCount));
                cmd.Parameters.Add(new SqlParameter("@OverItemAmount", batch.OverItemAmount));
                cmd.Parameters.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                cmd.Parameters.Add(new SqlParameter("@JobID", this.CurrentJobID));
                int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

                return true;
            }
            catch (Exception exc)
            {
                throw exc;
                //return false;
            }
        }//CreateJournal
        public override bool JournalReleaseHold()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Ach_Journal_Hold_Release";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@BankID", this.BankID));
                cmd.Parameters.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                cmd.Parameters.Add(new SqlParameter("@JobID", this.CurrentJobID));

                int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

                return true;
            }
            catch (Exception exc)
            {
                throw exc;
                //return false;
            }
        }//JournalReleaseHold 


    }
}
