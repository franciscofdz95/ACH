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
using PaymentXP.DataObjects;


public partial class wucLeads : wucBaseSearch
{
    public delegate void GridRowCommandHandler(object sender, GridViewCommandEventArgs e);
    public event GridRowCommandHandler GridRowCommand;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        AgentID.Attributes.Add("onKeyPress", "CheckNumeric();");
        MerchantID.Attributes.Add("onKeyPress", "CheckNumeric();");

        if (!this.IsPostBack)
        {
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());            
            LookupTableHandler.LoadLeadReps(AssignedUserID, true);
            LookupTableHandler.LoadLeadSources(SourceID, true);

            this.Search(true);
        }
    }

    public void SetDataSource(Hashtable prms, int pagesize)
    {
        grd.DataSourceID = "odsLeads";
        this.CurrentPage = 1;
        this.PageSize = pagesize;
        grd.PageSize = pagesize;
        this.m_Prms = prms;
        Search(false);
    }

    public void Search(bool IsOnLoad)
    {
        string CallResultsList = string.Empty;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter lead = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(lead, pnlSearch);
        }

        grd.DataBind();
        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;

    }

    public void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);

        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());

        this.Search(false);

        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;       
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grd.EditIndex = -1;
        this.CurrentPage = 1;
        this.PageSize = CommonUtility.Util.if_i(cboPageSize.SelectedValue, 0);
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.Search(false);
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

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
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

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    private void ClearGrid()
    {
        grd.DataSourceID = string.Empty;
        grd.DataBind();
    }

    protected void odsLeads_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter lead;

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@DBAName", BusinessDBAName.Text);
        
        if (AgentID.Text != string.Empty)
            prms.Add("@AgentID", AgentID.Text);

        if (AgentDBA.Text != string.Empty)
            prms.Add("@AgentDBA", AgentDBA.Text);
        
        if (SourceID.SelectedIndex > 0)
            prms.Add("@LeadsSourcesUID", SourceID.SelectedItem.Value);
        
        if (MerchantID.Text != string.Empty)
            prms.Add("@ZID", MerchantID.Text);

        if (LeadID.Text != string.Empty)
            prms.Add("@ID", LeadID.Text);

        if (AssignedUserID.SelectedIndex > 0)
            prms.Add("@AssignedUserID", AssignedUserID.SelectedValue);

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (prms.Count > 0)
        {           
            //Save search fields in session variable
            lead = new SearchParameter();
            FormBinding.BindControlsToObject(lead, pnlSearch);
            this.SearchParameters = lead;

            //user is passed as a parameter to determine whether the user is an agent or manager
            User user = UserSessions.CurrentUser;
            if (user != null)
                prms.Add("@UserName", user.UserName);

            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetLeadsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
        }
        else
        {
            // we do this so that we don't call the SP with zero paramters. it's very slow otherwise.
            e.Cancel = true;
        }

    }

}
