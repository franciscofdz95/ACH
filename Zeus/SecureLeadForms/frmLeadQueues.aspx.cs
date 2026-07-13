using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections.Generic;

public partial class SecureLeadForms_frmLeadQueues : System.Web.UI.Page
{
    private List<UserFilter> _UserFilters
    {
        set
        {
            Session["UserFilters"] = value;
        }
        get
        {
            return (List<UserFilter>)Session["UserFilters"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            ((HyperLink)this.Master.FindControl("lnkLeadQueues")).CssClass = "active";

            LookupTableHandler.LoadLeadSources(LeadSources, true);
            btnFilterUsers.Visible = !UserSessions.CurrentUser.IsAgent;

            this.LoadLeadUserFilter();
            this.BindGrids();
        }
    }

    private void BindGrids()
    {
        List<string> liID = new List<string>();

        if (this._UserFilters != null)
        {
            foreach (UserFilter item in this._UserFilters.FindAll(a => a.Filtered == true))
            {
                liID.Add(item.UserID.ToString());
            }
        }

        WucSalesAssigned.SetDataSource(this.GetQueueParams(Constants.LEADSTATUS_ASSIGNED, liID), "Assigned");

        WucSalesInCommunication.SetDataSource(this.GetQueueParams(Constants.LEADSTATUS_INCOMMUNICATION, liID), "In Communication");

        WucSalesAppOut.SetDataSource(this.GetQueueParams(Constants.LEADSTATUS_APPLICATIONSENT, liID), "App Out");

        WucSalesAppSubmitted.SetDataSource(this.GetQueueParams(Constants.LEADSTATUS_APPLICATIONRECEIVED, liID), "App Submitted");

        WucSalesStatementsReceived.SetDataSource(this.GetQueueParams(Constants.LEADSTATUS_STATEMENTSRECEIVED, liID), "Statements Received");

    }

    private Hashtable GetQueueParams(string statusUID, List<string> liID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@Username", UserSessions.CurrentUser.UserName);

        if (!UserSessions.CurrentUser.IsAgent)
        {

            if (!liID.Contains(UserSessions.CurrentUser.UserID))
            {
                liID.Add(UserSessions.CurrentUser.UserID);
            }

            if (liID.Count > 0)
            {
                prms.Add("@AssignedUserIDList", CommonUtility.Util.implode(liID, ","));
            }
        }

        prms.Add("@Statusuid", statusUID);

        if (LeadSources.SelectedIndex > 0)
            prms.Add("@LeadsSourcesUID", LeadSources.SelectedValue);

        return prms;
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
        DataLead.GetInstance().UpdateUserLeadFilter(Convert.ToInt32(UserSessions.CurrentUser.UserID), string.Join(",", filtered));

        this.BindGrids();

        //close the dialog
        this.dlgUserFilter.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void btnFilterUsers_Click(object sender, EventArgs e)
    {
        this.dlgUserFilter.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        LoadLeadUserFilter();
    }

    private void LoadLeadUserFilter()
    {
        //get all users associated with the user's role
        List<UserFilter> filter = new List<UserFilter>();

        if (!UserSessions.CurrentUser.IsAgent)
        {
            List<LeadSourceRep> liRep = DataLead.GetInstance().GetLeadSourceReps(0);

            List<int> liMine = this.GetMyUsers();


            if (liRep != null)
            {
                foreach (LeadSourceRep item in liRep)
                {
                    filter.Add(new UserFilter
                    {
                        UserID = item.UserID,
                        Filtered = liMine.Contains(item.UserID),
                        FullName = item.FullName,
                        UserName = item.UserName
                    });
                }
            }

            //save filter to viewstate
            this._UserFilters = filter;
        }

        BindFilterGrids(filter);
    }

    private List<int> GetMyUsers()
    {
        List<int> li = new List<int>();

        DataTable dt = DataLead.GetInstance().SearchUserLeadFilter(Convert.ToInt32(UserSessions.CurrentUser.UserID));

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                li.Add(Convert.ToInt32(dr["FilterUserID"]));
            }
        }

        return li;
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

    protected void LeadSources_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrids();
    }
}
