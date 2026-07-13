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
using Infragistics.Win.UltraWinGrid;
using System.Data.SqlClient;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmMonthEnd : Form
    {


        public frmMonthEnd()
        {
            InitializeComponent();

            DateTime date;

            date = DateTime.Parse(DateTime.Today.Month.ToString() + "/1/" + DateTime.Today.Year.ToString());
            txtBeginDate.Value = date;
            txtEndDate.Value = date.AddMonths(1);

            this.LoadMonthlyBillingJournals();
        }

        private void LoadMonthlyBillingJournals()
        {
            DataPendingJournal data = new DataPendingJournal();
            DataSet ds = data.Search(new ArrayList());

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grd.DataSource = bs;

            grd.DisplayLayout.Bands[0].Columns["PostedDate"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["DateProcessed"].Format = "MM/dd/yyyy HH:mm:ss";

        }
     

        private void frmMonthEnd_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void tabMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            string message = string.Empty;

            btnFinish.Enabled = false;
            btnNext.Enabled = true;
            btnPrevious.Enabled = true;

            switch (e.Tab.Index)
            {
                case 0:
                    message = "The list below displays fees for all merchants on monthly billing. Once the month has been closed these fees will be summarized and reflected on the merchant's statement.";
                    btnPrevious.Enabled = false;
                    break;
                case 1:
                    message = "Please select return file to import for bank ";
                    break;
                case 2:
                    message = "Please click the Import and Create Returns button to process file " + txtDirectory.Text + ".";
                    break;
                case 3:
                    message = "Please create the return files for merchants.";
                    btnFinish.Enabled = true ;
                    btnNext.Enabled = false;

                    break;
            }

            txtMessage.Text = message;
        }
    

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (tabMain.SelectedTab.Index)
            {
                case 0:
                    break;
                case 1:
                    break;
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

        private void grd_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Extended;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
        }

        private void btnCloseMonthEnd_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Month end process should only be executed on the last day of the month.Do you want close month end?");

            if (result == DialogResult.No)
                return;


            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            int JobID =  data.GetNextJobID("Create Journals For Monthly Billing.");
            prms.Add(new SqlParameter("@BeginDate", txtBeginDate.Value));
            prms.Add(new SqlParameter("@EndDate", txtEndDate.Value));
            prms.Add(new SqlParameter("@JobID", JobID));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
            long rows = data.ExecuteMonthlyBilling(prms);

            prms.Clear();
            prms.Add(new SqlParameter("@BeginDate", txtBeginDate.Value));
            prms.Add(new SqlParameter("@EndDate", txtEndDate.Value));
            prms.Add(new SqlParameter("@JobID", JobID));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
            rows = data.ExecuteMonthlyMinimum(prms);

            this.LoadMonthlyBillingJournals();

            FormHandler.DispalyInformationMessage("Close month end completed successfully.");
        }

   
    }
}