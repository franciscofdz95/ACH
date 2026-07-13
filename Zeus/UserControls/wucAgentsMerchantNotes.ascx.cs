using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucAgentsMerchantNotes : wucBaseSearch
    {
        Hashtable prms = new Hashtable();

        public string AgentUID
        {
            get { return Convert.ToString(ViewState["UID"]); }
            set { ViewState["UID"] = Convert.ToString(value); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.LoadAgentsMerchantNotes();
            }

        }

        protected void odsNotes_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (string.IsNullOrEmpty(this.AgentUID) && UserSessions.CurrentAgent.AgentUID != null)
                this.AgentUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
            else if (string.IsNullOrEmpty(this.AgentUID))
                this.AgentUID = Guid.Empty.ToString(); //"00000000-0000-0000-0000-000000000000";

            if (prms.Count <= 0)
                prms.Add("@AgentUID", this.AgentUID);
            e.InputParameters[0] = prms;
            e.InputParameters[3] = this.grdNotes.ID;
        }

        protected void grdNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNotes.PageIndex = e.NewPageIndex;
            this.CurrentPage = e.NewPageIndex + 1;
            this.LoadAgentsMerchantNotes();
        }

        protected void grdNotes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string subject = string.Empty;
            string notes = string.Empty;
            string dba = string.Empty;
            GridViewRow grdRow = null;

            if (e.CommandSource is LinkButton)
            {
                if (grdNotes != null)
                {
                    grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    notes = grdNotes.DataKeys[grdRow.RowIndex].Values["Notes"].ToString();
                    subject = grdNotes.DataKeys[grdRow.RowIndex].Values["Subject"].ToString();
                    dba = grdNotes.DataKeys[grdRow.RowIndex].Values["BusinessDBAName"].ToString();
                    chkInternal.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_MPSAll"]);
                    chkAgent.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Agent"]);
                    chkMerchant.Checked = DataLayer.Field2Bool(grdNotes.DataKeys[grdRow.RowIndex].Values["View_Merchant"]);
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

        protected void grdNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:

                    HtmlAnchor anc1 = ((HtmlAnchor)e.Row.FindControl("lnkmore11"));
                    HtmlAnchor anc2 = ((HtmlAnchor)e.Row.FindControl("lnkless11"));
                    GridView grid = ((GridView)this.grdNotes);
                    anc1.Attributes.Add("onclick", "return OpenCloseHeaderAMN('" + anc1.ClientID + "','" + anc2.ClientID + "','Open'" + ",'" + grid.ClientID + "');");
                    anc2.Attributes.Add("onclick", "return OpenCloseHeaderAMN('" + anc1.ClientID + "','" + anc2.ClientID + "','Close'" + ",'" + grid.ClientID + "');");

                    break;

                case DataControlRowType.DataRow:

                    //Convert html to text and display on the grid and tool tip. The html should still show on click of note.
                    string note = (DataBinder.Eval(e.Row.DataItem, "Notes").ToString());
                    note = WebUtil.ConvertHtml(Server.HtmlDecode(note));
                    e.Row.Cells[4].Attributes.Add("title", note);

                    Label Notes1 = ((Label)e.Row.Cells[4].FindControl("NotesMore1"));
                    Label Notes2 = ((Label)e.Row.Cells[4].FindControl("NotesLess1"));
                    HtmlAnchor anchor1 = ((HtmlAnchor)e.Row.FindControl("lnkmore1"));
                    HtmlAnchor anchor2 = ((HtmlAnchor)e.Row.FindControl("lnkless1"));
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
                    anchor1.Attributes.Add("onclick", "return OpenCloseAMN('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Open');");
                    anchor2.Attributes.Add("onclick", "return OpenCloseAMN('" + Notes1.ClientID + "','" + Notes2.ClientID + "','" + anchor1.ClientID + "','" + anchor2.ClientID + "','Close');");
                    Notes2.Text = CommonUtility.Formatting.nl2br(note);
                    
                    e.Row.Cells[5].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[5].Text);

                    break;

                case DataControlRowType.Footer:
                    break;

                default:
                    break;
            }
        }

        protected void grdNotes_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (this.SortOrder != e.SortExpression)
                this.SortDirectionSearch = e.SortDirection;
            this.SortOrder = e.SortExpression;
            this.LoadAgentsMerchantNotes();
        }

        public void LoadAgentsMerchantNotes()
        {
            prms = new Hashtable();
            if (string.IsNullOrEmpty(this.AgentUID) && UserSessions.CurrentAgent != null)
                this.AgentUID = UserSessions.CurrentAgent.AgentUID;

            if (!string.IsNullOrEmpty(this.AgentUID))
            {
                prms.Add("@PageSize", this.PageSize);
                prms.Add("@CurrentPage", this.CurrentPage);
                if (SortOrder == string.Empty || SortOrder.ToUpper().Equals("ID"))
                    SortOrder = "MerchantNoteID";
                prms.Add("@SortOrder", this.SortOrder);
                prms.Add("@SortDirection", this.SortDirectionSearch);
                prms.Add("@AgentUID", this.AgentUID);
                if (!String.IsNullOrEmpty(this.ZID.Text.Trim()))
                {
                    prms.Add("@ID", this.ZID.Text.Trim());
                }

                grdNotes.DataBind();

                int cnt = DataMerchantAppPaging.GetMerchantNotesPagingCount(prms, this.PageSize, this.CurrentPage, this.grdNotes.ID);
                lblRecordCount.Text = "Total records found: " + cnt.ToString();
             
            }
        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            grdNotes.PageSize = this.PageSize;
            this.LoadAgentsMerchantNotes();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            grdNotes.PageSize = this.PageSize;
            this.LoadAgentsMerchantNotes();
        }
    }
}