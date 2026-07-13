using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;


public partial class wucAlertContacts : System.Web.UI.UserControl
{
    public delegate void UpdateContactAlerts(int alertId, List<int> currentContacts, List<string> updatedContacts, string contactEmails, bool applyAll);

    /// <summary>
    /// this event gets called when user clicks the OK button to complete
    /// select contact emails to configure for an alert. the page/control
    /// should subscribe to this event to handle the updated contact email 
    /// selection
    /// </summary>
    public event UpdateContactAlerts UpdateContacts;

    #region Class Attributes

    public List<int> CurrentContacts
    {
        get
        {
            if (ViewState["CheckedContacts"] != null)
                return (List<int>)ViewState["CheckedContacts"];
            else
                ViewState["CheckedContacts"] = new List<int>();

            return (List<int>)ViewState["CheckedContacts"];
        }

        private set
        {
            ViewState["CheckedContacts"] = value;
        }
    }

    private eControlContactType AlertUserType
    {
        get
        {
            // NOTE: this will throw an exception if this key is not in the viewstate
            return (eControlContactType)ViewState["AlertUserType"];
        }

        set
        {
            ViewState["AlertUserType"] = value;
        }
    }

    public int AlertUserId
    {
        get
        {
            if (ViewState["AlertUserId"] == null)
                return 0;
            else
                return (int)ViewState["AlertUserId"];
        }

        set
        {
            ViewState["AlertUserId"] = value;
        }
    }

    protected SortDirection SortDirectionSearch
    {
        get
        {
            if (ViewState["SortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)ViewState["SortDirectionSearch"];
        }
        set { ViewState["SortDirectionSearch"] = value; }
    }

    protected string SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
                return string.Empty;
            else
                return ViewState["SortOrder"].ToString();
        }
        set { ViewState["SortOrder"] = value; }
    }

    protected Hashtable m_Prms
    {
        get
        {
            if (ViewState["m_Prms"] == null)
                return null;
            else
                return (Hashtable)ViewState["m_Prms"];
        }
        set { ViewState["m_Prms"] = value; }
    }

    private int AlertID
    {
        get
        {
            if (ViewState["AlertID"] == null)
                return 0;
            else
                return (int)ViewState["AlertID"];
        }

        set
        {
            ViewState["AlertID"] = value;
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void grdContacts_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.BindGrid();
    }

    protected void odsContacts_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
        e.InputParameters[3] = this.ID;
    }

    public void SetDataSource(eControlContactType userType, int userId, int alertId, int portalId, int pagesize, string currentList)
    {
        if (alertId != this.AlertID)
        {
            this.grdContacts.DataSourceID = "odsContacts";
            
            this.AlertUserType = userType;
            this.AlertUserId = userId;
            this.AlertID = alertId;

            Hashtable prms = new Hashtable();

            if (userType == eControlContactType.Merchant)
                prms.Add("@MerchantID", userId);
            else
                prms.Add("@AgentID", userId);

            this.m_Prms = prms;

            BindGrid();

            this.ClearAddContact();

            LoadEnabledContacts(new List<string>(currentList.Split(';')));
        }
    }

    private void LoadEnabledContacts(List<string> emailList)
    {
        foreach (GridViewRow row in this.grdContacts.Rows)
        {
            string contactEmailId = this.grdContacts.DataKeys[row.RowIndex].Values["ID"].ToString();

            if (emailList.Contains(contactEmailId))
            {
                CheckBox chkActive = (CheckBox)row.FindControl("chkEnabled");
                chkActive.Checked = true;

                int contactId = CommonUtility.Util.if_i(contactEmailId, 0);

                if (contactId > 0 && !this.CurrentContacts.Contains(contactId))
                    this.CurrentContacts.Add(contactId);
            }
        }
    }

    private void BindGrid()
    {
        this.odsContacts.SelectMethod = this.AlertUserType == eControlContactType.Merchant ? "GetMerchantContactsPaging" : "GetAgentContactsPaging";

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", "ID");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        this.grdContacts.DataBind();

        if (grdContacts.Rows.Count > 0)
            lblCount.Text = "** Total Number of the Contacts : " + grdContacts.Rows.Count;
        else
            lblCount.Text = "";
    }

