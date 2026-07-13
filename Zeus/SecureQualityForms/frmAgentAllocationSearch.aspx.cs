using PaymentXP.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAgentAllocationSearch : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "";      
       

        ((HyperLink)this.Master.FindControl("lnkAgentAllocation")).CssClass = "active";

        if (!IsPostBack)
        {
            string agentId = CommonUtility.Util.if_s(Request.QueryString["AgentId"], null);
            string action = CommonUtility.Util.if_s(Request.QueryString["Action"], null);
            
            //Apply security settings
            FormHandler.SetSecurity(this.Page);
            this.CurrentPage = 1;
            this.PageSize = 10;
         
            if (!string.IsNullOrWhiteSpace(action))
            {
                switch (action)
                {
                    case "Added":
                        lblMessage.Text = " Record for AgentId " + agentId + " successfully added.";
                        break;
                    case "Updated":
                        lblMessage.Text = " Record for AgentId " + agentId + " successfully updated.";
                        break;
                    case "Delete":
                        lblMessage.Text = " Record successfully deleted.";
                        break;
                    default:
                        lblMessage.Text = "";
                        break;
                }
            }
            LookupTableHandler.LoadAgentAllocationStatus(cboStatus, true);
        }
        if (UserSessions.CurrentUser != null)
        {
            this.calSearchCreatedBeginDate.Format = UserSessions.CurrentUser.DatePattern;
            this.calSearchCreatedEndDate.Format = UserSessions.CurrentUser.DatePattern;
           
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        lblMessage.Text = string.Empty;
      
        //validate search params
        string valError = ValidateSearchParams();

        if (!string.IsNullOrWhiteSpace(valError))
        {
            FormHandler.DisplayMessage(Page.ClientScript, valError);
            return;
        }

        this.SearchParameters = null;
        this.CurrentPage = 1;
        grd.PageIndex = 0;
        this.SortOrder = string.Empty;
        this.SortDirectionSearch = SortDirection.Descending;

        Search(false);
        this.grd.Sort(this.SortOrder, this.SortDirectionSearch);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        FormHandler.ClearAllControls(this);
        string url = "~/SecureQualityForms/frmAgentAllocationSearch.aspx?Adding=false";
        Response.Redirect(url);
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureQualityForms/frmAgentAllocationDetail.aspx?Adding=true";
        url += "&PostBackURL=~/SecureQualityForms/frmAgentAllocationSearch.aspx";
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
        TogglegridFields(true);
        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("AgentAllocationList.xls", grd);
        TogglegridFields(false);
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                HyperLink btn = (HyperLink)e.Row.FindControl("AgentKeyID");
                //btn.Attributes.Add("title", this.BuildTooltip(((DataRowView)e.Row.DataItem).Row));

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

    protected void odsAgentAllocations_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter AgentAlloc = new SearchParameter();

        if (AgentDBAName.Text != string.Empty)
            prms.Add("@AgentDBADisplayName", AgentDBAName.Text);

        if (!string.IsNullOrEmpty(AgentID.Text))
        {
            int agentIDInt = int.Parse(AgentID.Text);
            if (agentIDInt > 0)
                prms.Add("@AgentID", agentIDInt);
        }

        if (RepType.Text != string.Empty)
            prms.Add("@RepType", RepType.Text);

        if (!string.IsNullOrEmpty(WFBAllocation.Text))
        {
            int wfbAllocationInt = int.Parse(WFBAllocation.Text);
            if (wfbAllocationInt > 0)
                prms.Add("@Allocation", wfbAllocationInt);
        }

        DateTime createdBeginDate;
        DateTime createdEndDate;
        DateTime DateUpdated;

        //if (!string.IsNullOrEmpty(SearchDateCreated.Text)
        //  && DateTime.TryParseExact(this.SearchDateCreated.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //{
        //    prms.Add("@DateCreated", date);
        //}

        //if (!string.IsNullOrEmpty(SearchCreatedBeginDate.Text)
        //   && DateTime.TryParseExact(this.SearchCreatedBeginDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdBeginDate))
        if (!string.IsNullOrEmpty(SearchCreatedBeginDate.Text))
        {
            DateTime.TryParse(SearchCreatedBeginDate.Text, out createdBeginDate);
            prms.Add("@CreatedBeginDate", createdBeginDate);
        }

        if (!string.IsNullOrEmpty(SearchCreatedEndDate.Text))
        //   && DateTime.TryParseExact(this.SearchCreatedEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdEndDate))
        {
            DateTime.TryParse(SearchCreatedEndDate.Text, out createdEndDate);
            prms.Add("@CreatedEndDate", createdEndDate);
        }

        if (!string.IsNullOrEmpty(SearchDateUpdated.Text))
        //   && DateTime.TryParseExact(this.SearchCreatedEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdEndDate))
        {
            DateTime.TryParse(SearchDateUpdated.Text, out DateUpdated);
            prms.Add("@DateUpdated", DateUpdated);
        }

        if (UserCreated.Text != string.Empty)
            prms.Add("@UserCreated", UserCreated.Text);

        if (UserUpdated.Text != string.Empty)
            prms.Add("@UserUpdated", UserUpdated.Text);

        if (!string.IsNullOrEmpty(SourceNames.Text))
            prms.Add("@SourceName", SourceNames.Text);

        if (!string.IsNullOrEmpty(ReservePercentage.Text))
        {
            //decimal reservePercentageDec = decimal.Parse(ReservePercentage.Text);
            //if (reservePercentageDec > 0)
            //    prms.Add("@ReservePercentage", reservePercentageDec);
            //added by Amit for handle error
            decimal reservePercentageDec = CommonUtility.Util.if_dec(ReservePercentage.Text, -1);
            int val = Decimal.Compare(reservePercentageDec, 999.9999m);
            switch (val)
            {
                case -1:
                case 0:
                    prms.Add("@ReservePercentage", reservePercentageDec);
                    break;
                case 1:
                default:
                    prms.Add("@ReservePercentage", -1);
                    break;
            }
        }


        if (!string.IsNullOrEmpty(BBVAAllocation.Text))
        {
            int bbvaAllocationInt = int.Parse(BBVAAllocation.Text);
            if (bbvaAllocationInt > 0)
                prms.Add("@Allocation", bbvaAllocationInt);
        }

        if (!string.IsNullOrEmpty(CFGAllocations.Text))
        {
            int cfgAllocationInt = int.Parse(CFGAllocations.Text);
            if (cfgAllocationInt > 0)
                prms.Add("@CFGAllocations", cfgAllocationInt);
        }

        if(Convert.ToInt32(cboStatus.SelectedItem.Value) != -1)
        {
            int statusInt = Convert.ToInt32(cboStatus.SelectedItem.Value);
            prms.Add("@StatusId", statusInt);
        }
        


        //If procedure is called for the first time pass a dummy parameter to initial the grid
        //if (prms.Count > 0)
        //{
        if (UserSessions.CurrentUser.IsInternal)
            prms.Add("@InternalUserUID", UserSessions.CurrentUser.UID);

        //Save search fields in session variable
        prms.Add("@SortOrder", this.SortOrder);
        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));

        FormBinding.BindControlsToObject(AgentAlloc, pnlSearch);

        // the bindcontrolstoobject function does not support items to csv list. so we'll do it here manually for now.



        this.SearchParameters = AgentAlloc;

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetAgentAllocationsPagingCount(prms, 0, 0, this.grd.ID).ToString();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.lblMessage.Text = string.Empty;
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
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
    public override void Search(bool IsOnLoad)
    {
        //Populate search fields        
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter AgentAlloc = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(AgentAlloc, pnlSearch);
        }

        grd.DataBind();

        if (IsOnLoad)
            grd.Sort(this.SortOrder, this.SortDirectionSearch);

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

    private string ValidateSearchParams()
    {
        StringBuilder sb = new StringBuilder();
        DateTime date;
        int AgentIDValidation = 0;
        int WFBAllocationsValidation = 0;
        int BBVAAllocationValidation = 0;

        if (!string.IsNullOrEmpty(this.SearchCreatedBeginDate.Text)
            && !DateTime.TryParseExact(this.SearchCreatedBeginDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            this.SearchCreatedBeginDate.Text = string.Empty;
            sb.Append("Please enter a valid Created Begin Date.\\n");
        }

        if (!string.IsNullOrEmpty(this.SearchCreatedEndDate.Text)
            && !DateTime.TryParseExact(this.SearchCreatedEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            this.SearchCreatedEndDate.Text = string.Empty;
            sb.Append("Please enter a valid Created End Date.\\n");
        }

        if (!string.IsNullOrEmpty(this.SearchDateUpdated.Text)
            && !DateTime.TryParseExact(this.SearchDateUpdated.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            this.SearchCreatedEndDate.Text = string.Empty;
            sb.Append("Please enter a valid Date Updated.\\n");
        }
        if (!string.IsNullOrWhiteSpace(this.AgentID.Text) && !int.TryParse(this.AgentID.Text, out AgentIDValidation))
        {
            this.AgentID.Text = string.Empty;
            sb.Append("Please enter a valid Agent ID.\\n");
        }
        if (!string.IsNullOrWhiteSpace(this.WFBAllocation.Text) && !int.TryParse(this.WFBAllocation.Text, out WFBAllocationsValidation))
        {
            this.WFBAllocation.Text = string.Empty;
            sb.Append("Please enter a valid WFB Allocations.\\n");
        }
        if (!string.IsNullOrWhiteSpace(this.BBVAAllocation.Text) && !int.TryParse(this.BBVAAllocation.Text, out BBVAAllocationValidation))
        {
            this.BBVAAllocation.Text = string.Empty;
            sb.Append("Please enter a valid BBVA Allocations.\\n");
        }
        return sb.ToString();
    }
}
