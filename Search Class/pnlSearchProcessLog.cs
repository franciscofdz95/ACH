using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchProcessLog : UserControl
    {
        public pnlSearchProcessLog()
        {
            InitializeComponent();

            txtPostBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtPostEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            txtTableName.Tag = new SearchFieldInfo("@TableName", Search_Field_Type.String);
            txtSourceSystem.Tag = new SearchFieldInfo("@SourceSystem", Search_Field_Type.String);

        }
    }
}
