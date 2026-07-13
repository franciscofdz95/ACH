using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using System.Data;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.BusinessObjects;
using OfficeOpenXml;

namespace ZeusWeb.UserControls
{
    public partial class wucDivertGrid : wucBaseSearch
    {
        public delegate void EventClickReportDate(DateTime DivertDate, int zid);
        public event EventClickReportDate event_click_reportdate;

        /// <summary>
        /// set to false when you want to disable the submit button and hide the checkboxes.
        /// </summary>
        //public bool EnablePost
        //{
        //    get { return (bool)(ViewState["EnablePost"] ?? true); }
        //    set { ViewState["EnablePost"] = value; }
        //}

        private bool _ViewPendingRecords = false;

        public bool ViewPendingRecords
        {
            get { return _ViewPendingRecords; }
            set { _ViewPendingRecords = value; }
        }

        private bool _ShowMerchantColumns = false;

        public bool ShowMerchantColumns
        {
            get { return _ShowMerchantColumns; }
            set { _ShowMerchantColumns = value; }
        }

        private bool _ShowCheckbox = true;

        public bool ShowCheckbox
        {
            get { return _ShowCheckbox; }
            set { _ShowCheckbox = value; }
        }

        public const int COLUMN_CHECKBOX = 0;
        public const int COLUMN_ZID = 3;
        public const int COLUMN_MID = 4;
        public const int COLUMN_DBA = 5;

        public int GridRowCount
        {
            get { return (int)(ViewState["GridRowCount"] ?? 0); }
            set { ViewState["GridRowCount"] = value; }
        }

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

        public GridView DivertGrid
        {
            get
            {
                return this.grdMD050;
            }
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

            if (this.ViewPendingRecords)
            {
                prms.Add("@ViewPendingRecords", 1);
            }

            if (!string.IsNullOrEmpty(this.QAReportDate))
                prms.Add("@QAReportDate", this.QAReportDate);



            //List<RDBDivert> li = DataReserve.GetRDBDivert(prms);


            //if (li == null)
            //{
            //    // adds columns if empty
            //    li = new List<RDBDivert>();
            //}


            //grdMD050.DataSource = li;
            //grdMD050.DataBind();

            this.m_Prms = prms;


            DataSet ds = DataReserve.GetRDBDivertSummary(this.m_Prms);

            if (ds != null)
            {
                this.GridRowCount = ds.Tables[0].Rows.Count;
            }

            grdMD050.DataSource = ds;
            grdMD050.DataBind();


        }

        protected void grdMD050_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:


                    // int journalid = CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "JournalID"), 0);

