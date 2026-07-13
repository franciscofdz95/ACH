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

public partial class wucTransactionCCReadOnly : System.Web.UI.UserControl
{
    private int TID = 0;
  
    public void ToggleButtons()
    {
        bool perform = TransTypeName.Text.Trim().ToUpper() != "AUTH";
        perform = StatusName.Text.Trim().ToUpper() == "SUCCESS";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadGrid(TID);
        this.ToggleButtons();
    }

    private string GetTransTypeID()
    {
        string transtype = string.Empty;

        switch (TransTypeName.Text.Trim().ToUpper())
        {
            case "SALE":
                transtype = "312";
                break;
            case "AUTH":
                transtype = "310";
                break;
            case "Bl CREDIT":
                transtype = "326";
                break;
            default:
                transtype = "312";
                break;
        }
        return transtype;
    }   

    public void LoadGrid(int transID)
    {
        Hashtable prms = new Hashtable();
        if (transID != 0)
        {
            TID = transID;
            prms.Add("@TransID", transID);

            DataSet ds = DataAccess.DataRiskDao.GetTransIDDetails(prms);
            DataView dv = ds.Tables[0].DefaultView;
            //lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();

            //Bind grid
            grd.DataSource = dv;
        }
        else
            grd.DataSource = null;
        grd.DataBind();
        pnl.Visible = (grd.Rows.Count > 0);
        noRecords.Visible = !(grd.Rows.Count > 0);

    }

    //protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    grd.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
    //    LoadGrid(TID);
    //}

    //private string ConvertSortDirectionToSql(SortDirection sortDireciton)
    //{
    //    string newSortDirection = String.Empty;
    //    switch (sortDireciton)
    //    {
    //        case SortDirection.Ascending:
    //            newSortDirection = "ASC";
    //            CurrentSort = SortDirection.Descending;
    //            break;
    //        case SortDirection.Descending:
    //            newSortDirection = "DESC";
    //            CurrentSort = SortDirection.Ascending;
    //            break;
    //    }
    //    return newSortDirection;
    //}

    //public SortDirection CurrentSort
    //{
    //    get
    //    {
    //        if (ViewState["sortDir"] == null)
    //        {
    //            return SortDirection.Ascending;
    //        }
    //        return (SortDirection)ViewState["sortDir"];
    //    }
    //    set { ViewState["sortDir"] = value; }
    //}

    //public string CurrentExp
    //{
    //    get
    //    {
    //        if (ViewState["sortExp"] == null)
    //        {
    //            return "MID";
    //        }
    //        return ViewState["sortExp"].ToString();
    //    }
    //    set { ViewState["sortExp"] = value; }
    //}

    //protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    grd.PageIndex = e.NewPageIndex;
    //    LoadGrid(TID);
    //}

    //protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    LoadGrid(TID);
    //    DataTable dataTable = ((DataView)grd.DataSource).Table;
    //    if (dataTable != null)
    //    {
    //        DataView dataView = new DataView(dataTable);
    //        if (CurrentExp != e.SortExpression)
    //            CurrentSort = SortDirection.Ascending;
    //        dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
    //        grd.DataSource = dataView;
    //        grd.DataBind();
    //        CurrentExp = e.SortExpression;
    //    }
    //}
}
