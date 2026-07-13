using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using Infragistics.Win.UltraWinGrid;

using Nmc.Ach.Dal;
using System.Globalization;
using System.Linq;

namespace AchSystem
{
    public partial class frmAchProcessEngine : Form
    {
        bool m_ProcessCompleted = true;

        public frmAchProcessEngine()
        {
            InitializeComponent();

            LookUpTableHandler.LoadBanks(cboBank);

            if (cboBank.Items.Count > 0)
                cboBank.SelectedIndex = 0;
        }

        private void btnCreateBatch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (cboBank.SelectedIndex == -1)
                return;

            Bank bank = (Bank)cboBank.SelectedItem;
            bank.ProcessCreateBatches();
            //this.cbo_SelectedIndexChanged(cboBank, new EventArgs());

            this.Cursor = Cursors.Default;
        }

        private void btnCreateJournal_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;
            bank.ProcessCreateJournals();

            this.Cursor = Cursors.Default;
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            DateTime dateSubmitted = DateTime.Now;
            string overrideDate = string.Empty;
            string format =  "MM/dd/yyyy" ;

            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            if (ConfigurationManager.AppSettings.AllKeys.Contains("OverrideDate"))
                overrideDate = ConfigurationManager.AppSettings["OverrideDate"].ToString();

            if (!string.IsNullOrEmpty(overrideDate))
            {
                DateTime.TryParseExact(overrideDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateSubmitted);
                if(dateSubmitted == DateTime.MinValue) 
                    dateSubmitted = DateTime.Now;
            }

            bank.CreateSettlementFile(dateSubmitted);

