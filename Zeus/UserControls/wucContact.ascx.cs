using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections;
using Infragistics.Web.UI.EditorControls;
using System.Text;
using System.Linq;

namespace ZeusWeb.UserControls
{

    public partial class wucContact : wucBaseDataEntry
    {
        public string ControlCssClass
        {
            set
            {
                if (pnlMain != null)
                {
                    pnlMain.CssClass = value;
                }
            }
        }

        public int officeID
        {
            get
            {
                if (ViewState["officeID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["officeID"];
                }
            }
            set { ViewState["officeID"] = value; }
        }

        // TEMPORARY. we're gonna do this right now, until we get the alerts done.
        public string GetNotificationEmails
        {
            get
            {
                return NotificationEmails.Text;
            }
        }
        public bool GetDisableSendRDRNotifi
        {
            get
            {
                return CheckBoxRDRVerifi.Checked;
            }
        }

        // used to store the object id, ie: MerchantID, LeadID, AgentID.. etc... used in conjunction with ControlContactType
        public int ObjectID
        {
            get
            {
                if (ViewState["ObjectID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["ObjectID"];
                }
            }
            set { ViewState["ObjectID"] = value; }
        }

        public List<Contact> VSContact
        {
            get
            {
                if (ViewState["VSContact"] == null)
                {
                    return null;
                }
                else
                {
                    return (List<Contact>)ViewState["VSContact"];
                }
            }
            set { ViewState["VSContact"] = value; }
        }

        /// <summary>
        /// used to store the previous contact when selecting between contacts in the DDL on edit mode.
        /// </summary>
        private int PreviousContactID
        {
            get
            {
                if (ViewState["PreviousContactID"] == null)
                {
                    return -1;
                }
                else
                {
                    return (int)ViewState["PreviousContactID"];
                }
            }
            set { ViewState["PreviousContactID"] = value; }
        }

        private eControlContactType _ControlContactType;

        public eControlContactType ControlContactType
        {
            get { return _ControlContactType; }
            set { _ControlContactType = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PreRender += new EventHandler(wucContact_PreRender);

            //Added for Validation for phone number, Country Code and Extention 
            if (gvPhone.Rows != null && gvPhone.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvPhone.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList dpCountryCode = (DropDownList)gvr.FindControl("ContactPhoneCountryCode");
                        TextBox cd = (TextBox)gvr.FindControl("ContactCountryCodeDisplay");
                        cd.Text = dpCountryCode.SelectedValue;
                    }
                }
            }


            if (!Page.IsPostBack)
            {
                // on initial load, reset VSContact to prevent ghosting.
                this.VSContact = null;

                // pull fresh from database from the first time.
                this.FormShow("", false);
            }
        }

        private List<Contact> GetVSContactFromDB()
        {
            Hashtable prms = new Hashtable();

            List<Contact> li = null;

            if (this.ObjectID > 0)
            {
                switch (this.ControlContactType)
                {
                    case eControlContactType.Agent:
                        prms.Add("@AgentID", this.ObjectID);
                        break;

                    case eControlContactType.Lead:
                        prms.Add("@LeadID", this.ObjectID);
                        break;

                    case eControlContactType.Merchant:
                        prms.Add("@MerchantID", this.ObjectID);
                        break;
                }

                li = DataContact.SearchContact(prms, this.ControlContactType);
            }


            return li;


        }

        // id is the contactid
        public override void FormShow(string ID)
        {
            this.FormShow(ID, false);
        }

        private void FormShowInEditMode(int contact_id, List<Contact> liC)
        {

            ddlContacts.Items.Clear();

            bool already_selected = false;

            if (liC != null && liC.Count > 0)
            {
                foreach (Contact c in liC)
                {
                    if (c.IsActive)
                    {

                        ListItem li = null;
                        if (c.IsPrimary)
                        {
                            li = new ListItem("Primary: " + c.GetFullName() + " (" + CommonUtility.Util.GetEnumDescription(c.ContactType) + ")", c.ContactID.ToString());
                        }
                        else
                        {
                            li = new ListItem(c.GetFullName() + " (" + CommonUtility.Util.GetEnumDescription(c.ContactType) + ")", c.ContactID.ToString());
                        }

                        if (!already_selected && (c.ContactID == contact_id || contact_id == 0))
                        {
                            li.Selected = true;
                            cbIsPrimary.Checked = c.IsPrimary;
                            //PXP-6771 Fady Massoud 10/05/2018
                            tbFirstname.Text = c.FirstName;
                            tbMiddlename.Text = c.MiddleName;
                            tbLastname.Text = c.LastName;
                            ddlTitle.SelectedValue = Convert.ToString((int)c.ContactType);
                            //ddlAccessLevel.SelectedValue = Convert.ToString((int)c.AccessLevel); //PXP-2867 Rohit Thakur
                            ListHandler.ListFindItem(ddlAccessLevel, Convert.ToString((int)c.AccessLevel));

                            gvEmail.DataSource = c.EmailAddressList;
                            gvEmail.DataBind();

                            gvPhone.DataSource = c.PhoneList;
                            gvPhone.DataBind();

                            already_selected = true;


                        }

                        ddlContacts.Items.Add(li);
                    }
                }
            }
            else
            {

                cbIsPrimary.Checked = false;
                tbFirstname.Text = "";
                //PXP-6771 Fady Massoud 10/05/2018
                tbMiddlename.Text = "";
                tbLastname.Text = "";
                ddlTitle.SelectedIndex = 0; // default to first one);
                ddlAccessLevel.SelectedIndex = -1;


                Contact c = new Contact();
                c.ContactID = GetTempID();
                c.ContactType = ContactType.CEO;

                trContacts.Visible = false;

                if (this.Adding)
                {
                    // when adding, the DDL does not store viewstate, so we write to a contact in a hiddenvalue (which does store viewstate)
                    hidNewContactID.Value = c.ContactID.ToString();

                    // if youre adding for the first time, keep this hidden.
                    lbAddNewContact.Visible = !this.Adding;
                }


                this.VSContact = new List<Contact>();
                this.VSContact.Add(c);

                gvEmail.DataSource = this.GetEmailBoxes(1);
                gvEmail.DataBind();

                gvPhone.DataSource = this.GetPhoneBoxes(3);
                gvPhone.DataBind();
            }
            //PXP-2867 Rohit Thakur Zeus: Merchant Contact access level dropdown
            if (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING)
                || UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_APPLICATION_BOARDING))
            {
                ddlAccessLevel.Enabled = true;
            }
            else ddlAccessLevel.Enabled = false;
        }

