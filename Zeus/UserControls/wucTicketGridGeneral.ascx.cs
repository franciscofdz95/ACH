using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections;
using CommonUtility;
using System.Data;
using PaymentXP.Facade;
using System.IO;
using System.Diagnostics;

namespace ZeusWeb.UserControls
{
    public partial class wucTicketGridGeneral : System.Web.UI.UserControl
    {
        public string VSSelectedCategorySubCategoryID
        {
            get
            {
                if (ViewState["VSSelectedCategorySubCategoryID"] == null)
                    return null;
                else
                    return (string)ViewState["VSSelectedCategorySubCategoryID"];
            }
            set { ViewState["VSSelectedCategorySubCategoryID"] = value; }
        }

        public string VSSelectedCategorySubCategoryText
        {
            get
            {
                if (ViewState["VSSelectedCategorySubCategoryText"] == null)
                    return null;
                else
                    return (string)ViewState["VSSelectedCategorySubCategoryText"];
            }
            set { ViewState["VSSelectedCategorySubCategoryText"] = value; }
        }

        public TimeZones TimeZoneID
        {
            get { return (TimeZones)ViewState["TimeZoneID"]; }
            set { ViewState["TimeZoneID"] = value; }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                this.TimeZoneID = UserSessions.CurrentUser.TimeZone;

                LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess);

                this.grdTick.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);                
                LoadTickets();
                this.grdTick.Sort("TicketID", SortDirection.Descending);
                LookupTableHandler.LoadActiveInternalUsers(ddpUsers, false);
                LoadActions();

