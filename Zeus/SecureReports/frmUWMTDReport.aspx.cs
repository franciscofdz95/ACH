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
using System.Text;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class frmUWMTDReport : frmBaseSearch
{
    protected override void OnLoad(EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

        base.OnLoad(e);

        rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //LookupTableHandler.LoadAgentsNew(AgentUID, true);
            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);

            //load all dropdownlists
            for (int i = 0; i < 24; i++)
            {
                string year = DateTime.Today.AddMonths(-i).ToString("yyyy");
                string month = DateTime.Today.AddMonths(-i).ToString("MM");
                cboPeriod.Items.Add(new ListItem(year + "/" + month, month + "/1/" + year));
            }
            Records.Visible = false;
            NoRecords.Visible = true;
        }
    }

    public override void Search(bool IsOnLoad)
    {
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        DataSet ds = null;

        Hashtable prms = new Hashtable();
        prms.Add("@Date", cboPeriod.SelectedItem.Value);

        if (wucAgentSelector.m_AgentUID != string.Empty)//AgentUID.SelectedIndex > 0)
            prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);//AgentUID.SelectedItem.Value);

        if (MerchantAppTypeUID.SelectedIndex > 0)
            prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

        ds = data.GetUWMTDReport(prms);
        DataView dv = ds.Tables[0].DefaultView;
        dv.Sort = this.SortOrder;

        Records.Visible = (dv.Table.Rows.Count > 0);
        NoRecords.Visible = !(dv.Table.Rows.Count > 0);

        this.ShowReport(ds.Tables[0]);
    }

    private void ShowReport(DataTable dt)
    {
        rptViewer.Width = new Unit("100%");
        rptViewer.Height = new Unit("1056px");
        string reportPath = "Reports/UWMTD.rdlc";
        rptViewer.LocalReport.DataSources.Clear();
        rptViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1_sp_Report_UW_MTD", dt));
        rptViewer.LocalReport.ReportPath = reportPath;
        
        rptViewer.LocalReport.Refresh();
    }

    void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
    {
        string reportPath = string.Empty;
        SqlCommand cmd = new SqlCommand();
        DataSet ds = null;

        switch (e.ReportPath)
        {
            case "UWMTDSub":
                Hashtable prms = new Hashtable();

                prms.Add("@Date", cboPeriod.SelectedItem.Value);

                if (wucAgentSelector.m_AgentUID != string.Empty)
                    prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);

                if (MerchantAppTypeUID.SelectedIndex > 0)
                    prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);

                cmd.CommandText = "sp_Report_UW_MTD_Percent";
                cmd.CommandType = CommandType.StoredProcedure;
                DataLayer.AppendParamters(cmd, prms);

                ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                e.DataSources.Clear();
                e.DataSources.Add(new ReportDataSource("DataSet1_sp_Report_UW_MTD_Percent", ds.Tables[0]));
                break;
        }
    }

    private void FormClear()
    {
        //FormHandler.ClearAllControls(this);
        //wucAgentSelector.FormClear();
        Response.Redirect(WebUtil.GetMyUrl());
    }

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
    }

    protected void btnKeywordSearch_Click(object sender, ImageClickEventArgs e)
    {
        this.Search(false);
    }

    public void ToggleButtons()
    {
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.Search(false);
    }
}
