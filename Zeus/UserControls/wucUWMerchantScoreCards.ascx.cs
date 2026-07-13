using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using Infragistics.WebUI.WebDataInput;
using PaymentXP.Facade;
using System.Text;
using System.Data;
using System.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using CommonUtility;



public partial class wucMerchantScoreBoards : wucBaseSearch
{
    public delegate void GridViewRowCommandHandler(object sender, int ScoreCardID,string ScoreCardName, string TimePeriod);
    public event GridViewRowCommandHandler GridViewCommand;


    private int _MerchantID;

    public int MerchantID
    {
        get { return _MerchantID; }
        set { _MerchantID = value; }
    }

    public void SetDataSource(Hashtable prms)
    {
        GridView1.DataSourceID = "ods";        
        this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);        
        GridView1.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
        GridView1.PageIndex = this.CurrentPage - 1;

        this.m_Prms = prms;

        this.m_Prms["@PageSize"] = this.PageSize;
        this.m_Prms["@CurrentPage"] = this.CurrentPage;
        this.m_Prms["@SortOrder"] = this.SortOrder;

        BindGrid();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {           
            string[] arr = CommonUtility.Util.if_s(e.CommandArgument, "").Split(new char[] { '_' });

            int ScoreCardID = CommonUtility.Util.if_i(arr[0],0);
            string ScoreCardName = arr[1];
            string TimePeriod = arr[2];
           
            if (this.GridViewCommand != null)
            {
                this.GridViewCommand(GridView1, ScoreCardID, ScoreCardName, TimePeriod);
            }
        }

    }


    private void BindGrid()
    {

        if (m_Prms == null)
            m_Prms = new Hashtable();

        if (!m_Prms.ContainsKey("@PageSize"))
            m_Prms.Add("@PageSize", this.PageSize);
        else
            m_Prms["@PageSize"] = this.PageSize;

        if (!m_Prms.ContainsKey("@CurrentPage"))
            m_Prms.Add("@CurrentPage", this.CurrentPage);
        else
            m_Prms["@CurrentPage"] = this.CurrentPage;

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", "DateCreated");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

        int rowcount = DataMerchantAppPaging.GetMerchantScoreCardPagingCount(m_Prms, this.PageSize, this.CurrentPage);

        lblRowCount.Text = rowcount.ToString();

        GridView1.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                string datecreated = WebUtil.ConvertToUserShortDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCreated")));
                e.Row.Cells[3].Text = string.IsNullOrEmpty(datecreated) ? "" : datecreated;
                
                break;
        }
    }
    
    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);

        this.m_Prms["@PageSize"] = this.PageSize;
        this.m_Prms["@CurrentPage"] = this.CurrentPage;
        this.m_Prms["@SortOrder"] = this.SortOrder;

        this.SetDataSource(this.m_Prms);
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.m_Prms["@CurrentPage"] = this.CurrentPage;
        this.SetDataSource(this.m_Prms);
    }
    
    protected void cboPageSize2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
        this.m_Prms["@PageSize"] = this.PageSize;
        this.m_Prms["@CurrentPage"] = this.CurrentPage;

        this.SetDataSource(this.m_Prms);
    }      


}
