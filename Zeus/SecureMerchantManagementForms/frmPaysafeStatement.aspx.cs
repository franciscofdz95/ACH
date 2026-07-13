using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Reporting.WebForms;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;

public partial class frmPaysafeStatement : System.Web.UI.Page
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.ShowReport();
        }

        if (UserSessions.CurrentMerchantApp != null)
        {
            this.Page.Title = string.Format("DBA: {0} - Statement", UserSessions.CurrentMerchantApp.BusinessDBAName);
        }

    }

    private void ShowReport()
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;

        cmd.CommandText = "sp_SelectMerchantMonthlyInvoice";
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@MerchantID", UserSessions.CurrentMerchantApp.ID);
        cmd.Parameters.AddWithValue("@BillDate", Request["BillDate"]);

        ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        DataView dv = ds.Tables[0].DefaultView;

        if (dv.Table.Rows.Count > 0)
            rptMeritusStatement1.Visible = true;
        else
            rptMeritusStatement1.Visible = false;

        rptMeritusStatement1.Width = new Unit("100%");

        rptMeritusStatement1.Height = new Unit("1056px");

        string reportPath = "Reports/PaysafeStatement.rdlc";

        rptMeritusStatement1.LocalReport.DataSources.Clear();
        rptMeritusStatement1.LocalReport.DataSources.Add(new ReportDataSource("dsMerchantMonthlyInvoice", ds.Tables[0]));
        rptMeritusStatement1.LocalReport.ReportPath = reportPath;
        rptMeritusStatement1.LocalReport.Refresh();
    }

}
