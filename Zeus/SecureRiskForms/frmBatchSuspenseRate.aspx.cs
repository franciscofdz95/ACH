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

public partial class frmBatchSuspenseRate : frmBaseSearch
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

        ((HyperLink)this.Master.FindControl("lnkRiskReports")).CssClass = "active";

        if (!this.IsPostBack)
        {
            SearchBeginDate.Value = DateTime.Today.ToShortDateString();
            SearchEndDate.Value = DateTime.Today.ToShortDateString();

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists

            LookupTableHandler.MerchantAppStatusList(lstStatus, true, "Merchant Management");
            lstStatus.Items.Insert(0, new ListItem("All", "-1"));

            LookupTableHandler.RiskExceptionList(lstReason, true);
            BatchSuspenseRate1.Visible = false;

            this.Search(true);
        }
        lblError.Text = "";
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;
        int RecordCount = 0;
        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);

        if (lstStatus.SelectedIndex > 0)
            prms.Add("@StatusUID", lstStatus.SelectedItem.Value);

        if (lstReason.SelectedIndex > 0)
            prms.Add("@RiskID", lstReason.SelectedItem.Value);

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            DataRisk data = DataAccess.DataRiskDao;
            DataSet ds = null;

            ds = data.GetBatchSuspenseRate(prms);
            DataView dv = ds.Tables[0].DefaultView;
            RecordCount = dv.Table.Rows.Count;
            dv.Sort = this.SortOrder;
            BatchSuspenseRate1.Visible = (dv.Table.Rows.Count > 0);
            this.ShowReport(ds.Tables[0]);
        }
        lbl.Visible = !(BatchSuspenseRate1.Visible);
    }

    private void ShowReport(DataTable dt)
    {
        BatchSuspenseRate1.Width = new Unit("100%");
        BatchSuspenseRate1.Height = new Unit("1056px");
        string reportPath = "Reports\\BatchSuspenseRate.rdlc";

        BatchSuspenseRate1.LocalReport.DataSources.Clear();
        BatchSuspenseRate1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2_sp_ReportBatchSuspenseRates", dt));
        BatchSuspenseRate1.LocalReport.ReportPath = reportPath;
        BatchSuspenseRate1.LocalReport.Refresh();

        ReportParameter[] repParamHeader = new ReportParameter[4];

        string str = "Begin Date: " + SearchBeginDate.Text;
        repParamHeader[0] = new ReportParameter("BeginDate", str, false);
        str = ", End Date: " + SearchEndDate.Text;
        repParamHeader[1] = new ReportParameter("EndDate", str, false);
        str = ", Merchant Status: " + lstStatus.SelectedItem.Text;
        repParamHeader[2] = new ReportParameter("MerchantStatus", str, false);
        str = ", Suspense Reason: " + lstReason.SelectedItem.Text;
        repParamHeader[3] = new ReportParameter("SuspenseReason", str, false);
        BatchSuspenseRate1.LocalReport.SetParameters(repParamHeader);
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
        BatchSuspenseRate1.Visible = false;
        SearchBeginDate.Value = DateTime.Today.ToShortDateString();
        SearchEndDate.Value = DateTime.Today.ToShortDateString();
    }

    private bool FormDelete()
    {
        return false;
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        DateTime minDate = new DateTime(1753, 1, 2);
        DateTime maxDate = new DateTime(9999, 12, 30);
        if (Convert.ToDateTime(SearchBeginDate.Value) >= minDate && Convert.ToDateTime(SearchBeginDate.Value) <= maxDate && Convert.ToDateTime(SearchEndDate.Value) >= minDate && Convert.ToDateTime(SearchEndDate.Value) <= maxDate)
        {
            if (Convert.ToDateTime(SearchBeginDate.Value) <= Convert.ToDateTime(SearchEndDate.Value))
            {
                this.SortOrder = string.Empty;
                //this.IsKeywordSearch = false;
                this.SearchParameters = null;
                this.Search(false);
            }
            else
            {
                lblError.Text = "Begin Date Should be less than End Date.";
            }
        }
        else
            lblError.Text = "Invalid Date.";
    }
}
