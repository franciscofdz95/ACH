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

public partial class frmChangePasswordNotification : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = "You have " + Request["Days"].ToString() + " days to change your password.";
    }

    protected void btnChangeNow_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmChangePassword.aspx?Redirect=true&Password=true&PostBackURL=~/frmLogin.aspx");
    }

    protected void btnChangeLater_Click(object sender, EventArgs e)
    {
        //if (UserSessions.CurrentUser.IsAgent)
        //    Response.Redirect("~/SecureLeadForms/frmLeads.aspx");
        //FormHandler.OpenModule(UserSessions.CurrentUser.DefaultRoleName);

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
