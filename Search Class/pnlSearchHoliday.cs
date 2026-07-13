using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchHoliday : UserControl
    {
        public pnlSearchHoliday()
        {
            InitializeComponent();

            txtHolidayID.Tag = new SearchFieldInfo("@HolidayID", Search_Field_Type.Int);
            txtHoliday.Tag = new SearchFieldInfo("@Holiday", Search_Field_Type.Date);
            txtHolidayDesc.Tag = new SearchFieldInfo("@Name", Search_Field_Type.String);

            txtHolidayID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

        }
    }
}
