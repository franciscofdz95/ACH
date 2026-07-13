using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using PaymentXP.Facade;

namespace ZeusWeb.UserControls
{
    public partial class wucRecentlyAccessedMerchants : wucBaseSearch
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // handle recently viewed box
                litRecent.Text = this.BuildRecentViewTable();
                pnlRecent.Visible = !string.IsNullOrWhiteSpace(litRecent.Text);

            }
        }

        private string BuildRecentViewTable()
        {

            string ret = string.Empty;

            List<dynamic> li = MerchantFacade.GetMerchantAccessOfUser(new Guid(UserSessions.CurrentUser.UID));

            if (li != null && li.Count > 0)
            {

                StringBuilder sb = new StringBuilder();

                sb.Append("<table class='recently'>");

                foreach (dynamic d in li)
                {
                    if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.MerchantAppUID == d.MerchantAppUID)
                    {
                        continue;
                    }

                    sb.AppendFormat(@"
                        <tr>
                            <td class='tleft'>
                                <a href='{0}SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID={1}'>{2}</a>
                            </td>
                            <td class='tleft'>
                                {3} 
                            </td>
                            <td class='tright'>
                                <span class='minago'>({4})</span>
                            </td>
                        </tr>",
                        WebUtil.GetBaseUrl(),
                        d.MerchantAppUID,
                        d.ZID.ToString(),
                        d.BusinessDBAName,
                        CommonUtility.Util.GetFriendlyDateDiff(d.DateLastAccessed)
                        );


                }
                sb.Append("</table>");

                ret = sb.ToString();
            }

            return ret;

        }

    }
}