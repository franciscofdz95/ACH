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
using System.Text;
using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebHtmlEditor;
using CommonUtility;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects.Notify;

public partial class wucNotes : wucBaseSearch
{
    Hashtable prms = new Hashtable();
    int m_page = 0;

    private bool _Export2Excel = false;

    public int frmPage
    {
        set { m_page = value; }
        get { return m_page; }
    }

    public void LoadMerchantNotes()
    {
        prms = new Hashtable();
        if (string.IsNullOrEmpty(this.UID) && UserSessions.CurrentMerchantApp != null)
            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

        if (!string.IsNullOrEmpty(this.UID))
        {
            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);

            if (string.IsNullOrWhiteSpace(SortOrder))
                SortOrder = "MerchantNoteID";
            prms.Add("@SortOrder", this.SortOrder);
            prms.Add("@SortDirection", this.SortDirectionSearch);

            prms.Add("@MerchantAppUID", this.UID);

            if (ddpNoteType.SelectedIndex > 0)
                prms.Add("@type", ddpNoteType.SelectedItem.Text);

            if ((frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING) && cboNoteCode.SelectedIndex > 0)
                prms.Add("@Subject", cboNoteCode.SelectedItem.Text);

            prms.Add("@UserName", UserSessions.CurrentUser.UserName);

            if (!string.IsNullOrWhiteSpace(Search.Text))
                prms.Add("@SearchNote", Search.Text);

            if (UserSessions.CurrentUser.IsAgent)
                prms.Add("@View_Agent", true);
            else if (UserSessions.CurrentUser.IsBank)
                prms.Add("@View_Bank", true);

            if (UserSessions.CurrentUser.IsInternal)
                prms.Add("@View_MPSALL", true);

            grdNotes.DataBind();


            int cnt = DataMerchantAppPaging.GetMerchantNotesPagingCount(prms, this.PageSize, this.CurrentPage, this.grdNotes.ID);
            recordCnt.Text = "Total records found: " + cnt.ToString();
            NoRecords.Visible = !(cnt > 0);
            pnlRecords.Visible = (cnt > 0);
        }
    }

    protected void odsNotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (string.IsNullOrEmpty(this.UID) && UserSessions.CurrentMerchantApp != null)
            this.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
        else if (string.IsNullOrEmpty(this.UID))
            this.UID = "00000000-0000-0000-0000-000000000000";

        if (prms.Count <= 0)
            prms.Add("@MerchantAppUID", this.UID);
        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grdNotes.ID;
    }

    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }

    public string Module
    {
        get { return Convert.ToString(ViewState["Module"]); }
        set { ViewState["Module"] = Convert.ToString(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            this.LoadMerchantNotes();
            LoadNoteTypes();

            MerchantApp app = UserSessions.CurrentMerchantApp;

            string m_StatusName = string.Empty;
            bool isACHOnly = (app.AchID > 0 && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY);

            if (isACHOnly && UserSessions.ActiveAchMerchant != null)
                m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
            else
                m_StatusName = app.StatusName;

            if (frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING)
            {
                if (m_StatusName.Substring(0, 2) == "SS")// AP Queue is now changed to RM
                {
                    ListHandler.ListFindItem(cboNoteCode, "52b65e1b-d418-4834-8f54-dcd9849df78e");
                    ListHandler.ListFindItem(ddpNoteType, "MS");
                    View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
                    View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
                    Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
                }
                else
                {
                    ListHandler.ListFindItem(cboNoteCode, "7fbcf694-4b03-4f5a-92b0-7c9500225c4e");
                    ListHandler.ListFindItem(ddpNoteType, "UW");
                    View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
                    View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
                    Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
                }

                ddpNoteType.Enabled = false;

                if (cboNoteCode.Items.Count == 0)
                    LookupTableHandler.LoadMerchant_NoteCodes(cboNoteCode, false, "");

                cboNoteCode.Enabled = false;
                Subject.Text = cboNoteCode.SelectedItem.Text;

                Subject.Enabled = false;
                chkApplySameLegalName.Visible = false;//RequiresCallback.Enabled = false;
            }


            // if you're a bank, you don't need to see this panel of checkboxes.
            pnlNotesCBRow.Visible = !UserSessions.CurrentUser.IsBank;
            pnlNotesCBRowInWindow.Visible = !UserSessions.CurrentUser.IsBank;
        }
    }

    private void LoadNoteTypes()
    {
        ddpNoteType.Items.Clear();
        DataSet ds = new DataSet();
        DataMerchantApp da = new DataMerchantApp();

        Hashtable prms = new Hashtable();
        prms.Add("@UserUID", UserSessions.CurrentUser.UID);

        ds = da.GetNoteTypes(prms);
        ddpNoteType.DataSource = ds;

        ddpNoteType.DataTextField = "TypeDesc";
        ddpNoteType.DataValueField = "Type";
        ddpNoteType.DataBind();
        ddpNoteType.Items.Insert(0, "ALL");
        ddpNoteType.Items[0].Selected = true;
    }

    protected void grdNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                HtmlAnchor anc1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anc2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                if (!this._Export2Excel)
                {
                    anc1.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Open');");
                    anc2.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Close');");
                }
                else
                {
                    //hide more/less since we're exporting to excel
                    anc1.Visible = false;
                    anc2.Visible = false;
                }

                break;

            case DataControlRowType.DataRow:

                if ((frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING))
                {
                    e.Row.Cells[1].Enabled = false;
                    e.Row.Cells[5].Enabled = false;
                    e.Row.Cells[6].Enabled = false;
                }

                //Convert html to text and display on the grid and tool tip. The html should still show on click of note.
                string note = (DataBinder.Eval(e.Row.DataItem, "Notes").ToString());
                note = WebUtil.ConvertHtml(Server.HtmlDecode(note));
                e.Row.Cells[4].Attributes.Add("title", note);


                Label Notes1 = ((Label)e.Row.Cells[4].FindControl("Notes1"));
                Label Notes2 = ((Label)e.Row.Cells[4].FindControl("Notes2"));

                HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                if (!this._Export2Excel)
                {
                    if (note.Length > 45)
                    {
                        Notes1.Text = CommonUtility.Formatting.nl2br(note.Substring(0, 45).Trim()) + "...  ";
                        anchor1.Attributes.Add("style", "display:inline;cursor: pointer;");
                    }
                    else
                    {
                        Notes1.Text = CommonUtility.Formatting.nl2br(note.Trim()) + "  ";
                        anchor1.Attributes.Add("style", "dispaly:none;");
                    }

                    anchor1.Attributes.Add("onclick", "return OpenClose('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Open');");
                    anchor2.Attributes.Add("onclick", "return OpenClose('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Close');");
                }
                else
                {
                    anchor1.Visible = false;
                    anchor2.Visible = false;
                }

                Notes2.Text = CommonUtility.Formatting.nl2br(note);

                if (UserSessions.CurrentUser.IsBank)
                {
                    grdNotes.Columns[1].Visible = false; //Email Link
                    grdNotes.Columns[5].Visible = false; //Call Back
                    grdNotes.Columns[6].Visible = false; //Is Private

                }

                e.Row.Cells[9].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[9].Text);

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    protected void grdNotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string subject = string.Empty;
        string notes = string.Empty;
        string dba = string.Empty;
        GridViewRow grdRow = null;

        if (e.CommandSource is LinkButton)
        {
            if (grdNotes != null)
            {
                grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                notes = grdNotes.DataKeys[grdRow.RowIndex].Values["Notes"].ToString();
                subject = grdNotes.DataKeys[grdRow.RowIndex].Values["Subject"].ToString();
                dba = grdNotes.DataKeys[grdRow.RowIndex].Values["BusinessDBAName"].ToString();
                chkInternal.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_MPSAll"]);
                chkAgent.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Agent"]);
                chkMerchant.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Merchant"]);
            }
        }
        else
            return;

        switch (e.CommandName)
        {
            case "View":
                txtSubject.Text = subject;
                //Show the Binded html so that it is readable. Decodeing the encoed value so that the text is clear.
                txtNotes.Text = CommonUtility.Formatting.nl2br(System.Web.HttpUtility.HtmlDecode(notes));
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;

            case "Email":
                ((TextBox)WucEmail1.FindControl("Subject")).Text = "DBA : " + dba + " ( " + subject + " )";
                ((WebHtmlEditor)WucEmail1.FindControl("txtHTMLBody")).Text = Server.HtmlDecode(notes);
                WucEmail1.AgentEmail = hdnAgent.Value;
                WucEmail1.MerchantEmail = hdnMerchant.Value;
                WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;

            case "Ticket":

                string TicketUID = ((LinkButton)grdRow.FindControl("hypTID")).CommandArgument.Trim();
                ((LinkButton)grdRow.FindControl("hypTID")).Attributes.Add("onclick", "window.open('../../SecureTicketForms/frmTicketPopup.aspx?TicketUID=" + TicketUID + "&Adding=false');");
                break;
        }
    }

    protected void grdNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdNotes.PageIndex = e.NewPageIndex;
        this.CurrentPage = e.NewPageIndex + 1;
        this.LoadMerchantNotes();
    }

    protected void btnAddNotes_Click(object sender, EventArgs e)
    {
        if (this.AddNotes())
        {
            Notes.Text = string.Empty;
            if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT && frmPage != (int)CONDITIONS.CREDITUNDERWRITING && frmPage != (int)CONDITIONS.APPPROCESSING)
            {
                Subject.Text = string.Empty;
                cboNoteCode.SelectedIndex = 0;
                View_MPSAll.Checked = View_Agent.Checked = false;
            }

            LoadMerchantNotes();
        }
    }

    protected void btnClearNotes_Click(object sender, EventArgs e)
    {
        if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT && frmPage != (int)CONDITIONS.CREDITUNDERWRITING && frmPage != (int)CONDITIONS.APPPROCESSING)
            cboNoteCode.SelectedIndex = 0;

        Notes.Text = string.Empty;
        Subject.Text = string.Empty;
        Email_Agent.Checked = false;
        View_MPSAll.Checked = false;
        View_Agent.Checked = false;
        lblErr.Text = "";
    }

    public void chkCallback_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string noteid = string.Empty, ID = string.Empty;
            string merchantappuid = string.Empty;
            bool requirecallback = false;

            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            User user = UserSessions.CurrentUser;

            GridViewRow grdRow = ((GridViewRow)((CheckBox)sender).NamingContainer);
            int rowIndex = grdRow.RowIndex;
            noteid = grdNotes.DataKeys[rowIndex].Values["UID"].ToString();

            MerchantNotes ObjMerchantNotes = new MerchantNotes();
            prms = new Hashtable();

            ObjMerchantNotes = data.GetMerchantNotes(noteid, user.UserName);
            requirecallback = ObjMerchantNotes.RequiresCallback = Convert.ToBoolean(((CheckBox)grdRow.FindControl("chkCallback")).Checked);
            ObjMerchantNotes.UserUpdated = user.UserName;
            string name = grdRow.Cells[9].Text;
            merchantappuid = ObjMerchantNotes.MerchantAppUID;
            ID = ObjMerchantNotes.MerchantNoteID;

            data.UpdateMerchantNotes(ObjMerchantNotes);

            //Add notes when the requirecallback is true
            ObjMerchantNotes = new MerchantNotes();

            if (requirecallback == true)
                ObjMerchantNotes.Notes = "A callback is required by " + name;
            else
                ObjMerchantNotes.Notes = "The callback or issue is resolved from note id - " + ID;

            ObjMerchantNotes.Subject = "Miscellaneous";
            ObjMerchantNotes.MerchantAppUID = merchantappuid;
            ObjMerchantNotes.UserCreated = user.UserName;
            ObjMerchantNotes.RequiresCallback = false;
            ObjMerchantNotes.View_Agent = false;
            ObjMerchantNotes.View_Bank = false;
            ObjMerchantNotes.View_MPSAll = true;
            ObjMerchantNotes.Email_Agent = false;

            data.InsertMerchantNotes(ObjMerchantNotes);

            //Load the merchant notes
            LoadMerchantNotes();
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    protected void ddpNoteType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMerchantNotes();
    }

    protected void cboNoteCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboNoteCode.SelectedIndex > 0)
        {
            Subject.Text = cboNoteCode.SelectedItem.Text + ":";
            View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
            View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
            Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
        }
        else
        {
            Subject.Text = cboNoteCode.SelectedItem.Text;
            View_Agent.Checked = false;
            View_MPSAll.Checked = false;
            Email_Agent.Checked = false;
        }

    }

    public void LoadNotes(string MerchantAppUID, string module)
    {
        LookupTableHandler.LoadMerchant_NoteCodes(cboNoteCode, false, module);
        
        MerchantApp app = UserSessions.CurrentMerchantApp;

        string m_StatusName = string.Empty;
        bool isACHOnly = (app.AchID > 0 && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY);

        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
            m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
        else
            m_StatusName = app.StatusName;
        
        if (frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING)
        {
            if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT)
            {
                if (m_StatusName.Substring(0, 2) == "SS")// AP Queue is now changed to RM
                    ListHandler.ListFindItem(cboNoteCode, "52b65e1b-d418-4834-8f54-dcd9849df78e");
                else
                    ListHandler.ListFindItem(cboNoteCode, "7fbcf694-4b03-4f5a-92b0-7c9500225c4e");
            }
            else
                ListHandler.ListFindItem(cboNoteCode, "c2fab379-84d2-4c37-97b5-9d3aa195b065");
        }

        this.UID = MerchantAppUID;
        this.Module = Module;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        grdNotes.PageSize = this.PageSize;
        this.LoadMerchantNotes();
    }

    private bool AddNotes()
    {
        //There is a reason this is encoded twice. 
        //As This page and Ticket Notes page are related, the Infragistics control encodes the potentially unsafe text twice by default.
        //To accomplish the binding functionality on the grids, and to make the application work consistant, we need to encode the text here twice, 
        //This will eliminate any potentially unsafe code.  We dont have to do this on Agent Notes because there is no relation ship there with ticket notes.

        string notes = Server.HtmlEncode(Server.HtmlEncode(Notes.Text.Trim()));
        string error = string.Empty;

        if (string.IsNullOrWhiteSpace(notes))
            error = "Please enter notes.<br>";
        if (cboNoteCode.SelectedIndex <= 0)
            error += "Please select notecode.<br>";

        if (!string.IsNullOrWhiteSpace(error))
        {
            lblErr.Text = error;
            return false;
        }

        try
        {
            MerchantFacade facade = new MerchantFacade();
            MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
            User user = UserSessions.CurrentUser;
            string subject = string.Empty;

            if (cboNoteCode.SelectedIndex > 0)
                subject = cboNoteCode.SelectedItem.Text;

            MerchantNotes ObjMerchantNotes = new MerchantNotes();
            ObjMerchantNotes.MerchantAppUID = this.UID;
            ObjMerchantNotes.Subject = subject;
            ObjMerchantNotes.Notes = notes;
            ObjMerchantNotes.View_Agent = View_Agent.Checked;
            ObjMerchantNotes.View_Bank = true;
            ObjMerchantNotes.View_MPSAll = View_MPSAll.Checked;
            ObjMerchantNotes.Email_Agent = Email_Agent.Checked;
            ObjMerchantNotes.UserCreated = user.UserName;

            if (chkApplySameLegalName.Checked == false)
                DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);
            else
                DataAccess.DataMerchantAppDao.InsertMerchantNotesLegalName(ObjMerchantNotes);

            if (ObjMerchantNotes.Email_Agent)
            {
                string AlertName = string.Empty;
                string AlertNote = string.Empty;
                string deptType = string.Empty;
                string From = string.Empty;
                string AlertTypeName = string.Empty;

                From = UserSessions.CurrentUser.Email;

                if (app.PLEnabled)
                    From = app.PLEmail;

                deptType = subject.Split('-')[0].ToString().Trim();
                AlertName = subject;

                AgentNotification notification = null;

                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    if (deptType == "Customer Service" || deptType == "Tech" || deptType == "General")
                    {
                        if (cboNoteCode.SelectedValue == "4d1b6e24-d113-43f1-95a7-269c26082d30")
                            AlertTypeName = " - Training Scheduled";
                        else if (cboNoteCode.SelectedValue == "4d1b6e24-d113-43f1-95a7-269c26082d31")
                            AlertTypeName = " - Training Completed";
                        else if (cboNoteCode.SelectedValue == "6b7fb196-d0bf-4817-b8de-dc70affff744")
                            AlertTypeName = " - Equipment Shipped";

                        notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.ClientServiceUpdate, app.PrivateLabelUID);
                    }

                    else if (deptType == "Risk")
                    {
                        notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.RiskUpdate, app.PrivateLabelUID);
                    }

                    else if (deptType == "Collections")
                    {
                        notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.Collections, app.PrivateLabelUID);
                    }

                    if (notification != null && notification.Enabled)
                    {
                        if ((deptType == "Risk") || (deptType == "Collections"))
                        {
                            User firstTeamRep = DataMerchantApp.GetInstance().GetMerchantUser(app.MerchantAppUID, Constants.ROLE_FIRSTTEAM);

                            if (firstTeamRep != null)
                            {
                                notification.AddBccRecipient(firstTeamRep.Email);
                            }
                        }

                        notification.UserName = UserSessions.CurrentUser.UserName;
                        //Decode the note once and send to the email. As we have encoded it twice while saving.
                        //This will ensure that email is sent out correctly to the agent.
                        AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "", notification.Name, CommonUtility.Formatting.nl2br(Server.HtmlDecode(notes)), AlertTypeName, Portal: ePortals.ZEUS);
                    }
                }
            }

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadMerchantNotes();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.CurrentPage = 1;
        grdNotes.PageSize = this.PageSize;
        LoadMerchantNotes();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        this._Export2Excel = true;

        if (rdExport.SelectedValue.Equals("1"))
        {

            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = this.grdNotes.PageIndex + 1;
        }

        grdNotes.PageSize = this.PageSize;
        LoadMerchantNotes();

        FormHandler.Export2Excel("MerchantNotesList.xls", this.grdNotes);

    }

    protected void grdNotes_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (this.SortOrder != e.SortExpression)
            this.SortDirectionSearch = e.SortDirection;

        this.SortOrder = e.SortExpression;

        this.LoadMerchantNotes();
    }



}
