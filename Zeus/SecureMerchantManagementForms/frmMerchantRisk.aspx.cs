using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.EditorControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Collections.Generic;

public partial class frmMerchantRisk : frmBaseDataEntry
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
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Risk);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Risk");
            }

            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

            this.FormShow(this.UID);

            wucDiversionGrid1.ZID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID);
            wucDiversionGrid1.LoadDiverted(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));

            this.LoadAssociationProgramHistory();

        }

        wucDiversionGrid1.event_click_reportdate += new ZeusWeb.UserControls.wucDiversionGrid.EventClickReportDate(wucReserveDivertMethod1_event_click_reportdate);
        wucDiversionGrid1.event_click_undivertedsuccess += new ZeusWeb.UserControls.wucDiversionGrid.EventClickUnDivertedSuccess(wucDiversionGrid1_event_click_undivertedsuccess);

        wucDiversionDialog1.event_click_savesuccess += wucDiversionDialog1_event_click_savesuccess;

    }

    protected void wucDiversionDialog1_event_click_savesuccess(int zid)
    {
        this.RefreshScreen();
    }

    protected void wucDiversionGrid1_event_click_undivertedsuccess(int zid, int diversionid)
    {
        wucDiversionGrid1.ZID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID);
        wucDiversionGrid1.LoadDiverted(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
        lblDiversionNotice.Text = "";
        pnlDiversionDates.Visible = false;
    }

    protected void wucReserveDivertMethod1_event_click_reportdate(int diversionid)
    {
        wucDiversionDialog1.ZID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID);
        wucDiversionDialog1.DiversionID = diversionid;

        wucDiversionDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;


        wucDiversionDialog1.FormShow("");
    }


    private void LoadRiskParameters()
    {
        string mcc = FormHandler.GetMerchantRiskMCC(UserSessions.CurrentMerchantApp);
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", this.UID);
        prms.Add("@Enabled", 1);
        //Added by Koshlendra for PXP-2023: Zeus: Set default Risk parameters for PaymentXP merchants Start        
        prms.Add("@MCC", mcc);
        //Added by Koshlendra for PXP-2023: Zeus: Set default Risk parameters for PaymentXP merchants End 
        DataSet ds = DataAccess.DataRiskDao.GetMerchantRiskParameters(prms);
        //Added by Koshlendra for PXP-2023: Zeus: Set default Risk parameters for PaymentXP merchants Start 

        //Added by Koshlendra for PXP-3529- Zeus: Set default Risk parameters for Active merchants start           
        //Ani: DM-5589
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ds = FormHandler.UpdateMerchantRiskParameters(UserSessions.CurrentMerchantApp);
        }
        //Added by Koshlendra for PXP-3529- Zeus: Set default Risk parameters for Active merchants start


        grdRisk.DataSource = ds;
        grdRisk.DataBind();
        lblFee.Visible = (grdRisk.Rows.Count == 0);
       
    }

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        WucBusinessInfo1.pnlInfo.Enabled = false;

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        bool isACHOnly = agreement.AchID > 0 && agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY;

        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);


        FormBinding.BindObjectToControls(agreement, pnlRiskFlag);

        if (!agreement.SalesFlag)
        {

        }
        FormHandler.SetControlEditMode(pnlRiskFlag, this.EditMode);

        FormBinding.BindObjectToControls(agreement, pnlChargebackManagement);
        FormHandler.SetControlEditMode(pnlChargebackManagement, this.EditMode);


        UserForm frm = null;
        User user = UserSessions.CurrentUser;
        bool isCBMgtVisiblePermission = false;
        bool isDisputeViaEResponseEnabled= false;
        if (user.UserForms.TryGetValue("FRMMERCHANTRISK", out frm) && frm.HasAccess)
        {
            if (frm.ControlObjects == null)
                DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

            foreach (ControlObject obj in frm.ControlObjects)
            {
                if (obj.Type.ToUpper() == "ULTRAWEBTAB" && obj.ID.ToUpper() == "TABCHARGEBACKMANAGEMENT")
                {
                    isCBMgtVisiblePermission = obj.IsVisible && UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_RISK;
                    isDisputeViaEResponseEnabled = obj.IsEnabled;
                    break;
                }
            }


        }
        tabReport.Tabs[4].Hidden = !isCBMgtVisiblePermission;
        DisputeViaEResponse.Enabled = this.EditMode && isDisputeViaEResponseEnabled;


        UserSessions.CurrentMerchantApp = agreement;
        WucBusinessInfo1.LoadOffice(agreement);
        // check if account is diverted
        this.RefreshDiversionTab();

        WebUtil.SetUserSpecificDisplayMode(this.DateReceived);
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message                  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            MasterPageMerchant master = (MasterPageMerchant)this.Master;
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }
        /******** End of PXP-2206 **************/
        this.ResetCBAssocPrgControls();
        this.LoadRiskParameters();
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        MerchantApp app = UserSessions.CurrentMerchantApp;
        app.PlacedOnMATCH = PlacedOnMATCH.Checked;
        MerchantFacade facade = new MerchantFacade();
        bool VarsalesFlag = true;
        bool CurrSalesFlag = app.SalesFlag;

        if (SalesFlag.SelectedValue == "False")
            VarsalesFlag = false;
        app.SalesFlag = VarsalesFlag;

        /// <summary>
        /// Fady Massoud 03-02-2021
        /// PXP-16276
        /// </summary>
        if (AllowAmount.SelectedValue == "True")
            app.AllowAmount = true;
        else
            app.AllowAmount = false;

        if (DisputeViaEResponse.SelectedValue == "True")
            app.DisputeViaEResponse = true;
        else
            app.DisputeViaEResponse = false;

        if (UserSessions.CurrentUser != null)
            app.UserUpdated = UserSessions.CurrentUser.UserName;

        facade.UpdateMerchantApp(app);
        if (CurrSalesFlag != VarsalesFlag)
        {
            this.AddSalesFlagNotes(CurrSalesFlag, VarsalesFlag);

        }
        this.UpdateRiskParamters();
        this.UpdateMerchantAppTransDB();
        this.EditMode = false;
        this.FormShow(this.UID);
        this.ToggleButtons();

        return true;
    }

    private void UpdateMerchantAppTransDB()
    {
        DataRisk data = DataAccess.DataRiskDao;

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);

        prms.Add("@MeritusDeclineRule", MeritusDeclineRule.Checked);
        prms.Add("@ShowMeritusDeclineRule", ShowMeritusDeclineRule.Checked);

        data.UpdateMerchantAppTransDB(prms);
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
        {
            bool _IsValid = true;
            foreach (GridViewRow row in grdRisk.Rows)
            {
                var _Threshold = DataLayer.Decimal2Field(((WebNumericEditor)row.FindControl("WebNumericEditor")).Text);
                var _Enabled = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkEnabled")).Checked);
                var _Exception = row.Cells[1].Text.ToString();
                int riskID = Convert.ToInt32(row.Cells[0].Text.Replace("&nbsp;", ""));

                if (Constant.MaxVolumeIds.Contains(riskID) && ((_Enabled && _Threshold <= 0) || _Threshold < 0))
                {
                    CustomValidator customValidator = new CustomValidator();
                    customValidator.IsValid = false;
                    customValidator.ErrorMessage = _Exception + ": amount must be more than 0.0000";
                    Page.Validators.Add(customValidator);
                    _IsValid = false;
                }
            }
            return _IsValid;
        }
        else
            return false;
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
        btnRefresh.Enabled = !this.EditMode;
        btnCancel.Enabled = this.EditMode;

        pnlGrd.Enabled = this.EditMode;
        grdRisk.Enabled = this.EditMode;

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
                //   this.FormSave();
                if (this.FormSave())
                {
                    this.EditMode = false;
                    this.Adding = false;
                    this.ToggleButtons();
                    url = "~/SecureMerchantManagementForms/frmMerchantRisk.aspx?";
                    url += "MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                break;
            //   this.FormShow(this.UID);

            // break;
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
                    //Modified by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message
                    string firstUserdetail = MerchantFacade.GetFirstUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID);
                    if (!string.IsNullOrWhiteSpace(firstUserdetail))
                    {
                        string notification = firstUserdetail + " is currently editing this ZID. Please ensure you will not overwrite each other's work.";
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "Notification", "alert(" + '"' + notification + '"' + ");", true);
                        MasterPageMerchant master = (MasterPageMerchant)this.Master;
                        master.UpdateNotification(firstUserdetail + " is editing this ZID.");
                    }
                    MerchantFacade.AddNewUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
                    /******** End of PXP-2206 **************/
                    this.ToggleButtons();
                    this.Page_Load(null, null);
                }
                break;

        }
    }

    public string GetSelectedListItems(ListControl lst)
    {
        string selected = string.Empty;
        foreach (ListItem item in lst.Items)
        {
            if (item.Selected)
                selected += item.Value;
        }
        return selected;
    }

    public string SetSelectedListItems(ListControl lst, string selected)
    {
        for (int i = 0; i < selected.Length; i++)
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

    public void chkEnabled_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
        int rowIndex = grdRow.RowIndex;

        MerchantRiskParameter param = null;

        param = new MerchantRiskParameter();
        param.UID = grdRow.Cells[4].Text.Replace("&nbsp;", "");
        // grdRisk.DataKeys[rowIndex].Values["UID"].ToString();
        param.Threshold = DataLayer.Decimal2Field(((WebNumericEditor)grdRow.FindControl("WebNumericEditor")).Text);
        param.Enabled = DataLayer.Field2Bool(((CheckBox)grdRow.FindControl("chkEnabled")).Checked);
        User user = UserSessions.CurrentUser;
        param.UserUpdated = user.UserName;
        param.RiskID = Convert.ToInt32(grdRow.Cells[0].Text.Replace("&nbsp;", ""));

        DataAccess.DataRiskDao.UpdateMerchantRiskParameter(param);
    }

    protected void WebNumericEdit_ValueChange(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
    {
        GridViewRow grdRow = ((GridViewRow)((WebNumericEditor)sender).NamingContainer);
        int rowIndex = grdRow.RowIndex;

        MerchantRiskParameter param = null;

        param = new MerchantRiskParameter();
        param.UID = grdRow.Cells[4].Text.Replace("&nbsp;", "");
        //grdRisk.DataKeys[rowIndex].Values["UID"].ToString();
        param.Threshold = DataLayer.Decimal2Field(((WebNumericEditor)grdRow.FindControl("WebNumericEditor")).Text);
        param.Enabled = DataLayer.Field2Bool(((CheckBox)grdRow.FindControl("chkEnabled")).Checked);
        User user = UserSessions.CurrentUser;
        param.UserUpdated = user.UserName;
        param.RiskID = Convert.ToInt32(grdRow.Cells[0].Text.Replace("&nbsp;", ""));

        DataAccess.DataRiskDao.UpdateMerchantRiskParameter(param);
    }

    public bool UpdateRiskParamters()
    {
        DataRisk data = DataAccess.DataRiskDao;
        MerchantRiskParameter param = null;

        foreach (GridViewRow row in grdRisk.Rows)
        {
            param = new MerchantRiskParameter();
            param.UID = row.Cells[4].Text.Replace("&nbsp;", "");
            //grdRisk.DataKeys[row.RowIndex].Values["UID"].ToString();
            param.Threshold = DataLayer.Decimal2Field(((WebNumericEditor)row.FindControl("WebNumericEditor")).Text);
            param.Enabled = DataLayer.Field2Bool(((CheckBox)row.FindControl("chkEnabled")).Checked);
            User user = UserSessions.CurrentUser;
            param.UserUpdated = user.UserName;
            //Added by Koshlendra for PXP-2023,PXP-3529 Zeus: Set default Risk parameters for PaymentXP merchants Start  
            param.MCC = FormHandler.GetMerchantRiskMCC(UserSessions.CurrentMerchantApp);
            //Added by Koshlendra for PXP-2023,PXP-3529: Zeus: Set default Risk parameters for PaymentXP merchants End  
            param.RiskID = Convert.ToInt32(row.Cells[0].Text.Replace("&nbsp;", ""));

            data.UpdateMerchantRiskParameter(param);
            data.UpdateMonthlyTransactionVolume(param, UserSessions.CurrentMerchantApp.MerchantAppUID);
        }

        return true;
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        wucDiversionDialog1.FormNew();
        wucDiversionDialog1.ZID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID);
        wucDiversionDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.RefreshScreen();
    }

    protected void RefreshScreen()
    {
        wucDiversionGrid1.ZID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID);
        wucDiversionGrid1.BindGrid();

        this.RefreshDiversionTab();
    }

    public void RefreshDiversionTab()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ZID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
        prms.Add("@ActiveDiversionOnly", 1);
        List<RDBDiversion> li = DataReserve.GetReserveDiversion(prms);
        blDiv.Items.Clear();

        if (li != null && li.Count > 0)
        {
            pnlDiversionDates.Visible = true;

            if (li.Count == 1)
            {
                lblDiversionNotice.Text = string.Format("This account has 1 diversion, placed on {0}", WebUtil.ConvertToUserDatePattern(li[0].DateDiverted.ToShortDateString()));
            }
            else
            {
                lblDiversionNotice.Text = string.Format("This account has {0} active diversions placed on it. The dates they were diverted were on:", li.Count.ToString());
                foreach (RDBDiversion item in li)
                {
                    blDiv.Items.Add(new ListItem(WebUtil.ConvertToUserDatePattern(item.DateDiverted.ToShortDateString())));
                }
            }

            if (li.Count >= 2)
            {
                Button3.Enabled = false;
            }
            else
            {
                Button3.Enabled = true;
            }

        }
        else
        {
            pnlDiversionDates.Visible = false;
        }
    }

    protected void ChargebackCardID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedVal = this.ChargebackCardID.SelectedValue;

        if (selectedVal.Equals("0"))
        {
            this.ChargebackCardProgramID.Items.Clear();
            this.ChargebackCardProgramID.Items.Add("-- Select --");
        }
        else
        {
            LookupTableHandler.LoadChargebackPrograms(this.ChargebackCardProgramID, selectedVal);
        }
    }

    protected void btnAddProgram_Click(object sender, EventArgs e)
    {
        try
        {
            CBAssociationProgram association = new CBAssociationProgram();
            FormBinding.BindControlsToObject(association, pnlAssocProgram);

            association.ZID = int.Parse(UserSessions.CurrentMerchantApp.ID);
            association.UserCreated = UserSessions.CurrentUser.UserName;

            List<string> validationErrors = association.Validate();

            if (validationErrors.Count == 0)
            {
                MerchantFacade.SaveMerchantCBAssocProgram(association);
                ResetCBAssocPrgControls();
                this.lblErrorMsg.Text = "Card Association Program saved";
                this.lblErrorMsg.ForeColor = System.Drawing.Color.Green;

                this.LoadAssociationProgramHistory();
            }
            else
            {
                //we'll just display the first error message
                this.lblErrorMsg.Text = validationErrors[0];
            }
        }
        catch (SqlException ex)
        {
            this.lblErrorMsg.Text = ex.Message;
        }
        catch (Exception)
        {
            this.lblErrorMsg.Text = "Failed to save Card Association Program";
        }
    }

    private void ResetCBAssocPrgControls()
    {
        this.ChargebackCardID.SelectedIndex = 0;
        this.ChargebackCardProgramID.SelectedIndex = 0;
        this.ProgramMonth.SelectedIndex = 0;
        this.DateReceived.Text = "";

        DateTime calendar = DateTime.Now.AddMonths(-1);

        this.CalendarYear.Text = calendar.Year.ToString();
        this.CalendarMonth.SelectedValue = calendar.Month.ToString();
    }

    private void LoadAssociationProgramHistory()
    {
        List<CBAssociationProgram> history = DataRisk.GetInstance().GetMerchantCBAssocProgramHistory(int.Parse(UserSessions.CurrentMerchantApp.ID));

        this.grdAssociationProg.DataSource = history;
        this.grdAssociationProg.DataBind();

        DateTime calendar = DateTime.Now.AddMonths(-1);

        List<CBAssociationProgram> lastMonth = history.Where<CBAssociationProgram>(p => p.StartProgramDate == string.Format("{0}/{1}/1", calendar.Year, calendar.Month)).ToList<CBAssociationProgram>();

        if (lastMonth.Count > 0)
        {
            this.pnlAssocProgramAlert.Visible = true;
            this.lblAssocProgramMsg.Text = "Merchant placed on Association Program " + calendar.ToString("MMM yyyy") + ":";
            this.blAssocPrograms.Items.Clear();

            foreach (CBAssociationProgram cb in lastMonth)
            {
                this.blAssocPrograms.Items.Add(string.Format("{0} - {1}", cb.CardAssociationName, cb.AssociationProgramName));
            }
        }
        else
        {
            this.pnlAssocProgramAlert.Visible = false;
        }
    }

    protected void grdAssociationProg_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                e.Row.Cells[0].Text = WebUtil.ConvertToUserDatePattern(e.Row.Cells[0].Text);
                e.Row.Cells[4].Text = WebUtil.ConvertToUserDatePattern(e.Row.Cells[4].Text);
                e.Row.Cells[6].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[6].Text);
                break;

            default:
                break;
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;
        int cbProgramId = int.Parse(lb.CommandArgument);

        DataRisk.GetInstance().DeleteMerchantCBAssocProgram(cbProgramId, UserSessions.CurrentUser.UserName);

        LoadAssociationProgramHistory();
    }
    /* working over to make the change to capture the salesFlag */
    protected void AddSalesFlagNotes(bool CurrSalesFlag, bool VarsalesFlag)
    {

        if (UserSessions.CurrentMerchantApp != null)
        {
            MerchantFacade facade = new MerchantFacade();
            MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
            User user = UserSessions.CurrentUser;
            string subject = string.Empty;
            string Notes = string.Empty;
            subject = "Risk - Sales Flags Change";
            Notes = "Sales Flag Indicator gets updated from ";

            if (CurrSalesFlag)
                Notes += " ON";
            else
                Notes += " OFF";

            if (VarsalesFlag)
                Notes += " to ON";
            else
                Notes += " to OFF";

            MerchantNotes ObjMerchantNotes = new MerchantNotes();
            ObjMerchantNotes.MerchantAppUID = app.MerchantAppUID;
            ObjMerchantNotes.Subject = subject;
            ObjMerchantNotes.Notes = Notes;
            ObjMerchantNotes.View_Agent = false;
            ObjMerchantNotes.View_Bank = false;
            ObjMerchantNotes.View_MPSAll = true;
            ObjMerchantNotes.Email_Agent = false;
            ObjMerchantNotes.UserCreated = user.UserName;
            DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);
        }

    }
}



