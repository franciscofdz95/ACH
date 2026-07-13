using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmHoldRelease : Form
    {
        public frmHoldRelease()
        {
            InitializeComponent();

            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadBankID(cboBankID);
        }

        private void frmHoldRelease_Load(object sender, EventArgs e)
        {
        }

        public bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtDate.Text.Trim() == string.Empty)
                strError += "Please select release date\n";


            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void LoadReport()
        {
            this.Cursor = Cursors.WaitCursor;

            if (!this.FormDataCheck())
            {
                return;
            }
            AchListItem item = null;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Search_Hold_Release";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@HoldToRelease", txtDate.Text));

            if (txtAchID.Text.Trim() != string.Empty)
                cmd.Parameters.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text)));

            if (cboBankID.SelectedIndex != -1)
            {
                item = (AchListItem)cboBankID.SelectedItem;
                cmd.Parameters.Add(new SqlParameter("@BankID", item.ItemValue));
            }

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grdHoldReleases.DataSource = bs;



            //grdHoldReleases.Columns["Date"].Width = 125;
            //grdHoldReleases.Columns["Description"].Width = 400;
            //grdHoldReleases.Columns["Amount"].Width = 100;
            //grdHoldReleases.Columns["Available"].Width = 100;
            //grdHoldReleases.Columns["Balance"].Width = 100;
            //grdHoldReleases.Columns["Refcode"].Visible = false;
            //grdHoldReleases.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //grdHoldReleases.Columns["Available"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //grdHoldReleases.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            //grdHoldReleases.Columns["Amount"].DefaultCellStyle.Format = "C";
            //grdHoldReleases.Columns["Available"].DefaultCellStyle.Format = "C";
            //grdHoldReleases.Columns["Balance"].DefaultCellStyle.Format = "C";


            this.Cursor = Cursors.Default;
        }

        //private void grdStatement_RowPrePaint(object sender, UltraGridRowPrePaintEventArgs e)
        //{
        //    switch (grdHoldReleases.Rows[e.RowIndex].Cells["Refcode"].Value.ToString())
        //    {
        //        case "1":
        //        case "9":
        //            grdHoldReleases.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gray;
        //            grdHoldReleases.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
        //            break;

        //    }

        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadReport();
        }

        private void frmHoldRelease_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.LoadReport();
            }

        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            FormHandler.ExportGridToExcel(grdHoldReleases);
        }

        private void grdHoldReleases_InitializeLayout(object sender, InitializeLayoutEventArgs e)
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