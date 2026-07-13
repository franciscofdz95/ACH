using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchGroupMerchant : UserControl
    {
        public pnlSearchGroupMerchant()
        {
            InitializeComponent();

            txtGroupID.Tag = new SearchFieldInfo("@GroupID", Search_Field_Type.Int);
            txtGroupName.Tag = new SearchFieldInfo("@GroupName", Search_Field_Type.String);
            txtDescription.Tag = new SearchFieldInfo("@Description", Search_Field_Type.String);

            txtGroupID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
