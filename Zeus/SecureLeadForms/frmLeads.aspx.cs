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
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.Shared;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Infragistics.Web.UI.EditorControls;
using PaymentXP.Facade;
using System.Text;
using System.Linq;

public partial class frmLeads : frmBaseSearch
{
   
    public bool isSearch
    {
        get
        {
            if (ViewState["isSearch"] != null) return Convert.ToBoolean(ViewState["isSearch"]);
            else return true;
        }
        set { ViewState["isSearch"] = value; }
    }

    protected override void OnInit(EventArgs e)
    {
        wucOutlook1.ButtonClick += new wucAddtoOutlook.ButtonClickHandler(wucOutlook1_ButtonClick);
        base.OnInit(e);
        grdAgent.GridRowCommand += new wucAgent.GridRowCommandHandler(grdAgents_GridRowCommand);

    }

    void wucOutlook1_ButtonClick(object sender, EventArgs e)
    {
        Search(false);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkSearchLeads")).CssClass = "active";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        dlgAgent.Attributes.Add("onKeyDown", "KeyDownHandler('" + ((Button)grdAgent.FindControl("btnSearch")).ClientID + "')");
        grdAgent.DataSourceSelectCountMethod = "GetAgentsPagingRowCount";
        grdAgent.DataSourceSelectMethod = "GetAgentsPaging";

        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.IsBank)
            lbSelectAgent.Visible = false;

