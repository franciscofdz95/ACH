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
using System.Drawing;
using System.Text.RegularExpressions;
using Infragistics.Web.UI.LayoutControls;
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Text;
using System.Linq;
using Infragistics.Web.UI.EditorControls;
using ZeusWeb.Class;
using Okta.Sdk.Model;

public partial class frmMerchantProfile : frmBaseDataEntry
{
    bool isACHonly
    {
        get { return CommonUtility.Util.if_b(WucBusinessInfo1.isACHonly, false); }
        set { WucBusinessInfo1.isACHonly = isACHonly; }
    }
    //Code added by Gabriel Gonzalez for DM-4647
    public bool HasBankAccountRole
    {
        get
        {
            return (UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_BANK_ACCOUNT) && UserSessions.CurrentUser.UserRoles[Constants.ROLE_BANK_ACCOUNT].Enabled);
        }
    }
    //Code added by Gabriel Gonzalez for DM-4647
    public ControlObject PnlBankField
    {
        get
        {
            var form = this.GetType().BaseType.Name.ToUpper();
            var obj = UserSessions.CurrentUser.UserForms[form].ControlObjects.FirstOrDefault(c => c.ID == "pnlBankFields");
            return obj ?? new ControlObject();
        }
    }
    //DM-5125 ini
    public ControlObject CheckBoxAllocationField
    {
        get
        {
            try
            {
                var form = this.GetType().BaseType.Name.ToUpper();
                var obj = UserSessions.CurrentUser.UserForms[form].ControlObjects.FirstOrDefault(c => c.ID == ConstantFacade.ControlObjects.CHECKBOX_ALLOCATION);
                return obj ?? new ControlObject();
            }
            catch (Exception)
            {
                return new ControlObject();
            }
        }
    }
    //DM-5125 end
    void grdLeads_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;
        if (e.CommandSource is LinkButton)
        {
            lnk = (LinkButton)e.CommandSource;
        }
        else
        {
            return;
        }

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
        string uid = str[0];

        Lead objLead = LeadFacade.GetLead(uid);

        if (objLead != null && !string.IsNullOrEmpty(objLead.LeadUID))
        {
            LeadsID.Text = objLead.LeadID.ToString();
            LeadsUID.Value = objLead.LeadUID;
            Referral.Text = objLead.ReferenceNumber; //Over Write the Reference number from lead.
        }

        lbRemoveMerchant.Visible = true;
        dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
    }

    public string LeadID
    {
        get { return Convert.ToString(ViewState["LeadID"]); }
        set { ViewState["LeadID"] = Convert.ToString(value); }
    }

    public bool IsCopy
    {
        get
        {
            if (ViewState["IsCopy"] != null)
                return DataLayer.Field2Bool(ViewState["IsCopy"]);
            else
                return false;
        }
        set { ViewState["IsCopy"] = value; }
    }

    override protected void OnInit(EventArgs e)
    {
        grdLeads.GridRowCommand += grdLeads_GridRowCommand;
        confirm.ButtonClick += new wuConfirmDialog.ButtonClickHandler(confirm_ButtonClick);
        WucBusinessInfo1.TextChange += new wucBusinessInfo.TextChangeHandler(WucBusinessInfo1_TextChange);

        base.OnInit(e);

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadCountryCallingCodes(CustomerServicePhoneCallingCode);
        }
    }

    bool isValid = true;

    protected void WucBusinessInfo1_TextChange(object sender, EventArgs e)
    {

        switch (((TextBox)sender).ID)
        {
            case "BusinessTaxID":
                string BusinessTaxID = ((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text;
                if ((UserSessions.CurrentMerchantApp != null && BusinessTaxID != UserSessions.CurrentMerchantApp.BusinessTaxID) || (this.Adding && !string.IsNullOrWhiteSpace(BusinessTaxID)))
                {
                    ((TextBox)confirm.FindControl("txtTaxID")).Visible = true;
                    ((Label)confirm.FindControl("lblTaxID")).Visible = true;
                    ((TextBox)confirm.FindControl("txtTaxID")).Focus();
                    isValid = false;
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                }
                break;
            case "AuthPlatformMid":
                string AuthPlatformMid = ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text;
                if ((UserSessions.CurrentMerchantApp != null && AuthPlatformMid != UserSessions.CurrentMerchantApp.AuthPlatformMid) || (this.Adding && !string.IsNullOrWhiteSpace(AuthPlatformMid)))
                {
                    ((TextBox)confirm.FindControl("txtMID")).Visible = true;
                    ((Label)confirm.FindControl("lblMID")).Visible = true;
                    ((TextBox)confirm.FindControl("txtMID")).Focus();
                    isValid = false;
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                }
                break;
            case "SettlePlatformMid":
                string SettlePlatformMid = ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text;
                if ((UserSessions.CurrentMerchantApp != null && SettlePlatformMid != UserSessions.CurrentMerchantApp.SettlePlatformMid) || (this.Adding && !string.IsNullOrWhiteSpace(SettlePlatformMid)))
                {
                    ((TextBox)confirm.FindControl("txtBMID")).Visible = true;
                    ((Label)confirm.FindControl("lblBMID")).Visible = true;
                    ((TextBox)confirm.FindControl("txtBMID")).Focus();
                    isValid = false;
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                }
                break;
            case "AccountNumber":
                if ((UserSessions.CurrentMerchantApp != null && AccountNumber.Text != UserSessions.CurrentMerchantApp.AccountNumber) || (this.Adding && !string.IsNullOrWhiteSpace(AccountNumber.Text)))
                {
                    ((TextBox)confirm.FindControl("txtDDA")).Visible = true;
                    ((Label)confirm.FindControl("lblDDA")).Visible = true;
                    ((TextBox)confirm.FindControl("txtDDA")).Focus();
                    isValid = false;
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                }
                break;
            default:
                break;
        }
    }

    void confirm_ButtonClick(object sender, EventArgs e)
    {
        string url = string.Empty, message = string.Empty;
        bool count = true;
        MerchantApp app = UserSessions.CurrentMerchantApp;
        if (((TextBox)confirm.FindControl("txtTaxID")).Visible && ((TextBox)confirm.FindControl("txtTaxID")).Text != ((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text)
        {
            count = false;
            message += "Please enter valid TaxID.";
            ((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text = (app != null) ? app.BusinessTaxID : string.Empty;
        }
        if (((TextBox)confirm.FindControl("txtDDA")).Visible && ((TextBox)confirm.FindControl("txtDDA")).Text != AccountNumber.Text)
        {
            count = false;
            message += "Please enter valid Account number.";
            AccountNumber.Text = (app != null) ? app.AccountNumber : string.Empty;
        }
        if (((TextBox)confirm.FindControl("txtMID")).Visible && ((TextBox)confirm.FindControl("txtMID")).Text != ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text)
        {
            count = false;
            message += "Please enter valid Front MID.";
            ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text = (app != null) ? app.AuthPlatformMid : string.Empty;
        }
        if (((TextBox)confirm.FindControl("txtBMID")).Visible && ((TextBox)confirm.FindControl("txtBMID")).Text != ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text)
        {
            count = false;
            message += "Please enter valid Back MID.";
            ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text = (app != null) ? app.SettlePlatformMid : string.Empty;
        }

        if (!count)
            this.Master.AddMessageError(message); // lblError.Text = message;

        confirm.SetValue(false);
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    //Added for Validation for phone number, Country Code and Extention 
    private void wucBusinessInfo_OfficeIDChanged(object sender, EventArgs e)
    {
        wucBusinessInfo usercontrol = (wucBusinessInfo)sender;
        DropDownList office = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");
        int officeid;
        int.TryParse(office.SelectedValue, out officeid);
        DropDownList wucMerchantBrand = (DropDownList)usercontrol.FindControl("Brand");
        wucContact1.officeID = int.Parse(office.SelectedValue);
        wucContact1.PhoneValidations();
        //PXP-6627 Fixed with added code Start
        DropDownList wucddlMerchantApptypeUID = (DropDownList)WucBusinessInfo1.FindControl("MerchantAppTypeUID");
        //PXP-6627 Fixed with added code end
        TextBox FaxCountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("FaxCountryCodeDisplay");
        TextBox DBACountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("DBACountryCodeDisplay");
        DropDownList BusinessFaxCountryCodeddl = (DropDownList)WucBusinessInfo1.FindControl("BusinessFaxCountryCode");
        DropDownList BusinessDBAPhoneCountryCodeddl = (DropDownList)WucBusinessInfo1.FindControl("BusinessDBAPhoneCountryCode");
        FaxCountryCodetxt.Text = BusinessFaxCountryCodeddl.SelectedValue.ToString();
        DBACountryCodetxt.Text = BusinessDBAPhoneCountryCodeddl.SelectedValue.ToString();
        CallingCodeDisplay.Text = CustomerServicePhoneCallingCode.SelectedValue.ToString();

        if (officeid == (int)CommonUtility.Util.Offices.Irvine)
        {
            string strCustomerServicePhone = CommonUtility.Util.GetNumbersFromString(CustomerServicePhone.Value.ToString());
            CustomerServicePhone.Value = strCustomerServicePhone.ToString();
            CustomerServicePhone.InputMask = "000-000-0000";
            //PXP-6627 Fixed with added code Start
            //Code changes for PXP-10225 by koshlendra
            if (wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_WOODFOREST 
                || wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_WOODFOREST_SB
                || wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_CITIZENS
                || wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_BBVACOMPASS)
            {
                LookupTableHandler.LoadRefundPolicies(WFReturnPoliciesUID);
                if (UserSessions.CurrentMerchantApp != null)
                {
                    LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, UserSessions.CurrentMerchantApp.MerchantAppUID);
                }
                else
                {
                    LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, null);
                }
            }
            else
                LookupTableHandler.LoadReturnPolicies(ReturnPoliciesUID, false);
            //PXP-6627 Fixed with added code end
        }
        else
        {
            CustomerServicePhone.InputMask = "############";
        }

        //PXP-7469 abarua added
        if (UserSessions.CurrentMerchantApp != null)
        {
            isACHonly = (UserSessions.CurrentMerchantApp.AchID > 0 && wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_ACH_ONLY);

            Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(UserSessions.CurrentMerchantApp.MerchantAppUID);
            //PXP-7469 abarua
            // if UWIssues is empty, we prefill it with a default free form "template".


            if (objUW == null || (objUW != null && string.IsNullOrWhiteSpace(objUW.UWIssues)))
            {
                string AcqBank_OnlineBankOnly = "8d2281ae-e6f0-429e-94d0-c0fe6bfede01";
                string bank_achonly_uid = "DADC71E7-A732-4FCA-8C86-DE3E7253209C";
                StringBuilder sbuw = new StringBuilder();
                StringBuilder sbcc = new StringBuilder();
                string line_template = "{0}: {1}";
                if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {

                    sbcc.AppendLine(string.Format(line_template, "General Set up Details", ""));
                    sbcc.AppendLine(string.Format(line_template, "Billing type", ""));
                    sbuw.Append(sbcc.ToString()).ToString();
                    UWIssues.Text = sbuw.ToString();
                    if (UserSessions.CurrentMerchantApp.MerchantAppUID.ToUpper() == bank_achonly_uid && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                    {
                        UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(UserSessions.CurrentMerchantApp);

                    }
                    if (CommonUtility.Util.if_s(UserSessions.CurrentMerchantApp.MerchantAppUID).ToUpper() == AcqBank_OnlineBankOnly.ToUpper())
                    {
                        UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(UserSessions.CurrentMerchantApp);
                    }
                }
                else
                {
                    this.UWIssues.Text = FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));
                }
            }

        }
        else
        {
            string AcqBank_OnlineBankOnly = "8d2281ae-e6f0-429e-94d0-c0fe6bfede01";
            string bank_achonly_uid = "DADC71E7-A732-4FCA-8C86-DE3E7253209C";
            StringBuilder sbuw = new StringBuilder();
            StringBuilder sbcc = new StringBuilder();
            string line_template = "{0}: {1}";
            if (office.SelectedValue.Equals("1"))
            {
                sbcc.AppendLine(string.Format(line_template, "General Set up Details", ""));
                sbcc.AppendLine(string.Format(line_template, "Billing type", ""));
                sbuw.Append(sbcc.ToString()).ToString();
                UWIssues.Text = sbuw.ToString();
                if (wucddlMerchantApptypeUID.SelectedValue.ToUpper() == bank_achonly_uid && (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue) == MerchantBrand.Optimal)
                {
                    UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));

                }
                if (wucddlMerchantApptypeUID.SelectedValue.ToUpper().ToUpper() == AcqBank_OnlineBankOnly.ToUpper())
                {
                    UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));
                }
            }
            else
            {
                this.UWIssues.Text = FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));
            }
        }
    }

    private void wucBusinessInfo_MerchantAppTypeUIDChanged(object sender, EventArgs e)
    {
        wucBusinessInfo usercontrol = (wucBusinessInfo)sender;
        DropDownList wucddlMerchantApptypeUID = (DropDownList)usercontrol.FindControl("MerchantAppTypeUID");
        DropDownList wucMerchantBrand = (DropDownList)usercontrol.FindControl("Brand");
        //PXP-6627 Fixed with added code Start
        DropDownList office = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");
        //PXP-6627 Fixed with added code End
        TextBox FaxCountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("FaxCountryCodeDisplay");
        TextBox DBACountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("DBACountryCodeDisplay");
        DropDownList BusinessFaxCountryCodeddl = (DropDownList)WucBusinessInfo1.FindControl("BusinessFaxCountryCode");
        DropDownList BusinessDBAPhoneCountryCodeddl = (DropDownList)WucBusinessInfo1.FindControl("BusinessDBAPhoneCountryCode");
        //PXP-6627 Fixed with added code Start
        //Code changes for PXP-10225 by koshlendra
        if ((wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_WOODFOREST
            || wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_WOODFOREST_SB
            || wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_CITIZENS
            || wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_BBVACOMPASS) && office.SelectedItem.ToString() == "Irvine (US)")
        {
            LookupTableHandler.LoadRefundPolicies(WFReturnPoliciesUID);
            if (UserSessions.CurrentMerchantApp != null)
            {
                LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, UserSessions.CurrentMerchantApp.MerchantAppUID);
            }
            else
            {
                LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, null);
            }
        }
        else
            LookupTableHandler.LoadReturnPolicies(ReturnPoliciesUID, false);
        //PXP-6627 Fixed with added code End
        FaxCountryCodetxt.Text = BusinessFaxCountryCodeddl.SelectedValue.ToString();
        DBACountryCodetxt.Text = BusinessDBAPhoneCountryCodeddl.SelectedValue.ToString();
        CallingCodeDisplay.Text = CustomerServicePhoneCallingCode.SelectedValue.ToString();

        if (UserSessions.CurrentMerchantApp != null)
        {
            isACHonly = (UserSessions.CurrentMerchantApp.AchID > 0 && wucddlMerchantApptypeUID.SelectedValue.ToUpper() == Constants.BANK_ACH_ONLY);

            Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(UserSessions.CurrentMerchantApp.MerchantAppUID);
            //PXP-7469 abarua
            // if UWIssues is empty, we prefill it with a default free form "template".


            if (objUW == null || (objUW != null && string.IsNullOrWhiteSpace(objUW.UWIssues)))
            {
                string AcqBank_OnlineBankOnly = "8d2281ae-e6f0-429e-94d0-c0fe6bfede01";
                string bank_achonly_uid = "DADC71E7-A732-4FCA-8C86-DE3E7253209C";
                StringBuilder sbuw = new StringBuilder();
                StringBuilder sbcc = new StringBuilder();
                string line_template = "{0}: {1}";
                if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {

                    sbcc.AppendLine(string.Format(line_template, "General Set up Details", ""));
                    sbcc.AppendLine(string.Format(line_template, "Billing type", ""));
                    sbuw.Append(sbcc.ToString()).ToString();
                    UWIssues.Text = sbuw.ToString();
                    if (UserSessions.CurrentMerchantApp.MerchantAppUID.ToUpper() == bank_achonly_uid && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                    {
                        UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(UserSessions.CurrentMerchantApp);

                    }
                    if (CommonUtility.Util.if_s(UserSessions.CurrentMerchantApp.MerchantAppUID).ToUpper() == AcqBank_OnlineBankOnly.ToUpper())
                    {
                        UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(UserSessions.CurrentMerchantApp);
                    }
                }
                else
                {
                    this.UWIssues.Text = FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));
                }
            }

        }
        else
        {
            string AcqBank_OnlineBankOnly = "8d2281ae-e6f0-429e-94d0-c0fe6bfede01";
            string bank_achonly_uid = "DADC71E7-A732-4FCA-8C86-DE3E7253209C";
            StringBuilder sbuw = new StringBuilder();
            StringBuilder sbcc = new StringBuilder();
            string line_template = "{0}: {1}";
            if (office.SelectedValue.Equals("1"))
            {
                sbcc.AppendLine(string.Format(line_template, "General Set up Details", ""));
                sbcc.AppendLine(string.Format(line_template, "Billing type", ""));
                sbuw.Append(sbcc.ToString()).ToString();
                UWIssues.Text = sbuw.ToString();
                if (wucddlMerchantApptypeUID.SelectedValue.ToUpper() == bank_achonly_uid && (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue) == MerchantBrand.Optimal)
                {
                    UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));

                }
                if (wucddlMerchantApptypeUID.SelectedValue.ToUpper().ToUpper() == AcqBank_OnlineBankOnly.ToUpper())
                {
                    UWIssues.Text = sbuw.ToString() + "\r\n" + FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));
                }
            }
            else
            {
                this.UWIssues.Text = FormHandler.GetUWIssuesTemplate(wucddlMerchantApptypeUID.SelectedValue, (MerchantBrand)Convert.ToInt32(wucMerchantBrand.SelectedValue), Convert.ToInt32(office.SelectedValue));
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        WucBusinessInfo1.MerchantAppTypeUIDChanged += new
        EventHandler(wucBusinessInfo_MerchantAppTypeUIDChanged);

        WucBusinessInfo1.OfficeIDChanged += new EventHandler(wucBusinessInfo_OfficeIDChanged);

        wucMerchantCategories1.pnl.Visible = false;
        WebUtil.SetUserSpecificDisplayMode(BusinessStartDate);

        if (UserSessions.CurrentMerchantApp != null)
        {
            // UserSessions.CurrentMerchantApp.listOfficeAccess

            //List<CommonUtility.Util.Offices> MerchantOfficeSearch = new List<CommonUtility.Util.Offices>();
            //MerchantOfficeSearch.Add(UserSessions.CurrentMerchantApp.Office);
            //foreach (int loop in UserSessions.CurrentMerchantApp.listOfficeAccess)
            //{
            //    MerchantOfficeSearch.Add((CommonUtility.Util.Offices)loop);
            //}
            //// var t =  MerchantOfficeSearch.Find(x => x.Equals(UserSessions.CurrentUser.Office));

            //if ((UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.LosAngeles)
            //              && (!UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles)))
            //              && (MerchantOfficeSearch.Find(x => x.Equals(UserSessions.CurrentUser.Office)) == 0))
            //{
            //    HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
            //}          

            //Added for Validation for phone number, Country Code and Extention 
            DropDownList office = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");

            if (office.SelectedItem.ToString() == "Irvine (US)")
            {
                string strCustomerServicePhone = CommonUtility.Util.GetNumbersFromString(CustomerServicePhone.Value.ToString());
                CustomerServicePhone.Value = strCustomerServicePhone.ToString();
                CustomerServicePhone.InputMask = "000-000-0000";
            }
            else
            {
                CustomerServicePhone.InputMask = "############";
            }

            DataMerchantApp objMerchant = new DataMerchantApp();
            bool Validation = objMerchant.GMAValidationCheck(UserSessions.CurrentUser, UserSessions.CurrentMerchantApp);

            if (!Validation)
            {
                HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
            }
            else
            {
                if (!this.IsPostBack)
                {

                    this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Profile);

                    LookupTableHandler.LoadBusinessStructures(BusinessStructureUID, false);
                    LookupTableHandler.LoadReasonChanges(ReasonChangesUID, false);
                    //code changes for PXP-10225 by koshlendra
                    if (UserSessions.CurrentMerchantApp != null && (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                    {
                        LookupTableHandler.LoadRefundPolicies(WFReturnPoliciesUID);
                        LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, UserSessions.CurrentMerchantApp.MerchantAppUID);
                    }
                    else
                    {
                        if (UserSessions.CurrentMerchantApp == null)
                            LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, null);
                    }

                    LookupTableHandler.LoadReturnPolicies(ReturnPoliciesUID, false);
                    LookupTableHandler.LoadApplicationTypes(ApplicationTypeUID, false);
                    LookupTableHandler.LoadCurrencyCodes(BankCurrency, false);
                    // DM-7217 Ahmer Bashir
                    LookupTableHandler.LoadCurrencyCodes(ddlDepositBankCurrency, false);

                    //Set Adding flag
                    //WucBusinessInfo1.Adding = this.Adding = Convert.ToBoolean(Request["Adding"]);
                    //if (this.Adding)
                    //{
                    //    this.FormNew();
                    //}
                    //else
                    //{
                    //if (UserSessions.CurrentMerchantApp != null)
                    //{
                    UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

                    this.FormShow(this.UID);
                    //}
                    //}

                    if (UserSessions.CurrentUser.IsBank)
                    {
                        pnlTerminals.Visible = false;
                        pnlAgentInfo.Visible = false;
                    }
                }
            }
        }
        else
        {
            if (!this.IsPostBack)
            {

                this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Profile);

                LookupTableHandler.LoadBusinessStructures(BusinessStructureUID, false);
                LookupTableHandler.LoadReasonChanges(ReasonChangesUID, false);
                //code changes for PXP-10225 by koshlendra
                if (UserSessions.CurrentMerchantApp != null && (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {
                    LookupTableHandler.LoadRefundPolicies(WFReturnPoliciesUID);
                    LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, UserSessions.CurrentMerchantApp.MerchantAppUID);
                }
                else
                {
                    if (UserSessions.CurrentMerchantApp == null)
                        LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, null);
                }
                LookupTableHandler.LoadReturnPolicies(ReturnPoliciesUID, false);
                LookupTableHandler.LoadApplicationTypes(ApplicationTypeUID, false);
                LookupTableHandler.LoadCurrencyCodes(BankCurrency, false);
                // DM-7217 Ahmer Bashir
                LookupTableHandler.LoadCurrencyCodes(ddlDepositBankCurrency, false);

                //Set Adding flag
                WucBusinessInfo1.Adding = this.Adding = Convert.ToBoolean(Request["Adding"]);
                if (this.Adding)
                {
                    this.FormNew();
                }

                if (UserSessions.CurrentUser.IsBank)
                {
                    pnlTerminals.Visible = false;
                    pnlAgentInfo.Visible = false;
                }

            }
        }

        if (UserSessions.CurrentMerchantApp != null)
        {
            if (UserSessions.CurrentUser.OfficeAccess.Where(x => x.OfficeID == Convert.ToInt32(CommonUtility.Util.Offices.LosAngeles)).ToList().Count == 0
                && (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.LosAngeles)))
            {
                FormHandler.SetGMADisable(btnEdit, false);
            }



        }

        // 


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        CallingCodeDisplay.ReadOnly = true;
        string m_StatusUID = string.Empty;

        //if account has bank as ACH_Only and achid > 0 then accoutn is considered as ach only account 
        //in that case we consider the ach status
        if (isACHonly && UserSessions.ActiveAchMerchant != null)
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID;
        else if (UserSessions.CurrentMerchantApp != null)
            m_StatusUID = UserSessions.CurrentMerchantApp.StatusUID;

        if (!string.IsNullOrWhiteSpace(m_StatusUID))
        {
            if (m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_SS_RECEIVED)) // RM - Received
            {
                this.Gatewayonly.Enabled = this.EditMode;
            }
            else
            {
                this.Gatewayonly.Enabled = false;
            }
        }
    }

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);

        this.AccountGroups.ZID = agreement.ID;
        int zid;
        int.TryParse(agreement.ID, out zid);

        this.UID = agreement.MerchantAppUID;
        UserSessions.CurrentMerchantApp = agreement;

        AchMerchant achmerchant = null;

        DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
        DropDownList status = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");

        DropDownList BillingMethod = (DropDownList)WucBusinessInfo1.FindControl("BillingMethodUID");

        string m_StatusUID = string.Empty;
        string m_StatusName = string.Empty;

        //if account has bank as ACH_Only and achid > 0 then account is considered as ach only account 
        //in that case we consider the ach status
        if (isACHonly)
        {
            achmerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(CommonUtility.Util.if_i(agreement.ID, 0));
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = achmerchant;

            if (achmerchant != null)
            {
                LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", achmerchant.MerchantStatusName.Substring(0, 2));
                ListHandler.ListFindItem(achstatus, achmerchant.MerchantStatusUID);
                m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID;
                m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
            }
        }
        else
        {
            m_StatusUID = UserSessions.CurrentMerchantApp.StatusUID;
            m_StatusName = UserSessions.CurrentMerchantApp.StatusName;
        }

        // Added by Chandra for PXP-7898
        hidStatus.Value = m_StatusUID.ToUpper();

        this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Profile");


        if (isACHonly && achmerchant != null)
        {
            if (!(UserSessions.CurrentUser.UserName.ToLower() == "mnguyen" //Added so mark can change status to different queues
                || UserSessions.CurrentUser.UserName.ToLower() == "vpatel" //Added vishal and wilson so that they can change status as well
                || UserSessions.CurrentUser.UserName.ToLower() == "wnguy"))
                LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", agreement, achmerchant);
        }
        else
        {

            if (!(UserSessions.CurrentUser.UserName.ToLower() == "mnguyen" //Added so mark can change status to different queues
                || UserSessions.CurrentUser.UserName.ToLower() == "vpatel" //Added vishal and wilson so that they can change status as well
                || UserSessions.CurrentUser.UserName.ToLower() == "wnguy"))
                LookupTableHandler.MerchantAppStatus(status, false, "Merchant Management", agreement.StatusName.Substring(0, 2), agreement);
        }

        if (agreement.Brand == MerchantBrand.Optimal)
        {
            this.Gatewayonly.Visible = true;
            this.lblGatewayonly.Visible = true;
            //enable the Gatewayonly checkbox when the status is in RM Received. This is disabled for other status.
            if (m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_SS_RECEIVED)) // RM - Received
            {
                this.Gatewayonly.Enabled = true;
            }
        }


        status.Enabled = true;

        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        UserControl wucQIRLookup = (UserControl)WucEquipment.FindControl("QIUlookup");
        Button btnLookUp = (Button)wucQIRLookup.FindControl("btnLookup");
        btnLookUp.Enabled = false;

        WucBusinessInfo1.SelectButton.Enabled = this.EditMode;
        AgentID.Text = agreement.AgentID.ToString();

        CallingCodeDisplay.Text = string.IsNullOrEmpty(agreement.CustomerServicePhoneCallingCode) ? "+1" : agreement.CustomerServicePhoneCallingCode;

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID.ToUpper());

        MasterPageMerchant master = (MasterPageMerchant)this.Master;
        master.ShowNotes(agreement.UWNotes, agreement.AgentMemo, agreement.FirstTeamNotes, this.EditMode);

        WucSeasonalMonths.LoadSeasonalMonths(ID, 3);

        DropDownList discount = (DropDownList)WucBusinessInfo1.FindControl("DiscountMethod");
        discount.Enabled = false;

        DropDownList ReleaseMethodUID = (DropDownList)WucBusinessInfo1.FindControl("ReleaseMethodUID");
        ReleaseMethodUID.Enabled = false;

        DropDownList paymentSchedule = (DropDownList)WucBusinessInfo1.FindControl("PaymentScheduleID");
        paymentSchedule.Enabled = false;

        DropDownList paymentFrequency = (DropDownList)WucBusinessInfo1.FindControl("PaymentFrequencyID");
        paymentFrequency.Enabled = false;

        //First Team ddl changes require special permissons
        TextBox firstTeamRep = (TextBox)WucBusinessInfo1.FindControl("FirstTeamRep");
        if (!PaymentXP.BusinessObjects.User.HasFirstTeamEditPermission(UserSessions.CurrentUser))
        {
            if (firstTeamRep != null)
                firstTeamRep.Enabled = false;
        }

        //Load Equipments
        WucEquipment.LoadEquipments(agreement.MerchantAppUID);

        //categories
        wucMerchantCategories1.Visible = true;
        wucMerchantCategories1.pnlCat.Enabled = true;
        FormHandler.SetControlEditMode(wucMerchantCategories1, true);

        this.LeadID = agreement.LeadsUID;

        if (!string.IsNullOrEmpty(this.LeadID))
        {
            this.LeadID = agreement.LeadsUID;
            LeadsID.Text = agreement.LeadsID;
        }
        else
        {
            LeadsID.Text = string.Empty;
        }

        wucMerchantCategories1.MerchantAppUID = agreement.MerchantAppUID;
        wucMerchantCategories1.FormServices();

        if (string.IsNullOrWhiteSpace(AccountName.Text))
        {
            AccountName.Text = agreement.BusinessDBAName;
        }

        DropDownList office = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");
        ListHandler.ListFindItem(office, Convert.ToString((int)agreement.Office));

        //Added for Validation for phone number, Country Code and Extention 
        WebMaskEditor BusinessDBAPhonetxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessDBAPhone");
        WebMaskEditor BusinessFaxtxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessFax");

        if (agreement.Office == CommonUtility.Util.Offices.Irvine)
        {
            string strBusinessDBAPhonetxt = CommonUtility.Util.GetNumbersFromString(agreement.BusinessDBAPhone);
            BusinessDBAPhonetxt.Value = strBusinessDBAPhonetxt.ToString();
            BusinessDBAPhonetxt.InputMask = "000-000-0000";

            string strBusinessFaxtxt = CommonUtility.Util.GetNumbersFromString(agreement.BusinessFax);
            BusinessFaxtxt.Value = strBusinessFaxtxt.ToString();
            BusinessFaxtxt.InputMask = "000-000-0000";

            string strCustomerServicePhone = CommonUtility.Util.GetNumbersFromString(agreement.CustomerServicePhone);
            CustomerServicePhone.Value = strCustomerServicePhone.ToString();
            CustomerServicePhone.InputMask = "000-000-0000";
        }
        else
        {
            BusinessDBAPhonetxt.InputMask = "############";
            BusinessFaxtxt.InputMask = "############";
            CustomerServicePhone.InputMask = "############";
            CustomerServicePhone.Text = agreement.CustomerServicePhone;
        }

        wucContact1.EditMode = this.EditMode;
        wucContact1.ObjectID = zid;
        wucContact1.officeID = int.Parse(office.SelectedValue);
        wucContact1.FormShow("", false);

        ValidDescriptors.grdDesc.Enabled = true;
        ValidDescriptors.ddpPageSize.Enabled = true;

        if (!string.IsNullOrWhiteSpace(agreement.LeadsID))
        {
            lbRemoveMerchant.Visible = true;
            DisableLinkButton(lbRemoveMerchant, this.EditMode);
        }

        btnSelect.Enabled = this.EditMode;

        bool CCApproved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED) && !isACHonly;
        bool ACHApproved = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_APPROVED) && isACHonly;


        // CU-Approved Lockdown! only members of role "Special Access" can edit these fields once an account has been approved.
        // 03/18 - Lockdown the page when the status is in OP||DP||MS, as there are scenarios now when data is loaded from Optimal,
        //          the Merchants do not have CU- Approved status, Also the Gatewayonly Merchants will skip the CU status going forward.
        if (this.EditMode == true && ((CCApproved || ACHApproved) || (m_StatusName.Substring(0, 2).ToUpper() == "OP") || (m_StatusName.Substring(0, 2).ToUpper() == "DP") || (m_StatusName.Substring(0, 2).ToUpper() == "MS")))
        {
            bool has_special_access_role = (UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_SPECIALACCESS)
                && UserSessions.CurrentUser.UserRoles[Constants.ROLE_SPECIALACCESS].Enabled);

            if (!has_special_access_role)
            {
                // at first, we wanted to just control certains sections of the page. 
                // then we decided to lock down the entire page (by disabling the edit button)
                // but doing that prevented people from changing the status.
                // so now we're just going to selectively turn controls on.
                FormHandler.SetControlEditMode(pnlDetail, false);

                // selectively turn on status, because other people need to change it.
                DropDownList ddlStatus = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
                ddlStatus.Enabled = true;

                DropDownList ddlACHStatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
                ddlACHStatus.Enabled = true;

                //SetControlEditMode does not have logic to disable a linked button and 
                //we do not want to include it there as it is centralized and will disable all ink buttons on contact schema as well.
                LinkButton lnkbutton = (LinkButton)this.AccountGroups.FindControl("btnSelect");
                lnkbutton.Enabled = !EditMode;

                // selectively turn on contancts.. because you can't see the other contacts if its not enabled.
                FormHandler.SetControlEditMode(wucContact1, true);

                DisableLinkButton(lbRemoveMerchant, false);
                btnSelect.Enabled = false;

                //Start PXP-11879
                if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_OPERATIONS && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {
                    BillingMethod.Enabled = EditMode;
                    SuppressProcessingStatements.Enabled = EditMode;
                    ChargebackExcessiveFeeWaived.Enabled = EditMode;
                    WaiveOtherItemFee.Enabled = EditMode;
                }
                //End PXP-11879
            }

            else
            {
                LinkButton lnkbutton = (LinkButton)this.AccountGroups.FindControl("btnSelect");
                lnkbutton.Enabled = EditMode;

            }
        }
        else
        {
            LinkButton btn = (LinkButton)AccountGroups.FindControl("btnSelect");
            btn.Enabled = this.EditMode;

            // you can only edit when in edit mode, and when saleforce id is 0
            SalesForceID.ReadOnly = (UserSessions.CurrentMerchantApp.SalesForceID == 0 && this.EditMode) ? false : true;
            enableMCCByRoles(this.EditMode, m_StatusName);//code added for PXP-8254 
                                                          //PXP-11701 end
            if (agreement.Office.Equals(CommonUtility.Util.Offices.Irvine))
            {
                BillingMethod.Enabled = EditMode;
                SuppressProcessingStatements.Enabled = EditMode;
                ChargebackExcessiveFeeWaived.Enabled = EditMode;
                WaiveOtherItemFee.Enabled = EditMode;
            }
            else
            {
                BillingMethod.Enabled = false;
                SuppressProcessingStatements.Enabled = false;
                ChargebackExcessiveFeeWaived.Enabled = false;
                WaiveOtherItemFee.Enabled = false;
            }
            //PXP-11701 end
        }

        WucBusinessInfo1.FormShow(this.EditMode);
        PopulateProductSummary(zid);

        this.MCClookup.m_SicCode = agreement.SicCode;
        this.MCClookup.m_SicCodeDesc = agreement.SicCodeDesc;
        this.MCClookup.txtSicCodeDescReadonly = true;
        this.MCClookup.txtSicCodeReadonly = true;
        //PXP-12935-satrt by koshlendra
        this.MCClookup.m_VisaSicCode = agreement.VisaSicCode;
        this.MCClookup.m_VisaSicCodeDesc = agreement.VisaSicCodeDesc;
        this.MCClookup.txtVisaSicCodeDescReadonly = true;
        this.MCClookup.txtVisaSicCodeReadonly = true;
        //PXP-14480 by Satyajit
        if (agreement.IsPCCSSwitch != null)
            this.chkPCCSSwitch.Checked = agreement.IsPCCSSwitch;
        //PXP-12935-end by koshlendra
        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper().Equals(Constants.ROLE_BANK))
        {
            this.pnlAccGroups.Visible = false;
        }

        //PXP-9348 RThakur start
        hiddenCrmStatus.Value = agreement.CRMStatus;
        hiddenAcceptTransaction.Value = Convert.ToString(agreement.CRMAcceptTransactions);
        //PXP-9348 RThakur end

        Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(agreement.MerchantAppUID);
        // if UWIssues is empty, we prefill it with a default free form "template".
        if (objUW == null || (objUW != null && string.IsNullOrWhiteSpace(objUW.UWIssues)))
        {
            StringBuilder sbuw = new StringBuilder();
            StringBuilder sbcc = new StringBuilder();
            string line_template = "{0}: {1}";
            if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
            {

                sbcc.AppendLine(string.Format(line_template, "General Set up Details", ""));
                sbcc.AppendLine(string.Format(line_template, "Billing type", ""));
                sbuw.Append(sbcc.ToString()).ToString();
                UWIssues.Text = sbuw.ToString();
            }
            else
            {
                //Code changes done for PXP-15968 with decode the string by koshlendra start
                UWIssues.Text = System.Web.HttpUtility.HtmlDecode(FormHandler.GetUWIssuesTemplate(UserSessions.CurrentMerchantApp));
                //Code changes done for PXP-15968 with decode the string by koshlendra end
            }

        }

        else
        {
            //Code changes done for PXP-15968 with decode the string by koshlendra start
            this.UWIssues.Text = System.Web.HttpUtility.HtmlDecode(objUW.UWIssues);
            //Code changes done for PXP-15968 with decode the string by koshlendra end
        }

        //start :code added for IsInCollection flag****
        if (UserSessions.CurrentUser != null)
        {
            if (((agreement.IsInCollection && agreement.IsWithAgency) || agreement.IsWithAgency) && UserSessions.CurrentUser.IsInternal)
            {
                pnlYellowBanner.Visible = true;
                pnlRedBanner.Visible = false;
            }
            else if (agreement.IsInCollection && UserSessions.CurrentUser.IsInternal)
            {
                pnlRedBanner.Visible = true;
                pnlYellowBanner.Visible = false;
            }
            else
            {
                pnlRedBanner.Visible = false;
                pnlYellowBanner.Visible = false;
            }

            //Ani: DM-5746
            if(agreement.SOSDivertStatus)
                pnlOrangeBanner.Visible = true;
            else
                pnlOrangeBanner.Visible = false;

            if (agreement.RiskDivertStatus)
                pnlRiskDivertStatusBanner.Visible = true;
            else
                pnlRiskDivertStatusBanner.Visible = false;



            UserFacade userFacade = new UserFacade();
            var userRoles = userFacade.GetUser(UserSessions.CurrentUser.UID).UserRoles.Where(u => u.Value.Enabled == true);  // Dynamic list of enabled user roles;

            rowInCollection.Visible = userRoles.Any(s => s.Value.RoleID == Constants.ROLE_RISK) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_SPECIALACCESS)
                || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_DEPLOYMENT) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_CLIENT_SERVICES)
                || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_ADMIN);
            btnPDF.Visible = userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_CREDIT_UNDERWRITING));


            //start code added for PXP-15331
            rowWithAgency.Visible = userRoles.Any(s => s.Value.RoleID == Constants.ROLE_RISK) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_SPECIALACCESS)
                || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_DEPLOYMENT) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_CLIENT_SERVICES)
                || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_ADMIN);
            //end code added for PXP-15331

            //PXP-5444 abarua
            //code updated for PXP-18392 by koshlendra
            if (UserSessions.CurrentUser.DefaultRoleUID.ToLower() == Constants.ROLE_COLLECTIONS.ToLower() && this.EditMode)
            {
                IsInCollection.Enabled = true;
                IsWithAgency.Enabled = true;
            }
            else
            {  
                IsInCollection.Enabled = false;
                IsWithAgency.Enabled = false;
            }

            //PXP-8253:START: Add New Vertical Checkbox in profile page By Ali Khan
            if (userRoles.Any(s => s.Value.RoleID == Constants.ROLE_SALES_SUPPORT) && this.EditMode && !m_StatusName.ToUpper().StartsWith("OP") && !m_StatusName.ToUpper().StartsWith("DP") && !m_StatusName.ToUpper().StartsWith("MS"))
            {
                IsNewVertical.Enabled = true;
            }
            else
            {
                IsNewVertical.Enabled = false;
            }
            //PXP-8253:END: Add New Vertical Checkbox in profile page By Ali Khan
            // Start PXP-6524 Rohit Thakur 
            //code changes for PXP-10225 by koshlendra
            if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST 
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) &&
                UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine) &&
                (m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED) || m_StatusName.ToUpper().StartsWith("OP") ||
                m_StatusName.ToUpper().StartsWith("DP") || m_StatusName.ToUpper().StartsWith("MS")))
            {

                FormHandler.SetControlEditMode(pnlAdvertisingSalesDelivery, this.EditMode);
                if (userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_RISK)) || userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_CREDIT_UNDERWRITING)))
                {
                    pnlAdvertisingSalesDelivery.Enabled = true;
                }
                else
                {
                    pnlAdvertisingSalesDelivery.Enabled = false;
                }
            }
            // End PXP-6524
            //Start Code added for PXP-13345
            if (this.EditMode && UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_RISK && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine) && (m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_ACTIVE.ToUpper() || m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_INACTIVE.ToUpper()))
            {
                AccountClosureRisk.Enabled = true;
            }
            else
            {
                AccountClosureRisk.Enabled = false;
            }
            //end code added for PXP-13345
            //PXP-17195 Start
            bool CCReceived = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_RECEIVED) && !isACHonly;
            bool ACReceived = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_RECEIVED) && isACHonly;
            // if (userRoles.Any(s => s.Value.RoleID == Constants.ROLE_SALES_SUPPORT) && this.EditMode && !CCReceived && !ACReceived)
            if (this.EditMode && !CCReceived && !ACReceived)
            {
                DropDownList primaryContactUID = (DropDownList)WucBusinessInfo1.FindControl("PrimaryContactUID");
                primaryContactUID.Enabled = true;
            }
            else
            {
                DropDownList primaryContactUID = (DropDownList)WucBusinessInfo1.FindControl("PrimaryContactUID");
                primaryContactUID.Enabled = false;
            }

            //PXP-17195 End

            //DM-1799 Andres Ramos
            DropDownList statusUID = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
            var selectedStatus = statusUID.SelectedItem;
            var boolStatus = selectedStatus.Value.ToUpper() == Constants.QUEUESTATUS_SS_QA
                || selectedStatus.Value.ToUpper() == Constants.QUEUESTATUS_SS_DRAFT
                || selectedStatus.Value.ToUpper() == Constants.QUEUESTATUS_SS_RECEIVED
                || selectedStatus.Value.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE
                || selectedStatus.Value.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN;

            if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_SALES_SUPPORT && this.EditMode && boolStatus)
            {
                DropDownList SSQRep = (DropDownList)WucBusinessInfo1.FindControl("SSQRep");
                SSQRep.Enabled = true;
            }
            else
            {
                DropDownList SSQRep = (DropDownList)WucBusinessInfo1.FindControl("SSQRep");
                SSQRep.Enabled = false;
            }
            //END DM-1799

            //DM-5745 --raul vazquez
            RiskDivertStatus.Enabled = this.EditMode && userRoles.Any(s => s.Value.RoleID == Constants.ROLE_RISK);
        }
        //end :code added for IsInCollection flag****
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (!this.btnSave.Enabled && !this.EditMode))
        {
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);

        }

        //PXP-6182 start
        //code changes for PXP-10225 by koshlendra
        if (UserSessions.CurrentMerchantApp != null && (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST 
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {

            if (CommonUtility.Util.if_dec(agreement.Delivery07, 0M).Equals(100M))
                ListHandler.ListFindItem(DeliveryTime, "Delivery07");
            else if (CommonUtility.Util.if_dec(agreement.Delivery08, 0M).Equals(100M))
                ListHandler.ListFindItem(DeliveryTime, "Delivery08");
            else if (CommonUtility.Util.if_dec(agreement.Delivery15, 0M).Equals(100M))
                ListHandler.ListFindItem(DeliveryTime, "Delivery15");
            else if (CommonUtility.Util.if_dec(agreement.Delivery30, 0M).Equals(100M))
                ListHandler.ListFindItem(DeliveryTime, "Delivery30");

            foreach (ListItem item in WFReturnPoliciesUID.Items)
            {
                if (item.Value == agreement.WFReturnPoliciesUID)
                    item.Selected = true;
            }

            LookupTableHandler.LoadPurchaseProducts(PurchaseOrOrderProduct, UserSessions.CurrentMerchantApp.MerchantAppUID);
            if (RefundPolicyAwareness.SelectedValue == "0")
            {
                trRefundPolicyAwarenessReason.Visible = true;
            }
            else
            {
                trRefundPolicyAwarenessReason.Visible = false;
            }

        }
        //PXP-6182 end

        /******** End of PXP-2206 **************/

        //PXP-9051 RThakur

        if (!FormHandler.SetHdnValueIsAnyVerticalMarketsChecked(m_StatusName, m_StatusUID, agreement.IsNewVertical))
        {
            IsNewVerticalandMarkets.Value = "false";
        }
        else
        {
            IsNewVerticalandMarkets.Value = "true";
        }
        //PXP-14480
        if (this.EditMode)
        {
            chkPCCSSwitch.Enabled = false;
        }
        WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621

        //Code added by Gabriel Gonzalez for DM-4647, DM-4691
        AccountNumber.Text = agreement.AccountNumber;
        AccountTmp.Text = agreement.AccountNumberMask;
        // DM-7217 Ahmer Bashir
        FillDepositsSection(agreement);
        if (HasBankAccountRole)
        {
            if (EditMode)
            {
                FormHandler.SetControlEditMode(pnlBank, PnlBankField.IsEnabled);
                AccountTmp.Text = PnlBankField.IsVisible ? agreement.AccountNumber : agreement.AccountNumberMask;
                //DM-7217 AhmerBashir
                txtDepositAccountNumberMask.Text = PnlBankField.IsVisible ? agreement.DepositAccountNumber : agreement.DepositAccountNumberMask;
            }
        }
        else
            FormHandler.SetControlEditMode(pnlBank, false);
        //Code added by Gabriel Gonzalez for DM-4647, DM-4691

        //DM-5125 ini
        IsDMAllocation.Visible = CheckBoxAllocationField.IsVisible;
        IsDMAllocation.Enabled = this.EditMode && CheckBoxAllocationField.IsEnabled;
        //DM-5125 end

        //DM-5745 --raul vazquez
        SOSDivertStatus.Enabled = false;
    }


    /// <summary>
    /// Disables the link button.
    /// </summary>
    /// <param name="linkButton">The LinkButton to be disabled.</param>
    public static void DisableLinkButton(LinkButton linkButton, bool editmode)
    {
        linkButton.Enabled = editmode;

        if (!editmode)
            linkButton.OnClientClick = "return false";
        else
            linkButton.OnClientClick = "return confirm('Are you sure you want to delete the Lead ID?')";
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
        WucEquipment.FormClear();
    }

    public override bool FormSave()
    {
        bool perform = false;
        DataSet dsmultiLink = new DataSet();
        MerchantApp agreement = null;

        try
        {
            //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  start
            string oldSicCode = string.Empty;

            if (UserSessions.CurrentMerchantApp != null)
                oldSicCode = UserSessions.CurrentMerchantApp.SicCode;

            //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  end
            if (isValid)
            {
                string oldID = string.Empty;
                string oldLeadID = string.Empty;

                if (this.Adding)
                    agreement = new MerchantApp();
                else
                    agreement = (MerchantApp)UserSessions.CurrentMerchantApp;

                agreement.CloneMerchantApp();

                var DBABefore = agreement.BusinessDBAName;
                var EmailBefore = agreement.BusinessEmailAddress;

                string oldETfAssessed = agreement.MerchantAppClone.ETFAssessed;
                oldLeadID = agreement.MerchantAppClone.LeadsID;

                FormBinding.BindControlsToObject(agreement, pnlDetail);
                var DBAAfter = agreement.BusinessDBAName;
                MerchantFacade merchantFacade = new MerchantFacade();
                if (!this.Adding)
                {
                    //PXP-3526
                    MerchantApp currntMerchantApp = merchantFacade.GetMerchantAppZeus(agreement.MerchantAppUID);
                    agreement.EMVComplianceMerchant = currntMerchantApp.EMVComplianceMerchant;


                }


                //Asheesh/ Write a method to get office access detail. 
                List<int> officeAccess = new List<int>();
                CheckBoxList officelist = (CheckBoxList)WucBusinessInfo1.FindControl("listOfficeAccess");
                bool flag = true;

                foreach (ListItem item in officelist.Items)
                {

                    if (item.Selected)
                    {
                        if (flag)
                        {
                            agreement.listOfficeAccess.Clear();
                            flag = false;
                        }

                        agreement.listOfficeAccess.Add(int.Parse(item.Value));

                    }
                }

                AchMerchant achMerchant = null;
                DropDownList ACHStatusUID = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");

                //Added Business DBA and customer service phone country code and extentions
                WebMaskEditor BusinessDBAPhonetxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessDBAPhone");
                agreement.BusinessDBAPhone = BusinessDBAPhonetxt.Text;

                DropDownList BillingMethod = (DropDownList)WucBusinessInfo1.FindControl("BillingMethodUID");
                agreement.BillingMethodUID = BillingMethod.SelectedValue;

                WebMaskEditor BusinessDBAPhoneExttxt = (WebMaskEditor)WucBusinessInfo1.FindControl("DBAPhoneExt");
                agreement.BusinessDBAPhoneExt = BusinessDBAPhoneExttxt.Text;

                agreement.CustomerServicePhone = CustomerServicePhone.Text;

                agreement.CustomerServicePhoneExt = CustomerServicePhoneExt.Text;
                //PXP-17195- Start
                if (UserSessions.CurrentMerchantApp != null)
                {
                    bool CURecieved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_RECEIVED);
                    bool ACHRecieved = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_RECEIVED);
                    string ACHStatus = ACHStatusUID.SelectedValue;
                    DropDownList StatusUID = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
                    string m_Status = StatusUID.SelectedValue;

                    if (!CURecieved && !ACHRecieved && !(ACHStatus.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED.ToUpper())) && !(m_Status.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED.ToUpper())))
                    {
                        DropDownList PrimaryContactUID = (DropDownList)WucBusinessInfo1.FindControl("PrimaryContactUID");
                        if (PrimaryContactUID != null && PrimaryContactUID.SelectedIndex == 0)
                            agreement.PrimaryContactUID = PrimaryContactUID.SelectedValue;
                    }
                }
                //PXP-17195- End

                //PXP-15423: Bug Fix: Start
                if (!this.Adding)
                {
                    DropDownList cboStatus = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
                    if (cboStatus.SelectedValue.ToString().ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION)
                    {
                        DropDownList cbo = (DropDownList)WucBusinessInfo1.FindControl("MerchantClosureCodeUID");
                        if (cbo != null)
                        {
                            agreement.MerchantClosureCodeUID = cbo.SelectedValue;
                            agreement.MerchantClosureCode = cbo.SelectedItem.Text;
                        }
                    }
                }

                //PXP-15423: Bug Fix: End

                //Added Business Fax country code and extentions
                WebMaskEditor BusinessFaxExttxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessFaxExt");
                agreement.BusinessFaxExt = BusinessFaxExttxt.Text.ToString();

                if (UserSessions.ActiveAchMerchant != null && isACHonly)
                {
                    UserSessions.ActiveAchMerchant.CloneAchMerchant();
                    achMerchant = UserSessions.ActiveAchMerchant;
                    achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;
                    achMerchant.MerchantStatusUID = ACHStatusUID.SelectedValue;
                }
                agreement.SicCode = this.MCClookup.m_SicCode;
                agreement.SicCodeDesc = this.MCClookup.m_SicCodeDesc;

                //code added by koshlendra for PXP-12935 start
                agreement.VisaSicCode = this.MCClookup.m_VisaSicCode;
                agreement.VisaSicCodeDesc = this.MCClookup.m_VisaSicCodeDesc;
                //code added by koshlendra for PXP-12935 end

                agreement.NotificationEmails = wucContact1.GetNotificationEmails;
                agreement.DisableRDRVerifi = wucContact1.GetDisableSendRDRNotifi;

                agreement.UWNotes = ((TextBox)this.Master.FindControl("UWNotesEdit")).Text;
                agreement.FirstTeamNotes = ((TextBox)this.Master.FindControl("FirstTeamNotesEdit")).Text;

                if (!string.IsNullOrEmpty(BankCode.Text.Trim()))
                {
                    string BankCodeText = this.BankCode.Text.Trim();
                    agreement.BankCode = BankCodeText.Length < 4 ? BankCodeText.PadLeft(4, '0') : BankCodeText.Trim();
                }

                if (!string.IsNullOrEmpty(BankBranchCode.Text.Trim()))
                {
                    string BankBranchCodeText = this.BankBranchCode.Text.Trim();
                    agreement.BankBranchCode = BankBranchCodeText.Length < 5 ? BankBranchCodeText.PadLeft(5, '0') : BankBranchCodeText.Trim();
                }
                //PXP-6182 start
                //Code changes for PXP-10225 by koshlendra
                if ((agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && agreement.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {

                    agreement.Delivery07 = 0M;
                    agreement.Delivery08 = 0M;
                    agreement.Delivery15 = 0M;
                    agreement.Delivery30 = 0M;

                    //when bank is harris then we replace exisitng time frame question with a new one as per PDF
                    if (DeliveryTime.SelectedValue == "Delivery07")
                        agreement.Delivery07 = 100M;
                    else if (DeliveryTime.SelectedValue == "Delivery08")
                        agreement.Delivery08 = 100M;
                    else if (DeliveryTime.SelectedValue == "Delivery15")
                        agreement.Delivery15 = 100M;
                    else if (DeliveryTime.SelectedValue == "Delivery30")
                        agreement.Delivery30 = 100M;
                }
                //PXP-6182 end

                if (!this.IsCopy)
                {
                    UserSessions.CurrentMerchantApp = agreement;
                }

                MerchantFacade facade = new MerchantFacade();
                MerchantAccountFee accountFee = new MerchantAccountFee();
                //Get Accout fee for ETF Assessed logic.
                // TO DO : Analyze if this can be removed. Only changed based on earlier logic.
                if (!this.Adding)
                {
                    accountFee = facade.GetMerchantAccountFee(Convert.ToInt32(agreement.ID));
                    agreement.AccountFee = accountFee;
                }

                if (agreement.StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION || agreement.StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
                {
                    if (agreement.AccountFee != null)
                    {
                        if (agreement.AccountFee.EarlyTerminationWaived)
                        {
                            if (oldETfAssessed == agreement.ETFAssessed)
                                agreement.ETFAssessed = "W";
                        }
                    }
                }

                CommonUtility.Util.Offices officeClone = agreement.Office;

                DropDownList office = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");
                int officeid;
                int.TryParse(office.SelectedValue, out officeid);

                agreement.Office = (CommonUtility.Util.Offices)officeid;

                //7217 Ahmer Bashir
                agreement.DepositAccountName = txtDepositAccountName.Text;
                agreement.DepositBankName = txtDepositBankName.Text;
                agreement.DepositRoutingNumber = txtDepositRoutingNumber.Text;
                agreement.DepositAccountNumber = txtDepositAccountNumber.Text;
                agreement.DepositBankCurrency = ddlDepositBankCurrency.SelectedValue;

                //Added for Validation for phone number, Country Code and Extention 
                List<string> messages = wucContact1.FormDataCheck(agreement);
                if (!this.FormDataCheck(agreement) || messages.Count > 0)
                {
                    if (messages.Count > 0)
                    {
                        foreach (string mess in messages)
                            this.Master.AddMessageError(mess);
                    }

                    return false;
                }

                DataMerchantApp data = DataAccess.DataMerchantAppDao;
                PaymentXP.BusinessObjects.User user = UserSessions.CurrentUser;

                if (string.IsNullOrWhiteSpace(agreement.AccountName))
                {
                    agreement.AccountName = agreement.BusinessDBAName;
                }

                BulletedList Temp = (BulletedList)AccountGroups.FindControl("AccountGroups1");

                string AccountGroupsSelectedNames = null;
                foreach (ListItem item in Temp.Items)
                {
                    AccountGroupsSelectedNames += item.Text + ",";
                }

                string Statusuid = string.Empty, m_StatusUID = string.Empty;

                //if account has bank as ACH_Only and achid > 0 then account is considered as ach only account 
                //in that case we consider the ach status
                // get the clone status and status ,whcih will help compare the new and old status
                if (!isACHonly)
                {
                    if (agreement.MerchantAppClone != null)
                        Statusuid = agreement.MerchantAppClone.StatusUID;
                    m_StatusUID = agreement.StatusUID;
                }
                else if (isACHonly)
                {
                    if (achMerchant != null)
                    {
                        if (achMerchant.AchMerchantClone != null)
                            Statusuid = achMerchant.AchMerchantClone.MerchantStatusUID;
                        m_StatusUID = achMerchant.MerchantStatusUID;
                    }
                }


                if (!this.Adding)
                {
                    agreement.DateUpdated = DateTime.Now;
                    agreement.UserUpdated = user.UserName;

                    agreement.SicCode = this.MCClookup.m_SicCode;
                    agreement.SicCodeDesc = this.MCClookup.m_SicCodeDesc;

                    //code changes done by koshlendra for PXP-12935 start
                    agreement.VisaSicCode = this.MCClookup.m_VisaSicCode;
                    agreement.VisaSicCodeDesc = this.MCClookup.m_SicCodeDesc;
                    //code changes done by koshlendra for PXP-12935 end

                    if (WucEquipment.grdCount > 0 || WucEquipment.FormDataCheck())
                        agreement.NoEquipment = false;

                    bool CCApproved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED);

                    //Generate Discover,front and Back MIDs for Harris Bank and                     
                    //if the aplication is moved to CU - Approved Or if it was already in CU- Approved and Bank is changed.
                    if (agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BMO_HARRIS)
                        && (CCApproved || m_StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED.ToUpper())))
                    {
                        //Mid is generated only when the Auth MID and Settle MID are empty and bank is BMOHarris
                        if (string.IsNullOrEmpty(agreement.AuthPlatformMid)
                            && string.IsNullOrEmpty(agreement.SettlePlatformMid))
                        {
                            string MID = FormHandler.GenerateMID(agreement.MerchantAppTypeUID, agreement.Brand);
                            agreement.AuthPlatformMid = MID;
                            agreement.SettlePlatformMid = MID;
                        }

                        //Discover MID is generated
                        if (string.IsNullOrEmpty(agreement.DiscoverMid)
                            && agreement.DiscoverNovus
                        )
                        {
                            string DiscMID = FormHandler.GetDiscoverMID(agreement.MerchantAppTypeUID);
                            agreement.DiscoverMid = DiscMID;
                        }
                    }

                    //START:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan
                    Underwriting underWriting = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(UserSessions.CurrentMerchantApp.MerchantAppUID);
                    string agentLevel = underWriting == null ? string.Empty : underWriting.AgentLevel;
                    agreement = FormHandler.CheckGenerateMIDforWoodforest(agreement, CCApproved, m_StatusUID, agentLevel);
                    //END:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan

                    //start:PXP-10043 Autogenerated MID for BBVA bank By Ksingh
                    if (agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
                    {

                        agreement = FormHandler.CheckGenerateMIDforBBVA(agreement, CCApproved, m_StatusUID);

                    }
                    //END:PXP-10043 Autogenerated MID for BBVA bank By Ksingh
                    else if (agreement.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CHESAPEAKE))
                    {

                        agreement = FormHandler.CheckGenerateMIDforChesapeake(agreement, CCApproved, m_StatusUID);

                    }
                    // agreement.TangibleTrialEnabledSicCodes = facade.GetTangibleTrialEnabledSicCodes();

                    //if (!(agreement.SicCode.Equals(Constants.NUTRA_MCC[0]) || agreement.SicCode.Equals(Constants.NUTRA_MCC[1])))
                    //if (!(agreement.TangibleTrialEnabledSicCodes.Contains(agreement.SicCode)))
                    //{
                    //    agreement.IsNutraMerchant = false;
                    //}

                    //Added the code for PXP-7995, by Chandra
                    //when the checkbox is checked and the status is OP_Received then add the 'Nutra Trial account' into OPs instructions
                    //and when unchecked remove the 'Nutra Trial account' from the Ops instructions
                    if (agreement.IsNutraMerchant)
                    {
                        //Code changes done for PXP-15968 with decode the string by koshlendra start
                        if (!(UWIssues.Text.Contains("'Nutra Trial' account") || UWIssues.Text.Contains("'Tangible Trial' account")))
                        {
                            StringBuilder uwIssue = new StringBuilder();
                            uwIssue.AppendLine(UWIssues.Text);
                            uwIssue.AppendLine(" ");
                            uwIssue.AppendLine("'Tangible Trial' account");
                            UWIssues.Text = uwIssue.ToString();
                        }
                        else if (UWIssues.Text.Contains("'Nutra Trial' account"))
                        {
                            if (agreement.Office == CommonUtility.Util.Offices.Irvine)
                            {
                                UWIssues.Text = UWIssues.Text.Replace("'Nutra Trial' account", "'Tangible Trial' account");
                            }
                        }
                    }
                    else
                    {
                        if (UWIssues.Text.Contains("'Nutra Trial' account"))
                        {
                            UWIssues.Text = UWIssues.Text.Replace("'Nutra Trial' account", "");
                        }
                    }
                    //Commented By Abarua code merge issue
                    //bool moveStatus = (Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED.ToUpper());
                    ////Start code by Anuj for PXP-9311           
                    //if (agreement.MasterMRP)
                    //{
                    //    if (moveStatus)
                    //    {
                    //        StringBuilder opsData = new StringBuilder();
                    //        opsData.AppendLine(UWIssues.Text);

                    //        if (!UWIssues.Text.Contains("'Master MRP' account"))
                    //        {
                    //            opsData.AppendLine("'Master MRP' account");
                    //            UWIssues.Text = opsData.ToString();
                    //        }
                    //        else
                    //        {
                    //            opsData.Replace("'Master MRP' account", string.Empty);
                    //            opsData.AppendLine("'Master MRP' account");
                    //            UWIssues.Text = opsData.ToString();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (UWIssues.Text.Contains("'Master MRP' account"))
                    //    {
                    //        UWIssues.Text = UWIssues.Text.Replace("'Master MRP' account", "");
                    //    }
                    //}                    
                    //if (underWriting.HighRiskRegistered)
                    //{
                    //    if (moveStatus)
                    //    {
                    //        StringBuilder opsInstructionsData = new StringBuilder();
                    //        opsInstructionsData.AppendLine(UWIssues.Text);

                    //        if (!UWIssues.Text.Contains("'High Risk Registered' account"))
                    //        {
                    //            opsInstructionsData.AppendLine("'High Risk Registered' account");
                    //            UWIssues.Text = opsInstructionsData.ToString();
                    //        }
                    //        else
                    //        {
                    //            opsInstructionsData.Replace("'High Risk Registered' account", "");
                    //            opsInstructionsData.AppendLine("'High Risk Registered' account");
                    //            UWIssues.Text = opsInstructionsData.ToString();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (UWIssues.Text.Contains("'High Risk Registered' account"))
                    //    {
                    //        UWIssues.Text = UWIssues.Text.Replace("'High Risk Registered' account", "");
                    //    }
                    //}

                    //UWIssues.Text = RemoveExtraSpace(UWIssues.Text.ToString());
                    ////End code by Anuj for PXP-9311

                    bool moveStatus = (Statusuid.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED.ToUpper());
                    //Start code by Anuj for PXP-9311          
                    if (agreement.MasterMRP)
                    {
                        if (moveStatus)
                        {
                            StringBuilder opsData = new StringBuilder();
                            opsData.AppendLine(UWIssues.Text);

                            if (!UWIssues.Text.Contains("'Master MRP' account"))
                            {
                                opsData.AppendLine("'Master MRP' account");
                                UWIssues.Text = opsData.ToString();
                            }
                            else
                            {
                                opsData.Replace("'Master MRP' account", string.Empty);
                                opsData.AppendLine("'Master MRP' account");
                                UWIssues.Text = opsData.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (UWIssues.Text.Contains("'Master MRP' account"))
                        {
                            UWIssues.Text = UWIssues.Text.Replace("'Master MRP' account", "");
                        }
                    }
                    if (underWriting != null)
                    {
                        if (underWriting.HighRiskRegistered)
                        {
                            if (moveStatus)
                            {
                                StringBuilder opsInstructionsData = new StringBuilder();
                                opsInstructionsData.AppendLine(UWIssues.Text);

                                if (!UWIssues.Text.Contains("'High Risk Registered' account"))
                                {
                                    opsInstructionsData.AppendLine("'High Risk Registered' account");
                                    UWIssues.Text = opsInstructionsData.ToString();
                                }
                                else
                                {
                                    opsInstructionsData.Replace("'High Risk Registered' account", "");
                                    opsInstructionsData.AppendLine("'High Risk Registered' account");
                                    UWIssues.Text = opsInstructionsData.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (UWIssues.Text.Contains("'High Risk Registered' account"))
                            {
                                UWIssues.Text = UWIssues.Text.Replace("'High Risk Registered' account", "");
                            }
                        }
                    }

                    UWIssues.Text = RemoveExtraSpace(UWIssues.Text.ToString());
                    //End code by Anuj for PXP-9311
                
                    //PXP-11452 By sanidhya
                    agreement = FormHandler.ManageDP_SoftwareStatus(agreement);
                    perform = facade.UpdateMerchantApp(agreement) > 0;


                    if (agreement.listOfficeAccess.Count > 0)
                    {
                        perform = facade.SaveMerchantOfficeAccess(agreement) > 0;
                        perform = true;
                    }

                    facade.InsertMerchantAccountGroup(agreement, this.AccountGroups.AccountGroupIds, AccountGroupsSelectedNames, UserSessions.CurrentUser.UserName, Constants.PORTAL_ZEUS);
                    FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);

                    wucContact1.ObjectID = Convert.ToInt32(agreement.ID);
                    wucContact1.FormSave();
                    var EmailAfter = merchantFacade.GetMerchantAppZeus(agreement.MerchantAppUID).BusinessEmailAddress;

                    if ((DBABefore != DBAAfter || EmailBefore != EmailAfter) && perform)
                    {
                        facade.UpdateUserEmailAsMerchant(agreement.ID.ToString(), agreement.MerchantAppUID);
                        if (agreement.IsMFAEnabled && Constants.IS_MFA_ENABLED && !string.IsNullOrEmpty(agreement.OktaUserID))
                        {
                            UpdateOktaUser(new PaymentXP.BusinessObjects.User()
                            {
                                OktaUserID = agreement.OktaUserID,
                                FirstName = agreement.BusinessDBAName,
                                LastName = string.Empty,
                                UserName = agreement.ID.ToString(),
                                Email = EmailAfter
                            }, EmailBefore != EmailAfter);
                        }
                    }
                    TextBox fma = (TextBox)WucBusinessInfo1.FindControl("FMAID");
                    long fmaId;
                    long.TryParse(fma.Text, out fmaId);

                    agreement.FMAID = fmaId;
                    agreement.BankIBAN = BankIBAN.Text;
                    agreement.BankSwiftID = BankSwiftID.Text;
                    if (!string.IsNullOrEmpty(BankCode.Text.Trim()))
                    {
                        string BankCodeText = this.BankCode.Text.Trim();
                        agreement.BankCode = BankCodeText.Length < 4 ? BankCodeText.PadLeft(4, '0') : BankCodeText.Trim();
                    }

                    if (!string.IsNullOrEmpty(BankBranchCode.Text.Trim()))
                    {
                        string BankBranchCodeText = this.BankBranchCode.Text.Trim();
                        agreement.BankBranchCode = BankBranchCodeText.Length < 5 ? BankBranchCodeText.PadLeft(5, '0') : BankBranchCodeText.Trim();
                    }
                    // Removed IF condition for PXP-4398 Display FMAID for gateway=3rd Party and office=IRV                                       
                    data.UpdateFMAMerchant(agreement, user.UserName);

                    if (!string.IsNullOrWhiteSpace(oldETfAssessed.Replace("-", "")) && !string.IsNullOrWhiteSpace(agreement.ETFAssessed.Replace("-1", "")) && agreement.ETFAssessed != oldETfAssessed)
                    {
                        WucBusinessInfo1.SaveBusinessInfo();
                        if (agreement.AccountFee != null)
                        {
                            if (agreement.ETFAssessed == "W")
                                agreement.AccountFee.EarlyTerminationWaived = true;
                            else
                                agreement.AccountFee.EarlyTerminationWaived = false;

                            //Update the account fee as we may have modified the waived indicator for ETF
                            facade.UpdateMerchantAccountFee(agreement, accountFee, user.UserName, Constants.PORTAL_ZEUS);
                        }
                    }

                    if (isACHonly && achMerchant != null)
                    {
                        DataAccess.DataAchMerchantDao.UpdateAchMerchant(achMerchant);
                    }
                    if (agreement.IsNutraMerchant && agreement.Office == CommonUtility.Util.Offices.Irvine && m_StatusUID.ToUpper() != UserSessions.CurrentMerchantApp.MerchantAppClone.StatusUID.ToUpper() && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE)
                    {
                        FormHandler.AllowPxpForNutra();
                    }
                    //Start Code added for PXP-13345
                    if (agreement.AccountClosureRisk && UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_RISK && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine) && (m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_ACTIVE.ToUpper() || m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_INACTIVE.ToUpper()))
                    {
                        if (agreement.MerchantAppClone != null && agreement.AccountClosureRisk != agreement.MerchantAppClone.AccountClosureRisk)
                            CreateAccountCloseRiskTicket(data, agreement);
                    }
                    //End Code added for PXP-13345
                }
                else
                {
                    agreement.DateCreated = DateTime.Now;
                    agreement.UserCreated = user.UserName;

                    string old_mauid = agreement.MerchantAppUID.ToUpper();
                    data.InsertMerchantApp(agreement);

                    if (agreement.listOfficeAccess.Count > 0)
                    {
                        perform = facade.SaveMerchantOfficeAccess(agreement) > 0;
                        perform = true;
                    }

                    if (agreement.MerchantAppUID != "-1")
                    {
                        long fmaId = agreement.FMAID;
                        MerchantFacade facadetemp = new MerchantFacade();
                        agreement = facadetemp.GetMerchantAppZeus(agreement.MerchantAppUID);
                        facade.InsertMerchantAccountGroup(agreement, this.AccountGroups.AccountGroupIds, AccountGroupsSelectedNames, UserSessions.CurrentUser.UserName, Constants.PORTAL_ZEUS);

                        this.UID = agreement.MerchantAppUID.ToUpper();

                        agreement.FMAID = fmaId;
                        agreement.BankIBAN = BankIBAN.Text;
                        agreement.BankSwiftID = BankSwiftID.Text;
                        if (!string.IsNullOrEmpty(BankCode.Text.Trim()))
                        {
                            string BankCodeText = this.BankCode.Text.Trim();
                            agreement.BankCode = BankCodeText.Length < 4 ? BankCodeText.PadLeft(4, '0') : BankCodeText.Trim();
                        }

                        if (!string.IsNullOrEmpty(BankBranchCode.Text.Trim()))
                        {
                            string BankBranchCodeText = this.BankBranchCode.Text.Trim();
                            agreement.BankBranchCode = BankBranchCodeText.Length < 5 ? BankBranchCodeText.PadLeft(5, '0') : BankBranchCodeText.Trim();
                        }
                        data.UpdateFMAMerchant(agreement, user.UserName);

                        if (old_mauid != this.UID)
                        {
                            // uid's are different, so we delete the old copy because it's using session. its copied by reference. not deep copy.
                            if (UserSessions.diCurrentMerchantApp.ContainsKey(old_mauid))
                            {
                                UserSessions.diCurrentMerchantApp.Remove(old_mauid);
                            }
                        }

                        UserSessions.diCurrentMerchantApp[this.UID] = agreement;

                        perform = true;

                        wucContact1.ObjectID = Convert.ToInt32(agreement.ID);

                        if (!IsCopy)
                        {
                            wucContact1.FromScreenToVS();
                            wucContact1.FormSave();
                        }
                    }
                    // Save Risk Evaluation (dbo.MerchantFulfillment) details
                    data.SaveUWFulfillment(agreement, UserSessions.CurrentUser.UserName);
                }

                if (perform)
                {
                    if (!string.IsNullOrEmpty(agreement.LeadsID) && agreement.LeadsID != oldLeadID)
                    {
                        //here we will get all the lead services and copy them as merchant services 
                        //for the first tiem when we add a new leadID for the merchant
                        data.UpdateMerchantLeadServices(agreement.MerchantAppUID, agreement.LeadsUID);
                    }

                    if (agreement.SalesForceID > 0 || (this.Adding && CommonUtility.Util.if_i(SalesForceID.Text, 0) > 0))
                    {
                        try
                        {
                            DataSalesForce.InsertMerchantSalesForceID(Convert.ToInt32(agreement.ID), CommonUtility.Util.if_i(SalesForceID.Text, 0), 1, UserSessions.CurrentUser.UserName);
                        }
                        catch
                        {
                            this.Master.AddMessageError("Could not insert SalesForceID.");
                        }
                    }

                    FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);

                    WucSeasonalMonths.UpdateSeasonalMonths(agreement.MerchantAppUID);
                    if (!string.IsNullOrWhiteSpace(Statusuid))
                    {
                        //if new status is cs-cancellation update user portal access to payment xp
                        //and merchant web portal to deny
                        if (Statusuid.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION && m_StatusUID.ToUpper() != Statusuid.ToUpper())
                        {
                            DataUser dUser = DataAccess.DataUserDao;

                            dUser.UpdateMerchantUserPortals(agreement.MerchantAppUID, Constants.PORTAL_PAYMENTXP, false);
                            dUser.UpdateMerchantUserPortals(agreement.MerchantAppUID, Constants.PORTAL_MERCHANT, false);
                        }
                    }

                    this.LeadID = agreement.LeadsUID;

                    FormHandler.CompleteApplication(agreement, achMerchant, Statusuid, user.UserName);

                    if (agreement != null)
                    {
                        Hashtable prms = new Hashtable();
                        prms.Add("@MerchantAppUID", DataLayer.UID2Field(agreement.MerchantAppUID));
                        prms.Add("@UWIssues", this.UWIssues.Text);
                        data.UpdateMerchantUWNotes(prms);
                    }
                    //PXP-6182 start
                    //Code changes for PXP-10225 by koshlendra
                    if ((agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && agreement.Office.Equals(CommonUtility.Util.Offices.Irvine))
                    {
                        if (!string.IsNullOrEmpty(PurchaseOrOrderProduct.SelectedValue.ToString()))
                            facade.UpdateMerchantAppsPurchaseOrOrderProduct(PurchaseOrOrderProduct, agreement.MerchantAppUID);
                    }

                    //PXP-6182 end
                    if (!this.Adding)
                    {
                        // if we're adding, don't do a formshow, it will bomb.
                        try
                        {
                            var _StatusUIDControl = WucBusinessInfo1.FindControl("StatusUID") as DropDownList;
                            if ((_StatusUIDControl != null && _StatusUIDControl.SelectedValue.ToUpper().Equals(Constants.QUEUESTATUS_MS_RECEIVED))
                                && (hidStatus.Value.ToUpper().Equals(Constants.QUEUESTATUS_DP_RECEIVED_HARDWARE)
                                    || hidStatus.Value.ToUpper().Equals(Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE)))
                            {
                                var ExcelTemplatePath = HttpContext.Current.Server.MapPath(ConstantFacade.RDR.ZEUS_EXCEL_TEMPLATE_PATH);
                                RDRHelper.AutoSubcribeFromApplicacionXP(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser, ExcelTemplatePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            ZeusWeb.Logging.ErrorLog.ErrorFormat("Couldn't Auto generate verify Excel and send emai and can't Copy ApplicationXP Product Rules to Merchant Producto Rules", ex);
                        }
                        FormShow(agreement.MerchantAppUID);
                    }
                }
                else
                {
                    this.Master.AddMessageError("Error saving record.");
                }

                //save equipment 
                // start DM-2279 by Jorge
                if (!(UserSessions.CurrentUser.DefaultRoleUID.ToLower() == Constants.ROLE_COLLECTIONS.ToLower() && agreement.IsInCollection != agreement.MerchantAppClone.IsInCollection))
                {
                    perform = WucEquipment.FormSave(agreement.MerchantAppUID);
                }
                // end DM-2279 by Jorge
                //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP
                //merchant  start
                //Ani: DM-5589
                DataSet ds =  FormHandler.UpdateMerchantRiskParameters(agreement);
                //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  END

                if (UserSessions.CurrentMerchantApp != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(CommonUtility.Formatting.GetLogItemFormatted("RiskStatus", agreement.MerchantAppClone.RiskStatus.ToString(), agreement.RiskStatus.ToString()));
                    DataUser datauser = DataAccess.DataUserDao;
                    datauser.InsertChangeLog(UserSessions.CurrentMerchantApp.BusinessDBAName, UserSessions.CurrentUser.UserName, agreement.MerchantAppUID, Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), "Risk Status", sb.ToString(), Constants.PORTAL_ZEUS);
                }
                DropDownList _statusUID = isACHonly ? (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID") : (DropDownList)WucBusinessInfo1.FindControl("StatusUID");

                if (IsInCollection.Checked)
                {

                    //Niranjan : PXP-5307 Collections Ticket should not be created everytime when user edit anything and save profile page
                    //code modified for PXP-5443 by koshlendra start

                    //code modified for PXP-7435
                    //code changes done for Already incollection merchant by Sanidhya Kumar PXP:7226 start
                    if (agreement.IsInCollection && (agreement.MerchantAppClone != null && !agreement.MerchantAppClone.IsInCollection))
                    {
                        if (_statusUID.SelectedValue.ToUpper() != Constants.QUEUESTATUS_MS_CANCELLATION)
                        //code modified for PXP-5443 by koshlendra end
                        {
                            //Niranjan: PXP-4962 Create Ticket when account goes in collections
                            Ticket t1 = new Ticket();
                            t1.StatusID = Ticket.TICKET_OPEN;
                            t1.DepartmentID = "7";   /* Risk */
                            t1.ParentID = "1065";  /* Account Restriction Request */
                            t1.CategoryID = "1365"; /* Divert Account*/
                            string dt = DateTime.Now.ToString("MM/dd");
                            t1.Problem = "Please divert the account today " + dt + " at 2:00pm PST per collections.The merchant has been advised via email.";
                            DateTime dt1 = DateTime.Today;
                            t1.DueDate = new DateTime(dt1.Year, dt1.Month, dt1.Day, 14, 0, 0);
                            t1.MerchantAppUID = agreement.MerchantAppUID;
                            t1.AgentUID = agreement.AgentUID;
                            t1.TicketType = eTicketType.Ticket;
                            t1.OfficeID = agreement.Office.GetHashCode();
                            t1.TimeZone = "6";
                            t1.TicketSource = "i"; // "i" is for internal
                            t1.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                            t1.DateCreated = DateTime.Now;
                            t1.DateModified = DateTime.Now;
                            t1.UserCreated = UserSessions.CurrentUser.UserName;
                            t1.UserModified = UserSessions.CurrentUser.UserName;
                            t1.StatusID = Ticket.TICKET_OPEN;
                            t1.Origin = 4; //Internal

                            DataTicket.GetInstance().InsertTicket(t1);
                            PaymentXP.Facade.TicketNotification.NewTicketCreated(t1.TicketUID, false, false);
                            ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", t1.TicketUID);
                        }

                        string errorMsg = FormHandler.AddMerchantNotes();
                        if (!string.IsNullOrEmpty(errorMsg))
                            this.Master.AddMessageError("Merchant Note exception: " + errorMsg);
                    }
                    //code changes done for Already incollection merchant by Sanidhya Kumar PXP:7226 end
                }
                //code added by abarua PXP-5445
                else
                {
                    //code added by abarua PXP-5445

                    if (!agreement.IsInCollection && (agreement.MerchantAppClone != null && agreement.MerchantAppClone.IsInCollection))
                    {
                        if (_statusUID.SelectedValue.ToUpper() != Constants.QUEUESTATUS_MS_CANCELLATION)
                        {

                            Ticket t1 = new Ticket();
                            t1.StatusID = Ticket.TICKET_OPEN;
                            t1.DepartmentID = "7";   /* Risk */
                            t1.ParentID = "1065";  /* Account Restriction Request */
                            t1.CategoryID = "1369"; /* Un-Divert Account*/
                            string dt = DateTime.Now.ToString("MM/dd");
                            t1.Problem = "Please undivert the account today " + dt + " at 2:00pm PST per collections. Please review to release funds that were captured during the time the account was diverted less $0.00 to cover the balance owed.";    // PXP-8265 RThakur
                            DateTime dt1 = DateTime.Today;
                            t1.DueDate = new DateTime(dt1.Year, dt1.Month, dt1.Day, 14, 0, 0);
                            t1.MerchantAppUID = agreement.MerchantAppUID;
                            t1.AgentUID = agreement.AgentUID;
                            t1.TicketType = eTicketType.Ticket;
                            t1.OfficeID = agreement.Office.GetHashCode();
                            t1.TimeZone = "6";
                            t1.TicketSource = "i"; // "i" is for internal
                            t1.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                            t1.DateCreated = DateTime.Now;
                            t1.DateModified = DateTime.Now;
                            t1.UserCreated = UserSessions.CurrentUser.UserName;
                            t1.UserModified = UserSessions.CurrentUser.UserName;
                            t1.StatusID = Ticket.TICKET_OPEN;
                            t1.Origin = 4; //Internal

                            DataTicket.GetInstance().InsertTicket(t1);
                            PaymentXP.Facade.TicketNotification.NewTicketCreated(t1.TicketUID, false, false);
                            ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", t1.TicketUID);

                        }


                    }

                }

                //code changes for PXP-7803 fixes by koshlendra start
                if (!agreement.AmericanExpress && (agreement.MerchantAppClone != null && agreement.MerchantAppClone.AmericanExpress))
                {

                    //update merchant processing fees for AMEX ESA
                    agreement.MerchantProcessingFee = facade.GetMerchantAppProcessingFee(agreement.ID);
                    agreement.MerchantProcessingFee.AmexCredit.AuthApproved = 0;
                    agreement.MerchantProcessingFee.AmexCredit.FailedRequests = 0;
                    agreement.MerchantProcessingFee.AmexCredit.CreditCompleted = 0;
                    agreement.MerchantProcessingFee.AmexCredit.SettlementFee = 0;
                    agreement.MerchantProcessingFee.AmexDebit.AuthApproved = 0;
                    agreement.MerchantProcessingFee.AmexDebit.FailedRequests = 0;
                    agreement.MerchantProcessingFee.AmexDebit.CreditCompleted = 0;
                    agreement.MerchantProcessingFee.AmexDebit.SettlementFee = 0;

                    facade.UpdateMerchantAppProcessingFee(agreement, agreement.MerchantProcessingFee, user.UserName, Constants.PORTAL_ZEUS);

                }
                //code changes for PXP-7803 fixes by koshlendra end

				//PXP-9750 Rohit Thakur >> Start
				//Create new ticket for ‘Mastercard De-registration process
				if (!isACHonly && _statusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION && agreement.HighRiskRegistered && _statusUID.SelectedValue.ToUpper() != Statusuid.ToUpper())
				{
					FormHandler.AddTicketForMastercard(agreement, "i", "3", "2251", "2252", "3-Low", "6");
				}
                //PXP-9750 Rohit Thakur >> End
                if (!isACHonly && _statusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION && agreement.VIRPHighRiskRegistered && _statusUID.SelectedValue.ToUpper() != Statusuid.ToUpper())
                {
                    FormHandler.AddTicketForVisa(agreement, "i", "3", "2329", "2330", "3-Low", "6");
                }
            }
		}
		catch (Exception exc)
		{
			throw exc;
		}

        if (this.Adding && agreement != null && !string.IsNullOrEmpty(agreement.MerchantAppUID))
        {
            this.Master.AddMessageStatus("New Merchant Added");
            Response.Redirect(WebUtil.GetMyUrl("Adding=false&MerchantAppUID=" + agreement.MerchantAppUID));
        }

        return perform;
    }

    //Start code by anuj for PXP-9311
    private string RemoveExtraSpace(string OpsFieldData)
    {
        OpsFieldData = OpsFieldData.Replace("\r\n \r\n", "\r\n\r\n");
        while (OpsFieldData.Contains("\r\n\r\n"))
        {
            OpsFieldData = OpsFieldData.Replace("\r\n\r\n", "\r\n");
        }
        return OpsFieldData;
    }//Start code by anuj for PXP-9311sss

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnCopy.Enabled = !this.EditMode; //disabled if record is in edit mode.  system throws an error for new records.
        btnAch.Enabled = !this.EditMode;

        btnQA.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);

        wucContact1.EditMode = this.EditMode;
        wucContact1.ToggleButtons();

    }

    public bool FormDataCheck(MerchantApp agreement)
    {
        //code added for PXP-18392 by koshlendra
        if (!(UserSessions.CurrentUser.DefaultRoleUID.ToLower() == Constants.ROLE_COLLECTIONS.ToLower() && agreement.IsInCollection != agreement.MerchantAppClone.IsInCollection))
        {
            if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.AchID > 0)
            {
                if ((String.IsNullOrWhiteSpace(this.BusinessWebsite.Text)) && (UserSessions.ActiveAchMerchant.Secc == "WEB"))
                {
                    this.Master.AddMessageError("Need to populate Website");
                }
            }
            //Fmassoud 2017.08.28 Sending New Status to Formhandler
            DropDownList _statusUID = isACHonly ? (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID") : (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
            IList<string> message = FormHandler.MerchantDataCheck(agreement, false, this.Adding, _statusUID.SelectedValue.ToUpper(), UserSessions.ActiveAchMerchant);
            //PXP-8409 Sanidhya
            IList<string> _infoMsg = FormHandler.ValidateCRMFlow(agreement, this.Adding, UserSessions.ActiveAchMerchant);

            if (message.Count > 0)
            {
                foreach (string mess in message)
                    this.Master.AddMessageError(mess);
            }
            //PXP-8409 Sanidhya
            if (_infoMsg.Count > 0)
            {
                foreach (string msg in _infoMsg)
                {
                    this.Master.AddMessageStatus(msg);
                }
            }

            //if account has bank as ACH_Only and achid > 0 then accoutn is considered as ach only account 
            //in that case we consider the ach status
            if (!isACHonly)
            {
                DropDownList StatusUID = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
                if (StatusUID != null && StatusUID.SelectedIndex == 0)
                    this.Master.AddMessageError("Please select a Status.");
            }
            else
            {
                DropDownList achStatusUID = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
                if (achStatusUID != null && achStatusUID.SelectedIndex == 0)
                    this.Master.AddMessageError("Please select a Status.");
            }

            DropDownList OfficeID = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");
            if (OfficeID != null && OfficeID.SelectedIndex == 0)
                this.Master.AddMessageError("Please select an Office Location.");

            TextBox BusinessDBAName = (TextBox)WucBusinessInfo1.FindControl("BusinessDBAName");
            if (BusinessDBAName != null && string.IsNullOrWhiteSpace(BusinessDBAName.Text))
                this.Master.AddMessageError("Please enter DBA Name.");

            // DM-2279 by Jorge             
            if (WucEquipment.grdCount == 0 && !WucEquipment.FormDataCheck())
                this.Master.AddMessageError("Please select an equipment and review the Equipment Information.");

            //eluxa: disable routing# validation for now because we need to be able to enter Direct Debit and EFT
            //routing numbers
            //if (RoutingNumber.Text != string.Empty && !AchTransaction.ValidateCheckDigit(RoutingNumber.Text))
            //    this.Master.AddMessageError("Invalid Routing Number (Check Digit Error).");

            if (RoutingNumber.Text.Trim().Length != 0 && RoutingNumber.Text == AccountNumber.Text)
                this.Master.AddMessageError("Routing Number and Account Number cannot be the same.");
            
            if (!string.IsNullOrWhiteSpace(ActivationCode.Text))
            {
                string errorString = DataAccess.DataMerchantAppDao.CheckActivationCode(ActivationCode.Text.Trim(), agreement.ID);

                if (!string.IsNullOrEmpty(errorString))
                    this.Master.AddMessageError(errorString);
            }

            if (!string.IsNullOrWhiteSpace(CustomerServiceEmail.Text))
            {
                if (!CommonUtility.Util.IsValidEmail(CustomerServiceEmail.Text.Trim()))
                {
                    this.Master.AddMessageError("Customer Service Email is invalid.");
                }
            }

            //Validation for Business DBA and customer service phone country code and extentions
            WebMaskEditor DBAPhoneExttxt = (WebMaskEditor)WucBusinessInfo1.FindControl("DBAPhoneExt");
            string strDBAPhoneExttxt = CommonUtility.Util.GetNumbersFromString(DBAPhoneExttxt.Text);
            if (!strDBAPhoneExttxt.All(char.IsDigit))
            {
                this.Master.AddMessageError("Please enter numeric values.");
            }

            if (strDBAPhoneExttxt.Length > 6)
            {
                this.Master.AddMessageError("Please enter at the max 6 digit extention.");
            }

            WebMaskEditor BusinessDBAPhonetxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessDBAPhone");
            string strBusinessDBAPhonetxt = CommonUtility.Util.GetNumbersFromString(BusinessDBAPhonetxt.Text);
            if (agreement.Office == CommonUtility.Util.Offices.Irvine)
            {
                //string strBusinessDBAPhonetxt = BusinessDBAPhonetxt.Text.Replace("-", "").ToString();
                if (strBusinessDBAPhonetxt.Trim().Length >= 1 && strBusinessDBAPhonetxt.Trim().Length < 10)
                {
                    this.Master.AddMessageError("Please enter 10 digit Business DBA phone number.");
                }
            }

            if (BusinessDBAPhonetxt.Text.Trim().Length > 12)
            {
                this.Master.AddMessageError("Please enter at the max 12 digit phone number.");
            }

            if (strDBAPhoneExttxt.Length > 0 && string.IsNullOrWhiteSpace(strBusinessDBAPhonetxt))
            {
                this.Master.AddMessageError("Please enter Business DBA phone number.");
            }

            string strCustomerServicePhone = CommonUtility.Util.GetNumbersFromString(CustomerServicePhone.Text);
            string strCustomerServicePhoneExt = CommonUtility.Util.GetNumbersFromString(CustomerServicePhoneExt.Text);

            if (agreement.Office == CommonUtility.Util.Offices.Irvine)
            {
                //string strCustomerServicePhone = CustomerServicePhone.Text.Replace("-", "").ToString();
                if (strCustomerServicePhone.Trim().Length >= 1 && strCustomerServicePhone.Trim().Length < 10)
                {
                    this.Master.AddMessageError("Please enter 10 digit Customer Service phone number.");
                }
            }

            if (CustomerServicePhone.Text.Trim().Length > 12)
            {
                this.Master.AddMessageError("Please enter at the max 12 digit phone number.");
            }

            if (strCustomerServicePhoneExt.Length > 0 && string.IsNullOrWhiteSpace(strCustomerServicePhone))
            {
                this.Master.AddMessageError("Please enter Customer Service phone number.");
            }

            if (!strCustomerServicePhoneExt.All(char.IsDigit))
            {
                this.Master.AddMessageError("Please enter numeric values.");
            }

            if (strCustomerServicePhoneExt.Length > 6)
            {
                this.Master.AddMessageError("Please enter at the max 6 digit extention.");
            }

            WebMaskEditor BusinessFaxExttxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessFaxExt");
            string strBusinessFaxExttxt = CommonUtility.Util.GetNumbersFromString(BusinessFaxExttxt.Text);
            if (!strBusinessFaxExttxt.All(char.IsDigit))
            {
                this.Master.AddMessageError("Please enter numeric values.");
            }

            if (strBusinessFaxExttxt.Length > 6)
            {
                this.Master.AddMessageError("Please enter at the max 6 digit extention.");
            }

            WebMaskEditor BusinessFaxtxt = (WebMaskEditor)WucBusinessInfo1.FindControl("BusinessFax");
            string strBusinessFaxtxt = CommonUtility.Util.GetNumbersFromString(BusinessFaxtxt.Text);
            if (agreement.Office == CommonUtility.Util.Offices.Irvine)
            {
                //string strBusinessFaxtxt = BusinessFaxtxt.Text.Replace("-", "").ToString();
                if (strBusinessFaxtxt.Trim().Length >= 1 && strBusinessFaxtxt.Trim().Length < 10)
                {
                    this.Master.AddMessageError("Please enter 10 digit Business Fax phone number.");
                }
            }

            if (BusinessFaxtxt.Text.Trim().Length > 12)
            {
                this.Master.AddMessageError("Please enter at the max 12 digit Business Fax phone number.");
            }

            if (strBusinessFaxExttxt.Length > 0 && string.IsNullOrWhiteSpace(strBusinessFaxtxt))
            {
                this.Master.AddMessageError("Please enter Business Fax phone number.");
            }
            //Niranjan PXP-8239
            TextBox MerchantFirstName = (TextBox)wucContact1.FindControl("tbFirstname");
            TextBox MerchantLastName = (TextBox)wucContact1.FindControl("tbLastname");

            if (string.IsNullOrWhiteSpace(MerchantFirstName.Text))
                this.Master.AddMessageError("Please enter Merchant contact First Name.");

            if (string.IsNullOrWhiteSpace(MerchantLastName.Text))
                this.Master.AddMessageError("Please enter Merchant contact Last Name.");

            GridView grdPhone = (GridView)wucContact1.FindControl("gvPhone");
            if (grdPhone != null)
            {
                bool isPhoneExist = false;
                for (int i = 0; i < grdPhone.Rows.Count; i++)
                {
                    WebMaskEditor tbPhoneNumber = (WebMaskEditor)grdPhone.Rows[i].FindControl("tbPhoneNumber");
                    if (tbPhoneNumber != null && !(string.IsNullOrEmpty(tbPhoneNumber.Value.ToString().Trim())))
                    {
                        isPhoneExist = true;
                        break;
                    }
                }
                if (!isPhoneExist)
                {
                    this.Master.AddMessageError("Please enter Merchant contact phone number.");
                }

            }

            TextBox FaxCountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("FaxCountryCodeDisplay");
            TextBox DBACountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("DBACountryCodeDisplay");
            DropDownList BusinessFaxCountryCodeddl = (DropDownList)WucBusinessInfo1.FindControl("BusinessFaxCountryCode");
            DropDownList BusinessDBAPhoneCountryCodeddl = (DropDownList)WucBusinessInfo1.FindControl("BusinessDBAPhoneCountryCode");

            FaxCountryCodetxt.Text = BusinessFaxCountryCodeddl.SelectedValue.ToString();
            DBACountryCodetxt.Text = BusinessDBAPhoneCountryCodeddl.SelectedValue.ToString();
            CallingCodeDisplay.Text = CustomerServicePhoneCallingCode.SelectedValue.ToString();

            DropDownList Brand = (DropDownList)WucBusinessInfo1.FindControl("Brand");

            //Validate FMA
            TextBox fma = (TextBox)WucBusinessInfo1.FindControl("FMAID");
            if (fma != null)
            {
                long fmaId;
                int zid;
                int.TryParse(agreement.ID, out zid);

                // Removed IF condition for PXP-4398 Display FMAID for gateway=3rd Party and office=IRV 
                if (!string.IsNullOrWhiteSpace(fma.Text) && !long.TryParse(fma.Text, out fmaId))
                {
                    this.Master.AddMessageError("Please enter a valid FMA number.");
                }
                else if (DataAccess.DataMerchantAppDao.FMAIDExists(zid, agreement.FMAID))
                {
                    //check if fma id already exists for a different ZID
                    this.Master.AddMessageError("FMA number already exists.");
                }
            }
            //abarua PXP-6511
            decimal TotalDeposit;
            //Code changes for PXP-10225 by koshlendra
            if ((agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                ||agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && agreement.Office.Equals(CommonUtility.Util.Offices.Irvine))
            {
                TotalDeposit = agreement.Deposit_FutureServices + agreement.Cash_Carry;
                if (TotalDeposit > 100)
                {
                    this.Master.AddMessageError("Sum of percentage of your business Deposits / Future Services and  Cash & Carry cannot exceed 100%.");
                }
            }
            //DM-7217 Ahmer
            if (chkShowDeposits.Checked && (String.IsNullOrEmpty(agreement.DepositAccountName) || String.IsNullOrEmpty(agreement.DepositBankName) ||
                String.IsNullOrEmpty(agreement.DepositRoutingNumber) || String.IsNullOrEmpty(agreement.DepositAccountNumber)))
            {
                this.Master.AddMessageError("Deposits DDA is missing.");
            }
            if (chkShowDeposits.Checked && (RoutingNumber.Text == agreement.DepositRoutingNumber && AccountNumber.Text == agreement.DepositAccountNumber))
                this.Master.AddMessageError("Fees and Debits DDA and Deposits DDA cannot be the same.");

        }
        else
        {
            if (string.IsNullOrWhiteSpace(agreement.AgentUID))
                this.Master.AddMessageError("Please enter AgentID.");
        }
        //Code update for PXP-18392 by koshlendra end
        string m_StatusUID = string.Empty;

        if (this.Master.ErrorCount() == 0)
        {
            if (!this.Adding && agreement.MerchantAppClone.AgentUID.ToUpper() != agreement.AgentUID.ToUpper())
                DataAccess.DataMerchantAppDao.DeleteMerchantAgentContract(agreement.MerchantAppUID);

            return true;
        }
        else
        {
            if (!isACHonly)
            {
                if (!string.IsNullOrEmpty(agreement.MerchantAppClone.StatusUID))
                {
                    ListHandler.ListFindItem(((DropDownList)WucBusinessInfo1.FindControl("StatusUID")), agreement.MerchantAppClone.StatusUID);
                    agreement.StatusUID = agreement.MerchantAppClone.StatusUID;
                    m_StatusUID = agreement.StatusUID.ToUpper();

                }
            }
            else
            {
                AchMerchant achMerchant = UserSessions.ActiveAchMerchant;

                if (achMerchant != null && achMerchant.AchMerchantClone != null && !string.IsNullOrEmpty(achMerchant.AchMerchantClone.MerchantStatusUID))
                {
                    ListHandler.ListFindItem(((DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID")), achMerchant.AchMerchantClone.MerchantStatusUID);
                    achMerchant.MerchantStatusUID = achMerchant.AchMerchantClone.MerchantStatusUID;
                    m_StatusUID = achMerchant.MerchantStatusUID.ToUpper();

                }
            }


            if (m_StatusUID != Constants.QUEUESTATUS_MS_CANCELLATION && m_StatusUID != Constants.QUEUESTATUS_MS_PENDING_CANCELLATION) //Cancelled or Pending Cancellation
            {
                ((Label)WucBusinessInfo1.FindControl("lblMerchantClosureCodeUID")).Style["display"] = "none";
                ((DropDownList)WucBusinessInfo1.FindControl("MerchantClosureCodeUID")).Style["display"] = "none";
                ((HtmlGenericControl)WucBusinessInfo1.FindControl("ETF")).Style["display"] = "none";
                ((DropDownList)WucBusinessInfo1.FindControl("ETFAssessed")).Style["display"] = "none";
                ((Label)WucBusinessInfo1.FindControl("lblETF")).Style["display"] = "none";
                ((HtmlGenericControl)WucBusinessInfo1.FindControl("CancelDate")).Style["display"] = "none";
                ((Label)WucBusinessInfo1.FindControl("lblCancel")).Style["display"] = "none";
                ((WebDatePicker)WucBusinessInfo1.FindControl("CancellationDate")).Style["display"] = "none";
                ((Label)WucBusinessInfo1.FindControl("lblRiskStatus")).Style["display"] = "none";
                ((DropDownList)WucBusinessInfo1.FindControl("RiskStatus")).Style["display"] = "none";
            }


            return false;
        }


    }

    public override void FormNew()
    {
        this.FormClear();

        this.Adding = true;
        this.EditMode = true;

        WucBusinessInfo1.Adding = true;
        wucContact1.Adding = true;

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        WucBusinessInfo1.SelectButton.Enabled = this.EditMode;
        AgentID.Text = "";

        btnPDF.Visible = false;
        btnOpsPDF.Visible = false;

        this.ToggleButtons();

        DropDownList status = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
        LookupTableHandler.MerchantAppStatus(status, false, "Merchant Management", "SS");

        status.Enabled = false;
        if (status != null)
            ListHandler.ListFindItem(status, Constants.QUEUESTATUS_SS_RECEIVED); //RM Received Status


        DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
        LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", "SS");

        if (achstatus != null)
            ListHandler.ListFindItem(achstatus, Constants.QUEUESTATUS_SS_RECEIVED); //RM Received Status

        achstatus.Enabled = false;

        //to do
        ((Panel)this.Master.FindControl("pnlMerchantMemo")).Visible = false;
        ((Panel)this.Master.FindControl("pnlAgentMemo")).Visible = false;
        ((Panel)this.Master.FindControl("pnlFirstTeam")).Visible = false;

        WucSeasonalMonths.LoadSeasonalMonths(string.Empty, 2);

        DropDownList ReleaseMethodUID = (DropDownList)WucBusinessInfo1.FindControl("ReleaseMethodUID");
        ReleaseMethodUID.Enabled = false;
        DropDownList discount = (DropDownList)WucBusinessInfo1.FindControl("DiscountMethod");
        discount.Enabled = false;

        //Added for Validation for phone number, Country Code and Extention 
        TextBox FaxCountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("FaxCountryCodeDisplay");
        FaxCountryCodetxt.Text = "+1";
        TextBox DBACountryCodetxt = (TextBox)WucBusinessInfo1.FindControl("DBACountryCodeDisplay");
        DBACountryCodetxt.Text = "+1";

        this.CallingCodeDisplay.Text = "+1";
        WucBusinessInfo1.SetAgentSelectorEditMode();

        this.MCClookup.m_SicCode = "";
        this.MCClookup.m_SicCodeDesc = "";
        //code added by Koshlendra for PXp-12935 start
        this.MCClookup.m_VisaSicCode = "";
        this.MCClookup.m_VisaSicCodeDesc = "";
        //code added by Koshlendra for PXp-12935 end

        NAICS.Text = "";

        wucMerchantCategories1.Visible = false;

        DropDownList office = (DropDownList)WucBusinessInfo1.FindControl("OfficeID");

        wucContact1.Adding = true;
        wucContact1.EditMode = true;
        wucContact1.officeID = int.Parse(office.SelectedValue);
        wucContact1.FormNew();

        SalesForceID.ReadOnly = false;
        //Code added for PXP-13345
        AccountClosureRisk.Enabled = false;
        //End code added for PXP-13345 

        //start :code added for IsInCollection flag****
        if (UserSessions.CurrentUser != null)
        {
            pnlRedBanner.Visible = false;
            pnlYellowBanner.Visible = false;
            //Ani: DM-5746
            pnlOrangeBanner.Visible = false;
            pnlRiskDivertStatusBanner.Visible = false;

            UserFacade userFacade = new UserFacade();
            var userRoles = userFacade.GetUser(UserSessions.CurrentUser.UID).UserRoles.Where(u => u.Value.Enabled == true);  // Dynamic list of enabled user roles;

            rowInCollection.Visible = userRoles.Any(s => s.Value.RoleID == Constants.ROLE_RISK) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_SPECIALACCESS)
                || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_DEPLOYMENT) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_CLIENT_SERVICES)
                || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_ADMIN);


            rowWithAgency.Visible = userRoles.Any(s => s.Value.RoleID == Constants.ROLE_RISK) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_SPECIALACCESS)
              || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_DEPLOYMENT) || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_CLIENT_SERVICES)
              || userRoles.Any(s => s.Value.RoleID == Constants.ROLE_ADMIN);

            //Code updated for PXP-18392 by koshlendra
            if (UserSessions.CurrentUser.DefaultRoleUID.ToLower() == Constants.ROLE_COLLECTIONS.ToLower())
            {
                IsInCollection.Enabled = true;
                IsWithAgency.Enabled = true;
            }
            else
            {
                IsInCollection.Enabled = false;
                IsWithAgency.Enabled = false;
            }
        }

        //end :code added for IsInCollection flag****

    }

    public override bool FormDelete()
    {
        return false;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        WucBusinessInfo1.Adding = this.Adding = false;
        this.ToggleButtons();

        //Clear Equipments
        WucEquipment.FormClear();
        WucEquipment.LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);

        WucBusinessInfo1.SetAgentSelectorEditMode();

        // forces the contact control to read from the DB, not the VS.
        wucContact1.FormShow("", false);
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    private void CloseForm()
    {
        string url = string.Empty;

        if (Request.QueryString["PostBackURL"] != null)
            url = Convert.ToString(Request.QueryString["PostBackURL"]);

        if (!string.IsNullOrWhiteSpace(url))
            Response.Redirect(url);
        else
            Response.Redirect("frmMerchantSearch.aspx");
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        MerchantApp app = new MerchantApp();
        string url = string.Empty;

        if (UserSessions.CurrentMerchantApp != null)
        {
            MerchantFacade facade = new MerchantFacade();
            UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            app = (MerchantApp)UserSessions.CurrentMerchantApp;
        }

        switch (btn.Text)
        {
            case "Add":

                url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=true";
                Response.Redirect(url);

                break;
            case "Save":
                if (this.FormSave())
                {
                    WucBusinessInfo1.Adding = this.Adding = false;
                    this.EditMode = false;
                    url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false";
                    url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                this.IsCopy = false;
                if (string.IsNullOrWhiteSpace(this.UID))
                {
                    this.CloseForm();
                }
                else
                {
                    this.FormCancel();
                    url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false";
                    url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                break;

            case "Close":
                this.CloseForm();
                break;

            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
                break;

            case "Upload Document":
                Response.Redirect("~/DocumentUpload/frmDocumentUpload.aspx?MerchantAppUID=" + this.UID);
                break;

            case "QA Print":
                Response.Redirect("~/SecureMerchantManagementForms/frmCreateMerchantApp.aspx?MerchantAppUID=" + app.MerchantAppUID);
                break;

            case "Edit":
                {
                    this.EditMode = true;
                    this.FormShow(this.UID);
                    //Modified by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message 
                    string firstUserdetail = MerchantFacade.GetFirstUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID);
                    if (!string.IsNullOrWhiteSpace(firstUserdetail))
                    {
                        string notification = firstUserdetail + " is currently editing this ZID. Please ensure you will not overwrite each other's work.";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Notification", "alert(" + '"' + notification + '"' + ");", true);
                        MasterPageMerchant master = (MasterPageMerchant)this.Master;
                        master.UpdateNotification(firstUserdetail + " is editing this ZID.");
                    }
                    MerchantFacade.AddNewUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
                    /******** End of PXP-2206 **************/
                    this.ToggleButtons();

                }
                break;

            case "Copy":
                {
                    //Niranjan :  PXP-3641 Zeus for Montreal: Option to Copy Documents and ACH tab when Copying a Profile
                    if (UserSessions.CurrentMerchantApp.AchID != 0)
                    {
                        trAchProfile.Visible = true;
                    }
                    if (DataDocuments.GetInstance() != null)
                    {
                        Hashtable prms = new Hashtable();
                        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
                        prms.Add("@UserUID", UserSessions.CurrentUser.UID);

                        List<MDoc> li = DataDocuments.GetInstance().GetMDocuments(prms);

                        if (li != null && li.Count > 0)
                        {
                            var results = li.Where(o => o.IsPrivate == false).Where(o => o.DocTypeGroupID == 1);

                            chkDocumentlist.DataSource = results;
                            chkDocumentlist.DataBind();
                            trMerchantDocuments.Visible = true;
                        }
                    }

                    WebDialogWindow2.WindowState = DialogWindowState.Normal;

                    if (trAchProfile.Visible || trMerchantDocuments.Visible)
                    {
                        lblMessage.Visible = true;
                        lblNoACH.Visible = false;
                    }
                    else
                    {
                        lblNoACH.Visible = true;
                        lblMessage.Visible = false;
                    }
                    WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621
                }



                break;

            case "ACH":

                if (app.AchID == 0)
                    Response.Redirect("~/SecureMerchantManagementForms/frmMerchantACH.aspx?Adding=true&MerchantID=" + app.ID + "&MerchantAppUID=" + app.MerchantAppUID);
                else
                    Response.Redirect("~/SecureMerchantManagementForms/frmMerchantACH.aspx?AchID=" + app.AchID.ToString() + "&Adding=false&MerchantID=" + app.ID + "&MerchantAppUID=" + app.MerchantAppUID);
                break;
        }
    }

    private MerchantApp CopyMerchantAppV2(MerchantApp ma)
    {
        string uid = DataAccess.DataMerchantAppDao.InsertCopyMerchantApp(ma, UserSessions.CurrentUser.UserName);

        MerchantApp objMA = DataMerchantApp.GetInstance().GetMerchantApp(uid);

        return objMA;
    }
    //Niranjan :  PXP-3641 Zeus for Montreal: Option to Copy Documents and ACH tab when Copying a Profile.
    private MerchantApp CopyACHMerchant(string oldZid, string newZid)
    {
        DataAchMerchant data = DataAccess.DataAchMerchantDao;
        int rw = DataAccess.DataAchMerchantDao.InsertCopyAchMerchant(oldZid, newZid);
        MerchantApp achobj = new MerchantApp();
        return achobj;
    }

    private MerchantApp CopyMerchantApp()
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        DataSet dsmultiLink = new DataSet();
        MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
        agreement.UserCreated = UserSessions.CurrentUser.UserName;
        agreement.MultiAccLink = true;
        agreement.NoEquipment = true;
        agreement.Owners = UserSessions.CurrentMerchantApp.Owners;
        agreement.AgentUID = "";
        agreement.SettlePlatformMid = "";
        agreement.SettlePlatformUID = "";
        agreement.AuthPlatformMid = "";
        agreement.AuthPlatformUID = "";
        agreement.AccountName = "";
        agreement.AccountNumber = "";
        agreement.AccountNumberMask = "";
        agreement.LeadsID = "";
        agreement.LeadsUID = "";
        agreement.AchID = 0;
        agreement.BankName = "";
        agreement.AgentDBA = "";
        agreement.AgentID = 0;
        agreement.MerchantAppTypeUID = "";
        agreement.Descriptor = "";
        agreement.Bank = "";
        agreement.RoutingNumber = "";
        agreement.ReturnPoliciesUID = "";
        agreement.PrimaryContactUID = UserSessions.CurrentMerchantApp.PrimaryContactUID; //changes done for PXP-17195
        agreement.StatusUID = Constants.QUEUESTATUS_SS_RECEIVED;

        // request from plaidinh: says these values should ALWAYS be unique when copying over.
        agreement.DiscoverMid = "";
        agreement.TID = "";
        agreement.FMAID = 0;

        return agreement;

    }

    protected void btnAction_Click(object sender, EventArgs e)
    {
        FormHandler.UploadPDF(false, false, null);
        WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added for PXP-7621
    }

    protected void btnOpsAction_Click(object sender, EventArgs e)
    {
        FormHandler.UploadPDF(true, false, null);
    }
    public override bool FormDataCheck()
    {
        throw new Exception("don't use this. use the one that takes in a MerchantApp instead");
    }

    protected void AgentID_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/SecureAgentManagementForms/frmAgent.aspx?Adding=false&AgentUID=" + UserSessions.CurrentMerchantApp.AgentUID);
    }

    protected void Gatewayonly_OnCheckedChanged(object sender, EventArgs e)
    {
        if (!this.Adding)
        {
            MerchantApp agreement = UserSessions.CurrentMerchantApp;
            agreement.Gatewayonly = this.Gatewayonly.Checked;

            if (!isACHonly)
            {
                DropDownList status = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
                LookupTableHandler.MerchantAppStatus(status, false, "Merchant Management", "SS", agreement);
                status.SelectedValue = agreement.StatusUID;
            }
            else
            {
                AchMerchant achmerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));
                DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
                LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", agreement, achmerchant);
                achstatus.SelectedValue = achmerchant.MerchantStatusUID;
            }

        }

    }

    private void PopulateProductSummary(int zId)
    {
        //pass in zid and get list of products merchant is subscribed to
        List<PaymentXP.BusinessObjects.Subscription> subscriptions = DataProduct.GetMerchantCurrentProductSubscriptionList(zId);
        //PXP-3014
        List<PaymentXP.BusinessObjects.Subscription> deployment = null;
        DataTable dtDeployment = null;
        dtDeployment = new DataTable();

        DataRow row = null;

        if (CommonUtility.Util.IsValidGuid(UserSessions.CurrentMerchantApp.MerchantAppUID))
        {
            EquipmentFacade facade = new EquipmentFacade();
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
            prms.Add("@IsEnabled", true);
            DataSet ds = facade.GetMerchantAppItem(prms);
            DataView dv = ds.Tables[0].DefaultView;
            if (dv.Table.Rows.Count > 0)
            {
                // Niranjan : PXP-4512 Zeus: Merchant Summary should not display Non-EMV Compliant when no value selected       

                string strEMVComplianceMerchant = UserSessions.CurrentMerchantApp.EMVComplianceMerchant;

                bool perform1 = false;
                if (strEMVComplianceMerchant.Equals("0"))
                {
                    strEMVComplianceMerchant = "Merchant Non-EMV Compliant";
                    perform1 = true;
                }
                else if (strEMVComplianceMerchant.Equals("1"))
                {
                    strEMVComplianceMerchant = "Merchant EMV Compliant";
                    perform1 = true;
                }
                else
                {
                    strEMVComplianceMerchant = string.Empty;
                }

                if (perform1 == true)
                {
                    this.divProductSummary.Visible = true;
                    this.divDeployment.Visible = true;
                    this.divRDR.Visible = true;
                    dtDeployment.Columns.Add(new DataColumn("ProductName", typeof(System.String)));
                    row = dtDeployment.NewRow();
                    row["ProductName"] = strEMVComplianceMerchant;
                    dtDeployment.Rows.Add(row);
                }
            }
        }

        if (subscriptions == null)
        {
            this.dListDeployment.DataSource = dtDeployment;
            this.dListDeployment.DataBind();
            this.divAlternativePayments.Visible = false;
            this.divPaymentAcceptance.Visible = false;
            this.divRiskManagement.Visible = false;
        }

        if (subscriptions != null)
        {
            List<PaymentXP.BusinessObjects.Subscription> altPayments = subscriptions.Where<PaymentXP.BusinessObjects.Subscription>(p => p.Product.Category == ProductCategory.AlternativePayments && p.Product.IsActive == true).ToList<PaymentXP.BusinessObjects.Subscription>();
            deployment = subscriptions.Where<PaymentXP.BusinessObjects.Subscription>(p => p.Product.Category == ProductCategory.Deployment && p.Product.IsActive == true).ToList<PaymentXP.BusinessObjects.Subscription>();
            List<PaymentXP.BusinessObjects.Subscription> payAcceptance = subscriptions.Where<PaymentXP.BusinessObjects.Subscription>(p => p.Product.Category == ProductCategory.PaymentAcceptance && p.Product.IsActive == true).ToList<PaymentXP.BusinessObjects.Subscription>();
            List<PaymentXP.BusinessObjects.Subscription> risk = subscriptions.Where<PaymentXP.BusinessObjects.Subscription>(p => p.Product.Category == ProductCategory.RiskManagement && p.Product.IsActive == true).ToList<PaymentXP.BusinessObjects.Subscription>();
            List<PaymentXP.BusinessObjects.Subscription> rdr = subscriptions.Where<PaymentXP.BusinessObjects.Subscription>(p => p.Product.Category == ProductCategory.CBMS && p.Product.IsActive == true).ToList<PaymentXP.BusinessObjects.Subscription>();


            //if merchant is not subscribed to any products or does not have
            //any services enabled then hide the summary section
            if (altPayments.Count == 0 && dtDeployment.Rows.Count == 0 && deployment.Count == 0
                && payAcceptance.Count == 0 && risk.Count == 0 && rdr.Count == 0)
            {
                this.divProductSummary.Visible = false;
            }
            else
            {
                //hide product category if merchant is not subscribed to any products in the product category
                if (altPayments.Count == 0)
                {
                    this.divAlternativePayments.Visible = false;
                }
                else
                {
                    this.dListAlternativePayments.DataSource = this.GetProductDataSet(altPayments);
                    this.dListAlternativePayments.DataBind();
                }

                if (dtDeployment.Rows.Count == 0 && deployment.Count == 0)
                {
                    this.divDeployment.Visible = false;
                }
                else
                {
                    DataTable dtProductDataSet = this.GetProductDataSet(deployment);
                    if (dtDeployment.Rows.Count > 0)
                    {
                        dtProductDataSet.ImportRow(row);
                    }
                    this.dListDeployment.DataSource = dtProductDataSet;
                    this.dListDeployment.DataBind();
                }

                if (payAcceptance.Count == 0)
                {
                    this.divPaymentAcceptance.Visible = false;
                }
                else
                {
                    this.dListPaymentAcceptance.DataSource = this.GetProductDataSet(payAcceptance);
                    this.dListPaymentAcceptance.DataBind();
                }

                if (risk.Count == 0)
                {
                    this.divRiskManagement.Visible = false;
                }
                else
                {
                    this.dListRiskmanagement.DataSource = this.GetProductDataSet(risk);
                    this.dListRiskmanagement.DataBind();
                }
                //DM-3677
                if (rdr.Count == 0)
                {
                    this.divRDR.Visible = false;
                }
                else
                {
                    this.rdrList.DataSource = this.GetProductDataSet(rdr);
                    this.rdrList.DataBind();
                }
                //DM-3677 end

                this.divProductSummary.Visible = true;
            }
        }

    }

    private DataTable GetProductDataSet(List<PaymentXP.BusinessObjects.Subscription> subscriptions)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ProductName", typeof(System.String)));

        DataRow row = null;

        foreach (PaymentXP.BusinessObjects.Subscription subscription in subscriptions)
        {
            if (subscription.Product.IsActive)
            {
                row = dt.NewRow();
                row["ProductName"] = subscription.Product.Name;
                dt.Rows.Add(row);
            }
        }

        return dt;
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        grdLeads.FormClear();
        grdLeads.SetDataSource(prms, 10);
        dlgcontrol.Modal = false;
        dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void lbRemoveMerchant_Click(object sender, EventArgs e)
    {
        LeadsID.Text = string.Empty;
        LeadsUID.Value = string.Empty;

        lbRemoveMerchant.Visible = false;
    }

    //Niranjan :  PXP-3641 Zeus for Montreal: Option to Copy Documents and ACH tab when Copying a Profile.
    protected void btnOk_Click(object sender, EventArgs e)
    {
        lblErr.Text = "";

        if (((Button)sender).CommandArgument.ToUpper() == "YES")
        {
            if (chkCopyACHprofile.Checked || chkDocumentlist.Items.Cast<ListItem>().Count(li => li.Selected) > 0)
            {
                MerchantApp newMA = this.CopyMerchantAppV2(UserSessions.CurrentMerchantApp);
                if (chkCopyACHprofile.Checked && UserSessions.CurrentMerchantApp.AchID != 0)
                {
                    MerchantApp newach = this.CopyACHMerchant(UserSessions.CurrentMerchantApp.ID, newMA.ID);

                }
                DataMerchantApp data = DataAccess.DataMerchantAppDao;
                var selectedDoclist = string.Join(", ", chkDocumentlist.Items.Cast<ListItem>()
                         .Where(li => li.Selected).Select(x => x.Value).ToArray());
                if (selectedDoclist != string.Empty)
                    data.CopyDocument(selectedDoclist, newMA.ID, UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentUser.UserName);

                if (newMA != null && CommonUtility.Util.if_i(newMA.ID, 0) > 0)
                {
                    Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID={0}", newMA.MerchantAppUID));
                }
            }
            else
            {
                MerchantApp newMA = this.CopyMerchantAppV2(UserSessions.CurrentMerchantApp);

                if (newMA != null && CommonUtility.Util.if_i(newMA.ID, 0) > 0)
                {
                    Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID={0}", newMA.MerchantAppUID));
                }
            }
        }
        else
        {
            WebDialogWindow2.WindowState = DialogWindowState.Hidden;
        }
    }

    //Added for PXP-6182
    protected void RefundPolicyAwareness_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RefundPolicyAwareness.SelectedValue == "0")
        {
            trRefundPolicyAwarenessReason.Visible = true;
        }
        else
        {
            trRefundPolicyAwarenessReason.Visible = false;
        }
    }
    //Start code added for PXP-8254
    protected void enableMCCByRoles(bool isAppInEditMode, string appStatusName)
    {
        if (isAppInEditMode)
        {
            Button btnLookUp = (Button)MCClookup.FindControl("btnLookup");
            if (appStatusName.Substring(0, 2).ToUpper() == "SS" || (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_CREDIT_UNDERWRITING))
            {
                //enable MCC
                if (btnLookUp != null)
                    btnLookUp.Enabled = true;
            }
            else
            {
                //disable MCC
                if (btnLookUp != null)
                    btnLookUp.Enabled = false;
            }

        }
    }
    //End code added for PXP-8254
    //Start Code added for PXP-13345
    protected void CreateAccountCloseRiskTicket(DataMerchantApp dataApp, MerchantApp mapp)
    {

        try
        {
            Hashtable param = new Hashtable();
            param.Add("@MerchantAppUID", mapp.MerchantAppUID);
            param.Add("@CategoryID", 2061);
            param.Add("@ParentID", 2058);
            param.Add("@DepartmentID", 5);

            if (dataApp.CheckDuplicateTicketByTicketCategory(param) == false)
            {

                Ticket ticket = new Ticket();
                ticket.StatusID = Ticket.TICKET_OPEN;
                ticket.DepartmentID = "5";   /* Deployment */
                ticket.ParentID = "2058";  /* Account Closure */
                ticket.CategoryID = "2061"; /* Risk Closure*/
                string dt = DateTime.Now.ToString("MM/dd/yyyy");
                ticket.Problem = "Account Closure - " + dt + " - Risk reason";
                DateTime dt1 = DateTime.Now.AddDays(2);
                ticket.DueDate = new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, 0, 0);
                ticket.MerchantAppUID = mapp.MerchantAppUID;
                ticket.AgentUID = mapp.AgentUID;
                ticket.TicketType = eTicketType.Ticket;
                ticket.OfficeID = mapp.Office.GetHashCode();
                ticket.TimeZone = "6";
                ticket.TicketSource = "i"; // "i" is for internal
                ticket.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                ticket.DateCreated = DateTime.Now;
                ticket.DateModified = DateTime.Now;
                ticket.UserCreated = UserSessions.CurrentUser.UserName;
                ticket.UserModified = UserSessions.CurrentUser.UserName;
                ticket.StatusID = Ticket.TICKET_OPEN;
                ticket.Origin = 4; //Internal
                ticket.Priority = "Low";

                DataTicket.GetInstance().InsertTicket(ticket);
                if (ticket.TicketUID != "-1")
                {
                    PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false, false);
                    ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", ticket.TicketUID);
                    //Adding merchant notes
                    FormHandler.InsertMerchantNotesForTicket(ticket.MerchantAppUID, ticket.UserCreated, ticket);
                }
            }
        }
        catch (Exception)
        {


        }

    }
    //End code added for PXP-13345
    private void UpdateOktaUser(PaymentXP.BusinessObjects.User user, bool EmailChanged)
    {
        try
        {
            UpdateUserRequest updateUserRequest = new UpdateUserRequest()
            {
                Profile = new Okta.Sdk.Model.UserProfile(),
                Credentials = new Okta.Sdk.Model.UserCredentials()
            };

            updateUserRequest.Profile.FirstName = user.FirstName;
            updateUserRequest.Profile.LastName = user.LastName;
            updateUserRequest.Profile.Login = user.UserName;
            updateUserRequest.Profile.Email = user.Email;

            var oktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.UpdateUser(user.OktaUserID, updateUserRequest, UserSessions.PortalUID);

            if (EmailChanged)
            {
                bool isResponse = Paysafe.TwoFactorAuth.Client.Factor.Instance.EnrollActivateEmailFactor(user.OktaUserID, user.Email, UserSessions.PortalUID);
                if (isResponse)
                {
                    user.OktaUserID = user.OktaUserID;
                }
            }
        }
        catch (Exception ex)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Unable to update okta user" + user.UserName + ex.Message);
        }
    }

    //DM-7217 - Ahmer
    protected void chkShowDeposits_CheckedChanged(object sender, EventArgs e)
    {
        DepositSectionVisibilityChanged();
    }

    private void DepositSectionVisibilityChanged()
    {
        if (chkShowDeposits.Checked)
        {
            lblFeesAndDebits.Visible = true;
            tblFeesAndDebits.Visible = true;
            lblDeposits.Visible = true;
            if (ddlDepositBankCurrency.SelectedItem.Text == "--Select--")
            {
                ddlDepositBankCurrency.SelectedValue = "USD";
            }
        }
        else
        {
            lblFeesAndDebits.Visible = false;
            tblFeesAndDebits.Visible = false;
            lblDeposits.Visible = false;
            txtDepositAccountName.Text = string.Empty;
            txtDepositBankName.Text = string.Empty;
            txtDepositRoutingNumber.Text = string.Empty;
            txtDepositAccountNumber.Text = string.Empty;
            ddlDepositBankCurrency.SelectedValue = "";
        }
    }

    private void FillDepositsSection(MerchantApp agreement)
    {
        txtDepositAccountName.Text = agreement.DepositAccountName;
        txtDepositBankName.Text = agreement.DepositBankName;
        txtDepositRoutingNumber.Text = agreement.DepositRoutingNumber;
        txtDepositAccountNumber.Text = agreement.DepositAccountNumber;
        txtDepositAccountNumberMask.Text = agreement.DepositAccountNumberMask;
        ddlDepositBankCurrency.SelectedValue = agreement.DepositBankCurrency;

        chkShowDeposits.Checked = (!String.IsNullOrEmpty(agreement.DepositAccountName) && !String.IsNullOrEmpty(agreement.DepositBankName) &&
            !String.IsNullOrEmpty(agreement.DepositRoutingNumber) && !String.IsNullOrEmpty(agreement.DepositAccountNumber)) ?
            true : false;

        if (chkShowDeposits.Checked)
        {
            DepositSectionVisibilityChanged();
        }
    }
}



