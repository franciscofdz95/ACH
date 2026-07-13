using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.BusinessObjects;
using OfficeOpenXml;
using System.Data;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucDivertDetailGrid : wucBaseSearch
    {
        public delegate void EventClickReportDate(int divertid, int zid);
        public event EventClickReportDate event_click_reportdate;


        #region sort_stuff

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
        public SortDirection VsBankName
        {
            get
            {
                if (ViewState["VsBankName"] == null)
                {
                    ViewState["VsBankName"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsBankName"];
            }
            set { ViewState["VsBankName"] = value; }
        }
        public SortDirection VsDivertCategory
        {
            get
            {
                if (ViewState["VsDivertCategory"] == null)
                {
                    ViewState["VsDivertCategory"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsDivertCategory"];
            }
            set { ViewState["VsDivertCategory"] = value; }
        }
        public SortDirection VsAmount
        {
            get
            {
                if (ViewState["VsAmount"] == null)
                {
                    ViewState["VsAmount"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsAmount"];
            }
            set { ViewState["VsAmount"] = value; }
        }
        public SortDirection VsBatchWithHeldAmount
        {
            get
            {
                if (ViewState["VsBatchWithHeldAmount"] == null)
                {
                    ViewState["VsBatchWithHeldAmount"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsBatchWithHeldAmount"];
            }
            set { ViewState["VsBatchWithHeldAmount"] = value; }
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
        public SortDirection VsDivertClear
        {
            get
            {
                if (ViewState["VsDivertClear"] == null)
                {
                    ViewState["VsDivertClear"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsDivertClear"];
            }
            set { ViewState["VsDivertClear"] = value; }
        }
        public SortDirection VsDivertReject
        {
            get
            {
                if (ViewState["VsDivertReject"] == null)
                {
                    ViewState["VsDivertReject"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsDivertReject"];
            }
            set { ViewState["VsDivertReject"] = value; }
        }
        public SortDirection VsPostMerchant
        {
            get
            {
                if (ViewState["VsPostMerchant"] == null)
                {
                    ViewState["VsPostMerchant"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsPostMerchant"];
            }
            set { ViewState["VsPostMerchant"] = value; }
        }
        #endregion

        //private bool _ViewPendingRecords = false;

        //public bool ViewPendingRecords
        //{
        //    get { return _ViewPendingRecords; }
        //    set { _ViewPendingRecords = value; }
        //}

        //private bool _ShowMerchantColumns = false;

        //public bool ShowMerchantColumns
        //{
        //    get { return _ShowMerchantColumns; }
        //    set { _ShowMerchantColumns = value; }
        //}

        //private bool _ShowCheckbox = false;

        //public bool ShowCheckbox
        //{
        //    get { return _ShowCheckbox; }
        //    set { _ShowCheckbox = value; }
        //}

        //public const int COLUMN_CHECKBOX = 0;
        //public const int COLUMN_ZID = 3;
        //public const int COLUMN_MID = 4;
        //public const int COLUMN_DBA = 5;

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

        public int DivertID
        {
            get
            {
                if (ViewState["DivertID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["DivertID"];
                }
            }
            set { ViewState["DivertID"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExpExcel);

            if (!this.IsPostBack)
            {
                LoadMD050();
            }
        }

        protected void LoadMD050()
        {
            Hashtable prms = new Hashtable();

            prms.Add("@ZID", this.ZID);

            this.m_Prms = prms;

            List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);


            if (li == null)
            {
                // adds columns if empty
                li = new List<RDBDivert>();
            }


            grdMD050.DataSource = li;
            grdMD050.DataBind();


        }

        protected void grdMD050_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:


                    RDBDivert obj = (RDBDivert)e.Row.DataItem;

                    if (obj.BankID == eRDBBank.Wells)
                    {
                        
                    }


                    // int journalid = CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "JournalID"), 0);

                    // format report date
                    //LinkButton lnkReportDate = (LinkButton)e.Row.FindControl("lnkReportDate");
                    //lnkReportDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "ReportDate")).ToString("MM/dd/yyy");

                    //// dynamically determine status
                    //Literal litS = (Literal)e.Row.FindControl("litStatus");
                    //litS.Text = (journalid > 0) ? "Confirmed" : "Pending";


                    //LinkButton lb = (LinkButton)e.Row.FindControl("lnkReportDate");

                    //if (journalid > 0)
                    //{
                    //    // has journal, so read only.
                    //    lb.Enabled = false;
                    //    lb.Style.Add("color", "black");
                    //    lb.Style.Add("text-decoration", "none");
                    //}
                    //else
                    //{
                    //    lb.Enabled = true;
                    //}

                    break;

                case DataControlRowType.Footer:

                    break;

                default:
                    break;
            }
        }


        protected void grdMD050_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandSource is LinkButton)
            //{
            //    LinkButton lb = (LinkButton)e.CommandSource;

            //    switch (lb.CommandName)
            //    {
            //        case "ReportDate":

            //            string[] arr = lb.CommandArgument.Split(new char[] { '_' });

            //            int divert_id = CommonUtility.Util.if_i(arr[0], 0);
            //            int row_zid = CommonUtility.Util.if_i(arr[1], 0);

            //            if (event_click_reportdate != null)
            //            {
            //                event_click_reportdate(divert_id, row_zid);
            //            }

            //            break;
            //    }
            //}


        }

        protected void grdMD050_PreRender(object sender, EventArgs e)
        {

        }

        public void BindGrid()
        {
            this.LoadMD050();
        }

        /// <summary>
        /// pulls all checked items from the grid and modifies the dictioanry refernces. saves DB queries
        /// </summary>
        /// <param name="diR"></param>
        /// <param name="diMA"></param>
        public void GetChecked(ref Dictionary<int, RDBDivert> diD, ref Dictionary<int, MerchantApp> diMA)
        {
            throw new Exception("come back and fix, this, we removed the datakey names");

            diD = new Dictionary<int, RDBDivert>();
            diMA = new Dictionary<int, MerchantApp>();

            var checkedIDs = from GridViewRow msgRow in grdMD050.Rows
                             where ((CheckBox)msgRow.FindControl("cbSelect")).Checked
                             select Int32.Parse(grdMD050.DataKeys[msgRow.RowIndex].Value.ToString());

            List<int> liDivertID = checkedIDs.ToList();

            foreach (int Divert_id in liDivertID)
            {
                RDBDivert objRel = DataReserve.GetRDBDivert(Divert_id);

                if (objRel != null && !diD.ContainsKey(objRel.DivertID))
                {
                    diD.Add(objRel.DivertID, objRel);
                }


                if (objRel != null)
                {
                    if (!diMA.ContainsKey(objRel.ZID))
                    {
                        MerchantApp objMA = DataMerchantApp.GetInstance().GetMerchantApp(objRel.ZID);

                        diMA.Add(Convert.ToInt32(objMA.ID), objMA);
                    }
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {


            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Divert");

                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);


                string[,] liHeaders = new string[9, 3]
                    {
                        {"ReportDate", "Report Date", "string"},
                        {"ZID", "ZID", "integer"},
                        {"DivertCategory", "Category", "string"},
                        {"Amount", "Amount", "currency"},
                        {"BatchWithHeldAmount", "Batch Withheld", "currency"},
                        {"Reserve", "Reserve", "currency"},
                        {"DivertClear", "Divert", "currency"},
                        {"DivertReject", "Paysafe", "currency"},
                        {"PostMerchant", "Merchant", "currency"},
                    };

                DataTable dt = GridViewExportUtil.GetExportableDataTable<RDBDivert>(li, liHeaders);

                GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

                ws.Cells.LoadFromDataTable(dt, true);

                string filename = "";
                if (this.ZID > 0)
                {
                    filename = String.Format("RDBDivert_ZID-{0}_{1}.xlsx", this.ZID.ToString(), CommonUtility.Util.GetDateTimeStamp());
                }
                else
                {
                    filename = String.Format("RDBDivert_{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());
                }



                //Write it back to the client
                Response.Clear();   // necessary!!!
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!

            }
        }

        protected void grdMD050_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == "ReportDate")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsReportDate == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.ReportDate);
                    grdMD050.DataBind();
                    this.VsReportDate = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.ReportDate);
                    grdMD050.DataBind();
                    this.VsReportDate = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "BankName")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsBankName == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.BankName);
                    grdMD050.DataBind();
                    this.VsBankName = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.BankName);
                    grdMD050.DataBind();
                    this.VsBankName = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "DivertCategory")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsDivertCategory == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.DivertCategory);
                    grdMD050.DataBind();
                    this.VsDivertCategory = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.DivertCategory);
                    grdMD050.DataBind();
                    this.VsDivertCategory = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Amount")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsAmount == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.Amount);
                    grdMD050.DataBind();
                    this.VsAmount = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.Amount);
                    grdMD050.DataBind();
                    this.VsAmount = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "BatchWithHeldAmount")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsBatchWithHeldAmount == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.BatchWithHeldAmount);
                    grdMD050.DataBind();
                    this.VsBatchWithHeldAmount = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.BatchWithHeldAmount);
                    grdMD050.DataBind();
                    this.VsBatchWithHeldAmount = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Reserve")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsReserve == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.Reserve);
                    grdMD050.DataBind();
                    this.VsReserve = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.Reserve);
                    grdMD050.DataBind();
                    this.VsReserve = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "DivertClear")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsDivertClear == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.DivertClear);
                    grdMD050.DataBind();
                    this.VsDivertClear = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.DivertClear);
                    grdMD050.DataBind();
                    this.VsDivertClear = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "DivertReject")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsDivertReject == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.DivertReject);
                    grdMD050.DataBind();
                    this.VsDivertReject = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.DivertReject);
                    grdMD050.DataBind();
                    this.VsDivertReject = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "PostMerchant")
            {
                List<RDBDivert> li = DataReserve.GetRDBDivert(this.m_Prms);

                if (this.VsPostMerchant == SortDirection.Ascending)
                {
                    grdMD050.DataSource = li.OrderByDescending(x => x.PostMerchant);
                    grdMD050.DataBind();
                    this.VsPostMerchant = SortDirection.Descending;
                }
                else
                {
                    grdMD050.DataSource = li.OrderBy(x => x.PostMerchant);
                    grdMD050.DataBind();
                    this.VsPostMerchant = SortDirection.Ascending;
                }
            }
        }


    }
}
