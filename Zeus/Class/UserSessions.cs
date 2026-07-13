using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Collections;
using PaymentXP.BusinessObjects.Zeus;
using System.Web.SessionState;
using PaymentXP.BusinessObjects.App;//adedd for PXP-7622 by koshlendra

/// <summary>
/// Summary description for UserSessions
/// </summary>
public class UserSessions
{
    public UserSessions()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string PortalUID
    {
        get { return "e263d06c-a6f1-4c27-9103-231d8fee85c5"; }
    }

    #region CurrentMerchantApp
    public static int MerchantId
    {
        get
        {
            if (HttpContext.Current.Session["m"] == null)
                return 0;
            else
                return (int)HttpContext.Current.Session["m"];
        }
        set { HttpContext.Current.Session["m"] = value; }
    }


    public static Dictionary<string, MerchantApp> diCurrentMerchantApp
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentMerchantApp"] == null)
                return null;
            else
                return (Dictionary<string, MerchantApp>)HttpContext.Current.Session["diCurrentMerchantApp"];
        }
        set { HttpContext.Current.Session["diCurrentMerchantApp"] = value; }

    }


    // PXP-10181 RThakur Dynamic Footer Message
    public static AppsFooterMessage CurrentFooterMessage
    {
        get
        {
            if (HttpContext.Current.Session["CurrentFooterMessage"] == null)
                return null;
            else
                return (AppsFooterMessage)HttpContext.Current.Session["CurrentFooterMessage"];
        }
        set { HttpContext.Current.Session["CurrentFooterMessage"] = value; }
    }


    /// <summary>
    /// Requires MerchantAppUID in querystring
    /// </summary>
    public static MerchantApp CurrentMerchantApp
    {
        get
        {
            // first check to see param is good and valid.
            string merchant_app_uid = CommonUtility.Util.IsValidGuid(HttpContext.Current.Request.QueryString["MerchantAppUID"]) ? HttpContext.Current.Request.QueryString["MerchantAppUID"].ToUpper() : "";

            // if we have a good param, then we have a chance at retrieving the merchantapp
            if (!string.IsNullOrEmpty(merchant_app_uid))
            {
                // always make sure our dictionary is initialized
                if (diCurrentMerchantApp == null)
                {
                    diCurrentMerchantApp = new Dictionary<string, MerchantApp>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentMerchantApp.ContainsKey(merchant_app_uid))
                {
                    MerchantFacade facade = new MerchantFacade();
                    MerchantApp app = facade.GetMerchantAppZeus(merchant_app_uid);
                    if (app != null)
                    {
                        app.Owners = DataAccess.DataMerchantAppDao.GetOwners(app.MerchantAppUID);
                        app.TradeReferences = DataAccess.DataMerchantAppDao.GetTradeReferences(app.MerchantAppUID);

                        diCurrentMerchantApp[merchant_app_uid] = app;
                    }

                }

                return diCurrentMerchantApp[merchant_app_uid];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentMerchantApp == null)
            {
                diCurrentMerchantApp = new Dictionary<string, MerchantApp>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.IsValidGuid(value.MerchantAppUID))
            {
                string merchant_app_uid = value.MerchantAppUID.ToUpper();
                diCurrentMerchantApp[merchant_app_uid] = value;
                try
                {
                    MerchantId = int.Parse(diCurrentMerchantApp[merchant_app_uid].ID);
                }
                catch (Exception ex)
                { }
            }
        }
    }

    /// <summary>
    /// Merchant recent access list
    /// </summary>
    public static Dictionary<string, DateTime> diMAUID = null;

    #endregion

    #region CurrentAgent

    public static Dictionary<string, Agent> diCurrentAgent
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentAgent"] == null)
                return null;
            else
                return (Dictionary<string, Agent>)HttpContext.Current.Session["diCurrentAgent"];
        }
        set { HttpContext.Current.Session["diCurrentAgent"] = value; }

    }

    /// <summary>
    /// Requires AgentUID in querystring
    /// </summary>
    public static Agent CurrentAgent
    {
        get
        {
            // first check to see param is good and valid.
            string agent_uid = CommonUtility.Util.IsValidGuid(HttpContext.Current.Request.QueryString["AgentUID"]) ? HttpContext.Current.Request.QueryString["AgentUID"].ToUpper() : "";

            // if we have a good param, then we have a chance at retrieving the agentapp
            if (!string.IsNullOrEmpty(agent_uid))
            {
                // always make sure our dictionary is initialized
                if (diCurrentAgent == null)
                {
                    diCurrentAgent = new Dictionary<string, Agent>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentAgent.ContainsKey(agent_uid))
                {
                    diCurrentAgent[agent_uid] = DataAgent.GetInstance().GetAgent(agent_uid);
                }

                return diCurrentAgent[agent_uid];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentAgent == null)
            {
                diCurrentAgent = new Dictionary<string, Agent>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.IsValidGuid(value.AgentUID))
            {
                diCurrentAgent[value.AgentUID.ToUpper()] = value;
            }
        }
    }

    public static Dictionary<string, DateTime> diAUID = null;

    #endregion

    #region CurrentLead

    public static Dictionary<string, Lead> diCurrentLead
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentLead"] == null)
                return null;
            else
                return (Dictionary<string, Lead>)HttpContext.Current.Session["diCurrentLead"];
        }
        set { HttpContext.Current.Session["diCurrentLead"] = value; }

    }

    /// <summary>
    /// Requires LeadUID in querystring
    /// </summary>
    public static Lead CurrentLead
    {
        get
        {
            // first check to see param is good and valid.
            string Lead_uid = CommonUtility.Util.IsValidGuid(HttpContext.Current.Request.QueryString["LeadUID"]) ? HttpContext.Current.Request.QueryString["LeadUID"].ToUpper() : "";

            // if we have a good param, then we have a chance at retrieving the Leadapp
            if (!string.IsNullOrEmpty(Lead_uid))
            {
                // always make sure our dictionary is initialized
                if (diCurrentLead == null)
                {
                    diCurrentLead = new Dictionary<string, Lead>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentLead.ContainsKey(Lead_uid))
                {
                    diCurrentLead[Lead_uid] = DataLead.GetInstance().GetLead(Lead_uid);
                }

                return diCurrentLead[Lead_uid];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentLead == null)
            {
                diCurrentLead = new Dictionary<string, Lead>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.IsValidGuid(value.LeadUID))
            {
                diCurrentLead[value.LeadUID.ToUpper()] = value;
            }
        }
    }

    public static Dictionary<string, DateTime> diLUID = null;

    #endregion

    //code added by koshlendra for PXP-7622 start
    #region AttchemntsessiononCreateTicket
    public static int GetTemporaryAttachmentsCount(HttpSessionState State, string TicketID)
    {
        if (State[TicketID + "TemporarySession"] == null)
            return 0;
        else
            return ((Dictionary<string, byte[]>)State[TicketID + "TemporarySession"]).Count;
    }
    public static Dictionary<string, byte[]> GetTemporaryAttachments(HttpSessionState State, string TicketID)
    {
        return (Dictionary<string, byte[]>)State[TicketID + "TemporarySession"];
    }

    public static void AddToTemporaryAttachments(HttpSessionState State, string TicketId, string FileName, byte[] FileContent, int docTypeID)
    {
        if (State[TicketId + "TemporarySession"] == null)
        {
            State[TicketId + "TemporarySession"] = new Dictionary<string, byte[]>();
            State[TicketId + "DocumentTypes"] = new Dictionary<string, int>();
        }

        ((Dictionary<string, byte[]>)State[TicketId + "TemporarySession"]).Add(FileName, FileContent);
        ((Dictionary<string, int>)State[TicketId + "DocumentTypes"]).Add(FileName, docTypeID);
    }
    public static void ClearTemporaryAttachments(HttpSessionState State, string TicketID)
    {
        if (State[TicketID + "TemporarySession"] != null)
        {
            ((Dictionary<string, byte[]>)State[TicketID + "TemporarySession"]).Clear();
            ((Dictionary<string, int>)State[TicketID + "DocumentTypes"]).Clear();
        }

    }
    #endregion
    //code added by koshlendra for PXP-7622 end
    #region CurrentTicket

    public static Dictionary<string, Ticket> diCurrentTicket
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentTicket"] == null)
                return null;
            else
                return (Dictionary<string, Ticket>)HttpContext.Current.Session["diCurrentTicket"];
        }
        set { HttpContext.Current.Session["diCurrentTicket"] = value; }

    }

    /// <summary>
    /// Requires TicketUID in querystring
    /// </summary>
    public static Ticket CurrentTicket
    {
        get
        {
            // first check to see param is good and valid.
            string Ticket_uid = CommonUtility.Util.IsValidGuid(HttpContext.Current.Request.QueryString["TicketUID"]) ? HttpContext.Current.Request.QueryString["TicketUID"].ToUpper() : "";

            // if we have a good param, then we have a chance at retrieving the Ticketapp
            if (!string.IsNullOrEmpty(Ticket_uid))
            {
                // always make sure our dictionary is initialized
                if (diCurrentTicket == null)
                {
                    diCurrentTicket = new Dictionary<string, Ticket>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentTicket.ContainsKey(Ticket_uid))
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@UID", Ticket_uid);
                    diCurrentTicket[Ticket_uid] = DataTicket.GetInstance().GetTicket(prms, CurrentUser.TimeZone);
                }

                return diCurrentTicket[Ticket_uid];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentTicket == null)
            {
                diCurrentTicket = new Dictionary<string, Ticket>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.IsValidGuid(value.TicketUID))
            {
                diCurrentTicket[value.TicketUID.ToUpper()] = value;
            }
        }
    }
    
    #endregion
    //PXP-7674 abarua

    #region CurrentCRM

    public static Dictionary<string, Compliance> diCurrentCRM
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentCRM"] == null)
                return null;
            else
                return (Dictionary<string, Compliance>)HttpContext.Current.Session["diCurrentCRM"];
        }
        set { HttpContext.Current.Session["diCurrentCRM"] = value; }

    }
    /// <summary>
    /// Requires CRMUID in querystring
    /// </summary>
    public static Compliance CurrentCRM
    {
        get
        {
            // first check to see param is good and valid.
            string CRM_uid = CommonUtility.Util.IsValidGuid(HttpContext.Current.Request.QueryString["CRMUID"]) ? HttpContext.Current.Request.QueryString["CRMUID"].ToUpper() : "";

            // if we have a good param, then we have a chance at retrieving the Ticketapp
            if (!string.IsNullOrEmpty(CRM_uid))
            {
                // always make sure our dictionary is initialized
                if (diCurrentCRM == null)
                {
                    diCurrentCRM = new Dictionary<string, Compliance>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentCRM.ContainsKey(CRM_uid))
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@CRMUID", CRM_uid);
                    diCurrentCRM[CRM_uid] = DataCompliance.GetInstance().GetCRM(prms, CurrentUser.TimeZone);
                }

                return diCurrentCRM[CRM_uid];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentCRM == null)
            {
                diCurrentCRM = new Dictionary<string, Compliance>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.IsValidGuid(value.CRMUID))
            {
                diCurrentCRM[value.CRMUID.ToUpper()] = value;
            }
        }
    }

    //public static Dictionary<string, DateTime> diTUID = null;

    #endregion

    #region CurrentEmailBlaster

    public static Dictionary<string, EmailBlaster> diCurrentEmailBlaster
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentEmailBlaster"] == null)
                return null;
            else
                return (Dictionary<string, EmailBlaster>)HttpContext.Current.Session["diCurrentEmailBlaster"];
        }
        set { HttpContext.Current.Session["diCurrentEmailBlaster"] = value; }

    }

    /// <summary>
    /// Requires EmailBlasterUID in querystring
    /// </summary>
    public static EmailBlaster CurrentEmailBlaster
    {
        get
        {
            // first check to see param is good and valid.
            string EmailBlaster_uid = CommonUtility.Util.IsValidGuid(HttpContext.Current.Request.QueryString["EmailBlasterUID"]) ? HttpContext.Current.Request.QueryString["EmailBlasterUID"].ToUpper() : "";

            // if we have a good param, then we have a chance at retrieving the EmailBlasterapp
            if (!string.IsNullOrEmpty(EmailBlaster_uid))
            {
                // always make sure our dictionary is initialized
                if (diCurrentEmailBlaster == null)
                {
                    diCurrentEmailBlaster = new Dictionary<string, EmailBlaster>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentEmailBlaster.ContainsKey(EmailBlaster_uid))
                {
                    diCurrentEmailBlaster[EmailBlaster_uid] = DataCommunication.GetInstance().GetEmailBlaster(EmailBlaster_uid);
                }

                return diCurrentEmailBlaster[EmailBlaster_uid];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentEmailBlaster == null)
            {
                diCurrentEmailBlaster = new Dictionary<string, EmailBlaster>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.IsValidGuid(value.EmailBlasterID))
            {
                diCurrentEmailBlaster[value.EmailBlasterID.ToUpper()] = value;
            }
        }
    }

    #endregion

    #region CurrentWSCompliance

    public static Dictionary<int, WSCompliance> diCurrentWSCompliance
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentWSCompliance"] == null)
                return null;
            else
                return (Dictionary<int, WSCompliance>)HttpContext.Current.Session["diCurrentWSCompliance"];
        }
        set { HttpContext.Current.Session["diCurrentWSCompliance"] = value; }

    }

    /// <summary>
    /// requires WSComplianceID in query string
    /// </summary>
    public static WSCompliance CurrentWSCompliance
    {
        get
        {
            // first check to see param is good and valid.
            int WSCompliance_ID = CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["WSComplianceID"], 0);

            // if we have a good param, then we have a chance at retrieving the WSComplianceapp
            if (WSCompliance_ID > 0)
            {
                // always make sure our dictionary is initialized
                if (diCurrentWSCompliance == null)
                {
                    diCurrentWSCompliance = new Dictionary<int, WSCompliance>();
                }

                // entry does not exists, so we fetch a fresh copy based off the WSComplianceID
                if (!diCurrentWSCompliance.ContainsKey(WSCompliance_ID))
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@WSComplianceID", WSCompliance_ID);
                    diCurrentWSCompliance[WSCompliance_ID] = DataWSCompliance.SelectWSCompliance(prms, UserSessions.CurrentUser.TimeZone);
                }

                return diCurrentWSCompliance[WSCompliance_ID];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentWSCompliance == null)
            {
                diCurrentWSCompliance = new Dictionary<int, WSCompliance>();
            }

            if (CommonUtility.Util.if_i(value.ID, 0) > 0)
            {
                diCurrentWSCompliance[value.ID] = value;
            }
        }
    }

    #endregion

    #region CurrentAchMerchant

    public static Dictionary<int, AchMerchant> diCurrentAchMerchant
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentAchMerchant"] == null)
                return null;
            else
                return (Dictionary<int, AchMerchant>)HttpContext.Current.Session["diCurrentAchMerchant"];
        }
        set { HttpContext.Current.Session["diCurrentAchMerchant"] = value; }

    }

    /// <summary>
    /// Requires AchID in querystring
    /// </summary>
    public static AchMerchant CurrentAchMerchant
    {
        get
        {

            // first check to see param is good and valid.
            int AchMerchant_ID = CurrentMerchantApp != null ? CommonUtility.Util.if_i(CurrentMerchantApp.AchID, 0) : CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["AchID"], 0);

            // if we have a good param, then we have a chance at retrieving the AchMerchantapp
            if (AchMerchant_ID > 0)
            {
                // always make sure our dictionary is initialized
                if (diCurrentAchMerchant == null)
                {
                    diCurrentAchMerchant = new Dictionary<int, AchMerchant>();
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentAchMerchant.ContainsKey(AchMerchant_ID))
                {
                    diCurrentAchMerchant[AchMerchant_ID] = DataAccess.DataAchMerchantDao.GetAchMerchant(AchMerchant_ID);
                }

                return diCurrentAchMerchant[AchMerchant_ID];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentAchMerchant == null)
            {
                diCurrentAchMerchant = new Dictionary<int, AchMerchant>();
            }

            if (CommonUtility.Util.if_i(value.AchID, 0) > 0)
            {
                diCurrentAchMerchant[value.AchID] = value;
            }
        }
    }

    #endregion

    #region CurrentCommunication

    public static Dictionary<string, Communication> diCurrentCommunication
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentCommunication"] == null)
                return null;
            else
                return (Dictionary<string, Communication>)HttpContext.Current.Session["diCurrentCommunication"];
        }
        set { HttpContext.Current.Session["diCurrentCommunication"] = value; }

    }

    /// <summary>
    /// Requires CommunicationUID in querystring
    /// </summary>
    public static Communication CurrentCommunication
    {
        get
        {
            // first check to see param is good and valid.
            string Communication_UID = CommonUtility.Util.if_s(HttpContext.Current.Request.QueryString["CommunicationUID"], null);

            // if we have a good param, then we have a chance at retrieving the Communicationapp
            if (CommonUtility.Util.IsValidGuid(Communication_UID))
            {
                // always make sure our dictionary is initialized
                if (diCurrentCommunication == null)
                {
                    diCurrentCommunication = new Dictionary<string, Communication>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentCommunication.ContainsKey(Communication_UID))
                {
                    diCurrentCommunication[Communication_UID] = DataAccess.DataCommunicationDao.GetCommunication(Communication_UID);
                }

                return diCurrentCommunication[Communication_UID];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentCommunication == null)
            {
                diCurrentCommunication = new Dictionary<string, Communication>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (CommonUtility.Util.IsValidGuid(value.CommunicationID))
            {
                diCurrentCommunication[value.CommunicationID] = value;
            }
        }
    }

    #endregion

    #region CurrentUserObject

    public static Dictionary<string, User> diCurrentUserObject
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentUserObject"] == null)
                return null;
            else
                return (Dictionary<string, User>)HttpContext.Current.Session["diCurrentUserObject"];
        }
        set { HttpContext.Current.Session["diCurrentUserObject"] = value; }

    }

    /// <summary>
    /// Requires UserObjectUID in querystring
    /// </summary>
    public static User CurrentUserObject
    {
        get
        {
            // first check to see param is good and valid.
            string UserObject_UID = CommonUtility.Util.if_s(HttpContext.Current.Request.QueryString["UserObjectUID"], null);

            // if we have a good param, then we have a chance at retrieving the UserObjectapp
            if (CommonUtility.Util.IsValidGuid(UserObject_UID))
            {
                // always make sure our dictionary is initialized
                if (diCurrentUserObject == null)
                {
                    diCurrentUserObject = new Dictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diCurrentUserObject.ContainsKey(UserObject_UID))
                {
                    diCurrentUserObject[UserObject_UID] = DataUser.GetInstance().GetUser(UserObject_UID);
                }

                return diCurrentUserObject[UserObject_UID];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentUserObject == null)
            {
                diCurrentUserObject = new Dictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (CommonUtility.Util.IsValidGuid(value.UserID))
            {
                diCurrentUserObject[value.UserID] = value;
            }
        }
    }

    #endregion

    /// <summary>
    /// Used to store the login session of the user.
    /// </summary>
    public static User CurrentUser
    {
        get
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentUser"] != null)
                return (User)HttpContext.Current.Session["CurrentUser"];
            else
                return null;
        }
        set { HttpContext.Current.Session["CurrentUser"] = value; }
    }

    /// <summary>
    /// when agents log in, in addition to the user object, the agent object also gets filled.
    /// </summary>
    public static Agent CurrentLoggedInAgent
    {
        get
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentLoggedInAgent"] != null)
                return (Agent)HttpContext.Current.Session["CurrentLoggedInAgent"];
            else
                return null;
        }
        set { HttpContext.Current.Session["CurrentLoggedInAgent"] = value; }
    }

    public static IList<GenericListItem> CurrentAgentLevels
    {
        get
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentAgentLevels"] != null)
                return (IList<GenericListItem>)HttpContext.Current.Session["CurrentAgentLevels"];
            else
                return null;
        }
        set { HttpContext.Current.Session["CurrentAgentLevels"] = value; }
    }

    public static IDictionary<string, MerchantNoteCodes> MerchantNoteCodes
    {
        get
        {
            if (HttpContext.Current.Session["MerchantNoteCodes"] == null)
                return null;
            else
                return (IDictionary<string, MerchantNoteCodes>)HttpContext.Current.Session["MerchantNoteCodes"];
        }
        set { HttpContext.Current.Session["MerchantNoteCodes"] = value; }
    }

    public static IList<GenericListItem> AgentList
    {
        get
        {
            if (HttpContext.Current.Session["AgentList"] == null)
                return null;
            else
                return (IList<GenericListItem>)HttpContext.Current.Session["AgentList"];
        }
        set { HttpContext.Current.Session["AgentList"] = value; }
    }

    public static Dictionary<string, PrivateLabel> diCurrentPrivateLabel
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentPrivateLabel"] == null)
                return null;
            else
                return (Dictionary<string, PrivateLabel>)HttpContext.Current.Session["diCurrentPrivateLabel"];
        }
        set { HttpContext.Current.Session["diCurrentPrivateLabel"] = value; }
    }
        
    public static AchMerchant ActiveAchMerchant
    {
        get
        {
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs instead of Pages
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["ActiveAchMerchant"] != null)
                return (AchMerchant)HttpContext.Current.Session["ActiveAchMerchant"];
            else
                return CurrentMerchantApp != null ? DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(CurrentMerchantApp.ID)) : null;                        
        }
       set { HttpContext.Current.Session["ActiveAchMerchant"] = value; }
    }

    public static List<MerchantScoreCardItem> CurrentScoreCard
    {
        get
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentScoreCard"] != null)
                return (List<MerchantScoreCardItem>)HttpContext.Current.Session["CurrentScoreCard"];
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CurrentScoreCard"] = value;
        }
    }

    public static TicketTemplate CurrentTicketTemplate
    {
        get
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["CurrentTicketTemplate"] != null)
                return (TicketTemplate)HttpContext.Current.Session["CurrentTicketTemplate"];
            else
                return null;
        }
        set
        {
            HttpContext.Current.Session["CurrentTicketTemplate"] = value;
        }
    }


    #region CurrentAgentAllocation

    public static Dictionary<string, AgentAllocation> diCurrentAgentAllocation
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentAgentAllocation"] == null)
                return null;
            else
                return (Dictionary<string, AgentAllocation>)HttpContext.Current.Session["diCurrentAgentAllocation"];
        }
        set { HttpContext.Current.Session["diCurrentAgentAllocation"] = value; }

    }

    /// <summary>
    /// Requires AgentKeyID in querystring
    /// </summary>
    public static AgentAllocation CurrentAgentAllocation
    {
        get
        {
            // first check to see param is good and valid.
            int AgentAlloc_AgentKeyID = CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["AgentKeyID"], 0);
            int AgentAlloc_AgentID = CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["AgentID"], 0);
            string AgentAlloc_SourceName = CommonUtility.Util.if_s(HttpContext.Current.Request.QueryString["SourceName"]);


            // if we have a good param, then we have a chance at retrieving the Ticketapp
            if (AgentAlloc_AgentKeyID>0 && AgentAlloc_AgentID> 0 &&!string.IsNullOrEmpty(AgentAlloc_SourceName))
            {
                // always make sure our dictionary is initialized
                if (diCurrentAgentAllocation == null)
                {
                    diCurrentAgentAllocation = new Dictionary<string, AgentAllocation>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                    Hashtable prms = new Hashtable();
                    //prms.Add("@AgentKeyID", AgentAlloc_AgentKeyID);
                    prms.Add("@AgentID", AgentAlloc_AgentID);
                    prms.Add("@SourceName", AgentAlloc_SourceName);
             AgentAllocation aa =  DataAgent.GetInstance().GetSelectedAgentAllocation(prms);

             string uniqueKeyData = AgentAlloc_AgentID.ToString() + "&" + AgentAlloc_SourceName;
             diCurrentAgentAllocation[uniqueKeyData] = aa;

             return diCurrentAgentAllocation[uniqueKeyData];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentAgentAllocation == null)
            {
                diCurrentAgentAllocation = new Dictionary<string, AgentAllocation>(StringComparer.InvariantCultureIgnoreCase);
            }

            string uniqueKeyData = value.AgentID.ToString() +"&"+value.SourceName;
            if (value != null && !string.IsNullOrEmpty(uniqueKeyData))
            {
                diCurrentAgentAllocation[uniqueKeyData] = value;
            }
        }
    }

    #endregion

    #region QA Ticket Errors
    public static Dictionary<string, QATicketErrors> diQATicketErrors
    {
        get
        {
            if (HttpContext.Current.Session["diQATicketErrors"] == null)
                return null;
            else
                return (Dictionary<string, QATicketErrors>)HttpContext.Current.Session["diQATicketErrors"];
        }
        set { HttpContext.Current.Session["diQATicketErrors"] = value; }

    }

    public static QATicketErrors CurrentQATicketErrors
    {
        get
        {
            // first check to see param is good and valid.
            int QATicketErrorID = CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["QATicketErrorID"], 0);

            // if we have a good param, then we have a chance at retrieving the merchantapp
            if (QATicketErrorID > 0)
            {
                // always make sure our dictionary is initialized
                if (diQATicketErrors == null)
                {
                    diQATicketErrors = new Dictionary<string, QATicketErrors>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                if (!diQATicketErrors.ContainsKey(QATicketErrorID.ToString()))
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@QATicketErrorID", QATicketErrorID);
                    QATicketErrors objQATicketErrors = DataTicket.GetInstance().GetQATicketErrorDetails(prms);

                    if (objQATicketErrors != null)
                    {
                        diQATicketErrors[QATicketErrorID.ToString()] = objQATicketErrors;
                    }

                }

                return diQATicketErrors[QATicketErrorID.ToString()];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diQATicketErrors == null)
            {
                diQATicketErrors = new Dictionary<string, QATicketErrors>(StringComparer.InvariantCultureIgnoreCase);
            }

            if (value != null && CommonUtility.Util.if_i(value.QATicketErrorID, 0) > 0)
            {
                diQATicketErrors[value.QATicketErrorID.ToString().ToUpper()] = value;
            }
        }
    }
    #endregion

    #region CurrentQAAppError
    //for PXP-17782 Start
    public static Dictionary<string, QAAppError> diCurrentQAAppError
    {
        get
        {
            if (HttpContext.Current.Session["diCurrentQAAppError"] == null)
                return null;
            else
                return (Dictionary<string, QAAppError>)HttpContext.Current.Session["diCurrentQAAppError"];
        }
        set { HttpContext.Current.Session["diCurrentQAAppError"] = value; }

    }
    public static QAAppError CurrentQAAppError
    {
        get
        {
            // first check to see param is good and valid.
            int QAAppError_QAAppErrorKeyID = CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["QAAppErrorKeyID"], 0);
            int QAAppError_QAAppErrorID = CommonUtility.Util.if_i(HttpContext.Current.Request.QueryString["QAAppErrorID"], 0);
            string QAAppError_MID = CommonUtility.Util.if_s(HttpContext.Current.Request.QueryString["MID"]);


            // if we have a good param, then we have a chance at retrieving the Ticketapp
            if (QAAppError_QAAppErrorKeyID > 0 && QAAppError_QAAppErrorID > 0 && !string.IsNullOrEmpty(QAAppError_MID))
            {
                // always make sure our dictionary is initialized
                if (diCurrentQAAppError == null)
                {
                    diCurrentQAAppError = new Dictionary<string, QAAppError>(StringComparer.InvariantCultureIgnoreCase);
                }

                // entry does not exists, so we fetch a fresh copy.
                Hashtable prms = new Hashtable();

                prms.Add("@QAAppErrorID", QAAppError_QAAppErrorID);
                prms.Add("@MID", QAAppError_MID);
                QAAppError ae = DataMerchantApp.GetInstance().GetSelectedQAAppError(prms);

                string uniqueKeyData = QAAppError_QAAppErrorID.ToString() + "&" + QAAppError_MID;
                diCurrentQAAppError[uniqueKeyData] = ae;

                return diCurrentQAAppError[uniqueKeyData];
            }
            else
            {
                return null;
            }
        }
        set
        {
            // always deal with an initialized dictionary
            if (diCurrentQAAppError == null)
            {
                diCurrentQAAppError = new Dictionary<string, QAAppError>(StringComparer.InvariantCultureIgnoreCase);
            }

            string uniqueKeyData = value.QAAppErrorID.ToString() + "&" + value.MID;
            if (value != null && !string.IsNullOrEmpty(uniqueKeyData))
            {
                diCurrentQAAppError[uniqueKeyData] = value;
            }
        }
    }
    #endregion

}
