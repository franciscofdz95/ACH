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

public partial class frmApplicationStatus : frmBaseSearch
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
            LookupTableHandler.MerchantAppStatusList(lstStatus, true, "Merchant Management");
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);
            ApplicationStatus1.Visible = false;

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

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        StringBuilder sb = new StringBuilder();
        foreach (ListItem item in lstStatus.Items)
        {
            if (item.Selected)
                sb.Append(item.Value + ",");
        }

        if (sb.ToString() != string.Empty)
            prms.Add("@StatusUIDList", sb.ToString().Substring(0, sb.Length - 1));

        if (wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
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

            ds = data.GetMerchantStatusReport(prms);
            DataView dv = ds.Tables[0].DefaultView;
            RecordCount = dv.Table.Rows.Count;
            dv.Sort = this.SortOrder;
            ApplicationStatus1.Visible = (dv.Table.Rows.Count > 0);
            this.ShowReport(ds.Tables[0]);
        }

        pnlRecords.Visible = !(ApplicationStatus1.Visible);
    }

    private void ShowReport(DataTable dt)
    {
        ApplicationStatus1.Width = new Unit("100%");
        ApplicationStatus1.Height = new Unit("1056px");
        string reportPath = "Reports/ApplicationStatus.rdlc";

        ApplicationStatus1.LocalReport.DataSources.Clear();
        ApplicationStatus1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatus", dt));
        DataTable dt1 = AchReport();
        ApplicationStatus1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantStatusAch", dt1));
        ApplicationStatus1.LocalReport.ReportPath = reportPath;
        ApplicationStatus1.LocalReport.Refresh();
    }

    private DataTable AchReport()
    {
        Hashtable prms = new Hashtable();
        StringBuilder sb = new StringBuilder();
        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);

        foreach (ListItem item in lstStatus.Items)
        {
            if (item.Selected)
                sb.Append(item.Value + ",");
        }

        if (sb.ToString() != string.Empty)
            prms.Add("@StatusUIDList", sb.ToString().Substring(0, sb.Length - 1));

        if (wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);//AgentUID.SelectedItem.Value);

        if (MerchantAppTypeUID.SelectedIndex > 0)
            prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

        cmd.CommandText = "sp_ReportMerchantStatusAch";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

        return ds.Tables[0];
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
        ApplicationStatus1.Visible = false;
        pnlRecords.Visible = true;
        wucAgentSelector.FormClear();
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
