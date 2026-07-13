using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Linq;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Web;
using PaymentXP.BusinessObjects.Reserve;

public class DataMerchantAppPaging
{
    private static string keyCount = "{0}_Count";
    private static string keyTotal = "{0}_Total";

    public DataMerchantAppPaging()
    { }

    #region Document Paging

    /* Used by Merchant and Agent Document Library */

    //Merchant Documents - getting result set
    public static DataTable GetDocumentPaging(Hashtable prms, int pageSize, int currentPage)
    {
        string key = "GetDocumentPaging" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetDocumentPagingCore(prms, pageSize, currentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    //Merchant Documents - getting count
    public static int GetDocumentPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetDocumentPaging" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetDocumentPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    //Merchant Documents - Core function to hit data layer once
    private static void GetDocumentPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count)
    {
        int totalRecordCount = 0;
        if (!HttpContext.Current.Items.Contains(key))
        {
            List<MDoc> li = DataDocuments.GetInstance().GetDocuments_Paging(prms, out totalRecordCount);
            var table = CommonUtility.Util.ToDataTable(li);
            HttpContext.Current.Items.Add(key, table);
            HttpContext.Current.Items.Add(key_count, totalRecordCount);
        }
    }

    #endregion

    #region Merchant MRule Paging

    //Merchant MRule - getting result set
    public static List<MRule> GetMRuleMerchantPaging(Hashtable prms, int pageSize, int currentPage)
    {
        string key = "GetMRuleMerchantPaging" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMRuleMerchantPagingCore(prms, pageSize, currentPage, key, key_count);

        return (List<MRule>)HttpContext.Current.Items[key];
    }

    //Merchant MRule - getting count
    public static int GetMRuleMerchantPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetMRuleMerchantPaging" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMRuleMerchantPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    //Merchant Documents - Core function to hit data layer once
    private static void GetMRuleMerchantPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count)
    {
        int totalRecordCount = 0;
        if (!HttpContext.Current.Items.Contains(key))
        {
            List<MRule> li = DataMRule.SearchMRuleMerchantPaging(prms, out totalRecordCount);
            HttpContext.Current.Items.Add(key, li);
            HttpContext.Current.Items.Add(key_count, totalRecordCount);
        }
    }

    #endregion

    #region Multilink Paging

    //Merchant Multilink data - getting result set
    public static DataTable GetMultiLinkPaging(Hashtable prms, int pageSize, int currentPage, string merchantMID = null, int achID = 0)
    {
        string key = "GetMultiLinkPaging" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMultiLinkPagingCore(prms, pageSize, currentPage, key, key_count, merchantMID, achID);

        return (DataTable)HttpContext.Current.Items[key];
    }

    //Merchant Documents - getting count
    public static int GetMultiLinkPagingCount(Hashtable prms, int PageSize, int CurrentPage, string merchantMID = null, int achID = 0)
    {
        string key = "GetMultiLinkPaging" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMultiLinkPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    //Merchant Documents - Core function to hit data layer once
    private static void GetMultiLinkPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count, string merchantMID = null, int achID = 0)
    {
        if (!HttpContext.Current.Items.Contains(key))
        {
            if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "sp_SearchMultilinks_Paging";
                cmd.CommandType = CommandType.StoredProcedure;
                DataLayer.AppendParamters(cmd, prms);

                DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                ds.Tables[0].Columns.Add("YTDVolume");

                int _totalRecordCount = 0;

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                        }
                    }

