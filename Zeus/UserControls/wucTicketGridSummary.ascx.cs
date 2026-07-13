using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Data;
using System.Text;
using PaymentXP.Facade;

namespace ZeusWeb.UserControls
{
    public partial class wucTicketGridSummary : wucBaseSearch
    {

        private List<UserFilter> _UserFilters
        {
            set
            {
                ViewState["UserFilters"] = value; 
            }
            get
            {
                return (List<UserFilter>)ViewState["UserFilters"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FormShow();
            }
        }

        public void FormShow()
        {
            if (UserSessions.CurrentUser != null)
            {
                this.LoadTicketUserFilter();

                Hashtable prms = new Hashtable();
                prms["@IsAssigned"] = 1;
                prms["@UserUID"] = UserSessions.CurrentUser.UID;


                this.SetDataSource(prms);
            }
        }

        protected void odsSummary_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //hash table is use to store parameters which will be passed to the stored procedure
            Hashtable prms = new Hashtable();

            if (UserSessions.CurrentUser != null)
               // prms.Add("@RoleUID", UserSessions.CurrentUser.DefaultRoleUID);
                prms.Add("@UserUID", UserSessions.CurrentUser.UID);

            e.InputParameters[0] = prms;
        }

        protected void grdSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    LinkButton btn = (LinkButton)e.Row.FindControl("TicketCount");
                    btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketCount").ToString();

                    string str = string.Empty;
                    Hashtable prms = new Hashtable();
                    GridViewRow grdRow = ((GridViewRow)btn.NamingContainer);
                    string cnt = "";
                    string username = grdRow.Cells[0].Text.Trim();
                    string useruid = btn.CommandArgument.Trim();

                    //  btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");
                    if (useruid == UserSessions.CurrentUser.UID)
                    {
                        cnt = btn.Text;
                        str = "Working\\Assigned";
                        prms["@IsAssigned"] = 1;
                        if (useruid != string.Empty)
                            prms["@UserUID"] = useruid;

                        if (string.IsNullOrEmpty(cnt) || cnt.Trim() == "0")
                        {
                            prms = new Hashtable();

                            lblText.Text = "";
                            lblRecordCount.Text = "0";
                            pnlTickets.Visible = false;
                        }
                        else
                        {
                            lblText.Text = username + " has " + btn.Text + " Tickets " + str;
                            lblRecordCount.Text = btn.Text;
                            pnlTickets.Visible = true;
                        }

                        //wucFTGridTicket1.SetDataSource(prms);
                    }

                    break;

