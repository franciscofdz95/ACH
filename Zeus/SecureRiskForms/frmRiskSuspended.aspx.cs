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


public partial class frmRiskSuspended : frmBaseSearch
{
    decimal creditAmt = 0.0M, salesAmt = 0.0M;
    int creditCnt = 0, salesCnt = 0;

  
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkRiskSuspended")).CssClass = "active";

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadHeldBy(HeldBy, 1);
            LookupTableHandler.LoadReleasedBy(ReleasedBy, 1);

            lblError.Text = string.Empty;
            LoadGrid();                      
        }
    }

    private void LoadGrid()
    {
        Hashtable prms = new Hashtable();
        if (HeldBy.SelectedIndex != 0)
            prms.Add("@HeldBy", HeldBy.SelectedItem.Text);

        if (ReleasedBy.SelectedIndex != 0)
            prms.Add("@ReleasedBy", ReleasedBy.SelectedItem.Text);

        prms.Add("@StatusID", 1);

        DataSet ds = DataAccess.DataRiskDao.GetBatchHeader(prms);
        grdRisk.DataSource = ds;
        ds.Relations.Add("header", ds.Tables[0].Columns["BatchID"], ds.Tables[1].Columns["BatchID"]);
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

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        LoadGrid();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        ReleasedBy.SelectedIndex = 0;
        HeldBy.SelectedIndex = 0;
        LoadGrid();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Suspended Batch Report.xls", grdRisk);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grdRisk,true,"Suspended Batch Report.xls");
    }

    private void DoVoid(int MerchantID, int BatchID, string UserName)
    {
        try
        {
            bool perform = DataAccess.DataRiskDao.VoidBatch(MerchantID, BatchID, UserName, 1);
            if (!perform)
                lblError.Text = "Batch was not voided.  Batch may have been worked on by another user.";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
            throw ex;
        }
    }

    private void DoRelease(int MerchantID, int BatchID, string UserName)
    {
        try
        {
            bool perform = DataAccess.DataRiskDao.ReleaseBatch(MerchantID, BatchID, UserName, 1);

            if (!perform)
                lblError.Text = "Batch was not released.  Batch may have been worked on by another user.";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
            throw ex;
        }
    }

    private void DoHold(int MerchantID, int BatchID, string UserName)
    {
        try
        {
            bool perform = DataAccess.DataRiskDao.HoldBatch(MerchantID, BatchID, UserName, 1); ;

            if (!perform)
                lblError.Text = "Batch was not released.  Batch may have been worked on by another user.";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
            throw ex;
        }
    }

    protected void grdRisk_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        GridViewRow row = null;
        int MerchantID = 0, BatchID = 0;
        string merchantuid = string.Empty;

        if (e.CommandSource is LinkButton)
            row = ((GridViewRow)((LinkButton)(e.CommandSource)).NamingContainer);
        else if (e.CommandSource is Button)
            row = ((GridViewRow)((Button)(e.CommandSource)).NamingContainer);

        if (row != null)
        {
            MerchantID = Convert.ToInt32(grdRisk.Rows[row.RowIndex].Cells[9].Text);
            BatchID = Convert.ToInt32(grdRisk.DataKeys[row.RowIndex].Values["BatchID"].ToString());
            merchantuid = grdRisk.DataKeys[row.RowIndex].Values["MerchantAppUID"].ToString();
        }

        switch (e.CommandName)
        {
            //case "Merchant":
            //    Response.Redirect("frmRiskBatchDetails.aspx?MerchantAppUID=" + merchantuid + "&BatchID=" + BatchID.ToString());
            //    break;

            case "Edit":
                url = "frmRiskSuspendedDetails.aspx?";
                url += "MerchantAppUID=" + merchantuid;
                url += "&BatchID=" + BatchID.ToString();
                Response.Redirect(url);
                break;

            case "Release":
                this.DoRelease(MerchantID, BatchID, UserSessions.CurrentUser.UserName);
                LoadGrid();
                break;

            case "Hold":
                this.DoHold(MerchantID, BatchID, UserSessions.CurrentUser.UserName);
                LoadGrid();
                break;

            case "Void":
                this.DoVoid(MerchantID, BatchID, UserSessions.CurrentUser.UserName);
                LoadGrid();
                break;
        }
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
                ((HtmlImage)e.Row.FindControl("img1")).Attributes.Add("onclick", "CollapseExpand('" + batchid + "',null,'" + ((HtmlImage)e.Row.FindControl("img1")).ClientID+ "')");
                break;
            case DataControlRowType.Footer:
                e.Row.Cells[16].Text = salesCnt.ToString();
                e.Row.Cells[17].Text = salesAmt.ToString("0.00");//"c");
                e.Row.Cells[18].Text = creditCnt.ToString();
                e.Row.Cells[19].Text = creditAmt.ToString("0.00");//"c");
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
}
