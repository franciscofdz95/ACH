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


using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class wucAgentNotes : wucBaseSearch
{
    private bool _UsedInSideBar = false;

    public bool UsedInSideBar
    {
        get { return _UsedInSideBar; }
        set { _UsedInSideBar = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.CurrentPage = 1;

            LookupTableHandler.LoadAgentNoteCodes(cboNoteCode, false, "");

            if (this.UsedInSideBar)
            {
                lblRecordCount.Visible = false;
                ListHandler.ListFindItem(cboPageSize, "5");
                this.PageSize = 5;
                grdAgentNotes.Columns[2].Visible = false;


                if (UserSessions.CurrentAgent != null && CommonUtility.Util.IsValidGuid( UserSessions.CurrentAgent.AgentUID) && this.UsedInSideBar)
                {
                    hypMore.NavigateUrl = "~/SecureAgentManagementForms/frmAgentNotes.aspx?AgentUID=" + UserSessions.CurrentAgent.AgentUID;
                
                    pnlMore.Visible = true;
                }
            }

            LoadNotes();

        }
    }

    protected void btnAddNotes_Click1(object sender, EventArgs e)
    {
        if (this.AddNotes())
        {
            Notes.Text = string.Empty;
            this.LoadNotes();
        }
    }

    private bool AddNotes()
    {
        bool ret = false;
        if (UserSessions.CurrentAgent != null)
        {
            if (Notes.Text.Trim() == string.Empty)
            {
                ret = false;
            }

            if (cboNoteCode.SelectedIndex == 0)
                WucMessage1.AddMessageError("Please select a note code.");

            try
            {
                DataAgent data = DataAccess.DataAgentDao;
                User user = UserSessions.CurrentUser;

                string note = Server.HtmlEncode(Notes.Text);
                data.InsertAgentNotes(UserSessions.CurrentAgent.AgentUID, note, user.UserName, cboNoteCode.SelectedItem.Value);
                ret = true;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        return ret;
    }

    protected void btnClearNotes_Click1(object sender, EventArgs e)
    {
        cboNoteCode.SelectedIndex = -1;
        Notes.Text = string.Empty;
    }

    private void LoadNotes()
    {
        DataAgent data = DataAccess.DataAgentDao;

        grdAgentNotes.DataBind();

        pnlDetails.Visible = grdAgentNotes.Rows.Count > 0;
        lblNotes.Visible = grdAgentNotes.Rows.Count == 0;
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
        grdAgentNotes.PageSize = this.PageSize;
        this.LoadNotes();
    }

    protected void grdNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                HtmlAnchor anc1 = ((HtmlAnchor)e.Row.FindControl("lnkmore"));
                HtmlAnchor anc2 = ((HtmlAnchor)e.Row.FindControl("lnkless"));
                GridView grid = ((GridView)this.grdAgentNotes);
                    anc1.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Open'" + ",'" + grid.ClientID + "');");
                    anc2.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Close'" + ",'" + grid.ClientID + "');");
               
                

                break;

            case DataControlRowType.DataRow:


                //Convert html to text and display on the grid and tool tip. The html should still show on click of note.
                string note = (DataBinder.Eval(e.Row.DataItem, "Notes").ToString());
                note = WebUtil.ConvertHtml(Server.HtmlDecode(note));
                e.Row.Cells[4].Attributes.Add("title", note);


                Label Notes1 = ((Label)e.Row.Cells[4].FindControl("NotesMore"));
                Label Notes2 = ((Label)e.Row.Cells[4].FindControl("NotesLess"));

                HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnkmore"));
                HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnkless"));

               
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
                
               

                Notes2.Text = CommonUtility.Formatting.nl2br(note);

                e.Row.Cells[5].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[5].Text);


                LinkButton btnNotesID = (LinkButton)e.Row.FindControl("btnNotesID");
                btnNotesID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                e.Row.Cells[0].Text = Convert.ToString((e.Row.RowIndex + 1) + (grdAgentNotes.PageIndex * 10));
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
            if (grdAgentNotes != null)
            {
                grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                notes = grdAgentNotes.DataKeys[grdRow.RowIndex].Values["Notes"].ToString();
                subject = grdAgentNotes.DataKeys[grdRow.RowIndex].Values["NoteCode"].ToString();
            }
        }
        else
            return;

        switch (e.CommandName)
        {
            case "View":
                Subject.Text = subject;
                txtNotes.Text = Server.HtmlDecode(notes);
                 chkAgent.Checked = DataLayer.Field2Bool(grdAgentNotes.DataKeys[grdRow.RowIndex].Values["View_Agent"]);
                 chkMerchant.Checked = DataLayer.Field2Bool(grdAgentNotes.DataKeys[grdRow.RowIndex].Values["View_Merchant"]);
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;
        }
    }

    protected void grdNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        LoadNotes();
    }

    protected void grdNotes_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.LoadNotes();
    }

    protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (UserSessions.CurrentAgent != null && CommonUtility.Util.IsValidGuid(UserSessions.CurrentAgent.AgentUID))
        {
            Hashtable prms = new Hashtable();

            prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);
            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);
            grdAgentNotes.PageSize = this.PageSize;
            grdAgentNotes.PageIndex = this.CurrentPage - 1;

            if (this.SortOrder == string.Empty)
                prms["@SortOrder"] = "ID";
            else
                prms["@SortOrder"] = this.SortOrder;

            prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.ID;

            int record_count = DataMerchantAppPaging.GetAgentNotesPagingRowCount(prms, 0, 0, this.ID);
            lblRecordCount.Text = string.Format("Total Records Found: {0}", DataMerchantAppPaging.GetAgentNotesPagingRowCount(prms, 0, 0, this.ID).ToString());

            this.Visible = true;

        }
        else
        {
            e.Cancel = true;
            this.Visible = false;
        }

    }
}
