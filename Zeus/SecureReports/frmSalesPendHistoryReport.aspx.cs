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

public partial class frmSalesPendHistoryReport : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            SearchBeginDate.Value = DateTime.Today;
            SearchEndDate.Value = DateTime.Today.AddDays(1);
            //LookupTableHandler.LoadAgentsNew(AgentUID, true);
            //LookupTableHandler.LoadInternalUsers(PrimaryContactUID, true);
            LookupTableHandler.LoadUsersByRole(PrimaryContactUID, true, Constants.ROLE_AGENT_RELATIONS);
            LookupTableHandler.GetAgentCategories(AgentCategoryUID);

            LoadHistoryPending();
        }
    }

    public void LoadHistoryPending()
    {
        DataSet ds = new DataSet();
        Hashtable prms = new Hashtable();

        if (wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
        {
            if (chkSubAgent.Checked)
                prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
            else
                prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
        }

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@StartDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndDate", SearchEndDate.Value);


        if (ddpPend.SelectedIndex > 0)
            prms.Add("@PendType", ddpPend.SelectedValue);

        if (PrimaryContactUID.SelectedIndex > 0)
            prms.Add("@PrimaryContactUID", PrimaryContactUID.SelectedValue);

        string AgentCatUID = string.Empty;

        for (int i = 0; i < AgentCategoryUID.Items.Count; i++)
        {
            if (AgentCategoryUID.Items[i].Selected)

                AgentCatUID += AgentCategoryUID.Items[i].Value + ",";
        }

        if (AgentCatUID != string.Empty)
            prms.Add("@AgentCategoryUID", AgentCatUID.TrimEnd(','));

        ds = DataAccess.DataMerchantAppDao.GetPendingHistory(prms);
        DataView dv = ds.Tables[0].DefaultView;

        grdHistoryPending.PageSize = this.PageSize;
        grdHistoryPending.PageIndex = this.CurrentPage - 1;

        dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);

        grdHistoryPending.DataSource = dv;
        grdHistoryPending.DataBind();

        lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
        lblData.Visible = (ds.Tables[0].Rows.Count == 0);
        pnl1.Visible = (ds.Tables[0].Rows.Count > 0);
    }

    protected void grdHistoryPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                string[] str = DataBinder.Eval(e.Row.DataItem, "Conditions").ToString().Split('~');
                StringBuilder strCon = new StringBuilder();
                //strCon.Append("<dl>");

                for (int i = 1; i < str.Length; i++)
                    strCon.Append("<dt>" + i.ToString() + ". " + str[i].Replace("~", "").Trim().Replace("^", "</dt><dd><li>") + "</li></dd>");

                // strCon.Append("</dl>");
                e.Row.Cells[10].Text = strCon.ToString();
                e.Row.Cells[10].Width = Unit.Pixel(250);
                break;
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadHistoryPending();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadHistoryPending();
        DataTable dataTable = ((DataView)grdHistoryPending.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grdHistoryPending.DataSource = dataView;
            grdHistoryPending.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        LoadHistoryPending();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        SearchBeginDate.Value = DateTime.Today;
        SearchEndDate.Value = DateTime.Today.AddDays(1);
        ListHandler.ListFindItem(PrimaryContactUID, "-1");
        AgentCategoryUID.SelectedIndex = -1;

        pnl1.Visible = false;
        lblData.Visible = true;
        LoadHistoryPending();
        wucAgentSelector.FormClear();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdHistoryPending.AllowPaging = false;
        FormHandler.Export2Excel("SalesPendingHistoryReport.xls", grdHistoryPending);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grdHistoryPending.PageSize = this.PageSize;
        LoadHistoryPending();
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
}
