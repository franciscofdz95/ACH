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
using System.Text;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Globalization;

public partial class frmRiskSearch : frmBaseSearch
{
    decimal creditAmt = 0.0M, salesAmt = 0.0M;
    int creditCnt = 0, salesCnt = 0;

   

    private void LoadGrid()
    {
        Hashtable prms = new Hashtable();
        DateTime createdBeginDate;
        DateTime createdEndDate;

        if (HeldBy.SelectedIndex != 0)
            prms.Add("@HeldBy", HeldBy.SelectedItem.Text);

        if (ReleasedBy.SelectedIndex != 0)
            prms.Add("@ReleasedBy", ReleasedBy.SelectedItem.Text);

        if (BatchStatus.SelectedIndex != 0)
            prms.Add("@StatusID", BatchStatus.SelectedItem.Value);

        if (!string.IsNullOrEmpty(SearchCreatedBeginDate.Text))
        {
            DateTime.TryParse(SearchCreatedBeginDate.Text, out createdBeginDate);
            prms.Add("@BeginPostedDate", createdBeginDate);
        }

        if (!string.IsNullOrEmpty(SearchCreatedEndDate.Text))
        {
            DateTime.TryParse(SearchCreatedEndDate.Text, out createdEndDate);
            prms.Add("@EndPostedDate", createdEndDate);
        }

        if (ZID.Text != string.Empty)
            prms.Add("@MerchantID", ZID.Text);

        if (DBA.Text != string.Empty)
            prms.Add("@DBA", DBA.Text);

        prms.Add("@DisplayBatchStatus", 1);

        DataSet ds = DataAccess.DataRiskDao.GetBatchHeader(prms);
        ds.Relations.Add("header", ds.Tables[0].Columns["BatchID"], ds.Tables[1].Columns["BatchID"]);

        grdRisk.DataSource = ds;
        grdRisk.DataBind();
        lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();

        if (grdRisk.Rows.Count == 0)
        {
            pnl1.Visible = false;
            noData.Visible = true;
            pnl.Visible = false;
        }
        else
        {
            noData.Visible = false;
            pnl.Visible = true;
            pnl1.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkRiskSearch")).CssClass = "active";

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadHeldBy(HeldBy, -1);
            LookupTableHandler.LoadReleasedBy(ReleasedBy, -1);
            LookupTableHandler.LoadBatchStatus(BatchStatus, true);

            ReleasedBy.SelectedIndex = 0;
            SearchCreatedBeginDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            SearchCreatedEndDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            pnl1.Visible = false;
            noData.Visible = true;
            pnl.Visible = false;

        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string valError = ValidateSearchParams();
        if (!string.IsNullOrWhiteSpace(valError))
        {
            FormHandler.DisplayMessage(Page.ClientScript, valError);
            return;
        }
        LoadGrid();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        ReleasedBy.SelectedIndex = 0;
        HeldBy.SelectedIndex = 0;
        FormHandler.ClearAllControls(this);
        ResetGrid();
    }
    private void ResetGrid()
    {
        grdRisk.DataSource = null;
        grdRisk.DataBind();
        lblRecordCount.Text = string.Empty;

        pnl1.Visible = false;
        noData.Visible = true;
        pnl.Visible = false;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Batch Search Report.xls", grdRisk);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grdRisk, true, "Batch Search Report.xls");
    }

    protected void grdRisk_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        GridViewRow row = null;
        int BatchID = 0;
        string merchantuid = string.Empty;

        if (e.CommandSource is LinkButton)
            row = ((GridViewRow)((LinkButton)(e.CommandSource)).NamingContainer);

        if (row != null)
        {
            BatchID = Convert.ToInt32(grdRisk.DataKeys[row.RowIndex].Values["BatchID"].ToString());
            merchantuid = grdRisk.DataKeys[row.RowIndex].Values["MerchantAppUID"].ToString();
        }

        //switch (e.CommandName)
        //{
        //    case "Merchant":
        //        Response.Redirect("frmRiskBatchDetails.aspx?MerchantAppUID=" + merchantuid + "&BatchID=" + BatchID.ToString());
        //        break;
        //}
    }

    protected void grdRisk_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                creditAmt = 0.0M; salesAmt = 0.0M;
                creditCnt = 0; salesCnt = 0;
                break;
            case DataControlRowType.DataRow:
                DataSet ds1 = (DataSet)grdRisk.DataSource;
                DataRowView dr = (DataRowView)e.Row.DataItem;
                DataView dv = dr.CreateChildView(ds1.Tables[1].ParentRelations["header"]);
                dv.RowFilter = "RiskId <> '-1'";
                ((GridView)e.Row.FindControl("gvBatch")).DataSource = dv;
                ((GridView)e.Row.FindControl("gvBatch")).DataBind();

                creditAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditAmount").ToString());
                creditCnt += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CreditCount"));
                salesAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SalesAmount").ToString());
                salesCnt += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SalesCount"));
                string batchid = DataBinder.Eval(e.Row.DataItem, "BatchID").ToString();
                ((HtmlImage)e.Row.FindControl("img1")).Attributes.Add("onclick", "CollapseExpand('" + batchid + "',null,'" + ((HtmlImage)e.Row.FindControl("img1")).ClientID + "')");
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[13].Text = salesCnt.ToString();
                e.Row.Cells[14].Text = salesAmt.ToString("0.00");//"c");
                e.Row.Cells[15].Text = creditCnt.ToString();
                e.Row.Cells[16].Text = creditAmt.ToString("0.00");//"c");
                break;
            default:
                break;
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadGrid();
        DataTable dataTable = ((DataSet)grdRisk.DataSource).Tables[0];
        DataTable dt2 = ((DataSet)grdRisk.DataSource).Tables[1];
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            DataView dv2 = new DataView(dt2);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            DataSet ds = new DataSet();
            ds.Tables.Add(dataView.ToTable());
            ds.Tables.Add(dv2.ToTable());
            ds.Relations.Add("header", ds.Tables[0].Columns["BatchID"], ds.Tables[1].Columns["BatchID"]);
            grdRisk.DataSource = ds;
            grdRisk.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                CurrentSort = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                newSortDirection = "DESC";
                CurrentSort = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    public SortDirection CurrentSort
    {
        get
        {
            if (ViewState["sortDir"] == null)
            {
                return SortDirection.Ascending;
            }
            return (SortDirection)ViewState["sortDir"];
        }
        set { ViewState["sortDir"] = value; }
    }

    public string CurrentExp
    {
        get
        {
            if (ViewState["sortExp"] == null)
            {
                return "ZID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }
    private string ValidateSearchParams()
    {
        StringBuilder sb = new StringBuilder();
        DateTime date;

        if (!string.IsNullOrEmpty(this.SearchCreatedBeginDate.Text)
            && !DateTime.TryParseExact(this.SearchCreatedBeginDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            this.SearchCreatedBeginDate.Text = string.Empty;
            sb.Append("Please enter a valid Begin Date.\\n");
        }

        if (!string.IsNullOrEmpty(this.SearchCreatedEndDate.Text)
            && !DateTime.TryParseExact(this.SearchCreatedEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            this.SearchCreatedEndDate.Text = string.Empty;
            sb.Append("Please enter a valid End Date.\\n");
        }

        return sb.ToString();
    }
}
