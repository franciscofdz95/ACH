using System;
using System.Data;
using System.Data.SqlClient;
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


public partial class frmRiskBatchDetails : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.UID = Request["MerchantAppUID"].ToString();
            this.FormShow(this.UID);
            LookupTableHandler.LoadBatchExceptions(BatchExceptions, true, this.UID, string.Empty, Request["BatchID"].ToString());
            ListHandler.ListFindItem(StatusID, "0");
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = 1;

            LoadGrid();
        }
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

        LoadGrid();
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        FormHandler.Export2Excel("Risk Batch Summary.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
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

        LoadGrid();
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        FormHandler.ExportToPDF(grd, true, "Risk Batch Summary.xls");
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadGrid();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadGrid();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        LoadGrid();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        BatchExceptions.SelectedIndex = 0;
        StatusID.SelectedIndex = 0;
        lstTransTypeID.SelectedIndex = 0;
        LoadGrid();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grd.PageSize = this.PageSize;
        LoadGrid();
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

    private void LoadGrid()
    {
        grd.DataBind();

        pnl.Visible = (grd.Rows.Count > 0);
        noRecords.Visible = !(grd.Rows.Count > 0);

    }

    public override void FormShow(string ID)
    {
        
        MerchantFacade facade = new MerchantFacade();

        MerchantApp agreement = facade.GetMerchantAppZeus(ID);

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo_Risk1);
        WucBusinessInfo_Risk1.pnlInfo.Enabled = false;

        SqlDataReader dr = DataAccess.DataRiskDao.GetBatch(Convert.ToInt32(Request["BatchID"]));

        if (dr.Read())
        {
            txtSalesCount.Text = dr["SalesCount"].ToString();
            txtSalesAmount.Text = CommonUtility.Util.if_dec(dr["SalesAmount"], 0.0M).ToString("#,##0.00");
            txtCreditCount.Text = dr["CreditCount"].ToString();
            txtCreditAmount.Text = CommonUtility.Util.if_dec(dr["CreditAmount"], 0.0M).ToString("#,##0.00");

            txtDateVoided.Text = dr["DateVoided"].ToString();
            txtDateSuspended.Text = dr["DateSuspended"].ToString();
            txtDateHeld.Text = dr["DateHeld"].ToString();
            txtDateReleased.Text = dr["DateReleased"].ToString();

            txtMTDVolume.Text = CommonUtility.Util.if_dec(dr["MTD Volume"],0.0M).ToString("#,##0.00");
            txtFirstBatchDate.Text = dr["FirstBatch"].ToString();
            txtMaxDebit.Text = CommonUtility.Util.if_dec(dr["MaxBatchAmount"], 0.0M).ToString("#,##0.00");
            txtMaxDebitDate.Text = dr["MaxBatchDate"].ToString();
            txtMaxCredit.Text = CommonUtility.Util.if_dec(dr["MinBatchAmount"], 0.0M).ToString("#,##0.00");
            txtMaxCreditDate.Text = dr["MinBatchDate"].ToString();
            txtVoilations.Text = dr["Violations in last 30 days"].ToString();

            BatchStatus.Text = dr["BatchStatus"].ToString();

            TextBox txtDateApproved = (TextBox)WucBusinessInfo_Risk1.FindControl("DateApproved");
            txtDateApproved.Text = dr["DateApproved"].ToString();

            TextBox TinfoAverageMonthlyVMCVolume = (TextBox)WucBusinessInfo_Risk1.FindControl("TinfoAverageMonthlyVMCVolume");
            TinfoAverageMonthlyVMCVolume.Text = TinfoAverageMonthlyVMCVolume.Text;//"$" + 

            TextBox TinfoAverageVMCTicket = (TextBox)WucBusinessInfo_Risk1.FindControl("TinfoAverageVMCTicket");
            TinfoAverageVMCTicket.Text = TinfoAverageVMCTicket.Text;//"$" + 

            TextBox TinfoHighestTicketAmount = (TextBox)WucBusinessInfo_Risk1.FindControl("TinfoHighestTicketAmount");
            TinfoHighestTicketAmount.Text = TinfoHighestTicketAmount.Text;// "$" + 
        }

        dr.Close();
        UserSessions.CurrentMerchantApp = agreement;

    }

    protected void odsRisk_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        string role = string.Empty;

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        prms.Add("@MerchantAppUID", Request["MerchantAppUID"].ToString());
        prms.Add("@BatchID", Request["BatchID"].ToString());

        if (BatchExceptions.SelectedIndex != 0)
            prms.Add("@RiskID", BatchExceptions.SelectedItem.Value);

        if (StatusID.SelectedIndex != 0)
            prms.Add("@StatusIDList", StatusID.SelectedItem.Value);

        if (lstTransTypeID.SelectedIndex != 0)
            prms.Add("@TransTypeID", lstTransTypeID.SelectedItem.Value);

        if (CurrentBatchOnly.Checked)
            prms.Add("@CurrentBatchID", Request["BatchID"].ToString());

        if (prms.Count > 0)
        {
            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);

            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "TransID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grd.ID;
            lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetRiskBatchDetailsPagingRowCount(prms, 0, 0, this.grd.ID).ToString();
        }
        else
        {
            // we do this so that we don't call the SP with zero paramters. it's very slow otherwise.
            e.Cancel = true;
        }

    }
    
    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void lnkTransID_Click(object sender, EventArgs e)
    {
        string tranID, zid = "";
        if (((LinkButton)sender).Text != string.Empty)
        {
            tranID = ((LinkButton)sender).Text;
            if(UserSessions.CurrentMerchantApp != null)
            zid = UserSessions.CurrentMerchantApp.ID;
            pnlTransaction.LoadGrid(Convert.ToInt32(tranID));
            DataCreditCardTransaction facade = new DataCreditCardTransaction();
            CreditCardTransaction trans = facade.GetCCTransaction(zid, Convert.ToInt64(tranID));

            if (trans == null)
            {
                return;
            }
            FormBinding.BindObjectToControls(trans, pnlTransaction);

            Label Amount = (Label)FormBinding.FindFormControl(pnlTransaction, "Amount");
            Amount.Text =  Math.Round(trans.Amount, 2).ToString();//"$" +

            Label status = (Label)FormBinding.FindFormControl(pnlTransaction, "StatusName");


            status.Text = trans.StatusName;
            if (trans.StatusID == 0)
                status.ForeColor = System.Drawing.Color.Green;
            else
                status.ForeColor = System.Drawing.Color.Red;

            pnlTransaction.ToggleButtons();
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

        }
    }

}
