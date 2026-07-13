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
    public partial class frmChangeTransStatus : Form
    {
        private DataSet m_Data = null;

        public frmChangeTransStatus()
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

            if (txtTransBeginDate.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@BeginPostedDate", txtTransBeginDate.Value.Date ));
            }

            if (txtTransEndDate.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@EndPostedDate", txtTransEndDate.Value.Date));
            }

            if (txtAccountNo.Text  != string.Empty)
            {
                prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text ));
            }

            if (txtAccountName.Text != string.Empty)
            {
                prms.Add(new SqlParameter("@AccountName", txtAccountName.Text));
            }

            if (txtTransID.Text != string.Empty)
            {
                prms.Add(new SqlParameter("@TransID", txtTransID.Text));
            }

            if (txtRefID.Text != string.Empty)
            {
                prms.Add(new SqlParameter("@RefID", txtRefID.Text));
            }

            DataTransaction data = new DataTransaction();
            m_Data = data.SearchIncomingTrans(prms);
            
            BindingSource bs = new BindingSource();
            bs.DataSource = m_Data.Tables[0];
            
            grdSearch.DataSource = bs;

            //DataGridViewCellStyle style = new DataGridViewCellStyle();
            //{
            //    style.BackColor = Color.LemonChiffon;
            //}

            //// Apply the style as the default cell style.
            //grdSearch.AlternatingRowsDefaultCellStyle = style;
            this.Cursor = Cursors.Default;

        }

        private void FormClear()
        {
            txtAchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtTransBeginDate.Value  = DateTime.Today;
            txtTransEndDate.Value = DateTime.Today;
            txtAccountName.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtRefID.Text = string.Empty;
            txtTransID.Text = string.Empty;

            this.FormSearch();
        }

        private void frmChangeTransStatus_Load(object sender, EventArgs e)
        {
            this.FormSearch();
        }

        private void tbrTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Form frm;

            switch (e.ClickedItem.Text)
            {
                case "Search":
                    if (this.FormCheck())
                        this.FormSearch();
                    break;
                case "Clear":
                    this.FormClear();
                    break;
                case "Change Status":
                    frm = new frmUpdateTransaction(e.ClickedItem.Text,m_Data);
                    frm.ShowDialog();
                    frm = null;
                    this.FormSearch();
                    break;
                case "Change Process Date":
                    frm = new frmUpdateTransaction(e.ClickedItem.Text, m_Data);
                    frm.ShowDialog();
                    frm = null;
                    this.FormSearch();
                    break;
            }
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
                MessageBox.Show(strError, "Form Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void grdSearch_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            switch (Convert.ToInt32(grdSearch.Rows[e.RowIndex].Cells["Status ID"].Value))
            {
                case 0:
                case 2:
                case 3:
                    grdSearch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    grdSearch.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;
                default:
                    grdSearch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                    grdSearch.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;

            }
        }

        private void frmChangeTransStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.FormCheck())
                    this.FormSearch();
            }
        }

   

    }
}