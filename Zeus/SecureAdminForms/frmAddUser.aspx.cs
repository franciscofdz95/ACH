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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.NavigationControls;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.DataObjects;
using ZeusWeb.Class;
using System.Linq;
using Newtonsoft.Json;
using Okta.Sdk.Model;

public partial class frmAddUser : frmBaseDataEntry
{
    string adminUsers = ConfigurationManager.AppSettings["AdminUsers"];
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    public PaymentXP.BusinessObjects.User VSCurrentUser
    {
        get
        {
            if (ViewState["VSCurrentUser"] == null)
                return null;
            else
                return (PaymentXP.BusinessObjects.User)ViewState["VSCurrentUser"];
        }
        set { ViewState["VSCurrentUser"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {

            this.Master.SideMenuSelect(MasterPageAdmin.eMasterSideMenu.Users);

            //Apply security settings
            //this.UIDName = "UserID";
            //this.CurrentObject = null;
            this.VSCurrentUser = null;

            LookupTableHandler.LoadPortals(lstPortals, false);
            LookupTableHandler.LoadPortals(lstUserPortals, false);
            LookupTableHandler.LoadUserAccessLevels(AccessLevelUID, false);
            LookupTableHandler.LoadRoles(lstUserRoles, false);
            LookupTableHandler.LoadUserDefaultRoles(DefaultRoleUID, false);
            LookupTableHandler.LoadHookTables(HookTableUID, false);
            LookupTableHandler.LoadPartnerChannels(lstChannelPartners);
            LookupTableHandler.LoadOffices(OfficeID, false);
            LookupTableHandler.LoadOffices(lstOfficeAccess);
            LookupTableHandler.LoadInternalUsers(UserID, false);

            //LookupTableHandler.LoadHookTables(cboHookTables, true);
            cboHookTables.Items.Add(new ListItem("All", "-1"));
            cboHookTables.Items.Add(new ListItem("Internal", "E65F8292-7091-4EDD-A96D-378F8EA8567C"));
            cboHookTables.Items.Add(new ListItem("Bank", "C20FD654-4EE5-4B35-9F35-62628287195D"));

            this.FormClear();
            this.CreateTreeGroup();

            WucSelectMerchant1.WebDialogWindowClientID = this.WebDialogWindow2.ClientID;
            WucSelectMerchant1.HookTableDBAClientID = DBA.ClientID;
            WucSelectMerchant1.HookTableIDClientID = HookTableKeyID.ClientID;
            WucSelectMerchant1.HookTableUIDClientID = HookTableKeyUID.ClientID;

            WucSelectAgent1.WebDialogWindowClientID = this.WebDialogWindow1.ClientID;
            WucSelectAgent1.HookTableDBAClientID = DBA.ClientID;
            WucSelectAgent1.HookTableIDClientID = HookTableKeyID.ClientID;
            WucSelectAgent1.HookTableUIDClientID = HookTableKeyUID.ClientID;

            WucSelectBank1.WebDialogWindowClientID = this.WebDialogWindow3.ClientID;
            WucSelectBank1.HookTableDBAClientID = DBA.ClientID;
            WucSelectBank1.HookTableIDClientID = HookTableKeyID.ClientID;
            WucSelectBank1.HookTableUIDClientID = HookTableKeyUID.ClientID;

            FormHandler.SetControlEditMode(pnlDetail, UserSessions.CurrentUserObject != null);
        }
    }

    private void LoadFormPermissions(PaymentXP.BusinessObjects.User user)
    {
        UserFacade facade = new UserFacade();
        Hashtable prms = new Hashtable();

        prms.Add("@UserUID", user.UID);
        prms.Add("@PortalUID", lstPortals.SelectedItem.Value);

        if (lstRoles.Items.Count > 0)
        {
            prms.Add("@RoleUID", lstRoles.SelectedItem.Value);
        }
        DataSet ds = facade.GetUserFormPermissionsAll(prms);

        grdForms.DataSource = ds;
        grdForms.DataBind();
    }

    private void LoadOfficeAccess(PaymentXP.BusinessObjects.User user)
    {
        user.OfficeAccess = DataUser.GetInstance().GetUserOfficeAccess(user.UserID);

        lstOfficeAccess.ClearSelection();
        foreach (Office office in user.OfficeAccess)
        {

            foreach (ListItem item in lstOfficeAccess.Items)
            {
                if (item.Value == office.OfficeID.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }


    }

    private void LoadPartnerChannelAccess(PaymentXP.BusinessObjects.User user)
    {
        List<int> partnerChannel = DataUser.GetInstance().GetUserPartnerChannelAccess(user.UID);

        lstChannelPartners.ClearSelection();
        foreach (int pChannelId in partnerChannel)
        {
            foreach (ListItem item in lstChannelPartners.Items)
            {
                if (item.Value == pChannelId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        if (user.HookTableUID.ToUpper() == "E65F8292-7091-4EDD-A96D-378F8EA8567C")
        {
            //if user is internal then let's display the partner channel access
            this.pnlPartnerChannel.Visible = true;
        }
        else
        {
            this.pnlPartnerChannel.Visible = false;
        }
    }

    private void LoadUsers()
    {
        IList<PaymentXP.BusinessObjects.User> parents = this.GetUserParents();

        this.CreateTreeGroup();

        if (parents != null && parents.Count > 0)
        {
            this.LoadTree(parents);
            uwtUsers.ClearSelection();

            this.SelectNodeByText(uwtUsers.Nodes, txtUserName.Text);
        }
        else
        {
            this.FormClear();
        }
    }

    private void CreateTreeGroup()
    {
        DataTreeNode ParentNode = null;
        uwtUsers.Nodes.Clear();

        if (UserSessions.CurrentUser.IsInternal)
        {
            //ParentNode = new DataTreeNode("Internals", "E65F8292-7091-4EDD-A96D-378F8EA8567C", "", "", "");
            ParentNode = new DataTreeNode();
            ParentNode.Text = "Internals";
            ParentNode.Key = "E65F8292-7091-4EDD-A96D-378F8EA8567C";
            ParentNode.Expanded = true;
            uwtUsers.Nodes.Add(ParentNode);
        }

        if (UserSessions.CurrentUser.IsInternal || UserSessions.CurrentUser.IsMerchant)
        {
            //marknguyen20120124 ParentNode = new Node("Merchants", "904683F4-094B-4BDA-AEF2-1BD7931C77D0", "", "", "");
            ParentNode = new DataTreeNode();
            ParentNode.Text = "Merchants";
            ParentNode.Key = "904683F4-094B-4BDA-AEF2-1BD7931C77D0";
            ParentNode.Expanded = true;
            uwtUsers.Nodes.Add(ParentNode);
        }

        if (UserSessions.CurrentUser.IsInternal || UserSessions.CurrentUser.IsAgent)
        {
            //marknguyen20120124 ParentNode = new Node("Agents", "4CB95A71-7DD1-43F3-8F97-9BD15BB04834", "", "", "");
            ParentNode = new DataTreeNode();
            ParentNode.Text = "Agents";
            ParentNode.Key = "4CB95A71-7DD1-43F3-8F97-9BD15BB04834";
            ParentNode.Expanded = true;
            uwtUsers.Nodes.Add(ParentNode);
        }

        if (UserSessions.CurrentUser.IsInternal || UserSessions.CurrentUser.IsBank)
        {
            //marknguyen20120124 ParentNode = new Node("Banks", "C20FD654-4EE5-4B35-9F35-62628287195D", "", "", "");
            ParentNode = new DataTreeNode();
            ParentNode.Text = "Banks";
            ParentNode.Key = "C20FD654-4EE5-4B35-9F35-62628287195D";
            ParentNode.Expanded = true;
            uwtUsers.Nodes.Add(ParentNode);
        }
    }

    private void SelectNodeByText(DataTreeNodeCollection nodes, string text)
    {

        foreach (DataTreeNode node in nodes)
        {
            if (node.Text.ToLower() == txtUserName.Text.ToLower())
            {
                node.Selected = true;
                FormShow(node.Key);
                break;
            }
            else
            {
                SelectNodeByText(node.Nodes, text);
            }
        }
    }

    private void SelectFirstNode()
    {
        if (uwtUsers.Nodes.Count > 0)
        {
            uwtUsers.ClearSelection();

            foreach (DataTreeNode node in uwtUsers.Nodes)
            {
                if (node.Text.ToLower() == txtUserName.Text.ToLower())
                {
                    node.Selected = true;

                    break;
                }
            }

        }
    }

    private IList<PaymentXP.BusinessObjects.User> GetUserByUserName(string UserName)
    {
        UserFacade facade = new UserFacade();
        IList<PaymentXP.BusinessObjects.User> users = null;
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(UserName))
            prms.Add("@Username", UserName);

        if (prms.Count > 0)
        {
            users = facade.GetUserParents(prms);
        }
        return users;
    }

    private IList<PaymentXP.BusinessObjects.User> GetUserParents()
    {
        UserFacade facade = new UserFacade();
        IList<PaymentXP.BusinessObjects.User> users = null;
        Hashtable prms = new Hashtable();

        if (cboHookTables.SelectedIndex > 0)
            prms.Add("@HookTableUID", cboHookTables.SelectedItem.Value);

        if (!string.IsNullOrEmpty(txtDBA.Text))
            prms.Add("@Dba", txtDBA.Text);

        if (!string.IsNullOrEmpty(txtUserName.Text))
            prms.Add("@Username", txtUserName.Text);

        if (!string.IsNullOrEmpty(txtFirstName.Text))
            prms.Add("@FirstName", txtFirstName.Text);

        if (!string.IsNullOrEmpty(txtLastName.Text))
            prms.Add("@LastName", txtLastName.Text);

        if (prms.Count > 0)
        {
            users = facade.GetUserParents(prms);
        }
        return users;
    }

    private void GetChildUsers(PaymentXP.BusinessObjects.User user)
    {
        UserFacade facade = new UserFacade();

        facade.GetChildUsers(user);

        DataTreeNode node = null;
        DataTreeNode ParentNode = null;

        ParentNode = SelectNode(user.UID);

        if (ParentNode != null)
        {
            ParentNode.Nodes.Clear();
        }

        foreach (PaymentXP.BusinessObjects.User u in user.Users)
        {
            node = new DataTreeNode();
            node.Key = u.UID;

            if (u.IsInternal)
                node.Text = u.UserName + Master;
            else
                node.Text = u.UserName;

            if (ParentNode != null)
                ParentNode.Nodes.Add(node);
        }
    }

    private DataTreeNode SelectNode(string key)
    {
        DataTreeNode node = null;
        foreach (object obj in uwtUsers.AllNodes)
        {
            DataTreeNode parent = (DataTreeNode)obj;

            if (parent.Key.ToUpper() == key.ToUpper())
            {
                uwtUsers.ClearSelection();
                parent.Selected = true;
                uwtUsers.ActiveNode = parent;

                parent.ExpandAnscestors();
                parent.ExpandChildren();

                node = parent;
                break;
            }
            else
            {
                DataTreeNode n = parent.Nodes.FindNodeByKey(key);

                if (n != null && n.Key.ToUpper() == key.ToUpper())
                {
                    uwtUsers.ClearSelection();
                    n.Selected = true;
                    uwtUsers.ActiveNode = n;

                    n.ExpandAnscestors();
                    n.ExpandChildren();

                    node = n;
                    break;
                }
            }
        }

        return node;
    }

    private void LoadTree(IList<PaymentXP.BusinessObjects.User> users)
    {
        DataTreeNode node = null;
        DataTreeNode ParentNode = null;

        foreach (PaymentXP.BusinessObjects.User user in users)
        {
            if (user.ParentUID == string.Empty)
            {
                if (user.IsBank)
                    ParentNode = SelectNode("C20FD654-4EE5-4B35-9F35-62628287195D");
                else if (user.IsAgent)
                    ParentNode = SelectNode("4CB95A71-7DD1-43F3-8F97-9BD15BB04834");
                else if (user.IsMerchant)
                    ParentNode = SelectNode("904683F4-094B-4BDA-AEF2-1BD7931C77D0");
                else
                    ParentNode = SelectNode("E65F8292-7091-4EDD-A96D-378F8EA8567C");
            }
            else
                ParentNode = SelectNode(user.ParentUID);

            string Master = string.Empty;

            if (user.ParentUID == string.Empty)
                Master = " (Super User)";

            node = new DataTreeNode();
            node.Key = user.UID;
            if (user.IsInternal)
                node.Text = user.UserName + Master;
            else
                node.Text = user.UserName;


            if (ParentNode != null)
                ParentNode.Nodes.Add(node);
            else
                uwtUsers.Nodes.Add(node);

            if (user.Users.Count > 0)
                this.LoadTree(user.Users);
        }
    }

    public override void FormShow(string ID)
    {
        UserFacade facade = new UserFacade();
        PaymentXP.BusinessObjects.User user = facade.GetUser(ID);

        FormBinding.BindObjectToControls(user, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

        if (!this.Adding)
        {
            grdForms.Enabled = true;
            grdObjects.Enabled = true;
        }


        lnkLookup.Enabled = this.EditMode;

        lstPortals.Enabled = true;

        Password.Text = user.PasswordMask;

        ListHandler.ListFindItem(OfficeID, Convert.ToString((int)user.Office));

        lstUserRoles.Items.Clear();
        lstRoles.Items.Clear();
        foreach (KeyValuePair<string, UserRole> kvp in user.UserRoles)
        {
            ListItem lstItem = new ListItem(kvp.Value.Name, kvp.Value.RoleID);
            if (kvp.Value.Enabled)
            {
                lstItem.Selected = true;
                lstRoles.Items.Add(new ListItem(kvp.Value.Name, kvp.Value.RoleID));
            }

            lstUserRoles.Items.Add(lstItem);
        }

        lstUserPortals.Items.Clear();



        foreach (UserPortal item in user.UserPortals)
        {
            ListItem lstItem = new ListItem(item.Name, item.PortalID);
            if (item.Enabled)
                lstItem.Selected = true;

            lstUserPortals.Items.Add(lstItem);
        }

        this.LoadFormPermissions(user);
        this.LoadPartnerChannelAccess(user);
        this.LoadOfficeAccess(user);

        btnSendAccount.Visible = user.IsInternal || user.IsBank;

        if (adminUsers.Contains(UserSessions.CurrentUser.UserName.ToLower()) && this.EditMode)
        {
            Password.Visible = false;
            txtPassword.Visible = true;
            txtPassword.ReadOnly = false;
            txtPassword.Text = user.Password;

            ChangePwdDate.ReadOnly = false;
            LoginAttempts.ReadOnly = false;

        }

        this.VSCurrentUser = user;
        this.UID = user.UID;

        this.GetChildUsers(user);

        FormHandler.SetControlEditMode(pnlDetail, this.VSCurrentUser != null);

        if (user.LastPwdChangeDate == DateTime.MinValue)
        {
            LastPwdChangeDate.Text = "";
        }

    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(this);

        btnSendAccount.Visible = false;
        rowCopyUser.Visible = false;
    }

    public override bool FormSave()
    {
        string OrigPassword = string.Empty;
        UserFacade facade = new UserFacade();

        if (!this.FormDataCheck())
            return false;

        try
        {
            PaymentXP.BusinessObjects.User user;
            PaymentXP.BusinessObjects.User Copyuser;
            if (this.Adding)
            {
                user = new PaymentXP.BusinessObjects.User();
            }
            else
            {
                //user = (User)this.CurrentObject;
                user = VSCurrentUser;
                OrigPassword = user.Password;
            }

            //if current default role is First Team and it changes to another default role, clear any existing Merchant User records for this user
            if ((user.DefaultRoleUID.ToUpper().Equals(Constants.ROLE_FIRSTTEAM.ToUpper())) && (user.DefaultRoleUID != this.DefaultRoleUID.SelectedValue))
            {
                DataUser.GetInstance().DeleteMerchantUser(Constants.ROLE_FIRSTTEAM, user.UID);
            }

            FormBinding.BindControlsToObject(user, pnlDetail);
            PaymentXP.BusinessObjects.User user2 = UserSessions.CurrentUser;
            user.UserModified = user2.UserName;
            user.UserCreated = user2.UserName;

            int officeid;
            int.TryParse(OfficeID.SelectedValue, out officeid);

            user.Office = (CommonUtility.Util.Offices)officeid;



            if (!this.Adding)
            {
                if (Password.Text.Trim().ToUpper() != user.PasswordMask.ToUpper())
                {
                    user.Password = Password.Text;
                }
                else
                {
                    user.Password = OrigPassword;

                    if (adminUsers.Contains(UserSessions.CurrentUser.UserName.ToLower()) && this.EditMode)
                    {
                        user.Password = txtPassword.Text.Trim();

                    }

                }

                facade.UpdateUser(user);

                if (this.UserID != null && this.UserID.SelectedItem != null && !this.UserID.SelectedItem.Value.Equals("-1"))
                {
                    Copyuser = facade.GetUser(this.UserID.SelectedItem.Value);

                    user.UserRoles = Copyuser.UserRoles;
                    user.UserPortals = Copyuser.UserPortals;

                    this.SaveUserRoles(Copyuser);
                    this.SaveUserPortals(Copyuser);
                    this.SaveChannelPartners(Copyuser);
                    this.SaveOfficeAccess(Copyuser);
                    //TO DO: Save Form Permissions.
                }

                else
                {
                    this.SaveUserRoles();
                    this.SaveUserPortals();
                    this.SaveChannelPartners();
                    this.SaveOfficeAccess();

                }

                //Ani - DM-4775
                if (PaymentXP.BusinessObjects.Constants.IS_MFA_ENABLED &&
                    ((user.IsAgent || user.IsMerchant)))
                {
                    if (!string.IsNullOrWhiteSpace(user.OktaUserID))
                    {
                        if (user.IsMFAEnabled)
                        {
                            var oktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.GetUser(user.OktaUserID, UserSessions.PortalUID);

                            if (oktaUser != null)
                            {
                                Okta.Sdk.Model.User oktauser = Paysafe.TwoFactorAuth.Client.User.Instance.SwitchStatusUser(user.OktaUserID, user.HasDBAccess, UserSessions.PortalUID);
                                if (oktauser != null)
                                {
                                    if (oktauser.Status.Value.ToUpper().Equals(PaymentXP.BusinessObjects.Constants.OKTA_STATUS_ACTIVE))
                                        user.IsMFAActive = true;
                                    else
                                        user.IsMFAActive = false;
                                    UpdateOktaUser(user);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (user.HasDBAccess)
                            CreateAndActivateOktaUser(user);
                    }
                    facade.UpdateUser(user);
                }
            }
            else
            {
                facade.InsertUser(user);
                if (user.UID != "-1")
                {
                    this.UID = user.UID;
                    this.Adding = false;
                    this.VSCurrentUser = user;

                    //Do this when the user wants to copy from existing user.                    
                    if (this.UserID != null && this.UserID.SelectedItem != null && !this.UserID.SelectedItem.Value.Equals("-1"))
                    {
                        Copyuser = facade.GetUser(this.UserID.SelectedItem.Value);

                        user.UserRoles = Copyuser.UserRoles;
                        user.UserPortals = Copyuser.UserPortals;

                        this.SaveUserRoles(Copyuser);
                        this.SaveUserPortals(Copyuser);
                        this.SaveChannelPartners(Copyuser);
                        this.SaveOfficeAccess(Copyuser);
                        //TO DO: Save Form Permissions.
                    }

                    else
                    {
                        this.SaveUserRoles();
                        this.SaveUserPortals();
                        this.SaveChannelPartners();
                        this.SaveOfficeAccess();
                    }

                    //Ani - DM-4775
                    string statusMsg = string.Empty;
                    if (HookTableUID.SelectedItem.Value.ToUpper().Equals("4CB95A71-7DD1-43F3-8F97-9BD15BB04834") || HookTableUID.SelectedItem.Value.ToUpper().Equals("904683F4-094B-4BDA-AEF2-1BD7931C77D0"))
                    {
                        //Ani - DM-5294
                        statusMsg = CreateAndActivateOktaUser(user);
                    }
                    this.FormClear();
                    txtUserName.Text = user.UserName;
                    txtCustomError.Visible = true;
                    txtCustomError.Text = statusMsg;
                }
            }



            this.LoadUsers();

            LookupTableHandler.m_ActiveUsers = null;

            //select parent and load children
            if (!string.IsNullOrEmpty(user.ParentUID))
            {
                SelectNode(user.ParentUID);
                FormShow(user.ParentUID);
            }

            DataTreeNode node = SelectNode(user.UID);


            this.EditMode = false;
            this.ToggleButtons();
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    //Ani - DM-4775
    private string CreateAndActivateOktaUser(PaymentXP.BusinessObjects.User user)
    {
        string statusMsg = string.Empty;
        try
        {
            if ((user.UID != null) && (user.UID != null) && (PaymentXP.BusinessObjects.Constants.IS_MFA_ENABLED))
            {
                UserFacade facade = new UserFacade();
                string oktaPwd = facade.GetRandomPassword(8);
                PasswordCredential oktaPassword = new PasswordCredential() { Value = oktaPwd };
                CreateUserRequest createUserRequest = new CreateUserRequest()
                {
                    Profile = new Okta.Sdk.Model.UserProfile(),
                    Credentials = new Okta.Sdk.Model.UserCredentials()
                };
                createUserRequest.Profile.FirstName = user.FirstName;
                createUserRequest.Profile.LastName = user.LastName;
                createUserRequest.Profile.Login = user.UserName;
                createUserRequest.Profile.Email = user.Email;
                createUserRequest.Profile.UserType = user.HookTableUID.ToUpper() == "4CB95A71-7DD1-43F3-8F97-9BD15BB04834" ? Constants.OKTA_USERTYPE_AGENT :
                                           user.HookTableUID.ToUpper() == "904683F4-094B-4BDA-AEF2-1BD7931C77D0" ? Constants.OKTA_USERTYPE_MERCHANT : "OTHER";
                createUserRequest.Credentials.Password = oktaPassword;
                createUserRequest.GroupIds = new List<string>() { Constants.MFA_GROUP };


                //Ani: DM-5294
                txtCustomError.Visible = false;
                var OktaUser = Paysafe.TwoFactorAuth.Client.User.Instance.CreateUser(createUserRequest, portalUID: UserSessions.PortalUID);

                if (OktaUser != null)
                {
                    user.IsMFAEnabled = Constants.IS_MFA_ENABLED;
                    user.OktaUserID = OktaUser.Id;
                    user.OktaPassword = oktaPwd;
                    user.MFAGroupID = Constants.MFA_GROUP;
                    user.IsMFAActive = true;
                    facade.UpdateUser(user);
                }
                else
                    {
                        statusMsg = "User creation is not successful. Please contact Administrator.";
                    }
            }
        }
        catch (Exception ex)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Unable to create and activate okta user" + user.UserName + ex.Message);
        }
        return statusMsg;
    }

    private void UpdateOktaUser(PaymentXP.BusinessObjects.User user)
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

            if (oktaUser != null)
            {
                bool isResponse = Paysafe.TwoFactorAuth.Client.Factor.Instance.EnrollActivateEmailFactor(user.OktaUserID, user.Email, UserSessions.PortalUID);
                if (isResponse)
                {
                    user.OktaUserID = user.OktaUserID;
                }
            }

        }
        catch (Exception exc)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusLog.Error("Unable to update okta user" + user.UserName + exc.Message);
        }
    }

    private bool SaveUserRoles(PaymentXP.BusinessObjects.User CopyUser)
    {

        try
        {
            UserFacade facade = new UserFacade();
            PaymentXP.BusinessObjects.User objuser = this.VSCurrentUser;
            lstUserRoles.Items.Clear();
            foreach (KeyValuePair<string, UserRole> kvp in CopyUser.UserRoles)
            {
                ListItem lstItem = new ListItem(kvp.Value.Name, kvp.Value.RoleID);
                lstItem.Selected = kvp.Value.Enabled;
                facade.CopyUserRoles(this.UID, kvp.Value.RoleID, kvp.Value.Enabled, CopyUser.UID);
                lstUserRoles.Items.Add(lstItem);
            }

            return true;

        }
        catch (Exception exc)
        {
            throw exc;
        }

    }

    private bool SaveUserRoles()
    {
        try
        {
            UserFacade facade = new UserFacade();
            PaymentXP.BusinessObjects.User objuser = this.VSCurrentUser;
            UserRole userrole = null;

            foreach (ListItem item in lstUserRoles.Items)
            {
                userrole = null;

                if ((objuser.UserRoles.TryGetValue(item.Value, out userrole) && userrole.Enabled != item.Selected)
                    || (userrole == null && item.Selected))
                    facade.InsertUserRoles(this.UID, item.Value, item.Selected);
            }

            return true;

        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    private bool SaveUserPortals(PaymentXP.BusinessObjects.User CopyUser)
    {
        UserFacade facade = new UserFacade();

        foreach (UserPortal item in CopyUser.UserPortals)
        {
            facade.InsertUserPortals(this.UID, item.PortalID, item.Enabled);
        }

        return true;
    }

    private bool SaveUserPortals()
    {
        try
        {
            UserFacade facade = new UserFacade();
            foreach (ListItem item in lstUserPortals.Items)
            {
                facade.InsertUserPortals(this.UID, item.Value, item.Selected);
            }
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    private void SaveOfficeAccess(PaymentXP.BusinessObjects.User CopyUser)
    {
        List<int> officeAccess = new List<int>();
        CopyUser.OfficeAccess = DataUser.GetInstance().GetUserOfficeAccess(CopyUser.UserID);

        foreach (Office office in CopyUser.OfficeAccess)
        {

            foreach (ListItem item in lstOfficeAccess.Items)
            {
                if (item.Value == office.OfficeID.ToString())
                {
                    officeAccess.Add(int.Parse(item.Value));
                }
            }
        }

        UserFacade facade = new UserFacade();
        facade.SaveOfficeAccess(this.UID, officeAccess);
    }


    private void SaveOfficeAccess()
    {
        List<int> officeAccess = new List<int>();

        foreach (ListItem item in lstOfficeAccess.Items)
        {
            if (item.Selected)
            {
                officeAccess.Add(int.Parse(item.Value));
            }
        }

        UserFacade facade = new UserFacade();

        facade.SaveOfficeAccess(this.UID, officeAccess);
    }

    private void SaveChannelPartners(PaymentXP.BusinessObjects.User CopyUser)
    {
        List<int> partnerChannels = DataUser.GetInstance().GetUserPartnerChannelAccess(CopyUser.UID);
        UserFacade facade = new UserFacade();
        facade.SavePartnerChannelAccess(this.UID, partnerChannels);
    }

    private void SaveChannelPartners()
    {
        List<int> partnerChannels = new List<int>();

        foreach (ListItem item in lstChannelPartners.Items)
        {
            if (item.Selected)
            {
                partnerChannels.Add(int.Parse(item.Value));
            }
        }

        UserFacade facade = new UserFacade();

        facade.SavePartnerChannelAccess(this.UID, partnerChannels);
    }

    public override bool FormDelete()
    {
        if (this.UID.Equals(string.Empty))
            return false;

        UserFacade facade = new UserFacade();
        int rows = facade.DeleteUser(this.UID);
        if (rows > 0)
        {
            return true;
        }
        else
            return false;
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnAdd.Enabled = !btnAdd.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;

        uwtUsers.Enabled = !uwtUsers.Enabled;
        pnlSearchParameters.Enabled = !pnlSearchParameters.Enabled;
        lnkFrmDisableAll.Enabled = !lnkFrmDisableAll.Enabled;
        lnkFrmEnableAll.Enabled = !lnkFrmEnableAll.Enabled;
        lnkObjDisableAll.Enabled = !lnkObjDisableAll.Enabled;
        lnkObjEnableAll.Enabled = !lnkObjEnableAll.Enabled;
        pnlPartnerChannel.Enabled = !pnlPartnerChannel.Enabled;
        pnlRoles.Enabled = !pnlRoles.Enabled;
        pnlPortals.Enabled = !pnlPortals.Enabled;
        pnlDetail.Enabled = !pnlDetail.Enabled;
        pnlOfficeAccess.Enabled = !pnlOfficeAccess.Enabled;
    }

    public override void FormNew()
    {
        this.FormClear();

        HasDBAccess.Checked = true;
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        this.ToggleButtons();
        ChangePwdDate.ReadOnly = true;
        LoginAttempts.ReadOnly = true;
        if (this.VSCurrentUser != null)
        {
            PaymentXP.BusinessObjects.User user = this.VSCurrentUser;
            if (user.IsAgent || user.IsMerchant) //Agent and Merchant
            {
                DataTreeNode node = uwtUsers.SelectedNodes[0];
                ParentUserFullName.Text = node.Text;
                ParentUID.Text = node.Key;
                HookTableKeyUID.Value = user.HookTableKeyUID;
                ListHandler.ListFindItem(HookTableUID, user.HookTableUID);
                HookTableUID.Enabled = false;
            }
            else
            {
                ListHandler.ListFindItem(HookTableUID, user.HookTableUID);
                HookTableUID.Enabled = false;
            }

            //checkbox all sales partner channel by default if new user is internal
            if (user.IsInternal)
            {
                foreach (ListItem item in this.lstChannelPartners.Items)
                {
                    item.Selected = true;
                }
            }
        }

        grdForms.Enabled = false;
        grdObjects.Enabled = false;
        this.UserName.Enabled = true;

    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();


    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;

        if (HookTableUID.SelectedIndex == 0)
            this.Master.AddMessageError("Type Name is required.");

        if (UserName.Text == string.Empty)
            this.Master.AddMessageError("User Name is required.");

        if (FirstName.Text == string.Empty)
            this.Master.AddMessageError("First Name is required.");

        if (Email.Text == string.Empty)
            this.Master.AddMessageError("Email is required.");
        else
        {
            message = CommonUtility.Util.IsValidEmailList(Email.Text);
            if (!string.IsNullOrEmpty(message))
                this.Master.AddMessageError(message + " is not a valid Email Address.");
        }

        if (AccessLevelUID.SelectedItem.Value == "-1")
            this.Master.AddMessageError("Access Level is required.");

        if (DefaultRoleUID.SelectedItem.Value == "-1")
            this.Master.AddMessageError("Main Dept. is required.");

        if ((HookTableUID.SelectedItem.Value.ToUpper().Equals("4CB95A71-7DD1-43F3-8F97-9BD15BB04834") || HookTableUID.SelectedItem.Value.ToUpper().Equals("904683F4-094B-4BDA-AEF2-1BD7931C77D0")) && String.IsNullOrEmpty(HookTableKeyID.Text.Trim()))
        {
            this.Master.AddMessageError("Hook Table Lookup is Required.");
        }

        if (this.Adding)
        {
            UserFacade facade = new UserFacade();
            Hashtable prms = new Hashtable();
            prms.Add("@UserName", UserName.Text.Trim());
            PaymentXP.BusinessObjects.User user = facade.GetUser(prms);

            if (user != null)
                this.Master.AddMessageError("User already exists in the system. Please enter a new user name.");
        }

        if (OfficeID.SelectedItem.Value == "-1")
            this.Master.AddMessageError("Primary Office is required.");

        //check if atleast one office access is provided for the users.
        if (string.IsNullOrEmpty(lstOfficeAccess.SelectedValue))
        {
            this.Master.AddMessageError("Please provide at least one office access to the user.");
        }


        if (this.Master.ErrorCount() == 0)
            return true;
        else
        {
            return false;
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Add":
                if (this.VSCurrentUser != null)
                {
                    PaymentXP.BusinessObjects.User user = this.VSCurrentUser;

                    if (user.IsAgent || user.IsMerchant)
                    {
                        if (user.ParentUID != string.Empty)
                        {
                            this.Master.AddMessageError("Please add users under the Super User.");
                            return;
                        }
                    }
                }

                this.FormNew();
                break;
            case "Save":
                if (txtPassword.Visible && txtPassword.Text.Trim().Equals(string.Empty) && adminUsers.Contains(UserSessions.CurrentUser.UserName.ToLower()))
                {
                    this.Master.AddMessageError("Please provide password.");
                }

                if (this.FormSave())
                {
                    if (this.VSCurrentUser != null && uwtUsers.ActiveNode != null)//marknguyen20120124
                        this.FormShow(uwtUsers.ActiveNode.Key);//marknguyen20120124
                    txtPassword.Visible = false;
                    Password.Visible = true;
                }

                break;
            case "Refresh":
                if (!string.IsNullOrEmpty(this.UID))
                    this.FormShow(this.UID);
                txtPassword.Visible = false;
                Password.Visible = true;

                break;
            case "Cancel":
                if (this.UID.Equals(string.Empty))
                {
                    Response.Redirect("frmAddUser.aspx?");
                }
                else
                    this.FormCancel();
                txtPassword.Visible = false;
                Password.Visible = true;

                break;
            case "Close":

                break;
            case "Delete":
                if (this.VSCurrentUser == null)
                    return;

                if (this.FormDelete())
                {
                    this.UID = string.Empty;
                    this.LoadUsers();
                }

                break;
            case "Edit":

                if (this.VSCurrentUser == null)
                    return;

                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                this.UserName.Enabled = false;
                break;
            case "Email Account":
                if (this.VSCurrentUser != null)
                {
                    bool perform = UserFacade.InitiateResetPasswordProcess(this.VSCurrentUser, WebUtil.GetBaseUrl());
                    this.Master.AddMessageStatus("Account has been emailed!");

                    if (perform)
                        ZeusWeb.Logging.EmailLog.InfoFormat("Reset Password- Account details has been emailed for user : {0}", this.VSCurrentUser.UserName);
                    else
                        ZeusWeb.Logging.EmailLog.InfoFormat("Error while sending email for Reset Password : {0}", this.VSCurrentUser.UserName);

                }
                break;
        }
    }





    protected void uwtUsers_NodeClick(object sender, DataTreeNodeClickEventArgs e)
    {
        switch (uwtUsers.ActiveNode.Key) //marknguyen20120124
        {
            case "E65F8292-7091-4EDD-A96D-378F8EA8567C":
            case "904683F4-094B-4BDA-AEF2-1BD7931C77D0":
            case "4CB95A71-7DD1-43F3-8F97-9BD15BB04834":
            case "C20FD654-4EE5-4B35-9F35-62628287195D":
                //this.CurrentObject = null;
                this.VSCurrentUser = null;
                this.FormClear();
                break;
            default:
                this.FormShow(uwtUsers.ActiveNode.Key); //marknguyen20120124

                //Response.Redirect("~/SecureAdminForms/frmAddUser.aspx?UserObjectUID=" + uwtUsers.ActiveNode.Key);

                break;
        }

    }

    protected void lstPortals_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadForms();

    }
    protected void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadForms();
    }
    private void LoadForms()
    {
        //if (this.CurrentObject == null)
        if (this.VSCurrentUser == null)
            return;

        //User user = (User)this.CurrentObject;
        PaymentXP.BusinessObjects.User user = this.VSCurrentUser;
        this.LoadFormPermissions(user);
    }

    protected void lnkFrmEnableAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdForms.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkEnabled");
            chk.Checked = true;
        }
    }

    protected void lnkFrmDisableAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdForms.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkEnabled");
            chk.Checked = false;
        }
    }

    protected void lnkObjEnableAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdObjects.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkEnabled");
            chk.Checked = true;
        }
    }

    protected void lnkObjDisableAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdObjects.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkEnabled");
            chk.Checked = false;
        }
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
        this.UID = string.Empty;
        this.VSCurrentUser = null;
        this.CreateTreeGroup();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.LoadUsers();
        Password.Visible = true;
        txtPassword.Visible = false;
    }

    protected void grdForms_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkFormID");
                lnk.CommandArgument = DataBinder.Eval(e.Row.DataItem, "FormUID").ToString();
                lnk.CommandName = "Form";
                lnk.Text = DataBinder.Eval(e.Row.DataItem, "FormID").ToString();


                CheckBox chk = (CheckBox)e.Row.FindControl("chkEnabled");
                chk.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasAccess"));


                break;
            default:
                break;
        }
    }