            this.Cursor = Cursors.Default;
        }

        //private void lst2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Merchant merchant = (Merchant)lst2.SelectedItem;

        //    lst3.Items.Clear();
        //    foreach (object obj in merchant.Batches)
        //    {
        //        lst3.Items.Add((Batch)obj);
        //    }

        //    if (lst3.Items.Count > 0)
        //        lst3.SelectedIndex = 0;
        //}

        //private void cbo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Bank bank = (Bank)cboBank.SelectedItem;

        //    lst2.Items.Clear();
        //    lst3.Items.Clear();
        //    foreach (object obj in bank.Merchants)
        //    {
        //        lst2.Items.Add((Merchant)obj);
        //    }

        //    if (lst2.Items.Count > 0)
        //        lst2.SelectedIndex = 0;
        //}

        private void btnBatchPreview_Click(object sender, EventArgs e)
        {
            if (cboBank.SelectedIndex == -1)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            frmReportNew frm = new frmReportNew();
            frm.ShowBatchPreview(bank.BankID);
            frm.ShowDialog();

            //frmReport frm = new frmReport();
            //frm.ShowBatchPreview(bank.BankID);
            //frm.ShowDialog();

            this.Cursor = Cursors.Default;
        }

        private void btnBatchReview_Click(object sender, EventArgs e)
        {
            if (cboBank.SelectedIndex == -1)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;


            frmReportNew frm = new frmReportNew();
            frm.ShowBatchReview(bank.BankID);
            frm.ShowDialog();


            //frmReport frm = new frmReport();
            //frm.ShowBatchReview(bank.BankID);
            //frm.ShowDialog();

            this.Cursor = Cursors.Default;
        }

        private void btnCreateResponseFiles_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;
            bank.ProcessCreateResponseFiles();
            this.Cursor = Cursors.Default;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Bank bank = (Bank)cboBank.SelectedItem;
            List<Merchant> merchants = null;


            switch (tabMain.SelectedTab.Key)
            {
                case "SelectBank":
                    if (cboBank.SelectedIndex == -1)
                    {
                        FormHandler.DispalyWarningMessage("Please select a bank.");
                        return;
                    }
                    else
                    {
                        string sql = "select count(*) from Ach_BatchDetail with (Nolock) where ";
                        sql += "PostedDate >= '" + DateTime.Today.ToString("MM/dd/yyyy") + "' ";
                        sql += "And BankID = " + bank.BankID + " ";
                        string rows = DataLayer.ExecuteScalar(sql, DataLayer.ConnectStringBuild());

                        if (int.Parse(rows) > 0)
                        {
                            if (MessageBox.Show(bank.BankName + " has already been processed for today. Do you want to continue", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }


                        }

                        m_ProcessCompleted = false;
                    }

                    break;
                case "NoLocationID":
                    if (grdNoLocationID.Rows.Count > 0)
                    {
                        FormHandler.DispalyWarningMessage("Please resolve data issue.");
                        return;
                    }

                    break;
                case "DataError":
                    if (grd.Rows.Count > 0)
                    {
                        FormHandler.DispalyWarningMessage("Please resolve data issue.");
                        return;
                    }

                    break;
                case "NOCError":
                    if (grdNOC.Rows.Count > 0)
                    {
                        FormHandler.DispalyWarningMessage("Please resolve data issue.");
                        return;
                    }

                    break;
                case "CreateBatch":
                    bank = (Bank)cboBank.SelectedItem;
                    merchants = bank.GetPendingBatchMerchants();

                    if (merchants.Count > 0)
                    {
                        FormHandler.DispalyWarningMessage("Please create batch.");
                        return;
                    }
                    break;
                case "CreateJournal":
                    bank = (Bank)cboBank.SelectedItem;
                    merchants = bank.GetPendingJournalMerchants();

                    if (merchants.Count > 0)
                    {
                        FormHandler.DispalyWarningMessage("Please create journals.");
                        return;
                    }

                    break;

            }

            if (tabMain.ActiveTab.Index < tabMain.Tabs.Count - 1)
                tabMain.SelectedTab = tabMain.Tabs[tabMain.ActiveTab.Index + 1];
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab.Index != 0)
                tabMain.SelectedTab = tabMain.Tabs[tabMain.ActiveTab.Index - 1];
        }

        private void tabMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            string message = string.Empty;

            btnFinish.Enabled = false;
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;

            Bank bank = (Bank)cboBank.SelectedItem;


            switch (e.Tab.Key)
            {

                case "SelectBank":
                    message = "Please select a bank to process.";
                    btnPrevious.Enabled = false;
                    break;
                case "DataError":
                    message = "This page displays transactions with invalid field(s). Please resolve the issue(s) before moving on to the next step for " + cboBank.Text + " bank.";
                    this.LoadBadData();
                    break;
                case "NOCError":
                    message = "There are Notice of Changes(NOC) associated with these transactions. Please resolve the issue(s) before moving on to the next step for " + cboBank.Text + " bank.";
                    this.LoadNOCBadData();
                    break;
                case "OverTicket":
                    message = "This page displays all over ticket transactions for " + cboBank.Text + " bank.";
                    this.LoadOverTicketItems();
                    break;
                case "TransOnHold":
                    message = "This page displays all transactions on hold for " + cboBank.Text + " bank.";
                    this.LoadTransOnHold();
                    break;
                case "OverVolume":
                    message = "This page displays all merchants that processed over the volume limit.";
                    this.LoadOverVolumeMerchants();
                    break;
                case "DuplicateTransactions":
                    message = "This page displays all over duplicate transactions for " + cboBank.Text + " bank.";
                    this.LoadDuplicateTransactions();
                    break;
                case "BatchPreview":
                    message = "Please print Batch Preview report for " + cboBank.Text + " bank.";
                    break;
                case "CreateBatch":
                    message = "Please create the batches for " + cboBank.Text + " bank.";
                    break;
                case "BatchReview":
                    message = "Please print Batch Review report for " + cboBank.Text + " bank.";
                    break;
                case "CreateJournal":
                    message = "Please create the journals " + cboBank.Text + " bank.";
                    break;
                case "CreateResponse":
                    message = "Please create the response files for " + cboBank.Text + " bank.";
                    break;
                case "CreateReturnFile":
                    message = "Please create the return files for " + cboBank.Text + " bank.";
                    break;
                case "CreateSettlement":
                    message = "Please create the settlement file for " + cboBank.Text + " bank.";
                    break;
                case "SettlementReport":
                    message = "Please print out Settlement File Report for " + cboBank.Text + " bank and keep it with your bank records.";
                    btnFinish.Enabled = true;
                    btnNext.Enabled = false;
                    break;
                case "NoLocationID":

                    if (bank.BankID != 13)
                    {
                        //skip tab
                        if (e.PreviousSelectedTab.Index == 0) //DataError
                        {
                            message = "This page displays transactions with invalid field(s). Please resolve the issue(s) before moving on to the next step for " + cboBank.Text + " bank.";
                            this.LoadBadData();

                            tabMain.TabIndex = 2;
                        }
                        else //SelectBank
                        {
                            message = "Please select a bank to process.";
                            btnPrevious.Enabled = false;
                            tabMain.TabIndex = 0;
                        }

                    }
                    else
                    {
                        message = "Location ID is missing for IStream Merchants below.";
                        this.LoadNoLocationID();
                    }
                    break;
            }

            txtMessage.Text = message;

        }

        private void frmAchProcessEngine_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadBadData()
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@BankID", bank.BankID));
            DataSet ds = data.SelectBadData(prms);

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grd.DataSource = bs;

            bank = null;
            data = null;

            this.Cursor = Cursors.Default;

        }

        private void LoadNoLocationID()
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            DataAchProcess data = new DataAchProcess();
            DataSet ds = data.SelectNoLocationID();

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grdNoLocationID.DataSource = bs;

            bank = null;
            data = null;

            this.Cursor = Cursors.Default;

        }

        private void LoadNOCBadData()
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@BankID", bank.BankID));
            DataSet ds = data.SelectNOCBadData(prms);

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grdNOC.DataSource = bs;

            bank = null;
            data = null;

            this.Cursor = Cursors.Default;

        }

        private void LoadDuplicateTransactions()
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;


            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
            prms.Add(new SqlParameter("@BankID", bank.BankID));
            DataSet ds = data.SelectScrubRejectDuplicateTrans(prms);


            try
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = ds.Tables[0];
                grdDuplicateTrans.DataSource = bs;

            }
            catch (Exception exc)
            {
                throw exc;
            }



            this.Cursor = Cursors.Default;

        }

        private void LoadOverVolumeMerchants()
        {
            this.Cursor = Cursors.WaitCursor;

            DataSet ds = null;
            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();
            try
            {
                ds = data.SelectOverVolumeMerchants(prms);
                BindingSource bs = new BindingSource();
                bs.DataSource = ds.Tables[0];
                grdOverVolumeMerchants.DataSource = bs;

                grdOverVolumeMerchants.DisplayLayout.Bands[0].Columns["Monthly Volume"].Format = "C";
                grdOverVolumeMerchants.DisplayLayout.Bands[0].Columns["Current Volume"].Format = "C";
            }
            catch (Exception exc)
            {
                throw exc;
            }



            this.Cursor = Cursors.Default;

        }

        private void LoadTransOnHold()
        {
            this.Cursor = Cursors.WaitCursor;

            DataSet ds = null;
            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@BeginPostedDate", DateTime.Today.AddMonths(-1)));
            prms.Add(new SqlParameter("@StatusID", 16));

            try
            {
                ds = data.SearchTransactionsLight(prms);
                BindingSource bs = new BindingSource();
                bs.DataSource = ds.Tables[0];
                grdTransOnHold.DataSource = bs;

                if (grdTransOnHold.Rows.Count > 0)
                    grdTransOnHold.Rows[0].Selected = true;

            }
            catch (Exception exc)
            {
                throw exc;
            }



            this.Cursor = Cursors.Default;

        }

        private void LoadOverTicketItems()
        {
            this.Cursor = Cursors.WaitCursor;

            DataSet ds = null;
            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@BeginPostedDate", DateTime.Today.AddMonths(-1)));
            prms.Add(new SqlParameter("@StatusID", 14));
            try
            {
                ds = data.SearchOverTicketItems(prms);
                BindingSource bs = new BindingSource();
                bs.DataSource = ds.Tables[0];
                grdOverTicketItems.DataSource = bs;

            }
            catch (Exception exc)
            {
                throw exc;
            }



            this.Cursor = Cursors.Default;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.LoadBadData();
        }

        private void btnPrintSettlementFileReport_Click(object sender, EventArgs e)
        {
            if (cboBank.SelectedIndex == -1)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;


            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            {
                DateTime cuttOffTime = DateTime.Now.Date;
                for (var i = 2; i > 0; i--)
                {
                    frmReportNew frmMonday = new frmReportNew();
                    frmMonday.ProccessDate = DateTime.Now.AddDays(-i);
                    frmMonday.CuttoffTime = cuttOffTime.AddDays(-i - 1);
                    frmMonday.ShowSettlementFile(bank.BankID, DateTime.Today);
                    frmMonday.ShowDialog();
                    cuttOffTime = DateTime.Now;
                }
                frmReportNew frm = new frmReportNew();
                frm.ProccessDate = DateTime.Now;
                frm.CuttoffTime = DateTime.Now.AddDays(-1);
                frm.ShowSettlementFile(bank.BankID, DateTime.Today);
                frm.ShowDialog();
            }
            else
            {
                frmReportNew frm = new frmReportNew();
                frm.ShowSettlementFile(bank.BankID, DateTime.Today);
                frm.ShowDialog();
            }



            m_ProcessCompleted = true;

            this.Cursor = Cursors.Default;
        }

        private void btnRefreshOverTicketItems_Click(object sender, EventArgs e)
        {
            this.LoadOverTicketItems();
        }

        private void grdMultiSelect_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Extended;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
        }

        private void grd_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Single;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
        }

        private void grdOverVolumeMerchants_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Extended;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
        }

        private void grd_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            FormHandler.OpenDataForm(new frmTransaction(), DataLayer.Int2Field(grid.ActiveRow.Cells["TransID"].Value));
        }

        private void btnRefreshOverVolumeMerchants_Click(object sender, EventArgs e)
        {
            this.LoadOverVolumeMerchants();
        }

        private void btnReleaseAllBatch_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Are you sure you want to release all batches?");

            if (result == DialogResult.No)
                return;

            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();

            foreach (UltraGridRow dr in grdOverVolumeMerchants.Rows)
            {
                prms.Clear();
                prms.Add(new SqlParameter("@ACHID", dr.Cells["ACH ID"].Value));
                prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));
                data.UpdateReleaseOverVolumeMerchant(prms);
            }

            this.LoadOverVolumeMerchants();
        }

        private void btnReleaseBatch_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Are you sure you want to release the selected batches?");

            if (result == DialogResult.No)
                return;

            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();

            foreach (UltraGridRow dr in grdOverVolumeMerchants.Selected.Rows)
            {
                prms.Clear();
                prms.Add(new SqlParameter("@ACHID", dr.Cells["ACH ID"].Value));
                prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));
                data.UpdateReleaseOverVolumeMerchant(prms);
            }

            this.LoadOverVolumeMerchants();

        }

        private void btnRefreshDuplicateTrans_Click(object sender, EventArgs e)
        {
            this.LoadDuplicateTransactions();

        }

        private void btnTransOnHold_Click(object sender, EventArgs e)
        {
            this.LoadTransOnHold();
        }

        private void btnCreateReturnFiles_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;
            bank.ProcessCreateReturnFiles();

            FormHandler.DispalyInformationMessage("Return files created.");

            this.Cursor = Cursors.Default;
        }

        private void btnRefreshNoc_Click(object sender, EventArgs e)
        {
            this.LoadNOCBadData();
        }

        private void grdTransOnHold_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            UltraGrid grid = (UltraGrid)sender;
            FormHandler.OpenDataForm(new frmTransaction(), DataLayer.Int2Field(grid.ActiveRow.Cells["TransID"].Value));

        }

        private void btnRefreshNoLocationID_Click(object sender, EventArgs e)
        {
            this.LoadNoLocationID();
        }

        private void frmAchProcessEngine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_ProcessCompleted)
            {
                return;
            }

            if (MessageBox.Show("You have not completed the ACH Process.  Do you want to close this window?"
                , "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void grdMultiSelect_InitializeLayou(object sender, InitializeLayoutEventArgs e)
        {

        }







    }
}