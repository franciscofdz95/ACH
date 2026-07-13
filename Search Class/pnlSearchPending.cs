using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ACH2007
{
    public partial class pnlSearchPending : UserControl
    {
        public pnlSearchPending()
        {
            InitializeComponent();

            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtPendingID.Tag = new SearchFieldInfo("@PendingID", Search_Field_Type.Number);
            txtAmount.Tag = new SearchFieldInfo("@Amount", Search_Field_Type.Number);
            txtAccountNo.Tag = new SearchFieldInfo("@AccountNo", Search_Field_Type.String );
            txtTransRoute.Tag = new SearchFieldInfo("@TransRoute", Search_Field_Type.String);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtPendingID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
        }

        

    }
}
