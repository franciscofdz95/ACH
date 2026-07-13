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
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;
using Infragistics.WebUI.WebSchedule;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;


public partial class frmReportsViewer : frmBaseDataEntry
{

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        LeadsByStatus1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        this.ShowParameters();

        if (!this.IsPostBack)
        {
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Reports > " + Request.QueryString["Report"].ToString();

            LeadsByStatus1.Visible = false;
        }
    }

    private void ShowParameters()
    {
        string report = Request.QueryString["Report"].ToString();
        switch (report)
        {
            case "Leads By Status":
            case "Leads By Agent":
            case "Leads By Source":
                PlaceHolder1.Controls.Add(this.Page.LoadControl("../ReportPanel/pnlDates.ascx"));
                break;
        }

        lblTitle.Text = report + " Report";
    }

    private void ShowReport()
    {
        string reportPath = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;
        
        string report = Request.QueryString["Report"].ToString();

        WebDateChooser StartDate = null;
        WebDateChooser EndDate = null;

        LeadsByStatus1.Width = new Unit("816px");
        
        LeadsByStatus1.Height  = new Unit("1056px");

        switch (report)
        {
            case "Leads By Status":
                StartDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("StartDate");
                EndDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("EndDate");
                
                reportPath = "Reports/LeadsByStatus.rdlc";
                cmd.CommandText = "sp_ReportLeads";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AssignedStartDate", StartDate.Value.ToString()));
                cmd.Parameters.Add(new SqlParameter("@AssignedEndDate", EndDate.Value.ToString()));

                if (UserSessions.CurrentUser.AccessLevelUID.ToUpper() == "7b824322-b5a6-4abf-8810-a29ff271d8b6".ToUpper())
                    cmd.Parameters.Add(new SqlParameter("@AgentUID", UserSessions.CurrentUser.AgentUID));

                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                LeadsByStatus1.LocalReport.DataSources.Clear();
                LeadsByStatus1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportLeads", ds.Tables[0]));
                LeadsByStatus1.LocalReport.ReportPath = reportPath;
                LeadsByStatus1.LocalReport.Refresh();
                break;

            case "Leads By Agent":
                StartDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("StartDate");
                EndDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("EndDate");

                reportPath = "Reports/LeadsByAgent.rdlc";
                cmd.CommandText = "sp_ReportLeads";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AssignedStartDate", StartDate.Value.ToString()));
                cmd.Parameters.Add(new SqlParameter("@AssignedEndDate", EndDate.Value.ToString()));

                if (UserSessions.CurrentUser.AccessLevelUID.ToUpper() == "7b824322-b5a6-4abf-8810-a29ff271d8b6".ToUpper())
                    cmd.Parameters.Add(new SqlParameter("@AgentUID", UserSessions.CurrentUser.AgentUID));
                
                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                LeadsByStatus1.LocalReport.DataSources.Clear();
                LeadsByStatus1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportLeads", ds.Tables[0]));
                LeadsByStatus1.LocalReport.ReportPath = reportPath;
                LeadsByStatus1.LocalReport.Refresh();
                break;

            case "Leads By Source":
                StartDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("StartDate");
                EndDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("EndDate");

                reportPath = "Reports/LeadsBySource.rdlc";
                cmd.CommandText = "sp_ReportLeadsBySource";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AssignedStartDate", StartDate.Value.ToString()));
                cmd.Parameters.Add(new SqlParameter("@AssignedEndDate", EndDate.Value.ToString()));

                if (UserSessions.CurrentUser.AccessLevelUID.ToUpper() == "7b824322-b5a6-4abf-8810-a29ff271d8b6".ToUpper())
                    cmd.Parameters.Add(new SqlParameter("@AgentUID", UserSessions.CurrentUser.AgentUID));
                                
                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                LeadsByStatus1.LocalReport.DataSources.Clear();
                LeadsByStatus1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportLeadsBySource", ds.Tables[0]));
                LeadsByStatus1.LocalReport.ReportPath = reportPath;
                LeadsByStatus1.LocalReport.Refresh();
                break;

            case "Residual Income":
                reportPath = "Reports/ResidualIncome.rdlc";
                cmd.CommandText = "sp_Report_Residuals_Income";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RunID", 9));
                cmd.Parameters.Add(new SqlParameter("@AgentID", 1052));

                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                LeadsByStatus1.LocalReport.DataSources.Clear();
                LeadsByStatus1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_Report_Residuals_Income", ds.Tables[0]));
                LeadsByStatus1.LocalReport.ReportPath = reportPath;

                LeadsByStatus1.LocalReport.Refresh();
                break;
        }

        LeadsByStatus1.ShowPrintButton = true;
    }

    void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
    {
        string reportPath = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;
        WebDateChooser StartDate = null;
        WebDateChooser EndDate = null;

        switch (e.ReportPath)
        {
            case "LeadsBySource_Worked":
            case "LeadsBySource_BadLeads":

                StartDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("StartDate");
                EndDate = (WebDateChooser)PlaceHolder1.Controls[0].FindControl("EndDate");

                cmd.CommandText = "sp_ReportLeadsBySource_Worked";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@AssignedStartDate", StartDate.Value.ToString()));
                cmd.Parameters.Add(new SqlParameter("@AssignedEndDate", EndDate.Value.ToString()));

                if (UserSessions.CurrentUser.AccessLevelUID.ToUpper() == "7b824322-b5a6-4abf-8810-a29ff271d8b6".ToUpper())
                    cmd.Parameters.Add(new SqlParameter("@AgentUID", UserSessions.CurrentUser.AgentUID));

                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportLeadsBySource_Worked", ds.Tables[0]));
                break;              
        }
    }

    protected void btnViewReport_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        LeadsByStatus1.Visible = true;
        this.ShowReport();
    }

  

    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
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
        throw new Exception("The method or operation is not implemented.");
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
