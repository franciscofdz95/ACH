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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections.Generic;

public partial class frmQueueCU : frmBaseDataEntry
{

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
        pnlConditional.ValueChanged += new wucApplicationQueue.ValueChangedHandler(pnlConditional_ValueChanged);
        pnlConditional.CheckChanged += new wucApplicationQueue.CheckChangedHandler(pnlConditional_CheckChanged);
    }

    void pnlConditional_CheckChanged(object sender, EventArgs e)
    {
        this.LoadMerchantAppByStatus("", pnlConditional, "CONDITIONAL APPROVALS");
    }

    void pnlConditional_ValueChanged(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
    {
        this.LoadMerchantAppByStatus("", pnlConditional, "CONDITIONAL APPROVALS");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageHome.eMasterSideMenu.CreditUnderwriting);

            LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess); 
            LoadQueues();           
        }
    }

    public void LoadQueues()
    {
        this.LoadMerchantAppByStatus("87f2dfae-b0ec-4208-83fc-c9488393aa61", pnlReceived, "RECEIVED");
        this.LoadMerchantAppByStatus("908C113F-287F-4510-80AC-2D23FC5FA5E3", pnl3DEDecision, "3DE DECISION");
        this.LoadMerchantAppByStatus("4358b3a7-9936-448b-bee5-fc8db48fb9ff", pnlPending, "PENDING");
        this.LoadMerchantAppByStatus("b0535662-2335-4220-a67f-a729c3617159", pnlBankRequested, "BANK REQUEST");
        this.LoadMerchantAppByStatus("4358b3a7-9936-448b-bee5-fc8db48fb9ee", pnlSubmittedToBank, "SUBMITTED TO BANK");
        this.LoadMerchantAppByStatus("9a7c21b5-1fca-4e58-acb4-057a10d5a974", pnlInReview, "IN REVIEW");
        this.LoadMerchantAppByStatus("2fdda5e4-e80a-4155-8cb2-d5200992fa81", pnlApproved, "APPROVED");
        this.LoadMerchantAppByStatusByNutraCheck("2fdda5e4-e80a-4155-8cb2-d5200992fa81", pnlPendingRegistration, "PENDING REGISTRATIONS");//PXP-9308: By Sanidhya
        this.LoadMerchantAppByStatus("", pnlConditional, "CONDITIONAL APPROVALS");
        this.LoadMerchantAppByStatus("", pnlApprovalRequest, "HIERARCHY APPROVAL SIGN OFF");
        this.LoadMerchantAppByStatus("1AA190B1-C964-4636-A01E-3177E546836E", pnlReceivedPD, "RECEIVED PD");
        this.LoadMerchantAppByStatusByDate("FC8140C2-8789-4763-89E9-A86FF9153641", pnlWithdrawn, "WITHDRAWN LAST 30 DAYS");

        WucDocumentUploads1.OfficeIDList = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        WucDocumentUploads1.LoadDocuments();
    }

    protected void lstOfficeAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
        setCurrentPage();
        LoadQueues();
    }

    private void setCurrentPage()
    {
        pnl3DEDecision.CurrentPage = 1;
        pnlReceived.CurrentPage = 1;
        pnlBankRequested.CurrentPage = 1;
        pnlPending.CurrentPage = 1;
        pnlSubmittedToBank.CurrentPage = 1;
        pnlInReview.CurrentPage = 1;
        pnlApprovalRequest.CurrentPage = 1;
        pnlApproved.CurrentPage = 1;
        pnlConditional.CurrentPage = 1;
        pnlWithdrawn.CurrentPage = 1;
        pnlReceivedPD.CurrentPage = 1;
        pnlPendingRegistration.CurrentPage = 1;//PXP-9308 :by sanidhya
    }

    protected void lstVolumeLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        setCurrentPage();
        LoadQueues();
    }

    public void LoadMerchantAppByStatusByDate(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.VolumeLevel = lstVolumeLevel.SelectedValue;
        //PXP-9308:By sanidhya
        pnl.SetDataSource(status, Title, officeAccess, DateTime.Today.AddMonths(-1));
    }

    public void LoadMerchantAppByStatus(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.VolumeLevel = lstVolumeLevel.SelectedValue;
        pnl.SetDataSource(status, Title, officeAccess);
    }
    //PXP-9308:by Sanidhya
    public void LoadMerchantAppByStatusByNutraCheck(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.VolumeLevel = lstVolumeLevel.SelectedValue;
        pnl.SetDataSource(status, Title, officeAccess,DateTime.MinValue,true);
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
