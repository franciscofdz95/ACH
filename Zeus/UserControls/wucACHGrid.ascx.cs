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
public partial class wucACHGrid : System.Web.UI.UserControl
{
    decimal total = 0.0M;

    public delegate void GridViewRowCommandHandler(object sender, GridViewCommandEventArgs e);
    public event GridViewRowCommandHandler GridViewCommand;

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

    public string GridTitle
    {
        set { lblGridTitle.Text = value; }
        get { return lblGridTitle.Text; }
    }

    public GridView gridTran
    {
        get { return grdTran; }
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
            grdTran.DataSource = dv;
            grdTran.DataBind();
            lblRecordCount.Text = "Total Record(s) Found: " + dv.Tables[0].Rows.Count.ToString();
        }

        lblData.Visible = (grdTran.Rows.Count == 0);
        lblRecordCount.Visible = pnlRows.Visible = !(grdTran.Rows.Count == 0);
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
        FormHandler.Export2Excel(GridTitle + ".xls", grdTran);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        FormHandler.ExportToPDF(grdTran, false, GridTitle);
    }

    protected void grdTran_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandSource is Button))
            return;

        Button btn = (Button)e.CommandSource;
        GridViewRow grdRow = (GridViewRow)btn.NamingContainer;

        string tranID = grdRow.Cells[0].Text;
        switch (btn.Text)
        {
            case "Void":
                this.DoVoid(tranID);
                break;
            case "Resubmit":
                Response.Redirect("~/FormReports/frmAchResubmit.aspx?TransID=" + tranID + "&PostBackURL=" + this.PostBackURL);
                break;
            case "Credit":
                Response.Redirect("~/FormReports/frmAchRefund.aspx?TransID=" + tranID + "&PostBackURL=" + this.PostBackURL);
                break;
        }

        if (this.GridViewCommand != null)
        {
            this.GridViewCommand(grdTran, e);
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

                Button btn = (Button)e.Row.FindControl("btnAction");
                string action = DataBinder.Eval(e.Row.DataItem, "Action").ToString();

                total = total + Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));

                switch (action)
                {
                    case "C":
                        btn.Text = "Credit";
                        btn.OnClientClick = "return confirm('Do you want to credit this transaction?');";
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
                        btn.Width = new Unit("");
                        break;
                }
                break;

            case DataControlRowType.Footer:

                e.Row.Cells[0].Text = "Total:";
                e.Row.Cells[8].Text = total.ToString("0.00");//"c");
                break;
        }

    }
}
