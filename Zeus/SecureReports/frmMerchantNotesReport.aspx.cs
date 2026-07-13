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

using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using Microsoft.Reporting.WebForms;

public partial class frmMerchantNotesReport : frmBaseSearch
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
            MerchantNotes1.Visible = false;
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;
        if (IsOnLoad == true && this.SearchParameters == null)
            return;
        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }
        grd.DataBind();
        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;
    }

    protected void odsMerchants_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app = null;
        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);

        StringBuilder sb = new StringBuilder();
        foreach (ListItem item in lstStatus.Items)
        {
            if (item.Selected)
                sb.Append(item.Value + ",");
        }
        if (sb.ToString() != string.Empty)
            prms.Add("@StatusUIDList", sb.ToString().Substring(0, sb.Length - 1));

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@BusinessDBAName", BusinessDBAName.Text);

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

            //user is passed as a parameter to determine whether the user is an agent or manager
            User user = UserSessions.CurrentUser;
            prms.Add("@UserName", user.UserName);

            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);
        }
        else
        {
            prms.Add("@ID", -1);
        }

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    private int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = 0;
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }

        return newSortDirection;
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    private void FormClear()
    {
        lstSelected.Items.Clear();
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
        MerchantNotes1.Visible = false;
        this.Search(false);
        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
        wucAgentSelector.FormClear();
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        ListItem item = null;

        if (e.CommandSource is LinkButton)
        {
            LinkButton lnk = (LinkButton)e.CommandSource;
            GridViewRow grdRow = (GridViewRow)lnk.NamingContainer;

            switch (lnk.CommandName)
            {
                case "Select":
                    item = new ListItem();
                    item.Text = grdRow.Cells[3].Text;
                    item.Value = lnk.CommandArgument;
                    lstSelected.Items.Add(item);
                    break;
            }
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                LinkButton lnk = (LinkButton)e.Row.Cells[2].FindControl("lbtnSelect");
                lnk.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                break;
        }
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SortOrder = string.Empty;
        this.CurrentPage = 1;
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        grd.PageIndex = 0;
        this.CurrentPage = 1;
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("MerchantList.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grd, true, "Merchant Search Results");
    }

    protected void btnViewReport_Click(object sender, EventArgs e)
    {
        DataMerchantApp app = DataAccess.DataMerchantAppDao;
        Hashtable prms = new Hashtable();

        StringBuilder sb = new StringBuilder();
        foreach (ListItem item in lstSelected.Items)
        {
            sb.Append(item.Value + ",");
        }

        if (sb.ToString() != string.Empty)
            prms.Add("@MerchantList", sb.ToString().Substring(0, sb.Length - 1));

        if (prms.Count > 0)
        {
            DataSet ds = app.GetMerchantNotesReport(prms);

            if (ds.Tables[0].Rows.Count > 0)
                MerchantNotes1.Visible = true;
            else
                MerchantNotes1.Visible = false;
            this.ShowReport(ds.Tables[0]);
        }
    }

    private void ShowReport(DataTable dt)
    {
        MerchantNotes1.Width = new Unit("100%");

        MerchantNotes1.Height = new Unit("1056px");

        string reportPath = "Reports/MerchantNotes.rdlc";

        MerchantNotes1.LocalReport.DataSources.Clear();
        MerchantNotes1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_ReportMerchantNotes", dt));
        MerchantNotes1.LocalReport.ReportPath = reportPath;
        MerchantNotes1.LocalReport.Refresh();
    }

    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grd.Rows)
        {
            ListItem item = new ListItem();
            item.Text = row.Cells[3].Text;
            item.Value = ((LinkButton)row.Cells[2].FindControl("lbtnSelect")).CommandArgument;
            lstSelected.Items.Add(item);
        }
    }

    protected void btnClearList_Click(object sender, EventArgs e)
    {
        lstSelected.Items.Clear();
        MerchantNotes1.Visible = false;
    }
}
