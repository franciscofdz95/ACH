using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;

namespace AchSystem
{
    public partial class pnlSearchReturn : UserControl
    {
        public pnlSearchReturn()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtTransEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            cboReasonCode.Tag = new SearchFieldInfo("@ReasonCode", Search_Field_Type.String );
            txtTransID.Tag = new SearchFieldInfo("@TransID", Search_Field_Type.Int);
            txtJournalID.Tag = new SearchFieldInfo("@JournalID", Search_Field_Type.Int);
            txtRefID.Tag = new SearchFieldInfo("@RefID", Search_Field_Type.String);
            txtReturnID.Tag = new SearchFieldInfo("@ReturnID", Search_Field_Type.Int);
            txtAmount.Tag = new SearchFieldInfo("@Amount", Search_Field_Type.Number);
            cboBankID.Tag = new SearchFieldInfo("@BankID", Search_Field_Type.String);
            txtAccountName.Tag = new SearchFieldInfo("@AccountName", Search_Field_Type.String);
            cboSource.Tag = new SearchFieldInfo("@Source", Search_Field_Type.String);

            //txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumericOnly_KeyPress);
            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtReturnID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtJournalID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            LookUpTableHandler.LoadBankID(cboBankID);
            LookUpTableHandler.LoadAllReasonCodes(cboReasonCode);
            LookUpTableHandler.LoadTransSource(cboSource);

        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }

        private void opt_CheckedChanged(object sender, EventArgs e)
        {
            if (optReturnDate.Checked)
            {
                txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
                txtTransEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            }

            if (optTransDate.Checked)
            {
                txtTransBeginDate.Tag = new SearchFieldInfo("@BeginTransDate", Search_Field_Type.Date);
                txtTransEndDate.Tag = new SearchFieldInfo("@EndTransDate", Search_Field_Type.Date);
            }

            if (optDateProcessed.Checked)
            {
                txtTransBeginDate.Tag = new SearchFieldInfo("@BeginDateProcessed", Search_Field_Type.Date);
                txtTransEndDate.Tag = new SearchFieldInfo("@EndDateProcessed", Search_Field_Type.Date);
            }

        }
       
    }
}
