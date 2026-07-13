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

using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.DataObjects;

public partial class wucSelectMerchant : wucBaseSearch
{
   public string WebDialogWindowClientID
    {
        get { return ViewState["WebDialogWindowClientID"].ToString(); }
        set { ViewState["WebDialogWindowClientID"] = value; }
    }

    public string HookTableDBAClientID
    {
        get { return DataLayer.Field2Str(ViewState["HookTableDBAClientID"]); }
        set { ViewState["HookTableDBAClientID"] = value; }
    }

    public string HookTableIDClientID
    {
        get { return ViewState["HookTableIDClientID"].ToString(); }
        set { ViewState["HookTableIDClientID"] = value; }
    }

    public string HookTableUIDClientID
    {
        get { return ViewState["HookTableUIDClientID"].ToString(); }
        set { ViewState["HookTableUIDClientID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ZID.Attributes.Add("onKeyPress", "CheckNumeric();");

        if (!IsPostBack)
        {
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            Search(true);
        }
    }

    public void Search(bool IsOnLoad)
    {      
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        grd.DataBind();

        pnlRecords.Visible = (grd.Rows.Count > 0);
        pnlNoRecords.Visible = !(grd.Rows.Count > 0);

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        grd.PageIndex = 0;
        this.CurrentPage = 1;
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.Search(false);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        this.Search(false);
    }
    
    protected void grdMer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                string UID = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();
                LinkButton btn = ((LinkButton)e.Row.FindControl("btnSelect"));
                btn.Attributes.Add("onClick", "ShowHookTableSelectedMerchant('" + e.Row.Cells[2].Text + "','" + e.Row.Cells[3].Text + "','" + UID + "');");

                break;
            default:
                break;
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        this.Search(false);
    }

    protected void odsMerchants_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app;
        
        //If procedure is called for the first time pass a dummy parameter to initial the grid

        if (ZID.Text != string.Empty)
            prms.Add("@ID", ZID.Text);

        if (LegalName.Text != string.Empty)
            prms.Add("@BusinessLegalName", LegalName.Text);

        if (DBA.Text != string.Empty)
            prms.Add("@BusinessDBAName", DBA.Text);

        if (MID.Text != string.Empty)
            prms.Add("@SettlePlatformMid", MID.Text);

        if (BusinessContact.Text != string.Empty)
            prms.Add("@BusinessContact", BusinessContact.Text);

        if (prms.Count > 0)
        {           
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

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
        }
        else
        {
            // we do this so that we don't call the SP with zero paramters. it's very slow otherwise.
            e.Cancel = true;
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


}
