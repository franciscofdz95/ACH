using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;


public class CheckUtil
{
	private CheckDBUtil utility = new CheckDBUtil();

	public CheckUtil()
	{
	}
	public int GetUpdateStatusID(string merchantID, string type,int status)
	{
		int retVal = 0;

		if(IsTestMerchant(int.Parse(merchantID)))
		{
			retVal = 3;
		}
		else
		{
			switch(type)
			{
				case "VO":
				switch(status)
				{
					case 0:
						retVal = 121;
						break;
					case 1:
						retVal = 101;
						break;
					case 2:
						retVal = 123;
						break;
				}
					break;
				case "GO":
				switch(status)
				{
					case 0:
						retVal = 122;
						break;
					case 1:
						retVal = 102;
						break;
					case 2:
						retVal = 124;
						break;
				}
					break;
				case "VA":
				case "VR":
				switch(status)
				{
					case 0:
						retVal = 221;
						break;
					case 1:
						retVal = 202;
						break;
					case 2:
						retVal = 223;
						break;
				}
					break;
				case "GA":
				case "GR":
				switch(status)
				{
					case 0:
						retVal = 321;
						break;
					case 1:
						retVal = 302;
						break;
					case 2:
						retVal = 323;
						break;
				}
					break;
			}
		}
		return retVal;
	}
	public int GetNewStatusID(string type)
	{
		int retVal = 0;
		switch(type)
		{
			case "VO":
			case "GO":
				retVal = 100;
				break;
			case "VA":
			case "VR":
				retVal = 200;
				break;
			case "GA":
			case "GR":
				retVal = 300;
				break;
		}
		return retVal;
	}
	public string GetBillingCycle(BillingCycles type)
	{
		switch(type)
		{
			case BillingCycles.Monthly:
				return "M";
			case BillingCycles.Quarterly:
				return "Q";
			case BillingCycles.Yearly:
				return "Y";
			case BillingCycles.Weekly:
				return "W";
			case BillingCycles.BiWeekly:
				return "B";
			case BillingCycles.Every4weeks:
				return "D";
			case BillingCycles.Every8weeks:
				return "H";
			default :
				return "M";
		}
	}
	private bool IsTestMerchant(int merchantID)
	{
		bool isTestMerchant = false;

		using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["CentralDB"]))
		{
			SqlCommand cmd = new SqlCommand("GetMerchantInfo", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(utility.GetInputParameter("@merchantID", merchantID, SqlDbType.Int));
			cmd.Connection.Open();

			using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
			{
				if(dr.Read())
				{
					if(dr["Test"].ToString().ToUpper() == "Y")
					{
						isTestMerchant = true;
					}
				}
			}
		}

		return isTestMerchant;
	}
}