                case DataControlRowType.Footer:
                    break;
                default:
                    break;
            }
        }

        protected void grdSummary_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string url = string.Empty;

            if (!(e.CommandSource is LinkButton))
                return;

            LinkButton btn = (LinkButton)e.CommandSource;
            string str = string.Empty;
            Hashtable prms = new Hashtable();
            GridViewRow grdRow = ((GridViewRow)btn.NamingContainer);
            string username = grdSummary.DataKeys[grdRow.RowIndex].Values[0].ToString();
            string cnt = "";
            string useruid = btn.CommandArgument.Trim();

            if (!string.IsNullOrEmpty(useruid))
            {

                if (btn != null)
                {
                    switch (e.CommandName)
                    {
                        case "TicketCount":
                            cnt = btn.Text;
                            str = grdSummary.Columns[1].HeaderText;
                            prms["@IsAssigned"] = 1;
                            this.CurrentPage = 1;
                            if (useruid != string.Empty)
                                prms["@UserUID"] = useruid;

                            break;

                        case "NearSLA":
                            cnt = btn.Text;
                            str = grdSummary.Columns[2].HeaderText;
                            prms["@IsNearDueDate"] = 1;
                            this.CurrentPage = 1;
                            if (useruid != string.Empty)
                            {
                                prms["@TicketUser"] = useruid;
                                prms["@TicketUserName"] = username;
                                
                            }

                            break;

                        case "PastSLA":
                            cnt = btn.Text;
                            str = grdSummary.Columns[3].HeaderText;
                            prms["@IsPastDueDate"] = 1;
                            this.CurrentPage = 1;
                            if (useruid != string.Empty)
                            {
                                prms["@TicketUser"] = useruid;
                                prms["@TicketUserName"] = username;
                            }
                            break;

                        case "AssignedOut":
                            cnt = btn.Text;
                            str = grdSummary.Columns[4].HeaderText;
                            if (username != string.Empty)
                            {
                                prms["@TicketUserName"] = username;
                            }
                            prms["@IsAssignedOut"] = 1;
                            this.CurrentPage = 1;
                            break;

                        case "RecentlyClosed":
                            cnt = btn.Text;
                            str = grdSummary.Columns[5].HeaderText + " within 7 days";
                            if (username != string.Empty)
                            {
                                //prms["@TicketUser"] = useruid;
                                prms["@TicketUserName"] = username;
                            }
                            prms["@IsRecentlyClosed"] = 1;
                            this.CurrentPage = 1;
                            break;

                    }

                    if (string.IsNullOrEmpty(cnt) || cnt.Trim() == "0")
                    {
                        prms = new Hashtable();

                        lblText.Text = "";
                        lblRecordCount.Text = "0";
                        pnlTickets.Visible = false;
                    }
                    else
                    {
                        lblText.Text = username + " has " + btn.Text + " Tickets " + str;
                        lblRecordCount.Text = btn.Text;
                        pnlTickets.Visible = true;
                    }

                    this.SetDataSource(prms);


                }
            }
        }

        private void SetDataSource(Hashtable prms)
        {
            grdTickets.DataSourceID = "ods";
            grdTickets.PageIndex = this.CurrentPage - 1;
            grdTickets.PageSize = this.PageSize;
            this.m_Prms = prms;
            this.m_Prms["@PageSize"] = this.PageSize;
            this.m_Prms["@CurrentPage"] = this.CurrentPage;
        }

        protected void cboPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
            this.m_Prms["@PageSize"] = this.PageSize;
            this.m_Prms["@CurrentPage"] = this.CurrentPage;
            this.SetDataSource(this.m_Prms);
        }      

        protected void grdTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:


                    break;

                case DataControlRowType.DataRow:

                    // ticket button
                    LinkButton btn = (LinkButton)e.Row.FindControl("lbtnTicketID");
                    btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();
                    btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString();
                    btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");

                    //DM-690, comment as is causing issues in some browsers 
                    //btn.Attributes.Add("title", WebUtil.BuildTicketTooltip(((DataRowView)e.Row.DataItem).Row));

                    e.Row.Cells[8].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[8].Text);
                    e.Row.Cells[9].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[9].Text);
                    e.Row.Cells[12].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[12].Text);

                    ((Literal)e.Row.FindControl("lblProblem")).Text = CommonUtility.Formatting.nl2br(DataBinder.Eval(e.Row.DataItem, "Problem").ToString());

                    // if ticket status is not closed. do this for the rest of them.
                    if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")).ToUpper() != "CLOSED")
                    {
                        string pri = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Priority"));

                        switch (pri)
                        {
                            case "High":
                                e.Row.Cells[0].CssClass += "zeustooltip tickethigh";
                                e.Row.Cells[0].Attributes.Add("title", "High Priority");
                                break;

                            case "Medium":
                                e.Row.Cells[0].CssClass += "zeustooltip ticketmedium";
                                e.Row.Cells[0].Attributes.Add("title", "Medium Priority");
                                break;

                            default:
                                e.Row.Cells[0].CssClass += "zeustooltip ticketlow";
                                e.Row.Cells[0].Attributes.Add("title", "Low Priority");
                                break;
                        }
                    }
                    else
                    {
                        //Default to css class low for closed grid.
                        e.Row.Cells[0].CssClass += " ticketlow";
                    }
                    break;

                case DataControlRowType.Footer:
                    break;

                default:
                    break;
            }
        }

        protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //hash table is use to store parameters which will be passed to the stored procedure

            if (this.m_Prms == null)
            {
                this.m_Prms = new Hashtable();
            }

            if (this.m_Prms.Count > 0)
            {
                if (!m_Prms.ContainsKey("@PageSize"))
                    m_Prms.Add("@PageSize", "10");
                else
                    m_Prms["@PageSize"] = this.PageSize;


                if (!m_Prms.ContainsKey("@CurrentPage"))
                    m_Prms.Add("@CurrentPage", "1");
                else
                    m_Prms["@CurrentPage"] = this.CurrentPage;


                if (!m_Prms.ContainsKey("@SortOrder"))
                    m_Prms.Add("@SortOrder", "TicketID");
                else
                    m_Prms["@SortOrder"] = this.SortOrder;


                m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

                e.InputParameters[0] = this.m_Prms;

            }
        }

        protected void grdTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.SetDataSource(this.m_Prms);
        }

        protected void AddUserTicketFilter_Click(object sender, EventArgs e)
        {
            List<int> userIds = new List<int>();

            //loop thru all checked users to filter
            foreach (GridViewRow rw in grdNonFilteredUser.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkEnabled");
                if (chkBx != null && chkBx.Checked)
                {
                    HiddenField hdnId = rw.FindControl("UserID") as HiddenField;
                    userIds.Add(int.Parse(hdnId.Value));
                }
            }

            if (userIds.Count > 0)
            {
                //update view state _UserFilters
                foreach (UserFilter filter in this._UserFilters.Where(u => userIds.Contains(u.UserID)))
                {
                    filter.Filtered = true;
                }

                //rebind non-filter and filter grids
                BindFilterGrids(this._UserFilters);
            }
        }

        protected void DeleteUserTicketFilter_Click(object sender, EventArgs e)
        {
            List<int> userIds = new List<int>();

            //loop thru all checked users to filter
            foreach (GridViewRow rw in grdFilteredUsers.Rows)
            {
                CheckBox chkBx = (CheckBox)rw.FindControl("chkEnabled");
                if (chkBx != null && chkBx.Checked)
                {
                    HiddenField hdnId = rw.FindControl("UserID") as HiddenField;
                    userIds.Add(int.Parse(hdnId.Value));
                }
            }

            if (userIds.Count > 0)
            {
                //update view state _UserFilters
                foreach (UserFilter filter in this._UserFilters.Where(u => userIds.Contains(u.UserID)))
                {
                    filter.Filtered = false;
                }

                //rebind non-filter and filter grids
                BindFilterGrids(this._UserFilters);
            }
        }

        protected void Apply_Click(object sender, EventArgs e)
        {
            //apply the filter changes
            List<int> filtered = this._UserFilters.Where<UserFilter>(p => p.Filtered == true).Select(item => item.UserID).ToList();
            DataTicket.GetInstance().UpdateUserTicketFilter(UserSessions.CurrentUser.UserID, string.Join(",", filtered));

            //refresh the ticket summary grid
            this.grdSummary.DataBind();

            //close the dialog
            this.dlgUserFilter.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        }

        protected void btnFilterUsers_Click(object sender, EventArgs e)
        {
            this.dlgUserFilter.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            LoadTicketUserFilter();
        }

        private void LoadTicketUserFilter()
        {

            //get all users associated with the user's role
            List<UserFilter> filter = DataTicket.GetInstance().GetUserTicketFilter(UserSessions.CurrentUser.UID);

            //save filter to viewstate
            this._UserFilters = filter;

            BindFilterGrids(filter);
        }

        private void BindFilterGrids(List<UserFilter> filter)
        {
            //get the filtered users
            List<UserFilter> filtered = filter.Where<UserFilter>(p => p.Filtered == true).ToList<UserFilter>();

            //get the non filtered users
            List<UserFilter> nonFiltered = filter.Where<UserFilter>(p => p.Filtered == false).ToList<UserFilter>();

            //bind! bind! bind!
            this.grdFilteredUsers.DataSource = filtered;
            this.grdFilteredUsers.DataBind();

            this.grdNonFilteredUser.DataSource = nonFiltered;
            this.grdNonFilteredUser.DataBind();
        }

        protected void grdTickets_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.CurrentPage = 1;
            this.SortOrder = e.SortExpression;
            this.SortDirectionSearch = e.SortDirection;
            this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
            
            this.m_Prms["@PageSize"] = this.PageSize;
            this.m_Prms["@CurrentPage"] = this.CurrentPage;
            this.m_Prms["@SortOrder"] = this.SortOrder;

            this.SetDataSource(this.m_Prms);
        }

        
    }
}
