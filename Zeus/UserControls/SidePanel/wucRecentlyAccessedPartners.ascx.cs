using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ZeusWeb.UserControls.SidePanel
{
    public partial class wucRecentlyAccessedPartners : System.Web.UI.UserControl
    {
        public static int MAX_LAST_VIEW = 5;

        protected void Page_Load(object sender, EventArgs e)
        {
            litRecent.Text = "";

            if (UserSessions.diAUID != null)
            {
                pnlRecent.Visible = true;
                StringBuilder sb = new StringBuilder();


                var mylist = UserSessions.diAUID.OrderByDescending(x => x.Value).Take(MAX_LAST_VIEW).ToList();

                UserSessions.diAUID = mylist.ToDictionary(a => a.Key, b => b.Value);

                sb.Append("<table class='recently'>");

                foreach (KeyValuePair<string, DateTime> kvp in mylist)
                {
                    if (UserSessions.diCurrentAgent != null && UserSessions.diCurrentAgent.ContainsKey(kvp.Key))
                    {

                        sb.AppendFormat(@"
                        <tr>
                            <td>
                                <a href='{0}SecureAgentManagementForms/frmAgent.aspx?Adding=false&AgentUID={1}'>{2}</a>
                            </td>
                            <td>
                                {3} 
                            </td>
                            <td>
                                <span class='minago'>({4})</span>
                            </td>
                        </tr>",
                            WebUtil.GetBaseUrl(),
                            UserSessions.diCurrentAgent[kvp.Key].AgentUID,
                            UserSessions.diCurrentAgent[kvp.Key].AgentID.ToString(),
                            UserSessions.diCurrentAgent[kvp.Key].AgentDBA,
                            CommonUtility.Util.GetFriendlyDateDiff(kvp.Value)
                            );
                    }

                }
                sb.Append("</table>");

                litRecent.Text = sb.ToString();
            }
            else
            {
                pnlRecent.Visible = false;
            }
        }
    }
}