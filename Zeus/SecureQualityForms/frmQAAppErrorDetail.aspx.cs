using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using Infragistics.Web.UI.LayoutControls;
using Infragistics.WebUI.WebDataInput;
using System.Globalization;

public partial class frmQAAppErrorDetail : frmBaseDataEntry
{
   
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);

        base.StoreViewStateOnServer = true;
        this.UID = CommonUtility.Util.if_s(Request.QueryString["QAAPPErrorID"], null) + "&" + CommonUtility.Util.if_s(Request.QueryString["MID"], null);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserSessions.CurrentUser == null)
        {
            Response.Redirect("~/frmLogin.aspx");
        }
     
        ((HyperLink)this.Master.FindControl("lnkApplicationErrors")).CssClass = "active";
        lblError.Text = "";
        string queryStrQAAppErrorKeyID = CommonUtility.Util.if_s(Request.QueryString["QAAppErrorKeyID"], null);

        if (!IsPostBack)
        {
            if (Request.QueryString["Adding"] != null)
                this.Adding = Convert.ToBoolean(Request.QueryString["Adding"].ToString());

            if (Request.QueryString["statusmess"] != null && Request.QueryString["statusmess"].ToString() != string.Empty)
                this.Master.AddMessageStatus(Request.QueryString["statusmess"].ToString());

            if (UserSessions.CurrentQAAppError != null)
            {
                this.Master.SetStatusBarText(string.Format("<b>MID:</b> {0} ", UserSessions.CurrentQAAppError.MID));
            }

            LoadDropdownsREFData();

            //set Adding flag
            if (this.Adding)
            {
                //this.MID.Visible = true;
                //this.MIDTxt.Visible = false;
                this.FormNew();
            }
            else
            {
                this.MID.Visible = false;
                this.MIDTxt.Visible = true;
              
                lblQAAppErrorID.Text = CommonUtility.Util.if_s(Request.QueryString["QAAppErrorID"], null);
                this.UID = CommonUtility.Util.if_s(Request.QueryString["QAAppErrorID"], string.Empty) + "&" + CommonUtility.Util.if_s(Request.QueryString["MID"], string.Empty);

                if (!string.IsNullOrEmpty(this.UID))
                {
                    this.btnCancel.Enabled = true;
                    this.FormShow(this.UID);
                   
                }
            }

        }
    }


    public override void FormShow(string uid)
    {
        string[] splitUID = uid.Split('&');
        string appErrorId = splitUID[0];
        string mid = splitUID[1];
        lblQAAppErrorID.Text = appErrorId;
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        QAAppError qaAppError = new QAAppError();

        Hashtable prms = new Hashtable();
        if (this.Adding)
        {
            MIDTxt.Visible = true;
            this.MID.Visible = false;
        }
        else
        {
            MIDTxt.Visible = false;
            this.MID.Visible = true;
        }
        if (!this.Adding)
        {
            prms.Add("@QAAppErrorId", int.Parse(appErrorId));
            prms.Add("@MID", mid);
            qaAppError = data.GetSelectedQAAppError(prms);

            UserSessions.CurrentQAAppError = qaAppError;

            FormBinding.BindObjectToControls(qaAppError, pnlQAAppErrorDetail);
            FormHandler.SetControlEditMode(pnlQAAppErrorDetail, this.EditMode);
            DateErrorFound.Text = CommonUtility.Formatting.FormatDate(DateErrorFound.Text);

            if (qaAppError != null)
                this.DateCreated.Text = qaAppError.DateCreated.ToString("MM/dd/yyyy HH:mm tt");
         
            this.MIDTxt.Visible = false;
            this.MID.Visible = true;
            calDateErrorFound.Enabled = this.EditMode;
            imgDateErrorFound.Enabled = this.EditMode;
        }
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlQAAppErrorDetail);
    }

    public override bool FormSave()
    {

        bool perform = false;
        if (this.FormDataCheck())
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            Hashtable prms = new Hashtable();
            try
            {

                prms.Add("@Rep", Rep.Text);
                prms.Add("@DepartmentID", Dept.SelectedValue);
                prms.Add("@ErrorFoundByID", ErrorFoundBy.SelectedValue);
                prms.Add("@ErrorOccurredStageID", ErrorOccuredStage.SelectedValue);
                prms.Add("@CategoryID", Category.SelectedValue);
                prms.Add("@SubCategoryID", SubCategory.SelectedValue);
                prms.Add("@Description", Description.Text);
                prms.Add("@DateErrorFound", Convert.ToDateTime(DateErrorFound.Text));

                if (!this.Adding)
                {
                    prms.Add("QAAppErrorID", Convert.ToInt32(this.lblQAAppErrorID.Text));
                    prms.Add("@MID", MID.Text);
                    prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);
                    data.UpdateQAAppError(prms);
                    perform = true;
                }
                else
                {
                    prms.Add("@MID", MIDTxt.Text);
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
                    data.InsertQAAppError(prms);
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
                this.MID.Visible = true;
                this.MIDTxt.Visible = false;
            }
            else
            {
                this.MID.Visible = false;
                this.MIDTxt.Visible = true;
            }
        }
        return perform;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlQAAppErrorDetail, this.EditMode);
        this.ToggleButtons();
        lblQAAppErrorID.Text = string.Empty;
        calDateErrorFound.Enabled = true;
        imgDateErrorFound.Enabled = true;
        this.MID.Visible = false;
        this.MIDTxt.Visible = true;
    }

    public override bool FormDelete()
    {
        bool perform = false;
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        Hashtable prms = new Hashtable();

        try
        {
            prms.Add("@QAAppErrorID", Convert.ToInt32(lblQAAppErrorID.Text));
            prms.Add("@MID", MID.Text);
            prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);
            data.DeleteQAAppError(prms);
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

        if (string.IsNullOrWhiteSpace(MIDTxt.Text) && MIDTxt.Visible)
            this.Master.AddMessageError("Please enter MID.");

        if (Dept.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Department.");

        if (string.IsNullOrWhiteSpace(Rep.Text))
            this.Master.AddMessageError("Please enter Rep.");

        DateTime dateForValidation;
        if (string.IsNullOrWhiteSpace(DateErrorFound.Text) || !DateTime.TryParseExact(this.DateErrorFound.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateForValidation))
        {
            this.Master.AddMessageError("Please enter a valid Date Error Found.");
        }

        if (ErrorFoundBy.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Error Found By.");

        if (Category.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Category.");

        if (SubCategory.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Sub-Category.");


        if (ErrorOccuredStage.SelectedValue == "-1")
            this.Master.AddMessageError("Please select Error Occurred Stage.");

        if (string.IsNullOrWhiteSpace(Description.Text))
            this.Master.AddMessageError("Please enter Description.");

        if (this.Master.ErrorCount() == 0)
            return true;
        else
            return false;

    }

    public override void FormCancel()
    {
        lblError.Text = string.Empty;
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
        Response.Redirect("~/SecureQualityForms/frmQAAppErrorSearch.aspx");
    }
    public void FormClose(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = string.Empty;

        if (Request.QueryString["PostBackURL"] != null)
        {
            url = Convert.ToString(Request.QueryString["PostBackURL"]);
        }
        else
        {
            url = "~/SecureQualityForms/frmQAAppErrorSearch.aspx";
        }
        if (!url.Equals(string.Empty))
        {
            Response.Redirect(url);
        }
        else
        {
            if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
            {
                FormClear();
                ((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
            }

        }
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnAdd.Enabled = !btnAdd.Enabled;
        myclick.Enabled = !myclick.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        btnDelete.Enabled = !btnDelete.Enabled;
    }

    protected string GetSubmitPostBack()
    {
        return Page.ClientScript.GetPostBackEventReference(btnEdit, string.Empty);
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.CommandName)
        {
            case "Add":

                if (string.IsNullOrEmpty(this.UID))
                {
                    Response.Redirect(WebUtil.GetMyUrl("Adding=true"));
                }
                else
                {
                    Response.Redirect(WebUtil.GetMyUrl("Adding=true&UID=" + this.UID));
                }

                this.FormNew();

                break;


            case "Save":

                if (this.FormSave())
                {
                    string prm = string.Empty;
                    string[] key = this.UID.Split('&');
                    if (!string.IsNullOrWhiteSpace(key[0]))
                        prm = "QAAppErrorId=" + key[0];
                    else
                        prm = "QAAppErrorId=" + lblQAAppErrorID.Text;

                    if (!string.IsNullOrWhiteSpace(key[1]))
                        prm = "MID=" + key[1];
                    else
                        prm = "MID=" + MIDTxt.Text;


                    if (this.Adding)
                        prm = prm + "&" + "Action=Added";
                    else
                        prm = prm + "&" + "Action=Updated";
                    this.EditMode = false;
                    url = "~/SecureQualityForms/frmQAAppErrorSearch.aspx?Adding=false";
                    url += "&" + prm;
                    Response.Redirect(url);
                }
                break;


            case "Refresh":
                FormShow(this.UID);
                break;

            case "Cancel":

                if (this.UID == string.Empty)
                {
                    this.FormClose(sender, e);
                }
                else
                {
                    this.FormCancel();
                }
                break;

            case "Edit":

                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                this.btnCancel.Enabled = true;
                break;

            case "Delete":
                if (this.FormDelete())
                {
                    this.EditMode = false;
                    url = "~/SecureQualityForms/frmQAAppErrorSearch.aspx?Adding=false";
                    url += "&Action=Delete";
                    Response.Redirect(url);
                }
                break;

        }
    }
    private void LoadDropdownsREFData()
    {
        try
        {
            LookupTableHandler.LoadQAAppErrorOccurredStage(ErrorOccuredStage,false);
            LookupTableHandler.LoadQAAppErrorFoundBy(ErrorFoundBy,false);
            LookupTableHandler.LoadQAAppDepartment(Dept,false);
            LookupTableHandler.LoadQAAppCategory(Category,false);
            LookupTableHandler.LoadQAAppSubCategory(SubCategory, false);
        }
        catch (Exception)
        {
            
            
        }
    
    }


}