    protected void grdForms_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Form")
        {
            this.LoadObjectPermissions(this.UID, e.CommandArgument.ToString());

            //this.LoadObjectsForms(e.CommandArgument.ToString());
            dlgObjects.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
    }

    private void LoadObjectPermissions(string UserUID, string FormUID)
    {
        UserFacade facade = new UserFacade();
        Hashtable prms = new Hashtable();

        prms.Add("@UserUID", UserUID);
        prms.Add("@FormUID", FormUID);
        prms.Add("@PortalUID", lstPortals.SelectedItem.Value);
        prms.Add("@RoleUID", lstRoles.SelectedItem.Value);

        DataSet ds = facade.GetUserObjectPermissionsAll(prms);

        grdObjects.DataSource = ds;
        grdObjects.DataBind();
    }

    protected void grdObjects2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                CheckBox chk = (CheckBox)e.Row.FindControl("chkEnabled");
                chk.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Enabled"));

                CheckBox chk2 = (CheckBox)e.Row.FindControl("chkVisible");
                chk2.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Visible"));
                break;
            default:
                break;
        }
    }

    public void chkForms_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int rowIndex = grdRow.RowIndex;

            string UserRoleFormUID = grdForms.DataKeys[rowIndex].Values["UserRoleFormUID"].ToString();

            string UserUID = grdForms.DataKeys[rowIndex].Values["UserUID"].ToString();
            string RoleUID = grdForms.DataKeys[rowIndex].Values["RoleUID"].ToString();
            string FormUID = grdForms.DataKeys[rowIndex].Values["FormUID"].ToString();
            CheckBox chk = (CheckBox)grdRow.FindControl("chkEnabled");


            UserFacade facade = new UserFacade();

            //facade.UpdateUserRoleForm(UserRoleFormUID, Convert.ToBoolean(chk.Checked));
            facade.InsertUserRoleForm(UserUID, RoleUID, FormUID, Convert.ToBoolean(chk.Checked));


        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public void chkObjects_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int rowIndex = grdRow.RowIndex;

            string UserRoleObjectUID = grdObjects.DataKeys[rowIndex].Values["UserRoleObjectUID"].ToString();
            string FormRefUID = grdObjects.DataKeys[rowIndex].Values["FormRefUID"].ToString();
            CheckBox chkEnabled = (CheckBox)grdRow.FindControl("chkEnabled");
            CheckBox chkVisible = (CheckBox)grdRow.FindControl("chkVisible");

            string UserUID = grdObjects.DataKeys[rowIndex].Values["UserUID"].ToString();
            string RoleUID = grdObjects.DataKeys[rowIndex].Values["RoleUID"].ToString();
            string ObjectUID = grdObjects.DataKeys[rowIndex].Values["ObjectUID"].ToString();

            UserFacade facade = new UserFacade();

            if ((ObjectUID.ToUpper().Equals(Constants.PNL_BANK_FIELDS_MERCHANT_PROFILE) || ObjectUID.ToUpper().Equals(Constants.PNL_BANK_FIELDS_MERCHANT_ACH)) && chkEnabled.Checked)
                chkVisible.Checked = true;

            facade.AddUserRoleObject(UserUID, RoleUID, ObjectUID,
                    Convert.ToBoolean(chkVisible.Checked),
                    Convert.ToBoolean(chkEnabled.Checked));

            if (!string.IsNullOrEmpty(FormRefUID))
            {
                facade.InsertUserRoleForm(UserUID, RoleUID, FormRefUID, Convert.ToBoolean(chkEnabled.Checked));
            }

        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    protected void HookTableUID_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.HookTableUID.SelectedValue.ToUpper() == "E65F8292-7091-4EDD-A96D-378F8EA8567C")
        {
            //if user is internal then let's display the partner channel access
            this.pnlPartnerChannel.Visible = true;
        }
        else
        {
            this.pnlPartnerChannel.Visible = false;
        }
    }

    private void DisplayPartnerChannelPanel()
    {

    }



}
