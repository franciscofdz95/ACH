using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

public partial class frmMerchantAlerts : frmBaseDataEntry
{
    //protected override void OnPreInit(EventArgs e)
    //{
    //    base.OnPreInit(e);
    //    // Set Property to Store ViewState on Server
    //    base.StoreViewStateOnServer = true;
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Alerts);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Merchant Alerts");
            }

            this.FormShow("");
        }
    }

  

    public override void FormShow(string ID)
    {
        wucAlerts1.EditMode = this.EditMode;
        wucAlerts1.LoadGrid(eControlContactType.Merchant);
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            MasterPageMerchant master = (MasterPageMerchant)this.Master;
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }
        /******** End of PXP-2206 **************/
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        wucAlerts1.FormSave(eControlContactType.Merchant);

        this.EditMode = false;
        this.ToggleButtons();

        return true;
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
        return wucAlerts1.ValidateConfiguration();
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        
        this.Master.ToggleMenu(!this.EditMode);
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Add":
                this.FormNew();
                break;

            case "Save":
                if (this.FormSave())
                {
                    this.FormShow(this.UID);
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Close":
                break;

            case "Delete":
                break;

            case "Edit":
                {                    
                    this.EditMode = true;
                    this.FormShow(this.UID);
                    //Modified by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
                    string firstUserdetail = MerchantFacade.GetFirstUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID);
                    if (!string.IsNullOrWhiteSpace(firstUserdetail))
                    {
                        string notification = firstUserdetail + " is currently editing this ZID. Please ensure you will not overwrite each other's work.";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Notification", "alert(" + '"' + notification + '"' + ");", true);
                        MasterPageMerchant master = (MasterPageMerchant)this.Master;
                        master.UpdateNotification(firstUserdetail + " is editing this ZID.");
                    }
                    MerchantFacade.AddNewUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
                    /******** End of PXP-2206 **************/
                    this.ToggleButtons();
                }
                break;
        }
    }
}
