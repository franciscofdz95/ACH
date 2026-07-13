using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmTicketTemplates : frmBaseSearch
{ 

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "view")
        {
            int tempID = CommonUtility.Util.if_i(e.CommandArgument, 0);

            if (tempID > 0)
            {
                WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                TicketTemplate.FormShow(tempID.ToString());
            }
        }
    }

    private void BindGrid()
    {
        GridView1.PageIndex = this.CurrentPage - 1;
        GridView1.PageSize = this.PageSize;
        GridView1.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        TicketTemplate.ButtonClick += TicketTemplate_ButtonClick;
    }

    void TicketTemplate_ButtonClick(object sender, string args)
    {
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        
        string url = "~/SecureReports/frmTicketTemplates.aspx";

        Response.Redirect(url);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                string date = WebUtil.ConvertToUserShortDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateUpdated")));
                e.Row.Cells[7].Text = string.IsNullOrEmpty(date) ? "" : date;

                break;
        }
    }

    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@PageSize", this.PageSize);

        prms.Add("@CurrentPage", this.CurrentPage);

        if (string.IsNullOrWhiteSpace(this.SortOrder))
            this.SortOrder = "DateCreated";

        prms.Add("@SortOrder", this.SortOrder);

        prms.Add("@SortDirection", SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch));

        int rowcount = DataMerchantAppPaging.GetTicketTemplatesPagingCount(prms, this.PageSize, this.CurrentPage);

        lblRowCount.Text = rowcount.ToString();
        pnlPageSize.Visible = rowcount > 0;

        e.InputParameters[0] = prms;
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
        
        BindGrid();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        BindGrid();
    }

    protected void cboPageSize2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
        BindGrid();
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        TicketTemplate._TicketTemplateID = 0;
        TicketTemplate.FormNew();

    }      


}
