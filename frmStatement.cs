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
    public partial class frmStatement : Form
    {
        public frmStatement()
        {
            InitializeComponent();
        }

        private void frmStatement_Load(object sender, EventArgs e)
        {
        }

        public bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtAchID.Text.Trim() == string.Empty)
                strError += "Please enter an ACH ID.\n";

            if (cboPeriod.SelectedIndex == -1)
                strError += "Please select a Period.\n";


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

            string[] str = cboPeriod.Text.Split(new string[] { " - " },StringSplitOptions.None);

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Statement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text)));
            cmd.Parameters.Add(new SqlParameter("@Period", str[1] +"/1/" + str[0]));

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grdStatement.DataSource = bs;

            grdStatement.DisplayLayout.Override.CellClickAction = CellClickAction.RowSelect;
            grdStatement.DisplayLayout.Override.SelectTypeRow = SelectType.Extended;
            grdStatement.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            grdStatement.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            grdStatement.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            grdStatement.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            grdStatement.DisplayLayout.GroupByBox.Hidden = true;

            grdStatement.DisplayLayout.Bands[0].Columns["Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grdStatement.DisplayLayout.Bands[0].Columns["Date"].Width = 125;
            grdStatement.DisplayLayout.Bands[0].Columns["Description"].Width = 400;
            grdStatement.DisplayLayout.Bands[0].Columns["Amount"].Width = 100;
            grdStatement.DisplayLayout.Bands[0].Columns["Available"].Width = 100;
            grdStatement.DisplayLayout.Bands[0].Columns["Balance"].Width = 100;
            grdStatement.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
            grdStatement.DisplayLayout.Bands[0].Columns["Refcode"].Hidden  = true;
            grdStatement.DisplayLayout.Bands[0].Columns["Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grdStatement.DisplayLayout.Bands[0].Columns["Available"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grdStatement.DisplayLayout.Bands[0].Columns["Balance"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grdStatement.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            grdStatement.DisplayLayout.Bands[0].Columns["Available"].Format = "C";
            grdStatement.DisplayLayout.Bands[0].Columns["Balance"].Format = "C";

        
        }

        private void grdStatement_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            switch (grdStatement.Rows[e.RowIndex].Cells["Refcode"].Value.ToString())
            {
                case "1":
                case "9":
                    //grdStatement.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gray;
                    //grdStatement.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
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
                this.txtAchID_TextChanged(txtAchID, new EventArgs());
                //this.LoadReport();
            }

        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            FormHandler.ExportGridToExcel(grdStatement);
        }

        private void grdStatement_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            switch (e.Row.Cells["Refcode"].Value.ToString())
            {
                case "1":
                case "9":
                    e.Row.Appearance.BackColor = Color.Gray;
                    e.Row.Appearance.ForeColor = Color.White;
                    break;

            }
        }

        private void grdStatement_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.HeaderClickAction = HeaderClickAction.Select;
            e.Layout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row, pnlMain);
        }

        //private void txtAchID_Leave(object sender, EventArgs e)
        //{
        //    if (txtAchID.Text == string.Empty)
        //        return;

        //    LookUpTableHandler.LoadStatementPeriod(cboPeriod, Convert.ToInt32(txtAchID.Text));

        //    if (cboPeriod.Items.Count > 0)
        //        cboPeriod.SelectedIndex = 0;

        //    UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
        //    FormHandler.PopulateMerchantInfo(row, pnlMain);

        //    this.LoadReport();

        //}

        private void txtAchID_TextChanged(object sender, EventArgs e)
        {
            if (txtAchID.Text.Length < 4)
            {
                
                return;
            }

            LookUpTableHandler.LoadStatementPeriod(cboPeriod, Convert.ToInt32(txtAchID.Text));

            if (cboPeriod.Items.Count > 0)
                cboPeriod.SelectedIndex = 0;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row, pnlMain);

            this.LoadReport();
        }

     

      
    }
}