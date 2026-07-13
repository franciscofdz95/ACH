using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchOverTicketItems : UserControl
    {
        public pnlSearchOverTicketItems()
        {
            InitializeComponent();

            txtTransBeginDate.Tag = new SearchFieldInfo("@BeginPostedDate", Search_Field_Type.Date);
            txtTransEndDate.Tag = new SearchFieldInfo("@EndPostedDate", Search_Field_Type.Date);
            cboStatus.Tag = new SearchFieldInfo("@StatusID", Search_Field_Type.Int);

            LookUpTableHandler.LoadAllTransStatus(cboStatus);
            ListHandler.ListFindItem(cboStatus, "14");

        }

        
    }
}
