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
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;

public partial class frmETF : frmBaseDataEntry
{
    public string MerchantAppZID
    {
        get { return ViewState["MerchantAppZID"].ToString(); }
        set { ViewState["MerchantAppZID"] = value; }
    }

    public string MerchantAppUID
    {
        get { return ViewState["MerchantAppUID"].ToString(); }
        set { ViewState["MerchantAppUID"] = value; }
    }

    public string RoutingNumber
    {
        get { return ViewState["RoutingNumber"].ToString(); }
        set { ViewState["RoutingNumber"] = value; }
    }

    public string AccountNumber
    {
        get { return ViewState["AccountNumber"].ToString(); }
        set { ViewState["AccountNumber"] = value; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        grdACH.DataSourceSelectCountMethod = "GetACHTransactionsPagingRowCount";
        grdACH.DataSourceSelectMethod = "GetACHTransactionsPaging";

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadMerchantClosureCodes(MerchantClosureCodeUID, true);
            LookupTableHandler.MerchantAppStatus(StatusUID, true, "ETF");
            LookupTableHandler.MerchantAppStatus(ETFStatusUID, false, "ETF");
            LookupTableHandler.LoadMerchantAdjustmentFeeItems(this.cboCategory, false);

            ListHandler.ListFindItem(cboCategory, "184");
            txtRefID.Text = "ETF";
            DataAccess.DataETFDao.LoadCancelledAccounts();
            grdStatus.PageIndex = 0;
            BindGrid();

            img1.Attributes.Add("onclick", "CollapseExpand('" + div1.ClientID + "',null,'" + img1.ClientID + "')");
        }

        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    private void BindGrid()
    {
        if (m_Prms == null)
            m_Prms = new Hashtable();

        if (m_Prms.Count == 0)
            m_Prms.Add("@ETFUID", "00000000-0000-0000-0000-000000000000");

        grd.DataBind();

        pnlRecords.Visible = grd.Rows.Count > 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;

        if (grd.SelectedIndex == -1)
        {
            pnlETF.Visible = false;
            lblQuick.Visible = false;
            this.UID = "";
        }
    }

    public void ClearGrid()
    {
        lblRecordCount.Text = "";
        //grd.Columns.Clear();
        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
        pnlETF.Visible = false;
        lblQuick.Visible = false;
        this.UID = "";
    }


    public override void FormShow(string ID)
    {
        BindGrid();
        DataETF data = DataAccess.DataETFDao;

        ETF objETF = data.GetETFMerchant(ID, "");
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(objETF.MerchantAppUID);

        WucBusinessInfo1.MyMerchantApp = agreement;
        

        FormBinding.BindObjectToControls(agreement, pnlETF);
        FormBinding.BindObjectToControls(objETF, pnlETF);

        SuggestedETF.Text = objETF.SuggestedETF.ToString("0.00");//"c");
        Balance.Text = objETF.Balance.ToString("0.00");//"c");
        ACHAmount.Value = 0.0M;

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        FormHandler.SetControlEditMode(pnlGrid, !this.EditMode);
        FormHandler.SetControlEditMode(WucBusinessInfo1, false);

        FormHandler.ShowClosureCodes(WucBusinessInfo1, agreement.StatusUID.ToUpper());

        btnCalculateETFPreview.Enabled = true;

        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == "8AA21143-7A97-461C-AAA3-99482DB75876") // Default role as Accounting
        {
            pnlApprove.Visible = true; 
            pnlAch.Enabled = true;

            if (this.EditMode)
            {
                // if youre accounting, and you hit edit mode, then default to early termination, and prepopulate the suggest etf.
                txtAmount.Text = ActualETF.Text;
                if (cboCategory.Items.FindByValue("EARLYTERM") != null)
                {
                    cboCategory.SelectedValue = "EARLYTERM";
                }
            }
        }
        else
        {
            pnlApprove.Visible = false; 
            pnlAch.Enabled = false;
        }

        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == "F8A171AB-A609-4125-BCB3-3362213B0366") // Default role as App Processing
        {
            //pnlApp.Visible = true;
            ActualETF.ReadOnly = false;
        }
        else
        {
            ActualETF.ReadOnly = true;
            //pnlApp.Visible = false;
        }

