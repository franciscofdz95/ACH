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
using PaymentXP.Facade;
using PaymentXP.BusinessObjects;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.LayoutControls;


public partial class wucMerchants : System.Web.UI.UserControl
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
        if (!txtDBA.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@BusinessDBAName"))
                m_Prms.Add("@BusinessDBAName", txtDBA.Text);
            else
                m_Prms["@BusinessDBAName"] = txtDBA.Text;
        }

        if (!BusinessLegalName.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@BusinessLegalName"))
                m_Prms.Add("@BusinessLegalName", BusinessLegalName.Text);
            else
                m_Prms["@BusinessLegalName"] = BusinessLegalName.Text;
        }

        if (!string.IsNullOrEmpty(AgentID.Text))
        {
            if (!m_Prms.ContainsKey("@AgentID"))
                m_Prms.Add("@AgentID", AgentID.Text);
            else
                m_Prms["@AgentID"] = AgentID.Text;

        }

        if (!string.IsNullOrEmpty(AgentDBA.Text))
        {
         
            if (!m_Prms.ContainsKey("@AgentDBA"))
                m_Prms.Add("@AgentDBA", AgentDBA.Text);
            else
                m_Prms["@AgentDBA"] = AgentDBA.Text;
        }

        if (UserSessions.CurrentUser.IsAgent)
        {
            if (!m_Prms.ContainsKey("@AgentUIDSub"))
                m_Prms.Add("@AgentUIDSub", UserSessions.CurrentUser.HookTableKeyUID);
            else
                m_Prms["@AgentUIDSub"] = UserSessions.CurrentUser.HookTableKeyUID;
        }

        if (MerchantAppTypeUID.SelectedIndex > 0)
        {
            if (!m_Prms.ContainsKey("@MerchantAppTypeUID"))
                m_Prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);
            else
                m_Prms["@MerchantAppTypeUID"] = MerchantAppTypeUID.SelectedItem.Value;
        }

        if (!MerchantID.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@ID"))
                m_Prms.Add("@ID", MerchantID.Text);
            else
                m_Prms["@ID"] = MerchantID.Text;
        }

        if (!AchID.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@AchID"))
                m_Prms.Add("@AchID", AchID.Text);
            else
                m_Prms["@AchID"] = AchID.Text;
        }

        if (!string.IsNullOrWhiteSpace(FMAID.Text))
        {
            m_Prms["@FMAID"] = FMAID.Text;
        }

        if(!string.IsNullOrWhiteSpace(MLEName.Text))
        {
            m_Prms["@BusinessLegalName"] = MLEName.Text.Trim();
        }

        if (!SettlePlatformMid.Text.Equals(string.Empty))
        {
            if (!m_Prms.ContainsKey("@SettlePlatformMid"))
                m_Prms.Add("@SettlePlatformMid", SettlePlatformMid.Text);
            else
                m_Prms["@SettlePlatformMid"] = SettlePlatformMid.Text;
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

            //lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(m_Prms, 0, 0,this.ID).ToString();
            int rowcount = DataMerchantAppPaging.GetMerchantAppsPagingRowCount(m_Prms, 0, 0, this.ID);

            lblRecordCount.Text = "Total Records Found: " + rowcount.ToString();

            pnlRecords.Visible = (rowcount > 0);
            pnlNoRecords.Visible = !(rowcount > 0);
        }
    }

    public void ClearGrid()
    {
        txtDBA.Text = string.Empty;
        BusinessLegalName.Text = string.Empty;
        SettlePlatformMid.Text = string.Empty;
        AchID.Text = string.Empty;
        MerchantID.Text = string.Empty;
        lblRecordCount.Text = "Total Record(s) Found: 0";

        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
        MerchantAppTypeUID.SelectedIndex = -1;
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
            LinkButton lnkZID = (LinkButton)e.Row.FindControl("lnkZID");
            lnkZID.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
            lnkZID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString() + ',' + e.Row.Cells[1].Text;
        }
    }

    protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (this.m_Prms != null)
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
        AchID.Attributes.Add("onKeyPress", "CheckNumeric();");
        AgentID.Attributes.Add("onKeyPress", "CheckNumeric();");
        MerchantID.Attributes.Add("onKeyPress", "CheckNumeric();");
        btnSearch.Attributes.Add("onclick", "return validate('" + btnSearch.ClientID + "','" + AchID.ClientID + "','" + MerchantID.ClientID + "');");

        if (!this.IsPostBack)
        {
            this.CurrentPage = 1;
            //LookupTableHandler.LoadAgentsNew(AgentUID, true);
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);
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
        if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
        {
            ((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
        }

    }
}
