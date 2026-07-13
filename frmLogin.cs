using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using Nmc.Ach.Dal;
using System.Collections;

namespace AchSystem
{
    public partial class frmLogin : Form
    {

        
        public frmLogin()
        {

            
            //DataAchProcess.InsertCommunication("subject", "body", "bodyhtml", "admin@merituspayment.com", "mnguyen@merituspayment.com", "", "");


            InitializeComponent();

            Bank bank = new BankNcal();

            //bank.WriteReturnFile(7879, 21327, DateTime.Today);

            string[] strFiles;
            strFiles = Directory.GetFiles(Application.StartupPath,"grd_*");

            foreach (string strFile in strFiles)
            {
                DeleteFile(strFile);
            }

            //txtUserName.Text = "test";
            //txtPassword.Text = "password1";


            //txtUserName.Text = "mnguyen";
            //txtPassword.Text = "hung1441";


            lblVersion.Text = "Version 3.9.35";




            //ArrayList sb = new ArrayList();

            
            //foreach (object obj in sb)
            //{
            //    if (!File.Exists(obj.ToString()))
            //    {
            //        Console.WriteLine(obj.ToString());
            //    }
            //}
        }

        private void DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (System.IO.IOException exc)
            {
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.FormDataCheck())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                txtUserName.Focus();

            
        }

        public bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtUserName.Text.Trim() == string.Empty)
                strError += "Please enter a User Name.\n";

            if (txtPassword.Text.Trim() == string.Empty)
                strError += "Please enter a Password.\n";

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
        }
    }
}