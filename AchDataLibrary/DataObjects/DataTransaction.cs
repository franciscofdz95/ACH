using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace Nmc.Ach.Dal
{
    public class DataTransaction:iData 
    {
        public DataSet Search(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Transactions";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader Select(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Transaction";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public int Update(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Transaction";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public long Insert(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@TransID"].Value);
            else
                return -1;
        }
        public int UpdateReleaseOverVolumeMerchant(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Release_OverVolumeMerchant";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }
        public int UpdateReleaseOverTicketItem(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Release_OverTicketItem";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public long CopyRecord(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction_Copy";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@NewTransID"].Value);
            else
                return -1;

            
        }

        public long InsertCredit(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction_Credit";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@NewTransID"].Value);
            else
                return -1;
        }


        public long InsertResubmit(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction_Resubmit";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@NewTransID"].Value);
            else
                return -1;
        }

        public int Delete(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Delete_Transaction";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }


        public DataSet SearchUploadBatch(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Upload_Batch";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SearchReturnRates(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_ReturnRates";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SearchReturnSummary(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_ReturnSummaryByType";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SearchMonthlyActivityTotals(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_MonthlyActivityTotals";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SearchMerchantNegativeBalanceReturn(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_MerchantNegativeBalanceReturn";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SearchExceedAverageTicket(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_ExceedAverageTicket";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SearchExceedMonthlyVolume(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_ExceedMonthly";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SearchOTMonthlyActivityTotals(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_OTMonthlyActivityTotals";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SearchUnauthorizedReturnRates(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Report_UnauthorizedReturnRates";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }



        public int DeleteUploadBatch(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Delete_Upload_Batch";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int UpdateTransactionStatus(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Transaction_Status";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL (cmd, DataLayer.ConnectStringBuild());
        }

        public int UpdateTransactionProcessDate(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Transaction_ProcessDate";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }  

        public long InsertTransactionWS(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction_WS";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@TransID"].Value);
            else
                return -1;
        }

        public DataSet SearchOverTicketItems(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_OverTicket_Transactions";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SearchTransactionsLight(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Transactions_Light";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectRecurringTransactions(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Recurring_Transactions";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectQuickbooksResubmittals(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Quickbooks_Resubmittals";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectQuickbooksSettlements(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Quickbooks_Settlements";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectQuickbooksReturns(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Quickbooks_Returns";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SelectOverVolumeMerchants(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_OverVolumeMerchants";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet (cmd, DataLayer.ConnectStringBuild());
        }

        public void InsertBillRetryTransaction(ArrayList prms)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_Insert_BillRetryTransaction";
                cmd.CommandType = CommandType.StoredProcedure;
                DataLayer.AppendParamters(cmd, prms);

                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
            }
        }
    }
}
