using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using nmc.crypto;

public class DataLayer
{
    public const string Empty_Date_Time = "  /  /       :  :";
    public const int m_CommandTimeout = 300;

	public DataLayer()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    //public static string ConnectStringBuild(string LoginID,string Password)
    //{
    //    string strConn;

    //    if (LoginID == string.Empty)
    //    {
    //        LoginID = "sa";
    //    }
												
    //    strConn = string.Format("Server=(local);Database=AchCentral;User ID={0};Password={1}", LoginID, Password);
    //    return strConn;
    //}

    public static string ConnectStringBuild()
    {
        Crypto crypto = new Crypto();
        string conn;
        conn = "Data Source=" +  ConfigurationManager.AppSettings["AchDB_Server"].ToString() + ";";
        conn += "Initial Catalog=" +  ConfigurationManager.AppSettings["AchDB_Database"].ToString() + ";";
        conn += "User ID=" +  crypto.Decrypt(ConfigurationManager.AppSettings["AchDB_UserID"].ToString()) + ";";
        conn += "password=" + crypto.Decrypt(ConfigurationManager.AppSettings["AchDB_Password"].ToString()) + ";";

        return conn;
    }

    public static string ConnectStringMainBuild()
    {
        Crypto crypto = new Crypto();
        string conn;
        conn = "Data Source=" + ConfigurationManager.AppSettings["CenturionDB_Server"].ToString() + ";";
        conn += "Initial Catalog=" + ConfigurationManager.AppSettings["CenturionDB_Database"].ToString() + ";";
        conn += "User ID=" + crypto.Decrypt(ConfigurationManager.AppSettings["CenturionDB_UserID"].ToString()) + ";";
        conn += "password=" + crypto.Decrypt(ConfigurationManager.AppSettings["CenturionDB_Password"].ToString()) + ";";

        return conn;
    }

    public static SqlCommand CreateCommand(string strSP)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = strSP;
        cmd.CommandType = CommandType.StoredProcedure;

