using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchTransType : UserControl
    {
        public pnlSearchTransType()
        {
            InitializeComponent();

            txtTransTypeID.Tag = new SearchFieldInfo("@TransTypeID", Search_Field_Type.Int);
            txtDescription.Tag = new SearchFieldInfo("@TransTypeDesc", Search_Field_Type.String);

            txtTransTypeID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
