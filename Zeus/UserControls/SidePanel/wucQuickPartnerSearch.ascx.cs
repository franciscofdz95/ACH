using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.SidePanel
{
    public partial class wucQuickPartnerSearch : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            AgentID.Text = string.Empty;
            tbDBA.Text = string.Empty;
            tbFirst.Text = string.Empty;
            tbLast.Text = string.Empty;
            hidAgentUID.Text = "";
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                Hashtable prms = new Hashtable();
                prms["@ID"] = CommonUtility.Util.if_i(AgentID.Text, 0).ToString();

                if (UserSessions.CurrentUser.IsInternal)
                    prms["@InternalUserUID"] = UserSessions.CurrentUser.UID;

                DataTable dt = DataAgent.GetInstance().GetAgents_LightDT(prms);

                if (dt != null && dt.Rows.Count == 1)
                {
                    Dictionary<string, string> di = new Dictionary<string, string>();
                    di["AgentUID"] = dt.Rows[0]["AgentID"].ToString();
                    string url = string.Format(WebUtil.GetMyUrl(di, true));
                    Response.Redirect(url);
                }
                else
                {
                    RangeValidator1.IsValid = false;
                }
            }

        }
    }
}