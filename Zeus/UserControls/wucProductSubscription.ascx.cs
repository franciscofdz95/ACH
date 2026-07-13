using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Infragistics.WebUI.WebSchedule;
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebHtmlEditor;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.Web.UI.EditorControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.WebControls;
using PaymentXP.DataObjects.Reporting;
using System.Data;
using PaymentXP.Facade;
using Infragistics.Web.UI.LayoutControls;
using System.Drawing;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections;
using System.Configuration;
using CommonUtility;
using System.IO;
using PaymentXP.BusinessObjects.Logging;

public partial class wucProductSubscription : System.Web.UI.UserControl
{
    #region Properties

    public Guid MerchantUID { get; set; }
    public Guid UserUID { get; set; }



    private MerchantApp Merchant { get; set; }
    private bool isCompassMerchant { get; set; }
    private List<PaymentXP.BusinessObjects.Subscription> SubscriptionList = null;
    private List<Product> ProductList = null;

    private const string VS_KEY_EDIT = "EditMode";
    private const string CBMS_PRODUCT_ID = "3";
    private const string CBMSPLUS_PRODUCT_ID = "12";
    private const string RDR_PRODUCT_ID = "92";
    private const string RDR_SETUP_PRODUCT_ID = "93";
    private readonly IReadOnlyList<int> rdrproducts = new List<int>() { 92, 93 };

    private readonly IReadOnlyList<string> ROLES_ACCESS = new List<string>() {
                Constants.ROLE_OPERATIONS,
                Constants.ROLE_DEPLOYMENT
            };

    private bool VSCBMSEnabled
    {
        get
        {
            ViewState["VSCBMSEnabled"] = ViewState["VSCBMSEnabled"] ?? false;
            return (bool)ViewState["VSCBMSEnabled"];
        }
        set { ViewState["VSCBMSEnabled"] = value; }
    }

    #endregion

    #region Events

    protected override void LoadControlState(object savedState)
    {
        base.LoadControlState(savedState);
    }

    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        wucMerchantProductRuleSetup1.SetSubscribeID("ContentPlaceHolder1_wucProductSubscription1_TabControl_tmpl3_rdrGrid_ManageSubscription_0");


        if (this.MerchantUID == Guid.Empty) { return; }

        MerchantFacade facade = new MerchantFacade();
        this.Merchant = facade.GetMerchantAppZeus(this.MerchantUID.ToString());
        if (IsSubscribed(Convert.ToInt32(RDR_PRODUCT_ID)))
        {
            wucMerchantProductRuleSetup1.SendChangesRules_Visible = true;
            wucMerchantProductRuleSetup1.IsSubcribed = true;
            if (UserSessions.CurrentUser.UserRoles.Keys.Intersect(ROLES_ACCESS).Any())
            {
                wucMerchantProductRuleSetup1.SendChangesRules_Enabled = true;
            }
        }
        if (this.Merchant != null)
        {
            isCompassMerchant = (this.Merchant.AuthPlatformUID.ToUpper() == AuthorizationPlatforms.Compass
          && this.Merchant.SettlePlatformUID.ToUpper() == SettlementPlatforms.North);
        }