                    DataRowCollection drc = ds.Tables[0].Rows;
                    foreach (DataRow dr in drc)
                    {
                        Hashtable prmsYTDVolume = new Hashtable();
                        prmsYTDVolume.Add("@Date", DateTime.Now.AddDays(-1).Date);
                        prmsYTDVolume.Add("@MID", dr["MID"]);

                        if (dr["AchID"] != null && !string.IsNullOrEmpty(Convert.ToString(dr["AchID"])))
                        {
                            if (Convert.ToInt32(dr["AchID"]) > 1)
                            {
                                prmsYTDVolume.Add("@AchID", dr["AchID"]);
                            }
                        }

                        // TOL REMOVE: comment out these lines if you're running on test.

                        DataTable dtBatchSummary = DataCCBatchDeposits.GetBatchDepositSummary(prmsYTDVolume);

                        decimal YTDVolume = 0;
                        DataRowCollection drcBatchSummary = dtBatchSummary.Rows;
                        foreach (DataRow drBatchSummary in drcBatchSummary)
                        {
                            YTDVolume = YTDVolume + Convert.ToDecimal(drBatchSummary["YTDAmount"]);
                        }

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i]["MID"].Equals(dr["MID"]))
                            {
                                ds.Tables[0].Rows[i]["YTDVolume"] = YTDVolume;
                            }
                        }
                    }
                }

                HttpContext.Current.Items.Add(key, ds.Tables[0]);
                HttpContext.Current.Items.Add(key_count, _totalRecordCount);
            }
        }
    }

    #endregion

    public static int GetCommunicationsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetCommunicationsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetCommunicationsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetCommunicationsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetCommunicationsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetCommunicationsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetCommunicationsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchCommunications_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringTransDB());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetEmailBlasterPagingRowCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchEmailBlaster_PagingCount";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        string rows = DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild());

        return Convert.ToInt32(rows);
    }

    public static DataTable GetEmailBlasterPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchEmailBlaster_Paging";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        return ds.Tables[0];
    }

    public static int GetETFMerchantsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchETFMerchants_PagingCount";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        string rows = DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild());

        return Convert.ToInt32(rows);
    }

    public static DataTable GetETFMerchantsPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchETFMerchants_Paging";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        return ds.Tables[0];
    }

    public static int GetMerchantPCIPagingRowCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchMerchantPCI_PagingCount";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        string rows = DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild());

        return Convert.ToInt32(rows);

    }

    public static DataTable GetMerchantPCIPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchMerchantPCI_Paging";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        return ds.Tables[0];
    }

    /* GetMerchantAppsOnline: Paging */
    public static DataTable GetMerchantAppsOnlinePaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantAppsOnlinePaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantAppsOnlineCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* GetMerchantAppsOnline: Paging Count */
    public static int GetMerchantAppsOnlinePagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantAppsOnlinePaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantAppsOnlineCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* GetMerchantAppsOnline: Core */
    public static void GetMerchantAppsOnlineCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchAppsPaging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    /* Application Queue: Paging */
    public static DataTable GetQueuesPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetQueuesPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetQueuesCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* Application Queue: Count */
    public static int GetQueuesPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetQueuesPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetQueuesCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Application Queue: Core */
    public static void GetQueuesCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMerchantAppByStatusPaging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    /* Leads: Paging */
    public static DataTable GetLeadsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetLeadsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetLeadsPagingCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* Leads: Count */
    public static int GetLeadsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetLeadsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetLeadsPagingCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Leads: Core */
    public static void GetLeadsPagingCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "sp_SearchLeads_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    /* MerchantApps: Paging Count */
    public static int GetMerchantAppsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantAppsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantAppsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* MerchantApps: Paging */
    public static DataTable GetMerchantAppsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantAppsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantAppsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* MerchantApps: Core */
    public static void GetMerchantAppsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchMerchantApps_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    /* Recent Tickets: Paging */
    public static DataTable GetRecentTicketsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetRecentTicketsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRecentTicketsCore(prms, PageSize, CurrentPage, hashval, key, key_count);



        return (DataTable)HttpContext.Current.Items[key];
    }


    /* Recent Tickets: Count */
    public static int GetRecentTicketsPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetRecentTicketsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRecentTicketsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Recent Tickets: Core */
    public static void GetRecentTicketsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchRecentTickets_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    //PXP-7674 abarua
    /* CRM: Paging */
    public static DataTable GetCRMPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetCRMPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetCRMCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* CRM: Count */
    public static int GetCRMPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetCRMPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetCRMCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* CRM: Core */
    public static void GetCRMCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchCRM_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }



    /* Tickets: Paging */
    public static DataTable GetTicketsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetTicketsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetTicketsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* Tickets: Count */
    public static int GetTicketsPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetTicketsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetTicketsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Tickets: Core */
    public static void GetTicketsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchTickets_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    /* Agents: Paging */
    public static DataTable GetAgentsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetAgentsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetAgentsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* Agents: Count */
    public static int GetAgentsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetAgentsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetAgentsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Agents: Core */
    public static void GetAgentsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchAgents_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    //public static int GetAgentsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    //{
    //    string key = "GetAgentsPagingRowCount_" + ControlID;

    //    if (HttpContext.Current.Items[key] == null)
    //    {
    //        SqlCommand cmd = new SqlCommand();
    //        cmd.CommandText = "sp_SearchAgents_PagingCount";
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        DataLayer.AppendParamters(cmd, prms);

    //        string rows = DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild());

    //        HttpContext.Current.Items[key] = CommonUtility.Util.if_i(rows, 0);
    //    }

    //    return (int)HttpContext.Current.Items[key];

    //}

    //public static DataTable GetAgentsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    //{
    //    string key = "GetAgentsPaging_" + ControlID;

    //    if (HttpContext.Current.Items[key] == null)
    //    {
    //        SqlCommand cmd = new SqlCommand();
    //        cmd.CommandText = "sp_SearchAgents_Paging";
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        DataLayer.AppendParamters(cmd, prms);

    //        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
    //        HttpContext.Current.Items[key] = ds.Tables[0];
    //    }

    //    return (DataTable)HttpContext.Current.Items[key];
    //}

    public static DataTable SelectMeritusAlerts_Paging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectMeritusAlerts_Paging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMeritusAlerts_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
            HttpContext.Current.Items[key] = ds.Tables[0];
        }

        return (DataTable)HttpContext.Current.Items[key];
    }

    public static int SelectMeritusAlerts_PagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectMeritusAlerts_PagingCount_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMeritusAlerts_PagingCount";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int count = Convert.ToInt32(DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild()));
            HttpContext.Current.Items[key] = count;
        }

        return (int)HttpContext.Current.Items[key];
    }

    public static DataTable SelectMeritusNews_Paging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectMeritusNews_Paging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMeritusNews_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
            HttpContext.Current.Items[key] = ds.Tables[0];
        }

        return (DataTable)HttpContext.Current.Items[key];
    }

    public static int SelectMeritusNews_PagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectMeritusNews_PagingCount_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMeritusNews_PagingCount";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int count = Convert.ToInt32(DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild()));
            HttpContext.Current.Items[key] = count;
        }

        return (int)HttpContext.Current.Items[key];
    }

    //ACH Transactions - getting result set    
    public static DataView GetACHTransactionsPaging(Hashtable prms, int pageSize, int currentPage, string ControlID)
    {
        if (prms == null)
            return null;

        string key = "GetACHTransactionsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);
        string key_Total = String.Format(keyTotal, key);

        GetACHTransactionsPagingCore(prms, pageSize, currentPage, key, key_count, key_Total);

        return (DataView)HttpContext.Current.Items[key];
    }

    //ACH Transactions - getting count
    public static int GetACHTransactionsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetACHTransactionsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);
        string key_Total = String.Format(keyTotal, key);

        GetACHTransactionsPagingCore(prms, PageSize, CurrentPage, key, key_count, key_Total);

        return (int)HttpContext.Current.Items[key_count];
    }

    //ACH Transactions - getting total maount
    public static decimal GetACHTransactionsPagingRowTotal(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetACHTransactionsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);
        string key_Total = String.Format(keyTotal, key);

        GetACHTransactionsPagingCore(prms, PageSize, CurrentPage, key, key_count, key_Total);

        return (decimal)HttpContext.Current.Items[key_Total];

    }

    //ACH Transactions Paging - Core function to hit data layer once
    private static void GetACHTransactionsPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count, string key_total)
    {
        int totalRecordCount = 0;
        decimal totalAmount = 0;
        if (!HttpContext.Current.Items.Contains(key))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Transactions_Paging";
            cmd.CommandType = CommandType.StoredProcedure;

            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringACHBuild());

            DataView dv = new DataView();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dv = ds.Tables[0].DefaultView;
                totalRecordCount = (int)ds.Tables[0].Rows[0]["TotalRecordCount"];
                totalAmount = (decimal)ds.Tables[0].Rows[0]["TotalAmount"];

            }

            HttpContext.Current.Items.Add(key, dv);
            HttpContext.Current.Items.Add(key_count, totalRecordCount);
            HttpContext.Current.Items.Add(key_total, totalAmount);

        }
    }

    //public static List<UWConditionDocument> GetUWConditionDocument_Paging(Hashtable prms, int pageSize, int currentPage)
    //{

    //    if (!prms.ContainsKey("@PageSize"))
    //    {
    //        prms.Add("@PageSize", pageSize);
    //    }

    //    if (!prms.ContainsKey("@CurrentPage"))
    //    {
    //        prms.Add("@CurrentPage", currentPage);
    //    }

    //    string key = "GetUWConditionDocument_Paging" + CommonUtility.Security.GenerateMD5ForObject(prms);

    //    if (!HttpContext.Current.Items.Contains(key))
    //    {
    //        List<UWConditionDocument> li = DataConditions.GetUWConditionDocument_Paging(prms);
    //        HttpContext.Current.Items.Add(key, li);
    //    }

    //    return (List<UWConditionDocument>)HttpContext.Current.Items[key];
    //}

    //public static int GetUWConditionDocument_PagingCount(Hashtable prms, int PageSize, int CurrentPage)
    //{
    //    string key = "GetUWConditionDocument_PagingCount" + CommonUtility.Security.GenerateMD5ForObject(prms);

    //    if (!HttpContext.Current.Items.Contains(key))
    //    {
    //        int rowcount = DataConditions.GetUWConditionDocument_PagingCount(prms);
    //        HttpContext.Current.Items.Add(key, rowcount);
    //    }

    //    return (int)HttpContext.Current.Items[key];
    //}

    // were gonna put this here temporarily until we can find a home for it.


    //UWCondition Documents - getting count

    public static int GetUWConditionDocument_PagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetUWConditionDocumentPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetUWConditionDocumentPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    //UWCondition Documents - getting documents
    public static DataTable GetUWConditionDocument_Paging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetUWConditionDocumentPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetUWConditionDocumentPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    //UWCondition Documents Paging - Core function to hit data layer once
    private static void GetUWConditionDocumentPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchUWConditionDocument_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }

    }

    public static DataTable GetReportRMOverview(Hashtable prms)
    {
        string key = "GetReportRMOverview" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (!HttpContext.Current.Items.Contains(key))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReportRMOverview";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);
            DataTable dt = DataLayer.GetDataTable(cmd, DataLayer.ConnectStringBuild());
            HttpContext.Current.Items.Add(key, dt);
        }

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* recent merchant notes */
    public static int GetRecentMerchantNotesPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantNotesPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRecentMerchantNotesCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static DataTable GetRecentMerchantNotesPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantNotesPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRecentMerchantNotesCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];

    }

    public static void GetRecentMerchantNotesCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchRecentMerchantNotes_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    /* merchant notes */
    public static int GetMerchantNotesPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantNotesPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantNotesCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static DataTable GetMerchantNotesPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantNotesPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantNotesCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];

    }

    public static void GetMerchantNotesCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchMerchantNotes_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }
    //PXP-15332
    #region Merchant & MerchantNotes Objects
    public static DataSet GetMerchantNoteObjects(Hashtable prms)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_GetMerchantNoteObjects";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        if (ds != null)
        {
            return ds;
        }
        else
            return null;

    }
    #endregion
    public static DataTable GetWSCompliancePaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetWSCompliancePaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            HttpContext.Current.Items[key] = DataWSCompliance.SearchWSCompliancePaging(prms);
        }

        return (DataTable)HttpContext.Current.Items[key];
    }

    public static int GetWSCompliancePagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetWSCompliancePagingCount_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            HttpContext.Current.Items[key] = DataWSCompliance.SearchWSCompliancePagingCount(prms);
        }

        return (int)HttpContext.Current.Items[key];
    }

    #region Alert Contacts Paging

    public static DataView GetMerchantContactsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        if (prms == null)
            return null;

        string key = "GetMerchantContactsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMerchantContactsPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataView)HttpContext.Current.Items[key];
    }

    public static int GetMerchantContactsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {

        string key = "GetMerchantContactsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMerchantContactsPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    private static void GetMerchantContactsPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count)
    {
        int totalRecordCount = 0;

        if (!HttpContext.Current.Items.Contains(key))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_GetMerchantAlertContacts";
            cmd.CommandType = CommandType.StoredProcedure;

            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            DataView dv = new DataView();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dv = ds.Tables[0].DefaultView;
                totalRecordCount = (int)ds.Tables[0].Rows[0]["TotalRecordCount"];
            }

            HttpContext.Current.Items.Add(key, dv);
            HttpContext.Current.Items.Add(key_count, totalRecordCount);
        }
    }

    public static DataView GetAgentContactsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        if (prms == null)
            return null;

        string key = "GetAgentContactsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetAgentContactsPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataView)HttpContext.Current.Items[key];
    }

    public static int GetAgentContactsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {

        string key = "GetAgentContactsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetAgentContactsPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    private static void GetAgentContactsPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count)
    {
        int totalRecordCount = 0;

        if (!HttpContext.Current.Items.Contains(key))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_GetAgentContacts";
            cmd.CommandType = CommandType.StoredProcedure;

            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            DataView dv = new DataView();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dv = ds.Tables[0].DefaultView;
                totalRecordCount = (int)ds.Tables[0].Rows[0]["TotalRecordCount"];
            }

            HttpContext.Current.Items.Add(key, dv);
            HttpContext.Current.Items.Add(key_count, totalRecordCount);
        }
    }

    #endregion


    public static List<ChangeLogs> GetChangelogPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetChangelogPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            HttpContext.Current.Items[key] = DataChangeLogs.SearchChangeLogs_Paging(prms);
        }

        return (List<ChangeLogs>)HttpContext.Current.Items[key];
    }

    public static int GetChangelogPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetChangelogPagingCount_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        if (HttpContext.Current.Items[key] == null)
        {
            HttpContext.Current.Items[key] = DataChangeLogs.SearchChangeLogs_PagingCount(prms);
        }

        return (int)HttpContext.Current.Items[key];
    }

    public static int GetPotentialFTMerchantsCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetPotentialFTMerchantsCount";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetPotentialFTMerchantsCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static int GetMRuleRunLogPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetMRuleRunLogPagingCount";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMRuleRunLogCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static DataTable GetMRuleRunLogPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetMRuleRunLogPaging";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMRuleRunLogCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];

    }

    public static DataTable GetPotentialFTMerchants(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetPotentialFTMerchants";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetPotentialFTMerchantsCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];

    }

    public static void GetPotentialFTMerchantsCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_GetMerchantFTRepAssignment";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static void GetMRuleRunLogCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchMRuleRunLog_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    public static int GetFTRepViewPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetFTRepViewPaging";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetFTRepViewCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static DataTable GetFTRepViewPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetFTRepViewPaging";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetFTRepViewCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];

    }

    public static void GetFTRepViewCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_GetFTRepView";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetChangelogFTPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetChangelogFTPaging";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetChangelogFTCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static List<ChangeLogs> GetChangelogFTPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetChangelogFTPaging";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetChangelogFTCore(prms, PageSize, CurrentPage, key, key_count);

        return (List<ChangeLogs>)HttpContext.Current.Items[key];

    }

    public static void GetChangelogFTCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            List<ChangeLogs> li = DataChangeLogs.SearchChangeLogsFT_Paging(prms);

            int _totalRecordCount = 0;

            if (li != null && li.Count > 0)
            {
                _totalRecordCount = li[0].TotalRecordCount;
            }

            HttpContext.Current.Items.Add(key, li);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    /* AgentNotes: Paging */
    public static DataTable GetAgentNotesPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetAgentNotesPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetAgentNotesCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* AgentNotes: Count */
    public static int GetAgentNotesPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetAgentNotesPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetAgentNotesCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* AgentNotes: Core */
    public static void GetAgentNotesCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchAgentNotes_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    /* TicketSummary: Paging */
    public static DataTable GetTicketSummaryPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        if (prms != null && prms.Count > 0)
        {
            string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
            string key = "GetTicketSummaryPaging_" + hashval;
            string key_count = String.Format(keyCount, key);

            DataMerchantAppPaging.GetTicketSummaryCore(prms, PageSize, CurrentPage, hashval, key, key_count);

            return (DataTable)HttpContext.Current.Items[key];
        }
        else
        {
            return null;
        }
    }

    /* TicketSummary: Count */
    public static int GetTicketSummaryPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        if (prms != null && prms.Count > 0)
        {
            string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
            string key = "GetTicketSummaryPaging_" + hashval;
            string key_count = String.Format(keyCount, key);

            DataMerchantAppPaging.GetTicketSummaryCore(prms, PageSize, CurrentPage, hashval, key, key_count);

            return (int)HttpContext.Current.Items[key_count];
        }
        else
        {
            return 0;
        }
    }

    /* TicketSummary: Core */
    public static void GetTicketSummaryCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchTicketSummary_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }




    /* ReserveBalance: Paging */
    public static DataTable GetReserveBalancePaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetReserveBalancePaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetReserveBalanceCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* ReserveBalance: Count */
    public static int GetReserveBalancePagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetReserveBalancePaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetReserveBalanceCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* ReserveBalance: Core */
    public static void GetReserveBalanceCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_RDB_SelectBalance_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }




    /* ReserveSummary: Paging */
    public static List<RDBSummary> GetReserveSummaryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetReserveSummaryPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetReserveSummaryCore(prms, PageSize, CurrentPage, key, key_count);

        return (List<RDBSummary>)HttpContext.Current.Items[key];
    }

    /* ReserveSummary: Count */
    public static int GetReserveSummaryPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetReserveSummaryPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetReserveSummaryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* ReserveSummary: Core */
    public static void GetReserveSummaryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            List<RDBSummary> li = DataReserve.GetRDBSummary(prms);


            int _totalRecordCount = 0;

            if (li != null && li.Count > 0)
            {
                _totalRecordCount = li[0].TotalRecordCount;
            }

            HttpContext.Current.Items.Add(key, li);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static DataTable GetMerchantCardCurrency(Hashtable prms)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchMerchantCardCurrency";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringTransDB());

        return ds.Tables[0];
    }




    /* MerchantChain: Paging Count */
    public static int GetMerchantChainPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantChainPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantChainCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* MerchantChain: Paging */
    public static DataTable GetMerchantChainPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetMerchantChainPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantChainCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* MerchantChain: Core */
    public static void GetMerchantChainCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMerchantChain";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    /* BatchSummary_Risk: Paging Count */
    public static int GetRiskBatchSummaryPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetRiskBatchSummaryPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRiskBatchSummaryCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* BatchSummary_Risk: Paging */
    public static DataTable GetRiskBatchSummaryPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetRiskBatchSummaryPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRiskBatchSummaryCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* BatchSummary_Risk: Core */
    public static void GetRiskBatchSummaryCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            DataSet ds = DataAccess.DataRiskDao.GetBatchSummaryPaging(prms);

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    /* BatchDetails_Risk: Paging Count */
    public static int GetRiskBatchDetailsPagingRowCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetBatchDetailsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRiskBatchDetailsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* BatchDetails_Risk: Paging */
    public static DataTable GetRiskBatchDetailsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string key = "GetBatchDetailsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetRiskBatchDetailsCore(prms, PageSize, CurrentPage, ControlID, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* BatchDetails_Risk: Core */
    public static void GetRiskBatchDetailsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            DataSet ds = DataAccess.DataRiskDao.GetBatchDetailsPaging(prms);

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static DataTable GetMCProcessingMerchantsPaging(Hashtable prms, int pageSize, int currentPage)
    {
        string key = "GetMCProcessingMerchantsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMCProcessingMerchantsPagingCore(prms, pageSize, currentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    //Merchant Documents - getting count
    public static int GetMCProcessingMerchantsPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetMCProcessingMerchantsPaging_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        GetMCProcessingMerchantsPagingCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    //Merchant Documents - Core function to hit data layer once
    private static void GetMCProcessingMerchantsPagingCore(Hashtable prms, int pageSize, int currentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchMCPMerchants_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    //reteives the MID/Bank history for a given ZID
    public static DataTable GetMIDHistory(int zid)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchBankMIDHistory";
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ZID", zid);

        DataTable dt = DataLayer.GetDataTable(cmd, DataLayer.ConnectStringBuild());
        return dt;
    }

    public static int GetUserLeadAppointmentsCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetUserLeadAppointmentsCount";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetUserLeadAppointmentsCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    public static DataTable GetUserLeadAppointments(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "GetUserLeadAppointments";
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetUserLeadAppointmentsCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];

    }

    public static void GetUserLeadAppointmentsCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchUserLeadAppointments";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    //Retrieves Score cards for a merchant
    public static int GetMerchantScoreCardPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchMerchantScoreCards_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantScoreCardCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetMerchantScoreCardPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchMerchantScoreCards_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetMerchantScoreCardCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetMerchantScoreCardCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchMerchantScoreCards_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    //Retrieves Ticket Templates
    public static int GetTicketTemplatesPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchTicketTemplates_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetTicketTemplatesCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetTicketTemplatesPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchTicketTemplates_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetTicketTemplatesCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetTicketTemplatesCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchTicketTemplates_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }
    #region Quality Tab 

    #region Agent Allocation 
    /* Agent Allocation: Paging */
    public static DataTable GetAgentAllocationsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetAgentAllocationsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetAgentAllocationsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* Agent Allocation: Count */
    public static int GetAgentAllocationsPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetAgentAllocationsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetAgentAllocationsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Agent Allocation: Core */
    public static void GetAgentAllocationsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchAgentAllocations_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //_totalRecordCount = ds.Tables[0].Rows.Count;
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static MDoc getMDoc(DataRow dr)
    {
        //True means we only need to get one single row, because the property names on the datarow changed and need to be refactored. 
        var document = DataDocuments.GetInstance().FillMDocument(dr, true);
        return document;
    }

    #endregion Agent Allocation  
    #endregion Quality Tab 
    #region Quality Ticket Errors
    public static DataTable GetQATicketErrorsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetQATicketErrorsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetQATicketErrorsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* Ticket Errors: Count */
    public static int GetQATicketErrorsPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetQATicketErrorsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetQATicketErrorsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* Ticket Errors: Core */
    public static void GetQATicketErrorsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchQATicketErrors_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    #endregion
    #region "Quality App ERROR" // Added for PXP-17782 start
    /* QAAppErrors: Paging */
    public static DataTable GetQaAppErrorsPaging(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetQaAppErrorsPaging" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetQaAppErrorsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    /* QAAppError: Count */
    public static int GetQaAppErrorsPagingCount(Hashtable prms, int PageSize, int CurrentPage, string ControlID)
    {
        string hashval = CommonUtility.Security.GenerateMD5ForObject(prms);
        string key = "GetQaAppErrorsPaging_" + hashval;
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetQaAppErrorsCore(prms, PageSize, CurrentPage, hashval, key, key_count);

        return (int)HttpContext.Current.Items[key_count];
    }

    /* QAAppError: Core */
    public static void GetQaAppErrorsCore(Hashtable prms, int PageSize, int CurrentPage, string ControlID, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchQAAppErrors_Paging";

            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    #endregion //added for PXP-17782 End

    #region Merchant History //DM-769 alamadrid
    public static int GetChangeHistoryPagingCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetChangeHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetChangeHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectChangeHistory_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                        {
                            int value = Convert.ToInt32(ds.Tables[i].Rows[j]["ChangeHistoryFieldID"]);

                            if (value == 42)
                            {
                                if (UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
                                {
                                    ds.Tables[i].Rows[j]["ChangeHistoryField"] = "Tangible Trial";
                                }
                                else
                                {
                                    ds.Tables[i].Rows[j]["ChangeHistoryField"] = "Nutra Trial";
                                }
                            }
                        }

                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetSearchStatusHistoryCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchStatusHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSearchStatusHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetSearchStatusHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchStatusHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSearchStatusHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetSearchStatusHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchStatusHistory_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetACHSearchStatusHistoryCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "ACHSearchStatusHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetACHSearchStatusHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetACHSearchStatusHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "ACHSearchStatusHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetACHSearchStatusHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetACHSearchStatusHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchStatusHistory_Paging";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringACHBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetSearchDocumentsChangeHistoryCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchDocumentsChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSearchDocumentsChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetSearchDocumentsChangeHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SearchDocumentsChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSearchDocumentsChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetSearchDocumentsChangeHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchDocumentsChangeHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetSelectRelationshipsChangeHistoryCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectRelationshipsChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSelectRelationshipsChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetSelectRelationshipsChangeHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectRelationshipsChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSelectRelationshipsChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetSelectRelationshipsChangeHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectRelationshipsChangeHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetSelectEquipmentsChangeHistoryCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectEquipmentsChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSelectEquipmentsChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetSelectEquipmentsChangeHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectEquipmentsChangeHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetSelectEquipmentsChangeHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetSelectEquipmentsChangeHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectEquipmentsChangeHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }

    public static int GetProductRuleHistoryCount(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectProductRuleHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);

        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetProductRuleHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (int)HttpContext.Current.Items[key_count];

    }

    public static DataTable GetProductRuleHistoryPaging(Hashtable prms, int PageSize, int CurrentPage)
    {
        string key = "SelectProductRuleHistory_" + CommonUtility.Security.GenerateMD5ForObject(prms);
        string key_count = String.Format(keyCount, key);

        DataMerchantAppPaging.GetProductRuleHistoryCore(prms, PageSize, CurrentPage, key, key_count);

        return (DataTable)HttpContext.Current.Items[key];
    }

    private static void GetProductRuleHistoryCore(Hashtable prms, int PageSize, int CurrentPage, string key, string key_count)
    {
        if (!HttpContext.Current.Items.Contains(key) || !HttpContext.Current.Items.Contains(key_count))
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_GetProductRuleHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            int _totalRecordCount = 0;

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _totalRecordCount = int.Parse(ds.Tables[0].Rows[0]["TotalRecordCount"].ToString());
                    }
                }
            }

            HttpContext.Current.Items.Add(key, ds.Tables[0]);
            HttpContext.Current.Items.Add(key_count, _totalRecordCount);
        }
    }


    #endregion Merchant History
}


