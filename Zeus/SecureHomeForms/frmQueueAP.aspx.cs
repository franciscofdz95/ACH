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

public partial class frmQueueAP : frmBaseDataEntry
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
            this.Master.SideMenuSelect(MasterPageHome.eMasterSideMenu.ApplicationProcessing);

            LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess); 
            LoadQueues();
            //this.LoadMerchantAppByStatus("", pnlConditional, "CONDITIONAL APPROVALS");
        }
    }

    public void LoadQueues()
    {
        this.LoadMerchantAppByStatus("d96ec87c-ccb0-4c88-b9b8-2b497ba6e409", pnlReceived, "RECEIVED");
        this.LoadMerchantAppByStatus("f5faf4fe-a132-45f6-a854-8ccfb07aa8d9", pnlPending, "APP INCOMPLETE");
        this.LoadMerchantAppByStatus("4C315DAA-B1B3-47F0-89F3-EF09A5522BE6", pnlQA, "QA");
        //Code added for PXP-10755[Add new queue as Draft below App-Incompelet]by koshlendra start
        this.LoadMerchantAppByStatus("BF168EDA-B741-40FC-9213-7FD83D35491E", pnlDraft, "DRAFT");
        //Code added for PXP-10755[Add new queue as Draft below App-Incompelet]by koshlendra end
        this.LoadMerchantAppByStatusByDate("50EF0729-906F-4CFE-96D8-01129DAFEC08", pnlWithdrawn, "WITHDRAWN LAST 30 DAYS");            
    }

    private void LoadMerchantAppByStatusByDate(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.SetDataSource(status, Title, officeAccess, DateTime.Today.AddMonths(-1));
    }

    private void LoadMerchantAppByStatus(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.SetDataSource(status, Title, officeAccess);
    }

    protected void lstOfficeAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadQueues();
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
