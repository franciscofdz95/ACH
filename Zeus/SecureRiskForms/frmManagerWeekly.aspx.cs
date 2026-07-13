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

public partial class frmManagerWeekly : frmBaseSearch
{
   

    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkRiskReports")).CssClass = "active";

        if (!this.IsPostBack)
        {

            //Apply security settings
            FormHandler.SetSecurity(this.Page);
            rptViewer.Visible = false;
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        DataRisk data = DataAccess.DataRiskDao;
        DataSet ds = null; int RecordCount = 0;

        //ds = data.GetMerchantStatusReport(prms);
        DataView dv = ds.Tables[0].DefaultView;
        RecordCount = dv.Table.Rows.Count;
        dv.Sort = this.SortOrder;
        rptViewer.Visible = (dv.Table.Rows.Count > 0);
        this.ShowReport(ds.Tables[0]);

        lbl.Visible = !(rptViewer.Visible);
    }

    private void ShowReport(DataTable dt)
    {
        rptViewer.Width = new Unit("100%");
        rptViewer.Height = new Unit("1056px");
        string reportPath = "Reports/ManagerWeekly.rdlc";

        rptViewer.LocalReport.DataSources.Clear();
        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatus", dt));
        rptViewer.LocalReport.ReportPath = reportPath;
        rptViewer.LocalReport.Refresh();
    }
}
