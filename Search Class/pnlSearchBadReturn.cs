using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchBadReturn : UserControl
    {
        public pnlSearchBadReturn()
        {
            InitializeComponent();

            txtBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);

        }

        
    }
}
