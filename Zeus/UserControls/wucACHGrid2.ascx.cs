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
using CommonUtility;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

public partial class wucACHGrid2 : System.Web.UI.UserControl
{
    private decimal m_Amount = 0.00M;

    public Hashtable m_Prms
    {
        get
        {
            if (ViewState["m_Prms"] == null)
                return null;
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

    public Unit GridHeight
    {
        set { grd.Height = value; }
    }

    public GridView Grid
    {
        get { return grd; }
    }

    public void ToggleColumnDisplay(string type)
    {
        switch (type)
        {
            case "Admin":
                grd.Columns[16].Visible = true;
                grd.Columns[17].Visible = true;

                grd.Columns[8].Visible = false;
                break;
        }

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

    public void SetDataSource(Hashtable prms, int pagesize)
    {
        grd.DataSourceID = "odsTransactions";
        this.CurrentPage = 1;
        this.PageSize = pagesize;
        grd.PageIndex = 0;
        grd.PageSize = pagesize;
        this.m_Prms = prms;
        BindGrid();
    }

    private void BindGrid()
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
            m_Prms.Add("@SortOrder", "TRANSID");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

         m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);


        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetACHTransactionsPagingRowCount(m_Prms, 0, 0, this.ID).ToString();
                
        grd.DataBind();

        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;        

    }
    
    public void ClearGrid()
    {

        lblRecordCount.Text = "Total Record(s) Found: 0";
        grd.Columns.Clear();
        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
    
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.CurrentPage = 1;
        }
    }
    
    private void DoVoid(string tranid)
    {

        try
        {
            AchTransactionFacade facade = new AchTransactionFacade();
            facade.VoidTransaction(Convert.ToInt64(tranid));
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
            this.m_Amount += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));

            string action = DataBinder.Eval(e.Row.DataItem, "Action").ToString();

            //LinkButton lnkTransID = (LinkButton)e.Row.FindControl("lnkTransID");
            //lnkTransID.Text = DataBinder.Eval(e.Row.DataItem, "TransID").ToString();
            //lnkTransID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TransID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "Merchant ID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();



            LinkButton lnk = (LinkButton)e.Row.FindControl("lnk1");
            LinkButton lnk2 = (LinkButton)e.Row.FindControl("lnk2");

