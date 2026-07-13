using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Infragistics.Win.UltraWinGrid;

namespace AchSystem
{
    public partial class pnlSearchTransaction : UserControl
    {
        public pnlSearchTransaction()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date );
            txtTransEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtAccountNo.Tag = new SearchFieldInfo("@AccountNo", Search_Field_Type.String );
            txtAccountName.Tag = new SearchFieldInfo("@AccountName", Search_Field_Type.String);
            txtTransID.Tag = new SearchFieldInfo("@TransID", Search_Field_Type.Int );
            txtRefID.Tag = new SearchFieldInfo("@RefID", Search_Field_Type.String);
            txtUploadID.Tag = new SearchFieldInfo("@UploadID", Search_Field_Type.Int);
            cboTransType.Tag = new SearchFieldInfo("@TransType", Search_Field_Type.String);
            txtTransRoute.Tag = new SearchFieldInfo("@TransRoute", Search_Field_Type.String);
            txtBatchID.Tag = new SearchFieldInfo("@BatchID", Search_Field_Type.Int);
            cboStatus.Tag = new SearchFieldInfo("@StatusID", Search_Field_Type.Int );
            cboSecc.Tag = new SearchFieldInfo("@Secc", Search_Field_Type.String);
            cboOrigin.Tag = new SearchFieldInfo("@OriginID", Search_Field_Type.Int);
            txtAmount.Tag = new SearchFieldInfo("@Amount", Search_Field_Type.Number);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);
            txtMerchantName.Tag = new SearchFieldInfo("@MerchantName", Search_Field_Type.String);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtUploadID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboSecc.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboStatus.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboOrigin.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadTransactionTransType(cboTransType);
            LookUpTableHandler.LoadAllTransStatus(cboStatus);
            LookUpTableHandler.LoadSecc(cboSecc);
            LookUpTableHandler.LoadOrigins(cboOrigin);
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }

        private void opt_CheckedChanged(object sender, EventArgs e)
        {
            if (optTransDate.Checked)
            {
                txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
                txtTransEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            }

            if (optDateProcessed.Checked)
            {
                txtTransBeginDate.Tag = new SearchFieldInfo("@BeginDateProcessed", Search_Field_Type.Date);
                txtTransEndDate.Tag = new SearchFieldInfo("@EndDateProcessed", Search_Field_Type.Date);
            }

        }

      

        

        

       

      
    
  
    }
}
