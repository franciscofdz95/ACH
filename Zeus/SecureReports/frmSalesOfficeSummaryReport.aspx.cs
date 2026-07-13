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
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class frmSalesOfficeSummaryReport : frmBaseSearch
{
    // used to calculate totals
    public int _received_total = 0;
    public int _approved_total = 0;
    public int _declined_total = 0;
    public int _withdrawn_total = 0;
    public int _closed_total = 0;
    public int _pended_total = 0;
    public int _appended_total = 0;
    public int _portfolio_count = 0;
    public decimal _processing_volume_count = 0m;


    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.always_init();
            this.initialize();

            LookupTableHandler.LoadUsersByRole(PrimaryContactUID, true, Constants.ROLE_AGENT_RELATIONS);
            LookupTableHandler.GetAgentCategories(AgentCategoryUID);
            LookupTableHandler.LoadPartnerChannels(PartnerChannel, true);

            SetPartnerChannelAccess();
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);
    }

    protected void always_init()
    {
        blError.Items.Clear();
    }

    protected void initialize()
    {
        //Set the current page
        //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Merchant Search";

        //Apply security settings
        FormHandler.SetSecurity(this.Page);

        //// set default start. we'll do at the start of the month.
        //DateStart.Value = DateTime.Now.AddDays((DateTime.Now.Day * -1) + 1);

        //// set the enddate to today.
        //DateEnd.Value = DateTime.Today;
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

        if (this.isDataValid())
        {
            Hashtable prms = this.getPrmsList();

            //Save search fields in session variable
            app = new SearchParameter();
            FormBinding.BindControlsToObject(app, pnlSearch);
            this.SearchParameters = app;

            DataAgent data = DataAccess.DataAgentDao;
            DataSet ds = null;

            ds = data.GetReportSalesOfficeSummary(prms);

            if (ds.Tables.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "Received Desc";

                GridView1.PageSize = this.PageSize;
                GridView1.PageIndex = this.CurrentPage - 1;

                GridView1.DataSource = dv;
                GridView1.DataBind();

                //lblRecordCount.Text = "Total Records Found : " + ds.Tables[0].Rows.Count.ToString();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

            pnlRecords.Visible = (GridView1.Rows.Count > 0);
            pnlNoRecords.Visible = !(GridView1.Rows.Count > 0);
        }
    }

    private bool isDataValid()
    {
        if (!(DateStart.Value != null && DateStart.Value.ToString() != "" && Convert.ToDateTime(DateStart.Value) != DateTime.MinValue))
        {
            blError.Items.Add("Start Date Required");
            return false;
        }

        if (!(DateEnd.Value != null && DateEnd.Value.ToString() != "" && Convert.ToDateTime(DateEnd.Value) != DateTime.MinValue))
        {

            blError.Items.Add("End Date Required");
            return false;
        }

        DateTime dtS = DateTime.Parse(DateStart.Text);
        DateTime dtE = DateTime.Parse(DateEnd.Text);

        if (dtS > dtE)
        {
            blError.Items.Add("Begin Date must be less than End Date");
            return false;
        }

        if (UserSessions.CurrentUser.IsAgent && PartnerChannel.SelectedIndex < 0)
        {
            blError.Items.Add("Agent must have a channel.");
            return false;
        }

        return true;
    }

    private void FormClear()
    {
        this.SearchParameters = null;

        FormHandler.ClearAllControls(this);

        AgentCategoryUID.SelectedIndex = -1;

        if (!UserSessions.CurrentUser.IsAgent)
            PartnerChannel.SelectedIndex = -1;

        pnlRecords.Visible = false; 
        pnlNoRecords.Visible = true;

        this.CurrentPage = 1;
        this.PageSize = 10;

    }

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
    }

    private bool isFullMonth(DateTime dtS, DateTime dtE)
    {
        return (dtS.Day == 1 && dtE.Day == 1 && dtS < dtE) ? true : false;
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.SortOrder = string.Empty;
        this.SearchParameters = null;
        this.CurrentPage = 1;
        this.PageSize = 10;
        cboPageSize.SelectedIndex = 0;
        always_init();

        if (chkSubAgent.Checked && wucAgentSelector.m_AgentUID == string.Empty)
        {
            chkSubAgent.Checked = false;
            FormHandler.DisplayMessage(Page.ClientScript, "Please select an agent.");
        }
        else
            this.Search(false);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            if (chkSubAgent.Checked)
            {
                GridView1.ShowFooter = true;
                e.Row.Cells[0].Text = "Totals:";

                e.Row.Cells[2].Text = _received_total.ToString();
                e.Row.Cells[3].Text = _approved_total.ToString();
                e.Row.Cells[4].Text = _declined_total.ToString();
                e.Row.Cells[5].Text = _withdrawn_total.ToString();
                e.Row.Cells[6].Text = _closed_total.ToString();
                e.Row.Cells[7].Text = _pended_total.ToString();

                e.Row.Cells[14].Text = _portfolio_count.ToString();
                e.Row.Cells[15].Text = string.Format("{0:0.00}", _processing_volume_count);

                if (_received_total != 0)
                {
                    e.Row.Cells[8].Text = string.Format("Avg: {0:F2}%", (Convert.ToDecimal(_approved_total) / Convert.ToDecimal(_received_total)) * 100);
                    e.Row.Cells[9].Text = string.Format("Avg: {0:F2}%", (Convert.ToDecimal(_withdrawn_total) / Convert.ToDecimal(_received_total)) * 100);


                    e.Row.Cells[10].Text = string.Format("Avg: {0:F2}%", (Convert.ToDecimal(_appended_total) / Convert.ToDecimal(_received_total)) * 100);
                    e.Row.Cells[11].Text = string.Format("Avg: {0:F2}%", (Convert.ToDecimal(_pended_total) / Convert.ToDecimal(_received_total)) * 100);
                }

            }
            else
            {
                GridView1.ShowFooter = false;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (chkSubAgent.Checked)
            {
                GridView1.ShowFooter = true;
                DataRowView drv = (DataRowView)e.Row.DataItem;

                this._received_total += Convert.ToInt32(drv["Received"]);
                this._approved_total += Convert.ToInt32(drv["Approved"]);
                this._declined_total += Convert.ToInt32(drv["Declined"]);
                this._withdrawn_total += Convert.ToInt32(drv["Withdrawn"]);
                this._closed_total += Convert.ToInt32(drv["Closed"]);
                this._pended_total += Convert.ToInt32(drv["Pended"]);


                this._appended_total += Convert.ToInt32(drv["APPended"]);

                this._portfolio_count += Convert.ToInt32(drv["Portfolio Count"]);
                this._processing_volume_count += Convert.ToDecimal(drv["Processing Volume"]);

            }
            else
            {
                GridView1.ShowFooter = false;
            }

            //    decimal dReceived = Convert.ToDecimal(drv["AP - Received"]);

            //    decimal dApproved = Convert.ToDecimal(drv["CU - Approved"]);


            //    Label lbApprovalPer = (Label)e.Row.FindControl("lblApprovalPer");
            //    lbApprovalPer.Text = (dReceived != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dApproved / dReceived) * 100m) : "0.00%";

            //    decimal dWithdrawn = Convert.ToDecimal(CommonUtility.Util.if_i(drv["WithdrawnWithAPRECEIVED"], 0));

            //    Label lbWithdrawn = (Label)e.Row.FindControl("lblWithdrawn");
            //    lbWithdrawn.Text = (dWithdrawn != 0m) ? CommonUtility.Util.if_s(dWithdrawn, "0") : "0";

            //    Label lbWithdrawnPer = (Label)e.Row.FindControl("lblWithdrawnPer");
            //    lbWithdrawnPer.Text = (dReceived != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dWithdrawn / dReceived) * 100m) : "0.00%";

            //    decimal dAppPending = Convert.ToDecimal(drv["AP - Pending"]);
            //    Label lbAppPendingPer = (Label)e.Row.FindControl("lblAppPendingPer");
            //    lbAppPendingPer.Text = (dReceived != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dAppPending / dReceived) * 100m) : "0.00%";

            //    decimal dCuPending = Convert.ToDecimal(drv["CU - Pending"]);
            //    Label lbCreditPendingPer = (Label)e.Row.FindControl("lblCreditPendingPer");
            //    lbCreditPendingPer.Text = (dReceived != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dCuPending / dReceived) * 100m) : "0.00%";

            //    decimal dSwipeCount = Convert.ToDecimal(drv["SwipeCount"]);
            //    Label lbCardPresentPer = (Label)e.Row.FindControl("lblCardPresentPer");
            //    lbCardPresentPer.Text = (dReceived != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dSwipeCount / dReceived) * 100m) : "0.00%";

            //    decimal dHighRiskCount = Convert.ToDecimal(drv["HighRiskCount"]);
            //    Label lbHighRiskPer = (Label)e.Row.FindControl("lblHighRiskPer");
            //    lbHighRiskPer.Text = (dReceived != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dHighRiskCount / dReceived) * 100m) : "0.00%";


            //    string strProcVol = (drv["ProcVolume"] == DBNull.Value || drv["ProcVolume"].ToString() == "") ? "0" : drv["ProcVolume"].ToString();

            //    decimal dProcVol = Convert.ToDecimal(strProcVol);
            //    Label lbPortProcVol = (Label)e.Row.FindControl("lblPortProcVol");
            //    lbPortProcVol.Text = (dProcVol != 0m) ? string.Format("{0:C}", Convert.ToDecimal(dProcVol)) : "$0.00";

            //    string strProcVolAvg = (drv["ProcVolumeAvg"] == DBNull.Value || drv["ProcVolumeAvg"].ToString() == "") ? "0" : drv["ProcVolumeAvg"].ToString();

            //    decimal dProcVolAvg = Convert.ToDecimal(strProcVolAvg);
            //    Label lbPortCapProcPer = (Label)e.Row.FindControl("lblPortCapProcPer");

            //    string strProcCount = (drv["ProcVolumeCount"] == DBNull.Value || drv["ProcVolumeCount"].ToString() == "") ? "0" : drv["ProcVolumeCount"].ToString();
            //    Label lbPortProcCount = (Label)e.Row.FindControl("lblPortProcCount");
            //    lbPortProcCount.Text = strProcCount;


            //    if (this._isValidMonthIntervals == true)
            //    {
            //        lbPortCapProcPer.Text = (dProcVolAvg != 0m) ? string.Format("{0:f2}%", Convert.ToDecimal(dProcVol / dProcVolAvg) * 100m) : "0.00%";
            //    }
            //    else
            //    {
            //        lbPortCapProcPer.Text = "N/A";
            //    }

        }
    }

    protected void lbSentToExcel_Click(object sender, EventArgs e)
    {
        this.PageSize = 10000;
        Search(false);
        FormHandler.Export2Excel(string.Format("SalesOfficeSummaryReport_{0}.xls", DateTime.Now.ToString("yyyy-MM-dd")), GridView1);
    }

    protected void lbSendToPDF_Click(object sender, EventArgs e)
    {
        //if (rdExport.SelectedValue.Equals("1"))
        //{
        //    this.PageSize = 9999;
        //    this.CurrentPage = 1;
        //}
        //else
        //{
        //    this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        //    this.CurrentPage = GridView1.PageIndex + 1;
        //}

        //Search(false);
        //this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
        //FormHandler.ExportToPDF(GridView1, true, string.Format("SalesOfficeSummaryReport_{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd")));
    }

    protected void ddlTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
    {

        switch (ddlTimeFrame.SelectedValue)
        {
            case "1":
                // last 30 days
                DateStart.Value = DateTime.Now.AddDays(-30);
                DateEnd.Value = DateTime.Now;

                break;

            case "2":
                // last 60 days
                DateStart.Value = DateTime.Now.AddDays(-60);
                DateEnd.Value = DateTime.Now;
                break;

            case "3":
                // last 90 days
                DateStart.Value = DateTime.Now.AddDays(-90);
                DateEnd.Value = DateTime.Now;
                break;

            case "4":
                // last month
                DateTime dtLastMonth = DateTime.Now.AddMonths(-1);
                DateStart.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, 1);
                DateEnd.Value = new DateTime(dtLastMonth.Year, dtLastMonth.Month, DateTime.Now.AddDays(DateTime.Now.Day * -1).Day);
                break;

            case "5":
                // month to date
                DateStart.Value = DateTime.Now.AddDays((DateTime.Now.Day * -1) + 1);
                DateEnd.Value = DateTime.Now;
                break;

            default:
                // change your dates
                DateStart.Value = DateTime.Today;
                DateEnd.Value = DateTime.Today;

                break;

        }

    }

    private Hashtable getPrmsList()
    {

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        // pull this from the post!!        

        if (wucAgentSelector.m_AgentUID != string.Empty)
        {
            if (chkSubAgent.Checked)
                prms.Add("@MasterAgentUID", wucAgentSelector.m_AgentUID);
            else
                prms.Add("@AgentUID", wucAgentSelector.m_AgentUID);
        }

        if (DateStart.Value != null && DateStart.Value.ToString() != "")
        {
            prms.Add("@BeginPostedDate", DateStart.Value);
        }

        if (DateEnd.Value != null && DateEnd.Value.ToString() != "")
        {
            // add one day to it.
            prms.Add("@EndPostedDate", DateTime.Parse(DateEnd.Value.ToString()).AddDays(1).ToShortDateString());
        }

        if (PrimaryContactUID.SelectedIndex > 0)
            prms.Add("@PrimaryContactUID", PrimaryContactUID.SelectedValue);

        string AgentCatUID = string.Empty;

        for (int i = 0; i < AgentCategoryUID.Items.Count; i++)
        {
            if (AgentCategoryUID.Items[i].Selected)

                AgentCatUID += AgentCategoryUID.Items[i].Value + ",";
        }

        if (AgentCatUID != string.Empty)
            prms.Add("@AgentCategoryUID", AgentCatUID.TrimEnd(','));


        if (!string.IsNullOrWhiteSpace(PartnerChannel.SelectedValue))
            prms.Add("@AgentGroupID", PartnerChannel.SelectedValue);


        // add one day to end date


        // sets the flag for portfolio cap proc
        //this._isValidMonthIntervals = this.isFullMonth((DateTime.Parse(prms["@DateStart"].ToString())), (DateTime.Parse(prms["@DateEnd"].ToString())));

        return prms;
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        Search(false);
    }

    //void wucAgentSelector_GridRowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (wucAgentSelector.m_AgentUID != string.Empty)
    //    {
    //        chkSubAgent.Enabled = true;
    //    }
    //    else
    //    {
    //        chkSubAgent.Enabled = false;
    //        chkSubAgent.Checked = false;
    //    }
    //}

    private void SetPartnerChannelAccess()
    {
        if (UserSessions.CurrentUser.IsAgent)
        {
            DataAgent data = new DataAgent();
            Agent app = data.GetAgent(UserSessions.CurrentUser.AgentUID);

            if (app != null)
            {
                ListHandler.ListFindItem(PartnerChannel, app.AgentGroupID.ToString());
            }

            PartnerChannel.Enabled = false;
        }

        if (UserSessions.CurrentUser.IsInternal)
        {
            List<int> partnerChannel = DataUser.GetInstance().GetUserPartnerChannelAccess(UserSessions.CurrentUser.UID);

            List<ListItem> remove = new List<ListItem>();

            foreach (ListItem item in this.PartnerChannel.Items)
            {
                //
                bool found = false;

                foreach (int pChannelId in partnerChannel)
                {

                    if (item.Value == pChannelId.ToString())
                    {
                        found = true;
                        break;
                    }
                }

                //don't remove the "All" which has a value of an empty 
                //string from the drop down list item
                if (!found && !item.Value.Equals(""))
                {
                    remove.Add(item);
                }
            }

            //remove the "All" drop down item if we're removing any items
            if (remove.Count > 0)
            {
                this.PartnerChannel.Items.RemoveAt(0);
            }

            foreach (ListItem del in remove)
            {
                this.PartnerChannel.Items.Remove(del);
            }
        }
    }
}
