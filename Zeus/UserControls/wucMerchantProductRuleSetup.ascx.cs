using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucMerchantProductRuleSetup : System.Web.UI.UserControl
    {
        private readonly IReadOnlyList<string> ROLES_ACCESS = new List<string>() {
                Constants.ROLE_OPERATIONS,
                Constants.ROLE_DEPLOYMENT
            };

        public event EventHandler SendChangesRulesClick;
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!UserSessions.CurrentUser.UserRoles.Keys.Intersect(ROLES_ACCESS).Any())
            {
                SendChangesRules.Visible = false;
            }
        }

        protected void SendChangesRules_Click(object sender, EventArgs e)
        {
            if (SendChangesRulesClick != null)
            {
                SendChangesRulesClick(this, EventArgs.Empty);
            }
        }
    }
}