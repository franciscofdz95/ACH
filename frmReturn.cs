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
    public partial class frmReturn : AchSystem.frmBase
    {
        public frmReturn()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtReturnID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtReturnStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtJournalID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtPaidOutID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtFeeApplied.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            cboPrinted.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboReasonCode.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);


            LookUpTableHandler.LoadAllReasonCodes (cboReasonCode);
            LookUpTableHandler.LoadReturnTransType(cboTransType);
            LookUpTableHandler.LoadReturnType(cboType);
            LookUpTableHandler.LoadReturnPrinted(cboPrinted);

            this.Data = new DataReturn();
            this.KeyColumnName = "ReturnID";
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

            txtReturnID.Text = this.Dr["Return ID"].ToString().Trim();
            txtAchID.Text = this.Dr["Ach ID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();
            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();
            txtResubmitDate.Text = this.Dr["Resubmit Date"].ToString().Trim();
            txtJournalID.Text = this.Dr["Journal ID"].ToString().Trim();
            txtTransRoute.Text = this.Dr["Trans Route"].ToString().Trim();
            txtAccountNo.Text = this.Dr["Account No"].ToString().Trim();
            txtAccountName.Text = this.Dr["Account Name"].ToString().Trim();
            txtAmount.Text = this.Dr["Amount"].ToString().Trim();
            ListHandler.ListFindItem(cboTransType, this.Dr["Trans Type"].ToString().Trim());
            ListHandler.ListFindItem(cboReasonCode, this.Dr["Reason Code"].ToString().Trim());
            txtAddenCode.Text = this.Dr["Adden Code"].ToString().Trim();
            txtAddenInfo.Text = this.Dr["Adden Info"].ToString().Trim();
            txtOrigTrace .Text = this.Dr["Orig Trace"].ToString().Trim();
            txtOrigRDFI.Text = this.Dr["Orig RDFI"].ToString().Trim();
            txtTrace.Text = this.Dr["Trace"].ToString().Trim();
            txtSettleDate.Text = this.Dr["Settle Date"].ToString().Trim();
            txtTransID.Text = this.Dr["trans ID"].ToString().Trim();
            txtRefID.Text = this.Dr["Ref ID"].ToString().Trim();
            txtComments.Text = this.Dr["Comments"].ToString().Trim();
            ListHandler.ListFindItem(cboType, this.Dr["Type"].ToString().Trim());
            txtProcessDate.Text = this.Dr["Process Date"].ToString().Trim();
            txtBatchID.Text = this.Dr["Batch ID"].ToString().Trim();
            txtSource.Text = this.Dr["Source"].ToString().Trim();
            ListHandler.ListFindItem(cboPrinted, this.Dr["Printed"].ToString().Trim());
            txtReturnStatus.Text = this.Dr["Return Status"].ToString().Trim();
            txtFeeApplied.Text = this.Dr["Fee Applied"].ToString().Trim();
            txtPaidOutID.Text = this.Dr["Paid Out ID"].ToString().Trim();
            txtEncryptAccount.Text = this.Dr["Encrypt Account No"].ToString().Trim();

            FormHandler.PopulateControlTag(this);

            if (!main.g_User.IsAdmin)
            {
                txtAccountNo.Text = this.Dr["MaskedAccountNo"].ToString().Trim();
            }

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

            txtReturnID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtPostedDate.Text = string.Empty;
            txtResubmitDate.Text = string.Empty;
            txtJournalID.Text = string.Empty;
            txtTransRoute.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtAmount.Text = string.Empty;
            cboTransType.SelectedIndex = -1;
            cboReasonCode.SelectedIndex = -1;
            txtAddenCode.Text = string.Empty;
            txtAddenInfo.Text = string.Empty;
            txtOrigTrace.Text = string.Empty;
            txtOrigRDFI.Text = string.Empty;
            txtTrace.Text = string.Empty;
            txtSettleDate.Text = string.Empty;
            txtTransID.Text = string.Empty;
            txtRefID.Text = string.Empty;
            txtComments.Text = string.Empty;
            cboType.SelectedIndex = -1;
            txtProcessDate.Text = string.Empty;
            txtBatchID.Text = string.Empty;
            txtSource.Text = string.Empty;
            cboPrinted.SelectedIndex = -1;
            txtReturnStatus.Text = string.Empty;
            txtFeeApplied.Text = string.Empty;
            txtPaidOutID.Text = string.Empty;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@ReturnID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@ResubmitDate", DataLayer.Date2Field(txtResubmitDate.Text)));
            prms.Add(new SqlParameter("@JournalID", DataLayer.Int2Field(txtJournalID.Text)));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            prms.Add(new SqlParameter("@NameOnAccount", txtAccountName.Text));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            AchListItem item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            prms.Add(new SqlParameter("@ReasonCode", cboReasonCode.Text));
            prms.Add(new SqlParameter("@AddenCode", txtAddenCode.Text));
            prms.Add(new SqlParameter("@AddenInfo", txtAddenInfo.Text));
            prms.Add(new SqlParameter("@OrigTrace", txtOrigTrace.Text));
            prms.Add(new SqlParameter("@OrigRDFI", txtOrigRDFI.Text));
            prms.Add(new SqlParameter("@Trace", txtTrace.Text));
            prms.Add(new SqlParameter("@SettleDate", txtSettleDate.Text));
            prms.Add(new SqlParameter("@TransID", DataLayer.Int2Field(txtTransID.Text)));
            prms.Add(new SqlParameter("@RefID", txtRefID.Text));
            prms.Add(new SqlParameter("@Comments", txtComments.Text));
            prms.Add(new SqlParameter("@Type", cboType.Text.Substring(0, 2)));
            prms.Add(new SqlParameter("@ProcessDate", DataLayer.Date2Field(txtProcessDate.Text)));
            prms.Add(new SqlParameter("@BatchID", DataLayer.Int2Field(txtBatchID.Text)));
            prms.Add(new SqlParameter("@Source", txtSource.Text));
            prms.Add(new SqlParameter("@Printed", cboPrinted.Text.Substring(0, 1)));
            prms.Add(new SqlParameter("@ReturnType", txtReturntype.Text));
            prms.Add(new SqlParameter("@ReturnStatus", txtReturnStatus.Text));
            prms.Add(new SqlParameter("@FeeApplied", txtFeeApplied.Text));
            prms.Add(new SqlParameter("@PaidOutID", DataLayer.Int2Field(txtPaidOutID.Text)));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                this.ID = lngID;
                txtAchID.ReadOnly = true;
                btnMerchant.Enabled = false;

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

            prms.Add(new SqlParameter("@ReturnID", DataLayer.Int2Field(txtReturnID.Text)));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@ResubmitDate", DataLayer.Date2Field(txtResubmitDate.Text)));
            prms.Add(new SqlParameter("@JournalID", DataLayer.Int2Field(txtJournalID.Text)));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo .Text));
            prms.Add(new SqlParameter("@NameOnAccount", txtAccountName.Text));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            AchListItem item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            prms.Add(new SqlParameter("@ReasonCode", cboReasonCode.Text ));
            prms.Add(new SqlParameter("@AddenCode", txtAddenCode.Text ));
            prms.Add(new SqlParameter("@AddenInfo", txtAddenInfo.Text ));
            prms.Add(new SqlParameter("@OrigTrace", txtOrigTrace.Text));
            prms.Add(new SqlParameter("@OrigRDFI", txtOrigRDFI.Text ));
            prms.Add(new SqlParameter("@Trace", txtTrace.Text));
            prms.Add(new SqlParameter("@SettleDate", txtSettleDate.Text));
            if (txtTransID.Text.Trim() == string.Empty || txtTransID.Text == "0")
                prms.Add(new SqlParameter("@TransID", DBNull.Value));
            else
                prms.Add(new SqlParameter("@TransID", DataLayer.Int2Field(txtTransID.Text)));


            prms.Add(new SqlParameter("@RefID", txtRefID.Text ));
            prms.Add(new SqlParameter("@Comments", txtComments.Text ));
            prms.Add(new SqlParameter("@Type", cboType.Text.Substring(0,2)));
            prms.Add(new SqlParameter("@ProcessDate", DataLayer.Date2Field(txtProcessDate.Text)));
            prms.Add(new SqlParameter("@BatchID", DataLayer.Int2Field(txtBatchID.Text)));
            prms.Add(new SqlParameter("@Source", txtSource.Text ));
            prms.Add(new SqlParameter("@Printed", cboPrinted.Text.Substring(0,1)));
            prms.Add(new SqlParameter("@ReturnType", txtReturntype.Text ));
            prms.Add(new SqlParameter("@ReturnStatus", txtReturnStatus.Text));
            prms.Add(new SqlParameter("@FeeApplied", txtFeeApplied.Text ));
            prms.Add(new SqlParameter("@PaidOutID", DataLayer.Int2Field(txtPaidOutID.Text)));
            prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID ));

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

            if (txtMerchantID.Text.Trim() == string.Empty)
                strError += "Please enter a Merchant ID.\n";

            if (txtPostedDate.Text == DataLayer.Empty_Date_Time)
                strError += "Please enter a Posted Date.\n";

            if (txtPostedDate.Text != DataLayer.Empty_Date_Time)
            {
                if (!DataLayer.IsDate(txtPostedDate.Text.Trim()))
                    strError += "Please enter a valid Posted Date.\n";
            }

            if (txtAmount.Text.Trim() == string.Empty)
                strError += "Please enter an Amount.\n";

            if (txtAmount.Text.Trim() != string.Empty)
            {
                if (!DataLayer.IsNumeric(txtAmount.Text))
                    strError += "Please enter a valid Amount.\n";
            }

            if (cboTransType.SelectedIndex == -1)
                strError += "Please select a Trans Type.\n";

            if (txtTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a Trans Route.\n";

            if (txtAccountNo.Text.Trim() == string.Empty)
                strError += "Please enter an Account No.\n";

            if (txtAccountName.Text.Trim() == string.Empty)
                strError += "Please enter an Account Name.\n";

            if (cboReasonCode.SelectedIndex == -1)
                strError += "Please select a Reason Code.\n";

            if (cboType.SelectedIndex == -1)
                strError += "Please select a Type.\n";

            if (cboPrinted.SelectedIndex == -1)
                strError += "Please select a Printed status.\n";

            if (txtTransID.Text.Trim() == string.Empty)
                strError += "Please enter a TransID.\n";

            if (txtSource.Text.Trim() == string.Empty)
                strError += "Please enter a Source.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmReturn_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }

        }

        

        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row,pnlMain);
        }

 
        private void lnkJournalID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtJournalID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmJournal(), DataLayer.Int2Field(txtJournalID.Text));
        }

        private void lnkTransID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtTransID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmTransaction(), DataLayer.Int2Field(txtTransID.Text));
        }

        private void lnkBatchID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtBatchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmBatch(), DataLayer.Int2Field(txtBatchID.Text));
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row,pnlMain );

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickTransaction();
            if (row != null)
                txtTransID.Text = row.Cells["TransID"].Value.ToString();

        }

 

  

    }
}

