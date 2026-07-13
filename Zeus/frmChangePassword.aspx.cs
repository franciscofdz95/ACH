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

using PaymentXP.Facade;
using PaymentXP.BusinessObjects;


public partial class frmChangePassword : frmBaseDataEntry
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //DM-7343 Nisha Magnani
        if (!this.IsPostBack)
        {            
            RegularExpressionValidator1.ValidationExpression = ConstantFacade.PasswordRegex;
            RegularExpressionValidator1.ErrorMessage = ConstantFacade.PasswordErrorMessage;
        }
        lblError.Visible = false;
        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

    }
    protected void btnChange_Click(object sender, EventArgs e)
    {
        UserFacade facade = new UserFacade();

        int result = facade.ChangePassword(UserSessions.CurrentUser.UID, txtOldPassword.Text, txtNewPassword.Text);

        if (result > 0)
        {
            lblError.Text = string.Empty;
            FormHandler.DisplayMessage(this.Page.ClientScript, "Password changed successfully");
            //Set Authentication Cookie when password is reset successfully so that the users are taken to 
            //the respective screens(in the code below) with out any issues.
            FormsAuthentication.SetAuthCookie(UserSessions.CurrentUser.UserName, false);


            if (UserSessions.CurrentUser.IsAgent)
                Response.Redirect("~/SecureLeadForms/frmLeads.aspx");
            if (UserSessions.CurrentUser.IsMerchant)
                Response.Redirect("~/frmLogin.aspx");
            else if (UserSessions.CurrentUser.IsBank)
                Response.Redirect("~/SecureMerchantManagementForms/frmMerchantSearch.aspx");
            else
                Response.Redirect("~/SecureHomeForms/frmHome.aspx");

        }
        else
        {
            switch (result)
            {
                case (int)UserFacade.ePasswordError.UnknownFailure:
                    lblError.Text = "Not able to change the existing password.";
                    lblError.Visible = true;
                    break;
                case (int)UserFacade.ePasswordError.CurrentPasswordMismatch:
                    lblError.Text = "Current password does not match supplied old password";
                    lblError.Visible = true;
                    break;

                case (int)UserFacade.ePasswordError.UsedWithinLastXAttempts:
                    lblError.Text = "New password should be different than last 5 times used passwords.";
                    lblError.Visible = true;
                    break;

                default:
                    lblError.Text = "Could not change password.";
                    lblError.Visible = true;
                    break;
            }

        }

    }



    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
            if (UserSessions.CurrentUser.IsAgent)
                Response.Redirect("~/SecureLeadForms/frmLeads.aspx");
            if (UserSessions.CurrentUser.IsMerchant)
                Response.Redirect("~/frmLogin.aspx");
            else if (UserSessions.CurrentUser.IsBank)
                Response.Redirect("~/SecureMerchantManagementForms/frmMerchantSearch.aspx");
            else
                Response.Redirect("~/SecureHomeForms/frmHome.aspx");
    }
}
