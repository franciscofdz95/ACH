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
    public partial class pnlSearchJournal : UserControl
    {
        public pnlSearchJournal()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String );
            txtHoldID.Tag = new SearchFieldInfo("@HoldID", Search_Field_Type.String );
            txtJournalID.Tag = new SearchFieldInfo("@JournalID", Search_Field_Type.String );
            txtRefID.Tag = new SearchFieldInfo("@RefID", Search_Field_Type.String );
            cboRefcode.Tag = new SearchFieldInfo("@RefCode", Search_Field_Type.String );
            txtAmount.Tag = new SearchFieldInfo("@Amount", Search_Field_Type.Number);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtRefID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtHoldID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtJournalID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            cboRefcode.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadRefCodes(cboRefcode); 
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateAchID(row, grpRequired);
        }

      

       

    

    }
}
