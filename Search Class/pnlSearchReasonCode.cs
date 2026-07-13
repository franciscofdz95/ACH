using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchReasonCode : UserControl
    {
        public pnlSearchReasonCode()
        {
            InitializeComponent();

            txtReasonID.Tag = new SearchFieldInfo("@ReasonID", Search_Field_Type.Int);
            txtReasonCode .Tag = new SearchFieldInfo("@ReasonCode", Search_Field_Type.String);
            txtReason.Tag = new SearchFieldInfo("@ReasonDesc", Search_Field_Type.String);

            txtReasonID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
