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

using PaymentXP.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PaymentXP.Facade;

public partial class MasterPageTicket : frmBaseMaster
{
    public int ErrorCount()
    {
        return this.WucMessage1.ErrorCount();
    }

    public void AddMessageError(string msg)
    {
        this.WucMessage1.AddMessageError(msg);
    }

    public void AddMessageStatus(string msg)
    {
        this.WucMessage1.AddMessageStatus(msg);
    }

    public void SetStatusBarText(string msg)
    {
        wucTopMenu1.StatusBarText = msg;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");


        this.HandleSideMenu();

        if (!Page.IsPostBack)
        {


            if (CommonUtility.Util.IsValidGuid(Request.QueryString["TicketUID"]))
            {
                string tuid = Request.QueryString["TicketUID"].ToUpper();

                TicketNotification.InsertTicketAccessHistory(new Guid(UserSessions.CurrentUser.UID), new Guid(tuid));

                //UserApplication.AddViewLogTicket(tuid, UserSessions.CurrentUser.UserName);
            }

        }

        this.Page.PreRender += new EventHandler(Page_PreRender);
    }

    protected void HandleSideMenu()
    {
        foreach (Control c in pnlSideMenu.Controls)
        {
            if (c is HyperLink)
            {
                HyperLink h = (HyperLink)c;

                // the search link should always be visible no matter what.
                if (h.ID != "lnkSearchTickets")
                {
                    if (UserSessions.CurrentTicket != null)
                    {
                        h.Enabled = true;
                        h.NavigateUrl = WebUtil.InjectParam(h.NavigateUrl, "TicketUID", Request.QueryString["TicketUID"]);
                    }
                    else
                    {
                        h.NavigateUrl = string.Empty;
                        h.CssClass = "disabledText";
                        h.Enabled = false;
                    }
                }

            }
        }
    }

    private string BuildRecentViewTable()
    {
        List<dynamic> li = TicketNotification.GetTicketAccessOfUser(new Guid(UserSessions.CurrentUser.UID));

        if (li != null && li.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='recently'>");
            foreach (dynamic d in li)
            {

                if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.TicketID == d.TicketID.ToString())
                {
                    continue;
                }

                sb.AppendFormat(@"
                        <tr>
                            <td>
                                <a href='{0}SecureTicketForms/frmTicketDetail.aspx?Adding=false&TicketUID={1}'>{2}</a>
                            </td>
                            <td>
                                DBA: {3}
                            </td>
                            <td>
                                <span class='minago'>({4})</span>
                            </td>
                        </tr>",
                    WebUtil.GetBaseUrl(),
                    d.TicketUID,
                    d.TicketID,
                    d.BusinessDBAName,
                    CommonUtility.Util.GetFriendlyDateDiff(d.DateLastAccessed)
                    );
            }
            sb.Append("</table>");
            return sb.ToString();
        }
        else
        {
            return null;
        }
    }

    private string BuildRecentUserTable()
    {
        string ret = string.Empty;

        if (UserSessions.CurrentTicket != null)
        {
            List<dynamic> li = TicketNotification.GetTicketAccessOfTicket(new Guid(UserSessions.CurrentTicket.TicketUID));

            if (li != null && li.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table class='recently'>");
                foreach (dynamic d in li)
                {
                    if (d.Username == UserSessions.CurrentUser.UserName)
                    {
                        continue;
                    }

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
                sb.Append("</table>");

                ret = sb.ToString();
            }
        }

        return ret;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        // handle recently viewed box
        litRecent.Text = this.BuildRecentViewTable();
        pnlRecent.Visible = !string.IsNullOrWhiteSpace(litRecent.Text);

        // handle other users viewed box
        litUsers.Text = this.BuildRecentUserTable();
        pnlLastViewed.Visible = !string.IsNullOrWhiteSpace(litUsers.Text);
    }


}
