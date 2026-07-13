using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Data;
using System.Text;
using System.Collections.Generic;

/// <summary>
/// Summary description for AjaxWebservice
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AjaxWebservice : System.Web.Services.WebService
{

    public AjaxWebservice()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()  
    {
        return "Hello World";
    }

    [WebMethod]
    public SelectAgent GetAgent(int id, string AgentSelectorMasterAgentUID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ID", id);

        if (!string.IsNullOrEmpty(AgentSelectorMasterAgentUID))
            prms.Add("@AgentUID", AgentSelectorMasterAgentUID);

        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.IsInternal)
            prms["@InternalUserUID"] = UserSessions.CurrentUser.UID;

        SelectAgent retAgent = new SelectAgent();

        Agent agent = DataAccess.DataAgentDao.GetAgentLight(prms);

        if (agent != null)
        {
            retAgent.AgentDBA = agent.AgentDBA;
            retAgent.AgentUID = agent.AgentUID;
        }

        return retAgent;
    }


    [WebMethod]
    public List<string> GetMerchantDBA(string BusinessDBANameFragment)
    {

        BusinessDBANameFragment = (BusinessDBANameFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (BusinessDBANameFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@BusinessDBAName", BusinessDBANameFragment);
            prms.Add("@PageSize", 10);
            prms.Add("@CurrentPage", 1);
            prms.Add("@SortOrder", null);
            prms.Add("@SortDirection", 0);

            DataTable dt = DataMerchantAppPaging.GetMerchantAppsPaging(prms, 10, 1, "myid");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}:{1}", dr["ID"].ToString(), dr["BusinessDBAName"].ToString().Replace(":", " ")));
                }
            }
        }

        return li;
    }

    [WebMethod]
    public List<string> GetMerchantLegal(string BusinessLegalNameFragment)
    {

        BusinessLegalNameFragment = (BusinessLegalNameFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (BusinessLegalNameFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@BusinessLegalName", BusinessLegalNameFragment);
            prms.Add("@PageSize", 10);
            prms.Add("@CurrentPage", 1);
            prms.Add("@SortOrder", null);
            prms.Add("@SortDirection", 0);

            DataTable dt = DataMerchantAppPaging.GetMerchantAppsPaging(prms, 10, 1, "myid");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}:{1}", dr["ID"].ToString(), dr["BusinessLegalName"].ToString().Replace(":", " ")));
                }
            }
        }

        return li;
    }


    [WebMethod]
    public List<string> GetAgentDBA(string BusinessDBANameFragment)
    {

        BusinessDBANameFragment = (BusinessDBANameFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (BusinessDBANameFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@DBA", BusinessDBANameFragment);
            prms.Add("@PageSize", 10);
            prms.Add("@CurrentPage", 1);
            prms.Add("@SortOrder", null);
            prms.Add("@SortDirection", 0);

            DataTable dt = DataMerchantAppPaging.GetAgentsPaging(prms, 10, 1, "myid");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}:{1}:{2}", dr["ID"].ToString(), dr["DBA"].ToString().Replace(":", " "), dr["AgentID"].ToString()));
                }
            }
        }

        return li;
    }

    [WebMethod]
    public List<string> GetAgentFirst(string FirstFragment)
    {

        FirstFragment = (FirstFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (FirstFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@FirstName", FirstFragment);
            prms.Add("@PageSize", 10);
            prms.Add("@CurrentPage", 1);
            prms.Add("@SortOrder", null);
            prms.Add("@SortDirection", 0);

            DataTable dt = DataMerchantAppPaging.GetAgentsPaging(prms, 10, 1, "myid");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}:{1}:{2}", dr["ID"].ToString(), dr["FirstName"].ToString().Replace(":", " "), dr["AgentID"].ToString()));
                }
            }
        }

        return li;
    }

    [WebMethod]
    public List<string> GetAgentLast(string LastFragment)
    {

        LastFragment = (LastFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (LastFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@LastName", LastFragment);
            prms.Add("@PageSize", 10);
            prms.Add("@CurrentPage", 1);
            prms.Add("@SortOrder", null);
            prms.Add("@SortDirection", 0);

            DataTable dt = DataMerchantAppPaging.GetAgentsPaging(prms, 10, 1, "myid");

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}:{1}:{2}", dr["ID"].ToString(), dr["LastName"].ToString().Replace(":", " "), dr["AgentID"].ToString()));
                }
            }
        }

        return li;
    }

    [WebMethod]
    public List<string> GetEquipment(string ModelFragment)
    {

        ModelFragment = (ModelFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (ModelFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@Model", ModelFragment);
            
            DataSet ds = DataEquipment.GetInstance().GetEquipmentItems(prms);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    string myEMVCompliance=string.Empty;
                    string myUID = dr["UID"].ToString();
                    string myEquipmentType = dr["EquipmentType"].ToString().Trim().Replace("|", "");
                    string myEquipmentMaker = dr["EquipmentMaker"].ToString().Trim().Replace("|", "");
                    string myMakerModel = dr["Model"].ToString().Trim().Replace("|", "");
                    string myID = dr["ID"].ToString().Trim().Replace("|", "");
                    if (string.IsNullOrEmpty(Convert.ToString(dr["EMVCompliance"])))
                    {
                        myEMVCompliance = "";
                    }
                    else if (Convert.ToBoolean(dr["EMVCompliance"]) == true)
                    {
                        myEMVCompliance = "1";
                    }
                    else if (Convert.ToBoolean(dr["EMVCompliance"]) == false)
                    {
                        myEMVCompliance = "0";
                    }


                    li.Add(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", myUID, myEquipmentType, myEquipmentMaker, myMakerModel, myID, myEMVCompliance));
                }
            }
        }

        return li;
    }

    //:START: Added Web Method to autocomplete TPP Name PXP-8417 By Ali Khan
    [WebMethod]
    public List<string> GetTPPName(string Type, string TPPNameFragment)
    {
        TPPNameFragment = (TPPNameFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (TPPNameFragment != "")
        {
            Hashtable prms = new Hashtable();
            prms.Add("@TPPName", TPPNameFragment);
            prms.Add("@Type", Type);
            
            DataTable dt = DataEquipment.GetInstance().GetTPPName(prms);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}", dr["Name"].ToString()));
                }
            }
        }
        return li;
    }
    //:END: Added Web Method to autocomplete TPP Name PXP-8417 By Ali Khan

    // PXP-8408 By Sanidhya
    [WebMethod]
    public List<string> GetCRMName(string CRMNameFragment)
    {
        CRMNameFragment = (CRMNameFragment ?? "").Trim();

        List<string> li = new List<string>();

        if (!String.IsNullOrEmpty(CRMNameFragment))
        {
            Hashtable prms = new Hashtable();
            prms.Add("@TPPName", CRMNameFragment);

            DataTable dt = DataEquipment.GetInstance().GetTPPName(prms);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    li.Add(string.Format("{0}", dr["Name"].ToString()));
                }
            }
        }
        return li;
    }
    //:END:PXP-8408 By Sanidhya

    public class SelectAgent
    {
        public string AgentDBA
        {
            get;
            set;
        }

        public string AgentUID
        {
            get;
            set;
        }
    }
}

