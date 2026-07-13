using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AchSystem
{
    public partial class frmChangePassword : Form
    {
        public frmChangePassword()
        {
            InitializeComponent();
        }

       
        public bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtOldPassword.Text.Trim() == string.Empty)
                strError += "Please enter your old Password.\n";

            if (txtNewPassword.Text.Trim() == string.Empty)
                strError += "Please enter a new Password.\n";

            if (txtConfirmation.Text.Trim() == string.Empty)
                strError += "Please confirm your password.\n";

            if (txtConfirmation.Text.Trim() != txtNewPassword.Text.Trim())
                strError += "New password and confirmation does not match.\n";


            if (strError == string.Empty)
                return true;
            else
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (this.FormDataCheck())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                txtNewPassword.Focus();
        }
    }
}