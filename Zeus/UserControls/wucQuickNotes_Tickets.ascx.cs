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
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Collections.Generic;
using PaymentXP.BusinessObjects.Notify;

public partial class wucQuickNotes_Tickets : wucBaseSearch
{
    Hashtable prms = new Hashtable();
    int m_page = 0;

    public int frmPage
    {
        set { m_page = value; }
        get { return m_page; }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd.PageIndex = e.NewPageIndex;

        LoadTickets();
    }

    protected void grdNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdNotes.PageIndex = e.NewPageIndex;

        LoadMerchantNotes();
    }

    protected void grdNotes_Sorting(object sender, GridViewSortEventArgs e)
    {
        grdNotes.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;

        this.LoadMerchantNotes();
    }

    public void LoadMerchantNotes()
    {
        prms = null;

        if (!string.IsNullOrEmpty(this.UID))
        {
            //Load notes
            prms = new Hashtable();
            prms.Add("@PageSize", 4);
            prms.Add("@CurrentPage", grdNotes.PageIndex + 1);
            if (string.IsNullOrWhiteSpace(this.SortOrder))
                this.SortOrder = "DateCreated";
            prms.Add("@SortOrder", this.SortOrder);
            prms.Add("@SortDirection", ConvertSortDirectionToSql(this.SortDirectionSearch));
            prms.Add("@UserName", UserSessions.CurrentUser.UserName);
            prms.Add("@MerchantAppUID", this.UID);

            if (UserSessions.CurrentUser.IsAgent)
                prms.Add("@View_Agent", true);
            else if (UserSessions.CurrentUser.IsBank)
                prms.Add("@View_Bank", true);

            if (UserSessions.CurrentUser.IsInternal)
                prms.Add("@View_MPSALL", true);

            grdNotes.DataBind();
            NoRecords.Visible = !(grdNotes.Rows.Count > 0);
            Records.Visible = grdNotes.Rows.Count > 0;

            //Load tickets
            LoadTickets();
        }
        else
        {
            clearAll();
        }
    }

    private void LoadTickets()
    {
        //Load Tickets
        prms = new Hashtable();
        prms.Add("@PageSize", 4);
        prms.Add("@CurrentPage", grd.PageIndex + 1);
        prms.Add("@SortOrder", "TicketID");
        prms.Add("@SortDirection", "1");
        prms.Add("@MerchantAppUID", this.UID);

        grd.DataBind();
        grd.Sort("DateCreated", SortDirection.Descending);
        pnlNoRecords.Visible = !(grd.Rows.Count > 0);
        pnlRecords.Visible = grd.Rows.Count > 0;

        btnAddNotes.Enabled = true;
        btnClearNotes.Enabled = true;
        btnTicket.Enabled = true;
    }

    private int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = 0;
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    protected void odsNotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {         
        if (string.IsNullOrEmpty(this.UID))
            this.UID = "00000000-0000-0000-0000-000000000000";

        if (prms.Count <= 0)
            prms.Add("@MerchantAppUID", this.UID);
        e.InputParameters[0] = prms;
        e.InputParameters[3] = "Notes";
    }

    protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (string.IsNullOrEmpty(this.UID))
            this.UID = "00000000-0000-0000-0000-000000000000";

        if (prms.Count <= 0)
            prms.Add("@MerchantAppUID", this.UID);
        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;
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
            
            MerchantApp app = UserSessions.CurrentMerchantApp;

            string m_StatusName = string.Empty;
            bool isACHOnly = false;
            if (app != null)
            {
                isACHOnly = (app.AchID > 0 && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY);

                if (isACHOnly && UserSessions.ActiveAchMerchant != null)
                    m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
                else
                    m_StatusName = app.StatusName;
            }

            if (frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING)
            {
                if (m_StatusName.Substring(0, 2) == "SS")// AP Queue is now changed to RM
                {
                    ListHandler.ListFindItem(cboNoteCode, "52b65e1b-d418-4834-8f54-dcd9849df78e");
                    View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
                    View_Bank.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Bank;
                    View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
                    Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
                }
                else
                {
                    ListHandler.ListFindItem(cboNoteCode, "7fbcf694-4b03-4f5a-92b0-7c9500225c4e");
                    View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
                    View_Bank.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Bank;
                    View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
                    Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
                }

                if (cboNoteCode.Items.Count == 0)
                    LookupTableHandler.LoadMerchant_NoteCodes(cboNoteCode, false, "");

                cboNoteCode.Enabled = false;
                Subject.Value = cboNoteCode.SelectedItem.Text;

                chkApplySameLegalName.Visible = RequiresCallback.Enabled = false;
            }
        }

        if (UserSessions.CurrentMerchantApp != null)
        {
            btnTicket.Enabled = true;
            btnTicket.Attributes.Add("onclick", string.Format("NewTicketMerchant('{0}')", UserSessions.CurrentMerchantApp.MerchantAppUID));
        }
        else
        {
            btnTicket.Enabled = false;
        }
    }

    protected void grdNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                break;
            case DataControlRowType.DataRow:

                e.Row.Cells[1].Attributes.Add("title", Server.HtmlDecode(e.Row.Cells[1].Text));
                e.Row.Cells[1].Text = "<div style='width:150px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;'><NOBR>" + Server.HtmlDecode(e.Row.Cells[1].Text) + "</NOBR></div>";

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
                chkInternal.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_MPSAll"]);
                chkAgent.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Agent"]);                
            }
        }
        else
            return;

        switch (e.CommandName)
        {
            case "View":

                txtSubject.Text = subject;
                txtNotes.Text = Server.HtmlEncode(notes);
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

                break;
        }
    }

    protected void btnAddNotes_Click(object sender, EventArgs e)
    {
        if (this.AddNotes())
        {
            Notes.Text = string.Empty;
            if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT && frmPage != (int)CONDITIONS.CREDITUNDERWRITING && frmPage != (int)CONDITIONS.APPPROCESSING)
            {
                Subject.Value = string.Empty;
                cboNoteCode.SelectedIndex = 0;
                RequiresCallback.Checked = View_MPSAll.Checked = View_Agent.Checked = View_Bank.Checked = false;
                LoadMerchantNotes();
            }
            else
                LoadMerchantNotes();
        }
    }

    protected void btnClearNotes_Click(object sender, EventArgs e)
    {
        if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT && frmPage != (int)CONDITIONS.CREDITUNDERWRITING && frmPage != (int)CONDITIONS.APPPROCESSING)
            cboNoteCode.SelectedIndex = 0;

        Notes.Text = string.Empty;
        Subject.Value = string.Empty;
        View_MPSAll.Checked = false;
        View_Agent.Checked = false;
        View_Bank.Checked = false;
        Email_Agent.Checked = false;

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
            Subject.Value = cboNoteCode.SelectedItem.Text + ":";
            View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
            View_Bank.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Bank;
            View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
            Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
        }
        else
        {
            Subject.Value = cboNoteCode.SelectedItem.Text;
            View_Agent.Checked = false;
            View_Bank.Checked = false;
            View_MPSAll.Checked = false;
            Email_Agent.Checked = false;
        }

    }

    public void LoadNotes(string MerchantAppUID, string module)
    {
        LookupTableHandler.LoadMerchant_NoteCodes(cboNoteCode, false, module);
        
         MerchantFacade facade = new MerchantFacade();
         MerchantApp app = facade.GetMerchantAppZeus(MerchantAppUID);

        string m_StatusName = string.Empty;
        bool isACHOnly = (app.AchID > 0 && app.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY);

        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
            m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
        else
            m_StatusName = app.StatusName;

        if (!string.IsNullOrEmpty(MerchantAppUID))
        {
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

            this.hdnMerchantUID.Value = MerchantAppUID;
            this.UID = MerchantAppUID;
            this.Module = Module;

            this.LoadMerchantNotes();

        }
        else
        {
            this.UID = string.Empty;
            clearAll();
        }
    }

    public bool AddNotes()
    {
        string notes = Server.HtmlEncode(Notes.Text.Trim());
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

        if (!string.IsNullOrWhiteSpace(this.UID))
        {
            MerchantFacade facade = new MerchantFacade();
            MerchantApp app = facade.GetMerchantAppZeus(this.UID);
            User user = UserSessions.CurrentUser;
            string subject = string.Empty;
            bool send = false;

            if (cboNoteCode.SelectedIndex > 0)
                subject = cboNoteCode.SelectedItem.Text;

            MerchantNotes ObjMerchantNotes = new MerchantNotes();
            ObjMerchantNotes.MerchantAppUID = this.UID;
            ObjMerchantNotes.Subject = subject;
            ObjMerchantNotes.Notes = notes;
            ObjMerchantNotes.RequiresCallback = RequiresCallback.Checked;
            ObjMerchantNotes.View_Agent = View_Agent.Checked;
            ObjMerchantNotes.View_Bank = View_Bank.Checked;
            ObjMerchantNotes.View_MPSAll = View_MPSAll.Checked;
            ObjMerchantNotes.Email_Agent = Email_Agent.Checked;
            ObjMerchantNotes.UserCreated = user.UserName;

            if (chkApplySameLegalName.Checked == false)
                DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);
            else
                DataAccess.DataMerchantAppDao.InsertMerchantNotesLegalName(ObjMerchantNotes);

            if (ObjMerchantNotes.Email_Agent)
            {
                string AgentEmail = string.Empty;
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

                        AgentNotification notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.ClientServiceUpdate, app.PrivateLabelUID);

                        if (notification.Enabled)
                        {
                            notification.UserName = UserSessions.CurrentUser.UserName;
                            AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "", notification.Name, notes, AlertTypeName, Portal: ePortals.ZEUS);
                        }
                    }

                    else if (deptType == "Risk")
                    {
                        AgentNotification notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.RiskUpdate, app.PrivateLabelUID);

                        if (notification.Enabled)
                        {
                            notification.UserName = UserSessions.CurrentUser.UserName;
                            AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "", notification.Name, notes, AlertTypeName                , Portal: ePortals.ZEUS);
                        }
                    }

                    else if (deptType == "Collections")
                    {
                        AgentNotification notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.Collections, app.PrivateLabelUID);

                        if (notification.Enabled)
                        {
                            notification.UserName = UserSessions.CurrentUser.UserName;
                            AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "", notification.Name, notes, AlertTypeName, Portal: ePortals.ZEUS);
                        }
                    }
                }
            }

            return true;

        }

        return false;
    }


    //Ticket related coding

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lbtnTicket");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();

                btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");
                break;
        }
    }

    private void clearAll()
    {
        this.UID = string.Empty;
        this.hdnMerchantUID.Value = string.Empty;

        pnlNoRecords.Visible = true;
        pnlRecords.Visible = false;

        NoRecords.Visible = true;
        Records.Visible = false;

        btnAddNotes.Enabled = false;
        btnClearNotes.Enabled = false;
        btnTicket.Enabled = false;
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadTickets();
        pnl.Update();
    }
}