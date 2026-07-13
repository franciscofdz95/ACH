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
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();

            pnlPassword.Location = new Point(12, 12);
            pnlAdmin.Location = new Point(12, 12);

            pnlPassword.BringToFront();

            chkAdmin.Checked = main.g_User.IsAdmin;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (btnOk.Text == "Ok")
            {
                if (txtPassword.Text == "justin")
                {
                    btnOk.Text = "Apply";
                    pnlAdmin.BringToFront();
                }
            }
            else
            {
                main.g_User.IsAdmin = chkAdmin.Checked;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}