        if (!this.IsPostBack)
        {
            //Set the current page
        
            LookupTableHandler.LoadLeadStatus(StatusID, true);
            LookupTableHandler.LoadLeadSources(SourceID, false);
            LookupTableHandler.LoadLeadSources(Source, false);
            LookupTableHandler.LoadLeadSources(UploadSource, false);
            LookupTableHandler.LoadStates(BusinessState, true);
            LookupTableHandler.LoadLeadFollowupStatus(FollowUpStatusUID, true);
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            LookupTableHandler.LoadTimeZones(TimeZone, true);
            LookupTableHandler.LoadLeadReps(AssignedUserID, true);
            LookupTableHandler.LoadLeadOrigin(UploadOrigin, false);
            LookupTableHandler.LoadLeadOrigin(LeadOriginID, false);

            //added the datasourceid on grids in .aspx page
            //grd.DataSourceID = "odsLeads"; //DO NO REMOVE, OTHER GRID WILL NOT LOAD
            //GridView1.DataSourceID = "odsLeads"; //DO NO REMOVE, OTHER GRID WILL NOT LOAD

            this.Search(true);
            WebDialogWindow5.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

        }

    }

    public override void Search(bool IsOnLoad)
    {
        string CallResultsList = string.Empty;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter lead = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(lead, pnlSearch);
        }

        LoadActions();

        grd.DataBind();
        GridView1.DataBind();
        pnlRecords.Visible = pnlAction.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;

        FormHandler.ClearAllControls(wucOutlook1.pnlApp);
        wucOutlook1.PanelApp.Visible = false;
        LeadsNotes1.CloseBtn.Text = wucOutlook1.CloseBtn.Text = "Close";
        LeadsNotes1.PanelApp.Visible = false;

    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);

        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());

        this.Search(false);

        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
        DateType.SelectedIndex = 0;
        rdExport.SelectedIndex = 0;
        AgentUID.Value = string.Empty;
    }

    protected void btnAddMerchant_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "frmLeadsDetail.aspx?Adding=true";
        Response.Redirect(url);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {        
        grd.EditIndex = -1;
        this.CurrentPage = 1;
        this.PageSize = CommonUtility.Util.if_i(cboPageSize.SelectedValue,0);
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {           

            case DataControlRowType.DataRow:

                if (grd.EditIndex != e.Row.RowIndex || grd.EditIndex == -1)
                {
                   
                    Label lbFollowUp = ((Label)e.Row.FindControl("lblFollowUp"));
                    string FollowUpdate = WebUtil.ConvertToUserDateTimeSettings(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "FollowUpDate")));
                    lbFollowUp.Text = string.IsNullOrEmpty(FollowUpdate) ? "" : FollowUpdate;
                    
                }
                else
                {

                    DropDownList StatusUID = ((DropDownList)e.Row.FindControl("ddpStatus"));
                    DropDownList State = ((DropDownList)e.Row.FindControl("ddpState"));
                    Label lblStatus = ((Label)e.Row.FindControl("lblStatus1"));

                    LookupTableHandler.LoadLeadStatus(StatusUID, false);
                    LookupTableHandler.LoadStates(State, false);

                    ListHandler.ListFindItem(StatusUID, DataBinder.Eval(e.Row.DataItem, "StatusUID").ToString());
                    ListHandler.ListFindItem(State, DataBinder.Eval(e.Row.DataItem, "State").ToString());

                    if (UserSessions.CurrentUser.IsAgent && UserSessions.CurrentLoggedInAgent.ParentUID.ToUpper() == "F79D9B35-A759-4B92-B754-D63D8E62ED74")
                    {
                        if (DataBinder.Eval(e.Row.DataItem, "StatusUID").ToString().ToUpper() == Constants.LEADSTATUS_STATEMENTSRECEIVED)
                        {
                            lblStatus.Visible = true;
                            StatusUID.Visible = false;
                        }
                        else
                        {
                            lblStatus.Visible = false;
                            StatusUID.Visible = true;
                        }
                    }
                }

                if (DataBinder.Eval(e.Row.DataItem, "FollowupStatusUID").ToString().ToUpper() == Constants.HOT)
                    e.Row.BackColor = System.Drawing.Color.LightBlue;

                string datecreated = WebUtil.ConvertToUserDateTimeSettings(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCreated")));
                e.Row.Cells[7].Text = string.IsNullOrEmpty(datecreated) ? "" : datecreated;

                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        string status = string.Empty;
        string leaduid = string.Empty;
        string assignto = string.Empty;
        GridViewRow grdRow = null;

        // basically, we just want to have a whitelist of valid commands.
        Dictionary<string, string> diValidCommands = new Dictionary<string, string>();

        diValidCommands.Add("ID", "LinkButton");
        diValidCommands.Add("EditLead", "ImageButton");
        diValidCommands.Add("UpdateLead", "ImageButton");
        diValidCommands.Add("CancelLead", "ImageButton");
        diValidCommands.Add("Notes", "ImageButton");
        diValidCommands.Add("Outlook", "ImageButton");

        if (diValidCommands.ContainsKey(e.CommandName))
        {
            if (diValidCommands[e.CommandName] == "LinkButton")
            {
                LinkButton btn = (LinkButton)e.CommandSource;
                grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                if (btn.Text == "0")
                {
                    return;
                }
            }
            else if (diValidCommands[e.CommandName] == "ImageButton")
            {
                ImageButton btn = (ImageButton)e.CommandSource;
                grdRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            }
            else
                return;

            DataLead objdataLead = DataAccess.DataLeadDao;
            Lead lead = objdataLead.GetLead(grd.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString());

            switch (e.CommandName)
            {

                case "UpdateLead":

                    Lead objLead = lead;

                    objLead.ContactName = ((TextBox)grd.Rows[grdRow.RowIndex].FindControl("txtContactName")).Text;
                    objLead.ContactLastName = ((TextBox)grd.Rows[grdRow.RowIndex].FindControl("txtContactLastName")).Text;

                    // handle followup date
                    string strDate = ((WebDatePicker)grd.Rows[grdRow.RowIndex].FindControl("FollowUpDate")).Text;

                    if (strDate != string.Empty && objLead.FollowUpDate.Date != DataLayer.Field2Date(((WebDatePicker)grd.Rows[grdRow.RowIndex].FindControl("FollowUpDate")).Value).Date)
                    {
                        if (DataLayer.Field2Date(((WebDatePicker)grd.Rows[grdRow.RowIndex].FindControl("FollowUpDate")).Value) < DateTime.Today)
                        {
                            FormHandler.DisplayMessage(Page.ClientScript, "Date Cannot be in the past");
                            return;
                        }
                    }

                    if (strDate != string.Empty)
                    {
                        objLead.FollowUpDate = DataLayer.Field2Date(((WebDatePicker)grd.Rows[grdRow.RowIndex].FindControl("FollowUpDate")).Value);
                    }

                    objLead.PhoneNumber = ((TextBox)grd.Rows[grdRow.RowIndex].FindControl("txtPhone")).Text;

                    objLead.DBAName = ((TextBox)grd.Rows[grdRow.RowIndex].FindControl("txtDBA")).Text;

                    if (((DropDownList)grd.Rows[grdRow.RowIndex].FindControl("ddpState")).SelectedIndex > 0)
                    {
                        objLead.State = ((DropDownList)grd.Rows[grdRow.RowIndex].FindControl("ddpState")).SelectedValue;
                    }
                    string agentUID = objLead.AgentUID;
                    
                    if (objLead.StatusID.ToUpper() != ((DropDownList)grd.Rows[grdRow.RowIndex].FindControl("ddpStatus")).SelectedValue.ToUpper() &&
                        ((DropDownList)grd.Rows[grdRow.RowIndex].FindControl("ddpStatus")).SelectedValue.ToUpper() == Constants.LEADSTATUS_ASSIGNED)
                        objLead.DateAssigned = DateTime.Now;

                    if (((DropDownList)grd.Rows[grdRow.RowIndex].FindControl("ddpStatus")).SelectedIndex != 0)
                        objLead.StatusID = ((DropDownList)grd.Rows[grdRow.RowIndex].FindControl("ddpStatus")).SelectedValue;

                    if (objLead.AgentUID != agentUID && objLead.StatusID.ToUpper() == Constants.LEADSTATUS_NEW)
                    {
                        objLead.DateAssigned = DateTime.Now;
                        objLead.StatusID = Constants.LEADSTATUS_ASSIGNED;
                    }

                    //Approved/ Declined/Not Interested/ Withdrawn
                    if (objLead.StatusID.ToUpper() == Constants.LEADSTATUS_APPROVED || objLead.StatusID.ToUpper() == Constants.LEADSTATUS_DECLINED || objLead.StatusID.ToUpper() == Constants.LEADSTATUS_WITHDRAWN || objLead.StatusID.ToUpper() == Constants.LEADSTATUS_NOTINTERESTED)
                        objLead.ClosedDate = DateTime.Now;

                    objLead.DateUpdated = DateTime.Now;
                    objLead.UserUpdated = UserSessions.CurrentUser.UserName;

                    objdataLead.UpdateLead(objLead);
                    grd.EditIndex = -1;
                    Search(false);

                    break;

                case "EditLead":

                    grd.EditIndex = grdRow.RowIndex;
                    Search(false);
                    break;

                case "CancelLead":

                    grd.EditIndex = -1;
                    Search(false);
                    break;

                case "Outlook":

                    FormHandler.ClearAllControls(wucOutlook1.pnlApp);
                    // TOL: todo: figure out something for this
                    //wucOutlook1.LeadID = UserSessions.CurrentLead.LeadUID;
                    WebDialogWindow5.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                    wucOutlook1.DefaultStartDate = DateTime.Now;
                    wucOutlook1.DefaultEndDate = DateTime.Now;
                    break;

                case "Notes":

                    // TOL: todo: figure out something for this
                    //LeadsNotes1.LeadID = UserSessions.CurrentLead.LeadUID;
                    WebDialogWindow6.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                    break;
            }
        }
    }

    private void LoadAppointment(DateTime dt)
    {
        System.Text.StringBuilder sbICSFile = new System.Text.StringBuilder();
        DateTime dtNow = DateTime.Now;

        sbICSFile.AppendLine("BEGIN:VCALENDAR");
        sbICSFile.AppendLine("PRODID:-//ICSTestCS/");
        sbICSFile.AppendLine("CALSCALE:GREGORIAN");

        // Define the event.
        sbICSFile.AppendLine("BEGIN:VEVENT");

        sbICSFile.Append("DTSTART;TZID=US / Pacific:");
        sbICSFile.Append(dt.Year.ToString());
        sbICSFile.Append(FormatDateTimeValue(dt.Month));
        sbICSFile.Append(FormatDateTimeValue(dt.Day) + "T");
        //sbICSFile.AppendLine(dt.ToShortTimeString());

        sbICSFile.Append("DTEND;TZID=US / Pacific:");
        sbICSFile.Append(dt.Year.ToString());
        sbICSFile.Append(FormatDateTimeValue(dt.Month));
        sbICSFile.Append(FormatDateTimeValue(dt.Day) + "T");
        //sbICSFile.AppendLine(dt.ToShortTimeString());

        sbICSFile.AppendLine("SUMMARY:" + "New Appointment for " + UserSessions.CurrentLead.DBAName);
        sbICSFile.AppendLine("DESCRIPTION: NEW Appointment");
        sbICSFile.AppendLine("UID:0");
        sbICSFile.AppendLine("SEQUENCE:0");
        sbICSFile.AppendLine("METHOD:PUBLISH");

        sbICSFile.Append("DTSTAMP:" + dtNow.Year.ToString());
        sbICSFile.Append(FormatDateTimeValue(dtNow.Month));
        sbICSFile.Append(FormatDateTimeValue(dtNow.Day) + "T");
        sbICSFile.Append(FormatDateTimeValue(dtNow.Hour));
        sbICSFile.AppendLine(FormatDateTimeValue(dtNow.Minute) + "00");

        sbICSFile.AppendLine("END:VEVENT");
        sbICSFile.AppendLine("END:VCALENDAR");

        Response.ContentType = "text/calendar";
        Response.AddHeader("content-disposition", "attachment; filename=CalendarEvent1.ics");
        Response.Write(sbICSFile);
        Response.End();
    }

    private string FormatDateTimeValue(int DateValue)
    {
        if (DateValue < 10)
            return "0" + DateValue.ToString();
        else
            return DateValue.ToString();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    private int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = 0;
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }

        return newSortDirection;
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    private void ClearGrid()
    {
        grd.DataSourceID = string.Empty;
        grd.DataBind();

        GridView1.DataSourceID = string.Empty;
        GridView1.DataBind();
    }

    protected void odsLeads_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter lead;

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@DBAName", BusinessDBAName.Text);

        if (BusinessPhone.Text != string.Empty)
            prms.Add("@PhoneNumber", BusinessPhone.Text);

        if (AgentUID.Value != string.Empty)
            prms.Add("@AgentUID", AgentUID.Value);

        if (StatusID.SelectedIndex > 0)
            prms.Add("@StatusUID", StatusID.SelectedItem.Value);

        if (BusinessContact.Text != string.Empty)
            prms.Add("@ContactName", BusinessContact.Text);

        if (BusinessLastContact.Text != string.Empty)
            prms.Add("@ContactLastName", BusinessLastContact.Text);

        if (BusinessFax.Text != string.Empty)
            prms.Add("@FaxNumber", BusinessFax.Text);

        if (SourceID.SelectedIndex > 0)
            prms.Add("@LeadsSourcesUID", SourceID.SelectedItem.Value);

        if (BusinessState.SelectedIndex > 0)
            prms.Add("@State", BusinessState.SelectedItem.Value);

        if (BusinessEmailAddress.Text != string.Empty)
            prms.Add("@Email", BusinessEmailAddress.Text);

        if (TimeZone.SelectedIndex > 0)
            prms.Add("@TimeZone", TimeZone.SelectedItem.Value);

        if (FollowUpStatusUID.SelectedIndex > 0)
            prms.Add("@FollowupStatusUID", FollowUpStatusUID.SelectedItem.Value);

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@StartDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndDate", SearchEndDate.Value);       

        if (MerchantID.Text != string.Empty)
            prms.Add("@ZID", MerchantID.Text);

        if (LeadID.Text != string.Empty)
            prms.Add("@ID", LeadID.Text);

        if(AssignedUserID.SelectedIndex > 0)
            prms.Add("@AssignedUserID",AssignedUserID.SelectedValue);

        if (!string.IsNullOrWhiteSpace(SourceDescription.Text))
            prms.Add("@SourceDescription", SourceDescription.Text);

        if (LeadOriginID.SelectedIndex > 0)
            prms.Add("@LeadOriginID", LeadOriginID.SelectedValue);

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (prms.Count > 0)
        {
            if (DateType.SelectedIndex >= 0)
                prms.Add("@DateType", DateType.SelectedValue);

            //Save search fields in session variable
            lead = new SearchParameter();
            FormBinding.BindControlsToObject(lead, pnlSearch);
            this.SearchParameters = lead;
            
            //user is passed as a parameter to determine whether the user is an agent or manager
            User user = UserSessions.CurrentUser;
            if (user != null)
                prms.Add("@UserName", user.UserName);

            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            GridView1.PageSize = this.PageSize;
            GridView1.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetLeadsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
        }
        else
        {
            // we do this so that we don't call the SP with zero paramters. it's very slow otherwise.
            e.Cancel = true;
        }

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = GridView1.PageIndex + 1;
        }

        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("Leads.xls", GridView1);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string url = "~/SecureLeadForms/frmLeadsDetail.aspx?Adding=true";
        url += "&PostBackURL=~/SecureLeadForms/frmLeads.aspx";
        Response.Redirect(url);
    }

    protected void btnClearLead_Click(object sender, EventArgs e)
    {
        FormHandler.ClearAllControls(pnlDetail);
    }

    protected void btnAddLead_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(DBAName.Text))
            {
                WucMessage1.AddMessageError("DBA/Company is required.");
                return;
            }

            if (Source.SelectedIndex ==0)
            {
                WucMessage1.AddMessageError("Please select a Source.");
                return;
            }


            Lead lead = new Lead();
            //FormBinding.BindControlsToObject(lead, pnlDetail);

            lead.DBAName = DBAName.Text;
            lead.ProcessingVolume = Convert.ToDecimal(ProcessingVolume.Value);
            lead.MonthlyProfit = Convert.ToDecimal(MonthlyProfit.Value);

            Contact c = null;

            if (!string.IsNullOrWhiteSpace(ContactName.Text) && !string.IsNullOrWhiteSpace(ContactLastName.Text))
            {
                c = new Contact();
                c.ContactType = ContactType.Other;
                c.FirstName = ContactName.Text;
                c.LastName = ContactLastName.Text;

                c.PhoneList = new List<Phone>();
                Phone p = new Phone();
                p.PhoneNumber = CommonUtility.Formatting.phone_format_with_ext(CommonUtility.Formatting.ePhoneFormat.proper, PhoneNumber.Text);
                p.PhoneType = PhoneTypes.Work;
                c.PhoneList.Add(p);

                c.EmailAddressList = new List<EmailAddress>();
                EmailAddress ea = new EmailAddress();
                ea.Address = Email.Text;
                ea.Type = EmailAddressTypes.To;
                c.EmailAddressList.Add(ea);

                c.IsPrimary = true;
                c.IsActive = true;
            }

            lead.SourceID = Source.SelectedValue;
            lead.Source = Source.SelectedItem.Text;
            lead.UserCreated = UserSessions.CurrentUser.UserName;
            lead.DateCreated = DateTime.Now;

            if (InsertLead(lead) == true)
            {
                if (c != null)
                {

                    DataContact.InsertLeadContact(lead.LeadID, c);

                    lead.ContactList = new List<Contact>();
                    lead.ContactList.Add(c);
                }

                // send the message to our user.
                WucMessage1.AddMessageStatus("Lead added successfully");

                DataLead data = DataAccess.DataLeadDao;
               Lead emaillead = data.GetLead(lead.LeadUID);

               if (emaillead.AssignedUserID != 0)
                {
                    // send email about assignement
                    StringBuilder emailbody = new StringBuilder();
                    emailbody.Append("<tr><td>" + emaillead.LeadID + "</td>");
                    emailbody.Append("<td>" + emaillead.DBAName + "</td>");
                    emailbody.Append("<td>" + emaillead.Source + "</td>");
                    emailbody.Append("<td>" + emaillead.Email + "</td>");
                    emailbody.Append("<td>" + emaillead.PhoneNumber + "</td></tr>");
                    TicketNotification.SendNewLeadAssignedEmail(emailbody.ToString(), emaillead.AssignedUserID);
                }

                // clear the form.
                FormHandler.ClearAllControls(pnlDetail);

                //rebind our grid.
                grd.DataBind();

            }
        }
        catch (System.Exception exc)
        {
            throw exc;
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        List<Lead> leads = new List<Lead>();
        Lead lead = null;
        DataLead objLead = DataAccess.DataLeadDao;

        if (Leads.FileName.ToString() != "" && (Leads.FileName.ToString().Contains(".xls") || Leads.FileName.ToString().Contains(".xlsx")))
        {
            String filepath = Server.MapPath("UploadedFiles");

            // create the directory if it does not exists!
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            // append the unique user id to the upload to prevent any concurrancy issues
            string str = filepath + "\\" + Leads.PostedFile.FileName.Split('\\')[Leads.PostedFile.FileName.Split('\\').Length - 1] + UserSessions.CurrentUser.UID.ToString();

            // TODO: figure out a surefire way to detect that the destination is writeable!!!
            Leads.SaveAs(str);

            DataTable dt = GetExcelData(str);

            // the amount of columns we expect from a properly formatted excel spreadsheet.
            int expectedMinimumColumnCount = 13;

            int upload_count = 0;
            bool isValid = false;

            if (dt.Rows.Count > 0)
            {
                isValid = objLead.CheckLeadFileExists(Leads.FileName, dt.Rows.Count);
            }
            else
            {
                FormHandler.DisplayMessage(Page.ClientScript, "File is empty.");
                clearUploadControls();
                return;
            }

            if (!isValid) 
            {
                FormHandler.DisplayMessage(Page.ClientScript, "File already exisits with same name and row count.");
                clearUploadControls();
                return;
            }
            
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count >= expectedMinimumColumnCount)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // we only insert if dbaname column is not empty.
                    if (!string.IsNullOrWhiteSpace(dt.Rows[i][0].ToString()))
                    {
                        lead = new Lead();
                        lead.DBAName = dt.Rows[i][0].ToString().Trim();
                        lead.ContactName = dt.Rows[i][1].ToString().Trim();
                        lead.ContactLastName = dt.Rows[i][2].ToString().Trim();

                        string email = dt.Rows[i][4].ToString().Trim();
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            if (CommonUtility.Util.IsValidEmail(email))
                                lead.Email = email;
                            else
                                break;
                        }

                        string vol = dt.Rows[i][5].ToString().Replace("$", "").Trim();
                        if (!string.IsNullOrWhiteSpace(vol))
                        {
                            if (CommonUtility.Util.IsValidDec(vol))
                                lead.ProcessingVolume = DataLayer.Field2Dec(vol);
                            else
                                break;
                        }
                        else
                            lead.ProcessingVolume = 0.0M;

                        int agentid = CommonUtility.Util.if_i(dt.Rows[i][6].ToString().Trim(),0);

                        if(agentid > 0)
                            lead.AgentID = agentid;

                        if (!string.IsNullOrWhiteSpace(dt.Rows[i][7].ToString()))
                        {
                            //CommonUtility.Validation.IsAlphaNumeric(dt.Rows[i][7].ToString(), 20)
                            string phone = dt.Rows[i][7].ToString();

                            if (phone.Length <= 20)
                            {
                                lead.PhoneNumber = dt.Rows[i][7].ToString();
                            }
                            else
                                break;
                        }

                        string url = dt.Rows[i][8].ToString();

                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            if (CommonUtility.Validation.IsWebsite(url,null))
                                lead.Url = url;
                            else
                                break;
                        }
                        
                        lead.Address1 = dt.Rows[i][9].ToString().Trim();
                        lead.City = dt.Rows[i][10].ToString().Trim();
                        lead.State = dt.Rows[i][11].ToString().Trim();
                        lead.Country = dt.Rows[i][12].ToString().Trim();                        

                        lead.SourceID = UploadSource.SelectedValue;
                        lead.SourceDescription = SourceDesc.Text;
                        lead.UserCreated = UserSessions.CurrentUser.UserName;
                        lead.OriginID = CommonUtility.Util.if_i(UploadOrigin.SelectedValue,-1);

                        if (lead != null && leads.Count == i)
                        {
                            Contact newcontact = new Contact();

                            newcontact.FirstName = dt.Rows[i][1].ToString().Trim();
                            newcontact.LastName = dt.Rows[i][2].ToString().Trim();
                            
                            newcontact.PhoneList = new List<Phone>();
                            Phone cphone = new Phone();
                            
                            string phoneno = string.Empty;

                            if (!string.IsNullOrWhiteSpace(dt.Rows[i][3].ToString()))
                            {
                                phoneno = CommonUtility.Validation.ValidatePhoneNumber(dt.Rows[i][3].ToString());

                                if (!string.IsNullOrWhiteSpace(phoneno))
                                {
                                    cphone.PhoneNumber = phoneno;
                                    newcontact.PhoneList.Add(cphone);
                                }
                                else
                                    break;
                            }

                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                if (CommonUtility.Util.IsValidEmail(email))
                                {
                                    newcontact.EmailAddress = email;
                                }
                                else
                                    break;
                            }
                            
                            lead.ContactList = new List<Contact>();
                            lead.ContactList.Add(newcontact);

                            if (dt.Rows[i].ItemArray.Length > 13 && (!string.IsNullOrWhiteSpace(dt.Rows[i][13].ToString())
                            || !string.IsNullOrWhiteSpace(dt.Rows[i][14].ToString())
                            || !string.IsNullOrWhiteSpace(dt.Rows[i][15].ToString())))
                            {
                                PaymentXP.BusinessObjects.Appointment app = new PaymentXP.BusinessObjects.Appointment();

                                if (CommonUtility.Util.IsValidDateTime(dt.Rows[i][13].ToString().Trim()) && CommonUtility.Util.IsValidDateTime(dt.Rows[i][14].ToString().Trim()))
                                {
                                    app.StartDateTime = Convert.ToDateTime(dt.Rows[i][13].ToString().Trim());
                                    app.EndDateTime = Convert.ToDateTime(dt.Rows[i][14].ToString().Trim());
                                    app.Notes = dt.Rows[i][15].ToString().Trim();

                                    lead.LeadAppointments = new List<PaymentXP.BusinessObjects.Appointment>();
                                    lead.LeadAppointments.Add(app);
                                }
                                else
                                    break;

                            }
                        }

                        lead.ReferenceNumber = dt.Rows[i][16].ToString().Trim();
                        lead.BusinessType = dt.Rows[i][17].ToString().Trim();
                        lead.Currency = dt.Rows[i][18].ToString().Trim();
                    }

                    if (lead == null)
                    {
                        FormHandler.DisplayMessage(Page.ClientScript, "File is missing required field, DBA Name.");
                        clearUploadControls();
                        return;
                    }

                    //only if data is valid,. next iteration happens
                    upload_count++;
                    leads.Add(lead);
                }
            }

            if (upload_count > 0 && upload_count == dt.Rows.Count)
            {
                string uploadsource = null;
                if (!this.UploadSource.SelectedValue.Equals("-1"))
                {
                    uploadsource = this.UploadSource.SelectedValue;
                }

                DataTable dtresult = objLead.BulkInsertLeads(MakeLeadsTable(leads), Leads.FileName, dt.Rows.Count, uploadsource);

               int cnt = dtresult.Rows.Count;

                if (cnt > 0)
                {
                    string script = string.Format("alert('{0} Lead(s) added successfully'); window.location.href = 'frmLeads.aspx'; ", upload_count.ToString());
                    ClientScript.RegisterStartupScript(typeof(Page), "DisplayMessage", script, true);
                    SendBulkUploadAssignmentEmail(dtresult);                    
                }
                else
                {
                    FormHandler.DisplayMessage(Page.ClientScript, "File is not in correct format or has invalid data.");
                }

                clearUploadControls();
            }
            else
            {
                FormHandler.DisplayMessage(Page.ClientScript, "File is not in correct format or has invalid data.");
                clearUploadControls();

                return;
            }
           
            // clean up after ourselves. delete file when done.
            File.Delete(str);
        }
    }

    private void SendBulkUploadAssignmentEmail(DataTable dt)
    {
        List<string> AssignedUsers = dt.AsEnumerable().Select(x => x.Field<int?>("AssignedUserID").ToString()).Distinct().ToList();

        if (AssignedUsers.Count > 0)
        {
            foreach (string AssignedUserID in AssignedUsers)
            {
                if (!String.IsNullOrEmpty(AssignedUserID))
                {
                    StringBuilder emailbody = new StringBuilder();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (AssignedUserID == dr["AssignedUserID"].ToString())
                        {
                            emailbody.Append("<tr><td>" + DataLayer.Field2Str(dr["LeadID"]) + "</td>");
                            emailbody.Append("<td>" + DataLayer.Field2Str(dr["DBAName"]) + "</td>");
                            emailbody.Append("<td>" + DataLayer.Field2Str(dr["Source"]) + "</td>");
                            emailbody.Append("<td>" + DataLayer.Field2Str(dr["Email"]) + "</td>");
                            emailbody.Append("<td>" + DataLayer.Field2Str(dr["PhoneNumber"]) + "</td></tr>");
                        }
                    }
                    TicketNotification.SendNewLeadAssignedEmail(emailbody.ToString(), Convert.ToInt32(AssignedUserID));
                }
            }
        }
                
    }

    private void clearUploadControls()
    {
        UploadSource.SelectedIndex = 0;
        SourceDesc.Text = string.Empty;
        UploadOrigin.SelectedIndex = 0;
        SourceDesc.Text = string.Empty;
    }

    private static DataTable MakeLeadsTable(List<Lead> Leads)
    {
        DataTable dtLeads = new DataTable("Leads");

        dtLeads.Columns.Add("LeadsUID", typeof(Guid));
        dtLeads.Columns.Add("DBAName", typeof(string));
        dtLeads.Columns.Add("PhoneNumber", typeof(string));
        dtLeads.Columns.Add("ProcessingVolume", typeof(decimal));
        dtLeads.Columns.Add("AgentID", typeof(Int32));
        dtLeads.Columns.Add("Url", typeof(string));
        dtLeads.Columns.Add("Address1", typeof(string));
        dtLeads.Columns.Add("City", typeof(string));
        dtLeads.Columns.Add("State", typeof(string));
        dtLeads.Columns.Add("Country", typeof(string));
        dtLeads.Columns.Add("DateCreated", typeof(DateTime));
        dtLeads.Columns.Add("UserCreated", typeof(string));
        dtLeads.Columns.Add("SourceID", typeof(Guid));
        dtLeads.Columns.Add("SourceDescription", typeof(string));
        dtLeads.Columns.Add("OriginID", typeof(int));

        //for Lead contacts
        dtLeads.Columns.Add("FirstName", typeof(string));
        dtLeads.Columns.Add("LastName", typeof(string));
        dtLeads.Columns.Add("Phone", typeof(string));
        dtLeads.Columns.Add("Email", typeof(string));

        //for Lead appointments
        dtLeads.Columns.Add("StartDateTime", typeof(DateTime));
        dtLeads.Columns.Add("EndDateTime", typeof(DateTime));
        dtLeads.Columns.Add("Notes", typeof(string));

        dtLeads.Columns.Add("AssignedUserID", typeof(string));
        dtLeads.Columns.Add("StatusUID", typeof(Guid));
        dtLeads.Columns.Add("ReferenceNumber", typeof(string));
        dtLeads.Columns.Add("BusinessType", typeof(string));
        dtLeads.Columns.Add("Currency", typeof(string));

        Guid leadsUID = new Guid();

        foreach (Lead objleads in Leads)
        {
            DataRow dr = dtLeads.NewRow();

            leadsUID = Guid.NewGuid();

            dr["LeadsUID"] = leadsUID;
            dr["DBAName"] = objleads.DBAName;
            dr["PhoneNumber"] = objleads.PhoneNumber;
            dr["ProcessingVolume"] = objleads.ProcessingVolume;
            dr["AgentID"] = objleads.AgentID;
            dr["Url"] = objleads.Url;
            dr["Address1"] = objleads.Address1;
            dr["City"] = objleads.City;
            dr["State"] = objleads.State;
            dr["Country"] = objleads.Country;
            dr["DateCreated"] = DateTime.Now;
            dr["UserCreated"] = objleads.UserCreated;
            if (CommonUtility.Util.IsValidGuid(objleads.SourceID))
                dr["SourceID"] = objleads.SourceID;
            dr["SourceDescription"] = objleads.SourceDescription;
            dr["OriginID"] = objleads.OriginID;

             //Lead contacts
            if (objleads.ContactList != null && objleads.ContactList.Count > 0)
            {
                dr["FirstName"] = objleads.ContactList[0].FirstName;
                dr["LastName"] = objleads.ContactList[0].LastName;
                if(objleads.ContactList[0].PhoneList.Count > 0)
                    dr["Phone"] = objleads.ContactList[0].PhoneList[0].PhoneNumber;
                dr["Email"] = objleads.ContactList[0].EmailAddress;
            }

            //Lead appointments
            if (objleads.LeadAppointments != null && objleads.LeadAppointments.Count > 0)
            {
                dr["StartDateTime"] = objleads.LeadAppointments[0].StartDateTime;
                dr["EndDateTime"] = objleads.LeadAppointments[0].EndDateTime;
                dr["Notes"] = objleads.LeadAppointments[0].Notes;
            }

            dr["ReferenceNumber"] = objleads.ReferenceNumber;
            dr["BusinessType"] = objleads.BusinessType;
            dr["Currency"] = objleads.Currency;

            dtLeads.Rows.Add(dr);
        }

        return dtLeads;

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int i = 0; string LeadId = string.Empty;

        string LeadIds = string.Empty;

        if (AssignedUser.SelectedIndex == 0 && ddpAction.SelectedValue == "Assign")
        {
            lblError1.Text = "Please select a rep.";
            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
        else if (ddpStatus.SelectedIndex == 0 && ddpAction.SelectedValue == "Status")
        {
            lblError1.Text = "Please select a status.";
            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
        else
        {
            for (i = 0; i < grd.Rows.Count; i++)
            {
                CheckBox chk = ((CheckBox)grd.Rows[i].Cells[0].FindControl("chkAssign"));

                if (chk.Checked)
                {
                    LeadIds += grd.DataKeys[i].Values["ID"].ToString() + ",";
                }
            }

            if (LeadIds.Length > 1)
            {

                switch (ddpAction.SelectedValue)
                {
                    case "Assign":

                        LeadFacade.UpdateAssignLeads(int.Parse(AssignedUser.SelectedValue), LeadIds, UserSessions.CurrentUser.UserName);

                        break;

                    case "Status":

                        LeadFacade.UpdateLeadsStatus(ddpStatus.SelectedValue, LeadIds, UserSessions.CurrentUser.UserName);

                        break;
                }
            }

            Search(false);
        }
    }
    public DataTable GetExcelData(string ExcelFilePath)
    {
        DataTable dt = CommonUtility.ExcelHandling.GetExcelData(ExcelFilePath, "Sheet1", null);

        if (dt.Rows.Count > 0)
        {
            dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field == null | object.ReferenceEquals(field, DBNull.Value) | field.Equals(""))).CopyToDataTable();
        }


        return dt;
    }
    //public DataTable GetExcelData(string ExcelFilePath)
    //{
    //    string OledbConnectionString = string.Empty;
    //    OleDbConnection objConn = null;
    //    OledbConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
    //    objConn = new OleDbConnection(OledbConnectionString);

    //    if (objConn.State == ConnectionState.Closed)
    //    {
    //        objConn.Open();
    //    }

    //    OleDbCommand objCmdSelect = new OleDbCommand("Select * from [Sheet1$]", objConn);
    //    OleDbDataAdapter objAdapter = new OleDbDataAdapter();
    //    objAdapter.SelectCommand = objCmdSelect;
    //    DataSet objDataset = new DataSet();
    //    objAdapter.Fill(objDataset, "ExcelDataTable");
    //    objConn.Close();
    //    //return objDataset.Tables[0];
    //    DataTable dt = new DataTable();

    //    if (objDataset.Tables[0].Rows.Count > 0)
    //    {
    //        dt = objDataset.Tables[0].Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field == null | object.ReferenceEquals(field, DBNull.Value) | field.Equals(""))).CopyToDataTable();
    //    }
    //    else
    //    {
    //        dt = objDataset.Tables[0];
    //    }

    //    return dt;
    //}

    private bool InsertLead(Lead lead)
    {
        bool blnRet = false;
        DataLead data = DataAccess.DataLeadDao;

        User user = UserSessions.CurrentUser;
        lead.UserCreated = user.UserName;
        lead.DateCreated = DateTime.Now;
        lead.InitialDate = DateTime.Now;
        lead.StatusID = "410AC5DC-2721-459A-B346-BFB51662E231";
        lead.SourceID = lead.SourceID;
        lead.FollowupStatusID = "C111CF77-BA7B-4393-B208-3B7393C42E92";

        if (user.IsAgent)
        {
            lead.AgentUID = user.AgentUID;
            lead.StatusID = "CA64EB30-8BCC-41E5-B36E-C7948BF6A4D9";
        }

        data.InsertLead(lead);
        if (lead.LeadUID != "-1")
        {
            //UserSessions.CurrentLeadUID = lead.LeadUID;
            blnRet = true;
        }

        return blnRet;
    }

    protected void ddpAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadActions();
    }

    private void LoadActions()
    {
        if (ddpAction.SelectedValue == "Assign")
        {
            lblAction.Visible = false;
            pnlAgents.Visible = true;
            PnlStatus.Visible = false;
            LookupTableHandler.LoadLeadReps(AssignedUser, false);
        }
        else
        {
            lblAction.Visible = true;
            pnlAgents.Visible = false;
            PnlStatus.Visible = true;
            lblAction.Text = "Status:";
            LookupTableHandler.LoadLeadStatus(ddpStatus, false);
        }
    }

    private void grdAgents_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;
        if (e.CommandSource is LinkButton)
        {
            lnk = (LinkButton)e.CommandSource;
        }
        else
        {
            return;
        }

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
        string uid = str[0];

        DataAgent da = new DataAgent();
        Agent app = da.GetAgent(uid);

        if (isSearch)
        {
            AgentUID.Value = app.AgentUID;
            AgentDBA.Text = app.AgentDBA;
            AgentID.Text = app.AgentID.ToString();
        }

        grdAgent.ClearGrid();
        this.dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnAgentSelect_Click(object sender, EventArgs e)
    {
        if (((LinkButton)sender).ID == "lbSelectAgent")
            isSearch = true;
        else
            isSearch = false;

        Hashtable prms = new Hashtable();
        grdAgent.SetDataSource(prms, 10);
        dlgAgent.Modal = false;
        dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

}
