using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;

using OfficeOpenXml;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;
using Ionic.Zip;
using System.Collections;
using System.Text;
using System.Data;

namespace ZeusWeb
{
    public partial class frmReserveQueue : frmBaseSearch
    {
       
        public enum eDivertSource : int
        {
            Reserve = 1, // reserve + divert
            Meritus = 2, // meritus ops account
            Merchant = 3 // merchant's account

        }

        public const string MERITUS_RESERVE_ACCOUNT = "1309003281";
        public const string MERITUS_RESERVE_ROUTING = "113008465";

        public const string MERITUS_OPERATIONAL_ACCOUNT = "477989003";
        public const string MERITUS_OPERATIONAL_ROUTING = "322271627";

        public const string DIVERT_NCAL_LIST_TEMPLATE = "divert-ncal-list-template.xlsx";   // single form
        public const string DIVERT_WOODFOREST_LIST_TEMPLATE = "divert-woodforest-list-template.xlsx";  // colorful worksheet sheet

        public const string RELEASE_NCAL_TEMPLATE = "release-ncal-template.xlsx"; // single form
        public const string RELEASE_WELLS_LIST_TEMPLATE = "release-wells-list-template.xlsx"; // colorful worksheet sheet
        public const string RELEASE_WELLS_WIRE_TEMPLATE = "release-wells-wire-template.xlsx";   // single form
        public const string RELEASE_WOODFOREST_LIST_TEMPLATE = "release-woodforest-list-template.xlsx"; // colorful worksheet sheet
        

        // DO WE USE THESE ANYMORE??
        //public const string RELEASE_WELLS_CHECK_TEMPLATE = "release-wells-check-template.xlsx";
        //public const string DIVERT_WOODFOREST_DEBIT_TEMPLATE = "divert-woodforest-debit-template.xlsx";
        //public const string DIVERT_WOODFOREST_CREDIT_TEMPLATE = "divert-woodforest-credit-template.xlsx";


        public string DIRECTORY_RESERVE_FORMS
        {
            get { return (string)(ViewState["DIRECTORY_RESERVE_FORMS"] ?? ""); }
            set { ViewState["DIRECTORY_RESERVE_FORMS"] = value; }
        }

