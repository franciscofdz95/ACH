using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;


public partial class wucFTTicketList : System.Web.UI.UserControl
{

    public Hashtable m_Prms
    {
        get
        {
            if (ViewState["m_Prms"] == null)
                return (new Hashtable());
            else
                return (Hashtable)ViewState["m_Prms"];
        }
        set { ViewState["m_Prms"] = value; }
    }

    public int CurrentPage
    {
        get
        {
            if (ViewState["CurrentPage"] == null)
                return 1;
            else
                return (int)ViewState["CurrentPage"];
        }
        set { ViewState["CurrentPage"] = value; }
    }

    public int PageSize
    {
        get
        {
            if (ViewState["PageSize"] == null)
                return 10;
            else
                return (int)ViewState["PageSize"];
        }
        set { ViewState["PageSize"] = value; }
    }

    public SortDirection SortDirectionSearch
    {
        get
        {
            if (ViewState["SortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)ViewState["SortDirectionSearch"];
        }
        set { ViewState["SortDirectionSearch"] = value; }

    }

    public string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
                return string.Empty;
            else
                return ViewState["SortOrder"].ToString();
        }
        set { ViewState["SortOrder"] = value; }
    }

    public string PostBackURL
    {
        get
        {
            if (ViewState["PostBackURL"] == null)
                return string.Empty;
            else
                return ViewState["PostBackURL"].ToString();
        }
        set { ViewState["PostBackURL"] = value; }
    }

    //public string DataSourceSelectMethod
    //{
        //get { return odsTransactions.SelectMethod; }
        //set { odsTransactions.SelectMethod = value; }
   // }

    //public string DataSourceSelectCountMethod
    //{
    //    get { return odsTransactions.SelectCountMethod; }
    //    set { odsTransactions.SelectCountMethod = value; }
    //}

    public Unit GridHeight
    {
        set { grd.Height = value; }
    }

    public GridView Grid
    {
        get { return grd; }
    }

    public void SetDataSource(Hashtable prms, int pagesize)
    {
        grd.DataSourceID = "odsTransactions";
        this.CurrentPage = 1;
        this.PageSize = pagesize;
        grd.PageSize = pagesize;
        this.m_Prms = prms;
        BindGrid();
    }

    private void BindGrid()
    {
        //if (UserSessions.CurrentUser.IsAgent)
        //{
        //    if (!m_Prms.ContainsKey("@MasterAgentUID"))
        //        m_Prms.Add("@MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);
        //    else
        //        m_Prms["@MasterAgentUID"] = UserSessions.CurrentUser.HookTableKeyUID;
        //}

        //if (!AgentFirstName.Text.Equals(string.Empty))
        //{
        //    if (!m_Prms.ContainsKey("@FirstName"))
        //        m_Prms.Add("@FirstName", AgentFirstName.Text);
        //    else
        //        m_Prms["@FirstName"] = AgentFirstName.Text;
        //}

        //if (!AgentLastName.Text.Equals(string.Empty))
        //{
        //    if (!m_Prms.ContainsKey("@LastName"))
        //        m_Prms.Add("@LastName", AgentLastName.Text);
        //    else
        //        m_Prms["@LastName"] = AgentLastName.Text;
        //}

        //if (!AgentDBA.Text.Equals(string.Empty))
        //{
        //    if (!m_Prms.ContainsKey("@DBA"))
        //        m_Prms.Add("@DBA", AgentDBA.Text);
        //    else
        //        m_Prms["@DBA"] = AgentDBA.Text;
        //}

        //if (!AgentID.Text.Equals(string.Empty))
        //{
        //    if (!m_Prms.ContainsKey("@ID"))
        //        m_Prms.Add("@ID", AgentID.Text);
        //    else
        //        m_Prms["@ID"] = AgentID.Text;
        //}


        //if (m_Prms != null && m_Prms.Count > 0)
        //{
        //    if (!m_Prms.ContainsKey("@PageSize"))
        //        m_Prms.Add("@PageSize", this.PageSize);
        //    else
        //        m_Prms["@PageSize"] = this.PageSize;

        //    if (!m_Prms.ContainsKey("@CurrentPage"))
        //        m_Prms.Add("@CurrentPage", this.CurrentPage);
        //    else
        //        m_Prms["@CurrentPage"] = this.CurrentPage;

        //    if (!m_Prms.ContainsKey("@SortOrder"))
        //        m_Prms.Add("@SortOrder", "BusinessDBAName");
        //    else
        //        m_Prms["@SortOrder"] = this.SortOrder;

        //    m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        //    grd.DataBind();

        //    lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetAgentsPagingRowCount(m_Prms, 0, 0, this.ID).ToString();
        //    pnlRecords.Visible = grd.Rows.Count > 0;
        //    pnlNoRecords.Visible = grd.Rows.Count == 0;
        //}
    }

