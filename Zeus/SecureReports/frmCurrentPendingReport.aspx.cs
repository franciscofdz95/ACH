using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class frmCurrentPendingReport : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            LookupTableHandler.LoadUsersByRole(PrimaryContactUID, true, Constants.ROLE_AGENT_RELATIONS);
            LookupTableHandler.GetAgentCategories(AgentCategoryUID);
            LookupTableHandler.LoadPartnerChannels(PartnerChannel, true);

            SetPartnerChannelAccess();

            LoadCurrentPending();
            CurrentExp = "PendedDays";
            CurrentSort = SortDirection.Descending;
        }
    }

    protected void grdCurrentPending_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                string[] str = DataBinder.Eval(e.Row.DataItem, "Conditions").ToString().Split('~');
                StringBuilder strCon = new StringBuilder();

                for (int i = 1; i < str.Length; i++)
                    strCon.Append("<dt>" + i.ToString() + ". " + str[i].Replace("~", "").Trim().Replace("^", "</dt><dd><li>") + "</li></dd>");

                e.Row.Cells[10].Text = strCon.ToString();
                e.Row.Cells[10].Width = Unit.Pixel(250);
                e.Row.Cells[9].Text = e.Row.Cells[9].Text.Substring(0, 2);
                break;
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadCurrentPending();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadCurrentPending();
        SortGrid(e.SortExpression);
    }

    protected void SortGrid(string SortExpression)
    {
        DataTable dataTable = ((DataView)grdCurrentPending.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != SortExpression)
                CurrentSort = SortDirection.Descending;
            dataView.Sort = SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grdCurrentPending.DataSource = dataView;
            grdCurrentPending.DataBind();
            CurrentExp = SortExpression;
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SearchParameters = null;
        this.CurrentPage = 1;
        this.PageSize = 10;
        cboPageSize.SelectedIndex = 0;
        CurrentSort = SortDirection.Descending;
        CurrentExp = "PendedDays";
        LoadCurrentPending();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        FormHandler.ClearAllControls(pnlSearch);
        pnl1.Visible = false;
        lblData.Visible = true;
        AgentCategoryUID.SelectedIndex = -1;

        wucAgentSelector.FormClear(); 
        if (!UserSessions.CurrentUser.IsAgent)
            PartnerChannel.SelectedIndex = -1;

        this.CurrentPage = 1;
        this.PageSize = 10;
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdCurrentPending.AllowPaging = false;
        LoadCurrentPending();
        FormHandler.Export2Excel("CurrentPendingReport.xls", grdCurrentPending);
        grdCurrentPending.AllowPaging = true;
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        LoadCurrentPending();
    }

    protected void ddlTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
    {

        switch (ddlTimeFrame.SelectedValue)
        {
            default:
                // change your dates
                SearchBeginDate.Value = DateTime.Today;
                SearchEndDate.Value = DateTime.Today;

                break;
            case "1":
                // last 30 days
                SearchBeginDate.Value = DateTime.Today.AddDays(-30);
                SearchEndDate.Value = DateTime.Today;

                break;

            case "2":
                // last 60 days
                SearchBeginDate.Value = DateTime.Today.AddDays(-60);
                SearchEndDate.Value = DateTime.Today;
                break;

            case "3":
                // last 90 days
                SearchBeginDate.Value = DateTime.Today.AddDays(-90);
                SearchEndDate.Value = DateTime.Today;
                break;

            case "4":
                // last month
                DateTime dtLastMonth = DateTime.Today.AddMonths(-1);
                SearchBeginDate.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, 1);
                SearchEndDate.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, DateTime.Today.AddDays(DateTime.Today.Day * -1).Day);
                break;

            case "5":
                // month to date
                SearchBeginDate.Value = DateTime.Today.AddDays((DateTime.Today.Day * -1) + 1);
                SearchEndDate.Value = DateTime.Today;
                break;
        }
    }

    public void LoadCurrentPending()
    {        
        if (UserSessions.CurrentUser.IsAgent && PartnerChannel.SelectedIndex < 0)
        {
            blError.Items.Add("Agent must have a channel.");
            return;
        }

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
        
        if (!string.IsNullOrWhiteSpace(PartnerChannel.SelectedValue))
            prms.Add("@AgentGroupID", PartnerChannel.SelectedValue);

        string AgentCatUID = string.Empty;

        for (int i = 0; i < AgentCategoryUID.Items.Count; i++)
        {
            if (AgentCategoryUID.Items[i].Selected)

                AgentCatUID += AgentCategoryUID.Items[i].Value + ",";
        }

        if (AgentCatUID != string.Empty)
            prms.Add("@AgentCategoryUID", AgentCatUID.TrimEnd(','));

        ds = DataAccess.DataMerchantAppDao.GetCurrentPending(prms);
        DataView dv = ds.Tables[0].DefaultView;

        grdCurrentPending.PageSize = this.PageSize;
        grdCurrentPending.PageIndex = this.CurrentPage - 1;

        dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);

        grdCurrentPending.DataSource = dv;
        grdCurrentPending.DataBind();

        lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
        lblData.Visible = (ds.Tables[0].Rows.Count == 0);
        pnl1.Visible = (ds.Tables[0].Rows.Count > 0);

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
                return SortDirection.Descending;
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
                return "PendedDays";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    private void SetPartnerChannelAccess()
    {
        if (UserSessions.CurrentUser.IsAgent)
        {
            DataAgent data = new DataAgent();
            Agent app = data.GetAgent(UserSessions.CurrentUser.AgentUID);

            if (app != null)
            {
                ListHandler.ListFindItem(PartnerChannel, app.AgentGroupID.ToString());
            }

            PartnerChannel.Enabled = false;
        }

        if (UserSessions.CurrentUser.IsInternal)
        {
            List<int> partnerChannel = DataUser.GetInstance().GetUserPartnerChannelAccess(UserSessions.CurrentUser.UID);

            List<ListItem> remove = new List<ListItem>();

            foreach (ListItem item in this.PartnerChannel.Items)
            {
                //
                bool found = false;

                foreach (int pChannelId in partnerChannel)
                {

                    if (item.Value == pChannelId.ToString())
                    {
                        found = true;
                        break;
                    }
                }

                //don't remove the "All" which has a value of an empty 
                //string from the drop down list item
                if (!found && !item.Value.Equals(""))
                {
                    remove.Add(item);
                }
            }

            //remove the "All" drop down item if we're removing any items
            if (remove.Count > 0)
            {
                this.PartnerChannel.Items.RemoveAt(0);
            }

            foreach (ListItem del in remove)
            {
                this.PartnerChannel.Items.Remove(del);
            }
        }
    }
}
