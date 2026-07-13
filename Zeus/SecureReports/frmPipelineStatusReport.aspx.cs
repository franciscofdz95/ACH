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

public partial class frmPipelineStatusReport : frmBaseSearch
{
    public decimal[] a = new decimal[10];
    public int[] b;

    public string grid
    {
        set
        {
            ViewState["grid"] = value;
        }
        get
        {
            if (ViewState["grid"] != null)
                return ViewState["grid"].ToString();
            else
                return string.Empty;
        }
    }

    public int CurrentPage1
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_CurrentPage1"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_CurrentPage1"]);
        }
        set { ViewState[this.Page.ToString() + "_CurrentPage1"] = value; }
    }

    public int CurrentPage2
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_CurrentPage2"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_CurrentPage2"]);
        }
        set { ViewState[this.Page.ToString() + "_CurrentPage2"] = value; }
    }

    public int CurrentPage3
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_CurrentPage3"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_CurrentPage3"]);
        }
        set { ViewState[this.Page.ToString() + "_CurrentPage3"] = value; }
    }

    public int PageSize1
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_PageSize1"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_PageSize1"]);
        }
        set { ViewState[this.Page.ToString() + "_PageSize1"] = value; }
    }

    public int PageSize2
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_PageSize2"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_PageSize2"]);
        }
        set { ViewState[this.Page.ToString() + "_PageSize2"] = value; }
    }

    public int PageSize3
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_PageSize3"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_PageSize3"]);
        }
        set { ViewState[this.Page.ToString() + "_PageSize3"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkLeadReports")).CssClass = "active";

        if (!IsPostBack)
        {
            SearchBeginDate.Value = DateTime.Today;
            SearchEndDate.Value = DateTime.Today;
            // LookupTableHandler.LoadAgentsNew(AgentUID, true);

            // LoadHistoryPending();
        }

        img1.Attributes.Add("onclick", "CollapseExpand('" + div1.ClientID + "',null,'" + img1.ClientID + "')");
        img2.Attributes.Add("onclick", "CollapseExpand('" + div2.ClientID + "',null,'" + img2.ClientID + "')");
        img3.Attributes.Add("onclick", "CollapseExpand('" + div3.ClientID + "',null,'" + img3.ClientID + "')");
        img4.Attributes.Add("onclick", "CollapseExpand('" + div4.ClientID + "',null,'" + img4.ClientID + "')");
        //AgentUID.Attributes.Add("onchange", "ChangeCheckBox('" + AgentUID.ClientID + "');");//,'" + chkSubAgent.ClientID + "'
    }

    public void LoadHistoryPending(bool isLoad)
    {
        DataSet ds = new DataSet();
        Hashtable prms = new Hashtable();

        if (isDataValid())
        {
            if (wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
            {
                if (chkSubAgent.Checked)
                    prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
                else
                    prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
            }

            prms.Add("@IsActive", chkAgent.Checked);

            if (!string.IsNullOrEmpty(SearchBeginDate.Text))
                SearchBeginDate.Value = DateTime.Today;

            if (!string.IsNullOrEmpty(SearchEndDate.Text))
                SearchEndDate.Value = DateTime.Today;



            prms.Add("@BeginPostedDate", SearchBeginDate.Value);
            prms.Add("@EndPostedDate", SearchEndDate.Value);

            ds = DataAccess.DataMerchantAppDao.GetPipelineStatus(prms);
            pnl1.Visible = false;
            lblData.Visible = true;

            if (ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;

                //PageSize and PageIndex
                grdPipelineStatus.PageSize = this.PageSize;
                grdPipelineStatus.PageIndex = this.CurrentPage - 1;

                GridView1.PageSize = this.PageSize1;
                GridView1.PageIndex = this.CurrentPage1 - 1;

                GridView2.PageSize = this.PageSize2;
                GridView2.PageIndex = this.CurrentPage2 - 1;

                GridView3.PageSize = this.PageSize3;
                GridView3.PageIndex = this.CurrentPage3 - 1;

                //processing Volume on footer
                bool isTrue = ((wucAgentSelector.m_AgentUID != string.Empty) && chkSubAgent.Checked == false);
                grdPipelineStatus.ShowFooter = isTrue;
                GridView1.ShowFooter = isTrue;
                GridView2.ShowFooter = isTrue;
                GridView3.ShowFooter = isTrue;

                if (isLoad)
                {
                    dv.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);
                    ds.Tables[1].DefaultView.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);
                    ds.Tables[2].DefaultView.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);
                    ds.Tables[3].DefaultView.Sort = CurrentExp + " " + GetSortDirectionToSql(CurrentSort);
                }

                if (this.grid == string.Empty || this.grid == "grdPipelineStatus")
                {
                    grdPipelineStatus.DataSource = dv;
                    grdPipelineStatus.DataBind();
                }

                if (this.grid == string.Empty || this.grid == "GridView1")
                {
                    GridView1.DataSource = ds.Tables[1].DefaultView;
                    GridView1.DataBind();
                }

                if (this.grid == string.Empty || this.grid == "GridView2")
                {
                    GridView2.DataSource = ds.Tables[2].DefaultView;
                    GridView2.DataBind();
                }

                if (this.grid == string.Empty || this.grid == "GridView3")
                {
                    GridView3.DataSource = ds.Tables[3].DefaultView;
                    GridView3.DataBind();
                }

                //lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
                lblData.Visible = (ds.Tables[0].Rows.Count == 0);
                Panel1.Visible = pnl1.Visible = (ds.Tables[0].Rows.Count > 0);
            }
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                for (int i = 0; i < 4; i++)
                    a[i] = 0.0M;

                break;

            case DataControlRowType.DataRow:

                a[0] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ActiveVolume"));
                a[1] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DeadVolume"));
                a[2] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ApprovedVolume"));
                a[3] = a[0] + a[1] + a[2];

                e.Row.Cells[5].Text = Convert.ToString(Convert.ToInt32(e.Row.Cells[2].Text) + Convert.ToInt32(e.Row.Cells[3].Text) + Convert.ToInt32(e.Row.Cells[4].Text));

                break;

            case DataControlRowType.Footer:

                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0 && wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
                {
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 2;
                    oCell1.CssClass = "Footer";
                    oCell1.HorizontalAlign = HorizontalAlign.Left;
                    oCell1.Text = "Sum of Processing Volume</td>";

                    oCell1.Text += "<td align='right'>" + a[0].ToString("0.00")/*"c")*/ + "</td>";
                    oCell1.Text += "<td align='right'>" + a[1].ToString("0.00")/*"c")*/ + "</td>";
                    oCell1.Text += "<td align='right'>" + a[2].ToString("0.00")/*"c")*/ + "</td>";
                    oCell1.Text += "<td align='right'>" + a[3].ToString("0.00")/*"c")*/ + "</td>";
                    oCell1.Text += "<td align='right'></td>";

                    oCell1.Text += "</tr>";
                    e.Row.Cells.Add(oCell1);
                }

                break;
        }
    }

    protected void grd1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                for (int i = 0; i < e.Row.Cells.Count - 2; i++)
                    a[i] = 0.0M;

                break;

            case DataControlRowType.DataRow:

                a[0] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NewVol"));
                a[1] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "AssignedVol"));
                a[2] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "In CommunicationVol"));
                a[3] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "No AnswerVol"));
                a[4] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Follow upVol"));
                a[5] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Statements ReceivedVol"));
                a[6] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Application SentVol"));
                a[7] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Application ReceivedVol"));
                a[8] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "File In ReviewVol"));
                a[9] = a[0] + a[1] + a[2] + a[3] + a[4] + a[5] + a[6] + a[7] + a[8];

                e.Row.Cells[11].Text = Convert.ToString(Convert.ToInt32(e.Row.Cells[2].Text) + Convert.ToInt32(e.Row.Cells[3].Text) + Convert.ToInt32(e.Row.Cells[4].Text) +
                    Convert.ToInt32(e.Row.Cells[5].Text) + Convert.ToInt32(e.Row.Cells[6].Text) + Convert.ToInt32(e.Row.Cells[7].Text) +
                    Convert.ToInt32(e.Row.Cells[8].Text) + Convert.ToInt32(e.Row.Cells[9].Text) + Convert.ToInt32(e.Row.Cells[10].Text));

                break;

            case DataControlRowType.Footer:

                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0 && wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
                {
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 2;
                    oCell1.CssClass = "Footer";
                    oCell1.HorizontalAlign = HorizontalAlign.Left;
                    oCell1.Text = "Sum of Processing Volume</td>";

                    for (int j = 0; j < 10; j++)
                        oCell1.Text += "<td align='right'>" + a[j].ToString("0.00")/*"c")*/ + "</td>";

                    oCell1.Text += "</tr>";
                    e.Row.Cells.Add(oCell1);
                }

                break;
        }
    }

    protected void grd2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                for (int i = 0; i < 5; i++)
                    a[i] = 0.0M;

                break;

            case DataControlRowType.DataRow:

                a[0] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Not InterestedVol"));
                a[1] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "WithdrawnVol"));
                a[2] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DeclinedVol"));
                a[3] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Wrong Contact InfoVol"));
                a[4] = a[0] + a[1] + a[2] + a[3];

                e.Row.Cells[6].Text = Convert.ToString(Convert.ToInt32(e.Row.Cells[2].Text) + Convert.ToInt32(e.Row.Cells[3].Text) + Convert.ToInt32(e.Row.Cells[4].Text) + Convert.ToInt32(e.Row.Cells[5].Text));

                break;

            case DataControlRowType.Footer:

                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0 && wucAgentSelector.m_AgentUID != string.Empty)
                {
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 2;
                    oCell1.CssClass = "Footer";
                    oCell1.HorizontalAlign = HorizontalAlign.Left;
                    oCell1.Text = "Sum of Processing Volume</td>";

                    for (int j = 0; j < 5; j++)
                        oCell1.Text += "<td align='right'>" + a[j].ToString("0.00")/*"c")*/ + "</td>";

                    oCell1.Text += "</tr>";
                    e.Row.Cells.Add(oCell1);
                }

                break;
        }
    }

    protected void grd3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                a[0] = 0.0M;

                break;

            case DataControlRowType.DataRow:

                a[0] = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ApprovedVol"));

                break;

            case DataControlRowType.Footer:

                int cnt = ((GridView)sender).Rows.Count;

                if (cnt > 0 && wucAgentSelector.m_AgentUID != string.Empty)
                {
                    e.Row.Cells.Clear();
                    TableCell oCell1 = new TableCell();
                    oCell1.ColumnSpan = 2;
                    oCell1.CssClass = "Footer";
                    oCell1.HorizontalAlign = HorizontalAlign.Left;
                    oCell1.Text = "Sum of Processing Volume</td>";
                    oCell1.Text += "<td align='right'>" + a[0].ToString("0.00")/*"c")*/ + "</td>";

                    oCell1.Text += "</tr>";
                    e.Row.Cells.Add(oCell1);
                }

                break;
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid = ((GridView)sender).ID;
        this.CurrentPage = e.NewPageIndex + 1;
        LoadHistoryPending(false);
    }

    protected void grd1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid = ((GridView)sender).ID;
        this.CurrentPage1 = e.NewPageIndex + 1;
        LoadHistoryPending(false);
    }

    protected void grd2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid = ((GridView)sender).ID;
        this.CurrentPage2 = e.NewPageIndex + 1;
        LoadHistoryPending(false);
    }

    protected void grd3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grid = ((GridView)sender).ID;
        this.CurrentPage3 = e.NewPageIndex + 1;
        LoadHistoryPending(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        grid = ((GridView)sender).ID;
        LoadHistoryPending(false);
        DataTable dataTable = ((DataView)((GridView)sender).DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            ((GridView)sender).DataSource = dataView;
            ((GridView)sender).DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void ddlTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
    {

        switch (ddlTimeFrame.SelectedValue)
        {
            case "1":
                // last 30 days
                SearchBeginDate.Value = DateTime.Now.AddDays(-30);
                SearchEndDate.Value = DateTime.Now;

                break;

            case "2":
                // last 60 days
                SearchBeginDate.Value = DateTime.Now.AddDays(-60);
                SearchEndDate.Value = DateTime.Now;
                break;

            case "3":
                // last 90 days
                SearchBeginDate.Value = DateTime.Now.AddDays(-90);
                SearchEndDate.Value = DateTime.Now;
                break;

            case "4":
                // last month
                DateTime dtLastMonth = DateTime.Now.AddMonths(-1);
                SearchBeginDate.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, 1);
                SearchEndDate.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, DateTime.Now.AddDays(DateTime.Now.Day * -1).Day);
                break;

            case "5":
                // month to date
                SearchBeginDate.Value = DateTime.Now.AddDays((DateTime.Now.Day * -1) + 1);
                SearchEndDate.Value = DateTime.Now;
                break;

        }

    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //grdPipelineStatus.AllowPaging = !(chkSubAgent.Checked);
        if (chkSubAgent.Checked && wucAgentSelector.m_AgentUID == string.Empty)
        {
            FormHandler.DisplayMessage(Page.ClientScript, "Please select an agent.");
        }
        else
        {
            this.SortDirectionSearch = SortDirection.Descending;
            this.SortOrder = "AgentID";
            this.grid = string.Empty;
            LoadHistoryPending(true);
        }
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        SearchBeginDate.Value = DateTime.Today;
        SearchEndDate.Value = DateTime.Today;
        //ListHandler.ListFindItem(AgentUID, "-1");
        ListHandler.ListFindItem(cboPageSize, "10");
        ListHandler.ListFindItem(DropDownList1, "10");
        ListHandler.ListFindItem(DropDownList2, "10");
        ListHandler.ListFindItem(DropDownList3, "10");

        pnl1.Visible = false;
        lblData.Visible = true;
        grid = string.Empty;
        LoadHistoryPending(false);
        chkAgent.Checked = false;
        chkSubAgent.Checked = false;
        wucAgentSelector.FormClear();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dpPageSize = ((DropDownList)sender);
        if (dpPageSize.ID.ToUpper() == "CBOPAGESIZE")
        {
            grid = "grdPipelineStatus";
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        }
        else if (dpPageSize.ID.ToUpper() == "DROPDOWNLIST1")
        {
            grid = "GridView1";
            this.CurrentPage1 = 1;
            this.PageSize1 = Convert.ToInt32(DropDownList1.SelectedValue);
        }
        else if (dpPageSize.ID.ToUpper() == "DROPDOWNLIST2")
        {
            grid = "GridView2";
            this.CurrentPage2 = 1;
            this.PageSize2 = Convert.ToInt32(DropDownList2.SelectedValue);
        }
        else if (dpPageSize.ID.ToUpper() == "DROPDOWNLIST3")
        {
            grid = "GridView3";
            this.CurrentPage3 = 1;
            this.PageSize3 = Convert.ToInt32(DropDownList3.SelectedValue);
        }

        LoadHistoryPending(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        LoadHistoryPending(false);
        grdPipelineStatus.AllowPaging = false;

        FormHandler.Export2Excel("SalesOfficeActivityReport.xls", grdPipelineStatus);
    }

    //protected void Date_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadHistoryPending(false);
    //}

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

    private bool isDataValid()
    {
        if (string.IsNullOrEmpty(SearchBeginDate.Text))
        {
            lblError.Items.Add("Start Date Required");
            return false;
        }

        if (string.IsNullOrEmpty(SearchEndDate.Text))
        {

            lblError.Items.Add("End Date Required");
            return false;
        }



        DateTime dtS = DateTime.Parse(SearchBeginDate.Text);
        DateTime dtE = DateTime.Parse(SearchEndDate.Text);

        if (dtS > dtE)
        {
            lblError.Items.Add("Begin Date must be less than End Date");
            return false;
        }

        return true;
    }

}
