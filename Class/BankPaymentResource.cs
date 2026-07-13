using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections;
using System.Text;
using System.Configuration;
using Nmc.Ach.Dal;

namespace AchSystem
{
    /// <summary>
    /// Summary description for BankNcal.
    /// </summary>
    public class BankPaymentResource : Bank
    {
        public BankPaymentResource() { }

        public BankPaymentResource(string bank, int bankid)
            : base(bank, bankid) 
        {
            string SettleFileDirectory = ConfigurationManager.AppSettings["SettlementFileDirectory"].ToString();
            this.SettlementFileName = SettleFileDirectory + DateTime.Today.ToString("yyMMdd") + "01" + ".cck";

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
            SqlDataReader dr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());

            if (dr.Read())
                merchant.Batches.Add(new Batch(DataLayer.Field2Int(dr["AchID"]),
                   DataLayer.Field2Int(dr["BatchID"]),
                   DataLayer.Field2Dbl(dr["Debit"]),
                   DataLayer.Field2Dbl(dr["Credit"]),
                   DataLayer.Field2Int(dr["DebitCount"]),
                   DataLayer.Field2Int(dr["CreditCount"]),
                   DataLayer.Field2Int(dr["OverDailyAmountLimitCount"]),
                   DataLayer.Field2Int(dr["OverItemAmount"])));

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

