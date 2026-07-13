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

/// <summary>
/// Summary description for frmBaseMain
/// </summary>
public abstract class frmBaseMaster : System.Web.UI.MasterPage
{

    public static int MAX_LAST_VIEW = 5;
    
    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (Context.Session != null && UserSessions.CurrentUser == null)
        {
            if (Session.IsNewSession)
            {
                string szCookieHeader = Request.Headers["Cookie"];
                if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    Response.Redirect("~/frmLogin.aspx");
                }
            }
        }

        if (!this.IsPostBack)
        {
            FormHandler.SetSecurity(this);
        }


    }  


}
