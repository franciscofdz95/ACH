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
using System.Data.SqlClient;
using Infragistics.WebUI.WebDataInput;


using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class frmRoles : frmBaseDataEntry
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
        if (!this.IsPostBack)
        {
            this.Master.SideMenuSelect(MasterPageAdmin.eMasterSideMenu.ManageRoles);
            
            LookupTableHandler.LoadPortals(lstPortals, false);

            this.LoadRoles();

            if (Convert.ToString(Request.QueryString["RoleUID"]) != string.Empty)
            {
                this.UID = Request.QueryString["RoleUID"];
            }
        }
    }

    public override void FormShow(string ID)
    {

        //DataUser data = DataAccess.DataUserDao;
        //User user = data.GetUser(ID);
        //Hashtable prms = new Hashtable();
        //prms.Add("@UserID", ID);

        //user.UserRoles = data.GetUserRoles(prms);



        this.LoadForms();
        //FormBinding.BindObjectToControls(user, pnlDetail);
        //FormHandler.SetControlEditMode(pnlDetail, this.EditMode);


        //lblMessage.Text = string.Empty;
        lblError.Text = string.Empty;

        //this.CurrentObject = user;
        this.UID = lstRoles.SelectedItem.Value;

    }

    public override void FormClear()
    {
        //LeadID.Text = string.Empty;
        //DBAName.Text = string.Empty;

        //AppointmentID.Text = string.Empty;
        //StartDateTime.Value = null;
        //EndDateTime.Value = null;
        //StartTime.SelectedIndex = 0;
        //EndTime.SelectedIndex = 0;
        //Notes.Text = string.Empty;
        //UserCreated.Text = string.Empty;
        //DateCreated.Text = string.Empty;
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        try
        {


            DataUser data = DataAccess.DataUserDao;

            //Update form access
            foreach (GridViewRow row in grdForm2.Rows)
            {
                CheckBox chkHasAccess = (CheckBox)row.FindControl("chkHasAccess");
                data.UpdateRoleForm(grdForm2.DataKeys[row.RowIndex].Values["RoleFormUID"].ToString(), chkHasAccess.Checked);
            }

            foreach (GridViewRow row in grdObjects2.Rows)
            {
                CheckBox chkVisible = (CheckBox)row.FindControl("chkVisible");
                CheckBox chkEnabled = (CheckBox)row.FindControl("chkEnabled");

                data.UpdateRoleObject(grdObjects2.DataKeys[row.RowIndex].Values["RoleObjectUID"].ToString()
                    , chkVisible.Checked
                    , chkEnabled.Checked);
            }

            //foreach (UltraGridRow row in grdForms.Rows)
            //{
            //    data.UpdateRoleForm(row.Cells.FromKey("RoleFormUID").Value.ToString(), Convert.ToBoolean(row.Cells.FromKey("HasAccess").Value));
            //}

            ////Update object access
            //foreach (UltraGridRow row in grdObjects.Rows)
            //{
            //    data.UpdateRoleObject(row.Cells.FromKey("RoleObjectUID").Value.ToString(), Convert.ToBoolean(row.Cells.FromKey("Visible").Value), Convert.ToBoolean(row.Cells.FromKey("Enabled").Value));
            //}


            this.EditMode = false;
            //this.ToggleButtons();

            //this.UpdateAttachments();

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public override bool FormDelete()
    {
        if (this.UID.Equals(string.Empty))
            return false;

        DataUser data = DataAccess.DataUserDao;
        int rows = data.DeleteUser(this.UID);
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
        //btnClose.Enabled = !btnClose.Enabled;
        btnDelete.Enabled = !btnDelete.Enabled;

        //MasterPageAdmin master = (MasterPageAdmin)this.Master;
        //master.Menu.Enabled = !master.Menu.Enabled;
        lstRoles.Enabled = !lstRoles.Enabled;
        lstPortals.Enabled = !lstPortals.Enabled;

        grdForm2.Enabled = !grdForm2.Enabled;
        grdObjects2.Enabled = !grdObjects2.Enabled;
        //foreach (UltraGridColumn col in grdForms.Columns)
        //{
        //    if (col.Type != ColumnType.NotSet)
        //    {
        //        if (btnSave.Enabled)
        //            col.AllowUpdate = AllowUpdate.Yes;
        //        else
        //            col.AllowUpdate = AllowUpdate.No;
        //    }
        //}

        //foreach (UltraGridColumn col in grdObjects.Columns)
        //{
        //    if (col.Type != ColumnType.NotSet)
        //    {
        //        if (btnSave.Enabled)
        //            col.AllowUpdate = AllowUpdate.Yes;
        //        else
        //            col.AllowUpdate = AllowUpdate.No;
        //    }
        //}

    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        //this.ToggleButtons();

    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        //this.ToggleButtons();
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;

        //if (UserName.Text == string.Empty)
        //    message += "User Name is required.<br />";

        //if (Password.Text == string.Empty)
        //    message += "Password is required.<br />";

        //if (FirstName.Text == string.Empty)
        //    message += "First Name is required.<br />";

        //if (LastName.Text == string.Empty)
        //    message += "Last Name is required.<br />";

        //if (Email.Text == string.Empty)
        //    message += "Email is required.<br />";

        //if (AccessLevelUID.SelectedItem.Value == "-1")
        //    message += "Access Level is required.<br />";

        //if (DefaultRoleUID.SelectedItem.Value == "-1")
        //    message += "Default Role is required.<br />";

        if (message == string.Empty)
            return true;
        else
        {
            lblError.Text = message;
            return false;
        }
    }

    private void LoadRoles()
    {
        DataUser data = DataAccess.DataUserDao;

        IList<GenericListItem> list = data.GetRoles(new Hashtable());

        lstRoles.Items.Clear();
        foreach (GenericListItem item in list)
        {
            lstRoles.Items.Add(new ListItem(item.ItemText, item.ItemValue));
        }

        if (lstRoles.Items.Count > 0)
        {
            lstRoles.SelectedIndex = 0;
            this.FormShow(lstRoles.SelectedItem.Value);
        }

    }

    private void LoadForms()
    {
        DataUser data = DataAccess.DataUserDao;
        string PortalUID = null;

        PortalUID = lstPortals.SelectedItem.Value;

        DataSet ds = data.GetFormPermissions(lstRoles.SelectedItem.Value, PortalUID);

        grdForm2.DataSource = ds;
        grdForm2.DataBind();

        if (grdForm2.Rows.Count > 0)
        {
            GridViewRow row = grdForm2.Rows[0];
            string formuid = grdForm2.DataKeys[row.RowIndex].Values["FormUID"].ToString();
            this.LoadObjectsForms(formuid);
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
                break;
            case "Save":
                if (this.FormSave())
                {
                    //url = "~/SecureAdminForms/frmroles.aspx?Adding=false";
                    //url += "&PostBackURL=" + Request.QueryString["PostBackURL"];
                    //url += "&RoleUID=" + this.UID;
                    //Response.Redirect(url);

                    //this.FormShow(this.UID);
                }

                break;
            case "Refresh":
                this.FormShow(this.UID);

                break;
            case "Cancel":
                //if (this.UID.Equals(string.Empty))
                //{
                //    Response.Redirect("frmRoles.aspx?");
                //}
                //else
                this.FormCancel();

                break;
            case "Close":
                //this.CloseForm();

                break;
            case "Delete":
                if (this.FormDelete())
                {
                    this.UID = string.Empty;
                    this.LoadRoles();
                }

                break;
            case "Edit":
                this.EditMode = true;
                //this.FormShow(this.UID);
                //this.ToggleButtons();
                break;

        }
    }
    protected void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.FormShow(lstRoles.SelectedItem.Value);
    }

    private void LoadObjectsForms(string FormUID)
    {
        DataUser data = DataAccess.DataUserDao;
        string RoleUID = lstRoles.SelectedItem.Value;
        string PortalUID = null;
        PortalUID = lstPortals.SelectedItem.Value;
        DataSet ds = data.GetRoleObjects(RoleUID, FormUID, PortalUID);

        grdObjects2.DataSource = ds;
        grdObjects2.DataBind();
    }

    protected void grdForm2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Form")
        {
            this.LoadObjectsForms(e.CommandArgument.ToString());
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
    }

    protected void lstPortals_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.FormShow(this.UID);
    }

    protected void grdForm2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:


                LinkButton lnk = (LinkButton)e.Row.FindControl("lnkFormID");
                lnk.CommandArgument = DataBinder.Eval(e.Row.DataItem, "FormUID").ToString();
                lnk.CommandName = "Form";
                lnk.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

                int objectcount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ObjectCount"));
                if (objectcount == 0)
                    lnk.Enabled = false;


                CheckBox chk = (CheckBox)e.Row.FindControl("chkHasAccess");
                chk.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasAccess"));
                break;
            default:
                break;
        }
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


            string RoleFormUID = grdForm2.DataKeys[rowIndex].Values["RoleFormUID"].ToString();
            CheckBox chkHasAccess = (CheckBox)grdRow.FindControl("chkHasAccess");

            DataAccess.DataUserDao.UpdateRoleForm(RoleFormUID, Convert.ToBoolean(chkHasAccess.Checked));





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


            string RoleObjectUID = grdObjects2.DataKeys[rowIndex].Values["RoleObjectUID"].ToString();
            CheckBox chkVisible = (CheckBox)grdRow.FindControl("chkVisible");
            CheckBox chkEnabled = (CheckBox)grdRow.FindControl("chkEnabled");


            DataAccess.DataUserDao.UpdateRoleObject(RoleObjectUID
                    , chkVisible.Checked
                    , chkEnabled.Checked);




        }
        catch (Exception exc)
        {
            throw exc;
        }
    }


}
