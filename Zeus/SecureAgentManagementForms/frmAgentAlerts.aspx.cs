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
using Infragistics.WebUI.WebDataInput;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class frmAgentAlerts : frmBaseDataEntry
{
    //protected override void OnPreInit(EventArgs e)
    //{
    //    base.OnPreInit(e);
    //    // Set Property to Store ViewState on Server
    //    base.StoreViewStateOnServer = true;
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkAgentAlerts")).CssClass = "active";

        if (!this.IsPostBack)
        {
            this.FormShow("");
        }
    }



    public override void FormShow(string ID)
    {
        wucAlerts1.EditMode = this.EditMode;
        wucAlerts1.LoadGrid(eControlContactType.Agent);
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;

        try
        {
            wucAlerts1.FormSave(eControlContactType.Agent);
            this.EditMode = false;
            this.ToggleButtons();
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDataCheck()
    {
        return wucAlerts1.ValidateConfiguration();
    }

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;

        //MasterPageAgent master = (MasterPageAgent)this.Master;
        //master.Menu.Enabled = !master.Menu.Enabled;
        //master.SidePanel.Enabled = !master.SidePanel.Enabled;
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
                    this.FormShow(this.UID);
                }
                break;

            case "Refresh":
                this.FormShow(this.UID);
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Close":
                break;

            case "Delete":
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;
        }
    }

}
