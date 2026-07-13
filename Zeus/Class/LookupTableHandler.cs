using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using Infragistics.Web.UI.NavigationControls;
using System.Linq;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Text;
using PaymentXP.BusinessObjects.Tickets;
using PaymentXP.BusinessObjects.CRM;
using PaymentXP.BusinessObjects.Zeus;
using PaymentXP.BusinessObjects.Vendor;

/// <summary>
/// Summary description for LookupTableHandler
/// </summary>

public static class LookupTableHandler
{

    #region Static Lists

    //public static IList<GenericListItem> m_Agents;
    public static List<GenericListItem> m_Banks;
    public static List<GenericListItem> m_NewBanks;
    public static IList<GenericListItem> m_Users;
    public static IList<GenericListItem> m_SalesSupport;
    public static List<GenericListItem> m_FrontEnds;
    public static List<GenericListItem> m_BackEnds;
    public static IList<GenericListItem> m_BusinessStructures;
    public static IList<GenericListItem> m_ReasonChanges;
    public static IList<GenericListItem> m_ReturnPolicies;
    public static IList<GenericListItem> m_ApplicationTypes;
    public static IList<GenericListItem> m_PrivateLabels;
    public static IList<GenericListItem> m_GatewayMode;
    public static IList<GenericListItem> m_CCStatus;
    public static IList<GenericListItem> m_AVSResults;
    public static IList<GenericListItem> m_CVV2Results;
    public static IList<GenericListItem> m_DeviceResults;
    public static IList<GenericListItem> m_IPGeoResults;
    public static IList<GenericListItem> m_PCIVEndors;
    public static List<GenericListItem> m_InvoiceCategories;
    public static DataSet m_TicketCategoriesDS;
    public static Dictionary<int, MerchantClass> m_MerchantClass;
    public static List<GenericListItem> m_Channels;
    public static List<GenericListItem> m_UserTimeZones;
    public static Dictionary<string, IList<User>> m_UsersByRole;
    public static List<GenericListItem> m_Offices;
    public static List<GenericListItem> m_AccountGroups;
    public static List<GenericListItem> m_LegalEntities;
    public static List<GenericListItem> m_CurrencyCodes;
    public static List<GenericListItem> m_PaymentSchedule;
    public static List<GenericListItem> m_PaymentFrequency;
    public static Dictionary<string, string> m_CountryCodes;
    public static List<UWHeirarchyApprovalLimit> m_UWHeirarchyApprovalLimit;
    public static List<TicketTemplate> m_TicketTemplates;
    public static IList<GenericListItem> m_AgentLevels;
    public static List<ChangeHistoryFields> m_ChangeHistoryFields;
    public static IList<GenericListItem> m_Vendors;
    //Niranjan PXP-8045
    public static IList<GenericListItem> m_DeployTypeID;
    public static Dictionary<string, IList<User>> m_RequestedByUsers;
    public static IList<GenericListItem> m_SchedueATypeListItems;
    //agents
    //public static DataTreeNode m_AgentNodes;


    //leads
    public static IList<Status> m_LeadStatus;
    public static IList<LeadSource> m_LeadSource;
    public static IList<GenericListItem> m_States;
    public static IList<FollowupStatus> m_FollowupStatus;
    public static IList<GenericListItem> m_TimeZones;
    public static IList<LeadServices> m_LeadServices;
    public static IDictionary<string, string> m_MDocGroupType;
    public static IList<User> m_ActiveUsers;
    public static List<LeadSourceRep> m_LeadReps;
    public static IList<GenericListItem> m_LeadOrigin;
    public static IList<GenericListItem> m_LeadClosureCodes;

    #endregion

    #region " RESERVE STATIC LISTS "

    public static IList<GenericListItem> m_RDBBank;
    public static IList<GenericListItem> m_RDBDiversionReason;
    public static IList<GenericListItem> m_RDBDiversionResolution;
    public static IList<GenericListItem> m_RDBDiversionType;
    public static IList<GenericListItem> m_RDBEntryType;
    public static IList<GenericListItem> m_RDBTransactionType;
    public static IList<GenericListItem> m_RDBReserveSource;
    public static IList<GenericListItem> m_RDBReserveType;
    public static IList<GenericListItem> m_RDBManualCollection;
    //public static IList<GenericListItem> m_RDBTransactionMethod;

