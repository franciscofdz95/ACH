using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmJournal : AchSystem.frmBase 
    {
        public frmJournal()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtHoldID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtRefID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtCommissionCategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            cboRefcode.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadRefCodes(cboRefcode);

            this.Data = new DataJournal();
            this.KeyColumnName = "JournalID";
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

        public override  void FormShow()
        {
            this.Showing = true;

            txtJournalID.Text = this.Dr["Journal ID"].ToString().Trim();
            txtAchID.Text = this.Dr["Ach ID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();

            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();
            txtRefID.Text = this.Dr["Ref ID"].ToString().Trim();
            txtDescription.Text = this.Dr["Description"].ToString().Trim();
            txtHoldID.Text = this.Dr["Hold ID"].ToString().Trim();
            txtAmount.Text = this.Dr["Amount"].ToString().Trim();
            ListHandler.ListFindItem(cboRefcode, this.Dr["Ref Code"].ToString().Trim());
            txtCommissionCategory.Text = this.Dr["Commission Category"].ToString().Trim();

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
            btnMerchant.Enabled = true;

            txtPostedDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtJournalID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtPostedDate.Text = string.Empty;
            txtRefID.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtHoldID.Text = string.Empty;
            txtAmount.Text = string.Empty;
            cboRefcode.SelectedIndex = -1;
            txtCommissionCategory.Text = string.Empty;
            txtMerchantName.Text = string.Empty;
            txtMerchantID.Text = string.Empty; 

            this.Showing = false;
        }

  
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();
            AchListItem item = (AchListItem) cboRefcode.SelectedItem;

            SqlParameter prm = new SqlParameter("@JournalID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", txtAchID.Text));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@Description", txtDescription.Text));
            prms.Add(new SqlParameter("@RefID", DataLayer.Int2Field(txtRefID.Text)));
            prms.Add(new SqlParameter("@RefCode", item.ItemValue));
            prms.Add(new SqlParameter("@HoldID", DataLayer.Int2Field(txtHoldID.Text)));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@CommissionCategory", DataLayer.Int2Field(txtCommissionCategory.Text)));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID ));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                txtAchID.ReadOnly = true;
                btnMerchant.Enabled = false;
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
            AchListItem item;

            item = (AchListItem) cboRefcode.SelectedItem;

            prms.Add(new SqlParameter("@JournalID", DataLayer.Int2Field(txtJournalID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@Description", txtDescription.Text));
            prms.Add(new SqlParameter("@RefID", DataLayer.Int2Field(txtRefID.Text)));
            prms.Add(new SqlParameter("@RefCode", item.ItemValue));
            prms.Add(new SqlParameter("@HoldID", DataLayer.Int2Field(txtHoldID.Text)));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@CommissionCategory", DataLayer.Int2Field(txtCommissionCategory.Text)));
            prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));

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
            btnMerchant.Enabled = false;

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

            if (!DataLayer.IsDate(txtPostedDate.Text.Trim()))
                strError += "Please enter a valid Posted Date.\n";

            if (txtDescription.Text.Trim() == string.Empty)
                strError += "Please enter an Description.\n";

            if (cboRefcode.Text.Trim() == string.Empty)
                strError += "Please select a Refcode.\n";

            if (txtAmount.Text.Trim() == string.Empty)
                strError += "Please select a Amount.\n";

            if (txtAmount.Text.Trim() != string.Empty)
                if (!DataLayer.IsNumeric(txtAmount.Text))
                    strError += "Please enter a valid Amount.\n";

            if (txtCommissionCategory.Text.Trim() == string.Empty)
                strError += "Please enter a Commission Category.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmJournal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

        private void lnkHoldID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtHoldID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmHold(), DataLayer.Int2Field(txtHoldID.Text));
        }

        
        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row,pnlMain);
        }

   
        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row, pnlMain);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cboRefcode.SelectedIndex == -1)
                return;

            AchListItem item = (AchListItem)cboRefcode.SelectedItem;

            switch (item.ItemValue )
            { 
                case "F":
                case "B":
                    FormHandler.OpenDataForm(new frmBatch(), DataLayer.Int2Field(txtRefID.Text));
                    break;
                case "X":
                case "H":
                    FormHandler.OpenDataForm(new frmJournal(), DataLayer.Int2Field(txtRefID.Text));
                    break;
                case "C":
                case "P":
                    FormHandler.OpenDataForm(new frmEFT(), DataLayer.Int2Field(txtRefID.Text));
                    break;
            }
        }

 
    }
}