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

using Infragistics.WebUI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using Microsoft.Reporting.WebForms;

public partial class frmRMReportsByPortfolio : frmBaseSearch
{
    private decimal m_volcancellations = 0;
    private decimal m_vol = 0;


    public string SelectedARM
    {
        get
        {
            if (ViewState["SelectedARM"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SelectedARM"].ToString();
            }
        }
        set { ViewState["SelectedARM"] = value; }

    }

    public string SelectedARMUID
    {
        get
        {
            if (ViewState["SelectedARMUID"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SelectedARMUID"].ToString();
            }
        }
        set { ViewState["SelectedARMUID"] = value; }

    }

    public string SelectedQueue
    {
        get
        {
            if (ViewState["SelectedQueue"] == null)
            {
                return "";
            }
            else
            {
                return ViewState["SelectedQueue"].ToString();
            }
        }
        set { ViewState["SelectedQueue"] = value; }

    }

    public string grid
    {
        get
        {
            if (ViewState["grid"] == null)
                return string.Empty;
            else
                return ViewState["grid"].ToString();
        }
        set { ViewState["grid"] = value; }
    }

    public string grdSortOrder
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GSortOrder"] == null)
                return "AgentFullName";
            else
                return ViewState[this.Page.ToString() + "_GSortOrder"].ToString();
        }
        set { ViewState[this.Page.ToString() + "_GSortOrder"] = value; }
    }

