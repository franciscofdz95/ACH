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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Globalization;

public partial class frmLeadActivityReport : frmBaseSearch
{
    public decimal[] a;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkLeadReports")).CssClass = "active";

        if (!IsPostBack)
        {
            //LookupTableHandler.LoadAgentsNew(AgentUID, true);
            // LoadHistoryPending();
        }
        //AgentUID.Attributes.Add("onchange", "ChangeCheckBox('" + AgentUID.ClientID + "');");//,'" + chkSubAgent.ClientID + "'
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

        //Active Agents
        if (chkActive.Checked)
            prms.Add("@IsActive", chkActive.Checked);

        ds = DataAccess.DataMerchantAppDao.GetLeadActivity(prms);
        pnl1.Visible = false;
        lblData.Visible = true;

        if (ds.Tables.Count > 0)
        {
            DataView dv = ds.Tables[0].DefaultView;

            grdLeadsActivity.PageSize = this.PageSize;
            grdLeadsActivity.PageIndex = this.CurrentPage - 1;
            grdLeadsActivity.ShowFooter = (chkSubAgent.Checked);

            dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);

            grdLeadsActivity.DataSource = dv;
            grdLeadsActivity.DataBind();

            lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
            lblData.Visible = (ds.Tables[0].Rows.Count == 0);
            pnl1.Visible = (ds.Tables[0].Rows.Count > 0);
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
        DataTable dataTable = ((DataView)grdLeadsActivity.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grdLeadsActivity.DataSource = dataView;
            grdLeadsActivity.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        // grdLeadsActivity.AllowPaging = !(chkSubAgent.Checked);
        if (chkSubAgent.Checked && wucAgentSelector.m_AgentUID == string.Empty)
        {
            FormHandler.DisplayMessage(Page.ClientScript, "Please select an agent.");
        }
        else
        {
            this.PageSize = 10;
            LoadHistoryPending();
        }
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //ListHandler.ListFindItem(AgentUID, "-1");
        ListHandler.ListFindItem(cboPageSize, "10");

        pnl1.Visible = false;
        lblData.Visible = true;
        LoadHistoryPending();
        chkActive.Checked = false;
        chkSubAgent.Checked = false;
        wucAgentSelector.FormClear();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grdLeadsActivity.PageSize = this.PageSize;
        LoadHistoryPending();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdLeadsActivity.AllowPaging = false;
        LoadHistoryPending();
        grdLeadsActivity.AllowPaging = true;
        FormHandler.Export2Excel("SalesLeadActivityReport.xls", grdLeadsActivity);
    }

    protected void Date_SelectedIndexChanged(object sender, EventArgs e)
    {
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
                //CurrentSort = SortDirection.Ascending;
                break;
            case SortDirection.Descending:
                newSortDirection = "DESC";
                //CurrentSort = SortDirection.Descending;
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

    protected void grdLeadsActivity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandSource is LinkButton)
        {
            switch (e.CommandName)
            {
                case "LeadID":

                    Response.Redirect("~/SecureLeadForms/frmLeadsDetail.aspx?LeadUID=" + e.CommandArgument + "&LeadID=" + e.CommandArgument + "&Adding=false");
                    break;
            }
        }
    }

}
