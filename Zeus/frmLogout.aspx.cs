using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ZeusWeb
{
    public partial class frmLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
            PaymentXP.Facade.MerchantFacade.RemoveUserEditingForZIDOnLogOut(UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
            /******** End of PXP-2206 **************/
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect(FormsAuthentication.LoginUrl, true);
            
        }
    }
}