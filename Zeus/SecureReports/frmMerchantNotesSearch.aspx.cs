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
using Infragistics.WebUI.WebHtmlEditor;


public partial class frmMerchantNotesSearch : frmBaseSearch
{

    private bool _Export2Excel = false;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadInternalUsers(UserUID, true);
            LookupTableHandler.MerchantAppStatus(StatusUID, true, "Merchant Management");
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);
            LookupTableHandler.LoadMerchant_NoteCodes(NoteCodes, true, "");
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        SearchParameter app;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            app = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(app, pnlSearch);
        }


        grd.PageSize = this.PageSize;
        grd.PageIndex = this.CurrentPage - 1;

        grd.DataBind();

        lblTitle.Text = "Merchant Notes";

    }

    protected void odsNotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        SearchParameter app;

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        if (!string.IsNullOrEmpty(SearchBeginDate.Text))
            prms.Add("@BeginPostedDate", SearchBeginDate.Value);

        if (!string.IsNullOrEmpty(SearchEndDate.Text))
            prms.Add("@EndPostedDate", SearchEndDate.Value);

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (BusinessLegalName.Text != string.Empty)
            prms.Add("@BusinessLegalName", BusinessLegalName.Text);

        if (BusinessDBAName.Text != string.Empty)
            prms.Add("@BusinessDBAName", BusinessDBAName.Text);

        if (wucAgentSelector.m_AgentUID != string.Empty)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

        if (UserUID.SelectedIndex > 0)
            prms.Add("@UserUID", UserUID.SelectedItem.Value);

        if (StatusUID.SelectedIndex > 0)
            prms.Add("@StatusUID", StatusUID.SelectedItem.Value);

        if (MerchantID.Text != string.Empty)
            prms.Add("@ID", MerchantID.Text);

        if (Notes.Text != string.Empty)
            prms.Add("@Notes", Notes.Text.Trim());

        if (SettlePlatformMid.Text != string.Empty)
            prms.Add("@SettlePlatformMid", SettlePlatformMid.Text);

        if (MerchantAppTypeUID.SelectedIndex > 0)
            prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

        if (NoteCodes.SelectedIndex > 0)
            prms.Add("@Subject", NoteCodes.SelectedItem.Text);

        int cnt = 0;

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            //user is passed as a parameter to determine whether the user is an agent or manager
            User user = UserSessions.CurrentUser;
            prms.Add("@UserName", user.UserName);

            prms.Add("@PageSize", this.PageSize);
            prms.Add("@CurrentPage", this.CurrentPage);

            if (SortOrder == string.Empty)
                SortOrder = "MerchantNoteID";
            prms.Add("@SortOrder", this.SortOrder);
            prms.Add("@SortDirection", this.SortDirectionSearch);

            if (UserSessions.CurrentUser.IsAgent)
                prms.Add("@View_Agent", true);
            else if (UserSessions.CurrentUser.IsBank)
                prms.Add("@View_Bank", true);

            if (UserSessions.CurrentUser.IsInternal)
                prms.Add("@View_MPSALL", true);

            cnt = DataMerchantAppPaging.GetMerchantNotesPagingCount(prms, this.PageSize, this.CurrentPage, this.grd.ID);
        }
        else
        {
            prms.Add("@ID", "-1");
        }

        e.InputParameters[0] = prms;
        e.InputParameters[3] = this.grd.ID;

        lblRecordCount.Text = "Total records found: " + cnt.ToString();
        
        pnl1.Visible = (cnt > 0);
        NoData.Visible = !(cnt > 0);

    }
    
    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
        this.SearchParameters = null;
        //this.Search(false);
        wucAgentSelector.FormClear();
        pnl1.Visible = false;
        NoData.Visible = true;

        this.SortOrder = string.Empty;
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        this.SortDirectionSearch = SortDirection.Descending;
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SortOrder = string.Empty;
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        this.SortDirectionSearch = SortDirection.Descending;

        this.SearchParameters = null;
        this.Search(false);
    }

    private void FormClear()
    {
        FormHandler.ClearAllControls(this);
        Notes.Text = "";
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {

        this._Export2Excel = true;

        if (rdExport.SelectedValue.Equals("1"))
        {

            this.PageSize = 5000;
            this.CurrentPage = 1;
        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = this.grd.PageIndex + 1;
        }

        Search(false);

        FormHandler.Export2Excel("MerchantNotesList.xls", grd);

    }

    //protected void btnExportPDF_Click(object sender, EventArgs e)
    //{
    //    this._Export2Excel = true;


    //    if (rdExport.SelectedValue.Equals("1"))
    //    {

    //        this.PageSize = 5000;
    //        this.CurrentPage = 1;
    //    }
    //    else
    //    {
    //        this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
    //        this.CurrentPage = this.grd.PageIndex + 1;
    //    }

    //    Search(false);

    //    FormHandler.ExportToPDF(grd, false, "Merchant Notes List");
    //}

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string subject = string.Empty;
        string notes = string.Empty;
        string dba = string.Empty;
        GridViewRow grdRow = null;

        if (e.CommandSource is LinkButton)
        {
            if (grd != null)
            {
                grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                notes = grd.DataKeys[grdRow.RowIndex].Values["Notes"].ToString();
                subject = grd.DataKeys[grdRow.RowIndex].Values["Subject"].ToString();
                dba = grd.DataKeys[grdRow.RowIndex].Values["BusinessDBAName"].ToString();
                chkInternal.Checked = DataLayer.Field2Bool(grd.DataKeys[grdRow.RowIndex].Values["View_MPSAll"]);
                chkAgent.Checked = DataLayer.Field2Bool(grd.DataKeys[grdRow.RowIndex].Values["View_Agent"]);
            }
        }
        else
            return;

        switch (e.CommandName)
        {
            case "View":
                txtSubject.Text = subject;
                //Show the Binded html so that it is readable. Decodeing the encoed value so that the text is clear.
                txtNotes.Text = CommonUtility.Formatting.nl2br(System.Web.HttpUtility.HtmlDecode(notes));
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                break;

            case "Ticket":

                string TicketUID = ((LinkButton)grdRow.FindControl("hypTID")).CommandArgument.Trim();
                ((LinkButton)grdRow.FindControl("hypTID")).Attributes.Add("onclick", "window.open('../../SecureTicketForms/frmTicketPopup.aspx?TicketUID=" + TicketUID + "&Adding=false');");
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
                //Convert html to text and display on the grid and tool tip. The html should still show on click of note.
                string note = (DataBinder.Eval(e.Row.DataItem, "Notes").ToString());
                note = WebUtil.ConvertHtml(Server.HtmlDecode(note));

                e.Row.Cells[5].Attributes.Add("title",note);

                Label Notes1 = ((Label)e.Row.Cells[5].FindControl("Notes1"));
                Label Notes2 = ((Label)e.Row.Cells[5].FindControl("Notes2"));

                HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnk1"));
                HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnk2"));

                if (!this._Export2Excel)
                {
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
                }
                else
                {
                    anchor1.Visible = false;
                    anchor2.Visible = false;
                }

                Notes2.Text = CommonUtility.Formatting.nl2br(note);


                if (UserSessions.CurrentUser.IsBank)
                {
                    grd.Columns[1].Visible = false; //Email Link
                    grd.Columns[5].Visible = false; //Call Back
                    grd.Columns[6].Visible = false; //Is Private

                }

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }
    
    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        Search(false);
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        Search(false);
    }

}
