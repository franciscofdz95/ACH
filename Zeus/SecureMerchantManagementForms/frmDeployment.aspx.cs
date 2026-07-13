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
using System.Text;
using System.Collections.Generic;

using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebHtmlEditor;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using ZeusWeb.Class;
using Infragistics.Web.UI.LayoutControls;
using Tsys.TsysUtility;
using CommonUtility.Extensions;
using Microsoft.IdentityModel.Tokens;


public partial class frmDeployment : frmBaseDataEntry
{

    bool isACHonly
    {
        get { return CommonUtility.Util.if_b(WucBusinessInfo1.isACHonly, false); }
    }

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    public string MerchantAppZID
    {
        get { return ViewState["MerchantAppZID"].ToString(); }
        set { ViewState["MerchantAppZID"] = value; }
    }

    public string Email
    {
        get { return ViewState["Email"].ToString(); }
        set { ViewState["Email"] = value; }
    }
    //Added for PXP-16364 
    private DataTable dtSalesRiskEvent = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ReceiptsPanel.Visible = false; //hiding the receipts panel


        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Deployment);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Deployment");
            }

            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

            FillDropdowns();

            WucEquipment.LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);
            FormShow("");
            WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621

            //Ani: DM-5755
            this.LoadVendors();
        }


    }

    private void FillDropdowns()
    {
        LookupTableHandler.LoadGatewayMode(GatewayModeUID, false);
        LookupTableHandler.LoadPrivateLabels(PrivateLabelUID, false);

        LookupTableHandler.LoadHour(SettlementSchedule);
        SettlementSchedule.Items.Add(new ListItem("Manual", "Manual"));

        LookupTableHandler.LoadNMIVendors(CRMID, false); //DM-2403 VendorUID ZEUS Emanuel

        LookupTableHandler.LoadNmiAffiliatePlanIdList(AffiliatePlanId, false);

        /*drodpowns for Merhcnat boarding fields*/
        //LookupTableHandler.LoadMerchantETCTypes(ETCTypeUID, false);
        //LookupTableHandler.LoadDepositType(DespositTypeID, false);
        //LookupTableHandler.LoadSecurityProtocol(SecurityProtocolID, false);
        //LookupTableHandler.LoadTerminalActivationType(TerminalActivationTypeID, false);
        //LookupTableHandler.LoadAuthPOSType(AuthPOSTypeID, false);
        //LookupTableHandler.LoadMagneticStripeType(MagneticStripeTypeID, false);
        //LookupTableHandler.LoadPOSDeviceType(POSDeviceTypeID, false);
        //LookupTableHandler.LoadPOSCapabilityType(POSCapabilityTypeID, false);
        //LookupTableHandler.LoadMerchantTypeCode(MerchantTypeCode, false);
        //LookupTableHandler.LoadAMEXPCIDType(AMEXPCIDTypeCode, false);
    }

    //PXP-14480 by Satyajit
    private void dsVMandBT(DataSet dt, ref bool refDGDisabled, ref bool refFTEnabled)
    {
        System.Web.UI.WebControls.ListItem item = null;

        if (dt != null && dt.Tables.Count > 0 && dt.Tables[0] != null)
        {
            DataRow[] drVerticalMarket = dt.Tables[0].Select("VerticalMarketTypeID=" + 1);

            foreach (DataRow dr in drVerticalMarket)
            {
                item = new System.Web.UI.WebControls.ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[4].ToString());
                if (item.Text == "DG" && item.Value == "0")
                {
                    refDGDisabled = true;
                    break;
                }
            }
            drVerticalMarket = null;
            item = null;

            DataRow[] drBillingType = dt.Tables[0].Select("VerticalMarketTypeID=" + 2);

            foreach (DataRow dr in drBillingType)
            {
                item = new System.Web.UI.WebControls.ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[4].ToString());
                if (item.Text == "FT" && item.Value == "1")
                {
                    refFTEnabled = true;
                    break;
                }
            }
            drBillingType = null;
            item = null;
        }
    }

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
        //PXP-14480 by Satyajit
        if (agreement.IsPCCSSwitch != null)
            chkPCCSSwitch.Checked = agreement.IsPCCSSwitch;

        AchMerchant achmerchant = null;
        string m_StatusUID = string.Empty, m_StatusName = string.Empty;

        if (isACHonly)
        {
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));

            if (UserSessions.ActiveAchMerchant != null)
            {
                achmerchant = UserSessions.ActiveAchMerchant;
                LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management", agreement, achmerchant);
                ListHandler.ListFindItem(ACHStatusUID, achmerchant.MerchantStatusUID);
                m_StatusUID = achmerchant.MerchantStatusUID;
                m_StatusName = achmerchant.MerchantStatusName;
            }

            ACHStatus.Visible = true;
            CCStatus.Visible = false;
            lblStatus.Text = "Application ACH Status";
        }
        else
        {
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management", agreement.StatusName.Substring(0, 2), agreement);
            ListHandler.ListFindItem(StatusUID, agreement.StatusUID);
            m_StatusUID = agreement.StatusUID;
            m_StatusName = agreement.StatusName;
            ACHStatus.Visible = false;
            CCStatus.Visible = true;
            lblStatus.Text = "Application Status";
        }

        // Added by Chandra for PXP-7898
        hidStatus.Value = m_StatusUID.ToUpper();
        IsNutra.Value = (!agreement.IsNutraMerchant && agreement.SicCode == "5968").ToString().ToUpper();


        //PXP-9348 RThakur >> Start

        hiddenCrmStatus.Value = agreement.CRMStatus;
        hiddenAcceptTransaction.Value = Convert.ToString(agreement.CRMAcceptTransactions);
        hidIsNutraMerchant.Value = (agreement.IsNutraMerchant && agreement.SicCode == "5968").ToString().ToUpper();
        //PXP-9348 RThakur >> End

        //START:PXP-11487 Add new section Welcome email By  Alik
        if (agreement.AdditionalRecipients == null)
            agreement.AdditionalRecipients = agreement.AgentEmail.ToString();
        //END:PXP-11487 Add new section Welcome email By  Alik

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);
        WucBusinessInfo1.pnlInfo.Enabled = false;
        FormBinding.BindObjectToControls(agreement, pnlMerchantAdditionalInfo);
        FormBinding.BindObjectToControls(agreement, pnlMerchant);

        if (agreement.DeploymentNotes != null)
            this.DeploymentNotes.Text = System.Web.HttpUtility.HtmlDecode(agreement.DeploymentNotes.Trim());

        this.ShowMerchantAppTransDB();

        // DM-7160 AhmerBashir
        this.ShowPayHQ(agreement);

        this.MerchantAppZID = agreement.ID;
        this.Email = agreement.BusinessEmailAddress;

        WucServices1.LoadServices(UserSessions.CurrentMerchantApp.MerchantAppUID, ServiceCategories.DEPLOYMENT, 2);

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        //PXP-14528 - bug fix for PXP-14480
        DataSet dt = new DataSet();
        Hashtable prms = new Hashtable();
        bool isDGDisabled = false;
        bool isFTEnabled = false;

        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        dt = DataAccess.DataMerchantAppDao.GetVerticalMarkets(prms);

        dsVMandBT(dt, ref isDGDisabled, ref isFTEnabled);
        //PXP-14545 - bug fix for PXP-14480
        //code changes doe for PXP-16317 start
        if (agreement != null && (agreement.IsPCCSSwitch != null && !agreement.IsPCCSSwitch) && this.EditMode)
        {
            chkPCCSSwitch.Enabled = true;
        }
        else
        {
            //Code added for DM-3498
            if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_OPERATIONS && this.EditMode)
                chkPCCSSwitch.Enabled = true;
            else
                chkPCCSSwitch.Enabled = false;
            //End code added for DM-3498
        }
        //code changes doe for PXP-16317 end
        ((Button)WucEquipment.FindControl("btnLookup")).Enabled = this.Adding;

        if (EditMode)
        {
            pnlTerminals.Enabled = WucEquipment.grdCount > 0;
            FormHandler.SetControlEditMode(pnlTerminals, WucEquipment.grdCount > 0);
        }

        Subscription cbmsPlus = DataProduct.GetMerchantCurrentProductSubscription(int.Parse(agreement.ID), "CB_Refund");
        wucCBMS.Visible = cbmsPlus == null ? false : cbmsPlus.IsActive;

        if (wucCBMS.Visible)
        {
            wucCBMS.ShowCBMSConfiguration(int.Parse(agreement.ID));
        }

        WucPlatformCardCurrency.SetDataSource(agreement);

        MasterPageMerchant master = (MasterPageMerchant)this.Master;
        master.ShowNotes(agreement.UWNotes, agreement.AgentMemo, agreement.FirstTeamNotes, this.EditMode);

        if (agreement.AuthPlatformUID.ToUpper() == AuthorizationPlatforms.Compass
            || agreement.SettlePlatformUID.ToUpper() == AuthorizationPlatforms.Compass)
        {
            wucCompassParameters.Visible = true;
            wucCompassParameters.ShowCompassParameters();
        }


        //  MerchantApp app = UserSessions.CurrentMerchantApp;
        WucBusinessInfo1.LoadOffice(agreement);



        //this checkbox is added to configure the Transaction amount field on Hosted payment page        
        List<GatewayPageItem> lstGatewayItem = DataGatewayPage.GetGatewayPageItem(int.Parse(agreement.ID), eGatewayPage.ZeusDeploymentPage, eGatewayPageFieldCategory.EditableHostedPageFields);

        chkEditableAmount.Checked = false;

        if (lstGatewayItem != null && lstGatewayItem.Count > 0)
        {
            chkEditableAmount.Checked = CommonUtility.Util.if_b(lstGatewayItem[0].DefaultValue, false);
        }

        //    MerchantBoarding objBoarding = new MerchantBoarding();
        //    DataMerchantBoarding data1 = new DataMerchantBoarding();

        //    objBoarding = data1.GetMerchantBoarding(UserSessions.CurrentMerchantApp.ID);

        //    if(objBoarding != null){
        //        ListHandler.ListFindItem(DespositTypeID, objBoarding.DepositTypeID.ToString());
        //        ListHandler.ListFindItem(AuthPOSTypeID, objBoarding.AuthPOSTypeCode);
        //        FormBinding.BindObjectToControls(objBoarding, pnlBoarding);  
        //}

        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message     
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }
        /******** End of PXP-2206 **************/

        //Added by Chandra for PXP-7450
        //Ani: DM-5930
        pnlNutra.Visible = true;
        //START:Added by Abarua for PXP-7866
        //Ani: DM-5753
        PanelNMI.Visible = true;
        //Ani: DM-5773
        FormBinding.BindObjectToControls(agreement, PanelNMI);
        // DM-5800 by Jorge 
        if (UserSessions.CurrentMerchantApp?.VerticalMarket_BillingTypes == null)
        {
            UserSessions.CurrentMerchantApp.VerticalMarket_BillingTypes = BindVerticalMarketBillingData();
        }
        var nmiPlatform = NMIMerchantOnboardingAPI.GetPlatform(UserSessions.CurrentMerchantApp);
        btnNMILockDownAPI.Enabled = nmiPlatform != Platform.NoPlatform && this.EditMode;
        LockDownPlanID.Text = nmiPlatform == Platform.FirstDataNashville ? "610436" : "380909";
        // END DM-5800
        LockdownDate.Text = (agreement.LockdownDate.Equals(DateTime.MinValue) ? "" : agreement.LockdownDate.ToShortDateString());
        AffiliateDate.Text = (agreement.AffiliateDate.Equals(DateTime.MinValue) ? "" : agreement.AffiliateDate.ToShortDateString());
        //PXP-11314 disable btn NMI abarua
        btnNMIAffiliateAPI.Enabled = !string.IsNullOrEmpty(agreement.LockdownNMIUsername) && this.EditMode;

        //END:Added by Abarua for PXP-7866
        Compliance CurrentCRM = WebHostFacade.CRMList.Where(crm => crm.CRMID == UserSessions.CurrentMerchantApp.CRMID).FirstOrDefault(); //DM-2403 VendorUID ZEUS Emanuel
        //PXP-16364:Code Changes:Start
        DataTable dtAllowRisk = GetAllowRiskEvent(UserSessions.CurrentMerchantApp.ID);
        bool SavedAllowSalesValue = false;
        string ToolTipRiskEvntMsg = null;
        if (dtAllowRisk != null)
        {
            if (dtAllowRisk != null && dtAllowRisk.Rows.Count > 0)
            {
                SavedAllowSalesValue = CommonUtility.Util.if_b(dtSalesRiskEvent.Rows[0]["NewValue"], false);
                ToolTipRiskEvntMsg = string.Format("Allow Sales is disabled by {0} on {1} ", dtSalesRiskEvent.Rows[0]["UpdatedBy"], dtSalesRiskEvent.Rows[0]["UpdatedDate"]);
            }
        }
        if (!EditMode)
        {
            hidOldCRMID.Value = agreement.CRMID.ToString();//DM-2403 VendorUID ZEUS Emanuel
        }
        //PXP-16364:Code Changes:End
        if (CurrentCRM != null)
        {
            if (!CurrentCRM.AcceptTransactions && UserSessions.CurrentMerchantApp.IsNutraMerchant)
            {
                this.AllowSales.Enabled = false;
                //PXP-16364:Code Changes:Start
                String tooltip = "Allow Sales disabled as the CRM the merchant is using is not approved to accept Sales";
                this.AllowSales.ToolTip = tooltip;
                //PXP-16364:Code Changes:End
            }
            else if (!SavedAllowSalesValue && UserSessions.CurrentMerchantApp.IsNutraMerchant)
            {
                //PXP-16364:Code Changes:Start
                this.AllowSales.Enabled = false;
                this.AllowSales.ToolTip = ToolTipRiskEvntMsg;
                //PXP-16364:Code Changes:End
            }

            //PXP-8982 By Sanidhya
            UserFacade userFacade = new UserFacade();
            var userRoles = userFacade.GetUser(UserSessions.CurrentUser.UID).UserRoles.Where(u => u.Value.Enabled == true);  // Dynamic list of enabled user roles;      
            if (this.EditMode && UserSessions.CurrentMerchantApp.IsNutraMerchant && CurrentCRM.AcceptTransactions && (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_APPLICATION_BOARDING || UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_RISK || (userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_SPECIALACCESS)))))
            {
                this.AllowSales.Enabled = true;
            }
            if (this.EditMode && UserSessions.CurrentMerchantApp.IsNutraMerchant && CurrentCRM.AcceptTransactions && !(UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_APPLICATION_BOARDING || UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_RISK || (userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_SPECIALACCESS)))))
            {
                this.AllowSales.Enabled = false;
            }
        }
        //PXP-9051 RThakur
        if (!FormHandler.SetHdnValueIsAnyVerticalMarketsChecked(m_StatusName, m_StatusUID, agreement.IsNewVertical))
        {
            IsNewVerticalandMarkets.Value = "false";
            IsNewVertical.Value = agreement.IsNewVertical ? "True" : "False";
        }
        else
        {
            IsNewVerticalandMarkets.Value = "true";
            IsNewVertical.Value = agreement.IsNewVertical ? "True" : "False";
        }

        chkIsCOFEnabled.Checked = agreement.IsCOFEnabled;
        chkIsOnlineRefundEnabled.Checked = agreement.IsOnlineRefundEnabled;
        chkIsPartialAuthEnabled.Checked = agreement.IsPartialAuthEnabled;
        chkIsCOFEnabled.Enabled = false;
        chkIsOnlineRefundEnabled.Enabled = false;
        chkIsPartialAuthEnabled.Enabled = false;
    }

    public override void FormClear()
    {
        WucEquipment.FormClear();
    }
    //PXP-16364:Code Changes:Start
    private DataTable GetAllowRiskEvent(string ZID)
    {
        MerchantFacade facadeRiskEvent = new MerchantFacade();
        dtSalesRiskEvent = facadeRiskEvent.GetAllowSalesRiskEvent(new Hashtable { { "@ZID", Convert.ToInt32(ZID) } });
        if (dtSalesRiskEvent != null && dtSalesRiskEvent.Rows.Count > 0)
            return dtSalesRiskEvent;
        else
            return null;
    }
    //PXP-16364:Code Changes:End

    public override bool FormSave()
    {
        DataSet dsmultiLink = new DataSet();

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        MerchantFacade merchantFacade = new MerchantFacade();
        //save merchant
        MerchantApp agreement = null;

        agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
        agreement.CloneMerchantApp();
        //for session management of CRM Status,Count
        //PXP-8409 by Sanidhya
        if (!this.Adding)
        {

            MerchantApp currntMerchantApp = merchantFacade.GetMerchantAppZeus(agreement.MerchantAppUID);
            agreement.CRMStatus = currntMerchantApp.CRMStatus;
            agreement.CRMCount = currntMerchantApp.CRMCount;
            agreement.CRMAcceptTransactions = currntMerchantApp.CRMAcceptTransactions;
        }
        FormBinding.BindControlsToObject(agreement, pnlMerchantAdditionalInfo);
        FormBinding.BindControlsToObject(agreement, pnlMerchant);

        if (agreement.IsNutraMerchant)
        {
            Compliance UpdatedCRM = WebHostFacade.CRMList.Where(crm => crm.CRMID == agreement.CRMID).FirstOrDefault();//DM-2403 VendorUID ZEUS Emanuel
            //PXP-16364:Code Changes:Start

            bool IsCRMUpdating = (UpdatedCRM != null && hidOldCRMID.Value.ToString() != UpdatedCRM.CRMID.ToString()) ? true : false;//DM-2403 VendorUID ZEUS Emanuel
            bool IsAllowSalesChanged = (agreement.MerchantAppClone.AllowSales != AllowSales.Checked) ? true : false;

            if (IsCRMUpdating == false && IsAllowSalesChanged == true)
            {
                Hashtable prmsRiskEvent = new Hashtable();
                prmsRiskEvent.Add("@ZID", UserSessions.CurrentMerchantApp.ID);
                prmsRiskEvent.Add("@OldValue", agreement.MerchantAppClone.AllowSales);
                prmsRiskEvent.Add("@NewValue", AllowSales.Checked ? true : false);
                prmsRiskEvent.Add("@UpdatedBy", UserSessions.CurrentUser.UserName);

                merchantFacade.InsertAllowSalesRiskEvent(prmsRiskEvent);
            }
            //PXP-16364:Code Changes:End
            if (UpdatedCRM != null && IsCRMUpdating == false)
            {
                /*Old-Condition*/
                // agreement.AllowSales = UpdatedCRM.AcceptTransactions;
                //PXP-8982 by Sanidhya  
                agreement.AllowSales = AllowSales.Checked;
            }
            else
            {
                //PXP-16364:Code Changes:Start
                DataTable dtRiskEvent = GetAllowRiskEvent(UserSessions.CurrentMerchantApp.ID);
                if (dtRiskEvent != null && dtRiskEvent.Rows.Count > 0)
                {
                    agreement.AllowSales = CommonUtility.Util.if_b(dtSalesRiskEvent.Rows[0]["NewValue"], false);
                }
                //PXP-16364:Code Changes:End
            }
        }
        // BEGIN - DM-3437
        bool _hasPermissionToUpdate = merchantFacade.HasSpecificRole(Constants.ROLE_SALES_SUPPORT, UserSessions.CurrentUser.UserRoles);

        if (_hasPermissionToUpdate && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.SettlePlatformMid))
        {
            bool _status = false;
            try
            {
                _status = ((CheckBoxList)WucServices1.FindControl("chkServices")).Items.FindByValue("checkXcaliberMID").Selected;
            }
            catch
            {
                _status = false;
            }
            Hashtable _obj = new Hashtable();
            _obj.Add("@MerchantMID", UserSessions.CurrentMerchantApp.SettlePlatformMid);
            _obj.Add("@Status", _status);
            _obj.Add("@UserModified", UserSessions.CurrentUser.UserName);
            merchantFacade.UpdateCBXcaliberMID_ByUser(_obj);
        }
        //END -  DM-3437

        if (string.Equals(agreement.PrivateLabelUID, "-1"))
        {
            agreement.PrivateLabelUID = string.Empty;
        }

        agreement.DeploymentNotes = Server.HtmlEncode(DeploymentNotes.Text.Trim());

        UserSessions.CurrentMerchantApp = agreement;
        string m_StatusUID = string.Empty;

        AchMerchant achMerchant = null;

        //if account is achonly and activemerchant is not null then 
        //get the ach status or else get cc status
        if (UserSessions.ActiveAchMerchant != null && isACHonly)
        {
            UserSessions.ActiveAchMerchant.CloneAchMerchant();
            achMerchant = UserSessions.ActiveAchMerchant;
            achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;
            achMerchant.MerchantStatusUID = ACHStatusUID.SelectedValue;
            m_StatusUID = achMerchant.MerchantStatusUID;
        }
        else
        {
            m_StatusUID = agreement.StatusUID;
        }

        if (!this.FormDataCheck())
            return false;

        User user = UserSessions.CurrentUser;
        agreement.UserUpdated = user.UserName;
        agreement.DateCreated = DateTime.Now;

        bool isWelcomeKitSent = ((CheckBoxList)WucServices1.FindControl("chkServices")).Items.FindByValue("99c5f209-cb51-4088-adc3-a6413c4fb1d9").Selected;
        bool isQA = ((CheckBoxList)WucServices1.FindControl("chkServices")).Items.FindByValue("99c5f209-cb51-4088-adc3-a6413c4fb1d0").Selected;

        if (isWelcomeKitSent && isQA)
        {
            if (m_StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE)
                m_StatusUID = Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_SOFTWARE;
            else if (m_StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_HARDWARE)
                //DM-507 -Chandra
                m_StatusUID = Constants.QUEUESTATUS_DP_REVIEW; //Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_HARDWARE;
        }

        agreement.UWNotes = ((TextBox)this.Master.FindControl("UWNotesEdit")).Text;
        agreement.FirstTeamNotes = ((TextBox)this.Master.FindControl("FirstTeamNotesEdit")).Text;

        if (!isACHonly)
            agreement.StatusUID = m_StatusUID;


        string StatusUIDClone = string.Empty;

        // get the clone value for status before update which 
        //will be used when sendign out alert notifications 
        if (!isACHonly)
        {
            if (agreement.MerchantAppClone != null)
                StatusUIDClone = agreement.MerchantAppClone.StatusUID;
        }
        else
        {
            if (achMerchant != null && achMerchant.AchMerchantClone != null)
                StatusUIDClone = achMerchant.AchMerchantClone.MerchantStatusUID;
        }


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

        if (underWriting != null)
        {
            //Start code by Anuj for PXP-9311 
            if (agreement.IsNutraMerchant)
            {
                //Code changes done for PXP-15968 with decode the string by koshlendra start
                if (!(System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Nutra Trial' account") || System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Tangible Trial' account")))
                {
                    StringBuilder uwIssue = new StringBuilder();
                    uwIssue.AppendLine(System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues));
                    uwIssue.AppendLine(" ");
                    // PXP-12436: Start - Rohit Thakur
                    if (agreement.Office == CommonUtility.Util.Offices.Irvine)
                    {
                        uwIssue.AppendLine("'Tangible Trial' account");
                    }
                    else
                    {
                        uwIssue.AppendLine("'Nutra Trial' account");
                    }
                    // PXP-12436: End - Rohit Thakur
                    underWriting.UWIssues = uwIssue.ToString();
                }
                // PXP-12436: Start - Rohit Thakur
                else if (System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Nutra Trial' account"))
                {
                    if (agreement.Office == CommonUtility.Util.Offices.Irvine)
                    {
                        underWriting.UWIssues = System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Replace("'Nutra Trial' account", "'Tangible Trial' account");
                    }
                }// PXP-12436: End - Rohit Thakur
            }
            else
            {
                if (System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Nutra Trial' account"))
                {
                    // PXP-12436: Start - Rohit Thakur
                    underWriting.UWIssues = System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Replace("'Nutra Trial' account", "");
                    // PXP-12436: End - Rohit Thakur
                }
                // PXP-12436: Start - Rohit Thakur
                else if (System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Tangible Trial' account"))
                {
                    underWriting.UWIssues = System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Replace("'Tangible Trial' account", "");
                }
                //Code changes done for PXP-15968 with decode the string by koshlendra start
                // PXP-12436: End - Rohit Thakur

            }
            bool moveStatus = (StatusUIDClone.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED.ToUpper());
            if (agreement.MasterMRP)
            {
                if (moveStatus)
                {
                    StringBuilder opsData = new StringBuilder();
                    opsData.AppendLine(underWriting.UWIssues);

                    if (!underWriting.UWIssues.Contains("'Master MRP' account"))
                    {
                        opsData.AppendLine("'Master MRP' account");
                        underWriting.UWIssues = opsData.ToString();
                    }
                    else
                    {
                        opsData.Replace("'Master MRP' account", string.Empty);
                        opsData.AppendLine("'Master MRP' account");
                        underWriting.UWIssues = opsData.ToString();
                    }
                }
            }
            else
            {
                if (underWriting.UWIssues.Contains("'Master MRP' account"))
                {
                    underWriting.UWIssues = underWriting.UWIssues.Replace("'Master MRP' account", "");
                }
            }
            if (underWriting.HighRiskRegistered)
            {
                if (moveStatus)
                {
                    StringBuilder opsInstructionsData = new StringBuilder();
                    opsInstructionsData.AppendLine(underWriting.UWIssues);

                    if (!underWriting.UWIssues.Contains("'High Risk Registered' account"))
                    {
                        opsInstructionsData.AppendLine("'High Risk Registered' account");
                        underWriting.UWIssues = opsInstructionsData.ToString();
                    }
                    else
                    {
                        opsInstructionsData.Replace("'High Risk Registered' account", "");
                        opsInstructionsData.AppendLine("'High Risk Registered' account");
                        underWriting.UWIssues = opsInstructionsData.ToString();
                    }
                }
            }
            else
            {
                if (underWriting.UWIssues.Contains("'High Risk Registered' account"))
                {
                    underWriting.UWIssues = underWriting.UWIssues.Replace("'High Risk Registered' account", "");
                }
            }

            underWriting.UWIssues = RemoveExtraSpace(underWriting.UWIssues.ToString());
            DataAccess.DataUnderwritingDao.UpdateMerchantUWNotes(underWriting, agreement.ID, UserSessions.CurrentUser.UserName);
        }
        //End code by Anuj for PXP-9311

        //PXP-11452 By sanidhya
        agreement = FormHandler.ManageDP_SoftwareStatus(agreement);

        if (agreement.IsNutraMerchant && agreement.Office == CommonUtility.Util.Offices.Irvine && m_StatusUID.ToUpper() != StatusUIDClone.ToUpper() && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE)
        {
            FormHandler.AllowPxpForNutra();
        }

        //PXP-14480 by Satyajit
        DataSet dt = new DataSet();
        Hashtable prms = new Hashtable();
        bool isDGDisabled = false;
        bool isFTEnabled = false;

        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        dt = DataAccess.DataMerchantAppDao.GetVerticalMarkets(prms);

        dsVMandBT(dt, ref isDGDisabled, ref isFTEnabled);

        // DM-7160 --Ahmer Bashir    
        agreement.PHQUsername = txtPHQUserName.Text;
        agreement.PHQPassword = txtPHQPass.Text;
        agreement.PHQDeviceId = txtPHQDeviceID.Text;

        //PXP-14480 by Satyajit
        //code changes doe for PXP-16317 start
        if (agreement != null)
        {
            agreement.IsPCCSSwitch = chkPCCSSwitch.Checked;
        }
        //code changes doe for PXP-16317 end
        MerchantFacade facade = new MerchantFacade();
        int rows = facade.UpdateMerchantAppPage(agreement, 4);

        if (rows > 0)
        {
            this.UpdateMerchantAppTransDB();

            WucServices1.UpdateServices(UserSessions.CurrentMerchantApp.MerchantAppUID, "3");
            WucPlatformCardCurrency.SaveCardCurrency(agreement);

            if (this.wucCompassParameters.Visible)
            {
                this.wucCompassParameters.Save();
            }

            //saving ethoca and verifi IDs
            if (this.wucCBMS.Visible)
            {
                this.wucCBMS.Save(int.Parse(agreement.ID));
            }

            //save merchant boarding data
            //DataMerchantBoarding dataM = new DataMerchantBoarding();
            //MerchantBoarding item = new MerchantBoarding();
            //item.AccountFixedFeeTypeID = UserSessions.CurrentMerchantApp.PricingTypeID;
            //item.AuthPOSTypeCode = AuthPOSTypeID.SelectedValue;
            //item.DateCreated = DateTime.Now;
            //item.UserCreated = UserSessions.CurrentUser.UserName;
            //item.DepositTypeID = CommonUtility.Util.if_i(DespositTypeID.SelectedValue, -1);
            //item.TerminalActivationTypeID = CommonUtility.Util.if_i(TerminalActivationTypeID.SelectedValue, -1);
            //item.SecurityProtocolID = CommonUtility.Util.if_i(SecurityProtocolID.SelectedValue, -1);
            //item.DuesAssessmentTypeID = UserSessions.CurrentMerchantApp.PricingTypeID;
            //item.StatementDebitPrintTypeID = UserSessions.CurrentMerchantApp.PricingTypeID;
            //item.MagneticStripeTypeID = CommonUtility.Util.if_i(MagneticStripeTypeID.SelectedValue, -1);
            //item.POSDeviceTypeID = CommonUtility.Util.if_i(POSDeviceTypeID.SelectedValue, -1);
            //item.POSCapabilityTypeID = CommonUtility.Util.if_i(POSCapabilityTypeID.SelectedValue, -1);
            //item.InterchangeStatementFeeTypeID = UserSessions.CurrentMerchantApp.PricingTypeID;
            //item.MerchantTypeCode = MerchantTypeCode.SelectedValue;
            //item.MerchantChainCode = MerchantChainCode.Text;
            //item.AMEXPCIDTypeCode = AMEXPCIDTypeCode.SelectedValue;


            //dataM.InsertMerchantBoarding(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), item);

            FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);

            UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(agreement.MerchantAppUID);
        }

        if (agreement.AchID > 0)
        {
            DataAchMerchant data1 = new DataAchMerchant();

            if (achMerchant == null)
            {
                achMerchant = data1.GetAchMerchant(Int32.Parse(agreement.ID));
            }

            achMerchant.AllowBlindCredits = agreement.AllowBlindCredits;

            if (isACHonly)
                achMerchant.MerchantStatusUID = m_StatusUID;

            achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;

            data1.UpdateAchMerchant(achMerchant);
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));

        }

        bool perform = true;

        //save equipment
        if (pnlTerminals.Enabled)
        {
            perform = WucEquipment.FormSave(UserSessions.CurrentMerchantApp.MerchantAppUID);
            if (perform)
            {
                ToggleButtons();
                FormHandler.SetControlEditMode(WucEquipment, false);
            }
        }

        //store the checkbox "Editable Trans Amount" vlaue to the gateway settings table. 
        string mystr = string.Format("{0}^{1}^{2}^{3}^{4}", (int)eGatewayPageFieldMapping.Zeus_EditableTransAmount, "0", "1", "", chkEditableAmount.Checked ? "1" : "0");

        DataGatewayPage.ManageGatewayPageSetting(Convert.ToInt32(agreement.ID), mystr);


        FormHandler.CompleteApplication(agreement, achMerchant, StatusUIDClone, user.UserName);
        //PXP-9750 Rohit Thakur >> Start
        //Create new ticket for ‘Mastercard De-registration process
        if (!isACHonly && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION && agreement.HighRiskRegistered && m_StatusUID.ToUpper() != StatusUIDClone.ToUpper())
        {
            FormHandler.AddTicketForMastercard(agreement, "i", "3", "2251", "2252", "3-Low", "6");
        }
        //PXP-9750 Rohit Thakur >> End

        //Ani: 5755
        facade.UpdateVendorMerchants(VendorList.SelectedIndex, UserSessions.CurrentMerchantApp.ID);

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

    public bool ShowMerchantAppTransDB()
    {
        try
        {
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            MerchantApp app = DataAccess.DataMerchantAppDao.GetMerchantApp(prms);
            FormBinding.BindObjectToControls(app, pnlTSYS);

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    // region DM-7160 --Ahmer Bashir
    public bool ShowPayHQ(MerchantApp agreement)
    {
        try
        {
            this.txtPHQUserName.Text = agreement.PHQUsername;            
            this.txtPHQDeviceID.Text = agreement.PHQDeviceId;

            this.txtPHQPass.TextMode = TextBoxMode.Password;
            this.txtPHQPass.Text = agreement.PHQPassword;
            this.txtPHQPass.Attributes["value"] = agreement.PHQPassword;

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }
    // region END DM-7160 --Ahmer Bashir

    private bool UpdateMerchantAppTransDB()
    {
        DataRisk data = DataAccess.DataRiskDao;

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);

        prms.Add("@Tsys_MerchantNumber", Tsys_MerchantNumber.Text);
        prms.Add("@Tsys_AcquirerBin", Tsys_AcquirerBin.Text);
        prms.Add("@Tsys_StoreNumber", Tsys_StoreNumber.Text);
        prms.Add("@Tsys_LocationNumber", Tsys_LocationNumber.Text);
        prms.Add("@Tsys_TerminalNumber", Tsys_TerminalNumber.Text);
        prms.Add("@Tsys_AgentBank", Tsys_AgentBank.Text);
        prms.Add("@Tsys_AgentChain", Tsys_AgentChain.Text);
        /// <summary>
        /// AhmerBashir 16-03-2026
        /// DM-7315
        /// </summary>
        prms.Add("@Tsys_TID", Tsys_TID.Text);
        prms.Add("@GatewayAllowPayPal", GatewayAllowPayPal.Checked);

        data.UpdateMerchantAppTransDB(prms);

        return true;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        FormHandler.SetControlEditMode(pnlTerminals, this.EditMode);
        this.ToggleButtons();
        WucEquipment.LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);
        WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        MerchantApp agreement = UserSessions.CurrentMerchantApp;

        //Fmassoud 2017.08.28 Sending New Status to Formhandler       
        string NewStatus = isACHonly ? ACHStatusUID.SelectedValue.ToUpper() : StatusUID.SelectedValue.ToUpper();
        IList<string> message = FormHandler.MerchantDataCheck(agreement, false, false, NewStatus, UserSessions.ActiveAchMerchant);
        //PXP-8409 Sanidhya:Start
        IList<string> _infoMsg = FormHandler.ValidateCRMFlow(agreement, false, UserSessions.ActiveAchMerchant);
        if (_infoMsg.Count > 0)
        {
            foreach (string msg in _infoMsg)
            {
                this.Master.AddMessageStatus(msg);
            }
        }
        //PXP-8409 Sanidhya:End
        if (message.Count > 0)
        {
            foreach (string mess in message)
                this.Master.AddMessageError(mess);
        }

        //Niranjan: PXP-8400
        DropDownList TerminalStatusUID = (DropDownList)WucEquipment.FindControl("TerminalStatusUID");
        HiddenField hFieldItemUID = (HiddenField)WucEquipment.FindControl("ItemUID");
        //Fix for PXP-8396
        if (hFieldItemUID != null && !string.IsNullOrEmpty(hFieldItemUID.Value) && TerminalStatusUID.SelectedValue != "-1")
        {
            DropDownList DeployTypeID = (DropDownList)WucEquipment.FindControl("DeployTypeID");
            if (DeployTypeID != null && DeployTypeID.SelectedIndex == 0)
                this.Master.AddMessageError("Please select a deploy type.");
        }

        // start DM-2279 by Jorge
        var formDataIsValid = WucEquipment.FormDataCheck();
        if (pnlTerminals.Enabled
            && !formDataIsValid
            && WucEquipment != null
            && WucEquipment.grdCount <= 0)
        {
            this.Master.AddMessageError("Enter and review the Equipment Information.");
        }
        // end DM-2279 by Jorge

        if (UserSessions.CurrentMerchantApp.AuthPlatformUID.ToUpper() == AuthorizationPlatforms.TSys)
        {
            /// <summary>
            /// AhmerBashir 16-03-2026
            /// DM-7315
            /// </summary>
            bool isMakerPaymentXPExists = FindMakerPaymentXP(UserSessions.CurrentMerchantApp.MerchantAppUID);

            StringBuilder sb = new StringBuilder();
            sb.Append(Tsys_MerchantNumber.Text.Trim());
            sb.Append(Tsys_AcquirerBin.Text.Trim());            
            sb.Append(Tsys_LocationNumber.Text.Trim());            
            sb.Append(Tsys_AgentBank.Text.Trim());
            sb.Append(Tsys_AgentChain.Text.Trim());    
            
            /// <summary>
            /// AhmerBashir 16-03-2026
            /// Validate these ONLY if Maker Payment XP exists
            /// DM-7315
            /// </summary>
            if (isMakerPaymentXPExists)
            {
                if (Tsys_StoreNumber.Text.Length != 4 || !CommonUtility.Util.IsValidInt32(Tsys_StoreNumber.Text))
                    this.Master.AddMessageError("Invalid TSYS Store Number.");

                if (Tsys_TerminalNumber.Text.Length != 4 || !CommonUtility.Util.IsValidInt32(Tsys_TerminalNumber.Text))
                    this.Master.AddMessageError("Invalid TSYS Terminal Number.");

                if (Tsys_TID.Text.Length != 8 || !CommonUtility.Util.IsValidInt32(Tsys_TID.Text))
                    this.Master.AddMessageError("Invalid TID (PXP Only).");
            }
            //valid only 
            else if (sb.Length > 0)
            {
                if (Tsys_MerchantNumber.Text.Length != 12 || !CommonUtility.Util.IsValidLong(Tsys_MerchantNumber.Text))
                    this.Master.AddMessageError("Invalid TSYS Merchant Number.");

                if (Tsys_AcquirerBin.Text.Length != 6 || !CommonUtility.Util.IsValidInt32(Tsys_AcquirerBin.Text))
                    this.Master.AddMessageError("Invalid TSYS Acquirer BIN.");

                if (Tsys_LocationNumber.Text.Length != 5 || !CommonUtility.Util.IsValidInt32(Tsys_LocationNumber.Text))
                    this.Master.AddMessageError("Invalid TSYS Location Number.");

                if (Tsys_AgentBank.Text.Length != 6 || !CommonUtility.Util.IsValidInt32(Tsys_AgentBank.Text))
                    this.Master.AddMessageError("Invalid TSYS Agent Bank.");

                if (Tsys_AgentChain.Text.Length != 6 || !CommonUtility.Util.IsValidInt32(Tsys_AgentChain.Text))
                    this.Master.AddMessageError("Invalid TSYS Agent Chain.");                
            }
        }
        else if (this.wucCompassParameters.Visible)
        {
            IEnumerable<string> errors = wucCompassParameters.Validate();

            foreach (string mess in errors)
                this.Master.AddMessageError(mess);
        }

        if (this.wucCBMS.Visible)
        {
            IEnumerable<string> errors = wucCBMS.Validate();

            foreach (string mess in errors)
                this.Master.AddMessageError(mess);
        }

        if (!this.WucPlatformCardCurrency.FormDataCheck())
        {
            this.Master.AddMessageError("Platform does not support multi-currency.");
        }
        if (!string.IsNullOrEmpty(this.AlternateEmail.Text) && !CommonUtility.Util.IsValidEmail(this.AlternateEmail.Text.TrimEnd()))
            this.Master.AddMessageError("Invalid Alternate Email.");
        string messageEmail = CommonUtility.Util.IsValidEmailList(this.AdditionalRecipients.Text.TrimEnd());
        if (!string.IsNullOrEmpty(messageEmail))
            this.Master.AddMessageError("Invalid Additional Recipients.");
        if (this.Master.ErrorCount() == 0)
        {
            return true;
        }
        else
        {
            // when there are validation errors set the status dropdown value back to previous status
            if (!isACHonly)
            {
                ListHandler.ListFindItem(StatusUID, agreement.MerchantAppClone.StatusUID);
                agreement.StatusUID = agreement.MerchantAppClone.StatusUID;
            }
            else if (UserSessions.ActiveAchMerchant != null)
            {
                AchMerchant achMerchant = UserSessions.ActiveAchMerchant;
                ListHandler.ListFindItem(ACHStatusUID, achMerchant.AchMerchantClone.MerchantStatusUID);
                achMerchant.MerchantStatusUID = achMerchant.AchMerchantClone.MerchantStatusUID;
            }

            return false;
        }
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        this.Adding = false;
        this.ToggleButtons();


        WucEquipment.FormClear();
        WucEquipment.LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);
        FormShow("");

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);
        //Added by Abarua for PXP-7866
        this.chkNMIAffiliate.Enabled = false;
        this.chkNMILockDown.Enabled = false;
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;

        //DM-7160
        txtPHQPass.TextMode = TextBoxMode.Password;
        switch (btn.Text)
        {
            case "Add Equipment":

                // DM-2279 by Jorge 
                //if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.NoEquipment) 
                //{
                //    this.Master.AddMessageError("Please uncheck the 'No Equipment' box in Profile page to add an equipment.");
                //}
                //else
                this.FormNew();
                break;

            case "Save":
                if (this.FormSave())  // valid input values.
                {
                    this.EditMode = false;

                    try
                    {
                        if ((this.StatusUID != null && this.StatusUID.SelectedValue.ToUpper().Equals(Constants.QUEUESTATUS_MS_RECEIVED))
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

                    url = "~/SecureMerchantManagementForms/frmDeployment.aspx?Adding=false";
                    url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                break;

            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

                //if account is ach only then get the achmerchant object
                //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
                //if (isACHonly)
                UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(UserSessions.CurrentMerchantApp.ID));

                WucEquipment.LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);
                WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Delete":
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
                break;

            case "Edit":
                {
                    this.EditMode = true;
                    FormShow("");

                    //DM-7160
                    txtPHQPass.TextMode = TextBoxMode.SingleLine;

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
                    WucEquipment.EnableDisableEquipmentGrid(this.EditMode);//Code added by amit for PXP-7621
                }

                break;
        }
    }

    //Code removed by Amit for PXP-7621

    protected void btnMerchantLogin_Click(object sender, ButtonEventArgs e)
    {
        MerchantApp agreement = UserSessions.CurrentMerchantApp;
        WucCreateUser1.LoginType = "Merchant";
        WucCreateUser1.UserName = agreement.ID;
        WucCreateUser1.HookTableKeyUID = agreement.MerchantAppUID;
        // Send mail to the configured address if the users are meritcard users
        bool isMeritCardUser = DataAccess.DataMerchantAppDao.IsMeritCardUser(agreement.AgentUID, ConfigurationManager.AppSettings["MeritcardParentAgentUID"]);
        if (isMeritCardUser)
        {
            WucCreateUser1.EmailTo = ConfigurationManager.AppSettings["MeritcardMailsSendToAddress"];
        }
        else
        {
            WucCreateUser1.EmailTo = agreement.BusinessEmailAddress;
        }
        WucCreateUser1.DBA = agreement.BusinessDBAName;
        WucCreateUser1.PrivateLabelUID = agreement.PrivateLabelUID;

        if (!string.IsNullOrWhiteSpace(WucCreateUser1.PrivateLabelUID))
            WucCreateUser1.objPrivateLabel = DataAccess.DataMerchantAppDao.GetPrivateLabel(agreement.PrivateLabelUID);

        WucCreateUser1.Key = agreement.MerchantKey;
        WucCreateUser1.MerchantPin = agreement.MerchantPIN;
        WucCreateUser1.LockdownKey = agreement.LockdownKey; //PXP-11488

        Subscription subscription = DataProduct.GetMerchantCurrentProductSubscription(int.Parse(agreement.ID), "Meritus_Mobile");

        WucCreateUser1.HasMerchantPin = subscription == null ? false : subscription.IsActive;
        agreement.HasMerchantPin = subscription == null ? false : subscription.IsActive;

        WucCreateUser1.OfficeID = Convert.ToInt32(agreement.Office);
        WucCreateUser1.Status = agreement.StatusUID;
        WucCreateUser1.Formshow();
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    /// <summary>
    /// PXP-8289 by Sanidhya
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void onNMIVendorChanged(object sender, EventArgs e)
    {
        DropDownList ddlList = (DropDownList)sender;
        string selectedItem = ddlList.SelectedItem.Value;
        if (selectedItem != "-1")
        {
            Hashtable prms = new Hashtable();
            DataApp _dataApp = new DataApp();
            var _crmData = _dataApp.GetCRMList(prms).FirstOrDefault(x => (x.Type.ToUpper() == Constant.CRM_Type || x.Type.ToUpper() == "GATEWAY") && x.CRMID.ToString() == selectedItem);//DM-2403 VendorUID ZEUS Emanuel
            if (_crmData != null && _crmData.AcceptTransactions.ToString().ToUpper() == "TRUE")
            {
                this.AllowSales.Checked = true;
                UserFacade userFacade = new UserFacade();
                var userRoles = userFacade.GetUser(UserSessions.CurrentUser.UID).UserRoles.Where(u => u.Value.Enabled == true);  // Dynamic list of enabled user roles;      
                if ((UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_APPLICATION_BOARDING || UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_RISK || (userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_SPECIALACCESS)))))
                {
                    this.AllowSales.Enabled = true;
                }
                else
                {
                    this.AllowSales.Enabled = false;
                }
            }
            else if (_crmData != null && _crmData.AcceptTransactions.ToString().ToUpper() == "FALSE")
            {
                this.AllowSales.Checked = false;
                this.AllowSales.Enabled = false;
            }
            else
            {
                this.AllowSales.Checked = false;
            }
        }


    }
    //PXP-13342 by Satyajit
    private Dictionary<string, bool> BindVerticalMarketBillingData()
    {
        Dictionary<string, bool> VerticalMarket_Billing = new Dictionary<string, bool>();
        Hashtable prms = new Hashtable();
        DataSet dt = new DataSet();

        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
        dt = DataAccess.DataMerchantAppDao.GetVerticalMarkets(prms);

        if (dt != null && dt.Tables.Count > 0)
        {
            DataRow[] drVerticalMarketDG = dt.Tables[0].Select("VerticalMarketTypeID=" + 1 + " AND " + " Key='DG'");
            if (drVerticalMarketDG.Count() > 0)
            {
                VerticalMarket_Billing.Add("DG", DataLayer.Field2Bool((drVerticalMarketDG[0].ItemArray[4])));
            }

            DataRow[] drVerticalMarketFT = dt.Tables[0].Select("VerticalMarketTypeID=" + 2 + " AND " + " Key='FT'");
            if (drVerticalMarketFT.Count() > 0)
            {
                VerticalMarket_Billing.Add("FT", DataLayer.Field2Bool(((drVerticalMarketFT[0].ItemArray[4]))));
            }

            DataRow[] drVerticalMarketContinuity = dt.Tables[0].Select("VerticalMarketTypeID=" + 2 + " AND " + " Key='Continuity'");
            if (drVerticalMarketContinuity.Count() > 0)
            {
                VerticalMarket_Billing.Add("Continuity", DataLayer.Field2Bool(((drVerticalMarketContinuity[0].ItemArray[4]))));
            }

            DataRow[] drVerticalMarketOTS = dt.Tables[0].Select("VerticalMarketTypeID=" + 2 + " AND " + " Key='OTS'");
            if (drVerticalMarketOTS.Count() > 0)
            {
                VerticalMarket_Billing.Add("OTS", DataLayer.Field2Bool(((drVerticalMarketOTS[0].ItemArray[4]))));
            }
        }
        return VerticalMarket_Billing;
    }

    //START:Added by Abarua for PXP-7866
    /// <summary>
    /// PXP-7866 by Abarua
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNMILockDownAPI_Click(object sender, EventArgs e)
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;
        Hashtable prms = new Hashtable();
        string businessDBAName = app.BusinessDBAName.RemoveSpecialCharacters();
        prms.Add("@Username", "PXP" + app.ID + businessDBAName.Substring(0, Math.Min(businessDBAName.Length, 20))); //PXP-11312 bug fix asheesh
        prms.Add("@Type", "LOCKDOWN");
        if (DataAccess.DataMerchantAppDao.IsExistsNmiMerchantApiUsername(prms))
        {
            IsUsernameExistError.Text = "Username Already Exists";
            WebDialogWindow3.WindowState = DialogWindowState.Normal;
            return;
        }
        if (app != null)
        {
            //PXP-13342 by Satyajit
            //Dictionary<string, bool> VerticalMarket_BillingTypes = new Dictionary<string, bool>();
            app.VerticalMarket_BillingTypes = BindVerticalMarketBillingData();

            if (dataCheckNMI(app, "LOCKED"))
                return;
            NMIMerchantOnboardingAPI nmiMerchantOnboardingApi = new NMIMerchantOnboardingAPI("LOCKED");
            NMIApiResponse result = nmiMerchantOnboardingApi.LoadGatewayAccounts(app, "1", LockDownPlanID.Text);
            if (result.IsError)
            {
                this.Master.AddMessageError(result.message);
            }
            else
            {
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                FormBinding.BindObjectToControls(UserSessions.CurrentMerchantApp, PanelNMI);
                LockdownDate.Text = (UserSessions.CurrentMerchantApp.LockdownDate.Equals(DateTime.MinValue) ? "" : UserSessions.CurrentMerchantApp.LockdownDate.ToShortDateString());
                AffiliateDate.Text = (UserSessions.CurrentMerchantApp.AffiliateDate.Equals(DateTime.MinValue) ? "" : UserSessions.CurrentMerchantApp.AffiliateDate.ToShortDateString());
                //PXP-11314 disable btn NMI abarua
                btnNMIAffiliateAPI.Enabled = !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.LockdownNMIUsername) && this.EditMode;
                this.Master.AddMessageSuccess("Merchant boarded at NMI lock-down successfully");
            }
        }
    }
    /// <summary>
    /// PXP-9122 by Abarua
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNMIAffiliateAPI_Click(object sender, EventArgs e)
    {
        lbl.Text = "Are you sure you want to onboard merchant on NMI Affiliate version?";
        WebDialogWindow1.WindowState = DialogWindowState.Normal;

    }
    private bool dataCheckNMI(MerchantApp app, string version)
    {
        if (string.IsNullOrEmpty(app.BusinessAddress))
        {
            this.Master.AddMessageError("Enter the Business Address.");
        }
        if (string.IsNullOrEmpty(app.BusinessCity))
        {
            this.Master.AddMessageError("Enter the Business City.");
        }
        if (string.IsNullOrEmpty(app.BusinessDBAName))
        {
            this.Master.AddMessageError("Enter the Business DBA Name.");
        }
        if (string.IsNullOrEmpty(app.BusinessState))
        {
            this.Master.AddMessageError("Enter the Business State.");
        }
        if (string.IsNullOrEmpty(app.BusinessZip))
        {
            this.Master.AddMessageError("Enter the Business Zip.");
        }
        if (string.IsNullOrEmpty(app.BusinessCountry))
        {
            this.Master.AddMessageError("Enter the Business Country.");
        }
        if (app.ContactList == null)
        {
            app.ContactList = DataContact.SearchContact(Convert.ToInt32(app.ID), eControlContactType.Merchant);
        }

        var contactList = app.ContactList?.FirstOrDefault();
        if (contactList == null)
        {
            this.Master.AddMessageError("Enter the Contact information.");
        }
        else
        {
            if (string.IsNullOrEmpty(contactList.FirstName))
            {
                this.Master.AddMessageError("Enter the Contact information First Name.");
            }
            if (string.IsNullOrEmpty(contactList.LastName))
            {
                this.Master.AddMessageError("Enter the Contact information Last Name.");
            }
            if (string.IsNullOrEmpty(contactList.EmailAddress))
            {
                this.Master.AddMessageError("Enter the Contact information Email Address.");
            }
            if (contactList.PhoneList == null || contactList.PhoneList.FirstOrDefault() == null)
            {
                this.Master.AddMessageError("Enter the Contact information Phone");
            }
            else
            {
                if (string.IsNullOrEmpty(contactList.PhoneList.FirstOrDefault().PhoneNumber))
                {
                    this.Master.AddMessageError("Enter the Contact information Phone Number");
                }
            }
        }

        if (string.IsNullOrEmpty(app.BusinessWebsite))
        {
            this.Master.AddMessageError("Enter the Business Website.");
        }
        if (!version.Equals("LOCKED"))
        {
            if (string.IsNullOrEmpty(app.AccountNumber))
            {
                this.Master.AddMessageError("Enter the Account Number");
            }
            if (string.IsNullOrEmpty(app.RoutingNumber))
            {
                this.Master.AddMessageError("Enter the Routing Number");
            }
            if (string.IsNullOrEmpty(app.MerchantKey))
            {
                this.Master.AddMessageError("No Merchant Key");
            }
            if (AffiliatePlanId.SelectedItem.Value == "-1")
            {
                this.Master.AddMessageError("Please Select the Plan Id");
            }
        }
        if (string.IsNullOrEmpty(app.Descriptor))
        {
            app.Descriptor = MerchantFacade.GetPrioritizedMerchantDescriptor(app.ID, app.Descriptor);
            if (string.IsNullOrEmpty(app.Descriptor))
            {
                this.Master.AddMessageError("Enter the Descriptor");
            }
        }
        int i = 0;
        if (string.IsNullOrEmpty(app.SicCode) || !Int32.TryParse(app.SicCode, out i))
        {
            this.Master.AddMessageError("Enter MCC Code");
        }

        ///DM-5815 by Jorge 
        if (version.Equals("LOCKED"))
        {
            if (app.VerticalMarket_BillingTypes != null && app.VerticalMarket_BillingTypes.Count > 0)
            {
                if (!app.VerticalMarket_BillingTypes.ContainsKey("DG"))
                    this.Master.AddMessageError("Merchant does not have Billing Type DG");
                if (!app.VerticalMarket_BillingTypes.ContainsKey("FT"))
                    this.Master.AddMessageError("Merchant does not have Billing Type FT");
                if (!app.VerticalMarket_BillingTypes.ContainsKey("Continuity"))
                    this.Master.AddMessageError("Merchant does not have Billing Type Continuity");
                if (!app.VerticalMarket_BillingTypes.ContainsKey("OTS"))
                    this.Master.AddMessageError("Merchant does not have Billing Type OTS");
            }
            else
                this.Master.AddMessageError("Merchant does not have Billing Types");

            ///DM-5800 by Jorge         
            var nmiPlatform = NMIMerchantOnboardingAPI.GetPlatform(UserSessions.CurrentMerchantApp);
            if (nmiPlatform == Platform.FirstDataNashville)
            {
                if (string.IsNullOrEmpty(app.AuthPlatformMid))
                {
                    this.Master.AddMessageError("Merchant does not have a valid Merchant ID.");
                }
                if (string.IsNullOrEmpty(app.TID))
                {
                    this.Master.AddMessageError("Merchant does not have a valid Terminal ID.");
                }
            }
            ///END DM-5800          
        }
        else
        {
            if (string.IsNullOrEmpty(app.LockdownKey))
            {
                this.Master.AddMessageError("Merchant does not have a valid Lockdown Key.");
            }
        }
        ///END DM-5815
        return this.Master.ErrorCount() > 0;
    }
    protected void btnUsernameExistError_Click(object sender, EventArgs e)
    {
        WebDialogWindow3.WindowState = DialogWindowState.Hidden;
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {

        WebDialogWindow1.WindowState = DialogWindowState.Hidden;
        MerchantApp app = UserSessions.CurrentMerchantApp;
        Hashtable prms = new Hashtable();
        string businessDBAName = app.BusinessDBAName.RemoveSpecialCharacters();
        prms.Add("@Username", app.ID + businessDBAName.Substring(0, Math.Min(businessDBAName.Length, 20)));
        prms.Add("@Type", "AFFILIATE");
        if (DataAccess.DataMerchantAppDao.IsExistsNmiMerchantApiUsername(prms))
        {
            IsUsernameExistError.Text = "Username " + HostUserName.Text.Trim() + " Already Exists";
            WebDialogWindow3.WindowState = DialogWindowState.Normal;
            return;
        }

        if (app != null)
        {
            //Dictionary<string, bool> VerticalMarket_BillingTypes = new Dictionary<string, bool>();
            app.VerticalMarket_BillingTypes = BindVerticalMarketBillingData();

            if (dataCheckNMI(app, "AFFILIATE"))
                return;


            NMIMerchantOnboardingAPI nmiMerchantOnboardingApi = new NMIMerchantOnboardingAPI("AFFILIATE");
            NMIApiResponse result = nmiMerchantOnboardingApi.LoadGatewayAccounts(app, AffiliatePlanId.SelectedItem.Value, AffiliatePlanId.SelectedItem.Text);
            if (result.IsError)
            {
                this.Master.AddMessageError(result.message);
            }
            else //Refresh App
            {

                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                FormBinding.BindObjectToControls(UserSessions.CurrentMerchantApp, PanelNMI);
                LockdownDate.Text = (UserSessions.CurrentMerchantApp.LockdownDate.Equals(DateTime.MinValue) ? "" : UserSessions.CurrentMerchantApp.LockdownDate.ToShortDateString());
                AffiliateDate.Text = (UserSessions.CurrentMerchantApp.AffiliateDate.Equals(DateTime.MinValue) ? "" : UserSessions.CurrentMerchantApp.AffiliateDate.ToShortDateString());
                HostUserName.Text = UserSessions.CurrentMerchantApp.HostUserName ?? "";
                this.Master.AddMessageSuccess("Merchant boarded onto NMI Paysafe Affiliate successfully");
            }
        }
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        WebDialogWindow1.WindowState = DialogWindowState.Hidden;
    }
    protected string GetSubmitPostBack()
    {
        return Page.ClientScript.GetPostBackEventReference(Button1, string.Empty);
    }
    //END:Added by Abarua for PXP-7866

    /// <summary>
    /// Even handler added for PXP-16593 for refresh page on dialog windlow 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WebDialogWindow2_StateChanged(object sender, Infragistics.Web.UI.LayoutControls.DialogWindowStateChangedEventArgs e)
    {
        if (WebDialogWindow2.WindowState == Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden)
        {

            this.FormShow("");
        }
    }

    //Ani: DM-5755
    public void LoadVendors()
    {
        LookupTableHandler.LoadVendors(VendorList);
        if (UserSessions.CurrentMerchantApp != null)
            VendorList.SelectedIndex = LookupTableHandler.GetSelectedVendor(UserSessions.CurrentMerchantApp.ID);
    }


    /// <summary>
    /// AhmerBashir 16-03-2026
    /// DM-7315
    /// </summary>
    public bool FindMakerPaymentXP(string merchantappuid)
    {
        bool isPaymentXPExists = false;
        if (CommonUtility.Util.IsValidGuid(merchantappuid))
        {
            EquipmentFacade facade = new EquipmentFacade();
            Hashtable prms = new Hashtable
            {
                { "@MerchantAppUID", merchantappuid },
                { "@IsEnabled", true }
            };
            DataSet ds = facade.GetMerchantAppItem(prms);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (item["EquipmentMaker"].ToString().Equals("Payment XP", StringComparison.OrdinalIgnoreCase))
                {
                    isPaymentXPExists = true;
                    break;
                }
            }            
        }
        return isPaymentXPExists;
    }
}
