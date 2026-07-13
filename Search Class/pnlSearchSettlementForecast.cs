using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class pnlSearchSettlementForecast : UserControl
    {
        public pnlSearchSettlementForecast()
        {
            InitializeComponent();

            cboBankID.Tag = new SearchFieldInfo("@BankID", Search_Field_Type.String);

            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            LookUpTableHandler.LoadBankID(cboBankID);
        }
    }
}
