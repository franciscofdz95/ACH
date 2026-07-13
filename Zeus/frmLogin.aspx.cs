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
using System.Collections.Generic;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Reflection;


public partial class frmLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //MerchantApp app = new MerchantApp();
        //app = DataAccess.DataMerchantAppDao.GetMerchantAppZeus("CE143F92-6023-4873-87C1-6C20E1689032");
        //DataAccess.DataMerchantAppDao.FillMerchantAppZeusTransDB("CE143F92-6023-4873-87C1-6C20E1689032", app);

        //DataAccess.DataMerchantAppDao.GetMerchantAppZeus("CE143F92-6023-4873-87C1-6C20E1689032");

        //System.Reflection.Assembly MainAssembly = System.Reflection.Assembly.GetExecutingAssembly();
        //Type[] types = MainAssembly.GetTypes();
        //foreach(Type t in types )
        //{
        //    if (t != null && t.FullName.Substring(0,3) == "frm")
        //    {
        //        string s = t.ToString();

        //    }
        //}

        //	DM-5016 Save BIG-IP Cookie - Emanuel
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Request.Cookies: {0}", string.Join(";", Request.Cookies.AllKeys));
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));


        lblError.Text = "";
        lblError.Visible = false;
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (!this.IsPostBack)
        {
            if (Request.QueryString["relogin"] != null && Request.QueryString["relogin"] == "false")
            {
                Session.Clear();
                FormsAuthentication.SignOut();
            }
           


            //txtUserName.Text = "mnguyen";
            //txtPassword.TextMode = TextBoxMode.SingleLine;
            //txtPassword.Text = "Password@1";

            //txtUserName.Text = "mnguyen";
            //txtPassword.TextMode = TextBoxMode.SingleLine;
            //txtPassword.Text = "Justin!5";


            //txtUserName.Text = "stol";
            //txtPassword.TextMode = TextBoxMode.SingleLine;
            //txtPassword.Text = "Password@9";


            //txtUserName.Text = "VLombardo@nbcal.com";
            //txtPassword.TextMode = TextBoxMode.SingleLine;
            //txtPassword.Text = "Password@3";

            //txtUserName.Text = "mivy@mcpscorp.com";
            //txtPassword.TextMode = TextBoxMode.SingleLine;
            //txtPassword.Text = "Password@1";

            //txtUserName.Text = "stol";
            //txtPassword.TextMode = TextBoxMode.SingleLine;
            //txtPassword.Text = "Password@2";


        }
    }

    public bool FormDataCheck()
    {
        UserFacade facade = new UserFacade();

        string clientIp = CommonUtility.WebUtil.GetTrueClientIP(this.Request);

        if (!facade.IPAddressHasAccess(clientIp))
        {
            lblError.Text = "User not authorized to login.";
            lblError.Visible = true;
            return false;
        }

        User user = facade.GetUserLogin(txtUserName.Text, txtPassword.Text, UserSessions.PortalUID, clientIp);

        if (user == null)
        {
            lblError.Text = "Invalid login ID or password or your account may be disabled.";
            lblError.Visible = true;
            return false;
        }
        else
        {
            UserSessions.CurrentUser = user;
        }

        return true;
    }
    //Change the INfragistic for PXP-7232 By Ali Khan
    protected void btnLogin_Click(object sender, EventArgs e)
    {

        if (FormDataCheck())
        {
            if (UserSessions.CurrentUser != null)
            {
                User user = UserSessions.CurrentUser;

                //string hostheader = Request.ServerVariables["HTTP_HOST"].ToString().ToUpper();
                //string allowPublicIP = ConfigurationManager.AppSettings["AllowPublicIP"];

                //if (hostheader.Contains("MERITUSPAYMENT"))
                //{
                //    if (!(user.IsAgent && user.IsBank)) //allow partners and bank users
                //    {
                //        if (user.IsInternal && !allowPublicIP.Contains(Request.UserHostAddress)) //allow internal users that are whitelisted
                //        {
                //            lblError.Text = "YOU DO NOT HAVE ACCESS!";
                //            return;
                //        }
                //    }                    
                //}

                if (user.IsMerchant || (user.UserRoles != null && user.UserRoles.Count == 0))
                    Response.Redirect("~/frmLogin.aspx");
                else
                {
                    if (user.IsAgent)
                    {
                        UserSessions.CurrentLoggedInAgent = DataAccess.DataAgentDao.GetAgent_List(user.AgentUID);
                    }

                    if (user.PwdChangeDays > 0 && user.PwdChangeDays < 11)
                    {
                        //Set Aunthentication only when the password is active.
                        FormsAuthentication.SetAuthCookie(txtUserName.Text, false);
                        Response.Redirect("~/frmChangePasswordNotification.aspx?Days=" + user.PwdChangeDays.ToString());
                    }
                    else if (user.PwdChangeDays <= 0)
                        Response.Redirect("~/frmChangePassword.aspx?Redirect=true&Password=true");
                    else
                    {
                        //Set Aunthentication only when the password is active.
                        FormsAuthentication.SetAuthCookie(txtUserName.Text, false);
                        if (user.IsAgent)
                            Response.Redirect("~/SecureLeadForms/frmLeadQueues.aspx");
                        else if (user.IsBank)
                            Response.Redirect("~/SecureMerchantManagementForms/frmMerchantSearch.aspx");
                        else
                            Response.Redirect("~/SecureHomeForms/frmHome.aspx");
                    }
                }
            }
        }
    }

    private void GetControls(IList<ControlObject> objects)
    {
        DataUser data = DataAccess.DataUserDao;
        foreach (ControlObject obj in objects)
        {
            data.GetObjectDetails(obj);

            if (obj.ControlObjectDetails.Count > 0)
                this.GetControls(obj.ControlObjectDetails);
        }
    }
}
