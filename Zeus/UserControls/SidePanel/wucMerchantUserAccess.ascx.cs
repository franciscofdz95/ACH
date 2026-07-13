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
    public partial class wucMerchantUserAccess : wucBaseSearch
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FormShow();
            }
        }

        public void FormShow()
        {
            // handle other users viewed box
            litUsers.Text = this.BuildRecentUserTable();
            pnlLastViewed.Visible = !string.IsNullOrWhiteSpace(litUsers.Text);
        }

        private string BuildRecentUserTable()
        {

            string ret = string.Empty;

            List<dynamic> li = MerchantFacade.GetMerchantAccessOfMerchant(new Guid(UserSessions.CurrentMerchantApp.MerchantAppUID));

            if (li != null && li.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table class='recently'>");
                foreach (dynamic d in li)
                {

                    sb.AppendFormat(@"
                            <tr>
                                <td class='tleft'>
                                    {0}
                                </td>
                                <td class='tright'>
                                    <span class='minago'>({1})</span>
                                </td>
                            </tr>",
                        d.Username,
                        CommonUtility.Util.GetFriendlyDateDiff(d.DateLastAccessed)
                        );
                }

                ret = sb.ToString();
            }


            return ret;


        }

    }
}