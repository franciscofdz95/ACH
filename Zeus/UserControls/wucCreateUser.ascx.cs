using System;
using System.Configuration;
using System.Collections;
using System.Web;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects.Notify;
using CommonUtility;
using Newtonsoft.Json;
using System.Linq;
using Okta.Sdk.Model;
using System.Collections.Generic;

public partial class wucCreateUser : System.Web.UI.UserControl
{
    public string LoginType
    {
        get { return ViewState["LoginType"].ToString(); }
        set { ViewState["LoginType"] = value; }
    }

    public string UserName
    {
        get
        {
            if (ViewState["UserName"] == null)
                return string.Empty;
            else
                return ViewState["UserName"].ToString();
        }
        set { ViewState["UserName"] = value; }
    }

    public string DBA
    {
        get
        {
            if (ViewState["DBA"] == null)
                return string.Empty;
            else
                return ViewState["DBA"].ToString();
        }
        set { ViewState["DBA"] = value; }
    }

    public string HookTableKeyUID
    {
        get
        {
            if (ViewState["HookTableKeyUID"] == null)
                return string.Empty;
            else
                return ViewState["HookTableKeyUID"].ToString();
        }

        set { ViewState["HookTableKeyUID"] = value; }
    }

    public string EmailTo
    {
        get
        {
            if (ViewState["EmailTo"] == null)
                return string.Empty;
            else
                return ViewState["EmailTo"].ToString();
        }


        set { ViewState["EmailTo"] = value; }
    }

    public string UserUID
    {
        get
        {
            if (ViewState["UserUID"] == null)
                return string.Empty;
            else
                return ViewState["UserUID"].ToString();
        }
        set { ViewState["UserUID"] = value; }
    }

    public string PrivateLabelUID
    {
        get
        {
            if (ViewState["PrivateLabelUID"] == null)
                return string.Empty;
            else
                return ViewState["PrivateLabelUID"].ToString();
        }
        set { ViewState["PrivateLabelUID"] = value; }
    }

    public PrivateLabel objPrivateLabel
    {
        get
        {
            if (ViewState["objPrivateLabel"] == null)
                return null;
            else
                return (PrivateLabel)ViewState["objPrivateLabel"];
        }
        set { ViewState["objPrivateLabel"] = value; }
    }

    public string Key
    {
        get
        {
            if (ViewState["MerchantKey"] == null)
                return string.Empty;
            else
                return ViewState["MerchantKey"].ToString();
        }
        set { ViewState["MerchantKey"] = value; }
    }

    //PXP-11488 by Anuj Kumar
    public string LockdownKey
    {
        get
        {
            if (ViewState["LockdownKey"] == null)
                return string.Empty;
            else
                return ViewState["LockdownKey"].ToString();
        }
        set { ViewState["LockdownKey"] = value; }
    }


    public string HostedPaymentKey
    {
        get
        {
            if (ViewState["HostedPaymentKey"] == null)
                return string.Empty;
            else
                return ViewState["HostedPaymentKey"].ToString();
        }
        set { ViewState["HostedPaymentKey"] = value; }
    }

    public string MerchantPin
    {
        get
        {
            if (ViewState["MerchantPin"] == null)
                return string.Empty;
            else
                return ViewState["MerchantPin"].ToString();
        }
        set { ViewState["MerchantPin"] = value; }
    }

