using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

public partial class frmAssignLeads : frmBaseDataEntry
{
    private List<LeadSourceRep> _LeadReps
    {
        set
        {
            ViewState["LeadReps"] = value;
        }
        get
        {
            return (List<LeadSourceRep>)ViewState["LeadReps"];
        }
    }

    private int _LeadSourceID
    {
        set
        {
            ViewState["LeadSourceID"] = value;
        }
        get
        {
            return (int)ViewState["LeadSourceID"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkAssignLeads")).CssClass = "active";

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadLeadSources(SourceID, false);
            LookupTableHandler.LoadLeadReps(AssignedUserID, false);

            this.Search(true);

            LoadSourceRepGrid();
        }
    }

    private void Search(bool IsOnLoad)
    {
        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (IsOnLoad)
        {
            prms.Add("@UID", "00000000-0000-0000-0000-000000000000");
        }
        else
        {
            if (!IsAssigned.Checked)
                prms.Add("@StatusUID", Constants.LEADSTATUS_NEW); //"410ac5dc-2721-459a-b346-bfb51662e231"); //New Leads Status
            else
                prms.Add("@StatusUIDList", Constants.LEADSTATUS_NEW + "," + Constants.LEADSTATUS_ASSIGNED);
 
            if (SourceID.SelectedIndex > 0)
                prms.Add("@LeadsSourcesUID", SourceID.SelectedItem.Value);
        }

        DataLead data = DataAccess.DataLeadDao;

        DataSet ds = data.GetLeads(prms);

        lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();

        DataView dv = ds.Tables[0].DefaultView;

        if (this.SortOrder == string.Empty)
            this.SortOrder = "ID";

        dv.Sort = this.SortOrder + " " + ConvertSortDirectionToSql(this.SortDirectionSearch);


        grdLeads.DataSource = dv;
        grdLeads.DataBind();
        pnlRecords.Visible = (ds.Tables[0].Rows.Count > 0);
        pnlNoRecords.Visible = !(ds.Tables[0].Rows.Count > 0);
    }

    private string ConvertSortDirectionToSql(SortDirection direction)
    {
        string newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = "DESC";
                break;

            default:
                newSortDirection = "ASC";
                break;
        }
        return newSortDirection;
    }

    public override bool FormDataCheck()
    {
        string message = string.Empty;
        bool ischecked = true;

        foreach (GridViewRow grdRow in grdLeads.Rows)
        {
            if (((CheckBox)grdRow.Cells[0].FindControl("chkSelect")).Checked)
            {
                ischecked = false;
                break;
            }
        }

        if (ischecked)
            message += "* Please select rows to update.<br />";

        if (AssignedUserID.SelectedIndex == 0)
            message += "* Must select Rep to update.<br /><br />";

        lblError.Text = message;

        if (message == string.Empty)
            return true;
        else
            return false;
    }

    protected void btnMassUpdate_Click(object sender, EventArgs e)
    {
        if (!this.FormDataCheck())
        {
            return;
        }

        User user = UserSessions.CurrentUser;
        string LeadIds = string.Empty;

        foreach (GridViewRow grdRow in grdLeads.Rows)
        {
            if (((CheckBox)grdRow.Cells[0].FindControl("chkSelect")).Checked)
            {
               LeadIds += grdLeads.DataKeys[grdRow.RowIndex].Values["ID"].ToString() + ",";
                
            }
        }

        if (LeadIds.Length > 1 && AssignedUserID.SelectedIndex > 0)
            LeadFacade.UpdateAssignLeads(int.Parse(AssignedUserID.SelectedValue), LeadIds, user.UserName);

        this.Search(false);

        lblError1.Text = "Update completed.";
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                
                    Label lblAssigned = ((Label)e.Row.FindControl("lblAssignedTo"));
                    LeadSourceRep rep = LookupTableHandler.m_LeadReps.Find(item => item.UserID.ToString() == DataBinder.Eval(e.Row.DataItem, "AssignedUserID").ToString());
                    lblAssigned.Text = (rep != null)?rep.FullName:"";

                    string datecreated = WebUtil.ConvertToUserDateTimeSettings(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCreated")));
                    e.Row.Cells[6].Text = string.IsNullOrEmpty(datecreated) ? "" : datecreated;

                break;
        }
    }

