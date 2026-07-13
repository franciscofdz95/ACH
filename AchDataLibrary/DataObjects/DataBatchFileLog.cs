using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace Nmc.Ach.Dal
{
    public class DataBatchFileLog : iData
    {
        public SqlDataReader Select(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Batch_File_Log";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet Search(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Batch_File_Log";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public int Delete(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Delete_Batch_File_Log";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int Update(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Update_Batch_File_Log";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public long Insert(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_FileToBeUpload";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@FileID"].Value);
            else
                return -1;
        }
    }
}
