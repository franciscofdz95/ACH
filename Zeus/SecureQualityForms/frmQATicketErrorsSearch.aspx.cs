using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class frmQATicketErrorsSearch : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (UserSessions.CurrentUser == null)
        {
            Response.Redirect("~/frmLogin.aspx");
        }
        ((HyperLink)this.Master.FindControl("lnkTicketErrors")).CssClass = "active";
        
        dvMessage.InnerHtml = string.Empty;
        dvMessage.Visible = false;

        if (!IsPostBack)
        {
            LoadDropdownsREFData();

            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            this.CurrentPage = 1;
            this.PageSize = 10;
            this.Master.SetStatusBarText("Search");

            string valZID = CommonUtility.Util.if_s(Request.QueryString["ZID"], string.Empty);
            string valTicketID = CommonUtility.Util.if_s(Request.QueryString["TicketID"], string.Empty);
            string action = CommonUtility.Util.if_s(Request.QueryString["Action"], string.Empty);

            if (!string.IsNullOrWhiteSpace(action))
            {
                string message = string.Empty;
                switch (action)
                {
                    case "Added":
                        message = " Record for ZID " + valZID + " and Ticket ID " + valTicketID + " successfully added.";
                        break;
                    case "Updated":
                        message = " Record for ZID " + valZID + " and Ticket ID " + valTicketID + " successfully updated.";
                        break;
                    case "Deleted":
                        message = " Record for ZID " + valZID + " and Ticket ID " + valTicketID + " successfully deleted.";
                        break;
                    default:
                        message = string.Empty;
                        break;
                }
                dvMessage.InnerHtml = message;
                dvMessage.Visible = true;
            }
            if (UserSessions.CurrentUser != null)
            {
                this.calSearchCreatedBeginDate.Format = UserSessions.CurrentUser.DatePattern;
                this.calSearchCreatedEndDate.Format = UserSessions.CurrentUser.DatePattern;
                this.calsearchDateErrorFound.Format = UserSessions.CurrentUser.DatePattern;
            }
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

        this.SearchParameters = null;
        this.CurrentPage = 1;
        grdTicketErrorsSearch.PageIndex = 0;
        this.SortOrder = string.Empty;
        this.SortDirectionSearch = SortDirection.Descending;
        Search(false);
        this.grdTicketErrorsSearch.Sort(this.SortOrder, this.SortDirectionSearch);
    }
    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        FormHandler.ClearAllControls(this);
        string url = "~/SecureQualityForms/frmQATicketErrorsSearch.aspx?Adding=false";
        Response.Redirect(url);
    }
    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureQualityForms/frmQATicketErrorsDetail.aspx?Adding=true";
        url += "&PostBackURL=~/SecureQualityForms/frmQATicketErrorsSearch.aspx";
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
            this.CurrentPage = grdTicketErrorsSearch.PageIndex + 1;
        }
        TogglegridFields(true);
        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("QATicketErrorsList.xls", grdTicketErrorsSearch);
        TogglegridFields(false);
    }
    public override void Search(bool IsOnLoad)
    {
        //Populate search fields        
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter QATicketErrorSearchParams = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(QATicketErrorSearchParams, pnlSearch);
        }
        grdTicketErrorsSearch.DataBind();

        pnlRecords.Visible = grdTicketErrorsSearch.Rows.Count > 0;
        pnlNoRecords.Visible = grdTicketErrorsSearch.Rows.Count == 0;


    }
    protected void grdTicketErrorsSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdTicketErrorsSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }
    protected void grdTicketErrorsSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTicketErrorsSearch.PageIndex = e.NewPageIndex;
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    protected void grdTicketErrorsSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }
    protected void odsQATicketErrors_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter TicketErrorsSearchPrms = new SearchParameter();
        DateTime createdBeginDate;
        DateTime createdEndDate;
        DateTime dateErrorFound;

        if (!string.IsNullOrEmpty(SearchCreatedBeginDate.Text))
        {
            DateTime.TryParse(SearchCreatedBeginDate.Text, out createdBeginDate);
            prms.Add("@CreatedBeginDate", DataLayer.Date2Field(createdBeginDate));
        }

        if (!string.IsNullOrEmpty(SearchCreatedEndDate.Text))
        {
            DateTime.TryParse(SearchCreatedEndDate.Text, out createdEndDate);
            prms.Add("@CreatedEndDate", DataLayer.Date2Field(createdEndDate));
        }

        if (!string.IsNullOrWhiteSpace(txtZID.Text))
            prms.Add("@ZID", txtZID.Text);

        if (!string.IsNullOrWhiteSpace(txtTicketID.Text))
            prms.Add("@TicketID", txtTicketID.Text);

        if (!string.IsNullOrWhiteSpace(txtRep.Text))
            prms.Add("@Rep", txtRep.Text);

        if (!string.IsNullOrEmpty(txtDateErrorFound.Text))
        {
            DateTime.TryParse(txtDateErrorFound.Text, out dateErrorFound);
            prms.Add("@DateErrorFound", DataLayer.Date2Field(dateErrorFound));
        }

        if (Category.SelectedValue != "-1")
            prms.Add("@CategoryID", CommonUtility.Util.if_i(Category.SelectedValue, 0));

        if (SubCategory.SelectedValue != "-1")
            prms.Add("@SubCategoryID", CommonUtility.Util.if_i(SubCategory.SelectedValue, 0));

        if (!string.IsNullOrWhiteSpace(txtDescription.Text))
            prms.Add("@Description", txtDescription.Text);


        prms.Add("@SortOrder",this.SortOrder);

        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));

        FormBinding.BindControlsToObject(TicketErrorsSearchPrms, pnlSearch);

        // the bindcontrolstoobject function does not support items to csv list. so we'll do it here manually for now.
        this.SearchParameters = TicketErrorsSearchPrms;

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grdTicketErrorsSearch.PageSize = this.PageSize;
        grdTicketErrorsSearch.PageIndex = this.CurrentPage - 1;

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grdTicketErrorsSearch.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetQATicketErrorsPagingCount(prms, 0, 0, this.grdTicketErrorsSearch.ID).ToString();
    }
    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
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
    public void TogglegridFields(bool visible)
    {
        for (int i = 0; i < grdTicketErrorsSearch.Columns.Count; i++)
        {
            if (grdTicketErrorsSearch.Columns[i].ItemStyle.CssClass == "togle")
            {
                grdTicketErrorsSearch.Columns[i].Visible = visible;
            }
        }
    }
    private string ValidateSearchParams()
    {
        StringBuilder sb = new StringBuilder();
        DateTime date;
        int ZIDForvalidation;
        int TicketIDForValidation;

        //validate search begin date
        if (!string.IsNullOrWhiteSpace(this.SearchCreatedBeginDate.Text)
            && !DateTime.TryParseExact(this.SearchCreatedBeginDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid Begin Date.\\n");
            this.SearchCreatedBeginDate.Text = string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(this.SearchCreatedEndDate.Text)
            && !DateTime.TryParseExact(this.SearchCreatedEndDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid End Date.\\n");
            this.SearchCreatedEndDate.Text = string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(this.txtZID.Text) && !int.TryParse(this.txtZID.Text, out ZIDForvalidation))
        {
            this.txtZID.Text = string.Empty;
            sb.Append("Please enter a valid ZID.\\n");
        }
        if (!string.IsNullOrWhiteSpace(this.txtTicketID.Text) && !int.TryParse(this.txtTicketID.Text, out TicketIDForValidation))
        {
            this.txtTicketID.Text = string.Empty;
            sb.Append("Please enter a valid Ticket ID.\\n");
        }

        if (!string.IsNullOrWhiteSpace(this.txtDateErrorFound.Text)
            && !DateTime.TryParseExact(this.txtDateErrorFound.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            sb.Append("Please enter a valid Date Error Found.\\n");
            this.txtDateErrorFound.Text = string.Empty;
        }

        return sb.ToString();
    }

    private void LoadDropdownsREFData()
    {
        try
        {
            LookupTableHandler.LoadQATicketCategory(Category, true);
            LookupTableHandler.LoadQATicketSubCategory(SubCategory, true);
        }
        catch (Exception)
        {


        }
    }
}
