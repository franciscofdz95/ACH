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

public partial class frmDealCountSummaryReport : frmBaseSearch
{
    public string grid
    {
        get
        {
            if (ViewState["grid"] == null)
                return string.Empty;
            else
                return ViewState["grid"].ToString();
        }
        set { ViewState["grid"] = value; }
    }

    public string grdSortOrder
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GSortOrder"] == null)
                return "AgentFullName";
            else
                return ViewState[this.Page.ToString() + "_GSortOrder"].ToString();
        }
        set { ViewState[this.Page.ToString() + "_GSortOrder"] = value; }
    }

    public SortDirection grdSortDirectionSearch
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GSortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)ViewState[this.Page.ToString() + "_GSortDirectionSearch"];
        }
        set { ViewState[this.Page.ToString() + "_GSortDirectionSearch"] = value; }
    }

    public int grdCurrentPage
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GCurrentPage"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_GCurrentPage"]);
        }
        set { ViewState[this.Page.ToString() + "_GCurrentPage"] = value; }
    }

    public int grdPageSize
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GPageSize"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_GPageSize"]);
        }
        set { ViewState[this.Page.ToString() + "_GPageSize"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            //Set the current page
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Deal Count Summary Report";

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            this.grid = string.Empty;
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
        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            DataSet ds = null;
            DataView dv = null;
            string SortDir = "DESC";

            if (grid == string.Empty || grid.ToUpper() == "GRDCREDIT")
            {
                //Credit card Count
                ds = data.GetDealCountSummaryReport(prms);
                dv = ds.Tables[0].DefaultView;
                RecordCount = dv.Table.Rows.Count;

                if (SortDirectionSearch == SortDirection.Ascending)
                    SortDir = "ASC";
                if (dv.Table.Rows.Count > 0)
                {
                    PnlCreditCard.Visible = true;
                    dv.Sort = this.SortOrder + " " + SortDir;

                    grdCredit.PageSize = this.PageSize;
                    grdCredit.PageIndex = this.CurrentPage - 1;
                    grdCredit.DataSource = dv;
                    grdCredit.DataBind();

                    NoData.Visible = false;
                    lblRecordCount.Text = "Total Records Found: " + RecordCount.ToString();
                }
                else
                {
                    PnlCreditCard.Visible = false;
                    NoData.Visible = true;
                    lblRecordCount.Text = "";
                }
            }

            //ACH Count
            if (grid == string.Empty || grid.ToUpper() == "GRDACH")
            {
                ds = GetACHDealCountSummaryReport(prms);
                dv = ds.Tables[0].DefaultView;
                SortDir = "DESC";
                RecordCount = dv.Table.Rows.Count;
                if (dv.Table.Rows.Count > 0)
                {
                    PnlACH.Visible = true;

                    if (grdSortDirectionSearch == SortDirection.Ascending)
                        SortDir = "ASC";
                    dv.Sort = this.grdSortOrder + " " + SortDir;

                    grdACH.PageSize = this.grdPageSize;
                    grdACH.PageIndex = this.grdCurrentPage - 1;
                    grdACH.DataSource = dv;
                    grdACH.DataBind();

                    lblNoData.Visible = false;
                    lblCount.Text = "Total Records Found: " + RecordCount.ToString();
                }
                else
                {
                    PnlACH.Visible = false;
                    lblNoData.Visible = true;
                    lblCount.Text = "";
                }
            }
        }
    }

    public DataSet GetACHDealCountSummaryReport(Hashtable prms)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "sp_ReportDealCountSummaryACH";
        cmd.CommandType = CommandType.StoredProcedure;
        DataLayer.AppendParamters(cmd, prms);

        return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        //UserSessions.SearchResultsDataView = null;
        //UserSessions.SearchResultsDataView2 = null;
        FormHandler.ClearAllControls(this);
    }

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
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
        this.SortOrder = this.grdSortOrder = "AgentFullName";
        this.SearchParameters = null;
        this.grid = string.Empty;
        this.Search(false);
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=true";
        url += "&PostBackURL=~/SecureMerchantManagementForms/frmMerchantSearch.aspx";
        Response.Redirect(url);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        grid = "GRDCREDIT";
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    protected void cboPageSize1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.grdCurrentPage = 1;
        grid = "GRDACH";
        this.PageSize = Convert.ToInt32(DropDownList1.SelectedItem.Value);
        this.Search(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        grid = ((GridView)sender).ID.ToUpper();

        if (grid == "GRDCREDIT")
        {
            this.CurrentPage = 1;
            if (this.SortOrder == e.SortExpression)
                this.SortDirectionSearch = ConvertSortDirectionToSql(this.SortDirectionSearch);
            else
                this.SortDirectionSearch = SortDirection.Descending;
            this.SortOrder = e.SortExpression;
        }
        else
        {
            this.grdCurrentPage = 1;

            if (this.grdSortOrder == e.SortExpression)
                this.grdSortDirectionSearch = ConvertSortDirectionToSql(this.grdSortDirectionSearch);
            else
                this.grdSortDirectionSearch = SortDirection.Descending;
            this.grdSortOrder = e.SortExpression;

        }
        this.Search(false);
    }

    private SortDirection ConvertSortDirectionToSql(SortDirection direction)
    {
        switch (direction)
        {
            case SortDirection.Descending:
                return SortDirection.Ascending;

            default:
                return SortDirection.Descending;
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid = ((GridView)sender).ID.ToUpper();

        if (grid == "GRDCREDIT")
        {
            this.CurrentPage = e.NewPageIndex + 1;
        }
        else
            this.grdCurrentPage = e.NewPageIndex + 1;

        this.Search(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        grid = "GRDCREDIT";
        grdCredit.AllowPaging = false;
        Search(false);
        grdCredit.AllowPaging = true;
        FormHandler.Export2Excel("Creditcard_DealCountSummary.xls", grdCredit);
    }

    protected void btnExport1_Click(object sender, EventArgs e)
    {
        grid = "GRDACH";
        grdACH.AllowPaging = false;
        Search(false);
        grdACH.AllowPaging = true;
        FormHandler.Export2Excel("ACH_DealCountSummary.xls", grdACH);
    }
}
