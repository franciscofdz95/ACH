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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Collections.Generic;
using System.Linq;
using CommonUtility;

public partial class wucBusinessInfo : System.Web.UI.UserControl
{
    public delegate void TextChangeHandler(object sender, EventArgs e);
    public event TextChangeHandler TextChange;

    public event EventHandler MerchantAppTypeUIDChanged;
    public event EventHandler OfficeIDChanged;

    private void OnMerchantAppTypeUIDChanged()
    {
        if (MerchantAppTypeUIDChanged != null)
        {
            MerchantAppTypeUIDChanged(this, EventArgs.Empty);
        }
    }

    //Added event handler for validation of phone number, Country Code and Extention 
    private void OnOfficeIDChanged()
    {
        if (OfficeIDChanged != null)
        {
            OfficeIDChanged(this, EventArgs.Empty);
        }
    }

    private MerchantApp _MyMerchantApp = null;

    public MerchantApp MyMerchantApp
    {
        get { return _MyMerchantApp; }
        set { _MyMerchantApp = value; }
    }

    public LinkButton SelectButton
    {
        get { return wucAgentSelector.btnSelect; }
    }

    public bool Adding
    {
        get
        {
            if (ViewState["Adding"] != null)
                return Convert.ToBoolean(ViewState["Adding"]);
            else
                return false;
        }
        set { ViewState["Adding"] = value; }
    }
        
    public Panel pnlInfo
    {
        get { return pnlGeneralInfo; }

    }

    public bool isACHonly
    {
        set { ViewState["isACHonly"] = value; }
        get { return CommonUtility.Util.if_b(ViewState["isACHonly"], false); }
    }

