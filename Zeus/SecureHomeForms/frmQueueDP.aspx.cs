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

public partial class frmQueueDP : frmBaseDataEntry
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
            this.Master.SideMenuSelect(MasterPageHome.eMasterSideMenu.Deployment);

            LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess); 
            LoadQueues();
        }
    }

    public void LoadQueues()
    {
        this.LoadMerchantAppByStatus(Constants.QUEUESTATUS_DP_RECEIVED_HARDWARE, pnlReceivedST, "RECEIVED_HARDWARE");
        this.LoadMerchantAppByStatus(Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE, pnlReceivedGS, "RECEIVED_SOFTWARE");
        //this.LoadMerchantAppByStatus(Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_HARDWARE, pnlSchedulingDLST, "SCHEDULE DOWNLOAD/TRAINING_HARDWARE");
        //DM-507 -Chandra
        this.LoadMerchantAppByStatus(Constants.QUEUESTATUS_DP_REVIEW, pnlSchedulingDLST, "REVIEW");
        this.LoadMerchantAppByStatus(Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_SOFTWARE, pnlSchedulingDLGS, "SCHEDULE DOWNLOAD/TRAINING_SOFTWARE");

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
