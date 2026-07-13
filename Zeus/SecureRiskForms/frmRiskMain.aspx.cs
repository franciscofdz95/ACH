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

public partial class frmRiskMain : frmBaseSearch
{
    decimal creditAmt = 0.0M, salesAmt = 0.0M;
    int creditCnt = 0, salesCnt = 0;

 
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkRiskHome")).CssClass = "active"; 

        if (!this.IsPostBack)
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = 1;

            LoadGrid();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grd.PageIndex + 1;
        }

        LoadGrid();
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        FormHandler.Export2Excel("Risk Batch Summary.xls", grd);

    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
      
        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grd.PageIndex + 1;
        }

        LoadGrid();
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        
        FormHandler.ExportToPDF(grd, true, "Risk Batch Summary.xls");
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadGrid();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadGrid();        
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        LoadGrid();
    }

    private void LoadGrid()
    {
        grd.DataBind();

        pnl.Visible = (grd.Rows.Count > 0);
        noRecords.Visible = !(grd.Rows.Count > 0);
        
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
    
    protected void odsRisk_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        string role = string.Empty;

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);

        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        if (this.SortOrder == string.Empty)
            prms["@SortOrder"] = "ZID";
        else
            prms["@SortOrder"] = this.SortOrder;

        prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetRiskBatchSummaryPagingRowCount(prms, 0, 0, this.grd.ID).ToString();


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

}
