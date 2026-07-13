using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Linq;

public partial class frmMerchantACH : frmBaseDataEntry
{
    bool isValid = true;

    public int ZID
    {
        get
        {
            if (ViewState["ZID"] == null)
                return 0;
            else
                return (int)ViewState["ZID"];
        }
        set
        {

            ViewState["ZID"] = value;
        }
    }

    public int ACH_ID
    {
        get
        {
            if (ViewState["ACHID"] == null)
                return 0;
            else
                return (int)ViewState["ACHID"];
        }
        set
        {

            ViewState["ACHID"] = value;
        }
    }

    string[] ACHBankField 
    { 
        get 
        { 
            return new string[] { dfiAcctName.ID, TransRoute.ID, AccountNo.ID, AccountTmp.ID, AccountType.ID };
        } 
    }
    public bool HasBankAccountRole
    {
        get
        {
            return (UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_BANK_ACCOUNT) && UserSessions.CurrentUser.UserRoles[Constants.ROLE_BANK_ACCOUNT].Enabled);
        }
    }
    //Code added by Gabriel Gonzalez for DM-4691
    public ControlObject PnlBankField
    {
        get
        {
            var form = this.GetType().BaseType.Name.ToUpper();
            var obj = UserSessions.CurrentUser.UserForms[form].ControlObjects.FirstOrDefault(c => c.ID == "pnlBankFields");
            return obj ?? new ControlObject();
        }
    }
    //Code added by Gabriel Gonzalez for DM-4691
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        WucBusinessInfo1.pnlInfo.Enabled = false;

        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.ACH);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "ACH/DD");
            }

            LookupTableHandler.LoadDropDownList(this.Secc, "Ach_Search_SECC", "Secc", "Secc");
            LookupTableHandler.LoadDropDownListNoDefault(this.SeccList, "Ach_Search_SECC", "Secc", "SeccID");
            LookupTableHandler.LoadDropDownList(this.BankID, "Ach_Search_Bank_Info", "BankName", "BankID");
            LookupTableHandler.LoadDropDownList(this.MerchantTypeCode, "Ach_Search_Merchant_Type", "Description", "Type");
            LookupTableHandler.LoadDropDownList(this.Test, "Ach_Select_Merchant_Test", "TestDesc", "TestCode");
            LookupTableHandler.LoadDropDownList(this.GroupID, "Ach_Search_Group_Merchant", "Group Name", "GroupID");

            this.LoadMerchantInformation();

            this.Adding = false;

            if (this.Adding)
            {
                this.FormNew();
            }
            else
            {
                this.FormShow(this.UID);
            }

        }

    }

    //code added by koshlendra for PXP-4990 start
    protected void Page_PreRender(object sender, EventArgs e)
    {
        txtTotalACHTransCompleted.ReadOnly = true;
        txtTotalACHProductSold.ReadOnly = true;
    }
    //code added by koshlendra for PXP-4990 end

    private void LoadMerchantInformation()
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        UserSessions.CurrentMerchantApp = app;

        DropDownList brand = (DropDownList)WucBusinessInfo1.FindControl("Brand");

        //suppress fields for certain brand
        pnlMeritus.Visible = app.Brand == MerchantBrand.Meritus;
        pnlOptimal.Visible = app.Brand == MerchantBrand.Optimal;
        //code added by koshlendra for PXP-4990 start
        //PXP-10208: BBVA Bank By Sanidhya
        pnlWoodforestIrvine.Visible = app.Office == CommonUtility.Util.Offices.Irvine && (app.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            || app.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
            || app.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS || app.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS);
        //code added by koshlendra for PXP-4990 end 


        // you'll see this done a few more times. basically you can only have
        // one control with the same name at a time on a page.
        // so we create an opt* set of controls and just manually set them.
        // this way we don't interfere with the binding.
        if (pnlOptimal.Visible)
        {
            optMonthlyVolume.Value = MonthlyVolume.Value;
            optAverageTicket.Value = AverageTicket.Value;
            optItemLimitAmount.Value = HighTicket.Value;
            optReservepct.Value = Reservepct.Value;
            optReservePeriod.Value = ReservePeriod.Value;
            optHoldPeriod.Value = HoldPeriod.Value;
        }

        trMerchantTypeCode.Visible = app.Brand == MerchantBrand.Meritus;
        trNAICS.Visible = app.Brand == MerchantBrand.Meritus;
        trAchDiscrtn.Visible = app.Brand == MerchantBrand.Meritus;

        FormBinding.BindObjectToControls(app, WucBusinessInfo1);
        FormHandler.ShowClosureCodes(WucBusinessInfo1, app.StatusUID.ToUpper());

        this.MerchantID.Text = app.ID;
        this.AchCoName.Text = app.BusinessDBAName;
        this.lblMerchantUID.Text = app.MerchantAppUID;
        this.AchID.Text = app.AchID.ToString();
        this.ZID = CommonUtility.Util.if_i(app.ID, 0);
        this.UID = app.MerchantAppUID;
        this.ACH_ID = app.AchID;


        WucBusinessInfo1.LoadOffice(UserSessions.CurrentMerchantApp);
    }

    public override void FormShow(string ID)
    {
        AchMerchant ach = null;
        if (ZID > 0)
        {
            ach = DataAccess.DataAchMerchantDao.GetAchMerchant(ZID);
            if (ach != null)
            {
                ach.CloneAchMerchant();

                MerchantFacade facade = new MerchantFacade();
                MerchantApp app = facade.GetMerchantAppZeus(this.UID);

                LookupTableHandler.MerchantAppStatus(MerchantStatusUID, false, "Merchant Management", app, ach);
                MerchantStatusUID.Enabled = true;
                //if (ach.AccountNo != null)
                //{
                //    pnlAccountType.Visible = true;
                //    spAccountType.Visible = true;
                //}

                //check to see if the account is ACH only and get the ach status in case if it is or else the cc status                
                string m_StatusUID = app.StatusUID.ToUpper();

                //PXP-9348 RThakur >> Start
                hiddenCrmStatus.Value = app.CRMStatus;
                hiddenAcceptTransaction.Value = Convert.ToString(app.CRMAcceptTransactions);
                hidIsNutraMerchant.Value = (app.IsNutraMerchant && app.SicCode == "5968").ToString().ToUpper();
                hidStatus.Value = m_StatusUID.ToUpper();
                //PXP-9348 RThakur >> End

                if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
                {
                    m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
                }

                FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

                FormBinding.BindObjectToControls(ach, this.pnlDetail);
                FormBinding.BindObjectToControls(ach, this.pnlSalesForce);

                if (app.Brand == MerchantBrand.Optimal)
                {
                    optMonthlyVolume.Value = MonthlyVolume.Value;
                    optAverageTicket.Value = AverageTicket.Value;
                    optItemLimitAmount.Value = HighTicket.Value;
                    optReservepct.Value = Reservepct.Value;
                    optReservePeriod.Value = ReservePeriod.Value;
                    optHoldPeriod.Value = HoldPeriod.Value;
                }
                LoadWebsite();
                UserSessions.CurrentAchMerchant = ach;
                //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
                UserSessions.ActiveAchMerchant = ach;

                //Only allow admin and risk to set monthly billing false
                //logic works with default role Id
                switch (UserSessions.CurrentUser.DefaultRoleUID.ToUpper())
                {
                    case Constants.ROLE_ADMIN:
                    case Constants.ROLE_RISK:
                        WebTab1.Tabs.FindTabFromKey("Admin").Hidden = false;
                        break;

                    default:
                        break;
                }

                /* Allow only departments below to make changes to profile once app is live */
                if (MerchantStatusUID.SelectedItem.Text.Substring(0, 2).ToUpper() == "MS")
                {
                    bool has_access = false;

                    foreach (KeyValuePair<string, UserRole> kvp in UserSessions.CurrentUser.UserRoles)
                    {
                        switch (kvp.Value.RoleID)
                        {
                            case Constants.ROLE_CREDIT_UNDERWRITING:
                            case Constants.ROLE_IT:
                            case Constants.ROLE_ADMIN:
                            case Constants.ROLE_APPLICATION_BOARDING:
                            case Constants.ROLE_RISK:
                                has_access = true;
                                break;
                        }
                    }

                    if (btnSave.Enabled && this.EditMode)
                        FormHandler.SetControlEditMode(pnlDetail, has_access);

                    if (!has_access)
                        MerchantStatusUID.Enabled = true;
                }

                List<StandardEntryClassCode> merchantStandardEntryClassCodeList = DataAchMerchant.GetInstance().GetMerchantStandardEntryCodeList(ach.AchID);
                if (merchantStandardEntryClassCodeList != null)
                {
                    foreach (ListItem item in this.SeccList.Items)
                    {
                        foreach (StandardEntryClassCode c in merchantStandardEntryClassCodeList)
                        {
                            if (c.SeccID == int.Parse(item.Value))
                            {
                                item.Selected = true;
                            }
                        }
                    }
                }

                //PXP-4990 by koshlendra start
                txtTotalACHTransCompleted.Value = Convert.ToDouble(TinfoWrittenContractPercent.Value) + Convert.ToDouble(TinfoInternetInitiatedPercent.Value) + Convert.ToDouble(TinfoTelephoneInitiatedPercent.Value);
                txtTotalACHProductSold.Value = Convert.ToDouble(TinfoConsumerPercent.Value) + Convert.ToDouble(TinfoBusinessPercent.Value);
                if (Convert.ToDecimal(TinfoInternetInitiatedPercent.Value) > 1)
                {
                    trURLInternetPage.Visible = true;                    
                }
                //PXP-4990 by koshlendra end
            }
        }

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        //End 
        // CU-Approved Lockdown! only members of role "Special Access" can edit these fields once an account has been approved.
        if (this.EditMode == true && MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_APPROVED))
        {
            bool has_special_access_role = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS)
                && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS].Enabled == true);

            if (!has_special_access_role)
            {
                FormHandler.SetControlEditMode(pnlReqiredFields, false);
                FormHandler.SetControlEditMode(pnlFeeInformation, false);

                // selectively turn on status, because other people need to change it.
                MerchantStatusUID.Enabled = true;
            }
        }

        // you can only edit when in edit mode, and when saleforce id is 0 or achmerchant object is null.
        SalesForceID.ReadOnly = ((UserSessions.CurrentAchMerchant == null || UserSessions.CurrentAchMerchant.SalesForceID == 0) && this.EditMode) ? false : true;
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            MasterPageMerchant master = (MasterPageMerchant)this.Master;
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        } /******** End of PXP-2206 **************/

        //Code added by Gabriel Gonzalez for DM-4647, DM-4691
        if (ach != null)
        {
            AccountNo.Text = ach.AccountNo;
            AccountTmp.Text = ach.AccountNoMask;
            if (HasBankAccountRole)
            {
                if (EditMode)
                {
                    FormHandler.SetControlEditMode(pnlReqiredFields, ACHBankField, PnlBankField.IsEnabled);
                    AccountTmp.Text = PnlBankField.IsVisible ? ach.AccountNo : ach.AccountNoMask;
                }
            }
            else
                FormHandler.SetControlEditMode(pnlReqiredFields, ACHBankField, false);
        }
        //Code added by Gabriel Gonzalez for DM-4647, DM-4691
    }

    public override void FormClear()
    {

    }

    public override bool FormSave()
    {
        throw new Exception("use the other formsaveach instead");
    }

    private AchMerchant FormSaveACH()
    {
        AchMerchant achMerchant = null;

        try
        {
            if (isValid)
            {
                DataAchMerchant data = DataAccess.DataAchMerchantDao;

                AchMerchant clone = null;

                if (this.Adding)
                    achMerchant = new AchMerchant();
                else
                {
                    //to make sure we are pulling the correct ach record try with achid this time.in formshow we are using merhcant id to pull ach merchant
                    //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
                    if (UserSessions.CurrentAchMerchant == null && this.ACH_ID > 0)
                        UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.Get_AchMerchant(this.ACH_ID);

                    if (UserSessions.ActiveAchMerchant != null)
                    {
                        UserSessions.ActiveAchMerchant.CloneAchMerchant();
                        achMerchant = UserSessions.ActiveAchMerchant;
                        clone = (AchMerchant)achMerchant.Clone();
                    }
                }

                MerchantFacade facade = new MerchantFacade();
                MerchantApp agreement = facade.GetMerchantAppZeus(this.UID);
                if (BusinessWebsiteACH.Visible == true)
                {
                    agreement.BusinessWebsite = BusinessWebsiteACH.Text;
                }

                string OrigRouteNo = achMerchant.TransRoute;
                string OrigAcctNo = achMerchant.AccountNo;
                string OrigStatus = achMerchant.MerchantStatusUID;

                if (agreement.Brand == MerchantBrand.Optimal)
                {
                    MonthlyVolume.Value = optMonthlyVolume.Value;
                    AverageTicket.Value = optAverageTicket.Value;
                    HighTicket.Value = optItemLimitAmount.Value;
                    Reservepct.Value = optReservepct.Value;
                    ReservePeriod.Value = optReservePeriod.Value;
                    HoldPeriod.Value = optHoldPeriod.Value;
                }

                FormBinding.BindControlsToObject(achMerchant, this.pnlDetail);
                FormBinding.BindControlsToObject(achMerchant, this.pnlSalesForce);
                achMerchant.MerchantID = ZID;

                if (!this.FormDataCheck())
                    return null;

                int bankId = achMerchant.BankID;
                int rowsAffected = 0;

                AccountNo.BorderColor = System.Drawing.Color.Gray;
                TransRoute.BorderColor = System.Drawing.Color.Gray;
                MerchantStatusUID.BackColor = System.Drawing.Color.White;
                //PXP-11451 Anuj
                if ((agreement.IsNutraMerchant && agreement.Office == CommonUtility.Util.Offices.Irvine) && 
                (clone != null && MerchantStatusUID.SelectedValue.ToUpper() != clone.MerchantStatusUID.ToUpper() && MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE))
                {
                    FormHandler.AllowPxpForNutra();
                }

                if (!this.Adding)
                {
                    if (agreement.MultiAccLink && (MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && MerchantStatusUID.SelectedValue.ToUpper() != clone.MerchantStatusUID.ToUpper()) && !agreement.UWMultOverride)
                    {
                        MerchantStatusUID.BackColor = System.Drawing.Color.Red;
                        this.Master.AddMessageError("Status change denied. Due to a multiple account link, an override is required in order to change the status to Approved.");
                        ListHandler.ListFindItem(MerchantStatusUID, OrigStatus);
                        achMerchant.MerchantStatusUID = OrigStatus;
                        return null;
                    }

                    // this is a temporary change, we will make the 3DE decisioning available for other offices also.
                    if (MerchantStatusUID.SelectedValue != OrigStatus && MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_CU_3DEDECISION && UserSessions.CurrentMerchantApp.Office != CommonUtility.Util.Offices.Irvine)
                    {
                        this.Master.AddMessageError("3DE decision status is applicable to Irvine office only.");
                        ListHandler.ListFindItem(MerchantStatusUID, OrigStatus);
                        achMerchant.MerchantStatusUID = OrigStatus;
                        return null;
                    }

                    achMerchant.DateUpdated = DateTime.Now;
                    achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;

                    rowsAffected = data.UpdateAchMerchant(achMerchant);
                    FormHandler.LogFormChanges(achMerchant.AchCoName, "", achMerchant.MerchantID, clone, achMerchant);

                    if (rowsAffected > 0)
                        this.Master.AddMessageStatus("Successfully Updated Merchant ACH/DD Profile.");
                    else
                        this.Master.AddMessageError("There was an error in processing your request");
                }
                else
                {
                    achMerchant.DateAdded = DateTime.Now;
                    achMerchant.AddedBy = UserSessions.CurrentUser.UserName;
                    achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;

                    rowsAffected = data.InsertAchMerchant(achMerchant);

                    if (rowsAffected > 0)
                    {
                        this.Master.AddMessageStatus("Successfully Added Merchant ACH/DD Profile.");
                        UserSessions.CurrentMerchantApp.AchID = achMerchant.AchID;
                    }
                    else
                    {
                        this.Master.AddMessageError("There was an error in processing your request");
                    }
                }

                if (achMerchant.SalesForceID > 0)
                {
                    try
                    {
                        DataSalesForce.InsertMerchantSalesForceID(Convert.ToInt32(agreement.ID), achMerchant.SalesForceID, 2, UserSessions.CurrentUser.UserName);
                    }
                    catch
                    {
                        this.Master.AddMessageError("Could not insert SalesForceID.");
                    }
                }

                //Get the Default off Secc control
                StandardEntryClassCode defaultStandardEntryClassCode = new StandardEntryClassCode();

                foreach (ListItem item in this.Secc.Items)
                {
                    if (item.Selected)
                    {
                        defaultStandardEntryClassCode.Code = item.Value;
                        defaultStandardEntryClassCode.IsDefault = true;
                    }
                }
                //get ID - Secc control doesn't provide an Id
                foreach (ListItem item in SeccList.Items)
                {
                    if (item.Text == defaultStandardEntryClassCode.Code)
                    {
                        defaultStandardEntryClassCode.SeccID = int.Parse(item.Value);
                    }
                }

                //create a new list and populate it with selected items from SeccList
                List<PaymentXP.BusinessObjects.StandardEntryClassCode> seccCodeList = new List<StandardEntryClassCode>();

                foreach (ListItem item in SeccList.Items)
                {
                    if (item.Selected)
                    {
                        seccCodeList.Add(new StandardEntryClassCode()
                        {
                            SeccID = int.Parse(item.Value),
                            Code = (string)item.Text,
                        });
                    }
                }
                //Set the default item
                bool foundDefaultInList = false;
                foreach (StandardEntryClassCode code in seccCodeList)
                {
                    if (defaultStandardEntryClassCode != null)
                    {
                        if (defaultStandardEntryClassCode.SeccID == code.SeccID)
                        {
                            foundDefaultInList = true;
                            code.IsDefault = true;
                        }
                    }
                }
                if (!foundDefaultInList)
                {
                    if (defaultStandardEntryClassCode != null)
                    {
                        seccCodeList.Add(defaultStandardEntryClassCode);
                    }
                }

                //secc is only applicable to US banks so only update secc if the assigned bank is a US bank
                if (bankId >= 11 && bankId <= 19)
                    DataAchMerchant.GetInstance().UpdateMerchantStandardEntryCodeList(achMerchant.AchID, seccCodeList);

                if (rowsAffected > 0)
                {
                    // why do we do this?
                    agreement.AllowBlindCredits = achMerchant.AllowBlindCredits;

                    facade.UpdateMerchantApp(agreement);

                    //PXP-7620 RThakur
                    if (!this.Adding)
                    {
                        string statusUIDToCheck = string.Empty; // will contain previous value
                        string statusNameToCheck = string.Empty; //will contain new value
                        if (achMerchant != null)
                        {
                            statusUIDToCheck = achMerchant.MerchantStatusUID.ToUpper();
                            statusNameToCheck = achMerchant.MerchantStatusName;
                        }
                        else
                        {
                            statusUIDToCheck = agreement.StatusUID.ToUpper();
                            statusNameToCheck = agreement.StatusName;
                        }
                        if ((statusNameToCheck.StartsWith("CU") &&
                             (statusUIDToCheck.Equals(Constants.QUEUESTATUS_CU_DECLINED) || statusUIDToCheck.Equals(Constants.QUEUESTATUS_CU_WITHDRAWN))
                            )
                            ||
                            (statusNameToCheck.Equals("SS - App Incomplete") && statusUIDToCheck.Equals(Constants.QUEUESTATUS_SS_WITHDRAWN)
                            )
                           )
                        {
                            FormHandler.CloseApplicationTicket(agreement, UserSessions.CurrentUser);
                        }
                    }
                    //PXP-7620 RThakur
                }
                else
                {
                    achMerchant = null;
                }
            }
        }
        catch (Exception exc)
        {
            throw exc;
        }

        return achMerchant;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Active.Checked = true;
        this.CreateHold.Checked = true;
        this.Adding = true;
        this.EditMode = true;


        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        this.ToggleButtons();

        LookupTableHandler.MerchantAppStatus(MerchantStatusUID, false, "Merchant Management", "SS");
        MerchantStatusUID.Enabled = false;
        ListHandler.ListFindItem(MerchantStatusUID, "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409"); //Application Processing Received Status


        if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.ACH)
            ListHandler.ListFindItem(BankID, "13"); //KBT ACH bank

        ListHandler.ListFindItem(Test, "H");//Test flag is Hold
        MerchantTypeCode.SelectedIndex = 1;


        DropDownList brand = (DropDownList)WucBusinessInfo1.FindControl("Brand");
        if ((MerchantBrand)Convert.ToInt32(brand.SelectedValue) == MerchantBrand.Optimal)
        {
            ListHandler.ListFindItem(Secc, "CCD");//Test flag is Hold
            AchDiscrtn.Text = "ACH/DD";
        }

        SalesForceID.ReadOnly = false;

        if (EditMode && HasBankAccountRole && PnlBankField.IsEnabled)
            FormHandler.SetControlEditMode(pnlReqiredFields, ACHBankField, PnlBankField.IsEnabled);
        else
            FormHandler.SetControlEditMode(pnlReqiredFields, ACHBankField, false);
    }

    public override bool FormDelete()
    {
        return false;
    }

    public override bool FormDataCheck()
    {
        DropDownList brand = (DropDownList)WucBusinessInfo1.FindControl("Brand");

        this.ResetValidationIndicators();

        string errorMessage = String.Empty;
        DataAchMerchant data = DataAccess.DataAchMerchantDao;



        bool error = false;

        System.Drawing.Color red = System.Drawing.Color.Red;

        int bankId;
        int.TryParse(this.BankID.SelectedValue, out bankId);

        if ((MerchantBrand)Convert.ToInt32(brand.SelectedValue) == MerchantBrand.Meritus)
        {
            if (this.AchCoName.Text.Trim().Length == 0)
            {
                this.AchCoName.BorderColor = red;
                error = true;
            }
            //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
            if (this.AchDescrp.Text.Trim().Length == 0 && (UserSessions.CurrentAchMerchant == null || !(UserSessions.CurrentAchMerchant.AchMerchantClone != null && UserSessions.CurrentAchMerchant.AchMerchantClone.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && this.MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED)))
            {
                this.AchDescrp.BorderColor = red;
                error = true;
            }

            //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
            if (this.AchDiscrtn.Text.Trim().Length == 0 &&
                (UserSessions.CurrentAchMerchant == null ||
                !(UserSessions.CurrentAchMerchant.AchMerchantClone != null && UserSessions.CurrentAchMerchant.AchMerchantClone.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED
                && this.MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED)))
            {
                this.AchDiscrtn.BorderColor = red;
                error = true;
            }

            //Niranjan: Removed validation of ACH/DD bank. Story-PXP-2772
           

            if (this.MerchantStatusUID.SelectedIndex == 0)
            {
                this.MerchantStatusUID.BackColor = red;
                error = true;
            }

            if (this.TransRoute.Text.Trim().Length == 0)
            {
                this.Master.AddMessageError("Deposit Trans Routing Number is required.");
                this.TransRoute.BorderColor = red;
                error = true;
            }
            //eluxa: disable routing# validation for now because we need to be able to enter Direct Debit and EFT routing numbers.
            else if (bankId >= 11 && bankId <= 19)
            {
                //eluxa 2015.06.03: only enable routing validation for Meritus ACH banks request by mnguyen. Meritus banks are
                //BankID	BankName
                //12	FIFTH THIRD
                //11	NCAL
                //13	KBT
                //14	FDR
                //15	CHASE
                //17	WELLS
                //18	MB FINANCIAL
                //19    FPS - Forte Payment Systems
                if (!AchTransaction.ValidateCheckDigit(TransRoute.Text.Trim()) || !data.ValidateTransRouteNumber(this.TransRoute.Text.Trim()))
                {
                    this.Master.AddMessageError("Deposit Trans Route is INVALID");
                    this.TransRoute.BorderColor = red;
                    error = true;
                }
            }

            if (this.TransRoute.Text.Trim().Length != 0 && TransRoute.Text == AccountNo.Text)
            {
                this.Master.AddMessageError("Routing Number and Account Number cannot be the same.");
                this.TransRoute.BorderColor = red;
                error = true;
            }

            ListItem itemTEL = ListHandler.GetListItem(SeccList, "TEL");

            //2015-04-09 per lucy's request: allow TEL on MB Financial Bank
            if ((Secc.SelectedItem.Value == "TEL" || itemTEL.Selected) &&
                !(BankID.SelectedItem.Value == "15" || BankID.SelectedItem.Value == "18" || BankID.SelectedItem.Value == "19"))
            {
                this.Master.AddMessageError("TEL is only available on Chase, MB Financial and Forte Payment Systems");
                this.Secc.BorderColor = red;
                error = true;
            }

            //eluxa: disable routing# validation for now because we need to be able to enter Direct Debit and EFT
            //routing numbers
            //if (WithdrawTR.Text.Trim().Length > 0 && !AchTransaction.ValidateCheckDigit(WithdrawTR.Text.Trim()))
            //{
            //    this.Master.AddMessageError("Withdraw Tran Route is INVALID");
            //    this.WithdrawTR.BorderColor = red;
            //    error = true;
            //}                       

            //eluxa: disable routing# validation for now because we need to be able to enter Direct Debit and EFT
            //routing numbers
            //if (DSCFeeTransRoute.Text.Trim().Length > 0 && !AchTransaction.ValidateCheckDigit(DSCFeeTransRoute.Text.Trim()))
            //{
            //    this.Master.AddMessageError("DSC Fee Trans Route is INVALID");
            //    this.DSCFeeTransRoute.BorderColor = red;
            //    error = true;
            //}

            //Niranjan: Removed validation of NAICS. Story-PXP-2772

            if (this.AccountNo.Text.Trim().Length == 0)
            {
                this.Master.AddMessageError("Account Number is INVALID.");
                this.AccountNo.BorderColor = red;
                error = true;
            }
            //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
            else if (this.AccountType.SelectedIndex == 0 && (UserSessions.CurrentAchMerchant == null || !(UserSessions.CurrentAchMerchant.AchMerchantClone != null && UserSessions.CurrentAchMerchant.AchMerchantClone.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && this.MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED)))
            {
                this.Master.AddMessageError("Select AccountType");
                this.AccountType.BorderColor = red;
                error = true;

            }

            if (this.dfiAcctName.Text.Trim().Length == 0)
            {
                this.dfiAcctName.BorderColor = red;
                error = true;
            }

            if (this.MerchantTypeCode.SelectedIndex == 0)
            {
                this.MerchantTypeCode.BackColor = red;
                error = true;
            }

            if (this.Test.SelectedIndex == 0)
            {
                this.Test.BackColor = red;
                error = true;
            }
        }
        else
        {
            //Niranjan: Removed validation of ACH/DD bank. Story-PXP-2772
            //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
            if (this.AchDescrp.Text.Trim().Length == 0 && (UserSessions.CurrentAchMerchant == null || !(UserSessions.CurrentAchMerchant.AchMerchantClone != null && UserSessions.CurrentAchMerchant.AchMerchantClone.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && this.MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED)))
            {
                this.AchDescrp.BorderColor = red;
                error = true;
            }

            if (this.AchCoName.Text.Trim().Length == 0)
            {
                this.AchCoName.BorderColor = red;
                error = true;
            }
            if (this.AccountNo.Text.Trim().Length == 0)
            {
                this.AccountNo.BorderColor = red;
                error = true;
            }
            //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
            else if (this.AccountType.SelectedIndex == 0 && (UserSessions.CurrentAchMerchant == null || !(UserSessions.CurrentAchMerchant.AchMerchantClone != null && UserSessions.CurrentAchMerchant.AchMerchantClone.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && this.MerchantStatusUID.SelectedValue.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED)))
            {                                
                this.Master.AddMessageError("Select AccountType");
                this.AccountType.BorderColor = red;
                error = true;
            }
        }

        if (bankId >= 11 && bankId <= 19)
        {
            //SECC is only applicable if the user selects a US bank. the if statement 
            //above is totally cheesy but it works
            if (this.Secc.SelectedIndex == 0)
            {
                this.Secc.BackColor = red;
                error = true;
            }
        }

        if ((String.IsNullOrWhiteSpace(this.BusinessWebsiteACH.Text)) && (this.Secc.SelectedItem.Value == "WEB"))
        {
            this.Master.AddMessageError("Need to populate Website");
            this.BusinessWebsiteACH.BorderColor = red;
            error = true;
        }
        //if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY && !this.Adding)


        try
        {
            //Fmassoud 2017.08.23 - ACH Validation when changing Status to CU Recieved (to be handled inside MerchantDataCheck         
            AchMerchant _ach = new AchMerchant();
            string _NewStatusUID = String.IsNullOrEmpty(this.MerchantStatusUID.SelectedValue) ? "" : this.MerchantStatusUID.SelectedValue.ToUpper();
            
            if (ACH_ID > 0 && !this.Adding)
            {
                _ach = UserSessions.CurrentAchMerchant;
                if (_ach == null)
                    _ach = UserSessions.ActiveAchMerchant;
                _ach.MonthlyVolume = this.MonthlyVolume.ValueDecimal;
                _ach.AverageTicket = this.AverageTicket.ValueDecimal;
                _ach.HighTicket = this.HighTicket.ValueDecimal;
                //Niranjan: PXP-2773 Require ACH/DD Bank to be selected by Credit Underwriting Agent
                _ach.AccountType = AccountType.SelectedValue;
                _ach.BankID = int.Parse(BankID.SelectedValue);
                _ach.NAICS = NAICS.Text;
                _ach.AchDescrp = AchDescrp.Text;
                _ach.AchDiscrtn = AchDiscrtn.Text;
                _ach.DateUpdated = DateTime.Now;
                _ach.UpdatedBy = UserSessions.CurrentUser.UserName;
                _ach.FromACH = true;               
                
                if (!String.IsNullOrEmpty(_NewStatusUID))
                    _ach.MerchantStatusUID = _NewStatusUID;

                IList<string> message = FormHandler.MerchantDataCheck(UserSessions.CurrentMerchantApp, false, false, _NewStatusUID, _ach); //  );

                if (message.Count > 0)
                {
                    foreach (string mess in message)
                        this.Master.AddMessageError(mess);

                    error = true;
                }
            }
            //code added for PXP-4990 by koshlendra start
            //PXP-10208: BBVA Bank By Sanidhya
            if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST 
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB 
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS)) 
                && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
            {
                IList<string> errMessage = new List<string>();
                decimal totalACHTransCompleted = Convert.ToDecimal(TinfoWrittenContractPercent.Value) + Convert.ToDecimal(TinfoInternetInitiatedPercent.Value) + Convert.ToDecimal(TinfoTelephoneInitiatedPercent.Value);
                if (totalACHTransCompleted !=0  && totalACHTransCompleted != 100)
                {
                    //this.Master.AddMessageError("Sum of total transaction completed should be equal to 100.");
                    this.txtTotalACHTransCompleted.Value = totalACHTransCompleted;
                    errMessage.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_TotalACHTransCompleted));
                    this.txtTotalACHTransCompleted.BorderColor = red;

                }

                decimal totalAchProductSold = Convert.ToDecimal(TinfoConsumerPercent.Value) + Convert.ToDecimal(TinfoBusinessPercent.Value);
                if (totalAchProductSold !=0 && totalAchProductSold != 100)
                {
                    //this.Master.AddMessageError("Sum of total product sold should be equal to 100.");
                    txtTotalACHProductSold.Value = totalAchProductSold;
                    errMessage.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_TotalProductSold));
                    this.txtTotalACHProductSold.BorderColor = red;
                }

                if (Convert.ToDecimal(TinfoInternetInitiatedPercent.Value) > 1)
                {
                   
                    if(!string.IsNullOrWhiteSpace(TinfoPaymentPageURL.Text))
                    {
                        if (!(Regex.IsMatch(TinfoPaymentPageURL.Text.Trim(), Constants.UrlRegex)))
                        {
                            errMessage.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_PaymentPageURL));
                            this.TinfoPaymentPageURL.BorderColor = red;
                        }
                    }
                    else
                    {
                        errMessage.Add(Constants.GetDescription(Constants.ErrorCodes.ACH_PaymentPageURLRequired));
                        this.TinfoPaymentPageURL.BorderColor = red;
                    }
                    
                    

                    
                }
                

                if (errMessage.Count > 0)
                {
                    foreach (string msg in errMessage)
                        this.Master.AddMessageError(msg);

                    error = true;
                }

            }
       
            //code added for PXP-4990 by koshlendra end
        
        }
        catch
        {
            this.Master.AddMessageError(" Error Exception Happened ");
            error = true;
        }

        // else call 3DE

        return !error;
    }

    public override void FormCancel()
    {

        this.EditMode = false;
        this.FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;

        this.ResetValidationIndicators();

        this.Master.ToggleMenu(!this.EditMode);
    }

    private void CloseForm()
    {
        string url = Request.QueryString["PostBackURL"].ToString();

        if (!string.IsNullOrWhiteSpace(url))
            Response.Redirect(url);
        else
            Response.Redirect("frmMerchantSearch.aspx");
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {

            case "Save":
                if (this.FormDataCheck())  // valid input values.
                {
                    AchMerchant objACH = this.FormSaveACH();
                    if (objACH != null)
                    {
                        this.Adding = false;
                        this.EditMode = false;
                        url = "~/SecureMerchantManagementForms/frmMerchantACH.aspx?Adding=false";
                        url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                        url += "&AchID=" + objACH.AchID.ToString();
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
                if (string.IsNullOrWhiteSpace(this.AchID.Text) || this.AchID.Text.Trim() == "0")
                    Response.Redirect("~/SecureMerchantManagementForms/frmMerchantACH.aspx?Adding=False&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);
                else
                    this.FormCancel();
                break;

            case "Edit":
                {                    
                    this.EditMode = true;                   
                    if (UserSessions.CurrentMerchantApp.AchID <= 0)
                    {
                        this.FormNew();
                    }
                    else
                    {
                        this.FormShow(this.UID);
                    }
                    //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message//Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
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

        }
    }

    protected void TransRoute_TextChanged(object sender, EventArgs e)
    {
        if ((UserSessions.CurrentAchMerchant != null && TransRoute.Text != UserSessions.CurrentAchMerchant.TransRoute) || (this.Adding && !string.IsNullOrWhiteSpace(TransRoute.Text)))
        {
            ((TextBox)confirm.FindControl("txtRoute")).Visible = true;
            ((Label)confirm.FindControl("lblRoute")).Visible = true;
            ((TextBox)confirm.FindControl("txtRoute")).Focus();
            isValid = false;
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
    }

    protected void AccountNo_TextChanged(object sender, EventArgs e)
    {


        if ((UserSessions.CurrentAchMerchant != null && AccountNo.Text != UserSessions.CurrentAchMerchant.AccountNo) || (this.Adding && !string.IsNullOrWhiteSpace(AccountNo.Text)))
        {
            ((TextBox)confirm.FindControl("txtDDA")).Visible = true;
            ((Label)confirm.FindControl("lblDDA")).Visible = true;
            ((TextBox)confirm.FindControl("txtDDA")).Focus();
            isValid = false;
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

        }

    }

    void confirm_ButtonClick(object sender, EventArgs e)
    {
        string url = string.Empty, message = string.Empty;
        bool count = true;
        AchMerchant app = UserSessions.CurrentAchMerchant;

        if (((TextBox)confirm.FindControl("txtDDA")).Visible && ((TextBox)confirm.FindControl("txtDDA")).Text != AccountNo.Text)
        {
            count = false;
            message += "Account Number does not match.";
            AccountNo.BorderColor = System.Drawing.Color.Red;
            AccountNo.Text = (app != null) ? app.AccountNo : string.Empty;
        }
        if (((TextBox)confirm.FindControl("txtRoute")).Visible && ((TextBox)confirm.FindControl("txtRoute")).Text != TransRoute.Text)
        {
            count = false;
            message += "Routing Number does not match.";
            TransRoute.BorderColor = System.Drawing.Color.Red;
            TransRoute.Text = (app != null) ? app.TransRoute : string.Empty;
        }

        if (!count)
            this.Master.AddMessageError(message);

        confirm.SetValue(false);
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }


    private void ResetValidationIndicators()
    {
        //resets all visual indicators for validation errors back to default state

        //backgrounds
        this.Secc.BackColor = Color.White;
        //Niranjan: Removed validation of ACH/DD bank. Story-PXP-2772
       this.MerchantStatusUID.BackColor = Color.White;
        this.MerchantTypeCode.BackColor = Color.White;
        this.Test.BackColor = Color.White;

        //borders
        this.AccountType.BorderColor = Color.FromArgb(173, 195, 222);
        this.AchDescrp.BorderColor = Color.FromArgb(173, 195, 222);
        this.AchDiscrtn.BorderColor = Color.FromArgb(173, 195, 222);
        this.Secc.BorderColor = Color.FromArgb(173, 195, 222);
        this.TransRoute.BorderColor = Color.FromArgb(173, 195, 222);
        //Niranjan: Removed validation of NAICS. Story-PXP-2772
        this.AccountNo.BorderColor = Color.FromArgb(173, 195, 222);
        this.dfiAcctName.BorderColor = Color.FromArgb(173, 195, 222);
        this.WithdrawTR.BorderColor = Color.FromArgb(173, 195, 222);
        this.DSCFeeTransRoute.BorderColor = Color.FromArgb(173, 195, 222);
        this.BusinessWebsiteACH.BorderColor = Color.FromArgb(173, 195, 222);
        //code added by koshlendra for PXP-4990 start
        this.TinfoPaymentPageURL.BorderColor=Color.FromArgb(173, 195, 222);
        this.txtTotalACHProductSold.BorderColor=Color.FromArgb(173, 195, 222);
        this.txtTotalACHTransCompleted.BorderColor = Color.FromArgb(173, 195, 222);        
        //code added by koshlendra for PXP-4990 end
    }
    protected void Secc_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWebsite();
    }
    private void LoadWebsite()
    {
        if (this.Secc.SelectedItem.Value.Trim().ToUpper().Equals("WEB"))
        {
            BusinessWebsiteACH.Visible = true;
            lblwebsite.Visible = true;
            BusinessWebsiteACH.Text = UserSessions.CurrentMerchantApp.BusinessWebsite;
        }
        else
        {
            BusinessWebsiteACH.Visible = false;
            lblwebsite.Visible = false;

        }
    }

    //code added by koshlendra for PXP-4990 start
    protected void TinfoInternetInitiatedPercent_ValueChanged(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
    {
        if (Convert.ToDecimal(TinfoInternetInitiatedPercent.Value) > 1)
        {
            trURLInternetPage.Visible = true;
            txtTotalACHTransCompleted.Value = Convert.ToDouble(TinfoWrittenContractPercent.Value) + Convert.ToDouble(TinfoInternetInitiatedPercent.Value) + Convert.ToDouble(TinfoTelephoneInitiatedPercent.Value);
            txtTotalACHProductSold.Value = Convert.ToDouble(TinfoConsumerPercent.Value) + Convert.ToDouble(TinfoBusinessPercent.Value);
              
        }
        else 
        {
            trURLInternetPage.Visible = false;
            TinfoPaymentPageURL.Text = "";
            txtTotalACHTransCompleted.Value = Convert.ToDouble(TinfoWrittenContractPercent.Value) + Convert.ToDouble(TinfoInternetInitiatedPercent.Value) + Convert.ToDouble(TinfoTelephoneInitiatedPercent.Value);
            txtTotalACHProductSold.Value = Convert.ToDouble(TinfoConsumerPercent.Value) + Convert.ToDouble(TinfoBusinessPercent.Value);
              
        }
    }
    //code added by koshlendra for PXP-4990 end
}
