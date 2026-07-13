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
    public partial class pnlSearchAchRecurring : UserControl
    {
        public pnlSearchAchRecurring()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date );
            txtTransEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtScheduledDate.Tag = new SearchFieldInfo("@Schedule_Date", Search_Field_Type.Date);
            txtAccountNo.Tag = new SearchFieldInfo("@AccountNo", Search_Field_Type.String );
            txtAccountName.Tag = new SearchFieldInfo("@AccountName", Search_Field_Type.String);
            txtRecurID.Tag = new SearchFieldInfo("@RecurID", Search_Field_Type.Int);
            txtRefID.Tag = new SearchFieldInfo("@RefID", Search_Field_Type.String);
            txtTransRoute.Tag = new SearchFieldInfo("@TransRoute", Search_Field_Type.String);
            txtAmount.Tag = new SearchFieldInfo("@Amount", Search_Field_Type.Number);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtRecurID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            txtScheduledDate.Text = string.Empty;
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }

      
    
  
    }
}
