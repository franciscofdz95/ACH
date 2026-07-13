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
using PaymentXP.Facade;
using CommonUtility;



public partial class MasterPageMerchant : frmBaseMaster
{
    string strPrivateLabelAgentId;
    public enum eMasterSideMenu : int
    {
        NotSet,
        Search,
        Profile,
        Fees,
        Owners,
        ACH,
        Credit,
        Pending,
        WebsiteReview,
        Collections,
        Risk,
        FraudXP,
        Deployment,
        Statements,
        Invoicing,
        Services,
        Notes,
        Emails,
        History,
        Corporate,
        Documents,
        PCI,
        FirstTeam,
        Alerts,
        Products
    }

   public void SideMenuSelect(eMasterSideMenu eM)
    {
        //code added for PXP-16537
        wucTopMenu1.StatusBarZIDText = string.Empty;
        switch (eM)
        {

            case eMasterSideMenu.Search:              
                wucTopMenu1.StatusBarText = "Search";
                lnkMerchantTabSearch.CssClass = "active";
                break;

            case eMasterSideMenu.Profile:
                wucTopMenu1.StatusBarText = "Profile";
                lnkMerchantTabProfile.CssClass = "active";
                break;

            case eMasterSideMenu.Fees:
                wucTopMenu1.StatusBarText = "Fees";
                lnkMerchantTabFees.CssClass = "active";
                break;

            case eMasterSideMenu.Owners:
                wucTopMenu1.StatusBarText = "Owners";
                lnkMerchantTabOwners.CssClass = "active";
                break;

            case eMasterSideMenu.ACH:
                wucTopMenu1.StatusBarText = "ACH/DD";
                lnkMerchantTabACHProfile.CssClass = "active";
                break;

            case eMasterSideMenu.Credit:
                wucTopMenu1.StatusBarText = "Credit";
                lnkMerchantTabUnderwriting.CssClass = "active";
                break;

            case eMasterSideMenu.Pending:
                wucTopMenu1.StatusBarText = "Pending";
                lnkMerchantTabPending.CssClass = "active";
                break;

            case eMasterSideMenu.WebsiteReview:
                wucTopMenu1.StatusBarText = "Website Review";
                lnkMerchantCompliance.CssClass = "active";
                break;

            case eMasterSideMenu.Collections:
                wucTopMenu1.StatusBarText = "Collections";
                lnkMerchantCollections.CssClass = "active";
                break;

            case eMasterSideMenu.Risk:
                wucTopMenu1.StatusBarText = "Risk";
                lnkMerchantTabRisk.CssClass = "active";
                break;

            //case eMasterSideMenu.FraudXP:
            //    wucTopMenu1.StatusBarText = "FraudXP";
            //    lnkMerchantTabFraudXP.CssClass = "active";
            //    break;

            case eMasterSideMenu.Products:
                wucTopMenu1.StatusBarText = "Products";
                lnkMerchantProducts.CssClass = "active";
                break;

            case eMasterSideMenu.Deployment:
                wucTopMenu1.StatusBarText = "Deployment";
                lnkMerchantTabDeployment.CssClass = "active";
                break;

            case eMasterSideMenu.Statements:
                wucTopMenu1.StatusBarText = "Statements";
                lnkMerchantTabStatements.CssClass = "active";
                break;

            case eMasterSideMenu.Invoicing:
                wucTopMenu1.StatusBarText = "Invoicing";
                lnkMerchantTabInvoicing.CssClass = "active";
                break;

            //sedavelli : Commenting out the page instead of removing it from the code, as this page is no longer required.
            //case eMasterSideMenu.Services:
            //    wucTopMenu1.StatusBarText = "Services";
            //    lnkMerchantTabServices.CssClass = "active";
            //    break;

            case eMasterSideMenu.Notes:
                wucTopMenu1.StatusBarText = "Notes";
                lnkMerchantTabNotes.CssClass = "active";
                break;

            case eMasterSideMenu.Emails:
                wucTopMenu1.StatusBarText = "Emails";
                lnkMerchantTabEmails.CssClass = "active";
                break;

            case eMasterSideMenu.History:
                wucTopMenu1.StatusBarText = "History";
                lnkMerchantTabHistory.CssClass = "active";
                break;

            case eMasterSideMenu.Corporate:
                wucTopMenu1.StatusBarText = "Corporate";
                lnkMerchantTabCorporateAccounts.CssClass = "active";
                break;

            case eMasterSideMenu.Documents:
                wucTopMenu1.StatusBarText = "Documents";
                lnkMerchantTabDocuments.CssClass = "active";
                break;

            case eMasterSideMenu.PCI:
                wucTopMenu1.StatusBarText = "PCI";
                lnkMerchantTabPci.CssClass = "active";
                break;

            case eMasterSideMenu.FirstTeam:
                wucTopMenu1.StatusBarText = "FirstTeam";
                //lnkMerchantTabFirstTeam.CssClass = "active";
                break;

            case eMasterSideMenu.Alerts:
                wucTopMenu1.StatusBarText = "Alerts";
                lnkMerchantAlerts.CssClass = "active";
                break;


        }

 }

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

