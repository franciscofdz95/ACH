using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;


public partial class wucUserProfile : wucBaseSearch
{
    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["EditMode"]);
        }
        set { ViewState["EditMode"] = value; }
    }

    public bool Adding
    {
        get { return Convert.ToBoolean(ViewState["Adding"]); }
        set { ViewState["Adding"] = Convert.ToBoolean(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
            if (!this.IsPostBack)
            {

                //FormHandler.SetSecurity(this.Page);

                LoadTimeZones();
                LoadDateTimePatterns();
                LookupTableHandler.LoadOfficeQueueAccess(this.lstOfficeAccess); 

                this.FormShow();
            }
    }

    private void LoadTimeZones()
    {
        LookupTableHandler.LoadUserTimeZones(TimeZoneID, false);
    }
    
    private void LoadDateTimePatterns()
    {
        DatePattern.Items.Add(new ListItem("dd/MM/yyyy", "dd/MM/yyyy"));
        DatePattern.Items.Add(new ListItem("MM/dd/yyyy", "MM/dd/yyyy"));
        DatePattern.Items.Add(new ListItem("yyyy/MM/dd", "yyyy/MM/dd"));

        //TimePattern.Items.Add(new ListItem("HH:mm:ss", "HH:mm:ss"));
        TimePattern.Items.Add(new ListItem("24 Hour Format", "H:mm:ss"));
        //TimePattern.Items.Add(new ListItem("hh:mm:ss", "hh:mm:ss tt"));
        TimePattern.Items.Add(new ListItem("12 Hour Format (am/pm)", "h:mm:ss tt"));
    }

    protected void btnSave_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (this.FormSave())
        {
            Error.Text = "";
            Session.Clear();
            FormsAuthentication.SignOut();
            Response.Redirect("~/frmLogin.aspx");

        }
    }

    protected void btnEdit_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Error.Text = "";
        this.EditMode = true;
        this.FormShow();
    }

    protected void btnCancel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        Error.Text = "";
        FormCancel();
    }

    public void FormShow()
    {
        if (UserSessions.CurrentUser != null)
        {
            User user = UserSessions.CurrentUser;

            FormBinding.BindObjectToControls(user, pnlDetail);
            FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

            ListHandler.ListFindItem(TimeZoneID, Convert.ToString((int)user.TimeZone));

            Office.Text = user.OfficeName;
        }
        
        this.ToggleButtons();
    }

    public bool FormSave()
    {
        User user = UserSessions.CurrentUser;

        if (!FormDataCheck())
            return false;

        User user2 = UserSessions.CurrentUser;
        user.UserModified = user2.UserName;

        FormBinding.BindControlsToObject(user, pnlDetail);

        int timezoneId;
        if (int.TryParse(TimeZoneID.SelectedValue, out timezoneId))
            user.TimeZone = (TimeZones)timezoneId;

        DataAccess.DataUserDao.UpdateUser(user);

        //update user queue office 
        this.UpdateUserQueueOffice();

        UserSessions.CurrentUser.DatePattern = user.DatePattern;
        UserSessions.CurrentUser.TimePattern = user.TimePattern;
        UserSessions.CurrentUser.TimeZone = user.TimeZone;

        this.EditMode = false;
        this.ToggleButtons();

        return true;
    }

    public bool FormDataCheck()
    {
        
        string message = string.Empty;
        StringBuilder mess1 = new StringBuilder();

        if (FirstName.Text == string.Empty)
            mess1.Append("First Name is required.<br>");

        if (LastName.Text == string.Empty)
            mess1.Append("Last Name is required.<br>");
        
        if(TimeZoneID.SelectedValue == "0")
            mess1.Append("Time Zone is required.<br>");

        bool officeChecked = false;

        foreach (ListItem item in this.lstOfficeAccess.Items)
        {
            if (item.Selected)
            {
                officeChecked = true;
                break;
            }
        }

        if (!officeChecked)
        {
            mess1.Append("At least one office access must be checked.<br>");
        }
       
        if (mess1.Length == 0)
        {
            return true;
        }
        else
        {
            Error.Text = mess1.ToString();
            return false;
        }

    }

    public void FormCancel()
    {
        this.EditMode = false;
        FormShow();

        if (Request["PostBackURL"] != null)
            Response.Redirect(Request["PostBackURL"].ToString());
        else
        {
            if (UserSessions.CurrentUser.IsAgent)
                Response.Redirect("~/SecureLeadForms/frmLeads.aspx");
            if (UserSessions.CurrentUser.IsMerchant)
            {
                Session.Clear();
                FormsAuthentication.SignOut();
                Response.Redirect("~/frmLogin.aspx");
            }
            else if (UserSessions.CurrentUser.IsBank)
                Response.Redirect("~/SecureMerchantManagementForms/frmMerchantSearch.aspx");
            else
                Response.Redirect("~/SecureHomeForms/frmHome.aspx");
        }
    }

    public void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
    }

    protected void btnChngPwd_Click(object sender, EventArgs e)
    {
        //Use thw same logic to get to the the password reset page based on the days left for current password.
        if (UserSessions.CurrentUser != null)
        {
            User user = UserSessions.CurrentUser;

            if (user.PwdChangeDays <= 0)
                Response.Redirect("~/frmChangePassword.aspx?Redirect=true&Password=true");
            else 
            {
                Response.Redirect("~/frmChangePassword.aspx");
            }
        }
    }

    private void UpdateUserQueueOffice()
    {
        List<int> qOfficeAccess = new List<int>();
        int officeId;

        foreach (ListItem item in this.lstOfficeAccess.Items)
        {
            if (item.Selected && int.TryParse(item.Value, out officeId))
                qOfficeAccess.Add(officeId);
        }

        DataAccess.DataUserDao.SaveUserQueueOffice(UserSessions.CurrentUser.UID, qOfficeAccess, UserSessions.CurrentUser.UserName);
    }
}
