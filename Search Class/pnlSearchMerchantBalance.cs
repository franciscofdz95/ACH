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
    public partial class pnlSearchMerchantBalance : UserControl
    {
        public pnlSearchMerchantBalance()
        {
            InitializeComponent();

            txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);

        }

  

      
    }
}
