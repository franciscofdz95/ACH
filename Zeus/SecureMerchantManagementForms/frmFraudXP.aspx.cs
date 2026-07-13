using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using Infragistics.Web.UI.EditorControls;

public partial class frmFraudXP : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (!this.IsPostBack)
        {

            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.FraudXP);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "FraudXP");
            }

            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            //LookupTableHandler.LoadDeviceResults(DeviceCheckOptions);
            //LookupTableHandler.LoadIPGeoResults(lstDeclineCustPhoneInBillingLocOptions);
            //LookupTableHandler.LoadIPGeoResults(lstDeclineShipForwardOptions);
            //this.LoadTermConditions();
            this.FormShow(this.UID);
        }
    }

    //public void LoadTermConditions()
    //{

    //    Hashtable prms = new Hashtable();
    //    IList<TermsCondition> list = DataAccess.DataRiskDao.GetTermsConditions(prms);

    //    TCID.Items.Clear();

    //    TCID.Items.Add(new ListItem("Select", "-1"));

    //    foreach (TermsCondition item in list)
    //    {
    //        TCID.Items.Add(new ListItem(item.Name, item.TCID));
    //    }
    //}


    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();  
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        WucBusinessInfo1.pnlInfo.Enabled = false;


        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status        
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        FormHandler.SetControlEditMode(WucWebsiteMonitoring1, true);
        FormHandler.SetControlEditMode(UpdatePanel5, true);        

        CheckBox EnableWebsiteMonitoring = (CheckBox)WucWebsiteMonitoring1.FindControl("EnableWebsiteMonitoring");
        EnableWebsiteMonitoring.Enabled = this.EditMode;
        CheckBox URLReferrerValidate = (CheckBox)WucWebsiteMonitoring1.FindControl("URLReferrerValidate");
        URLReferrerValidate.Enabled = this.EditMode;

        pnlIPGeoHighTicketThreshold.Visible = IPGeoCheckOptions.SelectedItem.Value == "H";
        pnlDeviceHighTicketThreshold.Visible = DeviceCheckOptions.SelectedItem.Value == "H";

        WucWebsiteMonitoring1.LoadWebsiteURLs();
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            MasterPageMerchant master = (MasterPageMerchant)this.Master;
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);

        }/******** End of PXP-2206 **************/  
    }
    
    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        try
        {
            this.UpdateMerchantAppTransDB();

            this.EditMode = false;
            this.FormShow(this.UID);

            this.ToggleButtons();

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public bool UpdateMerchantAppTransDB()
    {
        try
        {
            DataRisk data = DataAccess.DataRiskDao;
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID",UserSessions.CurrentMerchantApp.MerchantAppUID);
            prms.Add("@FraudXPOn", FraudXPOn.Checked);
            prms.Add("@HighTicketDeviceThreshold", HighTicketDeviceThreshold.Text);

            if (DeviceCheckOptions.SelectedIndex == -1)
                prms.Add("@DeviceCheckOptions", "N");
            else
                prms.Add("@DeviceCheckOptions", DeviceCheckOptions.SelectedItem.Value);

            //transaction source
            CheckBox URLReferrerValidate = (CheckBox)WucWebsiteMonitoring1.FindControl("URLReferrerValidate");
            prms.Add("@URLReferrerValidate", URLReferrerValidate.Checked);
            prms.Add("@IPGeoCheckOptions", IPGeoCheckOptions.SelectedItem.Value);          
            prms.Add("@HighTicketIPGeoThreshold", HighTicketIPGeoThreshold.Text);          

            CheckBox EnableWebsiteMonitoring = (CheckBox)WucWebsiteMonitoring1.FindControl("EnableWebsiteMonitoring");
            prms.Add("@EnableWebsiteMonitoring", EnableWebsiteMonitoring.Checked);            
         
            data.UpdateMerchantAppTransDB(prms);
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
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
        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            //lblError.Text = message;
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
                this.FormSave();
                break;
            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                this.FormShow(this.UID);

                break;
            case "Cancel":
                if (this.UID != string.Empty)
                    this.FormCancel();

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
            //case "<<":
            //    FormHandler.RecordNav(RecordNavigation.First, this, UserSessions.SearchResultsDataView, false);
            //    break;
            //case "<":
            //    FormHandler.RecordNav(RecordNavigation.Previous, this, UserSessions.SearchResultsDataView, false);
            //    break;
            //case ">":
            //    FormHandler.RecordNav(RecordNavigation.Next, this, UserSessions.SearchResultsDataView, false);
            //    break;
            //case ">>":
            //    FormHandler.RecordNav(RecordNavigation.Last, this, UserSessions.SearchResultsDataView, false);
            //    break;
        }
    }
    
    public string GetSelectedListItems( ListControl lst)
    {
        string selected = string.Empty;
        foreach (ListItem item in lst.Items)
        {
            if (item.Selected)
                selected += item.Value;
        }
        return selected;
    }

    public string SetSelectedListItems(ListControl lst,string selected)
    {
        for (int i = 0; i < selected.Length;i++ )
        {
            ListItem item = lst.Items.FindByValue(selected.Substring(i, 1));
            if (item != null)
                item.Selected = true;
        }
        return selected;
    }

    protected void grdRisk_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                WebNumericEditor lblPayout = (WebNumericEditor)e.Row.FindControl("WebNumericEditor");
                lblPayout.Text = DataBinder.Eval(e.Row.DataItem, "Threshold").ToString();

                CheckBox chkEnable = (CheckBox)e.Row.FindControl("chkEnabled");
                chkEnable.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Enable").ToString());

                break;
            default:
                break;
        }
    }

    protected void DeviceCheckOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlDeviceHighTicketThreshold.Visible = DeviceCheckOptions.SelectedItem.Value == "H";

        if (DeviceCheckOptions.SelectedItem.Value == "H" && (string.IsNullOrEmpty(HighTicketDeviceThreshold.Text) || HighTicketDeviceThreshold.Text == "0"))
            HighTicketDeviceThreshold.Text = Convert.ToInt32(UserSessions.CurrentMerchantApp.TinfoHighestTicketAmount).ToString();
    }
    
    protected void IPGeoCheckOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlIPGeoHighTicketThreshold.Visible = IPGeoCheckOptions.SelectedItem.Value == "H";

        if (IPGeoCheckOptions.SelectedItem.Value == "H" && (string.IsNullOrEmpty(IPGeoCheckOptions.Text) || HighTicketIPGeoThreshold.Text == "0"))
            HighTicketIPGeoThreshold.Text = Convert.ToInt32(UserSessions.CurrentMerchantApp.TinfoHighestTicketAmount).ToString();
    }
}
