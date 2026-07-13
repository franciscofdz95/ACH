using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchSecc : UserControl
    {
        public pnlSearchSecc()
        {
            InitializeComponent();

            txtSeccID.Tag = new SearchFieldInfo("@SeccID", Search_Field_Type.Int);
            txtSecc.Tag = new SearchFieldInfo("@Secc", Search_Field_Type.String);

            txtSeccID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