        public string SelectedReportDate
        {
            get { return (string)(ViewState["SelectedReportDate"] ?? ""); }
            set { ViewState["SelectedReportDate"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

            // build the excel sheet from the successful list
            this.DIRECTORY_RESERVE_FORMS = Server.MapPath("~/forms/reserve/");

            wucDivertGrid1.event_click_reportdate += new UserControls.wucDivertGrid.EventClickReportDate(wucReserveMD0501_event_click_reportdate);
            wucReleaseGrid1.event_click_reportdate += new UserControls.wucReleaseGrid.EventClickReportDate(wucReleaseGrid1_event_click_reportdate);
            wucReserveGrid1.event_click_reportdate += new UserControls.wucReserveGrid.EventClickReportDate(wucReserveGrid1_event_click_reportdate);
            wucReserveGrid1.event_click_amount += new UserControls.wucReserveGrid.EventClickAmount(wucReserveGrid1_event_click_amount);
            wucReserveGrid1.event_click_reservepercent += wucReserveGrid1_event_click_reservepercent;
            wucReserveGrid1.event_click_reserveamount += wucReserveGrid1_event_click_reserveamount;


            wucReleaseDialog1.event_click_savesuccess += new UserControls.Reserve.wucReleaseDialog.EventClickSaveSuccess(wucReleaseDialog1_event_click_savesuccess);
            wucDivertDialog1.event_click_savesuccess += new UserControls.Reserve.wucDivertDialog.EventClickSaveSuccess(wucDivertDialog1_event_click_savesuccess);
            wucReserveDialog1.event_click_savesuccess += new UserControls.Reserve.wucReserveDialog.EventClickSaveSuccess(wucReserveDialog1_event_click_savesuccess);
            wucReservePercentDialog1.event_click_savesuccess += wucReservePercentDialog1_event_click_savesuccess;
            wucReserveAmountDialog1.event_click_savesuccess += wucReserveAmountDialog1_event_click_savesuccess;

            wucReleaseGrid1.event_click_export += wucReleaseGrid1_event_click_export;


            if (!Page.IsPostBack)
            {
                FormHandler.SetSecurity(this.Page);

                // all this does is refresh the display list of the batch for that particular day
                this.RefreshReleaseBatch(DateTime.Now);
                this.RefreshDivertBatch(DateTime.Now);

                //run data check on daily imports for SD077, MD050 and ADR060. if there's no issues
                //with these daily imports then we can display the import buttons, otherwise we'll
                //reports errors to the user
                DateTime reportDate = DateTime.Now.AddDays(-1).Date;

                ValidateImport(reportDate);

                DisplayLastImportReport();
            }

            //if (wucReserveGrid1.GridRowCount == 0)
            //{
            //    wucDivertGrid1.EnablePost = true;
            //}
            //else
            //{
            //    wucDivertGrid1.EnablePost = false;
            //}

            this.Page.PreRender += Page_PreRender;

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {

            //wucDivertGrid1.EnablePost = (wucReserveGrid1.GridRowCount == 0) ? true : false;


            //if (!this.IsADR060UploadedToday())
            //{
            //    // disable post for divert if no file is uploaded today.
            //    //wucDivertGrid1.EnablePost = false;
            //    pnlADR060.Visible = true;


            //}
            //else
            //{
            //    pnlADR060.Visible = false;
            //}

            //wucReleaseGrid1.EnablePost = (wucReserveGrid1.GridRowCount == 0 && wucDivertGrid1.GridRowCount == 0) ? true : false;

            ApplyPostRules();

        }

        //private bool IsADR060UploadedToday()
        //{
        //    bool ret = false;
        //    List<RDBUpload> li = DataReserve.GetRDBUpload(new Hashtable()
        //        {
        //            {"@PageSize", 1},
        //            {"@CurrentPage", 1},
        //            {"@UploadTypeID", (int)eRDBReserveUploadTypeID.ADR060}
        //        });

        //    if (li != null && li.Count > 0 && li[0].LastUploadedInType.ToShortDateString() == DateTime.Now.ToShortDateString())
        //    {
        //        ret = true;
        //    }

        //    return ret;
        //}

        protected void wucReleaseGrid1_event_click_export(List<RDBRelease> li, int zid)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Releases");

                string[,] liHeaders = new string[11, 3]
                {
                    {"ZID", "ZID", "integer"},
                    {"SettlePlatformMID", "MID", "string"},
                    {"BusinessDBAName", "DBA Name", "string"},
                    {"Amount", "Amount", "currency"},
                    {"TransType", "Release Type", "string"},
                    {"ReserveType", "Reserve Type", "string"},
                    {"Method", "Method", "string"},
                    {"Bank", "Bank", "string"},
                    {"BankNotes", "Bank Notes", "string"},
                    {"UserName", "Initiated By", "string"},
                    {"ApprovedBy", "Approved By", "string"},
                };

                DataTable dt = GridViewExportUtil.GetExportableDataTable<RDBRelease>(li, liHeaders);

                GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

                ws.Cells.LoadFromDataTable(dt, true);

                string filename = "";

                if (zid > 0)
                {
                    filename = String.Format("RDBReleases_ZID-{0}_{1}.xlsx", zid.ToString(), CommonUtility.Util.GetDateTimeStamp());
                }
                else
                {
                    filename = String.Format("RDBReleases_{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());
                }

                //Write it back to the client
                Response.Clear();   // necessary!!!
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!

            }
        }

        protected void wucReserveGrid1_event_click_reserveamount(int zid, int ReserveID)
        {
            wucReserveAmountDialog1.ZID = zid;
            wucReserveAmountDialog1.ReserveID = ReserveID;
            wucReserveAmountDialog1.FormShow("");
            wucReserveAmountDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        protected void wucReserveAmountDialog1_event_click_savesuccess()
        {
            wucReserveGrid1.BindGrid();
        }

        protected void wucReservePercentDialog1_event_click_savesuccess()
        {
            wucReserveGrid1.BindGrid();
        }

        protected void wucReserveGrid1_event_click_reservepercent(int zid, int ReserveID, string reportdate)
        {
            wucReservePercentDialog1.ZID = zid;
            wucReservePercentDialog1.ReserveID = ReserveID;
            wucReservePercentDialog1.ReportDate = DateTime.Parse(reportdate);
            wucReservePercentDialog1.FormShow();
            wucReservePercentDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        //private void HandleImportReserveButtonDisplay()
        //{
        //    DateTime dtLast = DataReserve.GetReserveLastReportDate();

        //    string strNextDay = dtLast.AddDays(1).ToShortDateString();

        //    lblLastImportedDateReserve.Text = dtLast.ToShortDateString();

        //    tbImportDateReserve.Text = strNextDay;

        //    btnImportReserve.Text = "Import Data for " + strNextDay;
        //    btnImportReserve.Attributes.Add("onclick", "return confirm('Are you sure you want to import RESERVE data for " + strNextDay + "')");

        //}

        //private void HandleImportDivertButtonDisplay()
        //{
        //    DateTime dtLast = DataReserve.GetDivertLastReportDate();

        //    string strNextDay = dtLast.AddDays(1).ToShortDateString();

        //    lblLastImportedDateDivert.Text = dtLast.ToShortDateString();

        //    tbImportDateDivert.Text = strNextDay;

        //    btnImportDivert.Text = "Import Data for " + strNextDay;
        //    btnImportDivert.Attributes.Add("onclick", "return confirm('Are you sure you want to import DIVERT data for " + strNextDay + "')");

        //}

        private void DisplayLastImportReport()
        {
            //in theory both last import dates for reserve and divert imports should be the same
            //since both reports get imported when clicking on the Import button for a given report 
            //date, but there can be a scenario where the import finishes importing divert but fails
            //to import reserve or vice versa. to account for this we'll do some validation and make 
            //sure the report dates for divert and reserve are the same. if they aren't the same then
            //we'll spit back an error to the user
            DateTime divLast = DataReserve.GetDivertLastReportDate();
            DateTime resLast = DataReserve.GetReserveLastReportDate();

            if (divLast.CompareTo(resLast) == 0)
            {
                //equal is a good thing
                this.lblLastImport.Text = "Last Imported Date: " + divLast.ToString("MM/dd/yyyy");
            }
            else
            {
                //oh crap, the eport dates are different, let's notify the user
                this.lblLastImport.Text = "Last import dates for Divert and Release are different, please contact IT.";
                this.lblLastImport.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void wucReserveDialog1_event_click_savesuccess(int zid)
        {
            wucReserveGrid1.BindGrid();
        }

        protected void wucDivertDialog1_event_click_savesuccess(int zid)
        {
            wucDivertGrid1.BindGrid();
        }

        protected void wucReleaseDialog1_event_click_savesuccess(int zid)
        {
            wucReleaseGrid1.BindGrid();
        }

        protected void wucReserveGrid1_event_click_amount(int zid, string reportdate)
        {
            wucReserveBatchDetailsDialog1.ZID = zid;
            wucReserveBatchDetailsDialog1.ReportDate = DateTime.Parse(reportdate);
            wucReserveBatchDetailsDialog1.FormShow("");
            wucReserveBatchDetailsDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        protected void wucReserveGrid1_event_click_reportdate(int reserveid, int zid)
        {
            wucReserveDialog1.ZID = zid;
            wucReserveDialog1.ReserveID = reserveid;
            wucReserveDialog1.FormShow("");
            wucReserveDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

        }

        protected void wucReleaseGrid1_event_click_reportdate(int releaseid, int zid)
        {
            wucReleaseDialog1.ZID = zid;
            wucReleaseDialog1.ReleaseID = releaseid;
            wucReleaseDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            wucReleaseDialog1.FormShow("");
        }

        protected void wucReserveMD0501_event_click_reportdate(DateTime DivertDate, int zid)
        {
            wucDivertDialog1.ZID = zid;
            wucDivertDialog1.DivertDate = DivertDate;
            wucDivertDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            wucDivertDialog1.FormShow("");
        }

        #region " RESERVE "

        protected void btnReserveConfirm_Click(object sender, EventArgs e)
        {

            // returns list of "ReserveID_ZID"
            List<string> li = wucReserveGrid1.GetChecked();

            foreach (string RID_ZID in li)
            {
                string[] arr = RID_ZID.Split(new char[] { '_' });

                int ReserveID = CommonUtility.Util.if_i(arr[0], 0);
                int ZID = CommonUtility.Util.if_i(arr[1], 0);

                if (ReserveID > 0 && ZID > 0)
                {

                    // first call to add to journal
                    DataReserve.InsertRDBJournalReserveTrans(ReserveID, ZID, Convert.ToInt32(UserSessions.CurrentUser.UserID), DateTime.MinValue);

                    // second call to post to journal.
                    DataReserve.InsertRDBJournalReserveTrans(ReserveID, ZID, Convert.ToInt32(UserSessions.CurrentUser.UserID), DateTime.Today);

                    //// rule from risk: we want to move all diverts to reserve and notate it. so effectively, it will zero out the divert amounts
                    //RDBReserve objR = DataReserve.GetRDBReserve(ReserveID, ZID);

                    //if (objR != null && objR.Divert > 0)
                    //{
                    //    DataReserve.InsertTransferDivert2Reserve(ZID, objR.Divert, objR.BankID, CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0));
                    //}

                    // update grid
                    wucReserveGrid1.BindGrid();
                }


            }
        }

        #endregion


        #region " DIVERT "

        protected void GetDivertZip(int batchid)
        {
            this.GetDivertZip(batchid, null);
        }

        protected void GetDivertZip(List<string> liRDZID)
        {
            this.GetDivertZip(0, liRDZID);
        }

        private void GetDivertZip(int batchid, List<string> liRDZID)
        {
            // KNOWLEDGE: for diverts. the grid that appears on the queue page is a summary. this summary is composed 


            // this function will handle all divert related materials from different banks.

            // each bank gets 1 excel file.


            // the temporary directory we will be dropping our files to
            string tempdir = string.Format(@"c:\temp\Reserve\{0}\", Guid.NewGuid().ToString());

            // the filename we're going to be naming our zip.
            string newfile = CommonUtility.Util.GetDateTimeStamp() + ".zip";

            string newzipfilefull = tempdir + newfile;

            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            Dictionary<int, MerchantApp> diMA = new Dictionary<int, MerchantApp>();

            if (batchid > 0 && liRDZID == null)
            {
                // get liRDZID based on batchid
                Hashtable prms = new Hashtable();
                prms.Add("@DivertBatchID", batchid);

                List<RDBDivert> liDiv = DataReserve.GetRDBDivert(prms);

                if (liDiv != null && liDiv.Count > 0)
                {
                    liRDZID = new List<string>();

                    foreach (RDBDivert r in liDiv)
                    {
                        liRDZID.Add(string.Format("{0}_{1}", r.ReportDate.ToShortDateString(), r.ZID.ToString()));
                    }
                }

            }


            // this will populate the directory with files.
            this.BuildExcel_Divert_Woodforest_List(liRDZID, tempdir);


            // TODO: add for any other's that need divert.


            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(tempdir);
                zip.Save(newzipfilefull);
            }

            byte[] b = CommonUtility.FileHandling.ReadFromDisk(newzipfilefull);

            if (b != null)
            {
                // now that we have the binary, delete everything in that temp dir
                Directory.Delete(tempdir, true);

                //Write it back to the client
                Response.Clear();   // necessary!!!
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment;  filename=" + newfile);
                Response.BinaryWrite(b);
                Response.End();
            }
        }

        protected void btnDivertPreviewExcel_Click(object sender, EventArgs e)
        {
            this.GetDivertZip(wucDivertGrid1.GetChecked_RD_ZID());
        }

        protected void btnConfirmDivert_Click(object sender, EventArgs e)
        {
            // get all select ReportDate, ZID pairs.

            List<string> li_RD_ZID = wucDivertGrid1.GetChecked_RD_ZID();

            if (li_RD_ZID != null && li_RD_ZID.Count > 0)
            {
                int divert_batch_id = DataReserve.InsertDivertBatch(Convert.ToInt32(UserSessions.CurrentUser.UserID));

                foreach (string pair in li_RD_ZID)
                {

                    string[] arr = pair.Split(new char[] { '_' });

                    DateTime report_date = DateTime.Parse(arr[0]);
                    int zid = CommonUtility.Util.if_i(arr[1], 0);

                    Hashtable prms = new Hashtable();
                    prms.Add("@QAReportDate", report_date);
                    prms.Add("@ViewPendingRecords", 1);
                    prms.Add("@ZID", zid);

                    List<RDBDivert> liD = DataReserve.GetRDBDivert(prms);

                    if (liD != null && liD.Count > 0)
                    {

                        if (divert_batch_id > 0)
                        {
                            foreach (RDBDivert item in liD)
                            {
                                // associate divert record with newly created batch id.
                                DataReserve.InsertDivertBatchDetail(divert_batch_id, item.DivertID);

                                //// add divert entry into journal.
                                //DataReserve.InsertRDBJournalDivertTrans(item);

                                //// post record
                                //item.PostedDate = DateTime.Now;
                                //DataReserve.InsertRDBJournalDivertTrans(item);

                            }

                            // insert it.
                            DataReserve.InsertRDBJournalDivertSummaryTrans(zid, report_date);

                            // post it.
                            DataReserve.InsertRDBJournalDivertSummaryTrans(zid, report_date, DateTime.Now);
                        }
                    }
                }

                wucDivertGrid1.BindGrid();

                this.RefreshDivertBatch(DateTime.Now);
            }

        }

        // obsolete, i don't think we need this anymore because we consolidated the 
        //private void BuildDivertSignatureFiles(decimal amount, MerchantApp ma, string tempdir, int bank_id, eDivertSource eDS, DateTime report_date)
        //{

        //    // wells
        //    if (bank_id == 1)
        //    {
                
        //    }

        //    // woodforest
        //    if (bank_id == 2)
        //    {

        //        // TODO: we don't need to do this anymore. just create that 1 excel sheet.

        //        if (amount < 0)
        //        {
        //            // debit
        //            this.BuildExcel_Divert_Woodforest_Debit(amount, ma, tempdir, eDS, report_date);
        //        }
        //        else if (amount > 0)
        //        {
        //            // credit
        //            this.BuildExcel_Divert_Woodforest_Credit(amount, ma, tempdir, eDS, report_date);

        //        }
        //    }

        //    // ncal
        //    if (bank_id == 5)
        //    {

        //    }


        //}

        //private byte[] GetDivertZip(string tempdir, string newfile, ref Dictionary<int, RDBDivert> diD)
        //{

        //    string newzipfilefull = tempdir + newfile;

        //    if (!Directory.Exists(tempdir))
        //    {
        //        Directory.CreateDirectory(tempdir);
        //    }

        //    // get all rows that were checked.
        //    diD = new Dictionary<int, RDBDivert>();
        //    Dictionary<int, MerchantApp> diMA = new Dictionary<int, MerchantApp>();
        //    wucDivertGrid1.GetChecked(ref diD, ref diMA);

        //    //this.BuildReleaseFiles(diR, diMA, tempdir);

        //    this.BuildDivertSignatureFiles(diD, diMA, tempdir);

        //    using (ZipFile zip = new ZipFile())
        //    {
        //        zip.AddDirectory(tempdir);
        //        zip.Save(newzipfilefull);
        //    }

        //    byte[] b = CommonUtility.FileHandling.ReadFromDisk(newzipfilefull);

        //    return b;
        //}




        // at max, only generates 2 files.


        #endregion


        #region " RELEASE "

        protected void btnGenPDF_Click(object sender, EventArgs e)
        {
            // create tempdir.
            string tempdir = string.Format(@"c:\temp\Reserve\{0}\", Guid.NewGuid().ToString());
            string newfile = CommonUtility.Util.GetDateTimeStamp() + ".zip";

            // we don't really need this here. just here to please getreleasezip()
            Dictionary<int, RDBRelease> di = new Dictionary<int, RDBRelease>();

            byte[] b = this.GetReleaseZip(tempdir, newfile, ref di);

            if (b != null)
            {
                // now that we have the binary, delete everything in that temp dir
                Directory.Delete(tempdir, true);

                //Write it back to the client
                Response.Clear();   // necessary!!!
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment;  filename=" + newfile);
                Response.BinaryWrite(b);
                Response.End();

            }

        }

        protected void btnReleaseFinal_Click(object sender, EventArgs e)
        {
            // get all rows that were checked.
            Dictionary<int, RDBRelease> diR = new Dictionary<int, RDBRelease>();
            Dictionary<int, MerchantApp> diMA = new Dictionary<int, MerchantApp>();

            wucReleaseGrid1.GetChecked(ref diR, ref diMA);

            int current_userid = CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0);

            if (diR != null && diR.Count > 0)
            {
                DataTable dtresult = DataReserve.BulkReleasesPost(GetReleaseTable(diR), current_userid);
            }

            wucReleaseGrid1.BindGrid();
            this.RefreshReleaseBatch(DateTime.Now);
        }


        private static DataTable GetReleaseTable(Dictionary<int, RDBRelease> diR)
        {
            DataTable dtRelease = new DataTable("RDBRelease");

            dtRelease.Columns.Add("ReleaseID", typeof(int));
            dtRelease.Columns.Add("ReportDate", typeof(DateTime));
            dtRelease.Columns.Add("ZID", typeof(int));
            dtRelease.Columns.Add("Amount", typeof(decimal));
            dtRelease.Columns.Add("TransTypeID", typeof(int));
            dtRelease.Columns.Add("BankNotes", typeof(string));
            dtRelease.Columns.Add("ReserveTypeID", typeof(int));
            dtRelease.Columns.Add("BankID", typeof(int));
            dtRelease.Columns.Add("MethodID", typeof(int));
            dtRelease.Columns.Add("ApprovedUserID", typeof(int));
            dtRelease.Columns.Add("DateApproved", typeof(DateTime));
            dtRelease.Columns.Add("InternalNotes", typeof(string));

            foreach (KeyValuePair<int, RDBRelease> kvp in diR)
            {
                DataRow dr = dtRelease.NewRow();

                RDBRelease item = kvp.Value;

                dr["ReleaseID"] = item.ReleaseID;
                dr["ZID"] = item.ZID;
                dr["ReportDate"] = item.ReportDate;
                dr["Amount"] = item.Amount;
                dr["TransTypeID"] = item.TransTypeID;
                dr["BankNotes"] = item.BankNotes;
                dr["ReserveTypeID"] = item.ReserveTypeID;
                dr["MethodID"] = item.MethodID;
                dr["BankID"] = item.BankID;
                dr["ApprovedUserID"] = item.ApprovedUserID;
                dr["DateApproved"] = DateTime.Now;
                dr["InternalNotes"] = item.InternalNotes;

                dtRelease.Rows.Add(dr);

            }

            return dtRelease;
        }

        private byte[] GetReleaseZip(string tempdir, string newfile, ref Dictionary<int, RDBRelease> diR)
        {
            string newzipfilefull = tempdir + newfile;

            if (!Directory.Exists(tempdir))
            {
                Directory.CreateDirectory(tempdir);
            }

            Dictionary<int, MerchantApp> diMA = new Dictionary<int, MerchantApp>();

            // if a dictionary was already passsed in, then we just use that.
            if (diR != null && diR.Count > 0)
            {
                // build our own merchant list
                foreach (KeyValuePair<int, RDBRelease> item in diR)
                {
                    if (!diMA.ContainsKey(item.Value.ZID))
                    {
                        MerchantApp objMA = DataMerchantApp.GetInstance().GetMerchantApp(item.Value.ZID);
                        diMA.Add(Convert.ToInt32(objMA.ID), objMA);
                    }
                }
            }
            else
            {
                // get all rows that were checked.
                diR = new Dictionary<int, RDBRelease>();
                wucReleaseGrid1.GetChecked(ref diR, ref diMA);
            }

            this.BuildReleaseListFiles(diR, diMA, tempdir);

            this.BuildReleaseSingleFiles(diR, diMA, tempdir);

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(tempdir);
                zip.Save(newzipfilefull);
            }

            byte[] b = CommonUtility.FileHandling.ReadFromDisk(newzipfilefull);

            return b;
        }

        private void BuildReleaseSingleFiles(Dictionary<int, RDBRelease> diR, Dictionary<int, MerchantApp> diMA, string tempdir)
        {
            foreach (KeyValuePair<int, RDBRelease> kvp in diR)
            {
                // wells
                if (kvp.Value.BankID == eRDBBank.Wells)
                {
                    if (kvp.Value.MethodID == eRDBTransactionMethod.ACH) // ACH/Wire
                    {
                        this.BuildExcel_Release_Wells_Wire(kvp.Value, diMA[kvp.Value.ZID], tempdir);
                    }
                }

                // ncal
                if (kvp.Value.BankID == eRDBBank.Ncal)
                {
                    this.BuildExcel_Release_Ncal(kvp.Value, diMA[kvp.Value.ZID], tempdir);
                }
            }
        }

        private void BuildReleaseListFiles(Dictionary<int, RDBRelease> diR, Dictionary<int, MerchantApp> diMA, string tempdir)
        {
            //breakdown of release dictionaries by bank
            Dictionary<int, Dictionary<int, RDBRelease>> diBank = new Dictionary<int, Dictionary<int, RDBRelease>>();

            // split release file by bank.
            foreach (KeyValuePair<int, RDBRelease> kvp in diR)
            {
                if (!diBank.ContainsKey((int)kvp.Value.BankID))
                {
                    diBank.Add((int)kvp.Value.BankID, new Dictionary<int, RDBRelease>());
                }
                diBank[(int)kvp.Value.BankID].Add(kvp.Key, kvp.Value);
            }

            // loop thru for each bank, then generate seperate Excel files.
            foreach (KeyValuePair<int, Dictionary<int, RDBRelease>> kvpBank in diBank)
            {
                if (kvpBank.Key == (int)eRDBBank.Wells)
                {
                    this.BuildExcel_Release_Wells_List(kvpBank.Value, diMA, tempdir); // ach
                    //this.BuildExcel_Release_Wells_Check(kvpBank.Value, diMA, tempdir);

                    //Dictionary<int, RDBRelease> diRel = kvpBank.Value;
                    //foreach (KeyValuePair<int, RDBRelease> kvpRel in diRel)
                    //{
                    //    this.BuildExcel_Release_Wells_Wire(kvpRel.Value, diMA[kvpRel.Value.ZID], tempdir);
                    //}
                }
                else if (kvpBank.Key == (int)eRDBBank.Woodforest)
                {
                    // note for now, only 
                    this.BuildExcel_Release_Woodforest_List(kvpBank.Value, diMA, tempdir);
                }

                // note: NCAL's release are single files. not colorful excel sheets.
            }
        }

        // helper function
        protected decimal GetCBLastXMonths(string MID, DateTime dtInput, int monthcount)
        {
            DateTime dtTemp = dtInput.AddMonths(monthcount * -1);

            DateTime dtStart = new DateTime(dtTemp.Year, dtTemp.Month, 1);
            DateTime dtEnd = new DateTime(dtInput.Year, dtInput.Month, 1);

            return DataReserve.GetChargebackInfo(MID, dtStart, dtEnd);
        }

        #endregion


        #region " BUILD_EXCEL "

        // done
        public void BuildExcel_Release_Woodforest_List(Dictionary<int, RDBRelease> diR, Dictionary<int, MerchantApp> diMA, string tempdir)
        {
            List<RDBRelease> liR = new List<RDBRelease>();

            foreach (KeyValuePair<int, RDBRelease> kvp in diR)
            {
                // look only for woodforest. and method 1 (ach)
                if (kvp.Value.BankID == eRDBBank.Woodforest && kvp.Value.MethodID == eRDBTransactionMethod.ACH)
                {
                    liR.Add(kvp.Value);
                }
            }

            if (liR != null && liR.Count > 0)
            {


                FileInfo newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + RELEASE_WOODFOREST_LIST_TEMPLATE);
                string filename = String.Format("woodforest_Release_ReqDt{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());

                if (newFile != null)
                {
                    using (ExcelPackage pck = new ExcelPackage(newFile))
                    {
                        var ws = pck.Workbook.Worksheets[1];

                        int row_index = 4;

                        // setup rows first
                        if (liR.Count > 1)
                        {
                            // there is already a row in there. if there are more than 1 row, then we add those.
                            ws.InsertRow(4, liR.Count - 1, 4);
                        }

                        foreach (RDBRelease objRel in liR)
                        {

                            if (diMA.ContainsKey(objRel.ZID) && diMA[objRel.ZID] != null)
                            {
                                MerchantApp objMA = diMA[objRel.ZID];

                                ws.Cells["A" + row_index.ToString()].Value = objRel.ReportDate; // request date
                                ws.Cells["B" + row_index.ToString()].Value = objMA.BusinessDBAName; // merchant name
                                ws.Cells["C" + row_index.ToString()].Value = objMA.SettlePlatformMid; // merchant id
                                ws.Cells["D" + row_index.ToString()].Value = objMA.AccountNumber; // account number
                                ws.Cells["E" + row_index.ToString()].Value = objMA.RoutingNumber; // routing number
                                ws.Cells["F" + row_index.ToString()].Value = objRel.BankNotes; // description

                                if (objRel.TransTypeID == eRDBTransactionType.CollectReserve)
                                {
                                    ws.Cells["G" + row_index.ToString()].Value = objRel.Amount; // recover amount to cleint operational account
                                    ws.Cells["H" + row_index.ToString()].Value = "";  // release amount to merchant
                                }
                                else
                                {
                                    ws.Cells["G" + row_index.ToString()].Value = ""; ; // recover amount to cleint operational account
                                    ws.Cells["H" + row_index.ToString()].Value = objRel.Amount * -1; // release amount to merchant
                                }

                            }

                            row_index++;
                        }


                        int li_count_min_1 = (liR.Count - 1);

                        // recover amount calc
                        string cell_recoverlastrow = "G" + (4 + li_count_min_1);
                        string cell_recovertotal = "G" + (4 + li_count_min_1 + 3).ToString();
                        ws.Cells[cell_recovertotal].Formula = "=SUM(G4:" + cell_recoverlastrow + ")";

                        // release amount to merchant
                        string cell_releaselastrow = "H" + (4 + li_count_min_1);
                        string cell_releasetotal = "H" + (4 + li_count_min_1 + 5).ToString();
                        ws.Cells[cell_releasetotal].Formula = "=SUM(H4:" + cell_releaselastrow + ")";

                        // grand total
                        string cell_grandtotal = "H" + (4 + li_count_min_1 + 8).ToString();
                        ws.Cells[cell_grandtotal].Formula = string.Format("={0}+{1}", cell_recovertotal, cell_releasetotal);

                        CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);

                    }
                }
            }

        }

        // done
        public void BuildExcel_Release_Wells_Wire(RDBRelease objRel, MerchantApp objMA, string tempdir)
        {

            if (objRel != null && objMA != null)
            {

                // wire only
                if (objRel.MethodID != eRDBTransactionMethod.Wire)
                {
                    return;
                }

                if (objMA.ContactList == null)
                {
                    objMA.ContactList = DataContact.SearchContact(objRel.ZID, eControlContactType.Merchant);
                }



                FileInfo newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + RELEASE_WELLS_WIRE_TEMPLATE);
                string filename = String.Format("wells_Release_ReqDt{0} (wire).xlsx", CommonUtility.Util.GetDateTimeStamp());

                if (newFile != null)
                {
                    using (ExcelPackage pck = new ExcelPackage(newFile))
                    {
                        var ws = pck.Workbook.Worksheets[1];

                        ws.Cells["B3"].Value = objRel.ReportDate; // requeest date
                        ws.Cells["K3"].Value = objRel.ReleaseBatchDateCreated.ToShortDateString(); // transfer date
                        ws.Cells["B7"].Value = objRel.Amount * -1; // amount
                        ws.Cells["B15"].Value = objMA.BankName; // credit bank info
                        ws.Cells["B17"].Value = ""; // credit city, state
                        ws.Cells["B19"].Value = objMA.RoutingNumber; // credit routing aba
                        ws.Cells["B21"].Value = objMA.AccountNumber; // credit account
                        ws.Cells["B23"].Value = objMA.BusinessLegalName; // credit account name
                        //ws.Cells["I15"].Value = ; // debit bankname
                        //ws.Cells["I17"].Value = ; // debit city,state
                        //ws.Cells["I19"].Value = ; // debit routing
                        //ws.Cells["I21"].Value = ; // debit account
                        //ws.Cells["I23"].Value = ; // debit accountname

                        CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
                    }
                }
            }
        }

        // done
        public void BuildExcel_Release_Wells_List(Dictionary<int, RDBRelease> diR, Dictionary<int, MerchantApp> diMA, string tempdir)
        {

            List<RDBRelease> liR = new List<RDBRelease>();

            foreach (KeyValuePair<int, RDBRelease> kvp in diR)
            {

                if (kvp.Value.BankID == eRDBBank.Wells // look only for wells
                    && kvp.Value.MethodID == eRDBTransactionMethod.ACH // and ach
                    )
                {
                    liR.Add(kvp.Value);
                }
            }

            if (liR != null && liR.Count > 0)
            {

                FileInfo newFile = null;

                string filename = "";

                newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + RELEASE_WELLS_LIST_TEMPLATE);
                filename = String.Format("wells_QMA_Release_ReqDt{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());

                if (newFile != null)
                {
                    using (ExcelPackage pck = new ExcelPackage(newFile))
                    {
                        var ws = pck.Workbook.Worksheets[1];

                        int row_index = 4;

                        // setup rows first
                        if (liR.Count > 1)
                        {
                            // there is already a row in there. if there are more than 1 row, then we add those.
                            ws.InsertRow(4, liR.Count - 1, 4);
                        }

                        foreach (RDBRelease objRel in liR)
                        {

                            if (diMA.ContainsKey(objRel.ZID) && diMA[objRel.ZID] != null)
                            {
                                MerchantApp objMA = diMA[objRel.ZID];

                                ws.Cells["A" + row_index.ToString()].Value = objRel.ReportDate; // request date
                                ws.Cells["B" + row_index.ToString()].Value = objMA.BusinessDBAName; // merchant name
                                ws.Cells["C" + row_index.ToString()].Value = objMA.SettlePlatformMid; // merchant id
                                ws.Cells["D" + row_index.ToString()].Value = objRel.BankNotes; // description 1
                                ws.Cells["E" + row_index.ToString()].Value = ""; // description 2
                                ws.Cells["F" + row_index.ToString()].Value = ""; // description 3

                                if (objRel.TransTypeID == eRDBTransactionType.CollectReserve)
                                {
                                    ws.Cells["G" + row_index.ToString()].Value = objRel.Amount;
                                    ws.Cells["H" + row_index.ToString()].Value = "";
                                }
                                else
                                {
                                    ws.Cells["G" + row_index.ToString()].Value = ""; ; // recover amount to cleint operational account
                                    ws.Cells["H" + row_index.ToString()].Value = objRel.Amount * -1; // release amount to merchant
                                }

                            }

                            row_index++;
                        }


                        int li_count_min_1 = (liR.Count - 1);

                        // recover amount calc
                        string cell_recoverlastrow = "G" + (4 + li_count_min_1);
                        string cell_recovertotal = "G" + (4 + li_count_min_1 + 3).ToString();
                        ws.Cells[cell_recovertotal].Formula = "=SUM(G4:" + cell_recoverlastrow + ")";

                        // release amount to merchant
                        string cell_releaselastrow = "H" + (4 + li_count_min_1);
                        string cell_releasetotal = "H" + (4 + li_count_min_1 + 5).ToString();
                        ws.Cells[cell_releasetotal].Formula = "=SUM(H4:" + cell_releaselastrow + ")";

                        // grand total
                        string cell_grandtotal = "H" + (4 + li_count_min_1 + 8).ToString();
                        ws.Cells[cell_grandtotal].Formula = string.Format("={0}+{1}", cell_recovertotal, cell_releasetotal);

                        CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);

                    }
                }
            }

        }

        // done. i think we don't do this anymore.
        //public void BuildExcel_Release_Wells_Check(Dictionary<int, RDBRelease> diR, Dictionary<int, MerchantApp> diMA, string tempdir)
        //{
        //    List<RDBRelease> liR = new List<RDBRelease>();

        //    foreach (KeyValuePair<int, RDBRelease> kvp in diR)
        //    {
        //        // look only for wells. and check.
        //        if (kvp.Value.BankID == eRDBBank.Wells && kvp.Value.MethodID == eRDBTransactionMethod.Check)
        //        {
        //            liR.Add(kvp.Value);
        //        }
        //    }

        //    if (liR != null && liR.Count > 0)
        //    {

        //        FileInfo newFile = null;

        //        string filename = "";

        //        newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + RELEASE_WELLS_CHECK_TEMPLATE);
        //        filename = String.Format("wells_QMA_Release_ReqDt{0} (check).xlsx", CommonUtility.Util.GetDateTimeStamp());

        //        if (newFile != null)
        //        {
        //            using (ExcelPackage pck = new ExcelPackage(newFile))
        //            {
        //                var ws = pck.Workbook.Worksheets[1];

        //                int row_index = 2;

        //                // setup rows first
        //                if (liR.Count > 1)
        //                {
        //                    // there is already a row in there. if there are more than 1 row, then we add those.
        //                    ws.InsertRow(row_index, liR.Count - 1, row_index);
        //                }

        //                foreach (RDBRelease objRel in liR)
        //                {


        //                    if (diMA.ContainsKey(objRel.ZID) && diMA[objRel.ZID] != null)
        //                    {

        //                        MerchantApp objMA = diMA[objRel.ZID];

        //                        if (objMA.ContactList == null)
        //                        {
        //                            objMA.ContactList = DataContact.SearchContact(objRel.ZID, eControlContactType.Merchant);
        //                        }

        //                        ws.Cells["A" + row_index.ToString()].Value = objRel.SettlePlatformMID; //Merchant ID	
        //                        ws.Cells["B" + row_index.ToString()].Value = objMA.BusinessDBAName; //DBA Name	
        //                        ws.Cells["C" + row_index.ToString()].Value = objMA.BusinessLegalName; //Legal Name	
        //                        ws.Cells["D" + row_index.ToString()].Value = objMA.GetPrimaryContact().GetFullName(); //Principal(s) / Authorized Signer(s)	
        //                        ws.Cells["E" + row_index.ToString()].Value = objMA.BusinessMailingAddress; //Business Mailing Address	
        //                        ws.Cells["F" + row_index.ToString()].Value = objMA.BusinessMailingCity; //City	
        //                        ws.Cells["G" + row_index.ToString()].Value = objMA.BusinessMailingState; //State	
        //                        ws.Cells["H" + row_index.ToString()].Value = objMA.BusinessMailingZip; //Zip	
        //                        ws.Cells["I" + row_index.ToString()].Value = objRel.Amount * -1; //Check to Merchant

        //                    }

        //                    row_index++;

        //                }

        //                int li_count_min_1 = (liR.Count - 1);

        //                ws.Cells["I" + (liR.Count + 3).ToString()].Formula = "=SUM(I2:I" + (liR.Count + 1).ToString() + ")";

        //                CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //            }
        //        }
        //    }
        //}

        // done
        public void BuildExcel_Release_Ncal(RDBRelease objRel, MerchantApp objMA, string tempdir)
        {
            if (objRel != null && objMA != null)
            {
                // for now, only NCAL
                if (objMA.ContactList == null)
                {
                    objMA.ContactList = DataContact.SearchContact(objRel.ZID, eControlContactType.Merchant);
                }

                FileInfo newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + RELEASE_NCAL_TEMPLATE);
                string filename = String.Format("ncal_Release_ReqDt{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());

                if (newFile != null)
                {
                    using (ExcelPackage pck = new ExcelPackage(newFile))
                    {
                        var ws = pck.Workbook.Worksheets[1];


                        ws.Cells["C6"].Value = objMA.BusinessDBAName; // Merchant Name


                        ws.Cells["G6"].Value = objRel.ReleaseBatchDateCreated.ToShortDateString();  // date
                        ws.Cells["C8"].Value = objMA.SettlePlatformMid; // mid
                        ws.Cells["H8"].Value = objMA.GetPrimaryContact().GetFirstPhone(); // contact phone
                        ws.Cells["B10"].Value = objMA.AccountNumber; // DDA#
                        ws.Cells["H10"].Value = objMA.GetPrimaryContact().GetFullName(); // ContactName


                        string Checkbox_Checked = "[  X  ]";

                        if (objRel.TransTypeID == eRDBTransactionType.Meritus) // pay to meritus
                        {

                            // to paysafe operational account.
                            ws.Cells["D37"].Value = "Reserve Release"; // Rlease reason (To Meritus operation bank account)
                            ws.Cells["D41"].Value = GetCBLastXMonths(objMA.SettlePlatformMid, objRel.ReleaseBatchDateCreated, 1);
                            ws.Cells["H41"].Value = GetCBLastXMonths(objMA.SettlePlatformMid, objRel.ReleaseBatchDateCreated, 6);
                            ws.Cells["E42"].Value = DataReserve.GetLastBatchDate(objMA.SettlePlatformMid);
                            ws.Cells["D44"].Value = objRel.Amount * -1; // release amount (to meritus)

                            ws.Cells["A34"].Value = Checkbox_Checked; // checkbox Meritus
                            if (objRel.MethodID == eRDBTransactionMethod.ACH) // ACH
                            {
                                ws.Cells["D36"].Value = Checkbox_Checked + " ACH Transfer"; // ACH Transfer
                            }
                            else if (objRel.MethodID == eRDBTransactionMethod.Check) // Cashier's Check
                            {
                                ws.Cells["F36"].Value = Checkbox_Checked + " Cashier's Check"; // Cashier's Check
                                //ws.Cells["E46"].Value = ""; // mail cashier's check to
                                //ws.Cells["E48"].Value = ""; // mail cashier's check to
                            }
                        }
                        else if (objRel.TransTypeID == eRDBTransactionType.Merchant) // pay to merchant 
                        {
                            // to merchant
                            ws.Cells["D21"].Value = "Reserve Release"; // Release Reason (To Merchant)
                            ws.Cells["D25"].Value = GetCBLastXMonths(objMA.SettlePlatformMid, objRel.ReleaseBatchDateCreated, 1);  // Release Reason (To Merchant) (CB Vol last month)
                            ws.Cells["H25"].Value = GetCBLastXMonths(objMA.SettlePlatformMid, objRel.ReleaseBatchDateCreated, 6);  // Release Reason (To Merchant) (CB Vol last 6 months)
                            ws.Cells["E26"].Value = DataReserve.GetLastBatchDate(objMA.SettlePlatformMid); // date last processed
                            ws.Cells["D28"].Value = objRel.Amount * -1;  // release amount to merchant


                            ws.Cells["A18"].Value = Checkbox_Checked;  // checkbox Merchant

                            if (objRel.MethodID == eRDBTransactionMethod.ACH) // ACH
                            {
                                ws.Cells["D20"].Value = Checkbox_Checked + " ACH Transfer";
                            }
                            else if (objRel.MethodID == eRDBTransactionMethod.Check) // Cashier's Check
                            {
                                ws.Cells["F20"].Value = Checkbox_Checked + " Cashier's Check";
                                ws.Cells["E30"].Value = objMA.BusinessMailingAddress;  // mail cashier's check to
                                ws.Cells["E32"].Value = string.Format("{0}, {1} {2}", objMA.BusinessMailingCity, objMA.BusinessMailingState, objMA.BusinessMailingZip); // mail cashier's check to
                            }
                        }


                        ws.Cells["I56"].Value = objRel.ReleaseBatchDateCreated.ToShortDateString(); // date

                        CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
                    }


                }
            }
        }

        // done
        public void BuildExcel_Divert_Woodforest_List(List<string> liRDZID, string tempdir)
        {

            // for each rd/zid pair, run sp_RDB_SelectDivertCategorySummary

            List<Dictionary<string, string>> liFinal = new List<Dictionary<string, string>>();

            List<Dictionary<string, string>> liBreakdown = new List<Dictionary<string, string>>();

            foreach (string str in liRDZID)
            {
                string[] arr = str.Split(new char[] { '_' });

                DateTime reportdate = DateTime.Parse(arr[0]);
                int zid = CommonUtility.Util.if_i(arr[1], 0);

                DataTable dtSummary = DataReserve.GetRDBDivertSummaryCategory(zid, reportdate);

                foreach (DataRow dr in dtSummary.Rows)
                {
                    string mid = dr["SettlePlatformMID"].ToString();

                    // only do woodforest!!
                    if (LookupTableHandler.GetBankByMid(mid) != eRDBBank.Woodforest)
                    {
                        break;
                    }

                    Dictionary<string, string> di = new Dictionary<string, string>();

                    di["RequestDate"] = DateTime.Parse(dr["ReportDate"].ToString()).ToShortDateString();
                    di["MerchantName"] = dr["BusinessDBAName"].ToString();
                    di["MerchantID"] = mid;
                    di["AccountNumber"] = dr["AccountNumber"].ToString();
                    di["RoutingNumber"] = dr["RoutingNumber"].ToString();
                    di["Description"] = string.Format("MD-050 Activity for report date of {0}", reportdate.ToShortDateString());
                    di["ReserveAccount"] = string.Format("{0:F2}", CommonUtility.Util.if_dec(dr["Reserve"], 0) + CommonUtility.Util.if_dec(dr["DivertClear"], 0));
                    di["OperationalAccount"] = string.Format("{0:F2}", CommonUtility.Util.if_dec(dr["DivertReject"], 0));
                    di["ToMerchant"] = string.Format("{0:F2}", CommonUtility.Util.if_dec(dr["PostMerchant"], 0));
                    di["ZID"] = dr["ZID"].ToString();

                    di["percReserveBoth"] = dr["percReserveBoth"].ToString();
                    di["percDivertReject"] = dr["percDivertReject"].ToString();
                    di["percPostMerchant"] = dr["percPostMerchant"].ToString();

                    di["DivertCategoryID"] = dr["DivertCategoryID"].ToString();

                    liFinal.Add(di);
                }
            }


            FileInfo newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + DIVERT_WOODFOREST_LIST_TEMPLATE);
            string filename = String.Format("woodforest_divert_{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());

            List<string> liMidRd = new List<string>();

            if (newFile != null)
            {
                using (ExcelPackage pck = new ExcelPackage(newFile))
                {
                    var ws = pck.Workbook.Worksheets[1];
                    ws.Name = "ACH Credits and Debits";
                    int row_index = 4;

                    var ws2 = pck.Workbook.Worksheets[2];
                    ws2.Name = "my mex";
                    int row_index_page2 = 2;

                    // setup rows first
                    if (liFinal.Count > 1)
                    {
                        // there is already a row in there. if there are more than 1 row, then we add those.
                        ws.InsertRow(4, liFinal.Count - 1, 4);
                    }

                    Dictionary<int, MerchantApp> diMA = new Dictionary<int, MerchantApp>();


                    foreach (Dictionary<string, string> di in liFinal)
                    {

                        ws.Cells["A" + row_index.ToString()].Value = di["RequestDate"].ToString();             // request date
                        ws.Cells["B" + row_index.ToString()].Value = di["MerchantName"].ToString();         // merchant name
                        ws.Cells["C" + row_index.ToString()].Value = di["MerchantID"].ToString();       // merchant id
                        ws.Cells["D" + row_index.ToString()].Value = di["AccountNumber"];           // account number
                        ws.Cells["E" + row_index.ToString()].Value = di["RoutingNumber"];           // routing number
                        ws.Cells["F" + row_index.ToString()].Value = di["Description"];  // description
                        ws.Cells["G" + row_index.ToString()].Value = Convert.ToDecimal(di["ReserveAccount"]);
                        ws.Cells["H" + row_index.ToString()].Value = Convert.ToDecimal(di["OperationalAccount"]);      // to operational
                        ws.Cells["I" + row_index.ToString()].Value = Convert.ToDecimal(di["ToMerchant"]);          // to merchant

                        row_index++;

                        // note: we need to track what we've seen so that we don't double count.
                        string mykey_MidRd = string.Format("{0}_{1}", di["MerchantID"].ToString(), di["RequestDate"].ToString());

                        if (!liMidRd.Contains(mykey_MidRd))
                        {
                            liMidRd.Add(mykey_MidRd);

                            DataTable dtBreakdown = DataReserve.GetRDBDivertBreakdown(di["MerchantID"].ToString(), DateTime.Parse(di["RequestDate"].ToString()));

                            foreach (DataRow drbd in dtBreakdown.Rows)
                            {
                                Dictionary<string, string> diB = new Dictionary<string, string>();

                                ws2.Cells["A" + row_index_page2.ToString()].Value = drbd["Sys"].ToString();
                                ws2.Cells["B" + row_index_page2.ToString()].Value = drbd["Prin"].ToString();
                                ws2.Cells["C" + row_index_page2.ToString()].Value = DateTime.Parse(drbd["Report Date"].ToString()).ToShortDateString();
                                ws2.Cells["D" + row_index_page2.ToString()].Value = drbd["MID"].ToString();
                                ws2.Cells["E" + row_index_page2.ToString()].Value = drbd["DBA"].ToString();
                                ws2.Cells["F" + row_index_page2.ToString()].Value = Convert.ToDecimal(drbd["Amount"]);
                                ws2.Cells["G" + row_index_page2.ToString()].Value = drbd["MEX Cat Code"].ToString();
                                ws2.Cells["H" + row_index_page2.ToString()].Value = drbd["Category"].ToString();
                                ws2.Cells["I" + row_index_page2.ToString()].Value = drbd["Dep Type"].ToString();
                                ws2.Cells["J" + row_index_page2.ToString()].Value = drbd["Batch No"].ToString();
                                ws2.Cells["L" + row_index_page2.ToString()].Value = "0";
                                ws2.Cells["M" + row_index_page2.ToString()].Value = Convert.ToDecimal(di["percReserveBoth"]) * Convert.ToDecimal(drbd["Amount"]);
                                ws2.Cells["N" + row_index_page2.ToString()].Value = Convert.ToDecimal(di["percDivertReject"]) * Convert.ToDecimal(drbd["Amount"]);
                                ws2.Cells["O" + row_index_page2.ToString()].Value = Convert.ToDecimal(di["percPostMerchant"]) * Convert.ToDecimal(drbd["Amount"]);

                                row_index_page2++;

                            }

                            row_index_page2++;
                        }



                    }

                    int li_count_min_1 = (liFinal.Count - 1);

                    // to reserve
                    string cell_recoverlastrow = "G" + (4 + li_count_min_1);
                    string cell_recovertotal = "G" + (4 + li_count_min_1 + 3).ToString();
                    ws.Cells[cell_recovertotal].Formula = "=SUM(G4:" + cell_recoverlastrow + ")";

                    // to operational
                    string cell_releaselastrow = "H" + (4 + li_count_min_1);
                    string cell_releasetotal = "H" + (4 + li_count_min_1 + 7).ToString();
                    ws.Cells[cell_releasetotal].Formula = "=SUM(H4:" + cell_releaselastrow + ")";

                    // to merchant
                    string cell_merchantlastrow = "I" + (4 + li_count_min_1);
                    string cell_merchanttotal = "I" + (4 + li_count_min_1 + 10).ToString();
                    ws.Cells[cell_merchanttotal].Formula = "=SUM(I4:" + cell_merchantlastrow + ")";


                    ///////////////////////////////////////////////////


                    string bd_last_row = (row_index_page2 - 1).ToString();
                    string bd_total_row = row_index_page2.ToString();

                    ws2.Cells[string.Format("F{0}", (bd_total_row))].Formula = string.Format("=SUM(F2:F{0})", (bd_last_row));
                    ws2.Cells[string.Format("L{0}", (bd_total_row))].Formula = string.Format("=SUM(L2:L{0})", (bd_last_row));
                    ws2.Cells[string.Format("M{0}", (bd_total_row))].Formula = string.Format("=SUM(M2:M{0})", (bd_last_row));
                    ws2.Cells[string.Format("N{0}", (bd_total_row))].Formula = string.Format("=SUM(N2:N{0})", (bd_last_row));
                    ws2.Cells[string.Format("O{0}", (bd_total_row))].Formula = string.Format("=SUM(O2:O{0})", (bd_last_row));

                    ws2.Cells[string.Format("A{0}:O{0}", bd_total_row)].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;
                    ws2.Cells[string.Format("A{0}:O{0}", bd_total_row)].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Double;

                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);

                }
            }

        }

        // Obsolete: We don't need anymore
        //public void BuildExcel_Divert_Woodforest_Credit(decimal amount, MerchantApp objMA, string tempdir, eDivertSource eDS, DateTime report_date)
        //{
        //    if (amount != 0 && objMA != null)
        //    {
        //        if (objMA.ContactList == null)
        //        {
        //            objMA.ContactList = DataContact.SearchContact(Convert.ToInt32(objMA.ID), eControlContactType.Merchant);
        //        }

        //        FileInfo newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + DIVERT_WOODFOREST_CREDIT_TEMPLATE);
        //        string filename = "";

        //        if (eDS == eDivertSource.Reserve)
        //        {
        //            filename = String.Format("Reserve & Divert_WNB_{0}-{1}-{2}_ACH Credit Request for MID # {3} (Reserve).xlsx",
        //                 DateTime.Now.ToString("MM")
        //                , DateTime.Now.ToString("dd")
        //                , DateTime.Now.ToString("yy")
        //                 , objMA.SettlePlatformMid
        //                 );

        //            if (newFile != null)
        //            {
        //                using (ExcelPackage pck = new ExcelPackage(newFile))
        //                {
        //                    var ws = pck.Workbook.Worksheets[1];

        //                    // todo: change the date to use the date the divert was confirmed.
        //                    ws.Cells["A6"].Value = "Date: " + DateTime.Now.ToShortDateString();  // Date:
        //                    ws.Cells["A11"].Value = "Merchant Name:" + objMA.BusinessDBAName; // Merchant Name:
        //                    ws.Cells["A13"].Value = "MID: " + objMA.SettlePlatformMid; // MID
        //                    ws.Cells["A16"].Value = string.Format("DDA: {0} (Meritus Reserve Account)", MERITUS_RESERVE_ACCOUNT); // DDA
        //                    ws.Cells["B16"].Value = "TRANS ROUTING: " + MERITUS_RESERVE_ROUTING; // TRANS ROUTING
        //                    ws.Cells["A18"].Value = "AMOUNT TO CREDIT: " + string.Format("{0:C2}", amount); // AMOUNT TO CREDIT: 
        //                    ws.Cells["A22"].Value = "REASON FOR ACH: MD-050 Divert Activity for " + report_date.ToShortDateString();

        //                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //                }
        //            }
        //        }
        //        else if (eDS == eDivertSource.Meritus)
        //        {
        //            filename = String.Format("Reserve & Divert_WNB_{0}-{1}-{2}_ACH Credit Request for MID # {3} (Meritus).xlsx",
        //                 DateTime.Now.ToString("MM")
        //                , DateTime.Now.ToString("dd")
        //                , DateTime.Now.ToString("yy")
        //                 , objMA.SettlePlatformMid
        //                 );

        //            if (newFile != null)
        //            {
        //                using (ExcelPackage pck = new ExcelPackage(newFile))
        //                {
        //                    var ws = pck.Workbook.Worksheets[1];

        //                    // todo: change the date to use the date the divert was confirmed.
        //                    ws.Cells["A6"].Value = "Date: " + DateTime.Now.ToShortDateString();  // Date:
        //                    ws.Cells["A11"].Value = "Merchant Name:" + objMA.BusinessDBAName; // Merchant Name:
        //                    ws.Cells["A13"].Value = "MID: " + objMA.SettlePlatformMid; // MID
        //                    ws.Cells["A16"].Value = string.Format("DDA: {0} (Meritus Operational Account)", MERITUS_OPERATIONAL_ACCOUNT); // DDA
        //                    ws.Cells["B16"].Value = "TRANS ROUTING: " + MERITUS_OPERATIONAL_ROUTING; // TRANS ROUTING
        //                    ws.Cells["A18"].Value = "AMOUNT TO CREDIT: " + string.Format("{0:C2}", amount); // AMOUNT TO CREDIT: 
        //                    ws.Cells["A22"].Value = "REASON FOR ACH: MD-050 Divert Activity for " + report_date.ToShortDateString();

        //                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //                }
        //            }

        //        }
        //        else if (eDS == eDivertSource.Merchant)
        //        {
        //            filename = String.Format("Reserve & Divert_WNB_{0}-{1}-{2}_ACH Credit Request for MID # {3} (Merchant).xlsx",
        //                 DateTime.Now.ToString("MM")
        //                , DateTime.Now.ToString("dd")
        //                , DateTime.Now.ToString("yy")
        //                 , objMA.SettlePlatformMid
        //                 );

        //            if (newFile != null)
        //            {
        //                using (ExcelPackage pck = new ExcelPackage(newFile))
        //                {
        //                    var ws = pck.Workbook.Worksheets[1];

        //                    ws.Cells["A6"].Value = "Date: " + DateTime.Now.ToShortDateString();  // Date:
        //                    ws.Cells["A11"].Value = "Merchant Name:" + objMA.BusinessDBAName; // Merchant Name:
        //                    ws.Cells["A13"].Value = "MID: " + objMA.SettlePlatformMid; // MID
        //                    ws.Cells["A16"].Value = string.Format("DDA: {0}", objMA.AccountNumber); // DDA
        //                    ws.Cells["B16"].Value = "TRANS ROUTING: " + objMA.RoutingNumber; // TRANS ROUTING
        //                    ws.Cells["A18"].Value = "AMOUNT TO CREDIT: " + string.Format("{0:C2}", amount); // AMOUNT TO CREDIT: 
        //                    ws.Cells["A22"].Value = "REASON FOR ACH: MD-050 Divert Activity for " + report_date.ToShortDateString();

        //                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //                }
        //            }

        //        }
        //    }

        //}



        // Obsolete: We don't need anymore!!
        //public void BuildExcel_Divert_Woodforest_Debit(decimal amount, MerchantApp objMA, string tempdir, eDivertSource eDS, DateTime report_date)
        //{
        //    if (amount != 0 && objMA != null)
        //    {
        //        if (objMA.ContactList == null)
        //        {
        //            objMA.ContactList = DataContact.SearchContact(Convert.ToInt32(objMA.ID), eControlContactType.Merchant);
        //        }

        //        FileInfo newFile = new FileInfo(DIRECTORY_RESERVE_FORMS + DIVERT_WOODFOREST_DEBIT_TEMPLATE);

        //        string filename = "";


        //        if (eDS == eDivertSource.Reserve)
        //        {
        //            filename = String.Format("Reserve & Divert_WNB_{0}-{1}-{2}_ACH Debit Request for MID # {3} (Reserve).xlsx",
        //               DateTime.Now.ToString("MM")
        //               , DateTime.Now.ToString("dd")
        //               , DateTime.Now.ToString("yy")
        //               , objMA.SettlePlatformMid
        //               );

        //            if (newFile != null)
        //            {
        //                using (ExcelPackage pck = new ExcelPackage(newFile))
        //                {
        //                    var ws = pck.Workbook.Worksheets[1];

        //                    // todo: change the date to use the date the divert was confirmed.
        //                    ws.Cells["A6"].Value = "Date: " + DateTime.Now.ToShortDateString();  // Date:
        //                    ws.Cells["A11"].Value = "Merchant Name:" + objMA.BusinessDBAName; // Merchant Name:
        //                    ws.Cells["A13"].Value = "MID: " + objMA.SettlePlatformMid; // MID
        //                    ws.Cells["A16"].Value = string.Format("DDA: {0} (Meritus Reserve Account)", MERITUS_RESERVE_ACCOUNT); // DDA
        //                    ws.Cells["B16"].Value = "TRANS ROUTING: " + MERITUS_RESERVE_ROUTING; // TRANS ROUTING
        //                    ws.Cells["A18"].Value = "AMOUNT TO DEBIT: " + string.Format("{0:C2}", amount); // AMOUNT TO DEBIT: 
        //                    ws.Cells["A22"].Value = "REASON FOR ACH: MD-050 Divert Activity for " + report_date.ToShortDateString();

        //                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //                }
        //            }
        //        }
        //        else if (eDS == eDivertSource.Meritus)
        //        {
        //            filename = String.Format("Reserve & Divert_WNB_{0}-{1}-{2}_ACH Debit Request for MID # {3} (Meritus).xlsx",
        //                DateTime.Now.ToString("MM")
        //                , DateTime.Now.ToString("dd")
        //                , DateTime.Now.ToString("yy")
        //                , objMA.SettlePlatformMid
        //                );

        //            if (newFile != null)
        //            {
        //                using (ExcelPackage pck = new ExcelPackage(newFile))
        //                {
        //                    var ws = pck.Workbook.Worksheets[1];

        //                    // todo: change the date to use the date the divert was confirmed.
        //                    ws.Cells["A6"].Value = "Date: " + DateTime.Now.ToShortDateString();  // Date:
        //                    ws.Cells["A11"].Value = "Merchant Name:" + objMA.BusinessDBAName; // Merchant Name:
        //                    ws.Cells["A13"].Value = "MID: " + objMA.SettlePlatformMid; // MID
        //                    ws.Cells["A16"].Value = string.Format("DDA: {0} (Meritus Operational Account)", MERITUS_OPERATIONAL_ACCOUNT); // DDA
        //                    ws.Cells["B16"].Value = "TRANS ROUTING: " + MERITUS_OPERATIONAL_ROUTING; // TRANS ROUTING
        //                    ws.Cells["A18"].Value = "AMOUNT TO DEBIT: " + string.Format("{0:C2}", amount); // AMOUNT TO DEBIT: 
        //                    ws.Cells["A22"].Value = "REASON FOR ACH: MD-050 Divert Activity for " + report_date.ToShortDateString();

        //                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //                }
        //            }
        //        }
        //        else if (eDS == eDivertSource.Merchant)
        //        {

        //            filename = String.Format("Reserve & Divert_WNB_{0}-{1}-{2}_ACH Debit Request for MID # {3} (Merchant).xlsx",
        //                DateTime.Now.ToString("MM")
        //                , DateTime.Now.ToString("dd")
        //                , DateTime.Now.ToString("yy")
        //                , objMA.SettlePlatformMid
        //                );

        //            if (newFile != null)
        //            {
        //                using (ExcelPackage pck = new ExcelPackage(newFile))
        //                {
        //                    var ws = pck.Workbook.Worksheets[1];

        //                    // todo: change the date to use the date the divert was confirmed.
        //                    ws.Cells["A6"].Value = "Date: " + DateTime.Now.ToShortDateString();  // Date:
        //                    ws.Cells["A11"].Value = "Merchant Name:" + objMA.BusinessDBAName; // Merchant Name:
        //                    ws.Cells["A13"].Value = "MID: " + objMA.SettlePlatformMid; // MID
        //                    ws.Cells["A16"].Value = string.Format("DDA: {0}", objMA.AccountNumber); // DDA
        //                    ws.Cells["B16"].Value = "TRANS ROUTING: " + objMA.RoutingNumber; // TRANS ROUTING
        //                    ws.Cells["A18"].Value = "AMOUNT TO DEBIT: " + string.Format("{0:C2}", amount); // AMOUNT TO DEBIT: 
        //                    ws.Cells["A22"].Value = "REASON FOR ACH: MD-050 Divert Activity for " + report_date.ToShortDateString();

        //                    CommonUtility.FileHandling.WriteToDisk(tempdir, pck.GetAsByteArray(), filename, false);
        //                }
        //            }

        //        }

        //    }
        //}

        #endregion

        protected void btnReCalc_Click(object sender, EventArgs e)
        {
            DataReserve.UpdateRDBReserveAllocation();
            wucMessage1.AddMessageStatus("Reserve Re-Calculation Complete!");
            Response.Redirect(WebUtil.GetMyUrl());
        }

        protected void chkShowHistory_CheckedChanged(object sender, EventArgs e)
        {
            QAReportDate.Visible = chkShowHistory.Checked;
            btnQASearch.Visible = chkShowHistory.Checked;

            if (chkShowHistory.Checked && QAReportDate.Text == string.Empty)
            {
                QAReportDate.Value = DateTime.Today.AddDays(-1);
            }

            RefreshView();
        }

        private void RefreshView()
        {
            if (chkShowHistory.Checked)
            {
                wucReserveGrid1.QAReportDate = QAReportDate.Text;
                wucDivertGrid1.QAReportDate = QAReportDate.Text;
                wucReleaseGrid1.QAReportDate = QAReportDate.Text;
                this.RefreshReleaseBatch(DateTime.Parse(QAReportDate.Text));
                this.RefreshDivertBatch(DateTime.Parse(QAReportDate.Text));
            }
            else
            {
                wucDivertGrid1.QAReportDate = string.Empty;
                wucReleaseGrid1.QAReportDate = string.Empty;
                wucReserveGrid1.QAReportDate = string.Empty;
                this.RefreshReleaseBatch(DateTime.Now);
                this.RefreshDivertBatch(DateTime.Now);
            }

            wucReserveGrid1.ViewPendingRecords = !chkShowHistory.Checked;
            wucDivertGrid1.ViewPendingRecords = !chkShowHistory.Checked;
            wucReleaseGrid1.ViewPendingRecords = !chkShowHistory.Checked;

            wucReserveGrid1.ShowCheckbox = !chkShowHistory.Checked;
            wucDivertGrid1.ShowCheckbox = !chkShowHistory.Checked;
            wucReleaseGrid1.ShowCheckbox = !chkShowHistory.Checked;

            wucDivertGrid1.BindGrid();
            wucReleaseGrid1.BindGrid();
            wucReserveGrid1.BindGrid();

            btnReCalc.Visible = !chkShowHistory.Checked;
            Button1.Visible = !chkShowHistory.Checked;
            btnConfirmDivert.Visible = !chkShowHistory.Checked;
            btnReleaseFinal.Visible = !chkShowHistory.Checked;
        }

        protected void RefreshReleaseBatch(DateTime dateTime)
        {
            grdRelDocs.DataSource = DataReserve.GetReleaseBatch(dateTime);
            grdRelDocs.DataBind();
        }

        protected void RefreshDivertBatch(DateTime dateTime)
        {
            grdDivDocs.DataSource = DataReserve.GetDivertBatch(dateTime);
            grdDivDocs.DataBind();
        }

        void lb_Command(object sender, CommandEventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            int batch_release_id = Convert.ToInt32(lb.CommandArgument);

            string command_name = lb.CommandName;

        }

        private Dictionary<int, List<RDBRelease>> GroupReleaseByBatchID(List<RDBRelease> list)
        {

            Dictionary<int, List<RDBRelease>> di = new Dictionary<int, List<RDBRelease>>();

            if (list != null && list.Count > 0)
            {

                foreach (RDBRelease item in list)
                {
                    if (!di.ContainsKey(item.ReleaseBatchID))
                    {
                        di.Add(item.ReleaseBatchID, new List<RDBRelease>());
                    }

                    di[item.ReleaseBatchID].Add(item);
                }

            }

            return di;

        }

        //protected void QAReportDate_ValueChanged(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
        //{
        //    this.RefreshView();
        //}

        protected void grdRelDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "BatchFiles":

                    int release_batch_id = Convert.ToInt32(e.CommandArgument);

                    Hashtable prms = new Hashtable();
                    prms.Add("@ReleaseBatchID", release_batch_id);
                    List<RDBRelease> li = DataReserve.GetRDBRelease(prms);

                    if (li != null && li.Count > 0)
                    {

                        string tempdir = string.Format(@"c:\temp\Reserve\{0}\", Guid.NewGuid().ToString());
                        string newfile = CommonUtility.Util.GetDateTimeStamp() + ".zip";

                        // convert to a dictionary
                        Dictionary<int, RDBRelease> di = new Dictionary<int, RDBRelease>();
                        foreach (RDBRelease item in li)
                        {
                            di.Add(item.ReleaseID, item);
                        }

                        byte[] b = this.GetReleaseZip(tempdir, newfile, ref di);

                        if (b != null)
                        {
                            // now that we have the binary, delete everything in that temp dir
                            Directory.Delete(tempdir, true);

                            //Write it back to the client
                            Response.Clear();   // necessary!!!
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "attachment;  filename=" + newfile);
                            Response.BinaryWrite(b);
                            Response.End();

                        }
                    }

                    break;

            }
        }

