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

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PaymentXP.Facade;

public partial class frmTicketSearch : frmBaseSearch
{
    

    private int MAX_CHARACTERS_TO_DISPLAY = 40; 

    protected void Page_PreRender(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>force_min();</script>", false);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkSearchTickets")).CssClass = "active";

        if (!IsPostBack)
        {
            BusinessDBAName.Focus();
            
            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists
            LookupTableHandler.LoadActiveInternalUsers(AssignToUID, true);
            LookupTableHandler.LoadInternalUsers(UserCreated, true);
            LookupTableHandler.MerchantAppStatus(StatusUIDList, true, "Ticket");
            StatusUIDList.Items.RemoveAt(0); // remove "All"
            LookupTableHandler.LoadTicketCategories(CategoryID, true, -1, "i", "0");
            LookupTableHandler.LoadDepartments(DepartmentID, true);
            LookupTableHandler.LoadRolesWithDefault(OriginDept);
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            LookupTableHandler.LoadUserTimeZones(TimeZone, true);

            SortOrder = "TicketID";
            SortDirectionSearch = SortDirection.Descending;

            this.calSearchBeginDate.Format = UserSessions.CurrentUser.DatePattern;
            this.calSearchEndDate.Format = UserSessions.CurrentUser.DatePattern;
            this.calCallbackDate.Format = UserSessions.CurrentUser.DatePattern;
            this.calDateCreated.Format = UserSessions.CurrentUser.DatePattern;

            if (!string.IsNullOrWhiteSpace(CommonUtility.Util.if_s(Request.QueryString["ZID"])))
            {
                this.FormClear();
                this.MerchantID.Text = CommonUtility.Util.if_s(Request.QueryString["ZID"]);
            }

            if (!string.IsNullOrWhiteSpace(CommonUtility.Util.if_s(Request.QueryString["AgentUID"])))
            {
                this.wucAgentSelector.m_AgentUID = CommonUtility.Util.if_s(Request.QueryString["AgentUID"]);
            }

            this.Search(true);
        }


    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        //validate search params
        string valError = ValidateSearchParams(); 

        if (!string.IsNullOrWhiteSpace(valError))
        {
            FormHandler.DisplayMessage(Page.ClientScript, valError);
            return;
        }

        SearchParameters = null;
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

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@DBAName", BusinessDBAName.Text);

        if (wucAgentSelector.m_AgentUID != string.Empty)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
        else if (UserSessions.CurrentUser.IsAgent)
        {
            if (!prms.ContainsKey("@MasterAgentUID"))
                prms.Add("@MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);
            else
                prms["@MasterAgentUID"] = UserSessions.CurrentUser.HookTableKeyUID;
        }

        //if (StatusID.SelectedIndex > 0)
        //    prms.Add("@StatusUID", StatusID.SelectedItem.Value);

        List<string> liStatusList = new List<string>();
        foreach (ListItem li in StatusUIDList.Items)
        {
            if (li.Selected)
            {
                liStatusList.Add(li.Value);
            }
        }

        if (liStatusList.Count > 0)
        {
            prms.Add("@StatusUIDList", CommonUtility.Util.implode(liStatusList, ","));
        }


        if (DepartmentID.SelectedIndex > 0)
            prms.Add("@DepartmentID", DepartmentID.SelectedItem.Value);

        if (SubCategory.SelectedIndex > 0)
            prms.Add("@CategoryID", SubCategory.SelectedItem.Value);

        if (CategoryID.SelectedIndex > 0)
            prms.Add("@ParentID", CategoryID.SelectedItem.Value);

        if (AssignToUID.SelectedIndex > 0)
            prms.Add("@UserUID", AssignToUID.SelectedItem.Value);

        if (UserCreated.SelectedIndex > 0)
            prms.Add("@UserCreated", UserCreated.SelectedItem.Value);

        if (BusinessContact.Text != string.Empty)
            prms.Add("@ContactName", BusinessContact.Text);

        if (TicketID.Text != string.Empty)
            prms.Add("@TicketID", TicketID.Text);

        if (TimeZone.SelectedIndex > 0)
            prms.Add("@TimeZone", TimeZone.SelectedItem.Value);

        if (SettlePlatformMid.Text != string.Empty)
            prms.Add("@MID", SettlePlatformMid.Text);

        if (OriginDept.SelectedIndex > 0)
            prms.Add("@UserCreatedDefaultRoleUID", OriginDept.SelectedItem.Value);

        switch (Source.SelectedValue)
        {
            case "a":
                prms.Add("@TicketSource", "a");
                break;

            case "m":
                prms.Add("@TicketSource", "m");
                break;

            case "i":
                prms.Add("@TicketSource", "i");
                break;

            case "x":
                prms.Add("@TicketSource", "x");
                break;
            case "e":
                prms.Add("@TicketSource", "e");
                break;
            default:
                // do nothing
                break;
        }

        if (CommonUtility.Util.if_i(Origin.SelectedValue, 0) > 0)
        {
            prms.Add("@Origin", Convert.ToInt32(Origin.SelectedValue));
        }

        if (Priority.SelectedIndex > 0)
            prms.Add("@Priority", Priority.SelectedItem.Value);

        DateTime date;

        if (!string.IsNullOrEmpty(CallbackDate.Text)
            && DateTime.TryParseExact(this.CallbackDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            prms.Add("@CallbackDate", date);
        }

        if (!string.IsNullOrEmpty(DateCreated.Text)
            && DateTime.TryParseExact(this.DateCreated.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            prms.Add("@DateCreated", date);
        }

        if (!string.IsNullOrEmpty(SearchBeginDate.Text)
            && DateTime.TryParseExact(this.SearchBeginDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            prms.Add("@BeginPostedDate", date);
        }

        if (!string.IsNullOrEmpty(SearchEndDate.Text)
            && DateTime.TryParseExact(this.SearchEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            prms.Add("@EndPostedDate", date);
        }

        if (!string.IsNullOrEmpty(Tags.Text))
            prms.Add("@Tags", Tags.Text);

        if (!string.IsNullOrEmpty(MerchantID.Text))
            prms.Add("@ZID", MerchantID.Text);

        if (!string.IsNullOrEmpty(FMAID.Text))
            prms.Add("@FMAID", FMAID.Text);

        if (!string.IsNullOrWhiteSpace(OfficeID.SelectedValue))
        {
            prms["@OfficeID"] = OfficeID.SelectedValue;
        }
        if (!string.IsNullOrEmpty(ScavengerEmailFrom.Text.Trim()))
            prms.Add("@ScavengerEmailFrom", ScavengerEmailFrom.Text.Trim());

        if (!string.IsNullOrWhiteSpace(IssuesNotes.Text))
        {
            prms["@IssuesNotes"] = IssuesNotes.Text;
        }

        if (!string.IsNullOrWhiteSpace(MLEName.Text.Trim()))
        {
            prms["@MLEName"] = MLEName.Text.Trim();
        }


        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (prms.Count > 0)
        {
            if (UserSessions.CurrentUser.IsInternal)
                prms.Add("@InternalUserUID", UserSessions.CurrentUser.UID);

            //Save search fields in session variable
            ticket = new SearchParameter();
            FormBinding.BindControlsToObject(ticket, pnlSearch);

            // the bindcontrolstoobject function does not support items to csv list. so we'll do it here manually for now.

            List<string> liSUID = new List<string>();
            foreach (ListItem li in StatusUIDList.Items)
            {
                if (li.Selected)
                {
                    liSUID.Add(li.Value.ToLower());
                }
            }
            ticket.StatusUIDList = CommonUtility.Util.implode(liSUID, ",");

            this.SearchParameters = ticket;

            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);
            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                this.SortOrder = "TicketID";
            prms.Add("@SortOrder", this.SortOrder);

            prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        }
        else
        {
            prms.Add("@TicketID", 0);
        }

     

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetTicketsPagingCount(prms, 0, 0, this.grd.ID).ToString();
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:


                string solution = (DataBinder.Eval(e.Row.DataItem, "Solution").ToString());
                solution = WebUtil.ConvertHtml(Server.HtmlDecode(solution));
                e.Row.Cells[4].Attributes.Add("title", solution);

                ((Literal)e.Row.FindControl("litSolution")).Text = CommonUtility.Formatting.nl2br(DataBinder.Eval(e.Row.DataItem, "Problem").ToString());
                
                HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));
                
                e.Row.Cells[5].Attributes.Add("class", "text");
                e.Row.Cells[12].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[12].Text);
                e.Row.Cells[13].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[13].Text);
                e.Row.Cells[14].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[14].Text);

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

                ((Label)e.Row.FindControl("LabelOffice")).Text = ((CommonUtility.Util.Offices)CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "OfficeID").ToString(), -1)).ToString();

                // if ticket status is not closed. do this for the rest of them.
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")).ToUpper() != "CLOSED")
                {
                    string pri = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Priority"));

                    switch (pri)
                    {
                        case "High":
                            e.Row.Cells[0].CssClass += "zeustooltip tickethigh";
                            e.Row.Cells[0].Attributes.Add("title", "High Priority");
                            break;

                        case "Medium":
                            e.Row.Cells[0].CssClass += "zeustooltip ticketmedium";
                            e.Row.Cells[0].Attributes.Add("title", "Medium Priority");
                            break;

                        default:
                            e.Row.Cells[0].CssClass += "zeustooltip ticketlow";
                            e.Row.Cells[0].Attributes.Add("title", "Low Priority");
                            break;
                    }
                }

                HyperLink btn = (HyperLink)e.Row.FindControl("hypTID");
                btn.Attributes.Add("title", this.BuildTooltip(((DataRowView)e.Row.DataItem).Row));

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
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    private void ClearGrid()
    {
        grd.DataSourceID = string.Empty;
        grd.DataBind();
    }

    public override void Search(bool IsOnLoad)
    {
        //Populate search fields        
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter Ticket = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(Ticket, pnlSearch);
            
            // the bindobjecttocontrols function does not handle csv string to checkboxlist, so write it ourselves.
            if (!string.IsNullOrWhiteSpace(Ticket.StatusUIDList))
            {
                foreach (ListItem li in StatusUIDList.Items)
                {
                    li.Selected = (Ticket.StatusUIDList.ToLower().Contains(li.Value.ToLower())) ? true : false;
                }
                
            }

        }

        grd.DataBind();

        if (IsOnLoad)
            grd.Sort(this.SortOrder, this.SortDirectionSearch);

        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;

      
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
     //   grd.Columns.Clear();
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        grd.PageIndex = 0;
        this.Search(false);

        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
   
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
        TogglegridFields(true);
        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("TicketList.xls", grd);
        TogglegridFields(false);
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
        pnlCat.Visible = false;

        if (CategoryID.SelectedIndex > 0)
        {
            pnlCat.Visible = true;
            LookupTableHandler.LoadTicketCategories(SubCategory, true, -1, "i", CategoryID.SelectedValue);
            //LookupTableHandler.LoadSubCategories(SubCategory, true, -1, "-1", CategoryID.SelectedValue);
        }
    }

    private string ValidateSearchParams()
    {
        StringBuilder sb = new StringBuilder();
        DateTime date;

        //validate search begin date
        if (!string.IsNullOrWhiteSpace(this.SearchBeginDate.Text)
            && !DateTime.TryParseExact(this.SearchBeginDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid Begin Date.\\n");
        }

        if (!string.IsNullOrWhiteSpace(this.SearchEndDate.Text)
            && !DateTime.TryParseExact(this.SearchEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid End Date.\\n");
        }

        if (!string.IsNullOrWhiteSpace(this.CallbackDate.Text)
            && !DateTime.TryParseExact(this.CallbackDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid Callback Date.\\n");
        }

        if (!string.IsNullOrWhiteSpace(this.DateCreated.Text)
            && !DateTime.TryParseExact(this.DateCreated.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid Date Created.");
        }

        return sb.ToString();
    }

    private string BuildTooltip(DataRow dr)
    {
        StringBuilder sb = new StringBuilder();
        rdExport.SelectedValue = "0";
        sb.Append("<table class='mGrid'>");

        if (dr.Table.Columns.Contains("DueDate"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Due Date", WebUtil.ConvertToUserDateTimeSettings(dr["DueDate"].ToString()));
        }

        if (dr.Table.Columns.Contains("Days"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Days Aged", dr["Days"].ToString());
        }

        if (dr.Table.Columns.Contains("Tags"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Tags", dr["Tags"].ToString());
        }

        if (dr.Table.Columns.Contains("MLEName") && dr["IsMLETicket"].ToString() == "True")
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "MLE", dr["MLEName"].ToString());
        }

        sb.Append("</table>");

        return sb.ToString();
    }

   
    
}
