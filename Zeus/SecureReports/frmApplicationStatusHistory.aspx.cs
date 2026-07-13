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

public partial class frmApplicationStatusHistory : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            //Set the current page
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Merchant Search";

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists
            //LookupTableHandler.LoadAgentsNew(AgentUID, true);
            //Loadtootltip();
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management");
            pnlRecords.Visible = false;
            pnlNoRecords.Visible = true;
            this.Search(true);
        }
    }

    //private void Loadtootltip()
    //{
    //    foreach (ListItem lstitem in AgentUID.Items)
    //    {
    //        lstitem.Attributes.Add("title",lstitem.Text);
    //    }
    //}

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        pnlRecords.Visible = false;

        if (StatusUID.SelectedIndex > 0)
            prms.Add("@SearchByStatusUID", StatusUID.SelectedItem.Value);

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (wucAgentSelector.m_AgentUID != string.Empty)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

        if (MerchantAppTypeUID.SelectedIndex > 0)
            prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            DataSet ds = null;

            ds = data.GetMerchantStatusReportHistory(prms);

            pnlRecords.Visible = (ds.Tables[0].Rows.Count > 0);

            DataSet ds2 = data.GetMerchantStatusReportAchHistory(prms);
            ApplicationStatus2.Width = new Unit("100%");
            ApplicationStatus2.Height = new Unit("1056px");
            string reportPath = "Reports/ApplicationStatusHistory.rdlc";

            ApplicationStatus2.LocalReport.DataSources.Clear();
            ApplicationStatus2.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatusHistory", ds.Tables[0]));
            ApplicationStatus2.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatusAchHistory", ds2.Tables[0]));
            ApplicationStatus2.LocalReport.ReportPath = reportPath;
            ApplicationStatus2.LocalReport.Refresh();

            lblRecordCount.Text = "Total Records Found: " + Convert.ToString(ds.Tables[0].Rows.Count + ds2.Tables[0].Rows.Count);
        }

        pnlNoRecords.Visible = !(pnlRecords.Visible);
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
                if (StatusUID.SelectedIndex > 0)
                    prms.Add("@SearchByStatusUID", StatusUID.SelectedItem.Value);

                if (!string.IsNullOrEmpty(SearchBeginDate.Text))
                    prms.Add("@BeginPostedDate", SearchBeginDate.Value);

                if (!string.IsNullOrEmpty(SearchEndDate.Text))
                    prms.Add("@EndPostedDate", SearchEndDate.Value);

                if (wucAgentSelector.m_AgentUID != string.Empty)
                    prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

                if (MerchantAppTypeUID.SelectedIndex > 0)
                    prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

                cmd.CommandText = "sp_ReportMerchantStatusAchHistory";
                cmd.CommandType = CommandType.StoredProcedure;
                DataLayer.AppendParamters(cmd, prms);

                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatusAchHistory", ds.Tables[0]));
                break;
        }
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        //UserSessions.SearchResultsDataView = null;
        //UserSessions.SearchResultsDataView2 = null;
        FormHandler.ClearAllControls(this);
        wucAgentSelector.FormClear();
        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
    }

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
    }

    protected void btnKeywordSearch_Click(object sender, ImageClickEventArgs e)
    {
        this.SortOrder = string.Empty;
        //this.IsKeywordSearch = true;
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnAddMerchant_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "frmLeadsDetail.aspx?Adding=true";
        Response.Redirect(url);
    }

    public void ToggleButtons()
    {
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

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=true";
        url += "&PostBackURL=~/SecureMerchantManagementForms/frmMerchantSearch.aspx";
        Response.Redirect(url);
    }
}