    protected void SourceID_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";

        if (SourceID.SelectedIndex > 0)
            this.Search(false);
        else
        {
            pnlRecords.Visible = false;
            pnlNoRecords.Visible = true;
            IsAssigned.Checked = false;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        FormHandler.ClearAllControls(this);
        lblError.Text = string.Empty;
        this.Search(true);
    }

    protected void grdLeads_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (this.SortOrder != e.SortExpression)
            this.SortDirectionSearch = e.SortDirection;
        else
        {
            switch (this.SortDirectionSearch)
            {
                case SortDirection.Descending:
                    this.SortDirectionSearch = SortDirection.Ascending;
                    break;

                default:
                    this.SortDirectionSearch = SortDirection.Descending;
                    break;
            }
        }

        this.SortOrder = e.SortExpression;

        this.Search(false);
    }

    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormClear()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormSave()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormNew()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override bool FormDelete()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    private void LoadSourceRepGrid()
    {
        this.grdSourceAssignment.DataSource = LeadFacade.GetLeadSourceRepSummary();
        this.grdSourceAssignment.DataBind();
    }

    protected void grdSourceAssignment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandSource is LinkButton)
        {
            LinkButton lb = (LinkButton)e.CommandSource;
            int sourceId = int.Parse(lb.CommandArgument);
            this.LoadLeadSourceReps(sourceId);

            this._LeadSourceID = sourceId;
            this.dlgLeadSourceRep.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
    }

    private void LoadLeadSourceReps(int sourceId)
    {
        List<LeadSourceRep> reps = LeadFacade.GetLeadSourceReps(sourceId);

        this._LeadReps = reps;

        BindFilterLists(reps);
    }

    protected void AssignReps_Click(object sender, EventArgs e)
    {
        //assign reps
        List<int> userIds = new List<int>();

        foreach (GridViewRow rw in grdUnassignedReps.Rows)
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
            //update view state _LeadReps
            foreach (LeadSourceRep filter in this._LeadReps.Where(u => userIds.Contains(u.UserID)))
            {
                filter.Assigned = true;
            }

            //rebind assigned and unassigned lists
            BindFilterLists(this._LeadReps);
        }
    }

    protected void UnassignReps_Click(object sender, EventArgs e)
    {
        //unassign reps
        List<int> userIds = new List<int>();

        foreach (GridViewRow rw in grdAssignedReps.Rows)
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
            //update view state _LeadReps
            foreach (LeadSourceRep filter in this._LeadReps.Where(u => userIds.Contains(u.UserID)))
            {
                filter.Assigned = false;
            }

            //rebind assigned and unassigned lists
            BindFilterLists(this._LeadReps);
        }
    }

    protected void Apply_Click(object sender, EventArgs e)
    {
        //apply the filter changes
        List<int> assigned = this._LeadReps.Where<LeadSourceRep>(p => p.Assigned == true).Select(item => item.UserID).ToList();

        LeadFacade.UpdateLeadSourceReps(this._LeadSourceID, string.Join(",", assigned));

        //refresh the ticket summary grid
        this.LoadSourceRepGrid();

        //close the dialog
        this.dlgLeadSourceRep.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    private void BindFilterLists(List<LeadSourceRep> reps)
    {
        List<LeadSourceRep> assigned = reps.Where<LeadSourceRep>(p => p.Assigned == true).ToList<LeadSourceRep>();
        List<LeadSourceRep> unassigned = reps.Where<LeadSourceRep>(p => p.Assigned == false).ToList<LeadSourceRep>();

        //bind! bind! bind!
        this.grdAssignedReps.DataSource = assigned;
        this.grdAssignedReps.DataBind();

        this.grdUnassignedReps.DataSource = unassigned;
        this.grdUnassignedReps.DataBind();
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void IsAssigned_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";

        if (SourceID.SelectedIndex > 0)
            Search(false);
        else
            lblError.Text = "Please select a source";
    }

}
