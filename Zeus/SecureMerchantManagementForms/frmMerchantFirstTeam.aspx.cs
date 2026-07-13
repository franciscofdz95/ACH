using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using CommonUtility;

public partial class frmMerchantFirstTeam : frmBaseDataEntry
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
        if (HttpContext.Current.UrlContains("frmMerchantFirstTeam.aspx"))
        {
            HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
        }
        else
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
                this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
            this.Page.Init += new EventHandler(Page_Init);

            if (!this.IsPostBack)
            {
                this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.FirstTeam);

                if (UserSessions.CurrentMerchantApp != null)
                {
                    this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Premier Services");
                }

                this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

                this.FormShow(this.UID);

                wucFTRuleFilter1.RefreshClick += new ZeusWeb.UserControls.wucFTRuleFilter.RefreshClickHandler(wucFTRuleFilter1_RefreshClick);
                wucFTRuleFilter1.SnoozeClick += new ZeusWeb.UserControls.wucFTRuleFilter.SnoozeClickHandler(wucFTRuleFilter1_SnoozeClick);
            }
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
       
    }

    void wucFTRuleFilter1_SnoozeClick(object sender, int merchantid, int mruleid)
    {
        HiddenField hfMID = (HiddenField)WebDialogWindow2.ContentPane.FindControl("hidMerchantID");

        HiddenField hfMRID = (HiddenField)WebDialogWindow2.ContentPane.FindControl("hidMRuleID");

        hfMID.Value = merchantid.ToString();
        hfMRID.Value = mruleid.ToString();


        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void wucFTRuleFilter1_RefreshClick(object sender, string str)
    {
        this.Refresh_ChanglogGrid();
    }

    protected override void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadUsersByRole(FirstTeamRepUID, true, Constants.ROLE_FIRSTTEAM);
        }

        base.OnInit(e);
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
                    this.EditMode = false;
                    this.Adding = false;
                    this.ToggleButtons();
                    url = "~/SecureMerchantManagementForms/frmMerchantFirstTeam.aspx?Adding=false";
                    url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                break;
            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                this.FormShow(this.UID);

                break;
            case "Cancel":
                this.FormCancel();

                break;
            case "Close":
                break;
            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {

    }

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);
        UserSessions.CurrentMerchantApp = agreement;
        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        WucBusinessInfo1.pnlInfo.Enabled = false;

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);


        MasterPageMerchant master = (MasterPageMerchant)this.Master;
        master.ShowNotes(agreement.UWNotes, agreement.AgentMemo, agreement.FirstTeamNotes, this.EditMode);

        WucServices1.LoadServices(UserSessions.CurrentMerchantApp.MerchantAppUID, ServiceCategories.FIRST_TEAM, 2);
        WucBusinessInfo1.LoadOffice(UserSessions.CurrentMerchantApp);

        if (agreement.FirstTeam && CommonUtility.Util.IsValidGuid(FirstTeamRepUID.SelectedValue))
        {

            WebTab1.Visible = true;

            // check to see that this merchant has an instance, if not, then create it.
            DataMRule.CreateSnapshotIfNone(Constants.ROLE_FIRSTTEAM, Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));

            this.Refresh_RulesGrid();

            this.Refresh_ChanglogGrid();

            this.Refresh_HistoryGrid();

            
        }
        else
        {
            WebTab1.Visible = false;
        }
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
       }
        /******** End of PXP-2206 **************/   
    }

    private void Refresh_RulesGrid()
    {

        // rules grid
        Hashtable prmsFilter = new Hashtable();
        prmsFilter.Add("@MerchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
        wucFTRuleFilter1.SetDataSource(prmsFilter);

    }

    private void Refresh_ChanglogGrid()
    {
        // param changelog grid
        Hashtable prmsLog = new Hashtable();
        prmsLog.Add("@ObjectNameList", "MRuleParam,MRule");
        prmsLog.Add("@UID", UserSessions.CurrentMerchantApp.MerchantAppUID);
        prmsLog.Add("@PortalUID", UserSessions.PortalUID);
        wucFTGridChangelog1.SetDataSource(prmsLog);
    }

    private void Refresh_HistoryGrid()
    {
        // history grid
        Hashtable prmsHistory = new Hashtable();
        prmsHistory.Add("@MerchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
        wucFTGridHistory1.SetDataSource(prmsHistory);
    }

    public override bool FormSave()
    {
        bool perform = false;
        DataSet dsmultiLink = new DataSet();

        try
        {
            if (!this.FormDataCheck())
                return false;

            MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
            MerchantApp clone = (MerchantApp)agreement.Clone();


            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            User user = UserSessions.CurrentUser;
            agreement.UserUpdated = user.UserName;

            string userUID = this.FirstTeamRepUID.SelectedItem.Value;

            if (!agreement.FirstTeamRepUID.ToUpper().Equals(userUID))
            {
                if (userUID == "-1")
                {
                    data.UpdateMerchantUsers(agreement.MerchantAppUID, Constants.ROLE_FIRSTTEAM, string.Empty);
                }
                else
                {
                    data.UpdateMerchantUsers(agreement.MerchantAppUID, Constants.ROLE_FIRSTTEAM, userUID.ToUpper());
                }
            }

            FormBinding.BindControlsToObject(agreement, pnlDetail);

            agreement.UWNotes = ((TextBox)this.Master.FindControl("UWNotesEdit")).Text;
            agreement.FirstTeamNotes = ((TextBox)this.Master.FindControl("FirstTeamNotesEdit")).Text;

            MerchantFacade facade = new MerchantFacade();
            int rows = facade.UpdateMerchantApp(agreement);

            
            UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

            FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), clone, agreement);

            WucServices1.UpdateServices(agreement.MerchantAppUID, "5");
            
            perform = true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
        return perform;
    }



    public override bool FormDataCheck()
    {
        string message = string.Empty;

        if (this.FirstTeam.Checked == true && this.FirstTeamRepUID.SelectedValue.Equals("-1"))
        {
            this.Master.AddMessageError("Please assign a Premier Representative."); 
        }

        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
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





    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void ButtonOK_Click(object sender, EventArgs e)
    {

        Button b = (Button)sender;

        HiddenField hfMID = (HiddenField)b.FindControl("hidMerchantID");
        HiddenField hfMRID = (HiddenField)b.FindControl("hidMRuleID");
        TextBox tbNSD = (TextBox)b.FindControl("tbNewStartDate");

        int days_add = CommonUtility.Util.if_i(tbNSD.Text, 0);

        if (days_add > 0)
        {
            int merchant_id = CommonUtility.Util.if_i(hfMID.Value, 0);
            int mrule_id = CommonUtility.Util.if_i(hfMRID.Value, 0);

            MRule objMR = DataMRule.GetMRuleMerchant(merchant_id, mrule_id, Constants.ROLE_FIRSTTEAM);

            objMR.Clone();

            if (objMR != null)
            {

                objMR.StartDate = objMR.StartDate.AddDays(days_add);

                DataMRule.UpdateMRuleMerchant(objMR);

                this.UpdateChangelog(objMR);

                this.Refresh_RulesGrid();

                this.Refresh_ChanglogGrid();

            }

        }

        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    private void UpdateChangelog(MRule objMR)
    {

        string DifferenceLog = "";

        // get differences before you save.
        if (objMR.MRuleClone != null)
        {
            DifferenceLog = objMR.GetDifferences();
        }

        // no exception, then save if there's any differences
        if (!string.IsNullOrEmpty(DifferenceLog))
        {
            DataUser.GetInstance().InsertChangeLog(
                objMR.BusinessDBAName
                , UserSessions.CurrentUser.UserName
                , CommonUtility.Util.if_s(objMR.MerchantAppUID, null)
                , objMR.MRuleID.ToString()
                , "MRule"
                , DifferenceLog
                , UserSessions.PortalUID
                );

        }
    }


    protected void ButtonCancel_Click(object sender, EventArgs e)
    {


        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;


    }









}
