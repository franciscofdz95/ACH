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
using PaymentXP.Facade;
using CommonUtility;


public partial class frmOnlineApps : frmBaseSearch
{



    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdAgent.GridRowCommand += new wucAgent.GridRowCommandHandler(grdAgents_GridRowCommand);

    }

    public override void Search(bool IsOnLoad)
    {

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
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        SearchBeginDate.Value = DateTime.Today;
        SearchEndDate.Value = DateTime.Today;
        rdExport.SelectedIndex = 0;
        lblError.Text = "";
        AgentUID.Value = string.Empty;
        this.Search(false);
    }

    private void ClearGrid()
    {
        grd.DataSourceID = string.Empty;
        grd.DataBind();
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
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        WebUtil.SetUserSpecificDisplayMode(SearchBeginDate);
        WebUtil.SetUserSpecificDisplayMode(SearchEndDate);

        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.IsBank)
            lbSelectAgent.Visible = false;

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageHome.eMasterSideMenu.OnlineApps);

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists

            //LookupTableHandler.LoadOnlineAgentsID(AgentAgentID, true, 0);
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());

            SearchBeginDate.Value = DateTime.Today;
            SearchEndDate.Value = DateTime.Today;

            this.Search(true);
        }
    }

    private void BindApplication(string AppUID)
    {
        /* summary */
        List<Apps> Apps = new List<Apps>();
        Apps.Add(DataApp.Instance.GetApp(AppUID));
        dvApplication.DataSource = Apps;
        dvApplication.DataBind();

        /* risk check */
        List<AppsDetails> details = new List<AppsDetails>();
        AppsDetails appdet = DataApp.Instance.GetAppsDetails(AppUID);
        if (appdet != null)
        {
            details.Add(appdet);
        }

        dvDetails.DataSource = details;
        dvDetails.DataBind();

        /* owners */
        List<AppsOwnerships> Ownerships = (List<AppsOwnerships>)DataApp.Instance.GetAppsOwnerships(AppUID);
        List<AppsOwnerships> FirstOwner = null;
        List<AppsOwnerships> SecondOwner = null;

        if (Ownerships != null)
        {
            FirstOwner = new List<AppsOwnerships>();
            FirstOwner.Add(Ownerships[0]);

            if (Ownerships.Count > 1)
            {
                fsSecondOwner.Visible = true;
                SecondOwner = new List<AppsOwnerships>();
                SecondOwner.Add(Ownerships[1]);
            }
            else
                fsSecondOwner.Visible = false;

            dvOwners1.DataSource = FirstOwner;
            dvOwners1.DataBind();

            dvOwners2.DataSource = SecondOwner;
            dvOwners2.DataBind();
        }

        DataDocuments data = DataAccess.DataDocumentsDao;
        Hashtable prms = new Hashtable();
        int ID = Apps[0].ZID;

        if (ID <= 0)
            ID = Apps[0].RetrievalID.Value;

        prms.Add("@PrimaryKeyID", ID);
        prms.Add("@MerchantID", ID);

        List<MDoc> li = data.GetMDocuments(prms);

        this.grdDocuments.DataSource = li;
        this.grdDocuments.DataBind();

    }

    private string GetLastStepText(int? LastStep)
    {
        switch (LastStep)
        {
            case 0:
                return "Business";

            case 1:
                return "Owners";

            case 3:
                return "Fees";

            case 2:
                return "Processing";

            //case 4:
            //    return "Merchant Bank";

            case 4:
                return "Documents";

            //case 6:
            //    return "Summary";

            //case 7:
            //    return "Program Guide";

            //case 7:
            //    return "Personal Guarantee";

            //case 7:
            //    return "Application";

            case 5:
                return "Completed";

            default:
                return null;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Literal ltlLastStep;

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                LinkButton btn2 = (LinkButton)e.Row.FindControl("lbtnRetrievalID");
                btn2.Text = DataBinder.Eval(e.Row.DataItem, "RetrievalID").ToString();
                btn2.CommandArgument = DataBinder.Eval(e.Row.DataItem, "UID").ToString();

                //if (DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString() == string.Empty)
                //    btn2.Enabled = false;

                //LinkButton btn = (LinkButton)e.Row.FindControl("lbtnZID");
                //btn.Text = DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString();

                //string str = "";
                //if (btn.Text == "0")
                //    str = "text-decoration:none";
                //else
                //    str = "text-decoration:underline";

                //btn.Attributes.Add("style", str);
                //btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();


                ltlLastStep = (Literal)e.Row.FindControl("ltlLastStep");
                ltlLastStep.Text = GetLastStepText(Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "LastStep")));
                e.Row.Cells[10].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[10].Text);

                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        string status = string.Empty;
        string leaduid = string.Empty;
        string assignto = string.Empty;
        GridViewRow grdRow = null;


        switch (e.CommandName)
        {
            case "RetrievalID":
                BindApplication(e.CommandArgument.ToString());
                WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;
            case "ID":

                LinkButton btn = (LinkButton)e.CommandSource;
                if (btn.Text == "0")
                    return;

                MerchantFacade facade = new MerchantFacade();
                MerchantApp app = facade.GetMerchantAppZeus(e.CommandArgument.ToString());
                UserSessions.CurrentMerchantApp = app;
                grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false";
                url += "&PostBackURL=~/SecureHomeForms/frmOnlineApps.aspx";
                url += "&MerchantAppUID=" + grd.DataKeys[grdRow.RowIndex].Values["MerchantAppUID"].ToString();
                Response.Redirect(url);
                break;
        }
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

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    protected void odsApps_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app;

        if (AgentUID.Value != string.Empty)
            prms.Add("@AgentUID", AgentUID.Value);

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
        /*PXP-7428 Bug fixes by Sanidhya: Trim Changes Start */
        if (RetrievalID.Text != string.Empty)
            prms.Add("@RetrievalID", RetrievalID.Text.Trim());
        /*PXP-7428 Bug fixes by Sanidhya: Trim Changes End */
        if (TabStop.SelectedIndex > 0)
            prms.Add("@LastStep", TabStop.SelectedItem.Value);

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);
        /*PXP-7428 Bug fixes by Sanidhya: Trim Changes Start */
        if (MerchantID.Text != string.Empty)
            prms.Add("@MerchantID", MerchantID.Text.Trim());
        /*PXP-7428 Bug fixes by Sanidhya: Trim Changes End */
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
        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        if (this.SortOrder == string.Empty)
            prms["@SortOrder"] = "ZID";
        else
            prms["@SortOrder"] = this.SortOrder;

        prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsOnlinePagingRowCount(prms, 0, 0, this.grd.ID).ToString();
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
        FormHandler.Export2Excel("OnlineMerchants.xls", grd);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }
    /// Changes by Sanidhya Kumar for PXP-5993
    /// <Changedetail>
    /// Implemented the check for zid and retid
    /// </Changedetail>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        grd.PageIndex = 0;
        this.CurrentPage = 1;
        this.SortOrder = string.Empty;
        //this.IsKeywordSearch = false;
        this.SearchParameters = null;
        lblError.Text = String.Empty;
        /*PXP-7428 Bug fixes by Sanidhya: Trim Changes Start */
        String zID = MerchantID.Text.Trim();
        String retID = RetrievalID.Text.Trim();
        /*PXP-7428 Bug fixes by Sanidhya: Trim Changes End */
        if (Convert.ToDateTime(SearchEndDate.Value) < Convert.ToDateTime(SearchBeginDate.Value))
        {
            lblError.Text = "Start Date should be less than End Date.";
        }
        /*PXP-7428 Bug fixes by Sanidhya:Start */
        else if (!String.IsNullOrEmpty(zID) && !CommonUtility.Validation.IsNumeric(zID, 10))
        {
            this.ClearGrid();
            pnlRecords.Visible = false;
            pnlNoRecords.Visible = true;
            grd.DataSourceID = "odsApps";
        }
        else if (!String.IsNullOrEmpty(retID) && !CommonUtility.Validation.IsNumeric(retID, 10))
        {
            this.ClearGrid();
            pnlRecords.Visible = false;
            pnlNoRecords.Visible = true;
            grd.DataSourceID = "odsApps";
        }
        /*PXP-7428 Bug Fixes by Sanidhya:End */
        else
        {
            this.Search(false);
        }

    }



    protected void btnAgentSelect_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        grdAgent.SetDataSource(prms, 10);
        dlgAgent.Modal = false;
        dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    private void grdAgents_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;
        if (e.CommandSource is LinkButton)
        {
            lnk = (LinkButton)e.CommandSource;
        }
        else
        {
            return;
        }

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
        string uid = str[0];

        DataAgent da = new DataAgent();
        Agent app = da.GetAgent(uid);

        AgentUID.Value = app.AgentUID;
        AgentDBA.Text = app.AgentDBA;
        AgentID.Text = app.AgentID.ToString();


        grdAgent.ClearGrid();
        this.dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void grdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                var row = ((DataRowView)e.Row.DataItem).Row;

                MDoc objM = DataMerchantAppPaging.getMDoc(row);

                HyperLink lnk = (HyperLink)e.Row.FindControl("hypOrigName");

                Dictionary<string, string> di = new Dictionary<string, string>();

                di["DocID"] = DataBinder.Eval(e.Row.DataItem, "DocID").ToString();
                di["MerchantAppUID"] = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();
                di["MerchantID"] = DataBinder.Eval(e.Row.DataItem, "MerchantID").ToString();
                di["AgentID"] = DataBinder.Eval(e.Row.DataItem, "AgentID").ToString();

                lnk.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di)));

                // strips off the directory and just puts the filename.
                string[] arr = lnk.Text.Split(new char[] { '\\' });
                lnk.Text = arr[arr.Length - 1];

                break;

            default:
                break;
        }
    }

}
