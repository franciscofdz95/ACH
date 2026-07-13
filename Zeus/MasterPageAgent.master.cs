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
using PaymentXP.Facade;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public partial class MasterPageAgent : frmBaseMaster
{


    protected void Page_Load(object sender, EventArgs e)
    {

        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");


        if (!this.IsPostBack)
        {
            FormHandler.SetSecurity(this.Page.Master);

            if (UserSessions.CurrentAgent != null)
            {
                string template = @"
                    <span class='infokey'>PartnerID:</span> <span class='infoval'>{0}</span>
                    <span class='infokey'>DBA:</span> <span class='infoval'>{1} </span>
                    <span class='infokey'>FullName:</span> <span class='infoval'>{2} {3}</span>";

                wucTopMenu1.StatusBarText = String.Format(template, UserSessions.CurrentAgent.AgentID.ToString()
                        , UserSessions.CurrentAgent.AgentDBA
                        , UserSessions.CurrentAgent.FirstName
                        , UserSessions.CurrentAgent.LastName);
            }

            foreach (Control c in pnlSideMenu.Controls)
            {
                if (c is HyperLink)
                {
                    HyperLink h = (HyperLink)c;

                    if (UserSessions.CurrentAgent != null)
                    {
                        h.Enabled = true;
                        h.NavigateUrl = WebUtil.InjectParam(h.NavigateUrl, "AgentUID", Request.QueryString["AgentUID"]);
                    }
                    else
                    {
                        h.NavigateUrl = string.Empty;
                        h.CssClass = "disabledText";
                        h.Enabled = false;
                    }

                }
            }

            if (CommonUtility.Util.IsValidGuid(Request.QueryString["AgentUID"]))
            {
                string auid = Request.QueryString["AgentUID"].ToUpper();

                if (UserSessions.diAUID == null)
                {
                    UserSessions.diAUID = new Dictionary<string, DateTime>();
                }

                UserSessions.diAUID[auid] = DateTime.Now;

            }


        }

        this.Page.PreRender += new EventHandler(Page_PreRender);
        SetInterlinkPermissions();



        this.HandleSidePanels();


    }

    private void HandleSidePanels()
    {
        bool HasAgentInSession = (UserSessions.CurrentAgent != null) ? true : false;

        wucQuickPartnerSearch1.Visible = HasAgentInSession;
        wucLastTicketsAgent1.Visible = HasAgentInSession;
        wucAgentNotes1.Visible = HasAgentInSession;
    }

    public void AddMessageError(string msg)
    {
        this.WucMessage1.AddMessageError(msg);
    }

    public void AddMessageStatus(string msg)
    {
        this.WucMessage1.AddMessageStatus(msg);
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    private void SetInterlinkPermissions()
    {
        if (UserSessions.CurrentAgent != null)
        {
            interlinkApex.Enabled = UserSessions.CurrentUser.HasAccessToPortal(Constants.PORTAL_AGENT);
            interlinkApex.CssClass = "";
        }
        else
        {
            interlinkApex.Enabled = false;
            interlinkApex.CssClass = "disabledText";
        }
    }

    protected void interlinkApex_Click(object sender, EventArgs e)
    {
        //1. Build Tokenized TransferUrl
        string url = LogonTokenFacade.BuildTransferUrl(ConfigurationManager.AppSettings["AgentPortalLoginUrl"], UserSessions.CurrentUser, this.Request);

        //2. open new window for the transfer url
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), CommonUtility.JSScriptProvider.BuildOpenWindowScript(url), true);
    }
}