    public void AddMessageSuccess(string msg)
    {
        this.WucMessage1.AddMessageSuccess(msg);
    }

    public void ToggleMenu(bool IsEnabled)
    {
        wucTopMenu1.ToggleMenu(IsEnabled);

        foreach (Control c in pnlSideMenu.Controls)
        {
            if (c is HyperLink)
            {
                HyperLink h = (HyperLink)c;
                h.Enabled = IsEnabled;
                h.CssClass = (IsEnabled) ? "" : "disabledText";
            }
        }
    }


    /// <summary>
    /// handles the notes in the boxes at the top.
    /// </summary>
    /// <param name="merchantMemo"></param>
    /// <param name="agentMemo"></param>
    /// <param name="firstteamMemo"></param>
    /// <param name="EditMode"></param>
    public void ShowNotes(string merchantMemo, string agentMemo, string firstteamMemo, bool EditMode)
    {
        pnlMasterTable.Visible = true;

        UWNotes.Text = "";
        UWSalesFNotes.Text = "";
        if (!UserSessions.CurrentMerchantApp.SalesFlag && !EditMode)
        {
            UWSalesFNotes.Text = ConfigurationManager.AppSettings["SalesFlagKey"].ToString() + "</br>";

          //  UWSalesFNotes.Text = "Sales Flag Disabled</br>";
        }
      
        UWNotes.Text = merchantMemo;
   
        AgentMemo.Text = agentMemo;
        FirstTeamNotes.Text = firstteamMemo;

        UWNotes.Enabled = true;
        UWNotesEdit.Visible = EditMode;
        UWNotesEdit.Text = HttpUtility.HtmlDecode(UWNotes.Text);
        UWNotes.Visible = !UWNotesEdit.Visible;

        //UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_CREDIT_UNDERWRITING, out role)

        bool has_access = false;

        foreach (KeyValuePair<string, UserRole> kvp in UserSessions.CurrentUser.UserRoles)
        {
            switch (kvp.Value.RoleID)
            {
                case Constants.ROLE_IT:
                case Constants.ROLE_ADMIN:
                case Constants.ROLE_FIRSTTEAM:
                    has_access = true;
                    break;
            }
        }

        FirstTeamNotes.Enabled = true;
        FirstTeamNotesEdit.Visible = EditMode && has_access;
        FirstTeamNotesEdit.Text = HttpUtility.HtmlDecode(FirstTeamNotes.Text);
        FirstTeamNotes.Visible = !FirstTeamNotesEdit.Visible;


        AgentMemo.Enabled = true;
        pnlAgentMemo.Visible = AgentMemo.Text.Trim() != string.Empty;
        pnlMerchantMemo.Visible = EditMode || UWNotes.Text.Trim() != string.Empty || UWSalesFNotes.Text.Trim() != string.Empty;
        pnlFirstTeam.Visible = EditMode || FirstTeamNotes.Text.Trim() != string.Empty;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //this.PreRender += new EventHandler(MasterPageMerchant_PreRender);

        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        this.HandleSideMenu();

        if (!Page.IsPostBack)
        {
            strPrivateLabelAgentId = ConfigurationManager.AppSettings["PrivateLabelAgentIds"];

            if (UserSessions.CurrentMerchantApp != null)
            {
                //code added for PXP-16537 start
                wucTopMenu1.StatusBarZIDText = "<span class='infokey'>ZID: </span>" + UserSessions.CurrentMerchantApp.ID.ToString();
                //code added for PXP-16537 end

                StringBuilder sbT = new StringBuilder();
                              
                sbT.AppendFormat("<span class='infokey'>AID:</span> <span class='infoval'>{0}</span>", UserSessions.CurrentMerchantApp.AchID.ToString());
                sbT.AppendFormat("<span class='infokey'>DBA:</span> <span class='infoval'>{0}</span>", UserSessions.CurrentMerchantApp.BusinessDBAName);

                if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() != Constants.BANK_ACH_ONLY)
                {
                    sbT.AppendFormat("<span class='infokey'>CC-Status:</span> <span class='infoval'>{0} </span>", UserSessions.CurrentMerchantApp.StatusName);
                }

                if (!string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.ACHStatus))
                {

                    sbT.AppendFormat("<span class='infokey'>ACH/DD-Status:</span> <span class='infoval'>{0} </span>", UserSessions.CurrentMerchantApp.ACHStatus);
                }

                if (!string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.ParentUID))
                {
                    sbT.AppendFormat("<span class='infokey'>PMLE:</span> <span class='infoval'><a href='{0}SecureMerchantManagementForms/frmCorporateAccounts.aspx?MerchantAppUID={1}'>{2}</a></span>"
                        , WebUtil.GetBaseUrl()
                        , UserSessions.CurrentMerchantApp.ParentUID
                        , UserSessions.CurrentMerchantApp.ParentBusinessDBAName);
                }