        return cmd;
    }

    public static void AppendParamters(SqlCommand cmd, ArrayList prms)
    {
        for(int i = 0;i < prms.Count;i++)
        {
            cmd.Parameters.Add((SqlParameter) prms[i]);
        }
    }

    public static string ParamtersToString(SqlCommand cmd, ArrayList prms)
    {
        string strValue = string.Empty;
        SqlParameter prm = null;

        for (int i = 0; i < prms.Count; i++)
        {
            prm = (SqlParameter)prms[i];
            strValue += prm.ParameterName; 
        }
        return strValue;
    }

	public static System.Data.DataSet GetDataSet (string SQL, string ConnectionString)
	{     
		SqlDataAdapter da; 
		System.Data.DataSet ds;

		try
		{
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = m_CommandTimeout;
            cmd.Connection = new SqlConnection(ConnectionString);
            cmd.Connection.Open();
            cmd.CommandText = SQL;
            da = new SqlDataAdapter(cmd);
			ds = new System.Data.DataSet();
			da.Fill(ds);
		}
		catch(SqlException e)
		{
			throw e;
		}
		return ds;
	}

    public static System.Data.DataSet GetDataSetPaging(string SQL, string ConnectionString,int StartRecord,int MaxRecord)
    {
        SqlDataAdapter da;
        System.Data.DataSet ds;

        try
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = m_CommandTimeout;
            cmd.Connection = new SqlConnection(ConnectionString);
            cmd.Connection.Open();
            cmd.CommandText = SQL;
            da = new SqlDataAdapter(cmd);
            ds = new System.Data.DataSet();
            da.Fill(ds, StartRecord,MaxRecord,"Search");
        }
        catch (SqlException e)
        {
            throw e;
        }
        return ds;
    }

    public static System.Data.DataSet GetDataSetPaging(SqlCommand cmd, string ConnectionString, int StartRecord, int MaxRecord)
    {
        SqlDataAdapter da;
        System.Data.DataSet ds;

        try
        {
            //Create New DataAdapter
            da = new SqlDataAdapter();
            cmd.Connection = new SqlConnection(ConnectionString);
            cmd.CommandTimeout = m_CommandTimeout; 
            cmd.Connection.Open();
            da.SelectCommand = cmd;

            //Fill DataSet from DataAdapter
            ds = new System.Data.DataSet();
            da.Fill(ds, StartRecord, MaxRecord, "Search");
        }
        catch (SqlException e)
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
            cmd.CommandTimeout = m_CommandTimeout; 
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
            cmd.CommandTimeout = m_CommandTimeout; 
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
            cmd.CommandTimeout = m_CommandTimeout; 
			cmd.Connection = new SqlConnection (ConnectionString);
			cmd.Connection.Open();
			dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
		}
		catch(SqlException e)
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
			cmd.Connection = new SqlConnection(ConnectionString);
            cmd.CommandTimeout = m_CommandTimeout; 
			cmd.CommandText = SQLStatement;
			cmd.CommandType = CommandType.Text;
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
		int intRows = 0;

		try
		{
			// Create a Connection object
			cmd.Connection = new SqlConnection(ConnectionString);
            cmd.CommandTimeout = m_CommandTimeout;
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

    public static string ExecuteScalar(SqlCommand cmd, string ConnectionString)
    {
        string strReturnValue = string.Empty;
        try
        {
            // Create a Connection object
            cmd.Connection = new SqlConnection(ConnectionString);
            cmd.CommandTimeout = m_CommandTimeout;
            cmd.Connection.Open();

            // Execute SQL

            strReturnValue = cmd.ExecuteScalar().ToString();
        }
        catch (SqlException e)
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

        return strReturnValue;
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

    public static int Field2Int(object obj)
    {
        if (obj == DBNull.Value)
            return 0;
        else
            return Convert.ToInt32(obj);
    }

    public static bool Field2Bool(object obj)
    {
        if (obj == DBNull.Value)
            return false;
        else
            return Convert.ToBoolean(obj);
    }

   

    public static long Field2Long(object obj)
    {
        if (obj == DBNull.Value)
            return 0;
        else
            return Convert.ToInt64(obj);
    }

    public static string Field2Str(object obj)
    {
        if (obj == DBNull.Value)
            return string.Empty;
        else
            return obj.ToString();
    }

    public static double Field2Dbl(object obj)
    {
        if (obj == DBNull.Value)
            return 0;
        else
            return Convert.ToDouble (obj);
    }

    public static decimal  Field2Dec(object obj)
    {
        if (obj == DBNull.Value)
            return 0;
        else
            return Convert.ToDecimal (obj);
    }

    public static DateTime Field2Date(object obj)
    {
        if (obj == DBNull.Value)
            return DateTime.Today;
        else
            return Convert.ToDateTime (obj);
    }

    public static object Int2Field(object obj)
    {
        if (obj.ToString() == string.Empty )
            return DBNull.Value;
        else
            return Convert.ToInt32(obj);
    }

    public static object Date2Field(object obj)
    {
        if (obj.ToString() == Empty_Date_Time)
            return DBNull.Value;
        else
            return Convert.ToDateTime(obj);
    }

    public static byte Bool2Field(object obj)
    {
        if (Convert.ToBoolean(obj))
            return 1;
        else
            return 0;
    }


    public static decimal Decimal2Field(object obj)
    {
        if (obj.ToString() == string.Empty )
            return 0;
        else
            return Convert.ToDecimal (obj);
    }

    public static string  FilePadAmount(decimal decAmount, Int32 intLen)
    {
        string strAmount = string.Empty;
        strAmount = decAmount.ToString("######0.00").Replace(".", "");
        strAmount = strAmount.PadLeft(intLen, '0');
        strAmount = strAmount.Substring(strAmount.Length - intLen, intLen);

        return strAmount;
    }

    public static string FilePadNumber(long lngNumber, Int32 intLen)
    {
        string strNumber = string.Empty;
        strNumber = lngNumber.ToString().PadLeft(intLen, '0');
        strNumber = strNumber.Substring(strNumber.Length - intLen, intLen);

        return strNumber;
    }

    public static string FilePadString(string strValue, Int32 intLen)
    {
        string str = string.Empty;
        str = strValue.PadRight(intLen, ' ');
        str = str.Substring(0,intLen);

        return Convert.ToString(str);
    }

    public static bool IsDate(string strDate)
    {
        DateTime dt;
        bool isDate = true;
        try
        {
            dt = DateTime.Parse(strDate);
        }
        catch
        {
            isDate = false;
        }
        return isDate;
    }

    public static bool IsNumeric(string  strAmount)
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

