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
using PaymentXP.Facade;
using PaymentXP.DataObjects;
using Infragistics.WebUI.WebSchedule;

public partial class wucLeadsQueue : wucBaseSearch
{
    
    public string StatusUID
    {
        get
        {
            if (ViewState["StatusUID"] == null)
                return string.Empty;
            else
                return ViewState["StatusUID"].ToString();
        }
        set { ViewState["StatusUID"] = value; }

    }

       

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void SetDataSource(Hashtable prms, string Title)
    {
        lblTitle.Text = Title;

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);

        this.m_Prms = prms;
        BindGrid();
    }

    private void BindGrid()
    {
        if (!m_Prms.ContainsKey("@PageSize"))
            m_Prms.Add("@PageSize", this.PageSize);
        else
            m_Prms["@PageSize"] = this.PageSize;

        if (!m_Prms.ContainsKey("@CurrentPage"))
            m_Prms.Add("@CurrentPage", this.CurrentPage);
        else
            m_Prms["@CurrentPage"] = this.CurrentPage;

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", "BusinessDBAName");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetLeadsPagingRowCount(m_Prms, 0, 0, this.ID).ToString();

        grd.DataBind();

        pnlRecords.Visible = grd.Rows.Count != 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                //((CheckBox)e.Row.FindControl("chkSelectAll")).Attributes.Add("onclick", "SelectAll('" + ((CheckBox)e.Row.FindControl("chkSelectAll")).ClientID + "');");

                break;

            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lbtnLeadID");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LeadID").ToString();

               

                if (DataBinder.Eval(e.Row.DataItem, "FollowupStatusUID").ToString().ToUpper() == Constants.HOT)
                    e.Row.BackColor = System.Drawing.Color.LightBlue;

                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        string status = string.Empty;
        string leaduid = string.Empty;
        string assignto = string.Empty;
        GridViewRow grdRow = null;
     
        if (e.CommandName!= "ID")
            return;

        LinkButton btn = (LinkButton)e.CommandSource;
        grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

        if (btn.Text == "0")
        {
            return;
        }


        DataLead objdataLead = DataAccess.DataLeadDao;
        Lead lead = objdataLead.GetLead(grd.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString());
        //UserSessions.CurrentLeadUID = lead.LeadUID; 
        //UserSessions.CurrentLeadDBA = lead.DBAName;

        switch (e.CommandName)
        {
            case "ID":

                url = "frmLeadsDetail.aspx?Adding=false";
                url += "&PostBackURL=~/SecureLeadForms/frmLeads.aspx";
                url += "&LeadID=" + grd.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString();
                url += "&LeadUID=" + grd.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString();

                Response.Redirect(url);
                break;


        }


    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.BindGrid();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.grd.PageSize = this.PageSize;
        this.BindGrid();
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

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.BindGrid();
    }

    protected void odsLeads_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
        e.InputParameters[3] = this.ID;
    }
}
