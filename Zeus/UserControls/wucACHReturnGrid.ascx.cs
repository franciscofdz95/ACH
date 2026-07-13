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

public partial class wucACHReturnGrid : System.Web.UI.UserControl
{
    decimal total = 0.0M;

    public delegate void GridViewRowCommandHandler(object sender, GridViewCommandEventArgs e);
    public event GridViewRowCommandHandler GridViewCommand;

    public string GridTitle
    {
        set { lblGridTitle.Text = value; }
    }

    public Unit GridHeight
    {
        set { grd.Height = value; }
    }

    public GridView Grid
    {
        get { return grd; }
    }

    public DataSet SessionName
    {
        get
        {
            if (ViewState["SessionName"] == null)
                return new DataSet();
            else
                return (DataSet)ViewState["SessionName"];
        }
        set { ViewState["SessionName"] = value; }
    }

    public void SetDataSource(DataSet sessionName)
    {
        this.SessionName = sessionName;
        LoadGrid();
    }

    public void ClearGrid()
    {
        this.SessionName = new DataSet();
        LoadGrid();
    }

    private void LoadGrid()
    {
        DataSet dv = (DataSet)this.SessionName;

        if (dv.Tables.Count > 0)
        {
            grd.DataSource = dv;
            grd.DataBind();
            lblRecordCount.Text = "Total Record(s) Found: " + dv.Tables[0].Rows.Count.ToString();
        }
        pnlRows.Visible = !(grd.Rows.Count == 0);
        lblData.Visible = (grd.Rows.Count == 0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LoadGrid();
        }
    }

    private void DoVoid(string tranid)
    {
        try
        {
            AchTransactionFacade facade = new AchTransactionFacade();
            facade.VoidTransaction(Convert.ToInt64(tranid));
            LoadGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel(lblGridTitle.Text + ".xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grd, false, lblGridTitle.Text);
    }

    protected void grdTran_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandSource is Button))
            return;

        Button btn = (Button)e.CommandSource;
        GridViewRow grdRow = (GridViewRow)btn.NamingContainer;

        string returnID = grd.DataKeys[grdRow.RowIndex].Values["ReturnID"].ToString();
        string tranID = grd.DataKeys[grdRow.RowIndex].Values["Trans ID"].ToString();

        switch (btn.Text)
        {
            case "Correction":
                Response.Redirect("~/FormReports/frmAchCorrections.aspx?ReturnID=" + returnID + "&PostBackURL=frmReturnList.aspx");
                break;
            case "Void":
                this.DoVoid(tranID);
                break;
            case "Resubmit":
                Response.Redirect("~/FormReports/frmAchResubmit.aspx?TransID=" + tranID + "&PostBackURL=frmReturnList.aspx");
                break;
            case "Credit":
                Response.Redirect("~/FormReports/frmAchRefund.aspx?TransID=" + tranID + "&PostBackURL=frmReturnList.aspx");
                break;
        }

        if (this.GridViewCommand != null)
        {
            this.GridViewCommand(grd, e);
        }
    }

    protected void grdTran_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                
                total = 0.0M;
                break;

            case DataControlRowType.DataRow:

                //((HyperLink)e.Row.FindControl("TransID")).NavigateUrl = "#";//"javascript:ShowReturn('" + grid.ClientID + "','dlgReturn')";

                Button btn = (Button)e.Row.FindControl("btnAction");
                string action = DataBinder.Eval(e.Row.DataItem,"Action").ToString();

                switch (action)
                {
                    case "C":
                        btn.Text = "Correction";
                        btn.OnClientClick = "return confirm('Do you want to view the corrections for this transaction?');";
                        break;
                    case "R":
                        btn.Text = "Resubmit";
                        btn.OnClientClick = "return confirm('Do you want to resubmit this transaction?');";
                        break;
                    case "V":
                        btn.Text = "Void";
                        btn.OnClientClick = "return confirm('Do you want to void this transaction?');";
                        break;
                    default:
                        btn.BorderStyle = BorderStyle.None;
                        btn.BorderWidth = new Unit("0px");
                        btn.BackColor = System.Drawing.Color.Transparent;
                        btn.Text = action == string.Empty ? "N/A" : action;
                        btn.Enabled = false;
                        btn.ToolTip = action;
                        btn.Width = new Unit("100%");
                        break;
                }

                total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "Total:";
                e.Row.Cells[7].Text = total.ToString("0.00");//"c");
                break;
        }
    }
}