                    // format report date
                    LinkButton lnkReportDate = (LinkButton)e.Row.FindControl("lnkReportDate");
                    lnkReportDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "ReportDate")).ToString("MM/dd/yyy");


                    Label lRD = (Label)e.Row.FindControl("lblReportDate");

                    //// dynamically determine status
                    //Literal litS = (Literal)e.Row.FindControl("litStatus");
                    //litS.Text = (journalid > 0) ? "Confirmed" : "Pending";


                    if (this.ViewPendingRecords == false)
                    {
                        // in history mode, hide link. show text.
                        lRD.Visible = true;
                    }
                    else
                    {
                        lRD.Visible = false;
                    }

                    lnkReportDate.Visible = !lRD.Visible;


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


                    // only show checkbox if it's the first earliest item for that zid.
                    CheckBox cb = (CheckBox)e.Row.FindControl("cbSelect");
                    cb.Enabled = (CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "Rnk"), 0) == 1) ? true : false;

                    break;

                case DataControlRowType.Footer:

                    break;

                default:
                    break;
            }
        }


        protected void grdMD050_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {
                LinkButton lb = (LinkButton)e.CommandSource;

                switch (lb.CommandName)
                {
                    case "ReportDate":

                        string[] arr = lb.CommandArgument.Split(new char[] { '_' });

                        DateTime divert_date = DateTime.Parse(arr[0]);
                        int row_zid = CommonUtility.Util.if_i(arr[1], 0);

                        if (event_click_reportdate != null)
                        {
                            event_click_reportdate(divert_date, row_zid);
                        }

                        break;
                }
            }


        }

        protected void grdMD050_PreRender(object sender, EventArgs e)
        {
            grdMD050.Columns[COLUMN_CHECKBOX].Visible = this.ShowCheckbox;
            grdMD050.Columns[COLUMN_ZID].Visible = this.ShowMerchantColumns;
            grdMD050.Columns[COLUMN_MID].Visible = this.ShowMerchantColumns;
            grdMD050.Columns[COLUMN_DBA].Visible = this.ShowMerchantColumns;


            //grdMD050.Columns[COLUMN_CHECKBOX].Visible = this.EnablePost;
            

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
        public List<string> GetChecked()
        {

            List<string> liZIDRD = this.GetChecked_RD_ZID();

            //foreach (GridViewRow gvr in grdMD050.Rows)
            //{
            //    CheckBox cb = (CheckBox)gvr.FindControl("cbSelect");
            //    if (cb.Checked)
            //    {

            //        int zid = Convert.ToInt32(DataBinder.Eval(gvr.DataItem, "ZID"));


            //        DateTime reportdate = Convert.ToDateTime(((Label)(gvr.FindControl("lblReportDate"))).Text);
            //    }
            //}

            //List<int> liDivertID = checkedIDs.ToList();

            //foreach (int Divert_id in liDivertID)
            //{
            //    RDBDivert objRel = DataReserve.GetRDBDivert(Divert_id);

            //    if (objRel != null && !diD.ContainsKey(objRel.DivertID))
            //    {
            //        diD.Add(objRel.DivertID, objRel);
            //    }


            //    if (objRel != null)
            //    {
            //        if (!diMA.ContainsKey(objRel.ZID))
            //        {
            //            MerchantApp objMA = DataMerchantApp.GetInstance().GetMerchantApp(objRel.ZID);

            //            diMA.Add(Convert.ToInt32(objMA.ID), objMA);
            //        }
            //    }
            //}

            return liZIDRD;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Dispostion of Diverted Funds");

                DataSet ds = DataReserve.GetRDBDivertSummary(this.m_Prms);

                string[,] liHeaders = new string[10, 3] {
			            {"ReportDate", "Report Date", "string"},	
                        {"ZID", "ZID", "integer"},	
                        {"settleplatformmid", "MID", "string"},	
                        {"businessdbaname", "DBAName", "string"},	
                        {"Amount", "Amount Withheld", "currency"},	
                        {"BatchWithHeldAmount", "Sent to Rsrv Acct", "currency"},	
                        {"Reserve", "Reserve", "currency"},	
                        {"DivertClear", "Divert", "currency"},	
                        {"DivertReject", "Paysafe", "currency"},	
                        {"PostMerchant", "Merchant", "currency"},	
                    };

                DataTable dt = GridViewExportUtil.GetExportableDataTable(ds.Tables[0], liHeaders);

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

        public List<string> GetChecked_DivertID()
        {
            List<string> li = new List<string>();

            var checkedIDs = from GridViewRow msgRow in grdMD050.Rows
                             where ((CheckBox)msgRow.FindControl("cbSelect")).Checked
                             select (((HiddenField)grdMD050.Rows[msgRow.RowIndex].FindControl("hidDivertIDs")).Value);

            if (checkedIDs != null)
            {
                foreach (string str in checkedIDs)
                {
                    string cleanstr = str.Trim();
                    string[] arr = cleanstr.Split(new char[] { ',' });

                    foreach (string s in arr)
                    {
                        string sclean = s.Trim();
                        if (!string.IsNullOrWhiteSpace(sclean))
                        {
                            li.Add(sclean);
                        }
                    }

                }
            }

            return li;

        }

        public List<string> GetChecked_RD_ZID()
        {


            var checkedIDs = from GridViewRow msgRow in grdMD050.Rows
                             where ((CheckBox)msgRow.FindControl("cbSelect")).Checked
                             select (grdMD050.DataKeys[msgRow.RowIndex].Values[0].ToString()) + "_" + (grdMD050.DataKeys[msgRow.RowIndex].Values[1].ToString());

            List<string> li = checkedIDs.ToList();

            return li;

        }
    }
}
