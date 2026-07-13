using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects;
using System.Linq;

namespace AchSystem
{
    public partial class frmLoadReturns : Form
    {
        private string m_ReturnFileDirectory = ConfigurationManager.AppSettings["ReturnFileDirectory"].ToString();
        private string m_ReturnFileOldDirectory = ConfigurationManager.AppSettings["ReturnFileOldDirectory"].ToString();


        public frmLoadReturns()
        {
            InitializeComponent();

            LookUpTableHandler.LoadBanks(cboBank);

            if (cboBank.Items.Count > 0)
                cboBank.SelectedIndex = 0;

            txtDirectory.Text = m_ReturnFileDirectory;


            //ManuallyWriteReturnFile(6960, 10240);
        }






        public bool FormDataCheck(string strFile)
        {
            string strError = string.Empty;

            DataBatchFileLog data = new DataBatchFileLog();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@FileName", strFile));
            SqlDataReader dr = data.Select(prms);

            if (dr.Read())
                strError += "File " + strFile + " has already been loaded.\n";

            dr.Close();
            dr = null;
            data = null;

            if (strError == string.Empty)
                return true;
            else
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            if (lstFiles.Items.Count > 0)
            {
                for (int i = 0; i < lstFiles.Items.Count; i++)
                {
                    FileInfo file = new FileInfo(lstFiles.Items[i].ToString());

                    if (this.FormDataCheck(file.Name))
                        if (bank.CreateReturns(file))
                        {
                            if (!File.Exists(m_ReturnFileOldDirectory + file.Name))
                                file.MoveTo(m_ReturnFileOldDirectory + file.Name);
                            else
                                file.MoveTo(m_ReturnFileOldDirectory + file.Name + "_" + DateTime.Now.ToString("MMddyyyy HHmmss"));
                        }
                }

                //this.SendPReturnEmail();
            }
            else
            {
                //If there are no return files, the statements below will create journals for scrubbed returns
                DataAchProcess data = new DataAchProcess();
                bank.CurrentJobID = data.GetNextJobID("Create Returns for BankID " + bank.BankID.ToString());
                data.TruncateStagingReturn();
                bank.CreateReturnsAndJournals();
                bank.ProcessCreateReturnEmails();
                //bank.ProcessCreateReturnFiles();

                ArrayList prms = new ArrayList();
                prms.Add(new SqlParameter("@BankID", bank.BankID));
                data.UpdateReturnPrintedFlag(prms);

                FormHandler.DispalyInformationMessage("Scrub returns created successfully for bank " + bank.BankName + ".");

                data = null;
                bank = null;
            }

            this.LoadPendingScrubbedReturns();
            this.Cursor = Cursors.Default;
        }



        private void frmLoadReturns_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void tabMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            string message = string.Empty;

            btnFinish.Enabled = false;
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
                        
            switch (e.Tab.Key)
            {
                case "SelectBank":
                    message = "Please select a bank to process.";
                    btnPrevious.Enabled = false;
                    break;
                case "ReturnFiles":
                    message = "Return file(s) in the list will be imported for bank " + cboBank.Text;
                    break;
                case "Import":
                    message = "Please click the Import and Create Returns button to process file " + txtDirectory.Text + ".";
                    btnFinish.Enabled = true;
                    btnNext.Enabled = false;
                    this.LoadPendingScrubbedReturns();
                    break;
            }

            txtMessage.Text = message;
        }


        private void SendPReturnEmail()
        {
            this.Cursor = Cursors.WaitCursor;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();
            int rows = data.SendPReturnEmail(prms);

            this.Cursor = Cursors.Default;

        }

