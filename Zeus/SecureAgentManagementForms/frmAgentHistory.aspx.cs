using System;
using System.Data;
using System.Data.SqlClient;
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
using PaymentXP.Facade;
using ZeusWeb.Class;
using ZeusWeb.Extensions;

public partial class frmAgentHistory : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentAgent != null)
            base.UID = UserSessions.CurrentAgent.AgentUID;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkAgentHistory")).CssClass = "active";

        if (!this.IsPostBack)
        {
            LookupTableHandler.LoadChangeHistoryFields(ddlChangeType, true, ChangeHistoryFields.ChangeHistoryFieldSource.Agent);
            this.UID = UserSessions.CurrentAgent.AgentUID;
            this.FormShow(this.UID);
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text);

                break;
            case DataControlRowType.Footer:
                break;
            default:
                break;
        }
    }

    public override void FormShow(string ID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@AgentUID", this.UID);
        DataSet ds = DataAccess.DataAgentDao.GetAgentStatusHistory(prms);
        grd.DataSource = ds;
        grd.DataBind();
        lblStatus.Visible = !(grd.Rows.Count > 0);

        LoadAgentFieldHistory();

        ConfigurePaymentSettingFieldHistoryGridView();
        LoadPaymentSettingFieldHistory(0);       //PXP-15643:Code Changes

        ConfigureMerchantSplitsHistoryGridView(); //DM-1240
        LoadMerchantSplitsHistory(0);     //DM-1240
        ConfigureAgentResidualReportHistoryGridView();//DM-1373
        LoadAgentResidualReportHistory(0);      //DM-1373
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

    public override bool FormDataCheck()
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

    protected void ddlChangeType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadAgentFieldHistory();
    }

    private void LoadAgentFieldHistory()
    {
        DataTable dt = DataChangeLogs.SearchAgentChangeHistory(new Hashtable { { "@AgentID", Convert.ToInt32(UserSessions.CurrentAgent.AgentID) } });

        int ChangeHistoryFieldID = CommonUtility.Util.if_i(ddlChangeType.SelectedValue, 0);

        if (ChangeHistoryFieldID == 0)
        {
            this.grdChange.DataSource = dt;
            grdChange.Columns[0].Visible = true;
        }
        else
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = String.Format("ChangeHistoryFieldID='{0}'", ChangeHistoryFieldID);
            this.grdChange.DataSource = dv;
            grdChange.Columns[0].Visible = false;
        }

        this.grdChange.DataBind();
    }
    protected void grdChange_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlChangeType.SelectedIndex == 0)
            {
                ((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = "Field Value";
            }
            else
            {
                ((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = ddlChangeType.SelectedItem.Text;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();
            value = value.Replace("\\", ";<br>");

            string name = DataBinder.Eval(e.Row.DataItem, "Description").ToString();

            name = name.Replace("\\", ";<br>");

            ((Label)e.Row.Cells[1].FindControl("lblValue")).Text = value;
            ((Label)e.Row.Cells[0].FindControl("lblName")).Text = name;
            e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);
        }
    }

    public void ConfigurePaymentSettingFieldHistoryGridView()
    {
        this.grvPaymentSettingChange.DefaultWithPager();
    }
    //PXP-15643:Code Changes:Start
    private void LoadPaymentSettingFieldHistory(int pageIndex)
    {
        var prams = new Hashtable();
        prams.Add("@AgentID", UserSessions.CurrentAgent.AgentID);
        prams.Add("@PageSize", this.grvPaymentSettingChange.PageSize);
        prams.Add("@PageIndex", pageIndex);
        prams.Add("@SortDirection", 1);

        DataTable dt = DataChangeLogs.SearchPaymentSettingChangeHistory(prams);
        this.grvPaymentSettingChange.SetDataSource(dt, pageIndex);
    }
    //PXP-15643:Code Changes:End
    protected void grvPaymentSettingChange_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ddlChangeType.SelectedIndex == 0)
            {
                ((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = "Field Value";
            }
            else
            {
                ((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = ddlChangeType.SelectedItem.Text;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();
            value = value.Replace("\\", ";<br>");

            string name = DataBinder.Eval(e.Row.DataItem, "Description").ToString();

            name = name.Replace("\\", ";<br>");

            ((Label)e.Row.Cells[1].FindControl("lblValue")).Text = value;
            ((Label)e.Row.Cells[0].FindControl("lblName")).Text = name;
            e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);

        }


    }
    protected void grvPaymentSettingChange_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadPaymentSettingFieldHistory(e.NewPageIndex);
    }
    //DM-1240:start
    private void ConfigureMerchantSplitsHistoryGridView()
    {
        this.grvMerchantSplitsHistory.DefaultWithPager();

        this.grvMerchantSplitsHistory.AddBoundColumn("ZID", "ZID");
        this.grvMerchantSplitsHistory.AddBoundColumn("DBA ID", "DBAID");
        this.grvMerchantSplitsHistory.AddBoundColumn("DBA", "DBA");
        this.grvMerchantSplitsHistory.AddBoundColumn("Field Name", "FieldName");
        this.grvMerchantSplitsHistory.AddBoundColumn("Old Value", "OldValue");
        this.grvMerchantSplitsHistory.AddBoundColumn("New Value", "NewValue");
        this.grvMerchantSplitsHistory.AddBoundColumn("Changed Date", "ChangedDate");
        this.grvMerchantSplitsHistory.AddBoundColumn("Changed By", "ChangedBy");
    }
    private void LoadMerchantSplitsHistory(int pageIndex)
    {
        var prams = new Hashtable();
        prams.Add("@AgentID", UserSessions.CurrentAgent.AgentID);
        prams.Add("@PageSize", this.grvMerchantSplitsHistory.PageSize);
        prams.Add("@PageIndex", pageIndex);
        prams.Add("@SortDirection", 1);

        DataTable dt = DataChangeLogs.SearchMerchantSplitsHistory(prams);
        this.grvMerchantSplitsHistory.SetDataSource(dt, pageIndex);
    }
    protected void grvMerchantSplitsHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadMerchantSplitsHistory(e.NewPageIndex);
    }
    //DM-1240:End       

    //DM-1373: Add tracking to logs Part 3 Agent Fees
    private void ConfigureAgentResidualReportHistoryGridView()
    {
        this.grdAgentResidualReportItemsHistory.DefaultWithPager();
    }
    private void LoadAgentResidualReportHistory(int pageIndex)
    {
        var prams = new Hashtable();
        prams.Add("@AgentID", UserSessions.CurrentAgent.AgentID);
        prams.Add("@PageSize", this.grdAgentResidualReportItemsHistory.PageSize);
        prams.Add("@PageIndex", pageIndex);
        prams.Add("@SortDirection", 1);

        DataTable dt = DataChangeLogs.SearchAgentResidualReportHistory(prams);
        this.grdAgentResidualReportItemsHistory.SetDataSource(dt, pageIndex);
        this.grdAgentResidualReportItemsHistory.DataBind();
    }
    protected void grdAgentResidualReportItemsHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LoadAgentResidualReportHistory(e.NewPageIndex);
    }
    //DM-1373: End
}