        if (!Page.IsPostBack)
        {
            if (ViewState[VS_KEY_EDIT] == null) { ViewState.Add(VS_KEY_EDIT, false); }

            PopulateProductGrid();

        }
        wucMerchantProductRuleSetup1.SendChangesRulesClick += new EventHandler(wucMerchantProductRuleSetup_SendChangesRules);
    }

    private void wucMerchantProductRuleSetup_SendChangesRules(object sender, EventArgs e)
    {
        int productId = Convert.ToInt32(RDR_PRODUCT_ID);
        List<Product> productList = DataProduct.GetAgentProductList(new Guid(this.Merchant.AgentUID), int.Parse(this.Merchant.ID), false, this.Merchant.Brand);
        Product product = productList.FirstOrDefault<Product>(x => x.ProductId == productId);

        VerifiRequestingEnrollment(Merchant, int.Parse(RDR_SETUP_PRODUCT_ID), SubscriptionAction.Maintenance);
        //DM-4023 by Jorge Perez
        ProductSubscriptionService.SendEmailMerchant(Merchant, EmailTemplateTypes.RDRSendChangesMerchantTemplate, UserSessions.CurrentUser.UserName, null);
    }

    #endregion

    #region Grids Events

    protected void ProductGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            this.GetSubscriptionList();

            HiddenField hf = ((HiddenField)e.Item.FindControl("ProductKey"));

            Button manageSubscription = (Button)e.Item.FindControl("ManageSubscription");
            manageSubscription.Text = "Subscribe";
            manageSubscription.OnClientClick = "return confirm('You are about to subscribe to this product. Are you sure you want to continue?');";

            manageSubscription.CommandArgument = hf.Value;
            bool isSubscribed = IsSubscribed(int.Parse(hf.Value));
            if (isSubscribed)
            {
                manageSubscription.Text = "Unsubscribe";
                manageSubscription.OnClientClick = "return confirm('You are about to unsubscribe to this product. Are you sure you want to continue?');";
            }
            if (hf.Value != RDR_SETUP_PRODUCT_ID)
            {
                ((GridView)e.Item.FindControl("ProductFeeGrid")).DataSource = GetFeeGridDataSource(int.Parse(hf.Value));
                ((GridView)e.Item.FindControl("ProductFeeGrid")).DataBind();
            }
            if (hf.Value == CBMS_PRODUCT_ID)
            {
                this.VSCBMSEnabled = isSubscribed;
            }

            if (CommonUtility.Util.if_i(hf.Value, 0) == (int)ProductTypes.UPDATE_XP)
            {
                //Ani: Start: Commented check for Compass and added only for Irvine office
                e.Item.Visible = (this.Merchant.Office == CommonUtility.Util.Offices.Irvine);
            }

            if (hf.Value == RDR_PRODUCT_ID)
            {
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                dt2 = new DataTable();
                for (int i = int.Parse(RDR_PRODUCT_ID); i <= int.Parse(RDR_SETUP_PRODUCT_ID); i++)
                {
                    dt1 = GetFeeGridDataSource(i);
                    dt2.Merge(dt1);
                }
                //dt2.Rows.Remove(dt2.Rows[dt2.Rows.Count - 1]);
                ((GridView)e.Item.FindControl("ProductFeeGrid")).DataSource = dt2;
                ((GridView)e.Item.FindControl("ProductFeeGrid")).DataBind();

                //get rules 
                List<RDRRules> rulelist = PaymentXP.DataObjects.Reporting.DataRDRRules.GetRDRRules(UserSessions.CurrentMerchantApp.ID, int.Parse(RDR_SETUP_PRODUCT_ID), ePortals.ZEUS);
                if (rulelist.Count <= 0 || !isSubscribed)
                {
                    manageSubscription.Enabled = false;
                    var _sendChanges = (LinkButton)wucMerchantProductRuleSetup1.FindControl("SendChangesRules");
                    if (_sendChanges != null)
                    {
                        _sendChanges.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
            }
        }
    }

    protected void ProductFeeGrid_ItemDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((DataBinder.Eval(e.Row.DataItem, "FeeID").ToString() == "343")
                || (DataBinder.Eval(e.Row.DataItem, "FeeID").ToString() == "342"))
            {
                e.Row.Visible = false;
            }

            if (DataBinder.Eval(e.Row.DataItem, "FeeID").ToString() == "343")
            {
                string value = e.Row.Cells[2].Text;

                e.Row.Cells[2].Text = value.Replace("$", "") + " %";
            }
        }
    }

    protected void ProductGrid_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Save":

                Button btn = (Button)e.CommandSource;
                Label lbl = (Label)e.Item.FindControl("lblProductMessage");

                Subscription(btn, lbl);

                break;
        }
    }

    #endregion

    #region Helper Methods

    private bool IsSubscribed(int productId)
    {
        if (SubscriptionList == null) { GetSubscriptionList(true); }

        if (SubscriptionList != null)
        {
            Subscription subscription = SubscriptionList.FirstOrDefault<Subscription>(x => x.Product.ProductId == productId);
            if (subscription != null && subscription.IsActive)
            {
                return true;
            }
        }
        return false;
    }

    private void GetSubscriptionList(bool forceRefresh = false)
    {
        if (forceRefresh || SubscriptionList == null)
        {
            int _merchantID = 0;
            if (this.Merchant != null)
            {
                int.TryParse(this.Merchant.ID, out _merchantID);
            }

            SubscriptionList = PaymentXP.DataObjects.DataProduct.GetMerchantCurrentProductSubscriptionList(_merchantID);
        }
    }

    private void RefreshProductData(int merchantID = int.MinValue)
    {
        this.ProductList = DataProduct.GetAgentProductList(new Guid(this.Merchant.AgentUID), int.Parse(this.Merchant.ID), false, this.Merchant.Brand);
    }

    private void PopulateProductGrid(int merchantID = int.MinValue, bool forceRefresh = false)
    {
        if (forceRefresh || (this.ProductList == null))
        {
            RefreshProductData(merchantID);
        }

        if (this.ProductList == null)
        {
            ShowError("No Products for " + this.Merchant.Brand);
            return;
        }

        try
        {
            BindAlternativePayments();
            BindDeployment();
            BindPaymentAcceptance();
            BindRiskManagement();
            BindRDR();
        }
        catch
        {
            ShowError("An error occurred while loading merchant products, please contact an IT administrator.");
        }
    }

    private void BindAlternativePayments()
    {
        List<Product> alternativePayments = this.ProductList.Where<Product>(p => p.Category == ProductCategory.AlternativePayments).ToList<Product>();

        if (alternativePayments.Count > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ProductID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ProductDescription", typeof(System.String)));

            DataRow row = null;

            foreach (Product product in alternativePayments)
            {
                if (!product.IsActive)
                {
                    //don't display in-active products
                    continue;
                }

                if (!product.IsProductOptInEnabled)
                {
                    //exclude Meritus Fees
                    continue;
                }

                row = dt.NewRow();
                row["ProductID"] = product.ProductId;
                row["ProductName"] = product.Name;
                row["ProductDescription"] = product.Description;
                dt.Rows.Add(row);
            }

            this.AlternatePaymentGrid.DataSource = dt;
            this.AlternatePaymentGrid.DataBind();
        }
        else
        {
            ContentTabItem altPaymentTab = this.TabControl.Tabs.FindTabFromKey("tabAltPayment");
            this.TabControl.Tabs.Remove(altPaymentTab);
        }
    }

    private void BindDeployment()
    {
        List<Product> deployment = this.ProductList.Where<Product>(p => p.Category == ProductCategory.Deployment).ToList<Product>();

        if (deployment.Count > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ProductID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ProductDescription", typeof(System.String)));

            DataRow row = null;

            foreach (Product product in deployment)
            {
                if (!product.IsActive)
                {
                    //don't display in-active products
                    continue;
                }

                if (!product.IsProductOptInEnabled)
                {
                    //exclude Meritus Fees
                    continue;
                }

                row = dt.NewRow();
                row["ProductID"] = product.ProductId;
                row["ProductName"] = product.Name;
                row["ProductDescription"] = product.Description;
                dt.Rows.Add(row);
            }

            this.DeploymentGrid.DataSource = dt;
            this.DeploymentGrid.DataBind();
        }
        else
        {
            ContentTabItem deploymentTab = this.TabControl.Tabs.FindTabFromKey("tabDeployment");
            this.TabControl.Tabs.Remove(deploymentTab);
        }
    }

    private void BindPaymentAcceptance()
    {
        List<Product> paymentAcceptance = this.ProductList.Where<Product>(p => p.Category == ProductCategory.PaymentAcceptance).ToList<Product>();

        if (paymentAcceptance.Count > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ProductID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ProductDescription", typeof(System.String)));

            DataRow row = null;

            foreach (Product product in paymentAcceptance)
            {
                if (!product.IsActive)
                {
                    //don't display in-active products
                    continue;
                }

                if (!product.IsProductOptInEnabled)
                {
                    //exclude Meritus Fees
                    continue;
                }
                //PXP-2461 Android pay 
                if (product.Key == "AndroidPay")
                {
                    if (this.Merchant.Office == CommonUtility.Util.Offices.Montreal || this.Merchant.Office == CommonUtility.Util.Offices.Gatineau)
                    {
                        row = dt.NewRow();
                        row["ProductID"] = product.ProductId;
                        row["ProductName"] = product.Name;
                        row["ProductDescription"] = product.Description;
                        dt.Rows.Add(row);
                    }
                }
                else
                {
                    row = dt.NewRow();
                    row["ProductID"] = product.ProductId;
                    row["ProductName"] = product.Name;
                    row["ProductDescription"] = product.Description;
                    dt.Rows.Add(row);
                }
            }

            this.PaymentAcceptanceGrid.DataSource = dt;
            this.PaymentAcceptanceGrid.DataBind();
        }
        else
        {
            ContentTabItem paymentAcceptanceTab = this.TabControl.Tabs.FindTabFromKey("tabPaymentAcceptance");
            this.TabControl.Tabs.Remove(paymentAcceptanceTab);
        }
    }

    private void BindRiskManagement()
    {
        List<Product> riskManagement = this.ProductList.Where<Product>(p => p.Category == ProductCategory.RiskManagement).ToList<Product>();

        if (riskManagement.Count > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ProductID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ProductDescription", typeof(System.String)));

            DataRow row = null;

            foreach (Product product in riskManagement)
            {
                if (!product.IsActive)
                {
                    //don't display in-active products
                    continue;
                }

                if (!product.IsProductOptInEnabled)
                {
                    //exclude Meritus Fees
                    continue;
                }

                row = dt.NewRow();
                row["ProductID"] = product.ProductId;
                row["ProductName"] = product.Name;
                row["ProductDescription"] = product.Description;
                dt.Rows.Add(row);
            }

            this.RiskManagementGrid.DataSource = dt;
            this.RiskManagementGrid.DataBind();
        }
        else
        {
            ContentTabItem riskTab = this.TabControl.Tabs.FindTabFromKey("tabRiskManagement");
            this.TabControl.Tabs.Remove(riskTab);
        }
    }

    private void BindRDR()
    {
        IEnumerable<Product> deployment = this.ProductList.Where(p => p.Category == ProductCategory.CBMS);

        if (deployment.Count() > 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ProductID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ProductDescription", typeof(System.String)));

            DataRow row = null;

            foreach (Product product in deployment)
            {
                if (!product.IsActive || !product.IsProductOptInEnabled || product.ProductId == int.Parse(RDR_SETUP_PRODUCT_ID)
                    || product.ProductId == (int)ProductTypes.RDRHandling)
                {
                    //don't display in-active products
                    continue;
                }

                row = dt.NewRow();
                row["ProductID"] = product.ProductId;
                row["ProductName"] = product.Name;
                row["ProductDescription"] = product.Description;
                dt.Rows.Add(row);
            }

            this.rdrGrid.DataSource = dt;
            this.rdrGrid.DataBind();
        }
        else
        {
            ContentTabItem rdrTab = this.TabControl.Tabs.FindTabFromKey("tabRDR");
            this.TabControl.Tabs.Remove(rdrTab);
        }
    }

    private DataTable GetFeeGridDataSource(int productId)
    {
        if (this.ProductList == null)
        {
            RefreshProductData();
        }

        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("FeeID", typeof(System.Int32)));
        dt.Columns.Add(new DataColumn("Name", typeof(System.String)));
        dt.Columns.Add(new DataColumn("MerchantCost", typeof(System.Decimal)));

        DataRow row = null;

        if (this.ProductList.First<Product>(x => x.ProductId == productId).FeeList == null)
        {
            return null;
        }

        foreach (Fee fee in this.ProductList.First<Product>(x => x.ProductId == productId).FeeList)
        {
            row = dt.NewRow();

            row["FeeID"] = fee.FeeId;
            row["Name"] = fee.Name;

            if (fee.MerchantCost < 0)
            {
                row["MerchantCost"] = fee.SuggestedMerchantCost;
            }
            else
            {
                row["MerchantCost"] = fee.MerchantCost;
            }

            dt.Rows.Add(row);
        }

        return dt;
    }

    private void ShowError(string error)
    {
        this.ControlPanel.Visible = false;
        this.ErrorPanel.Visible = true;
        this.lblError.Text = error;
    }

    private void Subscription(Button btn, Label lbl)
    {
        int productId = int.Parse(btn.CommandArgument);

        List<Product> productList = DataProduct.GetAgentProductList(new Guid(this.Merchant.AgentUID), int.Parse(this.Merchant.ID), false, this.Merchant.Brand);

        Product product = productList.FirstOrDefault<Product>(x => x.ProductId == productId);
        User user = DataUser.GetInstance().GetUser(this.UserUID.ToString());

        if (IsSubscribed(productId))
        {
            if (productId.ToString() == CBMS_PRODUCT_ID)
            {
                this.VSCBMSEnabled = false;
            }
            if (productId.ToString() == RDR_PRODUCT_ID)
            {
                foreach (Product _product in productList.Where(x => rdrproducts.Contains(x.ProductId)))
                {
                    PaymentXP.Facade.ProductSubscriptionService.TurnOffSubscription(this.Merchant, user, ePortals.ZEUS, _product);
                }

                //DM-3415 Send email to Verifi requesting unenrollment
                VerifiRequestingEnrollment(Merchant, int.Parse(RDR_SETUP_PRODUCT_ID), SubscriptionAction.Unenrollment);
            }
            else
            {
                PaymentXP.Facade.ProductSubscriptionService.TurnOffSubscription(this.Merchant, user, ePortals.ZEUS, product);
                //PXP-10890 Sanidhya Start
                PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOff(this.Merchant, productId);
                //PXP-10890 Sanidhya End
            }
            btn.OnClientClick = "return confirm('You are about to subscribe to this product. Are you sure you want to continue?');";
            btn.Text = "Subscribe";
            lbl.Text = " - Unsubscription successful";
        }
        else
        {
            if (productId.ToString() == CBMSPLUS_PRODUCT_ID)
            {
                if (!this.VSCBMSEnabled)
                {
                    lbl.ForeColor = Color.Red;
                    lbl.Text = " - Subscription to CBMS required";
                    return;
                }
            }
            if (productId.ToString() == CBMS_PRODUCT_ID)
            {
                this.VSCBMSEnabled = true;
            }
            // This is for RDR Product
            if (productId.ToString() == RDR_PRODUCT_ID)
            {
                foreach (Product _product in productList.Where(x => rdrproducts.Contains(x.ProductId)))
                {
                    PaymentXP.Facade.ProductSubscriptionService.Subscribe(this.Merchant, user, ePortals.ZEUS, _product);
                }
                //DM-3415 Send email to Verifi requesting enrollment
                VerifiRequestingEnrollment(Merchant, int.Parse(RDR_SETUP_PRODUCT_ID), SubscriptionAction.Enrollment);
                //DM-3868 Send Merchant Enrollment Email
                ProductSubscriptionService.SendMerchantSubscribeEmail(this.Merchant, user, ePortals.ZEUS);
            }
            else
            {
                PaymentXP.Facade.ProductSubscriptionService.Subscribe(this.Merchant, user, ePortals.ZEUS, product);
                //PXP-10890:Sanidhya Start
                PaymentXP.Facade.ProductSubscriptionService.Cbms_CbmsPlusAutomaticSubscriptionOn(this.Merchant, productId);
                //PXP-10890:Sanidhya end
            }
            btn.OnClientClick = "return confirm('You are about to unsubscribe to this product. Are you sure you want to continue?');";
            btn.Text = "Unsubscribe";
            lbl.Text = " - Subscription successful";
        }
    }

    //DM-3415 Send email to Verifi requesting unenrollment
    private void VerifiRequestingEnrollment(MerchantApp merchant, int productId, SubscriptionAction subscriptionAction)
    {
        try
        {
            //DM-4168
            var excelPathFile = HttpContext.Current.Server.MapPath(@"~/Excel/RDR Reseller Template 4.0.xlsx");
            string excelFilename = string.Format("Verifi RDR {0} Form.xls", subscriptionAction.GetAttribute<ActionAttribute>().ActionText);
            bool isRuleVisible = subscriptionAction != SubscriptionAction.Unenrollment;

            Dictionary<string, byte[]> excelAttach = ProductSubscriptionService.CreateVerifiExcel(merchant, ePortals.ZEUS, isRuleVisible, productId, excelFilename, excelPathFile);
            if (excelAttach != null && excelAttach.ContainsKey(excelFilename))
            {
                var excelAttachHash = new Hashtable(excelAttach);
                var isSent = ProductSubscriptionService.SendVerifiRequestingEnrollmentEmail(this.Merchant, int.Parse(RDR_SETUP_PRODUCT_ID), UserSessions.CurrentUser.UserName, ePortals.ZEUS, subscriptionAction, excelAttachHash);
                if (!isSent)
                {
                    ZeusWeb.Logging.ErrorLog.Info("Email to Verifi could not be sent.");
                }
                //Upload document to Zeus
                string filename = String.Format("Presolved_{0}_{1}.xls", this.Merchant.ID, DateTime.Now.ToString("MMddyy_HHmmss"));
                var fu = new ZeusWeb.MDocWS.FileUpload();
                fu.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];
                byte[] _attach = excelAttach[excelFilename];
                ZeusWeb.MDocWS.UploadResponse resp = fu.UploadFileWithSourceAndUser(_attach, Convert.ToInt32(this.Merchant.ID), this.Merchant.MerchantAppUID, this.Merchant.AgentID, this.Merchant.AgentUID, (int)MDoc.eMDocType.Visa_RDR_Form, filename, ePortals.ZEUS.ToString().ToLower(), 0, subscriptionAction.ToString(), "", Convert.ToInt32(this.Merchant.ID), (int)MDoc.eMDocSourceID.Merchant, UserSessions.CurrentUser.UserName);

                try
                {
                    ProductSubscriptionService.InsertProductRuleHistoryClick(Convert.ToInt32(merchant.ID), UserSessions.CurrentUser.UserName, ePortals.ZEUS, subscriptionAction);
                }
                catch (Exception ex)
                {
                    ZeusWeb.Logging.ErrorLog.Error(String.Format("ERROR InsertProductRuleHistoryClick For MerchantId: {0} ProductId: RDR  \r\n Exception Message: {1}", this.Merchant.ID, ex.Message), ex);
                }
            }
            else
            {
                ZeusWeb.Logging.ErrorLog.Info("Created Verifi Excel failed.");
            }
        }
        catch (Exception ex)
        {
            ZeusWeb.Logging.ErrorLog.Error(String.Format("Error Exception happened. Failed to create and to sent RDR Verify Email with Excel subscription Form For MerchantId: {0} ProductId: RDR  \r\n Exception Message: {1}", this.Merchant.ID, ex.Message), ex);
        }
    }
    //DM-3415 end
}
#endregion