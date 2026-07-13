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
using PaymentXP.DataObjects;

public partial class MasterPageLogin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserSessions.CurrentFooterMessage = null;
        if (!this.IsPostBack)
        {
            // PXP-10181 RThakur Dynamic Footer Message
            UserSessions.CurrentFooterMessage = DataApp.Instance.GetPortalFooterMessage(1);
            if (UserSessions.CurrentFooterMessage != null)
            {
                footerMessage.InnerText = String.Format(UserSessions.CurrentFooterMessage.FooterMessage, DateTime.Now.ToString("yyyy"));
            }
        }
    }
}
