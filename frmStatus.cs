using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmStatus : Form
    {
        public frmStatus()
        {
            InitializeComponent();
        }

     

        private void frmStatus_TextChanged(object sender, EventArgs e)
        {

            if (this.Text.Trim() == string.Empty)
                this.Hide();
            else
                this.Show();

            this.Activate();
            Application.DoEvents();
        }

        private void frmStatus_Activated(object sender, EventArgs e)
        {
            this.Refresh();
            Application.DoEvents();
        }
    }
}