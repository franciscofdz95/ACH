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

public partial class frmManagerDaily : frmBaseSearch
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkRiskReports")).CssClass = "active";

        if (!this.IsPostBack)
        {
            //Apply security settings
            FormHandler.SetSecurity(this.Page);
            ManagerDaily1.Visible = false;
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        DataRisk data = DataAccess.DataRiskDao;
        DataSet ds = null; int RecordCount = 0;
        Hashtable prms = new Hashtable();
        ds = data.GetManagerDailyReport(prms);
        DataView dv = ds.Tables[0].DefaultView;
        RecordCount = dv.Table.Rows.Count;
        dv.Sort = this.SortOrder;
        ManagerDaily1.Visible = (dv.Table.Rows.Count > 0);
        this.ShowReport(ds.Tables[0]);
        lbl.Visible = !(ManagerDaily1.Visible);
    }

    private void ShowReport(DataTable dt)
    {
        ManagerDaily1.Width = new Unit("100%");
        ManagerDaily1.Height = new Unit("1056px");
        string reportPath = "Reports/ManagerDaily.rdlc";

        ManagerDaily1.LocalReport.DataSources.Clear();
        ManagerDaily1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatus", dt));
        ManagerDaily1.LocalReport.ReportPath = reportPath;
        ManagerDaily1.LocalReport.Refresh();
    }
}
