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

public partial class frmQueueCS : frmBaseDataEntry
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
            this.Master.SideMenuSelect(MasterPageHome.eMasterSideMenu.ClientServices);

            LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess); 
            LoadQueues();
        }
    }

    public void LoadQueues()
    {
        this.LoadMerchantAppByStatus("a4ca9897-5719-4938-95f7-fa3d7a4bff5b", pnlReceived, "RECEIVED");
        this.LoadMerchantAppByStatus("f5f18138-d977-40ba-ab8c-415abb4a0760", pnlWelcomeCall, "SCHEDULED WELCOME CALL");
        this.LoadMerchantAppByStatus("7c4f0bb4-3e7f-4644-8734-263256be5ac1", pnlWelcomeCallCompleted, "CONDUCTED WELCOME CALL");
        //this.LoadMerchantAppByStatus(Constants.QUEUESTATUS_MS_INACTIVE, pnlCSInActive, "MS Inactive");
    }

    protected void lstOfficeAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadQueues();
    }

    public void LoadMerchantAppByStatus(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.SetDataSource(status, Title, officeAccess);
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