                if (ddlAction.SelectedIndex == 0)
                {
                    PnlUsers.Style["display"] = "none";
                }
            }

            //Check if the user has Operations role to show the checkbox.
            UserRole role = null;
            if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_APPLICATION_BOARDING, out role))
            {
                chkOpDispute.Visible = role.Enabled;
            }
        }

        private void LoadActions()
        {

            ddlAction.Items.Add(new ListItem("--Select--","-1"));
            ddlAction.Items.Add(new ListItem("Assign", "Assign"));

            User user = UserSessions.CurrentUser;

             UserForm frm = null;

             if (user.UserForms.TryGetValue("FRMHOME", out frm) && frm.HasAccess)
             {
                 if (frm.ControlObjects == null)
                     DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

                 foreach (ControlObject obj in frm.ControlObjects)
                 {
                     if (obj.Type == "Permission" && obj.ID == "BulkClose" && obj.IsVisible && obj.IsEnabled)
                         ddlAction.Items.Add(new ListItem("Close", "Close"));
                 }
             }
             

        }

        protected void chkNewApp_CheckedChanged(object sender, EventArgs e)
        {
            this.chkOpDispute.Checked = false;
            if (!chkNewApp.Checked)
            {
                ddlTicketCategory.SelectedValue = null;
            }

            LoadTickets();
        }

        private void LoadTickets()
        {
            grdTick.DataBind();
            lblTickets.Visible = !(grdTick.Rows.Count > 0);
            pnlPage.Visible = grdTick.Rows.Count > 0;

        }

        private void assignTicket(Ticket t)
        {
            string sendTo = string.Empty;
            string userId = string.Empty;
            string ticketText = string.Empty;
            DataTicket data = DataAccess.DataTicketDao;
            User user = UserSessions.CurrentUser;

                Ticket ticket = t;

                ticket.UserID = user.UID;
                ticket.UserModified = user.UserName;
                ticket.StatusID = "0AEE2CAB-CEC4-476B-9598-918DBABD43CF";

                int rows = 0;
                rows = data.UpdateTicket(ticket, this.TimeZoneID);
                userId = ticket.UserID;

                if (!string.IsNullOrEmpty(userId) && !(userId.Equals("-1")))
                {
                    User user1 = new User();
                    DataUser dao = DataAccess.DataUserDao;
                    user1 = dao.GetUser(userId);
                    sendTo = user1.Email;
                    ticketText = "New Ticket Assigned: ";
                }

                if (!sendTo.Equals(string.Empty))
                {
                    FormHandler.SendEmail(ticketText + t.TicketID, t.Problem, t.Problem, UserSessions.CurrentUser.Email, sendTo, "", "", new Hashtable(), null);
                }

            }

        protected void grdTick_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:


                    break;

                case DataControlRowType.DataRow:

                    LinkButton btn1 = (LinkButton)e.Row.FindControl("lbtnID");
                    btn1.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString();

                    btn1.Text = DataBinder.Eval(e.Row.DataItem, "Status").ToString();

                    if (DataBinder.Eval(e.Row.DataItem, "StatusUID").ToString().ToUpper().Equals("CDCD0A20-6603-4B07-94DC-F65D01290F6B")) //open
                    {
                        btn1.Enabled = true;
                    }
                    else
                    {
                        btn1.Enabled = false;
                    }

                    Label laID = (Label)e.Row.FindControl("labID");

                    //DM-690, comment as is causing issues in some browsers 
                    //laID.Attributes.Add("title", WebUtil.BuildTicketTooltip(((DataRowView)e.Row.DataItem).Row));

                    laID.Attributes.Add("onclick", string.Format("OpenTicket('{0}')", DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString()));

                    string user = UserSessions.CurrentUser.UserName.ToLower();

                    if (!string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "PrivateLabelUID").ToString()))
                    {
                        e.Row.CssClass = "pLabel";
                    }
                    e.Row.Cells[9].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[9].Text);
                    e.Row.Cells[10].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[10].Text);
                    e.Row.Cells[13].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[13].Text);
                    ((Literal)e.Row.FindControl("litSolution")).Text = CommonUtility.Formatting.nl2br(DataBinder.Eval(e.Row.DataItem, "Problem").ToString());

                    // setting the officeid
                    //DM-1916
                    //((Label)e.Row.FindControl("LabelOffice")).Text = ((CommonUtility.Util.Offices)CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "OfficeID").ToString(), -1)).ToString();


                    if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")).ToUpper() != "CLOSED")
                    {
                        string pri = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Priority"));

                        switch (pri)
                        {
                            case "High":
                                //e.Row.Cells[7].CssClass += " tickethigh";

                                e.Row.Cells[0].CssClass += "zeustooltip tickethigh";
                                e.Row.Cells[0].Attributes.Add("title", "High Priority");
                                break;

                            case "Medium":
                                //e.Row.Cells[7].CssClass += " ticketmedium";
                                e.Row.Cells[0].CssClass += "zeustooltip ticketmedium";
                                e.Row.Cells[0].Attributes.Add("title", "Medium Priority");
                    break;

                            default:
                                //e.Row.Cells[7].CssClass += " ticketlow";
                                e.Row.Cells[0].CssClass += "zeustooltip ticketlow";
                                e.Row.Cells[0].Attributes.Add("title", "Low Priority");
                                break;
                        }
                    }

                    break;

                case DataControlRowType.Footer:
                    break;
                default:
                    break;
            }
        }

        protected void grdTick_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string url = string.Empty;
            Hashtable prms = new Hashtable();
            Ticket ticket = new Ticket();
            switch (e.CommandName)
            {
                case "Assign":
                    
                    LinkButton btn1 = (LinkButton)e.CommandSource;
                    
                    if (btn1.Text == "0")
                        return;

                    prms = new Hashtable();
                    prms.Add("@UID", e.CommandArgument.ToString());
                    ticket = DataAccess.DataTicketDao.GetTicket(prms,UserSessions.CurrentUser.TimeZone);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "assignticketwindow", "OpenTicket('" + e.CommandArgument.ToString() + "')", true);

                    assignTicket(ticket);
                    LoadTickets();

                    break;
            }
        }

        protected void odsTick_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            string role = string.Empty;

            //hash table is use to store parameters which will be passed to the stored procedure
            Hashtable prms = new Hashtable();
            prms.Add("@UserID", UserSessions.CurrentUser.UID);

            //if (StatusID.SelectedIndex > 0)
                prms.Add("@StatusUID", Ticket.TICKET_OPEN.ToLower());

            //if (UserID.SelectedIndex > 0)
            //    prms.Add("@AssignedTo", UserID.SelectedValue);

            List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);

            prms.Add("@OfficeIDList", string.Join(",", officeAccess));

            if (ddlTicketCategory.SelectedIndex > 0)
                prms.Add("@CategoryID", ddlTicketCategory.SelectedValue);
            string selectedCategory = ddlTicketCategory.SelectedValue;

            prms.Add("@NewAppTicket", chkNewApp.Checked);
            prms.Add("@OpDisputeTicket", chkOpDispute.Checked);
            
            e.InputParameters[0] = prms;

            if (!string.IsNullOrEmpty(ddlTicketCategory.SelectedValue))
            {
                VSSelectedCategorySubCategoryID = ddlTicketCategory.SelectedValue;
                VSSelectedCategorySubCategoryText = ddlTicketCategory.SelectedItem.Text;
            }

            LoadCategorySubCategory(prms, selectedCategory);

        }

        private void LoadCategorySubCategory(Hashtable prms, string SelectedItem = null)
        {
            DataTicket data = new DataTicket();
            DataTable dt = data.GetTicketsCategoryByRole(prms);
            dt.Columns.Add("CatgorySubCategoryCount", typeof(string), "CatgorySubCategory + ' (' + Count + ')'");
            ddlTicketCategory.DataSource = dt;
            ddlTicketCategory.DataTextField = "CatgorySubCategoryCount";
            ddlTicketCategory.DataValueField = "SubCategoryID";
            ddlTicketCategory.DataBind();

            ListItem item = ddlTicketCategory.Items.FindByText("");
            if (item != null)
            {
                ddlTicketCategory.Items.Remove(item);
            }
            ddlTicketCategory.Items.Insert(0, new ListItem { Text = "All (" + dt.AsEnumerable().Sum(x => x.Field<int>("Count")).ToString() + ")", Value = "-1" });
            
            if (string.IsNullOrEmpty(SelectedItem) || SelectedItem.Equals("-1"))
            {
                ListItem selectedListItem = ddlTicketCategory.Items.FindByValue("-1");
                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                };
                
                 this.lblTicketCount.Text = dt.AsEnumerable().Sum(x => x.Field<int>("Count")).ToString();
               
            }
            else
            {
                ListItem selectedListItem = ddlTicketCategory.Items.FindByValue(SelectedItem);
                if (selectedListItem != null)
                {
                    selectedListItem.Selected = true;
                }
                
                // Do this if we did not find the selected value in Category Sub Category Selection.
                // Get the value from View state and make up an item in the drop down list so that user is clear about the returned count of tickets.
                else
                {
                    string SelectedCatSubCat = VSSelectedCategorySubCategoryText;
                    SelectedCatSubCat = SelectedCatSubCat.Remove(SelectedCatSubCat.Length - 3);
                    ddlTicketCategory.Items.Insert(ddlTicketCategory.Items.Count, new ListItem { Text = SelectedCatSubCat + "(0)", Value = VSSelectedCategorySubCategoryID });
                    selectedListItem = ddlTicketCategory.Items.FindByValue(VSSelectedCategorySubCategoryID);
                    selectedListItem.Selected = true;
                }

                DataRow[] row = dt.Select("SubCategoryID = '" + SelectedItem + "'");
                if (row.Count() > 0)
                {
                    this.lblTicketCount.Text = row[0]["Count"].ToString();
                }

            }

        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.grdTick.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            LoadTickets();
        }

        protected void UserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (StatusID.SelectedValue.ToUpper() == Ticket.TICKET_OPEN && UserID.SelectedIndex > 0)
            //    StatusID.SelectedIndex = 0;

            LoadTickets();
        }

        protected void StatusID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (StatusID.SelectedValue.ToUpper() == Ticket.TICKET_OPEN && UserID.SelectedIndex > 0)
            //    UserID.SelectedIndex = 0;

            LoadTickets();
        }

        protected void lstOfficeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTickets();
        }

        protected void ddlTicketCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTickets();
        }

        protected void chkOpDispute_CheckedChanged(object sender, EventArgs e)
        {
            this.chkNewApp.Checked = false;
                ddlTicketCategory.SelectedValue = null;
            LoadTickets();
        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool isValid = false;

            foreach (GridViewRow row in grdTick.Rows)
            {
                if (((CheckBox)row.FindControl("chkAction")).Checked)
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid && ddlAction.SelectedIndex > 0)
            {
                lblError1.Text = "Please select at least one ticket to continue.";
                ddlAction.SelectedIndex = 0;
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            }

            switch (ddlAction.SelectedValue)
            {
                case "Assign":

                    PnlUsers.Style["display"] = "inline";
                    pnlNotes.Visible = false;
                    ddpUsers.SelectedIndex = 0;

                    break;

                case "Close":

                    PnlUsers.Style["display"] = "none";
                    pnlNotes.Visible = true;
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

                    break;

                default:

                    PnlUsers.Style["display"] = "none";
                    pnlNotes.Visible = false;
                    ddpUsers.SelectedIndex = 0;

                    break;
            }            

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            closeDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            bool isComplete = false; int i = 0;

            //Use this for Bulk Insert of TicketNotes while closing.
            List<TicketNotes> TicketNotes = new List<TicketNotes>();
            List<int> ticketIds = new List<int>();


            if(ddlAction.SelectedValue.ToUpper() == "CLOSE" && string.IsNullOrWhiteSpace(Description.Text))
            {
                lblError.Text = "Please enter notes.";
                return;
            }
            else if (ddlAction.SelectedValue.ToUpper() == "ASSIGN" && ddpUsers.SelectedIndex == 0)
            {
                lblError1.Text = "Please select a user.";
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                return;
            }

            foreach (GridViewRow row in grdTick.Rows)
            {
                CheckBox chk = ((CheckBox)row.Cells[0].FindControl("chkAction"));

                i++;

                if (chk.Checked)
                {
                    switch (ddlAction.SelectedValue)
                    {

                        case "Assign":

                            //collect all the ticketids into a list
                            ticketIds.Add(int.Parse(grdTick.DataKeys[row.RowIndex].Values["TicketID"].ToString()));

                            break;

                        case "Close":
                                //The Bulk Close Note will not send out any emails to Agent/Merchant/Internal Users.
                                //As this is built only for few users in Optimal, this is ok now.
                                //If we have to extend this functionality, we need to do a bulk send emails. (Found on History of TicketNotification)
                                TicketNotes tn = new TicketNotes();
                                tn.ViewAgent = false;
                                tn.ViewMerchant = false;
                                tn.ViewBank = true; //This is always set to true
                                tn.Description = HttpUtility.HtmlEncode(Description.Text.Trim().Replace("\n", "").Replace("&nbsp;"," ")); //We encode it here because the note has to be encoded and saved in the merchantnotes table.
                                tn.TicketID = grdTick.DataKeys[row.RowIndex].Values["TicketID"].ToString();
                                tn.MerchantAppUID = grdTick.DataKeys[row.RowIndex].Values["MerchantUID"].ToString();
                                tn.MerchantNoteSubject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nNote"
                                                             , grdTick.DataKeys[row.RowIndex].Values["Department"].ToString()
                                                             , LookupTableHandler.GetTicketCategoryByID(grdTick.DataKeys[row.RowIndex].Values["ParentID"].ToString())
                                                             , grdTick.DataKeys[row.RowIndex].Values["Category"].ToString());
                                TicketNotes.Add(tn);

                                string old_solution_value = grdTick.DataKeys[row.RowIndex].Values["Solution"].ToString();
                                if (!string.IsNullOrWhiteSpace(old_solution_value))
                            {
                                    TicketNotes tnsol = new TicketNotes();
                                    tnsol.ViewAgent = false;
                                    tnsol.ViewMerchant = false;
                                    tnsol.ViewBank = true; //This is always set to true
                                    tnsol.Description = "Current Solution Changed. Old Solution: " + old_solution_value.Replace("\n", "").Trim(); // No need to encode as it was already encoded and saved.
                                    tnsol.TicketID = grdTick.DataKeys[row.RowIndex].Values["TicketID"].ToString();
                                    tnsol.MerchantNoteSubject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nSolution"
                                                                                , grdTick.DataKeys[row.RowIndex].Values["Department"].ToString()
                                                                                , LookupTableHandler.GetTicketCategoryByID(grdTick.DataKeys[row.RowIndex].Values["ParentID"].ToString())
                                                                                , grdTick.DataKeys[row.RowIndex].Values["Category"].ToString());
                                    TicketNotes.Add(tnsol);
                                }

                                break;

                        default:

                            lblError1.Text = "Please select a action.";
                            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                            PnlUsers.Style["display"] = "none";
                            ddpUsers.SelectedIndex = 0;
                            pnlNotes.Visible = false;

                            break;
                    }
                }
            }
            
            
            try
            {
                if (i == grdTick.Rows.Count)
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();

                    switch (ddlAction.SelectedValue)
                    {
                        case "Assign":

                            if (ticketIds.Count > 0)
                            {
                                isComplete = TicketNotification.BulkAssign(string.Join(",", ticketIds), UserSessions.CurrentUser.UserName, ddpUsers.SelectedValue);

                                if (isComplete)
                                {
                                    ZeusWeb.Logging.EmailLog.InfoFormat("Bulk assign for ticket(s): {0} assign to : {1}.", string.Join(",", ticketIds), ddpUsers.SelectedItem.Text);
                                    lblError1.Text = "Successfully assigned the tickets to " + ddpUsers.SelectedItem.Text;
                                    PnlUsers.Style["display"] = "none";
                                    ddpUsers.SelectedIndex = 0;
                                    WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                                }
                            }

                            break;

                        case "Close":
                            //Bulk Add notes
                            isComplete = TicketNotification.BulkAddNotes(TicketNotes, UserSessions.CurrentUser, Description.Text.Replace("\n", "").Replace("&nbsp;"," ").Trim());
                            if (isComplete)
                            {
                                lblError1.Text = "Successfully closed the tickets";
                                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                            }
                            break;
                    }

                    watch.Stop();
                    string Text = watch.Elapsed.Seconds.ToString();

                }

            }
            catch (Exception ex)
            {
                lblError.Text = "Some Thing went wrong with the bulk Update.";
            }

            Description.Text = string.Empty;
            closeDialog();

        }


        private void closeDialog()
        {
            ddlAction.ClearSelection();
            LoadTickets();
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        }

        protected void lstOfficeAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTickets();
        }
    }
}
