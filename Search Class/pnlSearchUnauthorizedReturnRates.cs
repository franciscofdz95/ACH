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
    public partial class pnlSearchUnauthorizedReturnRates : UserControl
    {
        public pnlSearchUnauthorizedReturnRates()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtBatchID.Tag = new SearchFieldInfo("@BatchID", Search_Field_Type.Int);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            DateTime dt = DateTime.Today;
            txtPostBeginDate.Value = DateTime.Parse(dt.Month.ToString() + "/1/" + dt.Year.ToString());

        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }
    }
}
