using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchBank : UserControl
    {
        public pnlSearchBank()
        {
            InitializeComponent();

            txtBankName.Tag = new SearchFieldInfo("@BankName", Search_Field_Type.String);
            txtTransRoute.Tag = new SearchFieldInfo("@TransRoute", Search_Field_Type.String);

            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
  

    }
}