            lnk.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TransID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "Merchant ID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();
            lnk2.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TransID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "Merchant ID").ToString() + "," + DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();

            lnk.CssClass = "boxEnabled";
            lnk2.CssClass = "boxEnabled";

            lnk.Visible = false;
            lnk2.Visible = false;

            switch (action)
            {
                case "C":
                    lnk.Text = "Credit";
                    lnk.OnClientClick = "return confirm('Do you want to credit this transaction? Click OK to continue to the next screen.');";
                    lnk.Visible = true;
                    break;
                case "R":
                    lnk.Text = "Resubmit";
                    lnk.OnClientClick = "return confirm('Do you want to resubmit this transaction? Click OK to continue to the next screen.');";
                    lnk.Visible = true;
                    break;
                case "V":
                    lnk.Text = "Void";
                    lnk.OnClientClick = "return confirm('Do you want to void this transaction? Click OK to void transaction.');";
                    lnk.Visible = true;
                    break;
                default:
                    lnk.CssClass = "boxDisabled";
                    lnk.BorderStyle = BorderStyle.None;
                    lnk.BorderWidth = new Unit("0px");
                    lnk.BackColor = System.Drawing.Color.Transparent;
                    lnk.Text = action == string.Empty ? "N/A" : action.Trim();
                    lnk.Enabled = false;
                    lnk.ToolTip = action;
                    lnk.Width = new Unit("100%");
                    lnk.Visible = true;
                    //btn.Width = new Unit("");
                    break;
            }
             string [] split = e.Row.Cells[7].Text.Split(' ');
             if (split.Length > 1)
             {
                 e.Row.Cells[7].Text = split[0] + " " + WebUtil.ConvertToUserShortDateTimeFormat(split[1]);
             }

            e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);
            e.Row.Cells[15].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[15].Text);
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "Totals:";
            e.Row.Cells[8].Text = this.m_Amount.ToString("0.00");//"c");
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Right;

        }


    }
    
    protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms; 
        e.InputParameters[3] = this.ID; //controlID
    }
    
    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
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
    
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        grd.Columns[8].Visible = true;
        grd.Columns[9].Visible = true;
        grd.Columns[10].Visible = true;
        grd.Columns[11].Visible = true;
        grd.Columns[12].Visible = true;
        grd.Columns[13].Visible = true;
        grd.Columns[14].Visible = true;
        grd.Columns[15].Visible = true;

        grd.Columns[16].Visible = true;
        grd.Columns[17].Visible = true;
        grd.Columns[18].Visible = true;
        grd.Columns[19].Visible = true;
        grd.Columns[20].Visible = true;
        grd.Columns[21].Visible = true;
        grd.Columns[22].Visible = true;
        grd.Columns[23].Visible = true;
        grd.Columns[24].Visible = true;
        grd.Columns[25].Visible = true;
        grd.Columns[26].Visible = true;
        grd.Columns[27].Visible = true;
        grd.Columns[28].Visible = true;



        if (lstExportPageSize.SelectedIndex == 0)
        {
            this.BindGrid();
            FormHandler.Export2Excel("ACH Transactions.xls", grd);
        }
        else
        {
            this.CurrentPage = 1;
            int pagesize = this.PageSize;
            this.PageSize = 5000;
            grd.PageSize = this.PageSize;
            this.BindGrid();
            FormHandler.Export2Excel("ACH Transactions.xls", grd);
            this.PageSize = pagesize;
            grd.PageSize = this.PageSize;
        }

        grd.Columns[8].Visible = false;
        grd.Columns[9].Visible = false;
        grd.Columns[10].Visible = false;
        grd.Columns[11].Visible = false;
        grd.Columns[12].Visible = false;
        grd.Columns[13].Visible = false;
        grd.Columns[14].Visible = false;
        grd.Columns[15].Visible = false;

        grd.Columns[16].Visible = false;
        grd.Columns[17].Visible = false;
        grd.Columns[18].Visible = false;
        grd.Columns[19].Visible = false;
        grd.Columns[20].Visible = false;
        grd.Columns[21].Visible = false;
        grd.Columns[22].Visible = false;
        grd.Columns[23].Visible = false;
        grd.Columns[24].Visible = false;
        grd.Columns[25].Visible = false;
        grd.Columns[26].Visible = false;
        grd.Columns[27].Visible = false;
        grd.Columns[28].Visible = false;

        this.BindGrid();

    }
    
    protected void btnPDF_Click(object sender, EventArgs e)
    {
        grd.Columns[8].Visible = true;
        grd.Columns[9].Visible = true;
        grd.Columns[10].Visible = true;
        grd.Columns[11].Visible = true;
        grd.Columns[12].Visible = true;
        grd.Columns[13].Visible = true;
        grd.Columns[14].Visible = true;
        grd.Columns[15].Visible = true;

        grd.Columns[16].Visible = true;
        grd.Columns[17].Visible = true;
        grd.Columns[18].Visible = true;
        grd.Columns[19].Visible = true;
        grd.Columns[20].Visible = true;
        grd.Columns[21].Visible = true;
        grd.Columns[22].Visible = true;
        grd.Columns[23].Visible = true;
        grd.Columns[24].Visible = true;
        grd.Columns[25].Visible = true;
        grd.Columns[26].Visible = true;
        grd.Columns[27].Visible = true;
        grd.Columns[28].Visible = true;


        if (lstExportPageSize.SelectedIndex == 0)
        {
            this.BindGrid();
            FormHandler.ExportToPDF(grd, true, "ACH Transactions");
        }
        else
        {
            this.CurrentPage = 1;
            int pagesize = this.PageSize;
            this.PageSize = 5000;
            grd.PageSize = this.PageSize;
            this.BindGrid();
            FormHandler.ExportToPDF(grd, true, "ACH Transactions");
            this.PageSize = pagesize;
            grd.PageSize = this.PageSize;
        }

        grd.Columns[8].Visible = false;
        grd.Columns[9].Visible = false;
        grd.Columns[10].Visible = false;
        grd.Columns[11].Visible = false;
        grd.Columns[12].Visible = false;
        grd.Columns[13].Visible = false;
        grd.Columns[14].Visible = false;
        grd.Columns[15].Visible = false;

        grd.Columns[16].Visible = false;
        grd.Columns[17].Visible = false;
        grd.Columns[18].Visible = false;
        grd.Columns[19].Visible = false;
        grd.Columns[20].Visible = false;
        grd.Columns[21].Visible = false;
        grd.Columns[22].Visible = false;
        grd.Columns[23].Visible = false;
        grd.Columns[24].Visible = false;
        grd.Columns[25].Visible = false;
        grd.Columns[26].Visible = false;
        grd.Columns[27].Visible = false;
        grd.Columns[28].Visible = false;

        this.BindGrid();

    }
    
    protected void grd_DataBinding(object sender, EventArgs e)
    {
        this.m_Amount = 0;
    }

}
