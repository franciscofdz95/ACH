using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;


namespace Nmc.Ach.Dal
{
    public class DataPendingJournal : iData
    {
        public DataSet Search(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Pending_Journal";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader Select(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Pending_Journal";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        
        public int Delete(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Delete_Pending_Journal";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int Update(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Pending_Journal";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public long Insert(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Pending_Journal";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@PendingJournalID"].Value);
            else
                return -1;
        }

   
    }
}
