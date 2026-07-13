using Infragistics.Web.UI.LayoutControls;
using Infragistics.WebUI.WebDataInput;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class wucAgentAllocationDetail : wucBaseDataEntry
{
    override protected void OnInit(EventArgs e)
    {

        base.OnInit(e);
        EnsureChildControls();
    }
    string _WindowCallID = "";
    static string sourceNameSelectedValue = "-1";
    /// <summary>
    /// the ticket control can either be called from a page, or a window. if its called from a window, then this value gets populated.
    /// </summary>
    public string WindowCallID
    {
        get { return _WindowCallID; }
        set { _WindowCallID = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.UID = CommonUtility.Util.if_s(Request.QueryString["AgentID"], null) + "&" + CommonUtility.Util.if_s(Request.QueryString["SourceName"], null);

        this.AgentID.Visible = true;
        this.AgentIDTxt.Visible = false;

        if (UserSessions.CurrentUser == null)
        {
            Response.Redirect("~/frmLogin.aspx");
        }

        if (!IsPostBack)
        {
            LookupTableHandler.LoadAgentAllocationStatus(StatusId, false);
            lblError.Text = "";

            //set Adding flag
            if (this.Adding)
            {
                this.FormNew();
            }
            else
            {
                if (this.UID == string.Empty)
                {
                    this.UID = (UserSessions.CurrentAgentAllocation == null) ? string.Empty : UserSessions.CurrentAgentAllocation.AgentID.ToString() + "&" + UserSessions.CurrentAgentAllocation.SourceName;
                }

                if (!string.IsNullOrEmpty(this.UID))
                {
                    this.FormShow(this.UID);
                    this.btnCancel.Enabled = true;
                }
            }

        }
    }

    //private void FillDropDown()
    //{
        //LookupTableHandler.LoadAgentSourceCodes(SourceNames, false);


    //}

    public override void FormShow(string uid)
    {
        string[] splitUID = uid.Split('&');
        string AgentID = splitUID[0];
        string sourceName = splitUID[1];
        DataAgent data = DataAccess.DataAgentDao;
        AgentAllocation agentAllocation = new AgentAllocation();
        Hashtable prms = new Hashtable();

        if (!this.Adding)
        {
            prms.Add("@AgentID", int.Parse(AgentID));
            prms.Add("@SourceName", sourceName);
            agentAllocation = data.GetSelectedAgentAllocation(prms);
            //pnlID.Visible = true;
            UserSessions.CurrentAgentAllocation = agentAllocation;

            FormBinding.BindObjectToControls(agentAllocation, pnlAgentAllocationDetail);
            FormHandler.SetControlEditMode(pnlAgentAllocationDetail, this.EditMode);

            this.AgentID.Visible = true;
            this.AgentIDTxt.Visible = false;
        }
    }

    public override void FormClear()
    {
        FormHandler.ClearAllControls(pnlAgentAllocationDetail);
    }

    public override bool FormSave()
    {

        bool perform = false;
        if (this.FormDataCheck())
        {
            DataAgent data = DataAccess.DataAgentDao;
            Hashtable prms = new Hashtable();
            try
            {

                prms.Add("@AgentDBADisplayName", AgentDBADisplayName.Text.Trim());
                prms.Add("@SourceName", SourceName.Text.Trim());
                prms.Add("@RepType", RepType.Text.Trim());
                prms.Add("@Allocation", Convert.ToInt32(Allocation.Text.Trim()));
                prms.Add("@AllocationBBVA", Convert.ToInt32(AllocationBBVA.Text.Trim()));
                prms.Add("@ReservePercentage", Convert.ToDecimal(ReservePercentage.Text.Trim()));
                //DM-320-Andres
                prms.Add("@StatusId", Convert.ToInt32(StatusId.SelectedValue));
                prms.Add("@CFGAllocations", Convert.ToInt32(CFGAllocations.Text.Trim()));

                if (!this.Adding)
                {

                    prms.Add("@AgentID", Convert.ToInt32(AgentID.Text.Trim()));
                    prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);
                    data.UpdateAgentAllocation(prms);
                    perform = true;
                }
                else
                {
                    prms.Add("@AgentID", Convert.ToInt32(AgentIDTxt.Text.Trim()));
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
                    data.InsertAgentAllocation(prms);
                    perform = true;

                }
            }
            catch (Exception)
            {
                perform = false;
                //if (this.Adding)
                //{
                //    AgentIDTxt.Visible = true;
                //    this.AgentID.Visible = false;
                //}
                //else
                //{
                //    AgentIDTxt.Visible = false;
                //    this.AgentID.Visible = true;
                //}

            }
        }
        else
        {
            if (!this.Adding)
            {
                this.AgentID.Visible = true;
                this.AgentIDTxt.Visible = false;
            }
            else
            {
                this.AgentID.Visible = false;
                this.AgentIDTxt.Visible = true;
            }
        }
        return perform;
    }

    public override void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;
        FormHandler.SetControlEditMode(pnlAgentAllocationDetail, this.EditMode);
        this.ToggleButtons();
        //FillDropDown();
        //pnlID.Visible = false;
        //imgCallbackDate.Enabled = true;

        this.AgentID.Visible = false;
        this.AgentIDTxt.Visible = true;
    }

    public override bool FormDelete()
    {
        bool perform = false;

        DataAgent data = DataAccess.DataAgentDao;
        string sourceName = string.Empty;
        Hashtable prms = new Hashtable();

        try
        {
            prms.Add("@AgentID", Convert.ToInt32(AgentID.Text.Trim()));
            prms.Add("@SourceName", SourceName.Text.Trim());
            prms.Add("@UserUpdated", UserSessions.CurrentUser.UserName);
            data.DeleteAgentAllocation(prms);
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
        int AgentIDValidation = 0;
        int AllocationValidation = 0;
        int AllocationBBVAValidation = 0;
        int AllocationCFGAllocations = 0;

        if (this.Adding)
        {
            AgentIDTxt.Visible = true;
            this.AgentID.Visible = false;
        }
        else
        {
            AgentIDTxt.Visible = false;
            this.AgentID.Visible = true;
        }
        if (!int.TryParse(AgentIDTxt.Text, out AgentIDValidation) && AgentIDTxt.Visible)
        {
            AgentIDTxt.Text = string.Empty;
            WucMessage1.AddMessageError("Please enter a valid Agent ID.");
        }
        if (string.IsNullOrWhiteSpace(AgentDBADisplayName.Text))
        {
            WucMessage1.AddMessageError("Please enter Agent DBA.");
        }
        if (string.IsNullOrWhiteSpace(RepType.Text))
        {
            WucMessage1.AddMessageError("Please enter Rep Type.");
        }
        if (!int.TryParse(Allocation.Text, out AllocationValidation))
        {
            Allocation.Text = string.Empty;
            WucMessage1.AddMessageError("Please enter a valid WFB Allocations.");
        }
        if (string.IsNullOrWhiteSpace(SourceName.Text))
        {
            WucMessage1.AddMessageError("Please enter Source Name.");
        }
        decimal reservePercentage = CommonUtility.Util.if_dec(ReservePercentage.Text, 0);
        if (reservePercentage <= 0)
        {
            WucMessage1.AddMessageError("Please enter a valid Reserve %.");
        }
        if (!int.TryParse(AllocationBBVA.Text, out AllocationBBVAValidation))
        {
            AllocationBBVA.Text = string.Empty;
            WucMessage1.AddMessageError("Please enter a valid BBVA Allocations.");
        }
        if (!int.TryParse(CFGAllocations.Text, out AllocationCFGAllocations))
        {
            CFGAllocations.Text = string.Empty;
            WucMessage1.AddMessageError("Please enter a valid CFG Allocation.");
        }
        if (!string.IsNullOrWhiteSpace(SourceName.Text))
        {
            int ValidateAgentId = 0;

            if ((!string.IsNullOrWhiteSpace(AgentIDTxt.Text) && AgentIDTxt.Visible))
            {
                ValidateAgentId = CommonUtility.Util.if_i((AgentIDTxt.Text), 0);
            }
            else if (!string.IsNullOrWhiteSpace(AgentID.Text) && AgentID.Visible)
            {
                ValidateAgentId = CommonUtility.Util.if_i((AgentID.Text), 0);
            }


            string existRecordCount = string.Empty;
            DataAgent data = DataAccess.DataAgentDao;
            Hashtable param = new Hashtable();

            bool isSourceNameMatched = false;
            if (UserSessions.CurrentAgentAllocation != null)
            {
                isSourceNameMatched = UserSessions.CurrentAgentAllocation.SourceName.ToLower().Trim() == SourceName.Text.ToLower().Trim();
            }

            if (this.Adding || (this.Adding == false && isSourceNameMatched == false))
            {
                param.Add("@AgentID", ValidateAgentId);
                param.Add("@SourceName", SourceName.Text.Trim());
                existRecordCount = data.CheckAgentAllocationExists(param);
                if (CommonUtility.Util.if_i(existRecordCount, 0) > 0)
                {
                    WucMessage1.AddMessageError("Agent ID and Source Name already in use.");
                }
            }
        }
        if (WucMessage1.ErrorCount() == 0)
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
        lblError.Text = string.Empty;
        this.EditMode = false;
        FormShow(this.UID);
        this.Adding = false;
        this.ToggleButtons();


        Response.Redirect("~/SecureQualityForms/frmAgentAllocationSearch.aspx");
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
            url = "~/SecureQualityForms/frmAgentAllocationSearch.aspx";
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


            if (!string.IsNullOrEmpty(this._WindowCallID))
            {
                ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
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
        return Page.ClientScript.GetPostBackEventReference(btnSave, string.Empty);
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.CommandName)
        {
            case "Add":

                if (string.IsNullOrEmpty(this.WindowCallID))
                {
                    Response.Redirect(WebUtil.GetMyUrl("Adding=true"));
                }
                else
                {
                    Response.Redirect(WebUtil.GetMyUrl("Adding=true&WindowCallID=" + this.WindowCallID));
                }

                this.FormNew();

                break;


            case "Save":
                {

                    if (this.FormSave())
                    {
                         string prm=string.Empty;
                        string[] uid = this.UID.Split('&');
                        if(!string.IsNullOrWhiteSpace(uid[0]))
                         prm = "AgentId=" + uid[0];
                        else
                            prm = "AgentId=" +AgentIDTxt.Text;
                       
                        if (this.Adding)
                            prm = prm + "&" + "Action=Added";
                        else
                            prm = prm  +"&" + "Action=Updated";
                        this.EditMode = false;
                        //FormHandler.DisplayMessage(this.Page.ClientScript, "Record saved/edited successfully");
                        
                        url = "~/SecureQualityForms/frmAgentAllocationSearch.aspx?Adding=false";
                        url += "&" + prm;
                        Response.Redirect(url);
                    }
                    break;
                }

                break;

            case "Refresh":
                FormShow(this.UID);

                //if (UserSessions.CurrentCRM != null)
                //{
                //    Hashtable prms = new Hashtable();
                //    prms.Add("@CRMUID", UserSessions.CurrentCRM.CRMUID);
                //    Response.Redirect(WebUtil.GetMyUrl("Adding=false&CRMUID=" + UserSessions.CurrentCRM.CRMUID));
                //}

                break;

            case "Cancel":

                if (!string.IsNullOrEmpty(this._WindowCallID))
                {
                    ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
                }
                else
                {

                    if (this.UID == string.Empty)
                    {
                        this.FormClose(sender, e);
                    }
                    else
                    {
                        this.FormCancel();
                    }
                }

                break;

            case "Edit":

                this.EditMode = true;
                this.FormShow(this.UID);
                this.ToggleButtons();
                this.btnCancel.Enabled = true;
                break;
            case "Delete":
                {
                    if (this.FormDelete())
                    {
                        this.EditMode = false;
                        url = "~/SecureQualityForms/frmAgentAllocationSearch.aspx?Adding=false";
                        url += "&Action=Delete";
                        Response.Redirect(url);
                    }
                }
                break;

        }
    }
}
