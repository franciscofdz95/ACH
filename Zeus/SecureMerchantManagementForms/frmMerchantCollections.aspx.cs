using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.Facade;
using System.Collections;
using PaymentXP.DataObjects;

public partial class frmMerchantCollections : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Collections);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Collections");
            }

            this.FormShow(this.UID);
            chkPrimaryContactEmail.Checked = true;
        }
    }

    public override void FormShow(string ID)
    {
        // Disable the editablity of the controls. they're for read only on this page.
        WucBusinessInfo1.pnlInfo.Enabled = false;

        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);
        UserSessions.CurrentMerchantApp = agreement;

        FormBinding.BindObjectToControls(agreement, WucBusinessInfo1);

        WucBusinessInfo1.LoadOffice(agreement);

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();

            DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
            LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2));
        }


        Email.Text = agreement.BusinessEmailAddress;
    }

    public override void FormClear()
    {
        SecondEmail.Text = string.Empty;
        Amount.Text = string.Empty;
        chkSecondaryEmail.Checked = false;
        chkPrimaryContactEmail.Checked = true;
    }


    public override bool FormSave()
    {
        return true;
    }

    public override void FormNew()
    {
        throw new NotImplementedException();
    }

    public override bool FormDelete()
    {
        throw new NotImplementedException();
    }

    public override bool FormDataCheck()
    {
        decimal amount = CommonUtility.Util.if_dec(Amount.Text, 0);

        if (amount <= 0)
        {
            this.Master.AddMessageError("Please enter a valid amount.");
        }

        if (!chkPrimaryContactEmail.Checked && !chkSecondaryEmail.Checked)
        {

            this.Master.AddMessageError("Please check at least one checkbox to send collections email.");
        }
        else
        {
            if (chkPrimaryContactEmail.Checked && string.IsNullOrWhiteSpace(Email.Text))
            {
                this.Master.AddMessageError("Please input a valid primary contact email address.");
            }
            if (chkSecondaryEmail.Checked && !CommonUtility.Util.IsValidEmail(SecondEmail.Text))
            {
                this.Master.AddMessageError("Please input a valid secondary email address.");
            }
        }

        if (this.Master.ErrorCount() == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void FormCancel()
    {
        throw new NotImplementedException();
    }

    public override void ToggleButtons()
    {
        throw new NotImplementedException();
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        OTPKey objOTPKey = new OTPKey();
        OTPService objOTPService = new OTPService();

        MerchantApp app = UserSessions.CurrentMerchantApp;
        string emailTo = string.Empty;
        string emailCC = string.Empty;
        string OTP;

        if (FormDataCheck())
        {
            if (app != null)
            {
                //get the hsotedkey of the merchant that will be used to generating the Hosted Collections payment link
                HostedPaymentToken merchantToken = DataToken.SelectHostedPaymentToken(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), null);

                objOTPKey.MerchantKey = app.MerchantAppUID;
                objOTPKey.MerchantID = CommonUtility.Util.if_i(app.ID, 0);
                
                //if secondary email is entered then use it or else use the primary contact email.
                if (chkPrimaryContactEmail.Checked && !chkSecondaryEmail.Checked)
                {
                    emailTo = Email.Text;
                }
                else if (chkSecondaryEmail.Checked && !chkPrimaryContactEmail.Checked)
                {
                    emailTo = SecondEmail.Text;
                }
                else
                {
                    emailTo = Email.Text;
                    emailCC = SecondEmail.Text;
                }

                objOTPKey.EmailAddress = emailTo.Trim();
                objOTPKey.SecondaryEmailAddress = emailCC.Trim();
                objOTPKey.CreatedBy = UserSessions.CurrentUser.UserName;
                objOTPKey.Counter = 1;
                decimal amount = CommonUtility.Util.if_dec(Amount.Text, 0);

                OTP = objOTPService.GenerateOTP(objOTPKey);

                if (!string.IsNullOrWhiteSpace(OTP))
                {
                    MPSEmailTemplate emailTemplate = OTPService.NotifyCollectionDue(null, app, OTP, amount, objOTPKey.OTPKeyID);

                    if (emailTemplate != null)
                    {
                        bool isSent = FormHandler.SendEmail(emailTemplate.Subject, "", emailTemplate.Content, Constants.COLLECTIONS_EMAIL, emailTo, emailCC, "", new Hashtable(), UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentMerchantApp.AgentUID);

                        FormClear();

                        if (isSent)
                            Master.AddMessageSuccess("Email Sent Successfully");
                    }
                }
                else
                {
                    Master.AddMessageError("Error generating OTP");
                }
            }

        }
    }


}