using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects.Reserve;
using OfficeOpenXml;
using System.Reflection;

namespace ZeusWeb.UserControls
{
    public partial class wucReserveGrid : wucBaseSearch
    {


        // Reserve Entry
        public delegate void EventClickReportDate(int reserveid, int zid);
        public event EventClickReportDate event_click_reportdate;

        // Manual Entry
        public delegate void EventClickManualEntry(int reserveid, int zid);
        public event EventClickManualEntry event_click_manualentry;

        public delegate void EventClickAmount(int zid, string reportdate);
        public event EventClickAmount event_click_amount;

        public delegate void EventClickReservePercent(int zid, int ReserveID, string reportdate);
        public event EventClickReservePercent event_click_reservepercent;

        public delegate void EventClickReserveAmount(int zid, int ReserveID);
        public event EventClickReserveAmount event_click_reserveamount;

        public int GridRowCount
        {
            get { return (int)(ViewState["GridRowCount"] ?? 0); }
            set { ViewState["GridRowCount"] = value; }
        }

        #region sorting_stuff

        public SortDirection VsSortAmount
        {
            get
            {
                if (ViewState["VsSortAmount"] == null)
                {
                    ViewState["VsSortAmount"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsSortAmount"];
            }
            set { ViewState["VsSortAmount"] = value; }
        }

        public SortDirection VsSortBatchAmount
        {
            get
            {
                if (ViewState["VsSortBatchAmount"] == null)
                {
                    ViewState["VsSortBatchAmount"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsSortBatchAmount"];
            }
            set { ViewState["VsSortBatchAmount"] = value; }
        }

        public SortDirection VsBatchNetAmount
        {
            get
            {
                if (ViewState["VsBatchNetAmount"] == null)
                {
                    ViewState["VsBatchNetAmount"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsBatchNetAmount"];
            }
            set { ViewState["VsBatchNetAmount"] = value; }
        }

        public SortDirection VsCalcReserve
        {
            get
            {
                if (ViewState["VsCalcReserve"] == null)
                {
                    ViewState["VsCalcReserve"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsCalcReserve"];
            }
            set { ViewState["VsCalcReserve"] = value; }
        }


        public SortDirection VsCalcReservePct
        {
            get
            {
                if (ViewState["VsCalcReservePct"] == null)
                {
                    ViewState["VsCalcReservePct"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsCalcReservePct"];
            }
            set { ViewState["VsCalcReservePct"] = value; }
        }
        public SortDirection VsReservepct
        {
            get
            {
                if (ViewState["VsReservepct"] == null)
                {
                    ViewState["VsReservepct"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReservepct"];
            }
            set { ViewState["VsReservepct"] = value; }
        }
        public SortDirection VsReserve
        {
            get
            {
                if (ViewState["VsReserve"] == null)
                {
                    ViewState["VsReserve"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReserve"];
            }
            set { ViewState["VsReserve"] = value; }
        }
        public SortDirection VsDivert
        {
            get
            {
                if (ViewState["VsDivert"] == null)
                {
                    ViewState["VsDivert"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsDivert"];
            }
            set { ViewState["VsDivert"] = value; }
        }

        public SortDirection VsReserveSource
        {
            get
            {
                if (ViewState["VsReserveSource"] == null)
                {
                    ViewState["VsReserveSource"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReserveSource"];
            }
            set { ViewState["VsReserveSource"] = value; }
        }

        public SortDirection VsReportDate
        {
            get
            {
                if (ViewState["VsReportDate"] == null)
                {
                    ViewState["VsReportDate"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReportDate"];
            }
            set { ViewState["VsReportDate"] = value; }
        }

        #endregion

        private bool _ViewPendingRecords = true;

        public bool ViewPendingRecords
        {
            get { return _ViewPendingRecords; }
            set { _ViewPendingRecords = value; }
        }


        private bool _IsQueue = false;

        public bool IsQueue
        {
            get { return _IsQueue; }
            set { _IsQueue = value; }
        }

        private bool _ShowMerchantColumns = false;

        public bool ShowMerchantColumns
        {
            get { return _ShowMerchantColumns; }
            set { _ShowMerchantColumns = value; }
        }

        private bool _ShowCheckbox = false;

        public bool ShowCheckbox
        {
            get { return _ShowCheckbox; }
            set { _ShowCheckbox = value; }
        }

        public const int COLUMN_CHECKBOX = 0;
        public const int COLUMN_ZID = 2;
        public const int COLUMN_MID = 3;
        public const int COLUMN_DBA = 4;
        public const int COLUMN_SOURCE = 13;

        public int ZID
        {
            get
            {
                if (ViewState["ZID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["ZID"];
                }
            }
            set { ViewState["ZID"] = value; }
        }

        public string QAReportDate
        {
            get
            {
                if (ViewState["QAReportDate"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ViewState["QAReportDate"];
                }
            }
            set { ViewState["QAReportDate"] = value; }
        }

        public GridView ReserveGrid
        {
            get
            {
                return this.grdReserve;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExpExcel);

            if (!this.IsPostBack)
            {
                LoadSD077();
            }
        }

        protected void LoadSD077()
        {
            Hashtable prms = new Hashtable();

            if (this.ZID == 0)
            {

                if (this.ViewPendingRecords)
                {
                    prms.Add("@ViewPendingRecords", 1);
                }

                if (!string.IsNullOrEmpty(this.QAReportDate))
                {
                    prms.Add("@QAReportDate", this.QAReportDate);
                }
            }
            else
            {
                prms.Add("@ZID", this.ZID);
            }

            //if no parameters
            if (prms.Count == 0)
            {
                prms.Add("@ZID", -1);
            }


            if (this.IncludeReservesHeldAtMeritus)
            {
                prms.Add("@IncludeReservesHeldAtMeritus", this.IncludeReservesHeldAtMeritus);
            }


            this.m_Prms = prms;

            List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

            if (li != null)
            {
                this.GridRowCount = li.Count;
            }

            grdReserve.DataSource = li;
            grdReserve.DataBind();
        }

        public void BindGrid()
        {

            this.LoadSD077();
        }

        protected void grdReserve_PreRender(object sender, EventArgs e)
        {
            grdReserve.Columns[COLUMN_CHECKBOX].Visible = this.ShowCheckbox;

            grdReserve.Columns[COLUMN_ZID].Visible = this.ShowMerchantColumns;
            grdReserve.Columns[COLUMN_MID].Visible = this.ShowMerchantColumns;
            grdReserve.Columns[COLUMN_DBA].Visible = this.ShowMerchantColumns;

            grdReserve.Columns[COLUMN_SOURCE].Visible = !this.IsQueue;
        }

        protected void grdReserve_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    RDBReserve obj = (RDBReserve)e.Row.DataItem;

                    Label lbReportDate = (Label)e.Row.FindControl("lblReportDate");
                    lbReportDate.Visible = true;

                    //LinkButton lnReportDate = (LinkButton)e.Row.FindControl("lnkReportDate");

                    //lnReportDate.Visible = this.IsQueue;
                    //lbReportDate.Visible = !this.IsQueue;

                    //if (!this.ViewPendingRecords)
                    //{
                    //    // show history
                    //    lnReportDate.Visible = false;
                    //    lbReportDate.Visible = true;
                    //}



                    decimal d = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "Reservepct"), 0);
                    decimal d2 = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "CalcReservepct"), 0);

                    bool has_diversions = obj.DiversionIDs.Trim() == "" ? false : true;

                    Label lblRP = (Label)e.Row.FindControl("lblRP");
                    lblRP.Text = String.Format("{0:P2}", d);

                    LinkButton linkRPerc = (LinkButton)e.Row.FindControl("linkRP");
                    linkRPerc.Text = String.Format("{0:P2}", d);

                    // the ReservePercent linkbutton should only appear if it's a queue. and it's a pending record.

                    if (this.ViewPendingRecords && this.IsQueue && has_diversions)
                    {
                        linkRPerc.Visible = true;
                        lblRP.Visible = false;
                    }
                    else
                    {
                        linkRPerc.Visible = false;
                        lblRP.Visible = true;
                    }



                    Label lblRPC = (Label)e.Row.FindControl("lblRPC");
                    lblRPC.Text = String.Format("{0:P2}", d2);




                    LinkButton rbReportDate = (LinkButton)e.Row.FindControl("lnkReportDate");

                    if (obj.ReserveSourceID == PaymentXP.BusinessObjects.eRDBReserveSourceID.FundsWithheldDepositHoldbackPercentage) //Batch Holdback Pct
                    {
                        rbReportDate.CommandName = "ReserveEntry";
                    }
                    else if (obj.ReserveSourceID == PaymentXP.BusinessObjects.eRDBReserveSourceID.ManualReserve) // Manual Reserve
                    {
                        rbReportDate.CommandName = "ManualEntry";
                    }

                    //Label lbDIDS = (Label)e.Row.FindControl("lblDiversions");


                    //string[] arrdiver = obj.DiversionIDs.Trim().Split(new char[] { ',' });

                    //if (arrdiver != null && arrdiver.Length > 0)
                    //{

                    //    foreach (string item in arrdiver)
                    //    {
                    //        string[] arr = item.Trim().Split(new char[] { ':' });

                    //        if (arr.Length == 3)
                    //        {
                    //            string reserve_id = arr[0].Trim();
                    //            decimal percent = Convert.ToDecimal(arr[1].Trim());
                    //            string daterange = arr[2].Trim();

                    //            lbDIDS.Text += string.Format("{0:P2}, {1}<br />", percent, daterange);
                    //        }
                    //    }


                    //}



                    LinkButton lnkResAmt = (LinkButton)e.Row.FindControl("lnkReserveAmount");
                    Label labResAmt = (Label)e.Row.FindControl("lblReserveAmount");

                    if (this.IsQueue && this.ViewPendingRecords)
                    {
                        lnkResAmt.Visible = true;
                        labResAmt.Visible = false;
                    }
                    else
                    {
                        lnkResAmt.Visible = false;
                        labResAmt.Visible = true;
                    }


                    // only show checkbox if it's the first earliest item for that zid.
                    CheckBox cb = (CheckBox)e.Row.FindControl("cbSelect");
                    cb.Enabled = (obj.Rnk == 1) ? true : false;

                    break;

                case DataControlRowType.Footer:

                    break;

                default:
                    break;
            }
        }

        protected void grdReserve_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandSource is LinkButton)
            {

                LinkButton lb = (LinkButton)e.CommandSource;

                switch (lb.CommandName)
                {
                    case "ReserveEntry":
                        {

                            string[] arr = lb.CommandArgument.Split(new char[] { '_' });
                            int ReserveID = CommonUtility.Util.if_i(arr[0], 0);
                            int row_zid = CommonUtility.Util.if_i(arr[1], 0);

                            if (event_click_reportdate != null)
                            {
                                event_click_reportdate(ReserveID, row_zid);
                            }
                        }

                        break;

                    case "ManualEntry":
                        {

                            string[] arr = lb.CommandArgument.Split(new char[] { '_' });
                            int ReserveID = CommonUtility.Util.if_i(arr[0], 0);
                            int row_zid = CommonUtility.Util.if_i(arr[1], 0);

                            if (event_click_manualentry != null)
                            {
                                event_click_manualentry(ReserveID, row_zid);
                            }
                        }

                        break;

                    case "Amount":
                        {
                            string[] arr = lb.CommandArgument.Split(new char[] { '_' });

                            int row_zid = CommonUtility.Util.if_i(arr[0], 0);
                            string row_reportdate = CommonUtility.Util.if_s(arr[1]);

                            if (event_click_amount != null)
                            {
                                event_click_amount(row_zid, row_reportdate);
                            }
                        }

                        break;

                    case "ReservePercent":
                        {

                            string[] arr = lb.CommandArgument.Split(new char[] { '_' });

                            int row_zid = CommonUtility.Util.if_i(arr[0], 0);
                            int ReserveID = CommonUtility.Util.if_i(arr[1], 0);
                            string row_reportdate = CommonUtility.Util.if_s(arr[2]);

                            if (event_click_reservepercent != null)
                            {
                                event_click_reservepercent(row_zid, ReserveID, row_reportdate);
                            }
                        }
                        break;

                    case "ReserveAmount":
                        {

                            string[] arr = lb.CommandArgument.Split(new char[] { '_' });

                            int row_zid = CommonUtility.Util.if_i(arr[0], 0);
                            int ReserveID = CommonUtility.Util.if_i(arr[1], 0);

                            if (event_click_reserveamount != null)
                            {
                                event_click_reserveamount(row_zid, ReserveID);
                            }
                        }
                        break;
                }

            }

        }

        public List<string> GetChecked()
        {


            var checkedIDs = from GridViewRow msgRow in grdReserve.Rows
                             where ((CheckBox)msgRow.FindControl("cbSelect")).Checked
                             select Convert.ToString(grdReserve.DataKeys[msgRow.RowIndex][0]) + "_" + Convert.ToString(grdReserve.DataKeys[msgRow.RowIndex][1]);

            List<string> liDivertID = checkedIDs.ToList();

            return liDivertID;
        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reserve");

                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                string[,] liHeaders = null;

                if (this.IsQueue)
                {
                    // display what you see in the queue. (many merchants)

                    liHeaders = new string[14, 3] {
			            {"ReportDate", "Report Date", "string"},	
                        {"ZID", "ZID", "integer"},	
                        {"SettlePlatformMID", "MID", "string"},	
                        {"BusinessDBAName", "DBA Name", "string"},	
                        {"BatchAmount", "Sale Amount", "currency"},	
                        {"BatchNetAmount", "Net Amount", "currency"},	
                        {"Amount", "Amt Withheld", "currency"},	
                        {"CalcReserve", "Amt Withheld (Expected)", "currency"},	
                        {"CalcReservePct", "Reserve % (Eff) (Gross Sales)", "percent"},	
                        {"ReservePct", "Reserve %", "percent"},	
                        {"Reserve", "Reserve", "currency"},	
                        {"Divert", "Divert", "currency"},	
                        {"Diversions", "Diversions", "string"},	
                        {"Bank", "Bank", "string"},	
                    };
                }
                else
                {
                    // display for a specific merchant
                    liHeaders = new string[12, 3] {
			            {"ReportDate", "Report Date", "string"},	
                        {"BatchAmount", "Sale Amount", "currency"},	
                        {"BatchNetAmount", "Net Amount", "currency"},	
                        {"Amount", "Amt Withheld", "currency"},	
                        {"CalcReserve", "Amt Withheld (Expected)", "currency"},	
                        {"CalcReservePct", "Reserve % (Eff) (Gross Sales)", "percent"},	
                        {"ReservePct", "Reserve %", "percent"},	
                        {"Reserve", "Reserve", "currency"},	
                        {"Divert", "Divert", "currency"},	
                        {"ReserveSource", "Source", "string"},	
                        {"Diversions", "Diversions", "string"},	
                        {"Bank", "Bank", "string"},	
                    };
                }

                DataTable dt = GridViewExportUtil.GetExportableDataTable<RDBReserve>(li, liHeaders);

                GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

                ws.Cells.LoadFromDataTable(dt, true);

                string filename = "";
                if (this.ZID > 0)
                {
                    filename = String.Format("RDBReserve_ZID-{0}_{1}.xlsx", CommonUtility.Util.GetDateTimeStamp(), this.ZID.ToString());
                }
                else
                {
                    filename = String.Format("ReserveSummary_{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());
                }


                //Write it back to the client
                Response.Clear();   // necessary!!!
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!



            }
        }




        protected void grdReserve_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == "ReportDate")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsReportDate == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.ReportDate);
                    grdReserve.DataBind();
                    this.VsReportDate = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.ReportDate);
                    grdReserve.DataBind();
                    this.VsReportDate = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Amount")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsSortAmount == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.Amount);
                    grdReserve.DataBind();
                    this.VsSortAmount = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.Amount);
                    grdReserve.DataBind();
                    this.VsSortAmount = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "BatchAmount")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsSortBatchAmount == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.BatchAmount);
                    grdReserve.DataBind();
                    this.VsSortBatchAmount = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.BatchAmount);
                    grdReserve.DataBind();
                    this.VsSortBatchAmount = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "BatchNetAmount")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsBatchNetAmount == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.BatchNetAmount);
                    grdReserve.DataBind();
                    this.VsBatchNetAmount = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.BatchNetAmount);
                    grdReserve.DataBind();
                    this.VsBatchNetAmount = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "CalcReserve")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsCalcReserve == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.CalcReserve);
                    grdReserve.DataBind();
                    this.VsCalcReserve = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.CalcReserve);
                    grdReserve.DataBind();
                    this.VsCalcReserve = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "CalcReservePct")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsCalcReservePct == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.CalcReservePct);
                    grdReserve.DataBind();
                    this.VsCalcReservePct = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.CalcReservePct);
                    grdReserve.DataBind();
                    this.VsCalcReservePct = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Reservepct")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsReservepct == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.ReservePct);
                    grdReserve.DataBind();
                    this.VsReservepct = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.ReservePct);
                    grdReserve.DataBind();
                    this.VsReservepct = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Reserve")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsReserve == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.Reserve);
                    grdReserve.DataBind();
                    this.VsReserve = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.Reserve);
                    grdReserve.DataBind();
                    this.VsReserve = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Divert")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsDivert == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.Divert);
                    grdReserve.DataBind();
                    this.VsDivert = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.Divert);
                    grdReserve.DataBind();
                    this.VsDivert = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "ReserveSource")
            {
                List<RDBReserve> li = DataReserve.GetRDBReserve(this.m_Prms);

                if (this.VsReserveSource == SortDirection.Ascending)
                {
                    grdReserve.DataSource = li.OrderByDescending(x => x.ReserveSource);
                    grdReserve.DataBind();
                    this.VsReserveSource = SortDirection.Descending;
                }
                else
                {
                    grdReserve.DataSource = li.OrderBy(x => x.ReserveSource);
                    grdReserve.DataBind();
                    this.VsReserveSource = SortDirection.Ascending;
                }
            }


        }

        protected DataTable ConvertToDataTable(GridView gv, DataTable dt)
        {
            DataTable TempTable = new DataTable();
            TempTable = dt.Clone();

            foreach (GridViewRow row in gv.Rows)
            {
                DataRow TempRow = TempTable.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    if (row.Cells[i].Controls[0].GetType().Equals(typeof(DataBoundLiteralControl)))
                    {
                        TempRow[i] = ((DataBoundLiteralControl)row.Cells[i].Controls[0] as DataBoundLiteralControl).Text;
                    }
                    else if (row.Cells[i].Controls[0].GetType().Equals(typeof(TextBox)))
                    {
                        TempRow[i] = ((TextBox)row.Cells[i].Controls[0]).Text;
                    }
                }
                TempTable.Rows.Add(TempRow);
            }
            return TempTable;
        }


    }
}
