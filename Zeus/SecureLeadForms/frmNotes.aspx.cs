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

using Infragistics.WebUI.WebControls;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class frmNotes : frmBaseSearch
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkLeadNotes")).CssClass = "active";

        if (!this.IsPostBack)
        {
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Notes Search";
            // LookupTableHandler.LoadAgentsNew(AgentAgentID, false);
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter notes;

        if (IsOnLoad && this.SearchParameters == null)
            return;
        else if (IsOnLoad)
        {
            notes = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(notes, pnlSearch);
        }

        grdLeadNotes.DataBind();

        pnlRecords.Visible = grdLeadNotes.Rows.Count > 0;
        pnlNoRecords.Visible = grdLeadNotes.Rows.Count == 0;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchParameters = null;
        grdLeadNotes.PageIndex = 0;
        this.CurrentPage = 1;
        this.SortOrder = string.Empty;
        this.Search(false);
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        this.FormClear();
    }

    protected void odsNotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        SearchParameter notes;
        DataLeadNotes data = DataAccess.DataLeadNotesDao;

        if (DBAName.Text != string.Empty)
            prms.Add("@DBAName", DBAName.Text);
        if (wucAgentSelector.m_AgentUID != string.Empty)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
        if (Notes.Text != string.Empty)
            prms.Add("@Notes", Notes.Text);

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            notes = new SearchParameter();
            FormBinding.BindControlsToObject(notes, pnlSearch);
            this.SearchParameters = notes;
        }
        else
        {
            prms.Add("@UID", "00000000-0000-0000-0000-000000000000");
        }

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        grdLeadNotes.PageSize = this.PageSize;
        grdLeadNotes.PageIndex = this.CurrentPage - 1;

        if (this.SortOrder == string.Empty)
            prms["@SortOrder"] = "ID";
        else
            prms["@SortOrder"] = this.SortOrder;

        prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);
        User user = UserSessions.CurrentUser;
        prms.Add("@UserName", user.UserName);

        e.InputParameters[0] = prms;
        lblRecordCount.Text = "Total Records Found: " + data.GetLeadNotesPagingCount(prms, 0, 0);
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                //LinkButton btn = (LinkButton)e.Row.FindControl("lbtnMerchantID");
                //btn.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                //btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LeadID").ToString();

                e.Row.Cells[3].Attributes.Add("title", e.Row.Cells[3].Text);
                e.Row.Cells[3].Text = "<div style='width:200px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;'>" + e.Row.Cells[3].Text + "</div>";

                e.Row.Cells[4].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[4].Text);
                break;
            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //GridViewRow grdRow = null;

        //if (e.CommandSource is LinkButton)
        //{
        //    LinkButton btn = (LinkButton)e.CommandSource;
        //    if (btn.Text == "0")
        //        return;
        //    grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //    //UserSessions.CurrentLeadUID = grdLeadNotes.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString();
        //    //UserSessions.CurrentLeadDBA = grdLeadNotes.DataKeys[grdRow.RowIndex].Values["DBAName"].ToString();
        //}
        //switch (e.CommandName)
        //{
        //    case "Edit":

        //        string url = string.Format("frmLeadsDetail.aspx?PostBackURL=frmNotes.aspx&Adding=false&LeadID={0}&LeadUID={0}", UserSessions.CurrentLead.LeadUID);
        //        Response.Redirect(url);
        //        break;
        //}
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        grdLeadNotes.PageIndex = 0;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    private void ClearGrid()
    {
        grdLeadNotes.DataSourceID = string.Empty;
        grdLeadNotes.DataBind();
    }

    private void FormClear()
    {
        FormHandler.ClearAllControls(this);
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        this.SearchParameters = null;
        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
        this.Search(false);
        wucAgentSelector.FormClear();
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

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.Search(false);
    }
}
