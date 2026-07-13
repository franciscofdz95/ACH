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
    public partial class frmReleaseAccountBlock : Form
    {
        private DataSet m_Data = null;

        public frmReleaseAccountBlock()
        {
            InitializeComponent();
        }

        private void FormSearch()
        {
            ArrayList prms = new ArrayList();

            if (txtMerchantID.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@MerchantID", txtMerchantID.Text));
            }

            if (txtAchID.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@PriID", txtAchID.Text));
            }

            if (txtTransRoute.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
            }

            if (txtAccountNo.Text.Trim() != string.Empty)
            {
                prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            }

            if (cboTransType.SelectedIndex != -1)
            {
                prms.Add(new SqlParameter("@TransType", cboTransType.Text));
            }

            DataBank data = new DataBank();
            m_Data = data.SearchPriRoutingTable(prms);

            BindingSource bs = new BindingSource();
            bs.DataSource = m_Data.Tables[0];

            grdSearch.DataSource = bs;

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            {
                style.BackColor = Color.LemonChiffon;
            }

            // Apply the style as the default cell style.
            grdSearch.AlternatingRowsDefaultCellStyle = style;

        }

        private void frmReleaseAccountBlock_Load(object sender, EventArgs e)
        {
            this.LoadTransType();
            this.FormSearch();
        }

        private void LoadTransType()
        {
            cboTransType.Items.Add(new AchListItem(1, "22"));
            cboTransType.Items.Add(new AchListItem(2, "27"));
            cboTransType.Items.Add(new AchListItem(3, "32"));
            cboTransType.Items.Add(new AchListItem(4, "37"));
        }

        private void FormClear()
        {
            txtAchID.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtTransRoute.Text = string.Empty;
            cboTransType.SelectedIndex = -1;

            this.FormSearch();
        }

        private bool FormCheck()
        {
            string strError = string.Empty;

            if (txtAchID.Text.Trim() == string.Empty  && txtMerchantID.Text.Trim() ==string.Empty )
            {
                strError += "Please enter either an ACH ID or a Merchant ID.\n";
            }

            if (txtTransRoute.Text.Trim() == string.Empty)
            {
                strError += "Please enter a Trans Route Number.\n";
            }

            if (txtAccountNo.Text.Trim() == string.Empty)
            {
                strError += "Please enter an Account Number.\n";
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
                case "Release Account Block":
                    frmUpdatePRIRoutingTable frm = new frmUpdatePRIRoutingTable(e.ClickedItem.Text, m_Data);
                    frm.ShowDialog();
                    frm = null;
                    this.FormSearch();
                    break;
                case "Add Account Block":
                    frmUpdatePRIRoutingTable frm2 = new frmUpdatePRIRoutingTable(e.ClickedItem.Text, m_Data);
                    frm2.ShowDialog();
                    if (frm2.tabMain.SelectedIndex == 1)
                    {
                        txtAchID.Text = frm2.txtAchID.Text;
                        txtTransRoute.Text = frm2.txtTransRoute.Text;
                        txtAccountNo.Text = frm2.txtAccountNo.Text;
                        cboTransType.Text = frm2.cboTransType.Text;
                    }
                    frm2 = null;
                    this.FormSearch();
                    break;
            }
        }

        private void frmReleaseAccountBlock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.FormCheck())
                    this.FormSearch();
            }
        }

       
    }
}