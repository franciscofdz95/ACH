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

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

using Infragistics.WebUI.WebDataInput;

public partial class frmCashAdvance : frmBaseDataEntry
{


    override protected void OnInit(EventArgs e)
    {
        grdMerchants.GridRowCommand += new wucMerchants.GridRowCommandHandler(grdMerchants_GridRowCommand);
        CashAdvance1.ButtonClick += new wucCashAdvance.ButtonClickHandler(CashAdvance1_ButtonClick);
        base.OnInit(e);
    }

    void CashAdvance1_ButtonClick(object sender, EventArgs e)
    {
        LookupTableHandler.LoadCashAdvanceMerchants(lstMerchants, false);
        if (UserSessions.CurrentMerchantApp != null)
            ListHandler.ListFindItem(lstMerchants, UserSessions.CurrentMerchantApp.MerchantAppUID);
        else
            lstMerchants.SelectedIndex = 0;
        ToggleButtons();
        this.EditMode = false;
        FormShow(lstMerchants.SelectedValue);
       // lstMerchants.Enabled = false;
    }

    private void grdMerchants_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;
        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;
        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
        string uid = str[0];

        grdMerchants.ClearGrid();
        this.dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        this.UID = uid;
        FormShow(uid);
        FormHandler.SetControlEditMode(CashAdvance1, true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        dlgcontrol.Attributes.Add("onKeyDown", "KeyDownHandler('" + ((Button)grdMerchants.FindControl("btnSearch")).ClientID + "')");

        //Set current page
        //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Ticket";
        lblError.Text = "";

        //Primary key column name
        //this.UIDName = "TicketUID";

        //Set Security on the page
        FormHandler.SetSecurity(this.Page);

        grdMerchants.DataSourceSelectCountMethod = "GetMerchantAppsPagingRowCount";
        grdMerchants.DataSourceSelectMethod = "GetMerchantAppsPaging";

        if (UserSessions.CurrentUser == null)
            Response.Redirect("~/frmLogin.aspx");

        if (!IsPostBack)
        {
            dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            LookupTableHandler.LoadCashAdvanceMerchants(lstMerchants, false);
            lstMerchants.SelectedIndex = 0;
            FormShow("");
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;
        MerchantApp app = new MerchantApp();
        string url = string.Empty;
        if (UserSessions.CurrentMerchantApp != null)
        {
            MerchantFacade facade = new MerchantFacade();
            UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            app = (MerchantApp)UserSessions.CurrentMerchantApp;
        }
        switch (btn.Text)
        {
            case "Add":
                this.FormNew();
                break;

            case "Save":
                if (this.FormSave())
                {
                    this.Adding = false;
                    this.EditMode = false;
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                if (this.UID != string.Empty)
                    this.FormCancel();
                break;

            case "Delete":
                this.FormDelete();

                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;
        }
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        FormHandler.SetControlEditMode(CashAdvance1, false);
        lstMerchants.SelectedIndex = -1;
        btnMerSelect.Visible = true;
        CashAdvance1.Formclear();
        CashAdvance1.grdVisible = false;        
        ToggleButtons();
    }

    public override bool FormDelete()
    {
        return false;
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        ListHandler.ListFindItem(lstMerchants, lstMerchants.Items[0].Value);
        FormShow("");
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void FormShow(string ID)
    {
        string uid = ID;
        if (ID == string.Empty)
            uid = lstMerchants.SelectedValue;
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(uid);
        this.UID = agreement.MerchantAppUID;
        UserSessions.CurrentMerchantApp = agreement;

        FormBinding.BindObjectToControls(agreement, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
       
        CashAdvance1.Formclear();
        CashAdvance1.grdVisible = true;
        CashAdvance1.LoadCashAdvance();
        
        lblError.Text = string.Empty;
        btnMerSelect.Visible = false;
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
    }

    public override bool FormSave()
    {
        bool perform = false;
        try
        {
            perform = true;

        }
        catch (Exception exc)
        {
            throw exc;
        }

        return perform;
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnAdd.Enabled = !btnAdd.Enabled;
        btnRefresh.Enabled = !btnRefresh.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        

        lstMerchants.Enabled = !lstMerchants.Enabled;
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;
        if (message == string.Empty)
            return true;
        else
        {
            lblError.Text = message;
            return false;
        }
    }

    protected void lstMerchants_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstMerchants.SelectedIndex > -1)
        {
            FormShow(lstMerchants.SelectedValue);
        }
    }

    protected void btnMerSelect_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        grdMerchants.SetDataSource(prms, 10);
        dlgcontrol.Modal = false;
        dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }
}