    public static IList<GenericListItem> RDBBank
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBBank == null)
            {
                m_RDBBank = DataReserve.GetRDBBank();
            }

            return m_RDBBank;
        }

    }

    public static void LoadRDBBank(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Banks", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBBank)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }

    public static void LoadRDBBank(ListControl lst, bool IsSearchForm, int ZID)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Banks", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        IList<GenericListItem> li = DataReserve.GetRDBBank(ZID);

        foreach (GenericListItem item in li)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }


    public static IList<GenericListItem> RDBDiversionReason
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBDiversionReason == null)
            {
                m_RDBDiversionReason = DataReserve.GetRDBDiversionReason();
            }

            return m_RDBDiversionReason;
        }

    }

    public static void LoadRDBDiversionReason(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Diversion Reasons", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBDiversionReason)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }


    public static IList<GenericListItem> RDBDiversionResolution
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBDiversionResolution == null)
            {
                m_RDBDiversionResolution = DataReserve.GetRDBDiversionResolution();
            }

            return m_RDBDiversionResolution;
        }

    }

    public static void LoadRDBDiversionResolution(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Diversion Resolutions", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBDiversionResolution)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }


    public static IList<GenericListItem> RDBDiversionType
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBDiversionType == null)
            {
                m_RDBDiversionType = DataReserve.GetRDBDiversionType();
            }

            return m_RDBDiversionType;
        }

    }


    public static void LoadDiversionType(ListControl myList)
    {
        myList.Items.Clear();


        Array itemValues = System.Enum.GetValues(typeof(eRDBDiversionTypeID));
        Array itemNames = System.Enum.GetNames(typeof(eRDBDiversionTypeID));

        for (int i = 0; i <= itemNames.Length - 1; i++)
        {
            ListItem item = new ListItem();
            item.Text = CommonUtility.Util.GetEnumDescription((eRDBDiversionTypeID)Convert.ToInt32((itemValues as int[])[i]));
            item.Value = Convert.ToInt32((itemValues as int[])[i]).ToString();

            myList.Items.Add(item);
        }
    }


    public static eRDBBank GetBankByMid(string mid)
    {
        eRDBBank eB = eRDBBank.NotSet;

        if (mid.StartsWith("510159"))
        {
            eB = eRDBBank.Wells;
        }
        else if (mid.StartsWith("548298"))
        {
            eB = eRDBBank.Woodforest;
        }
        else if (mid.StartsWith("513484"))
        {
            eB = eRDBBank.Ncal;
        }

        return eB;
    }


    //public static IList<GenericListItem> RDBDivertCategory
    //{
    //    get
    //    {
    //        // always returns a list, possibly an empty one if nothing fetched.
    //        if (m_RDBDivertCategory == null)
    //        {
    //            m_RDBDivertCategory = DataReserve.GetRDBDivertCategory();
    //        }

    //        return m_RDBDivertCategory;
    //    }

    //}

    //public static void LoadRDBDivertCategory(ListControl lst, bool IsSearchForm)
    //{

    //    lst.Items.Clear();

    //    if (IsSearchForm)
    //        lst.Items.Add(new ListItem("All Divert Categories", "0"));
    //    else
    //        lst.Items.Add(new ListItem("Select", "-1"));

    //    foreach (GenericListItem item in RDBDivertCategory)
    //    {
    //        lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
    //    }

    //}


    //public static IList<GenericListItem> RDBDivertGroup
    //{
    //    get
    //    {
    //        // always returns a list, possibly an empty one if nothing fetched.
    //        if (m_RDBDivertGroup == null)
    //        {
    //            m_RDBDivertGroup = DataReserve.GetRDBDivertGroup();
    //        }

    //        return m_RDBDivertGroup;
    //    }

    //}

    //public static void LoadRDBDivertGroup(ListControl lst, bool IsSearchForm)
    //{

    //    lst.Items.Clear();

    //    if (IsSearchForm)
    //        lst.Items.Add(new ListItem("All Divert Groups", "0"));
    //    else
    //        lst.Items.Add(new ListItem("Select", "-1"));

    //    foreach (GenericListItem item in RDBDivertGroup)
    //    {
    //        lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
    //    }

    //}


    public static IList<GenericListItem> RDBEntryType
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBEntryType == null)
            {
                m_RDBEntryType = DataReserve.GetRDBEntryType();
            }

            return m_RDBEntryType;
        }

    }

    public static void LoadRDBEntryType(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Entry Types", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBEntryType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }


    public static IList<GenericListItem> RDBTransactionType
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBTransactionType == null)
            {
                m_RDBTransactionType = DataReserve.GetRDBTransactionType();
            }

            return m_RDBTransactionType;
        }

    }

    public static void LoadRDBTransactionType(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Release Types", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBTransactionType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }


    public static IList<GenericListItem> RDBReserveSource
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBReserveSource == null)
            {
                m_RDBReserveSource = DataReserve.GetRDBReserveSource();
            }

            return m_RDBReserveSource;
        }

    }   

    public static void LoadRDBReserveSource(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Descriptions", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBReserveSource)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }


    public static IList<GenericListItem> RDBReserveType
    {
        get
        {
            // always returns a list, possibly an empty one if nothing fetched.
            if (m_RDBReserveType == null)
            {
                m_RDBReserveType = DataReserve.GetRDBReserveType();
            }

            return m_RDBReserveType;
        }

    }

    public static void LoadRDBReserveType(ListControl lst, bool IsSearchForm)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Reserve Types", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in RDBReserveType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }

    //public static IList<GenericListItem> RDBTransactionMethod
    //{
    //    get
    //    {
    //        // always returns a list, possibly an empty one if nothing fetched.
    //        if (m_RDBTransactionMethod == null)
    //        {
    //            m_RDBTransactionMethod = DataReserve.GetRDBTransactionMethod();
    //        }

    //        return m_RDBTransactionMethod;
    //    }

    //}

    public static void LoadRDBTransactionMethod(ListControl lst, bool IsSearchForm, eRDBTransactionMethodType mt)
    {

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All Transaction Methods", "0"));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        IList<GenericListItem> myTM = DataReserve.GetRDBTransactionMethod(mt);

        foreach (GenericListItem item in myTM)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }

    #endregion

    //MERCHANT BOARDING STATIC LISTS 
    public static IList<GenericListItem> m_PricingType;
    public static IList<GenericListItem> m_ParticipantType;
    public static IList<GenericListItem> m_SecurityProtocol;
    public static IList<GenericListItem> m_TerminalActivationType;
    public static IList<GenericListItem> m_DepositType;
    public static IList<GenericListItem> m_AuthPOSType;
    public static IList<GenericListItem> m_MagneticStripeType;
    public static IList<GenericListItem> m_POSDeviceType;
    public static IList<GenericListItem> m_POSCapabilityType;
    public static IList<GenericListItem> m_DiscoverRefNo;
    public static IList<GenericListItem> m_MerchantType;
    public static IList<GenericListItem> m_AMEXPCIDType;
    public static IList<GenericListItem> m_BillingMethod;
    public static IList<GenericListItem> m_ChannelSalesManager;
    public static IList<GenericListItem> m_TicketApprovalManager;
    // To get every 4th application for any of the listed agentID
    public static int m_AgentIDSpecificAppCounter = 0;

    public static IList<GenericListItem> m_AgentSourceCodes;

    public static IList<GenericListItem> m_QAAppErrorOccurredStage;
    public static IList<GenericListItem> m_QAAppErrorFoundBy;
    public static IList<GenericListItem> m_QAAppDepartment;
    public static IList<GenericListItem> m_QAAppCategory;
    public static IList<GenericListItem> m_QAAppSubCategory;

    public static IList<GenericListItem> m_QATicketCategory;
    public static IList<GenericListItem> m_QATicketSubCategory;
    //Ani:DM-5686
    public static IList<GenericListItem> m_SelectedVendor;

    public static int GetAgentIDSpecificAppCounter(string agentID)
    {
        List<string> allowedAgentIDs = ConfigurationManager.AppSettings["AgentIdsFor3de"].Split(',').ToList();
        if (allowedAgentIDs.Contains(agentID))
            m_AgentIDSpecificAppCounter += 1;
        return m_AgentIDSpecificAppCounter;
    }
    public static List<GenericListItem> LoadPaymentFrequency(ListControl lst, bool SearchForm)
    {
        if (m_PaymentFrequency == null)
        {
            DataUnderwritng data = new DataUnderwritng();
            DataTable dt = data.GetPaymentFrequency();
            m_PaymentFrequency = new List<GenericListItem>();

            GenericListItem item = null;

            foreach (DataRow dr in dt.Rows)
            {
                item = new GenericListItem();
                item.ItemText = dr["PaymentFrequency"].ToString();
                item.ItemValue = dr["PaymentFrequencyID"].ToString();
                m_PaymentFrequency.Add(item);
            }

        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_PaymentFrequency)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

        return m_PaymentFrequency;
    }

    public static List<GenericListItem> LoadPaymentSchedule(ListControl lst, bool SearchForm)
    {
        if (m_PaymentSchedule == null)
        {
            m_PaymentSchedule = new List<GenericListItem>();
            DataUnderwritng data = new DataUnderwritng();
            DataTable dt = data.GetPaymentSchedule();
            GenericListItem item = null;
            foreach (DataRow dr in dt.Rows)
            {
                item = new GenericListItem();
                item.ItemText = dr["PaymentSchedule"].ToString();
                item.ItemValue = dr["PaymentScheduleID"].ToString();
                m_PaymentSchedule.Add(item);
            }

        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_PaymentSchedule)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

        return m_PaymentSchedule;
    }

    public static Dictionary<int, MerchantClass> GetMerchantClass()
    {
        if (m_MerchantClass == null)
        {
            Hashtable prms = new Hashtable();
            m_MerchantClass = new Dictionary<int, MerchantClass>();

            m_MerchantClass = DataAccess.DataMerchantAppDao.GetMerchantClass(prms);

        }

        return m_MerchantClass;
    }

    public static List<GenericListItem> LoadAccountGroups()
    {
        if (m_AccountGroups == null)
        {
            DataMerchantApp data = new DataMerchantApp();
            Hashtable prms = new Hashtable();
            prms.Add("@IsDisabled", false);
            DataTable dt = data.GetAccountGroups(prms);
            List<GenericListItem> lst = new List<GenericListItem>();
            GenericListItem item = null;
            foreach (DataRow dr in dt.Rows)
            {
                item = new GenericListItem();
                item.ItemText = dr["AccountGroup"].ToString();
                item.ItemValue = dr["AccountGroupID"].ToString();
                lst.Add(item);
            }

            m_AccountGroups = lst;
        }

        return m_AccountGroups;
    }

    public static void LoadMDocGroupTypes(ListControl lst, bool SearchForm)
    {
        if (m_MDocGroupType == null)
        {
            Hashtable prms = new Hashtable();
            m_MDocGroupType = new Dictionary<string, string>();

            Array itemValues = System.Enum.GetValues(typeof(MDoc.eMDocTypeGroup));
            string[] itemNames = System.Enum.GetNames(typeof(MDoc.eMDocTypeGroup));

            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                m_MDocGroupType.Add(itemNames[i], Convert.ToString((int)((MDoc.eMDocTypeGroup)itemValues.GetValue(i))));
            }

        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (string str in m_MDocGroupType.Keys)
        {
            if (m_MDocGroupType[str] != "0")
                lst.Items.Add(new ListItem(str, m_MDocGroupType[str]));
        }
    }

    public static void LoadLeadTypes(ListControl lst, bool SearchForm)
    {
        if (m_LeadServices == null)
        {
            Hashtable prms = new Hashtable();
            DataLeadServices data = new DataLeadServices();

            prms.Add("@Category", "4");
            m_LeadServices = data.GetLeadServices(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (LeadServices service in m_LeadServices)
        {
            lst.Items.Add(new ListItem(service.Name, service.LeadServiceID));
        }
    }

    //public static void LoadAgents(ListControl lst, bool SearchForm)
    //{
    //    Hashtable prms = new Hashtable();
    //    IList<GenericListItem> list = null;

    //    if (UserSessions.CurrentUser != null)
    //        if (UserSessions.CurrentUser.IsAgent)
    //            prms.Add("@AgentUID", UserSessions.CurrentUser.HookTableKeyUID);

    //    DataAgent data = DataAccess.DataAgentDao;
    //    list = data.GetAgents_Light(prms);

    //    lst.Items.Clear();

    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (GenericListItem agent in list)
    //    {
    //        lst.Items.Add(new ListItem(agent.ItemText, agent.ItemValue));
    //    }
    //}

    //public static void LoadAgentnSubAgents(ListControl lst, bool SearchForm, string AgentUID)
    //{
    //    Hashtable prms = new Hashtable();
    //    prms.Add("@AgentUID", AgentUID);
    //    DataTable dt = new DataTable();

    //    dt = DataAgent.GetSubAgents(prms);
    //    lst.DataSource = dt;
    //    lst.DataBind();

    //    lst.Items.Clear();
    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        lst.Items.Add(new ListItem(dr.ItemArray[6].ToString(), dr.ItemArray[0].ToString()));
    //    }
    //}

    //public static void LoadAgentsNew(ListControl lst, bool SearchForm)
    //{
    //    if (UserSessions.AgentList == null)
    //    {
    //        Hashtable prms = new Hashtable();

    //        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.IsAgent)
    //            prms.Add("@AgentUID", UserSessions.CurrentUser.HookTableKeyUID);

    //        DataAgent data = DataAccess.DataAgentDao;
    //        UserSessions.AgentList = data.GetAgents_LightNew(prms);
    //    }

    //    lst.Items.Clear();

    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (GenericListItem agent in UserSessions.AgentList)
    //    {
    //        lst.Items.Add(new ListItem(agent.ItemText, agent.ItemValue));
    //    }
    //}    

    public static void LoadTimeZones(ListControl lst, bool SearchForm)
    {
        if (m_TimeZones == null)
        {
            Hashtable prms = new Hashtable();

            DataLead data = DataAccess.DataLeadDao;
            m_TimeZones = data.GetTimeZones(prms);

        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_TimeZones)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadLeadFollowupStatus(ListControl lst, bool SearchForm)
    {
        if (m_FollowupStatus == null)
        {
            Hashtable prms = new Hashtable();
            DataLead data = DataAccess.DataLeadDao;
            m_FollowupStatus = data.GetLeadFollowupStatus(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));

        foreach (FollowupStatus status in m_FollowupStatus)
        {
            lst.Items.Add(new ListItem(status.Name, status.FollowupStatusID));
        }
    }

    public static void LoadStates(ListControl lst, bool SearchForm)
    {
        if (m_States == null)
        {
            Hashtable prms = new Hashtable();

            DataLead data = DataAccess.DataLeadDao;
            m_States = data.GetStates(prms);
        }



        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_States)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static Dictionary<string, string> LoadCountries()
    {
        if (m_CountryCodes == null)
        {
            DataAgent data = DataAccess.DataAgentDao;
            m_CountryCodes = data.GetCountries();
        }
        return m_CountryCodes;
    }

    public static Dictionary<string, string> LoadCountryCallingCodes()
    {
        Dictionary<string, string> list = new Dictionary<string, string>();
        DataAgent data = DataAccess.DataAgentDao;
        list = data.GetCountryCallingCodes();

        return list;
    }
    public static Dictionary<int, string> LoadPricingProgram()
    {
        Dictionary<int, string> list = new Dictionary<int, string>();
        DataAgent data = DataAccess.DataAgentDao;
        DataTable dt  = data.GetPricingProgram();

        list.Clear();
        list.Add(-1, "--Select--");
        foreach (DataRow dr in dt.Rows)
        {
            list.Add((int)dr[0], dr[1].ToString());
        }

        return list;
    }

    public static void LoadLeadSources(ListControl lst, bool SearchForm)
    {
        if (m_LeadSource == null)
        {
            Hashtable prms = new Hashtable();
            prms.Add("@Deleted", "0");
            m_LeadSource = DataAccess.DataLeadDao.GetLeadSources(prms);
        }


        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (LeadSource source in m_LeadSource)
        {
            lst.Items.Add(new ListItem(source.Name, source.LeadSourceID));
        }
    }

    public static void LoadLeadStatus(ListControl lst, bool SearchForm)
    {

        if (m_LeadStatus == null)
        {
            Hashtable prms = new Hashtable();
            DataLead data = DataAccess.DataLeadDao;
            User user = UserSessions.CurrentUser;
            prms.Add("@UserName", user.UserName);
            prms.Add("@StatusTypeUID", "4ce65c76-7f9e-4e24-baa0-c11e9dccad0f");
            //prms.Add("@OfficeID", (int)user.Office);

            m_LeadStatus = data.GetLeadStatus(prms);
        }


        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (Status status in m_LeadStatus)
        {
            if (status.StatusID.ToUpper() == Constants.LEADSTATUS_FOLLOWUP.ToUpper() || status.StatusID.ToUpper() == Constants.LEADSTATUS_WRONGCONTACTINFO.ToUpper()
                   || status.StatusID.ToUpper() == Constants.LEADSTATUS_FILEINREVIEW.ToUpper() || status.StatusID.ToUpper() == Constants.LEADSTATUS_WITHDRAWN.ToUpper() ||
                   status.StatusID.ToUpper() == Constants.LEADSTATUS_NOANSWER.ToUpper())
                continue;


            if (!SearchForm
                    && UserSessions.CurrentUser.IsAgent
                    && (UserSessions.CurrentLoggedInAgent != null && UserSessions.CurrentLoggedInAgent.ParentUID.ToUpper() == "F79D9B35-A759-4B92-B754-D63D8E62ED74") // dba: DS - (F) Kelly Nelson
                    && status.StatusID.ToUpper() == "465F6A57-AC67-4340-9A6B-AF25A61F3C6E" // Statements Received
                    )
            {
                continue;
            }

            lst.Items.Add(new ListItem(status.Name, status.StatusID));
        }
    }

    public static void LoadIPGeoResults(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        if (m_IPGeoResults == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            //m_IPGeoResults = DataAccess.DataCreditCardTransactionDao.GetIPGeoResults(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_IPGeoResults)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAVSResults(ListControl lst)
    {
        Hashtable prms = new Hashtable();
        if (m_AVSResults == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_AVSResults = DataAccess.DataCreditCardTransactionDao.GetAVSResults(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_AVSResults)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadCVV2Results(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        if (m_CVV2Results == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_CVV2Results = DataAccess.DataCreditCardTransactionDao.GetCVV2Results(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_CVV2Results)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadDeviceResults(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        if (m_DeviceResults == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_DeviceResults = DataAccess.DataCreditCardTransactionDao.GetDeviceResults(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_DeviceResults)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadCCStatus(ListControl lst, bool IsSearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_CCStatus == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_CCStatus = DataAccess.DataAchTransactionDao.GetCCStatus(prms);
        }
        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in m_CCStatus)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadHeldBy(ListControl lst, int StatusID)
    {
        Hashtable prms = new Hashtable();

        if (StatusID != -1)
            prms.Add("@StatusID", StatusID);

        IList<GenericListItem> list = DataAccess.DataRiskDao.GetHeldBy(prms);

        lst.Items.Clear();

        lst.Items.Add(new ListItem("ALL", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadBatchStatus(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        IList<GenericListItem> list = DataAccess.DataRiskDao.GetBatchStatus(prms);

        lst.Items.Clear();

        lst.Items.Add(new ListItem("ALL", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadReleasedBy(ListControl lst, int StatusID)
    {
        Hashtable prms = new Hashtable();

        if (StatusID != -1)
            prms.Add("@StatusID", StatusID);

        IList<GenericListItem> list = DataAccess.DataRiskDao.GetReleasedBy(prms);

        lst.Items.Clear();

        lst.Items.Add(new ListItem("ALL", "-1"));
        lst.Items.Add(new ListItem("ALL EXCEPT SYSTEM", "-2"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadBatchExceptions(ListControl lst, bool SearchForm, string MerchantAppUID, string RiskID, string BatchID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", MerchantAppUID);

        if (RiskID != string.Empty)
            prms.Add("@RiskID", RiskID);

        if (BatchID != string.Empty)
            prms.Add("@BatchID", BatchID);

        IList<GenericListItem> list = DataAccess.DataRiskDao.GetBatchExceptions(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPlatforms(ListControl lst, bool SearchForm, string Type)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@PlatformTypeUID", Type);

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetPlatforms(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchantAdjustmentFeeItems(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        IList<GenericListItem> list = data.GetMerchantAdjustmentFeeItems(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadReturnReasons(ListControl lst, bool IsSearchForm)
    {
        AchTransactionFacade facade = new AchTransactionFacade();
        IList<GenericListItem> list = facade.GetReturnReasons(new Hashtable());

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("--All--", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));


        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadUserDefaultRoles(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataUser data = DataAccess.DataUserDao;

        prms.Add("@Description", "Module Access");
        IList<GenericListItem> list = data.GetRoles(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAMPM(ListControl lst)
    {
        lst.Items.Clear();
        lst.Items.Add("AM");
        lst.Items.Add("PM");
    }

    public static void LoadUserAgents(ListControl lst, bool SearchForm, string AgentUID)
    {
        Hashtable prms = new Hashtable();

        if (AgentUID != string.Empty)
            prms.Add("@UID", AgentUID);

        DataAgent data = DataAccess.DataAgentDao;
        IList<Agent> list = data.GetAgents(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (Agent agent in list)
        {
            string AgentID = Convert.ToString("00000" + agent.AgentID);
            lst.Items.Add(new ListItem(AgentID.Substring(AgentID.Length - 5, 5) + ": " + agent.AgentDBA, agent.AgentUID));
        }
    }

    public static void LoadUserMerchants(ListControl lst, bool SearchForm, string MerchantAppUID)
    {
        Hashtable prms = new Hashtable();
        if (MerchantAppUID != string.Empty)
            prms.Add("@UID", MerchantAppUID);

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        DataSet ds = data.GetMerchantApps(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (DataRow row in ds.Tables[0].Rows)
        {
            lst.Items.Add(new ListItem(row["BusinessDBAName"].ToString() + " - " + row["ID"].ToString(), row["MerchantAppUID"].ToString()));
        }
    }

    public static void LoadHookTables2(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataUser data = DataAccess.DataUserDao;
        IList<GenericListItem> list = data.GetHookTables(prms);

        lst.Items.Clear();

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadHookTables(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataUser data = DataAccess.DataUserDao;
        IList<GenericListItem> list = data.GetHookTables(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadRoles(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataUser data = DataAccess.DataUserDao;
        IList<GenericListItem> list = data.GetRoles(prms);

        lst.Items.Clear();

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadRolesWithDefault(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        DataUser data = DataAccess.DataUserDao;
        IList<GenericListItem> list = data.GetRoles(prms);

        lst.Items.Clear();
        lst.Items.Add(new ListItem("All", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPortals(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataUser data = DataAccess.DataUserDao;
        IList<GenericListItem> list = data.GetPortals(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadCustomers(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        MerchantFacade facade = new MerchantFacade();
        IList<GenericListItem> list = facade.GetCustomersGeneric(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadUserAccessLevels(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataUser data = DataAccess.DataUserDao;
        IList<GenericListItem> list = data.GetAccessLevels(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchantAppTypes(ListControl lst, bool SearchForm, bool IsEnabled)
    {
        Hashtable prms = new Hashtable();

        if (m_NewBanks == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;

            prms.Add("@IsEnabled", IsEnabled);
            m_NewBanks = (List<GenericListItem>)data.GetMerchantAppTypes(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_NewBanks)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchantAppTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_Banks == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_Banks = (List<GenericListItem>)data.GetMerchantAppTypes(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_Banks)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadInternalUsers(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_Users == null)
        {
            DataUser data = DataAccess.DataUserDao;
            m_Users = data.GetInternalUsers_List();
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_Users)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadSchedueATypeListItems(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_SchedueATypeListItems == null)
        {
            DataAgent data = DataAccess.DataAgentDao;
            //DataUser data = DataAccess.DataUserDao;
            m_SchedueATypeListItems = data.GetSchedueATypeListItems();
        }
        if (lst != null && lst.Items.Count > 0)
            lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_SchedueATypeListItems)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadActiveInternalUsers(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_ActiveUsers == null)
        {
            DataUser data = DataAccess.DataUserDao;
            prms.Add("@HasDBAccess", 1);

            m_ActiveUsers = data.GetInternalUsers(prms);
        }

        lst.Items.Clear();

        lst.DataSource = m_ActiveUsers;
        lst.DataTextField = "FirstLastName";
        lst.DataValueField = "UID";
        lst.DataBind();

        if (SearchForm)
            lst.Items.Insert(0, new ListItem("All", "-1"));
        else
            lst.Items.Insert(0, new ListItem("--Select--", "-1"));

    }

    public static void LoadReserveDivertedUsers(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_ActiveUsers == null)
        {
            DataUser data = DataAccess.DataUserDao;
            prms.Add("@HasDBAccess", 1);

            m_ActiveUsers = data.GetInternalUsers(prms);
        }

        lst.Items.Clear();

        lst.DataSource = m_ActiveUsers;
        lst.DataTextField = "FirstLastName";
        lst.DataValueField = "Username";
        lst.DataBind();

        if (SearchForm)
            lst.Items.Insert(0, new ListItem("All", "-1"));
        else
            lst.Items.Insert(0, new ListItem("--Select--", "-1"));

    }

    public static void LoadGatewayMode(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_GatewayMode == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_GatewayMode = data.GetGatewayMode(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_GatewayMode)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPrivateLabels(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_PrivateLabels == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_PrivateLabels = data.GetPrivateLabels(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_PrivateLabels)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadApplicationTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_ApplicationTypes == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_ApplicationTypes = data.GetApplicationTypes(prms);

        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_ApplicationTypes)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadReturnPolicies(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_ReturnPolicies == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_ReturnPolicies = data.GetReturnPolicies(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_ReturnPolicies)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    /// <summary>
    /// Added LoadRefundPolicies for PXP-6182 
    /// </summary>
    /// <param name="lst"></param>
    public static void LoadRefundPolicies(ListControl lst)
    {
        DataAgent data = DataAccess.DataAgentDao;
        IList<GenericListItem> list = DataMerchantApp.GetInstance().GetRefundPolicies(new Hashtable());

        lst.Items.Clear();

        lst.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }
    /// <summary>
    /// Added LoadPurchaseProducts for PXP-6182
    /// </summary>
    /// <param name="lstPurchaseProducts"></param>
    public static void LoadPurchaseProducts(ListControl lstPurchaseProducts,string merchantAppUID)
    {

        IList<GenericListItem> listPurchaseProducts = GetPurchaseProducts(merchantAppUID);

        lstPurchaseProducts.Items.Clear();

        foreach (GenericListItem item in listPurchaseProducts)
        {

            lstPurchaseProducts.Items.Add(new ListItem(item.ItemText, item.ItemValue));
            lstPurchaseProducts.Items.FindByValue(item.ItemValue).Selected = item.Selected;
        }

    }

    /// <summary>
    /// Get the Purchase Products list PXP-6524 Rohit Thakur
    /// </summary>
    /// <param name="merchantAppUID"></param>
    /// <returns></returns>
    public static IList<GenericListItem> GetPurchaseProducts(string merchantAppUID)
    {
        Hashtable prms = new Hashtable();
        DataMerchantApp data = new DataMerchantApp();
        prms.Add("@MerchantAppsUID", merchantAppUID);
        IList<GenericListItem> listPurchaseProducts = data.GetMerchantAppsPurchaseProductMethods(prms);
        return listPurchaseProducts;
    }

    public static void LoadReasonChanges(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_ReasonChanges == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_ReasonChanges = data.GetReasonChanges(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_ReasonChanges)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadBusinessStructures(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_BusinessStructures == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_BusinessStructures = data.GetBusinessStructures(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_BusinessStructures)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPCIVendors(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_PCIVEndors == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_PCIVEndors = data.GetPCIVendors(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_PCIVEndors)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadFrontEndPlatforms(ListControl lst, bool SearchForm)
    {
        if (m_FrontEnds == null)
        {
            Hashtable prms = new Hashtable();
            prms.Add("@PlatformTypeUID", "f1e74ea3-c8f2-47a8-a9a1-0512c013609d");
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_FrontEnds = (List<GenericListItem>)data.GetPlatforms(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_FrontEnds)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadBackEndPlatforms(ListControl lst, bool SearchForm)
    {
        if (m_BackEnds == null)
        {
            Hashtable prms = new Hashtable();
            prms.Add("@PlatformTypeUID", "a537624c-78da-40e3-82c8-56a6033d992c");
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_BackEnds = (List<GenericListItem>)data.GetPlatforms(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_BackEnds)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    /// <summary>
    /// stores the ID instead of the UID as the value
    /// </summary>
    /// <param name="lst"></param>
    /// <param name="SearchForm"></param>
    //public static void LoadAgents2(ListControl lst, bool SearchForm)
    //{
    //    Hashtable prms = new Hashtable();
    //    IList<GenericListItem> list = null;

    //    if (UserSessions.CurrentUser != null)
    //        if (UserSessions.CurrentUser.IsAgent)
    //            prms.Add("@AgentUID", UserSessions.CurrentUser.HookTableKeyUID);

    //    DataAgent data = DataAccess.DataAgentDao;
    //    list = data.GetAgents_Light2(prms);

    //    lst.Items.Clear();

    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (GenericListItem agent in list)
    //    {
    //        lst.Items.Add(new ListItem(agent.ItemText, agent.ItemValue));
    //    }
    //}


    //public static void LoadOnlineAgentsID(ListControl lst, bool SearchForm, int AgentID)
    //{
    //    Hashtable prms = new Hashtable();

    //    if (AgentID != 0)
    //    {
    //        prms.Add("@AgentID", AgentID);
    //    }

    //    IList<GenericListItem> list = null;

    //    DataAgent data = DataAccess.DataAgentDao;
    //    list = data.GetOnlineApp_Agent(prms);

    //    lst.Items.Clear();

    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (GenericListItem agent in list)
    //    {
    //        lst.Items.Add(new ListItem(agent.ItemText, agent.ItemValue));
    //    }
    //}

    public static void LoadPageSize(ListControl lst)
    {
        lst.Items.Clear();
        lst.Items.Add(new ListItem("10 per page", "10"));
        lst.Items.Add(new ListItem("25 per page", "25"));
        lst.Items.Add(new ListItem("50 per page", "50"));
        lst.Items.Add(new ListItem("75 per page", "75"));
        lst.Items.Add(new ListItem("100 per page", "100"));
        lst.Items.Add(new ListItem("150 per page", "150"));
        lst.Items.Add(new ListItem("200 per page", "200"));
    }

    public static void LoadAgentStatus(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataLead data = DataAccess.DataLeadDao;
        User user = UserSessions.CurrentUser;

        if (user != null)
        {
            prms.Add("@UserName", user.UserName);
            prms.Add("@StatusTypeUID", "613c7216-49b9-44af-89f4-06af9590f2b5");
            // prms.Add("@OfficeID", (int)user.Office);

            IList<Status> list = data.GetLeadStatus(prms);

            lst.Items.Clear();

            if (SearchForm)
                lst.Items.Add(new ListItem("All", "-1"));
            else
                lst.Items.Add(new ListItem("--Select--", "-1"));

            foreach (Status status in list)
            {
                lst.Items.Add(new ListItem(status.Name, status.StatusID));
            }
        }
    }

    public static void GetAgentCategories(ListControl lst)
    {
        DataAgent data = DataAccess.DataAgentDao;

        IList<GenericListItem> list = data.GetAgentCategories();
        lst.Items.Clear();
        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void FillDeclineReason(ListControl li, Guid StatusUID)
    {

        Dictionary<string, string> diItems = DataMerchantApp.GetInstance().GetMerchantDeclineReason(StatusUID);

        li.Items.Clear();
        li.Items.Add(new ListItem("Select", "-1"));

        foreach (KeyValuePair<string, string> kvp in diItems)
        {
            li.Items.Add(new ListItem(kvp.Value, kvp.Key));
        }

        //li.DataSource = diItems;
        //li.DataTextField = "Value";
        //li.DataValueField = "Key";
        //li.DataBind();
    }

    public static void MerchantAppStatus(ListControl lst, bool SearchForm, string Module)
    {
        Hashtable prms = new Hashtable();
        DataLead data = DataAccess.DataLeadDao;
        User user = UserSessions.CurrentUser;
        if (user != null)
        {
            prms.Add("@UserName", user.UserName);
            if (UserSessions.CurrentMerchantApp != null)
                prms.Add("@OfficeID", (int)UserSessions.CurrentMerchantApp.Office);

            if (Module != string.Empty)
                prms.Add("@StatusTypeName", Module);

            IList<Status> list = data.GetLeadStatus(prms);

            lst.Items.Clear();

            if (SearchForm)
                lst.Items.Add(new ListItem("All", "-1"));
            else
                lst.Items.Add(new ListItem("--Select--", "-1"));

            foreach (Status status in list)
            {
                lst.Items.Add(new ListItem(status.Name, status.StatusID));
            }
            foreach (ListItem item in lst.Items)
            {
                item.Attributes.Add("title", item.Text);
            }
        }
    }

    //this method is specifically for ach accounts only. as we need to go off of the ach status nad nto cc status
    public static void MerchantAppStatus(ListControl lst, bool SearchForm, string Module, MerchantApp agreement, AchMerchant achMerchant)
    {
        Hashtable prms = new Hashtable();
        DataLead data = DataAccess.DataLeadDao;
        User user = UserSessions.CurrentUser;
        string Queue = string.Empty;

        if (achMerchant != null)
            Queue = achMerchant.MerchantStatusName.Substring(0, 2);

        prms.Add("@UserName", user.UserName);
        if (UserSessions.CurrentMerchantApp != null)
            prms.Add("@OfficeID", (int)UserSessions.CurrentMerchantApp.Office);

        if (!string.IsNullOrWhiteSpace(Module))
            prms.Add("@StatusTypeName", Module);

        if (!string.IsNullOrWhiteSpace(Queue))
            prms.Add("@Queue", Queue);

        IList<Status> list = data.GetLeadStatus(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (Status status in list)
        {
            lst.Items.Add(new ListItem(status.Name, status.StatusID));
        }

        //This logic is added for handling the Gateway only for Optimal brand Merchant.
        if (achMerchant != null && agreement != null)
        {
            //Remove CU Recieved from the Status dropdown list when the status is RM received only.
            if (agreement.Gatewayonly && achMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_SS_RECEIVED))
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_CU_RECEIVED.ToLower())); //CU - Received
            }

            //Remove AB Recieved from the Status dropdown list when the gatewayonly is not checked and when the  status name is in RM
            else if ((!agreement.Gatewayonly) && achMerchant.MerchantStatusName.Substring(0, 2) == "SS")
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_OP_RECEIVED.ToLower())); //AB - Recieved
            }

            //Show the MS Received in OP queue only for Optimal
            if (agreement.Brand == MerchantBrand.Meritus && achMerchant.MerchantStatusName.Substring(0, 2) == "OP")
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_MS_RECEIVED.ToLower())); //MS - Received
            }

            //Show Status Value of OP-QA  only once merchant application status is in Operations Department.
            if (!(achMerchant.MerchantStatusName.Substring(0, 2) == "OP"))
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_OP_QA.ToLower())); //OP - QA
            }

            //Show Status Value of MS-Active  only once merchant application status is in Merchant Support Department and not in Operations Department.
            if (achMerchant.MerchantStatusName.Substring(0, 2) == "OP")
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_MS_ACTIVE.ToLower())); //OP - QA
            }
        }

    }

    public static void MerchantAppStatus(ListControl lst, bool SearchForm, string Module, string Queue, MerchantApp agreement)
    {
        Hashtable prms = new Hashtable();
        DataLead data = DataAccess.DataLeadDao;
        User user = UserSessions.CurrentUser;

        prms.Add("@UserName", user.UserName);
        if (UserSessions.CurrentMerchantApp != null)
            prms.Add("@OfficeID", (int)UserSessions.CurrentMerchantApp.Office);

        if (Module != string.Empty)
            prms.Add("@StatusTypeName", Module);

        if (Queue != string.Empty)
            prms.Add("@Queue", Queue);

        IList<Status> list = data.GetLeadStatus(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (Status status in list)
        {
            lst.Items.Add(new ListItem(status.Name, status.StatusID));
        }

        //This logic is added for handling the Gateway only for Optimal brand Merchant.
        if (agreement != null)
        {
            //Remove CU Recieved from the Status dropdown list when the status is RM received only.
            if (agreement.Gatewayonly && agreement.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_SS_RECEIVED))
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_CU_RECEIVED.ToLower())); //CU - Received
            }

            //Remove AB Recieved from the Status dropdown list when the gatewayonly is not checked and when the  status name is in RM
            else if ((!agreement.Gatewayonly) && agreement.StatusName.Substring(0, 2) == "SS")
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_OP_RECEIVED.ToLower())); //AB - Recieved

            }

            //Show the MS Received in OP queue only for Optimal
            if (agreement.Brand == MerchantBrand.Meritus && agreement.StatusName.Substring(0, 2) == "OP")
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_MS_RECEIVED.ToLower())); //MS - Received
            }

            //To Non-GMA User login (Office Location != Los Angeles) show Status Value of OP-QA  only once merchant application status is in Operations Department.
            if ((!UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles)) && !(agreement.StatusName.Substring(0, 2) == "OP"))
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_OP_QA.ToLower())); //OP - QA
            }

            //To GMA User login (Office Location = Los Angeles) show Status Value of OP-QA for Credit Underwriting (CU) Department user if current merchant application status is equal to "CU-Approved" 
            //along with display of status value of OP-QA to a Operations Department user.
            if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles) && (!(agreement.StatusName.Substring(0, 2) == "OP") && !(agreement.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))))
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_OP_QA.ToLower())); //OP - QA
            }

            //To Non-GMA User login (Office Location != Los Angeles) show Status Value of MS-Active  only once merchant application status is in Merchant Support Department.
            if ((!UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles)) && (agreement.StatusName.Substring(0, 2) == "OP"))
            {
                lst.Items.Remove(lst.Items.FindByValue(Constants.QUEUESTATUS_MS_ACTIVE.ToLower())); //OP - QA
            }
        }

    }

    public static void MerchantAppStatus(ListControl lst, bool SearchForm, string Module, string Queue)
    {
        MerchantAppStatus(lst, SearchForm, Module, Queue, null);
    }

    public static void MerchantAppStatusList(ListControl lst, bool SearchForm, string Module)
    {
        Hashtable prms = new Hashtable();
        DataLead data = DataAccess.DataLeadDao;
        User user = UserSessions.CurrentUser;

        if (user != null)
        {
            prms.Add("@UserName", user.UserName);
            if (UserSessions.CurrentMerchantApp != null)
                prms.Add("@OfficeID", (int)UserSessions.CurrentMerchantApp.Office);
        }
        if (Module != string.Empty)
            prms.Add("@StatusTypeName", Module);

        IList<Status> list = data.GetLeadStatus(prms);

        lst.Items.Clear();


        foreach (Status status in list)
        {
            lst.Items.Add(new ListItem(status.Name, status.StatusID));
        }
    }

    public static void LoadLeadCallResults(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataLead data = DataAccess.DataLeadDao;
        IList<CallResult> list = data.GetLeadCallResults(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));

        foreach (CallResult result in list)
        {
            lst.Items.Add(new ListItem(result.Name, result.CallResultID));
        }

        if (lst.Items.Count > 0)
            lst.SelectedIndex = 0;
    }

    public static void LoadAgentTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataAgent data = DataAccess.DataAgentDao;
        IList<GenericListItem> list = data.GetAgentTypes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadEquipmentTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataEquipment data = DataAccess.DataEquipmentDao;
        IList<GenericListItem> list = data.GetEquipmentTypes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadEquipmentMaker(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataEquipment data = DataAccess.DataEquipmentDao;
        IList<GenericListItem> list = data.GetEquipmentMaker(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchantNoteCodes(ListControl lst, bool SearchForm, string module)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        if (module != string.Empty)
            prms.Add("@Type", module);
        if (UserSessions.CurrentUser != null)
            prms.Add("@UserUID", UserSessions.CurrentUser.UID);

        IList<GenericListItem> list = data.GetMerchantNoteCodes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchant_NoteCodes(ListControl lst, bool SearchForm, string module)
    {
        if (UserSessions.MerchantNoteCodes == null)
        {
            Hashtable prms = new Hashtable();

            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            if (module != string.Empty)
                prms.Add("@Type", module);
            if (UserSessions.CurrentUser != null)
                prms.Add("@UserUID", UserSessions.CurrentUser.UID);

            UserSessions.MerchantNoteCodes = data.LoadMerchantNoteCodes(prms);
            UserSessions.MerchantNoteCodes.Add("-1", new MerchantNoteCodes());
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (KeyValuePair<string, MerchantNoteCodes> kvp in UserSessions.MerchantNoteCodes)
        {
            lst.Items.Add(new ListItem(kvp.Value.Description, kvp.Key.ToLower()));
        }
    }

    public static void LoadAgentNoteCodes(ListControl lst, bool SearchForm, string module)
    {
        Hashtable prms = new Hashtable();

        DataAgent data = DataAccess.DataAgentDao;
        if (module != string.Empty)
            prms.Add("@Type", module);

        IList<GenericListItem> list = data.GetAgentNoteCodes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadComplianceMerchantLevels(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<ComplianceMerchantLevel> list = data.GetComplianceMerchantLevels(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (ComplianceMerchantLevel item in list)
        {
            lst.Items.Add(new ListItem(item.Level.ToString(), item.ComplianceMerchantLevelID));
        }
    }

    public static void LoadUWConditions(ListControl lst, bool SearchForm, string Queue)
    {
        Hashtable prms = new Hashtable();
        if (Queue == "SS" || Queue == "CU")// AP Queue is now changed to RM
            prms.Add("@type", Queue);
        DataSet ds = DataAccess.DataMerchantAppDao.GetUWConditions(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (DataRow dr in ds.Tables[0].Rows)
            lst.Items.Add(new ListItem(dr["ConditionName"].ToString(), dr["ConditionID"].ToString()));

        ds = null;
    }

    public static void LoadRiskCategories(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<RiskCategory> list = data.GetRiskCategories(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (RiskCategory item in list)
        {
            if (item.PCI)
                lst.Items.Add(new ListItem("PCI - " + item.Name, item.RiskCategoryID));
            else
                lst.Items.Add(new ListItem(item.Name, item.RiskCategoryID));
        }
    }

    public static void LoadReleaseMethods(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetReleaseMethods(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchantClosureCodes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
       
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetMerchantClosureCodes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    //Added Code By Anuj for PXP-10756
    public static void LoadMerchantRiskStatus(ListControl lst, bool SearchForm, int eligibilityID)
    {
        Hashtable prms = new Hashtable();
        if(eligibilityID != 0)
        {
            prms.Add("@EligibilityID", eligibilityID);
        }
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetMerchantRiskStatus(prms);
        
        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else if(eligibilityID == 0)
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAdvertisingMethods(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetAdvertisingMethods(prms);

        lst.Items.Clear();

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadSeasonalBusinessMonths(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetSeasonalBusinessMonths(prms);

        lst.Items.Clear();

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadShipToLocations(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetShipToLocations(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadShippingMethods(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetShippingMethods(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadApplicationStatus(ListControl lst, Hashtable prms, bool SearchForm)
    {

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetApplicationStatus(prms);

        lst.Items.Clear();

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMDocType(ListControl lst, bool SearchForm, MDoc.eMDocTypeGroup dtg)
    {
        lst.Items.Clear();

        if (!SearchForm)
            lst.Items.Add(new ListItem("--Select--", "-1"));

        List<MDocType> li = DataDocuments.GetInstance().GetMDocType(dtg);

        if (li != null && li.Count > 0)
        {
            foreach (MDocType obj in li)
            {
              
               //Code added for fixing bug PXP-6833 by koshlendra start
                //Addded BBVA bank for PXP-10237 by koshlendra
               if (UserSessions.CurrentMerchantApp != null && obj.Name.Trim() == "Shopping Cart Agreement" 
                    && !(UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST 
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
                continue;
               else
               //Code added for fixing bug PXP-6833 by koshlendra end
                lst.Items.Add(new ListItem(obj.Name, obj.DocTypeID.ToString()));
                
               
                
                
            }
        }
    }

    //caution: this loads group types / not sources
    public static void LoadDocumentSources(DropDownList lstDocumentSources, MDoc.eMDocTypeGroup eMDocTypeGroup)
    {
        string[] DocumentSources = System.Enum.GetNames(typeof(MDoc.eMDocTypeGroup));

        foreach (string SourceName in DocumentSources)
        {
            int Value = (int)System.Enum.Parse(typeof(MDoc.eMDocTypeGroup), SourceName);
            ListItem Item = new ListItem(SourceName, Value.ToString());

            if (Value == (int)eMDocTypeGroup)
            {
                Item.Selected = true;
            }

            if (Item.Text.ToLower() == "notset")
            {
                Item.Text = "All";
            }

            lstDocumentSources.Items.Add(Item);
        }
    }

    //created new version of LoadDocumentSources b/c LoadDocumentSources doesn't use sources - it uses groups!
    public static void LoadDocumentSourceTypes(DropDownList lstDocumentSources, MDoc.eMDocSourceID eMDocSource)
    {
        string[] DocumentSources = System.Enum.GetNames(typeof(MDoc.eMDocSourceID));

        foreach (string SourceName in DocumentSources)
        {
            int Value = (int)System.Enum.Parse(typeof(MDoc.eMDocSourceID), SourceName);
            ListItem Item = new ListItem(SourceName, Value.ToString());

            if (Value == (int)eMDocSource)
            {
                Item.Selected = true;
            }

            if (Item.Text.ToLower() == "notset")
            {
                Item.Text = "All";
            }

            lstDocumentSources.Items.Add(Item);
        }
    }

    public static void LoadDocumentTypes2(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantRequiredDocument data = DataAccess.DataMerchantRequiredDocumentDao;
        IList<GenericListItem> list = data.GetDocumentTypes2(prms);

        lst.Items.Clear();

        //if (SearchForm)
        //    lst.Items.Add(new ListItem("All", "-1"));
        //else
        //    lst.Items.Add(new ListItem("--Select--", "-1"));

        if (!SearchForm)
            lst.Items.Add(new ListItem("--Select--", "-1"));


        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadDocumentTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantRequiredDocument data = DataAccess.DataMerchantRequiredDocumentDao;
        IList<GenericListItem> list = data.GetDocumentTypes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAreaZones(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetAreaZones(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAttachments(ListControl lst, bool SearchForm, string EmailBlasterID)
    {
        DataCommunication data = DataAccess.DataCommunicationDao;
        IList<Attachments> list = data.GetAttachments("", EmailBlasterID);

        lst.Items.Clear();

        foreach (Attachments item in list)
        {
            lst.Items.Add(new ListItem(item.Name, item.FileName + ";" + item.AttachmentID));
        }
    }

    public static void LoadSquareFootage(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetSquareFootage(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadLocationTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetLocationTypes(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadBusinessPremisesOwnerships(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetBusinessPremisesOwnerships(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadRefundPolicies(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetRefundPolicies(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadRefundPeriod(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetRefundPeriod(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadEquipmentProviders(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetEquipmentProviders(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPendingEquipments(ListControl lst, string merchantUID)
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<PendingTerminal> list = data.GetPendingTerminals(merchantUID);

        lst.Items.Clear();

        foreach (PendingTerminal item in list)
        {
            lst.Items.Add(new ListItem(item.Model, item.UID));
        }
    }

    public static void LoadMotoProductFulfillments(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetMotoProductFulfillments(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMotoVisaMCDepositPeriods(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        IList<GenericListItem> list = data.GetMotoVisaMCDepositPeriods(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadDepartments(ListControl lst, bool SearchForm)
    {
        IList<GenericListItem> list = CachedLookupFacade.GetCachedTicketDepartmentListGeneric(new Hashtable { { "@ShowZeus", 1 } });

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAgentTicketDepartments(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ShowAgent", 1);
        IList<GenericListItem> list = CachedLookupFacade.GetCachedTicketDepartmentListGeneric(prms);



        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            string department_display = item.ItemText;

            if (item.ItemText.ToUpper().Trim() == "IT")
            {
                department_display = "Support";
            }
            else if (item.ItemText.ToUpper().Trim() == "MERCHANT SUPPORT")
            {
                department_display = "Customer Service";
            }

            lst.Items.Add(new ListItem(department_display, item.ItemValue));

        }
    }

    public static void LoadMerchantTicketDepartments(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ShowMerchant", 1);

        IList<GenericListItem> list = CachedLookupFacade.GetCachedTicketDepartmentListGeneric(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            string department_display = item.ItemText;

            if (item.ItemText.ToUpper().Trim() == "IT")
            {
                department_display = "Support";
            }
            else if (item.ItemText.ToUpper().Trim() == "MERCHANT SUPPORT")
            {
                department_display = "Customer Service";
            }

            lst.Items.Add(new ListItem(department_display, item.ItemValue));

        }
    }

    public static void LoadPaymentXPTicketDepartments(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@ShowPaymentXP", 1);
        IList<GenericListItem> list = CachedLookupFacade.GetCachedTicketDepartmentListGeneric(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in list)
        {
            string department_display = item.ItemText;

            if (item.ItemText.ToUpper().Trim() == "IT")
            {
                department_display = "Support";
            }
            else if (item.ItemText.ToUpper().Trim() == "MERCHANT SUPPORT")
            {
                department_display = "Customer Service";
            }

            lst.Items.Add(new ListItem(department_display, item.ItemValue));

        }
    }

    public static void LoadTicketCategories(ListControl lst, bool SearchForm, int DepartmentID, string TicketSource, string ParentID)
    {
        Hashtable prms = new Hashtable();
        StringBuilder sb = new StringBuilder();

        prms.Add("@ParentID", ParentID);
        if (DepartmentID != -1)
        {
            prms.Add("@DepartmentID", DepartmentID);
        }

        switch (TicketSource)
        {
            case "a":
                prms.Add("@ShowAgent", true);
                break;

            case "m":
                prms.Add("@ShowMerchant", true);
                break;

            case "x":
                prms.Add("@ShowPaymentXP", true);
                break;

            case "i":
                prms.Add("@ShowZeus", true);
                break;
        }

        List<GenericListItem> li = CachedLookupFacade.GetCachedTicketCategoryListGeneric(prms, SearchForm) as List<GenericListItem>;

        lst.Items.Clear();
        lst.Items.Add(new ListItem((SearchForm ? "All" : "--Select--"), "-1"));
        li.ForEach(x => lst.Items.Add(new ListItem(x.ItemText, x.ItemValue)));

    }

    public static string GetTicketCategoryByID(string CatID)
    {
        List<TicketCategories> li = CachedLookupFacade.GetCachedTicketCategoryList(new Hashtable { { "@CategoryID", CatID } }) as List<TicketCategories>;
        if (li != null && li.Count == 1)
        {
            return li.FirstOrDefault().Name;
        }
        else
        {
            return string.Empty;
        }

    }

    //public static void LoadCategories(ListControl lst, bool SearchForm)
    //{
    //    LookupTableHandler.LoadCategories(lst, SearchForm, -1, "i");
    //}

    //public static void LoadCategories(ListControl lst, bool SearchForm, int DepartmentID, string TicketSource)
    //{
    //    Hashtable prms = new Hashtable();

    //    if (DepartmentID != -1)
    //    {
    //        prms.Add("@DepartmentID", DepartmentID.ToString());
    //    }

    //    switch (TicketSource)
    //    {
    //        case "a":
    //            prms.Add("@ShowAgent", "1");
    //            break;

    //        case "m":
    //            prms.Add("@ShowMerchant", "1");
    //            break;

    //        case "x":
    //            prms.Add("@ShowPaymentXP", "1");
    //            break;

    //        case "i":
    //            prms.Add("@ShowZeus", "1");
    //            break;
    //    }

    //    DataTicket data = DataAccess.DataTicketDao;
    //    IList<GenericListItem> list = data.GetTicketCategories(prms, SearchForm);

    //    lst.Items.Clear();

    //    if (SearchForm)
    //    {
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    }
    //    else
    //    {
    //        lst.Items.Add(new ListItem("--Select--", "-1"));
    //    }
    //    foreach (GenericListItem item in list)
    //    {
    //        lst.Items.Add(new ListItem(item.ItemText.Trim(), item.ItemValue));
    //    }
    //}

    //public static void LoadSubCategories(ListControl lst, bool SearchForm, int DepartmentID, string TicketSource, string ParentID)
    //{
    //    Hashtable prms = new Hashtable();

    //    if (DepartmentID != -1)
    //    {
    //        prms.Add("@DepartmentID", DepartmentID.ToString());
    //    }

    //    prms.Add("@ParentID", ParentID);

    //    switch (TicketSource)
    //    {
    //        case "a":
    //            prms.Add("@ShowAgent", "1");
    //            break;

    //        case "m":
    //            prms.Add("@ShowMerchant", "1");
    //            break;

    //        case "x":
    //            prms.Add("@ShowPaymentXP", "1");
    //            break;

    //        case "i":
    //            prms.Add("@ShowZeus", "1");
    //            break;
    //    }

    //    DataTicket data = DataAccess.DataTicketDao;
    //    IList<GenericListItem> list = data.GetTicketCategories(prms, SearchForm);

    //    lst.Items.Clear();

    //    if (SearchForm)
    //    {
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    }
    //    else
    //    {
    //        lst.Items.Add(new ListItem("--Select--", "-1"));
    //    }

    //    foreach (GenericListItem item in list)
    //    {
    //        lst.Items.Add(new ListItem(item.ItemText.Trim(), item.ItemValue));
    //    }
    //}

    public static void LoadTime(ListControl lst)
    {
        lst.Items.Clear();

        DateTime StartTime = DateTime.Today;
        DateTime EndTime = DateTime.Today.AddDays(1);
        lst.Items.Clear();
        do
        {
            lst.Items.Add(new ListItem(StartTime.ToString("HH:mm tt"), StartTime.ToShortTimeString()));
            StartTime = StartTime.AddMinutes(30);
        } while (!(StartTime >= EndTime));
    }

    public static void LoadTime1(ListControl lst)
    {
        lst.Items.Clear();

        DateTime StartTime = DateTime.Today;
        DateTime EndTime = DateTime.Today.AddDays(1);
        lst.Items.Clear();
        do
        {
            lst.Items.Add(new ListItem(StartTime.ToString("HH:mm tt"), StartTime.ToString("HH:mm tt")));
            StartTime = StartTime.AddMinutes(30);
        } while (!(StartTime >= EndTime));
    }

    public static void LoadHour(ListControl lst)
    {
        lst.Items.Clear();

        DateTime StartTime = DateTime.Today;
        DateTime EndTime = DateTime.Today.AddDays(1);
        lst.Items.Clear();
        do
        {
            lst.Items.Add(new ListItem(StartTime.ToString("HH:mm tt"), StartTime.ToShortTimeString()));
            StartTime = StartTime.AddMinutes(60);
        } while (!(StartTime >= EndTime));
    }

    public static void LoadDropDownList(ListControl control, String spName, String DataTextField, String DataValueField)
    {
        Hashtable prms = new Hashtable();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = spName;

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringACHBuild());

        control.DataTextField = DataTextField;
        control.DataValueField = DataValueField;

        control.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in ds.Tables[0].Rows)
            control.Items.Add(new ListItem(dr[DataTextField].ToString(), dr[DataValueField].ToString()));

        ds = null;
    }

    public static void LoadDropDownListNoDefault(ListControl control, String spName, String DataTextField, String DataValueField)
    {
        Hashtable prms = new Hashtable();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.CommandText = spName;

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringACHBuild());

        control.DataTextField = DataTextField;
        control.DataValueField = DataValueField;

        foreach (DataRow dr in ds.Tables[0].Rows)
            control.Items.Add(new ListItem(dr[DataTextField].ToString(), dr[DataValueField].ToString()));

        ds = null;
    }

    public static void LoadDropDownList(ListControl control, SqlCommand cmd, String DataTextField, String DataValueField)
    {
        Hashtable prms = new Hashtable();
        DataSet ds = DataLayer.GetDataSet(cmd, cmd.Connection.ConnectionString);

        control.DataTextField = DataTextField;
        control.DataValueField = DataValueField;

        control.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in ds.Tables[0].Rows)
            control.Items.Add(new ListItem(dr[DataTextField].ToString(), dr[DataValueField].ToString()));

        ds = null;
    }

    public static void BindListControl(ListControl control, DataTable source, string valueField, string textField, bool insertRowNone)
    {
        control.DataValueField = valueField;
        control.DataTextField = textField;

        if (insertRowNone)
            control.Items.Add(new ListItem("--Select--", "-1"));

        foreach (DataRow dr in source.Rows)
            control.Items.Add(new ListItem(dr[textField].ToString(), dr[valueField].ToString()));
    }

    public static void LoadDropDownList(ListControl control, DataTable source, String DataTextField, String DataValueField)
    {
        control.DataTextField = DataTextField;
        control.DataValueField = DataValueField;

        control.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in source.Rows)
            control.Items.Add(new ListItem(dr[DataTextField].ToString(), dr[DataValueField].ToString()));
    }

    #region " Custom web.config File Entries "

    public static string ApplicationStatus(ApplicationStatusType status)
    {
        string paramsString = string.Empty;

        ApplicationStatusConfigSection appStatus = (ApplicationStatusConfigSection)ConfigurationManager.GetSection("ApplicationStatus");

        if (appStatus != null)
        {
            int count;
            int i = 0;

            switch (status)
            {
                case ApplicationStatusType.AgentRelationsApplicationOut:
                    count = appStatus.AgentRelationsApplicationOutItems.Count - 1;

                    foreach (AgentRelationsApplicationOutElement p in appStatus.AgentRelationsApplicationOutItems)
                    {
                        if (count == i)
                            paramsString += p.StatusUID;
                        else
                            paramsString += p.StatusUID + ",";

                        i++;
                    }
                    break;

                case ApplicationStatusType.UnderwritingApproved:
                    count = appStatus.UnderwritingApprovedItems.Count - 1;

                    foreach (UnderwritingApprovedElement p in appStatus.UnderwritingApprovedItems)
                    {
                        if (count == i)
                            paramsString += p.StatusUID;
                        else
                            paramsString += p.StatusUID + ",";

                        i++;
                    }
                    break;
                case ApplicationStatusType.UnderwritingDeclined:
                    count = appStatus.UnderwritingDeclineItems.Count - 1;

                    foreach (UnderwritingDeclineElement p in appStatus.UnderwritingDeclineItems)
                    {
                        if (count == i)
                            paramsString += p.StatusUID;
                        else
                            paramsString += p.StatusUID + ",";

                        i++;
                    }
                    break;
                case ApplicationStatusType.UnderwritingPending:
                    count = appStatus.UnderwritingPendingItems.Count - 1;

                    foreach (UnderwritingPendingElement p in appStatus.UnderwritingPendingItems)
                    {
                        if (count == i)
                            paramsString += p.StatusUID;
                        else
                            paramsString += p.StatusUID + ",";

                        i++;
                    }
                    break;
            }
        }

        return paramsString;
    }

    public enum ApplicationStatusType
    {
        UnderwritingApproved = 100,
        UnderwritingDeclined = 101,
        UnderwritingPending = 102,
        AgentRelationsApplicationOut = 200
    }

    #endregion

    public static void LoadTicketStatusHistory(ListControl lst, IList<GenericListItem> glst)
    {
        DataTicket data = DataAccess.DataTicketDao;
        IList<GenericListItem> list = glst;

        lst.Items.Clear();

        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void RiskExceptionList(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataTable dt = new DataTable();
        DataRisk data = DataAccess.DataRiskDao;
        dt = data.GetRiskExceptions(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        foreach (DataRow dr in dt.Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void LoadLeadCategories(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataLeadServices data = new DataLeadServices();

        prms.Add("@Category", "5");
        IList<LeadServices> list = data.GetLeadServices(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (LeadServices service in list)
        {
            lst.Items.Add(new ListItem(service.Name, service.LeadServiceID));
        }
    }

    public static void LoadCashAdvanceLender(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataTable dt = new DataTable();
        DataRisk data = DataAccess.DataRiskDao;

        dt = data.GetCashAdvanceLender(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in dt.Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void LoadCashAdvanceCollection(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataTable dt = new DataTable();
        DataRisk data = DataAccess.DataRiskDao;

        dt = data.GetCashAdvanceCollection(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in dt.Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void LoadCashAdvanceStatus(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataTable dt = new DataTable();
        DataRisk data = DataAccess.DataRiskDao;

        dt = data.GetCashAdvanceStatus(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in dt.Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void LoadTerminalShippings(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataTable dt = new DataTable();

        dt = DataTerminals.GetTerminalShippings(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in dt.Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void LoadAgentQueues(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataSet dt = new DataSet();
        DataAgent data = DataAccess.DataAgentDao;

        dt = data.GetAllQueues(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in dt.Tables[0].Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString()));
        }
    }

    public static void LoadAllDepts(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataSet dt = new DataSet();
        DataAgent data = DataAccess.DataAgentDao;

        dt = data.GetAllDepts(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
        foreach (DataRow dr in dt.Tables[0].Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[0].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void LoadCashAdvanceMerchants(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataTable dt = new DataTable();
        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        dt = data.GetCashAdvanceMerchants(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();

        foreach (DataRow dr in dt.Rows)
        {
            lst.Items.Add(new ListItem(dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString()));
        }
    }

    public static void GetAgentLevels()
    {
        DataUnderwritng obj = new DataUnderwritng();

        if (m_AgentLevels == null)
            m_AgentLevels = obj.GetUWAgentLevels(new Hashtable());
    }


    public static void LoadAgentLevels(ListControl lst, bool SearchForm, string Bank)
    {
        DataUnderwritng obj = new DataUnderwritng();
        Hashtable prms = new Hashtable();

        string BankBIN = GetBin(Bank);

        if (!string.IsNullOrWhiteSpace(BankBIN))
            prms.Add("@BIN", BankBIN);

        UserSessions.CurrentAgentLevels = obj.GetUWAgentLevels(prms);

        lst.Items.Clear();

        if (SearchForm)
        {
            lst.Items.Add(new ListItem("All", "-1"));
        }
        else
        {
            lst.Items.Add(new ListItem("--Select--", "-1"));
        }

        if (UserSessions.CurrentAgentLevels != null)
        {
            foreach (GenericListItem item in UserSessions.CurrentAgentLevels)
            {
                lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
            }
        }
    }

    private static string GetBin(string Bank)
    {
        if (Bank == Constants.BANK_BMO_HARRIS)
            return "9109";

        return "0000";
    }

    public static void LoadAgentLevels(ListControl lst, bool SearchForm)
    {
        LoadAgentLevels(lst, SearchForm, "");
    }

    public static void LoadFirstTeamRoleUsers(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@DefaultRoleUID", Constants.ROLE_FIRSTTEAM);

        //IList<User> userList = DataAccess.DataUserDao.GetInternalUsers(); 
        IList<User> userList = DataAccess.DataUserDao.GetUsers(prms);

        lst.Items.Clear();

        SetDefaultItem(ref lst, SearchForm);

        foreach (User user in userList)
        {
            lst.Items.Add(new ListItem(user.ToFullNameString(), user.UID));
        }
    }

    public static void LoadFirstTeamRules(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@RoleUID", Constants.ROLE_FIRSTTEAM);

        Dictionary<int, MRule> di = DataMRule.SearchMRule(prms);

        lst.Items.Clear();

        SetDefaultItem(ref lst, SearchForm);

        foreach (KeyValuePair<int, MRule> kvp in di)
        {
            lst.Items.Add(new ListItem(kvp.Value.RuleNameNice, kvp.Value.MRuleID.ToString()));
        }
    }

    public static void LoadRMRoleUsers(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@DefaultRoleUID", Constants.ROLE_AGENT_RELATIONS);

        IList<User> userList = DataAccess.DataUserDao.GetInternalUsers(prms);

        lst.Items.Clear();

        SetDefaultItem(ref lst, SearchForm);

        foreach (User user in userList)
        {
            if (user.HasDBAccess)
            {
                lst.Items.Add(new ListItem(user.ToFullNameString(), user.UID));
            }
        }
    }

    public static void LoadUsersByRole(ListControl lst, bool SearchForm, string RoleUID)
    {
        Hashtable prms = new Hashtable();

        IList<User> list = null;

        if (m_UsersByRole != null)
        {
            m_UsersByRole.TryGetValue(RoleUID.ToUpper(), out list);
        }
        else
        {
            m_UsersByRole = new Dictionary<string, IList<User>>();
        }


        if (m_UsersByRole == null || list == null)
        {

            prms.Add("@RoleUID", RoleUID);
            list = DataAccess.DataUserDao.GetInternalUsers(prms);
            m_UsersByRole.Add(RoleUID.ToUpper(), list);
        }

        lst.Items.Clear();

        SetDefaultItem(ref lst, SearchForm);

        foreach (User user in list)
        {
            if (user.HasDBAccess)
            {
                lst.Items.Add(new ListItem(user.FirstLastName, user.UID));
            }
        }
    }

    public static void LoadSalesSupport(ListControl lst, bool SearchForm, string RoleUID)
    {
        DataUser data = DataAccess.DataUserDao;
        m_SalesSupport = data.GetSalesSupport_List(RoleUID);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_SalesSupport)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadRequestedByUsers(ListControl lst, bool SearchForm, string RoleUID)
    {
        Hashtable prms = new Hashtable();

        IList<User> list = null;

        if (m_RequestedByUsers != null)
        {
            m_RequestedByUsers.TryGetValue(RoleUID.ToUpper(), out list);
        }
        else
        {
            m_RequestedByUsers = new Dictionary<string, IList<User>>();
        }


        if (m_RequestedByUsers == null || list == null)
        {

            prms.Add("@RoleUID", RoleUID);
            list = DataAccess.DataUserDao.GetInternalUsers(prms);
            m_RequestedByUsers.Add(RoleUID.ToUpper(), list);
        }

        lst.Items.Clear();

        SetRequestedByDefaultItem(ref lst);

        foreach (User user in list)
        {
            if (user.HasDBAccess)
            {
                lst.Items.Add(new ListItem(user.FirstLastName, user.UID));
            }
        }
    }


    //public static void LoadReserveDivertedType(ListControl lst, bool SearchForm)
    //{
    //    Hashtable prms = new Hashtable();
    //    DataSet dt = new DataSet();

    //    dt = DataReserve.GetReserveDivertedType(prms);
    //    lst.DataSource = dt;
    //    lst.DataBind();

    //    lst.Items.Clear();
    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (DataRow dr in dt.Tables[0].Rows)
    //    {
    //        lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
    //    }
    //}

    //public static void LoadReserveReasons(ListControl lst, bool SearchForm)
    //{
    //    Hashtable prms = new Hashtable();
    //    DataSet dt = new DataSet();

    //    dt = DataReserve.GetReserveReason(prms);
    //    lst.DataSource = dt;
    //    lst.DataBind();

    //    lst.Items.Clear();
    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (DataRow dr in dt.Tables[0].Rows)
    //    {
    //        lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
    //    }
    //}

    //public static void LoadReserveResolution(ListControl lst, bool SearchForm)
    //{
    //    Hashtable prms = new Hashtable();
    //    DataSet dt = new DataSet();

    //    dt = DataReserve.GetReserveResolution(prms);
    //    lst.DataSource = dt;
    //    lst.DataBind();

    //    lst.Items.Clear();
    //    if (SearchForm)
    //        lst.Items.Add(new ListItem("All", "-1"));
    //    else
    //        lst.Items.Add(new ListItem("--Select--", "-1"));

    //    foreach (DataRow dr in dt.Tables[0].Rows)
    //    {
    //        lst.Items.Add(new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString()));
    //    }
    //}

    public static void LoadVerticalMarketValues(ListControl lst, ListControl lstBillingType, ListControl lstMktMethods)
    {
        Hashtable prms = new Hashtable();
        DataSet dt = new DataSet();

        dt = DataAccess.DataMerchantAppDao.GetVerticalMarkets(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();
        //if (SearchForm)
        //    lst.Items.Add(new ListItem("All", "-1"));
        //else
        //    lst.Items.Add(new ListItem("--Select--", "-1"));

        ListItem item = null;
        //Amit Patne : PXP-3818 - Zeus: Vertical Market and Billing Method
        DataRow[] drVerticalMarket = dt.Tables[0].Select("VerticalMarketTypeID=" + 1);
        foreach (DataRow dr in drVerticalMarket)
        {
            item = new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            item.Attributes.Add("title", dr.ItemArray[2].ToString());
            lst.Items.Add(item);
        }

        item = null;
        lstBillingType.Items.Clear();
        DataRow[] drBillingType = dt.Tables[0].Select("VerticalMarketTypeID=" + 2);
        foreach (DataRow dr in drBillingType)
        {
            item = new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            item.Attributes.Add("title", dr.ItemArray[2].ToString());
            lstBillingType.Items.Add(item);
        }

        //Add code by anuj for PXP-9250
        item = null;
        lstMktMethods.Items.Clear();
        DataRow[] drMktMethods = dt.Tables[0].Select("VerticalMarketTypeID=" + 3);
        foreach (DataRow dr in drMktMethods)
        {
            item = new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            item.Attributes.Add("title", dr.ItemArray[2].ToString());
            lstMktMethods.Items.Add(item);
        }

    }

    #region Helper Methods

    private static void SetDefaultItem(ref ListControl lst, bool SearchForm)
    {
        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));
    }

    private static void SetRequestedByDefaultItem(ref ListControl lst)
    {
        lst.Items.Add(new ListItem("--Select--", "-1"));
        lst.Items.Add(new ListItem("Other", "0"));
    }

    private static void FillListControl(ref ListControl lst, IList<GenericListItem> list)
    {
        foreach (GenericListItem item in list)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    #endregion

    public static void LoadChangeReason(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataSet dt = new DataSet();

        dt = DataAccess.DataTicketDao.GetDueDateChangeReasons(prms);
        lst.DataSource = dt;
        lst.DataBind();

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        ListItem item = null;

        foreach (DataRow dr in dt.Tables[0].Rows)
        {
            item = new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            lst.Items.Add(item);
        }
    }

    public static void LoadCountries(ListControl lst)
    {
        lst.DataSource = LookupTableHandler.LoadCountries();
        lst.DataTextField = "Value";
        lst.DataValueField = "Key";
        lst.DataBind();
    }

    public static void LoadCountryCallingCodes(ListControl lst)
    {
        lst.DataSource = LookupTableHandler.LoadCountryCallingCodes();
        lst.DataTextField = "Key";
        lst.DataValueField = "Value";
        lst.DataBind();
    }

    public static void LoadMerchantETCTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchETCType";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

        lst.DataSource = ds;
        lst.DataBind();

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        ListItem item = null;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            item = new ListItem(dr.ItemArray[3].ToString(), dr.ItemArray[0].ToString());
            lst.Items.Add(item);
        }

    }

    public static void LoadDescriptorTypes(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_SearchDescriptorType";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

        lst.DataSource = ds;
        lst.DataBind();

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        ListItem item = null;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            item = new ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            lst.Items.Add(item);
        }
    }

    public static void LoadPricingType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_PricingType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_PricingType = data.GetPricingType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_PricingType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadInvoiceCategories(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (LookupTableHandler.m_InvoiceCategories == null)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchInvoiceCategory";
            cmd.CommandType = CommandType.StoredProcedure;

            using (SqlDataReader dr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                LookupTableHandler.m_InvoiceCategories = new List<GenericListItem>();

                while (dr.Read())
                {
                    LookupTableHandler.m_InvoiceCategories.Add(new GenericListItem()
                    {
                        ItemText = dr["CategoryName"].ToString(),
                        ItemValue = dr["CategoryID"].ToString()
                    });
                }

                dr.Close();
            }
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in LookupTableHandler.m_InvoiceCategories)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadParticipantType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_ParticipantType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_ParticipantType = data.GetParticipantType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_ParticipantType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadSecurityProtocol(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_SecurityProtocol == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_SecurityProtocol = data.GetSecurityProtocol(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_SecurityProtocol)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadTerminalActivationType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_TerminalActivationType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_TerminalActivationType = data.GetTerminalActivationType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_TerminalActivationType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadDepositType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_DepositType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_DepositType = data.GetDepositType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_DepositType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAuthPOSType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_AuthPOSType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_AuthPOSType = data.GetAuthPOSType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_AuthPOSType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMagneticStripeType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_MagneticStripeType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_MagneticStripeType = data.GetMagneticStripeType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_MagneticStripeType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPOSDeviceType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_POSDeviceType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_POSDeviceType = data.GetPOSDeviceType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_POSDeviceType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPOSCapabilityType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_POSCapabilityType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_POSCapabilityType = data.GetPOSCapabilityType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_POSCapabilityType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadDiscoverRefNo(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_DiscoverRefNo == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_DiscoverRefNo = data.GetDiscoverRefNo(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_DiscoverRefNo)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadMerchantTypeCode(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_MerchantType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_MerchantType = data.GetMerchantType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_MerchantType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadAMEXPCIDType(ListControl lst, bool SearchForm)
    {
        //AMEXPCIDType

        Hashtable prms = new Hashtable();

        if (m_AMEXPCIDType == null)
        {
            DataMerchantBoarding data = new DataMerchantBoarding();
            m_AMEXPCIDType = data.GetAMEXPCIDType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_AMEXPCIDType)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadACHStatus(ListControl lst, bool IsSearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_CCStatus == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_CCStatus = DataAccess.DataAchTransactionDao.GetStatus(prms);
        }
        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in m_CCStatus)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadPartnerChannels(ListControl lst, bool IsSearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_Channels == null)
        {
            DataAgent data = DataAccess.DataAgentDao;
            m_Channels = data.GetPartnerChannels(prms);
        }

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        //else
        //    lst.Items.Add(new ListItem("Select", "-1"));

        foreach (GenericListItem item in m_Channels)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

        if (!IsSearchForm)
            ListHandler.GetListItem(lst, Convert.ToString((int)SalesPartnerGroupID.UnAssigned)); // default item to be selected is Assigned
    }

    public static void LoadPartnerChannels(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        if (m_Channels == null)
        {
            DataAgent data = DataAccess.DataAgentDao;
            m_Channels = data.GetPartnerChannels(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_Channels)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }

    public static void LoadMerchantCategories(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        DataMerchantServices data = new DataMerchantServices();

        prms.Add("@CategoryID", 6);
        IList<MerchantServices> list = data.GetMerchantServicesList(prms);

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (MerchantServices service in list)
        {
            lst.Items.Add(new ListItem(service.Name, service.MerchantServiceID));
        }
    }

    public static void LoadUserTimeZones(ListControl lst, bool IsSearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_UserTimeZones == null)
        {
            DataUser data = DataAccess.DataUserDao;
            m_UserTimeZones = data.GetTimeZones(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_UserTimeZones)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

        if (IsSearchForm)
        {
            //--Select-- (Value 0) Is DB driven.
            //We need it to be on the DB for referential integrity on ticket TimeZone.
            lst.Items.Remove(lst.Items.FindByValue("0"));
            lst.Items.Insert(0, new ListItem("All", string.Empty));
        }


    }

    public static void LoadUserOffice(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        lst.Items.Clear();
        lst.Items.Add(new ListItem("All", "-1"));


        foreach (Office office in UserSessions.CurrentUser.OfficeAccess)
        {
            lst.Items.Add(new ListItem(office.Name, office.OfficeID.ToString()));
        }

    }

    public static void LoadOfficeQueueAccess(ListControl lst)
    {
        foreach (Office office in UserSessions.CurrentUser.OfficeAccess)
        {
            ListItem item = new ListItem(office.Name, office.OfficeID.ToString());
            lst.Items.Add(item);

            item.Selected = office.ViewQueue;
        }
    }

    public static List<string> GetSelectedOffices(ListControl lst)
    {
        List<string> officeAccess = new List<string>();

        //i'm adding 0 because if the user wanted to be smartass and uncheck all offices, 
        //he should see nothing and this zero will show them nada
        officeAccess.Add("0");

        foreach (ListItem item in lst.Items)
        {
            if (item.Selected)
                officeAccess.Add(item.Value);
        }

        return officeAccess;
    }

    public static void LoadOffices(ListControl lst, bool IsSearchForm)
    {
        Hashtable prms = new Hashtable();

        if (m_Offices == null)
        {
            DataUser data = DataAccess.DataUserDao;
            m_Offices = data.GetOfficeLocations(prms);
        }

        lst.Items.Clear();

        if (IsSearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_Offices)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

    }

    public static void LoadOffices(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        if (m_Offices == null)
        {
            DataUser data = DataAccess.DataUserDao;
            m_Offices = data.GetOfficeLocations(prms);
        }

        lst.Items.Clear();

        foreach (GenericListItem item in m_Offices)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadLegalEntities(ListControl lst)
    {
        Hashtable prms = new Hashtable();

        if (m_LegalEntities == null)
        {
            m_LegalEntities = DataAccess.DataMerchantAppDao.GetLegalEntities();
        }

        lst.Items.Clear();
        lst.Items.Add(new ListItem("--Select--", "0"));

        foreach (GenericListItem item in m_LegalEntities)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadCurrencyCodes(DropDownList lst, bool IsSearchForm)
    {
        if (m_CurrencyCodes == null)
        {
            m_CurrencyCodes = DataMerchantApp.GetInstance().GetCurrencyCodes(new Hashtable { { "@ZeusVisible", true } });
        }

        lst.Items.Clear();
        lst.Items.Add(new ListItem((IsSearchForm) ? "All" : "--Select--", ""));
        m_CurrencyCodes.ToList().ForEach(x => lst.Items.Add(new ListItem(x.ItemValue + ": " + x.ItemText, x.ItemValue)));
    }

    public static void LoadChargebackPrograms(DropDownList lst, string cardTypeId)
    {
        lst.Items.Clear();
        lst.Items.Add("-- Select --");

        List<GenericListItem> cbPrograms = DataRisk.GetInstance().GetChargebackPrograms(cardTypeId);

        foreach (GenericListItem item in cbPrograms)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadLeadReps(DropDownList lst, bool SearchForm)
    {

        string UserUID = string.Empty;
        List<LeadSourceRep> m_LeadRepTemp = new List<LeadSourceRep>();


        if (m_LeadReps == null)
        {
            Hashtable prms = new Hashtable();

            DataLead data = DataAccess.DataLeadDao;

            m_LeadReps = data.GetLeadSourceReps(0);

        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        User user = UserSessions.CurrentUser;
        int userid = int.Parse(user.UserID);

        if (user.IsAgent)
            m_LeadRepTemp = m_LeadReps.Where<LeadSourceRep>(p => p.ParentUID.ToUpper() == user.UID.ToUpper() || p.UserID == userid).ToList<LeadSourceRep>();
        else
            m_LeadRepTemp = m_LeadReps;

        foreach (LeadSourceRep item in m_LeadRepTemp)
        {
            lst.Items.Add(new ListItem(item.FullName, item.UserID.ToString()));
        }
    }

    public static void LoadLeadOrigin(ListControl lst, bool SearchForm)
    {
        if (m_LeadOrigin == null)
        {
            m_LeadOrigin = DataAccess.DataLeadDao.GetLeadOrigin();
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_LeadOrigin)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static void LoadLeadClosureCodes(ListControl lst, bool SearchForm)
    {
        if (m_LeadClosureCodes == null)
        {
            m_LeadClosureCodes = DataAccess.DataLeadDao.GetLeadClosureCodes();
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (GenericListItem item in m_LeadClosureCodes)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }

    public static List<UWHeirarchyApprovalLimit> LoadUWHeirarchyApprovalLimit()
    {
        if (m_UWHeirarchyApprovalLimit == null)
        {
            DataUnderwritng data = DataAccess.DataUnderwritingDao;
            DataTable dt = data.GetUWHeirarchyApprovalLimit();

            //Convert Datatable to a List
            m_UWHeirarchyApprovalLimit = dt.AsEnumerable().Select(m => new UWHeirarchyApprovalLimit()
            {
                HeirarchyApprovalLimitID = m.Field<int>("HeirarchyApprovalLimitID"),
                IsMCCRestrictedIndustry = m.Field<bool>("IsMCCRestrictedIndustry"),
                AMVLowerLimit = m.Field<decimal>("AMVLowerLimit"),
                AMVUpperLimit = m.Field<decimal>("AMVUpperLimit"),
                Role = m.Field<string>("Role"),
                Descreption = m.Field<string>("Descreption"),
                EmailTo = m.Field<string>("EmailTo"),
                CCTo = m.Field<string>("CCTo"),
                AuthorizedUsers = LoadUWHeirarchyApprovalUsers(m.Field<int>("HeirarchyApprovalLimitID")),
            }).ToList();
        }

        return m_UWHeirarchyApprovalLimit;
    }

    private static List<string> LoadUWHeirarchyApprovalUsers(int HeirarchyApprovalLimitID)
    {
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        DataTable dt = data.GetUWHeirarchyApprovalUsers(HeirarchyApprovalLimitID);

        List<string> list = dt.AsEnumerable()
                           .Select(r => r.Field<string>("UserName").ToUpper())
                           .ToList();

        return list;
    }

    public static List<TicketTemplate> GetTicketTemplates()
    {
        if (m_TicketTemplates == null)
        {
            DataTicket data = DataAccess.DataTicketDao;
            DataTable dt = data.GetTicketTemplates(null);

            //Convert Active Ticket Templates to List
            m_TicketTemplates = dt.AsEnumerable().Select(m => new TicketTemplate()
            {
                TicketTemplateID = m.Field<int>("TicketTemplateID"),
                TicketTemplateName = m.Field<string>("TicketTemplateName"),
                Description = m.Field<string>("Description"),
                DepartmentID = m.Field<int>("DepartmentID"),
                CategoryID = m.Field<int>("CategoryID"),
                SubCategoryID = m.Field<int>("SubCategoryID"),
                DueDays = m.Field<int>("DueDays"),
                OfficeID = m.Field<int>("OfficeID"),
                IsActive = m.Field<bool>("IsActive"),
                Issue = m.Field<string>("Issue"),
            }).ToList();
        }

        return m_TicketTemplates;
    }

    public static void LoadTicketTemplates(ListControl lst, bool SearchForm)
    {
        GetTicketTemplates();

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (TicketTemplate item in m_TicketTemplates)
        {
            if (item.IsActive == true)
            {
                lst.Items.Add(new ListItem(item.TicketTemplateName, item.TicketTemplateID.ToString()));
            }
        }
    }

    public static void LoadChangeHistoryFields(ListControl lst, bool SearchForm, ChangeHistoryFields.ChangeHistoryFieldSource ChangeHistoryFieldSource)
    {
        if (m_ChangeHistoryFields == null)
        {
            m_ChangeHistoryFields = DataChangeLogs.GetChangeHistoryFields();
            List<ChangeHistoryFields> list = new List<ChangeHistoryFields>();
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", "-1"));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        string sourcename = System.Enum.GetName(typeof(ChangeHistoryFields.ChangeHistoryFieldSource), ChangeHistoryFieldSource);

        foreach (ChangeHistoryFields item in m_ChangeHistoryFields.Where(x => x.Source == sourcename))
        {
            if (item.ChangeHistoryField.ToString() == "Tangible Trial" || item.ChangeHistoryField.ToString() == "Nutra Trial")
            {
                if (!UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {
                    item.ChangeHistoryField = "Nutra Trial";
                    item.Description = "Nutra Trial";
                }
                else
                {
                    item.ChangeHistoryField = "Tangible Trial";
                    item.Description = "Tangible Trial";
                }
            }
            lst.Items.Add(new ListItem(item.Description, item.ChangeHistoryFieldID.ToString()));
        }
    }


  internal static void LoadNMIVendors(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        //Niranjan: PXP-8642
        IList<CRM> _mCRMList = new List<CRM>();
        DataApp data = new DataApp();
        _mCRMList = data.GetCRMList(prms);
        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "-1"));

        foreach (var item in _mCRMList)
        {
            lst.Items.Add(new ListItem(item.Name, item.CRMID.ToString()));
        }
    }

    //Abarua PXP-8896
  internal static void LoadNMIVendors(ListControl lst, bool SearchForm ,bool OnlyTwo)
  {
      Hashtable prms = new Hashtable();
      IList<CRM> _mCRMList = new List<CRM>();
      DataApp data = new DataApp();
      _mCRMList = data.GetCRMList(prms);
      if (OnlyTwo)
      {
          _mCRMList = _mCRMList.Where(x => x.EnableConsentAPI).ToList();
      }
      lst.Items.Clear();

      if (SearchForm)
          lst.Items.Add(new ListItem("All", string.Empty));
      else
          lst.Items.Add(new ListItem("--Select--", "-1"));

      foreach (var item in _mCRMList)
      {
        lst.Items.Add(new ListItem(item.Name, item.CRMID.ToString()));
      }
  }
    //Niranjan PXP-8045
   public static void LoadDeployType(ListControl lst, bool SearchForm)
    {
        Hashtable prms = new Hashtable();
        if (m_DeployTypeID == null)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            m_DeployTypeID = data.GetDeployType(prms);
        }

        lst.Items.Clear();

        if (SearchForm)
            lst.Items.Add(new ListItem("All", string.Empty));
        else
            lst.Items.Add(new ListItem("--Select--", "0"));

        foreach (GenericListItem item in m_DeployTypeID)
        {
            lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }
    }
   //PXP-8237 sanidhya start
   /// <summary>
   /// Load  CRM list from Database
   /// </summary>
   /// <param name="_activeCRMList"></param>
   public static void LoadCRMList(ListControl _cRMList)
   {

       Hashtable prms = new Hashtable();
       DataApp data = new DataApp();
       IList<CRM> _mCRMList = new List<CRM>();
       _mCRMList = data.GetCRMList(prms)
             .Where(x => x.Type.ToUpper() == Constant.CRM_Type).ToList();
       _cRMList.DataSource = _mCRMList;
       _cRMList.DataTextField = "Name";
       _cRMList.DataValueField = "CRMID"; //DM-2403 Emanuel
        _cRMList.DataBind();
   }
    //PXP-8237 sanidhya end

   //START:Added by Abarua for PXP-7866
    public static void LoadNmiAffiliatePlanIdList(ListControl planId, bool SearchForm=false)
   {

       Hashtable prms = new Hashtable();
       DataMerchantApp data = DataAccess.DataMerchantAppDao;
       IList<NmiAffiliatePlanId> plans = new List<NmiAffiliatePlanId>();
       plans = data.GetNmiAffiliatePlanIdList(prms);
       planId.Items.Clear();
       planId.DataSource = plans;
       planId.DataTextField = "PlanId";
       planId.DataValueField = "Id";
       planId.DataBind();       

       if (SearchForm)
           planId.Items.Insert(0,new ListItem("All", "-1"));
       else
           planId.Items.Insert(0, new ListItem("--Select--", "-1"));
   }
    //END:Added by Abarua for PXP-7866
   /// <summary>
   /// PXP-8431
   /// </summary>
   /// <param name="lst"></param>
   /// <param name="SearchForm"></param>
   /// <param name="prms"></param>
   internal static void LoadRelationShipsRecordIds(ListControl lst, bool SearchForm, string MerchantAppUID)
   {
       List<int> lstRrIds = DataChangeLogs.GetRelationShipsRecordIds(new Hashtable { { "@MerchantAppUID", MerchantAppUID } });

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       //string sourcename = System.Enum.GetName(typeof(ChangeHistoryFields.ChangeHistoryFieldSource), ChangeHistoryFieldSource);

       foreach (int item in lstRrIds)
       {
           lst.Items.Add(new ListItem(item.ToString()));
       }
   }
   /// <summary>
   /// PXP-8430
   /// </summary>
   /// <param name="lst"></param>
   /// <param name="SearchForm"></param>
   /// <param name="prms"></param>
   internal static void LoadEquipmentsRecordIds(ListControl lst, bool SearchForm, string MerchantAppUID)
   {
       List<int> lstErIds = DataChangeLogs.GetEquipmentsRecordIds(new Hashtable { { "@MerchantAppUID", MerchantAppUID } });

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (int item in lstErIds)
       {
           lst.Items.Add(new ListItem(item.ToString()));
       }
   }
   //Amit PXP-11701
   public static void LoadbillingMethod(ListControl lst, bool SearchForm)
   {
       Hashtable prms = new Hashtable();
       if (m_BillingMethod == null)
       {
           DataMerchantApp data = DataAccess.DataMerchantAppDao;
           m_BillingMethod = data.GetbillingMethod(prms);
       }

       lst.Items.Clear();

       foreach (GenericListItem item in m_BillingMethod)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }

   //Start Code added for PXP-12596
   public static void LoadChannelSalesManager(ListControl lst)
   {
       Hashtable prms = new Hashtable();

       if (m_ChannelSalesManager == null)
       {
           m_ChannelSalesManager = DataAccess.DataMerchantAppDao.GetChannelSalesManager(prms);
       }

       lst.Items.Clear();
       lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_ChannelSalesManager)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }
    //End Code added for PXP-12596

    //Start Code added for PXP-14238
   public static IList<GenericListItem> LoadTicketApprovalManager()
   {
       Hashtable prms = new Hashtable();
           
       m_TicketApprovalManager = DataAccess.DataMerchantAppDao.GetTicketApprovalManager(prms);
       
       return m_TicketApprovalManager;
   }
    //End code added for PXP-14238

   //Ani: PXP-17275 - Zeus - Add Tab & Page in Zeus for Quality-- AgentAllocation
   public static IList<GenericListItem> LoadAgentSourceCodes(ListControl lst, bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_AgentSourceCodes == null)
       {
           m_AgentSourceCodes = DataAccess.DataAgentDao.GetAgentSourceCodes_List();
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_AgentSourceCodes)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }

       return m_AgentSourceCodes;
   }

   //Start Code added for DM-107

   public static void LoadQAAppErrorOccurredStage(ListControl lst,bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QAAppErrorOccurredStage == null)
       {
           m_QAAppErrorOccurredStage = DataAccess.DataMerchantAppDao.GetQAAppErrorOccurredStage(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_QAAppErrorOccurredStage)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }

   public static void LoadQAAppErrorFoundBy(ListControl lst,bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QAAppErrorFoundBy == null)
       {
           m_QAAppErrorFoundBy = DataAccess.DataMerchantAppDao.GetQAAppErrorFoundBy(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));


       foreach (GenericListItem item in m_QAAppErrorFoundBy)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }
   public static void LoadQAAppDepartment(ListControl lst,bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QAAppDepartment == null)
       {
           m_QAAppDepartment = DataAccess.DataMerchantAppDao.GetQAAppDepartment(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_QAAppDepartment)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }
   public static void LoadQAAppCategory(ListControl lst,bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QAAppCategory == null)
       {
           m_QAAppCategory = DataAccess.DataMerchantAppDao.GetQAAppCategory(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_QAAppCategory)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }
   public static void LoadQAAppSubCategory(ListControl lst,bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QAAppSubCategory == null)
       {
           m_QAAppSubCategory = DataAccess.DataMerchantAppDao.GetQAAppSubCategory(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));


       foreach (GenericListItem item in m_QAAppSubCategory)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }

    //End Code added for DM-107

    //Start code for PXP-18162
   public static void LoadQATicketCategory(ListControl lst, bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QATicketCategory == null)
       {
           m_QATicketCategory = DataAccess.DataMerchantAppDao.GetQATicketCategory(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_QATicketCategory)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }
   public static void LoadQATicketSubCategory(ListControl lst, bool SearchForm)
   {
       Hashtable prms = new Hashtable();

       if (m_QATicketSubCategory == null)
       {
           m_QATicketSubCategory = DataAccess.DataMerchantAppDao.GetQATicketSubCategory(prms);
       }

       lst.Items.Clear();

       if (SearchForm)
           lst.Items.Add(new ListItem("All", "-1"));
       else
           lst.Items.Add(new ListItem("--Select--", "-1"));

       foreach (GenericListItem item in m_QATicketSubCategory)
       {
           lst.Items.Add(new ListItem(item.ItemText, item.ItemValue));
       }
   }
    //End code for PXP-18162

    //Ani: DM-5686
    /// <summary>
    /// Load  Vendor list from Database
    /// </summary>
    public static void LoadVendors(ListControl _vendorList)
    {

        Hashtable prms = new Hashtable();
        DataApp data = new DataApp();
        IList<Vendor> vdrList = new List<Vendor>();
        vdrList = data.GetVendorList(prms);
        _vendorList.DataSource = vdrList;
        _vendorList.DataTextField = "Name";
        _vendorList.DataValueField = "Id"; 
        _vendorList.DataBind();
    }

    //Ani: DM-5686
    /// <summary>
    /// Returns VendorID associated with the give MerchantID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static int GetSelectedVendor(string id)
    {        
        Hashtable prms = new Hashtable();
        DataApp data = new DataApp();
        prms.Add("@MerchantID", id);
        int selectedVendorID = data.GetSelectedVendor(prms);
        return selectedVendorID ;
    }

    //raul: DM-6094
    public static void LoadAgentAllocationStatus(ListControl listControl, bool SearchForm)
    {

        Hashtable prms = new Hashtable();
        DataApp data = new DataApp();
        IList<GenericListItem> raList = new List<GenericListItem>();
        raList = data.GetAgentAllocationStatusLookup(prms);

        listControl.Items.Clear();

        var item = new ListItem("All", "-1");
        item.Selected = true;
        if (SearchForm)
        {
            listControl.Items.Add(item);
        }
        else
        {
            item.Text = "--Select--";
            listControl.Items.Add(item);
        }

        foreach (GenericListItem rsItem in raList)
        {
            listControl.Items.Add(new ListItem(rsItem.ItemText, rsItem.ItemValue));
        }
        listControl.DataBind();
    }
}