                bool migrated = DataMerchantApp.GetInstance().GetMigratedMIDs(int.Parse(UserSessions.CurrentMerchantApp.ID)).Count > 0;

                if (migrated)
                {
                    //if we find any entries of migrated mids for the current merchant then we know they've been migrated
                    sbT.AppendFormat("<span class='infoval'><a href='{0}SecureMerchantManagementForms/frmCorporateAccounts.aspx?MerchantAppUID={1}'>Bank Migrated</a></span>"
                        , WebUtil.GetBaseUrl()
                        , UserSessions.CurrentMerchantApp.MerchantAppUID);
                }

                wucTopMenu1.StatusBarText = sbT.ToString();

                pnlFirstTeam.Visible = UserSessions.CurrentMerchantApp.FirstTeam;

                // hide notes on Merchant Search Page.
                string strPage = System.IO.Path.GetFileName(Request.Path).Replace(".aspx", "").ToUpper();
                if (strPage != "FRMMERCHANTSEARCH")
                {
                    ShowNotes(UserSessions.CurrentMerchantApp.UWNotes, UserSessions.CurrentMerchantApp.AgentMemo, UserSessions.CurrentMerchantApp.FirstTeamNotes, false);
                }

            }


            if (CommonUtility.Util.IsValidGuid(Request.QueryString["MerchantAppUID"]))
            {
                // logged the merchant user view
                string mauid = Request.QueryString["MerchantAppUID"].ToUpper();

                // log merchant access for this user
                MerchantFacade.InsertMerchantAccessHistory(new Guid(UserSessions.CurrentUser.UID), new Guid(mauid));
            }
          
            this.HandleSidePanels();

            this.SetInterlinkPermissions();
        }



        this.PreRender += new EventHandler(MasterPageMerchant_PreRender);

    }

    protected void HandleSideMenu()
    {
        List<string> liNeverDisable = new List<string>() { "lnkMerchantTabSearch" };

        // handles visibility of side menus. injects appropriate params to links.
        foreach (Control c in pnlSideMenu.Controls)
        {
            if (c is HyperLink)
            {
                HyperLink h = (HyperLink)c;

                if (UserSessions.CurrentMerchantApp != null)
                {    
                    // Calling the GMA validation Logic
                            DataMerchantApp objMerchant = new DataMerchantApp();
                            bool Validation = objMerchant.GMAValidationCheck(UserSessions.CurrentUser, UserSessions.CurrentMerchantApp);

                            if (!Validation)
                            {
                                //    HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
                                //}
                                //            {

                                 if (!liNeverDisable.Contains(h.ID))
                                {
                                    h.NavigateUrl = string.Empty;
                                    h.CssClass = "disabledText";
                                    h.Enabled = false;

                                }
                        }
                      else                              
                    {
                        h.Enabled = true;
                        h.NavigateUrl = WebUtil.InjectParam(h.NavigateUrl, "MerchantAppUID", Request.QueryString["MerchantAppUID"]);

                        if (h.Text.Contains("ACH") && CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.AchID, 0) > 0)
                        {
                            h.NavigateUrl = WebUtil.InjectParam(h.NavigateUrl, "AchID", UserSessions.CurrentMerchantApp.AchID.ToString());
                        }
                    }
                  }
                else
                {
                    if (!liNeverDisable.Contains(h.ID))
                    {
                        h.NavigateUrl = string.Empty;
                        h.CssClass = "disabledText";
                        h.Enabled = false;
                    }
                }

            }
        }
    }

    protected void HandleSidePanels()
    {

        if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentUser.IsBank == false)
        {

            wucLastNotesMerchant1.LoadNotes(UserSessions.CurrentMerchantApp.MerchantAppUID, "");
            wucQuickMerchantSearch1.Visible = true;
            wucLastTicketsMerchant1.Visible = true;
            wucLastNotesMerchant1.Visible = true;

        }
        else
        {
            wucQuickMerchantSearch1.Visible = false;
            wucLastTicketsMerchant1.Visible = false;
            wucLastNotesMerchant1.Visible = false;
        }
    }

    protected void MasterPageMerchant_PreRender(object sender, EventArgs e)
    {

        //NOTE: be really careful with the viewstate when modifying controls. on observation, since i maniplulate the page controls on masterpage render, it
        //resets the viewstate for the child buttons. this resulted in the cancel button not toggling correctly on some of the forms... because the viewstate was reset to default.
        //just keep that in mind when manipulating controls on prerender from the masterpage.
        if (UserSessions.CurrentMerchantApp != null)
        {
            this.HandleFirstTeamBanner();
            this.HandleRolloverBanner();
        }

        //DM-7233 Nisha Magnani
        this.HandleTilledBanner();
    }

    private string GetPrivateLabelName()
    {
        string ret = "";

        if (string.IsNullOrEmpty(strPrivateLabelAgentId))
        {
            strPrivateLabelAgentId = "5159,5209";
        }

        if ((UserSessions.CurrentMerchantApp != null && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.PrivateLabelUID)) || strPrivateLabelAgentId.Contains(UserSessions.CurrentMerchantApp.AgentID.ToString()))
        {
            string myPLUID = UserSessions.CurrentMerchantApp.PrivateLabelUID.ToUpper();                     

            if (UserSessions.diCurrentPrivateLabel == null)
            {
                UserSessions.diCurrentPrivateLabel = new Dictionary<string, PrivateLabel>();
            }

            if (!UserSessions.diCurrentPrivateLabel.ContainsKey(myPLUID))
            {
                UserSessions.diCurrentPrivateLabel[myPLUID] = DataMerchantApp.GetInstance().GetPrivateLabel(myPLUID);
            }


            if (strPrivateLabelAgentId.Contains(UserSessions.CurrentMerchantApp.AgentID.ToString()))
            {
                ret = string.Format("<span class='ftpl'>{0}</span>", "Merchant Bank Card");
            }
            else if (UserSessions.diCurrentPrivateLabel.ContainsKey(myPLUID))
            {
                ret = string.Format("<span class='ftpl'>{0}</span>", UserSessions.diCurrentPrivateLabel[myPLUID].PLCompanyName);
            }


        }

        return ret;

    }

    private void HandleFirstTeamBanner()
    {
        // hide banner on Merchant Search Page.

        string strPage = System.IO.Path.GetFileName(Request.Path).Replace(".aspx", "").ToUpper();
                
        if (strPage != "FRMMERCHANTSEARCH")
        {
            if ((UserSessions.CurrentMerchantApp != null && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.FirstTeamRep) && UserSessions.CurrentMerchantApp.FirstTeam))
            {
                // this handles the First Team Banner
                Panel p = (Panel)ContentPlaceHolder1.FindControl("pnlBanner");

                if (p != null)
                {
                    Label lb = new Label();
                    lb.Text = "Premier Services Merchant";

                    string pl = this.GetPrivateLabelName();


                    if (!string.IsNullOrEmpty(pl))
                    {
                        lb.Text += ", " + pl;
                    }
                    
                    lb.CssClass = "ftright";

                    p.Controls.AddAt(0, lb);
                }
            }
        }

        //ftrightorange
    }

    private void HandleRolloverBanner()
    {
        // hide banner on Merchant Search Page.

        string strPage = System.IO.Path.GetFileName(Request.Path).Replace(".aspx", "").ToUpper();

        if (strPage != "FRMMERCHANTSEARCH")
        {
            if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.IsRolloverAccount)
            {
                // this handles the First Team Banner
                Panel p = (Panel)ContentPlaceHolder1.FindControl("pnlRollover");

                if (p != null)
                {
                    Label lb = new Label();
                    lb.Text = "Rollover Account";

                    lb.CssClass = "ftrightgainsboro";

                    p.Controls.AddAt(0, lb);
                }
            }
        }
    }

    private void SetInterlinkPermissions()
    {
        if (UserSessions.CurrentMerchantApp != null)
        {
            UserSessions.CurrentMerchantApp.SubscribedPortals = DataMerchantApp.GetInstance().GetSubscribedPortals(UserSessions.CurrentMerchantApp, false);

            interlinkPxp.Enabled = UserSessions.CurrentMerchantApp.HasAccessToPortal(Constants.PORTAL_PAYMENTXP) && UserSessions.CurrentUser.HasAccessToPortal(Constants.PORTAL_PAYMENTXP);
            interlinkInsight.Enabled = UserSessions.CurrentMerchantApp.HasAccessToPortal(Constants.PORTAL_MERCHANT) && UserSessions.CurrentUser.HasAccessToPortal(Constants.PORTAL_MERCHANT);
        }
        else
        {
            interlinkPxp.Enabled = false;
            interlinkInsight.Enabled = false;

        }
        interlinkPxp.CssClass = (!interlinkPxp.Enabled) ? "disabledText" : interlinkPxp.CssClass;
        interlinkInsight.CssClass = (!interlinkInsight.Enabled) ? "disabledText" : interlinkInsight.CssClass;


        if (UserSessions.CurrentUser.IsBank)
        {
            interlinkPxp.Visible = false;
            interlinkInsight.Visible = false;
        }
    }

    protected void interlinkPxp_Click(object sender, EventArgs e)
    {
        //1. Build Tokenized TransferUrl
        string url = LogonTokenFacade.BuildTransferUrl(ConfigurationManager.AppSettings["PaymentXpPortalLoginUrl"], UserSessions.CurrentUser, this.Request, UserSessions.CurrentMerchantApp);

        //2. open new window for the transfer url
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), CommonUtility.JSScriptProvider.BuildOpenWindowScript(url, "pxpWindow"), true);
    }

    protected void interlinkInsight_Click(object sender, EventArgs e)
    {
        //1. Build Tokenized TransferUrl
        string url = LogonTokenFacade.BuildTransferUrl(ConfigurationManager.AppSettings["MerchantPortalLoginUrl"], UserSessions.CurrentUser, this.Request, UserSessions.CurrentMerchantApp);

        //2. open new window for the transfer url
        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), CommonUtility.JSScriptProvider.BuildOpenWindowScript(url, "insightWindow"), true);
    }
    
    //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
     /// <summary>
    /// Update User edit Notification.
    /// </summary>
    /// <param name="Notification"></param>    
    public void UpdateNotification(string notification)
    {
        wucTopMenu1.NotificationText = notification;

    }
    /******** End of PXP-2206 **************/

    //DM-7233 Nisha Magnani
    private void HandleTilledBanner()
    {
        Panel p = (Panel)ContentPlaceHolder1.FindControl("pnlGreenBanner");
        if (p != null)
        {
            if (UserSessions.CurrentMerchantApp != null)
            {
                p.Visible = WebUtil.IsTilledStatus(UserSessions.CurrentMerchantApp);
            }
            else
            {
                p.Visible = false;
            }
        }
    }
}
