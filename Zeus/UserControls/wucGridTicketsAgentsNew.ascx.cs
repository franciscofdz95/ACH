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
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class UserControls_wucGridTicketsAgentsNew : System.Web.UI.UserControl
{

    public Hashtable prms;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool has_access = false;

        UserRole role = null;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_AGENT_RELATIONS, out role))
            has_access = role.Enabled;

        //foreach (UserRole ur in UserSessions.CurrentUser.UserRoles)
        //{
        //    if (ur.RoleID.ToUpper().Trim() == AGENT_RELATIONS_ROLE)
        //    {
        //        has_access = true;
        //    }
        //}

        if (has_access == false)
        {
            this.Visible = false;
            return;
        }

        prms = new Hashtable();

        if (!Page.IsPostBack)
        {
            LookupTableHandler.LoadInternalUsers(UserID, false);

            if (UserSessions.CurrentUser != null)
                UserID.Items.FindByValue(UserSessions.CurrentUser.UID).Selected = true;

            odsTickets.SelectCountMethod = "GetTicketsPagingCount";
        }
    }

    protected void grdTickets_DataBound(object sender, EventArgs e)
    {
        pnlNoTickets.Visible = !(grdTickets.Rows.Count > 0);
    }

    protected void cboPageSize2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.grdTickets.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
        LoadPendingTickets();
    }

    private void LoadPendingTickets()
    {
        grdTickets.DataBind();
        pnlNoTickets.Visible = !(grdTickets.Rows.Count > 0);
    }

    protected void grdTickets_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lbtnTicketID");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString();

                if (!(e.Row.Cells[5].Text.Equals("&nbsp;")))
                    e.Row.Cells[5].ToolTip = e.Row.Cells[4].Text;
                e.Row.Cells[5].Text = "<div style='width:200px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;'>" + e.Row.Cells[5].Text + "</div>";

                btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");

                Image img1 = ((Image)e.Row.FindControl("img"));

                string user = UserSessions.CurrentUser.UserName.ToLower();
                string notecreated = DataBinder.Eval(e.Row.DataItem, "NoteCreatedBy").ToString().ToLower();
                string assignedto = DataBinder.Eval(e.Row.DataItem, "UserUID").ToString().ToLower();
                string usermodified = DataBinder.Eval(e.Row.DataItem, "UserModified").ToString().ToLower();

                if (assignedto == UserSessions.CurrentUser.UID.ToLower())
                    img1.Visible = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "AttentionReq"));
                else if ((notecreated == string.Empty || notecreated != user) && usermodified != user)
                    img1.Visible = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "AttentionReq"));
                else
                    img1.Visible = false;

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    protected void grdTickets_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;

        switch (e.CommandName)
        {
            case "View":

                //LinkButton btn = (LinkButton)e.CommandSource;
                //if (btn.Text == "0")
                //    return;
                //else
                //{
                //    Hashtable prms = new Hashtable();
                //    prms.Add("@UID", e.CommandArgument.ToString());
                //    Ticket ticket = DataAccess.DataTicketDao.GetTicket(prms);
                //    UserSessions.CurrentTicket = ticket;

                //    url = "~/SecureTicketForms/frmTicketPopup.aspx?Adding=false";
                //    url += "&PostBackURL=~/SecureHomeForms/frmHome.aspx";
                //    url += "&TicketUID=" + ticket.TicketUID.Trim();

                //    Response.Redirect(url);
                //}

                break;
        }
    }

    protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        prms.Add("@TicketSource", "a");
        prms.Add("@StatusUIDList", string.Format("{0},{1},{2}", Ticket.TICKET_OPEN, Ticket.TICKET_ASSIGNED, Ticket.TICKET_PENDING));
        prms.Add("@PageSize", Convert.ToInt32(hidPageSize.Value));
        prms.Add("@CurrentPage", Convert.ToInt32(hidCurrentPage.Value));
        prms.Add("@SortDirection", 1);

        if (UserID.SelectedIndex > 0)
            prms.Add("@UserUID", UserID.SelectedValue);

        e.InputParameters[0] = prms;
    }

    protected void UserID_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadPendingTickets();
    }
}
