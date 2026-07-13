using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;

namespace ACH2005
{
    public partial class frmPurgeBatch : Form
    {
        private DataSet m_Data = null;

        public frmPurgeBatch()
        {
            InitializeComponent();
        }

        private void FormSearch()
        {
            this.Cursor = Cursors.WaitCursor;

            ArrayList prms = new ArrayList();

            if (txtAchID.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@PriID", txtAchID.Text));
            }

            if (txtMerchantID.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@MerchantID", txtMerchantID.Text));
            }

            if (txtBatchDate.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@BatchDate", txtBatchDate.Value.Date ));
            }

            DataTransaction data = new DataTransaction();
            m_Data = data.SearchUploadBatch(prms);
            
            BindingSource bs = new BindingSource();
            bs.DataSource = m_Data.Tables[0];
            
            grdSearch.DataSource = bs;

            this.Cursor = Cursors.Default ;
        }

        private void FormClear()
        {
            txtAchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtBatchDate.Value  = DateTime.Today;

            this.FormSearch();
        }

        private void frmChangeTransStatus_Load(object sender, EventArgs e)
        {
            this.FormSearch();
        }

        
        private void tbrTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "Search":
                    if (this.FormCheck())
                        this.FormSearch();
                    break;
                case "Clear":
                    this.FormClear();
                    break;
                case "Delete Batch":
                    if (this.FormDelete())
                        this.FormSearch();
                    break;
            }
        }
        private bool FormDelete()
        {
            if (grdSearch.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a batch.",Application.ProductName, MessageBoxButtons.OK , MessageBoxIcon.Information );
                return false;
            }


            ArrayList prms = new ArrayList();
            DataTransaction data = new DataTransaction();

            DataGridViewRow dr = grdSearch.SelectedRows[0];

            if (Convert.ToInt32(dr.Cells["Total Record Count"].Value) != Convert.ToInt32(dr.Cells["Total Open Record"].Value))
            {
                MessageBox.Show("Unable to delete batch.  Total Record Count and Total Open Record must equal.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            DialogResult result;
            result = MessageBox.Show("Are you sure you want to delete Upload ID " + dr.Cells["Upload ID"].Value.ToString() + ".", Application.ProductName, MessageBoxButtons.YesNo , MessageBoxIcon.Question );
            if (result == DialogResult.No)
            {
                return false;
            }

            prms.Add(new SqlParameter("@PriID", Convert.ToInt32(dr.Cells["ACH ID"].Value)));
            prms.Add(new SqlParameter("@UploadID", Convert.ToInt32(dr.Cells["Upload ID"].Value)));
            prms.Add(new SqlParameter("@UserID", main.Current_User));

            if (data.DeleteUploadBatch(prms) == 0)
                MessageBox.Show("Failed to delete Upload ID " + dr.Cells["Upload ID"].Value.ToString() + ".", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information );

            return true;
     
        }

        private bool FormCheck()
        {
            string strError = string.Empty;

            if (txtAchID.Text.Trim() == string.Empty && txtMerchantID.Text.Trim() == string.Empty )
            {
                strError += "Please enter either an ACH ID or a Merchant ID.\n";
            }

            if (strError != string.Empty)
            {
                MessageBox.Show(strError, Application.ProductName , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void grdSearch_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            if (Convert.ToInt32(grdSearch.Rows[e.RowIndex].Cells["Total Record Count"].Value) == 
                    Convert.ToInt32(grdSearch.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grdSearch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grdSearch.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grdSearch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grdSearch.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }

        }

        private void frmPurgeBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.FormCheck())
                    this.FormSearch();
            }
        }

    

       

    }
}