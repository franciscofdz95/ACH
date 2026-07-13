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
using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using ZeusWeb.Extensions;
using OfficeOpenXml;

public partial class frmMerchantStatements : frmBaseDataEntry
{
    private bool _CheckAvailable { get; set; }
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Statements);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Statements");
            }

            ctrlStatements.LoadStatements(UserSessions.CurrentMerchantApp.SettlePlatformMid, ConfigurationManager.AppSettings.Get("StatementsPath"), "~/SecureMerchantManagementForms/frmStatementsPreview.aspx", UserSessions.CurrentMerchantApp?.BusinessDBAName);

            this.LoadStatements();
            this.BillingConfigureStatementGridView();
            this.FormShow(this.UID);
        }
    }

    public void LoadStatements()
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        DataSet ds = null;

        ds = data.GetMeritusStatements(int.Parse(UserSessions.CurrentMerchantApp.ID));

        grdStatements.DataSource = ds;
        grdStatements.DataBind();
    }
    public bool HasAccountingRole()
    {
        bool _exist = false;
        foreach (var _item in UserSessions.CurrentUser.UserRoles)
        {
            if (_item.Value.RoleID.ToUpper() == Constants.ROLE_ACCOUNTING) // Accounting Role ID
            {
                _exist = true;
                break;
            }
        }
        return _exist;
    }

    protected void grdStatements_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink link = (HyperLink)e.Row.Cells[2].Controls[0];

            if (link.Text.ToUpper() == "0")
            {
                e.Row.Visible = false;
            }
            else
            {
                link.NavigateUrl = String.Format("~/SecureMerchantManagementForms/frmPaysafeStatement.aspx?MerchantAppUID={0}&BillDate={1}", UserSessions.CurrentMerchantApp.MerchantAppUID, link.Text);
                link.Text = "View";
            }
        }
    }

    public override void FormShow(string ID)
    {
        WucBusinessInfo1.pnlInfo.Enabled = false;
        FormBinding.BindObjectToControls(UserSessions.CurrentMerchantApp, WucBusinessInfo1);

        MerchantApp app = UserSessions.CurrentMerchantApp;
        WucBusinessInfo1.LoadOffice(app);

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

    #region BillingRDRStatements
    public void BillingConfigureStatementGridView()
    {
        this.BillingRDRStatementGrid.DefaultWithPager(Convert.ToInt32(cboPageSize.SelectedItem.Value));

        BillingDateFrom.MinValue = DateTime.Now.AddYears(-1).Date;
        BillingDateFrom.MaxValue = DateTime.Now.Date;
        BillingDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;

        BillingDateTo.MinValue = DateTime.Now.AddYears(-1).Date;
        BillingDateTo.MaxValue = DateTime.Now.Date;
        BillingDateTo.Value = DateTime.Now.Date;

        this.LoadRDRStatements(0);
    }

    public void LoadRDRStatements(int index)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@Page", index + 1);
        prms.Add("@Size", BillingRDRStatementGrid.PageSize);
        prms.Add("@MID", UserSessions.CurrentMerchantApp.SettlePlatformMid);
        prms.Add("@DateFrom", BillingDateFrom.Date);
        prms.Add("@DateTo", BillingDateTo.Date);
        prms.Add("@InsightVisible", 0);

        DataSet ds = DataCCBatchDeposits.GetRDRBillingStatement(prms);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) //FIXED: prevent error if not get tables
        {
            lblRecordCount.Text = ds.Tables[0].Rows[0]["Index"].ToString();
            DataTable dt = ds.Tables[0];
            BillingRDRStatementGrid.SetDataSource(dt, index, "Index");
        }
        else
        {
            lblRecordCount.Text = "0";
            BillingRDRStatementGrid.DataSource = null;
            BillingRDRStatementGrid.DataBind();
        }
        // The following line is to validate if the user has permissions to edit Statements Table
        //_CheckAvailable = HasAccountingRole();
    }
    protected void Billing_OnPageIndexChangingStatement(object sender, GridViewPageEventArgs e)
    {
        BillingRDRStatementGrid.PageIndex = e.NewPageIndex;
        LoadRDRStatements(BillingRDRStatementGrid.PageIndex);
    }

    protected void Billing_OnRowDataBoundStatement(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell statusCell = e.Row.Cells[10]; // BillingMethod 
            TableCell TotalCell = e.Row.Cells[9]; // Total
            TableCell RateCell = e.Row.Cells[8]; // RATE
            TableCell VolumeCell = e.Row.Cells[7]; // Volume

            if (VolumeCell.Text == "0")
            {
                VolumeCell.Text = string.Empty;
            }
            if (RateCell.Text == "$0.00")
            {
                RateCell.Text = string.Empty;
            }

            CheckBox c = e.Row.FindControl("billingChkStatus") as CheckBox;
            Label lbl = e.Row.FindControl("billingLblStatus") as Label;

            if (c != null && lbl != null)
            {
                c.Checked = (lbl.Text == "True");
                c.Enabled = HasAccountingRole();
            }

            if (statusCell.Text == "Bank Debit")
            {
                statusCell.ToolTip = "Funds debited from the bank account on file";
            }
            if (statusCell.Text == "Reserve Debit")
            {
                statusCell.ToolTip = "Funds debited from your reserve account";
            }

            if (statusCell.Text == "AR Debit")
            {
                statusCell.ToolTip = "Added to outstanding balance due";
            }

            if (statusCell.Text == "Bank Credit")
            {
                statusCell.ToolTip = "Funds credited to the bank account on file";
                TotalCell.Text = "(" + TotalCell.Text + ")";
                TotalCell.Attributes.Add("style", "color: red");
            }

            if (statusCell.Text == "Reserve Credit")
            {
                statusCell.ToolTip = "Funds credited to your reserve account";
                TotalCell.Text = "(" + TotalCell.Text + ")";
                TotalCell.Attributes.Add("style", "color: red");
            }

            if (statusCell.Text == "AR Credit")
            {
                statusCell.ToolTip = "Funds credited towards outstanding balance due";
                TotalCell.Text = "(" + TotalCell.Text + ")";
                TotalCell.Attributes.Add("style", "color: red");
            }
        }
    }
    protected void BillingOnSelectedIndexChanged(object sender, EventArgs e)
    {
        BillingRDRStatementGrid.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        BillingRDRStatementGrid.PageIndex = 0; //FIXED: prevent blank page when change page size
        LoadRDRStatements(BillingRDRStatementGrid.PageIndex);
    }

    protected void btnBillingExpExcel_Click(object sender, EventArgs e)
    {
        string file = string.Empty;
        file = "Statements " + DateTime.Now.ToString().Replace("/", "-") + ".xls";
        Hashtable prms = new Hashtable();
        if (rdExport.SelectedValue == "1")
        {
            prms.Add("@Page", 0);
            prms.Add("@Size", PaymentXP.BusinessObjects.Constants.MAX_EXPORT_ROW_LIMIT);
        }
        else
        {
            prms.Add("@Page", BillingRDRStatementGrid.PageIndex + 1);
            prms.Add("@Size", BillingRDRStatementGrid.PageSize);
        }

        prms.Add("@MID", UserSessions.CurrentMerchantApp.SettlePlatformMid);
        prms.Add("@DateFrom", BillingDateFrom.Date);
        prms.Add("@DateTo", BillingDateTo.Date);
        //DataSet ds = DataCCBatchDeposits.GetRDRBillingStatement(prms);

        //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ///////////////////////
        using (ExcelPackage pck = new ExcelPackage())
        {
            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Statement");

            DataSet ds = DataCCBatchDeposits.GetRDRBillingStatement(prms);

            string[,] liHeaders = new string[9, 3] {
                        {"DateBillingAdjustment", "Date of Billing Adjusment", "Date"},
                        {"BillingPeriodFrom", "Billing Period Form", "Date"},
                        {"BillingDateTo", "Billing Date To", "Date"},
                        {"Product", "Product", "string"},
                        {"Description", "Description", "string"},
                        {"Volume", "Volume", "integer"},
                        {"Rate", "Rate", "currency"},
                        {"Total", "Total", "currency"},
                        {"BillingMethod", "Billing Method", "string"}
                    };

            DataTable dt = GridViewExportUtil.GetExportableDataTable(ds.Tables[0], liHeaders);
            GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

            ws.Cells.LoadFromDataTable(dt, true);

            for (int i = 1; i < dt.Rows.Count + 2; i++)
            {
                if (ws.Cells[i, 9].Text.Contains("Credit"))
                {
                    ws.Cells[i, 8].Style.Numberformat.Format = "[Red] ($#,##0.00) ; [Red] ($#,##0.00)";
                }
            }
            ws.Cells[dt.Rows.Count + 3, 1].LoadFromText("* AR = Accounts Receivable");
            ws.Cells[dt.Rows.Count + 4, 1].LoadFromText("Billing events occurring on the same day may be grouped together on your bank statement");
            Response.Clear();   // necessary!!!
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + file);
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!
        }
    }
    protected void btnBillingSearch_Click(object sender, EventArgs e)
    {
        LoadRDRStatements(BillingRDRStatementGrid.PageIndex);
    }

    protected void btnBillingReset_Click(object sender, EventArgs e)
    {
        BillingDateFrom.MinValue = DateTime.Now.AddYears(-1).Date;
        BillingDateFrom.MaxValue = DateTime.Now.Date;
        BillingDateFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;

        BillingDateTo.MinValue = DateTime.Now.AddYears(-1).Date;
        BillingDateTo.MaxValue = DateTime.Now.Date;
        BillingDateTo.Value = DateTime.Now.Date;

        LoadRDRStatements(0);
    }

    protected void billing_ChangedStatus(Object sender, EventArgs args)
    {
        CheckBox _checkCurrent = sender as CheckBox;
        bool itemState = _checkCurrent.Checked;
        GridViewRow _row = _checkCurrent.NamingContainer as GridViewRow;
        Label lbl = _row.FindControl("billingLblID") as Label;

        Label lblInvoice = _row.FindControl("billingLblInvoiceID") as Label; //DM-3141 by Jorge  
        Hashtable prms = new Hashtable();
        prms.Add("@Id", lbl.Text);
        prms.Add("@InvoiceId", lblInvoice.Text); //DM-3141 by Jorge 
        prms.Add("@Status", itemState);
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        data.UpdateBillingInsightVisible(prms);

    }
    #endregion
}
