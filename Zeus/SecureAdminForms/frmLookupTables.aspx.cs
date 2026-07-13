using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PaymentXP.DataObjects;

public partial class frmLookupTables : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageAdmin.eMasterSideMenu.LookupTables);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {

        LookupTableHandler.m_Banks = null;
        LookupTableHandler.m_Users = null;
        LookupTableHandler.m_FrontEnds = null;
        LookupTableHandler.m_BackEnds = null;
        LookupTableHandler.m_BusinessStructures = null;
        LookupTableHandler.m_ReasonChanges = null;
        LookupTableHandler.m_ReturnPolicies = null;
        LookupTableHandler.m_ApplicationTypes = null;
        LookupTableHandler.m_PrivateLabels = null;
        LookupTableHandler.m_GatewayMode = null;
        LookupTableHandler.m_CCStatus = null;
        LookupTableHandler.m_AVSResults = null;
        LookupTableHandler.m_CVV2Results = null;
        LookupTableHandler.m_DeviceResults = null;
        LookupTableHandler.m_IPGeoResults = null;
        LookupTableHandler.m_PCIVEndors = null;
        LookupTableHandler.m_TicketCategoriesDS = null;
        LookupTableHandler.m_InvoiceCategories = null;
        LookupTableHandler.m_MerchantClass = null;
        LookupTableHandler.m_NewBanks = null;
        LookupTableHandler.m_AccountGroups = null;
        LookupTableHandler.m_LegalEntities = null;
        LookupTableHandler.m_Offices = null;
        LookupTableHandler.m_PaymentSchedule = null;
        LookupTableHandler.m_PaymentFrequency = null;
        LookupTableHandler.m_CountryCodes = null;
        LookupTableHandler.m_UWHeirarchyApprovalLimit = null;
        LookupTableHandler.m_TicketTemplates = null;
        LookupTableHandler.m_ChangeHistoryFields = null;
        LookupTableHandler.m_AgentLevels = null;
        LookupTableHandler.m_RequestedByUsers = null;
        LookupTableHandler.m_SchedueATypeListItems = null; 

        //leads
        LookupTableHandler.m_LeadStatus = null;
        LookupTableHandler.m_LeadSource = null;
        LookupTableHandler.m_States = null;
        LookupTableHandler.m_FollowupStatus = null;
        LookupTableHandler.m_TimeZones = null;
        LookupTableHandler.m_LeadServices = null;
        LookupTableHandler.m_CurrencyCodes = null;

        LookupTableHandler.m_ActiveUsers = null;
        LookupTableHandler.m_Channels = null;
        LookupTableHandler.m_MDocGroupType = null;
        LookupTableHandler.m_UsersByRole = null;
        LookupTableHandler.m_UserTimeZones = null;
        LookupTableHandler.m_LeadReps = null;
        LookupTableHandler.m_LeadOrigin = null;
        LookupTableHandler.m_LeadClosureCodes = null;
        LookupTableHandler.m_Vendors = null;

        //Reset counter to zero
        LookupTableHandler.m_AgentIDSpecificAppCounter = 0;

        //Clear the EmailTemplates when new templates are added.
        DataMPSEmailTemplate.ClearEmailTemplates();
    }



    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }
}
