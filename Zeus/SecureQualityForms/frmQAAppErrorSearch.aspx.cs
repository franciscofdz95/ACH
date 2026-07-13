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

public partial class frmQAAppErrorSearch : frmBaseSearch
{
    protected void Page_Load(object sender, EventArgs e)
    {

       
        lblMessage.Text = "";
        if (UserSessions.CurrentUser == null)
        {
            Response.Redirect("~/frmLogin.aspx");
        }
         ((HyperLink)this.Master.FindControl("lnkApplicationErrors")).CssClass = "active";
        lblMessage.Text = String.Empty;
        if (!IsPostBack)
        {
            LoadDropdownsREFData();
            FormHandler.SetSecurity(this.Page);


            string mid = CommonUtility.Util.if_s(Request.QueryString["MID"], null);
            string action = CommonUtility.Util.if_s(Request.QueryString["Action"], null);
            this.CurrentPage = 1;
            this.PageSize = 10;
            this.Master.SetStatusBarText("Search");
           
            if (!string.IsNullOrWhiteSpace(action))
            {
                switch (action)
                {
                    case "Added":
                        lblMessage.Text = " Record for MID " + mid + " successfully added.";
                        break;
                    case "Updated":
                        lblMessage.Text = " Record for MID " + mid + " successfully updated.";
                        break;
                    case "Delete":
                        lblMessage.Text = " Record successfully deleted.";
                        break;
                    default:
                        lblMessage.Text = "";
                        break;
                }

            }
           
        }
      
        if (UserSessions.CurrentUser != null)
        {
            this.calSearchCreatedBeginDate.Format = UserSessions.CurrentUser.DatePattern;
            this.calSearchCreatedEndDate.Format = UserSessions.CurrentUser.DatePattern;
            this.calSearchDateErrorFound.Format = UserSessions.CurrentUser.DatePattern;
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
        grdQAAppErrors.PageIndex = 0;
        this.SortOrder = string.Empty;
        this.SortDirectionSearch = SortDirection.Descending;
        Search(false);
        this.grdQAAppErrors.Sort(this.SortOrder, SortDirectionSearch);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        FormHandler.ClearAllControls(this);
        string url = "~/SecureQualityForms/frmQAAppErrorSearch.aspx?Adding=false";
        Response.Redirect(url);
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureQualityForms/frmQAAppErrorDetail.aspx?Adding=true";
        url += "&PostBackURL=~/SecureQualityForms/frmQAAppErrorSearch.aspx";
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
            this.CurrentPage = grdQAAppErrors.PageIndex + 1;
        }
        TogglegridFields(true);
        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("QAAppErrorsList.xls", grdQAAppErrors);
        TogglegridFields(false);
    }

    protected void grdQAAppErrors_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void grdQAAppErrors_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void grdQAAppErrors_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdQAAppErrors.PageIndex = e.NewPageIndex;
        this.CurrentPage = e.NewPageIndex + 1;        
        this.Search(false);
    }

    protected void grdQAAppErrors_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    protected void odsQAAppErrors_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        Hashtable prms = new Hashtable();
        SearchParameter prmQAAppError = new SearchParameter(); ;
        DateTime createdBeginDate;
        DateTime createdEndDate;
        DateTime dateErrorFound;


        if (MID.Text != string.Empty)
            prms.Add("@MID", MID.Text);

        if (Rep.Text != string.Empty)
            prms.Add("@Rep", Rep.Text);

        if (Dept.SelectedValue != "-1")
            prms.Add("@DepartmentID", CommonUtility.Util.if_i(Dept.SelectedValue, 0));

       
        if (!string.IsNullOrEmpty(SearchCreatedBeginDate.Text))
        {
            DateTime.TryParse(SearchCreatedBeginDate.Text, out createdBeginDate);
            prms.Add("@CreatedBeginDate", createdBeginDate);
        }

        if (!string.IsNullOrEmpty(SearchCreatedEndDate.Text))
        {
            DateTime.TryParse(SearchCreatedEndDate.Text, out createdEndDate);
            prms.Add("@CreatedEndDate", createdEndDate);
        }

        if (!string.IsNullOrEmpty(DateErrorFound.Text))
        {
            DateTime.TryParse(DateErrorFound.Text, out dateErrorFound);
            prms.Add("@DateErrorFound", dateErrorFound);
        }

        if (ErrorFoundBy.SelectedValue != "-1")
            prms.Add("@ErrorFoundByID", CommonUtility.Util.if_i(ErrorFoundBy.SelectedValue, 0));

        if (Category.SelectedValue != "-1")
            prms.Add("@CategoryID", CommonUtility.Util.if_i(Category.SelectedValue, 0));

        if (SubCategory.SelectedValue != "-1")
            prms.Add("@SubCategoryID", CommonUtility.Util.if_i(SubCategory.SelectedValue, 0));

        if (Description.Text != string.Empty)
            prms.Add("@Description", Description.Text);       

        prms.Add("@SortDirection", this.ConvertSortDirectionToSql(this.SortDirectionSearch));
        prms.Add("@SortOrder", this.SortOrder);
     

        FormBinding.BindControlsToObject(prmQAAppError, pnlSearch);

        this.SearchParameters = prmQAAppError;

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grdQAAppErrors.PageSize = this.PageSize;
        grdQAAppErrors.PageIndex = this.CurrentPage - 1;


        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grdQAAppErrors.ID;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetQaAppErrorsPagingCount(prms, 0, 0, this.grdQAAppErrors.ID).ToString();
    }

    private object ConvertSortDirectionToSql(SortDirection direction)
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

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }

    public void TogglegridFields(bool visible)
    {
        for (int i = 0; i < grdQAAppErrors.Columns.Count; i++)
        {
            if (grdQAAppErrors.Columns[i].ItemStyle.CssClass == "togle")
            {
                grdQAAppErrors.Columns[i].Visible = visible;
            }
        }
    }
    public override void Search(bool IsOnLoad)
    {
        //Populate search fields        
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter prmAppError = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(prmAppError, pnlSearch);
        }

        grdQAAppErrors.DataBind();

        if (IsOnLoad)
            grdQAAppErrors.Sort(this.SortOrder, this.SortDirectionSearch);

        pnlRecords.Visible = grdQAAppErrors.Rows.Count > 0;
        pnlNoRecords.Visible = grdQAAppErrors.Rows.Count == 0;


    }


    private string ValidateSearchParams()
    {
        StringBuilder sb = new StringBuilder();
        DateTime date;

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

        if (!string.IsNullOrWhiteSpace(this.DateErrorFound.Text)
            && !DateTime.TryParseExact(this.DateErrorFound.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            this.DateErrorFound.Text = string.Empty;
            sb.Append("Please enter a valid Date Error Found.\\n");
            
        }

        return sb.ToString();
    }
    private void LoadDropdownsREFData()
    {
        try
        {
            LookupTableHandler.LoadQAAppErrorFoundBy(ErrorFoundBy,true);
            LookupTableHandler.LoadQAAppDepartment(Dept,true);
            LookupTableHandler.LoadQAAppCategory(Category,true);
            LookupTableHandler.LoadQAAppSubCategory(SubCategory,true);
        }
        catch (Exception)
        {


        }

    }
}