        public override bool CreateSettlementFile()
        {
            try
            {
                ArrayList prms = new ArrayList();
                DataAchProcess data = new DataAchProcess();

                this.CurrentJobID = data.GetNextJobID("Create File for BankID " + this.BankID.ToString());


                //Build settlment records
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@Date", DateTime.Today));

                SqlDataReader dr = data.SelectSettlementData(prms);

                if (!dr.HasRows)
                {
                    FormHandler.DispalyInformationMessage("No data to write to file.");
                    return false;
                }

                StreamWriter sw = new StreamWriter(this.SettlementFileName);


                string line = string.Empty;

                while (dr.Read())
                {

                    line = DataLayer.Field2StrQuoted(dr["Description"]) + ",";
                    if (dr["TransType"].ToString().Substring(0,1) == "2")
                        line += DataLayer.Field2StrQuoted("CK") + ",";
                    else
                        line += DataLayer.Field2StrQuoted("SA") + ",";

                    line += DataLayer.Field2StrQuoted(dr["TransType"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["TransRoute"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["AccountNo"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["NameOnAccount"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["TraceNumber"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["Amount"]).Replace(",", "") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted(dr["CompanyName"]);

                    sw.WriteLine(line);
                }

                dr.Close();
                dr = null;

                //Build hold release and preauth dr records
                prms.Clear();
                prms.Add(new SqlParameter("@BankID", this.BankID));
                prms.Add(new SqlParameter("@Date", DateTime.Today));
                prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                prms.Add(new SqlParameter("@JobID", this.CurrentJobID));

                dr = data.SelectPaymentData(prms);
                               
                while (dr.Read())
                {
                    if (dr["Description"].ToString() == "HOLD RELEA") //This condition is put in place to correct the reporting issue for TF
                        line = DataLayer.Field2StrQuoted("DEPOSIT") + ",";
                    else
                        line = DataLayer.Field2StrQuoted(dr["Description"]) + ",";

                    if (dr["TransType"].ToString().Substring(0, 1) == "2")
                        line += DataLayer.Field2StrQuoted("CK") + ",";
                    else
                        line += DataLayer.Field2StrQuoted("SA") + ",";

                    line += DataLayer.Field2StrQuoted(dr["TransType"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["TransRoute"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["AccountNo"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["NameOnAccount"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["TraceNumber"]) + ",";
                    line += DataLayer.Field2StrQuoted(dr["Amount"]).Replace(",","") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted("") + ",";
                    line += DataLayer.Field2StrQuoted(dr["CompanyName"]);

                    sw.WriteLine(line);
                }

                dr.Close();
                dr = null;

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

        public override bool CreateReturns(FileInfo fi)
        {
            DataAchProcess data = new DataAchProcess();
            NachaFile file = null;
            bool perform = false;

            try
            {
                this.CurrentJobID = data.GetNextJobID("Create Returns for BankID " + this.BankID.ToString());

                if (this.CurrentJobID != -1)
                    perform = data.TruncateStagingReturn();

                if (perform)
                    perform = this.InsertReturnsIntoStaging(fi);

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

                if (perform)
                    perform = this.ProcessCreateReturnFiles();

                if (perform)
                    perform = this.CreateAccountBlocks();

                if (perform)
                    FormHandler.DispalyInformationMessage("Return files created successfully for bank " + this.BankName + ".");

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

        private bool InsertReturnsIntoStaging(FileInfo fi)
        {
            DataAchProcess data = new DataAchProcess();
            SqlParameter prm = null;
            ArrayList prms = new ArrayList();
            long TransID = -1;
            string strLine = string.Empty;
            string strError = string.Empty;
            StreamReader sr = null;

            try
            {
                //Pass the file path and file name to the StreamReader constructor
                sr = new StreamReader(fi.FullName);

                //Read the first line of text
                strLine = sr.ReadLine();

                //Continue to read until you reach end of file
                while (strLine != null)
                {
                    string[] fields = strLine.Split(new string[] { "," }, StringSplitOptions.None);

                    if (fields.Length != 9)
                        throw new Exception("File format is invalid (Column Count Error)");


                    prms.Clear();
                    prm = new SqlParameter("@ReturnID", -1);
                    prm.Direction = ParameterDirection.Output;
                    prms.Add(prm);
                    prms.Add(new SqlParameter("@PostedDate", DateTime.Now));
                    prms.Add(new SqlParameter("@TransType", fields[6].Replace("\"", "")));
                    prms.Add(new SqlParameter("@TransRoute", string.Empty));
                    prms.Add(new SqlParameter("@AccountNo", string.Empty));
                    prms.Add(new SqlParameter("@NameOnAccount", string.Empty));
                    prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(fields[2].Replace("\"", ""))));
                    prms.Add(new SqlParameter("@AddenCode", string.Empty));
                    prms.Add(new SqlParameter("@ReasonCode", fields[3].Replace("\"", "")));
                    if (fields[3].Replace("\"", "").Substring(0, 1) == "C")
                    {
                        switch (fields[3].Replace("\"", ""))
                        { 
                            case "C01":
                                prms.Add(new SqlParameter("@AddenInfo", fields[7].Replace("\"", "")));
                                break;
                            case "C02":
                                prms.Add(new SqlParameter("@AddenInfo", fields[8].Replace("\"", "")));
                                break;
                            case "C03":
                                prms.Add(new SqlParameter("@AddenInfo", DataLayer.FilePadString(fields[8].Replace("\"", ""), 12) + fields[7].Replace("\"", "")));
                                break;
                            case "C05":
                                if (fields[6].Substring(0,1) == "2")
                                    prms.Add(new SqlParameter("@AddenInfo", "3" + fields[6].Replace("\"", "").Substring(1, 1)));
                                else
                                    prms.Add(new SqlParameter("@AddenInfo", "2" + fields[6].Replace("\"", "").Substring(1, 1)));

                                break;
                            case "C06":
                                prms.Add(new SqlParameter("@AddenInfo", DataLayer.FilePadString(fields[7].Replace("\"", ""), 20) + fields[7].Replace("\"", "")));
                                break;
                        }
                    }
                    else
                        prms.Add(new SqlParameter("@AddenInfo", string.Empty));

                    prms.Add(new SqlParameter("@OrigTrace", fields[0].Replace("\"", "")));
                    prms.Add(new SqlParameter("@OrigRDFI", string.Empty));
                    prms.Add(new SqlParameter("@Trace", fields[1].Replace("\"", "")));
                    prms.Add(new SqlParameter("@FileName", fi.Name));

                    data.InsertStagingReturn(prms);

                    strLine = sr.ReadLine();
                }

                sr.Close();

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



    }
}
