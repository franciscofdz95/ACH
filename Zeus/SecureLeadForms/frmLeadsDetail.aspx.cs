using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

using iTextSharp.text;
using iTextSharp.text.pdf;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects.Notify;

public partial class frmLeadsDetail : frmBaseDataEntry
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkLeadDetails")).CssClass = "active";
        dvAppsFees.Attributes["onchange"] = "AppsFees_OnChange(this)";
        wucOutlook1.PanelApp.Visible = true;
        wucOutlook1.CloseBtn.Text = "Refresh";

        if (!this.IsPostBack)
        {
            FormHandler.SetSecurity(this.Page);

            this.Adding = Convert.ToBoolean(Request.QueryString["Adding"]);
            if (this.Adding)
                this.FormNew();
            else
            {
                this.UID = UserSessions.CurrentLead.LeadUID;

                this.FormShow(this.UID);
            }
        }
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        wucContact1.Adding = true;
        btnCreateApp.Visible = false;
        btnPDF.Visible = false;

        User user = UserSessions.CurrentUser;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        categories.pnlGrid.Enabled = this.EditMode;
        FormHandler.SetControlEditMode(categories, true);

        this.ToggleButtons();
        wucLeadNotes1.btnAdd.Enabled = false;
        wucOutlook1.btnApp.Enabled = false;
        btnAddCommunication.Enabled = false;

        wucOutlook1.PanelApp.Visible = true;
        wucLeadNotes1.PanelApp.Visible = true;

        wucContact1.Adding = true;
        wucContact1.EditMode = true;
        wucContact1.FormNew();

        this.LeadInfo1.FormNew(this.EditMode);
        btnConvert.Enabled = false;
    }

    public override void FormShow(string LeadUID)
    {
        DataLead data = DataAccess.DataLeadDao;
        Lead lead = data.GetLead(LeadUID);

        UserSessions.CurrentLead = lead;

        FormClear();
        FormBinding.BindObjectToControls(lead, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        
        categories.pnlGrid.Enabled = this.EditMode;
        categories.pnl.Enabled = true;
        FormHandler.SetControlEditMode(categories, true);

        UserSessions.CurrentLead = lead;

        MasterPageSales master = (MasterPageSales)this.Master;
        if (lead.LeadAgent != null)
        {
            master.ShowNotes(lead.LeadAgent.AgentMemo, false);
        }

        wucLeadNotes1.LoadNotes();
        wucLeadNotes1.LeadID = wucOutlook1.LeadID = this.UID;
        wucOutlook1.PanelApp.Visible = true;
        wucLeadNotes1.PanelApp.Visible = true;
        wucOutlook1.LoadAppointments();
        
        this.TogglePanels(true);

        BusinessType.ReadOnly = !EditMode;
        BusinessType.Enabled = true;
        BusinessType.Text = Server.HtmlDecode(lead.BusinessType);


        categories.LeadUID = UserSessions.CurrentLead.LeadUID;
        categories.FormServices();

        wucContact1.EditMode = this.EditMode;
        wucContact1.ObjectID = Convert.ToInt32(lead.LeadID);
        wucContact1.FormShow("", false);

        this.LeadInfo1.FormShow(lead, this.EditMode);

        btnConvert.Enabled = !this.EditMode && (lead.CountofZIDs == 0);
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);

        wucOutlook1.ClearGrid();

        grdCommunications.DataSource = null;
        grdCommunications.DataBind();

        wucLeadNotes1.ClearGrid();
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        try
        {
            Lead lead;

            if (this.Adding)
            {
                lead = new Lead();
            }
            else
            {
                lead = UserSessions.CurrentLead;
            }

            int old_AssignedUser = lead.AssignedUserID;

            FormBinding.BindControlsToObject(lead, pnlDetail);
            DataLead data = DataAccess.DataLeadDao;

            User user = UserSessions.CurrentUser;
            lead.UserUpdated = user.UserName;
            lead.BusinessType = Server.HtmlEncode(this.BusinessType.Text);

            if (!lead.Country.Trim().ToUpper().Equals("US"))
            {
                TextBox txtProvince = (TextBox)this.LeadInfo1.FindControl("Province");
                lead.State = txtProvince.Text;
            }

            if (!this.Adding)
            {
                int rows = data.UpdateLead(lead);

                if (rows > 0)
                {
                    categories.LeadUID = this.UID;
                    categories.UpdtateServices();
                }

                wucContact1.ObjectID = lead.LeadID;
                wucContact1.FormSave();
            }
            else
            {
                lead.UserCreated = user.UserName;
                lead.DateCreated = DateTime.Now;
                lead.InitialDate = DateTime.Now;
                lead.FollowupStatusID = "C111CF77-BA7B-4393-B208-3B7393C42E92";
                
                data.InsertLead(lead);

                if (lead.LeadUID != "-1")
                {
                    this.UID = lead.LeadUID;
                    UserSessions.CurrentLead = lead;

                    MasterPageSales master = (MasterPageSales)this.Master;
                    categories.LeadUID = this.UID;
                    categories.UpdtateServices();

                    wucContact1.ObjectID = Convert.ToInt32(lead.LeadID);
                    wucContact1.FormSave();
                }
            }

            lead = data.GetLead(lead.LeadID);

            if ((!old_AssignedUser.Equals(lead.AssignedUserID)) && (lead.AssignedUserID > 0))
            {
                List<Lead> Leads = new List<Lead>();
                Leads.Add(lead);

                StringBuilder emailbody = new StringBuilder();
                foreach (Lead l in Leads)
                {
                    emailbody.Append("<tr><td>" + l.LeadID + "</td>");
                    emailbody.Append("<td>" + l.DBAName + "</td>");
                    emailbody.Append("<td>" + l.Source + "</td>");
                    emailbody.Append("<td>" + l.Email + "</td>");
                    emailbody.Append("<td>" + l.PhoneNumber + "</td></tr>");
                }
                TicketNotification.SendNewLeadAssignedEmail(emailbody.ToString(), lead.AssignedUserID);
            }

            wucLeadNotes1.LeadID = wucOutlook1.LeadID = lead.LeadUID;
            wucLeadNotes1.AddNotes();
            wucOutlook1.AddApppointment();
            ToggleButtons();
            return true;
        }
        catch (System.Exception exc)
        {
            throw exc;
        }
    }

    public override bool FormDataCheck()
    {
        List<string> message = new List<string>();

        if (string.IsNullOrWhiteSpace(this.LeadInfo1.AgentSelector.m_AgentUID))
        {
            message.Add("Agent is required");
        }

        string msg = this.LeadInfo1.FormDataCheck();
        
        if(!string.IsNullOrWhiteSpace(msg))
            message.Add(msg);
        
        foreach(string str in message)
            wucMessage1.AddMessageError(str);

        return (message.Count == 0);
    }

    public override bool FormDelete()
    {
        return false;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();

        wucContact1.FormShow("", false);
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnAdd.Enabled = !btnAdd.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        btnPDF.Enabled = !btnPDF.Enabled;
        btnCreateApp.Enabled = !btnCreateApp.Enabled;

        MasterPageSales master = (MasterPageSales)this.Master;

        wucContact1.EditMode = this.EditMode;
        wucContact1.ToggleButtons();
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
                    Response.Redirect("frmLeadsDetail.aspx?LeadUID=" + this.UID + "&Adding=false");
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                if (this.UID.Equals(string.Empty))
                {
                    this.CloseForm();
                }
                else
                    this.FormCancel();
                break;

            case "Close":
                this.CloseForm();
                break;

            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
                break;

            case "App PDF":
                url = "frmCreateMerchantApp2.aspx?LeadUID=" + this.UID + "&LeadID=" + this.UID + "&pdf=true&Adding=false&MerchantAppUID=" + this.LeadInfo1.MerchantUID + "&PostBackURL=~/SecureLeadForms/frmLeadsDetail.aspx?";
                Response.Redirect(url);
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;
                
            case "Create App":

                if (string.IsNullOrWhiteSpace(this.LeadInfo1.AgentSelector.m_AgentUID))
                {
                    wucMessage1.AddMessageError("Agent is required");
                    return;
                }

                BindAppsFees();
                WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;

            case "Convert Lead":

                ConvertLead2Merchant();
                
                break;
        }
    }

    private void ConvertLead2Merchant()
    {        
        Lead lead = UserSessions.CurrentLead;
       
        if (lead.CountofDocs == 0)
        {
            lblMessage.Text = "No documents are uploaded. Additional documents may be requested. Press Ok to proceed.";
            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
        else
            CreateMerchant();
    }

    private void BindAppsFees()
    {
        string AgentUID = this.LeadInfo1.AgentSelector.m_AgentUID;

        Dictionary<string, string> appsFees = new Dictionary<string, string>();
        appsFees = DataApp.Instance.GetAppsFeesKeyValue(AgentUID);

        dvAppsFees.DataSource = appsFees;
        dvAppsFees.DataTextField = "Value";
        dvAppsFees.DataValueField = "Key";
        dvAppsFees.DataBind();

        dvAppsFees.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", "-1"));
        dvAppsFees.Items.Insert(1, new System.Web.UI.WebControls.ListItem("** Create Fee Template **", "0000-0000-0000-0000"));
    }

    protected void grdCommunications_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                HyperLink img = (HyperLink)e.Row.FindControl("lnkEmailID");

                string url = "frmCommunicationsDetail.aspx?PostBackURL=frmLeadsDetail.aspx";
                url += "&LeadUID=" + UserSessions.CurrentLead.LeadUID;
                url += "&CommunicationUID=" + DataBinder.Eval(e.Row.DataItem, "CommunicationID");
                url += "&DBAName=" + this.LeadInfo1.BusinessDBA.Trim();
                url += "&Adding=false";

                img.NavigateUrl = url;
                break;
        }
    }

    protected void grdCommunications_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCommunications.PageIndex = e.NewPageIndex;
        this.LoadCommunications();
    }

    protected void btnAddCommunication_Click(object sender, EventArgs e)
    {
        if (!this.FormSave())
            return;

        string url = "frmCommunicationsDetail.aspx?";
        url += "LeadUID=" + this.UID;
        url += "&LeadID=" + this.UID;
        url += "&DBAName=" + this.LeadInfo1.BusinessDBA;
        url += "&Adding=true";

        // TODO: tol add this back in!!
        //url += "&Fax=" + FaxNumber.Text.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
        //url += "&Email=" + Email.Text.Trim();

        Response.Redirect(url);
    }

    protected void btnComOK_Click(object sender, EventArgs e)
    {
        WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnComCancel_Click(object sender, EventArgs e)
    {
        WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    private void CloseForm()
    {
        string url = string.Empty;

        if (Request.QueryString["PostBackURL"] != null)
            url = Request.QueryString["PostBackURL"].ToString();

        if (!url.Equals(string.Empty))
            Response.Redirect(url);
        else
            Response.Redirect("frmLeads.aspx");
    }

    private void TogglePanels(bool value)
    {
        FormHandler.SetControlEditMode(wucOutlook1, value);

        FormHandler.SetControlEditMode(pnlCommunications, value);
        FormHandler.SetControlEditMode(wucLeadNotes1, value);

        wucContact1.EditMode = this.EditMode;
        wucContact1.ToggleButtons();
    }

    private void LoadCommunications()
    {
        Lead lead = UserSessions.CurrentLead;
        DataCommunication data = DataAccess.DataCommunicationDao;

        DataSet ds = data.GetCommunications(lead);
        grdCommunications.DataSource = ds;
        grdCommunications.DataBind();
        
        pnlComm.Visible = !(grdCommunications.Rows.Count == 0);
        lblData.Visible = (grdCommunications.Rows.Count == 0);
    }

    protected void interlinkApex_Click(object sender, EventArgs e)
    {
        //1. Build Tokenized TransferUrl
        string url = LogonTokenFacade.BuildTransferUrl(ConfigurationManager.AppSettings["AgentPortalLoginUrl"], UserSessions.CurrentUser, this.Request);

        //2. open new window for the transfer url
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), CommonUtility.JSScriptProvider.BuildOpenWindowScript(url), true);
    }

    protected void interlinkApp_Click(object sender, EventArgs e)
    {
        string PartnerLink = string.Concat(ConfigurationManager.AppSettings["ApplicationUrl"], "?Key=", dvAppsFees.SelectedValue,
                            "&AgentUID=", this.LeadInfo1.AgentSelector.m_AgentUID, "&LeadUID=", UserSessions.CurrentLead.LeadUID);

        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ApplicationPopup", string.Format("var childWindow = window.open('{0}', 'ApplicationPopup', '');", PartnerLink), true);
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        dvAppsFees.SelectedIndex = 0;
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnCan_Click(object sender, EventArgs e)
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        CreateMerchant();
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

    }

    private void CreateMerchant()
    {
        Lead lead = UserSessions.CurrentLead;
        DataMerchantApp dataApp = DataAccess.DataMerchantAppDao;
        User user = UserSessions.CurrentUser;

        int ZID = dataApp.InsertLeadAsMerchant(lead.LeadID, user.UserID);

        if (ZID > 0)
        {
            wucMessage1.AddMessageSuccess("This Lead has been converted to a Merchant with ZID: " + ZID.ToString());
            FormShow(lead.LeadUID);
        }
    }

}
