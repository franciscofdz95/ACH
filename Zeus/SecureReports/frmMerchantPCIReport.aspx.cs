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
using System.Collections.Generic;
using System.Data.SqlClient;
using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;


public partial class frmMerchantPCIReport : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (!this.IsPostBack)
        {
            //Update Merchant PCI Status
            DataAccess.DataMerchantAppDao.UpdateMerchantPCIStatus();

            //Set the current page
            //UserSessions.CurrentPage = "> Merchant PCI Search";

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "PCI");
            //LookupTableHandler.LoadAgentsNew(AgentUID, false);
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            LookupTableHandler.LoadPCIVendors(VendorID, false);

            SearchBeginDate.Value = DateTime.Today;
            SearchEndDate.Value = DateTime.Today;
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        string CallResultsList = string.Empty;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        grd.DataBind();

        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);

        grd.Columns.Clear();

        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        SearchBeginDate.Value = DateTime.Today;
        SearchEndDate.Value = DateTime.Today;

        this.Search(false);

        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
        rdExport.SelectedIndex = 0;
        lblError.Text = "";
        wucAgentSelector.FormClear();
    }

    protected void btnKeywordSearch_Click(object sender, ImageClickEventArgs e)
    {
        this.SortOrder = string.Empty;
        this.CurrentPage = 1;
        grd.PageIndex = 0;
        //this.IsKeywordSearch = true;
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        grd.PageIndex = 0;
        this.CurrentPage = 1;
        this.SortOrder = string.Empty;
        //this.IsKeywordSearch = false;
        this.SearchParameters = null;
        lblError.Text = "";
        if (Convert.ToDateTime(SearchEndDate.Value) < Convert.ToDateTime(SearchBeginDate.Value))
            lblError.Text = "Start Date should be less than End Date.";
        else
            this.Search(false);
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                //LinkButton btn = (LinkButton)e.Row.FindControl("lbtnZID");
                //btn.Text = DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString();
                //btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();
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
        //string status = string.Empty;
        //string leaduid = string.Empty;
        //string assignto = string.Empty;
        //GridViewRow grdRow = null;

        //if (!(e.CommandSource is LinkButton))
        //    return;

        //LinkButton btn = (LinkButton)e.CommandSource;
        //if (btn.Text == "0")
        //    return;

        //switch (e.CommandName)
        //{
        //    case "MerchantID":

        //        MerchantApp app = DataAccess.DataMerchantAppDao.GetMerchantApp(e.CommandArgument.ToString());
        //        UserSessions.CurrentMerchantApp = app;
        //        grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

        //        url = "~/SecureMerchantManagementForms/frmMerchantPCI.aspx?Adding=false";
        //        url += "&MerchantAppUID=" + grd.DataKeys[grdRow.RowIndex].Values["MerchantAppUID"].ToString();
        //        Response.Redirect(url);
        //        break;
        //}
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
        SearchParameter app;

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@BusinessDBAName", BusinessDBAName.Text);

        if (BusinessLegalName.Text != string.Empty)
            prms.Add("@BusinessLegalName", BusinessLegalName.Text);

        if (BusinessPhone.Text != string.Empty)
            prms.Add("@BusinessPhone", BusinessPhone.Text);

        if (BusinessContact.Text != string.Empty)
            prms.Add("@BusinessContact", BusinessContact.Text);

        if (BusinessEmailAddress.Text != string.Empty)
            prms.Add("@BusinessEmailAddress", BusinessEmailAddress.Text);

        if (DateType.SelectedIndex == 0)
        {
            if (!string.IsNullOrEmpty(SearchBeginDate.Text))
                prms.Add("@BeginPostedDate", SearchBeginDate.Value);

            if (!string.IsNullOrEmpty(SearchEndDate.Text))
                prms.Add("@EndPostedDate", SearchEndDate.Value);
        }
        else
        {
            if (!string.IsNullOrEmpty(SearchBeginDate.Text))
                prms.Add("@BeginCompletedDate", SearchBeginDate.Value);

            if (!string.IsNullOrEmpty(SearchEndDate.Text))
                prms.Add("@EndCompletedDate", SearchEndDate.Value);


        }

        if (MerchantID.Text != string.Empty)
            prms.Add("@MerchantID", MerchantID.Text);

        if (StatusUID.SelectedIndex != 0)
            prms.Add("@StatusUID", StatusUID.SelectedItem.Value);

        if (wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex != 0)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

        if (VendorID.SelectedIndex != 0)
            prms.Add("@VendorID", VendorID.SelectedItem.Value);

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;
        }
        else
        {
            prms.Add("@UID", "00000000-0000-0000-0000-000000000000");
        }

        //user is passed as a parameter to determine whether the user is an agent or manager
        User user = UserSessions.CurrentUser;
        prms.Add("@UserName", user.UserName);

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        if (this.SortOrder == string.Empty)
            prms["@SortOrder"] = "RetrievalID";
        else
            prms["@SortOrder"] = this.SortOrder;

        prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);


        e.InputParameters[0] = prms;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantPCIPagingRowCount(prms, 0, 0).ToString();
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureLeadForms/frmLeadsDetail.aspx?Adding=true";
        url += "&PostBackURL=~/SecureLeadForms/frmLeads.aspx";
        Response.Redirect(url);
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
        FormHandler.Export2Excel("MerchantPIC.xls", grd);
    }
}
