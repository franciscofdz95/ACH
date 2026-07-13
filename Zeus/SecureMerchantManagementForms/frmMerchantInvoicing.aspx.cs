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

public partial class frmMerchantInvoicing : frmBaseDataEntry
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

    public bool IsAccountant
    {
        get { return Convert.ToBoolean(ViewState["IsAccountant"]); }
        set { ViewState["IsAccountant"] = value; }
    }

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

        grdACH.DataSourceSelectCountMethod = "GetACHTransactionsPagingRowCount";
        grdACH.DataSourceSelectMethod = "GetACHTransactionsPaging";
        WebUtil.SetUserSpecificDisplayMode(txtPostDate);
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Invoicing);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Invoicing");
            }

            LookupTableHandler.LoadMerchantAdjustmentFeeItems(this.cboCategory, false);
            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            this.FormShow(this.UID);
            //this.LoadInvoices();
            WucBusinessInfo1.pnlInfo.Enabled = false;
        }
    }



    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();  
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);


        this.MerchantAppUID = agreement.MerchantAppUID;
        this.MerchantAppZID = agreement.ID;
        this.AccountNumber = agreement.AccountNumber;
        this.RoutingNumber = agreement.RoutingNumber;

        this.LoadInvoices();
        this.IsCurrentUserAccountant();
        
       
        WucBusinessInfo1.LoadOffice(agreement);
        
    }

        
    private void LoadInvoices()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@ClientID", UserSessions.CurrentMerchantApp.ID);
        prms.Add("@AchID", ConfigurationManager.AppSettings["AggregatorAchID"]);

        grdACH.SetDataSource(prms, Convert.ToInt32(10));

        //Load ACH activities
        //Hashtable prms = new Hashtable();

        //prms.Add("@ClientID", this.MerchantAppZID);
        //prms.Add("@BeginPostedDate", UserSessions.CurrentMerchantApp.DateCreated);
        //AchTransactionFacade facade2 = new AchTransactionFacade();
        //DataSet ds = facade2.GetAchTransactions(ConfigurationManager.AppSettings["AggregatorAchMerchantID"], prms); //Aggregate ACH account for all merchants

        //grdRisk.DataSource = ds;
        //grdRisk.DataBind();

        //lblACH.Visible = (grdRisk.Rows.Count == 0);
    }

    #region IsCurrentUserAccountant
        //User Story-10922
        //Here we are checking if currently logged in user is working into the role of accounting or not. If user is not in accounting role then we don't have to allow them to add invoice for the selected merchant.
        //Here we have put the source code logic to disable the Add Invoice button if currently logged in user does not have the role to work for accounting.
        private void IsCurrentUserAccountant()
        {
            Hashtable prms = new Hashtable();
            prms.Add("@UserUID", UserSessions.CurrentUser.UID);
            prms.Add("@RoleUID", "8AA21143-7A97-461C-AAA3-99482DB75876");

            UserFacade objUserFacade = new UserFacade();
            IsAccountant = objUserFacade.IsUserInAccountingRole(prms);
            this.ToggleButtons();
        }

    #endregion

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false; ;

        if (this.CreateAchTransaction())
        {
            this.txtAmount.Value = 0;
            cboCategory.SelectedIndex = 0;
            cboType.SelectedIndex = 0;

            this.EditMode = false;
            this.Adding = false;
            this.LoadInvoices();
            this.ToggleButtons();
        }
        return true;
    }

    public bool CreateAchTransaction()
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
        trans.AccountName = UserSessions.CurrentMerchantApp.BusinessDBAName;
        trans.StatusID = 0;
        trans.NextProcessDate = DateTime.Now.ToString();
        trans.Amount = DataLayer.Decimal2Field(txtAmount.Value);
        trans.ReferenceNumber = txtRefID.Text;
        trans.UserCreated = UserSessions.CurrentUser.UserName;

        DataAchTransaction data = DataAccess.DataAchTransactionDao;
        bool perform = data.AddTransaction(trans);

        if (trans.TransactionResponse.TransID != "-1")
        {
            return true;
        }
        else
        {
            return false;
        }
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
        string message = string.Empty;

        if (this.RoutingNumber == string.Empty)
            this.Master.AddMessageError("Routing Number is missing in the profile.");

        if (this.RoutingNumber == string.Empty)
            this.Master.AddMessageError("Account Number is missing in the profile.");

        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            //lblError.Text = message;
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
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;

        pnlGrid.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);
        
        if (IsAccountant.Equals(false))
        {
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Add Invoice":
                IsAccountant = true;
                this.FormNew();
                break;
            case "Save":
                if (this.FormDataCheck())  // valid input values.
                {
                    if (this.FormSave())
                    {
                        url = "~/SecureMerchantManagementForms/frmMerchantInvoicing.aspx?Adding=false&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                        Response.Redirect(url);
                    }
                }
                break;
            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                this.FormShow(this.UID);

                break;
            case "Cancel":
                this.FormCancel();

                break;

            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
                break;
            case "Edit":
                //if (grdRisk.SelectedIndex != -1)
                //{
                //    this.EditMode = true;
                //    this.FormShow(this.UID);
                //    this.ToggleButtons();
                //}
                break;
            //case "<<":
            //    FormHandler.RecordNav(RecordNavigation.First, this, UserSessions.SearchResultsDataView, false);
            //    break;
            //case "<":
            //    FormHandler.RecordNav(RecordNavigation.Previous, this, UserSessions.SearchResultsDataView, false);
            //    break;
            //case ">":
            //    FormHandler.RecordNav(RecordNavigation.Next, this, UserSessions.SearchResultsDataView, false);
            //    break;
            //case ">>":
            //    FormHandler.RecordNav(RecordNavigation.Last, this, UserSessions.SearchResultsDataView, false);
            //    break;
        }
    }

    //private void DoVoid(string tranid)
    //{
    //    try
    //    {
    //        AchTransactionFacade facade = new AchTransactionFacade();
    //        facade.VoidTransaction(Convert.ToInt64(tranid));
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //private void DoResubmit(string tranid)
    //{
    //    try
    //    {
    //        long NewTransID = -1;
    //        AchTransactionFacade facade = new AchTransactionFacade();
    //        facade.AddTransactionResubmit(ref NewTransID, Convert.ToInt64(tranid));
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //protected void grdRisk_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    //Ach_Update_Transaction_Status_Cancel
    //    if (e.CommandSource is Button)
    //    {
    //        Button btn = (Button)e.CommandSource;
    //        GridViewRow grdRow = (GridViewRow)btn.NamingContainer;
    //        string tranID = grdRow.Cells[1].Text;
    //        grdRisk.SelectedIndex = grdRow.RowIndex;
    //        switch (btn.Text)
    //        {
    //            case "Void":
    //                this.DoVoid(tranID);
    //                this.LoadInvoices();
    //                break;
    //            case "Resubmit":
    //                this.DoResubmit(tranID);
    //                this.LoadInvoices();
    //                break;
    //        }
    //    }
    //}
    //protected void grdRisk_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    switch (e.Row.RowType)
    //    {
    //        case DataControlRowType.Header:
    //            break;
    //        case DataControlRowType.DataRow:

    //            Button btn = (Button)e.Row.FindControl("btnAction");
    //            string action = DataBinder.Eval(e.Row.DataItem, "Action").ToString();
    //            switch (action)
    //            {
    //                case "C":
    //                    btn.Text = "Credit";
    //                    btn.OnClientClick = "return confirm('Do you want to credit this transaction?');";
    //                    break;
    //                case "R":
    //                    btn.Text = "Resubmit";
    //                    btn.OnClientClick = "return confirm('Do you want to resubmit this transaction?');";
    //                    break;
    //                case "V":
    //                    btn.Text = "Void";
    //                    btn.OnClientClick = "return confirm('Do you want to void this transaction?');";
    //                    break;
    //                default:
    //                    btn.BorderStyle = BorderStyle.None;
    //                    btn.BorderWidth = new Unit("0px");
    //                    btn.BackColor = System.Drawing.Color.Transparent;
    //                    btn.Text = action == string.Empty ? "N/A" : action;
    //                    btn.Enabled = false;
    //                    btn.ToolTip = action;
    //                    btn.Width = new Unit("");
    //                    break;
    //            }
    //            break;
    //        case DataControlRowType.Footer:
    //           break;
    //        default:
    //            break;
    //    }
    //}
}
