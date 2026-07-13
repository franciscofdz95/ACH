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
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.NavigationControls;
using Infragistics.Web.UI.EditorControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;

public partial class frmAgentBanks : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentAgent != null)
            base.UID = UserSessions.CurrentAgent.AgentUID;

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkAgentBanks")).CssClass = "active";
        lblError.Visible = false;

        if (!this.IsPostBack)
        {
            this.FormShow("");
        }
    }

    public override void FormShow(string ID)
    {
        if (UserSessions.CurrentAgent != null)
        {
            this.LoadBanks();

        }
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    private AgentPayout GetAgentPayout(string UID)
    {
        Agent agent = UserSessions.CurrentAgent;
        AgentPayout p = null;
        foreach (AgentPayout payout in agent.AgentPayouts)
        {
            if (payout.UID.ToUpper() == UID.ToUpper())
            {
                p = payout;
                break;
            }
        }
        return p;
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
        throw new Exception("The method or operation is not implemented.");
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
                    url = "~/SecureAgentManagementForms/frmAgentBanks.aspx?Adding=false";
                    url += "&AgentID=" + this.UID;
                    url += "&AgentUID=" + this.UID;
                    Response.Redirect(url);
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

    protected void grdBanks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                CheckBox chkEnable = (CheckBox)e.Row.FindControl("chkEnabled");
                chkEnable.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Checked"));

                break;
            default:
                break;
        }
    }

    public void chkEnabled_CheckedChanged(object sender, EventArgs e)
    {

        try
        {
            GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int rowIndex = grdRow.RowIndex;


            int? RecordID = DataLayer.Field2Int(grdBanks.DataKeys[rowIndex].Values["RecordID"]);
            bool Enabled = DataLayer.Field2Bool(((CheckBox)grdRow.FindControl("chkEnabled")).Checked);
            string MerchantAppTypeUID = Convert.ToString(grdBanks.DataKeys[rowIndex].Values["MerchantAppTypeUID"]);
            string AgentUID = UserSessions.CurrentAgent.AgentUID;
            bool isDefault = DataLayer.Field2Bool(grdBanks.DataKeys[rowIndex].Values["IsDefault"]);
            int FileID = DataLayer.Field2Int(grdBanks.DataKeys[rowIndex].Values["FileID"]);
            int? AgentBankFileID = DataLayer.Field2Int(grdBanks.DataKeys[rowIndex].Values["AgentBankFileID"]);

            if (((CheckBox)sender).Checked == false)
            {
                bool istrue = false;
                foreach (GridViewRow row in grdBanks.Rows)
                {
                    if (((CheckBox)row.FindControl("chkEnabled")).Checked)
                    {istrue = true; break;}

                }

                if (!istrue)
                {
                    lblError.Visible = true;
                    lblError.Text = "Select atleast one bank application.";
                    return;
                }
            }

            if (AgentBankFileID != 0)
            {
                DataAccess.DataAgentDao.UpdateAgentBankFile(AgentBankFileID, Enabled);
                DataAccess.DataAgentDao.UpdateAgentBank(RecordID, Enabled, MerchantAppTypeUID, AgentUID);
            }
            else
            {
                int recordid = DataAccess.DataAgentDao.UpdateAgentBank(RecordID, Enabled, MerchantAppTypeUID, AgentUID);
                if (recordid > 0)
                    DataAccess.DataAgentDao.InsertAgentBankFile(recordid, FileID);
            }

            this.LoadBanks();

        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    private void LoadBanks()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);
        DataSet ds = DataAccess.DataAgentDao.GetAgentBankFiles(prms);
        grdBanks.DataSource = ds;
        grdBanks.DataBind();


    }

}
