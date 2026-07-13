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
using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;

public partial class frmMerchantNotes : frmBaseDataEntry
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

        WucBusinessInfo1.pnlInfo.Enabled = false;

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Notes);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Notes");
            }

            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            this.FormShow(this.UID);
            WucNotes1.LoadNotes(this.UID, "");
        }
    }

 
    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();        
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, pnlDetail);

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);
        
        ((HiddenField)WucNotes1.FindControl("hdnAgent")).Value = agreement.AgentEmail;
        ((HiddenField)WucNotes1.FindControl("hdnMerchant")).Value = agreement.BusinessEmailAddress;

        WucBusinessInfo1.LoadOffice(agreement);
        
        
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
        return true;
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
