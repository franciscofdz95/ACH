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

public partial class frmMerchantCategories : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null) 
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }
  
    protected void Page_Load(object sender, EventArgs e)
    {
        
        wucMerchantCategories1.pnlCat.Visible = false;

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Services);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Services");
            }

            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            this.FormShow(this.UID);
            WucBusinessInfo1.pnlInfo.Enabled = false;
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        string url = string.Empty;
        switch (btn.Text)
        {
            case "Save":

                if (this.FormSave())
                {
                    Response.Redirect(WebUtil.GetMyUrl());
                }
                break;

            case "Refresh":

                this.FormShow(this.UID);
                break;

            case "Cancel":

                if (!this.UID.Equals(string.Empty))
                {
                    this.FormCancel();
                }
                break;

            case "Edit":
                {                   
                    this.EditMode = true;
                    this.FormShow(this.UID);
                    //Modofied by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message     
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

 

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);

        DataLead objLead = DataAccess.DataLeadDao;
        Lead lead = new Lead();

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        wucMerchantCategories1.pnlGrid.Enabled = this.EditMode;
        wucMerchantCategories1.pnl.Enabled = true;
        FormHandler.SetControlEditMode(wucMerchantCategories1, true);
        wucMerchantCategories1.MerchantAppUID = agreement.MerchantAppUID;
        wucMerchantCategories1.FormServices();
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
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        if (UserSessions.CurrentMerchantApp != null)
        {
            wucMerchantCategories1.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            wucMerchantCategories1.UpdateServices();
        }

        ToggleButtons();
        return true;
    }

    public override void FormNew()
    {
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;

        //if (this.LeadID == string.Empty)
        //    message = "No Lead is associated with this merchant.";

        if (message == string.Empty)
            return true;
        else
        {
            lblError.Text = message;
            return false;
        }
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
        btnRefresh.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);
    }

}