    public bool HasMerchantPin
    {
        get
        {
            if (ViewState["HasMerchantPin"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["HasMerchantPin"]);
        }
        set { ViewState["HasMerchantPin"] = value; }
    }

    public int OfficeID
    {
        get
        {
            if (ViewState["OfficeID"] == null)
                return 1;
            else
                return (Int32)(ViewState["OfficeID"]);
        }
        set { ViewState["OfficeID"] = value; }
    }
    public string Status
    {
        get
        {
            if (ViewState["Status"] == null)
                return string.Empty;
            else
                return ViewState["Status"].ToString();
        }
        set { ViewState["Status"] = value; }
    }
    private string[] Statuses
    {
        get
        {
            return new string[] { Constants.QUEUESTATUS_MS_ACTIVE, Constants.QUEUESTATUS_MS_CONDUCTED_WELCOME_CALL, Constants.QUEUESTATUS_MS_SCHEDULED_WELCOME_CALL };
        }
    }
    public void SetParams(object o)
    {
        HttpRequest req = (HttpRequest)o;

        this.LoginType = req["LoginType"].ToString();
        this.UserName = req["UserName"].ToString();
        this.HookTableKeyUID = req["HookTableKeyUID"].ToString();
        this.EmailTo = req["EmailTo"].ToString();
        this.DBA = req["DBA"].ToString();

    }
    public void ForcePreRender()
    {
        this.Page_Load(null, null);


    }

    public void Formshow()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
        prms.Add("@UserName", this.UserName);
        UserFacade facade3 = new UserFacade();
        PaymentXP.BusinessObjects.User user = facade3.GetUser(prms);

        btnCreateAccount.Text = $"Create {this.LoginType} Login";

        if (user != null)
        {
            chkActive.Checked = user.HasDBAccess;
            this.UserUID = user.UID;
            //code changes for PXP-12673 by koshlendra start
            chkAlternativeGatewayKey.Checked = user.EnableAltGatewayKey;
            this.AlternativeGatewayKey.Text = user.AltGatewayKey;
            if (chkAlternativeGatewayKey.Checked)
            {
                lblAltKey.Visible = true;
                AlternativeGatewayKey.Visible = true;
            }
            //code changes for PXP-12673 by koshlendra end
            // DM-4897 alamadrid
            chkIsMFAEnabled.Checked = user.IsMFAEnabled;
        }
        else
        {
            chkPaymentXP.Checked = false;
            chkAgentWebsite.Checked = false;
            chkMerchantWebsite.Checked = false;
            chkActive.Checked = false;
            chkHostedPaymentPage.Checked = false;
            //code changes for PXP-12673 by koshlendra start
            chkAlternativeGatewayKey.Checked = false;
            AlternativeGatewayKey.Text = string.Empty;
            lblAltKey.Visible = false;
            AlternativeGatewayKey.Visible = false;
            //code changes for PXP-12673 by koshlendra end
            // DM-4897 alamadrid
            chkIsMFAEnabled.Checked = false;
        }

        lblDBA.Text = this.DBA;
        lblUserName.Text = this.UserName;
        lblEmail.Text = this.EmailTo;
        GatewayKey.Text = this.Key.ToUpper();

        //PXP-11488 by Anuj Kumar
        //PXP-11971 Fix By Asheesh
        // Ani - DM-5972
        
            string lockdownKey = this.LockdownKey;
            if (!string.IsNullOrEmpty(lockdownKey) && lockdownKey.Length > 0)
            {
                trLockdownKey.Visible = true;
                switch (UserSessions.CurrentUser.DefaultRoleUID.ToUpper())
                {
                    case Constants.ROLE_QUALITY:
                    case Constants.ROLE_APPLICATION_BOARDING:
                    case Constants.ROLE_DEPLOYMENT:
                    case Constants.ROLE_CLIENT_SERVICES:
                        NMILockdownAPIKey.Text = lockdownKey;
                        break;
                    default:
                        string firstChars = lockdownKey.Substring(0, 4);
                        string lastChars = lockdownKey.Substring(lockdownKey.Length - 4, 4);
                        string requiredMask = new String('X', lockdownKey.Length - firstChars.Length - lastChars.Length);
                        string maskedString = string.Concat(firstChars, requiredMask, lastChars);

                        NMILockdownAPIKey.Text = maskedString;
                        break;
                }
            }

        tbMerchantPin.Text = this.MerchantPin.ToUpper();

        btnCreateAccount.Enabled = user == null;
        btnResetPassword.Enabled = user != null;

        chkMerchantWebsite.Enabled = user != null;
        chkAgentWebsite.Enabled = user != null;
        chkPaymentXP.Enabled = user != null;
        chkHostedPaymentPage.Enabled = user != null;
        chkActive.Enabled = user != null;
        //code changes for PXP-12673 by koshlendra start
        chkAlternativeGatewayKey.Enabled = user != null;
        //code changes for PXP-12673 by koshlendra end

        if (user != null)
        {
            btnWelcomeEmailAgentWebsite.Enabled = false;
            btnWelcomeEmailMerchantWebsite.Enabled = false;
            btnWelcomeEmailPaymentXP.Enabled = false;
            btnWelcomeEmailHostedPaymentPage.Enabled = false;

            foreach (UserPortal item in user.UserPortals)
            {
                if (item.PortalID.ToUpper() == "4A77C310-4264-45C6-96C1-F7EFE61C7D2E") //PaymentXP
                {
                    chkPaymentXP.Checked = item.Enabled;
                    btnWelcomeEmailPaymentXP.Enabled = item.Enabled;
                }

                if (item.PortalID.ToUpper() == "8D37E9F5-4094-4D92-987F-C3E642E6B092") //Agent Website
                {
                    chkAgentWebsite.Checked = item.Enabled;
                    btnWelcomeEmailAgentWebsite.Enabled = item.Enabled;
                }

                if (item.PortalID.ToUpper() == "76411203-7F8E-4FC1-9CDC-9CF0C8084611") //Merchant Website
                {
                    chkMerchantWebsite.Checked = item.Enabled;
                    btnWelcomeEmailMerchantWebsite.Enabled = item.Enabled;
                }
            }

            if (user.IsMerchant)
            {
                HostedPaymentToken merchantToken = DataToken.SelectHostedPaymentToken(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), null);

                if (merchantToken != null)
                {
                    this.HostedPaymentKey = merchantToken.Token;
                    btnWelcomeEmailHostedPaymentPage.Enabled = merchantToken.IsActive;
                    lblHostedPaymentKey.Visible = merchantToken.IsActive;
                    HostedPaymentPageKey.Visible = merchantToken.IsActive;
                    HostedPaymentPageKey.Text = this.HostedPaymentKey.ToUpper();
                    chkHostedPaymentPage.Checked = merchantToken.IsActive;
                    chkMobilePaymentPage.Checked = merchantToken.IsMobileActive;
                }
                ValidateMobileCheck();

            }
        }

