using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects.Notify;

namespace ZeusWeb.UserControls
{
    public partial class wucLastNotesMerchant : wucBaseSearch
    {

        //public int frmPage
        //{
        //    set { m_page = value; }
        //    get { return m_page; }
        //}



        protected void grdNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
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
            this.m_Prms = null;

            if (UserSessions.CurrentMerchantApp != null)
            {
                //Load notes
                this.m_Prms = new Hashtable();
                this.m_Prms.Add("@PageSize", 4);
                this.m_Prms.Add("@CurrentPage", this.CurrentPage);

                if (this.SortOrder == string.Empty)
                {
                    this.SortOrder = "DateCreated";
                }
                this.m_Prms.Add("@SortOrder", this.SortOrder);
                this.m_Prms.Add("@SortDirection", SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch));
                this.m_Prms.Add("@UserName", UserSessions.CurrentUser.UserName);
                this.m_Prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);

                if (UserSessions.CurrentUser.IsAgent)
                {
                    this.m_Prms.Add("@View_Agent", true);
                }
                else if (UserSessions.CurrentUser.IsBank)
                {
                    this.m_Prms.Add("@View_Bank", true);
                }

                if (UserSessions.CurrentUser.IsInternal)
                {
                    this.m_Prms.Add("@View_MPSALL", true);
                }

                grdNotes.DataBind();
                NoRecords.Visible = !(grdNotes.Rows.Count > 0);
                Records.Visible = grdNotes.Rows.Count > 0;


