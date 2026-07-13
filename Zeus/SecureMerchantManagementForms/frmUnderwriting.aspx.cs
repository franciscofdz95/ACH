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
using Infragistics.Web.UI.EditorControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Text.RegularExpressions;
using System.Text;
using Infragistics.Web.UI.LayoutControls;
using System.IO;
using System.Net;
using PaymentXP.BusinessObjects.Request;
using PaymentXP.BusinessObjects.Reponse;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Serialization;
using Winnovative;
using CommonUtility;
using Infragistics.WebUI.WebHtmlEditor;
using Infragistics.WebUI.WebSchedule;
using System.Globalization;
using System.Linq;
using PaymentXP.BusinessObjects.Zeus;
using ZeusWeb.Class;
using Paysafe.Zeus3DE.Model;
using Zeus3DERestService.Model;
using ZeusWeb;
using System.Web.Configuration;

public partial class frmUnderwriting : frmBaseDataEntry
{

    public string ADMIN_ROLE = "0C3E6ADC-418C-4A00-90F2-4C559D378E63";
    public string SUBJECT_WITHDRAWN = "Underwriting - Withdrawn";
    public string SUBJECT_DECLINED = "Underwriting - Declined";


    List<UWHeirarchyApprovalLimit> m_lstHeirarchyapproval;
    Lookup<int, string> m_lookupHeirarchyApprovalUsers;

    bool isACHonly
    {
        set { WucBusinessInfo1.isACHonly = value; }
        get { return WucBusinessInfo1.isACHonly; }

    }

    bool isIrvineMerchant
    {
        get
        {
            if (UserSessions.CurrentMerchantApp != null)
                return UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine;
            else
                return false;
        }
    }

    bool IsWoodForestOnlyApp
    {
        get
        {
            //code changes for PXP-10232 by koshlendra
            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
                return true;
            else
                return false;
        }
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
        //CashAdvance.ButtonClick += new wucCashAdvance.ButtonClickHandler(CashAdvance_ButtonClick);
        base.OnInit(e);
    }