    public void ClearGrid()
    {
        //AgentFirstName.Text = string.Empty;
        //AgentLastName.Text = string.Empty;
        //AgentID.Text = string.Empty;
        //AgentDBA.Text = string.Empty;
        //lblRecordCount.Text = "Total Record(s) Found: 0";
        //grd.Columns.Clear();
        //pnlRecords.Visible = false;
        //pnlNoRecords.Visible = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            //Set the current page
            UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Ticket";

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists
            //LookupTableHandler.LoadAgentsNew(AgentAgentID, true);
            //LookupTableHandler.LoadInternalUsers(AssignToUID, true);
            //LookupTableHandler.LoadInternalUsers(UserCreated, true);
            //LookupTableHandler.MerchantAppStatus(StatusID, true, "Ticket");
            //LookupTableHandler.LoadCategories(CategoryID, true);

            //LookupTableHandler.LoadTicketCategories(CategoryID, true, -1, "i", "0");


            //LookupTableHandler.LoadDepartments(DepartmentID, true);
            ///ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());

            SortOrder = "TicketID";
            SortDirectionSearch = SortDirection.Descending;
            this.Search(true);
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //SearchParameters = null;
        CurrentPage = 1;
        grd.PageIndex = 0;
        SortOrder = "TicketID";
        SortDirectionSearch = SortDirection.Descending;

        Search(false);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureTicketForms/frmTicketDetail.aspx?Adding=true";
        url += "&PostBackURL=~/SecureTicketForms/frmTicketSearch.aspx";
        Response.Redirect(url);
    }

    protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter ticket = null;

        //if (BusinessDBAName.Text != string.Empty)
        //    prms.Add("@DBAName", BusinessDBAName.Text);

        //if (wucAgentSelector.m_AgentUID != string.Empty)
        //    prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
        //else if (UserSessions.CurrentUser.IsAgent)
        //{
        //    if (!prms.ContainsKey("@MasterAgentUID"))
        //        prms.Add("@MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);
        //    else
        //        prms["@MasterAgentUID"] = UserSessions.CurrentUser.HookTableKeyUID;
        //}

        //if (StatusID.SelectedIndex > 0)
        //    prms.Add("@StatusUID", StatusID.SelectedItem.Value);

        //if (DepartmentID.SelectedIndex > 0)
        //    prms.Add("@DepartmentID", DepartmentID.SelectedItem.Value);

        //if (SubCategory.SelectedIndex > 0)
        //    prms.Add("@CategoryID", SubCategory.SelectedItem.Value);

        //if (CategoryID.SelectedIndex > 0)
        //    prms.Add("@ParentID", CategoryID.SelectedItem.Value);

        //if (AssignToUID.SelectedIndex > 0)
        //    prms.Add("@UserUID", AssignToUID.SelectedItem.Value);

        //if (UserCreated.SelectedIndex > 0)
        //    prms.Add("@UserCreated", UserCreated.SelectedItem.Value);

        //if (BusinessContact.Text != string.Empty)
        //    prms.Add("@ContactName", BusinessContact.Text);

        //if (TicketID.Text != string.Empty)
        //    prms.Add("@TicketID", TicketID.Text);

        //if (TimeZone.SelectedIndex > 0)
        //    prms.Add("@TimeZone", TimeZone.SelectedItem.Value);

        //if (SettlePlatformMid.Text != string.Empty)
        //    prms.Add("@MID", SettlePlatformMid.Text);

        //switch (TicketSource.SelectedValue)
        //{
        //    case "a":
        //        prms.Add("@TicketSource", "a");
        //        break;

        //    case "m":
        //        prms.Add("@TicketSource", "m");
        //        break;

        //    case "i":
        //        prms.Add("@TicketSource", "i");
        //        break;

        //    case "x":
        //        prms.Add("@TicketSource", "x");
        //        break;

        //    default:
        //        // do nothing
        //        break;
        //}

        //if (CommonUtility.Util.if_i(Origin.SelectedValue, 0) > 0)
        //{
        //    prms.Add("@Origin", Convert.ToInt32(Origin.SelectedValue));
        //}

        //if (Priority.SelectedIndex > 0)
        //    prms.Add("@Priority", Priority.SelectedItem.Value);

        //if (!string.IsNullOrEmpty(CallbackDate.Text))
        //    prms.Add("@CallbackDate", Convert.ToDateTime(CallbackDate.Value));

        //if (!string.IsNullOrEmpty(DateCreated.Text))
        //    prms.Add("@DateCreated", Convert.ToDateTime(DateCreated.Value));

        //if (!string.IsNullOrEmpty(SearchBeginDate.Text))
        //    prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        //if (!string.IsNullOrEmpty(SearchEndDate.Text))
        //    prms.Add("@EndPostedDate", SearchEndDate.Value);

        //if (!string.IsNullOrEmpty(Tags.Text))
        //    prms.Add("@Tags", Tags.Text);

        ////If procedure is called for the first time pass a dummy parameter to initial the grid
        //if (prms.Count > 0)
        //{
        //    //Save search fields in session variable
        //    ticket = new SearchParameter();
        //    FormBinding.BindControlsToObject(ticket, pnlSearch);
        //    this.SearchParameters = ticket;