        chkPaymentXP.Visible = !(this.LoginType == "Agent");
        btnWelcomeEmailPaymentXP.Visible = !(this.LoginType == "Agent");
        chkAgentWebsite.Visible = this.LoginType == "Agent";
        btnWelcomeEmailAgentWebsite.Visible = this.LoginType == "Agent";
        chkHostedPaymentPage.Visible = !(this.LoginType == "Agent");
        btnWelcomeEmailHostedPaymentPage.Visible = !(this.LoginType == "Agent");
        //code changes for PXP-12673 by koshlendra start
        chkAlternativeGatewayKey.Visible = !(this.LoginType == "Agent");
        //code changes for PXP-12673 by koshlendra End

        if (this.LoginType.ToUpper() == "AGENT")
            lblKey.Text = "Agent Key:";
        else
            lblKey.Text = "Gateway Key:";

        // special rule, for agent id 1956 (Dennis Ideue), we send them the Merchant Pin, in addition to the merchant key
        if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.AgentUID.Trim().ToUpper() == "95A056DD-30FF-4B9D-82BA-95D44AA0E8C4")
        {
            trPin.Visible = true;
        }
        else if (HasMerchantPin)
        {
            trPin.Visible = true;
            UserSessions.CurrentMerchantApp.HasMerchantPin = HasMerchantPin;
        }
        else
        {
            trPin.Visible = false;
        }
    }

    public void ValidateMobileCheck()
    {
        if (!this.chkHostedPaymentPage.Checked)
            this.chkMobilePaymentPage.Enabled = false;
        else
            this.chkMobilePaymentPage.Enabled = true;
    }

    public bool FormDataCheck()
    {
        string message = string.Empty;

        if (this.EmailTo.Trim() == string.Empty)
            message += "Email in profile is missing.<br />";


        if (message == string.Empty)
            return true;
        else
        {
            lblMessage.Text = message;
            return false;
        }
    }

    void wucCreateUser_Init(object sender, EventArgs e)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //this.Formshow();
        }
        lblMessage.Text = "";
    }

    protected void btnWelcomeEmailAgentWebsite_Click(object sender, EventArgs e)
    {
        if (!this.FormDataCheck())
            return;

        try
        {
            Hashtable prms = new Hashtable();
            prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
            prms.Add("@UserName", this.UserName);
            UserFacade facade3 = new UserFacade();
            PaymentXP.BusinessObjects.User user = facade3.GetUser(prms);

            if (user != null)
            {
                string subject = "Paysafe Group plc Partner Website Account Information";
                string body = FormHandler.GetWelcomeEmailAgentWebsite(this.DBA, this.UserName, user.Password);
                string AgentID = string.Empty;

                if (UserSessions.CurrentAgent != null)
                    AgentID = UserSessions.CurrentAgent.AgentUID;

                //send login info
                FormHandler.SendEmail(subject, string.Empty, body, UserSessions.CurrentUser.Email, this.EmailTo, null, null, new Hashtable(), null, AgentID);

                //send password
                body = FormHandler.GetWelcomeEmailPassword("Agent Website", user.Password, "");
                FormHandler.SendEmail(subject, string.Empty, body, UserSessions.CurrentUser.Email, this.EmailTo, null, null, new Hashtable(), null, AgentID);

                lblMessage.Text = "Partner Website email sent successfully!";
                AddNotes("Partner Website email sent.", "8d6bca61-d5a3-4a49-b56a-d32115f37cfe");
            }
            else
            {
                lblMessage.Text = "Please create login";
            }
        }
        catch (Exception exc)
        {
            lblMessage.Text = "Partner Website email sent failed. Error - " + exc.Message;
        }
    }

    protected void btnWelcomeEmailMerchantWebsite_Click(object sender, EventArgs e)
    {
        if (!this.FormDataCheck())
            return;

        try
        {
            Hashtable prms = new Hashtable();
            prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
            prms.Add("@UserName", this.UserName);
            UserFacade facade3 = new UserFacade();
            PaymentXP.BusinessObjects.User user = facade3.GetUser(prms);

            if (user != null)
            {
                string company = ResourceService.AppResources["MeritusCompanyName"];
                string subject = string.Empty, AgentID = string.Empty;
                string From = Constants.IRVINE_INFO;
                string MerchantID = string.Empty;

                if (UserSessions.CurrentAgent != null)
                    AgentID = UserSessions.CurrentAgent.AgentUID;

                if (objPrivateLabel != null)
                {
                    company = objPrivateLabel.PLCompanyName;
                    From = objPrivateLabel.PLEmail;
                }

                if (UserSessions.CurrentMerchantApp != null)
                {
                    MerchantID = UserSessions.CurrentMerchantApp.MerchantAppUID;

                    MerchantNotification notification = NotificationService.GetMerchantNotification(MerchantID, PaymentXP.BusinessObjects.Notify.NotificationType.InsightAccount, UserSessions.CurrentMerchantApp.PrivateLabelUID);

                    notification.SetMessageParameter("LoginName", this.UserName);
                    notification.SetMessageParameter("BusinessDBA", this.DBA);

                    notification.FromAddress = From;
                    notification.AddRecipient(this.EmailTo);
                    notification.UserName = this.UserName;

                    NotificationService.SetDefaultMerchantNotification(notification, UserSessions.CurrentMerchantApp);
                    NotificationService.SendNotification(notification);
                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Sending merchant welcome email. Email sent to: {1}", this.DBA, this.EmailTo);

                    //by Chandra, for PXP-2866: Add the merchant notes for Insight login information email sent

                    AddMerchantNotes(MerchantID, notification.GetSubject());

                    //end of code -PXP-2866

                    notification = NotificationService.GetMerchantNotification(MerchantID, PaymentXP.BusinessObjects.Notify.NotificationType.InsightPassword, UserSessions.CurrentMerchantApp.PrivateLabelUID);

                    notification.SetMessageParameter("Password", user.Password);

                    if (this.LoginType.ToUpper().Contains("MERCHANT") && (UserSessions.CurrentMerchantApp != null
                        && (UserSessions.CurrentMerchantApp.AgentUID.Trim().ToUpper() == "95A056DD-30FF-4B9D-82BA-95D44AA0E8C4" || UserSessions.CurrentMerchantApp.HasMerchantPin)))
                    {
                        notification.SetMessageParameter("MobilePassword", string.Format("<b>Mobile Password:</b> {0}<br><br>", UserSessions.CurrentMerchantApp.MerchantPIN));
                    }

                    notification.FromAddress = From;
                    notification.AddRecipient(this.EmailTo);

                    NotificationService.SetDefaultMerchantNotification(notification, UserSessions.CurrentMerchantApp);
                    NotificationService.SendNotification(notification);
                    notification.UserName = this.UserName;

                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Sending merchant login details. Email sent to: {1}", this.DBA, this.EmailTo);
                    //by Chandra, for PXP-2866: Add the merchant notes for Isnight login information email sent

                    AddMerchantNotes(MerchantID, notification.GetSubject());

                    //end of code -PXP-2866


                    lblMessage.Text = "Merchant Reporting Website email sent successfully!";

                    AddNotes("Merchant Reporting Website email sent.", "8d6bca61-d5a3-4a49-b56a-d32115f37cfe");
                }
                else
                {

                    //get the template for LoginID email and send
                    subject = company + " Insight Account Login ID";
                    string body = FormHandler.GetWelcomeEmailMerchant(this.DBA, this.UserName, objPrivateLabel);
                    FormHandler.SendEmail(subject, string.Empty, body, From, this.EmailTo, null, null, new Hashtable(), MerchantID, AgentID);

                    //get the template for password eamil and send
                    subject = company + " Insight Account Password";
                    body = FormHandler.GetMerchantWebsitePassword(user.Password, objPrivateLabel, this.LoginType);

                    FormHandler.SendEmail(subject, string.Empty, body, From, this.EmailTo, null, null, new Hashtable(), MerchantID, AgentID);
                    lblMessage.Text = "Merchant Reporting Website email sent successfully!";

                    AddNotes("Merchant Reporting Website email sent.", "8d6bca61-d5a3-4a49-b56a-d32115f37cfe");

                }
            }
            else
            {
                lblMessage.Text = "Please create login";
            }
        }
        catch (Exception exc)
        {
            lblMessage.Text = "Merchant Reporting Website email sent failed. Error - " + exc.Message;
        }
    }

    protected void btnWelcomeEmailPaymentXP_Click(object sender, EventArgs e)
    {
        if (!this.FormDataCheck())
            return;

        try
        {
            Hashtable prms = new Hashtable();
            prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
            prms.Add("@UserName", this.UserName);
            UserFacade facade3 = new UserFacade();
            PaymentXP.BusinessObjects.User user = facade3.GetUser(prms);

            if (user != null)
            {
                //send login info
                string company = "Payment XP";
                string subject = string.Empty;

                string From = UserSessions.CurrentUser.Email;
                string AgentID = string.Empty;
                string MerchantID = string.Empty;

                if (UserSessions.CurrentAgent != null)
                {
                    AgentID = UserSessions.CurrentAgent.AgentUID;
                }

                if (UserSessions.CurrentMerchantApp != null)
                {
                    MerchantID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                }

                //override with PL values
                if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.PLEnabled)
                {
                    if (this.objPrivateLabel != null)
                    {
                        company = "Virtual Terminal";
                        From = objPrivateLabel.PLEmail;
                    }
                }

                if (UserSessions.CurrentMerchantApp != null)
                {
                    MerchantNotification notification = NotificationService.GetMerchantNotification(MerchantID, PaymentXP.BusinessObjects.Notify.NotificationType.PaymentXPLogin, UserSessions.CurrentMerchantApp.PrivateLabelUID);

                    notification.SetMessageParameter("LoginName", this.UserName);
                    notification.SetMessageParameter("BusinessDBA", this.DBA);

                    notification.FromAddress = From;
                    notification.AddRecipient(this.EmailTo);
                    notification.UserName = this.UserName;

                    NotificationService.SetDefaultMerchantNotification(notification, UserSessions.CurrentMerchantApp);
                    NotificationService.SendNotification(notification);
                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Sending merchant welcome email of Payment XP. Email sent to: {1}", this.DBA, this.EmailTo);

                    //by Chandra, for PXP-2866: Add the merchant notes for PXP login information email sent

                    AddMerchantNotes(MerchantID, notification.GetSubject());

                    //end of code -PXP-2866

                    notification = NotificationService.GetMerchantNotification(MerchantID, PaymentXP.BusinessObjects.Notify.NotificationType.PaymentXPPassword, UserSessions.CurrentMerchantApp.PrivateLabelUID);

                    notification.SetMessageParameter("Password", user.Password);
                    notification.SetMessageParameter("GatewayKey", this.Key);

                    notification.FromAddress = From;
                    notification.AddRecipient(this.EmailTo);
                    notification.UserName = this.UserName;

                    NotificationService.SetDefaultMerchantNotification(notification, UserSessions.CurrentMerchantApp);
                    NotificationService.SendNotification(notification);

                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Sending merchant password and gateway key. Email sent to: {1} GatewayKey: {2}", this.DBA, this.EmailTo, this.Key);

                    //by Chandra, for PXP-2866: Add the merchant notes for PXP login information email sent

                    AddMerchantNotes(MerchantID, notification.GetSubject());

                    //end of code -PXP-2866

                    lblMessage.Text = "Payment XP email sent successfully!";
                }
                else
                {
                    lblMessage.Text = "Failed to send Payment XP email.";
                }
            }
            else
            {
                lblMessage.Text = "Please create login";
            }
        }
        catch (Exception exc)
        {
            lblMessage.Text = "Payment XP email sent failed. Error - " + exc.Message;
        }
    }

    protected void btnCreateAccount_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
        prms.Add("@UserName", this.UserName);
        UserFacade facade = new UserFacade();
        PaymentXP.BusinessObjects.User user = facade.GetUser(prms);

        if (user == null)
        {
            user = new PaymentXP.BusinessObjects.User();
            user.HookTableKeyUID = this.HookTableKeyUID;

            if (this.LoginType == "Agent")
                user.HookTableUID = "4cb95a71-7dd1-43f3-8f97-9bd15bb04834"; //Agent
            else
                user.HookTableUID = "904683f4-094b-4bda-aef2-1bd7931c77d0"; //Merchant

            user.HookTableName = user.HookTableName;
            user.UserName = this.UserName;
            user.Password = RandomWordGenerator.WordSymNumber(2, true, 3);
            user.FirstName = this.DBA;
            user.LastName = string.Empty;
            user.Email = this.EmailTo;
            user.GroupUID = string.Empty;
            user.Disabled = false;
            user.HasDBAccess = true;
            user.AccessLevelUID = "7b824322-b5a6-4abf-8810-a29ff271d8b6";
            user.PasswordQuestion = string.Empty;
            user.PasswordAnswer = string.Empty;
            user.UserCreated = DateTime.Now.ToString();
            user.Office = (CommonUtility.Util.Offices)this.OfficeID;

            if (this.LoginType == "Agent")
                user.DefaultRoleUID = "640be7f9-1df9-4510-ae98-29a9b3814d6a"; //Agent
            else
                user.DefaultRoleUID = "a6a9eb47-6312-4e5a-8342-acaf457d8322"; //Merchant

            user.ParentUID = string.Empty;

            facade.InsertUser(user);

            if (user.UID != "-1")
            {
                if (this.LoginType == "Agent")
                    facade.InsertUserRoles(user.UID, "640be7f9-1df9-4510-ae98-29a9b3814d6a", true); //Agent
                else
                    facade.InsertUserRoles(user.UID, "A6A9EB47-6312-4E5A-8342-ACAF457D8322", true); //Merchant

                if (user.HookTableUID.ToUpper() == "4CB95A71-7DD1-43F3-8F97-9BD15BB04834" || user.HookTableUID.ToUpper() == "904683F4-094B-4BDA-AEF2-1BD7931C77D0")
                {
                    //Ani - DM-5294
                    CreateAndActivateOktaUser(user);
                }
            }
            this.Formshow();
        }
    }

    private void CreateAndActivateOktaUser(PaymentXP.BusinessObjects.User user)
    {
        try
        {
            if ((user.UID != "-1") && (user.UID != null) && (Constants.IS_MFA_ENABLED))
            {
                UserFacade facade = new UserFacade();
                string oktaPwd = facade.GetRandomPassword(8);
                PasswordCredential oktaPassword = new PasswordCredential() { Value = oktaPwd };
                CreateUserRequest createUserRequest = new CreateUserRequest()
                {
                    Profile = new Okta.Sdk.Model.UserProfile(),
                    Credentials = new Okta.Sdk.Model.UserCredentials()
                };
                createUserRequest.Profile.FirstName = user.FirstName;
                createUserRequest.Profile.LastName = user.LastName;
                createUserRequest.Profile.Login = user.UserName;
                createUserRequest.Profile.Email = user.Email;
                createUserRequest.Profile.UserType = user.HookTableUID.ToUpper() == "4CB95A71-7DD1-43F3-8F97-9BD15BB04834" ? Constants.OKTA_USERTYPE_AGENT :
                                           user.HookTableUID.ToUpper() == "904683F4-094B-4BDA-AEF2-1BD7931C77D0" ? Constants.OKTA_USERTYPE_MERCHANT : "OTHER";
                createUserRequest.Credentials.Password = oktaPassword;
                createUserRequest.GroupIds = new List<string>() { Constants.MFA_GROUP };

                var OktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.CreateUser(createUserRequest, portalUID: UserSessions.PortalUID);

                if (OktaUser != null)
                {
                    user.IsMFAEnabled = Constants.IS_MFA_ENABLED;
                    user.OktaUserID = OktaUser.Id;
                    user.OktaPassword = oktaPwd;
                    user.MFAGroupID = Constants.MFA_GROUP;
                    user.IsMFAActive = true;
                    chkIsMFAEnabled.Checked = user.IsMFAEnabled;
                    facade.UpdateUser(user);
                }
                else
                {
                    lblMessage.Text = "User creation is not successful. Please contact Administrator.";
                }
            }
            else
            {
                lblMessage.Text = "User creation is not successful. Please contact Administrator.";
            }
        }
        catch (Exception ex)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Unable to create and activate okta user" + user.UserName + ex.Message);
            throw;
        }
    }

    protected void btnResetPassword_Click(object sender, EventArgs e)
    {
        if (!Statuses.Contains(Status.ToUpper()) && LoginType.Equals("Merchant"))
        {
            lblMessage.Text = "Cannot reset password from here for this user. <br/><b>Reach out to helpdesk.</b>";
            lblMessage.Style.Add("Color", "red");
            return;
        }

        UserFacade facade = new UserFacade();
        string newpassword = RandomWordGenerator.WordSymNumber(2, true, 3);
        bool perform = facade.UpdateUserPassword(this.UserName, newpassword);

        if (perform)
        {
            string body = FormHandler.GetWelcomeEmailPassword("Paysafe portals", newpassword, objPrivateLabel, this.LoginType);
            string AgentID = string.Empty;
            string subject = "Paysafe Group plc Account Password Reset";
            string From = UserSessions.CurrentUser.Email;


            if (UserSessions.CurrentAgent != null)
                AgentID = UserSessions.CurrentAgent.AgentUID;

            if (this.objPrivateLabel != null)
            {
                subject = subject.Replace("Paysafe Group plc", objPrivateLabel.PLCompanyName);
                From = objPrivateLabel.PLEmail;
            }

            FormHandler.SendEmail(subject, string.Empty, body, From, this.EmailTo, null, null, new Hashtable(), null, AgentID);

            lblMessage.Text = "Password Reset successfully!";
            AddNotes("Password reset by " + UserSessions.CurrentUser.UserName, "fcc1f543-ad5d-4d22-b07d-d8a7691de1c7");
        }
    }

    protected void chkAgentWebsite_CheckedChanged(object sender, EventArgs e)
    {
        UserFacade facade = new UserFacade();
        facade.InsertUserPortals(this.UserUID, "8D37E9F5-4094-4D92-987F-C3E642E6B092", chkAgentWebsite.Checked);

        this.Formshow();
    }

    protected void chkMerchantWebsite_CheckedChanged(object sender, EventArgs e)
    {
        UserFacade facade = new UserFacade();
        facade.InsertUserPortals(this.UserUID, "76411203-7F8E-4FC1-9CDC-9CF0C8084611", chkMerchantWebsite.Checked);

        this.Formshow();
    }

    protected void chkPaymentXP_CheckedChanged(object sender, EventArgs e)
    {

        UserFacade facade = new UserFacade();
        facade.InsertUserPortals(this.UserUID, "4A77C310-4264-45C6-96C1-F7EFE61C7D2E", chkPaymentXP.Checked);
        //code added for assign default PXP settings on check box changes for PXP-16593 start
        if (UserSessions.CurrentMerchantApp != null)
        {
            DataAccess.DataMerchantAppDao.InsertPaymentXPDefaultSettings(UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentUser.UserName, chkPaymentXP.Checked);
        }
        //code added for assign default PXP settings on check box changes for PXP-16593 end
        this.Formshow();

        if (chkPaymentXP.Checked && this.LoginType == "Merchant")
        {
            if (UserSessions.CurrentMerchantApp != null)
            {
                String mcc = FormHandler.GetMerchantMCCForInsert();
                //Added by Koshlendra for PXP-3529- Zeus: Set default Risk parameters for Active merchants start         
                DataAccess.DataMerchantAppDao.ActivatePaymentXP(UserSessions.CurrentMerchantApp.MerchantAppUID
                    , UserSessions.CurrentMerchantApp.ID, mcc);
                //Added by Koshlendra for PXP-3529- Zeus: Set default Risk parameters for Active merchants end

                if (UserSessions.CurrentMerchantApp.AuthPlatformUID.ToUpper() == "E10439C3-F025-43F5-A8F4-0D3BD4D5EB2F" //frontend -- compass
                    && UserSessions.CurrentMerchantApp.SettlePlatformUID.ToUpper() == "9505AB8F-1F8C-463C-90C2-1445189AE79A") // backend -- Front
                {
                    DataGatewayPage.InsertLevelDataDefaults(UserSessions.CurrentMerchantApp.ID);
                }
            }

        }
    }

    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        string active = string.Empty;
        UserFacade facade = new UserFacade();
        facade.UpdateUserStatus(this.UserUID, chkActive.Checked);

        this.Formshow();


        try
        {
            Hashtable prms = new Hashtable();
            prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
            prms.Add("@UserName", this.UserName);
            PaymentXP.BusinessObjects.User user = facade.GetUser(prms);

            if (user != null && PaymentXP.BusinessObjects.Constants.IS_MFA_ENABLED &&
                (user.HookTableUID.ToUpper() == "4CB95A71-7DD1-43F3-8F97-9BD15BB04834" || user.HookTableUID.ToUpper() == "904683F4-094B-4BDA-AEF2-1BD7931C77D0"))
            {

                if (!string.IsNullOrWhiteSpace(user.OktaUserID))
                {
                    if (user.IsMFAEnabled)
                    {
                        Okta.Sdk.Model.User oktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.GetUser(user.OktaUserID, UserSessions.PortalUID);

                        if (oktaUser != null)
                        {
                            Okta.Sdk.Model.User oktauser = Paysafe.TwoFactorAuth.Client.User.Instance.SwitchStatusUser(user.OktaUserID, user.HasDBAccess, UserSessions.PortalUID);
                            if (oktauser != null)
                            {
                                if (oktauser.Status.Value.ToUpper().Equals(PaymentXP.BusinessObjects.Constants.OKTA_STATUS_ACTIVE))
                                    user.IsMFAActive = true;
                                else
                                    user.IsMFAActive = false;
                                UpdateOktaUser(user);
                            }
                        }
                    }
                }
                else
                {
                    if (user.HasDBAccess)
                        CreateAndActivateOktaUser(user);
                }
            }
        }
        catch (Exception ex)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Error while trying to access okta" + ex);
        }


        if (chkActive.Checked)
            active = "Active";
        else
            active = "Inactive";

        AddNotes("Agent login is " + active, "6f065641-bc3b-47b2-aa93-c4497b91f954");

    }

    private void UpdateOktaUser(PaymentXP.BusinessObjects.User user)
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

            if (oktaUser != null)
            {
                bool isResponse = Paysafe.TwoFactorAuth.Client.Factor.Instance.EnrollActivateEmailFactor(user.OktaUserID, user.Email, UserSessions.PortalUID);
                if (isResponse)
                {
                    user.OktaUserID = user.OktaUserID;
                }
            }

        }
        catch (Exception exc)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Unable to update okta user" + user.UserName + exc.Message);
        }
    }

    public bool AddNotes(string notes, string notesUID)
    {
        try
        {
            DataAgent data = DataAccess.DataAgentDao;
            PaymentXP.BusinessObjects.User user = UserSessions.CurrentUser;
            if (UserSessions.CurrentAgent != null)
                data.InsertAgentNotes(UserSessions.CurrentAgent.AgentUID, notes, user.UserName, notesUID);

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    protected void chkHostedPaymentPage_CheckedChanged(object sender, EventArgs e)
    {
        ValidateMobileCheck();
        if (UserSessions.CurrentMerchantApp != null)
        {
            string EncryptKey = RandomWordGenerator.GetHostedPaymentKey(8);
            Hashtable prms = new Hashtable();
            prms.Add("@ZID", UserSessions.CurrentMerchantApp.ID);
            prms.Add("@IsActive", this.chkHostedPaymentPage.Checked);
            prms.Add("@IsMobileActive", this.chkMobilePaymentPage.Checked);
            prms.Add("@MerchantKey", UserSessions.CurrentMerchantApp.MerchantKey);
            prms.Add("@EncryptKey", EncryptKey);
            prms.Add("@HostedPaymentToken", CommonUtility.CryptoAES256.Encrypt(UserSessions.CurrentMerchantApp.MerchantKey, EncryptKey));
            prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);

            DataAccess.DataMerchantAppDao.ActivateHostedPaymentPage(prms);
        }

        this.Formshow();
    }

    protected void btnWelcomeEmailHostedPaymentPage_Click(object sender, EventArgs e)
    {
        if (!this.FormDataCheck())
            return;

        try
        {
            Hashtable prms = new Hashtable();
            prms.Add("@HookTableKeyUID", this.HookTableKeyUID);
            prms.Add("@UserName", this.UserName);
            UserFacade facade3 = new UserFacade();
            PaymentXP.BusinessObjects.User user = facade3.GetUser(prms);

            if (user != null)
            {
                //send login info
                string company = "Payment XP";
                string subject = string.Empty;

                string From = UserSessions.CurrentUser.Email;
                string AgentID = string.Empty;
                string MerchantID = string.Empty;

                if (UserSessions.CurrentAgent != null)
                {
                    AgentID = UserSessions.CurrentAgent.AgentUID;
                }

                if (UserSessions.CurrentMerchantApp != null)
                {
                    MerchantID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                }

                //override with PL values
                if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.PLEnabled)
                {
                    if (this.objPrivateLabel != null)
                    {
                        company = "Virtual Terminal";
                        From = objPrivateLabel.PLEmail;
                    }
                }

                if (UserSessions.CurrentMerchantApp != null)
                {
                    MerchantNotification notification = NotificationService.GetMerchantNotification(MerchantID, PaymentXP.BusinessObjects.Notify.NotificationType.PaymentXPLogin, UserSessions.CurrentMerchantApp.PrivateLabelUID);

                    notification.SetMessageParameter("LoginName", this.UserName);
                    notification.SetMessageParameter("BusinessDBA", this.DBA);

                    notification.FromAddress = From;
                    notification.AddRecipient(this.EmailTo);
                    notification.UserName = this.UserName;

                    NotificationService.SetDefaultMerchantNotification(notification, UserSessions.CurrentMerchantApp);
                    NotificationService.SendNotification(notification);

                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Sending merchant email for PXP login. Email sent to: {1}", this.DBA, this.EmailTo);

                    //by Chandra, for PXP-2866: Add the merchant notes for PXP login and hosted key information email sent
                    AddMerchantNotes(MerchantID, notification.GetSubject() + " – Hosted Key");
                    //end of code PXP-2866

                    notification = NotificationService.GetMerchantNotification(MerchantID, PaymentXP.BusinessObjects.Notify.NotificationType.PaymentXPPassword, UserSessions.CurrentMerchantApp.PrivateLabelUID);

                    notification.SetMessageParameter("Password", user.Password);
                    notification.SetMessageParameter("GatewayKey", this.HostedPaymentKey);

                    notification.FromAddress = From;
                    notification.AddRecipient(this.EmailTo);
                    notification.UserName = this.UserName;

                    NotificationService.SetDefaultMerchantNotification(notification, UserSessions.CurrentMerchantApp);
                    NotificationService.SendNotification(notification);

                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} Sending merchant email for PXP login and hosted key information. Email sent to: {1} HostedKey: {2}", this.DBA, this.EmailTo, this.HostedPaymentKey);

                    //by Chandra, for PXP-2866: Add the merchant notes for PXP login and hosted key information email sent
                    AddMerchantNotes(MerchantID, notification.GetSubject() + " – Hosted Key");
                    //end of code PXP-2866

                    lblMessage.Text = "Hosted Payment email sent successfully!";
                }
                else
                {
                    lblMessage.Text = "Failed to send Hosted Payment email.";
                }
            }
            else
            {
                lblMessage.Text = "Please create login";
            }
        }
        catch (Exception exc)
        {
            lblMessage.Text = "Hosted Payment email sent failed. Error - " + exc.Message;
        }
    }


    protected void chMobilePaymentPage_CheckedChanged(object sender, EventArgs e)
    {
        if (!this.chkHostedPaymentPage.Checked)
            this.chkMobilePaymentPage.Checked = false;
        else
            chkHostedPaymentPage_CheckedChanged(sender, e);
    }
    //code changes for PXP-12673 by koshlendra start
    protected void chkAlternativeGatewayKey_CheckedChanged(object sender, EventArgs e)
    {
        if (this.chkAlternativeGatewayKey.Checked)
        {
            string sKey = ConfigurationManager.AppSettings["PaysafeEncryption_Key"];
            this.AlternativeGatewayKey.Text = CommonUtility.PaysafeEncryption.Encrypt(this.GatewayKey.Text, sKey);
            this.lblAltKey.Visible = true;
            this.AlternativeGatewayKey.Visible = true;

        }
        else
        {
            this.lblAltKey.Visible = false;
            this.AlternativeGatewayKey.Visible = false;
            this.AlternativeGatewayKey.Text = string.Empty;
        }
        UserFacade facade = new UserFacade();
        facade.UpdateAlternativeGatewayKey(this.UserUID, chkAlternativeGatewayKey.Checked, this.AlternativeGatewayKey.Text);
    }
    //code changes for PXP-12673 by koshlendra end
    private void AddMerchantNotes(string MerchantID, string subject)
    {
        MerchantNotes objMN = new MerchantNotes()
        {
            MerchantAppUID = MerchantID,
            Subject = subject,
            Notes = subject,
            RequiresCallback = false,
            UserCreated = UserSessions.CurrentUser.UserName,
            Email_Agent = false,
            View_MPSAll = true,
            View_Agent = false,
            View_Bank = false,
            RepeatIssue = false,
            Complaint = false,
            View_Merchant = false
        };

        DataMerchantApp.GetInstance().InsertMerchantNotes(objMN);

    }

    private void GetNMIKeyFromMerchant(string lockdownKey, string defaultRole)
    {

    }
}
