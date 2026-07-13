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
    public partial class frmSettleFileSummaryReport : Form
    {

        public frmSettleFileSummaryReport()
        {
            InitializeComponent();
            LookUpTableHandler.LoadBankID(cboBankID);
        }

        private void frmSettleFileSummaryReport_Load(object sender, EventArgs e)
        {
        }

        public bool FormDataCheck()
        {
            string strError = string.Empty;

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
            if (!this.FormDataCheck())
            {
                return;
            }


            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Recon_BankFile_Summary";
            cmd.CommandType = CommandType.StoredProcedure;

            if (cboBankID.SelectedIndex != -1)
            {
                AchListItem item = (AchListItem) cboBankID.SelectedItem;
                cmd.Parameters.Add(new SqlParameter("@BankID", item.ItemValue));
            }

            cmd.Parameters.Add(new SqlParameter("@BeginPostedDate", txtPostBeginDate.Value.ToString("MM/dd/yyyy")));
            cmd.Parameters.Add(new SqlParameter("@EndPostedDate", txtPostEndDate.Value.ToString("MM/dd/yyyy")));

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grd.DataSource = bs;

            grd.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
            grd.DisplayLayout.Override.SelectTypeRow = SelectType.Extended;
            grd.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grd.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grd.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            grd.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grd.DisplayLayout.GroupByBox.Hidden = true;

            //grd.DisplayLayout.Bands[0].Columns["Date"].Format = "MM/dd/yyyy HH:mm:ss";
            //grd.DisplayLayout.Bands[0].Columns["Date"].Width = 125;
            //grd.DisplayLayout.Bands[0].Columns["Description"].Width = 400;
            //grd.DisplayLayout.Bands[0].Columns["Amount"].Width = 100;
            //grd.DisplayLayout.Bands[0].Columns["Available"].Width = 100;
            //grd.DisplayLayout.Bands[0].Columns["Balance"].Width = 100;
            //grd.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
            //grd.DisplayLayout.Bands[0].Columns["Refcode"].Hidden  = true;
            //grd.DisplayLayout.Bands[0].Columns["Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Columns["Available"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Columns["Balance"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Debit Count"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Credit Count"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grd.DisplayLayout.Bands[0].Columns["Debit Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Credit Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["OverDraft Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Hold Release Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Total Debit Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Total Credit Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Net Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;


            grd.DisplayLayout.Bands[0].Columns["Debit Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Credit Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["OverDraft Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Hold Release Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Total Debit Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Total Credit Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Net Amount"].Format = "C";

        
        }

        private void grd_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            switch (grd.Rows[e.RowIndex].Cells["Refcode"].Value.ToString())
            {
                case "1":
                case "9":
                    //grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gray;
                    //grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                    break;

            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.LoadReport();
        }

        private void frmStatement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.LoadReport();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormHandler.ExportGridToExcel(grd);
        }

        private void grd_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
        }

        private void grd_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
            e.Layout.Override.HeaderClickAction = HeaderClickAction.Select;
            e.Layout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
        }

        private void grd_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            int BankID = Convert.ToInt32(grd.ActiveRow.Cells["BankID"].Text);
            DateTime date = Convert.ToDateTime(grd.ActiveRow.Cells["Posted Date"].Text);

            frmReportNew frm = new frmReportNew();
            frm.ShowSettlementFile(BankID, date);
            frm.ShowDialog();

            this.Cursor = Cursors.Default;


            //frmSettleFileDetailsReport frm = new frmSettleFileDetailsReport(BankID, date);
            //frm.ShowDialog();
        }
    

      
    }
}