    public SortDirection grdSortDirectionSearch
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GSortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)ViewState[this.Page.ToString() + "_GSortDirectionSearch"];
        }
        set { ViewState[this.Page.ToString() + "_GSortDirectionSearch"] = value; }
    }

    public int grdCurrentPage
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GCurrentPage"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_GCurrentPage"]);
        }
        set { ViewState[this.Page.ToString() + "_GCurrentPage"] = value; }
    }

    public int grdPageSize
    {
        get
        {
            if (ViewState[this.Page.ToString() + "_GPageSize"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState[this.Page.ToString() + "_GPageSize"]);
        }
        set { ViewState[this.Page.ToString() + "_GPageSize"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {

            //Set the current page
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > RM Reports By Portfolio";

            SearchBeginDate.Text = DateTime.Today.ToString("MM/01/yyyy");
            SearchEndDate.Text = DateTime.Today.ToString("MM/dd/yyyy");


            this.grid = string.Empty;
            this.Search(true);
        }
    }

    public override void Search(bool IsOnLoad)
    {
        //Save search fields in session variable
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        DataSet ds = null;
        DataView dv = null;

        //Credit card Count
        Hashtable prms = new Hashtable();

        switch (UserSessions.CurrentUser.AccessLevelUID.ToUpper())
        {
            case "7B824322-B5A6-4ABF-8810-A29FF271D8B6": //local access
                prms.Add("@useruid", UserSessions.CurrentUser.UID);
                this.ShowApprovals(UserSessions.CurrentUser.UID);
                break;
        }




        ds = DataAccess.DataAgentDao.GetRMReportsByPortfolioSummary(prms);
        dv = ds.Tables[0].DefaultView;

        grd.DataSource = dv;
        grd.DataBind();

        pnlSummary.Visible = dv.Table.Rows.Count > 0;
        lblNoDataSummary.Visible = dv.Table.Rows.Count == 0;
        lblRecordCount.Text = "Total Records Found: " + dv.Table.Rows.Count.ToString();

        if (grd.Rows.Count == 1)
        {
            grd.SelectedIndex = 0;
            this.SelectedARMUID = grd.DataKeys[grd.SelectedIndex].Values["UserUID"].ToString();
            this.SelectedARM = grd.SelectedRow.Cells[1].Text;
        }

    }

    private void FormClear()
    {
        this.SearchParameters = null;
        //UserSessions.SearchResultsDataView = null;
        //UserSessions.SearchResultsDataView2 = null;
        FormHandler.ClearAllControls(this);
    }

    private bool FormDelete()
    {
        return false;
    }

    private void FormShow()
    {
    }

    protected void btnAddMerchant_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "frmLeadsDetail.aspx?Adding=true";
        Response.Redirect(url);
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
        this.SortOrder = this.grdSortOrder = "AgentFullName";
        this.SearchParameters = null;
        this.grid = string.Empty;
        this.Search(false);
    }

    protected void btnAdd_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        string url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=true";
        url += "&PostBackURL=~/SecureMerchantManagementForms/frmMerchantSearch.aspx";
        Response.Redirect(url);
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.Search(false);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        //grdCredit.AllowPaging = false;
        //Search(false);
        //grdCredit.AllowPaging = true;
        //FormHandler.Export2Excel("RM Reports by Portfolio", grdCredit);
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton lnkUserName = (LinkButton)e.Row.FindControl("lnkUserName");
                lnkUserName.Text = DataBinder.Eval(e.Row.DataItem, "UserName").ToString();
                lnkUserName.CommandArgument = "Details|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString() + "|";

                LinkButton lnkAP = (LinkButton)e.Row.FindControl("lnkAP");
                lnkAP.Text = DataBinder.Eval(e.Row.DataItem, "RM").ToString();
                lnkAP.CommandArgument = "Details|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString() + "|SS";

                LinkButton lnkCU = (LinkButton)e.Row.FindControl("lnkCU");
                lnkCU.Text = DataBinder.Eval(e.Row.DataItem, "CU").ToString();
                lnkCU.CommandArgument = "Details|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString() + "|CU";

                LinkButton lnkAB = (LinkButton)e.Row.FindControl("lnkAB");
                lnkAB.Text = DataBinder.Eval(e.Row.DataItem, "AB").ToString();
                lnkAB.CommandArgument = "Details|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString() + "|OP";

                LinkButton lnkDP = (LinkButton)e.Row.FindControl("lnkDP");
                lnkDP.Text = DataBinder.Eval(e.Row.DataItem, "DP").ToString();
                lnkDP.CommandArgument = "Details|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString() + "|DP";

                LinkButton lnkTickets = (LinkButton)e.Row.FindControl("lnkTickets");
                lnkTickets.Text = DataBinder.Eval(e.Row.DataItem, "Tickets").ToString();
                lnkTickets.CommandArgument = "Tickets|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString();

                LinkButton lnkTopSP = (LinkButton)e.Row.FindControl("lnkTopSP");
                lnkTopSP.Text = DataBinder.Eval(e.Row.DataItem, "TopSP").ToString();
                lnkTopSP.CommandArgument = "TopSP|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString();

                LinkButton lnkCancelled = (LinkButton)e.Row.FindControl("lnkCancelled");
                lnkCancelled.Text = DataBinder.Eval(e.Row.DataItem, "Cancelled").ToString();
                lnkCancelled.CommandArgument = "Cancelled|" + DataBinder.Eval(e.Row.DataItem, "UserUID").ToString();



                break;

            case DataControlRowType.Footer:

                break;

            default:
                break;
        }
    }

    protected void grdDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lnkZID");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "ZID").ToString();
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();

                string statusuid = DataBinder.Eval(e.Row.DataItem, "StatusUID").ToString().ToUpper();

                DateTime currenttime = DateTime.Now;
                DateTime currentday = DateTime.Today;

                switch (statusuid)
                {
                    case "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409": //earlier the status was AP - Received  and now RM - Received
                        //case "87F2DFAE-B0EC-4208-83FC-C9488393AA61": //CU - Received
                        DateTime APreceiveddate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "apreceiveddate"));
                        DateTime ap_sla_date = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy 12:00:00"));


                        if ((APreceiveddate < ap_sla_date && APreceiveddate < currentday)
                            || (APreceiveddate < ap_sla_date && APreceiveddate >= currentday && currenttime >= ap_sla_date))
                            e.Row.BackColor = System.Drawing.Color.Tomato;

                        break;

                    case "87F2DFAE-B0EC-4208-83FC-C9488393AA61": //CU - Received

                        DateTime CUreceiveddate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "cureceiveddate"));
                        DateTime cu_sla_date = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy 12:00:00"));


                        if ((CUreceiveddate < cu_sla_date && CUreceiveddate < currentday)
                            || (CUreceiveddate < cu_sla_date && CUreceiveddate >= currentday && currenttime >= cu_sla_date))
                            e.Row.BackColor = System.Drawing.Color.Tomato;

                        break;

                    case "73FC4B27-98D4-40EA-B9FC-1370C564CB12": //AB - Received

                        DateTime ABreceiveddate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "abreceiveddate"));
                        DateTime ab_sla_date = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy 13:00:00"));

                        if ((ABreceiveddate < ab_sla_date && ABreceiveddate < currentday)
                            || (ABreceiveddate < ab_sla_date && ABreceiveddate >= currentday && currenttime >= ab_sla_date))
                            e.Row.BackColor = System.Drawing.Color.Tomato;

                        break;

                    case "158F32CA-4447-48CE-9CD4-ED45514E24D8": //DP - Received - ST
                    case "158F32CA-4447-48CE-9CD4-ED45514E24D9": //DP - Received - GS

                        DateTime DPreceiveddate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "dpreceiveddate"));
                        DateTime dp_sla_date = Convert.ToDateTime(DateTime.Today.ToString("MM/dd/yyyy 18:00:00"));

                        if ((DPreceiveddate < dp_sla_date && DPreceiveddate < currentday)
                            || (DPreceiveddate < dp_sla_date && DPreceiveddate >= currentday && currenttime >= dp_sla_date))
                            e.Row.BackColor = System.Drawing.Color.Tomato;

                        break;
                }


                LinkButton btn3 = (LinkButton)e.Row.FindControl("lnkDBA");
                btn3.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();

                Label lbl3 = (Label)e.Row.FindControl("lblDBA");

                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasConditions")))
                {
                    btn3.Text = DataBinder.Eval(e.Row.DataItem, "DBA").ToString();
                    lbl3.Text = string.Empty;
                }


                btn3.Visible = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasConditions")) == true;
                lbl3.Visible = !Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasConditions")) == true;

                m_vol += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Approved Vol"));
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "Page Total:";
                e.Row.Cells[11].Text = this.m_vol.ToString("0.00");/*"c")*/
                e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                break;

            default:
                break;
        }
    }

    protected void grdCancellations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                LinkButton btn = (LinkButton)e.Row.FindControl("lnkZID");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "ZID").ToString();
                btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "MerchantAppUID").ToString();

                m_volcancellations += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Approved Vol"));
                break;

            case DataControlRowType.Footer:
                e.Row.Cells[0].Text = "Page Total:";
                e.Row.Cells[10].Text = this.m_volcancellations.ToString("0.00");/*"c")*/
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                break;

            default:
                break;
        }
    }

    protected void grdTickets_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:


                //LinkButton lnkTicketID = (LinkButton)e.Row.FindControl("lnkTicketID");
                //lnkTicketID.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();
                //lnkTicketID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();


                LinkButton btn = (LinkButton)e.Row.FindControl("lnkTicketID");
                btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();

                btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");


                break;

            case DataControlRowType.Footer:

                break;

            default:
                break;
        }
    }

    protected void lnkSummary_Click(object sender, EventArgs e)
    {

    }

    private void LoadDetails()
    {
        if (string.IsNullOrEmpty(this.SelectedARMUID) || string.IsNullOrEmpty(this.SelectedQueue))
            return;


        switch (this.SelectedQueue)
        {
            case "OP":
                grdDetails.Columns[5].Visible = false; //ap-received
                grdDetails.Columns[6].Visible = false; //cu-received
                grdDetails.Columns[7].Visible = true; //ab-received
                grdDetails.Columns[8].Visible = false; //dp-received
                break;

            case "DP":
                grdDetails.Columns[5].Visible = false; //ap-received
                grdDetails.Columns[6].Visible = false; //cu-received
                grdDetails.Columns[7].Visible = false; //ab-received
                grdDetails.Columns[8].Visible = true; //dp-received
                break;

            case "CU":
                grdDetails.Columns[5].Visible = false; //ap-received
                grdDetails.Columns[6].Visible = true; //cu-received
                grdDetails.Columns[7].Visible = false; //ab-received
                grdDetails.Columns[8].Visible = false; //dp-received
                break;

            default:
                grdDetails.Columns[5].Visible = true; //ap-received
                grdDetails.Columns[6].Visible = false; //cu-received
                grdDetails.Columns[7].Visible = false; //ab-received
                grdDetails.Columns[8].Visible = false; //dp-received
                break;
        }

        DataSet ds = null;
        DataView dv = null;
        Hashtable prms = new Hashtable();



        prms.Add("@UserUID", this.SelectedARMUID);
        prms.Add("@queue", this.SelectedQueue);

        ds = DataAccess.DataAgentDao.GetRMReportsByPortfolioDetails(prms);
        dv = ds.Tables[0].DefaultView;
        grdDetails.DataSource = dv;
        grdDetails.DataBind();

        pnlDetails.Visible = dv.Table.Rows.Count > 0;
        lblNoDataDetails.Visible = dv.Table.Rows.Count == 0;
        lblRecordCountDetails.Text = "Total Records Found: " + dv.Table.Rows.Count.ToString();

        this.tabReport.SelectedIndex = 1;
    }

    private void LoadTickets()
    {
        if (string.IsNullOrEmpty(this.SelectedARMUID))
            return;

        //string[] str = this.SearchParameters.ToString().Split(new char[] { '|' });

        Hashtable prms = new Hashtable();
        prms.Add("@UserUID", this.SelectedARMUID);
        DataSet ds = DataAccess.DataAgentDao.GetRMReportsByPortfolioTickets(prms);

        grdAssigned.DataSource = ds.Tables[0].DefaultView;
        grdAssigned.DataBind();

        pnlAssigned.Visible = ds.Tables[0].Rows.Count > 0;
        lblNoDataAssigned.Visible = ds.Tables[0].Rows.Count == 0;
        lblRecordCountAssigned.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();



        grdAssignedOut.DataSource = ds.Tables[1].DefaultView;
        grdAssignedOut.DataBind();

        pnlAssignedOut.Visible = ds.Tables[1].Rows.Count > 0;
        lblNoDataAssignedOut.Visible = ds.Tables[1].Rows.Count == 0;
        lblRecordCountAssignedOut.Text = "Total Records Found: " + ds.Tables[1].Rows.Count.ToString();





        //lblRecordCountDetails.Text = "Total Records Found: " + dv.Table.Rows.Count.ToString();

        this.tabReport.SelectedIndex = 2;
    }

    private void LoadCancellations()
    {
        if (string.IsNullOrEmpty(this.SelectedARMUID))
            return;

        DataSet ds = null;
        DataView dv = null;
        Hashtable prms = new Hashtable();


        //string[] str = this.SearchParameters.ToString().Split(new char[] { '|' });

        prms.Add("@UserUID", this.SelectedARMUID);
        prms.Add("@BeginPostedDate", SearchBeginDate.Text);
        prms.Add("@EndPostedDate", SearchEndDate.Text);


        ds = DataAccess.DataAgentDao.GetRMReportsByPortfolioCancellations(prms);
        dv = ds.Tables[0].DefaultView;
        grdCancellations.DataSource = dv;
        grdCancellations.DataBind();

        pnlCancelled.Visible = ds.Tables[0].Rows.Count > 0;
        lblNoDataCancelled.Visible = ds.Tables[0].Rows.Count == 0;
        lblRecordCountCancellations.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();



        this.tabReport.SelectedIndex = 4;
    }

    private void LoadTopSPs()
    {

        if (string.IsNullOrEmpty(this.SelectedARMUID))
            return;


        Hashtable prms = new Hashtable();
        prms.Add("@UserUID", this.SelectedARMUID);
        prms.Add("@BeginPostedDate", SearchBeginDate.Text);
        prms.Add("@EndPostedDate", SearchEndDate.Text);


        if (lstSPOptions.SelectedIndex == 0)
            prms.Add("@viewAll", 0);
        else
            prms.Add("@viewAll", 1);

        DataSet ds = DataAccess.DataAgentDao.GetRMReportsByPortfolioTopSPs(prms);

        grdTopSPPct.DataSource = ds.Tables[0].DefaultView;
        grdTopSPPct.DataBind();

        pnlSPApprovals.Visible = ds.Tables[0].Rows.Count > 0;
        lblNoDataSPApprovals.Visible = ds.Tables[0].Rows.Count == 0;
        lblRecordCountTopApproval.Text = "Total Records Found: " + ds.Tables[0].Rows.Count.ToString();



        grdTopSPVol.DataSource = ds.Tables[1].DefaultView;
        grdTopSPVol.DataBind();

        pnlSPVolume.Visible = ds.Tables[1].Rows.Count > 0;
        lblNoDataSPVolume.Visible = ds.Tables[1].Rows.Count == 0;
        lblRecordCountTopVolume.Text = "Total Records Found: " + ds.Tables[1].Rows.Count.ToString();




        this.tabReport.SelectedIndex = 3;
    }

    private void ShowApprovals(string ARMUID)
    {

        if (this.SelectedARMUID.ToUpper() != ARMUID.ToUpper())
        {


            Hashtable prms = new Hashtable();
            prms.Add("@UserUID", ARMUID);

            SqlDataReader dr = DataAccess.DataAgentDao.GetRMReportsByPortfolioApprovals(prms);

            if (dr.Read())
            {
                this.SelectedARM = dr["UserName"].ToString();
                lblArm.Text = this.SelectedARM;


                LastMonthCnt.Text = CommonUtility.Util.if_dec(dr["LastMonthCnt"],0.0M).ToString("###,##0");
                LastMonthVol.Text = CommonUtility.Util.if_dec(dr["LastMonthVol"], 0.0M).ToString("###,##0");
                LastMonthActualVol.Text = CommonUtility.Util.if_dec(dr["LastMonthActualVol"], 0.0M).ToString("###,##0");

                decimal approvedvoldelta = 0;
                decimal lastmonthvol = CommonUtility.Util.if_dec(dr["LastMonthVol"],0.0M);
                decimal priormonthvol = CommonUtility.Util.if_dec(dr["PriorMonthVol"], 0.0M);

                if (priormonthvol != 0)
                {
                    approvedvoldelta = (lastmonthvol / priormonthvol) - 1;
                }

                ApprovedVolDelta.Text = approvedvoldelta.ToString("#0.00%");
                if (ApprovedVolDelta.Text.Contains("-"))
                    ApprovedVolDelta.ForeColor = System.Drawing.Color.Red;
                else
                    ApprovedVolDelta.ForeColor = System.Drawing.Color.Green;


                decimal actualvoldelta = 0;
                decimal lastmonthactualvol = CommonUtility.Util.if_dec(dr["LastMonthActualVol"], 0.0M);
                decimal priormonthactualvol = CommonUtility.Util.if_dec(dr["PriorMonthActualVol"], 0.0M);

                if (priormonthactualvol != 0)
                {
                    actualvoldelta = (lastmonthactualvol / priormonthactualvol) - 1;

                }

                ActualVolDelta.Text = actualvoldelta.ToString("#0.00%");
                if (ActualVolDelta.Text.Contains("-"))
                    ActualVolDelta.ForeColor = System.Drawing.Color.Red;
                else
                    ActualVolDelta.ForeColor = System.Drawing.Color.Green;


            }
            dr.Close();

        }
    }

    protected void grdCancellations_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;



        //FormHandler.SetCurrentMerchant(lnk.CommandArgument);
        Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=False&MerchantAppUID=" + lnk.CommandArgument);

    }

    protected void grdDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;

        //FormHandler.SetCurrentMerchant(lnk.CommandArgument);

        switch (e.CommandName)
        {
            case "ZID":

                Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=False&MerchantAppUID=" + lnk.CommandArgument);

                break;

            case "DBAName":

                Response.Redirect("~/SecureMerchantManagementForms/frmUnderwritingPending.aspx?UID=" + lnk.CommandArgument + "&MerchantAppUID=" + lnk.CommandArgument);

                break;
        }

    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;

        if (e.CommandSource is LinkButton)
            lnk = (LinkButton)e.CommandSource;
        else
            return;

        this.SearchParameters = e.CommandArgument.ToString();

        string[] str = this.SearchParameters.ToString().Split(new char[] { '|' });

        if (str.Length > 2)
            this.SelectedQueue = str[2];

        this.ShowApprovals(str[1]);

        this.SelectedARMUID = str[1].ToUpper();

        grd.SelectedIndex = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;


        switch (str[0])
        {
            case "Details":
                LoadDetails();
                break;
            case "Tickets":
                LoadTickets();
                break;
            case "TopSP":
                LoadTopSPs();
                break;
            case "Cancelled":
                LoadCancellations();
                break;
        }

    }

    protected void lstSPOptions_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTopSPs();
    }

    protected void tabReport_SelectedIndexChanged(object sender, Infragistics.Web.UI.LayoutControls.TabSelectedIndexChangedEventArgs e)
    {
        switch (e.NewIndex)
        {
            case 1:
                LoadDetails();
                break;
            case 2:
                LoadTickets();
                break;
            case 3:
                LoadTopSPs();
                break;
            case 4:
                LoadCancellations();
                break;
        }
    }

    protected void btnApply2_Click(object sender, EventArgs e)
    {
        LoadTopSPs();
    }

    protected void btnApply_Click(object sender, EventArgs e)
    {
        LoadCancellations();
    }

    protected void lnkExportSummary_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Summary.xls", grd);

    }

    protected void lnkExportDetails_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Details.xls", grdDetails);

    }

    protected void lnkExportAssigned_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("TicketsAssigned.xls", grdAssigned);

    }

    protected void lnkExportAssignedOut_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("TicketsAssignedOut.xls", grdAssignedOut);

    }

    protected void lnkExportTopSPApprovals_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Top SPs Approvals.xls", grdTopSPPct);

    }

    protected void lnkExportTopSPVolume_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Top SPs Volume.xls", grdTopSPVol);

    }

    protected void lnkExportCancellations_Click(object sender, EventArgs e)
    {
        FormHandler.Export2Excel("Cancellations.xls", grdCancellations);

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        LoadTickets();
    }
}
