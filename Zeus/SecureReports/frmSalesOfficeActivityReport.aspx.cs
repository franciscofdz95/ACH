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
using System.Collections.Generic;

public partial class frmSalesOfficeActivityReport : frmBaseSearch
{
    public int ColumnCount
    {
        set { ViewState["ColumnCount"] = value; }
        get { if (ViewState["ColumnCount"] == null) return 0; else return Convert.ToInt32(ViewState["ColumnCount"]); }
    }

    public decimal[] a;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            SearchBeginDate.Value = DateTime.Today;
            LookupTableHandler.LoadUsersByRole(PrimaryContactUID, true, Constants.ROLE_AGENT_RELATIONS);
            LookupTableHandler.GetAgentCategories(AgentCategoryUID);
            LookupTableHandler.LoadPartnerChannels(PartnerChannel,true);

            SetPartnerChannelAccess();

            


            LoadHistoryPending();
        }
    }

    public void LoadHistoryPending()
    {

        if (UserSessions.CurrentUser.IsAgent && PartnerChannel.SelectedIndex < 0)
        {
            blError.Items.Add("Agent must have a channel.");
            return;
        }

        DataSet ds = new DataSet();
        Hashtable prms = new Hashtable();

        if (wucAgentSelector.m_AgentUID != string.Empty)
        {
            if (chkSubAgent.Checked)
                prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
            else
                prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
        }

        if (PrimaryContactUID.SelectedIndex > 0)
            prms.Add("@PrimaryContactUID", PrimaryContactUID.SelectedValue);

        if (string.IsNullOrEmpty(SearchBeginDate.Text))
            SearchBeginDate.Value = DateTime.Today;

        prms.Add("@Date", SearchBeginDate.Value);

        string AgentCatUID = string.Empty;

        for (int i = 0; i < AgentCategoryUID.Items.Count; i++)
        {
            if(AgentCategoryUID.Items[i].Selected)

            AgentCatUID += AgentCategoryUID.Items[i].Value + ",";
        }

        if (AgentCatUID != string.Empty)
            prms.Add("@AgentCategoryUID", AgentCatUID.TrimEnd(','));

        if (!string.IsNullOrWhiteSpace(PartnerChannel.SelectedValue))
            prms.Add("@AgentGroupID", PartnerChannel.SelectedValue);

        ds = DataAccess.DataMerchantAppDao.GetSalesOfficeActivity(prms);

        pnl1.Visible = false;
        lblData.Visible = true;

        if (ds.Tables.Count > 0)
        {
            DataView dv = ds.Tables[0].DefaultView;

            this.ColumnCount = dv.Table.Columns.Count;

            grdSalesActivity.PageSize = this.PageSize;
            grdSalesActivity.PageIndex = this.CurrentPage - 1;
            grdSalesActivity.ShowFooter = (chkSubAgent.Checked);

            dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);

            grdSalesActivity.DataSource = dv;
            grdSalesActivity.DataBind();

            lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
            lblData.Visible = (ds.Tables[0].Rows.Count == 0);
            pnl1.Visible = (ds.Tables[0].Rows.Count > 0);
        }

    }

    protected void grdSalesActivity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                a = new decimal[ColumnCount];
                for (int t = 0; t < this.ColumnCount; t++)
                    a[t] = 0.0M;

                e.Row.Cells[0].Text = "Rep #";
                e.Row.Cells[1].Text = "Name";
                e.Row.Cells[2].Text = "Activation<br>Date";
                e.Row.Cells[3].Text = "Monthly<br>Avg";
                e.Row.Cells[4].Text = "Monthly<br>Expected";
                e.Row.Cells[5].Text = "Overall<br>Approval %";
                e.Row.Cells[6].Text = "MTD";
                e.Row.Cells[7].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-1).Month);
                e.Row.Cells[8].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-2).Month);
                e.Row.Cells[9].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-3).Month);
                e.Row.Cells[10].Text = "YTD";
                e.Row.Cells[11].Text = "MTD";
                e.Row.Cells[12].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-1).Month);
                e.Row.Cells[13].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-2).Month);
                e.Row.Cells[14].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-3).Month);
                e.Row.Cells[15].Text = "YTD";
                //e.Row.Cells[16].Text = "MTD";
                //e.Row.Cells[17].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-1).Month);
                //e.Row.Cells[18].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-2).Month);
                //e.Row.Cells[19].Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToDateTime(SearchBeginDate.Value).AddMonths(-3).Month);
                //e.Row.Cells[20].Text = "YTD";

                //int k = 21;
                //while (k < this.ColumnCount)
                //{
                //    if (k < this.ColumnCount)
                //        e.Row.Cells[k++].Text = "R";
                //    if (k < this.ColumnCount)
                //        e.Row.Cells[k++].Text = "A";
                //    if (k < this.ColumnCount)
                //        e.Row.Cells[k++].Text = "D";
                //    if (k < this.ColumnCount)
                //        e.Row.Cells[k++].Text = "W";
                //    if (k < this.ColumnCount)
                //        e.Row.Cells[k++].Text = "M";
                //}

                TableCell oCell2 = new TableCell();
                DateTime dt = Convert.ToDateTime(SearchBeginDate.Value);
                oCell2.ColumnSpan = 3;
                oCell2.Attributes.Add("style", "background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;");
                oCell2.HorizontalAlign = HorizontalAlign.Center;
                oCell2.Text = "Summary Data";
                oCell2.Text += "<td align='center' colspan='3' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Deals</td>";
                oCell2.Text += "<td align='center' colspan='5' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Transaction Count</td>";
                oCell2.Text += "<td align='center' colspan='5' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Processing Volume</td>";
                //oCell2.Text += "<td align='center' colspan='5' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Residuals</td>";
                //oCell2.Text += "<td align='center' colspan='5' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>MTD</td>";

                //for (int i = 22, j = 1; i < this.ColumnCount - 10; i = i + 5, j++)
                //    oCell2.Text += "<td align='center' colspan='5' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>" + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dt.AddDays(-1).AddMonths(j * -1).Month) + "</td>";
                //oCell2.Text += "<td align='center' colspan='5' style='background-color:#e5e5e5;font-family: Verdana;font-size: small; font-weight:bold;'>Totals</td>";
                oCell2.Text += "</tr>";
                e.Row.Cells.AddAt(0, oCell2);

                break;

            case DataControlRowType.DataRow:

                e.Row.Cells[0].Width = Unit.Pixel(10);
                e.Row.Cells[1].Width = Unit.Pixel(150);
                e.Row.Cells[2].Width = Unit.Pixel(75);
                if (e.Row.Cells[2].Text == "&nbsp;") e.Row.Cells[2].Text = string.Empty;
                if (e.Row.Cells[2].Text != string.Empty)
                    e.Row.Cells[2].Text = DateTime.Parse(e.Row.Cells[2].Text).ToString("MM/dd/yyyy");
                e.Row.Cells[3].Width = Unit.Pixel(50);
                e.Row.Cells[4].Width = Unit.Pixel(50);
                e.Row.Cells[5].Width = Unit.Pixel(50);
                e.Row.Cells[6].Width = Unit.Pixel(50);
                e.Row.Cells[7].Width = Unit.Pixel(50);
                e.Row.Cells[8].Width = Unit.Pixel(50);
                e.Row.Cells[9].Width = Unit.Pixel(50);
                e.Row.Cells[10].Width = Unit.Pixel(50);
                e.Row.Cells[11].Width = Unit.Pixel(50);
                e.Row.Cells[12].Width = Unit.Pixel(50);
                e.Row.Cells[13].Width = Unit.Pixel(50);
                e.Row.Cells[14].Width = Unit.Pixel(50);
                e.Row.Cells[15].Width = Unit.Pixel(50);
                //e.Row.Cells[16].Width = Unit.Pixel(50);
                //e.Row.Cells[17].Width = Unit.Pixel(50);
                //e.Row.Cells[18].Width = Unit.Pixel(50);

                for (int d = 3; d < this.ColumnCount; d++)
                {
                    if (e.Row.Cells[d].Text == string.Empty || e.Row.Cells[d].Text == "&nbsp;")
                        e.Row.Cells[d].Text = "0";

                    if (d < 16 && d >= 11)
                    {
                        e.Row.Cells[d].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[d].Text = Convert.ToDecimal(e.Row.Cells[d].Text).ToString("0.00");/*"c")*/
                    }
                    else if (d == 3 || d == 5)
                    {
                        e.Row.Cells[d].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[d].Text = Convert.ToDecimal(e.Row.Cells[d].Text).ToString("0.00");
                    }
                    //else if (d > 18)
                    //{
                    //    e.Row.Cells[d].HorizontalAlign = HorizontalAlign.Right;
                    //    e.Row.Cells[d].Width = Unit.Pixel(10);
                    //}
                    else if (d > 5 && d < 11)
                    {
                        e.Row.Cells[d].HorizontalAlign = HorizontalAlign.Right;
                        e.Row.Cells[d].Text = Convert.ToInt32(e.Row.Cells[d].Text).ToString("N0");
                    }
                }

                //for (int cn = 25; cn < this.ColumnCount; cn = cn + 5)
                //{
                //    e.Row.Cells[cn].Text = Convert.ToDecimal(e.Row.Cells[cn].Text).ToString("c");
                //}

                for (int s = 0; s < this.ColumnCount; s++)
                {
                    e.Row.Cells[s].ToolTip = e.Row.Cells[s].Text;

                    if (s >= 3)
                    {
                        if (s == 4 || s == 16) continue;
                        else if (s == 3)
                            a[s] += Convert.ToDecimal(e.Row.Cells[s].Text.Replace("$", "").Replace("(", "-").Replace(")", "")) * DateTime.Today.AddDays(-1).AddMonths(-1).Month;
                        else
                            a[s] += Convert.ToDecimal(e.Row.Cells[s].Text.Replace("$", "").Replace("(", "-").Replace(")", ""));
                    }
                }

                break;

            case DataControlRowType.Footer:

                for (int b = 3; b < this.ColumnCount; b++)
                {
                    if (b == 4 || b == 5) continue;
                    else if (b == 3)
                        e.Row.Cells[b].Text = Convert.ToDecimal(a[b] / DateTime.Today.AddDays(-1).AddMonths(-1).Month).ToString();
                    else if (b >= 11 && b <= 15)
                        e.Row.Cells[b].Text = a[b].ToString("0.00");//"c");
                    else
                        e.Row.Cells[b].Text = a[b].ToString("N0");
                }

                //if (!a[ColumnCount - 4].Equals(0))
                //{
                //    e.Row.Cells[16].Text = Convert.ToDecimal(a[ColumnCount - 3] / a[ColumnCount - 4]).ToString("0.00");
                //}
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
        DataTable dataTable = ((DataView)grdSalesActivity.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grdSalesActivity.DataSource = dataView;
            grdSalesActivity.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        grdSalesActivity.AllowPaging = !(chkSubAgent.Checked);
        this.CurrentPage = 1;
        this.PageSize = 10;
        cboPageSize.SelectedIndex = 0;

        if (chkSubAgent.Checked && wucAgentSelector.m_AgentUID == string.Empty)
            FormHandler.DisplayMessage(Page.ClientScript, "Select an agent.");
        else
        {
            this.SortDirectionSearch = SortDirection.Descending;
            this.SortOrder = "";
            LoadHistoryPending();
        }
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        SearchBeginDate.Value = DateTime.Today;

        pnl1.Visible = false;
        lblData.Visible = true;
        chkSubAgent.Checked = false;
        wucAgentSelector.FormClear();
        PrimaryContactUID.SelectedIndex = 0;
        AgentCategoryUID.SelectedIndex = -1;
        if(!UserSessions.CurrentUser.IsAgent)
            PartnerChannel.SelectedIndex = -1;

        this.CurrentPage = 1;
        this.PageSize = 10;
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grdSalesActivity.PageSize = this.PageSize;
        LoadHistoryPending();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        grdSalesActivity.AllowPaging = false;
        LoadHistoryPending();

        GridViewRow grdRow = grdSalesActivity.Rows[0];
        for (int i = 0; i < this.ColumnCount; i++)
            grdRow.Cells[i].Width = Unit.Pixel(50);
        grdRow.Cells[2].Width = Unit.Pixel(75);

        FormHandler.Export2Excel("SalesOfficeActivityReport.xls", grdSalesActivity);

        grdSalesActivity.AllowPaging = true;
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
