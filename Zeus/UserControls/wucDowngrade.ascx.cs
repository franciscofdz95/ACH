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

public partial class wucDowngrade : wucBaseSearch
{
    
    public bool isSales
    {
        get { if (ViewState["isSales"] == null) return false; else return Convert.ToBoolean(ViewState["isSales"]); }
        set { ViewState["isSales"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DateTime dt = Convert.ToDateTime(DateTime.Today.ToString("MM/01/yyyy"));
            SearchBeginDate.Value = dt;
            SearchEndDate.Value = DateTime.Today;

            // LookupTableHandler.LoadAgentsNew(AgentUID, !isSales);

            if (!isSales)
                Search(true);
        }
    }

    public void Search(bool IsOnLoad)
    {
        DataBankFiles data = DataAccess.DataBankFilesDao;
        DataSet ds = null;

        Hashtable prms = new Hashtable();
        string message = FormDataCheck();

        if (message == string.Empty)
        {
            
            if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            {
                prms.Add("@BeginPostedDate", SearchBeginDate.Value);
                FromDate = SearchBeginDate.Text;
            }

            if (!string.IsNullOrEmpty(SearchEndDate.Text))
            {
                ToDate = SearchEndDate.Text;
                prms.Add("@EndPostedDate", SearchEndDate.Value);
            }

            if (wucAgentSelector.m_AgentUID != string.Empty)
            {
                if (chkSubAgent.Checked)
                    prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
                else
                    prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
            }

            if (MID.Text != string.Empty)
                prms.Add("@MID", MID.Text);

            if (ZID.Text != string.Empty)
                prms.Add("@ZID", ZID.Text);

            ds = data.GetDownGradeSummaryReport(prms);

            if (ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;

                grdDownGrade.PageSize = this.PageSize;
                grdDownGrade.PageIndex = this.CurrentPage - 1;

                dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);
                grdDownGrade.DataSource = dv;
                grdDownGrade.DataBind();

                lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
                lblData.Visible = (ds.Tables[0].Rows.Count == 0);
                pnl1.Visible = (ds.Tables[0].Rows.Count > 0);
            }
        }
        else
        {
            FormHandler.DisplayMessage(Page.ClientScript, message);
        }
    }

    public string FormDataCheck()
    {
        string message = string.Empty;

        if (SearchBeginDate.Value == null)
            message = "Please enter the begin date.";

        if (SearchEndDate.Value == null)
            message += "Please enter the end date.";

        if (wucAgentSelector.m_AgentUID.Trim() == string.Empty && isSales)
            message += "Please select an agent.";

        if (message == string.Empty)
            return string.Empty;
        else
        {
            return message;
            // return false;
        }
    }

    private void FormClear()
    {
        FormHandler.ClearAllControls(this);
        DateTime dt = Convert.ToDateTime(DateTime.Today.ToString("MM/01/yyyy"));
        SearchBeginDate.Value = dt;
        SearchEndDate.Value = DateTime.Today;
        grdDownGrade.DataSource = null;
        grdDownGrade.DataBind();
        pnl1.Visible = false;
        lblData.Visible = true;
    }

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
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
        grdDownGrade.PageIndex = 0;
        this.Search(false);
    }

    protected void grdDownGrade_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                break;

            case DataControlRowType.DataRow:

                if (e.Row.Cells[4].Text != string.Empty && e.Row.Cells[4].Text != "&nbsp;")
                    e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text).ToString("0.00");//"c");

                if (e.Row.Cells[6].Text != string.Empty && e.Row.Cells[6].Text != "&nbsp;")
                    e.Row.Cells[6].Text = Convert.ToDecimal(e.Row.Cells[6].Text).ToString("0.00");//"c");

                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Downgrade Cnt")) == 0)
                {
                    ((Label)e.Row.Cells[5].FindControl("lblCnt")).Attributes.Add("style", "display:inline;");
                    ((LinkButton)e.Row.Cells[5].FindControl("lnkCnt")).Attributes.Add("style", "display:none;");
                }
                else
                {
                    ((Label)e.Row.Cells[5].FindControl("lblCnt")).Attributes.Add("style", "display:none;");
                    ((LinkButton)e.Row.Cells[5].FindControl("lnkCnt")).Attributes.Add("style", "display:inline;");
                }
                break;
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        Search(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        Search(false);
        DataTable dataTable = ((DataView)grdDownGrade.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grdDownGrade.DataSource = dataView;
            grdDownGrade.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grdDownGrade.PageSize = this.PageSize;
        Search(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        Search(false);
        grdDownGrade.AllowPaging = false;

        FormHandler.Export2Excel("DownGradeReport.xls", grdDownGrade);
    }

    protected void btnCnt_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        Hashtable prms = new Hashtable();
        GridView grd = new GridView();
        if (btn.Text != "0")
        {
            if (FromDate != "")
                prms.Add("@BeginPostedDate", FromDate);

            if (ToDate != "")
                prms.Add("@EndPostedDate", ToDate);

            prms.Add("@MID", btn.CommandArgument);

            grd.DataSource = DataAccess.DataBankFilesDao.GetDownGradeDetailsReport(prms);
            grd.DataBind();
            FormHandler.Export2Excel("DownGradeDetailReport.xls", grd);
        }
    }

    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "DESC";
                CurrentSort = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                newSortDirection = "ASC";
                CurrentSort = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    private string GetSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                // CurrentSort = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                newSortDirection = "DESC";
                // CurrentSort = SortDirection.Ascending;
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
                return "AgentID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    public string FromDate
    {
        get
        {
            if (ViewState["FromDate"] == null)
            {
                return "";
            }
            return ViewState["FromDate"].ToString();
        }
        set { ViewState["FromDate"] = value; }
    }

    public string ToDate
    {
        get
        {
            if (ViewState["ToDate"] == null)
            {
                return "";
            }
            return ViewState["ToDate"].ToString();
        }
        set { ViewState["ToDate"] = value; }
    }
}
