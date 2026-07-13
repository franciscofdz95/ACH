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
using Infragistics.Win.Misc;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmEFT : AchSystem.frmBase 
    {

        public frmEFT()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtJournalIDIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            cboEFTDescription.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadTransactionTransType(cboTransType);
            LookUpTableHandler.EFTTypeDescription(cboEFTDescription);
            LookUpTableHandler.LoadPaymentStatus(cboStatus);
            LookUpTableHandler.LoadTransSource(cboSource);

            this.Data = new DataPayment();
            this.KeyColumnName = "TransID";
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

            txtTransID.Text = this.Dr["Trans ID"].ToString().Trim();
            txtAchID.Text = this.Dr["Ach ID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();

            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();
            txtAmount.Text = this.Dr["Amount"].ToString().Trim();
            txtDateProcessed.Text = this.Dr["Date Processed"].ToString().Trim();
            txtJournalIDIn.Text = this.Dr["Journal ID"].ToString().Trim();
            txtAchTransRoute.Text = this.Dr["Trans Route"].ToString().Trim();
            txtAchAccountNo.Text = this.Dr["Account No"].ToString().Trim();
            txtAchAccountName.Text = this.Dr["Account Name"].ToString().Trim();
            txtEncryptAccount.Text = this.Dr["Encrypt Account No"].ToString().Trim();
            txtBatchID.Text = this.Dr["Batch ID"].ToString().Trim();

            ListHandler.ListFindItem(cboTransType , this.Dr["Trans Type"].ToString().Trim());
            ListHandler.ListFindItem(cboEFTDescription, this.Dr["Description"].ToString().Trim());
            ListHandler.ListFindItem(cboStatus, this.Dr["Status ID"].ToString().Trim());
            ListHandler.ListFindItem(cboSource, this.Dr["Source"].ToString().Trim());

            FormHandler.PopulateControlTag(this);

            if (!main.g_User.IsAdmin)
            {
                txtAchAccountNo.Text = this.Dr["MaskedAccountNo"].ToString().Trim();
            }

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
            txtJournalIDIn.ReadOnly = true;
            ListHandler.ListFindItem(cboStatus, "2");
            ListHandler.ListFindItem(cboSource, "P");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtTransID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtPostedDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            txtAmount.Text = string.Empty;
            txtDateProcessed.Text = string.Empty;
            txtJournalIDIn.Text = "0";
            txtAchTransRoute.Text = string.Empty;
            txtAchAccountNo.Text = string.Empty;
            txtAchAccountName.Text = string.Empty;
            cboEFTDescription.SelectedIndex = -1;
            cboTransType.SelectedIndex = -1;
            txtEncryptAccount.Text = string.Empty;
            txtMerchantName.Text = string.Empty;
            txtMerchantID.Text = string.Empty; 

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();
            AchListItem item;

            SqlParameter prm = new SqlParameter("@TransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text)));
            prms.Add(new SqlParameter("@MerchantID", Convert.ToInt32(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@DateProcessed", DataLayer.Date2Field(txtDateProcessed.Text)));
            prms.Add(new SqlParameter("@Amount", Convert.ToDecimal(txtAmount.Text)));
            item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            item = (AchListItem)cboStatus.SelectedItem;
            prms.Add(new SqlParameter("@StatusID", item.ItemValue));
            prms.Add(new SqlParameter("@TransRoute", txtAchTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAchAccountNo.Text));
            prms.Add(new SqlParameter("@NameOnAcct", txtAchAccountName.Text));
            item = (AchListItem)cboEFTDescription.SelectedItem;
            prms.Add(new SqlParameter("@Description", item.ItemValue));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID ));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                txtAchID.ReadOnly = true;
                btnMerchant.Enabled = false;
                txtJournalIDIn.ReadOnly = false;
                this.ID = lngID;
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                FormHandler.DispalyInformationMessage("EFT and Journal record have been created!");
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

            prms.Add(new SqlParameter("@TransID", txtTransID.Text));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@MerchantID", Convert.ToInt32(txtMerchantID.Text)));
            prms.Add(new SqlParameter("@TransDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@DateProcessed", DataLayer.Date2Field(txtDateProcessed.Text)));
            prms.Add(new SqlParameter("@Amount", Convert.ToDecimal(txtAmount.Text)));
            item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            item = (AchListItem)cboStatus.SelectedItem;
            prms.Add(new SqlParameter("@StatusID", item.ItemValue));
            prms.Add(new SqlParameter("@JournalID", DataLayer.Int2Field(txtJournalIDIn.Text)));
            prms.Add(new SqlParameter("@TransRoute", txtAchTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAchAccountNo.Text));
            prms.Add(new SqlParameter("@NameOnAccount", txtAchAccountName.Text));
            item = (AchListItem)cboEFTDescription.SelectedItem;
            prms.Add(new SqlParameter("@Description", item.ItemValue));
            prms.Add(new SqlParameter("@BatchID", DataLayer.Int2Field(txtBatchID.Text)));

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

            if (txtDateProcessed.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtDateProcessed.Text.Trim()))
                    strError += "Please enter a valid Processed Date.\n";

            if (txtAmount.Text.Trim() == string.Empty )
                strError += "Please enter an Amount.\n";

            if (txtAmount.Text.Trim() != string.Empty)
                if (!DataLayer.IsNumeric(txtAmount.Text))
                    strError += "Please enter a valid Amount.\n";

            if (cboTransType.Text.Trim() == string.Empty)
                strError += "Please select a Trans Type.\n";

            if (txtAchTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a Trans Route.\n";

            if (txtAchAccountNo.Text.Trim() == string.Empty)
                strError += "Please enter an Account No.\n";

            if (txtAchAccountName .Text.Trim() == string.Empty)
                strError += "Please enter an Account Name.\n";

            if (cboEFTDescription.SelectedIndex == -1)
                strError += "Please enter an EFT Decription.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmEFT_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }


        private void lnkJournalIDIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtJournalIDIn.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmJournal(), DataLayer.Int2Field(txtJournalIDIn.Text));
        }

        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row,pnlMain );

        }

        private void btnAchID_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row,pnlMain);
        }

        private void frmEFT_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

     
   

    }
}