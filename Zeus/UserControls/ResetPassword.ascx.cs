using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects.Notify;
using Newtonsoft.Json;
using PaymentXP.BusinessObjects.Logging;
using CommonUtility;

public partial class UserControls_ResetPassword : UserControl
{

    // we keep these objects handy so that we dont have to keep reloading them.
    private User _MyUser = null;

    private UserPasswordReset _MyUserPasswordReset = null;

    private string _EmailSubject = "Zeus: Password Reset Request";

    protected void Page_Load(object sender, EventArgs e)
    {

        this.always_init();

        if (!Page.IsPostBack)
        {
            this.initialize();
            //DM-7343 Nisha Magnani
            RegularExpressionValidator1.ValidationExpression = ConstantFacade.PasswordRegex;
            RegularExpressionValidator1.ErrorMessage = ConstantFacade.PasswordErrorMessage;
        }
    }

    protected void always_init()
    {
        this.Page.Title = "Reset Password";

        blError.Items.Clear();

        lblEmailSubject.Text = this._EmailSubject;


        if (!string.IsNullOrEmpty(Request.QueryString["c"]))
        {
            // if "c" is encountered in the url string, then that means the user requested a reset, and is coming back to change his password.
            // c contains the value of the primary key of the userpasswordreset table.

            pnlEmail.Visible = false;
            pnlConfirmation.Visible = false;

            if (this.IsCodeValid(Request.QueryString["c"]) == true)
            {
                pnlNewPassword.Visible = true;
                pnlError.Visible = false;
            }
            else
            {
                pnlNewPassword.Visible = false;
                pnlError.Visible = true;
            }
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["a"]))
        {
            // if "a" is encountered then that means a password reset was initiated and an email was sent to that person. i didnt want this
            // screen to be stuck on postback, so i had it redirect to escape postback.

            pnlEmail.Visible = false;
            pnlConfirmation.Visible = true;
            pnlNewPassword.Visible = false;
            pnlError.Visible = false;
            pnlThanks.Visible = false;
            this.EmailAddressDisplay.Text = Request["email"];
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["b"]))
        {
            // if "b" is encountered, then that means the password was successfully reset, and we are displaying the thank you screen.
            pnlEmail.Visible = false;
            pnlConfirmation.Visible = false;
            pnlNewPassword.Visible = false;
            pnlError.Visible = false;
            pnlThanks.Visible = true;
        }
    }

    private bool IsCodeValid(string code)
    {
        bool blnRet = false;

        UserPasswordReset objUPR = this.GetUserPasswordReset(code);

        if (objUPR != null)
        {
            this._MyUserPasswordReset = objUPR;
            blnRet = true;
        }

        return blnRet;
    }

    // a handy function to get us our object
    private UserPasswordReset GetUserPasswordReset(string UID)
    {
        DataUser dataU = DataAccess.DataUserDao;

        UserPasswordReset objUPR = dataU.GetUserPasswordReset(UID);

        return (objUPR != null) ? objUPR : null;

    }

    protected void initialize()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["c"]))
        {
            pnlEmail.Visible = false;
            pnlConfirmation.Visible = false;
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["a"]))
        {
            pnlEmail.Visible = false;
            pnlConfirmation.Visible = true;
        }
        else if (!string.IsNullOrEmpty(Request.QueryString["b"]))
        {
            pnlThanks.Visible = true;
        }
        else
        {
            pnlEmail.Visible = true;
            pnlConfirmation.Visible = false;
        }


    }

    protected IEnumerable<string> ValidateUsername()
    {
        if (string.IsNullOrEmpty(tbUsername.Text.Trim()))
        {
            yield return "Please Enter Your Username";
        }

        if (this.CheckIfUsernameExists(tbUsername.Text.Trim()) == false)
        {
            yield return "Error, We could not find your username";
        }

    }

    protected bool CheckIfUsernameExists(string username)
    {
        bool ret = false;

        User objU = this.GetUser(username);

        if (objU != null)
        {
            // we save the user in this private variable so that we dont have to fetch it again.
            this._MyUser = objU;
            ret = true;
        }

        return ret;

    }

    // a handy function to get us our user object
    private User GetUser(string username)
    {
        Hashtable prms = new Hashtable();
        DataUser data = DataAccess.DataUserDao;
        prms.Add("@UserName", username);
        IList<User> liUser = data.GetUsers(prms);
        return (liUser.Count == 1) ? liUser[0] : null;
    }

    protected bool CheckDataUsername()
    {
        foreach (string str in this.ValidateUsername())
        {
            blError.Items.Add(new ListItem(str));
        }

        return (blError.Items.Count == 0) ? true : false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        if (CheckDataUsername() && this._MyUser != null)
        {
            // we got the user.

            // insert into UserPasswordReset
            DataUser dataU = DataAccess.DataUserDao;

            string confirmation_key = dataU.InsertUserPasswordReset(this._MyUser.UID, this._MyUser.Email);

            if (confirmation_key != "")
            {
                // send email to him.
                this.SendEmailReset(confirmation_key);
            }
            else
            {
                blError.Items.Add("Could not insert into UserPasswordReset table");
            }

            // build redirect string and redirect.
            Dictionary<string, string> di = new Dictionary<string, string>();
            di.Add("a", "true");
            di.Add("email", this._MyUser.Email);
            string redir = WebUtil.GetMyUrl(di, true);
            Response.Redirect(redir);
        }
    }

    protected void SendEmailReset(string confirmation_key)
    {
        //get zeus pw reset internal notification
        InternalNotification notification = NotificationService.GetInternalNotification(NotificationType.ZeusResetPassword);

        //set user parameters
        NotificationService.SetDefaultInternalNotification(notification, this._MyUser);
        ///  PXP-16577 Fady Massoud 03-31-2021
        ///  Prevent Host Header Attack - Password Reset Poisoning        
        notification.SetMessageParameter("ResetLink", string.Format("{0}resetpassword.aspx?c={1}", CommonUtility.WebUtil.GetConfigBaseURL(), confirmation_key));

        //send notification
        NotificationService.SendNotification(notification);
        ZeusWeb.Logging.EmailLog.InfoFormat("ResetLink : {0} Password Reset link URL. and its confirmation key: {1}", CommonUtility.WebUtil.GetConfigBaseURL(), confirmation_key);
    }

    protected IEnumerable<string> ValidateConfirmation()
    {
        if (string.IsNullOrEmpty(tbUsernameConfirm.Text.Trim()))
        {
            yield return "Please Enter Your Username";
        }

        if (this._MyUserPasswordReset == null)
        {
            yield return "Password reset is expired.";
        }

        if (tbUsernameConfirm.Text.Trim().ToLower() != this._MyUserPasswordReset.Username.ToLower())
        {
            yield return "Username does not match!";
        }

        if (string.IsNullOrEmpty(tbPassword1.Text) || string.IsNullOrEmpty(tbPassword2.Text))
        {
            yield return "Passwords cannot be empty!";
        }
        else if (tbPassword1.Text != tbPassword2.Text)
        {
            yield return "Passwords do not match!";
        }
        else if (CommonUtility.Validation.PasswordCheck(tbPassword1.Text) != "")
        {
            yield return "Password must be 8 characters long, contain letters and numbers, and at least 1 uppercase letter.";
        }

    }

    protected bool CheckDataConfirmation()
    {
        foreach (string str in this.ValidateConfirmation())
        {
            blError.Items.Add(new ListItem(str));
        }

        return (blError.Items.Count == 0);
    }

    protected void btnSubmitConfirm_Click(object sender, EventArgs e)
    {

        if (this.CheckDataConfirmation() && this._MyUserPasswordReset != null)
        {
            // get user
            this._MyUser = this.GetUser(this._MyUserPasswordReset.Username);

            // change his password.
            UserFacade facade = new UserFacade();

            int result = facade.ChangePassword(this._MyUser.UID, this._MyUser.Password, tbPassword1.Text, this._MyUserPasswordReset.UID);

            if (result > 0)
                {
                    // password has changed.
                    if (this._MyUser.IsMFAEnabled && Constants.IS_MFA_ENABLED && 
                        (this._MyUser.IsAgent || this._MyUser.IsMerchant))
                    {
                        this._MyUser = this.GetUser(this._MyUserPasswordReset.Username);

                        try
                        {
                            if (!string.IsNullOrEmpty(this._MyUser.OktaUserID))
                            {
                                var oktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.SwitchStatusUser(this._MyUser.OktaUserID, this._MyUser.HasDBAccess, UserSessions.PortalUID);
                                if (oktaUser.Status.Equals(Okta.Sdk.Model.UserStatus.ACTIVE))
                                    this._MyUser.IsMFAActive = true;
                                else
                                    this._MyUser.IsMFAActive = false;

                                facade.UpdateUser(this._MyUser);
                            }
                            else if (this._MyUser.HasDBAccess)
                            {

                                string oktaPwd = facade.GetRandomPassword(8);
                                CreateOktaUser(ref this._MyUser, oktaPwd);
                                if (!string.IsNullOrEmpty(this._MyUser.OktaUserID))
                                    facade.UpdateUser(this._MyUser);
                            }
                        }
                        catch (Exception ex)
                        {
                            XPLogger.ZeusLog.Error($"Error happened while trying to do Okta Process - User: [{this._MyUser.UserName}], Exception: {ex.Message}");
                        }
                    }

                    Dictionary<string, string> di = new Dictionary<string, string>();
                    di.Add("b", "true");
                    string redir = WebUtil.GetMyUrl(di, true);
                    Response.Redirect(redir);
                }
            else
            {
                blError.Items.Add(Validation.ResetPasswordMessage(result));
            }
        }

    }
    private void CreateOktaUser(ref User user, string oktaPwd)
    {
        try
        {
            if (user != null && user.UID != "-1" && Constants.IS_MFA_ENABLED)
            {
                string userType = user.IsMerchant ? Constants.OKTA_USERTYPE_MERCHANT : Constants.OKTA_USERTYPE_AGENT;

                var oktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.CreateUser(new Okta.Sdk.Model.CreateUserRequest()
                {
                    Profile = new Okta.Sdk.Model.UserProfile()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Login = user.UserName,
                        Email = user.Email,
                        UserType = userType
                    },
                    GroupIds = new List<string>() { Constants.MFA_GROUP },
                    Credentials = new Okta.Sdk.Model.UserCredentials()
                    {
                        Password = new Okta.Sdk.Model.PasswordCredential()
                        {
                            Value = oktaPwd
                        }
                    }
                }, portalUID: UserSessions.PortalUID);

                if (oktaUser != null)
                {
                    user.IsMFAEnabled = true;
                    user.IsMFAActive = true;
                    user.OktaUserID = oktaUser.Id;
                    user.OktaPassword = oktaPwd;
                    user.MFAGroupID = Constants.MFA_GROUP;

                }
                else
                    XPLogger.ZeusLog.Error("User creation is not successful, UserName [" + user.UserName + "]. Please contact Administrator.");
            }
        }
        catch (Exception ex)
        {
            XPLogger.ZeusLog.Error("User creation is not successful, UserName [" + user != null ? user.UserName : string.Empty + "], Exception: " + ex.Message);
            throw;
        }
    }
}
