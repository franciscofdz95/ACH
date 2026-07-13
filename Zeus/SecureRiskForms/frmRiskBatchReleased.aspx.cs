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
using PaymentXP.Facade;


public partial class frmRiskBatchReleased : frmBaseSearch
{
   
    private void LoadGrid()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ToProcessDate", DateTime.Today);
        DataSet ds = DataAccess.DataRiskDao.GetBatchReleased(prms);
        grd.DataSource = ds;
        grd.DataBind();

        lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();

        //Select the fist row if row count is > 0
        if (grd.Rows.Count > 0)
        {
            grd.SelectedIndex = 0;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LoadGrid();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Risk Batch Released.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grd, false, "Risk Batch Released");
    }

    private void DoVoid(int MerchantID, int BatchID, string UserName)
    {
        try
        {
            DataAccess.DataRiskDao.VoidBatch(MerchantID, BatchID, UserName, 2);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DoRelease(int MerchantID, int BatchID, string UserName)
    {
        try
        {
            DataAccess.DataRiskDao.ReleaseBatch(MerchantID, BatchID, UserName, 2);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DoHold(int MerchantID, int BatchID, string UserName)
    {
        try
        {
            DataAccess.DataRiskDao.HoldBatch(MerchantID, BatchID, UserName, 2);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;

        if (!(e.CommandSource is Button))
            return;

        Button btn = (Button)e.CommandSource;
        GridViewRow grdRow = (GridViewRow)btn.NamingContainer;

        int MerchantID = Convert.ToInt32(grd.DataKeys[grdRow.RowIndex].Values["ZID"].ToString());
        int BatchID = Convert.ToInt32(grd.DataKeys[grdRow.RowIndex].Values["BatchID"].ToString());

        switch (btn.CommandName)
        {
            case "Hold":
                this.DoHold(MerchantID, BatchID, UserSessions.CurrentUser.UserName);
                LoadGrid();
                break;
            case "Void":
                this.DoVoid(MerchantID, BatchID, UserSessions.CurrentUser.UserName);
                LoadGrid();
                break;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //switch (e.Row.RowType)
        //{
        //    case DataControlRowType.DataRow:

        //        ((Button)e.Row.Cells[0].FindControl("btnHold")).Enabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Hold"));
        //        ((Button)e.Row.Cells[1].FindControl("btnVoid")).Enabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Void"));
        //        ((HyperLink)e.Row.Cells[2].FindControl("lnkMID")).NavigateUrl = "frmRiskBatchDetails.aspx?MerchantAppUID=" + grd.DataKeys[e.Row.RowIndex].Values["MerchantAppUID"].ToString() + "&BatchID=" + grd.DataKeys[e.Row.RowIndex].Values["BatchID"].ToString();
        //        break;

        //    default:
        //        break;
        //}
    }
}
