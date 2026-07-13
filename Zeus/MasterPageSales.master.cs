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
using PaymentXP.DataObjects;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public partial class MasterPageSales : frmBaseMaster
{

    public enum eMasterSideMenu : int
    {
        NotSet,
        SearchLeads,
        LeadDetails,
        AssignLeads,
        Notes,
        Appointments,
        History,
        Reports,
        Documents,
        StatusQueues
    }

    public void SideMenuSelect(eMasterSideMenu eM)
    {
        switch (eM)
        {

            case eMasterSideMenu.SearchLeads:
                lnkSearchLeads.CssClass = "active";
                break;

            case eMasterSideMenu.LeadDetails:
                lnkLeadDetails.CssClass = "active";
                break;

            case eMasterSideMenu.AssignLeads:
                lnkAssignLeads.CssClass = "active";
                break;

            case eMasterSideMenu.History:
                lnkHistory.CssClass = "active";
                break;

            case eMasterSideMenu.Reports:
                lnkLeadReports.CssClass = "active";
                break;

            case eMasterSideMenu.Documents:
                lnkLeadDocuments.CssClass = "active";
                break;

            case eMasterSideMenu.StatusQueues:
                lnkLeadQueues.CssClass = "active";
                break;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");


        this.HandleSideMenu();

        if (!Page.IsPostBack)
        {
        
            if (UserSessions.CurrentLead != null)
            {
                wucTopMenu1.StatusBarText = String.Format("<span class='infokey'>Lead:</span> <span class='infoval'>{0}</span>", UserSessions.CurrentLead.DBAName);
            }

            if (CommonUtility.Util.IsValidGuid(Request.QueryString["LeadUID"]))
            {
                string leaduid = Request.QueryString["LeadUID"].ToUpper();

                if (UserSessions.diLUID == null)
                {
                    UserSessions.diLUID = new Dictionary<string, DateTime>();
                }

                UserSessions.diLUID[leaduid] = DateTime.Now;

            }
        }

        this.Page.PreRender += new EventHandler(Page_PreRender);

    }

    protected void HandleSideMenu()
    {
        List<string> liNeverDisabled = new List<string>() { "lnkSearchLeads", "lnkLeadReports", "lnkAssignLeads", "lnkLeadQueues" };

        foreach (Control c in pnlSideMenu.Controls)
        {
            if (c is HyperLink)
            {
                HyperLink h = (HyperLink)c;

                if (UserSessions.CurrentLead != null)
                {
                    h.Enabled = true;
                    h.NavigateUrl = WebUtil.InjectParam(h.NavigateUrl, "LeadUID", Request.QueryString["LeadUID"]);
                }
                else
                {
                    if (!liNeverDisabled.Contains( h.ID))
                    {
                        h.NavigateUrl = string.Empty;
                        h.CssClass = "disabledText";
                        h.Enabled = false;
                    }
                }
            }
        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        litRecent.Text = "";

        if (UserSessions.diLUID != null)
        {
            pnlRecent.Visible = true;
            StringBuilder sb = new StringBuilder();


            var mylist = UserSessions.diLUID.OrderByDescending(x => x.Value).Take(MAX_LAST_VIEW).ToList();

            UserSessions.diLUID = mylist.ToDictionary(a => a.Key, b => b.Value);

            sb.Append("<table class='recently'>");

            foreach (KeyValuePair<string, DateTime> kvp in mylist)
            {
                if (UserSessions.diCurrentLead != null && UserSessions.diCurrentLead.ContainsKey(kvp.Key))
                {

                    sb.AppendFormat(@"
                        <tr>
                            <td>
                                <a href='{0}SecureLeadForms/frmLeadsDetail.aspx?Adding=false&LeadUID={1}'>{2}</a>
                            </td>
                            <td>
                                {3} 
                            </td>
                            <td>
                                <span class='minago'>({4})</span>
                            </td>
                        </tr>",
                        WebUtil.GetBaseUrl(),
                        UserSessions.diCurrentLead[kvp.Key].LeadUID,
                        UserSessions.diCurrentLead[kvp.Key].LeadID.ToString(),
                        UserSessions.diCurrentLead[kvp.Key].DBAName,
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

    /// <summary>
    /// handles the notes in the boxes at the top.
    /// </summary>
    /// <param name="merchantMemo"></param>
    /// <param name="agentMemo"></param>
    /// <param name="firstteamMemo"></param>
    /// <param name="EditMode"></param>
    public void ShowNotes(string agentMemo, bool EditMode)
    {
        pnlMasterTable.Visible = true;

        AgentMemo.Text = agentMemo;
        AgentMemo.Enabled = true;
        pnlAgentMemo.Visible = AgentMemo.Text.Trim() != string.Empty;
       

    }


}