        private void FormShowInViewMode(int contact_id, List<Contact> liC)
        {
            bool already_selected = false;

            ddlName.Items.Clear();

            if (liC != null && liC.Count > 0)
            {
                ddlName.Visible = true;

                foreach (Contact c in liC)
                {
                    if (c.IsActive)
                    {

                        if (c.IsPrimary)
                        {
                            ListItem li = new ListItem("Primary: " + c.GetFullName() + " (" + CommonUtility.Util.GetEnumDescription(c.ContactType) + ")", c.ContactID.ToString());
                            ddlName.Items.Add(li);
                        }
                        else
                        {
                            ddlName.Items.Add(new ListItem(c.GetFullName() + " (" + CommonUtility.Util.GetEnumDescription(c.ContactType) + ")", c.ContactID.ToString()));
                        }


                        if (!already_selected && (contact_id == c.ContactID || contact_id == 0))
                        {
                            litContactType.Text = CommonUtility.Util.GetEnumDescription(c.ContactType);
                            litAccessLevel.Text = CommonUtility.Util.GetEnumDescription(c.AccessLevel);

                            litEmailList.Text = "";

                            if (c.EmailAddressList != null && c.EmailAddressList.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                sb.Append("<ul>");
                                foreach (EmailAddress ea in c.EmailAddressList)
                                {
                                    sb.AppendFormat("<li><a href='mailto:{0}'>{0}</a></li>", ea.Address);
                                }
                                sb.Append("</ul>");

                                litEmailList.Text = sb.ToString();

                            }
                            else
                            {
                                litEmailList.Text = "(None Entered)";
                            }

                            blPhone.Items.Clear();
                            blPhone.DataSource = c.PhoneList;
                            blPhone.DataTextField = "PhoneNumberWithType";
                            blPhone.DataValueField = "PhoneID";
                            blPhone.DataBind();

                            if (blPhone.Items.Count == 0)
                            {
                                blPhone.Items.Add(new ListItem("(None Entered)"));
                            }

                            //Added phone number, Country Code and Extention 
                            blCountry.Items.Clear();
                            blCountry.DataSource = c.PhoneList;
                            blCountry.DataTextField = "PhoneCountryCode";
                            blCountry.DataValueField = "PhoneID";
                            blCountry.DataBind();

                            if (blCountry.Items.Count == 0)
                            {
                                blCountry.Items.Add(new ListItem("(None Entered)"));
                            }

                            blExt.Items.Clear();
                            blExt.DataSource = c.PhoneList;
                            blExt.DataTextField = "PhoneExt";
                            blExt.DataValueField = "PhoneID";
                            blExt.DataBind();

                            if (blExt.Items.Count == 0)
                            {
                                blExt.Items.Add(new ListItem("(None Entered)"));
                            }


                            ddlName.SelectedValue = c.ContactID.ToString();

                            // if coming here for the first time, the we just select the first one. once we select it, set to -1 so we dont keep populating.
                            already_selected = true;

                        }

                    }
                }
            }
            else
            {
                ddlName.Visible = false;
            }


        }

        public void FormShow(string ID, bool UseVS)
        {
            lblError.Text = string.Empty;

            // only show notification emails for merchants
            tabNotificationEmails.Visible = (ControlContactType == eControlContactType.Merchant) ? true : false;

            if (UserSessions.CurrentMerchantApp != null && this.Adding == false && this.ControlContactType == eControlContactType.Merchant)
            {
                NotificationEmails.Text = UserSessions.CurrentMerchantApp.NotificationEmails;
                CheckBoxRDRVerifi.Checked = UserSessions.CurrentMerchantApp.DisableRDRVerifi;

                string[] emailsUsers = Constants.DISABLE_RDRVERIFI_USERS.Split(Constants.Delimters, StringSplitOptions.RemoveEmptyEntries);

                if (emailsUsers.Contains(UserSessions.CurrentUser.Email))
                {
                    CheckBoxRDRVerifi.Visible = true;
                    lbDisableRDRVerifi.Visible = true;
                }
            }

            FillListFromContactType(ddlTitle);

            int contact_id = 0;

            if (!string.IsNullOrEmpty(ID))
            {
                contact_id = Convert.ToInt32(ID);
            }

            pnlEdit.Visible = this.EditMode;
            pnlView.Visible = !this.EditMode;

            List<Contact> liC = null;

            if (UseVS && this.VSContact != null)
            {
                liC = this.VSContact;
            }
            else
            {
                liC = this.GetVSContactFromDB();

                // save to the view state for edits
                this.VSContact = liC;

                // since we pulled from the database, we want to save a cloned copy.
                if (liC != null && liC.Count > 0)
                {
                    foreach (Contact c in liC)
                    {
                        c.Clone();
                    }
                }

            }

            if (this.EditMode)
            {
                this.FormShowInEditMode(contact_id, liC);
            }
            else
            {
                this.FormShowInViewMode(contact_id, liC);
            }

            if (this.VSContact != null)
            {

                if (this.VSContact.Count == 0 || this.VSContact.Count == 1)
                {
                    lbRemoveContact.Visible = false;
                }
                else
                {
                    lbRemoveContact.Visible = true;
                }
            }

            trLitAccessLevel.Visible = false;
            trDdlAccessLevel.Visible = false;
            switch (this.ControlContactType)
            {
                case eControlContactType.Agent:
                    litTitle.Text = "Agent Contact";
                    break;

                case eControlContactType.Merchant:
                    litTitle.Text = "Merchant Contact";
                    trLitAccessLevel.Visible = true;
                    trDdlAccessLevel.Visible = true;
                    gvEmail.ShowFooter = false;
                    break;

                case eControlContactType.Lead:
                    litTitle.Text = "Lead Contact";
                    break;

                default:
                    litTitle.Text = "Contact";
                    break;
            }

        }

