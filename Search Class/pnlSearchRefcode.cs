using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchRefcode : UserControl
    {
        public pnlSearchRefcode()
        {
            InitializeComponent();

            txtRefcodeID.Tag = new SearchFieldInfo("@RefcodeID", Search_Field_Type.Int);
            txtRefcode.Tag = new SearchFieldInfo("@Refcode", Search_Field_Type.String);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);

            txtRefcodeID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
