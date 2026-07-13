using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;


public class CheckDBUtil
{
	public CheckDBUtil()
	{
	}
	public SqlParameter GetInputParameter(string name,object val,SqlDbType type)
	{
		SqlParameter parm = new SqlParameter(name,type);
		parm.Direction = ParameterDirection.Input;
		parm.Value = val;
		return parm;
	}
	public SqlParameter GetOutputParameter(string name,SqlDbType type)
	{
		SqlParameter parm = new SqlParameter(name,type);
		parm.Direction = ParameterDirection.Output;
		return parm;
	}
}