        private void LoadPendingScrubbedReturns()
        {
            this.Cursor = Cursors.WaitCursor;

            Bank bank = (Bank)cboBank.SelectedItem;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@BankID", bank.BankID));
            DataSet ds = data.SelectPendingScrubbedReturns(prms);

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grd.DataSource = bs;

            bank = null;
            data = null;

            this.Cursor = Cursors.Default;

        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {


            fbdMain.ShowDialog();

            if (fbdMain.SelectedPath == string.Empty)
                return;

            Bank bank = (Bank)cboBank.SelectedItem;
            this.DisplayReturnFiles(fbdMain.SelectedPath, bank.BankID);
        }

        private void DisplayReturnFiles(string directory, int bankID)
        {
            var aptReturnFileStartWith = "ach-inbound";
            if (ConfigurationManager.AppSettings.AllKeys.Contains("AptPayReturnFileStartWith"))
                aptReturnFileStartWith = ConfigurationManager.AppSettings["AptPayReturnFileStartWith"].ToString();

            var bankFileFilters = new Dictionary<AchBankInfo, Func<string, bool>>
            {
                { AchBankInfo.NCAL, FileName => FileName.StartsWith("meritusach", StringComparison.OrdinalIgnoreCase) || FileName.StartsWith("meritusbcd", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.KBT, FileName => FileName.StartsWith("meritus", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.CHASE, FileName => FileName.StartsWith("aymaret", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.WELLS, FileName => FileName.StartsWith("d", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.MB_FINANCIAL, FileName => FileName.StartsWith("otkgrt", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.FPS, FileName => FileName.StartsWith("s", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.GOLDMAN_SACHS, FileName => FileName.StartsWith("gsbach", StringComparison.OrdinalIgnoreCase) },
                { AchBankInfo.APT_PAY, FileName => FileName.StartsWith(aptReturnFileStartWith, StringComparison.OrdinalIgnoreCase) } //TODO_RV: 
            };
            lstFiles.Items.Clear();
            Func<string, bool> fileFilter;
            if (bankFileFilters.TryGetValue((AchBankInfo)bankID, out fileFilter))
            {
                var files = Directory.EnumerateFiles(directory).Where(f => fileFilter(Path.GetFileName(f))).ToArray();
                lstFiles.Items.AddRange(files);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Bank bank = (Bank)cboBank.SelectedItem;

            switch (tabMain.SelectedTab.Index)
            {
                case 0:
                    if (cboBank.SelectedIndex == -1)
                    {
                        FormHandler.DispalyWarningMessage("Please select a bank.");
                        return;
                    }

                    this.DisplayReturnFiles(txtDirectory.Text, bank.BankID);


                    //Check if this is the first day of the month.  Make sure monthly process has been done execute prior to loading returns.
                    if (DateTime.Today.Day == 1)
                    {
                        string sql = "select count(*) from Ach_Journal with (Nolock) where ";
                        sql += "PostedDate >= dateadd(day,-1,'" + DateTime.Today.ToString("MM/dd/yyyy") + "') ";
                        sql += "And PostedDate < '" + DateTime.Today.ToString("MM/dd/yyyy") + "' ";
                        sql += "And EntryDescription Like '%Account Maintenance Fee%' ";

                        string rows = DataLayer.ExecuteScalar(sql, DataLayer.ConnectStringBuild());

                        if (int.Parse(rows) == 0)
                        {
                            MessageBox.Show("Monthly process has been run for the first day of the month. Please contact admin.", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);                            
                            return;
                        }
                    }

                    break;
                case 1:



                    break;
                //if (lstFiles.Items.Count == 0)
                //{
                //    FormHandler.DispalyWarningMessage("No return files to process.");
                //    return;
                //}
                //break;
                case 2:
                case 3:
                    break;
            }

            if (tabMain.ActiveTab.Index < tabMain.Tabs.Count - 1)
                tabMain.SelectedTab = tabMain.Tabs[tabMain.ActiveTab.Index + 1];
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (tabMain.ActiveTab.Index != 0)
                tabMain.SelectedTab = tabMain.Tabs[tabMain.ActiveTab.Index - 1];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            txtDirectory.Text = m_ReturnFileDirectory;
            Bank bank = (Bank)cboBank.SelectedItem;
            this.DisplayReturnFiles(txtDirectory.Text, bank.BankID);

        }

        private void grd_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Extended;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;

        }
    }
}