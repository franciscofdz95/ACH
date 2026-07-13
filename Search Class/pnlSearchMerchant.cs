using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchMerchant : UserControl
    {
        public pnlSearchMerchant()
        {
            InitializeComponent();

            txtMerchantName.Tag = new SearchFieldInfo("@AchCoName", Search_Field_Type.String);
            txtMerchantID.Tag = new SearchFieldInfo("@MerchantID", Search_Field_Type.Int);
            txtAchID.Tag = new SearchFieldInfo("@AchID", Search_Field_Type.Int);
            cboBankID.Tag = new SearchFieldInfo("@BankID", Search_Field_Type.Int);
            cboMerchantType.Tag = new SearchFieldInfo("@MerchantTypeCode", Search_Field_Type.Int);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboMerchantType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadBankID(cboBankID);
            LookUpTableHandler.LoadMerchantType(cboMerchantType);
        }
   
        

        
    }
}
