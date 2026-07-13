using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using Infragistics.WebUI.WebDataInput;


public partial class frmProductManagement : frmBaseDataEntry
{
    #region Properties

    private int ProductID = int.MinValue;

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        this.ParseQueryString();
        
        this.Adding = Convert.ToBoolean(Request["Adding"]);
        
        if (!Page.IsPostBack)
        {
            this.MarketingContent.ImageDirectory = String.Format("{0}/ig_common/Images/htmleditor", WebUtil.GetBaseUrl());
        }

        if (!Page.IsPostBack && this.Adding)
        {
            this.SetButtons(eButtonSet.Add);
            this.FormNew();
        }
        else if (!Page.IsPostBack)
        {
            this.FormShow("");
            this.SetButtons(eButtonSet.ReadOnly);
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Save":
                if (this.FormDataCheck())
                {
                    if (this.FormSave())
                    {
                        this.EditMode = false;
                        this.SetButtons(eButtonSet.ReadOnly);
                        this.FormShow("");
                    }
                }
                else
                {
                    this.EditMode = true;
                    this.SetButtons(eButtonSet.Edit);
                    break;
                }
                break;

            case "Refresh":
                this.FormShow("");
                break;

            case "Cancel":
                if (this.Adding)
                {
                    Response.Redirect("~/SecureReports/frmProducts.aspx");
                }
                else
                {
                    this.EditMode = false;
                    this.SetButtons(eButtonSet.ReadOnly);
                    this.FormShow("");
                }
                break;

            case "Edit":
                this.EditMode = true;
                this.SetButtons(eButtonSet.Edit);
                this.FormShow("");
                break;

            case "Add":
                Response.Redirect("~/SecureReports/frmProductManagement.aspx?Adding=true");
                break;

            case "Preview":
                string newWinUrl = String.Format("{0}{1}", WebUtil.GetBaseUrl(), String.Format("SecureReports/frmProductPreview.aspx?ID={0}", this.ProductID));
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), Guid.NewGuid().ToString(), CommonUtility.JSScriptProvider.BuildOpenWindowScript(newWinUrl, "productPreviewWindow"), true);
                break;
        }
    }

    #endregion

    #region Helper Methods

    private void ParseQueryString()
    {
        if (Request.QueryString["ID"] != null)
        {
            int.TryParse(Request.QueryString["ID"], out ProductID);
        }
    }

    private void ToogleForm(bool isEnabled)
    {
        this.ProductName.Enabled = isEnabled;
        this.ProductDescription.Enabled = isEnabled;
        this.MarketingContent.Enabled = isEnabled;


        this.InsightVisible.Enabled = isEnabled;
        this.ApexVisible.Enabled = isEnabled;
        this.PaymentXPVisible.Enabled = isEnabled;
        this.ApplicationXPVisible.Enabled = isEnabled;

        this.IsNOTVisibleOnPrivateLabel.Enabled = isEnabled;
        this.StatusList.Enabled = isEnabled;
        this.MeritusBrand.Enabled = isEnabled;
        this.OptimalBrand.Enabled = isEnabled;
        this.CategoryID.Enabled = isEnabled;
        this.CreateTicket.Enabled = isEnabled;
        this.CategoryID.DataSource = PaymentXP.DataObjects.DataProduct.GetAllCategories();
        this.CategoryID.DataTextField = "CategoryName";
        this.CategoryID.DataValueField = "ProductCategoryID";
        this.CategoryID.DataBind();
    }

    private void SetButtons(eButtonSet eBS)
    {
        // clear all first.
        btnAdd.Enabled = false;
        btnEdit.Enabled = false;
        btnSave.Enabled = false;
        btnCancel.Enabled = false;
        btnRefresh.Enabled = false;
        btnPreview.Enabled = false;

        switch (eBS)
        {
            case eButtonSet.Add:
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                break;

            case eButtonSet.Edit:
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                this.ToogleForm(true);
                break;

            case eButtonSet.ReadOnly:
                btnAdd.Enabled = true;
                btnEdit.Enabled = true;
                btnRefresh.Enabled = true;
                btnPreview.Enabled = true;
                this.ToogleForm(false);
                break;
        }
    }

    #endregion

    #region Implement frmBaseDataEntry

    public override void FormShow(string ID)
    {
        if (!Page.IsPostBack)
        {
            
        }

        Product product = DataProduct.GetProductConfiguration(this.ProductID);

        if (product != null)
        {
            this.ProductName.Text = product.Name;
            this.IsNOTVisibleOnPrivateLabel.Checked = product.IsVisibleOnPrivateLabel;

            this.InsightVisible.Checked = product.IsVisibleOnMerchantPortal;
            this.ApexVisible.Checked = product.IsVisibleOnAgentPortal;
            this.PaymentXPVisible.Checked = product.IsVisibleOnPaymentXP;
            this.ApplicationXPVisible.Checked = product.IsVisibleOnApplicationXP;

            this.ProductDescription.Text = product.Description;
            this.MarketingContent.Text = product.MarketingContent;
            this.StatusList.SelectedValue = product.IsActive.GetHashCode().ToString();

            this.MeritusBrand.Checked = product.IsMeritusProduct;
            this.OptimalBrand.Checked = product.IsOptimalProduct;

            this.CategoryID.SelectedValue = product.Category.GetHashCode().ToString();
            this.CreateTicket.Checked = product.CreateTicket;
        }
    }

    public override void FormClear()
    {
        throw new NotImplementedException();
    }

    public override bool FormSave()
    {
        if (this.ProductID == int.MinValue) { this.ParseQueryString(); }

        bool IsVisibleOnPaymentXP = this.PaymentXPVisible.Checked;
        bool IsVisibleOnMerchantPortal = this.InsightVisible.Checked;
        bool IsVisibleOnAgentPortal = this.ApexVisible.Checked;
        bool IsVisibleOnApplicationXP = this.ApplicationXPVisible.Checked;

        
        if (this.Adding)
        {
            int iProductID = DataProduct.InsertProduct(
                        this.ProductName.Text.Trim(),
                        this.ProductDescription.Text.Trim(),
                        this.MarketingContent.Text.Trim(),
                        (this.StatusList.SelectedItem.Value == "1"),
                        true,   //isProductOptInEnabled
                        this.IsNOTVisibleOnPrivateLabel.Checked,
                        true,  //isVisibleOnZeus -- default to true b/c all products are visible on Zeus so that Client Services can manage them
                        IsVisibleOnMerchantPortal,
                        IsVisibleOnAgentPortal,
                        IsVisibleOnPaymentXP,
                        IsVisibleOnApplicationXP,
                        this.MeritusBrand.Checked,
                        this.OptimalBrand.Checked,
                        int.Parse(UserSessions.CurrentUser.UserID),
                        int.Parse(this.CategoryID.SelectedValue),
                        this.CreateTicket.Checked);

            Response.Redirect("~/SecureReports/frmProducts.aspx");
        }
        else
        {
            DataProduct.UpdateProduct(
                        ProductID,
                        this.ProductName.Text.Trim(),
                        this.ProductDescription.Text.Trim(),
                        this.MarketingContent.Text.Trim(),
                        (this.StatusList.SelectedItem.Value == "1"),
                        true,   //isProductOptInEnabled
                        this.IsNOTVisibleOnPrivateLabel.Checked,
                        false,  //isVisibleOnZeus -- none of the products are visible on Zeus
                        IsVisibleOnMerchantPortal,
                        IsVisibleOnAgentPortal,
                        IsVisibleOnPaymentXP,
                        IsVisibleOnApplicationXP,
                        this.MeritusBrand.Checked,
                        this.OptimalBrand.Checked,
                        int.Parse(UserSessions.CurrentUser.UserID),
                        int.Parse(this.CategoryID.SelectedValue),
                        this.CreateTicket.Checked);
        }

        return true;
    }

    public override void FormNew()
    {
        this.ProductName.Text = "";
        this.ProductDescription.Text = "";
        this.MarketingContent.Text = "";
        this.SetButtons(eButtonSet.Add);
    }

    public override bool FormDelete()
    {
        throw new NotImplementedException();
    }

    public override bool FormDataCheck()
    {
        if (this.ProductName.Text.Trim().Length == 0) 
        {
            this.Master.AddMessageError("Product Name is required.");                
            return false; 
        }

        if (this.ProductDescription.Text.Trim().Length == 0) 
        {
            this.Master.AddMessageError("Product Description is required.");
            return false; 
        }

        if (this.MarketingContent.Text.Trim().Length == 0) 
        {
            this.Master.AddMessageError("Marketing Content is required.");
            return false; 
        }

        if (this.ProductName.Text.Trim().Length > 50) 
        {
            this.Master.AddMessageError("Product Name is too long - max 50 characters.");
            return false; 
        }

        if (this.ProductDescription.Text.Trim().Length > 2000) 
        {
            this.Master.AddMessageError("Product Description is too long - max 2000 characters.");
            return false; 
        }

        if (!this.MeritusBrand.Checked && !this.OptimalBrand.Checked)
        {
            this.Master.AddMessageError("Please assign a Brand to Product.");
            return false; 
        }

        if (this.CategoryID.SelectedValue == "0")
        {
            this.Master.AddMessageError("Please assign Category to Product.");
            return false;
        }

        return true;
    }

    public override void FormCancel()
    {
        throw new NotImplementedException();
    }

    public override void ToggleButtons()
    {
        throw new NotImplementedException();
    }

    #endregion
}