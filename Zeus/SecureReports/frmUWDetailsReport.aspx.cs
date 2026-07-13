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

public partial class frmUWDetailsReport : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Today.ToString("MM/01/yyyy"));
            SearchBeginDate.Value = dt;
            SearchEndDate.Value = DateTime.Today;
            UWDetails1.Visible = false;
        }
    }

    public override void Search(bool IsOnLoad)
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        DataSet ds = null;
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);


        ds = data.GetUWDetailsReport(prms);
        DataView dv = ds.Tables[0].DefaultView;
        DataSet ds2 = data.GetUWDetailDeclinesReport(prms);

        if (dv.Table.Rows.Count > 0)
            UWDetails1.Visible = true;
        else
            UWDetails1.Visible = false;

        UWDetails1.Width = new Unit("100%");
        UWDetails1.Height = new Unit("1056px");

        string reportPath = "Reports/UWDetails.rdlc";
        UWDetails1.LocalReport.DataSources.Clear();
        UWDetails1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_Report_UW_Details", ds.Tables[0]));
        UWDetails1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_Report_UW_Details_Declines", ds2.Tables[0]));
        UWDetails1.LocalReport.ReportPath = reportPath;
        UWDetails1.LocalReport.Refresh();
    }

    //private void FormClear()
    //{
    //    //FormHandler.ClearAllControls(this);
    //    //DateTime dt = Convert.ToDateTime(DateTime.Today.ToString("MM/01/yyyy"));
    //    //SearchBeginDate.Value = dt.Date;
    //    //SearchEndDate.Value = DateTime.Today;


    //}

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
    }

    public void ToggleButtons()
    {
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //this.FormClear();
        Response.Redirect("~/SecureReports/frmUWDetailsReport.aspx");
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.Search(false);
    }
}
