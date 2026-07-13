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

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.Shared;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class wucAppointments : wucBaseSearch
{
    public Panel SearchPanel
    {
        get { return pnlSearch; }
    }

    public string SelectedDate
    {
        get
        {
            if (ViewState["SelectedDate"] == null)
                return string.Empty;
            else
                return ViewState["SelectedDate"].ToString();
        }
        set { ViewState["SelectedDate"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (this.SelectedDate != string.Empty)
                WebScheduleInfo1.ActiveDayUtc = WebScheduleInfo1.ConvertTimeZoneTimeToUtc(new SmartDate(this.SelectedDate));
            this.Search(false);
        }

        App.Visible = pnlSearch.Visible;
    }

    public void Search(bool IsOnLoad)
    {
        PaymentXP.BusinessObjects.Appointment app;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (PaymentXP.BusinessObjects.Appointment)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        Hashtable prms = new Hashtable();

        if (IsOnLoad && this.SearchParameters == null)
        {
            prms.Add("@UID", "00000000-0000-0000-0000-000000000000");
        }
        else
        {
            if (DBAName.Text != string.Empty)
                prms.Add("@DBAName", DBAName.Text);

            if (AgentUID.Value != string.Empty)
                prms.Add("@AgentUID", AgentUID.Value);

            if (pnlSearch.Visible)
                prms.Add("@Active", Active.Checked);
            else
                prms.Add("@Active", true);

            //Save search fields
            app = new PaymentXP.BusinessObjects.Appointment();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;
        }

        User user = UserSessions.CurrentUser;
        prms.Add("@UserName", user.UserName);

        DataAppointment data = DataAccess.DataAppointmentDao;
        DataSet ds = null;

        ds = data.GetAppointments(prms);

        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = this.SortOrder;
        lblRecordCount.Text = "Total Records Found: " + dv.Table.Rows.Count.ToString();
        this.LoadAppointments(dv);
    }

    protected void WebScheduleInfo1_ActiveDayChanged(object sender, ActiveDayChangedEventArgs e)
    {
        this.SelectedDate = WebScheduleInfo1.ActiveDayUtc.ToShortDateString();
        Search(false);
    }

    private void LoadAppointments(DataView dv)
    {
        foreach (DataRowView row in dv)
        {
            Infragistics.WebUI.WebSchedule.Appointment app = new Infragistics.WebUI.WebSchedule.Appointment(WebScheduleInfo1);

            app.StartDateTime = new SmartDate(row["StartDateTime"].ToString());
            app.EndDateTime = new SmartDate(row["EndDateTime"].ToString());
            app.Key = row["LeadID"].ToString();
            app.Subject = row["DBAName"].ToString();
            app.Description = row["Notes"].ToString();
            app.EnableReminder = true;

            app.Style.ForeColor = System.Drawing.Color.White;

            if (DataLayer.Field2Bool(row["Active"]))
                app.Style.BackColor = System.Drawing.Color.Green;
            else
                app.Style.BackColor = System.Drawing.Color.Red;

            wmvMain.AppointmentTooltipFormatString = "Time: <START_DATE_TIME>-<END_DATE_TIME><NEW_LINE>DBA : <SUBJECT><NEW_LINE>Notes: <DESCRIPTION>";
            WebScheduleInfo1.Activities.Add(app);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.FormClear();
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        DBAName.Text = string.Empty;
        AgentUID.Value = AgentDBA.Text = AgentID.Text = string.Empty;
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

        AgentUID.Value = str[0];
        DataAgent da = new DataAgent();
        Agent app = da.GetAgent(uid);
        AgentDBA.Text = app.AgentDBA;
        AgentID.Text = app.AgentID.ToString();

        grdAgent.ClearGrid();
        this.dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnAgentSelect_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        grdAgent.SetDataSource(prms, 10);
        dlgAgent.Modal = false;
        dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }
}
