using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.NavigationControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using Okta.Sdk.Model;

public partial class frmAgent : frmBaseDataEntry
{

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        wucContact1.ControlCssClass = "agentcontacts";

        PaymentXP.BusinessObjects.Agent ValidationAgent = UserSessions.CurrentAgent;

        if (!this.EditMode)
        {

            if (UserSessions.CurrentAgent.BillingCountry != null)
            {
                if (UserSessions.CurrentAgent.BillingCountry.Trim().ToUpper().Equals("US") || UserSessions.CurrentAgent.BillingCountry.Trim().ToUpper().Equals(""))
                {
                    this.BillingProvince.Visible = false;
                    this.BillingState.Visible = true;
                }

                else
                {
                    this.BillingProvince.Visible = true;
                    this.BillingState.Visible = false;
                }
            }

            if (UserSessions.CurrentAgent.Country != null)
            {
                if (UserSessions.CurrentAgent.Country.Trim().ToUpper().Equals("US") || UserSessions.CurrentAgent.Country.Trim().ToUpper().Equals(""))
                {
                    this.Province.Visible = false;
                    this.State.Visible = true;
                }

                else
                {
                    this.Province.Visible = true;
                    this.State.Visible = false;
                }
            }
        }

        if (!this.IsPostBack)
        {
            //Set the current page
            ((HyperLink)this.Master.FindControl("lnkAgentProfile")).CssClass = "active";


            LookupTableHandler.LoadAgentTypes(AgentTypeUID, false);
            LookupTableHandler.LoadAgentStatus(StatusUID, false);
            LookupTableHandler.LoadUsersByRole(PrimaryContactUID, true, Constants.ROLE_AGENT_RELATIONS);
            LookupTableHandler.LoadBusinessStructures(BusinessStructureUID, false);
            LookupTableHandler.LoadPartnerChannels(AgentGroupID, false);
            LookupTableHandler.LoadOffices(OfficeID, false);
            LookupTableHandler.LoadLegalEntities(LegalEntityID);
            LookupTableHandler.LoadChannelSalesManager(TerritoryManager);//Code added for PXP-12596
            //PXP-15731:Code Changes:Start
            LookupTableHandler.LoadRequestedByUsers(RequestedBy, true, Constants.ROLE_AGENT_RELATIONS);
            if (UserSessions.CurrentAgent.IsOtherUser == true)
            {
                RequestedBy.SelectedValue = "0";
                OtherUser.Text = UserSessions.CurrentAgent.RequestedByUser;
                OtherUser.Attributes.Add("style", "display:block;");
            }
            else
            {
                RequestedBy.SelectedValue = UserSessions.CurrentAgent.RequestedByUser;
                OtherUser.Attributes.Add("style", "display:none;");
            }
            //PXP-15731:Code Changes:End
            LookupTableHandler.LoadSchedueATypeListItems(ddlScheduleATypes, false);
            if (!this.Adding)
                this.ddlScheduleATypes.SelectedValue = UserSessions.CurrentAgent.ScheduleAFeeTypeID.ToString();

            this.LoadAgents2();

            this.Country.DataSource = LookupTableHandler.LoadCountries();
            this.Country.DataTextField = "Value";
            this.Country.DataValueField = "Key";
            this.Country.DataBind();
            this.BillingCountry.DataSource = LookupTableHandler.LoadCountries();
            this.BillingCountry.DataTextField = "Value";
            this.BillingCountry.DataValueField = "Key";
            this.BillingCountry.DataBind();
            //PXP-15731:Code Changes:Start
            this.PricingProgram.DataSource = LookupTableHandler.LoadPricingProgram();
            this.PricingProgram.DataTextField = "Value";
            this.PricingProgram.DataValueField = "Key";
            this.PricingProgram.DataBind();
            this.PricingProgram.SelectedValue = UserSessions.CurrentAgent.PricingProgramID.ToString();

            this.OnboardingNote.Text = Server.HtmlDecode(UserSessions.CurrentAgent.OnboardingNote);
            //PXP-15731:Code Changes:End

            if (UserSessions.CurrentAgent != null)
                this.UID = UserSessions.CurrentAgent.AgentUID.ToUpper();
            else
                this.UID = UserSessions.CurrentUser.AgentUID.ToUpper();

            DataTreeNode node = SelectNode(this.UID);

            if (node != null)
            {
                node.Selected = true;
                uwtMain.ActiveNode = node;
                this.FormShow(uwtMain.ActiveNode.Key);
            }
            else
            {
                if (UserSessions.CurrentUser != null)
                {
                    FormShow(UserSessions.CurrentUser.AgentUID.ToUpper());

                    wucContact1.ObjectID = UserSessions.CurrentAgent.AgentID;
                }
            }
        }
        this.PrimaryContactUID.Enabled = false;
    }

    private void LoadTopMerchants()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);

        DataAgent data = DataAccess.DataAgentDao;
        DataSet ds = data.GetAgentTop5MerchantVol(prms);

        grdMTDTopMerchants.DataSource = ds;
        grdMTDTopMerchants.DataBind();

        grdMTDTopMerchants.Visible = (grdMTDTopMerchants.Rows.Count > 0);
        lblNoTopMerchants.Visible = !(grdMTDTopMerchants.Rows.Count > 0);
    }

    private void LoadDealCount()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);

        DataAgent data = DataAccess.DataAgentDao;
        SqlDataReader dr = data.GetAgentDealCount(prms);

        if (dr.Read())
        {
            WTDReceived.Text = dr["WTDReceived"].ToString();
            MTDReceived.Text = dr["MTDReceived"].ToString();
            YTDReceived.Text = dr["YTDReceived"].ToString();

            WTDApproved.Text = dr["WTDApproved"].ToString();
            MTDApproved.Text = dr["MTDApproved"].ToString();
            YTDApproved.Text = dr["YTDApproved"].ToString();

            WTDCancelled.Text = dr["WTDCancelled"].ToString();
            MTDCancelled.Text = dr["MTDCancelled"].ToString();
            YTDCancelled.Text = dr["YTDCancelled"].ToString();

            lblActive.Text = dr["Active"].ToString();
        }
        dr.Close();
    }

    private void LoadAgents2()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@agentid", UserSessions.CurrentAgent.AgentID);
        IList<PaymentXP.BusinessObjects.Agent> agents = DataAccess.DataAgentDao.GetAgentTree(prms);


        DataTreeNode node = null;
        DataTreeNode ParentNode = null;

        uwtMain.AllNodes.Clear();

        foreach (PaymentXP.BusinessObjects.Agent agent in agents)
        {
            ParentNode = SelectNode(agent.ParentUID);

            if (ParentNode == null)
            {
                ParentNode = new DataTreeNode();
                ParentNode.Text = string.Format("{0} - {1}", agent.AgentID.ToString(), agent.AgentDBA);
                ParentNode.Key = agent.AgentUID.ToUpper();

                uwtMain.Nodes.Add(ParentNode);
            }
            else
            {
                node = new DataTreeNode();
                node.Text = string.Format("{0} - {1}", agent.AgentID.ToString(), agent.AgentDBA);
                node.Key = agent.AgentUID.ToUpper();
                ParentNode.Nodes.Add(node);
            }
        }
    }

    private void LoadAgents2Children(DataTreeNode ParentNode)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@agentuid", ParentNode.Key);
        IList<PaymentXP.BusinessObjects.Agent> agents = DataAccess.DataAgentDao.GetAgentTreeChildren(prms);


        DataTreeNode node = null;

        ParentNode.IsEmptyParent = false;
        foreach (PaymentXP.BusinessObjects.Agent agent in agents)
        {
            node = new DataTreeNode();
            node.Text = string.Format("{0} - {1}", agent.AgentID.ToString(), agent.AgentDBA);
            node.Key = agent.AgentUID.ToUpper();
            ParentNode.Nodes.Add(node);
        }
    }

    public override void FormShow(string ID)
    {
        DataAgent data = DataAccess.DataAgentDao;
        PaymentXP.BusinessObjects.Agent agent = data.GetAgent_List(ID);

        UserSessions.CurrentAgent = agent;

        FormBinding.BindObjectToControls(agent, pnlDetail);
        this.OnboardingNote.Text = Server.HtmlDecode(agent.OnboardingNote);

        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        this.UID = agent.AgentUID.ToUpper();
        btnLogin.Visible = !(UserSessions.CurrentUser.IsAgent);

        ListHandler.ListFindItem(OfficeID, Convert.ToString((int)agent.Office));

        DataTreeNode node = SelectNode(this.UID);

        if (node != null)
        {
            node.Selected = true;
            uwtMain.ActiveNode = node;
            node.Text = string.Format("{0} - {1}", agent.AgentID.ToString(), agent.AgentDBA);
        }

        this.LoadFeesEquipments();
        this.LoadTopMerchants();
        this.LoadDealCount();

        wucContact1.EditMode = this.EditMode;
        wucContact1.ObjectID = agent.AgentID;
        wucContact1.FormShow("", false);

        // you can only edit when in edit mode, and when saleforce id is 0
        SalesForceID.ReadOnly = (UserSessions.CurrentAgent.SalesForceID == 0 && this.EditMode) ? false : true;

        if (agent.AgentFMAID == 0)
            AgentFMAID.Text = "";

        this.Brand.Enabled = this.Adding;

        AgentEdgeId.Text = agent.AgentEdgeID;

        if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas))
        {
            tdAgentEdgeLabel.Visible = true;
            tdAgentEdgeText.Visible = true;
        }
        else
        {
            tdAgentEdgeLabel.Visible = false;
            tdAgentEdgeText.Visible = false;
        }
        if (this.Adding)
        {
            this.ddlScheduleATypes.Enabled = true;
            this.CompareValidator3.Enabled = true;
        }
        else
        {
            this.ddlScheduleATypes.Enabled = false;
            this.PrimaryContactUID.Enabled = false;
            this.CompareValidator3.Enabled = false;
        }
    }

    private DataTreeNode SelectNode(string key)
    {
        DataTreeNode node = null;
        foreach (object obj in uwtMain.AllNodes)
        {
            DataTreeNode n = (DataTreeNode)obj;

            if (n.Key.ToUpper() == key.ToUpper())
            {
                uwtMain.ClearSelection();
                n.Selected = true;
                uwtMain.ActiveNode = n;
                n.ExpandAnscestors();
                node = n;
                break;
            }
        }

        return node;
    }

    private void LoadFeesEquipments()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@AgentUID", this.UID);
        prms.Add("@FeeReportItemTypeList", ConfigurationManager.AppSettings["Agent Module Fees"].ToString());
    }

    public override void FormClear()
    {
        string ParentBrand = this.Brand.SelectedValue;
        FormHandler.ClearAllControls(pnlDetail);
        this.AgentID.Text = string.Empty;
        this.Brand.SelectedValue = ParentBrand;
        this.AgentEdgeId.Text = string.Empty;
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck() || !wucContact1.FormDataCheck())
        {
            return false;
        }

        try
        {
            DataTreeNode node = null;
            PaymentXP.BusinessObjects.Agent agent = null;

            if (this.Adding)
                agent = new PaymentXP.BusinessObjects.Agent();
            else
            {
                agent = (PaymentXP.BusinessObjects.Agent)UserSessions.CurrentAgent;
                agent.CloneAgent();
            }

            bool OrigETF = agent.ETFWaived;

            var DBABefore = agent.AgentDBA;
            var EmailBefore = agent.Email;

            FormBinding.BindControlsToObject(agent, pnlDetail);
            DataAgent data = DataAccess.DataAgentDao;
            PaymentXP.BusinessObjects.User user = UserSessions.CurrentUser;
            agent.UserUpdated = user.UserName;
            agent.AgentFMAID = CommonUtility.Util.if_l(AgentFMAID.Text, 0);

            var DBAAfter = agent.AgentDBA;
            var EmailAfter = agent.Email;

            int officeid;
            int.TryParse(OfficeID.SelectedValue, out officeid);

            agent.Office = (CommonUtility.Util.Offices)officeid;
            agent.AgentEdgeID = CommonUtility.Util.if_s(AgentEdgeId.Text, string.Empty);
            //Amit Patne : PXP-4071
            agent.AgentDBA = System.Text.RegularExpressions.Regex.Replace(agent.AgentDBA, @"\s+", " ").Trim();

            if (!this.Adding)
            {
                if (!agent.BillingCountry.Trim().ToUpper().Equals("US"))
                {
                    agent.BillingState = this.BillingProvince.Text;
                }

                if (!agent.Country.Trim().ToUpper().Equals("US"))
                {
                    agent.State = this.Province.Text;
                }

                //PXP-15731:Code Changes:Start
                if (RequestedBy.SelectedValue == "0")
                {
                    agent.IsOtherUser = true;
                    agent.RequestedByUser = OtherUser.Text.Trim();
                }
                else
                {
                    agent.IsOtherUser = false;
                    agent.RequestedByUser = RequestedBy.SelectedValue.Trim();
                }

                int PPID;
                if (int.TryParse(PricingProgram.SelectedValue, out PPID))
                {
                    agent.PricingProgramID = PPID;
                }
                if (OnboardingNote.Text.Length > 0)
                {
                    agent.OnboardingNote = Server.HtmlEncode(OnboardingNote.Text.Trim());
                }
                //PXP-15731:Code Changes:End

                int rows = data.UpdateAgent(agent);

                if (rows > 0)
                {
                    uwtMain.ActiveNode.Text = AgentDBA.Text;

                    if (DBABefore != DBAAfter || EmailBefore != EmailAfter)
                    {
                        if (agent.IsMFAEnabled && Constants.IS_MFA_ENABLED && !string.IsNullOrEmpty(agent.OktaUserID))
                        {
                            UpdateOktaUser(new PaymentXP.BusinessObjects.User()
                            {
                                OktaUserID = agent.OktaUserID,
                                FirstName = agent.AgentDBA,
                                LastName = string.Empty,
                                UserName = agent.AgentID.ToString(),
                                Email = EmailAfter
                            }, EmailBefore != EmailAfter);
                        }
                    }

                    try
                    {
                        string differences = agent.GetChangedValues();

                        if (!string.IsNullOrEmpty(differences))
                        {
                            DataUser.GetInstance().InsertChangeLog(agent.AgentDBA, UserSessions.CurrentUser.UserName, UserSessions.CurrentAgent.AgentUID, UserSessions.CurrentAgent.AgentID, "Partner", differences, Constants.PORTAL_ZEUS);
                        }
                    }
                    catch { }

                    //remove auto generate note on update requested by wilson nguy 12/11/2012
                    //WucCreateUser1.AddNotes("Agent profile updated by " + UserSessions.CurrentUser.UserName, "6f065641-bc3b-47b2-aa93-c4497b91f954");

                    wucContact1.ObjectID = agent.AgentID;
                    wucContact1.FormSave();
                }
                else
                {
                    WucMessage1.AddMessageError("Error saving record.");
                    return false;
                }
            }
            else
            {
                agent.UserCreated = user.UserName;
                node = uwtMain.ActiveNode;
                agent.ParentUID = node.Key;
                if (!agent.BillingCountry.Trim().ToUpper().Equals("US"))
                {
                    agent.BillingState = this.BillingProvince.Text;
                }

                if (!agent.Country.Trim().ToUpper().Equals("US"))
                {
                    agent.State = this.Province.Text;
                }

                agent.ReportSuppressDetails = false;
                if (this.ddlScheduleATypes.SelectedValue != null || this.ddlScheduleATypes.SelectedValue.ToString() != "-1")
                    agent.ScheduleAFeeTypeID = int.Parse(this.ddlScheduleATypes.SelectedValue);

                data.InsertAgent(agent);

                if (agent.AgentUID != "-1")
                {
                    agent = data.GetAgent(agent.AgentUID);

                    uwtMain.ActiveNode.Nodes.Add(agent.AgentDBA, agent.AgentUID);//marknguyen20120124

                    this.UID = agent.AgentUID.ToUpper();
                    this.Adding = false;

                    wucContact1.ObjectID = agent.AgentID;
                    wucContact1.FormSave();
                }

            }

            if (agent.SalesForceID > 0 && agent.AgentID > 0)
            {
                try
                {
                    DataSalesForce.InsertPartnerSalesForceID(agent.AgentID, CommonUtility.Util.if_i(agent.SalesForceID, 0), UserSessions.CurrentUser.UserName);
                }
                catch
                {
                    WucMessage1.AddMessageError("Could not insert SalesForceID.");
                }
            }

            node = SelectNode(this.UID);

            UserSessions.CurrentAgent = agent;
            this.EditMode = false;
            this.ToggleButtons();

            if (OrigETF != agent.ETFWaived && agent.ETFWaived == true)
            {
                data.UpdateMerchantETFAssessed(agent.AgentUID);
            }

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnAdd.Enabled = !btnAdd.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        uwtMain.Enabled = !uwtMain.Enabled;
        wucContact1.EditMode = this.EditMode;
        wucContact1.ToggleButtons();
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;

        if (AgentDBA.Text == string.Empty)
            WucMessage1.AddMessageError("DBA is required. ");

        if (Email.Text == string.Empty)
            WucMessage1.AddMessageError("Email is required. ");

        if (AgentTypeUID.SelectedItem.Value == "-1")
            WucMessage1.AddMessageError("Type is required. ");

        if (StatusUID.SelectedItem.Value == "-1")
            WucMessage1.AddMessageError("Status is required. ");

        if (EnableOnlineAppReview.Checked && !EnableOnlineApp.Checked)
            WucMessage1.AddMessageError("Please enable onlineApp to enable the review checbox.");

        //Validate FMA

        long fmaId = 0;
        int partnerid;
        int.TryParse(AgentID.Text, out partnerid);

        if (!string.IsNullOrWhiteSpace(AgentFMAID.Text))
        {
            if (!long.TryParse(AgentFMAID.Text, out fmaId))
            {
                WucMessage1.AddMessageError("Please enter a valid Agent FMA number.");
            }
            else if (DataAccess.DataAgentDao.FMAIDExists(partnerid, fmaId))
            {
                //check if fma id already exists for a different ZID
                WucMessage1.AddMessageError("Agent FMA number already exists.");
            }
        }

        if (WucMessage1.ErrorCount() == 0)
            return true;
        else
        {
            return false;
        }

    }

    public override void FormNew()
    {
        this.FormClear();
        DataTreeNode node = uwtMain.ActiveNode;
        ParentAgentFullName.Text = node.Text;
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        this.ToggleButtons();
        if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.Dallas))
        {
            tdAgentEdgeLabel.Visible = true;
            tdAgentEdgeText.Visible = true;
        }
        else
        {
            tdAgentEdgeLabel.Visible = false;
            tdAgentEdgeText.Visible = false;
        }


        wucContact1.Adding = true;
        wucContact1.EditMode = true;
        wucContact1.FormNew();

        this.SalesForceID.ReadOnly = false;
        this.PrimaryContactUID.Enabled = false;
        ListHandler.GetListItem(AgentGroupID, Convert.ToString((int)SalesPartnerGroupID.UnAssigned)); // default item to be selected is unAssigned for channel dropdown
        this.CompareValidator3.Enabled = true;
    }

    public override bool FormDelete()
    {
        if (this.UID.Equals(string.Empty))
            return false;

        DataAgent data = DataAccess.DataAgentDao;
        int rows = data.DeleteAgent(this.UID);
        if (rows > 0)
        {
            return true;
        }
        else
            return false;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();

        wucContact1.FormShow("", false);
    }

    private IList<PaymentXP.BusinessObjects.Agent> GetAgents()
    {
        DataAgent data = DataAccess.DataAgentDao;
        IList<PaymentXP.BusinessObjects.Agent> agents = new List<PaymentXP.BusinessObjects.Agent>();

        if (UserSessions.CurrentUser.IsAgent)
        {
            PaymentXP.BusinessObjects.Agent agent = data.GetAgent_List(UserSessions.CurrentUser.AgentUID);
            agents.Add(agent);
        }
        else
        {
            agents = data.GetParentAgents(new Hashtable());
        }

        this.GetChildAgents(agents);
        return agents;
    }

    private void LoadTree(IList<PaymentXP.BusinessObjects.Agent> agents, DataTreeNode ParentNode)
    {
        DataTreeNode node = null;
        foreach (PaymentXP.BusinessObjects.Agent agent in agents)
        {
            node = new DataTreeNode();
            node.Text = string.Format("{0} - {1}", agent.AgentID.ToString(), agent.AgentDBA);
            node.Key = agent.AgentUID.ToUpper();

            if (ParentNode != null)
                ParentNode.Nodes.Add(node);
            else
                uwtMain.Nodes.Add(node);

            if (agent.Agents.Count > 0)
                this.LoadTree(agent.Agents, node);
        }

        uwtMain.Nodes[0].Expanded = true;
    }

    private void GetChildAgents(IList<PaymentXP.BusinessObjects.Agent> agents)
    {
        DataAgent data = DataAccess.DataAgentDao;

        foreach (PaymentXP.BusinessObjects.Agent agent in agents)
        {
            data.GetChildAgents(agent);

            if (agent.Agents.Count > 0)
                this.GetChildAgents(agent.Agents);
        }
    }

    private void CloseForm()
    {
        string url = Request.QueryString["PostBackURL"].ToString();

        if (!url.Equals(string.Empty))
            Response.Redirect(url);
        else
        {
            Response.Redirect("frmAgent.aspx?Adding=false&AgentUID=" + UserSessions.CurrentAgent.AgentUID);
        }
    }


    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Add":
                this.FormNew();

                this.Country.DataSource = LookupTableHandler.LoadCountries();
                this.Country.DataTextField = "Value";
                this.Country.DataValueField = "Key";
                this.Country.DataBind();
                this.State.Visible = true;
                this.Province.Visible = false;

                this.BillingCountry.DataSource = LookupTableHandler.LoadCountries();
                this.BillingCountry.DataTextField = "Value";
                this.BillingCountry.DataValueField = "Key";
                this.BillingCountry.DataBind();
                this.BillingState.Visible = true;
                this.BillingProvince.Visible = false;

                break;

            case "Save":
                if (this.FormSave())
                {
                    url = "~/SecureAgentManagementForms/frmAgent.aspx?Adding=false";
                    url += "&AgentUID=" + this.UID;
                    Response.Redirect(url);
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Close":
                break;

            case "Delete":
                if (uwtMain.ActiveNode.Nodes.Count > 0)
                {
                    WucMessage1.AddMessageError("Please delete sub agents first.");
                    return;
                }
                if (this.FormDelete())
                {
                    this.LoadAgents2();
                    if (uwtMain.Nodes.Count > 0)
                    {
                        uwtMain.Nodes[0].Selected = true;
                        this.FormShow(uwtMain.ActiveNode.Key);
                    }
                }
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();

                UserRole role = null;
                if (!UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_SPECIALACCESS, out role))
                {
                    this.PrimaryContactUID.Enabled = false;
                }
                //Niranjan :- PXP-4795 Restrict users to be able to update Agent Memo field 
                if (!UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_AGENT_SUPER_ACCESS, out role))
                {
                    this.AgentMemo.Enabled = false;
                }
                break;
        }
    }

    public void DisableShippingAddressControls()
    {
        this.AddressLine1.ReadOnly = true;
    }

    protected void uwtMain_NodeClick(object sender, DataTreeNodeClickEventArgs e)
    {
        PaymentXP.BusinessObjects.Agent a = DataAccess.DataAgentDao.GetAgent(uwtMain.ActiveNode.Key);

        Response.Redirect(WebUtil.GetMyUrl("Adding=false&AgentUID=" + a.AgentUID));
    }

    protected void uwtMain_NodePopulate(object sender, DataTreeNodeEventArgs e)
    {
        LoadAgents2Children(e.Node);
    }

    protected void BillingAddressAsAbove_CheckedChanged(object sender, EventArgs e)
    {
        if (this.BillingAddressAsAbove.Checked == true)
        {
            this.Country.SelectedValue = this.BillingCountry.SelectedValue;
            this.Country.SelectedItem.Value = this.BillingCountry.SelectedItem.Value;

            this.Country.Text = this.BillingCountry.Text;
            this.AddressLine1.Text = this.BillingAddressLine1.Text;
            this.AddressLine2.Text = this.BillingAddressLine2.Text;
            this.City.Text = this.BillingCity.Text;
            this.State.SelectedItem.Text = this.BillingState.SelectedItem.Text;
            this.State.SelectedItem.Value = this.BillingState.SelectedItem.Value;
            this.State.SelectedValue = this.BillingState.SelectedValue;
            this.State.Text = this.BillingState.Text;
            this.ZipCode.Text = this.BillingZipCode.Text;
            this.Province.Text = this.BillingProvince.Text;
            this.ShippingFullName.Text = this.BillingFullName.Text;

            this.Country.Attributes.Add("disabled", "disabled");
            this.AddressLine1.ReadOnly = true;
            this.AddressLine2.ReadOnly = true;
            this.City.ReadOnly = true;
            this.State.Attributes.Add("disabled", "disabled");
            this.ZipCode.ReadOnly = true;
            this.Province.ReadOnly = true;
            this.ShippingFullName.ReadOnly = true;

            if (this.BillingCountry.SelectedItem.Value.Trim().ToUpper().Equals("US"))
            {
                this.Province.Visible = false;
                this.State.Visible = true;
            }

            else
            {
                this.Province.Visible = true;
                this.State.Visible = false;
            }
        }

        else if (this.BillingAddressAsAbove.Checked == false)
        {
            this.AddressLine1.Text = null;
            this.AddressLine2.Text = null;
            this.City.Text = null;
            this.ZipCode.Text = null;
            this.ShippingFullName.Text = null;
            this.Province.Text = null;

            this.Country.Attributes.Remove("disabled");
            this.State.Attributes.Remove("disabled");
            this.AddressLine1.ReadOnly = false;
            this.AddressLine2.ReadOnly = false;
            this.City.ReadOnly = false;
            this.ZipCode.ReadOnly = false;
            this.ShippingFullName.ReadOnly = false;
            this.Province.ReadOnly = false;

        }
    }

    protected void BillingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.BillingCountry.SelectedItem.Value.Trim().ToUpper().Equals("US"))
        {
            this.BillingProvince.Visible = false;
            this.BillingState.Visible = true;

        }

        else
        {
            this.BillingProvince.Visible = true;
            this.BillingState.Visible = false;
            this.BillingProvince.Text = null;

        }
    }

    protected void Country_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.Country.SelectedItem.Value.Trim().ToUpper().Equals("US"))
        {
            this.Province.Visible = false;
            this.State.Visible = true;
        }
        else
        {
            this.Province.Visible = true;
            this.State.Visible = false;
            this.Province.Text = null;
        }
    }

    protected void btnLogin_Click(object sender, ButtonEventArgs e)
    {
        PaymentXP.BusinessObjects.Agent agent = UserSessions.CurrentAgent;

        WucCreateUser1.LoginType = "Agent";
        WucCreateUser1.UserName = agent.AgentID.ToString();
        WucCreateUser1.HookTableKeyUID = agent.AgentUID;
        // Send mail to the configured address if the users are meritcard users
        bool isMeritCardUser = DataAccess.DataMerchantAppDao.IsMeritCardUser(agent.AgentUID, ConfigurationManager.AppSettings["MeritcardParentAgentUID"]);
        if (isMeritCardUser)
        {
            WucCreateUser1.EmailTo = ConfigurationManager.AppSettings["MeritcardMailsSendToAddress"];
        }
        else
        {
            WucCreateUser1.EmailTo = agent.NotificationEmails;
        }
        WucCreateUser1.DBA = agent.AgentDBA;
        WucCreateUser1.Key = agent.AgentUID;
        WucCreateUser1.Status = agent.StatusUID;
        WucCreateUser1.OfficeID = Convert.ToInt32(agent.Office);
        WucCreateUser1.Formshow();

        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }
    private void UpdateOktaUser(PaymentXP.BusinessObjects.User user, bool EmailChanged)
    {
        try
        {
            UpdateUserRequest updateUserRequest = new UpdateUserRequest()
            {
                Profile = new Okta.Sdk.Model.UserProfile(),
                Credentials = new Okta.Sdk.Model.UserCredentials()
            };

            updateUserRequest.Profile.FirstName = user.FirstName;
            updateUserRequest.Profile.LastName = user.LastName;
            updateUserRequest.Profile.Login = user.UserName;
            updateUserRequest.Profile.Email = user.Email;

            var oktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.UpdateUser(user.OktaUserID, updateUserRequest, UserSessions.PortalUID);

            if (EmailChanged)
            {
                bool isResponse = Paysafe.TwoFactorAuth.Client.Factor.Instance.EnrollActivateEmailFactor(user.OktaUserID, user.Email, UserSessions.PortalUID);
                if (isResponse)
                {
                    user.OktaUserID = user.OktaUserID;
                }
            }
        }
        catch (Exception ex)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Unable to update okta user" + user.UserName + ex.Message);
        }
    }
}
