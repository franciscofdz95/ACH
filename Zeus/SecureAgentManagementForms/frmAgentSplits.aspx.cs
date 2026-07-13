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
using System.Collections.Generic;
using CommonUtility;

public partial class frmAgentSplits : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentAgent != null)
            base.UID = UserSessions.CurrentAgent.AgentUID;
    }

    decimal payout = 0.0m;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkAgentSplits")).CssClass = "active";

        if (!this.IsPostBack)
        {
            this.FormShow("");
        }
    }

    public override void FormShow(string ID)
    {
        this.UID = UserSessions.CurrentAgent.AgentUID;
        DataAgent da = new DataAgent();
        Agent agent = new Agent();

        this.BillingCountry.DataSource = LookupTableHandler.LoadCountries();
        this.BillingCountry.DataTextField = "Value";
        this.BillingCountry.DataValueField = "Key";
        this.BillingCountry.DataBind();

        this.BankCountry.DataSource = LookupTableHandler.LoadCountries();
        this.BankCountry.DataTextField = "Value";
        this.BankCountry.DataValueField = "Key";
        this.BankCountry.DataBind();

        LookupTableHandler.LoadCurrencyCodes(BankCurrency, false);

        if (UserSessions.CurrentAgent.BillingCountry != null)
        {
            if (UserSessions.CurrentAgent.BillingCountry.Trim().ToUpper().Equals("US") || UserSessions.CurrentAgent.BillingCountry.Trim().ToUpper().Equals(""))
            {
                this.BillingProvince.Visible = false;
                this.BillingState.Visible = true;
            }

            else
            {
                this.BillingProvince.Visible = true;
                this.BillingState.Visible = false;
            }
        }

        if (UserSessions.CurrentAgent.BankCountry != null)
        {
            if (UserSessions.CurrentAgent.BankCountry.Trim().ToUpper().Equals("US") || UserSessions.CurrentAgent.BankCountry.Trim().ToUpper().Equals(""))
            {
                this.BankProvince.Visible = false;
                this.BankState.Visible = true;
            }

            else
            {
                this.BankProvince.Visible = true;
                this.BankState.Visible = false;
            }
        }

        if (UserSessions.CurrentAgent != null)
        {
            agent = da.GetAgent(UserSessions.CurrentAgent.AgentUID);
            //BankAccountNumber.Text = agent.BankAccountNumber;
            //BankABA.Text = agent.BankABA;
            FormBinding.BindObjectToControls(agent, pnlDetails);

            if (!this.EditMode)
            {
                this.BankAccountNumber.Text = StringExtensions.MaskValue(agent.BankAccountNumber);
                this.BankABA.Text = StringExtensions.MaskValue(agent.BankABA);
                this.BankSwiftID.Text = StringExtensions.MaskValue(agent.BankSwiftID);
                this.BankIBAN.Text = StringExtensions.MaskValue(agent.BankIBAN);
                this.BankCode.Text = StringExtensions.MaskValue(agent.BankCode);
                this.BankBranchCode.Text = StringExtensions.MaskValue(agent.BankBranchCode);
            }

            ResidualPayoutThreshold.Text = agent.ResidualPayoutThreshold.ToString();
        }

        grdPayouts.DataSource = UserSessions.CurrentAgent.AgentPayouts;
        grdPayouts.DataBind();
        grdPayouts.Enabled = btnSave.Enabled;
        lblError.Text = "";

        FormHandler.SetControlEditMode(pnlDetails, btnSave.Enabled);
        ResidualPayoutThreshold.Enabled = btnSave.Enabled;
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }



    public override bool FormSave()
    {
        DataAgent data = DataAccess.DataAgentDao;
        Agent agent = new Agent();

        if (UserSessions.CurrentAgent != null)
        {
            agent = (Agent)UserSessions.CurrentAgent;


            agent.CloneAgent();

            User user = UserSessions.CurrentUser;

            if (!string.IsNullOrEmpty(BankCode.Text.Trim()))
            {
                string BankCodeText = this.BankCode.Text.Trim();
                this.BankCode.Text = BankCodeText.Length < 4 ? BankCodeText.PadLeft(4, '0') : BankCodeText.Trim();
            }

            if (!string.IsNullOrEmpty(BankBranchCode.Text.Trim()))
            {
                string BankBranchCodeText = this.BankBranchCode.Text.Trim();
                this.BankBranchCode.Text = BankBranchCodeText.Length < 5 ? BankBranchCodeText.PadLeft(5, '0') : BankBranchCodeText.Trim();
            }
            FormBinding.BindControlsToObject(agent, pnlDetails);
            agent.UserUpdated = user.UserName;
            agent.ResidualPayoutThreshold = CommonUtility.Util.if_dec(ResidualPayoutThreshold.Value, 0);
        }

        if (!this.FormDataCheck())
            return false;

        try
        {
            if (!agent.BillingCountry.Trim().ToUpper().Equals("US"))
            {
                agent.BillingState = this.BillingProvince.Text;
            }

            if (!agent.BankCountry.Trim().ToUpper().Equals("US"))
            {
                agent.BankState = this.BankProvince.Text;
            }

            int retval = data.UpdateAgent(agent);

            if (retval > 0)
            {
                try
                {
                    string differences = agent.GetChangedValues();

                    if (!string.IsNullOrEmpty(differences))
                    {
                        //DM-1372 
                        DataUser.GetInstance().InsertChangeLog(agent.AgentDBA, UserSessions.CurrentUser.UserName, UserSessions.CurrentAgent.AgentUID, UserSessions.CurrentAgent.AgentID, "Agent Splits", differences, Constants.PORTAL_ZEUS);
                    }
                }
                catch { }
            }

            this.UpdateAgentPayouts();
            this.EditMode = false;
            this.ToggleButtons();
            UserSessions.CurrentAgent = agent;
            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
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

    public bool UpdateAgentPayouts()
    {
        try
        {
            DataAgent data = DataAccess.DataAgentDao;
            AgentPayout payout = null;
            AgentPayout clone = null;

            foreach (GridViewRow row in grdPayouts.Rows)
            {
                payout = GetAgentPayout(grdPayouts.DataKeys[row.RowIndex].Values["UID"].ToString());

                if (payout != null)
                {
                    clone = (AgentPayout)payout.Clone();
                    payout.UID = grdPayouts.DataKeys[row.RowIndex].Values["UID"].ToString();
                    payout.ID = Convert.ToInt32(row.Cells[0].Text);
                    payout.Level = Convert.ToInt32(row.Cells[2].Text);
                    payout.Dba = row.Cells[1].Text;
                    payout.Rate = DataLayer.Decimal2Field(((WebPercentEditor)row.Cells[3].FindControl("WebPercentEdit1")).Value);
                    payout.Payout = ((CheckBox)row.Cells[4].FindControl("ckhEnabled")).Checked;

                    User user = UserSessions.CurrentUser;
                    payout.UserUpdated = user.UserName;
                    data.UpdateAgentPayout(payout);
                    FormHandler.LogFormChanges(UserSessions.CurrentAgent.AgentDBA, payout.UID, Convert.ToInt32(payout.ID), clone, payout);
                }
            }
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
        string message = string.Empty;

        if (grdPayouts.Rows.Count > 0)
        {
            decimal total = 0;
            foreach (GridViewRow row in grdPayouts.Rows)
            {
                total += DataLayer.Decimal2Field(((WebPercentEditor)row.Cells[3].FindControl("WebPercentEdit1")).Value);
            }
            if (total != 100)
                message = "Payout percent must equal 100%. ";
        }

        //eluxa: disable routing# validation for now because we need to be able to enter Direct Debit and EFT
        //routing numbers
        //if (BankABA.Text.Trim().Length > 0 && !AchTransaction.ValidateCheckDigit(BankABA.Text.Trim()))
        //{
        //    message += "<br>Routing Number is Invalid.";
        //}

        if (message == string.Empty)
        {
            lblError.Text = "";
            return true;
        }
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
        grdPayouts.Enabled = btnSave.Enabled;

        FormHandler.SetControlEditMode(pnlDetails, btnSave.Enabled);
        ResidualPayoutThreshold.Enabled = btnSave.Enabled;
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
                    url = "~/SecureAgentManagementForms/frmAgentSplits.aspx?Adding=false";
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

    protected void grdPayouts_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                payout = 0.0M;
                break;
            case DataControlRowType.DataRow:

                WebNumericEditor lblPayout = (WebNumericEditor)e.Row.FindControl("WebPercentEdit1");
                lblPayout.Text = DataBinder.Eval(e.Row.DataItem, "Rate").ToString();

                CheckBox chkEnable = (CheckBox)e.Row.Cells[4].FindControl("ckhEnabled");
                chkEnable.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Payout").ToString());

                payout += lblPayout.Text.Equals(string.Empty) ? 0.00M : Convert.ToDecimal(lblPayout.Text.Replace('%', ' ').Trim());
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[3].Text = payout.ToString("0.00");//"c");
                break;
            default:
                break;
        }
    }

    protected void BankCountry_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (this.BankCountry.SelectedItem.Value.Trim().ToUpper().Equals("US"))
        {
            this.BankProvince.Visible = false;
            this.BankState.Visible = true;

        }

        else
        {
            this.BankProvince.Visible = true;
            this.BankState.Visible = false;
            this.BankProvince.Text = null;

        }
    }

    protected void BillingCountry_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (this.BillingCountry.SelectedItem.Value.Trim().ToUpper().Equals("US"))
        {
            this.BillingProvince.Visible = false;
            this.BillingState.Visible = true;

        }

        else
        {
            this.BillingProvince.Visible = true;
            this.BillingState.Visible = false;
            this.BillingProvince.Text = null;

        }
    }
}
