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
    public partial class frmAchRecurring : AchSystem.frmBase 
    {
        public frmAchRecurring()
        {
            InitializeComponent();

            FormHandler.CreateToolBarButton(tbrTop, "Credit");
            FormHandler.CreateToolBarButton(tbrTop, "Resubmit");

            tbrTop.Toolbars[0].Tools["Credit"].ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.CreditTransaction_ToolClick);
            tbrTop.Toolbars[0].Tools["Resubmit"].ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ResubmitTransaction_ToolClick);

            FormHandler.AddControlChangedEvent(this);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboAchSecc.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboOriginID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);

            LookUpTableHandler.LoadOrigins(cboOriginID);
            LookUpTableHandler.LoadTransactionTransType(cboTransType);
            LookUpTableHandler.LoadSecc(cboAchSecc);
            this.Data = new DataRecurring();
            this.KeyColumnName = "Recur_ID";
            FormHandler.SetSecurity(this);

        }

        private void CreditTransaction_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Are you sure you want to Credit Trans ID " + txtTransID.Text + ".");

            if (result == DialogResult.No)
            {
                return;
            }

            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@NewTransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@TransID", DataLayer.Int2Field(txtTransID.Text)));

            DataTransaction data = new DataTransaction();
            long lngID = data.InsertCredit(prms);

            if (lngID != -1)
            {
                if (this.FormFind())
                    this.FormShow();

            }

            data = null;

        }

        private void ResubmitTransaction_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure you want to Resubmit Trans ID " + txtTransID.Text + ".", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@NewTransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@TransID", DataLayer.Int2Field(txtTransID.Text)));

            DataTransaction data = new DataTransaction();
            long lngID = data.InsertResubmit(prms);

            if (lngID != -1)
            {
                if (this.FormFind())
                    this.FormShow();

                FormHandler.OpenDataForm(new frmTransaction(), DataLayer.Int2Field(lngID));

            }

            data = null;

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

            txtMerchantID.Text = this.Dr["MerchantID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();
            txtAchID.Text = this.Dr["AchID"].ToString().Trim();
            txtAchDescription.Text = this.Dr["Ach_Description"].ToString().Trim();
            txtDescDate.Text = this.Dr["DescDate"].ToString().Trim();
            txtAchCompanyName.Text = this.Dr["CompanyName"].ToString().Trim();
            ListHandler.ListFindItem(cboTransType , this.Dr["TransType"].ToString().Trim());
            ListHandler.ListFindItem(cboAchSecc , this.Dr["Secc"].ToString().Trim());
            txtTransRoute.Text = this.Dr["TransRoute"].ToString().Trim();
            txtAccountNo.Text = this.Dr["AccountNo"].ToString().Trim();
            txtAccountName.Text = this.Dr["NameOnAccount"].ToString().Trim();
            txtRefID.Text = this.Dr["RefID"].ToString().Trim();
            txtAmount.Text = this.Dr["Amount"].ToString().Trim();
            
            txtEncryptAccount.Text = this.Dr["EncryptAccountNo"].ToString().Trim();
           
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

            ListHandler.ListFindItem(cboOriginID, "19"); //Manual
            ListHandler.ListFindItem(cboTransType , "27");

            txtTransDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            txtDescDate.Text = DateTime.Now.ToString("yyMMdd");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtTransID.Text = string.Empty;
            txtTransDate.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtMerchantName.Text = string.Empty;
            txtAchID.Text = string.Empty;
            cboOriginID.SelectedIndex = -1;
            txtAchDescription.Text = string.Empty;
            txtDescDate.Text = string.Empty;
            txtAchCompanyName.Text = string.Empty;
            cboTransType.SelectedIndex = -1;
            cboAchSecc.SelectedIndex = -1;
            txtTransRoute.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtRefID.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtEncryptAccount.Text = string.Empty;
            this.Showing = false;
        }

        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();
            AchListItem item;

            SqlParameter prm = new SqlParameter("@TransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@TransDate", DataLayer.Date2Field(txtTransDate.Text)));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(txtMerchantID.Text)));

            item = (AchListItem)cboOriginID.SelectedItem;
            prms.Add(new SqlParameter("@OriginID", DataLayer.Int2Field(item.ItemValue)));
            prms.Add(new SqlParameter("@Description", txtAchDescription.Text.Trim()));
            prms.Add(new SqlParameter("@DescDate", txtDescDate.Text.Trim()));
            prms.Add(new SqlParameter("@CompanyName", txtAchCompanyName.Text.Trim()));
            item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            item = (AchListItem)cboAchSecc.SelectedItem;
            prms.Add(new SqlParameter("@Secc", item.ItemValue));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text.Trim()));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text.Trim()));
            prms.Add(new SqlParameter("@NameOnAccount", txtAccountName.Text.Trim()));
            prms.Add(new SqlParameter("@RefID", txtRefID.Text.Trim()));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                this.ID = lngID;
                txtAchID.ReadOnly = true;
                btnMerchant.Enabled = false;

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

            prms.Add(new SqlParameter("@TransID", DataLayer.Int2Field(txtTransID.Text)));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@TransDate", DataLayer.Date2Field(txtTransDate.Text)));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field (txtMerchantID.Text)));
            prms.Add(new SqlParameter("@Description", txtAchDescription.Text.Trim()));
            prms.Add(new SqlParameter("@DescDate", txtDescDate.Text.Trim()));
            prms.Add(new SqlParameter("@CompanyName", txtAchCompanyName.Text.Trim()));
            item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            item = (AchListItem)cboAchSecc.SelectedItem;
            prms.Add(new SqlParameter("@Secc", item.ItemValue));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text.Trim()));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text.Trim()));
            prms.Add(new SqlParameter("@NameOnAccount", txtAccountName.Text.Trim()));
            prms.Add(new SqlParameter("@RefID", txtRefID.Text.Trim()));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@StatusID", item.ItemValue));
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

            if (txtMerchantID.Text.Trim() == string.Empty)
                strError += "Please enter a Merchant ID.\n";

            if (txtTransDate.Text == DataLayer.Empty_Date_Time)
                strError += "Please enter a Trans Date.\n";

            if (txtTransDate.Text != DataLayer.Empty_Date_Time)
            {
                if (!DataLayer.IsDate(txtTransDate.Text.Trim()))
                    strError += "Please enter a valid Trans Date.\n";
            }

            if (cboOriginID.SelectedIndex == -1)
                strError += "Please select an Origin.\n";

            if (txtAchDescription.Text.Trim() == string.Empty)
                strError += "Please enter a Description.\n";

            if (txtDescDate.Text.Trim() == string.Empty)
                strError += "Please enter a Desc Date.\n";

            if (txtAchCompanyName.Text.Trim() == string.Empty)
                strError += "Please enter a Company Name.\n";

            if (cboTransType.SelectedIndex == -1)
                strError += "Please select a Trans Type.\n";

            if (cboAchSecc.SelectedIndex == -1)
                strError += "Please select a Secc.\n";

            if (txtTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a Trans Route.\n";

            if (txtAccountNo.Text.Trim() == string.Empty)
                strError += "Please enter an Account No.\n";

            if (txtAccountName.Text.Trim() == string.Empty)
                strError += "Please enter an Account Name.\n";



            if (txtAmount.Text.Trim() == string.Empty)
                strError += "Please enter an Amount.\n";

            if (txtAmount.Text.Trim() != string.Empty)
            {
                if (!DataLayer.IsNumeric(txtAmount.Text))
                    strError += "Please enter a valid Amount.\n";
            }


            if (strError == string.Empty)
                return true;
            else
            {
                MessageBox.Show(strError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public void PopulateMerchantInfo(TextBox txtInput,TextBox txtOutput,string strParam,string strField)
        {
            if (!this.tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled )
                return;

            if (txtInput.Text.Trim() == string.Empty)
                return;

            if (this.Adding)
            {
                DataSet ds = null;
                ArrayList prms = new ArrayList();

                prms.Add(new SqlParameter(strParam, Convert.ToInt32(txtInput.Text)));

                DataMerchant data = new DataMerchant();
                ds = data.Search(prms);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];

                    txtOutput.Text = row[strField].ToString();

                    if (txtAchCompanyName.Text.Trim() == string.Empty)
                        txtAchCompanyName.Text  = row["Merchant Name"].ToString();

                    if (txtAchDescription.Text.Trim() == string.Empty)
                        txtAchDescription.Text = row["Ach Description"].ToString();

                    if (cboAchSecc.SelectedIndex == -1)
                        ListHandler.ListFindItem(cboAchSecc, row["Secc"].ToString().ToUpper());
                    
                }
            }
        }
        
        private void txtMerchantID_Leave(object sender, EventArgs e)
        {
            this.PopulateMerchantInfo(txtMerchantID, txtAchID, "MerchantID", "Ach ID");
        }

        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row,pnlMain);
        }

        //public void PopulateMerchantInfo(UltraGridRow row)
        //{
        //    if (row != null)
        //    {
        //        txtAchID.Text = row.Cells["AchID"].Value.ToString();
        //        txtMerchantID.Text = row.Cells["Merchant ID"].Value.ToString();
        //        txtMerchantName.Text = row.Cells["Merchant Name"].Value.ToString();

        //        if (txtCompanyName.Text.Trim() == string.Empty)
        //            txtCompanyName.Text = row.Cells["Merchant Name"].Value.ToString();

        //        if (txtDescription.Text.Trim() == string.Empty)
        //            txtDescription.Text = row.Cells["Ach Description"].Value.ToString();

        //        if (cboSecc.SelectedIndex == -1)
        //            ListHandler.ListFindItem(cboSecc, row.Cells["Secc"].Value.ToString().ToUpper());

        //    }
        //}

        private void frmTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            

        }

        

   

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row,pnlMain);

        }

        private void frmTransaction_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }

 
    }
}