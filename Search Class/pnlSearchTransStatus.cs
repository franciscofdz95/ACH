using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchTransStatus : UserControl
    {
        public pnlSearchTransStatus()
        {
            InitializeComponent();

            txtStatusID.Tag = new SearchFieldInfo("@StatusID", Search_Field_Type.Int);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);
            txtAction.Tag = new SearchFieldInfo("@ActionAvailable", Search_Field_Type.String);

            txtStatusID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
