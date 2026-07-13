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
	/// Summary description for BankFifthThird.
	/// </summary>
	public class BankFifthThird:Bank
	{
		public BankFifthThird(){}

        public BankFifthThird(string bank, int bankid)
            : base(bank, bankid)
        {
            string SettleFileDirectory = ConfigurationManager.AppSettings["SettlementFileDirectory"].ToString();
            this.SettlementFileName = SettleFileDirectory + DateTime.Today.ToString("yyMMdd") + "01" + ".FED";

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



   
 
	}
}
