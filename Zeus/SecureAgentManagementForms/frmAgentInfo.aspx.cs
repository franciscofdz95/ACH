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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using System.Collections.Generic;

public partial class frmAgentInfo : frmBaseDataEntry
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
        ((HyperLink)this.Master.FindControl("lnkAgentInfo")).CssClass = "active";
        WebUtil.SetUserSpecificDisplayMode(DOB);
        WebUtil.SetUserSpecificDisplayMode(Child1DOB);
        WebUtil.SetUserSpecificDisplayMode(Child2DOB);
        WebUtil.SetUserSpecificDisplayMode(Child3DOB);
        WebUtil.SetUserSpecificDisplayMode(Anniversary);
        if (!this.IsPostBack)
        {
            //Set the current page
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Agent Background";
            LookupTableHandler.GetAgentCategories(AgentCategoryUID);
            //this.UIDName = "AgentID";
            this.UID = UserSessions.CurrentAgent.AgentUID;
            if (!(string.IsNullOrEmpty(UID)))
                this.FormShow(this.UID);
        }
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Save":
                if (this.FormSave())
                {
                    FormShow(this.UID);
                }
                break;

            case "Cancel":
                this.FormCancel();
                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                break;
        }
    }

    public override void FormShow(string ID)
    {
        DataAgent data = DataAccess.DataAgentDao;
        Agent agent = data.GetAgent(ID);

        FormBinding.BindObjectToControls(agent, pnlDetail);
        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
        lblError.Text = string.Empty;

        //MasterPageAgent master = (MasterPageAgent)this.Master;
        //master.SetDBA = "<b>Agent:</b> " + agent.AgentDBA;

        this.UID = agent.AgentUID;
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
    }

    public override bool FormSave()
    {
        if (!this.FormDataCheck())
            return false;
        try
        {
            //LookupTableHandler.m_AgentNodes = null;
            UserSessions.AgentList = null;

            Agent agent = null;
            Agent clone = null;

            agent = (Agent)UserSessions.CurrentAgent;
            clone = (Agent)agent.Clone();

            FormBinding.BindControlsToObject(agent, pnlDetail);
            DataAgent data = DataAccess.DataAgentDao;
            User user = UserSessions.CurrentUser;
            agent.UserUpdated = user.UserName;

            data.UpdateAgent(agent);
            FormHandler.LogFormChanges(agent.AgentDBA, agent.AgentUID, Convert.ToInt32(agent.AgentID), clone, agent);

            UserSessions.CurrentAgent = agent;
            this.UID = agent.AgentUID;
            this.EditMode = false;
            this.ToggleButtons();
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
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

    public override void FormCancel()
    {
        this.EditMode = false;
        FormShow(this.UID);
        this.ToggleButtons();
    }    
    
    public override void FormNew()
    {
    }

    public override bool FormDelete()
    {
        return true;
    }
}
