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
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.LayoutControls;

public partial class wucAgent : System.Web.UI.UserControl
{
    public delegate void GridRowCommandHandler(object sender, GridViewCommandEventArgs e);
    public event GridRowCommandHandler GridRowCommand;

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

    public string DataSourceSelectMethod
    {
        get { return odsTransactions.SelectMethod; }
        set { odsTransactions.SelectMethod = value; }
    }

    public string DataSourceSelectCountMethod
    {
        get { return odsTransactions.SelectCountMethod; }
        set { odsTransactions.SelectCountMethod = value; }
    }

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
        if (UserSessions.CurrentUser.IsAgent)
        {
            if (!m_Prms.ContainsKey("@MasterAgentUID"))
                m_Prms.Add("@MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);
            else
                m_Prms["@MasterAgentUID"] = UserSessions.CurrentUser.HookTableKeyUID;
        }

        if (!AgentFirstName.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@FirstName"))
                m_Prms.Add("@FirstName", AgentFirstName.Text);
            else
                m_Prms["@FirstName"] = AgentFirstName.Text;
        }

        if (!AgentLastName.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@LastName"))
                m_Prms.Add("@LastName", AgentLastName.Text);
            else
                m_Prms["@LastName"] = AgentLastName.Text;
        }

        if (!AgentDBA.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@DBA"))
                m_Prms.Add("@DBA", AgentDBA.Text);
            else
                m_Prms["@DBA"] = AgentDBA.Text;
        }

        if (!AgentID.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@ID"))
                m_Prms.Add("@ID", AgentID.Text);
            else
                m_Prms["@ID"] = AgentID.Text;
        }

        if (!string.IsNullOrWhiteSpace(FMAID.Text))
        {
            m_Prms["@AgentFMAID"] = FMAID.Text;
        }


        if (m_Prms != null && m_Prms.Count > 0)
        {
            if (!m_Prms.ContainsKey("@PageSize"))
                m_Prms.Add("@PageSize", this.PageSize);
            else
                m_Prms["@PageSize"] = this.PageSize;

            if (!m_Prms.ContainsKey("@CurrentPage"))
                m_Prms.Add("@CurrentPage", this.CurrentPage);
            else
                m_Prms["@CurrentPage"] = this.CurrentPage;

            if (!m_Prms.ContainsKey("@SortOrder"))
                m_Prms.Add("@SortOrder", "BusinessDBAName");
            else
                m_Prms["@SortOrder"] = this.SortOrder;

            m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

            grd.DataBind();

            int rowcount = DataMerchantAppPaging.GetAgentsPagingRowCount(m_Prms, 0, 0, this.ID);
            lblRecordCount.Text = "Total Records Found: " + rowcount.ToString();
            pnlRecords.Visible = rowcount > 0;
            pnlNoRecords.Visible = rowcount == 0;
        }
    }

    public void ClearGrid()
    {
        AgentFirstName.Text = string.Empty;
        AgentLastName.Text = string.Empty;
        AgentID.Text = string.Empty;
        AgentDBA.Text = string.Empty;
        lblRecordCount.Text = "Total Record(s) Found: 0";
       
        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.BindGrid();
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkZID = (LinkButton)e.Row.FindControl("lnkAgentID");
            lnkZID.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
            lnkZID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "AgentID").ToString() + ',' + e.Row.Cells[1].Text;
        }
    }

    protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (this.m_Prms != null && this.m_Prms.Count > 0)
            e.InputParameters[0] = this.m_Prms;
        else
            e.InputParameters[0] = (new Hashtable());

        e.InputParameters[3] = this.ID;
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;

        if (this.GridRowCommand != null)
        {
            this.GridRowCommand(grd, e);
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.BindGrid();
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

    protected void Page_Load(object sender, EventArgs e)
    {
        AgentID.Attributes.Add("onKeyPress", "CheckNumeric();");

        if (!this.IsPostBack)
        {
            this.CurrentPage = 1;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        grd.PageIndex = 0;
        Hashtable prms = new Hashtable();
        ViewState["m_Prms"] = prms;
        BindGrid();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        clearAll();
        if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
        {
            ((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
        }

    }

    public void clearAll()
    {
        AgentDBA.Text = string.Empty;
        AgentID.Text = string.Empty;
        AgentLastName.Text = string.Empty;
        AgentFirstName.Text = string.Empty;
        m_Prms = null;
    }
}
