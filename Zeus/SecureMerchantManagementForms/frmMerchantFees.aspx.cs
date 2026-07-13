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
using ZeusWeb.Class;
using PaymentXP.BusinessObjects.Zeus;
using System.Text;


public partial class frmMerchantFees : frmBaseDataEntry
{
    bool isACHonly
    {
        set { WucBusinessInfo1.isACHonly = value; }
        get { return WucBusinessInfo1.isACHonly; }

    }

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    override protected void OnInit(EventArgs e)
    {
        confirm.ButtonClick += new wuConfirmDialog.ButtonClickHandler(confirm_ButtonClick);
        base.OnInit(e);
        //Niranjan :PXP-4989 Zeus: Add fields in 'Transaction Information' section on Fees Page.
        //Bug fix PXP-6485
        if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
        {

            mailorderpercent.Text = "MOTO (Mail/Telephone):";
            Edatacapture.Text = "Card Present Swiped:";
            ManualentrywithImpr.Text = "Card Present (Keyed):";
            Manualentrynocardpresent.Text = "Card Not Present (Keyed):";
            telephoneorder.Visible = false;
            TinfoTelephoneOrderPercent.Visible = false;
            OffPremise.Visible = true;
            TinfoOffPremisePercent.Visible = true;
            TradeShow.Visible = true;
            TinfoTradeShowPercent.Visible = true;
            lblOther.Visible = true;
            TinfoOtherPercent.Visible = true;
            Otherspecify.Visible = true;
            Specify.Visible = true;

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
            this.Master.AddMessageError("Please enter valid TaxID.");
            ((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text = (app != null) ? app.BusinessTaxID : string.Empty;
        }
        if (((TextBox)confirm.FindControl("txtMID")).Visible && ((TextBox)confirm.FindControl("txtMID")).Text != ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text)
        {
            count = false;
            this.Master.AddMessageError("Please enter valid Front MID.");
            ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text = (app != null) ? app.AuthPlatformMid : string.Empty;
        }
        if (((TextBox)confirm.FindControl("txtBMID")).Visible && ((TextBox)confirm.FindControl("txtBMID")).Text != ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text)
        {
            count = false;
            this.Master.AddMessageError("Please enter valid Back MID");
            ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text = (app != null) ? app.SettlePlatformMid : string.Empty;
        }

        if (!count)
            this.Master.AddMessageError(message);

        confirm.SetValue(false);
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        TotalPeriodVolume.ReadOnly = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        WucBusinessInfo1.pnlInfo.Enabled = false;
        //PricingTypeID.Attributes["onchange"] = "PricingType_OnChange(this);";

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Fees);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Fees");
            }

            //LookupTableHandler.LoadPricingType(PricingTypeID, false);

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

            if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
            {
                MonthlyMinimumTypeID.Visible = false;
            }
        }
        //code changes for PXP-10216 by koshlendra
        if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST 
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS )

        {
            trThirdParty.Visible = true;
            lblGatewayHeader.Text = "Payment XP Gateway Fees";
        }
        else
        {
            trThirdParty.Visible = false;
            lblGatewayHeader.Text = "Gateway";
        }

    }

    private MerchantProcessingFee MapMerchantProcessingFeeToBusiness(WebUtilMerchantProcessingFee WebObject, MerchantApp agreement)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantProcessingFee BusinessObject = facade.GetMerchantAppProcessingFee(agreement.ID);

        Type WebobjType = WebObject.GetType();
        PropertyInfo[] WebobjPropertiesArray = WebobjType.GetProperties();

        foreach (PropertyInfo WebObjProp in WebobjPropertiesArray)
        {
            try
            {
                string[] WebObjPropName = WebObjProp.Name.Split('_');
                var temp = WebObjProp.GetValue(WebObject, null);
                switch (WebObjPropName[0])
                {
                    case "VISACredit":
                        if (agreement.Visa)
                        {
                            BusinessObject.VISACredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.VISACredit, temp, null);
                        }
                        break;
                    case "VISADebit":
                        if (agreement.Visa)
                        {
                            BusinessObject.VISADebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.VISADebit, temp, null);
                        }
                        break;
                    case "VISAElectronDebit":
                        BusinessObject.VISAElectronDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.VISAElectronDebit, temp, null);
                        break;
                    case "MasterCardCredit":
                        if (agreement.MasterCard)
                        {
                            BusinessObject.MasterCardCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.MasterCardCredit, temp, null);
                        }
                        break;
                    case "MasterCardDebit":
                        if (agreement.MasterCard)
                        {
                            BusinessObject.MasterCardDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.MasterCardDebit, temp, null);
                        }
                        break;
                    case "MaestroDebit":
                        if (agreement.Maestro)
                        {
                            BusinessObject.MaestroDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.MaestroDebit, temp, null);
                        }
                        break;
                    case "AmexCredit":
                        if (agreement.AmericanExpress)
                        {
                            BusinessObject.AmexCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.AmexCredit, temp, null);
                        }
                        break;
                    case "AmexDebit":
                        if (agreement.AmericanExpress)
                        {
                            BusinessObject.AmexDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.AmexDebit, temp, null);
                        }
                        break;
                    case "AmexOPCredit":
                        if (agreement.AmexOP)
                        {
                            BusinessObject.AmexOPCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.AmexOPCredit, temp, null);
                        }
                        break;
                    case "AmexOPDebit":
                        if (agreement.AmexOP)
                        {
                            BusinessObject.AmexOPDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.AmexOPDebit, temp, null);
                        }
                        break;
                    case "AmexOBCredit":
                        if (agreement.AmexOB)
                        {
                            BusinessObject.AmexOBCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.AmexOBCredit, temp, null);
                        }
                        break;
                    case "AmexOBDebit":
                        if (agreement.AmexOB)
                        {
                            BusinessObject.AmexOBDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.AmexOBDebit, temp, null);
                        }
                        break;
                    case "DiscoverCredit":
                        if (agreement.DiscoverNovus)
                        {
                            BusinessObject.DiscoverCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.DiscoverCredit, temp, null);
                        }
                        break;
                    case "DiscoverDebit":
                        if (agreement.DiscoverNovus)
                        {
                            BusinessObject.DiscoverDebit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.DiscoverDebit, temp, null);
                        }
                        break;
                    case "DinersClubCredit":
                        if (agreement.Diners)
                        {
                            BusinessObject.DinersClubCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.DinersClubCredit, temp, null);
                        }
                        break;
                    case "JCBCredit":
                        if (agreement.Jcb)
                        {
                            BusinessObject.JCBCredit.GetType().GetProperty(WebObjPropName[1]).SetValue(BusinessObject.JCBCredit, temp, null);
                        }
                        break;

                }
            }

            catch
            {
                continue;
            }

        }

        return BusinessObject;
    }

    private WebUtilMerchantProcessingFee MapMerchantProcessingFeeToWeb(MerchantProcessingFee BusinessObject)
    {
        WebUtilMerchantProcessingFee Webobject = new WebUtilMerchantProcessingFee();

        Type BusinessobjType = BusinessObject.GetType();
        PropertyInfo[] BusinessobjPropertiesArray = BusinessobjType.GetProperties();

        foreach (PropertyInfo BusinessObjProp in BusinessobjPropertiesArray)
        {
            string CardTypeName = BusinessObjProp.Name;
            MerchantCardFee TempCardFee = BusinessObject.FindMerchantCardFeeByName(CardTypeName);
            if (TempCardFee != null)
            {
                Type objType = TempCardFee.GetType();
                PropertyInfo[] objPropertiesArray = objType.GetProperties();
                foreach (PropertyInfo prop in objPropertiesArray)
                {

                    var temp = prop.GetValue(TempCardFee, null);
                    if (Webobject.GetType().GetProperty(CardTypeName + "_" + prop.Name) != null)
                    {
                        Webobject.GetType().GetProperty(CardTypeName + "_" + prop.Name).SetValue(Webobject, temp, null);
                    }
                }
            }
        }
        return Webobject;
    }

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);
        //PXP-4983 Abarua
        HdnOffice.Value = agreement.Office.ToString();
        HdnBank.Value = agreement.MerchantAppTypeUID.ToUpper().ToString();
        //DM-7003 ask for directions
        HdnWoodforestUID.Value = agreement.MerchantAppTypeUID.ToUpper().ToString() == Constants.BANK_CITIZENS ? Constants.BANK_CITIZENS :  Constants.BANK_WOODFOREST; // WF or CB

        HdnBBVAUID.Value = Constants.BANK_BBVACOMPASS;// Code added for PXP-10225 by koshlendra
        //PXP-6183 abarua//PXP-10228 abarua
        if (!(agreement.Office.ToString() == "Irvine" && (agreement.MerchantAppTypeUID.ToUpper().ToString() == Constants.BANK_WOODFOREST 
            || agreement.MerchantAppTypeUID.ToUpper().ToString() == Constants.BANK_WOODFOREST_SB
            || agreement.MerchantAppTypeUID.ToUpper().ToString() == Constants.BANK_CITIZENS
            || agreement.MerchantAppTypeUID.ToUpper().ToString() == Constants.BANK_BBVACOMPASS)))
        {
            trEBT.Visible = false;
        }
        agreement.MerchantProcessingFee = facade.GetMerchantAppProcessingFee(agreement.ID);
        MapMerchantProcessingFeeToWeb(agreement.MerchantProcessingFee);

        bool isAch = agreement.AchID > 0 && agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY;

        if (isAch)
        {
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));
            UserSessions.ActiveAchMerchant.CloneAchMerchant();
        }

        if (agreement.Brand == MerchantBrand.Optimal)
        {
            // load currency before we bind merchantapp to panel
            LookupTableHandler.LoadCurrencyCodes(GeneralInvoiceCurrency, false);
        }

        //This is to determine which card types are not selected and hide those columns on Merchant Processing Fees
        Dictionary<string, bool> SelectedCardTypes = new Dictionary<string, bool>();

        SelectedCardTypes.Add("VI", agreement.Visa);
        SelectedCardTypes.Add("MC", agreement.MasterCard);
        SelectedCardTypes.Add("MD", agreement.Maestro);
        SelectedCardTypes.Add("DI", agreement.DiscoverNovus);
        SelectedCardTypes.Add("AM", agreement.AmericanExpress);
        SelectedCardTypes.Add("AO", agreement.AmexOP);
        SelectedCardTypes.Add("AB", agreement.AmexOB);
        SelectedCardTypes.Add("DC", agreement.Diners);
        SelectedCardTypes.Add("JC", agreement.Jcb);
        SelectedCardTypes.Add("VE", agreement.VisaElectron);

        StringBuilder strCardTypes = new StringBuilder();
        foreach (KeyValuePair<string, bool> item in SelectedCardTypes)
        {
            if (!item.Value)
            {
                strCardTypes.Append(item.Key + ",");
            }
        }
        this.DisableCardTypes.Value = strCardTypes.ToString().TrimEnd(',');
        this.Brand.Value = agreement.Brand.ToString();


        //get list of products
        int merchantId = 0;
        int.TryParse(agreement.ID, out merchantId);

        MerchantAccountFee accountFee = facade.GetMerchantAccountFee(merchantId);
        agreement.AccountFee = accountFee;

        UserSessions.CurrentMerchantApp = agreement;

        DropDownList status = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
        LookupTableHandler.MerchantAppStatus(status, false, "Merchant Management", agreement.StatusName.Substring(0, 2));

        if (isAch && UserSessions.ActiveAchMerchant != null)
        {
            DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
            LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2));
        }

        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormBinding.BindObjectToControls(MapMerchantProcessingFeeToWeb(agreement.MerchantProcessingFee), PanelProcessingFee);
        FormBinding.BindObjectToControls(accountFee, pnlDetail);
        BindPreviousVoidFee();

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        WucBusinessInfo1.SelectButton.Enabled = this.EditMode;

        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        //Niranjan :PXP-4989 Zeus: Add fields in 'Transaction Information' section on Fees Page.
        //Bug fix PXP-6485
        //PXP-18631: Code Change : Start
        if (UserSessions.CurrentMerchantApp != null)
        {
            if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
            {
                txtTotalSalesType.Value = Convert.ToDouble(TinfoStoreFrontSwipedPercent.Value) + Convert.ToDouble(TinfoInterntPercent.Value) + Convert.ToDouble(TinfoMailOrderPercent.Value) + Convert.ToDouble(TinfoOffPremisePercent.Value) + Convert.ToDouble(TinfoTradeShowPercent.Value) + Convert.ToDouble(TinfoOtherPercent.Value);
            }
            else
            {
                txtTotalSalesType.Value = Convert.ToDouble(TinfoStoreFrontSwipedPercent.Value) + Convert.ToDouble(TinfoInterntPercent.Value) + Convert.ToDouble(TinfoMailOrderPercent.Value) + Convert.ToDouble(TinfoTelephoneOrderPercent.Value);
            }
        }
        //PXP-18631: Code Change : End
        txtTotalTransCompleted.Value = Convert.ToDouble(TinfoElectronicDataCaptureSwipedPercent.Value) + Convert.ToDouble(TinfoManualEntryWithImprintPercent.Value) + Convert.ToDouble(TinfoManualEntryNoCardNoImprintPercent.Value) + Convert.ToDouble(TinfoVoiceAuthPercent.Value);

        MasterPageMerchant master = (MasterPageMerchant)this.Master;
        master.ShowNotes(agreement.UWNotes, agreement.AgentMemo, agreement.FirstTeamNotes, this.EditMode);

        DropDownList ReleaseMethodUID = (DropDownList)WucBusinessInfo1.FindControl("ReleaseMethodUID");
        ReleaseMethodUID.Enabled = false;

        DropDownList Discount = (DropDownList)WucBusinessInfo1.FindControl("DiscountMethod");
        Discount.Enabled = false;

        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == "8AA21143-7A97-461C-AAA3-99482DB75876" || UserSessions.CurrentUser.UserName.ToLower() == "mnguyen")
            MonthlyProfit.Enabled = true;
        else
            MonthlyProfit.Enabled = false;

        bool CCApproved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED) && !isAch;
        bool ACHApproved = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_APPROVED) && isAch;

        // CU-Approved Lockdown! only members of role "Special Access" can edit these fields once an account has been approved.
        if (this.EditMode == true && (CCApproved || ACHApproved))
        {
            bool has_special_access_role = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS)
                && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS].Enabled == true);

            if (!has_special_access_role)
            {
                FormHandler.SetControlEditMode(pnlDetail, false);
            }
        }


        WucBusinessInfo1.LoadOffice(agreement);
        BindFulfillment();
        if (merchantId > 0)
        {
            if (agreement.Brand == MerchantBrand.Optimal)
            {
                //show optimal fees
                ToggleMeritusFees(false);
                ToggleOptimalFees(true);

                //hide Meritus product fees
                this.pnlProducts.Visible = false;

            }
            else
            {
                //show meritus fees
                List<Subscription> subscriptions = DataProduct.GetMerchantCurrentProductSubscriptionList(merchantId);

                if (subscriptions != null)
                {
                    trUpdateXP.Visible = subscriptions.Find(p => p.Product.ProductId == ProductTypes.UPDATE_XP.GetHashCode()) != null ? true : false;
                    trFraudXP.Visible = subscriptions.Find(p => p.Product.ProductId == ProductTypes.FXP.GetHashCode()) != null ? true : false;
                    trCBMS.Visible = subscriptions.Find(p => p.Product.ProductId == ProductTypes.CBMS.GetHashCode()) != null ? true : false;
                    trCBMSPlus.Visible = subscriptions.Find(p => p.Product.ProductId == ProductTypes.CB_REFUND.GetHashCode()) != null ? true : false;
                    trApriva.Visible = subscriptions.Find(p => p.Product.ProductId == ProductTypes.APRIVA.GetHashCode()) != null ? true : false;
                    trRDR.Visible = subscriptions.Find(p => p.Product.ProductId == ProductTypes.RDR.GetHashCode()) != null ? true : false;
                }

                ToggleMeritusFees(true);
                ToggleOptimalFees(false);
            }
        }
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message     
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled &&!this.EditMode))
        {
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }
        /******** End of PXP-2206 **************/

        //DM-4228 //DM-4347
        if (!UserSessions.CurrentMerchantApp.RDRHandlingWaived
            && UserSessions.CurrentMerchantApp.RDRHandlingFee == 0)
        {
            try
            {
                var currentAgentUID = new Guid(UserSessions.CurrentMerchantApp.AgentUID);
                var currentMerchantAppID = int.Parse(UserSessions.CurrentMerchantApp.ID);
                var productList = DataProduct.GetAgentProductList(currentAgentUID, currentMerchantAppID);

                if (productList != null)
                {
                    var rdrHandlingProduct = productList.Find(p => p.ProductId == (int)ProductTypes.RDRHandling);
                    if (rdrHandlingProduct != null)
                    {
                        var fee = rdrHandlingProduct.FeeList.Find(p => p.FeeId == 850);

                        if (fee != null)
                        {
                            RDRHandlingFee.Text = fee.MerchantCost.ToString();
                            //trRDRsetup.Visible = fee.IsVisible;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                ZeusWeb.Logging.ErrorLog.ErrorFormat("Couldn't get prducto RDR Handling for this agent " + UserSessions.CurrentMerchantApp.AgentUID + " and " + UserSessions.CurrentMerchantApp.ID + " merchant", ex);
            }
        }
        //DM-4228
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
        DebitMonthlyFee.Value = 0;
        AvsVoiceAuthFee.Value = 1.50;
        AvsFee.Value = .10;
        StatementFee.Value = 10;
        MerchantClub.Value = 0;
        MonthlyMinimumFee.Value = 25;
        WirelessPerTrans.Value = 0;
        WirelessServiceFee.Value = 0;
        ApplicationFee.Value = 0;
        AnnualFee.Value = 0;
        VoiceAuthFee.Value = 1.00;
        ReturnItemFee.Value = 25;
        WirelessPerDeviceFee.Value = 0;
        //code changes for PXP-18232 start
        HighRiskMonitoringFee.Value = 0;
        //code changes for PXP-18232 end

        //DM-2957 ini
        RDRSetupFee.Value = 0;
        RDRMonthlyFee.Value = 0;
        RDRTransFee.Value = 0;
        RDRHandlingFee.Value = 0;
        //DM-2957 end
    }

    public override bool FormSave()
    {
        bool perform = false;
        DataSet dsmultiLink = new DataSet();

        MerchantApp agreement = null;
        AchMerchant achMerchant = null;

        if (this.Adding)
            agreement = new MerchantApp();
        else
            agreement = (MerchantApp)UserSessions.CurrentMerchantApp;

        agreement.CloneMerchantApp();

        string OrigStatus = agreement.StatusUID.ToUpper();
        string OrigAgentUID = agreement.AgentUID.ToUpper();
        string OrigETF = agreement.ETFAssessed;

        if (isACHonly)
        {
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));
            UserSessions.ActiveAchMerchant.CloneAchMerchant();
            achMerchant = UserSessions.ActiveAchMerchant;
            OrigStatus = achMerchant.MerchantStatusUID.ToUpper();
        }

        FormBinding.BindControlsToObject(agreement, pnlDetail);

        WebUtilMerchantProcessingFee webutilmerchant = new WebUtilMerchantProcessingFee();
        FormBinding.BindControlsToObject(webutilmerchant, PanelProcessingFee);

        MerchantAccountFee accountFee = new MerchantAccountFee();
        FormBinding.BindControlsToObject(accountFee, pnlDetail);
        agreement.AccountFee = accountFee;
        BindPreviousVoidFeeToControl(agreement);//Changes doen forPXP-12634   

        UserSessions.CurrentMerchantApp = agreement;
        //Niranjan :PXP-4989 Zeus: Add fields in 'Transaction Information' section on Fees Page.
        //Bug fix PXP-6485
        //PXP-18631: Code Change : Start
        if (agreement.MerchantAppTypeUID != null)
        {
            if ((agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && agreement.Office == CommonUtility.Util.Offices.Irvine)
            {
                txtTotalSalesType.Value = Convert.ToDouble(TinfoStoreFrontSwipedPercent.Value) + Convert.ToDouble(TinfoInterntPercent.Value) + Convert.ToDouble(TinfoMailOrderPercent.Value) + Convert.ToDouble(TinfoOffPremisePercent.Value) + Convert.ToDouble(TinfoTradeShowPercent.Value) + Convert.ToDouble(TinfoOtherPercent.Value);
            }
            else
            {
                txtTotalSalesType.Value = Convert.ToDouble(TinfoStoreFrontSwipedPercent.Value) + Convert.ToDouble(TinfoInterntPercent.Value) + Convert.ToDouble(TinfoMailOrderPercent.Value) + Convert.ToDouble(TinfoTelephoneOrderPercent.Value);
            }
        }
        //PXP-18631: Code Change : End

        txtTotalTransCompleted.Value = Convert.ToDouble(TinfoElectronicDataCaptureSwipedPercent.Value) + Convert.ToDouble(TinfoManualEntryWithImprintPercent.Value) + Convert.ToDouble(TinfoManualEntryNoCardNoImprintPercent.Value) + Convert.ToDouble(TinfoVoiceAuthPercent.Value);

        if (!this.FormDataCheck())
            return false;

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        User user = UserSessions.CurrentUser;
        agreement.UserUpdated = user.UserName;

        if (OrigAgentUID != agreement.AgentUID.ToUpper())
            data.DeleteMerchantAgentContract(agreement.MerchantAppUID);


        agreement.UWNotes = ((TextBox)this.Master.FindControl("UWNotesEdit")).Text;
        agreement.FirstTeamNotes = ((TextBox)this.Master.FindControl("FirstTeamNotesEdit")).Text;

        MerchantFacade facade = new MerchantFacade();
        int rows = facade.UpdateMerchantApp(agreement);
        facade.UpdateMerchantAppProcessingFee(agreement, MapMerchantProcessingFeeToBusiness(webutilmerchant, agreement), user.UserName, Constants.PORTAL_ZEUS);

        if (rows > 0)
        {
            facade.UpdateMerchantAccountFee(agreement, accountFee, user.UserName, Constants.PORTAL_ZEUS);

            FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);

            UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

            FormHandler.CompleteApplication(agreement, achMerchant, OrigStatus, user.UserName);

        }

        SaveUWFulfillment();
        perform = true;
        return perform;
    }

    private void SaveUWFulfillment()
    {

        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UWFulfillment uwfulfillment = data.GetUWFulfillment(UserSessions.CurrentMerchantApp.ID);
        if (uwfulfillment.ZID == 0)
        {
            uwfulfillment = new UWFulfillment { ZID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID) };
        }
        FormBinding.BindControlsToObject(uwfulfillment, pnlUWFulfillment);

        data.SaveUWFulfillment(uwfulfillment, UserSessions.CurrentUser.UserName);
    }

    private void BindFulfillment()
    {
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UWFulfillment uwfulfillment = data.GetUWFulfillment(UserSessions.CurrentMerchantApp.ID);

        FormBinding.BindObjectToControls(uwfulfillment, pnlUWFulfillment);
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);
    }

    public override bool FormDataCheck()
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;
        AchMerchant achmerchant = UserSessions.ActiveAchMerchant;
        string m_Status = string.Empty;
        string m_CloneStatusUID = string.Empty;

        if (isACHonly && achmerchant != null)
        {
            m_Status = achmerchant.MerchantStatusUID;

            if (achmerchant.AchMerchantClone != null)
                m_CloneStatusUID = achmerchant.AchMerchantClone.MerchantStatusUID;
        }
        else
        {
            m_Status = app.StatusUID;

            if (app.MerchantAppClone != null)
                m_CloneStatusUID = app.MerchantAppClone.StatusUID;
        }
        //Niranjan :PXP-4989 Zeus: Add fields in 'Transaction Information' section on Fees Page.
        decimal Totaltranstype = 0;
        //Bug fix PXP-6485
        //PXP-18631: Code Change : Start
        if (UserSessions.CurrentMerchantApp != null)
        {
            if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS) && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
            {
                Totaltranstype = app.TinfoStoreFrontSwipedPercent + app.TinfoInterntPercent + app.TinfoMailOrderPercent + app.TinfoOffPremisePercent + app.TinfoTradeShowPercent + app.TinfoOtherPercent;
            }
            else
            {
                Totaltranstype = app.TinfoStoreFrontSwipedPercent + app.TinfoInterntPercent + app.TinfoMailOrderPercent + app.TinfoTelephoneOrderPercent;
            }
        }
        //PXP-18631: Code Change : End
        if (Totaltranstype != 100 && Totaltranstype != 0)
        {
            this.Master.AddMessageError(Constants.GetDescription(Constants.ErrorCodes.Fees_TotalTransactionTypePercentage));
        }

        decimal Totaltransction = app.TinfoElectronicDataCaptureSwipedPercent + app.TinfoManualEntryWithImprintPercent + app.TinfoManualEntryNoCardNoImprintPercent + app.TinfoVoiceAuthPercent;
        if (Totaltransction != 100 && Totaltranstype != 0)
        {
            this.Master.AddMessageError(Constants.GetDescription(Constants.ErrorCodes.Fees_TotalTransactionCompletedPercentage));
        }
        //Fmassoud 2017.08.28 Sending New Status to Formhandler
        DropDownList _statusUID = isACHonly ? (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID") : (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
        IList<string> message = FormHandler.MerchantDataCheck(app, false, this.Adding, _statusUID.SelectedValue.ToUpper(),UserSessions.ActiveAchMerchant);

        if (UserSessions.CurrentMerchantApp != null)
        {
            decimal totPeriodVlm = 0;
            decimal[] periodVolumes = { Convert.ToDecimal(Period1Volume.Value), Convert.ToDecimal(Period2Volume.Value), Convert.ToDecimal(Period3Volume.Value) };
            string[] ndxDays = { this.Period1NDXDays.Text.Trim(), this.Period2NDXDays.Text.Trim(), this.Period3NDXDays.Text.Trim() };

            message = FormHandler.RiskEvaluationCheck(app, m_Status, m_CloneStatusUID, ndxDays, periodVolumes, out totPeriodVlm, message);
            this.TotalPeriodVolume.Value = totPeriodVlm.ToString();
        }

        if (message.Count > 0)
        {
            foreach (string mess in message)
                this.Master.AddMessageError(mess);
        }

        //Ani:START: PXP3144 [CAU: Bill per match fee to UpdateXP merchant]
        if (app.AccountUpdaterOn && AUPerMatchFee.ValueDecimal <= 0M && !this.AUPerMatchWaived.Checked)
            this.Master.AddMessageError("Update XP per match fee should be greater than 0.");
        //Ani:END: PXP3144 [CAU: Bill per match fee to UpdateXP merchant]

        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            ListHandler.ListFindItem(((DropDownList)WucBusinessInfo1.FindControl("StatusUID")), app.MerchantAppClone.StatusUID);
            app.StatusUID = app.MerchantAppClone.StatusUID;

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
        DropDownList cbo = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
        if (cbo != null)
            ListHandler.ListFindItem(cbo, "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409"); //Application Processing Received Status       
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

    protected void btnViewPDF_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (this.FormSave())
            this.FormShow(this.UID);
        else
            return;
        MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
        User user = UserSessions.CurrentUser;
        string OutputFileName = user.UserName + " - Merchant Application.pdf";
        Response.Redirect("~/PDF/" + OutputFileName);
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
                if (this.FormSave())
                {
                    this.EditMode = false;
                    this.ToggleButtons();
                    this.Adding = false;
                    this.FormShow(this.UID);
                }
                break;
            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                this.FormShow(this.UID);
                break;
            case "Cancel":
                if (string.IsNullOrWhiteSpace(this.UID))
                    this.CloseForm();
                else
                    this.FormCancel();
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
            case "PDF":
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
                    this.ToggleWaivableFees();
                    this.HiddenEditMode.Value = "True";
                }
                break;
            case "Copy":
                break;
        }
    }

    private void CopyMerchantApp()
    {
        //DataMerchantApp data = DataAccess.DataMerchantAppDao;
        //MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
        //agreement.UserCreated = UserSessions.CurrentUser.UserName;
        //data.InsertCopyMerchantApp(agreement);
        //this.UID = agreement.MerchantAppUID;
        //this.FormShow(this.UID);
    }

    private void ToggleMeritusFees(bool show)
    {
        this.trAnnualFee.Visible = show;
        this.trVoiceAuthFee.Visible = show;
        this.trVRUARUFee.Visible = show;
        this.trAvsVoiceAuthFee.Visible = show;
        this.trRegulatoryFee.Visible = show;
        this.trAvsFee.Visible = show;
        this.trMerchantClub.Visible = show;
        this.trPCIFee.Visible = show;
        this.trOtherFees.Visible = show;
    }

    private void ToggleOptimalFees(bool show)
    {
        this.trIntlAssociationFee.Visible = show;
        this.trDuesAssociationFee.Visible = show;
        this.trGeneralInvoice.Visible = show;
        this.trSource.Visible = show;
        this.trCBInsuFee.Visible = show;
        this.trPCIInsuFee.Visible = show;
        this.trPciNonComply.Visible = show;
        this.trBACSReturnFee.Visible = show;
        this.trBACSTransFee.Visible = show;
        this.trEFTReturnFee.Visible = show;
        this.trEFTTransFee.Visible = show;
        this.trPaymentReturnFee.Visible = show;
        this.trPaymentTransFee.Visible = show;
        this.trWIREReturnFee.Visible = show;
        this.trWIRETransFee.Visible = show;
        this.trGenerateInvoiceIn.Visible = show;
    }

    private void ToggleWaivableFees()
    {
        this.GatewayTransFee.Enabled = !this.GatewayTransWaived.Checked;
        this.GatewayMonthlyFee.Enabled = !this.GatewayMonthlyWaived.Checked;
        this.GatewaySetupFee.Enabled = !this.GatewaySetupWaived.Checked;

        this.CBMSMonthly1.Enabled = !this.CBMSMonthly1Waived.Checked;
        this.CBMSMonthly2.Enabled = !this.CBMSMonthly2Waived.Checked;
        this.CBMSSetupFee.Enabled = !this.CBMSSetupWaived.Checked;

        this.CBMSPlusAlertFee.Enabled = !this.CBMSPlusAlertWaived.Checked;

        this.FraudXPMonthlyFee.Enabled = !this.FraudXPMonthlyWaived.Checked;
        this.FraudXPSetupFee.Enabled = !this.FraudXPSetupWaived.Checked;
        this.FraudXPTransFee.Enabled = !this.FraudXPTransWaived.Checked;

        this.AUMonthlyFee.Enabled = !this.AUMonthlyWaived.Checked;
        this.AUSetupFee.Enabled = !this.AUSetupWaived.Checked;

        //Ani:START: PXP3144 [CAU: Bill per match fee to UpdateXP merchant]
        this.AUPerMatchFee.Enabled = !this.AUPerMatchWaived.Checked;
        //Ani:END: PXP3144 [CAU: Bill per match fee to UpdateXP merchant]

        this.WirelessPerTrans.Enabled = !this.WirelessPerTransWaived.Checked;
        this.WirelessServiceFee.Enabled = !this.WirelessServiceWaived.Checked;
        this.WirelessStmntFee.Enabled = !this.WirelessStmntWaived.Checked;
        this.WirelessPerDeviceFee.Enabled = !this.WirelessPerDeviceWaived.Checked;

        this.EarlyTerminationFee.Enabled = !this.EarlyTerminationWaived.Checked;

        //ANI : START : PXP-6123
        this.ThirdPartyTransFee.Enabled = !this.ThirdPartyTransWaived.Checked;
        this.ThirdPartyMonthlyFee.Enabled = !this.ThirdPartyMonthlyWaived.Checked;
        this.ThirdPartySetupFee.Enabled = !this.ThirdPartySetupWaived.Checked;
        //ANI : END : PXP-6123

        //DM-2957 ini
        this.RDRTransFee.Enabled = !this.RDRTransWaived.Checked;
        this.RDRMonthlyFee.Enabled = !this.RDRMonthlyWaived.Checked;
        this.RDRSetupFee.Enabled = !this.RDRSetupWaived.Checked;
        //this.RDRHandlingFee.Enabled = !this.RDRHandlingWaived.Checked;  DM-4228
        //DM-2957 end

        //DM-4228
        switch (UserSessions.CurrentUser.DefaultRoleUID.ToUpper())
        {
            case Constants.ROLE_QUALITY:
            case Constants.ROLE_OPERATIONS:
            case Constants.ROLE_SALES_SUPPORT:
                {
                    if (this.EditMode)
                    {
                        RDRHandlingFee.Enabled = !this.RDRHandlingWaived.Checked;
                        RDRHandlingWaived.Enabled = true;
                    }
                    break;
                }
            default:
                {
                    RDRHandlingFee.Enabled = false;
                    RDRHandlingWaived.Enabled = false;
                    break;
                }
        }
        //DM-4228
    }
    //Koshlendra : Start : PXP-4513
    private void BindPreviousVoidFee()
    {

        if(this.EarlyTerminationWaived.Checked)
        {
            this.EarlyTerminationFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.EarlyTerminationFeeWaived.ToString();
        }
        else
        {
            this.EarlyTerminationFee.ToolTip="";
        }
        if(this.GatewayMonthlyWaived.Checked)
        {
            this.GatewayMonthlyFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.GatewayMonthlyFeeWaived.ToString();
        }
        else
        {
            this.GatewayMonthlyFee.ToolTip="";
        }
        if(this.GatewayTransWaived.Checked)
        {
            this.GatewayTransFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.GatewayTransFeeWaived.ToString();
        }
        else
        {
            this.GatewayTransFee.ToolTip="";
        }
        if(this.GatewaySetupWaived.Checked)
        {
            this.GatewaySetupFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.GatewaySetupFeeWaived.ToString();
        }
        else
        {
            this.GatewaySetupFee.ToolTip="";
        }
        if(this.FraudXPTransWaived.Checked)
        {
            this.FraudXPTransFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.FraudXPTransFeeWaived.ToString();
        }
        else
        {
            this.FraudXPTransFee.ToolTip="";
        }
        if(this.FraudXPMonthlyWaived.Checked)
        {
            this.FraudXPMonthlyFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.FraudXPMonthlyFeeWaived.ToString();
        }
        else
        {
            this.FraudXPMonthlyFee.ToolTip="";

        }
        if(this.FraudXPSetupWaived.Checked)
        {
            this.FraudXPSetupFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.FraudXPSetupFeeWaived.ToString();
        }
        else
        {
            this.FraudXPSetupFee.ToolTip="";
        }
        if(this.CBMSMonthly1Waived.Checked)
        {
            this.CBMSMonthly1.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.CBMSMonthly1FeeWaived.ToString();
        }
        else
        {
            this.CBMSMonthly1.ToolTip="";
        }
        if(this.CBMSMonthly2Waived.Checked)
        {
            this.CBMSMonthly2.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.CBMSMonthly2FeeWaived.ToString();
        }
        else
        {
            this.CBMSMonthly2.ToolTip="";
        }
        if(this.CBMSSetupWaived.Checked)
        {
            this.CBMSSetupFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.CBMSSetupFeeWaived.ToString();
        }
        else
        {
            this.CBMSSetupFee.ToolTip="";
        }
        if(this.AUPerMatchWaived.Checked)
        {
            this.AUPerMatchFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.AUPerMatchfeeWaived.ToString();
        }
        else
        {
            this.AUPerMatchFee.ToolTip="";
        }
        if(this.AUMonthlyWaived.Checked)
        {
            this.AUMonthlyFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.AUMonthlyFeeWaived.ToString();
        }
        else
        {
            this.AUMonthlyFee.ToolTip="";
        }
        if(this.AUSetupWaived.Checked)
        {
            this.AUSetupFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.AUSetupFeeWaived.ToString();
        }
        else
        {
            this.AUSetupFee.ToolTip="";
        }
        if(this.CBMSPlusAlertWaived.Checked)
        {
            this.CBMSPlusAlertFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.CBMSPlusAlertFeeWaived.ToString();
        }
        else
        {
            this.CBMSPlusAlertFee.ToolTip="";
        }
        if(this.WirelessPerTransWaived.Checked)
        {
            this.WirelessPerTrans.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.WirelessPerTransFeeWaived.ToString();
        }
        else
        {
            this.WirelessPerTrans.ToolTip="";
        }
        if(this.WirelessServiceWaived.Checked)
        {
            this.WirelessServiceFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.WirelessServiceFeeWaived.ToString();
        }        
        else
        {
            this.WirelessServiceFee.ToolTip="";
        }
        if(this.WirelessStmntWaived.Checked)
        {
            this.WirelessStmntFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.WirelessStmntFeeWaived.ToString();
        }
        else
        {
            this.WirelessStmntFee.ToolTip="";
        }
        if(this.WirelessPerDeviceWaived.Checked)
        {
            this.WirelessPerDeviceFee.ToolTip= "Amount Waived: $"+ UserSessions.CurrentMerchantApp.AccountFee.WirelessPerDeviceFeeWaived.ToString();
        }
        else
        {
            this.WirelessPerDeviceFee.ToolTip="";
        }

        //DM-3677 ini        
        if (this.RDRTransWaived.Checked)
        {
            this.RDRTransFee.ToolTip = "Amount Waived: $" + UserSessions.CurrentMerchantApp.AccountFee.RDRTransFee.ToString();
        }
        else
        {
            this.RDRTransFee.ToolTip = "";
        }
        if (this.RDRMonthlyWaived.Checked)
        {
            this.RDRMonthlyFee.ToolTip = "Amount Waived: $" + UserSessions.CurrentMerchantApp.AccountFee.RDRMonthlyFee.ToString();
        }
        else
        {
            this.RDRMonthlyFee.ToolTip = "";
        }
        if (this.RDRSetupWaived.Checked)
        {
            this.RDRSetupFee.ToolTip = "Amount Waived: $" + UserSessions.CurrentMerchantApp.AccountFee.RDRSetupFee.ToString();
        }
        else
        {
            this.RDRSetupFee.ToolTip = "";
        }
        if (this.RDRHandlingWaived.Checked)
        {
            this.RDRHandlingFee.ToolTip = "Amount Waived: $" + UserSessions.CurrentMerchantApp.AccountFee.RDRHandlingFee.ToString();
        }
        else
        {
            this.RDRHandlingFee.ToolTip = "";
        }
        //DM-3677 end
    }

    private void BindPreviousVoidFeeToControl( MerchantApp agreement )
    {

        if((agreement.AccountFee.EarlyTerminationFee== agreement.MerchantAppClone.AccountFee.EarlyTerminationFee) &&  this.EarlyTerminationWaived.Checked)
        {
            agreement.AccountFee.EarlyTerminationFeeWaived= agreement.MerchantAppClone.AccountFee.EarlyTerminationFeeWaived;
        }

        if(agreement.AccountFee.GatewayMonthlyFee== agreement.MerchantAppClone.AccountFee.GatewayMonthlyFee && this.GatewayMonthlyWaived.Checked)
        {
            agreement.AccountFee.GatewayMonthlyFeeWaived= agreement.MerchantAppClone.AccountFee.GatewayMonthlyFeeWaived;
        }

        if(agreement.AccountFee.GatewayTransFee== agreement.MerchantAppClone.AccountFee.GatewayTransFee && this.GatewayTransWaived.Checked)
        {
            agreement.AccountFee.GatewayTransFeeWaived= agreement.MerchantAppClone.AccountFee.GatewayTransFeeWaived;
        }

        if(agreement.AccountFee.GatewaySetupFee== agreement.MerchantAppClone.AccountFee.GatewaySetupFee && this.GatewaySetupWaived.Checked)
        {
            agreement.AccountFee.GatewaySetupFeeWaived= agreement.MerchantAppClone.AccountFee.GatewaySetupFeeWaived;
        }

        if(agreement.AccountFee.FraudXPTransFee== agreement.MerchantAppClone.AccountFee.FraudXPTransFee && this.FraudXPTransWaived.Checked)
        {
            agreement.AccountFee.FraudXPTransFeeWaived= agreement.MerchantAppClone.AccountFee.FraudXPTransFeeWaived;
        }

        if(agreement.AccountFee.FraudXPMonthlyFee== agreement.MerchantAppClone.AccountFee.FraudXPMonthlyFee && this.FraudXPMonthlyWaived.Checked)
        {
            agreement.AccountFee.FraudXPMonthlyFeeWaived= agreement.MerchantAppClone.AccountFee.FraudXPMonthlyFeeWaived;
        }

        if(agreement.AccountFee.FraudXPSetupFee== agreement.MerchantAppClone.AccountFee.FraudXPSetupFee && this.FraudXPSetupWaived.Checked)
        {
            agreement.AccountFee.FraudXPSetupFeeWaived= agreement.MerchantAppClone.AccountFee.FraudXPSetupFeeWaived;
        }

        if(agreement.AccountFee.CBMSMonthly1== agreement.MerchantAppClone.AccountFee.CBMSMonthly1 && this.CBMSMonthly1Waived.Checked)
        {
            agreement.AccountFee.CBMSMonthly1FeeWaived= agreement.MerchantAppClone.AccountFee.CBMSMonthly1FeeWaived;
        }

        if(agreement.AccountFee.CBMSMonthly2== agreement.MerchantAppClone.AccountFee.CBMSMonthly2 && this.CBMSMonthly2Waived.Checked)
        {
            agreement.AccountFee.CBMSMonthly2FeeWaived= agreement.MerchantAppClone.AccountFee.CBMSMonthly2FeeWaived;
        }

        if(agreement.AccountFee.CBMSSetupFee== agreement.MerchantAppClone.AccountFee.CBMSSetupFee && this.CBMSSetupWaived.Checked)
        {
            agreement.AccountFee.CBMSSetupFeeWaived= agreement.MerchantAppClone.AccountFee.CBMSSetupFeeWaived;
        }

        if(agreement.AccountFee.AUPerMatchFee== agreement.MerchantAppClone.AccountFee.AUPerMatchFee && this.AUPerMatchWaived.Checked)
        {
            agreement.AccountFee.AUPerMatchfeeWaived= agreement.MerchantAppClone.AccountFee.AUPerMatchfeeWaived;
        }

        if(agreement.AccountFee.AUMonthlyFee== agreement.MerchantAppClone.AccountFee.AUMonthlyFee && this.AUMonthlyWaived.Checked)
        {
            agreement.AccountFee.AUMonthlyFeeWaived= agreement.MerchantAppClone.AccountFee.AUMonthlyFeeWaived;
        }

        if(agreement.AccountFee.AUSetupFee== agreement.MerchantAppClone.AccountFee.AUSetupFee && this.AUSetupWaived.Checked)
        {
            agreement.AccountFee.AUSetupFeeWaived= agreement.MerchantAppClone.AccountFee.AUSetupFeeWaived;
        }
        if(agreement.AccountFee.CBMSPlusAlertFee== agreement.MerchantAppClone.AccountFee.CBMSPlusAlertFee && this.CBMSPlusAlertWaived.Checked)
        {
            agreement.AccountFee.CBMSPlusAlertFeeWaived= agreement.MerchantAppClone.AccountFee.CBMSPlusAlertFeeWaived;
        }

        if(agreement.AccountFee.WirelessPerTrans== agreement.MerchantAppClone.AccountFee.WirelessPerTrans && this.WirelessPerTransWaived.Checked)
        {
            agreement.AccountFee.WirelessPerTransFeeWaived= agreement.MerchantAppClone.AccountFee.WirelessPerTransFeeWaived;
        }

        if(agreement.AccountFee.WirelessServiceFee== agreement.MerchantAppClone.AccountFee.WirelessServiceFee && this.WirelessServiceWaived.Checked)
        {
            agreement.AccountFee.WirelessServiceFeeWaived= agreement.MerchantAppClone.AccountFee.WirelessServiceFeeWaived;
        }

        if(agreement.AccountFee.WirelessStmntFee== agreement.MerchantAppClone.AccountFee.WirelessStmntFee && this.WirelessStmntWaived.Checked)
        {
            agreement.AccountFee.WirelessStmntFeeWaived= agreement.MerchantAppClone.AccountFee.WirelessStmntFeeWaived;
        }

        if(agreement.AccountFee.WirelessPerDeviceFee== agreement.MerchantAppClone.AccountFee.WirelessPerDeviceFee && this.WirelessPerDeviceWaived.Checked)
        {
            agreement.AccountFee.WirelessPerDeviceFeeWaived= agreement.MerchantAppClone.AccountFee.WirelessPerDeviceFeeWaived;
        }
    }
    //Koshlendra : End : PXP-4513

}