    public static IList<GenericListItem> m_ChannelSalesManager;
    protected override void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management");
            LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management");
            LookupTableHandler.LoadFrontEndPlatforms(AuthPlatformUID, false);
            LookupTableHandler.LoadBackEndPlatforms(SettlePlatformUID, false);
            LookupTableHandler.LoadReleaseMethods(this.ReleaseMethodUID, false);
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, false, true);
            LookupTableHandler.LoadInternalUsers(PrimaryContactUID, false);
            LookupTableHandler.LoadSalesSupport(SSQRep, false,Constants.ROLE_SALES_SUPPORT);
            LookupTableHandler.LoadMerchantClosureCodes(MerchantClosureCodeUID, false);
            LookupTableHandler.LoadMerchantRiskStatus(RiskStatus, false, 0); //Added Code by anuj for PXP-10756
            LookupTableHandler.LoadCountries(BusinessCountry);
            LookupTableHandler.LoadCountries(BusinessMailingCountry);
            LookupTableHandler.LoadOffices(OfficeID, false);
            LookupTableHandler.LoadLegalEntities(LegalEntityID);

            LookupTableHandler.LoadCurrencyCodes(Currency, false);
           LookupTableHandler.LoadOffices(listOfficeAccess);

            LookupTableHandler.LoadPaymentSchedule(PaymentScheduleID, false);
            LookupTableHandler.LoadPaymentFrequency(PaymentFrequencyID, false);

            //Added Business DBA phone country code
            LookupTableHandler.LoadCountryCallingCodes(BusinessDBAPhoneCountryCode);
            LookupTableHandler.LoadCountryCallingCodes(BusinessFaxCountryCode);
            LookupTableHandler.LoadbillingMethod(BillingMethodUID, false);
            LoadChannelSalesManager();

            if (UserSessions.CurrentMerchantApp != null)
            {
                if (!string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.MerchantAppTypeUID) && MerchantAppTypeUID.Items.FindByValue(UserSessions.CurrentMerchantApp.MerchantAppTypeUID) == null)
                {
                    MerchantAppTypeUID.Items.Add(new ListItem(UserSessions.CurrentMerchantApp.Bank, UserSessions.CurrentMerchantApp.MerchantAppTypeUID));
                }

                isACHonly = UserSessions.CurrentMerchantApp.AchID > 0 && UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY;
            }
        }
    }

    //Added Business DBA country code
    protected void Page_PreRender(object sender, EventArgs e)
    {
        DBACountryCodeDisplay.ReadOnly = true;
        FaxCountryCodeDisplay.ReadOnly = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WebUtil.SetUserSpecificDisplayMode(CancellationDate);

        TextBox AgentDBA = (TextBox)wucAgentSelector.FindControl("AgentDBA");
        TextBox agentID = (TextBox)wucAgentSelector.FindControl("AgentID");
        AgentDBA.ReadOnly = true;

        string m_StatusUID = string.Empty;

        // basically this control should exclusivly use MyMerchantApp. if its empty, then we pull from current merchant app.. if that's empty too then we just don't do anything.
        if (this.MyMerchantApp == null && UserSessions.CurrentMerchantApp != null)
        {
            this.MyMerchantApp = UserSessions.CurrentMerchantApp;
        }

        if (!this.IsPostBack)
        {
            if (this.MyMerchantApp != null)
            {
                //check if the accoutn is ach only
                isACHonly = this.MyMerchantApp.AchID > 0 && MerchantAppTypeUID.SelectedValue.ToUpper() == Constants.BANK_ACH_ONLY;

                if (this.MyMerchantApp.MonthendApproved)
                    DiscountMethod.SelectedIndex = 0;
                else
                    DiscountMethod.SelectedIndex = 1;

                ListHandler.ListFindItem(OfficeID, Convert.ToString((int)MyMerchantApp.Office));

                //Added Business DBA phone country code and extentions
                DBACountryCodeDisplay.Text = string.IsNullOrEmpty(this.MyMerchantApp.BusinessDBAPhoneCountryCode) ? "+1" : this.MyMerchantApp.BusinessDBAPhoneCountryCode;
                DBAPhoneExt.Text = this.MyMerchantApp.BusinessDBAPhoneExt.ToString();

                //Added Business Fax country code and extentions
                FaxCountryCodeDisplay.Text = string.IsNullOrEmpty(this.MyMerchantApp.BusinessFaxCountryCode) ? "+1" : this.MyMerchantApp.BusinessFaxCountryCode;
                BusinessFaxExt.Text = this.MyMerchantApp.BusinessFaxExt.ToString();

            }

            if (UserSessions.CurrentUser.IsBank)
            {
                MerchantAppTypeUID.Visible = false;
                txtBank.Visible = true;
                txtBank.Text = MerchantAppTypeUID.SelectedItem.Text;

                wucAgentSelector.btnSelect.Visible = false;
            }

            //if hte account is ach only then we use ach satus for closurecode dropdwon visibility
            //ETF dropdown is shown only for cc status
            if (isACHonly)
                m_StatusUID = ACHStatusUID.SelectedValue.ToUpper();
            else
                m_StatusUID = StatusUID.SelectedValue.ToUpper();

            MerchantApp app = UserSessions.CurrentMerchantApp;
            string visible = "none";
            string display = "none";

           // if (app != null && (m_StatusUID == Constants.QUEUESTATUS_MS_CANCELLATION || m_StatusUID == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION))
            if (app != null && (m_StatusUID == Constants.QUEUESTATUS_MS_CANCELLATION))  // ***PXP 1261 : remove closur code & ETF validation MS_RETENTION-PENDING-CANCELLATION status
            {
                display = "block";
                if (m_StatusUID == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION && !this.Adding)
                    visible = "block";
            }

            lblMerchantClosureCodeUID.Style["display"] = display;
            MerchantClosureCodeUID.Style["display"] = display;

            lblRiskStatus.Style["display"] = display;
            RiskStatus.Style["display"] = display;
            
            CancelDate.Style["display"] = visible;
            CancellationDate.Style["display"] = visible;
            lblCancel.Style["display"] = visible;

            AgentDBA.TabIndex = 90;
            agentID.TabIndex = 190;

            phPaymentSchedule.Visible = !this.Adding;
            phPaymentFrequency.Visible = !this.Adding;

            if (!this.Adding)
            {
                if (UserSessions.CurrentMerchantApp != null)
                {
                    setACHStatus();

                    Underwriting objUW = DataUnderwritng.GetInstance().LoadMerchantUWNotes(UserSessions.CurrentMerchantApp.MerchantAppUID);

                    if (objUW != null)
                    {
                        ListHandler.ListFindItem(PaymentScheduleID, Convert.ToString((int)objUW.PaymentScheduleID));
                        ListHandler.ListFindItem(PaymentFrequencyID, Convert.ToString((int)objUW.PaymentFrequencyID));
                    }
                }

            }
        }       

        SetAgentSelectorEditMode();
        if (OfficeID.SelectedValue.Equals("5"))
        {
            DivOfficeAccess.Visible = true;

        }

        phAssociationNumber.Visible = (MerchantAppTypeUID.SelectedValue.ToUpper() == Constants.BANK_CHESAPEAKE  &&
            AuthPlatformUID.SelectedValue.ToUpper() == AuthorizationPlatforms.TSys);

    }

    private void setACHStatus()
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;

        if (isACHonly)
        {
            if (UserSessions.ActiveAchMerchant != null)
            {
                AchMerchant achmerchant = UserSessions.ActiveAchMerchant;
                LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management", achmerchant.MerchantStatusName.Substring(0, 2));
                ListHandler.ListFindItem(ACHStatusUID, achmerchant.MerchantStatusUID);
            }
            else
                LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management", "SS");

            lblACHStatus.Style["display"] = "block";
            lblCCStatus.Style["display"] = "none";

            ACHStatusUID.Style["display"] = "block";
            StatusUID.Style["display"] = "none";
        }
        else
        {
            lblACHStatus.Style["display"] = "none";
            lblCCStatus.Style["display"] = "block";

            ACHStatusUID.Style["display"] = "none";
            StatusUID.Style["display"] = "block";
        }

    }

    protected void MerchantAppTypeUID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(UserSessions.CurrentMerchantApp != null)
            isACHonly = (MerchantAppTypeUID.SelectedValue.ToUpper() == Constants.BANK_ACH_ONLY && UserSessions.CurrentMerchantApp.AchID > 0);

        setACHStatus();
        OnMerchantAppTypeUIDChanged();
    }

    public void SetAgentSelectorEditMode()
    {
        if (this.MyMerchantApp != null && !this.Adding)
        {
            string m_statusName = this.MyMerchantApp.StatusName.Substring(0, 2).ToUpper();

            if (isACHonly && UserSessions.ActiveAchMerchant != null)
            {
                m_statusName = UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2).ToUpper();
            }

            if (m_statusName == "DP" || m_statusName == "MS")
            {
                if (UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_ADMIN) || UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_ACCOUNTING))
                    wucAgentSelector.SetEditMode(true);
                else
                    wucAgentSelector.SetEditMode(false);
            }

            amexOptBlue.Visible = this.MyMerchantApp.AmexOB;

            Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(this.MyMerchantApp.MerchantAppUID);

            //Start code by Anuj for PXP-9749
            DataTable dt = DataChangeLogs.SearchChangeHistory(new Hashtable { { "@ZID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID) } });
            bool oldMasterMRPValue = false;
            bool oldHRRegistredValue = false;
            bool oldMasterVIRPvalue  = false;
            bool oldVIRPHRRegistredvalue  = false;

            if (dt.Rows.Count > 0)
            {
                var OldMasterMRP = GetOldValueFromREF_ChangeHistoryField(dt, 64);

                var OldHRRegistred = GetOldValueFromREF_ChangeHistoryField(dt, 66);

                oldMasterMRPValue = OldMasterMRP == "Yes" ? true : false;
                oldHRRegistredValue = OldHRRegistred == "Yes" ? true : false;

                var oldMasterVIRP = GetOldValueFromREF_ChangeHistoryField(dt, 137); 
                var oldHRRegisteredVIRP = GetOldValueFromREF_ChangeHistoryField(dt, 138);

                oldMasterVIRPvalue = oldMasterVIRP.Equals("Yes");
                oldVIRPHRRegistredvalue = oldHRRegisteredVIRP.Equals("Yes");
            }    

            if (objUW != null)
            {
                //Code updated for PXP-11841 by koshlendra Start  
                    if (MyMerchantApp.MasterMRP && !oldMasterMRPValue)
                    {
                        PnlMasterMRP.Visible = this.MyMerchantApp.MasterMRP;
                    }
                    else if (!MyMerchantApp.MasterMRP && oldMasterMRPValue)
                    {
                        PnlMasterMRP.Visible = true;
                        IsValMaster.Text = string.Empty;
                    }
                    else
                    {
                        PnlMasterMRP.Visible = this.MyMerchantApp.MasterMRP;
                    }

                    if (MyMerchantApp.HighRiskRegistered && !oldHRRegistredValue)
                    {
                        Registered.Visible = this.MyMerchantApp.HighRiskRegistered;
                    }
                    else if (!MyMerchantApp.HighRiskRegistered && oldHRRegistredValue)
                    {
                        Registered.Visible = true;
                        IsValHRReg.Text = string.Empty;
                    }
                    else
                    {
                        Registered.Visible = this.MyMerchantApp.HighRiskRegistered;
                    }

                if (MyMerchantApp.MasterVIRP && !oldMasterVIRPvalue)
                {
                    PnlMasterVIRP.Visible = this.MyMerchantApp.MasterVIRP;
                }
                else if (!MyMerchantApp.MasterVIRP && oldMasterVIRPvalue)
                {
                    PnlMasterVIRP.Visible = true;
                    IsValMasterVIRP.Text = string.Empty;
                }
                else
                {
                    PnlMasterVIRP.Visible = this.MyMerchantApp.MasterVIRP;
                }

                if (MyMerchantApp.VIRPHighRiskRegistered && !oldVIRPHRRegistredvalue)
                {
                    VIRPRegistered.Visible = this.MyMerchantApp.VIRPHighRiskRegistered;
                }
                else if (!MyMerchantApp.VIRPHighRiskRegistered && oldVIRPHRRegistredvalue)
                {
                    VIRPRegistered.Visible = true;
                    IsVIRPHRReg.Text = string.Empty;
                }
                else
                {
                    VIRPRegistered.Visible = this.MyMerchantApp.VIRPHighRiskRegistered;
                }
                //Code updated for PXP-11841 by koshlendra End  
                //End code by Anuj for PXP-9749                                
            }

            //Start Code added for PXP-17188
            string SalesManager = string.Empty;
            if (m_ChannelSalesManager != null)
            {
                var selectedChannelSalesManager = m_ChannelSalesManager.FirstOrDefault<GenericListItem>(x => x.ItemValue == Convert.ToString(this.MyMerchantApp.ChannelSalesManagerID));
                if (selectedChannelSalesManager != null && !string.IsNullOrEmpty(selectedChannelSalesManager.ItemText))
                {
                    SalesManager = selectedChannelSalesManager.ItemText;
                }
            }
            ChannelSalesManager.Text = SalesManager;
            //End Code added for PXP-17188
        }
        else if (this.Adding)
        {
            wucAgentSelector.SetEditMode(true);

            Registered.Visible = false;
            amexOptBlue.Visible = false;
            PnlMasterMRP.Visible = false;
            ChannelSalesManager.ReadOnly = true;
        }
    }

    protected void StatusUID_SelectedIndexChanged(object sender, EventArgs e)
    {

        string display = "none";
        string dateDisplay = "none";
        string status = string.Empty;

        //check to see if the accoutn is ahconly ,if it is then use the ahc status and shwo the closure code dorpdown for cancellation status
        //ETF dropdown is used only by cc accounts.
        if (isACHonly)
            status = ACHStatusUID.SelectedValue.ToUpper();
        else
            status = StatusUID.SelectedValue.ToUpper();

        //if (status == Constants.QUEUESTATUS_MS_CANCELLATION || status == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION) //Cancelled or Pending Cancellation
        if (status == Constants.QUEUESTATUS_MS_CANCELLATION)  // ***PXP 1261 : remove closur code & ETF validation MS_RETENTION-PENDING-CANCELLATION status
        {
            display = "block";

            //if (status == "DF56BE69-1C2D-4465-BCDB-BD2CE97566C4")//Pending Cancellation 
            //{
            //    dateDisplay = "block";
            //    if (string.IsNullOrWhiteSpace(CancellationDate.Text))
            //        CancellationDate.Value = DateTime.Today.AddDays(180);
            //}
        }

        lblMerchantClosureCodeUID.Style["display"] = display;
        MerchantClosureCodeUID.Style["display"] = display;

        lblRiskStatus.Style["display"] = display;
        RiskStatus.Style["display"] = display;

        if (!isACHonly)
        {
            ETF.Style["display"] = display;
            ETFAssessed.Style["display"] = display;
            lblETF.Style["display"] = display;
        }
        else
        {
            ETF.Style["display"] = "none";
            ETFAssessed.Style["display"] = "none";
            lblETF.Style["display"] = "none";
        }

        CancelDate.Style["display"] = dateDisplay;
        lblCancel.Style["display"] = dateDisplay;
        CancellationDate.Style["display"] = dateDisplay;

    }

    protected void BusinessTaxID_TextChanged(object sender, EventArgs e)
    {
        if (this.TextChange != null)
        {
            TextChange(BusinessTaxID, e);
        }
    }

    protected void AuthPlatformMid_TextChanged(object sender, EventArgs e)
    {
        if (this.TextChange != null)
        {
            TextChange(AuthPlatformMid, e);
        }
    }

    protected void SettlePlatformMid_TextChanged(object sender, EventArgs e)
    {
        if (this.TextChange != null)
        {
            TextChange(SettlePlatformMid, e);
        }
    }

    public void SaveBusinessInfo()
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        MerchantApp app = UserSessions.CurrentMerchantApp;

        if (app != null && (app.StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION || StatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION))
        {
            if (app.ETFAssessed.ToUpper() != "-1")
            {
                ETF objETF = data.GetETFMerchant("", app.MerchantAppUID);

                if (objETF != null)
                {
                    if (app.ETFAssessed.ToUpper() == "W")
                    {
                        objETF.ETFStatusUID = Constants.ETF_ETF_WAIVED;
                        objETF.SuggestedETF = 0.0M;
                        objETF.UserUpdated = UserSessions.CurrentUser.UserName;
                    }
                    else if (app.ETFAssessed.ToUpper() == "A")
                    {
                        objETF.ETFStatusUID = Constants.ETF_OPEN;
                        objETF.SuggestedETF = data.GetSuggestedEtf(Convert.ToInt32(app.ID), 36);
                        objETF.UserUpdated = UserSessions.CurrentUser.UserName;
                    }

                    if (!string.IsNullOrWhiteSpace(objETF.ETFID) || !string.IsNullOrWhiteSpace(objETF.UID) || !string.IsNullOrWhiteSpace(objETF.MerchantAppUID))
                        data.UpdateETFMerchant(objETF);
                }
            }
        }
    }

    public void FormShow(bool iseditmode)
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        MerchantApp app = UserSessions.CurrentMerchantApp;
        string display = "none";
        string visible = "none";

        setACHStatus();

        LoadOffice(app);

        //Added Business DBA phone country code and extentions
        DBACountryCodeDisplay.Text = string.IsNullOrEmpty(app.BusinessDBAPhoneCountryCode) ? "+1" : app.BusinessDBAPhoneCountryCode;
        DBAPhoneExt.Text = app.BusinessDBAPhoneExt.ToString();

        //Added Business Fax country code and extentions
        FaxCountryCodeDisplay.Text = string.IsNullOrEmpty(app.BusinessFaxCountryCode) ? "+1" : app.BusinessFaxCountryCode;
        BusinessFaxExt.Text = app.BusinessFaxExt.ToString();


        if (UserSessions.CurrentMerchantApp.BusinessCountry != null)
        {
            if (UserSessions.CurrentMerchantApp.BusinessCountry.Trim().ToUpper().Equals("US") || UserSessions.CurrentMerchantApp.BusinessCountry.Trim().ToUpper().Equals(""))
            {
                this.BusinessProvince.Attributes.Add("Style", "display:none");
                this.BusinessState.Attributes.Add("Style", "display:inline");
            }
            else
            {
                this.BusinessProvince.Attributes.Add("Style", "display:inline");
                this.BusinessState.Attributes.Add("Style", "display:none");
            }
        }

        if (UserSessions.CurrentMerchantApp.BusinessMailingCountry != null)
        {
            if (UserSessions.CurrentMerchantApp.BusinessMailingCountry.Trim().ToUpper().Equals("US") || UserSessions.CurrentMerchantApp.BusinessMailingCountry.Trim().ToUpper().Equals(""))
            {
                this.BusinessMailingProvince.Attributes.Add("Style", "display:none");
                this.BusinessMailingState.Attributes.Add("Style", "display:inline");

            }

            else
            {
                this.BusinessMailingProvince.Attributes.Add("Style", "display:inline");
                this.BusinessMailingState.Attributes.Add("Style", "display:none");
            }
        }
        
        string m_StatusUID = StatusUID.SelectedValue.ToUpper();

        if (isACHonly)
            m_StatusUID = ACHStatusUID.SelectedValue.ToUpper();

        //if (app != null && (m_StatusUID == Constants.QUEUESTATUS_MS_CANCELLATION || m_StatusUID == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION))
        if (app != null && (m_StatusUID == Constants.QUEUESTATUS_MS_CANCELLATION)) // hide closur code & ETF if status is MS-Retention-PndingCancellation
        {
            if (!isACHonly)
            {
                display = "block";

                ETF objETF = data.GetETFMerchant("", app.MerchantAppUID);

                if (objETF != null)
                {
                    if (iseditmode)
                        ETFAssessed.Enabled = (objETF.ETFStatusUID.ToUpper() == Constants.ETF_ETF_WAIVED || objETF.ETFStatusUID.ToUpper() == Constants.ETF_OPEN);
                }
            }

            //if (m_StatusUID == Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)  // ***MS_PENDING_CANCELLATION status has been replaced by MS_RETENTION_PENDING_CANCELLATION
            //    visible = "block";                                             // Closure code and ETF will not required for MS_RETENTION_PENDING_CANCELLATION status 
        }
        

        ETF.Style["display"] = display;
        lblETF.Style["display"] = display;
        ETFAssessed.Style["display"] = display;

        CancelDate.Style["display"] = visible;
        CancellationDate.Style["display"] = visible;
        lblCancel.Style["display"] = visible;
        
        if (app.FMAID == 0)
            this.FMAID.Text = "";

        this.Brand.Enabled = this.Adding || (iseditmode && m_StatusUID == Constants.QUEUESTATUS_SS_RECEIVED);

        ChannelSalesManager.ReadOnly = true;
    }

    public void SetBusinessInfoEditMode(bool iseditmode)
    {
        FormHandler.SetControlTrueEditMode(pnlGeneralInfo, false);        

        SettlePlatformMid.Enabled = iseditmode;
        SettlePlatformMid.ReadOnly = !iseditmode;
        SettlePlatformUID.Enabled = iseditmode;
        AuthPlatformMid.Enabled = iseditmode;
        AuthPlatformMid.ReadOnly = !iseditmode;
        AuthPlatformUID.Enabled = iseditmode;
        ReservePercent.Enabled = iseditmode;
        ReservePercent.ReadOnly = !iseditmode;
        UpfrontReserve.Enabled = iseditmode;
        UpfrontReserve.ReadOnly = !iseditmode;
        DiscountMethod.Enabled = iseditmode;
        ReleaseMethodUID.Enabled = iseditmode;
        PaymentFrequencyID.Enabled = iseditmode;
        PaymentScheduleID.Enabled = iseditmode;
        BillingMethodUID.Enabled = iseditmode;

        ((Panel)wucAgentSelector.FindControl("pnlAgent")).Enabled = false; 

    }
    //protected void LoadOffice(MerchantApp agreement)
    //{
    //    foreach (Office office in agreement.listOfficeAccess)
    //    {

    //        foreach (ListItem item in listOfficeAccess.Items)
    //        {
    //            if (item.Value == office.OfficeID.ToString())
    //            {
    //                item.Selected = true;
    //            }
    //        }
    //    }
    //}

    public void LoadOffice(MerchantApp agreement)
    {
        if (agreement.listOfficeAccess.Count > 0)    
        {
            int loopcount = 0 ;
            while (loopcount < agreement.listOfficeAccess.Count)
            {
               foreach (ListItem item in listOfficeAccess.Items)
                    {
                        if (item.Value == agreement.listOfficeAccess[loopcount].ToString())
                        {
                            item.Selected = true;
                        }
                    }
               loopcount++;
            }
        }
    }

    protected void OfficeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;

        //LookupTableHandler.LoadOffices(listOfficeAccess);
        if (OfficeID.SelectedValue.Equals("5"))
        {
            DivOfficeAccess.Visible = true;

        }
        else
        {
            DivOfficeAccess.Visible = false; 
        }

        if (OfficeID.SelectedItem.Equals("Irvine (US)") || OfficeID.SelectedValue.Equals("1"))
        {
            string strBusinessDBAPhonetxt = CommonUtility.Util.GetNumbersFromString(BusinessDBAPhone.Value.ToString());
            BusinessDBAPhone.Value = strBusinessDBAPhonetxt.ToString();
            BusinessDBAPhone.InputMask = "000-000-0000";


            string strBusinessFaxtxt = CommonUtility.Util.GetNumbersFromString(BusinessFax.Value.ToString());
            BusinessFax.Value = strBusinessFaxtxt.ToString();
            BusinessFax.InputMask = "000-000-0000";
            

        }
        else
        {
            BusinessDBAPhone.InputMask = "############";
            BusinessFax.InputMask = "############";
            
        }
        OnOfficeIDChanged();
    }

    //Added Code By Anuj for PXP-10756
    protected void MerchantClosureCodeUID_SelectedIndexChanged(object sender, EventArgs e)
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;
       
        DataMerchantApp dataApp = DataAccess.DataMerchantAppDao;
        if (!MerchantClosureCodeUID.SelectedValue.Equals("-1"))
        {
            int eligibilityID = dataApp.GetEligibilityByUID(MerchantClosureCodeUID.SelectedValue);
            LookupTableHandler.LoadMerchantRiskStatus(RiskStatus, false, eligibilityID);
        }
        else
        {
            LookupTableHandler.LoadMerchantRiskStatus(RiskStatus, false, 0);
        }       
    }
    protected void LoadChannelSalesManager()
    {
        Hashtable prms = new Hashtable();

        if (m_ChannelSalesManager == null)
        {
            m_ChannelSalesManager = DataAccess.DataMerchantAppDao.GetChannelSalesManager(prms);
        }
    }
    private string GetOldValueFromREF_ChangeHistoryField(DataTable values, int FieldID)
    {
        string response = string.Empty;

        response = (from lastValue in values.AsEnumerable()
                    where lastValue.Field<int>("ChangeHistoryFieldID") == FieldID
                    orderby lastValue.Field<DateTime>("ChangedDate") descending
                    select lastValue.Field<string>("OldValue")).FirstOrDefault();
        return string.IsNullOrEmpty(response) == true ? string.Empty : response;
    }
}
