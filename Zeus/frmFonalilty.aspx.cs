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


public partial class frmFonalilty : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Request["q"] != null)
            lblPhone.Text = Request["q"];

        if (Request["mid"] != null)
            lblMID.Text = Request["mid"];


        if (Request["username"] != null)
            txtUserName.Text = Request["username"];


        if (Request["password"] != null)
            txtPassword.Text = Request["password"];

        if (Request["landingpage"] != null)
            lblLandingPage.Text = Request["landingpage"];

        //txtUserName.Text = "mnguyen";
        //txtPassword.Text = "db66884f40fcebc6e7c2bcc678b4a176";
        //lblPhone.Text = "714-679-2099";
        //lblMID.Text = "510159390101806";

        if (FormDataCheck())
        {
            if (UserSessions.CurrentUser != null)
            {
                User user = UserSessions.CurrentUser;

                if (!user.IsInternal)
                {
                    lblError.Text = "You DO NOT have access!";
                    return;
                }

                FormsAuthentication.SetAuthCookie(txtUserName.Text, false);

                if (user.UserRoles.Count == 0)
                    Response.Redirect("~/frmLogin.aspx");
                else
                {
                    switch (lblLandingPage.Text.ToUpper())
                    {
                        case "AGENT":
                            FormSearchAgent();
                            break;
                        default:
                            FormSearchMerchant();
                            break;
                    }

                }
            }
        }
    }

    public bool FormDataCheck()
    {
        UserFacade facade = new UserFacade();

        string clientIp = CommonUtility.WebUtil.GetTrueClientIP(this.Request);

        if (!facade.IPAddressHasAccess(clientIp))
        {
            lblError.Text = "Your IP Address is not allow to access this website!";
            return false;
        }

        Hashtable prms = new Hashtable();
        prms.Add("@Username", txtUserName.Text);
        User u = facade.GetUser(prms);

        if (u == null)
        {
            lblError.Text = "Invalid User!";
            return false;
        }

        string pwdhash = CommonUtility.Security.getMd5Hash(u.Password);

        if (txtPassword.Text != pwdhash)
        {
            lblError.Text = "Invalid User!";
            return false;
        }

        User user = facade.GetUserLogin(txtUserName.Text, u.Password, UserSessions.PortalUID, clientIp);
        
        if (user == null)
        {
            lblError.Text = "Invalid user name or password";
            return false;
        }
        else
            UserSessions.CurrentUser = user;

        return true;
    }
    //Ali Change control PXP-7232
    protected void btnLogin_Click(object sender,EventArgs e)
    {

        if (FormDataCheck())
        {
            if (UserSessions.CurrentUser != null)
            {
                User user = UserSessions.CurrentUser;

                if (!user.IsInternal)
                {
                    lblError.Text = "You DO NOT have access!";
                    return;
                }

                FormsAuthentication.SetAuthCookie(txtUserName.Text, false);

                if (user.UserRoles.Count == 0)
                    Response.Redirect("~/frmLogin.aspx");
                else
                {
                    switch (lblLandingPage.Text.ToUpper())
                    {
                        case "AGENT":
                            FormSearchAgent();
                            break;
                        default:
                            FormSearchMerchant();
                            break;
                    }
                    


                }
            }
        }
    }

    protected void FormSearchMerchant()
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (lblPhone.ToString().Trim() != string.Empty)
            prms.Add("@BusinessDBAPhone", lblPhone.Text);

        if (lblMID.ToString().Trim() != string.Empty)
            prms.Add("@SettlePlatformMid", lblMID.Text);

        
        if (prms.Count > 0)
        {
            //Do not move code below
            //if (UserSessions.CurrentUser.IsAgent)
            //    prms.Add("MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);

            //Save search fields in session variable

            SearchParameter sp = new SearchParameter();
            sp.BusinessDBAPhone = lblPhone.Text;
            Session["ASP.securemerchantmanagementforms_frmmerchantsearch_aspx_SearchParamters"] = sp;

            DataSet ds =  DataAccess.DataMerchantAppDao.GetMerchantApps(prms);

            string url = string.Empty;
            if (ds.Tables[0].Rows.Count == 1)
            {
                MerchantFacade facade = new MerchantFacade();
                MerchantApp app = facade.GetMerchantAppZeus(ds.Tables[0].Rows[0][0].ToString());

                url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + app.MerchantAppUID + "Adding=false&PostBackURL=~/SecureHomeForms/frmHome.aspx";
                Response.Redirect(url);

            }
            else
            {
                url = "~/SecureMerchantManagementForms/frmMerchantSearch.aspx?PostBackURL=~/SecureHomeForms/frmHome.aspx";
            }

            Response.Redirect(url);


        }
        else
        {
            prms.Add("@ID", -1);
        }


    }


    protected void FormSearchAgent()
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (lblPhone.ToString().Trim() != string.Empty)
            prms.Add("@Phone", lblPhone.Text);


        if (prms.Count > 0)
        {
            //Do not move code below
            //if (UserSessions.CurrentUser.IsAgent)
            //    prms.Add("MasterAgentUID", UserSessions.CurrentUser.HookTableKeyUID);

            //Save search fields in session variable

            SearchParameter sp = new SearchParameter();
            sp.AgentPhone = lblPhone.Text;
            Session["ASP.secureagentmanagementforms_frmsearchagents_aspx_SearchParamters"] = sp;
                     

            DataSet ds = DataAccess.DataAgentDao.GetAgentsDS(prms);

            string url = string.Empty;
            if (ds.Tables[0].Rows.Count == 1)
            {
                Agent agent = DataAccess.DataAgentDao.GetAgent_List(ds.Tables[0].Rows[0][0].ToString());
                //UserSessions.CurrentAgent = agent;

                url = "~/SecureAgentManagementForms/frmAgent.aspx?Adding=false&AgentUID=" + agent.AgentUID + "&PostBackURL=~/SecureHomeForms/frmHome.aspx";
                Response.Redirect(url);

            }
            else
            {
                url = "~/SecureAgentManagementForms/frmSearchAgents.aspx?PostBackURL=~/SecureHomeForms/frmHome.aspx";
            }

            Response.Redirect(url);


        }
        else
        {
            prms.Add("@ID", -1);
        }


    }

}