        this.MerchantAppUID = agreement.MerchantAppUID;
        this.MerchantAppZID = agreement.ID;
        this.AccountNumber = agreement.AccountNumber;
        this.RoutingNumber = agreement.RoutingNumber;

        LoadETFStatus();
        LoadInvoices();

        if (pnlApprove.Visible && this.EditMode)
        {
            btnApprove.Enabled = false;
            ActualETF.ReadOnly = true;
        }

        if (this.EditMode)
        {
            btnCalculate.Enabled = btnApprove.Enabled = !(objETF.ETFStatusUID.ToUpper() == Constants.ETF_SUBMIT_ACH || objETF.ETFStatusUID.ToUpper() == Constants.ETF_CLOSED || objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED);
            btnSubmit.Enabled = !(objETF.ETFStatusUID.ToUpper() == Constants.ETF_CLOSED || objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED);
            ActualETF.ReadOnly = (objETF.ETFStatusUID.ToUpper() == Constants.ETF_CLOSED || objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED);
            ACHAmount.ReadOnly = (objETF.ETFStatusUID.ToUpper() == Constants.ETF_CLOSED || objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED);
        }

        grdStatus.Enabled = true;
        Notes_Tickets.LoadNotes(agreement.MerchantAppUID, "");
    }

    private void LoadInvoices()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ClientID", this.MerchantAppZID);
        prms.Add("@AchID", ConfigurationManager.AppSettings["AggregatorAchID"]);

        grdACH.SetDataSource(prms, Convert.ToInt32(10));
    }

    private void ClearInvoice()
    {
        this.txtAmount.Value = 0;
        ListHandler.GetListItem(cboCategory, "184");
        cboType.SelectedIndex = 0;
        txtRefID.Text = "ETF";
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlGrid);
        ClearGrid();
        ClearInvoice();
    }

    public override bool FormSave()
    {
        bool perform = false;
        try
        {
            if (!this.FormDataCheck())
                return false;


            ETF objETF = new ETF();
            DataETF data = DataAccess.DataETFDao;
            objETF = data.GetETFMerchant(this.UID, "");

            FormBinding.BindControlsToObject(objETF, pnlETF);

            if (objETF.Balance == 0.0M && objETF.AvgMonthlyFees != objETF.ActualETF)
                objETF.Balance = objETF.ActualETF;
            else
                objETF.Balance = objETF.Balance - decimal.Parse(ACHAmount.Value.ToString());

            objETF.AvgMonthlyFees = objETF.ActualETF - objETF.Balance;

            ACHAmount.Value = 0;
            User user = UserSessions.CurrentUser;
            objETF.UserUpdated = user.UserName;
            objETF.DateUpdated = DateTime.Now;
            objETF.SuggestedETF = DataLayer.Field2Dec(SuggestedETF.Text.Replace("$", "").Trim());

            //if (objETF.Balance == 0.0M && objETF.ActualETF != 0.0M && objETF.ActualETF == objETF.AvgMonthlyFees)
            //    objETF.ETFStatusUID = Constants.ETF_CLOSED;

            if (objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED)
            {
                objETF.SuggestedETF = 0.0M;
                objETF.ActualETF = 0.0M;
            }

            data.UpdateETFMerchant(objETF);
            this.UID = objETF.UID;

            if (objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED)
            {
                MerchantFacade facade = new MerchantFacade();
                MerchantApp app = facade.GetMerchantAppZeus(objETF.MerchantAppUID);
                app.ETFAssessed = "W";
                app.UserUpdated = UserSessions.CurrentUser.UserName;

                
                facade.UpdateMerchantApp(app);
            }

            perform = true;
        }
        catch (Exception exc)
        {
            throw exc;
        }

        if (perform)
        {
            this.EditMode = false;
            this.Adding = false;
            this.ToggleButtons();
            this.FormShow(this.UID);
        }
        else
        {
            lblError2.Text = "Record failed to save.";
        }

        return perform;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        this.ToggleButtons();
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {

        if (ETFStatusUID.SelectedValue == "-1")
           wucMessage1.AddMessageError("Enter ETF Status.");
        else if (ETFStatusUID.SelectedValue.ToUpper() != Constants.ETF_ETF_WAIVED)
        {
            if (ContractMonths.Text.Trim() == string.Empty || Convert.ToInt32(ContractMonths.Text) <= 0)
                wucMessage1.AddMessageError("Enter Contract Months.");

            if (ActualETF.Text.Trim() == string.Empty || Convert.ToDecimal(ActualETF.Text.Replace("$", "").Replace("(", "").Replace(")", "")) <= 0.0M)
                wucMessage1.AddMessageError("Enter the Actual ETF.");
        }

        if (wucMessage1.ErrorCount() == 0)
        {
            return true;
        }
        else
        {            
            return false;
        }
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnCancel.Enabled = !btnCancel.Enabled;
        btnEdit.Enabled = !btnEdit.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
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

    public string CreateAchTransaction(MerchantApp objMA)
    {
        ArrayList prms = new ArrayList();

        AchTransaction trans = new AchTransaction();
        trans.PostedDate = DateTime.Now;
        trans.MerchantID = ConfigurationManager.AppSettings["AggregatorAchMerchantID"];
        trans.OriginID = 29;
        trans.ClientID = this.MerchantAppZID;
        trans.Description = cboCategory.SelectedItem.Value.Trim();
        trans.CompanyName = "Paysafe Merchant Invoicing";

        if (cboType.SelectedItem.Value == "27")
            trans.TransTypeID = TransTypeID.ACHCheckingDebit;
        else
            trans.TransTypeID = TransTypeID.ACHCheckingCredit;

        trans.Secc = "CCD";
        trans.RoutingNumber = this.RoutingNumber;
        trans.AccountNumber = this.AccountNumber;
        trans.AccountName = objMA.BusinessDBAName;
        trans.StatusID = 0;
        trans.NextProcessDate = DateTime.Now.ToString();
        trans.Amount = DataLayer.Decimal2Field(txtAmount.Value);
        trans.ReferenceNumber = txtRefID.Text;
        trans.UserCreated = UserSessions.CurrentUser.UserName;

        DataAchTransaction data = DataAccess.DataAchTransactionDao;
        bool perform = data.AddTransaction(trans);

        if (trans.TransactionResponse.TransID != "-1")
        {
            return trans.TransactionResponse.TransID;
        }
        else
        {
            return string.Empty;
        }
    }

    private void LoadETFStatus()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ETFUID", this.UID);
        DataSet ds = DataAccess.DataETFDao.GetETFStatusHistory(prms);

        grdStatus.DataSource = ds;
        grdStatus.DataBind();
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        string url = string.Empty;

        switch (btn.Text)
        {
            case "Add":
                this.FormNew();
                break;

            case "Save":
                this.FormSave();
                break;

            case "Refresh":
                if (!string.IsNullOrEmpty(this.UID))
                    this.FormShow(this.UID);
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Close":
                break;

            case "Delete":
                this.FormDelete();
                break;

            case "Edit":
                if (!string.IsNullOrEmpty(this.UID))
                {
                    this.EditMode = true;
                    this.FormShow(this.UID);
                    this.ToggleButtons();
                }
                else
                {
                    wucMessage1.AddMessageError("Select an item from the search results.");
                }
                break;
        }
    }

    protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
    }

    protected void odsEtf_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        DataETF data = DataAccess.DataETFDao;
        if (m_Prms == null)
            m_Prms = new Hashtable();

        if (m_Prms.Contains("@PageSize"))
            m_Prms["@PageSize"] = this.PageSize;
        else
            m_Prms.Add("@PageSize", this.PageSize);

        if (m_Prms.Contains("@CurrentPage"))
            m_Prms["@CurrentPage"] = this.CurrentPage;
        else
            m_Prms.Add("@CurrentPage", this.CurrentPage);

        if (m_Prms.Contains("@SortOrder"))
            m_Prms["@SortOrder"] = this.SortOrder;
        else
            m_Prms.Add("@SortOrder", this.SortOrder);

        if (m_Prms.Contains("@SortDirection"))
            m_Prms["@SortDirection"] = ConvertSortDirectionToSql(this.SortDirectionSearch);
        else
            m_Prms.Add("@SortDirection", ConvertSortDirectionToSql(this.SortDirectionSearch));


        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        e.InputParameters[0] = m_Prms;
        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetETFMerchantsPagingRowCount(m_Prms, 0, 0).ToString();
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
            LinkButton lnkUID = (LinkButton)e.Row.FindControl("lnkUID");
            lnkUID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "UID").ToString();

            if (DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "FirstTeam")))
                e.Row.BackColor = System.Drawing.Color.LightBlue;

            e.Row.Cells[10].Attributes.Add("title", Server.HtmlDecode(e.Row.Cells[10].Text));
            e.Row.Cells[10].Text = "<div style='width:100px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;'><NOBR>" + Server.HtmlDecode(e.Row.Cells[10].Text) + "</NOBR></div>";
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;

        string uid = e.CommandArgument.ToString();
        if (e.CommandName == "UID")
        {
            this.UID = uid;
            this.MerchantAppZID = grd.Rows[((GridViewRow)lnk.NamingContainer).RowIndex].Cells[1].Text;
            lblQuick.Visible = pnlETF.Visible = true;

            grd.SelectedIndex = ((GridViewRow)lnk.NamingContainer).RowIndex;
            ClearInvoice();
            this.FormShow(this.UID);
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.BindGrid();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        BindGrid();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        m_Prms = new Hashtable();

        if (txtZID.Text != string.Empty)
            m_Prms.Add("@ZID", txtZID.Text);

        if (txtDBA.Text != string.Empty)
            m_Prms.Add("@DBAName", txtDBA.Text);

        if (StatusUID.SelectedIndex > 0)
            m_Prms.Add("@StatusUID", StatusUID.SelectedValue);

        if (MerchantClosureCodeUID.SelectedIndex > 0)
            m_Prms.Add("@ClosureCodeUID", MerchantClosureCodeUID.SelectedValue);

        if (wucAgentSelector.m_AgentUID != string.Empty)
            m_Prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            m_Prms.Add("@BeginDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            m_Prms.Add("@EndDate", SearchEndDate.Value);



        grd.SelectedIndex = -1;
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.FormClear();
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        ETF objETF = new ETF();
        DataETF data = DataAccess.DataETFDao;
        objETF = data.GetETFMerchant(this.UID, "");

        FormBinding.BindControlsToObject(objETF, pnlETF);

        User user = UserSessions.CurrentUser;
        objETF.UserUpdated = user.UserName;
        objETF.DateUpdated = DateTime.Now;
        objETF.ETFApprovedDate = DateTime.Now;

        if (ActualETF.Text.Trim() == string.Empty || Convert.ToDecimal(ActualETF.Text.Replace("$", "").Replace("(", "").Replace(")", "")) <= 0.0M)
            wucMessage1.AddMessageError("Enter the actual etf.");
        else
        {
            if (objETF.ETFStatusUID.ToUpper() != Constants.ETF_MANAGER_APPROVED && objETF.ETFStatusUID.ToUpper() != Constants.ETF_EXECUTIVE_APPROVED)
                wucMessage1.AddMessageError("Please select approved status.");
            else
            {
                if (objETF.Balance == 0.0M && objETF.AvgMonthlyFees != objETF.ActualETF)
                    objETF.Balance = objETF.ActualETF;
                else
                    objETF.Balance = objETF.Balance - decimal.Parse(ACHAmount.Value.ToString());

                objETF.AvgMonthlyFees = objETF.ActualETF - objETF.Balance;
                ACHAmount.Value = 0;
                data.UpdateETFMerchant(objETF);
                this.UID = objETF.UID;

                FormShow(this.UID);

                btnApprove.Enabled = false;
                ActualETF.ReadOnly = true;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ETF objETF = new ETF();
        DataETF data = DataAccess.DataETFDao;
        objETF = data.GetETFMerchant(this.UID, "");

        FormBinding.BindControlsToObject(objETF, pnlETF);

        User user = UserSessions.CurrentUser;
        objETF.UserUpdated = user.UserName;
        objETF.DateUpdated = DateTime.Now;
        objETF.ETFApprovedDate = DateTime.Now;

        if (ACHAmount.Text.Trim() == string.Empty || Convert.ToDecimal(ACHAmount.Text.Replace("$", "").Replace("(", "").Replace(")", "")) <= 0.0M)
            wucMessage1.AddMessageError("Enter the ACH Amount.");
        else
        {
            if (objETF.Balance == 0.0M && objETF.AvgMonthlyFees != objETF.ActualETF)
                objETF.Balance = objETF.ActualETF;
            else
                objETF.Balance = objETF.Balance - decimal.Parse(ACHAmount.Value.ToString());

            objETF.AvgMonthlyFees = objETF.ActualETF - objETF.Balance;

            objETF.ETFStatusUID = Constants.ETF_SUBMIT_ACH;

            ACHAmount.Value = 0;
            data.UpdateETFMerchant(objETF);
            this.UID = objETF.UID;
            FormShow(this.UID);
        }
    }

    protected void btnCalculateETFPreview_Click(object sender, EventArgs e)
    {
        DataETF data = new DataETF();
        decimal etf = 0.0M;

        if (!string.IsNullOrEmpty(txtZIDPreview.Text) && !string.IsNullOrEmpty(txtContractMonthsPreview.Text) )
            etf = data.GetSuggestedEtf(Convert.ToInt32(txtZIDPreview.Text), Convert.ToInt32(txtContractMonthsPreview.Text.Trim()));


        lblCalculateETFPreview.Text = etf.ToString("0.00");//"c");
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        DataETF data = new DataETF();
        decimal etf = 0.0M;

        if (MerchantAppZID != string.Empty && ContractMonths.Text.Trim() != string.Empty)
            etf = data.GetSuggestedEtf(Convert.ToInt32(MerchantAppZID), Convert.ToInt32(ContractMonths.Text.Trim()));

        FormShow(this.UID);
        SuggestedETF.Text = etf.ToString("0.00");//"c");
    }

    protected void SuggestedETF_Click(object sender, EventArgs e)
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

        DataSet ds = new DataSet();
        ds = DataAccess.DataETFDao.GetETFDetails(this.MerchantAppUID);

        if (ds.Tables.Count > 0)
        {
            grdDetails.DataSource = ds;
            grdDetails.DataBind();            
        }
    
    }

    protected void btnAddinvoice_Click(object sender, EventArgs e)
    {
        string TransID = string.Empty;

        if (this.RoutingNumber == string.Empty)
            lblError2.Text = "Routing Number is missing in the profile.\n";

        if (this.RoutingNumber == string.Empty)
            lblError2.Text += "Account Number is missing in the profile.\n";

        if (cboCategory.SelectedIndex <= 0)
            lblError2.Text += "Select Category.\n";

        if (txtAmount.Value == null)
            lblError2.Text += "Enter Amount.";

        if (lblError2.Text.Trim() == "")
        {
            MerchantFacade facade = new MerchantFacade();
            MerchantApp ma = facade.GetMerchantAppZeus(this.MerchantAppUID);
            if (ma != null)
            {
                TransID = this.CreateAchTransaction(ma);
                if (TransID != string.Empty)
                {
                    ClearInvoice();
                    this.LoadInvoices();

                    DataETF data = DataAccess.DataETFDao;
                    ETF objETF = data.GetETFMerchant(this.UID, "");

                    objETF.TransID = TransID;
                    data.UpdateETFMerchant(objETF);
                }
            }
            else
            {
                lblError2.Text = "Could not get merchant.\n";
            }
        }
    }

    protected void grdStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdStatus.PageIndex = e.NewPageIndex;
        LoadETFStatus();
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

        BindGrid();
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("ETFReport.xls", grd);
    }

}
