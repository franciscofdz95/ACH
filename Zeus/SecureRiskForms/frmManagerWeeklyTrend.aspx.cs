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
using System.Text;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class frmManagerWeeklyTrend : frmBaseSearch
{
   
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        ManagerWeeklyTrend1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkRiskReports")).CssClass = "active";

        if (!this.IsPostBack)
        {
            //Apply security settings
            FormHandler.SetSecurity(this.Page);
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        DataRisk data = DataAccess.DataRiskDao;
        DataSet ds = null;
        int RecordCount = 0;

        //ds = data.GetMerchantStatusReport(prms);
        DataView dv = ds.Tables[0].DefaultView;
        RecordCount = dv.Table.Rows.Count;
        dv.Sort = this.SortOrder;
        ManagerWeeklyTrend1.Visible = (dv.Table.Rows.Count > 0);
        this.ShowReport(ds.Tables[0]);
        lbl.Visible = !(ManagerWeeklyTrend1.Visible);
    }

    private void ShowReport(DataTable dt)
    {
        ManagerWeeklyTrend1.Width = new Unit("100%");
        ManagerWeeklyTrend1.Height = new Unit("1056px");
        string reportPath = "Reports/ManagerWeeklyTrend.rdlc";

        ManagerWeeklyTrend1.LocalReport.DataSources.Clear();
        ManagerWeeklyTrend1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatus", dt));
        ManagerWeeklyTrend1.LocalReport.ReportPath = reportPath;
        ManagerWeeklyTrend1.LocalReport.Refresh();
    }

    void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
    {
        string reportPath = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;

        switch (e.ReportPath)
        {
            case "ApplicationStatusAch":

                Hashtable prms = new Hashtable();
                StringBuilder sb = new StringBuilder();

                cmd.CommandText = "";
                cmd.CommandType = CommandType.StoredProcedure;
                DataLayer.AppendParamters(cmd, prms);

                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatusAch", ds.Tables[0]));
                break;
        }
    }
}
