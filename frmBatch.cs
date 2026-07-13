using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmBatch : AchSystem.frmBase 
    {
        public frmBatch()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtPaidOutID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtDebitCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtCreditCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtDebit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtCredit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtOverDailyAmt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtOverItemAmt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            this.Data = new DataBatch();
            this.KeyColumnName = "BatchID";
            FormHandler.SetSecurity(this);

        }

         public override bool FormFind()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@" + this.KeyColumnName, this.ID));
            this.Dr = this.Data.Select(prms);

            if (this.Dr.Read())
                return true;
            else
                return false;
        }

        public override void FormShow()
        {
            this.Showing = true;

            txtBatchID.Text = this.Dr["Batch ID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();
            txtAchID.Text = this.Dr["Ach ID"].ToString().Trim();
            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();
            txtToProcessDate.Text = this.Dr["To Process Date"].ToString().Trim();
            txtProcessedDate.Text = this.Dr["Processed Date"].ToString().Trim();
            txtDebit.Text = this.Dr["Debit"].ToString().Trim();
            txtCredit.Text = this.Dr["Credit"].ToString().Trim();
            txtDebitCount.Text = this.Dr["Debit Count"].ToString().Trim();
            txtCreditCount.Text = this.Dr["Credit Count"].ToString().Trim();
            txtOverDailyAmt.Text = this.Dr["Over Daily Amount Limit Count"].ToString().Trim();
            txtOverItemAmt.Text = this.Dr["Over Item Amount"].ToString().Trim();
            txtReconciled.Text = this.Dr["Reconciled"].ToString().Trim();
            txtSettleDate.Text = this.Dr["Settle Date"].ToString().Trim();
            txtPaidDate.Text = this.Dr["PaidDate"].ToString().Trim();
            txtPaidOutID.Text = this.Dr["Paid Out ID"].ToString().Trim();
            txtTransBase.Text = this.Dr["Trans Base"].ToString().Trim();

            this.LoadBatchDetail();

            FormHandler.PopulateControlTag(this);

            this.Showing = false;
        }

        public override void FormOpen(UltraGridRow dr)
        {
            if (dr != null)
            {
                this.ID = Convert.ToInt64(dr.Cells[this.KeyColumnName].Value);
                if (this.FormFind())
                    this.FormShow();

                this.ShowDialog();
            }
            else
            {
                if (this.ID == -1)
                    if (!tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled )
                        this.Close();
            }
        }

        public override void FormNew()
        {
            this.Adding = true;
            this.FormClear();
            FormHandler.ClearControlTag(this);
            this.FormToggleButtons();

            txtAchID.ReadOnly = false;
            btnMerchant.Enabled = true;
            txtPostedDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;


            txtBatchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtMerchantName.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtPostedDate.Text = string.Empty;
            txtToProcessDate.Text = string.Empty;
            txtProcessedDate.Text = string.Empty;
            txtDebit.Text = string.Empty;
            txtCredit.Text = string.Empty;
            txtDebitCount.Text = string.Empty;
            txtCreditCount.Text = string.Empty;
            txtOverDailyAmt.Text = string.Empty;
            txtOverItemAmt.Text = string.Empty;
            txtReconciled.Text = string.Empty;
            txtSettleDate.Text = string.Empty;
            txtPaidDate.Text = string.Empty;
            txtPaidOutID.Text = string.Empty;
            txtTransBase.Text = string.Empty;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@BatchID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@ToProcessDate", DataLayer.Date2Field(txtToProcessDate.Text)));
            prms.Add(new SqlParameter("@ProcessedDate", DataLayer.Date2Field(txtProcessedDate.Text)));
            prms.Add(new SqlParameter("@Debit", DataLayer.Decimal2Field (txtDebit.Text)));
            prms.Add(new SqlParameter("@Credit", DataLayer.Decimal2Field(txtCredit.Text)));
            prms.Add(new SqlParameter("@DebitCount", DataLayer.Int2Field(txtDebitCount.Text)));
            prms.Add(new SqlParameter("@CreditCount", DataLayer.Int2Field(txtCreditCount.Text)));
            prms.Add(new SqlParameter("@OverDailyAmountLimitCount", DataLayer.Decimal2Field(txtOverDailyAmt.Text)));
            prms.Add(new SqlParameter("@OverItemAmount", DataLayer.Decimal2Field(txtOverItemAmt.Text)));
            prms.Add(new SqlParameter("@Reconciled", DataLayer.Date2Field(txtReconciled.Text)));
            prms.Add(new SqlParameter("@SettleDate", DataLayer.Date2Field(txtSettleDate.Text)));
            prms.Add(new SqlParameter("@PaidDate", DataLayer.Date2Field(txtPaidDate.Text)));
            prms.Add(new SqlParameter("@PaidOutID", DataLayer.Int2Field(txtPaidOutID.Text)));
            prms.Add(new SqlParameter("@TransBase", txtTransBase.Text));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID ));

            long lngID = this.Data.Insert (prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                txtAchID.ReadOnly = true;
                btnMerchant.Enabled = false;
                this.ID = lngID;
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool FormUpdate()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@BatchID", Convert.ToInt32(txtBatchID.Text)));
            prms.Add(new SqlParameter("@MerchantID", Convert.ToInt32(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@ToProcessDate", DataLayer.Date2Field(txtToProcessDate.Text)));
            prms.Add(new SqlParameter("@ProcessedDate", DataLayer.Date2Field(txtProcessedDate.Text)));
            prms.Add(new SqlParameter("@Debit", Convert.ToDecimal(txtDebit.Text)));
            prms.Add(new SqlParameter("@Credit", Convert.ToDecimal(txtCredit.Text)));
            prms.Add(new SqlParameter("@DebitCount", DataLayer.Int2Field(txtDebitCount.Text)));
            prms.Add(new SqlParameter("@CreditCount", DataLayer.Int2Field(txtCreditCount.Text)));
            prms.Add(new SqlParameter("@OverDailyAmountLimitCount", Convert.ToDecimal(txtOverDailyAmt.Text)));
            prms.Add(new SqlParameter("@OverItemAmount", Convert.ToDecimal(txtOverItemAmt.Text)));
            prms.Add(new SqlParameter("@Reconciled", DataLayer.Date2Field(txtReconciled.Text)));
            prms.Add(new SqlParameter("@SettleDate", DataLayer.Date2Field(txtSettleDate.Text)));
            prms.Add(new SqlParameter("@PaidDate", DataLayer.Date2Field(txtPaidDate.Text)));
            prms.Add(new SqlParameter("@PaidOutID", DataLayer.Int2Field(txtPaidOutID.Text)));
            prms.Add(new SqlParameter("@TransBase", txtTransBase.Text));
            prms.Add(new SqlParameter("@UpdatedBy",main.g_User.UserID ));

            int intRows = this.Data.Update(prms);

            if (intRows > 0)
            {
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void FormUndo()
        {
            this.Adding = false;

            this.FormToggleButtons();
            txtAchID.ReadOnly = true;
            btnMerchant.Enabled = false;
            if (this.ID != -1)
            {
                if (this.FormFind())
                    this.FormShow();
            }
            else
            {
                this.Close();
            }
        }

        public override bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtAchID.Text.Trim() == string.Empty)
                strError += "Please enter an ACH ID.\n";

            if (txtPostedDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtPostedDate.Text.Trim()))
                    strError += "Please enter a valid Posted Date.\n";

            if (txtToProcessDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtToProcessDate.Text.Trim()))
                    strError += "Please enter a valid To Process Date.\n";

            if (txtProcessedDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtProcessedDate.Text.Trim()))
                    strError += "Please enter a valid Processed Date.\n";

            if (txtReconciled.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtReconciled.Text.Trim()))
                    strError += "Please enter a valid Reconciled Date.\n";

            if (txtSettleDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtSettleDate.Text.Trim()))
                    strError += "Please enter a valid Settle Date.\n";

            if (txtPaidDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtPaidDate.Text.Trim()))
                    strError += "Please enter a valid Paid Date.\n";
            
            if (strError == string.Empty)
                return true;
            else
            {
                MessageBox.Show(strError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private void frmBatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

        

        private void LoadBatchDetail()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@BatchID",this.ID ));

            DataBatchDetail data = new DataBatchDetail();

            DataSet ds = data.SelectBatchDetails(prms);
 
            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grdBatchDetail.DataSource = bs;

            // Apply the style as the default cell style.
            grdBatchDetail.DisplayLayout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
            data = null;
        }

        private void grdBatchDetail_DoubleClick(object sender, EventArgs e)
        {
            if (grdBatchDetail.Selected.Rows.Count > 0)
            {
               UltraGridRow dr = grdBatchDetail.ActiveRow;
               if (dr.Cells["Source"].Value.ToString() == "C")
                   FormHandler.OpenDataForm(new frmTransaction(), DataLayer.Int2Field(dr.Cells["Trans ID"].Value));
                else
                   FormHandler.OpenDataForm(new frmEFT(), DataLayer.Int2Field(dr.Cells["Trans ID"].Value));

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row, pnlMain);
        }

        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row, pnlMain);
        }

 
    }
}