    public bool isGuaranty
    {
        set { ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "isGuaranty"] = value; }
        get
        {
            if (ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "isGuaranty"] != null)
                return Convert.ToBoolean(ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "isGuaranty"]);
            else return false;
        }
    }

    public bool isRequest
    {
        set { ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "isRequest"] = value; }
        get
        {
            if (ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "isRequest"] != null)
                return Convert.ToBoolean(ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "isRequest"]);
            else return false;
        }
    }


    void CashAdvance_ButtonClick(object sender, EventArgs e)
    {
        CashAdvance.pnlAdv.Visible = (CashAdvance.grdcount > 0);
    }



    protected void Page_Load(object sender, EventArgs e)
    {

        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        string decrypted = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ6ZXVzIiwidXNlcl9uYW1lIjoiemV1cyIsImFwcE5hbWUiOiJaZXVzQmFja2VuZCIsImlzcyI6IlBheXNhZmUgUExDIiwic2Vzc2lvbklkIjoiWmV1c1Nlc3Npb25JZCIsImF1dGhvcml0aWVzIjpbIkJPX01PQl9VU0FDUV9aZXVzX1N5c3RlbSJdLCJjbGllbnRfaWQiOiI5NjQ5Mjk3MjgyZDBlNjM3YjRiNjQwOTJkODAwZjFkNjU3ZmNhZDVjIiwic2NvcGUiOlsibWVyY2hhbnRPbmJvYXJkaW5nIl0sImV4cCI6MTYyMjk2MTA1OCwiaWF0IjoxNTU5ODQ3MjA2LCJicmFuZCI6IlBheXNhZmUiLCJqdGkiOiIzYTk2MDk2NC1iYmQxLTRlZTUtOWRhNy02ZGQ2MzAxYzg1NGEiLCJhdXRoZW50aWNhdGlvbkdyb3VwIjpudWxsfQ.DFEYIYwAls-az4Rdr1Iw4w_hXAxbZXSVOoSLlRq41Ko8qsjQOAdUAQGpfIHwZBvSlyJuyvJYjq1xhf-QSgoZoE2FFM64mJwy2sZNDnGg4fbFL_dSIl0397p0PX84ZcFWkVBXP-zMgIKfm1EuVRaiEGJLItLdo_DX4UmeepDJQpFRNoEplj5jcJAUT9dcBtsH_CWtUfuy6PmXzFUlMuLl4p0qbcbXJgKflVi9ZPOOtPBL7Gcdqg-kF_2UDQVRdxYWz9kz59XZGzb1AKL958CvMriMY0vFkjEi0i4e9-B4gmxdBvob6SGWiNW-fV9Ch5NySsEqtHiPENXFI9hBRn9xbg";
        Crypto crypto = new Crypto();

        string encrypted = crypto.Encrypt(decrypted);

        if (UserSessions.CurrentMerchantApp.Bank.Equals("Woodforest"))
            if (IsWoodForestOnlyApp)
            {
                WucOwnerUW5.Visible = false;
                wucCorpBuzUW1.Visible = true;
            }
            else
            {
                WucOwnerUW5.Visible = true;
                wucCorpBuzUW1.Visible = false;
            }
        m_lstHeirarchyapproval = LookupTableHandler.LoadUWHeirarchyApprovalLimit();

        CashAdvance.ButtonClick += new wucCashAdvance.ButtonClickHandler(CashAdvance_ButtonClick);
        MerchantScoreCards.GridViewCommand += MerchantScoreCards_GridViewCommand;
        FinancialScoreCardGrid.ButtonClick += FinancialScoreCardGrid_ButtonClick;

        DelaysApproved.Attributes.Add("onKeyPress", "return CheckNumeric();");
        WebUtil.SetUserSpecificDisplayMode(ResIncorporationDate);
        WebUtil.SetUserSpecificDisplayMode(ConditionalDueDate);
        WebUtil.SetUserSpecificDisplayMode(BusinessStartDate);
        WebUtil.SetUserSpecificDisplayMode(PhysicalVisitOn);

        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Credit);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Credit");
            }
            //PXP-4979 by koshlendra start
            //code changes for PXP-10232 by koshlendra
            if (UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && (
                UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
            {
                ((CheckBox)WucOwnerUW0.FindControl("AuthorizedSignature")).Text = "Controller";
                ((CheckBox)WucOwnerUW1.FindControl("AuthorizedSignature")).Text = "Controller";
                ((CheckBox)WucOwnerUW2.FindControl("AuthorizedSignature")).Text = "Controller";
                ((CheckBox)WucOwnerUW3.FindControl("AuthorizedSignature")).Text = "Controller";
                ((CheckBox)WucOwnerUW4.FindControl("AuthorizedSignature")).Text = "Controller";

                // PXP-6467 Fady Massoud 08/04/2018
                for (int i = 0; i < 6; i++)
                {
                    ((CheckBox)pnlCredit.FindControl(("WucOwnerUW" + i)).FindControl("CBRWaived")).Text = "T&C Signed (Run CBR)";
                }

            }
            else
            {
                ((CheckBox)WucOwnerUW0.FindControl("CBRWaived")).Visible = false;
                ((CheckBox)WucOwnerUW1.FindControl("CBRWaived")).Visible = false;
                ((CheckBox)WucOwnerUW2.FindControl("CBRWaived")).Visible = false;
                ((CheckBox)WucOwnerUW3.FindControl("CBRWaived")).Visible = false;
                ((CheckBox)WucOwnerUW4.FindControl("CBRWaived")).Visible = false;
                ((CheckBox)WucOwnerUW0.FindControl("CBRWaived")).Checked = false;
                ((CheckBox)WucOwnerUW1.FindControl("CBRWaived")).Checked = false;
                ((CheckBox)WucOwnerUW2.FindControl("CBRWaived")).Checked = false;
                ((CheckBox)WucOwnerUW3.FindControl("CBRWaived")).Checked = false;
                ((CheckBox)WucOwnerUW4.FindControl("CBRWaived")).Checked = false;
                ((CheckBox)WucOwnerUW5.FindControl("CBRWaived")).Visible = false;
                ((CheckBox)WucOwnerUW5.FindControl("CBRWaived")).Checked = false;
            }
            //PXP-4979 by koshlendra end

            WucOwnerUW0.SetTitle = "Owner 1";
            WucOwnerUW1.SetTitle = "Owner 2";
            WucOwnerUW2.SetTitle = "Owner 3";
            WucOwnerUW3.SetTitle = "Owner 4";
            //PXP-3118 Rohit Thakur
            WucOwnerUW4.SetTitle = "Owner 5";
            WucOwnerUW5.SetTitle = "Owner 6";
            ConditionalDueDate.Value = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            //Amit Patne : PXP-3818 - Zeus: Vertical Market and Billing Method
            LookupTableHandler.LoadVerticalMarketValues(VerticalMarket, BillingTypes, MarketingMethods);
            LookupTableHandler.LoadAgentLevels(this.AgentLevel, false, UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper());

            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BMO_HARRIS)
                Level.Text = "Association Number:";
            else
                Level.Text = "Agent Level:";

            if (Request["MerchantAppUID"] != null)
                UID = Request["MerchantAppUID"].ToString();
            else
                UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

            this.PageSize = 5;
            this.CurrentPage = 1;

            this.FormShow(this.UID);
            ConditionalApproval.Attributes.Add("onclick", "return ChangeDate()");
            PhysicalSiteVisit.Attributes.Add("onclick", "return ChangePhysicalVisit()");
            ResOFACMatch.Attributes.Add("onclick", "return ChangeOFAC()");
            CashAdvance.pnlAdv.Visible = (CashAdvance.grdcount > 0);

            if (UserSessions.CurrentUser.IsBank)
            {
                CashAdvance.Visible = false;
                pnlMultipleLinks.Visible = false;
            }

            GetRecent3DEStatus(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));

        }
         
        phAgentlevel.Visible = !(UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BMO_HARRIS);
        phAssociationNumber.Visible = UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BMO_HARRIS;
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
                //Reverted Conditional Approval part
                //if (this.FormDataCheck())
                //{
                if (this.FormSave())
                {
                    this.EditMode = false;
                    this.Adding = false;
                    this.ToggleButtons();
                    //Generate_Upload_PDF(false, true);

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

                    url = "~/SecureMerchantManagementForms/frmUnderWriting.aspx?Adding=false";
                    url += "&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
                    Response.Redirect(url);
                }
                //}

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
                if (this.FormDelete())
                    Response.Redirect("frmLeads.aspx");
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
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchMCC();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtMCC.Text = string.Empty;
        txtMCCDesc.Text = string.Empty;
        grd.DataSource = null;
        grd.DataBind();
        lblNoRecords.Visible = true;
    }

    protected void btnRequest_Click(object sender, EventArgs e)
    {

        isRequest = true;
        MerchantApp app = UserSessions.CurrentMerchantApp;
        AchMerchant achApp = UserSessions.ActiveAchMerchant;

        string to = string.Empty;
        StringBuilder note = new StringBuilder();
        decimal vol = app.TinfoAverageMonthlyVMCVolume;

        app.CloneMerchantApp();

        List<Owner> ownerc = new List<Owner>();
        for (int i = 0; i < app.Owners.Count; i++)
        {
            app.Owners[i].CloneOwner();
            ownerc.Add(app.Owners[i].OwnerClone);
        }

        FormBinding.BindControlsToObject(app, pnlDetail);

        if (isACHonly && achApp != null)
        {
            achApp.MerchantStatusUID = Constants.QUEUESTATUS_CU_APPROVED;
            achApp.MerchantStatusName = ACHStatusUID.SelectedItem.Text;
        }
        else
        {
            app.StatusUID = Constants.QUEUESTATUS_CU_APPROVED;
            app.StatusName = StatusUID.SelectedItem.Text;
        }

        WebPercentEditor ReservePercent = (WebPercentEditor)WucBusinessInfo1.FindControl("ReservePercent");
        app.ReservePercent = Convert.ToDecimal(ReservePercent.Value);

        app.DelaysApproved = DelaysApproved.Text;

        DropDownList ReleaseMethodUID = (DropDownList)WucBusinessInfo1.FindControl("ReleaseMethodUID");
        app.ReleaseMethodUID = ReleaseMethodUID.SelectedValue;

        WebNumericEditor UpfrontReserve = (WebNumericEditor)WucBusinessInfo1.FindControl("UpfrontReserve");
        app.UpfrontReserve = Convert.ToDecimal(UpfrontReserve.Value);

        UserSessions.CurrentMerchantApp = app;

        if (!this.FormDataCheck())
        {
            isRequest = false;
            return;
        }
        else
        {
            DataUnderwritng data = DataAccess.DataUnderwritingDao;

            Underwriting objUW = new Underwriting();
            objUW = data.LoadMerchantUWNotes(app.MerchantAppUID);
            if (objUW != null)
            {
                objUW.MerchantAppUID = app.MerchantAppUID;
                objUW.ApprovalRequestedBy = UserSessions.CurrentUser.UserName;
                objUW.ApprovalRequestedOn = DateTime.Now;

                UWHeirarchyApprovalLimit CurrentHeirarchyapproval = this.m_lstHeirarchyapproval.Where(hm => hm.AMVLowerLimit <= vol && hm.AMVUpperLimit > vol && hm.IsMCCRestrictedIndustry == FormHandler.IsMCCRestricted(app.SicCode)).FirstOrDefault();

                if (CurrentHeirarchyapproval != null)
                {
                    note.Append("<p><b><span style='font-size:11.0pt;font-family:Century Gothic;'>ZID:</span></b><span style='font-size:11.0pt;font-family:Century Gothic;'> " + app.ID + "</span><br>");
                    note.Append("<p><b><span style='font-size:11.0pt;font-family:Century Gothic;'>OFFICE:</span></b><span style='font-size:11.0pt;font-family:Century Gothic;'> " + app.Office.ToString() + "</span><br>");
                    note.Append("<p><b><span style='font-size:11.0pt;font-family:Century Gothic;'>VOLUME REQUESTED:</span></b><span style='font-size:11.0pt;font-family:Century Gothic;'> " + String.Format("{0:c}", vol) + "</span><br>");
                    note.Append("<p><b><span style='font-size:11.0pt;font-family:Century Gothic;'>MCC:</span></b><span style='font-size:11.0pt;font-family:Century Gothic;'> " + app.SicCode + " Restricted:" + (CurrentHeirarchyapproval.IsMCCRestrictedIndustry == true ? "Yes" : "No") + "</span><br>");
                    note.Append("<p><b><span style='font-size:11.0pt;font-family:Century Gothic;'>APPROVAL REQUIRED:</span></b><span style='font-size:11.0pt;font-family:Century Gothic;'> " + CurrentHeirarchyapproval.Role + "(" + String.Format("{0:c0}", CurrentHeirarchyapproval.AMVLowerLimit, 0) + "-" + (CurrentHeirarchyapproval.AMVUpperLimit > 99999999999 ? "Max Amount" : String.Format("{0:c0}", CurrentHeirarchyapproval.AMVUpperLimit)) + ")</span><br>");
                    note.Append("</br>");
                    note.Append("This is to inform you that this account is ready for approval. Due to the requested processing volume of this account, additional approval is required from you. Please log in to Zeus to complete the approval process. Review the steps below.");
                    note.Append("<ol><li>Login to Zeus. https://zeus.paysafe.com/ </li>");
                    note.Append("<li>Select “Credit Underwriting” on the left sidebar. </li>");
                    note.Append("<li>Scroll down to section “Hierarchy Approval Sign Off”, find this account and click on the ZID: " + app.ID + ". </li>");
                    note.Append("<li>Click “Edit” action button at the top. </li>");
                    note.Append("<li>Scroll down to section “Hierarchy Approval Sign Off”, click the checkbox to approve. </li>");
                    note.Append("<li>Click “Save” action button at the top. </li>");
                    note.Append("<li>Done.</li></ol>");

                    string AgentDBA = app.AgentDBA + "</span><span style='color:#1F497D'></span><br><br><br><b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'>Monthly Volume:</span></b><span style='font-size:10.0pt;font-family:Verdana,sans-serif'> " + vol;

                    if (!string.IsNullOrWhiteSpace(CurrentHeirarchyapproval.EmailTo))
                    {
                        data.UpdateMerchantUWNotes(objUW, app.ID, UserSessions.CurrentUser.UserName);

                        AlertNotification.SendUWAlertNotification(true, UserSessions.CurrentUser.Email, app.MerchantAppUID, app.BusinessDBAName, app.BusinessLegalName, "Credit Underwriting Notification", CurrentHeirarchyapproval.EmailTo, CurrentHeirarchyapproval.CCTo + ";" + UserSessions.CurrentUser.Email, note.ToString(), " - Approval Request for High Volume Account", app.AgentUID, UserSessions.CurrentUser.UserName, string.Empty, app.AgentDBA, app.Bank);
                        this.Master.AddMessageStatus("Request for Approval Email Sent.");
                        isRequest = false;
                        ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Credit Underwriting Notification for Approval Email Sent. Email Sent to: {1}", app.BusinessDBAName, CurrentHeirarchyapproval.EmailTo);
                    }
                }
            }
            else
            {
                isRequest = false;
                this.Master.AddMessageError("Please enter all the required fields before you request for approval");
            }
        }

    }

    protected void btnAction_Click(object sender, EventArgs e)
    {
        Generate_Upload_PDF(false, false);
    }

    private void Generate_Upload_PDF(bool isOpsForm, bool isStatuschanged)
    {
        FormHandler.UploadPDF(isOpsForm, isStatuschanged, null);
    }

    protected void btnOpsAction_Click(object sender, EventArgs e)
    {
        Generate_Upload_PDF(true, false);
    }
    //code added for PXP-9145 start
    protected void btnMrpAction_Click(object sender, EventArgs e)
    {
        FormHandler.Upload_MRP_PDF( false);
    }
    //code added for PXP-9145 end

    public override void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);
        AchMerchant achmerchant = null;
        string m_StatusUID = string.Empty;

        string stat = string.Empty;

        if (agreement != null)
        {
            agreement.Owners = DataAccess.DataMerchantAppDao.GetOwners(agreement.MerchantAppUID);
            agreement.TradeReferences = DataAccess.DataMerchantAppDao.GetTradeReferences(agreement.MerchantAppUID);
            m_StatusUID = agreement.StatusUID;
            stat = agreement.StatusName.Substring(0, 2);

            //when acount is ach only show the ach status instead of cc status
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
                    stat = achmerchant.MerchantStatusName.Substring(0, 2);
                }

                ACHStatus.Visible = true;
                CCStatus.Visible = false;
            }
            else
            {
                LookupTableHandler.MerchantAppStatus(StatusUID, false, "Merchant Management", agreement.StatusName.Substring(0, 2), agreement);
                ListHandler.ListFindItem(StatusUID, agreement.StatusUID);

                ACHStatus.Visible = false;
                CCStatus.Visible = true;
            }

        }

        // Added by Chandra for PXP-7898
        hidStatus.Value = m_StatusUID.ToUpper();


        //PXP-9348 RThakur >> Start
        hiddenCrmStatus.Value = agreement.CRMStatus;
        hiddenAcceptTransaction.Value = Convert.ToString(agreement.CRMAcceptTransactions);
        //PXP-9348 RThakur >> End

        UserSessions.CurrentMerchantApp = agreement;
        FinancialScoreCardGrid.MerchantID = int.Parse(agreement.ID);

        agreement.DelaysApproved = agreement.DelaysApproved.Equals("-1") ? string.Empty : agreement.DelaysApproved;
        DelaysApproved.Text = agreement.DelaysApproved;
        agreement.ReleaseMethodUID = agreement.ReleaseMethodUID.Equals("") ? "-1" : agreement.ReleaseMethodUID;

        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        WucBusinessInfo1.SetBusinessInfoEditMode(this.EditMode);
        DropDownList DiscountMethod = (DropDownList)WucBusinessInfo1.FindControl("DiscountMethod");
        DiscountMethod.SelectedValue = agreement.MonthendApproved ? "63b90eb6-4b2e-4b4f-a9f3-7bfd0a81963c" : "8398a86b-718e-4103-abd9-b60d2d4d14ce";

        this.MCClookup.m_SicCode = agreement.SicCode;
        this.MCClookup.txtSicCodeReadonly = true;

        this.MCClookup.m_SicCodeDesc = agreement.SicCodeDesc;
        this.MCClookup.txtSicCodeDescReadonly = true;

        //code added for PXP-12932 by koshlendra start
        this.MCClookup.m_VisaSicCode = agreement.VisaSicCode;
        this.MCClookup.txtVisaSicCodeReadonly = true;

        this.MCClookup.m_VisaSicCodeDesc = agreement.VisaSicCodeDesc;
        this.MCClookup.txtVisaSicCodeDescReadonly = true;
        //code added for PXP-12932 by koshlendra start

        //code added by koshlendra for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] start
        // Validation added for PXP-9511 bug fixing
        if (agreement.MasterMRP)
        {
            string masterMRPZID = string.Empty;
            masterMRPZID = facade.GetMasterMRPZIDForSameMLEandTaxID(agreement.ID, agreement.BusinessTaxID);
            if (!string.IsNullOrWhiteSpace(masterMRPZID))
                agreement.MasterMRP = false;

        }
        if (agreement.MasterVIRP)
        {
            string masterVIRPZID = string.Empty;
            masterVIRPZID = facade.GetMasterMRPZIDForSameMLEandTaxID(agreement.ID, agreement.BusinessTaxID, true);
            if (!string.IsNullOrWhiteSpace(masterVIRPZID))
                agreement.MasterVIRP = false;

        }
        this.MasterMRP.Checked = agreement.MasterMRP;
        this.MasterVIRP.Checked = agreement.MasterVIRP;
        //PXP-11670 & PXP-11671  >> Rthakur
        if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {
            //this.rowRegisteredURLs.Visible = this.MasterMRP.Checked;
            this.tdlabelRegisteredURLs.Visible = this.MasterMRP.Checked;
            this.tdtextbRegisteredURLs.Visible = this.MasterMRP.Checked;
            this.VIRPRegisteredURLsTR.Visible = this.MasterVIRP.Checked;
            //PXP-12066: Start by Rohit Thakur
            this.CharactersCount.Visible = this.MasterMRP.Checked;
            this.VIRPCharactersCount.Visible = this.MasterVIRP.Checked;
            if (this.RegisteredURLs.ToString() != string.Empty)
            {
                this.charLength.InnerText = this.RegisteredURLs.Text.Length.ToString();
            }
            this.VIRPcharLength.InnerText = string.IsNullOrEmpty(this.VIRPRegisteredURLs.Text) == false ? this.VIRPRegisteredURLs.Text.Length.ToString() : "0";
            //PXP-12066: End by Rohit Thakur
            if (!UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_CREDIT_UNDERWRITING))
                RegisteredURLs.ReadOnly = true;
            if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.RegisteredURLs) && agreement.MasterMRP && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.BusinessWebsite))
                this.RegisteredURLs.Text = UserSessions.CurrentMerchantApp.BusinessWebsite;

            if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.RegisteredURLs) && agreement.MasterVIRP && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.BusinessWebsite))
                this.VIRPRegisteredURLs.Text = UserSessions.CurrentMerchantApp.BusinessWebsite;
        }
        else
        {
            //PXP-12066: Start by Rohit Thakur
            this.CharactersCount.Visible = false;
            //PXP-12066: End by Rohit Thakur
        }

        //code added by koshlendra for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] end
        //code added by koshlendra for PXP-9310[Generate and save High Risk Merchant registration request in csv format] start
        this.SendMRPRequest.Checked = agreement.SendMRPRequest;
        //code added by koshlendra for PXP-9310[Generate and save High Risk Merchant registration request in csv format] end
        grdLinks.Enabled = btnEdit.Enabled;

        WebPercentEditor ReservePercent = (WebPercentEditor)WucBusinessInfo1.FindControl("ReservePercent");
        ReservePercent.Value = agreement.ReservePercent;

        if (ConditionalApproval.Checked)
        {
            ConditionalDueDate.Attributes.Add("style", "display:inline");
            date.Attributes.Add("style", "display:inline");
            date.Enabled = true;
            if (ConditionalDueDate.Value == null)
            {
                ConditionalDueDate.Value = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
            }
        }
        else
        {
            ConditionalDueDate.Attributes.Add("style", "display:none");
            date.Attributes.Add("style", "display:none");
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(agreement.MerchantAppUID);


        #region DM-5610: Dismantle approval hierarchy
        //decimal vol = agreement.TinfoAverageMonthlyVMCVolume;

        //All the If else conditions happens when we find the Heirarchy approval. The limits are now saved on [dbo].[REF_HeirarchyApprovalLimit]
        /// 
        //UWHeirarchyApprovalLimit CurrentHeirarchyapproval = this.m_lstHeirarchyapproval.Where(hm => hm.AMVLowerLimit <= vol && hm.AMVUpperLimit > vol && hm.IsMCCRestrictedIndustry == FormHandler.IsMCCRestricted(agreement.SicCode)).FirstOrDefault();

        //if (CurrentHeirarchyapproval != null && !(UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas)))
        //{
        //    pnlHierarchy.Visible = true;
        //    HierarchyApprovalSignOff.Text = CurrentHeirarchyapproval.Role + ": " + CurrentHeirarchyapproval.Descreption;
        //    btnRequest.Enabled = true && (UserSessions.CurrentUser.DefaultRoleUID.ToLower() == Constants.ROLE_CREDIT_UNDERWRITING.ToLower());

        //    if (CurrentHeirarchyapproval.AuthorizedUsers.ConvertAll(d => d.ToUpper()).Contains(UserSessions.CurrentUser.UserName.ToUpper()))
        //    {
        //        //Enable it only in Edit Mode
        //        HierarchyApprovalSignOff.Enabled = true && this.EditMode;
        //    }
        //    else
        //    {
        //        HierarchyApprovalSignOff.Enabled = false;
        //    }
        //}
        //else
        //{
        //    pnlHierarchy.Visible = false;
        //}
        ///
        #endregion

        //load the products required section
        WucServices1.LoadServices(ID, ServiceCategories.CREDITPRODUCTS, 4);

        if (objUW != null)
        {
            //PXP-14251 - Fix Zeus Ticket Notes displaying Blank HTML Page

            if (objUW.NotesUW != string.Empty)
            {
                objUW.NotesUW = System.Web.HttpUtility.HtmlDecode(objUW.NotesUW);
            }
            if (objUW.UWIssues != string.Empty)
            {
                objUW.UWIssues = System.Web.HttpUtility.HtmlDecode(objUW.UWIssues);

            }
            FormBinding.BindObjectToControls(objUW, pnlDetail);

            #region DM-5610: Dismantle approval hierarchy
            //if (CurrentHeirarchyapproval != null && !(UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas)))
            //{
            //    btnRequest.Visible = true;
            //    btnRequest.Enabled = (string.IsNullOrWhiteSpace(objUW.RiskApproval) && string.IsNullOrWhiteSpace(objUW.ExecutiveApproval) && (UserSessions.CurrentUser.DefaultRoleUID.ToLower() == Constants.ROLE_CREDIT_UNDERWRITING.ToLower()));
            //}

            //HierarchyApprovalSignOff.Checked = (!string.IsNullOrWhiteSpace(objUW.RiskApproval) || !string.IsNullOrWhiteSpace(objUW.ExecutiveApproval));

            //if (EditMode)
            //{
            //    //Check everything from CurrentHeirarchyapproval 
            //    if (CurrentHeirarchyapproval != null && !(UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas)))
            //    {
            //        //Is the User is on the Authorized list on [dbo].[REF_HeirarchyApprovalUser] he should be able to approve, 
            //        //This goes by the rules set on [dbo].[REF_HeirarchyApprovalLimit]
            //        //The owner who requested the approval cannot approve by self.
            //        if ((CurrentHeirarchyapproval.AuthorizedUsers.ConvertAll(d => d.ToUpper()).Contains(UserSessions.CurrentUser.UserName.ToUpper()) || UserSessions.CurrentUser.UserRoles.ContainsKey(ADMIN_ROLE)) && !string.IsNullOrWhiteSpace(objUW.ApprovalRequestedBy))
            //        {
            //            HierarchyApprovalSignOff.Enabled = true;
            //        }
            //    }

            //}
            #endregion

            if (PhysicalSiteVisit.Checked)
            {
                PhysicalVisitOn.Attributes.Add("style", "display:block");
                Visit.Attributes.Add("style", "display:inline");
                Visit.Enabled = true;

                if (PhysicalVisitOn.Value == null)
                {
                    PhysicalVisitOn.Value = DateTime.Today.AddDays(1).ToString(UserSessions.CurrentUser.DatePattern);
                }
            }
            else
            {
                PhysicalVisitOn.Attributes.Add("style", "display:none");
                Visit.Attributes.Add("style", "display:none");
            }


            //DM-5610: Dismantle approval hierarchy
            //btnRequest.Visible = !(m_StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED || stat == "OP" || stat == "DP" || stat == "MS"); //status change AB = OP and CS = MS

        }

        // only show the DaysHoldType Row if merchant brand is optimal.
        trDaysHoldType.Visible = (agreement.Brand == MerchantBrand.Optimal) ? true : false;
        tdEmpty.Visible = !trDaysHoldType.Visible;

        BindOwners(agreement);
        BindData();
        BindChecklist();
        BindVerticalMarketData();
        BindFulfillment();
        GetMultiLinkRefreshLog(int.Parse(UserSessions.CurrentMerchantApp.ID));

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        MerchantScoreCards.SetDataSource(prms);

        FormHandler.SetControlEditMode(CashAdvance, true);

        SetEditmode(false);

        //to fill the premierprofile fields 
        FillPremierProfileReponse();

        //OFAC.Visible = ResOFACMatch.Checked;
        OFACLabel.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";
        ResOFACDescription.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";

        WucOwnerUW0.SetEditmode(this.EditMode);
        WucOwnerUW1.SetEditmode(this.EditMode);
        WucOwnerUW2.SetEditmode(this.EditMode);
        WucOwnerUW3.SetEditmode(this.EditMode);
        //PXP-3118 Rohit Thakur
        WucOwnerUW4.SetEditmode(this.EditMode);
        WucOwnerUW5.SetEditmode(this.EditMode);

        btnRefreshList.Enabled = true;
        btnRefreshDesc.Enabled = true;

        ValidDescriptors.grdDesc.Enabled = true;
        ValidDescriptors.ddpPageSize.Enabled = true;

        if (isACHonly && achmerchant != null)
        {
            this.StatusUID_SelectedIndexChanged(ACHStatusUID, null);
        }
        else
        {
            this.StatusUID_SelectedIndexChanged(StatusUID, null);
        }

        if (m_StatusUID.ToLower() == Constants.QUEUESTATUS_CU_DECLINED.ToLower())
        {
            string myval = (agreement.DeclineReasonID.ToString() == "0") ? "-1" : agreement.DeclineReasonID.ToString();
            ddlddlDeclineReason.SelectedValue = (ddlddlDeclineReason.Items.FindByValue(myval) != null) ? myval : "-1";
            tbPrimaryReason.Text = agreement.ReasonForDecline;
        }
        else if (m_StatusUID.ToLower() == Constants.QUEUESTATUS_CU_WITHDRAWN.ToLower())
        {
            string myval = (agreement.DeclineReasonID.ToString() == "0") ? "-1" : agreement.DeclineReasonID.ToString();
            ddlddlDeclineReason.SelectedValue = (ddlddlDeclineReason.Items.FindByValue(myval) != null) ? myval : "-1";
            tbPrimaryReason.Text = agreement.ReasonForWithdrawn;
        }
        WucBusinessInfo1.LoadOffice(agreement);

        //code added by abarua PXP-7469
        // if UWIssues is empty, we prefill it with a default free form "template".   
        if (objUW == null || (objUW != null && string.IsNullOrWhiteSpace(objUW.UWIssues)))
        {
            if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
            {
                string AcqBank_OnlineBankOnly = "8d2281ae-e6f0-429e-94d0-c0fe6bfede01";
                string bank_achonly_uid = "DADC71E7-A732-4FCA-8C86-DE3E7253209C";
                StringBuilder sbuw = new StringBuilder();
                StringBuilder sbcc = new StringBuilder();
                string line_template = "{0}: {1}";
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
                UWIssues.Text = FormHandler.GetUWIssuesTemplate(UserSessions.CurrentMerchantApp);
            }
        }
        else
        {
            UWIssues.Text = objUW.UWIssues;
        }

        // we only allow descriptor override on these settlement platforms.
        AllowDescriptorOverride.Enabled = this.EditMode && CreditCardTransactionFacade.IsDescriptorOverridable(agreement.SettlePlatformUID);


        //Only if merchant belongs to Irvine office we make the 3DE request button visible
        pnl3DE.Visible = isIrvineMerchant;
        // Enable/Disable 3DE Request & Response Buttons
        if (pnl3DE.Visible && this.EditMode)
        {
            if (UserSessions.CurrentMerchantApp.StatusUID.ToUpper() != Constants.QUEUESTATUS_CU_WITHDRAWN)
            {
                //string EndPointURL = ConfigurationManager.AppSettings["Zeus3deGetZeuseModel"];
                //string jsonresponse = JSONClient.PostRestService(UserSessions.CurrentMerchantApp.ID, EndPointURL);
                //ZeusModel _model = JSONClient.Serializer.Deserialize<ZeusModel>(jsonresponse);
                //Request3DE.Enabled = _model.Request3DE_Btn;
                //Response3DE.Enabled = _model.Response3DE_Btn;      
                Request3DE.Enabled = DataAccess.DataMerchantAppDao.Is3DEResponseExist(UserSessions.CurrentMerchantApp.ID);
                Response3DE.Enabled = Request3DE.Enabled ? false : true;
            }
            else
            {
                Request3DE.Enabled = false;
                Response3DE.Enabled = false;
            }
        }

        //PXP-3118 Rohit Thakur
        //Code commented for PXP-3199:Owner 5 and Owner 6 should be display for all office and merchant by Koshlendra on 28/12/2017 start 
        //if (!UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Cambridge))
        //{
        //    WucOwnerUW4.Visible = false;
        //    WucOwnerUW5.Visible = false;
        //}
        //Code commented for PXP-3199:Owner 5 and Owner 6 should be display for all office and merchant by Koshlendra on 28/12/2017 end 
        UserFacade userFacade = new UserFacade();
        var userRoles = userFacade.GetUser(UserSessions.CurrentUser.UID).UserRoles.Where(u => u.Value.Enabled == true);  // Dynamic list of enabled user roles;
        btnPDF.Visible = userRoles.Any(s => s.Value.RoleID.Equals(Constants.ROLE_CREDIT_UNDERWRITING));


        //Code added by koshlendra for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] start       

        bool CCApproved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED);
        bool isApproved = CCApproved;

        bool ACHApproved = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_APPROVED);

        //if account is ACHOnly then we need check for ACH status approval
        if (isACHonly)
        {
            isApproved = ACHApproved;
        }
        btnMrpPDF.Visible = MasterMRP.Checked && isApproved;
        //btnVIRPPDF.Visible = MasterVIRP.Checked && isApproved;

        //Code added by koshlendra for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] end     
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
        {
            MasterPageMerchant master = (MasterPageMerchant)this.Master;
            master.UpdateNotification("");
            MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        }
        /******** End of PXP-2206 **************/
        //Information message added by koshlendra for PPX-9310 is MARMAster is not checked and Send MRP Request is checked  end
        this.Page.PreRender += Page_PreRender;
    }

    private void BindVerticalMarketData()
    {
        Hashtable prms = new Hashtable();
        DataSet dt = new DataSet();

        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        dt = DataAccess.DataMerchantAppDao.GetVerticalMarkets(prms);
        VerticalMarket.DataSource = dt;
        VerticalMarket.DataBind();

        VerticalMarket.Items.Clear();

        System.Web.UI.WebControls.ListItem item = null;
        //Amit Patne : PXP-3818 - Zeus: Vertical Market and Billing Method
        DataRow[] drVerticalMarket = dt.Tables[0].Select("VerticalMarketTypeID=" + 1);
        foreach (DataRow dr in drVerticalMarket)
        {
            item = new System.Web.UI.WebControls.ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            item.Attributes.Add("title", dr.ItemArray[2].ToString());
            item.Selected = DataLayer.Field2Bool(dr.ItemArray[4]);
            VerticalMarket.Items.Add(item);
        }

        item = null;
        BillingTypes.Items.Clear();
        DataRow[] drBillingType = dt.Tables[0].Select("VerticalMarketTypeID=" + 2);
        foreach (DataRow dr in drBillingType)
        {
            item = new System.Web.UI.WebControls.ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            item.Attributes.Add("title", dr.ItemArray[2].ToString());
            item.Selected = DataLayer.Field2Bool(dr.ItemArray[4]);
            BillingTypes.Items.Add(item);
        }

        ////Add code by anuj for PXP-9250
        item = null;
        MarketingMethods.Items.Clear();
        DataRow[] drMktMethods = dt.Tables[0].Select("VerticalMarketTypeID=" + 3);
        foreach (DataRow dr in drMktMethods)
        {
            item = new System.Web.UI.WebControls.ListItem(dr.ItemArray[1].ToString(), dr.ItemArray[0].ToString());
            item.Attributes.Add("title", dr.ItemArray[2].ToString());
            item.Selected = DataLayer.Field2Bool(dr.ItemArray[4]);
            MarketingMethods.Items.Add(item);
        }


    }

    public override bool FormSave()
    {
        bool perform = false;
        DataSet dsmultiLink = new DataSet();
        MerchantFacade merchantFacade = new MerchantFacade();

        try
        {
            //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  start
            string oldSicCode = string.Empty;
            if (UserSessions.CurrentMerchantApp != null)
                oldSicCode = UserSessions.CurrentMerchantApp.SicCode;
            //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  end
            // Added by koshlendra for PXP-9145 Zeus:[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] start
            bool isMasterMRP= false;
            bool isMasterVIRP = false;
            if (UserSessions.CurrentMerchantApp != null)
            {
                isMasterMRP = UserSessions.CurrentMerchantApp.MasterMRP;
                isMasterVIRP = UserSessions.CurrentMerchantApp.MasterVIRP;
            }
            // Added by koshlendra for PXP-9145 [Zeus:Ability to generate and review High Risk Merchant registration request for Mastercardconnect] end  

            // Added by koshlendra for PXP-9310[Generate and save High Risk Merchant registration request in csv format] start
            bool sendMRPRequest = false;
            bool _sendVIRPRequest = false;
            if (UserSessions.CurrentMerchantApp != null)
            {
                sendMRPRequest = UserSessions.CurrentMerchantApp.SendMRPRequest;
                _sendVIRPRequest = UserSessions.CurrentMerchantApp.SendVIRPRequest;
            }
            // Added by koshlendra for PXP-9310[Generate and save High Risk Merchant registration request in csv format]end  

            MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
            agreement.CloneMerchantApp();

            List<Owner> ownerc = new List<Owner>();
            for (int i = 0; i < agreement.Owners.Count; i++)
            {
                agreement.Owners[i].CloneOwner();
                ownerc.Add(agreement.Owners[i].OwnerClone);
            }

            FormBinding.BindControlsToObject(agreement, pnlDetail);

            agreement.DelaysApproved = DelaysApproved.Text;
            agreement.SicCode = this.MCClookup.m_SicCode;
            agreement.SicCodeDesc = this.MCClookup.m_SicCodeDesc;
            DropDownList DiscountMethod = (DropDownList)WucBusinessInfo1.FindControl("DiscountMethod");
            agreement.MonthendApproved = DiscountMethod.SelectedValue.ToUpper() == "63B90EB6-4B2E-4B4F-A9F3-7BFD0A81963C" ? true : false;

            AchMerchant achMerchant = null;
            //code added for PXP-12932 by koshlendra start
            agreement.VisaSicCode = this.MCClookup.m_VisaSicCode;
            agreement.VisaSicCodeDesc = this.MCClookup.m_VisaSicCodeDesc;
            //code added for PXP-12932 by koshlendra end

            string m_StatusUID = null, m_statusName = null, m_cloneStatusUID = null;

            if (UserSessions.ActiveAchMerchant != null && isACHonly)
            {
                UserSessions.ActiveAchMerchant.CloneAchMerchant();
                achMerchant = UserSessions.ActiveAchMerchant;
                achMerchant.UpdatedBy = UserSessions.CurrentUser.UserName;
                m_StatusUID = achMerchant.MerchantStatusUID = ACHStatusUID.SelectedValue;
                m_statusName = achMerchant.MerchantStatusName;
                m_cloneStatusUID = achMerchant.AchMerchantClone.MerchantStatusUID;
            }
            else
            {
                //m_StatusUID = !FormHandler.IsConditionExist(UserSessions.CurrentMerchantApp.MerchantAppUID) ? StatusUID.SelectedItem.Value : agreement.StatusUID;
                //m_statusName = !FormHandler.IsConditionExist(UserSessions.CurrentMerchantApp.MerchantAppUID) ? StatusUID.SelectedItem.Text : agreement.StatusName;
                m_StatusUID = agreement.StatusUID = StatusUID.SelectedItem.Value;
                m_statusName = agreement.StatusName = StatusUID.SelectedItem.Text;

                m_cloneStatusUID = agreement.MerchantAppClone.StatusUID;
            }
            //for session management of CRM Status,Count
            //PXP-8409 by Sanidhya
            if (!this.Adding)
            {
                MerchantApp currntMerchantApp = merchantFacade.GetMerchantAppZeus(agreement.MerchantAppUID);
                agreement.CRMStatus = currntMerchantApp.CRMStatus;
                agreement.CRMCount = currntMerchantApp.CRMCount;
                agreement.CRMAcceptTransactions = currntMerchantApp.CRMAcceptTransactions;
            }

            if (!this.FormDataCheck())
            {
                OFACLabel.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";
                ResOFACDescription.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";

                return false;
            }


            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            User user = UserSessions.CurrentUser;
            agreement.UserUpdated = user.UserName;

            if (agreement.AgentUID.ToUpper() == "0BD7BA26-2C1E-4132-8CDD-2EEC417D3C30")
            {
                agreement.SicCode = "8398";
                agreement.SicCodeDesc = "Organizations, Charitable and Social Service";
            }

            Underwriting objUW = new Underwriting();

            string HighRiskDescriptor = GetHighRiskDescriptor(agreement);

            bool CCApproved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED);

            //check to see if account is already approved -cc status by default
            bool isApproved = CCApproved;

            bool ACHApproved = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_APPROVED);

            //if account is ACHOnly then we need check for ACH status approval
            if (isACHonly)
            {
                isApproved = ACHApproved;
            }
            //START:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan
            bool isHierarchyApprovalSignOff = false;
            //END:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan

            #region DM-5610: Dismantle approval hierarchy
            ////this block should only be called if the account is not approved yet. if it is approved once then we do not need this anymore.
            //if (HierarchyApprovalSignOff.Checked && (m_statusName.Substring(0, 2) == "SS" || m_statusName.Substring(0, 2) == "CU")) //RM Queue is now changed to SS
            //{
            //    if (!isApproved)
            //    {
            //        string SicCode = this.MCClookup.m_SicCode;

            //        if (!isACHonly)
            //        {
            //            if (SicCode == "5960" || SicCode == "5962" || SicCode == "5964" || SicCode == "5965" ||
            //            SicCode == "5966" || SicCode == "5967" || SicCode == "5968" || SicCode == "5969")
            //            {
            //                if (string.IsNullOrWhiteSpace(HighRiskDescriptor))
            //                {
            //                    this.Master.AddMessageError("Please add High Risk Descriptor.");

            //                    ListHandler.ListFindItem(StatusUID, agreement.MerchantAppClone.StatusUID);
            //                    agreement.StatusUID = agreement.MerchantAppClone.StatusUID;
            //                    return false;
            //                }
            //            }
            //        }

            //        objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(agreement.MerchantAppUID);

            //        if (isACHonly && achMerchant != null)
            //            achMerchant.MerchantStatusUID = Constants.QUEUESTATUS_CU_APPROVED;
            //        else
            //            agreement.StatusUID = Constants.QUEUESTATUS_CU_APPROVED;
            //        //START:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan
            //        isHierarchyApprovalSignOff = true;
            //        //END:PXP-9144 Autogenerated MID for Woodforest bank By Ali Khan
            //        if (objUW != null)
            //        {
            //            //code updated for PXP-13335 by koshlendra start 
            //            agreement.UserUpdated = user.UserName ;
            //            //code updated for PXP-13335 by koshlendra start 
            //        }
            //    }
            //}
            #endregion


            if (m_StatusUID.ToLower() == Constants.QUEUESTATUS_CU_DECLINED.ToLower())
            {
                agreement.ReasonForDecline = tbPrimaryReason.Text.Trim();
            }
            else if (m_StatusUID.ToLower() == Constants.QUEUESTATUS_CU_WITHDRAWN.ToLower())
            {
                agreement.ReasonForWithdrawn = tbPrimaryReason.Text.Trim();
            }

            //PXP-9750 Rohit Thakur >> Start
            //Create new ticket for ‘Mastercard De-registration process
            if (!isACHonly && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION && agreement.HighRiskRegistered && m_StatusUID.ToUpper() != m_cloneStatusUID.ToUpper())
            {
                FormHandler.AddTicketForMastercard(agreement, "i", "3", "2251", "2252", "3-Low", "6");
            }
            //PXP-9750 Rohit Thakur >> End
            if (!isACHonly && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_MS_CANCELLATION && agreement.VIRPHighRiskRegistered && m_StatusUID.ToUpper() != m_cloneStatusUID.ToUpper())
            {
                FormHandler.AddTicketForVisa(agreement, "i", "3", "2329", "2330", "3-Low", "6");
            }
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
            agreement = FormHandler.CheckGenerateMIDforWoodforest(agreement, CCApproved, isHierarchyApprovalSignOff ? agreement.StatusUID : m_StatusUID, AgentLevel.SelectedItem.Value);
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
                    UWIssues.Text = uwIssue.ToString();
                }
                else if (UWIssues.Text.Contains("'Nutra Trial' account"))
                {
                    // PXP-12436: Start by Rohit Thakur
                    if (agreement.Office == CommonUtility.Util.Offices.Irvine)
                    {
                        UWIssues.Text = UWIssues.Text.Replace("'Nutra Trial' account", "'Tangible Trial' account");
                    }
                    // PXP-12436: End by Rohit Thakur                  
                }                
                //code added by koshlendra for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] end

                // Added by koshlendra for PXP-9310[Generate and save High Risk Merchant registration request in csv format] end       
                if (agreement.IsNutraMerchant && agreement.Office == CommonUtility.Util.Offices.Irvine && m_StatusUID.ToUpper() != UserSessions.CurrentMerchantApp.MerchantAppClone.StatusUID.ToUpper() && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_DP_RECEIVED_SOFTWARE)
                {
                    FormHandler.AllowPxpForNutra();
                }
            }
            else
            {
                if (UWIssues.Text.Contains("'Nutra Trial' account"))
                {
                    UWIssues.Text = UWIssues.Text.Replace("'Nutra Trial' account", "");
                }
                else if (UWIssues.Text.Contains("'Tangible Trial' account"))
                {
                    UWIssues.Text = UWIssues.Text.Replace("'Tangible Trial' account", "");
                }
            }
            //code added by koshlendra for PXP-9145[Ability to generate and review High Risk Merchant registration request for Mastercardconnect] start
            if (isMasterMRP != MasterMRP.Checked)
            {
                string masterMRPZID = string.Empty;
                masterMRPZID = merchantFacade.GetMasterMRPZIDForSameMLEandTaxID(agreement.ID, agreement.BusinessTaxID);
                if (MasterMRP.Checked && (!string.IsNullOrWhiteSpace(masterMRPZID)))
                {
                    this.Master.AddMessageError("Master MRP is already checked for ZID: " + masterMRPZID + " under this MLE.");
                    MasterMRP.Checked = false;
                }
                agreement.MasterMRP = MasterMRP.Checked;

            }

            // DM-5292.- Add validation for VIRP to see if this ZID has another same MLE
            if (isMasterVIRP != MasterVIRP.Checked)
            {
                string masterVIRPZID = string.Empty;
                masterVIRPZID = merchantFacade.GetMasterMRPZIDForSameMLEandTaxID(agreement.ID, agreement.BusinessTaxID,MasterVIRP.Checked);
                if (MasterVIRP.Checked && (!string.IsNullOrWhiteSpace(masterVIRPZID)))
                {
                    this.Master.AddMessageError("Master VIRP is already checked for ZID: " + masterVIRPZID + " under this MLE.");
                    MasterVIRP.Checked = false;
                }
                agreement.MasterVIRP = MasterVIRP.Checked;

            }

            // DM-4634 .- This change apply only for the following banks and only if the app is one of those ones three 
            if (m_cloneStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED &&
                (agreement.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            || agreement.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
            || agreement.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
            || agreement.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
            {
                if (sendMRPRequest != SendMRPRequest.Checked)
                {
                    if (SendMRPRequest.Checked && MasterMRP.Checked)
                    {
                        FormHandler.Upload_MRP_PDF(true);
                        FormHandler.Upload_MRP_CSV();
                        agreement.MRPDateSubmitted = DateTime.Now;
                    }
                    agreement.SendMRPRequest = SendMRPRequest.Checked;
                    agreement.MasterMRP = MasterMRP.Checked;
                }
                if (SendVIRPRequest.Checked != _sendVIRPRequest)
                {
                    if (MasterVIRP.Checked && SendVIRPRequest.Checked)
                    {
                        FormHandler.Upload_VIRP_CSV();
                        agreement.VIRPDateSubmitted = DateTime.Now;
                    }
                }
            }
            bool moveStatus = (m_cloneStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_StatusUID.ToUpper() == Constants.QUEUESTATUS_OP_RECEIVED.ToUpper());
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

            bool registeredHR = HighRiskRegistered.Checked;
            if (registeredHR)
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

            UWIssues.Text = RemoveExtraSpace(UWIssues.Text.ToString());
            //End code by Anuj for PXP-9311


            //PXP-11452 By sanidhya
            agreement = FormHandler.ManageDP_SoftwareStatus(agreement);

            MerchantFacade facade = new MerchantFacade();
            int rows = facade.UpdateMerchantApp(agreement);
            //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  start
            //Ani: DM-5589
            FormHandler.UpdateMerchantRiskParameters(agreement);
            //Added by Koshlendra for PXP-3529: Zeus: Set default Risk parameters on basis while mcc change for PaymentXP merchant  end
            if (rows > 0)
            {
                if (isACHonly && achMerchant != null)
                {
                    DataAccess.DataAchMerchantDao.UpdateAchMerchant(achMerchant);
                    //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
                    UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(UserSessions.CurrentMerchantApp.ID));
                }

                //save the products required section
                WucServices1.UpdateServices(agreement.MerchantAppUID, CommonUtility.Util.if_s((int)ServiceCategories.CREDITPRODUCTS));

                UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

                objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(agreement.MerchantAppUID);

                if (objUW != null)
                    objUW.CloneUnderwriting();
                else
                    objUW = new Underwriting();

                FormBinding.BindControlsToObject(objUW, pnlDetail);
                objUW.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;

                //// NOTE: why don't these variable get set when they bind?

                DropDownList PaymentFrequencyID = (DropDownList)WucBusinessInfo1.FindControl("PaymentFrequencyID");
                DropDownList PaymentScheduleID = (DropDownList)WucBusinessInfo1.FindControl("PaymentScheduleID");

                objUW.PaymentScheduleID = CommonUtility.Util.if_i(PaymentScheduleID.SelectedValue, -1);
                objUW.PaymentFrequencyID = CommonUtility.Util.if_i(PaymentFrequencyID.SelectedValue, -1);


                decimal vol = agreement.TinfoAverageMonthlyVMCVolume;
                UWHeirarchyApprovalLimit CurrentHeirarchyapproval = this.m_lstHeirarchyapproval.Where(hm => hm.AMVLowerLimit <= vol && hm.AMVUpperLimit > vol && hm.IsMCCRestrictedIndustry == FormHandler.IsMCCRestricted(agreement.SicCode)).FirstOrDefault();

                #region DM-5610: Dismantle approval hierarchy
                //// this hierarchy approval request was earlier doen only for Meritus merhcants but now we are doing it for optimal also
                //if (HierarchyApprovalSignOff.Checked && CurrentHeirarchyapproval != null && !(UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas)))
                //{
                //    if (CurrentHeirarchyapproval.AuthorizedUsers.ConvertAll(d => d.ToUpper()).Contains(UserSessions.CurrentUser.UserName.ToUpper()) || UserSessions.CurrentUser.UserRoles.ContainsKey(ADMIN_ROLE))
                //    {
                //        //I am using the existing Executive Approval column for the Executive category.  
                //        //All other categories use Risk approval column, instead of following the old design to add new columns which is bad.
                //        if (CurrentHeirarchyapproval.HeirarchyApprovalLimitID == 3 || CurrentHeirarchyapproval.HeirarchyApprovalLimitID == 6)
                //        {
                //            objUW.ExecutiveApproval = user.UserName;
                //            objUW.ExecutiveApprovalDate = DateTime.Now;
                //        }
                //        else
                //        {
                //            objUW.RiskApproval = user.UserName;
                //            objUW.RiskApprovalDate = DateTime.Now;
                //        }

                //        //Send Email
                //        if (m_StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_StatusUID.ToUpper() != m_cloneStatusUID.ToUpper())
                //        {
                //            string From = user.Email;
                //            string note = "This account has been electronically approved by " + user.FirstLastName + " on " + DateTime.Today.ToShortDateString() + ".";
                //            //Send email to requestor and copy the Managers who are eligible to approve.
                //            string to = LookupTableHandler.m_ActiveUsers.Where(u => u.UserName.ToUpper().Trim() == objUW.ApprovalRequestedBy.ToUpper().Trim()).Select(u => u.Email).FirstOrDefault();
                //            if (to != null)
                //            {
                //                AlertNotification.SendUWAlertNotification(true, From, agreement.MerchantAppUID, agreement.BusinessDBAName, agreement.BusinessLegalName, "Credit Underwriting Notification", to, CurrentHeirarchyapproval.EmailTo, note, " - Hierarchy Sign Off Completed", agreement.AgentUID, user.UserName, string.Empty, agreement.AgentDBA, agreement.Bank);
                //                ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Credit Underwriting Notification for Hierarchy Sign Off Completed. Email sent to: {1}", agreement.BusinessDBAName, to);
                //            }
                //        }
                //    }
                //}
                #endregion

                //BEGIN: Code added by Gabriel Gonzalez for DM-1868
                var DGCheck = VerticalMarket.Items.FindByValue("5");
                StringBuilder uwIssue = new StringBuilder(UWIssues.Text);

                if (DGCheck.Selected)
                {
                    if (!uwIssue.ToString().Contains("'Digital' account"))
                    {
                        uwIssue.AppendLine("'Digital' account");
                        objUW.UWIssues = uwIssue.ToString();
                    }
                }
                else
                {
                    if (uwIssue.ToString().Contains("'Digital' account"))
                    {
                        uwIssue.Replace("'Digital' account", "");
                        objUW.UWIssues = uwIssue.ToString();
                    }
                }
                //END: Code added by Gabriel Gonzalez for DM-1868

                //PXP-14251 - Fix Zeus Ticket Notes displaying Blank HTML Page
                if (objUW.NotesUW != string.Empty)
                {
                    objUW.NotesUW = Server.HtmlEncode(objUW.NotesUW);

                }
                if (objUW.UWIssues != string.Empty)
                {
                    objUW.UWIssues = Server.HtmlEncode(objUW.UWIssues);

                }


                objUW.HighRiskDescriptor = HighRiskDescriptor;
                DataAccess.DataUnderwritingDao.UpdateMerchantUWNotes(objUW, agreement.ID, UserSessions.CurrentUser.UserName);

                SaveOwners();
                SaveCheckList();
                SaveVerticalMarketData();
                SaveUWFulfillment();

                for (int i = 0, j = 0; i < ownerc.Count; i++, j++)
                {
                    if (j < agreement.MerchantAppClone.Owners.Count)
                        agreement.MerchantAppClone.Owners[j] = ownerc[i];
                }

                FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);

                // handle adding merchant notes. this must be done before sending the email. FormHandler.CompleteApplication()
                if (m_StatusUID.ToLower() == Constants.QUEUESTATUS_CU_WITHDRAWN.ToLower())
                {
                    // add decline reason, then merchant note:
                    DataMerchantApp.GetInstance().ManageMerchantDeclineReason(CommonUtility.Util.if_i(agreement.ID, 0), CommonUtility.Util.if_i(ddlddlDeclineReason.SelectedValue, 0));

                    if (!this.IsExistingNote(agreement.MerchantAppUID, SUBJECT_WITHDRAWN, tbPrimaryReason.Text))
                    {
                        MerchantNotes objMN = new MerchantNotes()
                        {
                            MerchantAppUID = agreement.MerchantAppUID,
                            Subject = SUBJECT_WITHDRAWN,
                            Notes = tbPrimaryReason.Text,
                            RequiresCallback = false,
                            UserCreated = UserSessions.CurrentUser.UserName,
                            Email_Agent = true,
                            View_MPSAll = true,
                            View_Agent = true,
                            View_Bank = true,
                            RepeatIssue = false,
                            Complaint = false
                        };

                        DataMerchantApp.GetInstance().InsertMerchantNotes(objMN);
                    }
                }
                else if (m_StatusUID.ToLower() == Constants.QUEUESTATUS_CU_DECLINED.ToLower())
                {
                    // add decline reason, then merchant note:
                    DataMerchantApp.GetInstance().ManageMerchantDeclineReason(CommonUtility.Util.if_i(agreement.ID, 0), CommonUtility.Util.if_i(ddlddlDeclineReason.SelectedValue, 0));

                    if (!this.IsExistingNote(agreement.MerchantAppUID, SUBJECT_DECLINED, tbPrimaryReason.Text))
                    {

                        MerchantNotes objMN = new MerchantNotes()
                        {
                            MerchantAppUID = agreement.MerchantAppUID,
                            Subject = SUBJECT_DECLINED,
                            Notes = tbPrimaryReason.Text,
                            RequiresCallback = false,
                            UserCreated = UserSessions.CurrentUser.UserName,
                            Email_Agent = true,
                            View_MPSAll = true,
                            View_Agent = true,
                            View_Bank = true,
                            RepeatIssue = false,
                            Complaint = false
                        };

                        DataMerchantApp.GetInstance().InsertMerchantNotes(objMN);
                    }
                }

                FormHandler.CompleteApplication(agreement, achMerchant, m_cloneStatusUID, user.UserName);

                //send email if there is change in the Approved Sales Profile section
                string stats = UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2).ToUpper();

                if (isACHonly && achMerchant != null)
                    stats = achMerchant.MerchantStatusName.Substring(0, 2).ToUpper();

                if (m_StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED || stats == "OP" || stats == "MS" || stats == "DP")
                    CheckApprovedSalesChanges(agreement, objUW);

                //update the premierprofile table
                if (CommonUtility.Util.if_i(PremierProfileID.Value, 0) > 0)
                {
                    ZeusPremierProfile objPP = new ZeusPremierProfile();
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.PremierProfileID = CommonUtility.Util.if_i(PremierProfileID.Value, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    DataNetConnect objConnect = new DataNetConnect();
                    objConnect.UpdatePremierProfile(objPP);
                }
                else
                {
                    DataNetConnect objNC = new DataNetConnect();

                    Hashtable prms = new Hashtable();
                    prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
                    prms.Add("@IsCurrent", true);

                    ZeusPremierProfile objPP = new ZeusPremierProfile();
                    objPP = objNC.GetPremierProfile(prms);

                    if (objPP != null)
                    {
                        objPP.UserModified = UserSessions.CurrentUser.UserName;
                        objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                        objPP.IsCurrent = false;

                        DataNetConnect objConnect = new DataNetConnect();
                        objConnect.UpdatePremierProfile(objPP);
                    }
                }

                //store the values of Premier profile score seperately .this helps user to enter manual data for the fields.
                MerchantPremierProfile objMPP = DataAccess.DataUnderwritingDao.LoadMerchantPremierProfile(agreement.ID);

                if (objMPP != null)
                    objMPP.CloneMerchantPremierProfile();
                else
                    objMPP = new MerchantPremierProfile();


                objMPP.UserCreated = UserSessions.CurrentUser.UserName;
                objMPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                objMPP.ResIntelliScore = CommonUtility.Util.if_i(ResIntelliScore.Text, 0);
                objMPP.ResIntelliRiskLevel = ResIntelliRiskLevel.Text;
                objMPP.ResFinancialStabilityScore = CommonUtility.Util.if_i(ResFinancialStabilityScore.Text, 0);
                objMPP.ResFinancialStabilityRiskLevel = ResFinancialStabilityRiskLevel.Text;
                objMPP.ResDBT = CommonUtility.Util.if_i(ResDBT.Text, 0);
                objMPP.ResYearsOnFile = CommonUtility.Util.if_i(ResYearsOnFile.Text, 0);
                if (ResIncorporationDate.Value != null && !string.IsNullOrWhiteSpace(ResIncorporationDate.Text))
                    objMPP.ResIncorporationDate = CommonUtility.Util.if_date(ResIncorporationDate.Value.ToString(), DateTime.MinValue);
                else
                    objMPP.ResIncorporationDate = DateTime.MinValue;
                objMPP.ResCurrentStatus = ResCurrentStatus.Text;
                objMPP.ResOFACMatch = CommonUtility.Util.if_b(ResOFACMatch.Checked, false);
                objMPP.ResOFACDescription = ResOFACDescription.Text;
                objMPP.ResIncorporationState = ResIncorporationState.Text;

                DataAccess.DataUnderwritingDao.UpdateMerchantPremierProfile(objMPP);

                //add the PDF generated from resposneXML to PDF  doc library
                AddPDF();
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

    private string GetHighRiskDescriptor(MerchantApp agreement)
    {

        string HighRiskDescriptor = string.Empty;

        DataSet ds = DataAccess.DataMerchantAppDao.GetMerchantDescriptors(agreement.ID, (int)MerchantDescriptorTypeID.HighRisk);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            HighRiskDescriptor = ds.Tables[0].Rows[0]["Descriptor"].ToString();
        }

        return HighRiskDescriptor.Trim();
    }

    /// <summary>
    /// returns true of that subject and note was found
    /// </summary>
    /// <param name="merchantappuid"></param>
    /// <param name="subject"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private bool IsExistingNote(string merchantappuid, string subject, string note)
    {
        bool ret = false;

        DataTable dt = DataMerchantApp.GetMerchantNotesPaging(new Hashtable() {
            {"@PageSize", 99999},
            {"@SortOrder", "DateCreated"},
            {"@SortDirection", 1},
            {"@MerchantAppUID", merchantappuid},
            {"@Subject", subject} }, 1, 1);

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (note == dr["Notes"].ToString())
                {
                    ret = true;
                    break;
                }
            }
        }

        return ret;
    }

    private void SaveVerticalMarketData()
    {
        MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        data.DeleteMerchantVerticalData(agreement.ID, "");

        //Added code by anuj for PXP-9250
        for (int i = 0; i < MarketingMethods.Items.Count; i++)
        {
            if (MarketingMethods.Items[i].Selected)
            {
                data.InsertMerchantVerticalData(agreement.ID, MarketingMethods.Items[i].Value, UserSessions.CurrentUser.UserID);
                break;
            }
        }

        for (int i = 0; i < VerticalMarket.Items.Count; i++)
        {
            if (VerticalMarket.Items[i].Selected)
            {
                data.InsertMerchantVerticalData(agreement.ID, VerticalMarket.Items[i].Value, UserSessions.CurrentUser.UserID);
            }
        }
        //Amit Patne : PXP-3818 - Zeus: Vertical Market and Billing Method
        for (int i = 0; i < BillingTypes.Items.Count; i++)
        {
            if (BillingTypes.Items[i].Selected)
            {
                data.InsertMerchantVerticalData(agreement.ID, BillingTypes.Items[i].Value, UserSessions.CurrentUser.UserID);
            }
        }
        //Amit Patne : PXP-3818 - Zeus: Vertical Market and Billing Method
        for (int i = 0; i < BillingTypes.Items.Count; i++)
        {
            if (BillingTypes.Items[i].Selected)
            {
                data.InsertMerchantVerticalData(agreement.ID, BillingTypes.Items[i].Value, UserSessions.CurrentUser.UserID);
            }
        }

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

    public override bool FormDataCheck()
    {

        MerchantApp app = UserSessions.CurrentMerchantApp;
        AchMerchant achmerchant = UserSessions.ActiveAchMerchant;
        string m_Status = string.Empty;
        string m_CloneStatusUID = string.Empty;
        //Reverted Conditional Approval part
        // app.CloneMerchantApp();

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

        //Fmassoud 2017.08.28 Sending New Status to Formhandler        
        string NewStatus = isACHonly ? ACHStatusUID.SelectedValue.ToUpper() : StatusUID.SelectedValue.ToUpper();
        IList<string> message = FormHandler.MerchantDataCheck(app, false, false, NewStatus, UserSessions.ActiveAchMerchant);
        //PXP-8409
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

        if (ConditionalApproval.Checked && (ConditionalDueDate.Value == null || string.IsNullOrWhiteSpace(ConditionalDueDate.Text)))
        {
            ConditionalDueDate.Attributes.Add("style", "display:inline");
            date.Attributes.Add("style", "display:inline");
            this.Master.AddMessageError("Please select a due date.");
        }

        if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {
            if (!FormHandler.CheckURLseparators(app.RegisteredURLs))
            {
                this.Master.AddMessageError("RegisteredURLs not valid (URLs separators must be semicolon ';')");
            }
        }

        IList<UWConditions> _ConditionList = FormHandler.GetConditionList(app.MerchantAppUID);

        if (ConditionalApproval.Checked && CommonUtility.Util.if_date(ConditionalDueDate.Text, DateTime.MinValue) != DateTime.MinValue)
        {
            // conditional approval checked, and has valid duedate
            // check to see that there is at least one open pending condition!

            // find pending condition
            bool _IsReceivedInfo = false;

            foreach (UWConditions _Condition in _ConditionList)
            {
                if (_Condition.ReceivedInfo == false)
                {
                    _IsReceivedInfo = true;
                    break;
                }
            }

            if (!_IsReceivedInfo)
            {
                this.Master.AddMessageError("Conditional Approval, please add at least 1 pending condition.");
            }
        }


        if (PhysicalSiteVisit.Checked && (PhysicalVisitOn.Value == null || string.IsNullOrWhiteSpace(PhysicalVisitOn.Text)))
        {
            PhysicalVisitOn.Attributes.Add("style", "display:block");
            Visit.Attributes.Add("style", "display:inline");
            this.Master.AddMessageError("Please select a physical visit date.");
        }

        bool isTrue = true;

        if ((m_Status.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_Status.ToUpper() != m_CloneStatusUID.ToUpper()) || isRequest)
        {
            // NOTE FROM TOL: please peer review this logic!
            if (app.Brand == MerchantBrand.Optimal && Convert.ToInt32(app.DaysHoldTypeID) == (int)eDaysHoldType.NotSet)
            {
                // when setting an optimal branded app to cu-approved, you must select a DaysHoldType
                message.Add("Please select a Days HoldType.");
            }

            if (AgentLevel.SelectedItem.Value == "-1" && !isACHonly)
                message.Add("Please select " + Level.Text.Replace(":", "") + ".");

            DropDownList ReleaseMethodUID = ((DropDownList)WucBusinessInfo1.FindControl("ReleaseMethodUID"));
            if (app.ReservePercent > 0 && ReleaseMethodUID.SelectedIndex == 0)
                message.Add("Please select release method.");

            foreach (GridViewRow row in grdChecklist.Rows)
            {
                // at least one of these (received or waived) must be true. 
                if (!(((CheckBox)row.FindControl("Checked")).Checked == true || ((CheckBox)row.FindControl("Exception")).Checked == true))
                {
                    // bad! 
                    isTrue = false;

                    // break out of the loop to avoid overwriting "isTrue"
                    break;
                }

            }

            if (pnlCredit.Visible)
            {
                if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
                {
                    if (((DropDownList)WucOwnerUW0.FindControl("NameAddressPhoneSummary")).SelectedIndex == 0)
                        message.Add("Please select a guaranty for Owner1");
                }

                if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
                {
                    if (((DropDownList)WucOwnerUW1.FindControl("NameAddressPhoneSummary")).SelectedIndex == 0)
                        message.Add("Please select a guaranty for Owner2");
                }

                if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
                {
                    if (((DropDownList)WucOwnerUW2.FindControl("NameAddressPhoneSummary")).SelectedIndex == 0)
                        message.Add("Please select a guaranty for Owner3");
                }

                if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("SSN")).Text.Replace("-", "").Replace("-", "").Trim()))
                {
                    if (((DropDownList)WucOwnerUW3.FindControl("NameAddressPhoneSummary")).SelectedIndex == 0)
                        message.Add("Please select a guaranty for Owner4");
                }

                //PXP-3118 Rohit Thakur
                if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("SSN")).Text.Replace("-", "").Replace("-", "").Trim()))
                {
                    if (((DropDownList)WucOwnerUW4.FindControl("NameAddressPhoneSummary")).SelectedIndex == 0)
                        message.Add("Please select a guaranty for Owner5");
                }
                if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("SSN")).Text.Replace("-", "").Replace("-", "").Trim()))
                {
                    if (((DropDownList)WucOwnerUW5.FindControl("NameAddressPhoneSummary")).SelectedIndex == 0)
                        message.Add("Please select a guaranty for Owner6");
                }
            }


        }

        if (!isTrue)
            this.Master.AddMessageError("Review/Waive all the checklist items to approve the account.");


        decimal vol = app.TinfoAverageMonthlyVMCVolume;
        //for calculating AMV for all linked account
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UFAMVData uwAMVData = data.GetAMVData(UserSessions.CurrentMerchantApp.ID);
        if (vol > uwAMVData.ApprovedVolume)
        {
            decimal AMV = vol - uwAMVData.ApprovedVolume;
            uwAMVData.TotalApprovedVolume = uwAMVData.TotalApprovedVolume + AMV;
        }
        else if (vol < uwAMVData.ApprovedVolume)
        {
            decimal AMV = uwAMVData.ApprovedVolume - vol;
            uwAMVData.TotalApprovedVolume = uwAMVData.TotalApprovedVolume - AMV;
        }

        if (!isRequest)
        {
            UWHeirarchyApprovalLimit CurrentHeirarchyapproval = this.m_lstHeirarchyapproval.Where(hm => hm.AMVLowerLimit <= vol && hm.AMVUpperLimit > vol && hm.IsMCCRestrictedIndustry == FormHandler.IsMCCRestricted(app.SicCode)).FirstOrDefault();

            #region DM-5610: Dismantle approval hierarchy
            ////If the current merchant falls under approval heirarchy limits.
            //if (CurrentHeirarchyapproval != null && !(UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas)))
            //{
            //    if (HierarchyApprovalSignOff.Checked == false && (m_Status.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_Status.ToUpper() != m_CloneStatusUID.ToUpper()))
            //        this.Master.AddMessageError("Monthly volume over limit. Please request approval for this account.");

            //    //PXP-4088 RThakur
            //    if (HierarchyApprovalSignOff.Checked)
            //    {
            //        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
            //        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
            //        {
            //            if (UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled.Equals(true))
            //            {
            //                if (app != null)
            //                {
            //                    if (app.MerchantAppClone != null)
            //                    {
            //                        //if (UserSessions.CurrentMerchantApp.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED) || UserSessions.CurrentMerchantApp.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
            //                        if (app.MerchantAppClone.StatusName.ToUpper().Contains("CU") && !app.MerchantAppClone.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED)) //&& UserSessions.CurrentMerchantApp.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
            //                        {
            //                            //PXP-4979 by koshlendra start
            //                            //Code changes for PXP-10232 by koshlendra
            //                            if (app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            //                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
            //                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS)
            //                            {

            //                                message=BenfOwnerOrAuthSignCheckedForWoodFroestnMessage(message);
            //                                //For pxp-6736 - Added by abarua
            //                                //For pxp-6935 - Added by abarua
            //                                if (!IsAuthorizedSignatureChecked() && !message.Contains(Constant.AuthorizedSignatureErrorMsg))
            //                                    message.Add(Constant.AuthorizedSignatureErrorMsg);
            //                                else
            //                                {
            //                                    if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
            //                                        message.Remove(Constant.AuthorizedSignatureErrorMsg);
            //                                }


            //                                if (!IsBeneficialOwnerChecked() && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
            //                                    message.Add(Constant.BeneficialOwnerErrorMsg);
            //                                else
            //                                {
            //                                    if (message.Contains(Constant.BeneficialOwnerErrorMsg))
            //                                        message.Remove(Constant.BeneficialOwnerErrorMsg);
            //                                }

            //                            }
            //                            else
            //                            {
            //                                //PXP-4979 by koshlendra end
            //                                if (!IsBenfOwnerOrAuthSignChecked() && !message.Contains(Constant.BenefeciaryOwnerErrorMsg))
            //                                    message.Add(Constant.BenefeciaryOwnerErrorMsg);
            //                                else
            //                                {
            //                                    if (message.Contains(Constant.BenefeciaryOwnerErrorMsg))
            //                                        message.Remove(Constant.BenefeciaryOwnerErrorMsg);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //                if (achmerchant != null)
            //                {
            //                    if (achmerchant.AchMerchantClone != null)
            //                    {
            //                        //if (UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_RECEIVED) || UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
            //                        if (achmerchant.AchMerchantClone.MerchantStatusName.ToUpper().Contains("CU") && !achmerchant.AchMerchantClone.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED)) // && UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
            //                        {
            //                            //PXP-4979 by koshlendra start
            //                            //Code changes for PXP-10232 by koshlendra
            //                            if (app.MerchantAppClone.Office == CommonUtility.Util.Offices.Irvine &&
            //                                ( app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
            //                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
            //                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
            //                            {
            //                                message=BenfOwnerOrAuthSignCheckedForWoodFroestnMessage(message);
            //                                //For pxp-6736 - Added by abarua
            //                                //For pxp-6935 - Added by abarua
            //                                if (!IsAuthorizedSignatureChecked() && !message.Contains(Constant.AuthorizedSignatureErrorMsg))
            //                                    message.Add(Constant.AuthorizedSignatureErrorMsg);
            //                                else
            //                                {
            //                                    if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
            //                                        message.Remove(Constant.AuthorizedSignatureErrorMsg);
            //                                }

            //                                if (!IsBeneficialOwnerChecked() && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
            //                                    message.Add(Constant.BeneficialOwnerErrorMsg);
            //                                else
            //                                {
            //                                    if (message.Contains(Constant.BeneficialOwnerErrorMsg))
            //                                        message.Remove(Constant.BeneficialOwnerErrorMsg);
            //                                }
            //                            }
            //                            else
            //                            {
            //                                if (!IsBenfOwnerOrAuthSignChecked() && !message.Contains(Constant.BenefeciaryOwnerErrorMsg))
            //                                    message.Add(Constant.BenefeciaryOwnerErrorMsg);
            //                                else
            //                                {
            //                                    if (message.Contains(Constant.BenefeciaryOwnerErrorMsg))
            //                                        message.Remove(Constant.BenefeciaryOwnerErrorMsg);
            //                                }
            //                            }
            //                            //PXP-4979 by koshlendra end
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        if (!string.IsNullOrWhiteSpace(ResDBT.Text) && !CommonUtility.Util.IsValidInt32(ResDBT.Text))
            this.Master.AddMessageError("Please enter only numbers in DBT field.");

        if (!string.IsNullOrWhiteSpace(ResYearsOnFile.Text) && !CommonUtility.Util.IsValidInt32(ResYearsOnFile.Text))
            this.Master.AddMessageError("Please enter only numbers in Years On File field.");

        if (!string.IsNullOrWhiteSpace(ResIntelliScore.Text) && !CommonUtility.Util.IsValidInt32(ResIntelliScore.Text))
            this.Master.AddMessageError("Please enter only numbers in Intelli Score field.");

        if (!string.IsNullOrWhiteSpace(ResFinancialStabilityScore.Text) && !CommonUtility.Util.IsValidInt32(ResFinancialStabilityScore.Text))
            this.Master.AddMessageError("Please enter only numbers in Financial Stability Score field.");


        DateTime dtResIncorporationDate;
        DateTime.TryParseExact(ResIncorporationDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtResIncorporationDate);
        if (!string.IsNullOrWhiteSpace(ResIncorporationDate.Text) && !CommonUtility.Util.IsValidDateTime(dtResIncorporationDate.ToString("MM/dd/yyyy")))
            this.Master.AddMessageError("Please enter valid date in Date of Incorporation field.");

        if (m_Status.ToLower() == Constants.QUEUESTATUS_CU_DECLINED.ToLower())
        {
            if(string.IsNullOrWhiteSpace(this.MCClookup.m_SicCode))
            {
                this.Master.AddMessageError("Please enter Non-Visa MCC Code.");
            }

            if (string.IsNullOrWhiteSpace(this.MCClookup.m_VisaSicCode))
            {
                this.Master.AddMessageError("Please enter Visa MCC Code.");
            }

            if (ddlddlDeclineReason.SelectedValue == "-1")
            {
                this.Master.AddMessageError("If CU-Declined, then please select a Decline Reason");
            }

            if (string.IsNullOrWhiteSpace(tbPrimaryReason.Text))
            {
                this.Master.AddMessageError("If CU-Declined, then please add a reason.");
            }
        }

        if (m_Status.ToLower() == Constants.QUEUESTATUS_CU_WITHDRAWN.ToLower())
        {
            if (ddlddlDeclineReason.SelectedValue == "-1")
            {
                this.Master.AddMessageError("If CU-Withdrawn, then please select a Withdrawl Reason");
            }

            if (string.IsNullOrWhiteSpace(tbPrimaryReason.Text))
            {
                this.Master.AddMessageError("If CU-Withdrawn, then please add a reason.");
            }
        }

        string HighRiskDescriptor = GetHighRiskDescriptor(app);
        string SicCode = this.MCClookup.m_SicCode;
        //PXP-12932-start
        string VisaSicCode = this.MCClookup.m_VisaSicCode;
        //PXP-12932-end
        if (!isACHonly)
        {
            if ((m_Status.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && m_Status.ToUpper() != m_CloneStatusUID.ToUpper()) || isRequest)
            {
                if (SicCode == "5960" || SicCode == "5962" || SicCode == "5964" || SicCode == "5965" ||
                SicCode == "5966" || SicCode == "5967" || SicCode == "5968" || SicCode == "5969")
                {
                    if (string.IsNullOrWhiteSpace(HighRiskDescriptor))
                    {
                        this.Master.AddMessageError("Please add High Risk Descriptor.");
                    }
                }
            }
        }
        decimal totPeriodVlm = 0;
        decimal[] periodVolumes = { Convert.ToDecimal(Period1Volume.Value), Convert.ToDecimal(Period2Volume.Value), Convert.ToDecimal(Period3Volume.Value) };
        string[] ndxDays = { this.Period1NDXDays.Text.Trim(), this.Period2NDXDays.Text.Trim(), this.Period3NDXDays.Text.Trim() };
        message = FormHandler.RiskEvaluationCheck(app, m_Status, m_CloneStatusUID, ndxDays, periodVolumes, out totPeriodVlm, message);

        this.TotalPeriodVolume.Value = totPeriodVlm.ToString();
        //TODO change after confirming what to use 
        if ((uwAMVData.TotalApprovedVolume) >= 100000)
        {
            if (app.IsRolloverAccount != true && app.Office != CommonUtility.Util.Offices.LosAngeles && app.Office != CommonUtility.Util.Offices.Dallas)
            {
                if (m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_ACTIVE && m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_INACTIVE && m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_CANCELLATION && m_CloneStatusUID.ToUpper() != Constants.QUEUESTATUS_MS_PENDING_CANCELLATION)
                {
                    //PXP-3235
                    if (m_Status.ToUpper() == Constants.QUEUESTATUS_MS_ACTIVE || m_Status.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED)
                    {
                        if (this.RiskExposure.Text.Trim() != string.Empty)
                        {
                            decimal value;
                            if (decimal.TryParse(this.RiskExposure.Text.Trim(), out value))
                            {
                                if (Convert.ToDecimal(value) == 0)
                                {
                                    message.Add("Please enter Risk Exposure.");
                                }
                            }
                        }
                        else
                        {
                            message.Add("Please enter Risk Exposure.");
                        }

                        if (this.RefundDays.Text.Trim().All(char.IsDigit))
                        {
                            if (this.RefundDays.Text.Trim() != string.Empty)
                            {
                                if (Convert.ToDecimal(this.RefundDays.Text) == 0)
                                {
                                    message.Add("Please enter Refund Days.");
                                }
                            }
                            else
                            {
                                message.Add("Please enter Refund Days.");
                            }
                        }
                    }
                }
            }
        }

        //PXP-3957 RThakur
        if (isRequest)
        {
            if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
            {
                if (UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled.Equals(true))
                {
                    if (UserSessions.CurrentMerchantApp != null)
                    {
                        if (UserSessions.CurrentMerchantApp.StatusName.ToUpper().Contains("CU") || UserSessions.CurrentMerchantApp.StatusName.ToUpper().Contains("SS"))
                        {
                            //PXP-4979 by koshlendra start
                            //Code changes for PXP-10232 by koshlendra
                            if (app.MerchantAppClone.Office == CommonUtility.Util.Offices.Irvine &&
                                (app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
                            {
                                message=BenfOwnerOrAuthSignCheckedForWoodFroestnMessage(message);
                                //For pxp-6736 - Added by abarua
                                //For pxp-6935 - Added by abarua
                                if (!IsAuthorizedSignatureChecked() && !message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                    message.Add(Constant.AuthorizedSignatureErrorMsg);
                                else
                                {
                                    if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                        message.Remove(Constant.AuthorizedSignatureErrorMsg);
                                }
                                if (!IsBeneficialOwnerChecked() && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
                                    message.Add(Constant.BeneficialOwnerErrorMsg);
                                else
                                {
                                    if (message.Contains(Constant.BeneficialOwnerErrorMsg))
                                        message.Remove(Constant.BeneficialOwnerErrorMsg);
                                }
                            }
                            else
                            {
                                //PXP-4979 by koshlendra end
                                if (!IsBenfOwnerOrAuthSignChecked())
                                {
                                    if (!message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                        message.Add(Constant.BenefeciaryOwnerErrorMsg);
                                }
                                else
                                {
                                    if (message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                        message.Remove(Constant.BenefeciaryOwnerErrorMsg);
                                }
                            }
                        }
                    }
                    if (UserSessions.ActiveAchMerchant != null)
                    {
                        if (UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper().Contains("CU") || UserSessions.ActiveAchMerchant.MerchantStatusName.ToUpper().Contains("SS"))
                        {
                            //PXP-4979 by koshlendra start
                            //Code changes for PXP-10232 by koshlendra
                            if (app.MerchantAppClone.Office == CommonUtility.Util.Offices.Irvine &&
                                (app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                                || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
                            {
                                message=BenfOwnerOrAuthSignCheckedForWoodFroestnMessage(message);
                                //For pxp-6736 - Added by abarua
                                //For pxp-6935 - Added by abarua
                                if (!IsAuthorizedSignatureChecked() && !message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                    message.Add(Constant.AuthorizedSignatureErrorMsg);
                                else
                                {
                                    if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                        message.Remove(Constant.AuthorizedSignatureErrorMsg);
                                }
                                if (!IsBeneficialOwnerChecked() && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || UserSessions.CurrentMerchantApp.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
                                    message.Add(Constant.BeneficialOwnerErrorMsg);
                                else
                                {
                                    if (message.Contains(Constant.BeneficialOwnerErrorMsg))
                                        message.Remove(Constant.BeneficialOwnerErrorMsg);
                                }
                            }
                            else
                            {
                                //PXP-4979 by koshlendra end
                                if (!IsBenfOwnerOrAuthSignChecked())
                                {
                                    if (!message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                        message.Add(Constant.BenefeciaryOwnerErrorMsg);
                                }
                                else
                                {
                                    if (message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                        message.Remove(Constant.BenefeciaryOwnerErrorMsg);
                                }
                            }
                        }
                    }
                }
            }

        }
        else
        {
            if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
            {
                if (UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled.Equals(true))
                {
                    if (app != null)
                    {
                        if (app.MerchantAppClone != null)
                        {
                            if (app.MerchantAppClone.StatusName.ToUpper().Contains("CU") && !app.MerchantAppClone.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED) && UserSessions.CurrentMerchantApp.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                            {
                                //PXP-4979 code changes by koshlendra start
                                //Code changes for PXP-10232 by koshlendra
                                if (app.MerchantAppClone.Office == CommonUtility.Util.Offices.Irvine &&
                                    (app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                                    || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                                    || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
                                {
                                    message = BenfOwnerOrAuthSignCheckedForWoodFroestnMessage(message);
                                    //For pxp-6736 - Added by abarua
                                    if (!IsAuthorizedSignatureChecked() && !message.Contains(Constant.AuthorizedSignatureErrorMsg)) // && app.BusinessStructureUID.ToUpper() != Constant.OWNERSHIP_SOLE_PROPRIETORSHIP)
                                        message.Add(Constant.AuthorizedSignatureErrorMsg);
                                    else
                                    {
                                        if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                            message.Remove(Constant.AuthorizedSignatureErrorMsg);
                                    }

                                    if (!IsBeneficialOwnerChecked() && !message.Contains(Constant.BeneficialOwnerErrorMsg) && (app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_SOLE_PROPRIETORSHIP || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PRIVATE_CORPORATION || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_PARTNERSHIP || app.BusinessStructureUID.ToUpper() == Constant.OWNERSHIP_LIMITED_LIABILITY_COMPANY))
                                        message.Add(Constant.BeneficialOwnerErrorMsg);
                                    else
                                    {
                                        if (message.Contains(Constant.BeneficialOwnerErrorMsg))
                                            message.Remove(Constant.BeneficialOwnerErrorMsg);
                                    }

                                }
                                else
                                {
                                    //PXP-4979 code changes by koshlendra end
                                    if (!IsBenfOwnerOrAuthSignChecked())
                                    {
                                        if (!message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                            message.Add(Constant.BenefeciaryOwnerErrorMsg);
                                    }
                                    else
                                    {
                                        if (message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                            message.Remove(Constant.BenefeciaryOwnerErrorMsg);
                                    }
                                }
                            }
                        }
                    }
                    if (achmerchant != null)
                    {
                        if (achmerchant.AchMerchantClone != null)
                        {
                            if (achmerchant.AchMerchantClone.MerchantStatusName.ToUpper().Contains("CU") && !achmerchant.AchMerchantClone.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED) && UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                            {
                                //PXP-4979 code changes by koshlendra start
                                //Code changes for PXP-10232 by koshlendra
                                if (app.MerchantAppClone.Office == CommonUtility.Util.Offices.Irvine &&
                                    (app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                                    || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
                                    || app.MerchantAppClone.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
                                {
                                    message = BenfOwnerOrAuthSignCheckedForWoodFroestnMessage(message);
                                    //For pxp-6736 - Added by abarua
                                    if (!IsAuthorizedSignatureChecked() && !message.Contains(Constant.AuthorizedSignatureErrorMsg)) //&& app.BusinessStructureUID.ToUpper() != Constant.OWNERSHIP_SOLE_PROPRIETORSHIP)
                                        message.Add(Constant.AuthorizedSignatureErrorMsg);
                                    else
                                    {
                                        if (message.Contains(Constant.AuthorizedSignatureErrorMsg))
                                            message.Remove(Constant.AuthorizedSignatureErrorMsg);
                                    }
                                }
                                else
                                {
                                    //PXP-4979 code changes by koshlendra end
                                    if (!IsBenfOwnerOrAuthSignChecked())
                                    {
                                        if (!message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                            message.Add(Constant.BenefeciaryOwnerErrorMsg);
                                    }
                                    else
                                    {
                                        if (message.Contains(Constant.BenefeciaryOwnerErrorMsg))
                                            message.Remove(Constant.BenefeciaryOwnerErrorMsg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }



        if (message.Count > 0)
        {
            foreach (string mess in message)
                this.Master.AddMessageError(mess);
        }

        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            if (isACHonly && achmerchant != null && !string.IsNullOrWhiteSpace(m_CloneStatusUID))
            {
                ListHandler.ListFindItem(ACHStatusUID, m_CloneStatusUID);
                achmerchant.MerchantStatusUID = m_CloneStatusUID;
            }
            else
            {
                ListHandler.ListFindItem(StatusUID, m_CloneStatusUID);
                app.StatusUID = m_CloneStatusUID;
            }
            return false;
        }

    }


    /// <summary>PXP-3957 RThakur
    /// This method is used to check whether Beneficiary Owner or Actual Authorized Signature have been checked for any of the owners.
    /// </summary>
    /// <param name="statusToCheck"></param>
    /// <returns></returns>
    public bool CheckIsBenfOwnerOrAuthSignChecked(string statusToCheck)
    {
        bool retVal = false;
        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
        {
            if (UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled.Equals(true))
            {
                if (UserSessions.CurrentMerchantApp != null)
                {
                    if (UserSessions.CurrentMerchantApp.StatusUID.ToUpper().Equals(statusToCheck))
                    //if (ccstatus.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                    {
                        if (IsBenfOwnerOrAuthSignChecked()) retVal = true;
                    }
                }
                if (UserSessions.ActiveAchMerchant != null)
                {
                    if (UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper().Equals(statusToCheck))
                    //if (achStatus.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                    {
                        if (IsBenfOwnerOrAuthSignChecked()) retVal = true;
                    }
                }
            }
        }
        return retVal;
    }



    //For pxp-6935 - Added by abarua
    /// <summary>
    /// Method to check if any one of the AuthorizedSignature is checked.
    /// </summary>
    /// <returns></returns>
    public bool IsBeneficialOwnerChecked()
    {
        bool isBSignChecked = false;
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW0.FindControl("BeneficialOwner")).Checked) isBSignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW1.FindControl("BeneficialOwner")).Checked) isBSignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW2.FindControl("BeneficialOwner")).Checked) isBSignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW3.FindControl("BeneficialOwner")).Checked) isBSignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW4.FindControl("BeneficialOwner")).Checked) isBSignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW5.FindControl("BeneficialOwner")).Checked) isBSignChecked = true;
        }
        return isBSignChecked;
    }



    //For pxp-6736 - Added by abarua
    /// <summary>
    /// Method to check if any one of the AuthorizedSignature is checked.
    /// </summary>
    /// <returns></returns>
    public bool IsAuthorizedSignatureChecked()
    {
        bool isASignChecked = false;
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW0.FindControl("AuthorizedSignature")).Checked) isASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW1.FindControl("AuthorizedSignature")).Checked) isASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW2.FindControl("AuthorizedSignature")).Checked) isASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW3.FindControl("AuthorizedSignature")).Checked) isASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW4.FindControl("AuthorizedSignature")).Checked) isASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW5.FindControl("AuthorizedSignature")).Checked) isASignChecked = true;
        }
        return isASignChecked;
    }



    //PXP-2891 and PXP-3932 Rohit Thakur
    /// <summary>
    /// Method to check if any one of the BeneficialOwner or AuthorizedSignature is checked.
    /// </summary>
    /// <returns></returns>
    public bool IsBenfOwnerOrAuthSignChecked()
    {
        bool isBOwnOrASignChecked = false;
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW0.FindControl("BeneficialOwner")).Checked || ((CheckBox)WucOwnerUW0.FindControl("AuthorizedSignature")).Checked) isBOwnOrASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW1.FindControl("BeneficialOwner")).Checked || ((CheckBox)WucOwnerUW1.FindControl("AuthorizedSignature")).Checked) isBOwnOrASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW2.FindControl("BeneficialOwner")).Checked || ((CheckBox)WucOwnerUW2.FindControl("AuthorizedSignature")).Checked) isBOwnOrASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW3.FindControl("BeneficialOwner")).Checked || ((CheckBox)WucOwnerUW3.FindControl("AuthorizedSignature")).Checked) isBOwnOrASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW4.FindControl("BeneficialOwner")).Checked || ((CheckBox)WucOwnerUW4.FindControl("AuthorizedSignature")).Checked) isBOwnOrASignChecked = true;
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW5.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (((CheckBox)WucOwnerUW5.FindControl("BeneficialOwner")).Checked || ((CheckBox)WucOwnerUW5.FindControl("AuthorizedSignature")).Checked) isBOwnOrASignChecked = true;
        }
        return isBOwnOrASignChecked;
    }

    // PXP-4979code added by koshlendra start
    public IList<string> BenfOwnerOrAuthSignCheckedForWoodFroestnMessage( IList<string> msg)
    {
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW0.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (!(((CheckBox)WucOwnerUW0.FindControl("BeneficialOwner")).Checked) && !(((CheckBox)WucOwnerUW0.FindControl("AuthorizedSignature")).Checked))
            {
                if(!msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "1."))
                    msg.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "1.") ;
            }
            else
            {
                if (msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "1."))
                    msg.Remove(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "1.");
            }
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW1.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (!((CheckBox)WucOwnerUW1.FindControl("BeneficialOwner")).Checked && !(((CheckBox)WucOwnerUW1.FindControl("AuthorizedSignature")).Checked))
            {
                if(!msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "2."))
                    msg.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "2." );
            }
            else
            {
                if (msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "2."))
                    msg.Remove(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "2.");
            }
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW2.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (!(((CheckBox)WucOwnerUW2.FindControl("BeneficialOwner")).Checked) && !(((CheckBox)WucOwnerUW2.FindControl("AuthorizedSignature")).Checked))
            {
                if(!msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "3."))
                    msg.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "3." );
            }
            else
            {
                if (msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "3."))
                    msg.Remove(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "3.");
            }
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW3.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (!(((CheckBox)WucOwnerUW3.FindControl("BeneficialOwner")).Checked) && !(((CheckBox)WucOwnerUW3.FindControl("AuthorizedSignature")).Checked) )
            {
                if(!msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "4."))
                    msg.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "4." );
            }
            else
            {
                if (msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "4."))
                    msg.Remove(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "4.");
            }
        }
        if (!string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("lblFullName")).Text) || !string.IsNullOrWhiteSpace(((TextBox)WucOwnerUW4.FindControl("SSN")).Text.Replace("-", "").Replace("-", "")))
        {
            if (!(((CheckBox)WucOwnerUW4.FindControl("BeneficialOwner")).Checked )&& !(((CheckBox)WucOwnerUW4.FindControl("AuthorizedSignature")).Checked))
            {
                if (!msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest + "5."))
                    msg.Add(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "5." );
            }
            else
            {
                if (msg.Contains(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "5."))
                    msg.Remove(Constant.BenefeciaryOwnerErrorMsgForWoodForest+ "5.");
            }
        }
        return msg;
    }
    // PXP-4979code added by koshlendra end

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
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

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;
        grdLinks.Enabled = !this.EditMode;
        
        
        int checkListUID_ColIndex = grdChecklist.Columns.IndexOf(grdChecklist.Columns.OfType<BoundField>().FirstOrDefault(c => c.DataField == "CheckListUID"));
        //Niranjan:PXP-4864 AD: Capture 3DE Response Logs for TransUnion credit report
        // Ani:List all except Experian Premier
        grdChecklist.Rows.Cast<GridViewRow>().ToList().ForEach(row => {
            if (row.RowType == DataControlRowType.DataRow && 
                new[] { "7885C4F6-791B-41E8-82FF-526206F8D22E", "A12BADEA-2241-4F91-AD51-3FD7F12D104F" }.Contains(row.Cells[checkListUID_ColIndex].Text.ToUpper()) && 
                row.FindControl("chkSelect") is CheckBox chkSelect)
            {
                chkSelect.Enabled = false;
            }
        });

        this.Master.ToggleMenu(!this.EditMode);
    }

    public bool SaveCheckList()
    {
        Hashtable prms = null;
        grdChecklist.Columns[6].Visible = true;

        foreach (GridViewRow grdRow in grdChecklist.Rows)
        {
            prms = new Hashtable();
            prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
            prms.Add("@CheckListUID", grdRow.Cells[6].Text);
            prms.Add("@Checked", ((CheckBox)grdRow.Cells[1].FindControl("Checked")).Checked);

            if (grdRow.Cells[6].Text.ToUpper() == "A10C1CB1-A2F4-4C9F-996B-AD13FD8231B7")
                prms.Add("@Exception", isGuaranty);
            else
                prms.Add("@Exception", ((CheckBox)grdRow.Cells[2].FindControl("Exception")).Checked);

            prms.Add("@Notes", ((TextBox)grdRow.Cells[3].FindControl("Notes")).Text);
            prms.Add("@UserName", UserSessions.CurrentUser.UserName);
            DataAccess.DataUnderwritingDao.UpdateUWCheckList(prms);
        }

        grdChecklist.Columns[6].Visible = false;
        return true;
    }

    public bool SaveOwners()
    {
        try
        {
            MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
            DataMerchantApp data = DataAccess.DataMerchantAppDao;

            //Loading the Owners and Tradereferences again as agreement is a new object here.
            agreement.Owners = data.GetOwners(agreement.MerchantAppUID);
            agreement.TradeReferences = data.GetTradeReferences(agreement.MerchantAppUID);
            agreement.CloneMerchantApp();

            DataNetConnect objConnect = new DataNetConnect();
            ZeusBusinessProfile objPP = new ZeusBusinessProfile();
            string BID = string.Empty;

            //Save owner 1
            Owner owner = null;
            Owner clone = null;
            string ownerID = string.Empty;

            isGuaranty = true;

            if (agreement.Owners.Count > 0)
            {
                owner = (Owner)agreement.Owners[0];
                clone = (Owner)owner.Clone();
                ownerID = owner.OwnerID;
            }
            else
            {
                owner = new Owner();
                clone = new Owner();
            }


            FormBinding.BindControlsToObject(owner, WucOwnerUW0);

            owner.Position = 1;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;
            if (owner.SSN != null && owner.SSN.Length <= 4)
                owner.SSN = owner.SSN.Replace("-", "").Replace("-", "");

            if ((owner.OwnerClone != null && owner.CreditScore.Trim() != owner.OwnerClone.CreditScore.Trim()) || (owner.OwnerClone == null && !string.IsNullOrWhiteSpace(owner.CreditScore)))
            {
                if (owner.CreditDate == null)
                    owner.CreditDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(ownerID))
            {
                owner.UserCreated = UserSessions.CurrentUser.UserName;
                data.InsertOwner(owner);
            }
            else
            {
                if (string.IsNullOrEmpty(owner.OwnerID))
                    owner.OwnerID = ownerID;
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }


            BID = string.Empty;
            if (((HiddenField)WucOwnerUW0.FindControl("BusinessProfileID")) != null)
                BID = ((HiddenField)WucOwnerUW0.FindControl("BusinessProfileID")).Value;

            if (!string.IsNullOrWhiteSpace(BID))
            {
                Hashtable prms = new Hashtable();
                prms.Add("@BusinessProfileID", BID);

                objPP = objConnect.GetBusinessProfile(prms);

                if (objPP.IsCurrent == false)
                {
                    //update the premierprofile table
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.BusinessProfileID = CommonUtility.Util.if_i(BID, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    objConnect.UpdateBusinessProfile(objPP);

                    WucOwnerUW0.FormSave();
                }

            }

            if (owner.NameAddressPhoneSummary == "1" || owner.NameAddressPhoneSummary == "2")
                isGuaranty = false;

            //Save owner 2
            owner = null;
            clone = null;
            ownerID = null;

            if (agreement.Owners.Count > 1)
            {
                owner = (Owner)agreement.Owners[1];
                clone = (Owner)owner.Clone();
                ownerID = owner.OwnerID;
            }
            else
            {
                owner = new Owner();
                clone = new Owner();
            }

            FormBinding.BindControlsToObject(owner, WucOwnerUW1);

            owner.Position = 2;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;
            if (owner.SSN != null && owner.SSN.Length <= 4)
                owner.SSN = owner.SSN.Replace("-", "").Replace("-", "");

            if ((owner.OwnerClone != null && owner.CreditScore.Trim() != owner.OwnerClone.CreditScore.Trim()) || (owner.OwnerClone == null && !string.IsNullOrWhiteSpace(owner.CreditScore)))
            {
                if (owner.CreditDate == null)
                    owner.CreditDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(ownerID))
            {
                owner.UserCreated = UserSessions.CurrentUser.UserName;
                data.InsertOwner(owner);
            }
            else
            {
                if (string.IsNullOrEmpty(owner.OwnerID))
                    owner.OwnerID = ownerID;

                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            BID = string.Empty;
            if (((HiddenField)WucOwnerUW1.FindControl("BusinessProfileID")) != null)
                BID = ((HiddenField)WucOwnerUW1.FindControl("BusinessProfileID")).Value;

            if (!string.IsNullOrWhiteSpace(BID))
            {
                Hashtable prms = new Hashtable();
                prms.Add("@BusinessProfileID", BID);

                objPP = objConnect.GetBusinessProfile(prms);

                if (objPP.IsCurrent == false)
                {
                    //update the premierprofile table
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.BusinessProfileID = CommonUtility.Util.if_i(BID, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    objConnect.UpdateBusinessProfile(objPP);

                    WucOwnerUW1.FormSave();
                }

            }

            if (owner.NameAddressPhoneSummary == "1" || owner.NameAddressPhoneSummary == "2")
                isGuaranty = false;


            //Save owner 3
            owner = null;
            clone = null;
            ownerID = null;

            if (agreement.Owners.Count > 2)
            {
                owner = (Owner)agreement.Owners[2];
                clone = (Owner)owner.Clone();
                ownerID = owner.OwnerID;
            }
            else
            {
                owner = new Owner();
                clone = new Owner();
            }

            FormBinding.BindControlsToObject(owner, WucOwnerUW2);

            owner.Position = 3;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;

            if (owner.SSN != null && owner.SSN.Length <= 4)
                owner.SSN = owner.SSN.Replace("-", "").Replace("-", "");

            if ((owner.OwnerClone != null && owner.CreditScore.Trim() != owner.OwnerClone.CreditScore.Trim()) || (owner.OwnerClone == null && !string.IsNullOrWhiteSpace(owner.CreditScore)))
            {
                if (owner.CreditDate == null)
                    owner.CreditDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(ownerID))
            {
                owner.UserCreated = UserSessions.CurrentUser.UserName;
                data.InsertOwner(owner);
            }
            else
            {
                if (string.IsNullOrEmpty(owner.OwnerID))
                    owner.OwnerID = ownerID;
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            BID = string.Empty;
            if (((HiddenField)WucOwnerUW2.FindControl("BusinessProfileID")) != null)
                BID = ((HiddenField)WucOwnerUW2.FindControl("BusinessProfileID")).Value;

            if (!string.IsNullOrWhiteSpace(BID))
            {
                Hashtable prms = new Hashtable();
                prms.Add("@BusinessProfileID", BID);

                objPP = objConnect.GetBusinessProfile(prms);

                if (objPP.IsCurrent == false)
                {
                    //update the premierprofile table
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.BusinessProfileID = CommonUtility.Util.if_i(BID, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    objConnect.UpdateBusinessProfile(objPP);

                    WucOwnerUW2.FormSave();
                }

            }

            if (owner.NameAddressPhoneSummary == "1" || owner.NameAddressPhoneSummary == "2")
                isGuaranty = false;


            //Save owner 4
            owner = null;
            clone = null;
            ownerID = null;

            if (agreement.Owners.Count > 3)
            {
                owner = (Owner)agreement.Owners[3];
                clone = (Owner)owner.Clone();
                ownerID = owner.OwnerID;
            }
            else
            {
                owner = new Owner();
                clone = new Owner();
            }

            FormBinding.BindControlsToObject(owner, WucOwnerUW3);

            owner.Position = 4;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;
            if (owner.SSN != null && owner.SSN.Length <= 4)
                owner.SSN = owner.SSN.Replace("-", "").Replace("-", "");

            if ((owner.OwnerClone != null && owner.CreditScore.Trim() != owner.OwnerClone.CreditScore.Trim()) || (owner.OwnerClone == null && !string.IsNullOrWhiteSpace(owner.CreditScore)))
            {
                if (owner.CreditDate == null)
                    owner.CreditDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(ownerID))
            {
                owner.UserCreated = UserSessions.CurrentUser.UserName;
                data.InsertOwner(owner);
            }
            else
            {
                if (string.IsNullOrEmpty(owner.OwnerID))
                    owner.OwnerID = ownerID;
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            BID = string.Empty;
            if (((HiddenField)WucOwnerUW3.FindControl("BusinessProfileID")) != null)
                BID = ((HiddenField)WucOwnerUW3.FindControl("BusinessProfileID")).Value;

            if (!string.IsNullOrWhiteSpace(BID))
            {
                Hashtable prms = new Hashtable();
                prms.Add("@BusinessProfileID", BID);

                objPP = objConnect.GetBusinessProfile(prms);

                if (objPP.IsCurrent == false)
                {
                    //update the premierprofile table
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.BusinessProfileID = CommonUtility.Util.if_i(BID, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    objConnect.UpdateBusinessProfile(objPP);

                    WucOwnerUW3.FormSave();
                }

            }

            if (owner.NameAddressPhoneSummary == "1" || owner.NameAddressPhoneSummary == "2")
                isGuaranty = false;

            //PXP-3118 Rohit Thakur
            //Save owner 5
            owner = null;
            clone = null;
            ownerID = null;

            if (agreement.Owners.Count > 4)
            {
                owner = (Owner)agreement.Owners[4];
                clone = (Owner)owner.Clone();
                ownerID = owner.OwnerID;
            }
            else
            {
                owner = new Owner();
                clone = new Owner();
            }

            FormBinding.BindControlsToObject(owner, WucOwnerUW4);

            owner.Position = 5;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;
            if (owner.SSN != null && owner.SSN.Length <= 5)
                owner.SSN = owner.SSN.Replace("-", "").Replace("-", "");

            if ((owner.OwnerClone != null && owner.CreditScore.Trim() != owner.OwnerClone.CreditScore.Trim()) || (owner.OwnerClone == null && !string.IsNullOrWhiteSpace(owner.CreditScore)))
            {
                if (owner.CreditDate == null)
                    owner.CreditDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(ownerID))
            {
                owner.UserCreated = UserSessions.CurrentUser.UserName;
                data.InsertOwner(owner);
            }
            else
            {
                if (string.IsNullOrEmpty(owner.OwnerID))
                    owner.OwnerID = ownerID;
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            BID = string.Empty;
            if (((HiddenField)WucOwnerUW4.FindControl("BusinessProfileID")) != null)
                BID = ((HiddenField)WucOwnerUW4.FindControl("BusinessProfileID")).Value;

            if (!string.IsNullOrWhiteSpace(BID))
            {
                Hashtable prms = new Hashtable();
                prms.Add("@BusinessProfileID", BID);

                objPP = objConnect.GetBusinessProfile(prms);

                if (objPP.IsCurrent == false)
                {
                    //update the premierprofile table
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.BusinessProfileID = CommonUtility.Util.if_i(BID, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    objConnect.UpdateBusinessProfile(objPP);

                    WucOwnerUW4.FormSave();
                }

            }

            if (owner.NameAddressPhoneSummary == "1" || owner.NameAddressPhoneSummary == "2")
                isGuaranty = false;

            //Save owner 6
            owner = null;
            clone = null;
            ownerID = null;

            if (agreement.Owners.Count > 5)
            {
                owner = (Owner)agreement.Owners[5];
                clone = (Owner)owner.Clone();
                ownerID = owner.OwnerID;
            }
            else
            {
                owner = new Owner();
                clone = new Owner();
            }

            FormBinding.BindControlsToObject(owner, WucOwnerUW5);

            owner.Position = 6;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;
            if (owner.SSN != null && owner.SSN.Length <= 6)
                owner.SSN = owner.SSN.Replace("-", "").Replace("-", "");

            if ((owner.OwnerClone != null && owner.CreditScore.Trim() != owner.OwnerClone.CreditScore.Trim()) || (owner.OwnerClone == null && !string.IsNullOrWhiteSpace(owner.CreditScore)))
            {
                if (owner.CreditDate == null)
                    owner.CreditDate = DateTime.Now;
            }

            if (string.IsNullOrEmpty(ownerID))
            {
                owner.UserCreated = UserSessions.CurrentUser.UserName;
                data.InsertOwner(owner);
            }
            else
            {
                if (string.IsNullOrEmpty(owner.OwnerID))
                    owner.OwnerID = ownerID;
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            BID = string.Empty;
            if (((HiddenField)WucOwnerUW5.FindControl("BusinessProfileID")) != null)
                BID = ((HiddenField)WucOwnerUW5.FindControl("BusinessProfileID")).Value;

            if (!string.IsNullOrWhiteSpace(BID))
            {
                Hashtable prms = new Hashtable();
                prms.Add("@BusinessProfileID", BID);

                objPP = objConnect.GetBusinessProfile(prms);

                if (objPP.IsCurrent == false)
                {
                    //update the premierprofile table
                    objPP.UserModified = UserSessions.CurrentUser.UserName;
                    objPP.BusinessProfileID = CommonUtility.Util.if_i(BID, 0);
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                    objPP.IsCurrent = true;

                    objConnect.UpdateBusinessProfile(objPP);

                    WucOwnerUW5.FormSave();
                }

            }

            if (owner.NameAddressPhoneSummary == "1" || owner.NameAddressPhoneSummary == "2")
                isGuaranty = false;
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public bool SaveRiskEvaluation()
    {
        return true;
    }

    public bool SaveFinancials()
    {
        return true;
    }

    private void BindData()
    {
        grdLinks.PageSize = this.PageSize;
        grdLinks.PageIndex = this.CurrentPage - 1;

        grdLinks.DataBind();

        pnlRecords.Visible = (grdLinks.Rows.Count > 0);
        pnlNoRecords.Visible = !pnlRecords.Visible;
        pnlRecords.Enabled = pnlRecords.Visible;

        cboPageSize.Enabled = true;
        lblRecordCount.Enabled = true;
        ddlRange.Enabled = ddlSelectExportFormat.Enabled = btnExport1.Enabled = pnlRecords.Visible;
    }

    private void BindOwners(MerchantApp agreement)
    {
        ExtractCorporateBusinessFromOwners(agreement);
        if (agreement.Owners.Count > 0)
        {
            FormBinding.BindObjectToControls(agreement.Owners[0], WucOwnerUW0);
            WucOwnerUW0.SetFullName = agreement.Owners[0].FirstName + " " + agreement.Owners[0].LastName;
            WucOwnerUW0.SaveButton.CommandArgument = agreement.Owners[0].OwnerID;
            //WucOwnerUW0.btnHistory.Enabled = true;
            HiddenField hidAddress = ((HiddenField)WucOwnerUW0.FindControl("hidAddress"));
            if (hidAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(agreement.Owners[0].Address1) && !string.IsNullOrWhiteSpace(agreement.Owners[0].State.Replace("-1", ""))
                    && !string.IsNullOrWhiteSpace(agreement.Owners[0].City) && !string.IsNullOrWhiteSpace(agreement.Owners[0].Zip))
                {
                    hidAddress.Value = agreement.Owners[0].Address1 + ", " + agreement.Owners[0].City + ", " + agreement.Owners[0].State + ", " + agreement.Owners[0].Zip;
                }
            }

            WucOwnerUW0.FillBusinessProfileReponse();

        }


        if (agreement.Owners.Count > 1)
        {
            FormBinding.BindObjectToControls(agreement.Owners[1], WucOwnerUW1);
            WucOwnerUW1.SetFullName = agreement.Owners[1].FirstName + " " + agreement.Owners[1].LastName;
            WucOwnerUW1.SaveButton.CommandArgument = agreement.Owners[1].OwnerID;
            // WucOwnerUW1.btnHistory.Enabled = true;
            HiddenField hidAddress = ((HiddenField)WucOwnerUW1.FindControl("hidAddress"));
            if (hidAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(agreement.Owners[1].Address1) && !string.IsNullOrWhiteSpace(agreement.Owners[1].State.Replace("-1", ""))
                    && !string.IsNullOrWhiteSpace(agreement.Owners[1].City) && !string.IsNullOrWhiteSpace(agreement.Owners[1].Zip))
                {
                    hidAddress.Value = agreement.Owners[1].Address1 + ", " + agreement.Owners[1].City + ", " + agreement.Owners[1].State + ", " + agreement.Owners[1].Zip;
                }
            }

            WucOwnerUW1.FillBusinessProfileReponse();
        }


        if (agreement.Owners.Count > 2)
        {
            FormBinding.BindObjectToControls(agreement.Owners[2], WucOwnerUW2);
            WucOwnerUW2.SetFullName = agreement.Owners[2].FirstName + " " + agreement.Owners[2].LastName;
            WucOwnerUW2.SaveButton.CommandArgument = agreement.Owners[2].OwnerID;
            // WucOwnerUW2.btnHistory.Enabled = true;
            HiddenField hidAddress = ((HiddenField)WucOwnerUW2.FindControl("hidAddress"));
            if (hidAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(agreement.Owners[2].Address1) && !string.IsNullOrWhiteSpace(agreement.Owners[2].State.Replace("-1", ""))
                    && !string.IsNullOrWhiteSpace(agreement.Owners[2].City) && !string.IsNullOrWhiteSpace(agreement.Owners[2].Zip))
                {
                    hidAddress.Value = agreement.Owners[2].Address1 + ", " + agreement.Owners[2].City + ", " + agreement.Owners[2].State + ", " + agreement.Owners[2].Zip;
                }
            }

            WucOwnerUW2.FillBusinessProfileReponse();
        }


        if (agreement.Owners.Count > 3)
        {
            FormBinding.BindObjectToControls(agreement.Owners[3], WucOwnerUW3);
            WucOwnerUW3.SetFullName = agreement.Owners[3].FirstName + " " + agreement.Owners[3].LastName;
            WucOwnerUW3.SaveButton.CommandArgument = agreement.Owners[3].OwnerID;
            //WucOwnerUW3.btnHistory.Enabled = true;
            HiddenField hidAddress = ((HiddenField)WucOwnerUW3.FindControl("hidAddress"));
            if (hidAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(agreement.Owners[3].Address1) && !string.IsNullOrWhiteSpace(agreement.Owners[3].State.Replace("-1", ""))
                    && !string.IsNullOrWhiteSpace(agreement.Owners[3].City) && !string.IsNullOrWhiteSpace(agreement.Owners[3].Zip))
                {
                    hidAddress.Value = agreement.Owners[3].Address1 + ", " + agreement.Owners[3].City + ", " + agreement.Owners[3].State + ", " + agreement.Owners[3].Zip;
                }
            }

            WucOwnerUW3.FillBusinessProfileReponse();
        }

        //PXP-3118 Rohit Thakur
        if (agreement.Owners.Count > 4)
        {
            FormBinding.BindObjectToControls(agreement.Owners[4], WucOwnerUW4);
            WucOwnerUW4.SetFullName = agreement.Owners[4].FirstName + " " + agreement.Owners[4].LastName;
            WucOwnerUW4.SaveButton.CommandArgument = agreement.Owners[4].OwnerID;
            //WucOwnerUW3.btnHistory.Enabled = true;
            HiddenField hidAddress = ((HiddenField)WucOwnerUW4.FindControl("hidAddress"));
            if (hidAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(agreement.Owners[4].Address1) && !string.IsNullOrWhiteSpace(agreement.Owners[4].State.Replace("-1", ""))
                    && !string.IsNullOrWhiteSpace(agreement.Owners[4].City) && !string.IsNullOrWhiteSpace(agreement.Owners[4].Zip))
                {
                    hidAddress.Value = agreement.Owners[4].Address1 + ", " + agreement.Owners[4].City + ", " + agreement.Owners[4].State + ", " + agreement.Owners[4].Zip;
                }
            }

            WucOwnerUW4.FillBusinessProfileReponse();
        }

        if (agreement.Owners.Count > 5)
        {
            FormBinding.BindObjectToControls(agreement.Owners[5], WucOwnerUW5);
            WucOwnerUW5.SetFullName = agreement.Owners[5].FirstName + " " + agreement.Owners[5].LastName;
            WucOwnerUW5.SaveButton.CommandArgument = agreement.Owners[5].OwnerID;
            //WucOwnerUW3.btnHistory.Enabled = true;
            HiddenField hidAddress = ((HiddenField)WucOwnerUW5.FindControl("hidAddress"));
            if (hidAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(agreement.Owners[5].Address1) && !string.IsNullOrWhiteSpace(agreement.Owners[5].State.Replace("-1", ""))
                    && !string.IsNullOrWhiteSpace(agreement.Owners[5].City) && !string.IsNullOrWhiteSpace(agreement.Owners[5].Zip))
                {
                    hidAddress.Value = agreement.Owners[5].Address1 + ", " + agreement.Owners[5].City + ", " + agreement.Owners[5].State + ", " + agreement.Owners[5].Zip;
                }
            }

            WucOwnerUW5.FillBusinessProfileReponse();
        }

        if (agreement.CorpBuzOwners.OwnerCatagoryID.Equals(1))
            wucCorpBuzUW1.FillCorporateBusiness(agreement.CorpBuzOwners);

        // pnlCredit.Visible = (agreement.Owners.Count > 0);

        WucOwnerUW1.SetReadOnly();
        WucOwnerUW2.SetReadOnly();
        WucOwnerUW3.SetReadOnly();
        WucOwnerUW0.SetReadOnly();
        //PXP-2883
        WucOwnerUW4.SetReadOnly();
        WucOwnerUW5.SetReadOnly();
    }

    public static void ExtractCorporateBusinessFromOwners(MerchantApp agreement)
    {
        foreach (Owner item in agreement.Owners)
        {
            if (item.OwnerCatagoryID == 1)
            {
                agreement.CorpBuzOwners = item;
                agreement.Owners.Remove(item);
                break;
            }
        }
    }
    private void BindChecklist()
    {
        grdChecklist.Columns[6].Visible = true;
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        DataSet ds = data.GetUWCheckList(UserSessions.CurrentMerchantApp.MerchantAppUID);
        grdChecklist.DataSource = ds.Tables[0].DefaultView;
        grdChecklist.DataBind();
        grdChecklist.Columns[6].Visible = false;
        //Niranjan:PXP-4864 AD: Capture 3DE Response Logs for TransUnion credit report
        string reschk = (ds.Tables[0].Rows[5]["ResponseLog"]).ToString();
        if (reschk.Contains("pass"))
        {
            grdChecklist.Rows[9].Cells[4].Text = "Not Initiated";
        }

        grdChecklist.Columns[4].Visible = isIrvineMerchant;
        grdChecklist.Columns[5].Visible = isIrvineMerchant;
    }

    private void BindFinancials()
    {
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        DataSet ds = data.GetUWFinancials(UserSessions.CurrentMerchantApp.MerchantAppUID);

        IList<UWFinancials> objFinance = new List<UWFinancials>();
        objFinance.Add(new UWFinancials());

        lnkFinance.Visible = ds.Tables[0].Rows.Count > 0;
        lblNoFinance.Visible = ds.Tables[0].Rows.Count == 0;
        BindGrid(ds.Tables[0], grdFinancial, objFinance, null);

        lnkIncome.Visible = ds.Tables[1].Rows.Count > 0;
        lblNoIncome.Visible = ds.Tables[1].Rows.Count == 0;
        BindGrid(ds.Tables[1], grdIncome, objFinance, null);

        lnkBank.Visible = ds.Tables[2].Rows.Count > 0;
        lblNoBank.Visible = ds.Tables[2].Rows.Count == 0;
        BindGrid(ds.Tables[2], grdBank, objFinance, null);
    }

    private void BindFulfillment()
    {
        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UWFulfillment uwfulfillment = data.GetUWFulfillment(UserSessions.CurrentMerchantApp.ID);

        FormBinding.BindObjectToControls(uwfulfillment, pnlUWFulfillment);
    }

    public void BindGrid(DataTable dt, GridView grd, IList<UWFinancials> objFinance, IList<UWRiskEvaluation> objRisk)
    {
        if (dt.Rows.Count == 0)
        {
            if (objFinance != null)
            {
                grd.DataSource = objFinance;
                grd.DataBind();
            }
            else
            {
                grd.DataSource = objRisk;
                grd.DataBind();
                grd.FooterRow.Visible = false;
            }

            // Get the total number of columns in the GridView to know what the Column Span should be       
            int columnsCount = grd.Columns.Count;
            grd.Rows[0].Cells.Clear();

            // clear all the cells in the row      
            grd.Rows[0].Cells.Add(new TableCell());

            //add a new blank cell      
            grd.Rows[0].Cells[0].ColumnSpan = columnsCount;
            grd.Rows[0].Cells[0].CssClass = "EmptyDataRowStyle";

            //set No Results found to the new added cell       
            grd.Rows[0].Cells[0].Text = ".....No Records....";
            grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

            grd.Visible = false;
        }
        else
        {
            grd.DataSource = dt.DefaultView;
            grd.DataBind();
        }
    }

    private void SearchMCC()
    {
        MerchantFacade facade = new MerchantFacade();
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrWhiteSpace(txtMCC.Text))
            prms.Add("@Name", txtMCC.Text);

        if (!string.IsNullOrWhiteSpace(txtMCCDesc.Text))
            prms.Add("@Description", txtMCCDesc.Text);

        DataSet ds = facade.GetSicCodes(prms);
        DataView dv = ds.Tables[0].DefaultView;

        grd.DataSource = dv;
        grd.DataBind();

        lblNoRecords.Visible = (grd.Rows.Count == 0);
    }



    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;

        switch (lnk.CommandName)
        {
            case "Finance":

                grdFinancial.Visible = true;
                lblNoFinance.Visible = false;

                if (grdFinancial.Rows[0].Cells[0].Text.Contains("No Records"))
                {
                    grdFinancial.Rows[0].Visible = false;
                    grdFinancial.Rows[0].Cells.Clear();
                }
                grdFinancial.FooterRow.Visible = true;

                break;

            case "Income":

                grdIncome.Visible = true;
                lblNoIncome.Visible = false;

                if (grdIncome.Rows[0].Cells[0].Text.Contains("No Records"))
                {
                    grdIncome.Rows[0].Visible = false;
                    grdIncome.Rows[0].Cells.Clear();
                }
                grdIncome.FooterRow.Visible = true;

                break;

            case "Bank":

                grdBank.Visible = true;
                lblNoBank.Visible = false;

                if (grdBank.Rows[0].Cells[0].Text.Contains("No Records"))
                {
                    grdBank.Rows[0].Visible = false;
                    grdBank.Rows[0].Cells.Clear();
                }
                grdBank.FooterRow.Visible = true;

                break;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string fileName = string.Empty;

        switch (lnk.CommandName)
        {
            case "Finance":

                fileName = "Financial Statements for" + UserSessions.CurrentMerchantApp.BusinessDBAName + ".xls";
                FormHandler.Export2Excel(fileName, grdFinancial);
                break;

            case "Income":

                fileName = "Income Statements for" + UserSessions.CurrentMerchantApp.BusinessDBAName + ".xls";
                FormHandler.Export2Excel(fileName, grdIncome);
                break;

            case "Bank":

                fileName = "Bank Statements for" + UserSessions.CurrentMerchantApp.BusinessDBAName + ".xls";
                FormHandler.Export2Excel(fileName, grdBank);
                break;
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        BindData();
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                break;
            default:
                break;
        }
    }

    protected void grdCheck_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                if (DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "Is3DEVendor")))
                {
                    ((CheckBox)e.Row.FindControl("chkSelect")).Visible = true;
                }
                else
                {
                    ((CheckBox)e.Row.FindControl("chkSelect")).Visible = false;
                }

                ((CheckBox)e.Row.FindControl("Checked")).Checked = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "Checked"));

                if (DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "IsException")))
                    ((CheckBox)e.Row.FindControl("Exception")).Checked = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "Exception"));
                else
                    ((CheckBox)e.Row.FindControl("Exception")).Visible = false;

                if (DataBinder.Eval(e.Row.DataItem, "CheckListUID").ToString().ToUpper() == "A10C1CB1-A2F4-4C9F-996B-AD13FD8231B7") // PG signature
                    ((CheckBox)e.Row.FindControl("Exception")).Enabled = false;

                ((TextBox)e.Row.FindControl("Notes")).Text = DataLayer.Field2Str(DataBinder.Eval(e.Row.DataItem, "Notes"));

                ((Label)e.Row.FindControl("ResponseLog")).Text = DataLayer.Field2Str(DataBinder.Eval(e.Row.DataItem, "ResponseLog"));

                break;

            default:
                break;
        }
    }

    protected void grd1_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        BindData();
    }

    protected void grd1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        grdLinks.PageIndex = e.NewPageIndex;
        BindData();
    }

    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();
        SearchParameter app;
        MerchantApp agreement = UserSessions.CurrentMerchantApp;

        prms.Add("@MerchantID", agreement.ID);

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            prms.Add("@PageSize", this.PageSize);

            prms.Add("@CurrentPage", this.CurrentPage);

            grd.PageSize = this.PageSize;
            grd.PageIndex = this.CurrentPage - 1;

            if (string.IsNullOrWhiteSpace(this.SortOrder))
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);
        }
        else
        {
            prms.Add("@ID", -1);
        }

        e.InputParameters[0] = prms;
        e.InputParameters[3] = agreement.SettlePlatformMid;
        e.InputParameters[4] = agreement.AchID;
        lblRecordCount.Text = "<b>Total Records Found:</b> " + DataMerchantAppPaging.GetMultiLinkPagingCount(prms, 0, 0).ToString();
    }

    protected void grdFinance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandSource is ImageButton))
            return;

        ImageButton btn = (ImageButton)e.CommandSource;
        GridViewRow Row1 = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UWFinancials objFinance = new UWFinancials();

        switch (e.CommandName)
        {
            case "AddUW":

                GridViewRow grdRow = grdFinancial.FooterRow;
                WebDatePicker ddlDate = (WebDatePicker)grdRow.Cells[1].FindControl("Period");

                if (ddlDate != null && !string.IsNullOrEmpty(ddlDate.Text))
                {
                    objFinance.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    objFinance.Period = ddlDate.Text;
                    objFinance.CurrentAsset = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("CurrentAsset")).Value);
                    objFinance.CurrentLiability = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("CurrentLiability")).Value);
                    objFinance.TotalAsset = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("TotalAsset")).Value);
                    objFinance.TotalLiability = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("TotalLiability")).Value);

                    data.UpdateUWFinanceStatements(objFinance);

                    BindFinancials();
                }
                else
                {
                    FormHandler.DisplayMessage(Page.ClientScript, "Please select a Date.");
                }

                break;

            case "CancelUW":

                grdFinancial.EditIndex = -1;
                BindFinancials();

                break;

            case "EditUW":

                lnkAddFinance.Enabled = false;
                grdFinancial.EditIndex = Row1.RowIndex;
                BindFinancials();

                break;

            case "UpdateUW":

                objFinance = new UWFinancials();

                Label Date = ((Label)Row1.Cells[1].FindControl("lblPeriod1"));

                if (Date != null && !string.IsNullOrEmpty(Date.Text))
                {
                    objFinance.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    objFinance.Period = Date.Text;
                    objFinance.CurrentAsset = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("CurrentAsset1")).Value);
                    objFinance.CurrentLiability = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("CurrentLiability1")).Value);
                    objFinance.TotalAsset = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("TotalAsset1")).Value);
                    objFinance.TotalLiability = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("TotalLiability1")).Value);

                    int rows = data.UpdateUWFinanceStatements(objFinance);

                    grdFinancial.EditIndex = -1;
                    BindFinancials();
                }

                lnkAddFinance.Enabled = true;

                break;
        }
    }

    protected void grdFinance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                decimal Asset = 0.0M, Liability = 0.0M, TotalAsset = 0.0M, TotalLiability = 0.0M;

                GridViewRow Row1 = e.Row;

                if (grdFinancial.EditIndex != e.Row.RowIndex || grdFinancial.EditIndex == -1)
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");

                    if (((Label)Row1.FindControl("lblAsset")) != null)
                    {
                        ((Label)Row1.FindControl("lblAsset")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "CurrentAsset")).ToString("0.00");//"c");
                        Asset = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "CurrentAsset"));
                    }

                    if (((Label)Row1.FindControl("lblLiability")) != null)
                    {
                        ((Label)Row1.FindControl("lblLiability")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "CurrentLiability")).ToString("0.00");//"c");
                        Liability = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "CurrentLiability"));
                    }

                    if (((Label)Row1.FindControl("lblTotalAsset")) != null)
                    {
                        ((Label)Row1.FindControl("lblTotalAsset")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "TotalAsset")).ToString("0.00");//"c");
                        TotalAsset = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "TotalAsset"));
                    }

                    if (((Label)Row1.FindControl("lblTotalLiability")) != null)
                    {
                        ((Label)Row1.FindControl("lblTotalLiability")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "TotalLiability")).ToString("0.00");//"c");
                        TotalLiability = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "TotalLiability"));
                    }
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");

                    if (((WebNumericEditor)Row1.FindControl("CurrentAsset1")) != null)
                        Asset = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("CurrentAsset1")).Value);

                    if (((WebNumericEditor)Row1.FindControl("CurrentLiability1")) != null)
                        Liability = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("CurrentLiability1")).Value);

                    if (((WebNumericEditor)Row1.FindControl("TotalAsset1")) != null)
                        TotalAsset = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("TotalAsset1")).Value);

                    if (((WebNumericEditor)Row1.FindControl("TotalLiability1")) != null)
                        TotalLiability = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("TotalLiability1")).Value);
                }

                e.Row.Cells[4].Text = Liability > 0.0M ? Convert.ToDecimal(Asset / Liability).ToString("#0.00%") : "0.0%";
                e.Row.Cells[5].Text = Convert.ToDecimal(Asset - Liability).ToString("0.00");//"c");
                e.Row.Cells[8].Text = Liability > 0.0M ? Convert.ToDecimal(Asset / Liability).ToString("#0.00%") : "0.0%";
                e.Row.Cells[9].Text = Convert.ToDecimal(TotalAsset - TotalLiability).ToString("0.00");//"c");

                if (DataBinder.Eval(e.Row.DataItem, "Period").ToString() == "Total" || DataBinder.Eval(e.Row.DataItem, "Period").ToString() == "Average")
                {
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                    e.Row.Font.Bold = true;
                }

                break;

            default:
                break;
        }
    }

    protected void grdIncome_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandSource is ImageButton))
            return;

        ImageButton btn = (ImageButton)e.CommandSource;
        GridViewRow Row1 = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UWFinancials objFinance = new UWFinancials();

        switch (e.CommandName)
        {
            case "AddUW":

                GridViewRow grdRow = grdIncome.FooterRow;
                WebDatePicker ddlDate = (WebDatePicker)grdRow.Cells[1].FindControl("Period");

                if (ddlDate != null && !string.IsNullOrEmpty(ddlDate.Text))
                {
                    objFinance.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    objFinance.Period = ddlDate.Text;
                    objFinance.GrossSales = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("GrossSales")).Value);
                    objFinance.NetIncome = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("NetIncome")).Value);

                    data.UpdateUWIncomeStatements(objFinance);

                    BindFinancials();
                }
                else
                {
                    FormHandler.DisplayMessage(Page.ClientScript, "Please select a Date.");
                }
                break;

            case "CancelUW":

                grdIncome.EditIndex = -1;
                BindFinancials();

                break;

            case "EditUW":

                lnkAddIncome.Enabled = false;
                grdIncome.EditIndex = Row1.RowIndex;
                BindFinancials();

                break;

            case "UpdateUW":

                objFinance = new UWFinancials();

                Label Date = ((Label)Row1.Cells[1].FindControl("lblPeriod1"));

                if (Date != null && !string.IsNullOrEmpty(Date.Text))
                {
                    objFinance.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    objFinance.Period = Date.Text;
                    objFinance.GrossSales = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("GrossSales1")).Value);
                    objFinance.NetIncome = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("NetIncome1")).Value);

                    int rows = data.UpdateUWIncomeStatements(objFinance);

                    lnkAddIncome.Enabled = true;
                    grdIncome.EditIndex = -1;
                    BindFinancials();
                }

                break;
        }

    }

    protected void grdIncome_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                decimal GrossSales = 0.0M, NetIncome = 0.0M;
                GridViewRow Row1 = e.Row;

                if (grdIncome.EditIndex != e.Row.RowIndex || grdIncome.EditIndex == -1)
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");


                    if (((Label)Row1.FindControl("lblGrossSales")) != null)
                    {
                        ((Label)Row1.FindControl("lblGrossSales")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "GrossSales")).ToString("0.00");//"c");
                        GrossSales = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "GrossSales"));
                    }

                    if (((Label)Row1.FindControl("lblNetIncome")) != null)
                    {
                        ((Label)Row1.FindControl("lblNetIncome")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "NetIncome")).ToString("0.00");//"c");
                        NetIncome = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "NetIncome"));
                    }
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                }

                if (DataBinder.Eval(e.Row.DataItem, "Period").ToString() == "Total" || DataBinder.Eval(e.Row.DataItem, "Period").ToString() == "Average")
                {
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                    e.Row.Font.Bold = true;
                }

                break;

            default:
                break;
        }
    }

    protected void grdBank_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandSource is ImageButton))
            return;

        ImageButton btn = (ImageButton)e.CommandSource;
        GridViewRow Row1 = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

        DataUnderwritng data = DataAccess.DataUnderwritingDao;
        UWFinancials objFinance = new UWFinancials();

        switch (e.CommandName)
        {
            case "AddUW":

                GridViewRow grdRow = grdBank.FooterRow;
                WebDatePicker ddlDate = (WebDatePicker)grdRow.Cells[1].FindControl("Period");

                if (ddlDate != null && !string.IsNullOrEmpty(ddlDate.Text))
                {
                    objFinance.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    objFinance.Period = ddlDate.Text;
                    objFinance.SavingsBalance = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("SavingsBalance")).Value);
                    objFinance.CheckingBalance = DataLayer.Field2Dec(((WebNumericEditor)grdRow.FindControl("CheckingBalance")).Value);

                    data.UpdateUWBankStatements(objFinance);

                    BindFinancials();
                }
                else
                {
                    FormHandler.DisplayMessage(Page.ClientScript, "Please select a Date.");
                }
                break;

            case "CancelUW":

                grdFinancial.EditIndex = -1;
                BindFinancials();

                break;

            case "EditUW":

                lnkAddBank.Enabled = false;
                grdBank.EditIndex = Row1.RowIndex;
                BindFinancials();

                break;

            case "UpdateUW":

                objFinance = new UWFinancials();

                Label Date = ((Label)Row1.Cells[1].FindControl("lblPeriod1"));

                if (Date != null && !string.IsNullOrEmpty(Date.Text))
                {
                    objFinance.MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    objFinance.Period = Date.Text;
                    objFinance.SavingsBalance = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("SavingsBalance1")).Value);
                    objFinance.CheckingBalance = DataLayer.Field2Dec(((WebNumericEditor)Row1.FindControl("CheckingBalance1")).Value);

                    int rows = data.UpdateUWBankStatements(objFinance);

                    grdBank.EditIndex = -1;
                    BindFinancials();
                }

                lnkAddBank.Enabled = true;

                break;
        }
    }

    protected void grdBank_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                decimal SavingsBalance = 0.0M, CheckingBalance = 0.0M;

                GridViewRow Row1 = e.Row;

                if (grdBank.EditIndex != e.Row.RowIndex || grdBank.EditIndex == -1)
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");


                    if (((Label)Row1.FindControl("lblCheckingBalance")) != null)
                    {
                        ((Label)Row1.FindControl("lblCheckingBalance")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "CheckingBalance")).ToString("0.00");//"c");
                        CheckingBalance = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "CheckingBalance"));
                    }

                    if (((Label)Row1.FindControl("lblSavingsBalance")) != null)
                    {
                        ((Label)Row1.FindControl("lblSavingsBalance")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "SavingsBalance")).ToString("0.00");//"c");
                        SavingsBalance = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "SavingsBalance"));
                    }
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                }

                if (DataBinder.Eval(e.Row.DataItem, "Period").ToString() == "Total" || DataBinder.Eval(e.Row.DataItem, "Period").ToString() == "Average")
                {
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                    e.Row.Font.Bold = true;
                }

                break;

            default:
                break;
        }
    }

    protected void btnExport1_Click(object sender, EventArgs e)
    {
        DropDownList ddl = ddlSelectExportFormat;

        switch (ddl.SelectedValue.Trim().ToLower())
        {
            case "excel":

                if (ddlRange.SelectedIndex == 0)
                {
                    this.BindData();
                    FormHandler.Export2Excel("PxpDailySummary.xls", grdLinks);
                }
                else
                {
                    this.CurrentPage = 1;
                    int pagesize = this.PageSize;
                    this.PageSize = 10000;
                    grd.PageSize = this.PageSize;
                    this.BindData();
                    FormHandler.Export2Excel("PxpDailySummary.xls", grdLinks);
                    this.PageSize = pagesize;
                    grd.PageSize = this.PageSize;
                }

                this.BindData();
                break;

            case "csv":

                if (ddlRange.SelectedIndex == 0)
                {
                    this.BindData();

                    FormHandler.Export2CSV("PxpDailySummary.cvs", grdLinks);
                }
                else
                {
                    this.CurrentPage = 1;
                    int pagesize = this.PageSize;
                    this.PageSize = 10000;
                    grd.PageSize = this.PageSize;
                    this.BindData();
                    FormHandler.Export2CSV("PxpDailySummary.cvs", grdLinks);

                    this.PageSize = pagesize;
                    grd.PageSize = this.PageSize;
                }

                this.BindData();
                break;

            case "tab":

                if (ddlRange.SelectedIndex == 0)
                {
                    this.BindData();
                    FormHandler.Export2Tab("PxpDailySummary.txt", grdLinks);
                }
                else
                {
                    this.CurrentPage = 1;
                    int pagesize = this.PageSize;
                    this.PageSize = 10000;
                    grd.PageSize = this.PageSize;
                    this.BindData();
                    FormHandler.Export2Tab("PxpDailySummary.txt", grdLinks);

                    this.PageSize = pagesize;
                    grd.PageSize = this.PageSize;
                }

                this.BindData();
                break;

            case "pdf":

                if (ddlRange.SelectedIndex == 0)
                {
                    this.BindData();

                    // this strips out the html for the "Grand Total";
                    grd.FooterRow.Cells[6].Text = "";
                    FormHandler.ExportToPDF(grdLinks, true, "PxpDailySummary", 7);
                }
                else
                {
                    this.CurrentPage = 1;
                    int pagesize = this.PageSize;
                    this.PageSize = 10000;
                    grd.PageSize = this.PageSize;
                    this.BindData();

                    grd.FooterRow.Cells[6].Text = "";
                    FormHandler.ExportToPDF(grdLinks, true, "PxpDailySummary", 7);
                    this.PageSize = pagesize;
                    grd.PageSize = this.PageSize;
                }

                this.BindData();
                break;
        }
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

    protected void CheckApprovedSalesChanges(MerchantApp app, Underwriting objuw)
    {
        string notes = string.Empty, subject = string.Empty;
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        string value1 = string.Empty, value2 = string.Empty;

        User user = UserSessions.CurrentUser;
        MerchantApp clone = app.MerchantAppClone;

        if (app.TinfoAverageMonthlyVMCVolume != clone.TinfoAverageMonthlyVMCVolume)
        {
            value1 = string.IsNullOrEmpty(clone.TinfoAverageMonthlyVMCVolume.ToString("0.00"))/*"c"))*/ ? "NULL" : clone.TinfoAverageMonthlyVMCVolume.ToString("0.00");//"c");
            value2 = string.IsNullOrEmpty(app.TinfoAverageMonthlyVMCVolume.ToString("0.00"))/*"c"))*/  ? "NULL" : app.TinfoAverageMonthlyVMCVolume.ToString("0.00");//"c");
            notes = "Approved Monthly Volume has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.MonthendApproved != clone.MonthendApproved)
        {
            value1 = string.IsNullOrEmpty(clone.MonthendApproved.ToString()) ? "NULL" : clone.MonthendApproved.ToString();
            value2 = string.IsNullOrEmpty(app.MonthendApproved.ToString()) ? "NULL" : app.MonthendApproved.ToString();
            notes += "\r\nMonth End Approved has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.SicCode != clone.SicCode)
        {
            value1 = string.IsNullOrEmpty(clone.SicCode.ToString()) ? "NULL" : clone.SicCode.ToString();
            value2 = string.IsNullOrEmpty(app.SicCode.ToString()) ? "NULL" : app.SicCode.ToString();
            notes += "\r\nMCC has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.TinfoAverageVMCTicket != clone.TinfoAverageVMCTicket)
        {
            value1 = string.IsNullOrEmpty(clone.TinfoAverageVMCTicket.ToString("0.00"))/*"c"))*/ ? "NULL" : clone.TinfoAverageVMCTicket.ToString("0.00");//"c");
            value2 = string.IsNullOrEmpty(app.TinfoAverageVMCTicket.ToString("0.00"))/*"c"))*/ ? "NULL" : app.TinfoAverageVMCTicket.ToString("0.00");//"c");
            notes += "\r\nApproved Average Ticket has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.TinfoHighestTicketAmount != clone.TinfoHighestTicketAmount)
        {
            value1 = string.IsNullOrEmpty(clone.TinfoHighestTicketAmount.ToString("0.00"))/*"c"))*/ ? "NULL" : clone.TinfoHighestTicketAmount.ToString("0.00");//"c");
            value2 = string.IsNullOrEmpty(app.TinfoHighestTicketAmount.ToString("0.00"))/*"c"))*/ ? "NULL" : app.TinfoHighestTicketAmount.ToString("0.00");//"c");
            notes += "\r\nApproved High Ticket has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.PCILevel != clone.PCILevel)
        {
            string pci = FormHandler.getPciLevel(clone.PCILevel);
            value1 = string.IsNullOrEmpty(pci) ? "NULL" : pci;

            pci = FormHandler.getPciLevel(app.PCILevel);
            value2 = string.IsNullOrEmpty(pci) ? "NULL" : pci;

            notes += "\r\nPCI Level has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.ReservePercent != clone.ReservePercent)
        {
            value1 = string.IsNullOrEmpty(clone.ReservePercent.ToString("0.00")) ? "NULL" : clone.ReservePercent.ToString("0.00") + "%";
            value2 = string.IsNullOrEmpty(app.ReservePercent.ToString("0.00")) ? "NULL" : app.ReservePercent.ToString("0.00") + "%";
            notes += "\r\nApproved Reserve has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.DelaysApproved != clone.DelaysApproved)
        {
            value1 = string.IsNullOrEmpty(clone.DelaysApproved) ? "NULL" : clone.DelaysApproved;
            value2 = string.IsNullOrEmpty(app.DelaysApproved) ? "NULL" : app.DelaysApproved;
            notes += "\r\nDays Hold has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.MerchantTag != clone.MerchantTag)
        {
            value1 = string.IsNullOrEmpty(clone.MerchantTag) ? "NULL" : clone.MerchantTag;
            value2 = string.IsNullOrEmpty(app.MerchantTag) ? "NULL" : app.MerchantTag;

            notes += "\r\nMerchant Tag has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.MerchantSells != clone.MerchantSells)
        {
            value1 = string.IsNullOrEmpty(clone.MerchantSells) ? "NULL" : clone.MerchantSells;
            value2 = string.IsNullOrEmpty(app.MerchantSells) ? "NULL" : app.MerchantSells;

            notes += "\r\nMerchant Sells has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.UpfrontReserve != clone.UpfrontReserve)
        {
            value1 = string.IsNullOrEmpty(clone.UpfrontReserve.ToString("0.00"))/*"c"))*/ ? "NULL" : clone.UpfrontReserve.ToString("0.00");//"c");
            value2 = string.IsNullOrEmpty(app.UpfrontReserve.ToString("0.00"))/*"c"))*/ ? "NULL" : app.UpfrontReserve.ToString("0.00");//"c");

            notes += "\r\nUpfront Reserve has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.ReleaseMethodUID != clone.ReleaseMethodUID)
        {
            string val = FormHandler.getReleaseType(clone.ReleaseMethodUID);
            value1 = string.IsNullOrEmpty(val) ? "NULL" : val;

            val = FormHandler.getReleaseType(app.ReleaseMethodUID);
            value2 = string.IsNullOrEmpty(val) ? "NULL" : val;

            if (value1 != value2)
                notes += "\r\nRelease Method has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.ConditionalApproval != clone.ConditionalApproval)
        {
            value1 = clone.ConditionalApproval == true ? "Yes" : "No";
            value2 = app.ConditionalApproval == true ? "Yes" : "No";

            notes += "\r\nConditional Approval has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.ConditionalDueDate != clone.ConditionalDueDate)
        {
            value1 = string.IsNullOrEmpty(clone.ConditionalDueDate.ToShortDateString()) || clone.ConditionalDueDate.ToShortDateString() == "1/1/0001" ? "NULL" : clone.ConditionalDueDate.ToShortDateString();
            value2 = string.IsNullOrEmpty(app.ConditionalDueDate.ToShortDateString()) || app.ConditionalDueDate.ToShortDateString() == "1/1/0001" ? "NULL" : app.ConditionalDueDate.ToShortDateString();

            notes += "\r\nConditional Due Date has changed from " + value1 + " to " + value2 + ".";
        }

        if (app.HighRisk != app.MerchantAppClone.HighRisk)
        {
            string HighRisk = app.MerchantAppClone.HighRisk == true ? "Yes" : "No";
            value1 = string.IsNullOrEmpty(HighRisk) ? "NULL" : HighRisk;
            value2 = app.HighRisk == true ? "Yes" : "No";
            notes += "\r\nHigh Risk has changed from " + value1 + " to " + value2 + ".";
        }

        //Underwriting Fields that are changed

        if (objuw.UnderwritingClone != null)
        {
            //code added by Chandra for PXP-9349
            if (objuw.HighRiskRegistered != objuw.UnderwritingClone.HighRiskRegistered && HighRiskRegistered.Checked)
            {
                notes += "\r\nMerchant has been High Risk Registered with Mastercard.";
            }

            if (objuw.HardCap != objuw.UnderwritingClone.HardCap)
            {
                value1 = string.IsNullOrEmpty(objuw.UnderwritingClone.HardCap.ToString()) ? "NULL" : objuw.UnderwritingClone.HardCap.ToString();
                value2 = string.IsNullOrEmpty(objuw.HardCap.ToString()) ? "NULL" : objuw.HardCap.ToString();
                notes += "\r\nHard Cap has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.HighRiskDescriptor != objuw.UnderwritingClone.HighRiskDescriptor)
            {
                value1 = string.IsNullOrEmpty(objuw.UnderwritingClone.HighRiskDescriptor) ? "NULL" : objuw.UnderwritingClone.HighRiskDescriptor;
                value2 = string.IsNullOrEmpty(objuw.HighRiskDescriptor) ? "NULL" : objuw.HighRiskDescriptor;
                notes += "\r\nHigh Risk Descriptor has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.AgentLevel != objuw.UnderwritingClone.AgentLevel)
            {
                string agentlevel = FormHandler.getAgentLevel(objuw.UnderwritingClone.AgentLevel.Trim());
                value1 = string.IsNullOrEmpty(agentlevel) ? "NULL" : agentlevel;

                agentlevel = FormHandler.getAgentLevel(objuw.AgentLevel.Trim());
                value2 = string.IsNullOrEmpty(agentlevel) ? "NULL" : agentlevel;

                if (value1 != value2)
                    notes += "\r\nPartner Level has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.DivertUponBoarding != objuw.UnderwritingClone.DivertUponBoarding)
            {
                value1 = objuw.UnderwritingClone.DivertUponBoarding.ToUpper() == "TRUE" ? "Yes" : "No";
                value2 = objuw.DivertUponBoarding.ToUpper() == "TRUE" ? "Yes" : "No";
                notes += "\r\nDivert Upon Boarding has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.BuyPassIndicator != objuw.UnderwritingClone.BuyPassIndicator)
            {
                value1 = objuw.UnderwritingClone.BuyPassIndicator.ToUpper() == "TRUE" ? "Yes" : "No";
                value2 = objuw.BuyPassIndicator.ToUpper() == "TRUE" ? "Yes" : "No";
                notes += "\r\nByPass Indicator has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.FlagsOffUponBoarding != objuw.UnderwritingClone.FlagsOffUponBoarding)
            {
                value1 = objuw.UnderwritingClone.FlagsOffUponBoarding.ToUpper() == "TRUE" ? "Yes" : "No";
                value2 = objuw.FlagsOffUponBoarding.ToUpper() == "TRUE" ? "Yes" : "No";
                notes += "\r\nFlags Off Upon Boarding has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.ProductTag != objuw.UnderwritingClone.ProductTag)
            {
                value1 = string.IsNullOrEmpty(objuw.UnderwritingClone.ProductTag) ? "NULL" : objuw.UnderwritingClone.ProductTag;
                value2 = string.IsNullOrEmpty(objuw.ProductTag) ? "NULL" : objuw.ProductTag;
                notes += "\r\nProduct Tag has changed from " + value1 + " to " + value2 + ".";
            }

            if (objuw.NotifyRiskDept != objuw.UnderwritingClone.NotifyRiskDept)
            {
                string Notify = objuw.UnderwritingClone.NotifyRiskDept == true ? "Yes" : "No";
                value1 = string.IsNullOrEmpty(Notify) ? "NULL" : Notify;
                value2 = objuw.NotifyRiskDept == true ? "Yes" : "No";
                notes += "\r\nNotify Risk Dept has changed from " + value1 + " to " + value2 + ".";
            }
        }

        if (!string.IsNullOrWhiteSpace(notes))
        {
            try
            {
                MerchantNotes ObjMerchantNotes = new MerchantNotes();
                ObjMerchantNotes.MerchantAppUID = app.MerchantAppUID;
                ObjMerchantNotes.Subject = "Underwriting - Profile Change";
                ObjMerchantNotes.Notes = notes;
                ObjMerchantNotes.View_Agent = false;
                ObjMerchantNotes.View_Bank = false;
                ObjMerchantNotes.View_MPSAll = true;
                ObjMerchantNotes.Email_Agent = false;
                ObjMerchantNotes.UserCreated = user.UserName;

                data.InsertMerchantNotes(ObjMerchantNotes);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        if (objuw.VIRPHighRiskRegistered != objuw.UnderwritingClone.VIRPHighRiskRegistered && VIRPHighRiskRegistered.Checked)
        {
            try
            { 
                MerchantNotes ObjMerchantNotes = new MerchantNotes();
                ObjMerchantNotes.MerchantAppUID = app.MerchantAppUID;
                ObjMerchantNotes.Subject = ConstantFacade.VIRP.VIRP_SUBJECT_NOTE;
                ObjMerchantNotes.Notes = ConstantFacade.VIRP.VIRP_BODY_NOTE;
                ObjMerchantNotes.View_Agent = false;
                ObjMerchantNotes.View_Bank = false;
                ObjMerchantNotes.View_MPSAll = true;
                ObjMerchantNotes.Email_Agent = false;
                ObjMerchantNotes.UserCreated = user.UserName;

                data.InsertMerchantNotes(ObjMerchantNotes);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ValidDescriptors.LoadValidators();
    }

    protected void btnRefreshList_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        MerchantApp agreement = UserSessions.CurrentMerchantApp;

        prms.Add("@UID", agreement.MerchantAppUID);
        prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
        prms.Add("@RefreshLinkLog", 1);

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        bool isMultiLink = data.CheckMultiLinkInfoV2(prms);

        if (isMultiLink)
            BindData();

        GetMultiLinkRefreshLog(int.Parse(UserSessions.CurrentMerchantApp.ID));
    }

    protected void btnGet1_Click(object sender, EventArgs e)
    {
        try
        {
            Logging.ExperianLog.InfoFormat("Experian Get report started.");
            NetConnectRequest objNCR = new NetConnectRequest();
            DataNetConnect objNC = new DataNetConnect();

            objNCR.EAI = "FL1QEPJZ";

            if (ConfigurationManager.AppSettings["ExperianUserID"].ToString().ToUpper() == "MERITUS_DEMO")
                objNCR.DBHost = NetConnectRequestDBHost.BISTESTP;
            else
                objNCR.DBHost = NetConnectRequestDBHost.BISPROD;

            objNCR.ReferenceId = "user1abc001";

            objNCR.Request = new RequestType();

            objNCR.Request.Products = new RequestTypeProducts();

            PremierProfileType bpt = new PremierProfileType();

            bpt = FillPremierProfileRequest();

            objNCR.Request.Products.Item = bpt;

            Logging.ExperianLog.InfoFormat("Before calling certificate");
            string errCertificate = objNC.GetExperianCertificationTest();
            bool IsPassed = (string.IsNullOrWhiteSpace(errCertificate));

            if (IsPassed)
            {
                Logging.ExperianLog.InfoFormat("Experian cert checks passed");
                // convert object to XML
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(NetConnectRequest));
                StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, objNCR);
                char[] charsToTrim = { ',', '.', ' ' };
                string request_body_xml = textWriter.ToString().TrimStart('{').TrimEnd('}');
                string postData = string.Empty;


                postData = "NETCONNECT_TRANSACTION=" + System.Web.HttpUtility.UrlEncode(request_body_xml);
                Logging.ExperianLog.InfoFormat("PostData to Experian, Request XML {0}", postData);
                Crypto crypto = new Crypto();

                //Enforce the call to use TLS 1.2,
                //By Default Framework 4.5 will resolve to SSL3 and TLS 1.0 is they are present on the server.
                string TLSVersion = ConfigurationManager.AppSettings["TLSVersion"].ToString();

                // Chandra: PXP-2982 TLS1.2: Zeus
                //System.Net.ServicePointManager.SecurityProtocol = TLSVersion.Equals("TLS12") ? System.Net.SecurityProtocolType.Tls12 : System.Net.SecurityProtocolType.Tls11;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                Logging.ExperianLog.InfoFormat("Before calling GetExperianServerName()");
                HttpWebRequest experianRequest = (HttpWebRequest)WebRequest.Create(objNC.GetExperianServerName());
                Logging.ExperianLog.InfoFormat("After calling GetExperianServerName(): {0}", objNC.GetExperianServerName());
                experianRequest.Method = "POST";
                experianRequest.ContentType = "application/x-www-form-urlencoded";
                string UserIDFormated = ConfigurationManager.AppSettings["ExperianUserID"].ToString() + ":" + crypto.Decrypt(ConfigurationManager.AppSettings["ExperianPassword"].ToString());
                experianRequest.Headers.Add("Authorization", "BASIC " + objNC.ConvertToBase64String(UserIDFormated));
                experianRequest.Timeout = 100000;
                experianRequest.KeepAlive = false;
                experianRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
                System.Text.ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byteData;
                byteData = encoding.GetBytes(postData);

                experianRequest.AllowAutoRedirect = true;
                experianRequest.ContentLength = byteData.Length;
                Stream newStream = experianRequest.GetRequestStream();
                newStream.Write(byteData, 0, byteData.Length);
                newStream.Close();

                GetReponse(experianRequest);

                this.ToggleButtons();
            }
            else
            {
                WucMessage1.AddMessageError(errCertificate);
            }

            WebDialogWindow2.WindowState = DialogWindowState.Hidden;
        }

        catch (Exception ex)
        {
            Logging.ExperianLog.ErrorFormat("Error in Get Experian report, Message: {0}", ex.Message);
            Logging.ExperianLog.ErrorFormat("Error in Get Experian report, StackTrace:{0}", ex.StackTrace);

        }
        finally
        {
            WebDialogWindow2.WindowState = DialogWindowState.Hidden;
        }

    }

    private void GetReponse(HttpWebRequest experianRequest)
    {
        try
        {

            NetConnectResponse objAFR = new NetConnectResponse();
            XmlSerializer xmlserial = new XmlSerializer(typeof(NetConnectResponse));
            Logging.ExperianLog.InfoFormat("Attempting to get reponse from Experian, connection is alive here.");
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpWebResponse experianResponse = (HttpWebResponse)experianRequest.GetResponse();

            ////Base 64 convert routine
            if (experianResponse != null && experianResponse.StatusCode.ToString() == "OK")
            {
                using (var reader = new StreamReader(experianResponse.GetResponseStream()))
                {
                    //var doc = new XmlDocument();
                    //doc.LoadXml(reader.ReadToEnd());

                    string xml = reader.ReadToEnd();
                    Logging.ExperianLog.InfoFormat("Experian Response:{0}", xml);
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);


                    //you may want to compare case in-sensitive
                    MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(doc.InnerXml));
                    NetConnectResponse resultingMessage = (NetConnectResponse)xmlserial.Deserialize(memStream);

                    if (resultingMessage.CompletionCode == "4000")
                    {
                        WucMessage1.AddMessageError(resultingMessage.ErrorMessage);
                        return;
                    }
                    else if (resultingMessage.CompletionCode == "2000")
                    {
                        WucMessage1.AddMessageError("Authorization failed. Please update the password.");
                        return;
                    }
                    else if (resultingMessage.CompletionCode != "0000")
                    {
                        WucMessage1.AddMessageError("System error. Call Experian Technical Support at 1 800 854 7201.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(resultingMessage.ErrorMessage))
                    {
                        ProductsType objPP = (ProductsType)resultingMessage.Item;

                        # region fill the UI fields
                        for (int k = 0; k < objPP.Items.Length; k++)
                        {
                            if (objPP.Items[k].GetType() == typeof(PremierProfile))
                            {
                                PremierProfile objP = (PremierProfile)objPP.Items[k];

                                for (int i = 0; i < objP.Items.Length; i++)
                                {
                                    if (objP.Items[i].GetType() == typeof(BIS_BusinessNameAndAddressType))
                                    {
                                        if (((BIS_BusinessNameAndAddressType)objP.Items[i]).Items.Length > 0)
                                        {
                                            BIS_BusinessNameAndAddressType objBNA = (BIS_BusinessNameAndAddressType)objP.Items[i];
                                            if (objBNA.Items[0].GetType() == typeof(RequiredCodeType) && ((RequiredCodeType)objBNA.Items[0]).code.Trim() == "NO RECORD")
                                            {
                                                WucMessage1.AddMessageError(((RequiredCodeType)objBNA.Items[0]).code.Trim());
                                                break;
                                            }
                                        }
                                    }
                                    else if (objP.Items[i].GetType() == typeof(BIS_CommercialFraudShieldSummaryType))
                                    {
                                        BIS_CommercialFraudShieldSummaryType objPTT = (BIS_CommercialFraudShieldSummaryType)objP.Items[i];
                                        ResOFACMatch.Checked = (!string.IsNullOrWhiteSpace(objPTT.OFACMatchCode.code) && objPTT.OFACMatchCode.code.Trim() != "01");
                                        ResOFACDescription.Text = objPTT.OFACMatchCode.Value;
                                    }
                                    else if (objP.Items[i].GetType() == typeof(BIS_BusinessFactsType))
                                    {
                                        BIS_BusinessFactsType objPTT = (BIS_BusinessFactsType)objP.Items[i];

                                        if (objPTT.FileEstablishedDate != null)
                                        {
                                            int yr = CommonUtility.Util.if_i(objPTT.FileEstablishedDate.Substring(0, 4), 0);
                                            int mn = CommonUtility.Util.if_i(objPTT.FileEstablishedDate.Substring(4, 2), 0);

                                            DateTime a = new DateTime(yr, mn, 1);

                                            int years = DateTime.Today.Year - a.Year;
                                            ResYearsOnFile.Text = years.ToString();
                                        }
                                        else
                                            ResYearsOnFile.Text = "";

                                        if (objPTT.DateOfIncorporation != null)
                                        {
                                            int yr = CommonUtility.Util.if_i(objPTT.DateOfIncorporation.Substring(0, 4), 0);
                                            int mn = CommonUtility.Util.if_i(objPTT.DateOfIncorporation.Substring(4, 2), 0);
                                            int dt = CommonUtility.Util.if_i(objPTT.DateOfIncorporation.Substring(6, 2), 0);

                                            DateTime a = new DateTime(yr, mn, dt);
                                            ResIncorporationDate.Value = a.ToShortDateString();
                                        }
                                        else
                                            ResIncorporationDate.Value = null;

                                        ResIncorporationState.Text = objPTT.StateOfIncorporation;

                                    }
                                    else if (objP.Items[i].GetType() == typeof(BIS_ExecutiveSummaryType))
                                    {
                                        BIS_ExecutiveSummaryType objPTT = (BIS_ExecutiveSummaryType)objP.Items[i];
                                        ResDBT.Text = objPTT.BusinessDBT.code;
                                    }
                                    else if (objP.Items[i].GetType() == typeof(BIS_IntelliscoreScoreInformation))
                                    {
                                        BIS_IntelliscoreScoreInformation objPTT = (BIS_IntelliscoreScoreInformation)objP.Items[i];
                                        if (objPTT.ModelInformation.ModelTitle.ToUpper().Contains("FINANCIAL STABILITY RISK"))
                                        {
                                            string score = CommonUtility.Util.if_s(CommonUtility.Util.if_i(objPTT.ScoreInfo.Score, 0) / 100);
                                            ResFinancialStabilityScore.Text = score;
                                            ResFinancialStabilityRiskLevel.Text = objPTT.Action.Replace("TEST", "").Trim();
                                        }
                                        else if (objPTT.ModelInformation.ModelTitle.ToUpper().Contains("INTELLISCORE PLUS"))
                                        {
                                            string score = CommonUtility.Util.if_s(CommonUtility.Util.if_i(objPTT.ScoreInfo.Score, 0) / 100);
                                            ResIntelliScore.Text = score;
                                            ResIntelliRiskLevel.Text = objPTT.Action.Replace("TEST", "").Trim();
                                        }
                                    }
                                    else if (objP.Items[i].GetType() == typeof(BIS_CorporateRegistrationType))
                                    {
                                        BIS_CorporateRegistrationType objPTT = (BIS_CorporateRegistrationType)objP.Items[i];
                                        if (objPTT.StatusFlag != null)
                                            ResCurrentStatus.Text = objPTT.StatusFlag.Value;
                                        else
                                            ResCurrentStatus.Text = "";
                                    }
                                }
                            }
                        }
                        #endregion

                        //OFAC.Visible = ResOFACMatch.Checked;
                        OFACLabel.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";
                        ResOFACDescription.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";

                        #region Values to database

                        if (WucMessage1.ErrorCount() == 0 && string.IsNullOrWhiteSpace(ResFinancialStabilityScore.Text) && string.IsNullOrWhiteSpace(ResIntelliScore.Text))
                            WucMessage1.AddMessageError("No Record Found.Contact Experian.");

                        if (WucMessage1.ErrorCount() == 0)
                        {
                            ZeusPremierProfile objZPP = new ZeusPremierProfile();

                            objZPP.DateCreated = DateTime.Now;
                            objZPP.UserCreated = UserSessions.CurrentUser.UserName;
                            objZPP.ResponseXML = doc.InnerXml;
                            objZPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                            objZPP.ReqOpInitials = "GG";
                            objZPP.ReqSubCode = ConfigurationManager.AppSettings["SubCode"].ToString();//"688500";
                            objZPP.ReqBusinessName = UserSessions.CurrentMerchantApp.BusinessDBAName;
                            objZPP.ReqCurrentAddress = UserSessions.CurrentMerchantApp.BusinessAddress;
                            objZPP.ReqCurrentAddressCity = UserSessions.CurrentMerchantApp.BusinessCity;
                            objZPP.ReqCurrentAddressState = UserSessions.CurrentMerchantApp.BusinessState;
                            objZPP.ReqCurrentAddressZip = UserSessions.CurrentMerchantApp.BusinessZip;
                            objZPP.ReqVendorNumber = "BIS045";
                            objZPP.ReqCustomerName = "TESTING";
                            objZPP.ReqBusinessTaxID = UserSessions.CurrentMerchantApp.BusinessTaxID;
                            objZPP.ResCreditReportDate = DateTime.Now;
                            objZPP.ResIntelliScore = CommonUtility.Util.if_i(ResIntelliScore.Text, 0);
                            objZPP.ResIntelliRiskLevel = ResIntelliRiskLevel.Text;
                            objZPP.ResFinancialStabilityScore = CommonUtility.Util.if_i(ResFinancialStabilityScore.Text, 0);
                            objZPP.ResFinancialStabilityRiskLevel = ResFinancialStabilityRiskLevel.Text;
                            objZPP.ResDBT = CommonUtility.Util.if_i(ResDBT.Text, 0);
                            objZPP.ResYearsOnFile = CommonUtility.Util.if_i(ResYearsOnFile.Text, 0);

                            if (ResIncorporationDate.Value != null && !string.IsNullOrWhiteSpace(ResIncorporationDate.Text))
                                objZPP.ResIncorporationDate = CommonUtility.Util.if_date(ResIncorporationDate.Text, DateTime.MinValue);

                            objZPP.ResCurrentStatus = ResCurrentStatus.Text;
                            objZPP.ResOFACMatch = ResOFACMatch.Checked;
                            objZPP.ResOFACDescription = ResOFACDescription.Text;
                            objZPP.ResIncorporationState = ResIncorporationState.Text;
                            objZPP.IsCurrent = false;

                            DataNetConnect objNetCon = new DataNetConnect();
                            PremierProfileID.Value = objNetCon.InsertPremierProfile(objZPP);

                            btnView.Visible = true;
                            btnClear.Visible = true;
                            btnView.Enabled = true;

                            if (CommonUtility.Util.if_i(PremierProfileID.Value, 0) > 0)
                                SetEditmode(false);

                            WucMessage1.AddMessageSuccess("Business Credit report is generated successfully.");

                        }

                        #endregion

                    }
                    else
                    {
                        WucMessage1.AddMessageError(resultingMessage.ErrorMessage);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            WucMessage1.AddMessageError("System Error. No Record Found. Contact Experian.");
        }

    }

    private Winnovative.PdfConverter GetPdfConverter()
    {
        Winnovative.PdfConverter pdfConverter = new Winnovative.PdfConverter();
        pdfConverter.LicenseKey = "2FZFV0ZXQUZDQ1dGQllHV0RGWUZFWU5OTk5XRw==";// "/NfN3M7P3MTF3MrSzNzPzdLNztLFxcXF";

        //pdfConverter.LicenseKey = "put your license key here";

        // set the HTML page width in pixels
        // the default value is 1024 pixels
        pdfConverter.HtmlViewerWidth = 1024; // autodetect the HTML page width

        //set the PDF page size 
        pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
        // set the PDF compression level
        pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
        // set the PDF page orientation (portrait or landscape)
        pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
        //set the PDF standard used to generate the PDF document
        pdfConverter.PdfDocumentOptions.PdfStandardSubset = PdfStandardSubset.Full;
        // show or hide header and footer
        pdfConverter.PdfDocumentOptions.ShowHeader = false;
        pdfConverter.PdfDocumentOptions.ShowFooter = false;
        //set the PDF document margins
        pdfConverter.PdfDocumentOptions.LeftMargin = 0;
        pdfConverter.PdfDocumentOptions.RightMargin = 0;
        pdfConverter.PdfDocumentOptions.TopMargin = 0;
        pdfConverter.PdfDocumentOptions.BottomMargin = 0;
        // set if the HTTP links are enabled in the generated PDF
        pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
        // set if the HTML content is resized if necessary to fit the PDF page width - default is true
        pdfConverter.PdfDocumentOptions.FitWidth = true;
        // set if the PDF page should be automatically resized to the size of the HTML content when FitWidth is false
        pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
        // embed the true type fonts in the generated PDF document
        pdfConverter.PdfDocumentOptions.EmbedFonts = false;
        // compress the images in PDF with JPEG to reduce the PDF document size - default is true
        pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
        // set if the JavaScript is enabled during conversion 
        pdfConverter.JavaScriptEnabled = false;

        // set if the converter should try to avoid breaking the images between PDF pages
        pdfConverter.PdfDocumentOptions.AvoidImageBreak = false;


        pdfConverter.PdfBookmarkOptions.HtmlElementSelectors = false ? new string[] { "h1", "h2" } : null;

        return pdfConverter;
    }

    public string getcssImages(string input)
    {
        if (input == null)
            return string.Empty;
        string tempInput = input;
        string pattern = @"../images";
        string src = string.Empty;
        HttpContext context = HttpContext.Current;
        string ImagePath = WebConfigurationManager.AppSettings["Experian.ZeusURL"];
        string IsNewDataCenter = WebConfigurationManager.AppSettings["IsNewDataCenter"];
        //Change the relative URL's to absolute URL's for an image, if any in the HTML code.
        foreach (Match m in Regex.Matches(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.RightToLeft))
        {
            if (m.Success)
            {
                string tempM = m.Value;
                src = m.Value.Replace("\"", "");

                if (src.ToLower().Contains("http://") == false || src.ToLower().Contains("https://") == false)
                {
                    //Insert new URL in img tag
                    string path;

                    if (IsNewDataCenter.Equals("TRUE"))
                    {
                        path = new Uri(ImagePath).GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath;
                    }

                    //Works in Irvine this way
                    else
                    {
                        path = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath;
                    }

                    src = path + src.Replace("..", "");
                    src = src.Replace("//images", "/images");
                    try
                    {
                        //insert new url img tag in whole html code
                        tempInput = tempInput.Remove(m.Index, m.Length);
                        tempInput = tempInput.Insert(m.Index, src);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        return tempInput;
    }

    private PremierProfileType FillPremierProfileRequest()
    {
        PremierProfileType bpt = new PremierProfileType();

        bpt.Subscriber = new BIS_SubscriberType();
        bpt.Subscriber.OpInitials = "GG";
        bpt.Subscriber.SubCode = ConfigurationManager.AppSettings["SubCode"].ToString();

        bpt.BusinessApplicant = new BusinessApplicantType();
        bpt.BusinessApplicant.BusinessName = UserSessions.CurrentMerchantApp.BusinessDBAName;
        bpt.BusinessApplicant.CurrentAddress = new BusinessApplicant_AddressType();
        bpt.BusinessApplicant.CurrentAddress.Street = UserSessions.CurrentMerchantApp.BusinessAddress;
        bpt.BusinessApplicant.CurrentAddress.City = UserSessions.CurrentMerchantApp.BusinessCity;
        bpt.BusinessApplicant.CurrentAddress.State = UserSessions.CurrentMerchantApp.BusinessState;
        bpt.BusinessApplicant.CurrentAddress.Zip = UserSessions.CurrentMerchantApp.BusinessZip;

        bpt.OutputType = new BusinessProfile_OutputType();
        bpt.OutputType.ItemElementName = new ItemChoiceType3();
        bpt.OutputType.ItemElementName = ItemChoiceType3.XML;

        BIS_XMLType xmlt = new BIS_XMLType();
        xmlt.Verbose = new ChoiceType();
        xmlt.Verbose = ChoiceType.Y;

        bpt.OutputType.Item = xmlt;

        bpt.Vendor = new Business_VendorType();
        bpt.Vendor.VendorNumber = "BIS045";

        bpt.Options = new BISOptions3_OptionsType();
        bpt.Options.CustomerName = "TESTING";

        return bpt;
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        DataNetConnect objNC = new DataNetConnect();

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
        prms.Add("@PremierProfileID", PremierProfileID.Value);

        string xmlResponse = objNC.GetPremierProfileXmlResponse(prms);

        if (!string.IsNullOrWhiteSpace(xmlResponse))
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(HttpContext.Current.Server.MapPath("~\\Styles\\premierprofile\\PPR.xslt"));

            XPathDocument mydata = new XPathDocument(new XmlTextReader(xmlResponse, XmlNodeType.Document, null));

            StringWriter sw = new StringWriter();

            xslt.Transform(mydata, new XsltArgumentList(), sw);

            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

            //Try adding source strings for each image in content
            string tempPostContent = getcssImages(sw.ToString());
            ZeusWeb.Logging.ExperianLog.InfoFormat("CreditReport Content : {0}", tempPostContent);

            TextReader reader1 = new StringReader(tempPostContent);

            //Winnovative.HtmlToPdfConverter pdfConverter = GetPdfConverter();
            //MemoryStream memstr = new MemoryStream();

            //pdfConverter.SavePdfFromHtmlStringToStream(tempPostContent, memstr);

            //New code by Chandra
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = "2FZFV0ZXQUZDQ1dGQllHV0RGWUZFWU5OTk5XRw==";

            // Set PDF page size which can be a predefined size like A4 or a custom size in points 
            // Leave it not set to have a default A4 PDF page
            htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = Winnovative.PdfPageSize.A4;

            // Set PDF page orientation to Portrait or Landscape
            // Leave it not set to have a default Portrait orientation for PDF page
            htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

            // Set the maximum time in seconds to wait for HTML page to be loaded 
            // Leave it not set for a default 60 seconds maximum wait time
            htmlToPdfConverter.NavigationTimeout = 120;

            // The buffer to receive the generated PDF document
            byte[] byteArr = null;

            byteArr = htmlToPdfConverter.ConvertHtml(tempPostContent, "");


            //byte[] byteArr = memstr.ToArray();

            string filename = "Business_Premier_Profile_" + UserSessions.CurrentMerchantApp.ID + ".pdf";

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader(
                                    "Content-Disposition",
            "attachment; filename=" + filename + "; size="
                            + byteArr.Length.ToString());

            response.Flush();
            response.BinaryWrite(byteArr);
            response.End();

        }

    }


    protected void AddPDF()
    {
        string tempPostContent = string.Empty;

        try
        {

            DataNetConnect objNC = new DataNetConnect();
            ZeusPremierProfile objPP = new ZeusPremierProfile();
            Hashtable prms = new Hashtable();

            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            prms.Add("@PremierProfileID", CommonUtility.Util.if_i(PremierProfileID.Value, 0));

            objPP = objNC.GetPremierProfile(prms);

            if (objPP != null)
            {
                string xmlResponse = objPP.ResponseXML;

                if (!string.IsNullOrWhiteSpace(xmlResponse) && objPP.DocID <= 0)
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(HttpContext.Current.Server.MapPath("~\\Styles\\premierprofile\\PPR.xslt"));

                    XPathDocument mydata = new XPathDocument(new XmlTextReader(xmlResponse, XmlNodeType.Document, null));

                    StringWriter sw = new StringWriter();

                    xslt.Transform(mydata, new XsltArgumentList(), sw);

                    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

                    //Try adding source strings for each image in content
                    tempPostContent = getcssImages(sw.ToString());

                    TextReader reader1 = new StringReader(tempPostContent);

                    //Winnovative.WnvHtmlConvert.PdfConverter pdfConverter = GetPdfConverter();

                    //MemoryStream memstr = new MemoryStream();

                    //pdfConverter.SavePdfFromHtmlStringToStream(tempPostContent, memstr);

                    //byte[] byteArr = memstr.ToArray();

                    //New code by Chandra
                    // Create a HTML to PDF converter object with default settings
                    HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

                    // Set license key received after purchase to use the converter in licensed mode
                    // Leave it not set to use the converter in demo mode
                    htmlToPdfConverter.LicenseKey = "2FZFV0ZXQUZDQ1dGQllHV0RGWUZFWU5OTk5XRw==";

                    // Set PDF page size which can be a predefined size like A4 or a custom size in points 
                    // Leave it not set to have a default A4 PDF page
                    htmlToPdfConverter.PdfDocumentOptions.PdfPageSize = Winnovative.PdfPageSize.A4;

                    // Set PDF page orientation to Portrait or Landscape
                    // Leave it not set to have a default Portrait orientation for PDF page
                    htmlToPdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

                    // Set the maximum time in seconds to wait for HTML page to be loaded 
                    // Leave it not set for a default 60 seconds maximum wait time
                    htmlToPdfConverter.NavigationTimeout = 120;

                    // The buffer to receive the generated PDF document
                    byte[] byteArr = null;

                    byteArr = htmlToPdfConverter.ConvertHtml(tempPostContent, "");


                    ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
                    objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

                    ZeusWeb.MDocWS.UploadResponse objR = objFU.UploadFileWithSourceAndUser(
                            byteArr
                            , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                            , UserSessions.CurrentMerchantApp.MerchantAppUID
                            , 0
                            , ""
                            , (int)MDoc.eMDocType.CreditReport
                            , "Business Premier Profile_" + UserSessions.CurrentMerchantApp.ID + ".pdf"
                            , "Zeus"
                            , 0
                            , ""
                            , ""
                            , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                            , (int)MDoc.eMDocSourceID.Merchant
                            , UserSessions.CurrentUser.UserName
                        );

                    if (objR.DocID > 0)
                    {
                        objPP = objNC.GetPremierProfile(prms);

                        objPP.DocID = objR.DocID;
                        objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                        objPP.PremierProfileID = CommonUtility.Util.if_i(PremierProfileID.Value, 0);

                        objNC.UpdatePremierProfile(objPP);

                        WucMessage1.AddMessageSuccess("Credit Report PDF Generated.");
                    }
                    else
                    {
                        WucMessage1.AddMessageError(objR.StatusMessage);
                    }

                }
            }
        }
        catch (Exception ex)
        {
            //Display parser errors in PDF. 
            WucMessage1.AddMessageError("Error:" + ex.Message);
            WucMessage1.AddMessageError("PDF Error.");
        }

        this.ToggleButtons();

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.ToggleButtons();
        FormHandler.ClearAllControls(ExperianData1);
        FormHandler.ClearAllControls(ExperianData2);
        PremierProfileID.Value = "";
        btnView.Visible = false;
        SetEditmode(true);
    }

    protected void btnGet_Click(object sender, EventArgs e)
    {

        btnGetOld.Visible = false;

        if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessLegalName))
        {
            FormHandler.DisplayMessage(Page.ClientScript, "Legal name is missing."); return;
        }
        else
        {

            if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessDBAName))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "DBA name is missing."); return;
            }
            else if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessAddress))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Business address is missing."); return;
            }
            else if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessCity))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Business city is missing."); return;
            }
            else if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessState.Replace("-1", "")))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Business state is missing."); return;
            }
            else if (string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessZip))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Business zip is missing."); return;
            }
            else
            {
                btnGetOld.Visible = true;

                Hashtable prms = new Hashtable();
                prms.Add("@BusinessName", UserSessions.CurrentMerchantApp.BusinessDBAName);

                if (!string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessTaxID))
                    prms.Add("@BusinessTaxID", UserSessions.CurrentMerchantApp.BusinessTaxID);

                DataNetConnect objNC = new DataNetConnect();
                ZeusPremierProfile objPP = new ZeusPremierProfile();
                objPP = objNC.GetPremierProfile(prms);

                if (objPP != null)
                {
                    lbl.Text = "You are about to pull credit for " + UserSessions.CurrentMerchantApp.BusinessLegalName + ". The last credit report was pulled on " + objPP.LastCreditDate.ToShortDateString() + " for ZID: " + objPP.MerchantID + ". Press Ok to proceed.";
                    btnGetOld.Visible = true;
                }
                else
                {
                    lbl.Text = "You are about to pull credit for " + UserSessions.CurrentMerchantApp.BusinessLegalName + ". Press Ok to proceed.";
                    btnGetOld.Visible = false;
                }
            }
        }

        WebDialogWindow2.WindowState = DialogWindowState.Normal;

    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        this.ToggleButtons();
        WebDialogWindow2.WindowState = DialogWindowState.Hidden;
    }

    protected void btnGetOld_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@BusinessName", UserSessions.CurrentMerchantApp.BusinessDBAName);

        if (!string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.BusinessTaxID))
            prms.Add("@BusinessTaxID", UserSessions.CurrentMerchantApp.BusinessTaxID);

        DataNetConnect objNC = new DataNetConnect();
        ZeusPremierProfile objPP = new ZeusPremierProfile();
        objPP = objNC.GetPremierProfile(prms);

        if (objPP != null)
        {

            ResCurrentStatus.Text = objPP.ResCurrentStatus;
            ResIntelliScore.Text = CommonUtility.Util.if_s(objPP.ResIntelliScore, "");
            ResIntelliRiskLevel.Text = objPP.ResIntelliRiskLevel;
            ResFinancialStabilityScore.Text = CommonUtility.Util.if_s(objPP.ResFinancialStabilityScore, "");
            ResFinancialStabilityRiskLevel.Text = objPP.ResFinancialStabilityRiskLevel;
            ResDBT.Text = objPP.ResDBT.ToString();
            ResYearsOnFile.Text = objPP.ResYearsOnFile.ToString();
            if (objPP.ResIncorporationDate != null)
                ResIncorporationDate.Value = objPP.ResIncorporationDate.Value;
            ResIncorporationState.Text = objPP.ResIncorporationState;
            ResOFACDescription.Text = objPP.ResOFACDescription;
            ResOFACMatch.Checked = objPP.ResOFACMatch;
            OFACLabel.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";
            ResOFACDescription.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";

            btnView.Visible = true;
            btnView.Enabled = true;

            objPP.UserCreated = UserSessions.CurrentUser.UserName;
            objPP.UserModified = string.Empty;
            objPP.DateModified = null;
            objPP.DateCreated = DateTime.Now;
            objPP.IsCurrent = false;

            if (objPP.MerchantID.ToString() != UserSessions.CurrentMerchantApp.ID)
            {
                objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);

                if (objPP.MerchantID > 0)
                    PremierProfileID.Value = objNC.InsertPremierProfile(objPP);
            }
            else
            {
                prms = new Hashtable();
                prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
                prms.Add("@IsCurrent", true);

                ZeusPremierProfile objPP1 = new ZeusPremierProfile();
                objPP1 = objNC.GetPremierProfile(prms);

                if (objPP1 == null)
                {
                    objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);

                    if (objPP.MerchantID > 0)
                        PremierProfileID.Value = objNC.InsertPremierProfile(objPP);
                }
            }

            if (CommonUtility.Util.if_i(PremierProfileID.Value, 0) > 0)
                SetEditmode(false);

        }
        else
        {
            ResCurrentStatus.Text = string.Empty;
            ResIntelliScore.Text = string.Empty;
            ResIntelliRiskLevel.Text = string.Empty;
            ResFinancialStabilityScore.Text = string.Empty;
            ResFinancialStabilityRiskLevel.Text = string.Empty;
            ResDBT.Text = string.Empty;
            ResYearsOnFile.Text = string.Empty;
            ResIncorporationDate.Value = null;
            ResIncorporationState.Text = string.Empty;
            ResOFACDescription.Text = string.Empty;
            ResOFACMatch.Checked = false;
            OFACLabel.Style["display"] = "none";
            ResOFACDescription.Style["display"] = "none";
            btnView.Visible = false;
            PremierProfileID.Value = "";

        }

        WebDialogWindow2.WindowState = DialogWindowState.Hidden;
    }

    private void FillPremierProfileReponse()
    {
        DataNetConnect objNC = new DataNetConnect();

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
        prms.Add("@IsCurrent", true);

        ZeusPremierProfile objPP = new ZeusPremierProfile();
        objPP = objNC.GetPremierProfile(prms);

        if (objPP != null)
        {
            btnView.Visible = true;
            PremierProfileID.Value = objPP.PremierProfileID.ToString();
            btnView.Enabled = true;
            SetEditmode(false);
        }
        else
        {
            btnView.Visible = false;
            PremierProfileID.Value = "";
            CreditReportDate.Value = "";

            if (this.EditMode)
                SetEditmode(true);

        }

        MerchantPremierProfile objMPP = new MerchantPremierProfile();
        objMPP = DataAccess.DataUnderwritingDao.LoadMerchantPremierProfile(UserSessions.CurrentMerchantApp.ID);

        if (objMPP != null)
        {
            ResCurrentStatus.Text = objMPP.ResCurrentStatus;

            if (objMPP.ResIntelliScore > 0)
                ResIntelliScore.Text = CommonUtility.Util.if_s(objMPP.ResIntelliScore, "");
            else
                ResIntelliScore.Text = "";

            ResIntelliRiskLevel.Text = objMPP.ResIntelliRiskLevel;

            if (objMPP.ResFinancialStabilityScore > 0)
                ResFinancialStabilityScore.Text = CommonUtility.Util.if_s(objMPP.ResFinancialStabilityScore, "");
            else
                ResFinancialStabilityScore.Text = "";

            ResFinancialStabilityRiskLevel.Text = objMPP.ResFinancialStabilityRiskLevel;

            if (objMPP.ResDBT > 0)
                ResDBT.Text = CommonUtility.Util.if_s(objMPP.ResDBT, "");
            else
                ResDBT.Text = "";

            if (objMPP.ResYearsOnFile > 0)
                ResYearsOnFile.Text = CommonUtility.Util.if_s(objMPP.ResYearsOnFile, "");
            else
                ResYearsOnFile.Text = "";

            ResIncorporationDate.Value = objMPP.ResIncorporationDate;
            ResIncorporationState.Text = objMPP.ResIncorporationState;
            ResOFACDescription.Text = objMPP.ResOFACDescription;
            ResOFACMatch.Checked = DataLayer.Field2Bool(objMPP.ResOFACMatch);
            if (objMPP.ResCreditReportDate != null)
                CreditReportDate.Value = objMPP.ResCreditReportDate.ToShortDateString();
            else
                CreditReportDate.Value = "";
            OFACLabel.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";
            ResOFACDescription.Style["display"] = (ResOFACMatch.Checked) ? "inline" : "none";
        }

    }

    private void SetEditmode(bool istrue)
    {
        SetReadOnly(ExperianData1, istrue);
        SetReadOnly(ExperianData2, istrue);
    }

    protected string GetSubmitPostBack()
    {
        return Page.ClientScript.GetPostBackEventReference(Button1, string.Empty);
    }

    public void SetReadOnly(Control container, bool EditMode)
    {
        foreach (Control control in container.Controls)
        {
            if (control != null)
            {
                if (control is ListControl)
                {
                    ListControl listControl = (ListControl)control;
                    listControl.Enabled = EditMode;
                    if (!EditMode)
                        listControl.BackColor = System.Drawing.Color.LightGray;
                    else
                        listControl.BackColor = System.Drawing.Color.White;
                }
                else if (control is TextBox)
                {
                    TextBox txt = (TextBox)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is CheckBox)
                {
                    CheckBox chk = (CheckBox)control;
                    chk.Enabled = EditMode;
                }
                else if (control is Button)
                {
                    Button btn = (Button)control;
                    btn.Enabled = EditMode;
                    if (!EditMode)
                        btn.BackColor = System.Drawing.Color.LightGray;
                    else
                        btn.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebTextEditor)
                {
                    WebTextEditor txt = (WebTextEditor)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebMaskEditor)
                {
                    WebMaskEditor txt = (WebMaskEditor)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebDateTimeEditor)
                {
                    WebDateTimeEditor txt = (WebDateTimeEditor)control;
                    txt.ReadOnly = !EditMode;
                }
                else if (control is WebDateChooser)
                {
                    WebDateChooser txt = (WebDateChooser)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebNumericEditor)
                {
                    WebNumericEditor txt = (WebNumericEditor)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebCurrencyEditor)
                {
                    WebCurrencyEditor txt = (WebCurrencyEditor)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebPercentEditor)
                {
                    WebPercentEditor txt = (WebPercentEditor)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is WebHtmlEditor)
                {
                    WebHtmlEditor txt = (WebHtmlEditor)control;
                    txt.ReadOnly = !EditMode;
                    if (!EditMode)
                        txt.BackColor = System.Drawing.Color.LightGray;
                    else
                        txt.BackColor = System.Drawing.Color.White;
                }
                else if (control is FileUpload)
                {
                    FileUpload txt = (FileUpload)control;
                    txt.Enabled = EditMode;
                }
                else if (control is GridView)
                {
                    GridView grd = (GridView)control;
                    grd.Enabled = EditMode;
                    if (!EditMode)
                        grd.BackColor = System.Drawing.Color.LightGray;
                    else
                        grd.BackColor = System.Drawing.Color.White;
                }
                if (control.HasControls())
                {
                    SetReadOnly(control, EditMode);
                }
            }
        }
    }

    protected void StatusUID_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList ddl = (DropDownList)sender;

        if (ddl.SelectedValue.ToLower() == Constants.QUEUESTATUS_CU_DECLINED.ToLower())
        {
            lblddlDeclineReason.Text = "Decline Reason: ";
            LookupTableHandler.FillDeclineReason(ddlddlDeclineReason, new Guid(Constants.QUEUESTATUS_CU_DECLINED));
            ddlddlDeclineReason.Visible = true;
            lbltbPrimaryReason.Visible = true;
            tbPrimaryReason.Visible = true;
            lbltbPrimaryReason.Text = "Primary Reason for Decline: ";
        }
        else if (ddl.SelectedValue.ToLower() == Constants.QUEUESTATUS_CU_WITHDRAWN.ToLower())
        {
            lblddlDeclineReason.Text = "Withdrawn Reason: ";
            LookupTableHandler.FillDeclineReason(ddlddlDeclineReason, new Guid(Constants.QUEUESTATUS_CU_WITHDRAWN));
            ddlddlDeclineReason.Visible = true;
            lbltbPrimaryReason.Visible = true;
            tbPrimaryReason.Visible = true;
            lbltbPrimaryReason.Text = "Primary Reason for Withdrawal: ";
        }
        else
        {
            lblddlDeclineReason.Text = string.Empty;
            ddlddlDeclineReason.Items.Clear();
            ddlddlDeclineReason.Visible = false;
            lbltbPrimaryReason.Visible = false;
            tbPrimaryReason.Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        TotalPeriodVolume.ReadOnly = true;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        WebDialogWindow1.WindowState = DialogWindowState.Normal;
        FinancialScoreCardGrid._ScoreCardID = 0;
        FinancialScoreCardGrid.MerchantID = int.Parse(UserSessions.CurrentMerchantApp.ID);
        FinancialScoreCardGrid.FormNew();

    }

    protected void MerchantScoreCards_GridViewCommand(object sender, int ScoreCardID, string ScoreCardName, string TimePeriod)
    {
        WebDialogWindow1.WindowState = DialogWindowState.Normal;
        FinancialScoreCardGrid.FormClear();
        FinancialScoreCardGrid._ScoreCardID = ScoreCardID;
        FinancialScoreCardGrid._ScoreCardName = ScoreCardName;
        FinancialScoreCardGrid._TimePeriod = TimePeriod;
        FinancialScoreCardGrid.MerchantID = int.Parse(UserSessions.CurrentMerchantApp.ID);
        FinancialScoreCardGrid.FormCancel();
        FinancialScoreCardGrid.FormShow(ScoreCardID.ToString());

    }

    protected void FinancialScoreCardGrid_ButtonClick(object sender, string args)
    {
        WebDialogWindow1.WindowState = DialogWindowState.Hidden;

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        MerchantScoreCards.SetDataSource(prms);

    }

    protected void btnRefresh_Click1(object sender, EventArgs e)
    {
        WebDialogWindow1.WindowState = DialogWindowState.Hidden;

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        MerchantScoreCards.SetDataSource(prms);

    }

    protected void Request3DE_Click(object sender, EventArgs e)
    {
        string vendorList = string.Empty;
        bool isVendorSelected = false;

        foreach (GridViewRow row in grdChecklist.Rows)
        {
            if (((CheckBox)row.FindControl("chkSelect")).Checked)
            {
                string vendor = row.Cells[7].Text;

                if (!string.IsNullOrWhiteSpace(vendor) && !vendor.Equals("9"))
                {
                    vendorList += row.Cells[7].Text + ",";
                }

                isVendorSelected = true;
            }
        }

        if (!isVendorSelected)
        {
            FormHandler.DisplayMessage(Page.ClientScript, "Please select at least one vendor");
            return;
        }
        else
        {
            MerchantApp app = UserSessions.CurrentMerchantApp;
            IList<string> message = new List<string>();

            if (app.Office == CommonUtility.Util.Offices.Irvine)
            {
                Validate3DEVendorsData validate3DEVendorsData = new Validate3DEVendorsData();
                message = validate3DEVendorsData.ValidateZeus3DEData(app);
            }
            if (message.Count > 0)
            {
                foreach (string mess in message)
                    this.Master.AddMessageError(mess);
            }
            else
            {

                FormHandler.Call3DEVendors(UserSessions.CurrentMerchantApp, UserSessions.CurrentUser.UserName, false, vendorList);
                GetRecent3DEStatus(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
            }
        }
        //To get out of Edit mode
        this.EditMode = false;
        this.FormShow(this.UID);
        this.ToggleButtons();
    }

    private void GetRecent3DEStatus(int zid)
    {
        Zeus3DEStatus recentstatus = new Zeus3DEStatus();

        recentstatus = MerchantFacade.GetRecent3DEStatus(zid);
        if (!String.IsNullOrEmpty(recentstatus.RequestMessage))
        {
            this.lbl3DEStatus.Text = "3DEStatus: " + recentstatus.RequestMessage + " Last Requested Date: " + recentstatus.DateCreated.ToString();
        }

    }

    private void GetMultiLinkRefreshLog(int zid)
    {
        DataSet ds = MerchantFacade.GetZeusMultiLinkRefreshLog(zid);

        lblRefresh.Text = "Click Refresh List button to see latest changes on the Multilink accounts.";

        /*modified by chandra for PXP-2716- to show aggregated cc/ach monthly volume and YTD volume for the Multilink accounts*/
        if (ds != null && ds.Tables.Count > 0)
        {
            StringBuilder str = new StringBuilder();
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                str.Append("(Last Updated: <b>" + dt.Rows[0]["DateRefreshed"].ToString() + "</b> by <b>" + dt.Rows[0]["UserRefreshed"].ToString() + "</b>)");
                lblRefresh.Text += str.ToString();
            }

            if (ds.Tables.Count > 1)
            {
                dt = ds.Tables[1];

                if (dt.Rows.Count > 0)
                {
                    lblCCVolume.Text = "<b>Total Appr monthly CC Vol:</b> $" + dt.Rows[0]["AggregatedCCVolume"].ToString() + "&nbsp;";
                    lblACHVolume.Text = "<b>Total Appr monthly ACH Vol:</b> $" + dt.Rows[0]["AggregatedACHVolume"].ToString() + "&nbsp;";
                    lblYTDVolume.Text = "<b>Total YTD Vol:</b> $" + dt.Rows[0]["AggregatedYTDVolume"].ToString() + "&nbsp;";
                }
                else
                {
                    lblCCVolume.Text = "";
                    lblACHVolume.Text = "";
                    lblYTDVolume.Text = "";
                }
            }
        }
        /******** end of PXP-2716 **************/

    }

    //code added by Abarua PXP-7469
    //protected void OnBillingTypes_Changed(object sender, EventArgs e)
    //{
    //    if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
    //    {
    //        string uWissues = UWIssues.Text;
    //        StringBuilder sbcc = new StringBuilder();

    //        bool checkBilltype = true;
    //        for (int i = 0; i < BillingTypes.Items.Count; i++)
    //        {
    //            switch (BillingTypes.Items[i].Text)
    //            {
    //                case "FT":
    //                    if (BillingTypes.Items[i].Selected) {

    //                        if (!uWissues.Contains("Free Trial"))
    //                        {  checkBilltype = false;
    //                            sbcc.Append(" Free Trial");
    //                        }
    //                    }
    //                    else if (uWissues.Contains("Free Trial")) 
    //                    {
    //                        uWissues = uWissues.Replace("Free Trial,", string.Empty);
    //                        uWissues = uWissues.Replace("Free Trial", string.Empty);
    //                    }
    //                    break;
    //                case "Continuity":
    //                    if (BillingTypes.Items[i].Selected)
    //                    {

    //                        if (!uWissues.Contains("Continuity"))
    //                        {checkBilltype = false;
    //                            sbcc.Append(" Continuity");
    //                        }
    //                    }
    //                    else if (uWissues.Contains("Continuity"))
    //                    {
    //                        uWissues = uWissues.Replace("Continuity,", string.Empty);
    //                        uWissues = uWissues.Replace("Continuity", string.Empty);
    //                    }
    //                    break;
    //                case "Install":
    //                    if (BillingTypes.Items[i].Selected)
    //                    {

    //                        if (!uWissues.Contains("Install"))
    //                        {checkBilltype = false;
    //                            sbcc.Append(" Install");
    //                        }
    //                    }
    //                    else if (uWissues.Contains("Install"))
    //                    {
    //                        uWissues = uWissues.Replace("Install,", string.Empty);
    //                     uWissues=uWissues.Replace("Install", string.Empty);
    //                    }
    //                    break;
    //                case "OTS":
    //                    if (BillingTypes.Items[i].Selected)
    //                    {

    //                        if (!uWissues.Contains("One Time Sale"))
    //                        {checkBilltype = false;
    //                            sbcc.Append(" One Time Sale");
    //                        }
    //                    }
    //                    else if (uWissues.Contains("One Time Sale"))
    //                    {
    //                        uWissues = uWissues.Replace("One Time Sale,", string.Empty);
    //                        uWissues = uWissues.Replace("One Time Sale", string.Empty);
    //                    }
    //                    break;
    //                default: break;
    //            }

    //        }
    //        if (!checkBilltype)
    //        {
    //            if (uWissues.Contains("Billing type:"))
    //            {
    //                uWissues = uWissues.Replace("Billing type:", "Billing type:" + sbcc.ToString() + ",");
    //            }
    //            else
    //            {
    //                if (uWissues.Contains("General Set up Details: HR MONITORING FEE"))
    //                {
    //                    uWissues = uWissues.Replace("General Set up Details: HR MONITORING FEE", "General Set up Details: HR MONITORING FEE" +"\r\n"+ "Billing type:" + sbcc.ToString());
    //                }
    //                else if (uWissues.Contains("General Set up Details:"))
    //                {
    //                    uWissues = uWissues.Replace("General Set up Details:", "General Set up Details:" + "\r\n" + "Billing type:" + sbcc.ToString());
    //                }

    //            }

    //        }
    //        UWIssues.Text = uWissues;
    //    }
    //}




    //code modified by koshlendra for PXP-4963[Add Monitoring Fee instructions automatically on Ops Form] start

    //protected void HRMonitoringFee_CheckedChanged(object sender, EventArgs e)
    //{
    //    string uWissues = UWIssues.Text;
    //    if (HRMonitoringFee.Checked)
    //    {
    //        if (uWissues.Contains("General Set up Details:"))
    //        {
    //            uWissues = uWissues.Replace("General Set up Details:", "General Set up Details: HR MONITORING FEE");

    //        }
    //        else
    //        {
    //            uWissues = "General Set up Details: HR MONITORING FEE" + "\r\n" + uWissues;
    //        }

    //    }
    //    else
    //    {
    //        if (uWissues.Contains("General Set up Details: HR MONITORING FEE"))
    //        {
    //            uWissues = uWissues.Replace("General Set up Details: HR MONITORING FEE", "General Set up Details:");

    //        }  
    //    }
    //    UWIssues.Text = uWissues;

    //}

    //code modified by koshlendra for PXP-4963[Add Monitoring Fee instructions automatically on Ops Form] end

    protected void Response3DE_Click(object sender, EventArgs e)
    {
        try
        {
            string EndPointURL = ConfigurationManager.AppSettings["Zeus3deGetResponse"];
            string jsonresponse = JSONClient.PostRestService(UserSessions.CurrentMerchantApp.ID, EndPointURL);

            GetRecent3DEStatus(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
        }
        catch (Exception ex)
        {
            //Log the Exception
        }
        //To get out of Edit mode
        this.EditMode = false;
        this.FormShow(this.UID);
        this.ToggleButtons();
    }

    protected void MasterMRP_CheckedChanged(object sender, EventArgs e)
    {
        if (UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {
            //this.rowRegisteredURLs.Visible = this.MasterMRP.Checked;
            this.tdlabelRegisteredURLs.Visible = this.MasterMRP.Checked;
            this.tdtextbRegisteredURLs.Visible = this.MasterMRP.Checked;
            this.VIRPRegisteredURLsTR.Visible = this.MasterVIRP.Checked;
            //PXP-12066: Start by Rohit Thakur
            this.CharactersCount.Visible = this.MasterMRP.Checked;
            this.VIRPCharactersCount.Visible = this.MasterVIRP.Checked;
            //PXP-12066: End by Rohit Thakur
            if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.RegisteredURLs) && this.MasterMRP.Checked && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.BusinessWebsite))
            {
                this.RegisteredURLs.Text = UserSessions.CurrentMerchantApp.BusinessWebsite;
                //PXP-12066: Start by Rohit Thakur
                this.charLength.InnerText = this.RegisteredURLs.Text.Length.ToString();
                //PXP-12066: End by Rohit Thakur
            }
            if (!UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_CREDIT_UNDERWRITING))
                RegisteredURLs.ReadOnly = true;
            if (string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.RegisteredURLs) && this.MasterVIRP.Checked && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.BusinessWebsite))
            {
                this.VIRPRegisteredURLs.Text = UserSessions.CurrentMerchantApp.BusinessWebsite;
                this.VIRPcharLength.InnerText = this.RegisteredURLs.Text.Length.ToString();
            }
            if (!UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_CREDIT_UNDERWRITING))
                VIRPRegisteredURLs.ReadOnly = true;
        }
    }
    //PXP-12066: Start by Rohit Thakur
    protected void btnText_Changed(object sender, EventArgs e)
    {
        this.charLength.InnerText = this.RegisteredURLs.Text.Length.ToString();
    }
    //PXP-12066: End by Rohit Thakur
}

