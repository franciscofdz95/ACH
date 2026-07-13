using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Nmc.Ach.Dal
{
    public class DataRecurring : iData
    {
        public SqlDataReader Select(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Recurring";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet Search(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Recurring";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public int Delete(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Delete_Recurring";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int Update(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Recurring";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public long Insert(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Recurring";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@Recur_ID"].Value);
            else
                return -1;
        }

        public SqlDataReader SelectDuePayments(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_RecurringDuePayments";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public int UpdatePaymentSchedule(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_UpdateRecurringPaymentSchedule";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }


        public void InsertACHSilentPost(long transId, int merchantId, string responseStr)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ACH_Insert_Silent_Response";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TransID", transId);
            cmd.Parameters.AddWithValue("@MerchantID", merchantId);
            cmd.Parameters.AddWithValue("@ResponseString", responseStr);

            DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public DataTable GetScheduleRecurringPayments_Notifications(DateTime rundate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Ach_Search_Upcoming_Recurring";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@RunDate", rundate);

            return DataLayer.GetDataTable(cmd, DataLayer.ConnectStringBuild());
        }
    }
}
