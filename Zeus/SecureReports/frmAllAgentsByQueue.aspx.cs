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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class frmAllAgentsByQueue : frmBaseSearch
{
    int a1, a5, a6, a7, a8, a9;
    decimal a2, a3, a4;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            LookupTableHandler.LoadAgentQueues(Queues, true);
            StartDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
            EndDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
            LookupTableHandler.LoadHour(StartTime);
            LookupTableHandler.LoadHour(EndTime);
            EndTime.SelectedIndex = EndTime.Items.Count - 1;
            this.Search();
        }
    }

    public void Search()
    {
        DataAgent data = DataAccess.DataAgentDao;
        DataSet ds = null;
        Hashtable prms = new Hashtable();

        prms.Add("@StartDate", Convert.ToDateTime(StartDateTime.Text.ToString() + ' ' + StartTime.SelectedItem.Text));
        prms.Add("@EndDate", Convert.ToDateTime(EndDateTime.Text.ToString() + ' ' + EndTime.SelectedItem.Text));

        if (Queues.SelectedIndex > 0)
            prms.Add("@QueueName", Queues.SelectedItem.Value);

        ds = data.GetSummaryByQueues(prms);

        grd.DataSource = ds;
        ds.Relations.Add("header", ds.Tables[0].Columns["Ext"], ds.Tables[1].Columns["Ext"]);
        grd.DataBind();

        lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();
        noData.Visible = ds.Tables[0].Rows.Count == 0;
        pnl1.Visible = !(noData.Visible);
    }

    protected void grdQueue_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                a1 = 0; a5 = 0; a6 = 0; a7 = 0; a8 = 0; a9 = 0;
                a2 = 0.0M; a3 = 0.0M; a4 = 0.0M;
                break;

            case DataControlRowType.DataRow:
                a1 += int.Parse(e.Row.Cells[1].Text);
                a2 += decimal.Parse(e.Row.Cells[2].Text);
                a3 += decimal.Parse(e.Row.Cells[3].Text);
                a4 += decimal.Parse(e.Row.Cells[4].Text);
                a5 += int.Parse(e.Row.Cells[5].Text);
                a6 += int.Parse(e.Row.Cells[6].Text);
                a7 += int.Parse(e.Row.Cells[7].Text);
                a8 += int.Parse(e.Row.Cells[8].Text);
                a9 += int.Parse(e.Row.Cells[9].Text);
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "Totals";
                e.Row.Cells[1].Text = a1.ToString();
                e.Row.Cells[2].Text = a2.ToString();
                e.Row.Cells[3].Text = a3.ToString();
                e.Row.Cells[4].Text = a4.ToString();
                e.Row.Cells[5].Text = a5.ToString();
                e.Row.Cells[6].Text = a6.ToString();
                e.Row.Cells[7].Text = a7.ToString();
                e.Row.Cells[8].Text = a8.ToString();
                e.Row.Cells[9].Text = a9.ToString();
                break;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Queues.SelectedIndex = 0;
        StartDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
        EndDateTime.Value = DateTime.Now.ToString("MM/dd/yyyy");
        LookupTableHandler.LoadHour(StartTime);
        LookupTableHandler.LoadHour(EndTime);
        EndTime.SelectedIndex = EndTime.Items.Count - 1;
        Search();
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                break;
            case DataControlRowType.DataRow:

                DataSet ds1 = (DataSet)grd.DataSource;
                DataRowView dr = (DataRowView)e.Row.DataItem;
                DataView dv = dr.CreateChildView(ds1.Tables[1].ParentRelations["header"]);
                dv.RowFilter = "Ext <> ' '";
                ((GridView)e.Row.FindControl("grdQueue")).DataSource = dv;
                ((GridView)e.Row.FindControl("grdQueue")).DataBind();

                ((HtmlImage)e.Row.FindControl("img1")).Attributes.Add("onclick", "CollapseExpand('" + ((HtmlGenericControl)e.Row.FindControl("div1")).ClientID + "',null,'" + ((HtmlImage)e.Row.FindControl("img1")).ClientID + "')");
                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        Search();
        DataTable dataTable = ((DataSet)grd.DataSource).Tables[0];
        DataTable dt2 = ((DataSet)grd.DataSource).Tables[1];
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
            ds.Relations.Add("header", ds.Tables[0].Columns["Ext"], ds.Tables[1].Columns["Ext"]);
            grd.DataSource = ds;
            grd.DataBind();
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Search();
        FormHandler.Export2Excel("SummaryByQueues.xls", grd);
    }
}
