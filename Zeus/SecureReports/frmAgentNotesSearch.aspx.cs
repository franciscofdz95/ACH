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


public partial class frmAgentNotesSearch : frmBaseSearch
{

    private bool _Export2Excel = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadAgentStatus(StatusUID, true);
            LookupTableHandler.LoadAgentNoteCodes(NoteCode, false, string.Empty);
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;
        DataAgent data = DataAccess.DataAgentDao;
        DataSet ds = null;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);

        if (AgentFirstName.Text != string.Empty)
            prms.Add("@FirstName", AgentFirstName.Text);

        if (AgentLastName.Text != string.Empty)
            prms.Add("@LastName", AgentLastName.Text);

        if (AgentDBA.Text != string.Empty)
            prms.Add("@DBA", AgentDBA.Text);

        if (Notes.Text != string.Empty)
            prms.Add("@Notes", Notes.Text);

        if (AgentPhone.Text != string.Empty)
            prms.Add("@Phone", AgentPhone.Text);

        if (AgentUID.Text != string.Empty)
            prms.Add("@ID", AgentUID.Text);

        if (NoteCode.SelectedIndex > 0)
            prms.Add("@AgentNoteCodeUID", NoteCode.SelectedItem.Value);

        if (StatusUID.SelectedIndex > 0)
            prms.Add("@StatusUID", StatusUID.SelectedItem.Value);

        //user is passed as a parameter to determine whether the user is an agent or manager

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            ds = data.GetAgentNotesDS(prms);
            DataView dv = ds.Tables[0].DefaultView;

            lblRecordCount.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();
        }

        grd1.DataSource = ds;
        grd1.DataBind();

        pnl1.Visible = (grd1.Rows.Count > 0);
        NoData.Visible = !(grd1.Rows.Count > 0);
        lblTitle.Text = "Agent Notes";
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        grd1.PageIndex = 1;
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SortOrder = string.Empty;
        grd1.PageIndex = 1;
        this.SearchParameters = null;
        this.Search(false);
    }

    private void FormClear()
    {
        SearchParameters = null;
        FormHandler.ClearAllControls(this);
        this.Search(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        this._Export2Excel = true;
        FormHandler.Export2Excel("AgentNotesList.xls", grd1);
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        this._Export2Excel = true;
        FormHandler.ExportToPDF(grd1, false, "Agent Notes List");
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string subject = string.Empty;
        string notes = string.Empty;
        string dba = string.Empty;
        GridViewRow grdRow = null;

        if (e.CommandSource is LinkButton)
        {
            if (grd1 != null)
            {
                grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                notes = grd1.DataKeys[grdRow.RowIndex].Values["Notes"].ToString();
                subject = grd1.DataKeys[grdRow.RowIndex].Values["NoteCode"].ToString();
            }
        }
        else
            return;

        switch (e.CommandName)
        {
            case "View":
                Subject.Text = subject;
                txtNotes.Text = Server.HtmlDecode(notes);
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                HtmlAnchor anc1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anc2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                if (!this._Export2Excel)
                {
                    anc1.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Open');");
                    anc2.Attributes.Add("onclick", "return OpenCloseHeader('" + anc1.ClientID + "','" + anc2.ClientID + "','Close');");
                }
                else
                {
                    //hide more/less since we're exporting to excel
                    anc1.Visible = false;
                    anc2.Visible = false;
                }

                break;

            case DataControlRowType.DataRow:


                string note = DataBinder.Eval(e.Row.DataItem, "Notes").ToString();

                e.Row.Cells[6].Attributes.Add("title", Server.HtmlDecode(note).Trim());

                Label Notes1 = ((Label)e.Row.Cells[4].FindControl("Notes1"));
                Label Notes2 = ((Label)e.Row.Cells[4].FindControl("Notes2"));

                HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                if (!this._Export2Excel)
                {
                    if (note.Length > 45)
                    {
                        Notes1.Text = note.Substring(0, 45).Trim() + "...  ";
                        anchor1.Attributes.Add("style", "display:inline;cursor: pointer;");
                    }
                    else
                    {
                        Notes1.Text = note.Trim() + "  ";
                        anchor1.Attributes.Add("style", "dispaly:none;");
                    }

                    anchor1.Attributes.Add("onclick", "return OpenClose('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Open');");
                    anchor2.Attributes.Add("onclick", "return OpenClose('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Close');");
                }
                else
                {
                    anchor1.Visible = false;
                    anchor2.Visible = false;
                }

                Notes2.Text = note;

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

}