        protected void gvPhone_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Phone p = (Phone)e.Row.DataItem;
                DropDownList ddlPT = (DropDownList)e.Row.FindControl("ddlPhoneType");
                ddlPT.SelectedValue = Convert.ToString((int)p.PhoneType);

                WebMaskEditor tb = (WebMaskEditor)e.Row.FindControl("tbPhoneNumber");

                //Added for Validation for phone number, Country Code and Extention 
                DropDownList dpCountryCode = (DropDownList)e.Row.FindControl("ContactPhoneCountryCode");
                TextBox Countrytxt = (TextBox)e.Row.FindControl("ContactCountryCodeDisplay");
                Countrytxt.Text = dpCountryCode.SelectedValue;
                Countrytxt.ReadOnly = true;

                if (this.officeID == (int)CommonUtility.Util.Offices.Irvine)
                {
                    string strtbtxt = CommonUtility.Util.GetNumbersFromString(tb.Value.ToString());
                    tb.Value = strtbtxt.ToString();
                    tb.InputMask = "000-000-0000";
                }
                else
                {
                    tb.InputMask = "############";
                }

                LookupTableHandler.LoadCountryCallingCodes(dpCountryCode);

                if (p.PhoneCountryCode == "" || p.PhoneCountryCode == null)
                {
                    ListHandler.ListFindItem(dpCountryCode, "+1");
                    Countrytxt.Text = "+1";
                }
                else
                {
                    ListHandler.ListFindItem(dpCountryCode, p.PhoneCountryCode.ToString().Trim());
                    Countrytxt.Text = p.PhoneCountryCode.ToString().Trim();
                }

