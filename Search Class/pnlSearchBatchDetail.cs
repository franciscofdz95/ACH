using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;

namespace AchSystem
{
    public partial class pnlSearchBatchDetail : UserControl
    {
        public pnlSearchBatchDetail()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtBatchID.Tag = new SearchFieldInfo("@BatchID", Search_Field_Type.Int);
            txtTransID.Tag = new SearchFieldInfo("@TransID", Search_Field_Type.Int);
            txtTraceNumber.Tag = new SearchFieldInfo("@TraceNumber", Search_Field_Type.Int);
            cboBankID.Tag = new SearchFieldInfo("@BankID", Search_Field_Type.Int);
            cboSource.Tag = new SearchFieldInfo("@Source", Search_Field_Type.Int);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTraceNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            cboSource.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadBankID(cboBankID);
            LookUpTableHandler.LoadBatchSource(cboSource); 
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }
    }
}
