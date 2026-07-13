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

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;


public partial class frmRiskBatchPreview : frmBaseSearch
{
    decimal creditAmt = 0.0M, salesAmt = 0.0M;
    int creditCnt = 0, salesCnt = 0;

   

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkRiskBatchPreview")).CssClass = "active";

        if (!this.IsPostBack)
        {
            LoadGrid();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
            grd.PageSize = 5000;
        else
            grd.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        LoadGrid();
        FormHandler.Export2Excel("Risk Batch Summary.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
            grd.PageSize = 5000;
        else
            grd.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        LoadGrid();
        FormHandler.ExportToPDF(grd, true, "Risk Batch Summary.xls");
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd.PageIndex = e.NewPageIndex;
        LoadGrid();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadGrid();
        DataTable dataTable = ((DataView)grd.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grd.DataSource = dataView;
            grd.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grd.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        LoadGrid();
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
                return "MID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    private void LoadGrid()
    {
        DataSet ds = DataAccess.DataRiskDao.GetBatchPreview(new Hashtable());
        DataView dv = ds.Tables[0].DefaultView;
        grd.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Text);
        lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();

        //Bind grid
        grd.DataSource = dv;
        grd.DataBind();
        pnl.Visible = (grd.Rows.Count > 0);
        noRecords.Visible = !(grd.Rows.Count > 0);
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //switch (e.CommandName)
        //{
        //    case "Merchant":
        //        GridViewRow row = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer);
        //        Response.Redirect("frmRiskBatchPreviewDetail.aspx?MerchantAppUID=" + grd.DataKeys[row.RowIndex].Values["MerchantAppUID"].ToString());
        //        break;
        //    default:
        //        break;
        //}
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                creditAmt = 0.0M; salesAmt = 0.0M;
                creditCnt = 0; salesCnt = 0;
                break;
            case DataControlRowType.DataRow:
                creditAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "CreditAmount").ToString());
                creditCnt += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "CreditCount"));
                salesAmt += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "SalesAmount").ToString());
                salesCnt += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SalesCount"));
                e.Row.Cells[0].Attributes.Add("class", "text");
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[4].Text = salesCnt.ToString();
                e.Row.Cells[5].Text = salesAmt.ToString("0.00");//"c");
                e.Row.Cells[6].Text = creditCnt.ToString();
                e.Row.Cells[7].Text = creditAmt.ToString("0.00");//"c");
                break;
        }
    }
}
