using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.BusinessObjects;
using OfficeOpenXml;

namespace ZeusWeb.UserControls
{
    public partial class wucReleaseGrid : wucBaseSearch
    {
        public delegate void EventClickReportDate(int releaseid, int zid);
        public event EventClickReportDate event_click_reportdate;

        public delegate void EventClickExport(List<RDBRelease> li, int zid);
        public event EventClickExport event_click_export;

        public bool EnablePost
        {
            get { return (bool)(ViewState["EnablePost"] ?? true); }
            set { ViewState["EnablePost"] = value; }
        }


        #region sorting_stuff
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
        public SortDirection VsTransType
        {
            get
            {
                if (ViewState["VsTransType"] == null)
                {
                    ViewState["VsTransType"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsTransType"];
            }
            set { ViewState["VsTransType"] = value; }
        }
        public SortDirection VsReserveType
        {
            get
            {
                if (ViewState["VsReserveType"] == null)
                {
                    ViewState["VsReserveType"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsReserveType"];
            }
            set { ViewState["VsReserveType"] = value; }
        }
        public SortDirection VsMethod
        {
            get
            {
                if (ViewState["VsMethod"] == null)
                {
                    ViewState["VsMethod"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsMethod"];
            }
            set { ViewState["VsMethod"] = value; }
        }
        public SortDirection VsBank
        {
            get
            {
                if (ViewState["VsBank"] == null)
                {
                    ViewState["VsBank"] = SortDirection.Ascending;
                }

                return (SortDirection)ViewState["VsBank"];
            }
            set { ViewState["VsBank"] = value; }
        }
        #endregion

        private bool _ViewPendingRecords = false;

        public bool ViewPendingRecords
        {
            get { return _ViewPendingRecords; }
            set { _ViewPendingRecords = value; }
        }

        public bool IsApproveVisible
        {
            get
            {
                if (ViewState["_IsApproveVisible"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["_IsApproveVisible"];
                }
            }
            set { ViewState["_IsApproveVisible"] = value; }
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

        public GridView ReleaseGrid
        {
            get
            {
                return this.grdRelease;
            }
        }

        public const int COLUMN_CHECKBOX = 0;
        public const int COLUMN_ZID = 3;
        public const int COLUMN_MID = 4;
        public const int COLUMN_DBA = 5;

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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                User user = UserSessions.CurrentUser;

                UserForm frm = null;

                if (user.UserForms.TryGetValue("FRMRESERVEQUEUE", out frm) && frm.HasAccess)
                {
                    if (frm.ControlObjects == null)
                        DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

                    foreach (ControlObject obj in frm.ControlObjects)
                    {
                        if (obj.Type.ToUpper() == "BUTTON" && obj.ID.ToUpper() == "APPROVEBUTTON")
                            IsApproveVisible = obj.IsVisible && obj.IsEnabled;
                    }
                }

                this.BindGrid();
            }


            this.PreRender += new EventHandler(wucReleaseGrid_PreRender);

            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExpExcel);

        }

        public void BindGrid()
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

            if (prms.Count == 0)
                prms.Add("@ZID", -1);

            prms.Add("@ApproverUserID", Convert.ToInt32(UserSessions.CurrentUser.UserID));
            //prms.Add("@ApproverUserID", 78); //jeff

            if (this.IncludeReservesHeldAtMeritus)
            {
                prms.Add("@IncludeReservesHeldAtMeritus", this.IncludeReservesHeldAtMeritus);
            }


            this.m_Prms = prms;

            List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

            if (li == null)
            {
                li = new List<RDBRelease>();
            }

            grdRelease.DataSource = li;
            grdRelease.DataBind();

        }

        protected void grdRelease_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == "ReportDate")
            {
                List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                if (this.VsReportDate == SortDirection.Ascending)
                {
                    grdRelease.DataSource = li.OrderByDescending(x => x.ReportDate);
                    grdRelease.DataBind();
                    this.VsReportDate = SortDirection.Descending;
                }
                else
                {
                    grdRelease.DataSource = li.OrderBy(x => x.ReportDate);
                    grdRelease.DataBind();
                    this.VsReportDate = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Amount")
            {
                List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                if (this.VsAmount == SortDirection.Ascending)
                {
                    grdRelease.DataSource = li.OrderByDescending(x => x.Amount);
                    grdRelease.DataBind();
                    this.VsAmount = SortDirection.Descending;
                }
                else
                {
                    grdRelease.DataSource = li.OrderBy(x => x.Amount);
                    grdRelease.DataBind();
                    this.VsAmount = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "TransType")
            {
                List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                if (this.VsTransType == SortDirection.Ascending)
                {
                    grdRelease.DataSource = li.OrderByDescending(x => x.TransType);
                    grdRelease.DataBind();
                    this.VsTransType = SortDirection.Descending;
                }
                else
                {
                    grdRelease.DataSource = li.OrderBy(x => x.TransType);
                    grdRelease.DataBind();
                    this.VsTransType = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "ReserveType")
            {
                List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                if (this.VsReserveType == SortDirection.Ascending)
                {
                    grdRelease.DataSource = li.OrderByDescending(x => x.ReserveType);
                    grdRelease.DataBind();
                    this.VsReserveType = SortDirection.Descending;
                }
                else
                {
                    grdRelease.DataSource = li.OrderBy(x => x.ReserveType);
                    grdRelease.DataBind();
                    this.VsReserveType = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Method")
            {
                List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                if (this.VsMethod == SortDirection.Ascending)
                {
                    grdRelease.DataSource = li.OrderByDescending(x => x.Method);
                    grdRelease.DataBind();
                    this.VsMethod = SortDirection.Descending;
                }
                else
                {
                    grdRelease.DataSource = li.OrderBy(x => x.Method);
                    grdRelease.DataBind();
                    this.VsMethod = SortDirection.Ascending;
                }
            }
            else if (e.SortExpression == "Bank")
            {
                List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                if (this.VsBank == SortDirection.Ascending)
                {
                    grdRelease.DataSource = li.OrderByDescending(x => x.Bank);
                    grdRelease.DataBind();
                    this.VsBank = SortDirection.Descending;
                }
                else
                {
                    grdRelease.DataSource = li.OrderBy(x => x.Bank);
                    grdRelease.DataBind();
                    this.VsBank = SortDirection.Ascending;
                }
            }
        }

        protected void grdRelease_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RDBRelease objRR = (RDBRelease)e.Row.DataItem;

                Literal litS = (Literal)e.Row.FindControl("litStatus");
                litS.Text = (objRR.JournalID > 0) ? "Confirmed" : "Pending";

                // hide edit ability if it's already in the journal.
                LinkButton lb = (LinkButton)e.Row.FindControl("lnkPostedDate");

                lb.Visible = true;

                if (objRR.PostedDate != DateTime.MinValue)
                {
                    lb.Visible = false;
                }
                else if (objRR.VoidReleaseID > 0)
                {
                    lb.Visible = false;
                }
                else if (objRR.TransTypeID == eRDBTransactionType.Void ||
                    objRR.TransTypeID == eRDBTransactionType.VoidMeritusOps ||
                    objRR.TransTypeID == eRDBTransactionType.VoidGovernment ||
                    objRR.TransTypeID == eRDBTransactionType.VoidReceiver ||
                    objRR.TransTypeID == eRDBTransactionType.VoidCollectReserve
                    ) // void
                {
                    lb.Visible = false;
                }
                else
                {
                    lb.Visible = true;
                    lb.Text = "Pending";
                }

                if (this.IsQueue == false)
                {
                    // if we're not in the queue, that means we're on the search page, so we always display this button.
                    lb.Visible = true;

                    if (objRR.TransTypeID == eRDBTransactionType.Void                       // void trans
                            || objRR.TransTypeID == eRDBTransactionType.VoidMeritusOps      // Void Transaction (Pay to Meritus)
                            || objRR.TransTypeID == eRDBTransactionType.VoidGovernment      // Void Transaction (Pay to Government)
                            || objRR.TransTypeID == eRDBTransactionType.VoidReceiver        // Void Transaction (Pay to Receiver)
                            || objRR.TransTypeID == eRDBTransactionType.VoidCollectReserve
                            || objRR.VoidReleaseID > 0                                      // voided record
                        )
                    {
                        lb.Visible = false;
                    }

                }


                // only show the "Approve" button if the logged in user is the person who can approve it.
               // LinkButton lbAppr = (LinkButton)e.Row.FindControl("lbApprove");

                //lbAppr.Visible = (objRR.CanApprove && objRR.DateApproved == DateTime.MinValue);

                // hack for stol/eddie. useful for testing. remove when going to production.
                //if (UserSessions.CurrentUser.UserName.ToLower().Trim().Equals("brainwater") || UserSessions.CurrentUser.UserName.ToLower().Trim().Equals("plara"))
                //{
                //    lbAppr.Visible = true;
                //}

                //to remove the hardcoded values, we added the approve button permissions in to the form permissions list.
                // we control the approve button visibility using the control permissions on this form.
                //lbAppr.Visible = IsApproveVisible;

                //if (!string.IsNullOrEmpty(objRR.ApprovedBy)
                //    || objRR.TransTypeID == eRDBTransactionType.Void // Void
                //    || objRR.TransTypeID == eRDBTransactionType.VoidMeritusOps // VoidMeritusOps
                //    || objRR.TransTypeID == eRDBTransactionType.VoidGovernment // VoidGovernment
                //    || objRR.TransTypeID == eRDBTransactionType.VoidReceiver // VoidReceiver
                //    || objRR.TransTypeID == eRDBTransactionType.VoidCollectReserve
                //    )
                //{
                //    lbAppr.Visible = false;
                //}

                // once the row is approved, then enable the checkbox.
                ////CheckBox cb = (CheckBox)e.Row.FindControl("cbSelect");
                ////cb.Enabled = objRR.DateApproved != DateTime.MinValue;

            }
        }


        protected void grdRelease_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {

                LinkButton lb = (LinkButton)e.CommandSource;

                switch (lb.CommandName)
                {
                    case "PostedDate":

                        string[] arr = lb.CommandArgument.Split(new char[] { '_' });
                        int ReleaseID = CommonUtility.Util.if_i(arr[0], 0);
                        int row_zid = CommonUtility.Util.if_i(arr[1], 0);

                        if (event_click_reportdate != null)
                        {
                            event_click_reportdate(ReleaseID, row_zid);
                        }
                        
                        break;

                    case "Approve":

                        int current_userid = CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0);
                        int release_id = CommonUtility.Util.if_i(lb.CommandArgument, 0);

                        RDBRelease obj = DataReserve.GetRDBRelease(release_id, current_userid);

                        if (obj != null)
                        {
                            obj.DateApproved = DateTime.Now;
                            obj.ApprovedUserID = current_userid;
                            DataReserve.UpdateRDBRelease(obj);

                            this.BindGrid();
                        }

                        break;
                }
            }



        }

        protected void wucReleaseGrid_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>force_min();</script>", false);

            grdRelease.Columns[COLUMN_CHECKBOX].Visible = this.ShowCheckbox;
            grdRelease.Columns[COLUMN_ZID].Visible = this.ShowMerchantColumns;
            grdRelease.Columns[COLUMN_MID].Visible = this.ShowMerchantColumns;
            grdRelease.Columns[COLUMN_DBA].Visible = this.ShowMerchantColumns;

            //grdRelease.Columns[COLUMN_CHECKBOX].Visible = this.EnablePost;
        }

        /// <summary>
        /// pulls all checked items from the grid and modifies the dictioanry refernces. saves DB queries
        /// </summary>
        /// <param name="diR"></param>
        /// <param name="diMA"></param>
        public void GetChecked(ref Dictionary<int, RDBRelease> diR, ref Dictionary<int, MerchantApp> diMA)
        {
            diR = new Dictionary<int, RDBRelease>();
            diMA = new Dictionary<int, MerchantApp>();

            var checkedIDs = from GridViewRow msgRow in grdRelease.Rows
                             where ((CheckBox)msgRow.FindControl("cbSelect")).Checked
                             select Int32.Parse(grdRelease.DataKeys[msgRow.RowIndex].Value.ToString());

            List<int> liReleaseID = checkedIDs.ToList();

            foreach (int release_id in liReleaseID)
            {
                RDBRelease objRel = DataReserve.GetRDBRelease(release_id);

                if (objRel != null && !diR.ContainsKey(objRel.ReleaseID))
                {
                    diR.Add(objRel.ReleaseID, objRel);
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
            if (event_click_export != null)
            {
                event_click_export(DataReserve.GetRDBRelease(this.m_Prms), this.ZID);
            }
            else
            {
                using (ExcelPackage pck = new ExcelPackage())
                {

                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Releases");

                    List<RDBRelease> li = DataReserve.GetRDBRelease(this.m_Prms);

                    string[,] liHeaders = new string[9, 3] {
			            {"PostedDate", "Posted Date", "string"},
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

                    if (this.ZID > 0)
                    {
                        filename = String.Format("RDBReleases_ZID-{0}_{1}.xlsx", this.ZID.ToString(), CommonUtility.Util.GetDateTimeStamp());
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
        }


    }
}
