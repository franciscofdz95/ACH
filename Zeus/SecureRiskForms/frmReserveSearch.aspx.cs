using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using System.Data;
using System.Collections;
using PaymentXP.Facade;
using PaymentXP.BusinessObjects.Reserve;

namespace ZeusWeb
{
    public partial class frmReserveSearch : frmBaseSearch
    {
       
        public int SelectedZID
        {
            get { return (int)(ViewState["SelectedZID"] ?? 0); }
            set { ViewState["SelectedZID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

            if (!this.IsPostBack)
            {
                Search(true);
                //this.RefreshControls(this.SelectedZID);

                this.CheckRHAM(cbRHAM.Checked);
               
            }

            // events from the gridview controls. these usually raise dialogs.
            if (wucBalanceGrid1 != null)
                wucBalanceGrid1.event_click_zid += new UserControls.Reserve.wucBalanceGrid.EventClickZID(wucBalanceGrid1_event_click_zid);
            if (wucReleaseGrid1 != null)
                wucReleaseGrid1.event_click_reportdate += new UserControls.wucReleaseGrid.EventClickReportDate(wucReleaseGrid1_event_click_reportdate);
            // wucDivertGrid1.event_click_reportdate += new UserControls.wucDivertGrid.EventClickReportDate(wucDivertGrid1_event_click_reportdate);
            if (wucReserveGrid1 != null)
                wucReserveGrid1.event_click_amount += new UserControls.wucReserveGrid.EventClickAmount(wucReserveGrid1_event_click_amount);
            if (wucReserveGrid1 != null)
                wucReserveGrid1.event_click_manualentry += new UserControls.wucReserveGrid.EventClickManualEntry(wucReserveGrid1_event_click_manualentry);

            // events from the dialog boxes. most of these just refresh the screen.
            if (wucManualDialog1 != null)
                wucManualDialog1.event_click_savesuccess += new UserControls.Reserve.wucManualDialog.EventClickSaveSuccess(manual_event_click_savesuccess);
            if (wucReleaseDialog1 != null)
                wucReleaseDialog1.event_click_savesuccess += new UserControls.Reserve.wucReleaseDialog.EventClickSaveSuccess(release_event_click_savesuccess);
            if (wucTransferDialog1 != null)
                wucTransferDialog1.event_click_savesuccess += new UserControls.Reserve.wucTransferDialog.EventClickSaveSuccess(transfer_event_click_savesuccess);

          
            this.Page.PreRender += new EventHandler(Page_PreRender);
        }

        protected void manual_event_click_savesuccess(int zid)
        {
            this.RefreshControls(zid);
            tabReport.SelectedIndex = 1;
        }

        protected void release_event_click_savesuccess(int zid)
        {
            this.RefreshControls(zid);
            tabReport.SelectedIndex = 5;
        }

        protected void transfer_event_click_savesuccess(int zid)
        {
            this.RefreshControls(zid);
            tabReport.SelectedIndex = 1;
        }

        protected void wucReserveGrid1_event_click_manualentry(int reserveid, int zid)
        {
            wucManualDialog1.ZID = zid;
            wucManualDialog1.ReserveID = reserveid;
            wucManualDialog1.FormShow("");
            wucManualDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        protected void wucReserveGrid1_event_click_amount(int zid, string reportdate)
        {
            wucReserveBatchDetailsDialog1.ZID = zid;
            wucReserveBatchDetailsDialog1.ReportDate = DateTime.Parse(reportdate);
            wucReserveBatchDetailsDialog1.FormShow("");
            wucReserveBatchDetailsDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {


        }

        //protected void wucDivertGrid1_event_click_reportdate(DateTime DivertDate, int zid)
        //{
        //    wucDivertDialog1.ZID = zid;
        //    wucDivertDialog1.DivertDate = DivertDate;
        //    wucDivertDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        //    wucDivertDialog1.FormShow("");
        //}

        protected void wucReleaseGrid1_event_click_reportdate(int releaseid, int zid)
        {
            wucReleaseDialog1.ZID = zid;
            wucReleaseDialog1.ReleaseID = releaseid;
            wucReleaseDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
            wucReleaseDialog1.FormShow("");
        }

        public override void Search(bool IsOnLoad)
        {
            Hashtable prms = new Hashtable();

            if (IsOnLoad)
            {
                if (this.SearchParameters != null)
                {
                    SearchParameter app = (SearchParameter)this.SearchParameters;
                    FormBinding.BindObjectToControls(app, pnlSearch);
                }

                // override what's bound
                if ((Request.QueryString["MID"] ?? "") != "")
                {
                    MerchantID.Text = "";
                    SettlePlatformUID.Text = Request.QueryString["MID"].Trim();
                    BusinessDBAName.Text = "";
                    BusinessLegalName.Text = "";
                }
            }


            if (!string.IsNullOrEmpty(MerchantID.Text))
                prms.Add("@ZID", MerchantID.Text.Trim());

            if (!string.IsNullOrEmpty(SettlePlatformUID.Text))
                prms.Add("@MID", SettlePlatformUID.Text.Trim());

            if (!string.IsNullOrEmpty(BusinessDBAName.Text))
                prms.Add("@DBAName", BusinessDBAName.Text.Trim());

            if (!string.IsNullOrEmpty(BusinessLegalName.Text))
                prms.Add("@LegalName", BusinessLegalName.Text.Trim());

            if (prms.Count > 0)
            {
                int single_zid = wucBalanceGrid1.SetDataSource(prms);

                if (single_zid > 0)
                {
                    this.wucBalanceGrid1_event_click_zid(single_zid);
                }

            }


            SearchParameter app1 = new SearchParameter();
            FormBinding.BindControlsToObject(app1, pnlSearch);
            this.SearchParameters = app1;

        }

        protected void wucBalanceGrid1_event_click_zid(int zid)
        {
            this.RefreshControls(zid);
            this.SelectedZID = zid;

            Hashtable prms = new Hashtable();
            prms.Add("@ID", zid);
            MerchantApp agreement = DataMerchantApp.GetInstance().FillMerchantApp(prms);

            FormBinding.BindObjectToControls(agreement, wucRDBBusinessInfo1);



        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search(false);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            MerchantID.Text = "";
            SettlePlatformUID.Text = "";
            BusinessDBAName.Text = "";
            BusinessLegalName.Text = "";

            if (this.SearchParameters != null)
            {
                ((SearchParameter)this.SearchParameters).MerchantID = "";
                ((SearchParameter)this.SearchParameters).SettlePlatformUID = "";
                ((SearchParameter)this.SearchParameters).BusinessDBAName = "";
                ((SearchParameter)this.SearchParameters).BusinessLegalName = "";
            }

            this.SelectedZID = 0;

            wucBalanceGrid1.SetDataSource(null);

            this.RefreshControls(this.SelectedZID);
        }



        protected void RefreshControls(int zid)
        {

            if (zid > 0)
            {
                pnlMerchant.Visible = true;

                Hashtable prms = new Hashtable();
                prms.Add("@ID", zid);
                MerchantApp agreement = DataMerchantApp.GetInstance().FillMerchantApp(prms);

                if (agreement != null)
                {
                    pnlOmaha.Visible = (agreement.SettlePlatformUID.ToUpper() != "C044948E-DC40-47F1-BDDC-E88ADDEDF2BF"); // omaha
                    pnlRDBButtons.Visible = !pnlOmaha.Visible; // only omaha can get these buttons
                }

                FormBinding.BindObjectToControls(agreement, wucRDBBusinessInfo1);

                wucRDBBusinessInfo1.ZID = zid;
                wucSummaryGrid1.ZID = zid;


                List<RDBSummary> liSum = wucSummaryGrid1.SetDataSource(new Hashtable()); // zid will be passed if no params sent.

                if (liSum != null && liSum.Count == 1)
                {
                    btnDivertToReserve.Enabled = (liSum[0].Divert_Net > 0);
                }
                else
                {
                    btnDivertToReserve.Enabled = false;
                }

                wucStatementGrid1.ZID = zid;
                wucStatementGrid1.HasPending = wucSummaryGrid1.HasPending;
                wucStatementGrid1.BindGrid();

                wucReserveGrid1.ZID = zid;
                wucReserveGrid1.BindGrid();

                //wucDivertGrid1.ZID = zid; // divert
                //wucDivertGrid1.BindGrid();
                //wucDivertDialog1.ZID = zid;

                wucDivertDetailGrid1.ZID = zid;
                wucDivertDetailGrid1.BindGrid();

                wucReleaseGrid1.ZID = zid;
                wucReleaseGrid1.BindGrid();
                wucReleaseDialog1.ZID = zid;

                wucManualDialog1.ZID = zid;
                wucTransferDialog1.ZID = zid;


                wucRejectGrid1.ZID = zid;
                wucRejectGrid1.BindGrid();

                // we don't want items to be release able if their are pending QA items for this merchant.

                bool has_qa_items = this.HasQAItems(zid);
                btnAddRelease.Enabled = !has_qa_items;
                btnDivertToReserve.Enabled = !has_qa_items;

                //access to Add Manual Reserve button uses form object permissions
                //configure white listed users in the Admin page
                btnAddManual.Enabled = btnAddManual.Enabled && !has_qa_items;
            }
            else
            {
                pnlMerchant.Visible = false;
            }

        }

        protected void btnAddManual_Click(object sender, EventArgs e)
        {
            wucManualDialog1.FormNew();
            wucManualDialog1.ZID = this.SelectedZID;
            wucManualDialog1.FormShow("");
            wucManualDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;





        }

        protected void btnAddRelease_Click(object sender, EventArgs e)
        {
            wucReleaseDialog1.FormNew();
            wucReleaseDialog1.ZID = this.SelectedZID;
            wucReleaseDialog1.FormShow("");
            wucReleaseDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;


        }

        //protected void btnAddMD050_Click(object sender, EventArgs e)
        //{
        //    //wucDivertDialog1.FormNew();
        //    //wucDivertDialog1.ZID = this.SelectedZID;
        //    //wucDivertDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        //}


        protected void btnDRTransfer_Click(object sender, EventArgs e)
        {
            wucTransferDialog1.FormNew();
            wucTransferDialog1.ZID = this.SelectedZID;
            wucTransferDialog1.FormShow("");
            wucTransferDialog1.WinInstance.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshControls(this.SelectedZID);
        }

        /// <summary>
        /// if this zid has anything in the queue, then return TRUE
        /// </summary>
        /// <param name="zid"></param>
        /// <returns></returns>
        protected bool HasQAItems(int zid)
        {
            bool ret = false;
            Hashtable prms = new Hashtable();
            prms.Add("@ZID", zid);
            prms.Add("@ViewPendingRecords", true);
            
            if (ret == false)
            {
                List<PaymentXP.BusinessObjects.Reserve.RDBReserve> li = DataReserve.GetRDBReserve(prms);

                if (li != null && li.Count > 0)
                {
                    ret = true;
                }
            }

            if (ret == false)
            {
                DataSet ds = DataReserve.GetRDBDivertSummary(prms);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ret = true;
                }
            }

            //if (ret == false)
            //{
            //    List<RDBRelease> li = DataReserve.GetRDBRelease(prms);

            //    if (li != null && li.Count > 0)
            //    {
            //        ret = true;
            //    }
            //}

            return ret;
        }

        protected void cbRHAM_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckRHAM(cbRHAM.Checked);
        }

        private void CheckRHAM(bool includeRHAM)
        {
            wucSummaryGrid1.IncludeReservesHeldAtMeritus = includeRHAM;
            wucStatementGrid1.IncludeReservesHeldAtMeritus = includeRHAM;
            wucReserveGrid1.IncludeReservesHeldAtMeritus = includeRHAM;
            wucReleaseGrid1.IncludeReservesHeldAtMeritus = includeRHAM;

            this.RefreshControls(this.SelectedZID);
        }
    
    
    }
}