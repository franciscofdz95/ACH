using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;

public partial class frmUnderwritingPending : frmBaseDataEntry
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

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Pending);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Pending");
            }

            WucBusinessInfo1.pnlInfo.Enabled = false;

            if (Request["UID"] != null)
                UID = Request["UID"].ToString();
            else
                UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

            this.FormShow(this.UID);
        }
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
                    this.Adding = false;
                    this.ToggleButtons();
                    url = "~/SecureMerchantManagementForms/frmUnderWritingPending.aspx?Adding=false";
                    url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                Conditions.LoadConditions();
                break;
            case "Refresh":
                MerchantFacade facade = new MerchantFacade();
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                this.FormShow(this.UID);
                break;
            case "Cancel":
                this.FormCancel();

                break;

            case "Close":
                break;

            case "Delete":
                break;

            case "Edit":
                {
                    this.EditMode = true;
                    this.FormShow(this.UID);
                    //Modofied by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
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

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);

        DataMerchantApp objMerchant = new DataMerchantApp();
        bool Validation = objMerchant.GMAValidationCheck(UserSessions.CurrentUser, agreement);

        if (!Validation)
        {
            HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
        }
        
        AchMerchant achmerchant = null;

        string m_StautsUID = string.Empty, m_StatusName = string.Empty;

        if (isACHonly)
        {
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));

            if (UserSessions.ActiveAchMerchant != null)
            {
                achmerchant = UserSessions.ActiveAchMerchant;
                LookupTableHandler.MerchantAppStatus(ACHStatusUID, false, "Merchant Management", agreement, achmerchant);
                ListHandler.ListFindItem(ACHStatusUID, achmerchant.MerchantStatusUID);
                m_StautsUID = achmerchant.MerchantStatusUID;
                m_StatusName = achmerchant.MerchantStatusName;
            }

            ACHStatus.Visible = true;
            CCStatus.Visible = false;
        }
        else
        {
            LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management", agreement.StatusName.Substring(0, 2), agreement);
            ListHandler.ListFindItem(StatusUID, agreement.StatusUID);

            m_StautsUID = agreement.StatusUID;
            m_StatusName = agreement.StatusName;

            ACHStatus.Visible = false;
            CCStatus.Visible = true;
        }

        // Added by Chandra for PXP-7898
        hidStatus.Value = m_StautsUID.ToUpper();
        IsNutra.Value = (!agreement.IsNutraMerchant && agreement.SicCode == "5968").ToString().ToUpper();
        //PXP-9348 RThakur
        hidIsNutraMerchant.Value = (agreement.IsNutraMerchant && agreement.SicCode == "5968").ToString().ToUpper();

        FormHandler.SetControlEditMode(pnlDetail, true);

        agreement.DelaysApproved = agreement.DelaysApproved.Equals("-1") ? string.Empty : agreement.DelaysApproved;
        agreement.ReleaseMethodUID = agreement.ReleaseMethodUID.Equals("") ? "-1" : agreement.ReleaseMethodUID;

        FormBinding.BindObjectToControls(agreement, pnlDetail);

        Conditions.isACHOnly = isACHonly;
        Conditions.isEdit = ((LinkButton)Conditions.FindControl("lnkAdd")).Enabled = this.EditMode;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_AGENT_RELATIONS.ToUpper()) // Role is RELATIONSHIP MANAGEMENT
        {
            Conditions.setValues(2);
        }
        else
        {
            Conditions.setValues(1);
            pnlStatus.Visible = true;
        }

        //PXP-9348 RThakur start
        hiddenCrmStatus.Value = agreement.CRMStatus;
        hiddenAcceptTransaction.Value = Convert.ToString(agreement.CRMAcceptTransactions);
        //PXP-9348 RThakur end

        WucBusinessInfo1.LoadOffice(agreement);

        if (m_StatusName.Substring(0, 2) != "SS") //"AP"); RM Queue is now changed to SS
            ListHandler.ListFindItem(((DropDownList)Conditions.FindControl("cboType")), "CU");

        Conditions.LoadConditions();

        Conditions.Enable(true);

        MasterPageMerchant master = (MasterPageMerchant)this.Master;
        master.ShowNotes(agreement.UWNotes, agreement.AgentMemo, agreement.FirstTeamNotes, this.EditMode);

        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {           
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }
        /******** End of PXP-2206 **************/

        //PXP-9051 RThakur
        if (!FormHandler.SetHdnValueIsAnyVerticalMarketsChecked(m_StatusName, m_StautsUID, agreement.IsNewVertical))
        {
            IsNewVerticalandMarkets.Value = "false";
            IsNewVerticalNew.Value = agreement.IsNewVertical ? "True" : "False";
        }
        else
        {
            IsNewVerticalandMarkets.Value = "true";
            IsNewVerticalNew.Value = agreement.IsNewVertical ? "True" : "False";
        }
    }

    public override bool FormSave()
    {
        bool perform = false;
        DataSet dsmultiLink = new DataSet();

        try
        {
            MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
            agreement.CloneMerchantApp();

            FormBinding.BindControlsToObject(agreement, pnlDetail);
            string m_StatusUID = string.Empty, m_statusName = string.Empty, m_ClonestatusName = string.Empty;
            string StatusUIDClone = string.Empty;
            if (!isACHonly)
            {
                m_StatusUID = agreement.StatusUID = StatusUID.SelectedItem.Value;
                m_statusName = agreement.StatusName = StatusUID.SelectedItem.Text;
                m_ClonestatusName = agreement.MerchantAppClone.StatusName;
                StatusUIDClone = agreement.MerchantAppClone.StatusUID;
            }
            //for session management of CRM Status,Count
            //PXP-8409 by Sanidhya
            if (!this.Adding)
            {
                MerchantFacade merchantFacade = new MerchantFacade();
                MerchantApp currntMerchantApp = merchantFacade.GetMerchantAppZeus(agreement.MerchantAppUID);
                agreement.CRMStatus = currntMerchantApp.CRMStatus;
                agreement.CRMCount = currntMerchantApp.CRMCount;
                agreement.CRMAcceptTransactions = currntMerchantApp.CRMAcceptTransactions;
            }

            UserSessions.CurrentMerchantApp = agreement;
            AchMerchant achMerchant = null;

            if (UserSessions.ActiveAchMerchant != null && isACHonly)
            {
                UserSessions.ActiveAchMerchant.CloneAchMerchant();
                achMerchant = UserSessions.ActiveAchMerchant;
                achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;
                m_StatusUID = achMerchant.MerchantStatusUID ;//= ACHStatusUID.SelectedValue;
                m_statusName = achMerchant.MerchantStatusName ;//= ACHStatusUID.SelectedItem.Text;
                m_ClonestatusName = ACHStatusUID.SelectedItem.Text;// achMerchant.AchMerchantClone.MerchantStatusName;
            }

            if (!this.FormDataCheck())
                return false;

            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            User user = UserSessions.CurrentUser;
            agreement.UserUpdated = user.UserName;

            agreement.UWNotes = ((TextBox)this.Master.FindControl("UWNotesEdit")).Text;
            agreement.FirstTeamNotes = ((TextBox)this.Master.FindControl("FirstTeamNotesEdit")).Text;


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

            //Start code by Anuj for PXP-9311 
            if (underWriting != null)
            {
                if (agreement.IsNutraMerchant)
                {
                    //Code changes done for PXP-15968 with decode the string by koshlendra start
                    if (!(System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Nutra Trial' account") || System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues).Contains("'Tangible Trial' account")))
                    {
                        StringBuilder uwIssue = new StringBuilder();
                        uwIssue.AppendLine(System.Web.HttpUtility.HtmlDecode(underWriting.UWIssues));
                        uwIssue.AppendLine(" ");
                        // PXP-12436: Start by Rohit Thakur
                        if (agreement.Office == CommonUtility.Util.Offices.Irvine)
                        {
                            
                                uwIssue.AppendLine("'Tangible Trial' account");
                        }
                        else
                        {
                           
                                uwIssue.AppendLine("'Nutra Trial' account");
                        }
                        // PXP-12436: End by Rohit Thakur
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
                    //Code changes done for PXP-15968 with decode the string by koshlendra end
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
            MerchantFacade facade = new MerchantFacade();
            int rows = facade.UpdateMerchantApp(agreement);

            if (rows > 0)
            {
                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);

                string origStatus = string.Empty;

                if (isACHonly && achMerchant != null)
                {
                    DataAccess.DataAchMerchantDao.UpdateAchMerchant(achMerchant);

                    if (achMerchant.AchMerchantClone != null)
                        origStatus = achMerchant.AchMerchantClone.MerchantStatusUID;

                    UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(UserSessions.CurrentMerchantApp.ID));
                }
                else if (agreement.MerchantAppClone != null)
                {
                    origStatus = agreement.MerchantAppClone.StatusUID;
                }

                //PXP-7620 RThakur
                if (Conditions.PendingNote.Count > 0)
                {
                    FormHandler.CompleteApplication(agreement, achMerchant, origStatus, user.UserName, false);
                }
                else
                {
                    FormHandler.CompleteApplication(agreement, achMerchant, origStatus, user.UserName);

                }
                Conditions.StatusName = m_ClonestatusName;

                if (m_statusName.Substring(0, 2) != "AW")
                {
                    Conditions.FormSave();
                }

                //PXP-9750 Rohit Thakur >> Start
                //Create new ticket for ‘Mastercard De-registration process
                string NewStatus = isACHonly ? ACHStatusUID.SelectedValue.ToUpper() : StatusUID.SelectedValue.ToUpper();
                if (!isACHonly && NewStatus == Constants.QUEUESTATUS_MS_CANCELLATION && agreement.HighRiskRegistered && NewStatus != origStatus.ToUpper())
                {
                    FormHandler.AddTicketForMastercard(agreement, "i", "3", "2251", "2252", "3-Low", "6");
                }
                //PXP-9750 Rohit Thakur >> End

            }
             
            perform = true;
        }
        catch (Exception exc)
        {
            throw exc;
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

    public override bool FormDataCheck()
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;        
        AchMerchant achmerchant = UserSessions.ActiveAchMerchant;

        //Fmassoud 2017.08.28 Sending New Status to Formhandler        
        string NewStatus = isACHonly ? ACHStatusUID.SelectedValue.ToUpper() : StatusUID.SelectedValue.ToUpper();
        IList<string> message = FormHandler.MerchantDataCheck(app, false, this.Adding, NewStatus, UserSessions.ActiveAchMerchant);
        //PXP-8409 Sanidhya:Start
        IList<string> _infoMsg = FormHandler.ValidateCRMFlow(app, false, UserSessions.ActiveAchMerchant);
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

        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            
            if (!isACHonly)
            {
                if (app.MerchantAppClone != null)
                {
                    ListHandler.ListFindItem(StatusUID, app.MerchantAppClone.StatusUID);
                    app.StatusUID = app.MerchantAppClone.StatusUID;
                }
            }
            else
            {
                if (achmerchant != null && achmerchant.AchMerchantClone != null)
                {
                    ListHandler.ListFindItem(ACHStatusUID, achmerchant.AchMerchantClone.MerchantStatusUID);
                    achmerchant.MerchantStatusUID = achmerchant.AchMerchantClone.MerchantStatusUID;
                }
            }

            return false;
        }
    }

    public override void FormCancel()
    {
        ((TextBox)Conditions.FindControl("txtEmailAdd")).Text = "";
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;

        this.Master.ToggleMenu(!this.EditMode);
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }
}
