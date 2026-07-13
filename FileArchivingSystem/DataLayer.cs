using System;
using System.Data;
using System.Data.SqlClient;

namespace FileArchivingSystem
{
	/// <summary>
	/// Summary description for DataLayer.
	/// </summary>
	/// 
	public class DataLayer
	{
		public DataLayer()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string ConnectStringBuild(string LoginID,string Password)
		{
			string strConn;

			if (LoginID == string.Empty)
			{
				LoginID = "sa";
			}
													
			strConn = string.Format("Server=(local);Database=;User ID={0};Password={1}", LoginID, Password);
			return strConn;
		}

		public static string ConnectStringBuild()
		{
			return "Data Source=SQLBATCH;Initial Catalog=ACHCentral;User ID=Program_ACHCentral;password=!12Ceiling#;";
		}


		public static System.Data.DataSet GetDataSet (string SQL, string ConnectionString)
		{     
			SqlDataAdapter da; 
			System.Data.DataSet ds;

			try
			{
				//Create New DataAdapter
				da = new SqlDataAdapter(SQL, ConnectionString);

				//Fill DataSet from DataAdapter
				ds = new System.Data.DataSet();
				da.Fill(ds);
			}
			catch(SqlException e)
			{
				throw e;
			}
			return ds;
		}

		public static System.Data.DataSet GetDataSet (SqlCommand cmd, string ConnectionString)
		{     
			SqlDataAdapter da; 
			System.Data.DataSet ds;

			try
			{
				//Create New DataAdapter
				da = new SqlDataAdapter();
				cmd.Connection = new SqlConnection(ConnectionString);
				cmd.Connection.Open();
				da.SelectCommand = cmd;

				//Fill DataSet from DataAdapter
				ds = new System.Data.DataSet();
				da.Fill(ds);
			}
			catch(SqlException e)
			{
				throw e;
			}
			return ds;
		}

		public static SqlDataReader GetDataReader(string SQLStatement, string ConnectionString) 
		{
			SqlDataReader dr; 

			try
			{
				
				SqlCommand cmd = new SqlCommand();
				cmd.Connection = new SqlConnection (ConnectionString);
				cmd.Connection.Open();
				cmd.CommandText = SQLStatement;
				dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch(SqlException e)
			{
				throw e;
			}
			return dr;

		}

        public static SqlDataReader GetDataReader(SqlCommand cmd, string ConnectionString)
        {
            SqlDataReader dr;

            try
            {
                cmd.Connection = new SqlConnection(ConnectionString);
                cmd.Connection.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException e)
            {
                throw e;
            }
            return dr;
        }

		public static int ExecuteSQL(string SQLStatement, string ConnectionString)
		{
			SqlCommand cmd = new SqlCommand();
			int intRows;

			try
			{
				// Create a Connection object
				cmd.Connection = new SqlConnection(ConnectionString);

				// Fill in command text, set type
				cmd.CommandText = SQLStatement;
				cmd.CommandType = CommandType.Text;

				// Open the Connection
				cmd.Connection.Open();

				// Execute SQL
				intRows = cmd.ExecuteNonQuery();
			}
			catch(SqlException e)
			{
				throw e;
			}
			finally
			{
				if (cmd.Connection.State == ConnectionState.Open)
				{
					cmd.Connection.Close();
				}
			}

			return intRows;
		}

		public static SqlParameter GetInputParameter(string name,object val,SqlDbType type)
		{
			SqlParameter parm = new SqlParameter(name,type);
			parm.Direction = ParameterDirection.Input;
			parm.Value = val;
			return parm;
		}
		public static SqlParameter GetOutputParameter(string name,SqlDbType type)
		{
			SqlParameter parm = new SqlParameter(name,type);
			parm.Direction = ParameterDirection.Output;
			return parm;
		}

		public static int ExecuteSQL(SqlCommand cmd, string ConnectionString)
		{
			int intRows;

			try
			{
				// Create a Connection object
				cmd.Connection = new SqlConnection(ConnectionString);
				cmd.Connection.Open();

				// Execute SQL
				intRows = cmd.ExecuteNonQuery();
			}
			catch(SqlException e)
			{
				throw e;
			}
			finally
			{
				if (cmd.Connection.State == ConnectionState.Open)
				{
					cmd.Connection.Close();
				}
			}

			return intRows;
		}

		public static string QuoteConvert(string strValue)
		{
			return strValue.Replace("'", "''");
		}

		public static string Str2Field(string strValue)
		{
			if (strValue == string.Empty)
			{
				return "Null";
			}
			else
			{
				return "'" + QuoteConvert(strValue) + "'";
			}
		}
        public static bool IsNumeric(string strAmount)
        {
            decimal amt;
            bool isNumeric = true;
            try
            {
                amt = decimal.Parse(strAmount);
            }
            catch
            {
                isNumeric = false;
            }
            return isNumeric;
        }

	}
}
