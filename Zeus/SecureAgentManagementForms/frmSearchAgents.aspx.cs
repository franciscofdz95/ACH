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
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.NavigationControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class frmSearchAgents : frmBaseSearch
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        ((HyperLink)this.Master.FindControl("lnkSearchAgent")).CssClass = "active";
        WebUtil.SetUserSpecificDisplayMode(SearchBeginDate);
        WebUtil.SetUserSpecificDisplayMode(SearchEndDate);
        if (!this.IsPostBack)
        {
           
            //Set the current page
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Agent Profile";
            LookupTableHandler.LoadAgentStatus(StatusUID, true);

            LookupTableHandler.GetAgentCategories(AgentCategoryUID);
            LookupTableHandler.LoadAgentTypes(AgentTypeUID, true);
            LookupTableHandler.LoadUsersByRole(PrimaryContactUID, true, Constants.ROLE_AGENT_RELATIONS);
            
            AgentCategoryUID.Items.Insert(0, "All");
            AgentCategoryUID.SelectedIndex = 0;
            this.Search(true);

        }

        if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas))
            {
            tdAgentEdgeLabel.Visible = true;
            tdAgentEdgeText.Visible = true;
            }
        else
            {
            tdAgentEdgeLabel.Visible = false;
            tdAgentEdgeText.Visible = false;
            }
        this.PrimaryContactUID.Enabled = false;
    }

    public override void Search(bool IsOnLoad)
    {

        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        grd.DataBind();

        pnlRecords.Visible = (grd.Rows.Count > 0);
        pnlNoRecords.Visible = !(grd.Rows.Count > 0);

    }

    private void CloseForm()
    {
        string url = Request.QueryString["PostBackURL"].ToString();

        if (!url.Equals(string.Empty))
            Response.Redirect(url);
        else
        {
            Response.Redirect("frmAgent.aspx?Adding=false&AgentUID=" + UserSessions.CurrentAgent.AgentUID);
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Search":
                this.Search(false);
                break;
            case "Clear":
                this.ClearSearch();
                break;
        }
    }

    public void ClearSearch()
    {
        FormHandler.ClearAllControls(this); 
        this.SearchParameters = null;

        grd.DataSource = null;
        grd.DataBind();

        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;        
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

    protected void odsAgents_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app;

        if (AgentFirstName.Text != string.Empty)
            prms.Add("@FirstName", AgentFirstName.Text);

        if (AgentLastName.Text != string.Empty)
            prms.Add("@LastName", AgentLastName.Text);

        if (AgentDBA.Text != string.Empty)
            //Amit Patne : PXP-4071
            prms.Add("@DBA", System.Text.RegularExpressions.Regex.Replace(AgentDBA.Text,@"\s+", " ").Trim());

        if (AgentContactFirstName.Text != string.Empty)
            prms.Add("@ContactFirstName", AgentContactFirstName.Text);

        if (CommonUtility.Util.GetNumbersFromString(AgentPhone.Text) != "")
        {
            prms.Add("@Phone", AgentPhone.Text.Trim());
        }

        if (UserSessions.CurrentUser.IsAgent)
            prms.Add("@MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);
        
        if (AgentUID.Text != string.Empty)
            prms.Add("@ID", AgentUID.Text);

        if (AgentTaxID.Text != string.Empty)
            prms.Add("@TaxID", AgentTaxID.Text);

        if (StatusUID.SelectedIndex > 0)
            prms.Add("@StatusUID", StatusUID.SelectedItem.Value);

        if (AgentCategoryUID.SelectedIndex > 0)
            prms.Add("@AgentCategoryUID", AgentCategoryUID.SelectedItem.Value);

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@StartDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndDate", SearchEndDate.Value);

        if (AgentTypeUID.SelectedIndex > 0)
            prms.Add("@AgentTypeUID", AgentTypeUID.SelectedItem.Value);

        if (PrimaryContactUID.SelectedIndex > 0)
            prms.Add("@PrimaryContactUID", PrimaryContactUID.SelectedItem.Value);

        if (!string.IsNullOrWhiteSpace(FMAID.Text))
            prms.Add("@AgentFMAID", FMAID.Text.Trim());

        //if (FMAName.Text != string.Empty)
        //    prms.Add("@AgentFMAName", FMAName.Text.Trim());

        if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas))
            {
            if (AgentEdgeID.Text != string.Empty)
                prms.Add("@AgentEdgeID", AgentEdgeID.Text);
            }
        

        if (prms.Count > 0)
        {
            if (UserSessions.CurrentUser.IsInternal)
                prms.Add("@InternalUserUID", UserSessions.CurrentUser.UID);

            prms.Add("@isSearch", true);

            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            prms.Add("@PageSize", this.PageSize);

            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);
        }
        else
        {
            prms.Add("@ID", -1);
        }

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetAgentsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
    }
    
    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }
    
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Cells[6].Text = WebUtil.ConvertToUserShortDateTimeFormat(e.Row.Cells[6].Text);

                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //string url = string.Empty;

        //switch (e.CommandName)
        //{
        //    case "AgentID":
        //        Agent agent = DataAccess.DataAgentDao.GetAgent_List(e.CommandArgument.ToString());
        //        UserSessions.CurrentAgent = agent;

        //        url = "frmAgent.aspx?Adding=false";
        //        url += "&PostBackURL=~/SecureAgentManagementForms/frmSearchAgents.aspx";

        //        Response.Redirect(url);
        //        break;
        //}
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        this.Search(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grd.PageIndex + 1;
        }

        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("AgentList.xls", grd);
    }

}
