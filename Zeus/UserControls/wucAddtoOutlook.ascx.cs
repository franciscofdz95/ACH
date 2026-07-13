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
using System.Text;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

using Infragistics.Web.UI.LayoutControls;
using System.Collections.Generic;
using System.Linq;
using CommonUtility;

public partial class wucAddtoOutlook : wucBaseSearch
{
    public delegate void ButtonClickHandler(object sender, EventArgs e);
    public event ButtonClickHandler ButtonClick;


    public Panel PanelApp
    {
        get { return pnlApp1; }
    }

    public GridView grdApp
    {
        get { return grdAppointments; }
    }

    public Button CloseBtn
    {
        get { return btnClose; }
    }

    public Button btnApp
    {
        get { return btnAddAppointment; }
    }

    public StringBuilder Appoint
    {
        get { if (ViewState[UserSessions.CurrentLead.LeadUID + "Appointment"] != null) return (StringBuilder)ViewState[UserSessions.CurrentLead.LeadUID + "Appointment"]; else return new StringBuilder(); }
        set { ViewState[UserSessions.CurrentLead.LeadUID + "Appointment"] = value; }
    }

    public Panel pnlApp
    {
        get { return pnlAppointments; }
    }

    public TimeZones TimeZoneID
    {
        get { return (TimeZones)ViewState["TimeZoneID"]; }
        set { ViewState["TimeZoneID"] = value; }

    }

