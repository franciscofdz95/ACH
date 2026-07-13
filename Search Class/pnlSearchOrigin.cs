using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchOrigin : UserControl
    {
        public pnlSearchOrigin()
        {
            InitializeComponent();

            txtOriginID.Tag = new SearchFieldInfo("@OriginID", Search_Field_Type.Int);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);

            txtOriginID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
