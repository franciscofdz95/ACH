using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace AchSystem
{
    public partial class pnlSearchLog : UserControl
    {
        public pnlSearchLog()
        {
            InitializeComponent();

            cboUser.Tag = new SearchFieldInfo("@LoginID", Search_Field_Type.String );
            txtID.Tag = new SearchFieldInfo("@ID", Search_Field_Type.Int);
            txtTableName.Tag = new SearchFieldInfo("@TableName", Search_Field_Type.String );
            txtNote.Tag = new SearchFieldInfo("@Note", Search_Field_Type.Int);
            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            cboUser.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadUsers(cboUser); 
        }

  

      
    }
}
