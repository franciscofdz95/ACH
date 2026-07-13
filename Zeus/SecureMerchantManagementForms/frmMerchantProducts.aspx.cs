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
using System.Reflection;
using System.IO;
using System.Collections.Generic;

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Linq;
using System.Web.Services;
using PaymentXP.WebControls.Editor;

public partial class frmMerchantProducts : frmBaseDataEntry
{
    private const int _productID = 93;
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        WucBusinessInfo1.pnlInfo.Enabled = false;

        if (UserSessions.CurrentMerchantApp != null)
        {
            wucProductSubscription1.MerchantUID = new Guid(UserSessions.CurrentMerchantApp.MerchantAppUID);
            wucProductSubscription1.UserUID = new Guid(UserSessions.CurrentUser.UID);
        }

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Products);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Products");
            }

            //Set current page
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Merchant Profile";

            //Primary key column name
            //this.UIDName = "MerchantAppUID";

            //Set Security on the page
            //FormHandler.SetSecurity(this.Page);

            //Set Adding flag            
            this.Adding = Convert.ToBoolean(Request["Adding"]);

            if (this.Adding)
            {
                this.FormNew();
            }
            else
            {
                if (UserSessions.CurrentMerchantApp == null) return;
                this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

                this.FormShow(this.UID);
            }
        }
    }

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);

        DropDownList status = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
        LookupTableHandler.MerchantAppStatus(status, false, "Merchant Management", agreement.StatusName.Substring(0, 2));
        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        WucBusinessInfo1.SelectButton.Enabled = this.EditMode;

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();

            DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
            LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2));
        }
        WucBusinessInfo1.LoadOffice(agreement);
        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
        //DebitMonthlyFee.Value = 0;
        //...
    }

    public override bool FormSave()
    {
        return true;
    }

    public override void ToggleButtons()
    {
        this.Master.ToggleMenu(!this.EditMode);
    }

    public override bool FormDataCheck()
    {
        return true;
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
        return false;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    protected string ValidateSubmitRule(int operatorsID, string operatorDefinition, string value)
    {
        bool _response = false;
        string _answer = string.Empty;

        switch (operatorsID)
        {
            case (int)AttributesRDR.PersonalAccount:
                _response = RDRReseller.PersonalAccountValidate(value);
                break;
            case (int)AttributesRDR.TransactionDate:
                _response = RDRReseller.TransactionDateValidate(value);
                break;
            case (int)AttributesRDR.TransactionAmount:
                _response = RDRReseller.TransactionAmountValidate(decimal.Parse(value));
                break;
            case (int)AttributesRDR.TransactionCurrencyCode:
                _response = RDRReseller.TransactionCurrencyCodeValidate(value);
                break;
            case (int)AttributesRDR.PurchaseIdentifier:
                _response = RDRReseller.PurchaseIdentifierValidate(value);
                break;
            case (int)AttributesRDR.DisputeCategory:
                _response = RDRReseller.DisputeCategoryValidate(int.Parse(value));
                break;
            case (int)AttributesRDR.DisputeConditionCode:
                _response = RDRReseller.DisputeConditionCodeValidate(value);
                break;
        }

        if (!_response)
        {
            _answer = "The Attribute " + operatorDefinition + " has an incorrect value, please check it.";
        }

        return _answer;
    }

    #region RDR

    [WebMethod]
    public static IReadOnlyList<REF_Attribute> GetRuleAttributes()
    {

        return wucRDRProductRulesSetup.GetAttributesRules();
    }

    [WebMethod]
    public static IReadOnlyList<REF_Operator> GetRuleOperators()
    {
        return wucRDRProductRulesSetup.GetOperators();
    }

    [WebMethod]
    public static IReadOnlyList<REF_AttributeUIControlValue> GetRuleControlValues()
    {
        return wucRDRProductRulesSetup.GetAttributeControlValues();
    }

    [WebMethod]
    public static IReadOnlyList<ProductRule> GetRulesByProducID()
    {
        List<ProductRule> _return = new List<ProductRule>();
        if (UserSessions.MerchantId > 0)
        {
            _return = wucRDRProductRulesSetup.GetRulesByProducID(ePortals.ZEUS, _productID, 0, UserSessions.MerchantId, 0, 0);
        }
        return _return;
    }

    [WebMethod]
    public static ProductRuleView SaveAllRules(ProductRuleView rules)
    {
        ProductRuleView _return = rules;
        if (UserSessions.MerchantId > 0 && UserSessions.CurrentUser != null)
        {
            _return = wucRDRProductRulesSetup.SaveAllRules(rules, UserSessions.CurrentUser.UserName, _productID, ePortals.ZEUS, UserSessions.MerchantId, 0, 0);
        }
        return _return;
    }
    #endregion
}