        //    prms.Add("@PageSize", this.PageSize);
        //    prms.Add("@CurrentPage", this.CurrentPage);
        //    grd.PageSize = this.PageSize;
        //    grd.PageIndex = this.CurrentPage - 1;

        //    if (this.SortOrder == string.Empty)
        //        this.SortOrder = "TicketID";
        //    prms.Add("@SortOrder", this.SortOrder);

        //    prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        //}
        //else
        //{
        //    prms.Add("@TicketID", 0);
        //}

        //e.InputParameters[0] = prms;
        //e.InputParameters[3] = this.grd.ID;
        //lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetTicketsPagingCount(prms, 0, 0, this.grd.ID).ToString();
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        switch (e.CommandName)
        {
            case "ID":
                LinkButton btn = (LinkButton)e.CommandSource;
                if (btn.Text == "0")
                    return;
                else
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@UID", e.CommandArgument.ToString());
                    Ticket ticket = DataAccess.DataTicketDao.GetTicket(prms);
                    UserSessions.CurrentTicket = ticket;
                    bool AlertEnable = false;
                    string key = Constants.ALERT_MERCHANTTICKETS;
                    AgentAlerts objAlerts = null;
                    IDictionary<string, AgentAlerts> agentAlerts = null;

                    if (ticket.AgentID != string.Empty)
                    {
                        agentAlerts = DataAccess.DataAgentDao.LoadAgentAlerts(ticket.AgentID);

                        if (agentAlerts.TryGetValue(key, out objAlerts) && objAlerts.Checked)
                        {
                            AlertEnable = true;
                        }
                    }

                    url = "frmTicketDetail.aspx?Adding=false";
                    url += "&PostBackURL=~/SecureTicketForms/frmTicketSearch.aspx";
                    url += "&TicketID=" + btn.Text.Trim() + "&AlertEnable=" + AlertEnable.ToString();
                    Response.Redirect(url);
                }
                break;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lbtnTicketID");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString();

                e.Row.Cells[4].Attributes.Add("class", "text");

                Label lbSolution = (Label)e.Row.FindControl("lblSolution");

                if (lbSolution != null)
                {
                    lbSolution.ToolTip = lbSolution.Text;
                    lbSolution.Text = CommonUtility.Util.TruncateText(lbSolution.Text, 100);
                }

                Image img1 = ((Image)e.Row.FindControl("img"));

                string user = UserSessions.CurrentUser.UserName.ToLower();
                string notecreated = DataBinder.Eval(e.Row.DataItem, "NoteCreatedBy").ToString().ToLower();
                string assignedto = DataBinder.Eval(e.Row.DataItem, "UserUID").ToString().ToLower();
                string usermodified = DataBinder.Eval(e.Row.DataItem, "UserModified").ToString().ToLower();

                if (assignedto == UserSessions.CurrentUser.UID.ToLower())
                    img1.Visible = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "AttentionReq"));
                else if ((notecreated == string.Empty || notecreated != user) && usermodified != user)
                    img1.Visible = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "AttentionReq"));
                else
                    img1.Visible = false;


                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd.PageIndex = e.NewPageIndex;
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        //this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    public void Search(bool IsOnLoad)
    {
        //Populate search fields        
        //if (IsOnLoad && this.SearchParameters != null)
        //{
        //    SearchParameter Ticket = (SearchParameter)this.SearchParameters;
        //    FormBinding.BindObjectToControls(Ticket, pnlSearch);
        //}

        //grd.DataBind();

        //if (IsOnLoad)
        //    grd.Sort(this.SortOrder, this.SortDirectionSearch);

        //pnlRecords.Visible = grd.Rows.Count > 0;
        //pnlNoRecords.Visible = grd.Rows.Count == 0;
    }

    private void FormClear()
    {
       // this.SearchParameters = null;
        FormHandler.ClearAllControls(this);

        //grd.Columns.Clear();
        //ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        //grd.PageIndex = 0;
        //this.Search(false);

        //pnlRecords.Visible = false;
        //pnlNoRecords.Visible = true;
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        //if (rdExport.SelectedValue.Equals("1"))
        //{
        //    this.PageSize = 5000;
        //    this.CurrentPage = 1;
        //}
        //else
        //{
        //    this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        //    this.CurrentPage = grd.PageIndex + 1;
        //}
        //TogglegridFields(true);
        //Search(false);
        //this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        //FormHandler.Export2Excel("TicketList.xls", grd);
        //TogglegridFields(false);
    }

    public void TogglegridFields(bool visible)
    {
        for (int i = 0; i < grd.Columns.Count; i++)
        {
            if (grd.Columns[i].ItemStyle.CssClass == "togle")
            {
                grd.Columns[i].Visible = visible;
            }
        }
    }

    protected void CategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        //pnlCat.Visible = false;

        //if (CategoryID.SelectedIndex > 0)
        //{
        //    pnlCat.Visible = true;
        //    LookupTableHandler.LoadTicketCategories(SubCategory, true, -1, "i", CategoryID.SelectedValue);
        //    //LookupTableHandler.LoadSubCategories(SubCategory, true, -1, "-1", CategoryID.SelectedValue);
        //}
    }
}
