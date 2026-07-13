using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmBatchDetail : AchSystem.frmBase 
    {
        public frmBatchDetail()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboSource.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            this.Data = new DataBatch();
            this.KeyColumnName = "TraceNumber";
            FormHandler.SetSecurity(this);

        }

         public override bool FormFind()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@" + this.KeyColumnName, this.ID));
            this.Dr = this.Data.Select(prms);

            if (this.Dr.Read())
                return true;
            else
                return false;
        }

        public override void FormShow()
        {
            this.Showing = true;

            txtBatchID.Text = this.Dr["Batch ID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtAchID.Text = this.Dr["Ach ID"].ToString().Trim();
            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();

            FormHandler.PopulateControlTag(this);

            this.Showing = false;
        }

        public override void FormOpen(UltraGridRow dr)
        {
            if (dr != null)
            {
                this.ID = Convert.ToInt64(dr.Cells[this.KeyColumnName].Value);
                if (this.FormFind())
                    this.FormShow();

                this.ShowDialog();
            }
            else
            {
                if (this.ID == -1)
                    if (!tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled )
                        this.Close();
            }
        }

        public override void FormNew()
        {
            this.Adding = true;
            this.FormClear();
            FormHandler.ClearControlTag(this);
            this.FormToggleButtons();

            txtAchID.ReadOnly = false;
            txtMerchantID.ReadOnly = false;
            txtPostedDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtBatchID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtPostedDate.Text = string.Empty;

            txtBatchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtPostedDate.Text = string.Empty;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@BatchID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));

            long lngID = this.Data.Insert (prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                txtAchID.ReadOnly = true;
                txtMerchantID.ReadOnly = true;
                this.ID = lngID;
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool FormUpdate()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@BatchID", Convert.ToInt32(txtBatchID.Text)));
            prms.Add(new SqlParameter("@MerchantID", Convert.ToInt32(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));

            int intRows = this.Data.Update(prms);

            if (intRows > 0)
            {
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void FormUndo()
        {
            this.Adding = false;

            this.FormToggleButtons();
            txtAchID.ReadOnly = true;
            txtMerchantID.ReadOnly = true;

            if (this.ID != -1)
            {
                if (this.FormFind())
                    this.FormShow();
            }
            else
            {
                this.Close();
            }
        }

        public override bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtAchID.Text.Trim() == string.Empty)
                strError += "Please enter an ACH ID.\n";

            if (txtPostedDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtPostedDate.Text.Trim()))
                    strError += "Please enter a valid Posted Date.\n";

            
            if (strError == string.Empty)
                return true;
            else
            {
                MessageBox.Show(strError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private void frmBatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }
    }
}