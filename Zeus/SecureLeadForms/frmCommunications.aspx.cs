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

public partial class SecureLeadForms_frmCommunications : frmBaseSearch
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HyperLink)this.Master.FindControl("lnkEmail")).CssClass = "active";

        if (!this.IsPostBack)
        {
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Email Search";
            //LookupTableHandler.LoadAgentsNew(AgentAgentID, true);

            this.Search(true);
        }
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SearchParameters = null;
        this.Search(false);
    }

    public override void Search(bool IsOnLoad)
    {
        Communication comm;

        //Populate search fields
        if (IsOnLoad && this.SearchParameters != null)
        {
            comm = (Communication)this.SearchParameters;
            FormBinding.BindObjectToControls(comm, pnlSearch);
        }

        Hashtable prms = new Hashtable();

        if (IsOnLoad && this.SearchParameters == null)
        {
            prms.Add("@UID", "00000000-0000-0000-0000-000000000000");
        }
        else
        {
            if (BusinessDBAName.Text != string.Empty)
                prms.Add("@DBAName", BusinessDBAName.Text);

            if (wucAgentSelector.m_AgentUID != string.Empty)
                prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

            if (Subject.Text != string.Empty)
                prms.Add("@Subject", Subject.Text);

            if (Cc.Text != string.Empty)
                prms.Add("@Cc", Cc.Text);

            if (To.Text != string.Empty)
                prms.Add("@To", To.Text);

            //Save search fields
            comm = new Communication();
            FormBinding.BindControlsToObject(comm, pnlSearch);
            this.SearchParameters = comm;
        }

        if (prms.Count == 0)
            prms.Add("@UID", "00000000-0000-0000-0000-000000000000");

        DataCommunication data = DataAccess.DataCommunicationDao;
        DataSet ds = null;

        ds = data.GetCommunications(prms);

        DataView dv = ds.Tables[0].DefaultView;

        lblRecordCount.Text = "Total Records Found: " + dv.Table.Rows.Count.ToString();

        if (this.SortOrder == string.Empty)
            this.SortOrder = "CommunicationID";
        dv.Sort = this.SortOrder + " " + ConvertSortDirectionToSql(this.SortDirectionSearch);

        grdCommunication.DataSource = dv;
        grdCommunication.DataBind();

        lblData.Visible = (grdCommunication.Rows.Count == 0);
        lblRecordCount.Visible = !(lblData.Visible);
    }

    private string ConvertSortDirectionToSql(SortDirection direction)
    {
        string newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = "DESC";
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = "ASC";
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    private void FormClear()
    {
        FormHandler.ClearAllControls(this);
        lblRecordCount.Text = "Total Record(s) Found: 0";
        this.SearchParameters = null;

        grdCommunication.DataSource = null;
        grdCommunication.DataBind();
        wucAgentSelector.FormClear();
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnAddAppointment_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        // TOL: come back to this, test this.
        string url = "frmCommunicationsDetail.aspx?PostBackURL=&Adding=true";
        Response.Redirect(url);
    }

    protected void grdCommunication_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandSource is LinkButton))
            return;

        LinkButton lnk = (LinkButton)e.CommandSource;
        GridViewRow grdRow = (GridViewRow)lnk.NamingContainer;

        switch (lnk.CommandName)
        {
            case "Edit":
                //UserSessions.CurrentLeadUID = grdCommunication.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString();
                //UserSessions.CurrentLeadDBA = grdRow.Cells[2].Text;

                string url = "frmCommunicationsDetail.aspx?PostBackURL=frmCommunications.aspx&Adding=false&CommunicationUID=" + grdCommunication.DataKeys[grdRow.RowIndex].Values["CommunicationID"].ToString();
                Response.Redirect(url);
                break;

            case "Delete":
                break;
        }
    }

    protected void grdCommunication_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton lnk = (LinkButton)e.Row.FindControl("lbtnMerchantID");
                if (lnk != null)
                    lnk.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();

                break;

            default:
                break;
        }
    }

    protected void grdCommunication_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCommunication.PageIndex = e.NewPageIndex;
        Search(false);
    }

    protected void grdCommunication_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression != this.SortOrder)
            this.SortDirectionSearch = e.SortDirection;
        this.SortOrder = e.SortExpression;
        Search(false);
    }
}
