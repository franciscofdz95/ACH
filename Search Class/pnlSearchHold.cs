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
    public partial class pnlSearchHold : UserControl
    {
        public pnlSearchHold()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtHoldID.Tag = new SearchFieldInfo("@HoldID", Search_Field_Type.Int );
            txtAmount.Tag = new SearchFieldInfo("@Amount", Search_Field_Type.Number );
            cboType.Tag = new SearchFieldInfo("@Type", Search_Field_Type.String);
            cboMerchantType.Tag = new SearchFieldInfo("@MerchantType", Search_Field_Type.String);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtHoldID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            cboType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboMerchantType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);


            LookUpTableHandler.LoadHoldTypes(cboType);
            LookUpTableHandler.LoadMerchantType(cboMerchantType); 
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }

       

      

    }
}