    private int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = 0;
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }

        return newSortDirection;
    }

    protected void lnkOk_Click(object sender, EventArgs e)
    {
        ApplyContacts(false);
    }

    protected void btnAddContact_Click(object sender, EventArgs e)
    {
        //add new contact and then rebind grid
        if (!ValidateContact())
            return;

        try
        {
            Contact contact = new Contact();
            contact.FirstName = this.txtFirstName.Text;
            contact.LastName = this.txtLastName.Text;
            contact.ContactType = ContactType.Other;

            EmailAddress emailAddr = new EmailAddress() { Address = this.txtAddEmail.Text, Type = EmailAddressTypes.To };
            contact.EmailAddressList = new List<EmailAddress>() { emailAddr };
            contact.IsActive = true;
            contact.IsPrimary = false;
            int contactId = 0;

            if (this.AlertUserType == eControlContactType.Merchant)
                contactId = DataContact.InsertMerchantContact(this.AlertUserId, contact);
            else
                contactId = DataContact.InsertAgentContact(this.AlertUserId, contact);

            List<string> selected = this.SelectedContacts();

            BindGrid();

            CheckNewContact(contactId, selected);

            this.ClearAddContact();

            SetStatus(Color.Green, "Contact added.");
        }
        catch
        {
            SetStatus(Color.Red, "Failed to add contact.");
        }
    }

    private void ApplyContacts(bool applyAll)
    {
        string emails = "";
        List<string> newContacts = new List<string>();

        //get a list of contact email addresses and ids checkboxed
        foreach (GridViewRow row in this.grdContacts.Rows)
        {
            int contactEmailId = CommonUtility.Util.if_i(this.grdContacts.DataKeys[row.RowIndex].Values["ID"].ToString(), 0);

            if (contactEmailId > 0 && ((CheckBox)row.FindControl("chkEnabled")).Checked)
            {
                newContacts.Add(contactEmailId.ToString());
                emails += row.Cells[2].Text + ";";
            }
        }

        if (this.UpdateContacts != null)
            UpdateContacts(this.AlertID, this.CurrentContacts, newContacts, emails, applyAll);
    }

    private void ClearAddContact()
    {
        this.txtAddEmail.Text = "";
        this.txtFirstName.Text = "";
        this.txtLastName.Text = "";
    }

    private bool ValidateContact()
    {
        if (string.IsNullOrEmpty(this.txtFirstName.Text))
        {
            SetStatus(Color.Red, "First name required.");
            return false;
        }

        if (string.IsNullOrEmpty(this.txtLastName.Text))
        {
            SetStatus(Color.Red, "Last name required.");
            return false;
        }

        if (!CommonUtility.Util.IsValidEmail(this.txtAddEmail.Text))
        {
            SetStatus(Color.Red, "Valid email address required.");
            return false;
        }

        return true;
    }

    private void SetStatus(Color color, string message)
    {
        this.lblMessage.Text = message;
        this.lblMessage.ForeColor = color;
    }

    private List<string> SelectedContacts()
    {
        List<string> selected = new List<string>();

        foreach (GridViewRow row in this.grdContacts.Rows)
        {
            CheckBox chkEnable = (CheckBox)row.FindControl("chkEnabled");

            if (chkEnable.Checked)
                selected.Add(this.grdContacts.DataKeys[row.RowIndex].Values["ID"].ToString());
        }

        return selected;
    }

    private void CheckNewContact(int contactId, List<string> emailList)
    {
        foreach (GridViewRow row in this.grdContacts.Rows)
        {
            int id = CommonUtility.Util.if_i(this.grdContacts.DataKeys[row.RowIndex].Values["ContactID"].ToString(), 0);
            string contactEmailId = this.grdContacts.DataKeys[row.RowIndex].Values["ID"].ToString();

            if (contactId == id || emailList.Contains(contactEmailId))
            {
                CheckBox chkEnable = (CheckBox)row.FindControl("chkEnabled");
                chkEnable.Checked = true;
            }
        }
    }

    protected void btnApplyAll_Click(object sender, EventArgs e)
    {
        ApplyContacts(true);
    }

}