                WebMaskEditor Exttxt = (WebMaskEditor)e.Row.FindControl("ContactPhoneExt");
                Exttxt.Text = p.PhoneExt.ToString().Trim();

            }
        }

        protected void wucContact_PreRender(object sender, EventArgs e)
        {
            // ensures that the ddl contacts will always be selectable.
            if (pnlView.Visible)
            {
                ddlName.Enabled = true;
            }
        }

        /// <summary>
        /// clear the inputs on the screen 
        /// </summary>
        public void FormClearScreen()
        {
            tbFirstname.Text = "";
            tbLastname.Text = "";
            tbMiddlename.Text = "";
            cbIsPrimary.Checked = false;
            ddlTitle.SelectedIndex = 0;  // set to first one
            ddlAccessLevel.SelectedIndex = 0;
            this.NotificationEmails.Text = "";

            // do not clear this.
            //this.PreviousContactID = -1;

            // clear dropdown to make sure we're not saving under a different contactid.
            ddlContacts.Items.Clear();

            // clear gv
            gvEmail.DataSource = null;
            gvEmail.DataBind();

            gvPhone.DataSource = null;
            gvPhone.DataBind();
        }

        /// <summary>
        /// clean everything involved with this control
        /// </summary>
        public override void FormClear()
        {
            this.FormClearScreen();

            this.VSContact = null;
            this.ObjectID = 0;

        }

        /// <summary>
        /// force all other contacts to be non-primary except what's specified.
        /// </summary>
        /// <param name="contact_id"></param>
        private void RemovePrimaryExcept(int contact_id)
        {
            foreach (Contact c in this.VSContact)
            {
                if (c.ContactID != contact_id)
                {
                    c.IsPrimary = false;
                }
            }
        }

        /// <summary>
        /// save the info to the VS, (not the DB)
        /// </summary>
        /// <returns></returns>
        public bool FormSaveVS()
        {

            bool ret = false;

            bool set_is_primary = false;

            int new_contact_id = -1;

            foreach (Contact c in this.VSContact)
            {
                if (c.ContactID == -1)
                {
                    c.ContactID = this.GetTempID();

                    // keep the contactid, incase this is set as primary.
                    new_contact_id = c.ContactID;
                    set_is_primary = c.IsPrimary;

                    ret = true;

                    break;
                }


            }

            // force all the other contacts to not be primary.
            if (set_is_primary)
            {
                this.RemovePrimaryExcept(new_contact_id);
            }

            return ret;
        }

        /// <summary>
        /// save VS to the Database.
        /// </summary>
        /// <returns></returns>
        public override bool FormSave()
        {

            bool ret = false;

            // handles the were the viewstate is not update to date yet. ie: edit, change, save.
            this.SaveInfoFromScreen();

            if (this.lblError.Text.Length == 0)
            {
                List<Contact> liC = (List<Contact>)ViewState["VSContact"];

                this.ForceOnlyOnePrimary(liC);

                if (this.ObjectID > 0 && liC != null && liC.Count > 0)
                {
                    foreach (Contact c in liC)
                    {

                        if (c.IsValid())
                        {
                            switch (this.ControlContactType)
                            {
                                case eControlContactType.Merchant:

                                    if (c.ContactID < 0)
                                    {
                                        DataContact.InsertMerchantContact(this.ObjectID, c);
                                    }
                                    else
                                    {
                                        this.UpdateWithChangeLogMerchant(this.ObjectID, c);
                                    }

                                    break;

                                case eControlContactType.Lead:


                                    if (c.ContactID < 0)
                                    {
                                        DataContact.InsertLeadContact(this.ObjectID, c);
                                    }
                                    else
                                    {
                                        this.UpdateWithChangeLogLead(this.ObjectID, c);

                                    }

                                    break;

                                case eControlContactType.Agent:

                                    if (c.ContactID < 0)
                                    {
                                        DataContact.InsertAgentContact(this.ObjectID, c);
                                    }
                                    else
                                    {
                                        this.UpdateWithChangeLogAgent(this.ObjectID, c);
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private bool UpdateWithChangeLogMerchant(int MerchantID, Contact c)
        {
            bool ret = true;
            try
            {
                string DifferenceLog = "";

                // get differences before you save.
                if (c.ContactID > 0 && c.ContactClone != null)
                {
                    DifferenceLog = c.GetDifferences();
                }

                // attempt to save.. will throw exception if fail
                DataContact.UpdateMerchantContact(this.ObjectID, c);

                // no exception, then save if there's any differences
                if (!string.IsNullOrEmpty(DifferenceLog))
                {
                    DataUser.GetInstance().InsertChangeLog(UserSessions.CurrentMerchantApp.BusinessDBAName
                        , UserSessions.CurrentUser.UserName
                        , UserSessions.CurrentMerchantApp.MerchantAppUID
                        , c.ContactID
                        , this.ControlContactType.ToString() + "Contact"
                        , DifferenceLog
                        , UserSessions.PortalUID
                        );
                }

            }
            //catch (Exception ex)
            catch
            {
                // todo: swallowing exception right now... 
                ret = false;
            }

            return ret;
        }

        private bool UpdateWithChangeLogAgent(int AgentID, Contact c)
        {
            bool ret = true;
            try
            {
                string DifferenceLog = "";

                // get differences before you save.
                if (c.ContactID > 0 && c.ContactClone != null)
                {
                    DifferenceLog = c.GetDifferences();
                }

                // attempt to save.. will throw exception if fail
                DataContact.UpdateAgentContact(this.ObjectID, c);

                // no exception, then save if there's any differences
                if (!string.IsNullOrEmpty(DifferenceLog))
                {
                    DataUser.GetInstance().InsertChangeLog(UserSessions.CurrentAgent.AgentDBA
                        , UserSessions.CurrentUser.UserName
                        , UserSessions.CurrentAgent.AgentUID
                        , c.ContactID
                        , this.ControlContactType.ToString() + "Contact"
                        , DifferenceLog
                        , UserSessions.PortalUID
                        );
                }

            }
            //catch (Exception ex)
            catch
            {
                // todo: swallowing exception right now... 
                ret = false;
            }

            return ret;
        }

        private bool UpdateWithChangeLogLead(int LeadID, Contact c)
        {
            bool ret = true;
            try
            {
                string DifferenceLog = "";

                // get differences before you save.
                if (c.ContactID > 0 && c.ContactClone != null)
                {
                    DifferenceLog = c.GetDifferences();
                }

                // attempt to save.. will throw exception if fail
                DataContact.UpdateLeadContact(this.ObjectID, c);

                // no exception, then save if there's any differences
                if (!string.IsNullOrEmpty(DifferenceLog))
                {
                    DataUser.GetInstance().InsertChangeLog(UserSessions.CurrentLead.DBAName
                        , UserSessions.CurrentUser.UserName
                        , UserSessions.CurrentLead.LeadUID
                        , c.ContactID
                        , this.ControlContactType.ToString() + "Contact"
                        , DifferenceLog
                        , UserSessions.PortalUID
                        );
                }

            }
            //catch (Exception ex)
            catch
            {
                // todo: swallowing exception right now... 
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// ensures that at most, and at minimum, there is always 1 that is primary.
        /// </summary>
        /// <param name="liC"></param>
        private void ForceOnlyOnePrimary(List<Contact> liC)
        {
            List<int> is_primary_list = new List<int>();

            if (this.ObjectID > 0 && liC != null && liC.Count > 0)
            {
                foreach (Contact c in liC)
                {
                    // set to true if we saw at least 1 primary.
                    if (c.IsPrimary)
                    {
                        is_primary_list.Add(c.ContactID);
                    }
                }

                if (is_primary_list.Count == 0)
                {
                    // no primary countact count. set the first one as primary.
                    liC[0].IsPrimary = true;
                }
                else if (is_primary_list.Count > 0)
                {
                    // multiple primarys found.. yikes. default to the first one.
                    int primary_contact_id = is_primary_list[0];
                    this.RemovePrimaryExcept(primary_contact_id);
                }
            }
        }

        /// <summary>
        /// new contact for new merchant
        /// </summary>
        public override void FormNew()
        {
            if (this.Adding == true)
            {
                this.FormClear();
                this.FormNewVS();


            }
        }

        /// <summary>
        /// new contact form with VS
        /// </summary>
        public void FormNewVS()
        {
            if (this.EditMode)
            {

                trContacts.Visible = false;

                lbAddNewContact.Visible = false;

                // if we're adding for the first time, these buttons don't make sense.
                // they should only appear in edit mode.
                lbSave.Visible = !this.Adding;
                lbCancel.Visible = !this.Adding;


                cbIsPrimary.Checked = false;


                gvEmail.DataSource = this.GetEmailBoxes(1); 
                gvEmail.DataBind();

                gvPhone.DataSource = this.GetPhoneBoxes(3);
                gvPhone.DataBind();

                Contact c = new Contact();
                c.ContactID = this.GetTempID();
                c.ContactType = ContactType.Owner;

                if (this.VSContact == null)
                {
                    this.VSContact = new List<Contact>();
                }

                this.VSContact.Add(c);


                this.hidNewContactID.Value = c.ContactID.ToString();

            }
        }

        private List<EmailAddress> GetEmailBoxes(int count)
        {
            List<EmailAddress> li = null;
            if (count > 0)
            {
                li = new List<EmailAddress>();

                for (int i = 0; i < count; i++)
                {
                    EmailAddress ea = new EmailAddress();
                    ea.Type = EmailAddressTypes.To;
                    ea.EmailAddressID = this.GetTempID();
                    li.Add(ea);
                }

            }

            return li;
        }

        private List<Phone> GetPhoneBoxes(int count)
        {
            List<Phone> li = null;
            if (count > 0)
            {
                li = new List<Phone>();

                for (int i = 0; i < count; i++)
                {
                    Phone ea = new Phone();
                    ea.PhoneID = this.GetTempID();
                    ea.PhoneType = PhoneTypes.Cell;
                    li.Add(ea);
                }

            }

            return li;
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// only validates with current on screen values.
        /// </summary>
        /// <returns></returns>
        public override bool FormDataCheck()
        {
            this.FormDataCheckVS();

            return (lblError.Text.Trim().Length == 0) ? true : false;
        }

        // performs a check in editmode to make sure we have everything we need before we write to the VS
        // check is performed with onscreen values.       
        private bool FormDataCheckVS()
        {
            //Added Validation for phone number, Country Code and Extention 
            if (gvPhone.Rows != null && gvPhone.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvPhone.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        WebMaskEditor tb = (WebMaskEditor)gvr.FindControl("tbPhoneNumber");
                        DropDownList dpCountryCode = (DropDownList)gvr.FindControl("ContactPhoneCountryCode");
                        TextBox cd = (TextBox)gvr.FindControl("ContactCountryCodeDisplay");

                        cd.Text = dpCountryCode.SelectedValue;

                        if (this.officeID == (int)CommonUtility.Util.Offices.Irvine)
                        {
                            string strtbtxt = CommonUtility.Util.GetNumbersFromString(tb.Value.ToString());
                            tb.Value = strtbtxt.ToString();
                            tb.InputMask = "000-000-0000";
                        }
                        else
                        {
                            tb.InputMask = "############";
                        }
                    }
                }
            }

            //// check to make sure at least firstname is populated.

            //List<string> liError = new List<string>();

            //foreach (string err in this.ValidateContactOnScreen())
            //{
            //    liError.Add(err);
            //}

            //if (liError.Count > 0)
            //{
            //    lblError.Text = CommonUtility.Util.implode(liError, ", ");
            //}
            //else
            //{
            //    lblError.Text = string.Empty;
            //}

            //return (liError.Count > 0) ? false : true;

            return true;
        }

        public IEnumerable ValidateContactOnScreen()
        {
            if (string.IsNullOrEmpty(tbFirstname.Text))
            {
                yield return "First name required";
            }

        }

        /// <summary>
        /// this performs a cancel when they're in edit mode. this is not intended to be called from the parent page.
        /// </summary>
        public override void FormCancel()
        {
            if (this.EditMode)
            {
                trContacts.Visible = true;

                lbSave.Visible = false;
                lbCancel.Visible = false;
                lbAddNewContact.Visible = true;


                // we added a new contact in the VS when we added, now we're removing it.

                int del_index = 0;
                bool has_found = false;
                foreach (Contact c in this.VSContact)
                {
                    if (c.ContactID < 0)
                    {
                        has_found = true;
                        break;
                    }
                    else
                    {
                        ++del_index;
                    }

                }

                if (has_found)
                {
                    this.VSContact.RemoveAt(del_index);
                }


            }

        }

        public override void ToggleButtons()
        {


            pnlView.Visible = !this.EditMode;
            pnlEdit.Visible = this.EditMode;

            // toggle buttons, but still using the VS values
            this.FormShow(ddlName.SelectedValue, true);

            if (this.EditMode)
            {
                if (ddlContacts != null && ddlContacts.Items.Count > 0 && CommonUtility.Util.IsValidInt32(ddlContacts.SelectedValue))
                {
                    this.PreviousContactID = Convert.ToInt32(ddlContacts.SelectedValue);
                }
            }

        }

        /// <summary>
        /// in non-edit mode (aka read only), change the contact on the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.FormShow(ddl.SelectedValue, true);
        }

        /// <summary>
        /// in edit mode, change the contact in the ddl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlContacts_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList ddl = (DropDownList)sender;

            // save what's on the screen into VS
            this.SaveInfoFromScreen(this.PreviousContactID);

            // use VS to populate form
            this.FormShow(ddlContacts.SelectedValue, true);

            this.PreviousContactID = Convert.ToInt32(ddlContacts.SelectedValue);
        }

        /// <summary>
        /// save this contact information to what's in the Contact Dropdownlist.
        /// </summary>
        /// <returns></returns>
        public Contact SaveInfoFromScreen()
        {
            return SaveInfoFromScreen(0);
        }

        /// <summary>
        /// which contact do you want to save the screen information to?
        /// </summary>
        /// <param name="contact_id"></param>
        /// <returns></returns>
        public Contact SaveInfoFromScreen(int contact_id)
        {
            Contact myC = null;

            int main_index = 0;

            if (contact_id == 0 || contact_id == -1)
            {
                if (CommonUtility.Util.IsValidInt32(ddlContacts.SelectedValue))
                {
                    contact_id = Convert.ToInt32(ddlContacts.SelectedValue);
                }
                else if (CommonUtility.Util.IsValidInt32(hidNewContactID.Value) && hidNewContactID.Value.StartsWith("-"))
                {
                    contact_id = Convert.ToInt32(hidNewContactID.Value);
                }
            }

            if (this.VSContact != null)
            {
                foreach (Contact c in this.VSContact)
                {
                    if (c.ContactID == contact_id)
                    {
                        myC = c;
                        break;
                    }

                    ++main_index;
                }
            }

            if (myC != null)
            {
                if (this.FormDataCheckVS())
                {
                    //PXP-6771 Fady Massoud 10/05/2018
                    myC.FirstName = tbFirstname.Text.Trim();
                    myC.LastName = tbLastname.Text.Trim();
                    myC.MiddleName = tbMiddlename.Text.Trim();
                    myC.ContactType = (ContactType)Convert.ToInt32(ddlTitle.SelectedValue);
                    myC.AccessLevel = (ContactAccessLevel)Convert.ToInt32(ddlAccessLevel.SelectedValue); //PXP-2867 Rohit Thakur
                    myC.IsPrimary = cbIsPrimary.Checked;
                    myC.EmailAddressList = this.GetEmailListFromGV();
                    myC.PhoneList = this.GetPhoneListFromGV();

                    this.VSContact[main_index] = myC;

                    if (myC.IsPrimary)
                    {
                        this.RemovePrimaryExcept(myC.ContactID);
                    }
                }
            }



            return myC;
        }

        /// <summary>
        /// Generates a complete phone list from the gridview
        /// </summary>
        /// <returns></returns>
        private List<Phone> GetPhoneListFromGV()
        {
            List<Phone> liP = new List<Phone>();

            if (gvPhone.Rows != null && gvPhone.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvPhone.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        WebMaskEditor tb = (WebMaskEditor)gvr.FindControl("tbPhoneNumber");
                        HiddenField hf = (HiddenField)gvr.FindControl("hidPhoneID");
                        DropDownList ddl = (DropDownList)gvr.FindControl("ddlPhoneType");

                        //Added Validation for phone number, Country Code and Extention 
                        DropDownList dpCountryCode = (DropDownList)gvr.FindControl("ContactPhoneCountryCode");
                        TextBox cd = (TextBox)gvr.FindControl("ContactCountryCodeDisplay");
                        WebMaskEditor cp = (WebMaskEditor)gvr.FindControl("ContactPhoneExt");

                        cd.Text = dpCountryCode.SelectedValue.ToString().Trim();

                        if (UserSessions.CurrentMerchantApp != null)
                        {
                            if (this.officeID == (int)CommonUtility.Util.Offices.Irvine)
                            {
                                string strtbtxt = CommonUtility.Util.GetNumbersFromString(tb.Value.ToString());
                                tb.Value = strtbtxt.ToString();
                                tb.InputMask = "000-000-0000";
                            }
                            else
                            {
                                tb.InputMask = "############";
                            }
                        }

                        string myphone = tb.Text.ToString().Trim();

                        string countrycode = dpCountryCode.SelectedValue.ToString().Trim();
                        string phonextn = cp.Text.ToString();


                        int myid = Convert.ToInt32(hf.Value);
                        int myptid = Convert.ToInt32(ddl.SelectedValue);

                        Phone p = new Phone();
                        p.PhoneType = (PhoneTypes)myptid;
                        p.PhoneNumber = myphone.Trim();
                        if (myid > 0)
                        {
                            p.PhoneID = myid;
                        }
                        p.PhoneCountryCode = countrycode.Trim();
                        p.PhoneExt = phonextn.Trim();
                        if (!string.IsNullOrEmpty(CommonUtility.Util.GetNumbersFromString(p.PhoneNumber)))
                            liP.Add(p);

                    }
                }
            }

            return liP;
        }

        /// <summary>
        /// generates a complete email list from the gridview
        /// </summary>
        /// <returns></returns>
        private List<EmailAddress> GetEmailListFromGV()
        {
            List<EmailAddress> liE = new List<EmailAddress>();

            if (gvEmail.Rows != null && gvEmail.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvEmail.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {

                        TextBox tb = (TextBox)gvr.FindControl("tbEmailAddress");
                        HiddenField hf = (HiddenField)gvr.FindControl("hidEmailAddressID");

                        string myemail = tb.Text.Trim();
                        int myid = Convert.ToInt32(hf.Value);

                        if (!string.IsNullOrEmpty(myemail) && CommonUtility.Util.IsValidEmail(myemail))
                        {
                            EmailAddress ea = new EmailAddress();
                            ea.Address = myemail;
                            ea.Type = EmailAddressTypes.To;
                            if (myid > 0)
                            {
                                ea.EmailAddressID = myid;
                            }
                            liE.Add(ea);
                        }
                    }
                }
            }

            return liE;

        }

        protected void lbAddNewContact_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.SaveInfoFromScreen();

                // clear visible inputs.
                this.FormClearScreen();

                this.FormNewVS();
            }

        }

        protected void lbSave_Click(object sender, EventArgs e)
        {

            // save what's on the screen into VS for the new contact.
            this.SaveInfoFromScreen(-1);

            this.FormSaveVS();

            this.FormClearScreen();

            // pull info from VS
            this.FormShow("", true);


            trContacts.Visible = true;

            lbAddNewContact.Visible = true;

            lbSave.Visible = false;
            lbCancel.Visible = false;


        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                this.FormCancel();

                this.FormShow("", true);


            }
        }

        protected void gvEmail_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.SaveInfoFromScreen();

            GridViewRow row = (GridViewRow)gvEmail.Rows[e.RowIndex];

            // pull info from row
            HiddenField hf = (HiddenField)row.FindControl("hidEmailAddressID");
            int email_address_id = Convert.ToInt32(hf.Value);



            // locare our record in the contact 
            EmailAddress ea_remove = null;
            int contact_index = 0;

            int current_contact = 0;

            foreach (Contact c in this.VSContact)
            {
                if (CommonUtility.Util.IsValidInt32(ddlContacts.SelectedValue))
                {
                    current_contact = Convert.ToInt32(ddlContacts.SelectedValue);
                }
                else if (CommonUtility.Util.IsValidInt32(hidNewContactID.Value))
                {
                    current_contact = Convert.ToInt32(hidNewContactID.Value);
                }

                if (c.ContactID == current_contact)
                {
                    if (c.EmailAddressList != null && c.EmailAddressList.Count > 0)
                    {

                        foreach (EmailAddress ea in c.EmailAddressList)
                        {
                            if (ea.EmailAddressID == email_address_id)
                            {
                                ea_remove = ea;
                                break;
                            }
                        }

                    }
                }

                if (ea_remove != null)
                {
                    break;
                }

                contact_index++;


            }

            if (ea_remove != null)
            {
                // remove it from our VS
                this.VSContact[contact_index].EmailAddressList.Remove(ea_remove);
            }

            // rebind with correct list.
            foreach (Contact c in this.VSContact)
            {
                if (c.ContactID == current_contact)
                {
                    gvEmail.DataSource = c.EmailAddressList;
                    gvEmail.DataBind();
                }
            }
        }

        protected void gvPhone_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            this.SaveInfoFromScreen();
            GridViewRow row = (GridViewRow)gvPhone.Rows[e.RowIndex];
            // pull info from row
            HiddenField hf = (HiddenField)row.FindControl("hidPhoneID");
            int phone_id = Convert.ToInt32(hf.Value);
            // locare our record in the contact 
            Phone ea_remove = null;
            int contact_index = 0;

            int current_contact = 0;



            foreach (Contact c in this.VSContact)
            {
                if (CommonUtility.Util.IsValidInt32(ddlContacts.SelectedValue))
                {
                    current_contact = Convert.ToInt32(ddlContacts.SelectedValue);
                }
                else if (CommonUtility.Util.IsValidInt32(hidNewContactID.Value))
                {
                    current_contact = Convert.ToInt32(hidNewContactID.Value);
                }

                if (c.ContactID == current_contact)
                {
                    if (c.PhoneList != null && c.PhoneList.Count > 0)
                    {

                        foreach (Phone ea in c.PhoneList)
                        {
                            if (ea.PhoneID == phone_id)
                            {
                                ea_remove = ea;
                                break;
                            }
                        }

                    }
                }

                if (ea_remove != null)
                {
                    break;
                }

                contact_index++;
            }

            if (ea_remove != null)
            {
                // remove it from our VS
                this.VSContact[contact_index].PhoneList.Remove(ea_remove);
            }



            // rebind with correct list.
            foreach (Contact c in this.VSContact)
            {
                if (c.ContactID == current_contact)
                {
                    gvPhone.DataSource = c.PhoneList;
                    gvPhone.DataBind();
                }
            }
        }

        protected void ContactPhoneCountryCode_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvPhone.Rows != null && gvPhone.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvPhone.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList dpCountryCode = (DropDownList)gvr.FindControl("ContactPhoneCountryCode");
                        TextBox cd = (TextBox)gvr.FindControl("ContactCountryCodeDisplay");

                        cd.Text = dpCountryCode.SelectedValue.ToString().Trim();
                    }
                }
            }
        }

        protected void lbAddNewPhone_Click(object sender, EventArgs e)
        {
            this.SaveInfoFromScreen();

            List<Phone> liPhone = this.GetPhoneListFromGV();

            Phone p = new Phone();
            p.PhoneNumber = "";
            p.PhoneType = PhoneTypes.Cell;
            p.PhoneID = this.GetTempID();
            liPhone.Add(p);

            gvPhone.DataSource = liPhone;
            gvPhone.DataBind();
        }

        protected void lbAddNewEmail_Click(object sender, EventArgs e)
        {

            this.SaveInfoFromScreen();

            List<EmailAddress> liEmail = this.GetEmailListFromGV();

            EmailAddress p = new EmailAddress();
            p.Address = "";
            p.Type = EmailAddressTypes.To;
            p.EmailAddressID = this.GetTempID();
            liEmail.Add(p);

            gvEmail.DataSource = liEmail;
            gvEmail.DataBind();

        }

        protected void lbRemoveContact_Click(object sender, EventArgs e)
        {
            int contact_id = Convert.ToInt32(ddlContacts.SelectedValue);

            foreach (Contact c in this.VSContact)
            {
                if (c.ContactID == contact_id)
                {
                    c.IsActive = false;
                    break;
                }
            }

            this.FormShow("", true);

        }

        /// <summary>
        /// returns a random id, in the negative range for use with temporary ids.
        /// </summary>
        /// <returns></returns>
        private int GetTempID()
        {
            Random random = new Random();
            int randomNumber = random.Next(10, 99999999);
            return -1 * randomNumber;
        }

        public static void FillListFromContactType(ListControl myList)
        {
            myList.Items.Clear();

            Array itemValues = System.Enum.GetValues(typeof(ContactType));
            Array itemNames = System.Enum.GetNames(typeof(ContactType));

            for (int i = 0; i <= itemNames.Length - 1; i++)
            {
                ListItem item = new ListItem();
                item.Text = CommonUtility.Util.GetEnumDescription((ContactType)Convert.ToInt32((itemValues as int[])[i]));
                item.Value = Convert.ToInt32((itemValues as int[])[i]).ToString();

                myList.Items.Add(item);
            }
        }

        private void SaveLog(string DifferenceLog)
        {
            if (!string.IsNullOrEmpty(DifferenceLog))
            {
                DataUser.GetInstance().InsertChangeLog(UserSessions.CurrentMerchantApp.BusinessDBAName
                    , UserSessions.CurrentUser.UserName
                    , UserSessions.CurrentMerchantApp.MerchantAppUID
                    , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                    , this.ControlContactType.ToString() + "Contact"
                    , DifferenceLog
                    , UserSessions.PortalUID
                    );
            }
        }

        /// <summary>
        /// then we pull info from the screen. writes it to VS
        /// </summary>
        public void FromScreenToVS()
        {
            // create a new object first, then bind to the viewstate.
            Contact objCN = new Contact();
            objCN.ContactID = -1;
            objCN.IsPrimary = true;
            //PXP-6771 Fady Massoud 10/05/2018
            objCN.FirstName = tbFirstname.Text;
            objCN.MiddleName = tbMiddlename.Text;
            objCN.LastName = tbLastname.Text;
            objCN.ContactType = (ContactType)Convert.ToInt32(ddlTitle.SelectedValue);
            objCN.AccessLevel = (ContactAccessLevel)Convert.ToInt32(ddlAccessLevel.SelectedValue); //PXP-2867 Rohit Thakur
            objCN.EmailAddressList = this.GetEmailListFromGV();
            objCN.PhoneList = this.GetPhoneListFromGV();

            this.VSContact = new List<Contact>();
            this.VSContact.Add(objCN);
        }

        /// <summary>
        /// used from the merchant profile -> copy function. only copies the primary contact.
        /// </summary>
        /// <param name="liOrig"></param>
        public void CloneContactsForEdit(List<Contact> liOrig)
        {
            if (liOrig != null && liOrig.Count > 0)
            {
                foreach (Contact c in liOrig)
                {
                    if (c.IsPrimary)
                    {
                        // bind info to the screen.
                        cbIsPrimary.Checked = c.IsPrimary;
                        //PXP-6771 Fady Massoud 10/05/2018
                        tbFirstname.Text = c.FirstName;
                        tbMiddlename.Text = c.MiddleName;
                        tbLastname.Text = c.LastName;
                        ddlTitle.SelectedValue = Convert.ToString((int)c.ContactType);
                        ddlAccessLevel.SelectedValue = Convert.ToString((int)c.AccessLevel); //PXP-2867 Rohit Thakur

                        hidNewContactID.Value = "0";

                        gvEmail.DataSource = c.EmailAddressList;
                        gvEmail.DataBind();

                        gvPhone.DataSource = c.PhoneList;
                        gvPhone.DataBind();


                        // create a new object first, then bind to the viewstate.
                        Contact objCN = new Contact();
                        objCN.ContactID = -1;
                        objCN.IsPrimary = true;
                        //PXP-6771 Fady Massoud 10/05/2018
                        objCN.FirstName = c.FirstName;
                        objCN.MiddleName = c.MiddleName;
                        objCN.LastName = c.LastName;
                        objCN.ContactType = c.ContactType;
                        objCN.EmailAddressList = c.EmailAddressList;
                        objCN.PhoneList = c.PhoneList;


                        this.VSContact = new List<Contact>();
                        this.VSContact.Add(objCN);

                        break;
                    }
                }
            }

        }


        /// <summary>
        /// validates values on screen in edit mode
        /// </summary>
        /// <returns></returns>
        public List<string> FormDataCheck(MerchantApp app)
        {
            List<string> liError = new List<string>();

            if (gvPhone.Rows != null && gvPhone.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvPhone.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        WebMaskEditor tb = (WebMaskEditor)gvr.FindControl("tbPhoneNumber");
                        WebMaskEditor tbContactPhoneExt = (WebMaskEditor)gvr.FindControl("ContactPhoneExt");


                        //string myphone = tb.Text.ToString().Replace("-", "").Trim();
                        string myphone = CommonUtility.Util.GetNumbersFromString(tb.Text);
                        string strContactPhoneExt = CommonUtility.Util.GetNumbersFromString(tbContactPhoneExt.Text);

                        if (!string.IsNullOrEmpty(myphone))
                        {
                            if (app.Office == CommonUtility.Util.Offices.Irvine)
                            {
                                if ((myphone.Length != 10))
                                {
                                    liError.Add("Please enter 10 digit Merchant contact number.");
                                }
                            }
                            if (myphone.Length > 12)
                            {
                                liError.Add("Please enter at the max 12 digits for merchant contact numbers.");
                            }
                        }

                        if (strContactPhoneExt.Length > 0 && string.IsNullOrWhiteSpace(myphone))
                        {
                            liError.Add("Please enter Merchant Contact phone number for Ext# " + strContactPhoneExt + ".");
                        }
                    }
                }
            }

            return liError;
        }

        public void PhoneValidations()
        {
            //Added Validation for phone number, Country Code and Extention 
            if (gvPhone.Rows != null && gvPhone.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in gvPhone.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        WebMaskEditor tb = (WebMaskEditor)gvr.FindControl("tbPhoneNumber");
                        DropDownList dpCountryCode = (DropDownList)gvr.FindControl("ContactPhoneCountryCode");
                        TextBox cd = (TextBox)gvr.FindControl("ContactCountryCodeDisplay");

                        cd.Text = dpCountryCode.SelectedValue;

                        if (this.officeID == (int)CommonUtility.Util.Offices.Irvine)
                        {
                            string strtbtxt = CommonUtility.Util.GetNumbersFromString(tb.Value.ToString());
                            tb.Value = strtbtxt.ToString();
                            tb.InputMask = "000-000-0000";
                        }
                        else
                        {
                            tb.InputMask = "############";
                        }
                    }
                }
            }
        }


    }
}