    public string LeadID
    {
        get
        {
            if (UserSessions.CurrentLead == null)
            {
                if (!string.IsNullOrEmpty(Session["LeadUID"].ToString()))
                {
                    return Session["LeadUID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (!string.IsNullOrEmpty(UserSessions.CurrentLead.LeadUID))
            {
                return UserSessions.CurrentLead.LeadUID;
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            Session["LeadUID"] = value;
        }
    }

    public string lblError
    {
        get
        {
            if (ViewState["lblError"] != null) return ViewState["lblError"].ToString();
            else return string.Empty;
        }
        set { ViewState["lblError"] = value; }
    }

    private DateTime _defaultEndDate = DateTime.MinValue;
    private DateTime _defaultStartDate = DateTime.MinValue;
    private string _defaultNotes = string.Empty;

    public DateTime DefaultEndDate
    {
        get { return _defaultEndDate; }
        set { _defaultEndDate = value; }
    }

    public string DefaultNotes
    {
        get { return _defaultNotes; }
        set { _defaultNotes = value; }
    }

    public DateTime DefaultStartDate
    {
        get { return _defaultStartDate; }
        set { _defaultStartDate = value; }
    }

    private bool _SoftLinkDates = false;

    /// <summary>
    /// if this is true, the end date will match the start date, and the end time will be a 1/2 increment of the start time. 
    /// </summary>
    public bool SoftLinkDates
    {
        get { return _SoftLinkDates; }
        set { _SoftLinkDates = value; }
    }

    public void ClearGrid()
    {
        grdAppointments.DataSource = null;
        grdAppointments.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // i could not get the infragistics date to load during postback. 
        // only way i could get it to display was if it was set on the prerender
        this.Page.PreRender += new EventHandler(Page_PreRender);

        if (!IsPostBack)
        {
            this.TimeZoneID = UserSessions.CurrentUser.TimeZone;

            // set default start/end times to today on initial load
            StartDateTime.Value = DateTime.Today;
            WebUtil.SetUserSpecificDisplayMode(StartDateTime);

            EndDateTime.Value = DateTime.Today;
            WebUtil.SetUserSpecificDisplayMode(EndDateTime);
        }

        if (this.SoftLinkDates == true)
        {
            StartDateTime.ValueChanged += new Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventHandler(StartDateTime_ValueChanged);
        }

        if (grdAppointments.Rows.Count == 0)
            LoadEmptyGrid();
    }

    private bool _onStart = true;

    protected void StartDateTime_ValueChanged(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
    {
        DateTime dtstart = (DateTime)StartDateTime.Value;
        dtstart.AddMinutes(30);

        EndDateTime.Value = dtstart;

        DateTime dt1 = DateTime.Parse(StartTime.SelectedItem.Text);
        DateTime dt2 = new DateTime();

        dt2 = dt1.AddMinutes(30);

        EndTime.SelectedValue = dt2.Hour.ToString().PadLeft(2, '0') + dt2.Minute.ToString().PadLeft(2, '0') + "00";

        this._onStart = false;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            // on postback, we set the default start date. this case arises when you have the "outlook"
            // button embedded in a gridview when its rendered on a postback.
            if (this._defaultStartDate != null && this._defaultStartDate != DateTime.MinValue)
            {
                StartDateTime.Value = this._defaultStartDate;
                WebUtil.SetUserSpecificDisplayMode(StartDateTime);
            }

            if (this._defaultEndDate != null && this._defaultEndDate != DateTime.MinValue)
            {
                EndDateTime.Value = this._defaultEndDate;
                WebUtil.SetUserSpecificDisplayMode(EndDateTime);
            }

            if (this._defaultNotes != null && this._defaultNotes != string.Empty)
            {
                txtAppointmentNotes.Text = this._defaultNotes;
            }

            if (this._onStart == true && this._SoftLinkDates == true)
            {

                int hour = DateTime.Now.AddHours(1).Hour;
                string hour_string = hour.ToString().PadLeft(2, '0');
                StartTime.SelectedValue = hour_string + "0000";
                EndTime.SelectedValue = hour_string + "3000";
                this._onStart = false;
            }
        }
    }

    protected void btnAddAppointment_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.AddApppointment())
            {
                StartDateTime.Value = DateTime.Today;
                EndDateTime.Value = DateTime.Today;
                StartTime.SelectedIndex = 0;
                EndTime.SelectedIndex = 0;
                txtAppointmentNotes.Text = string.Empty;

                Appoint = LoadAppointment();
                Response.ContentType = "text/calendar";
                Response.AddHeader("content-disposition", "attachment; filename=CalendarEvent1.ics");
                Response.Write(this.Appoint);
                this.Appoint = new StringBuilder();
                Response.End();

            }
            else
            {
                this.LoadAppointments();
                // could not add the appointment. raise an error message.
                //btnAddAppointment.OnClientClick = "alert('" + lblError + "'); btnAddAppointment.OnClientClick();";
            }
        }
        catch (System.Exception exc)
        {
            throw exc;
        }
       
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        StartDateTime.Value = DateTime.Today;
        EndDateTime.Value = DateTime.Today;
        StartTime.SelectedIndex = 0;
        EndTime.SelectedIndex = 0;
        txtAppointmentNotes.Text = string.Empty;

        if (((Button)sender).NamingContainer.Parent.NamingContainer is WebDialogWindow)
        {
            ((WebDialogWindow)((Button)sender).NamingContainer.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
            ErrorMess.Visible = false;
            lblError = string.Empty;
            if (this.ButtonClick != null)
                this.ButtonClick(sender, e);
        }
        else
            LoadAppointments();

        //ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        ErrorMess.Text = "";
        ErrorMess.Visible = false;
    }

    private StringBuilder LoadAppointment()
    {
        //StringBuilder sbICSFile = new StringBuilder();
        //DateTime dtNow = DateTime.Now;

        //sbICSFile.AppendLine("BEGIN:VCALENDAR");
        //sbICSFile.AppendLine("PRODID:-//ICSTestCS/");
        //sbICSFile.AppendLine("CALSCALE:GREGORIAN");

        //// Define the event.
        //sbICSFile.AppendLine("BEGIN:VEVENT");

        //sbICSFile.Append("DTSTART;TZID=US / Pacific:");
        //sbICSFile.Append(Convert.ToDateTime(StartDateTime.Value).Year.ToString());
        //sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(StartDateTime.Value).Month));
        //sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(StartDateTime.Value).Day) + "T");
        //sbICSFile.AppendLine(StartTime.SelectedValue);

        //sbICSFile.Append("DTEND;TZID=US/Pacific:");
        //sbICSFile.Append(Convert.ToDateTime(EndDateTime.Value).Year);
        //sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(EndDateTime.Value).Month));
        //sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(EndDateTime.Value).Day) + "T");
        //sbICSFile.AppendLine(EndTime.SelectedValue);

        //sbICSFile.AppendLine("SUMMARY:" + "New Appointment for " + UserSessions.CurrentLeadDBA);
        //sbICSFile.AppendLine("BEGIN:VALARM" + Environment.NewLine);
        //sbICSFile.AppendLine("TRIGGER:-PT15M" + Environment.NewLine);
        //sbICSFile.AppendLine("ACTION:DISPLAY" + Environment.NewLine);

        //sbICSFile.AppendLine("DESCRIPTION:" + txtAppointmentNotes.Text+";ENCODING=QUOTED-PRINTABLE:=0A"+ Environment.NewLine); 
        //sbICSFile.AppendLine("END:VALARM" + Environment.NewLine);
        //sbICSFile.AppendLine("UID:1");
        //sbICSFile.AppendLine("SEQUENCE:0");
        //sbICSFile.AppendLine("DTSTAMP:" + dtNow.Year.ToString());
        //sbICSFile.AppendLine(FormatDateTimeValue(dtNow.Month));
        //sbICSFile.AppendLine(FormatDateTimeValue(dtNow.Day) + "T");
        //sbICSFile.Append(FormatDateTimeValue(dtNow.Hour));
        //sbICSFile.AppendLine(FormatDateTimeValue(dtNow.Minute) + "00");

        //sbICSFile.AppendLine("END:VEVENT");
        //sbICSFile.AppendLine("END:VCALENDAR");


        System.Text.StringBuilder sbICSFile = new System.Text.StringBuilder();
        DateTime dtNow = DateTime.Now;

        sbICSFile.AppendLine("BEGIN:VCALENDAR");
        sbICSFile.AppendLine("VERSION:2.0");
        sbICSFile.AppendLine("PRODID:-//ICSTestCS/");
        sbICSFile.AppendLine("CALSCALE:GREGORIAN");

        //We are doing this because there is no way we can convert Dot Net TimeZones to Olson TZID.
        //One timezone in dotnet can map to multiple olson TZIDs and it is not worth to create a mapping in our application for just this page.
        //Our users at this time are only in 4 timezones.
        string tzid;
        switch (this.TimeZoneID)
        {
            case TimeZones.EasternStandardTime:
                //US- Eastern.
                sbICSFile.AppendLine("BEGIN:VTIMEZONE");
                sbICSFile.AppendLine("TZID:US/Eastern");
                sbICSFile.AppendLine("BEGIN:STANDARD");
                sbICSFile.AppendLine("DTSTART:20071104T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=1SU;BYMONTH=11");
                sbICSFile.AppendLine("TZOFFSETFROM:-0400");
                sbICSFile.AppendLine("TZOFFSETTO:-0500");
                sbICSFile.AppendLine("TZNAME:EST");
                sbICSFile.AppendLine("END:STANDARD");
                sbICSFile.AppendLine("BEGIN:DAYLIGHT");
                sbICSFile.AppendLine("DTSTART:20070311T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=2SU;BYMONTH=3");
                sbICSFile.AppendLine("TZOFFSETFROM:-0500");
                sbICSFile.AppendLine("TZOFFSETTO:-0400");
                sbICSFile.AppendLine("TZNAME:EDT");
                sbICSFile.AppendLine("END:DAYLIGHT");
                sbICSFile.AppendLine("END:VTIMEZONE");
                tzid = "TZID=US/Eastern:";
                break;

            case TimeZones.CentralStandardTime:
                //US- Central.
                sbICSFile.AppendLine("BEGIN:VTIMEZONE");
                sbICSFile.AppendLine("TZID:US/Central");
                sbICSFile.AppendLine("BEGIN:STANDARD");
                sbICSFile.AppendLine("DTSTART:20071104T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=1SU;BYMONTH=11");
                sbICSFile.AppendLine("TZOFFSETFROM:-0500");
                sbICSFile.AppendLine("TZOFFSETTO:-0600");
                sbICSFile.AppendLine("TZNAME:CST");
                sbICSFile.AppendLine("END:STANDARD");
                sbICSFile.AppendLine("BEGIN:DAYLIGHT");
                sbICSFile.AppendLine("DTSTART:20070311T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=2SU;BYMONTH=3");
                sbICSFile.AppendLine("TZOFFSETFROM:-0600");
                sbICSFile.AppendLine("TZOFFSETTO:-0500");
                sbICSFile.AppendLine("TZNAME:CDT");
                sbICSFile.AppendLine("END:DAYLIGHT");
                sbICSFile.AppendLine("END:VTIMEZONE");
                tzid = "TZID=US/Central:";
                break;

            case TimeZones.GMTStandardTime:
                tzid = "TZID=Europe/London:";
                 sbICSFile.AppendLine("BEGIN:VTIMEZONE");
                sbICSFile.AppendLine("TZID:Europe/London");
                sbICSFile.AppendLine("BEGIN:STANDARD");
                sbICSFile.AppendLine("DTSTART:19961027T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=-1SU;BYMONTH=10");
                sbICSFile.AppendLine("TZOFFSETFROM:+0100");
                sbICSFile.AppendLine("TZOFFSETTO:+0000");
                sbICSFile.AppendLine("TZNAME:GMT");
                sbICSFile.AppendLine("END:STANDARD");
                sbICSFile.AppendLine("BEGIN:DAYLIGHT");
                sbICSFile.AppendLine("DTSTART:19810329T010000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=-1SU;BYMONTH=3");
                sbICSFile.AppendLine("TZOFFSETFROM:+0000");
                sbICSFile.AppendLine("TZOFFSETTO:+0100");
                sbICSFile.AppendLine("TZNAME:GMT+01:00");
                sbICSFile.AppendLine("END:DAYLIGHT");
                sbICSFile.AppendLine("END:VTIMEZONE");
                break;

            default:
                // US/Pacific
                sbICSFile.AppendLine("BEGIN:VTIMEZONE");
                sbICSFile.AppendLine("TZID:US/Pacific");
                sbICSFile.AppendLine("BEGIN:STANDARD");
                sbICSFile.AppendLine("DTSTART:20071104T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=1SU;BYMONTH=11");
                sbICSFile.AppendLine("TZOFFSETFROM:-0700");
                sbICSFile.AppendLine("TZOFFSETTO:-0800");
                sbICSFile.AppendLine("TZNAME:PST");
                sbICSFile.AppendLine("END:STANDARD");
                sbICSFile.AppendLine("BEGIN:DAYLIGHT");
                sbICSFile.AppendLine("DTSTART:20070311T020000");
                sbICSFile.AppendLine("RRULE:FREQ=YEARLY;BYDAY=2SU;BYMONTH=3");
                sbICSFile.AppendLine("TZOFFSETFROM:-0800");
                sbICSFile.AppendLine("TZOFFSETTO:-0700");
                sbICSFile.AppendLine("TZNAME:PDT");
                sbICSFile.AppendLine("END:DAYLIGHT");
                sbICSFile.AppendLine("END:VTIMEZONE");
                tzid = "TZID=US/Pacific:";
                break;

        }

        // Define the event.
        sbICSFile.AppendLine("BEGIN:VEVENT");

       
        sbICSFile.Append("DTSTART;"+tzid);
        sbICSFile.Append(Convert.ToDateTime(StartDateTime.Value).Year.ToString());
        sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(StartDateTime.Value).Month));
        sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(StartDateTime.Value).Day) + "T");
        sbICSFile.AppendLine(StartTime.SelectedValue);
        sbICSFile.AppendLine();

        sbICSFile.Append("DTEND;"+tzid);
        sbICSFile.Append(Convert.ToDateTime(EndDateTime.Value).Year);
        sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(EndDateTime.Value).Month));
        sbICSFile.Append(FormatDateTimeValue(Convert.ToDateTime(EndDateTime.Value).Day) + "T");
        sbICSFile.AppendLine(EndTime.SelectedValue);

        sbICSFile.AppendLine("SUMMARY:" + "New Appointment for " + UserSessions.CurrentLead.DBAName);
        //sbICSFile.AppendLine("DESCRIPTION:" + );
        sbICSFile.AppendLine("BEGIN:VALARM" + Environment.NewLine);
        sbICSFile.AppendLine("TRIGGER:-PT30M" + Environment.NewLine);
        sbICSFile.AppendLine("ACTION:DISPLAY" + Environment.NewLine);

        sbICSFile.AppendLine("DESCRIPTION:" + txtAppointmentNotes.Text);
        sbICSFile.AppendLine("UID:1");
        sbICSFile.AppendLine("SEQUENCE:0");

        sbICSFile.Append("DTSTAMP:" + dtNow.Year.ToString());
        sbICSFile.Append(FormatDateTimeValue(dtNow.Month));
        sbICSFile.Append(FormatDateTimeValue(dtNow.Day) + "T");
        sbICSFile.Append(FormatDateTimeValue(dtNow.Hour));
        sbICSFile.AppendLine(FormatDateTimeValue(dtNow.Minute) + "00");

        sbICSFile.AppendLine("BEGIN:VALARM" + Environment.NewLine);
        sbICSFile.AppendLine("TRIGGER:-PT15M" + Environment.NewLine);
        sbICSFile.AppendLine("ACTION:DISPLAY" + Environment.NewLine);
        sbICSFile.AppendLine("DESCRIPTION:Reminder" + Environment.NewLine);
        sbICSFile.AppendLine("END:VALARM" + Environment.NewLine);

        sbICSFile.AppendLine("END:VEVENT");
        sbICSFile.AppendLine("END:VCALENDAR");

        return sbICSFile;
    }

    private string FormatDateTimeValue(int DateValue)
    {
        if (DateValue < 10)
            return "0" + DateValue.ToString();
        else
            return DateValue.ToString();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (hdnAppId.Value != string.Empty)
        {
            if (Convert.ToDateTime(igtbl_StartDateTime.Text) <= Convert.ToDateTime(igtbl_EndDateTime.Text))
            {
                UpdateApppointments(Convert.ToDateTime(igtbl_StartDateTime.Text), Convert.ToDateTime(igtbl_EndDateTime.Text), txtAppNotes.Text.Trim(), chkAppNotes.Checked);
            }
            else
                appErr.Text = "Invalid Date";
        }

        if (appErr.Text == string.Empty)
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    private void UpdateApppointments(DateTime dt1, DateTime dt2, string notes, bool isActive)
    {
        Appointment app = new Appointment();
        DataAppointment data = DataAccess.DataAppointmentDao;

        if (hdnAppId.Value != string.Empty)
        {
            app.AppointmentID = hdnAppId.Value;
            app.StartDateTime = dt1;
            app.EndDateTime = dt2;
            app.Notes = notes;
            app.Active = isActive;

            User user = UserSessions.CurrentUser;
            app.UserUpdated = user.UserName;
            app.UserCreated = user.UserName;
            app.LeadID = LeadID;

            data.UpdateAppointment(app);
            LoadAppointments();
        }
    }

    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        appErr.Text = "";
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void grdAppointments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string notes = "";
        GridViewRow grdRow = null;

        if (e.CommandSource is LinkButton)
            grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        else if (e.CommandSource is ImageButton)
            grdRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        else
            return;

        hdnAppId.Value = ((LinkButton)grdRow.FindControl("lnkID")).CommandArgument.Trim();

        switch (e.CommandName)
        {
            case "ID":

                igtbl_StartDateTime.Text = grdRow.Cells[1].Text.Trim();
                igtbl_EndDateTime.Text = grdRow.Cells[2].Text.Trim();
                if (grdRow.Cells[3].Text.Equals(' ') || grdRow.Cells[3].Text.Equals("&nbsp;"))
                    txtAppNotes.Text = string.Empty;
                else
                    txtAppNotes.Text = grdRow.Cells[3].ToolTip;
                chkAppNotes.Checked = true;
                appErr.Text = "";
                WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;

            case "Active":

                if (grdRow.Cells[3].Text.Equals(' ') || grdRow.Cells[3].Text.Equals("&nbsp;"))
                    notes = string.Empty;
                else
                    notes = grdRow.Cells[3].ToolTip;
                UpdateApppointments(Convert.ToDateTime(grdRow.Cells[1].Text.Trim()), Convert.ToDateTime(grdRow.Cells[2].Text.Trim()), notes, false);
                break;
        }
    }

    protected void grdAppointments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdAppointments.PageIndex = e.NewPageIndex;
        this.LoadAppointments();
    }

    protected void grdAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                if (grdAppointments.DataSource is DataSet)
                {
                    ((LinkButton)e.Row.Cells[0].FindControl("lnkID")).Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                    ((LinkButton)e.Row.Cells[0].FindControl("lnkID")).CommandArgument = DataBinder.Eval(e.Row.DataItem, "AppointmentID").ToString();

                    if (e.Row.Cells[3].Text.Equals("&nbsp;"))
                        e.Row.Cells[3].ToolTip = "";
                    else
                        e.Row.Cells[3].ToolTip = e.Row.Cells[3].Text;
                    e.Row.Cells[3].Text = "<div style='width:90px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;'>" + e.Row.Cells[3].Text + "</div>";
                }

                e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text); //StartDate Time
                e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text); //EndDate Time
                e.Row.Cells[4].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[4].Text); //Crated Datetime

                break;
            default:
                break;
        }
    }

    public bool AddApppointment()
    {
        try
        {
            string note = Server.HtmlEncode(txtAppointmentNotes.Text.Trim());
            string message = string.Empty;

            if (!datacheck())
            {
                ErrorMess.Text = lblError;
                ErrorMess.Visible = true;
                if (this.ButtonClick != null)
                    this.ButtonClick(btnAddAppointment, new EventArgs());
                return false;
            }

            if (note == string.Empty)
            {
                lblError = "Please add a note to the appointment.<br>";
                ErrorMess.Text = lblError;
                ErrorMess.Visible = true;
                if (this.ButtonClick != null)
                    this.ButtonClick(btnAddAppointment, new EventArgs());
                return false;
            }

            Appointment app = new Appointment();
            DataAppointment data = DataAccess.DataAppointmentDao;

            if (StartDateTime.Value != null && EndDateTime.Value != null && LeadID != string.Empty)
            {
                
                app.StartDateTime = Convert.ToDateTime(Convert.ToDateTime(StartDateTime.Value).ToString("MM/dd/yyyy") + " " + StartTime.SelectedItem.Text);
                app.EndDateTime = Convert.ToDateTime(Convert.ToDateTime(EndDateTime.Value).ToString("MM/dd/yyyy") + " " + EndTime.SelectedItem.Text);
                app.Notes = note;
                User user = UserSessions.CurrentUser;
                app.UserUpdated = user.UserName;
                app.UserCreated = user.UserName;
                app.LeadID = LeadID;

                data.InsertAppointment(app,TimeZoneID);

                Hashtable prms = new Hashtable();
                prms.Add("@LeadsUID", LeadID);
                prms.Add("@Active", true);
                DataSet ds = data.GetAppointments(prms);
                DateTime minfollowup = new DateTime();

                //Get the min date.
                if (ds.Tables[0].Rows.Count > 0)
                {
                    minfollowup = ds.Tables[0].AsEnumerable().Where(p => p["StartDateTime"] != DBNull.Value).Min(a => DateTime.Parse(a["StartDateTime"].ToString()));
                }

                //Update the lead followup date to the earliest appointment after present time.
                Lead objLead = new Lead();
                DataLead objdataLead = DataAccess.DataLeadDao;
                objLead = objdataLead.GetLead(LeadID);
                objLead.FollowUpDate = minfollowup;
                objdataLead.UpdateLead(objLead);

                return true;
            }

            return false;
        }
        catch (System.Exception exc)
        {
            throw exc;
        }
    }

    public bool datacheck()
    {
        string message = string.Empty;

        DateTime startdt = Convert.ToDateTime(Convert.ToDateTime(StartDateTime.Value).ToString("MM/dd/yyyy") + " " + StartTime.SelectedItem.Text);
        DateTime enddt = Convert.ToDateTime(Convert.ToDateTime(EndDateTime.Value).ToString("MM/dd/yyyy") + " " + EndTime.SelectedItem.Text);

        if (startdt < DateTime.Today)
            message = "Please select a future date.<br>";
        else if (startdt > enddt)
            message = "End datetime must be greater than start datetime.<br>";

        if (!message.Equals(string.Empty))
        {
            lblError = message;
            return false;
        }
        else
        {
            lblError = "";
            return true;
        }
    }

    public void LoadAppointments()
    {
        lblRecordCount.Text = "";
        if (LeadID != string.Empty)
        {
            //Lead lead = DataAccess.DataLeadDao.GetLead(LeadID);
            DataAppointment data = DataAccess.DataAppointmentDao;
            Hashtable prms = new Hashtable();

            prms.Add("@LeadsUID", LeadID);
            prms.Add("@Active", true);

            DataSet ds = data.GetAppointments(prms);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdAppointments.DataSource = ds;
                grdAppointments.DataBind();
                grdAppointments.Visible = true;
                grd.Visible = false;
                lblRecordCount.Visible = true;
                lblRecordCount.Text = "Total records found:" + ds.Tables[0].Rows.Count.ToString();
            }

            else
                LoadEmptyGrid();
        }
    }

    protected void StartTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this._SoftLinkDates == true)
        {
            DateTime dtSt = new DateTime();
            dtSt = DateTime.Parse(StartTime.SelectedItem.Text);

            DateTime dtEn = new DateTime();
            dtEn = dtSt.AddMinutes(30);

            EndTime.SelectedValue = dtEn.Hour.ToString().PadLeft(2, '0') + dtEn.Minute.ToString().PadLeft(2, '0') + "00";

            this._onStart = false;
        }
    }

    private void LoadEmptyGrid()
    {
        IList<Appointment> lstAppointments = new List<Appointment>();

        lstAppointments.Add(new Appointment());
        grd.Visible = true;
        grd.DataSource = lstAppointments;
        grd.DataBind();

        grdAppointments.Visible = false;
        grdAppointments.DataSource = null;
        grdAppointments.DataBind();

        int columnsCount = grd.Columns.Count;
        grd.Rows[0].Cells.Clear();

        // clear all the cells in the row      
        grd.Rows[0].Cells.Add(new TableCell());

        //add a new blank cell      
        grd.Rows[0].Cells[0].ColumnSpan = columnsCount;
        grd.Rows[0].Cells[0].CssClass = "EmptyDataRowStyle";

        //set No Results found to the new added cell       
        grd.Rows[0].Cells[0].Text = ".....No follow ups for the lead....";
        grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

        lblRecordCount.Visible = false;
    }

}
