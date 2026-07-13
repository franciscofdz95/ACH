using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchUser : UserControl
    {
        public pnlSearchUser()
        {
            InitializeComponent();

            txtUserID.Tag = new SearchFieldInfo("@UserID", Search_Field_Type.Int);
            txtLoginID.Tag = new SearchFieldInfo("@LoginID", Search_Field_Type.String);
            txtFirstname.Tag = new SearchFieldInfo("@Firstname", Search_Field_Type.String );
            txtLastname.Tag = new SearchFieldInfo("@Lastname", Search_Field_Type.String);

            txtUserID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }

       

    }
}
