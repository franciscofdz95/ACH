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

public partial class frmDeclineRiskRatios : frmBaseSearch
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
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
            //Load Risk Metric dropdown
            rptViewer.Visible = false;

            this.Search(true);
        }
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

        if (lstRiskMetric.SelectedIndex > 0)
            prms.Add("@RiskMetric", lstRiskMetric.SelectedItem.Value);


        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            DataRisk data = DataAccess.DataRiskDao;
            DataSet ds = null;

           // ds = data.GetMerchantStatusReport(prms);
            DataView dv = ds.Tables[0].DefaultView;
            RecordCount = dv.Table.Rows.Count;
            dv.Sort = this.SortOrder;
            rptViewer.Visible = (dv.Table.Rows.Count > 0);
            this.ShowReport(ds.Tables[0]);
        }
        lbl.Visible = !(rptViewer.Visible);
    }

    private void ShowReport(DataTable dt)
    {
        rptViewer.Width = new Unit("100%");
        rptViewer.Height = new Unit("1056px");
        string reportPath = "Reports/DeclineRiskRatios.rdlc";

        rptViewer.LocalReport.DataSources.Clear();
        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatus", dt));
        rptViewer.LocalReport.ReportPath = reportPath;
        rptViewer.LocalReport.Refresh();
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
        SearchBeginDate.Value = DateTime.Today.ToShortDateString();
        SearchEndDate.Value = DateTime.Today.ToShortDateString();
        rptViewer.Visible = false;
        lbl.Visible = !(rptViewer.Visible);
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
        this.SortOrder = string.Empty;
        //this.IsKeywordSearch = false;
        this.SearchParameters = null;
        this.Search(false);
    }
}
