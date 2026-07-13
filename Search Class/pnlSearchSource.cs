using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchSource : UserControl
    {
        public pnlSearchSource()
        {
            InitializeComponent();

            txtSourceID.Tag = new SearchFieldInfo("@SourceID", Search_Field_Type.Int);
            txtSource.Tag = new SearchFieldInfo("@Source", Search_Field_Type.String);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);

            txtSourceID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