                hypMore.NavigateUrl = "~/SecureMerchantManagementForms/frmMerchantNotes.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID;
            }
          
        }

       

        protected void odsNotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            
            e.InputParameters[0] = this.m_Prms;
            e.InputParameters[3] = this.ClientID;
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

                // hide the checkbox row if you're logged in as a bank.
                pnlNotesCBRow.Visible = !UserSessions.CurrentUser.IsBank;

                //if (frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING)
                //{
                //    if (UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2) == "AP")
                //    {
                //        ListHandler.ListFindItem(cboNoteCode, "52b65e1b-d418-4834-8f54-dcd9849df78e");
                //        View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
                //        View_Bank.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Bank;
                //        View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
                //        Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
                //    }
                //    else
                //    {
                //        ListHandler.ListFindItem(cboNoteCode, "7fbcf694-4b03-4f5a-92b0-7c9500225c4e");
                //        View_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Agent;
                //        View_Bank.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_Bank;
                //        View_MPSAll.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].View_MPSAll;
                //        Email_Agent.Checked = UserSessions.MerchantNoteCodes[cboNoteCode.SelectedValue].Email_Agent;
                //    }

                //    if (cboNoteCode.Items.Count == 0)
                //        LookupTableHandler.LoadMerchant_NoteCodes(cboNoteCode, false, "");

                //    cboNoteCode.Enabled = false;
                //    Subject.Value = cboNoteCode.SelectedItem.Text;

                //    chkApplySameLegalName.Visible = RequiresCallback.Enabled = false;
                //}
            }

           
        }

        protected void grdNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:

                    break;
                case DataControlRowType.DataRow:

                    //Convert html to text and display on the grid and tool tip. The html should still show on click of note.
                    string note = DataBinder.Eval(e.Row.DataItem, "Notes").ToString();
                    note = WebUtil.ConvertHtml(Server.HtmlDecode(note));
                    e.Row.Cells[1].Text = "<div style='width:150px;overflow:hidden;white-space:nowrap;text-overflow:ellipsis;'><NOBR>" + note + "</NOBR></div>";
                    e.Row.Cells[1].Attributes.Add("title", note);

                    e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);

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
                    notes = CommonUtility.Formatting.nl2br(grdNotes.DataKeys[grdRow.RowIndex].Values["Notes"].ToString());
                    subject = grdNotes.DataKeys[grdRow.RowIndex].Values["Subject"].ToString();
                    chkInternal.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_MPSAll"]);
                    chkAgent.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Agent"]);
                    ChkMerchant.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Merchant"]);
                    //chkBank.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Bank"]);
                    //chkCallback.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["RequiresCallback"]);
                }
            }
            else
                return;

            switch (e.CommandName)
            {
                case "View":

                    txtSubject.Text = subject;
                    // PXP-14251 Fix Zeus Ticket Notes displaying Blank HTML Page
                  // txtNotes.Text = WebUtil.ConvertHtml(Server.HtmlDecode(notes));
                   txtNotes.Text = CommonUtility.Formatting.nl2br(System.Web.HttpUtility.HtmlDecode(notes));
                    WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

                    break;
            }
        }

        protected void btnAddNotes_Click(object sender, EventArgs e)
        {
            if (this.AddNotes())
            {
                Notes.Text = string.Empty;
                //if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT && frmPage != (int)CONDITIONS.CREDITUNDERWRITING && frmPage != (int)CONDITIONS.APPPROCESSING)
                //{
                //    Subject.Value = string.Empty;
                //    cboNoteCode.SelectedIndex = 0;
                //    RequiresCallback.Checked = View_MPSAll.Checked = View_Agent.Checked = View_Bank.Checked = false;
                //    LoadMerchantNotes();
                //}
                //else
                    LoadMerchantNotes();
            }
        }

        protected void btnClearNotes_Click(object sender, EventArgs e)
        {
            //if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT && frmPage != (int)CONDITIONS.CREDITUNDERWRITING && frmPage != (int)CONDITIONS.APPPROCESSING)
            //    cboNoteCode.SelectedIndex = 0;

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
                Hashtable prms = new Hashtable();

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

            if (!string.IsNullOrEmpty(MerchantAppUID))
            {
                //if (frmPage == (int)CONDITIONS.RELATIONSHIPMANAGEMENT || frmPage == (int)CONDITIONS.CREDITUNDERWRITING || frmPage == (int)CONDITIONS.APPPROCESSING)
                //{
                //    if (frmPage != (int)CONDITIONS.RELATIONSHIPMANAGEMENT)
                //    {
                //        if (UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2) == "AP")
                //            ListHandler.ListFindItem(cboNoteCode, "52b65e1b-d418-4834-8f54-dcd9849df78e");
                //        else
                //            ListHandler.ListFindItem(cboNoteCode, "7fbcf694-4b03-4f5a-92b0-7c9500225c4e");
                //    }
                //    else
                //        ListHandler.ListFindItem(cboNoteCode, "c2fab379-84d2-4c37-97b5-9d3aa195b065");
                //}

                //this.hdnMerchantUID.Value = MerchantAppUID;
                //this.UID = MerchantAppUID;
                //this.Module = Module;

                this.LoadMerchantNotes();

            }
            else
            {
                //this.UID = string.Empty;
                //clearAll();
            }
        }

        public bool AddNotes()
        {
            // tested, good.
            string notes = Server.HtmlEncode(Notes.Text.Trim());

            //PXP-14251 
            string notesText = Server.HtmlEncode(notes);
            
            string error = string.Empty;

            if (notes == string.Empty)
                error = "Please enter notes.<br>";
            if (cboNoteCode.SelectedIndex <= 0)
                error += "Please select notecode.<br>";

            if (error.Trim() != string.Empty)
            {
                lblErr.Text = error;
                return false;
            }

            if (UserSessions.CurrentMerchantApp != null)
            {
                MerchantFacade facade = new MerchantFacade();
                MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
                User user = UserSessions.CurrentUser;
                string subject = string.Empty;

                if (cboNoteCode.SelectedIndex > 0)
                    subject = cboNoteCode.SelectedItem.Text;
                
                MerchantNotes ObjMerchantNotes = new MerchantNotes()
                {
                    MerchantAppUID = UserSessions.CurrentMerchantApp.MerchantAppUID,
                    Subject = subject,
                    Notes = notesText,
                    RequiresCallback = RequiresCallback.Checked,
                    View_Agent = View_Agent.Checked,
                    View_Bank = View_Bank.Checked,
                    View_MPSAll = View_MPSAll.Checked,
                    Email_Agent = Email_Agent.Checked,
                    UserCreated = user.UserName
                };

                if (chkApplySameLegalName.Checked == false)
                    DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);
                else
                    DataAccess.DataMerchantAppDao.InsertMerchantNotesLegalName(ObjMerchantNotes);

                if (ObjMerchantNotes.Email_Agent)
                {
                    string AlertName = string.Empty;
                    string deptType = string.Empty;
                    string From = string.Empty;
                    string AlertTypeName = string.Empty;

                    From = UserSessions.CurrentUser.Email;

                    if (app.PLEnabled)
                        From = app.PLEmail;

                    deptType = subject.Split('-')[0].ToString().Trim();
                    AlertName = subject;

                    if (deptType != string.Empty)
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
                                AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "", notification.Name, notes, AlertTypeName, Portal: ePortals.ZEUS);
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

    }
}