        protected void grdDivDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "BatchFiles":


                    // Knowledge: woodforest needs the excel breakout. wells does not need it.

                    int batchid = Convert.ToInt32(e.CommandArgument);

                    this.GetDivertZip(batchid);
                    break;
            }

        }

        protected void grdDivDocs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                decimal amountMeritus = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "DivertReject"), 0);
                decimal amountMerchant = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "PostMerchant"), 0);

                if (amountMerchant == 0 && amountMerchant == 0)
                {
                    // both records have no amount. so no money is moved. therefore we cancel the display.
                    e.Row.Visible = false;

                }
                else
                {

                    Label lrd = (Label)e.Row.FindControl("lblReportDate");

                    if (lrd != null)
                    {
                        lrd.Text = DateTime.Parse(lrd.Text).ToShortDateString();
                    }
                }
            }
        }

        protected void btnQASearch_Click(object sender, EventArgs e)
        {
            this.RefreshView();
        }
        
        private void ApplyPostRules()
        {
            //disable rows for each grid according to control flow rules. the rules go as follows:
            //allow users to post rows elements (reserve, release, divert) based on chronological
            //order and then in the following order reserve->divert->release

            //eluxa 10/03/2014: i was struggling with the algorithm to detect rows to disable for 
            //each row element in their respective grids. i really wanted to write an sp to return 
            //all three datasets with an extra column to disable checkboxes but this would require 
            //a major tear down of the current structure for the page and could take 3-4 days 
            //of development. so what i'll be doing here is applying my logic before the grids 
            //are rendered in all three user controls. if any of these three grids require paging
            //then this solution will break because this solution requires all rows be rendered.
            //this solution will also break if we change the column indices or control name
            //in the code front for any of the three grids

            //step 1: check to see if there are existing mids in the the divert table that has 
            //a report date prior to a report date for the same mid in the reserve grid. if
            //such a case exists for a mid then we'll disable the checkbox for the current
            //mid row on the reserve table
            foreach (GridViewRow resRow in this.wucReserveGrid1.ReserveGrid.Rows)
            {
                ValidateReserveRow(resRow, this.wucDivertGrid1.DivertGrid);
            }

            //step 2: check to see if there is an existing mid in the reserve grid that
            //has a report date prior to a report date for the same mid in the divert grid
            //if such a case exists disable the checkbox for the current mid row in the divert table
            foreach (GridViewRow divRow in this.wucDivertGrid1.DivertGrid.Rows)
            {
                ValidateDivertRow(divRow, this.wucReserveGrid1.ReserveGrid);
            }

            //step 3: check to see if there are any existing mids in the divert or reserve table
            //for the same mid found in the release table. if there are en existing mids in the divert
            //or reserve table then disable then checkbox for the current mid row in the release table
            foreach (GridViewRow relRow in this.wucReleaseGrid1.ReleaseGrid.Rows)
            {
                ValidateReleaseRow(relRow, this.wucReserveGrid1.ReserveGrid, this.wucDivertGrid1.DivertGrid);
            }
        }

        private void ValidateReserveRow(GridViewRow reserveRow, GridView divertGrid)
        {
            CheckBox chkBox = (CheckBox)reserveRow.Cells[0].FindControl("cbSelect");
            Label lblReportDate = (Label)reserveRow.Cells[1].FindControl("lblReportDate");
            HyperLink midLink = (HyperLink)reserveRow.Cells[3].FindControl("hypMID");

            foreach (GridViewRow divRow in divertGrid.Rows)
            {
                HyperLink divMidLink = (HyperLink)divRow.Cells[4].FindControl("hypMID");
                
                if (divMidLink.Text == midLink.Text)
                {
                    //if the report date in the divert grid is prior to the report date from
                    //the reserve row then disable the check box from the reserve row and exit
                    Label divLblReportDate = (Label)divRow.Cells[2].FindControl("lblReportDate");

                    DateTime resDate;
                    DateTime divDate;

                    if (DateTime.TryParse(lblReportDate.Text, out resDate) && DateTime.TryParse(divLblReportDate.Text, out divDate))
                    {
                        if (divDate.CompareTo(resDate) < 0)
                        {
                            //the divert report date is prior to the reserve row so we need to disable
                            //the checkbox for the reserve row because we need to process in chronological
                            //order first. this means the divert row for this mid with a prior report date
                            //needs to be posted before the reserve row with a later report date
                            chkBox.Enabled = false;
                            return;
                        }
                    }
                }
            }
        }

        private void ValidateDivertRow(GridViewRow divertRow, GridView reserveGrid)
        {
            CheckBox chkBox = (CheckBox)divertRow.Cells[0].FindControl("cbSelect");
            Label lblReportDate = (Label)divertRow.Cells[2].FindControl("lblReportDate");
            HyperLink midLink = (HyperLink)divertRow.Cells[4].FindControl("hypMID");

            foreach (GridViewRow reserveRow in reserveGrid.Rows)
            {
                HyperLink resMidLink = (HyperLink)reserveRow.Cells[3].FindControl("hypMID");

                if (resMidLink.Text == midLink.Text)
                {
                    //if the report date in the revert grid is equal to or prior to the report date from
                    //the divert row then disable the check box from the divert row and exit
                    Label resLblReportDate = (Label)reserveRow.Cells[1].FindControl("lblReportDate");

                    DateTime resDate;
                    DateTime divDate;

                    if (DateTime.TryParse(lblReportDate.Text, out divDate) && DateTime.TryParse(resLblReportDate.Text, out resDate))
                    {
                        if (resDate.CompareTo(divDate) < 1)
                        {
                            //reserve report date is equal to or prior than the report date from 
                            //the divert row so we need to disable the divert row checkbox
                            chkBox.Enabled = false;
                            return;
                        }
                    }
                }
            }

        }

        private void ValidateReleaseRow(GridViewRow releaseRow, GridView reserveGrid, GridView divertGrid)
        {
            CheckBox chkBox = (CheckBox)releaseRow.Cells[0].FindControl("cbSelect");
            HyperLink midLink = (HyperLink)releaseRow.Cells[4].FindControl("hypMID");

            foreach (GridViewRow reserveRow in reserveGrid.Rows)
            {
                HyperLink resMidLink = (HyperLink)reserveRow.Cells[3].FindControl("hypMID");

                if (resMidLink.Text == midLink.Text)
                {
                    //reserve grid contains an existing mid from the release row. we need to disable
                    //the release row checkbox since we need to process in order from 
                    //reserve -> divert -> release
                    chkBox.Enabled = false;
                    return;
                }
            }

            foreach (GridViewRow divRow in divertGrid.Rows)
            {
                HyperLink divMidLink = (HyperLink)divRow.Cells[4].FindControl("hypMID");

                if (divMidLink.Text == midLink.Text)
                {
                    //divert grid contains an existing mid from the release row. we need to disable
                    //the release row checkbox since we need to process in order from 
                    //reserve -> divert -> release
                    chkBox.Enabled = false;
                    return;
                }
            }
        }

        protected void btnOverride_Click(object sender, EventArgs e)
        {
            DateTime reportDate;
            DateTime.TryParse(this.ReportDate.Text, out reportDate);
            ValidateImport(reportDate);
        }

        private void ValidateImport(DateTime reportDate)
        {
            //run data check on daily imports for SD077, MD050 and ADR060. if there's no issues
            //with these daily imports then we can display the import buttons, otherwise we'll
            //reports errors to the user

            List<RDBImportError> errors = DataReserve.ValidateDailyImport(reportDate);


            this.SelectedReportDate = reportDate.ToString("MM/dd/yyyy");

            if (errors.Count == 0)
            {
                this.pnlImport.Visible = true;
                this.pnlImportError.Visible = false;

                this.ReportDate.Text = reportDate.ToString("MM/dd/yyyy");
                this.btnImport.Text = "Import Data for " + reportDate.ToString("MM/dd/yyyy");
                this.btnImport.Attributes.Add("onclick", "return confirm('Are you sure you want to import data for " + reportDate.ToString("MM/dd/yyyy") + "')");
            }
            else
            {
                this.pnlImport.Visible = false;
                this.pnlImportError.Visible = true;

                rReportDate.DataSource = errors;
                rReportDate.DataBind();
            }
        }

        protected void rReportDate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            BulletedList bl = (BulletedList)e.Item.FindControl("blErrorMessage");
            RDBImportError error = (RDBImportError)e.Item.DataItem;

            foreach (string errMsg in error.ErrorMessages)
            {
                bl.Items.Add(errMsg);
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            DateTime dt = CommonUtility.Util.if_date(this.SelectedReportDate, DateTime.MinValue);

            //check to see if the last report date is prior to yesterday's report before allowing 
            //the user to import yesterday's report. does vishal's sp work?

            if (dt != DateTime.MinValue)
            {
                //check vishal's sp to see if we have any issues with past imports
                List<RDBImportError> errors = DataReserve.ValidateReportImport(dt);

                if (errors.Count == 0)
                {
                    DataReserve.ImportRDB_Reserve(dt);
                    DataReserve.ImportRDB_Divert(dt);

                    Response.Redirect(WebUtil.GetMyUrl());
                }
                else
                {
                    this.pnlImport.Visible = false;
                    this.pnlImportError.Visible = true;

                    rReportDate.DataSource = errors;
                    rReportDate.DataBind();
                }
            }

            
        }
    }
}
