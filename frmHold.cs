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
    public partial class frmHold : AchSystem.frmBase 
    {
        public frmHold()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtJournalIDIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtJournalIDOut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            cboReleaseHold.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadHoldTypes(cboType);
            LookUpTableHandler.LoadReleaseHold(cboReleaseHold);

            this.Data = new DataHold();
            this.KeyColumnName = "HoldID";
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

            txtHoldID.Text = this.Dr["Hold ID"].ToString().Trim();
            txtAchID.Text = this.Dr["Ach ID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();

            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();
            txtAmount.Text = this.Dr["Amount"].ToString().Trim();
            txtJournalIDIn.Text = this.Dr["Journal ID In"].ToString().Trim();
            txtJournalIDOut.Text = this.Dr["Journal ID Paid"].ToString().Trim();

            if (this.Dr["Release Date"] == DBNull.Value)
                txtReleaseDate.Text = string.Empty;
            else
                txtReleaseDate.Text = this.Dr["Release Date"].ToString().Trim();


            if (this.Dr["Paid Date"] == DBNull.Value)
                txtPaidDate.Text = string.Empty;
            else
                txtPaidDate.Text = this.Dr["Paid Date"].ToString().Trim();

            ListHandler.ListFindItem(cboType, this.Dr["Type"].ToString().Trim());
            ListHandler.ListFindItem(cboReleaseHold, this.Dr["Release Hold"].ToString().Trim());

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
            txtJournalIDIn.Text = "0";
            txtJournalIDOut.Text = "0";

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtHoldID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtPostedDate.Text = string.Empty;

            txtAmount.Text = string.Empty;
            txtJournalIDIn.Text = string.Empty;
            txtJournalIDOut.Text = string.Empty;
            txtReleaseDate.Text = string.Empty;
            txtPaidDate.Text = string.Empty;
            cboType.SelectedIndex = -1;
            cboReleaseHold.SelectedIndex = -1;
            txtMerchantName.Text = string.Empty;
            txtMerchantID.Text = string.Empty; 

            this.Showing = false;
        }
 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();
            AchListItem item = (AchListItem)cboType.SelectedItem;
            AchListItem item2 = (AchListItem)cboReleaseHold.SelectedItem;

            SqlParameter prm = new SqlParameter("@HoldID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", txtAchID.Text));
            prms.Add(new SqlParameter("@JournalIDIN", DataLayer.Int2Field(txtJournalIDIn.Text)));
            prms.Add(new SqlParameter("@PostedDate", DataLayer.Date2Field(txtPostedDate.Text)));
            prms.Add(new SqlParameter("@ReleasedDate", DataLayer.Date2Field(txtReleaseDate.Text)));
            prms.Add(new SqlParameter("@DatePaid", DataLayer.Date2Field(txtPaidDate.Text)));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@Type", item.ItemValue ));
            prms.Add(new SqlParameter("@JournalIDPaid", DataLayer.Int2Field(txtJournalIDOut.Text)));
            prms.Add(new SqlParameter("@ReleaseThisHold", item2.ItemValue ));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

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
                FormHandler.DispalyInformationMessage("Hold and Journal record have been created!");

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
            AchListItem item = (AchListItem)cboType.SelectedItem;
            AchListItem item2 = (AchListItem)cboReleaseHold.SelectedItem;

            prms.Add(new SqlParameter("@HoldID", DataLayer.Int2Field(txtHoldID.Text)));
            prms.Add(new SqlParameter("@JournalIDIN", DataLayer.Int2Field(txtJournalIDIn.Text)));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@ReleasedDate", DataLayer.Date2Field(txtReleaseDate.Text)));
            prms.Add(new SqlParameter("@DatePaid", DataLayer.Date2Field(txtPaidDate.Text)));
            prms.Add(new SqlParameter("@Type", item.ItemValue));
            prms.Add(new SqlParameter("@JournalIDPaid", DataLayer.Int2Field(txtJournalIDOut.Text)));
            prms.Add(new SqlParameter("@ReleaseThisHold", item2.ItemValue));
            prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID ));

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

            if (txtPostedDate.Text == DataLayer.Empty_Date_Time)
                strError += "Please enter a Posted Date.\n";
            else
                if (!DataLayer.IsDate(txtPostedDate.Text.Trim()))
                    strError += "Please enter a valid Posted Date.\n";

            if (txtAmount.Text.Trim() == string.Empty)
                strError += "Please enter an Amount.\n";

            if (txtAmount.Text.Trim() != string.Empty)
                if (!DataLayer.IsNumeric(txtAmount.Text))
                    strError += "Please enter a valid Amount.\n";

            if (txtReleaseDate.Text == DataLayer.Empty_Date_Time )
                strError += "Please enter a Release Date.\n";
            else
                if (!DataLayer.IsDate(txtReleaseDate.Text.Trim()))
                    strError += "Please enter a valid Release Date.\n";

            if (txtPaidDate.Text != DataLayer.Empty_Date_Time)
                if (!DataLayer.IsDate(txtPaidDate.Text.Trim()))
                    strError += "Please enter a valid Processed Date.\n";

            if (cboType.Text.Trim() == string.Empty)
                strError += "Please select a Type.\n";
            
            if (cboReleaseHold.Text.Trim() == string.Empty)
                strError += "Please select a Release Hold option.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void txtPaidDate_Enter(object sender, EventArgs e)
        {
            SendKeys.Send("{HOME}");
        }

        private void frmHold_FormClosed(object sender, FormClosedEventArgs e)
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

        private void lnkJournalIDOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtJournalIDOut.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmJournal(), DataLayer.Int2Field(txtJournalIDOut.Text));

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


    }
}