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

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

using Infragistics.Web.UI.LayoutControls;
using System.Collections.Generic;

public partial class wucLeadNotes : wucBaseSearch
{
    public delegate void ButtonClickHandler(object sender, EventArgs e);
    public event ButtonClickHandler ButtonClick;


    public Panel PanelApp
    {
        get { return pnlApp1; }
    }

    public Button CloseBtn
    {
        get { return btnClear; }
    }

    public Button btnAdd
    {
        get { return btnAddNotes; }
    }

    public Panel NotesPanel
    {
        get { return pnlNotes; }
    }

    public string LeadID
    {
        get
        {
            if (UserSessions.CurrentLead == null)
            {
                if (!string.IsNullOrEmpty(Session["LeadUID"].ToString()))
                {
                    return Session["LeadUID"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (!string.IsNullOrEmpty(UserSessions.CurrentLead.LeadUID))
            {
                return UserSessions.CurrentLead.LeadUID;
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            Session["LeadUID"] = value;
        }
    }

    public string lblError
    {
        get
        {
            if (ViewState["lblError"] != null) return ViewState["lblError"].ToString();
            else return string.Empty;
        }
        set { ViewState["lblError"] = value; }
    }

    public void ClearGrid()
    {
        grdLeadNotes.DataSource = null;
        grdLeadNotes.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (grdLeadNotes.Rows.Count == 0)
            LoadEmptyGrid();
        if (!IsPostBack)
        {
            this.PageSize = 10;
            this.CurrentPage = 1;
            ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        }
    }

    protected void grdLeadNotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "ID":
                GridViewRow grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                txtLeadNotes.Text = Server.HtmlDecode(grdRow.Cells[1].ToolTip);
                txtLeadNotes.ReadOnly = true;
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;
        }
    }

    protected void grdLeadNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                if (grdLeadNotes.DataSource is DataSet)
                {
                    ((LinkButton)e.Row.Cells[0].FindControl("lnkID")).Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                    e.Row.Cells[1].ToolTip = Server.HtmlDecode(e.Row.Cells[1].Text);
                    e.Row.Cells[1].Text = "<div style='width:200px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;'>" + Server.HtmlDecode(e.Row.Cells[1].Text) + "</div>";
                }
                e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);
                break;

            default:
                break;
        }
    }

    protected void grdLeadNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.LoadNotes();
    }

    protected void btnAddNotes_Click(object sender, EventArgs e)
    {
        if (this.AddNotes())
        {
            this.LoadNotes();
            if (((Button)sender).NamingContainer.Parent.NamingContainer is WebDialogWindow)
                ((WebDialogWindow)((Button)sender).NamingContainer.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
        }
    }

    protected void btnClearNotes_Click(object sender, EventArgs e)
    {
        txtNotes.Text = string.Empty;
        if (((Button)sender).NamingContainer.Parent.NamingContainer is WebDialogWindow)
            ((WebDialogWindow)((Button)sender).NamingContainer.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;

        ErrorMess.Text = ""; lblError = string.Empty;
        ErrorMess.Visible = false;
    }

    protected void btnCloseNotes_Click(object sender, EventArgs e)
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
    }

    public bool AddNotes()
    {
        string note = Server.HtmlEncode(txtNotes.Text.Trim());

        if (note == string.Empty)
        {
            lblError = "Please add a note.<br>";
            ErrorMess.Text = lblError;
            ErrorMess.Visible = true;
            if (this.ButtonClick != null)
                this.ButtonClick(btnAdd, new EventArgs());

            return false;
        }

        try
        {
            LeadNotes notes = new LeadNotes();
            DataLeadNotes data = DataAccess.DataLeadNotesDao;

            if(!string.IsNullOrWhiteSpace(LeadID))
            {
                notes.DateCreated = DateTime.Now;
                notes.Notes = note;
                notes.LeadID = LeadID;

                User user = UserSessions.CurrentUser;
                notes.UserUpdated = user.UserName;
                notes.UserCreated = user.UserName;

                data.InsertLeadNotes(notes);
                txtNotes.Text = string.Empty;
                return true;
            }

            return false;
        }
        catch (System.Exception exc)
        {
            throw exc;
        }
    }

    public void LoadNotes()
    {
        Hashtable prms = new Hashtable();
        DataLeadNotes data = DataAccess.DataLeadNotesDao;

        prms.Add("@LeadsUID", LeadID);
        DataSet ds = data.GetLeadNotes(prms);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            grdLeadNotes.Visible = true;
            grdLeadNotes.PageSize = this.PageSize;
            grdLeadNotes.PageIndex = this.CurrentPage - 1;
            grdLeadNotes.DataSource = ds;
            grdLeadNotes.DataBind();
            grd.Visible = false;
            lblRecordCount.Visible = true;
            lblRecordCount.Text = "Total records found:" + ds.Tables[0].Rows.Count.ToString();
        }
        else
            LoadEmptyGrid();
    }

    private void LoadEmptyGrid()
    {
        IList<LeadNotes> lstLeadNotes = new List<LeadNotes>();

        lstLeadNotes.Add(new LeadNotes());
        grd.Visible = true;
        grd.DataSource = lstLeadNotes;
        grd.DataBind();

        lblRecordCount.Visible = false;

        grdLeadNotes.Visible = false;

        int columnsCount = grd.Columns.Count;
        grd.Rows[0].Cells.Clear();

        // clear all the cells in the row      
        grd.Rows[0].Cells.Add(new TableCell());

        //add a new blank cell      
        grd.Rows[0].Cells[0].ColumnSpan = columnsCount;
        grd.Rows[0].Cells[0].CssClass = "EmptyDataRowStyle";

        //set No Results found to the new added cell       
        grd.Rows[0].Cells[0].Text = ".....No notes for the lead....";
        grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.LoadNotes();
    }
}
