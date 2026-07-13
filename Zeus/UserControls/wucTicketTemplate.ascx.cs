using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using CommonUtility;
using System.Drawing;
using Infragistics.WebUI.WebDataInput;

public partial class wucTicketTemplate : wucBaseDataEntry
{

    public delegate void ButtonClickHandler(object sender, string args);
    public event ButtonClickHandler ButtonClick;

    public int _TicketTemplateID
    {
        get { return CommonUtility.Util.if_i(ViewState["TemplateID"], 0); }
        set { ViewState["TemplateID"] = value; }
    }
        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();
        }
    }

    private void LoadDropdowns()
    {
        LookupTableHandler.LoadDepartments(DepartmentID, false);
        pnlCategory.Visible = false;
        pnlSubCategory.Visible = false;

        if (UserSessions.CurrentTicketTemplate != null && UserSessions.CurrentTicketTemplate.DepartmentID > 0 && this.Adding == false)
        {
            DepartmentID.SelectedValue = UserSessions.CurrentTicketTemplate.DepartmentID.ToString();
        }

        if (DepartmentID.SelectedValue == "-1")
        {
            pnlCategory.Visible = false;
            pnlSubCategory.Visible = false;
        }
        else
        {
            LookupTableHandler.LoadTicketCategories(CategoryID, false, CommonUtility.Util.if_i(DepartmentID.SelectedValue, -1), "i", "0");

            if (UserSessions.CurrentTicketTemplate != null && UserSessions.CurrentTicketTemplate.CategoryID >  0 && this.Adding == false)
            {
                pnlCategory.Visible = true;
                pnlSubCategory.Visible = true;

                if (CategoryID.Items.FindByValue(UserSessions.CurrentTicketTemplate.CategoryID.ToString()) != null)
                {
                    CategoryID.SelectedValue = UserSessions.CurrentTicketTemplate.CategoryID.ToString();
                    LookupTableHandler.LoadTicketCategories(SubCategoryID, false, -1, "i", CategoryID.SelectedValue);
                }
                else
                {
                    CategoryID.SelectedValue = "-1";
                }
            }
        }

    }

    public override void FormShow(string ID)
    {
        _TicketTemplateID = int.Parse(ID);

        Hashtable prms = new Hashtable();
        prms.Add("@TicketTemplateID", _TicketTemplateID);

        TicketTemplate ticketTemp = DataAccess.DataTicketDao.GetTicketTemplate(prms, UserSessions.CurrentUser.TimeZone);

        if (ticketTemp != null)
        {
            UserSessions.CurrentTicketTemplate = ticketTemp;

            LoadDropdowns();
            
            FormBinding.BindObjectToControls(ticketTemp, pnlEdit);
            
            Status.SelectedValue = ticketTemp.IsActive.GetHashCode().ToString();
        }

        FormHandler.SetControlEditMode(pnlEdit, this.EditMode);       
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlEdit);
        
        lblError.Text = string.Empty;
    }

    public override bool FormSave()
    {
        TicketTemplate tickTemp = null;
        List<string> errorlist = new List<string>();
        lblError.Text = string.Empty;

        if (!FormDataCheck())
            return false;

        if (this.Adding)
        {
            tickTemp = new TicketTemplate();
        }
        else
        {
            tickTemp = UserSessions.CurrentTicketTemplate;            
        }

        DataTicket data = DataAccess.DataTicketDao;
        User user = UserSessions.CurrentUser;

        FormBinding.BindControlsToObject(tickTemp, pnlEdit);

        tickTemp.IsActive = (Status.SelectedValue == "1");
        
        int rows = 0;
        
        if (!this.Adding)
        {
            // IN EDITING  
            tickTemp.TicketTemplateID = this._TicketTemplateID;
            tickTemp.UserUpdated = user.UserName;  
            rows = data.UpdateTicketTemplate(tickTemp);
           
            UserSessions.CurrentTicketTemplate = tickTemp;
        }
        else
        {
            // IN ADDING
            
            tickTemp.UserCreated = user.UserName;
            data.InsertTicketTemplate(tickTemp);
            
            if (tickTemp.TicketTemplateID > 0)
            {               
                UserSessions.CurrentTicketTemplate = tickTemp;                
            }
        }
        
        return true;
    }

    public override void FormNew()
    {
        this.FormClear();

        this.Adding = true;
        this.EditMode = true;

        FormHandler.SetControlEditMode(pnlEdit, this.EditMode);

        pnlCategory.Visible = false;
        pnlSubCategory.Visible = false;

        this.ToggleButtons();
    }

    public override bool FormDelete()
    {        
        return true;
    }

    public override bool FormDataCheck()
    {
        List<string> errorlist = new List<string>();
        
        if (string.IsNullOrWhiteSpace(TicketTemplateName.Text))
        {
            errorlist.Add("Title is required.");
            //ToggleClass(TicketTemplateName, true);
        }
        else if (TicketTemplateName.Text.Length > 50)
        {
            errorlist.Add("Title accepts only 50 characters.");
        }
        else
        {

            if (DataAccess.DataTicketDao.TicketTemplateNameExists(TicketTemplateName.Text,_TicketTemplateID))
            {
                errorlist.Add("Title already exists.");
                //ToggleClass(TicketTemplateName, true);
            }
        }

        if (DepartmentID.SelectedIndex <= 0)
        {
            errorlist.Add("Department is required.");
            //ToggleClass(CategoryID, true);
        }

        if(CategoryID.SelectedIndex <= 0)
        {
            errorlist.Add("Category is required.");
            //ToggleClass(CategoryID, true);
        }

        if (SubCategoryID.SelectedIndex <= 0)
        {
            errorlist.Add("SubCategory is required.");
            //ToggleClass(SubCategoryID, true);
        }

        if (!string.IsNullOrWhiteSpace(DueDays.Text) && CommonUtility.Util.if_i(DueDays.Text.Trim(), 0) >= 100)
            errorlist.Add("Due days cannot exceed 99.");

        if (string.IsNullOrWhiteSpace(Description.Text))
        {
            errorlist.Add("Description is required.");
            //ToggleClass(Description, true);
        }
        else if (Description.Text.Length > 200)
        {
            errorlist.Add("Description accepts only 200 characters.");
        }
        
        if (string.IsNullOrWhiteSpace(Issue.Text))
        {
            errorlist.Add("Issue is required.");
            //ToggleClass(Issue, true);
        }
         
        if (errorlist.Count > 0)
        {
            foreach (string str in errorlist)
                lblError.Text += str + "<br>";
            
            lblError.Text += "<br>";

            return false;
        }

        return true;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(_TicketTemplateID.ToString());
        this.Adding = false;
        this.ToggleButtons();

        if (_TicketTemplateID == 0)
        {
            if (this.ButtonClick != null)
                this.ButtonClick(null, "");
        }

    }

    public void SetControlEditMode(bool isEdit)
    {
        FormHandler.SetControlEditMode(pnlEdit, isEdit);
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnCancel.Enabled = this.EditMode;
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        string url = string.Empty;


        switch (btn.Text)
        {
            case "Add":

                FormNew();

                break;

            case "Save":

                if (this.FormSave())
                {
                    lblError.Text = "";
                    this.Adding = false;
                    this.EditMode = false;
                    ToggleButtons();

                    FormShow(_TicketTemplateID.ToString());
                }

                break;

            case "Refresh":
                this.FormShow(_TicketTemplateID.ToString());
                break;

            case "Cancel":

                this.FormCancel();
                
                break;

            case "Delete":
                this.FormDelete();
                
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(_TicketTemplateID.ToString());
                this.ToggleButtons();
                break;

        }
    }

    private void ToggleClass(Control Ctrl, bool istrue)
    {

        Color backclr = Color.White;
        Color borderclr = Color.FromName("#999999");

        if (istrue)
        {
            backclr = Color.FromName("#fcc");
            borderclr = Color.Red;
        }

        if (Ctrl != null)
        {
            if (Ctrl is TextBox)
            {
                TextBox txt = (TextBox)Ctrl;
                txt.BorderColor = borderclr;
                txt.BorderWidth = Unit.Pixel(2);
                txt.BackColor = backclr;
            }
            else if (Ctrl is DropDownList)
            {
                DropDownList ddp = (DropDownList)Ctrl;
                ddp.BorderColor = borderclr;
                ddp.BorderWidth = Unit.Pixel(2);
                ddp.BackColor = backclr;
            }
        }
    }

    protected void DepartmentID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        int department_id = -1;

        if (ddl.SelectedValue != "-1" && ddl.SelectedValue != "")
        {
            department_id = CommonUtility.Util.if_i(ddl.SelectedValue, -1);

            LookupTableHandler.LoadTicketCategories(CategoryID, false, department_id, "i", "0");
            LookupTableHandler.LoadTicketCategories(SubCategoryID, false, department_id, "i", CategoryID.SelectedValue);

            pnlCategory.Visible = true;
            pnlSubCategory.Visible = true;
        }
        else
        {
            pnlSubCategory.Visible = false;
            pnlCategory.Visible = false;
        }

        CategoryID.SelectedValue = "-1";

        if (UserSessions.CurrentTicketTemplate != null
                && CommonUtility.Util.if_i(UserSessions.CurrentTicketTemplate.DepartmentID, 0) == department_id
                && CommonUtility.Util.if_i(UserSessions.CurrentTicketTemplate.CategoryID, -1) != -1
                && this.Adding == false
            )
        {
            ListHandler.ListFindItem(SubCategoryID, UserSessions.CurrentTicketTemplate.SubCategoryID.ToString());
            ListHandler.ListFindItem(CategoryID, UserSessions.CurrentTicketTemplate.CategoryID.ToString());
        }

        DepartmentID.Focus();

    }

    protected void CategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        int Category_id = -1;

        if (ddl.SelectedValue != "-1" && ddl.SelectedValue != "")
        {
            Category_id = CommonUtility.Util.if_i(ddl.SelectedValue, -1);
            LookupTableHandler.LoadTicketCategories(SubCategoryID, false, CommonUtility.Util.if_i(DepartmentID.SelectedValue, -1), "i", Category_id.ToString());
            pnlSubCategory.Visible = true;
        }
        else
        {
            pnlSubCategory.Visible = false;
        }

        SubCategoryID.SelectedValue = "-1";

        if (UserSessions.CurrentTicketTemplate != null
                && CommonUtility.Util.if_i(UserSessions.CurrentTicketTemplate.DepartmentID, 0) > 0
                && CommonUtility.Util.if_i(UserSessions.CurrentTicketTemplate.CategoryID, -1) == Category_id
                && this.Adding == false
            )
        {
            CategoryID.SelectedValue = UserSessions.CurrentTicketTemplate.CategoryID.ToString();

            if (CommonUtility.Util.if_i(UserSessions.CurrentTicketTemplate.SubCategoryID, -1) > 0)
                SubCategoryID.SelectedValue = UserSessions.CurrentTicketTemplate.SubCategoryID.ToString();
        }

        CategoryID.Focus();
    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        if (this.ButtonClick != null)
            this.ButtonClick(sender, "");
    }
   

}
