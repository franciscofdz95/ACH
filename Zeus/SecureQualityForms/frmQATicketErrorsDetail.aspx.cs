using Infragistics.Web.UI.LayoutControls;
using Infragistics.WebUI.WebDataInput;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class frmQATicketErrorsDetail : frmBaseDataEntry
{
    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
        EnsureChildControls();
    }
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (UserSessions.CurrentUser == null)
        {
            Response.Redirect("~/frmLogin.aspx");
        }
        ((HyperLink)this.Master.FindControl("lnkTicketErrors")).CssClass = "active";

        if (!IsPostBack)
        {

            LoadDropdownsREFData();

            //Apply security settings
            FormHandler.SetSecurity(this.Page);
            this.Adding = CommonUtility.Util.if_b(Request.QueryString["Adding"], false);

            if (this.Adding)
            {
                this.FormNew();
            }
            else
            {
                this.UID = CommonUtility.Util.if_s(Request.QueryString["QATicketErrorID"], string.Empty);

                if (!string.IsNullOrEmpty(this.UID))
                {
                    this.FormShow(this.UID);
                }
            }
        }
    }

    public override void FormShow(string ID)
    {

        int QATicketErrorID = CommonUtility.Util.if_i(ID, 0);
        
        DataTicket data = DataAccess.DataTicketDao;
        QATicketErrors qaTicketErrors = new QATicketErrors();
        Hashtable prms = new Hashtable();

        if (!this.Adding)
        {
            prms.Add("@QATicketErrorID", QATicketErrorID);
            qaTicketErrors = data.GetQATicketErrorDetails(prms);

            UserSessions.CurrentQATicketErrors = qaTicketErrors;

            FormBinding.BindObjectToControls(qaTicketErrors, pnlTicketErrorsDetail);
            FormHandler.SetControlEditMode(pnlTicketErrorsDetail, this.EditMode);

            this.ZID.Visible = true;
            this.txtZID.Visible = false;
            calsearchDateErrorFound.Enabled = this.EditMode;
            imgSearchDateErrorFound.Enabled = this.EditMode;
            if (UserSessions.CurrentQATicketErrors != null)
            {
                this.DateErrorFound.Text = UserSessions.CurrentQATicketErrors.DateErrorFound.ToString("MM/dd/yyyy");
                this.DateCreated.Text = UserSessions.CurrentQATicketErrors.DateCreated.ToString("MM/dd/yyyy HH:mm tt");
                this.Master.SetStatusBarText(string.Format("<b>ZID:</b> {0}  <b>TicketID:</b> {1}", UserSessions.CurrentQATicketErrors.ZID, UserSessions.CurrentQATicketErrors.TicketID));
            }
        }
    }
    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlTicketErrorsDetail);
    }

    public override bool FormSave()
    {
        bool perform = false;
        if (this.FormDataCheck())
        {
            DataTicket data = DataAccess.DataTicketDao;
            Hashtable prms = new Hashtable();
            try
            {

                DateTime dtDateErrorFound = DateTime.Parse(DateErrorFound.Text);
                prms.Add("@DateErrorFound", dtDateErrorFound.ToString("yyyy-MM-dd"));
                prms.Add("@TicketID", DataLayer.Field2Int(TicketID.Text.Trim()));
                prms.Add("@Rep", DataLayer.Field2Str(Rep.Text.Trim()));
                prms.Add("@CategoryID", DataLayer.Field2IntSafe(Category.Text.Trim()));
                prms.Add("@SubCategoryID", DataLayer.Field2IntSafe(SubCategory.Text.Trim()));
                prms.Add("@Description", DataLayer.Field2Str(Description.Text.Trim()));

                if (!this.Adding)
                {
                    prms.Add("@QATicketErrorID", DataLayer.Field2Int(this.UID));
                    prms.Add("@ZID", DataLayer.Field2Int(ZID.Text.Trim()));
                    prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);
                    data.UpdateQATicketErrors(prms);
                    perform = true;
                }
                else
                {
                    prms.Add("@ZID", DataLayer.Field2Int(txtZID.Text.Trim()));
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
                    data.InsertQATicketErrors(prms);
                    perform = true;

                }
            }
            catch (Exception)
            {
                perform = false;

            }
        }
        else
        {
            if (!this.Adding)
            {
                ZID.Visible = true;
                txtZID.Visible = false;
            }
            else
            {
                ZID.Visible = false;
                txtZID.Visible = true;
            }
        }
        return perform;
    }

    public override void FormNew()
    {
        UserSessions.CurrentQATicketErrors = null;
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlTicketErrorsDetail, this.EditMode);
        this.ToggleButtons();
        this.ZID.Visible = false;
        this.txtZID.Visible = true;
        calsearchDateErrorFound.Enabled = true;
        imgSearchDateErrorFound.Enabled = true;
        DateCreated.Text = string.Empty;
    }

    public override bool FormDelete()
    {
        bool perform = false;

        DataTicket data = DataAccess.DataTicketDao;
        Hashtable prms = new Hashtable();

        try
        {
            prms.Add("@QATicketErrorID", DataLayer.Field2Int(this.UID));
            prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);
            data.DeleteQATicketErrors(prms);
            perform = true;

        }
        catch (Exception)
        {
            perform = false;

        }
        return perform;
    }

    public override bool FormDataCheck()
    {
        DateTime dateForValidation;
        int ZIDForvalidation;
        int TicketIDForValidation;

        if (this.Adding)
        {
            this.ZID.Visible = false;
            this.txtZID.Visible = true;
        }
        else
        {
            this.ZID.Visible = true;
            this.txtZID.Visible = false;
        }
        if (!int.TryParse(txtZID.Text, out ZIDForvalidation) && txtZID.Visible)
        {
            txtZID.Text = string.Empty;
            this.Master.AddMessageError("Please enter a valid ZID.");
        }
        if (!int.TryParse(TicketID.Text, out TicketIDForValidation))
        {
            TicketID.Text = string.Empty;
            this.Master.AddMessageError("Please enter a valid Ticket ID.");
        }
        if (string.IsNullOrWhiteSpace(Rep.Text))
        {
            this.Master.AddMessageError("Please enter Rep.");
        }
        if (string.IsNullOrWhiteSpace(DateErrorFound.Text) || !DateTime.TryParseExact(this.DateErrorFound.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateForValidation))
        {
            this.Master.AddMessageError("Please enter a valid Date Error Found.");
        }
        if (Category.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Category.");

        if (SubCategory.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Sub-Category.");


        if (string.IsNullOrWhiteSpace(Description.Text))
        {
            this.Master.AddMessageError("Please enter Description.");
        }
        if (this.Master.ErrorCount() == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
        Response.Redirect("~/SecureQualityForms/frmQATicketErrorsSearch.aspx");
    }

    public void FormClose(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = string.Empty;

        if (Request.QueryString["PostBackURL"] != null)
            url = Convert.ToString(Request.QueryString["PostBackURL"]);

        if (!string.IsNullOrWhiteSpace(url))
            Response.Redirect(url);
        else
            Response.Redirect("~/SecureQualityForms/frmQATicketErrorsSearch.aspx?Adding=false");

    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !this.EditMode;
        btnAdd.Enabled = !this.EditMode;
        btnSave.Enabled = this.EditMode;
        btnRefresh.Enabled = !this.EditMode;
        btnCancel.Enabled = true;
        btnDelete.Enabled = !this.EditMode;
    }

    protected string GetSubmitPostBack()
    {
        return Page.ClientScript.GetPostBackEventReference(btnSave, string.Empty);
    }
    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.CommandName)
        {
            case "Add":

                Response.Redirect("~/SecureQualityForms/frmQATicketErrorsDetail.aspx?Adding=true");
                break;

            case "Save":
                {
                    if (this.FormSave())
                    {
                        string prm = string.Empty;
                        if (this.Adding)
                        {
                            prm = "ZID=" + txtZID.Text;
                            prm = prm + "&" + "TicketID=" + TicketID.Text + "";
                            prm = prm + "&" + "Action=Added";
                        }
                        else
                        {
                            prm = "ZID=" + ZID.Text;
                            prm = prm + "&" + "TicketID=" + TicketID.Text + "";
                            prm = prm + "&" + "Action=Updated";
                        }
                        this.EditMode = false;
                        url = "~/SecureQualityForms/frmQATicketErrorsSearch.aspx?Adding=false";
                        url += "&" + prm;
                        Response.Redirect(url);
                    }

                }

                break;

            case "Refresh":

                FormShow(this.UID);
                break;

            case "Cancel":

                this.FormClose(sender, e);
                break;

            case "Edit":

                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;

            case "Delete":
                {
                    if (this.FormDelete())
                    {
                        this.EditMode = false;
                        string prm = string.Empty;

                        prm = "ZID=" + ZID.Text;
                        prm = prm + "&" + "TicketID=" + TicketID.Text + "";
                        prm = prm + "&" + "Action=Deleted";

                        url = "~/SecureQualityForms/frmQATicketErrorsSearch.aspx?Adding=false";
                        url += "&" + prm;
                        Response.Redirect(url);
                    }
                }
                break;

        }
    }
    private void LoadDropdownsREFData()
    {
        try
        {
            LookupTableHandler.LoadQATicketCategory(Category,false);
            LookupTableHandler.LoadQATicketSubCategory(SubCategory, false);
        }
        catch (Exception)
        {
            
           
        }
    }
}
