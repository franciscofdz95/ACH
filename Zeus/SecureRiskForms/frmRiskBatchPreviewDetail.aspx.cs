using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

public partial class frmRiskBatchPreviewDetail : frmBaseDataEntry
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
            string UID = Request["MerchantAppUID"].ToString();
            this.FormShow(UID);
            LoadGrid();
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
            grd.PageSize = 5000;
        else
            grd.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);

        LoadGrid();
        FormHandler.Export2Excel("Risk Batch Summary.xls", grd);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
            grd.PageSize = 5000;
        else
            grd.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        LoadGrid();
        FormHandler.ExportToPDF(grd, true, "Risk Batch Summary.xls");
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd.PageIndex = e.NewPageIndex;
        LoadGrid();
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        LoadGrid();
        DataTable dataTable = ((DataView)grd.DataSource).Table;
        if (dataTable != null)
        {
            DataView dataView = new DataView(dataTable);
            if (CurrentExp != e.SortExpression)
                CurrentSort = SortDirection.Ascending;
            dataView.Sort = e.SortExpression + " " + ConvertSortDirectionToSql(CurrentSort);
            grd.DataSource = dataView;
            grd.DataBind();
            CurrentExp = e.SortExpression;
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        LoadGrid();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        grd.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        LoadGrid();
    }

    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
        string newSortDirection = String.Empty;
        switch (sortDirection)
        {
            case SortDirection.Ascending:
                newSortDirection = "ASC";
                CurrentSort = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                newSortDirection = "DESC";
                CurrentSort = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    public SortDirection CurrentSort
    {
        get
        {
            if (ViewState["sortDir"] == null)
            {
                return SortDirection.Ascending;
            }
            return (SortDirection)ViewState["sortDir"];
        }
        set { ViewState["sortDir"] = value; }
    }

    public string CurrentExp
    {
        get
        {
            if (ViewState["sortExp"] == null)
            {
                return "MID";
            }
            return ViewState["sortExp"].ToString();
        }
        set { ViewState["sortExp"] = value; }
    }

    private void LoadGrid()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", Request["MerchantAppUID"].ToString());
        DataSet ds = DataAccess.DataRiskDao.GetBatchPreviewDetails(prms);
        grd.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Text);
        DataView dv = ds.Tables[0].DefaultView; 
        lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();

        //Bind grid
        grd.DataSource = dv;
        grd.DataBind();

        pnl.Visible = (grd.Rows.Count > 0);
        noRecords.Visible = !(grd.Rows.Count > 0);
    }

    public override void FormShow(string ID)
    {
        
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);
        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        WucBusinessInfo1.MyMerchantApp = agreement;
        WucBusinessInfo1.pnlInfo.Enabled = false;
        UserSessions.CurrentMerchantApp = agreement;
    }

    protected void lnkTransID_Click(object sender, EventArgs e)
    {
        string tranID, zid = "";
        if (((LinkButton)sender).Text != string.Empty)
        {
            tranID = ((LinkButton)sender).Text;
            if (UserSessions.CurrentMerchantApp != null)
                zid = UserSessions.CurrentMerchantApp.ID;
            pnlTransaction.LoadGrid(Convert.ToInt32(tranID));
            //CreditCardTransactionFacade facade = new CreditCardTransactionFacade();
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
}
