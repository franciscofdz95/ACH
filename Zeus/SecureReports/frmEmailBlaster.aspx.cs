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
using System.Collections.Generic;
using Infragistics.WebUI.WebDataInput;
using System.Text.RegularExpressions;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;


public partial class frmEmailBlaster : frmBaseSearch
{
    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }

    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(ViewState["EditMode"]);
            }
        }
        set { ViewState["EditMode"] = value; }

    }

    public bool Adding
    {
        get
        {
            if (ViewState["Adding"] != null)
            {
                return Convert.ToBoolean(ViewState["Adding"]);
            }
            else
            {
                return true;
            }
        }
        set { ViewState["Adding"] = Convert.ToBoolean(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!IsPostBack)
        {
            //Apply security settings
            FormHandler.SetSecurity(this.Page);

            //load all dropdownlists
            LookupTableHandler.LoadInternalUsers(UserUID, true);
            LookupTableHandler.LoadPortals(PortalUID, true);
            //LookupTableHandler.LoadAgentsNew(AgentUID, true);
            LookupTableHandler.LoadAgentTypes(AgentTypeUID, true);
            LookupTableHandler.LoadMerchantAppTypes(MerchantTypeUID, true);

            this.Search(true);

            // on pageload, we always show the search.
            pnlEdit.Visible = false;
            pnlSearch.Visible = true;

            // handle buttons
            btnEdit.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnRefresh.Enabled = false;

            btnSend.Enabled = false;
            btnCopy.Enabled = false;
        }
    }

    protected void grdEmail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    protected void grdEmail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbHC = (Label)e.Row.FindControl("lblHtmlContent");
            DataRowView drv = (DataRowView)e.Row.DataItem;

            string fulltext = Regex.Replace(drv["HTMLContent"].ToString(), @"<(.|\n)*?>", string.Empty);

            lbHC.Text = CommonUtility.Util.TruncateText(fulltext, 50);
            lbHC.ToolTip = CommonUtility.Util.TruncateText(fulltext, 500);

            ((Image)e.Row.FindControl("imgAtt")).Visible = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "HasAttachments"));

            ((LinkButton)e.Row.FindControl("btnView")).Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grdEmail.PageSize = this.PageSize;
        grdEmail.PageIndex = this.CurrentPage - 1;
        this.Search(false);
    }

    public override void Search(bool IsOnLoad)
    {
        //Populate search fields        
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter sp = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(sp, pnlSearch);
        }
        grdEmail.DataBind();
        pnlRecords.Visible = grdEmail.Rows.Count > 0;
        lblEmail.Visible = grdEmail.Rows.Count == 0;
    }

    protected void grdEmail_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.CurrentPage = 1;
        grdEmail.PageIndex = this.CurrentPage - 1;
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.Search(false);
    }

    private void FormClear()
    {
        this.CurrentPage = 1;
        grdEmail.PageIndex = this.CurrentPage - 1;
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);

        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        this.Search(false);
        wucEmailBlaster1.LoadValues(new EmailBlaster());
        rdExport.SelectedIndex = 0;
        wucAgentSelector.FormClear();
    }

    protected void grdEmail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow grdRow = null;
        DataCommunication data = DataAccess.DataCommunicationDao;

        if (e.CommandSource is LinkButton)
        {
            switch (e.CommandName)
            {
                case "View":

                    grdRow = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer);

                    EmailBlaster comm = data.GetEmailBlaster(grdEmail.DataKeys[grdRow.RowIndex].Values["EmailBlasterID"].ToString());
                    FormBinding.BindObjectToControls(comm, wucEmailBlaster1);
                    
                    grdEmail.SelectedIndex = grdRow.RowIndex;

                    this.UID = comm.EmailBlasterID;
                    UserSessions.CurrentEmailBlaster = comm;
//                    UserSessions.CurrentEmailBlasterID = this.UID;
                   
                    wucEmailBlaster1.LoadValues(comm);

                    pnlEdit.Visible = true;
                    pnlSearch.Visible = false;
                    this.SetButtons(eButtonSet.ReadOnly);

                    FormShow(comm.EmailBlasterID);
                    break;
            }
        }
    }

    protected IEnumerable<string> ValidateMeritusAlert()
    {
        if (string.IsNullOrEmpty(wucEmailBlaster1.CSubject))
        {
            yield return "Subject Required.";
        }

        if (string.IsNullOrEmpty(wucEmailBlaster1.CHTMLContent))
        {
            yield return "Html Body Required.";
        }

        if (wucEmailBlaster1.CrblEmail == "0")
        {
            if (wucEmailBlaster1.CAgentUID == false && wucEmailBlaster1.CMerchantAppTypeUID == 0 && !(wucEmailBlaster1.CPortalUID))
                yield return "Select at least one Portal or Agent or Bank.";
        }

        if (wucEmailBlaster1.CrblEmail == "1")
        {
            if (wucEmailBlaster1.CAgentTypeUID == 0)
                yield return "Select at least one Agent type.";
        }
    }

    protected bool FormDataCheck()
    {
        foreach (string str in this.ValidateMeritusAlert())
        {
            WucMessage1.AddMessageError(str);
        }

        return (WucMessage1.ErrorCount() == 0) ? true : false;
    }

    protected bool FormSave()
    {
        bool perform = false;

        if (!this.FormDataCheck())
            return false;

        EmailBlaster comm = null;

        if (this.Adding)
            comm = new EmailBlaster();
        else
        {
            comm = (EmailBlaster)UserSessions.CurrentEmailBlaster;
        }

        DataCommunication data = DataAccess.DataCommunicationDao;
        User user = UserSessions.CurrentUser;

        if (!this.Adding)
        {
            comm.UserUpdated = user.UserName;
            comm.DateUpdated = DateTime.Now;
        }
        else
        {
            comm.DateCreated = DateTime.Now;
            comm.UserCreated = user.UserName;
        }

        comm.IsMailMerge = Convert.ToBoolean(Convert.ToInt32(wucEmailBlaster1.CrblMail));
        string uid = wucEmailBlaster1.SaveEmail(comm);

        if (uid != "-1")
        {
            comm = data.GetEmailBlaster(uid);
            this.UID = comm.EmailBlasterID;
            UserSessions.CurrentEmailBlaster = comm;
            perform = true;
        }

        return perform;
    }

    protected void FormNew()
    {
        this.FormClear();
        this.Adding = true;
        this.EditMode = true;

        pnlEdit.Visible = true;
        pnlSearch.Visible = false;

        FormHandler.SetControlEditMode(pnlEdit, true);
        this.SetButtons(eButtonSet.Add);

        //by default properties listbox should be disabled.
        wucEmailBlaster1.PropertiesList.Enabled = false;
    }

    protected void FormShow(string ID)
    {
        EmailBlaster comm = new EmailBlaster();

        this.Search(false);
        if (ID != string.Empty)
            comm = DataAccess.DataCommunicationDao.GetEmailBlaster(ID);
        else
        {

            if (grdEmail.Rows.Count > 0)
            {
                grdEmail.SelectedIndex = 0;
                comm = DataAccess.DataCommunicationDao.GetEmailBlaster(grdEmail.DataKeys[0].Values["EmailBlasterID"].ToString());
            }
        }

        this.UID = comm.EmailBlasterID;
        UserSessions.CurrentEmailBlaster = comm;
        //UserSessions.CurrentEmailBlasterID = this.UID;

        wucEmailBlaster1.LoadValues(comm);
        FormHandler.SetControlEditMode(wucEmailBlaster1, this.EditMode);

        if (this.EditMode)
            wucEmailBlaster1.PropertiesList.Enabled = comm.IsMailMerge;
    }

    public void FormCancel()
    {

    }

    protected void FormEdit()
    {

    }

    private void CopyCommunication()
    {
        DataCommunication data = DataAccess.DataCommunicationDao;
        EmailBlaster comm = (EmailBlaster)UserSessions.CurrentEmailBlaster;
        comm.UserCreated = UserSessions.CurrentUser.UserName;
        comm.HasAttachments = false;

        data.InsertEmailBlaster(comm);
        this.UID = comm.EmailBlasterID;
        this.FormShow(this.UID);
        grdEmail.SelectedIndex = 0;
        WucMessage1.AddMessageStatus("Email template successfully copied");
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
                    if (this.Adding)
                    {
                        WucMessage1.AddMessageStatus("New Email template successfully added.");
                        Response.Redirect("~/SecureReports/frmEmailBlaster.aspx");
                    }
                    else
                    {
                        WucMessage1.AddMessageStatus("Email template successfully saved");
                        this.Adding = false;
                        this.EditMode = false;
                        FormShow(this.UID);
                        pnlSearch.Visible = false;
                        pnlEdit.Visible = true;
                        SetButtons(eButtonSet.ReadOnly);
                    }
                }
                break;

            case "Refresh":

                FormShow(this.UID);
                break;

            case "Cancel":

                if (this.Adding)
                {
                    this.SetButtons(eButtonSet.Search);
                    pnlSearch.Visible = true;
                    pnlEdit.Visible = false;
                }
                else
                {
                    if (this.EditMode)
                    {
                        this.EditMode = false;
                        this.SetButtons(eButtonSet.ReadOnly);
                        FormShow(this.UID);
                    }
                    else
                    {
                        // go back to search mode.
                        pnlSearch.Visible = true;
                        pnlEdit.Visible = false;
                        this.SetButtons(eButtonSet.Search);
                    }
                }

                // grdEmail.DataBind();
                this.Search(false);

                break;

            case "Edit":

                this.EditMode = true;
                this.Adding = false;
                this.FormShow(this.UID);
                FormHandler.SetControlEditMode(pnlEdit, true);
                this.SetButtons(eButtonSet.Edit);
                wucEmailBlaster1.PropertiesList.Enabled = !(wucEmailBlaster1.CrblMail == "0");
                break;

            case "Copy":
                this.CopyCommunication();
                break;

            case "Send":
                wucEmailBlaster1.SendMail(false);
                break;

            case "Send Test":
                wucEmailBlaster1.SendMail(true);
                break;
        }
    }

    private void SetButtons(eButtonSet eBS)
    {
        // clear all first.
        btnEdit.Enabled = false;
        btnAdd.Enabled = false;
        btnSave.Enabled = false;
        btnCancel.Enabled = false;
        btnRefresh.Enabled = false;
        btnSend.Enabled = false;

        btnCopy.Enabled = false;
        hypBack.Visible = false;

        pnlToolbar.Visible = false;
        switch (eBS)
        {
            case eButtonSet.Add:
                pnlToolbar.Visible = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                hypBack.Visible = true;

                break;

            case eButtonSet.Edit:
                pnlToolbar.Visible = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                hypBack.Visible = true;
                break;

            case eButtonSet.ReadOnly:
                pnlToolbar.Visible = true;
                btnEdit.Enabled = true;
                btnCancel.Enabled = true;
                btnRefresh.Enabled = true;
                hypBack.Visible = true;
                btnSend.Enabled = true;

                btnCopy.Enabled = true;
                btnAdd.Enabled = true;
                break;

            case eButtonSet.Search:
                btnAdd.Enabled = true;
                break;
        }
    }

    protected void odsEmails_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        prms.Add("@SortOrder", this.SortOrder);

        prms.Add("@SortDirection", SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch));

        if (CommonUtility.Util.if_s(EmailTemplateID.Text) != "")
        {
            prms.Add("@EmailBlasterID", EmailTemplateID.Text.ToString());
        }

        if (rblEmail.SelectedValue == "0")
            prms.Add("@EmailBlaster", "M");
        else if (rblEmail.SelectedValue == "1")
            prms.Add("@EmailBlaster", "A");

        if (CommonUtility.Util.if_s(Subject.Text) != "")
            prms.Add("@Subject", Subject.Text.Trim());

        if (UserUID.SelectedIndex > 0)
            prms.Add("@UserUID", UserUID.SelectedItem.Value);

        if (PortalUID.SelectedIndex > 0)
            prms.Add("@PortalUID", PortalUID.SelectedItem.Value);

        if (wucAgentSelector.m_AgentUID != string.Empty)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

        if (MerchantTypeUID.SelectedIndex > 0)
            prms.Add("@MerchantTypeUID", MerchantTypeUID.SelectedItem.Value);

        if (AgentTypeUID.SelectedIndex > 0)
            prms.Add("@AgentTypeUID", AgentTypeUID.SelectedItem.Value);

        grdEmail.PageSize = this.PageSize;
        grdEmail.PageIndex = this.CurrentPage - 1;

        e.InputParameters[0] = prms;

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            SearchParameter sp = new SearchParameter();
            FormBinding.BindControlsToObject(sp, pnlSearch);
            this.SearchParameters = sp;
        }

        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetEmailBlasterPagingRowCount(prms, this.PageSize, this.CurrentPage).ToString();

    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grdEmail.PageIndex + 1;
        }

        Search(false);
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        FormHandler.Export2Excel("Emails.xls", grdEmail);
    }

}
