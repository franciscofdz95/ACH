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
    public partial class frmWallet : AchSystem.frmBase 
    {
        private int m_AchID;
        private int m_MerchantID;
        private string m_MerchantName;
        
        public frmWallet(int AchID, int MerchantID, string MerchantName)
        {
            InitializeComponent();

            m_AchID = AchID;
            m_MerchantID = MerchantID;
            m_MerchantName = MerchantName;

            txtAchID.Text = AchID.ToString();
            txtMerchantID.Text = MerchantID.ToString();
            txtMerchantName.Text = MerchantName;

            FormHandler.CreateToolBarButton(tbrTop, "Process Wallet");

            tbrTop.Toolbars[0].Tools["Process Wallet"].ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ProcessWallet_ToolClick);


            FormHandler.AddControlChangedEvent(this);

            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);

            cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboAchSecc.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            LookUpTableHandler.LoadTransactionTransType(cboTransType);
            LookUpTableHandler.LoadSecc(cboAchSecc);
            this.Data = new DataWallet();
            this.KeyColumnName = "WalletID";
            FormHandler.SetSecurity(this);

            this.LoadWallets();

        }

        private void LoadWallets()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AchID", m_AchID));
            DataSet ds = this.Data.Search(prms);

            grdWallet.DataSource = ds;

            if (grdWallet.Rows.Count > 0)
            {
                grdWallet.ActiveRow = grdWallet.Rows[0];
                grpRequired.Enabled = true;
                grpOptional.Enabled = true;
            }
            else
            {
                grpRequired.Enabled = false;
                grpOptional.Enabled = false;
            }
        
        }

        private void ProcessWallet_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Are you sure you want to process this wallet.");

            if (result == DialogResult.No)
            {
                return;
            }


            ArrayList prms = new ArrayList();
            DataWallet data = new DataWallet();

            prms.Add(new SqlParameter("@AchID", m_AchID));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

            int rows = data.ProcessWallet(prms);

            if (rows > 0)
                FormHandler.DispalyInformationMessage("Wallet processed successfully");
            else
                FormHandler.DispalyWarningMessage("Process did not create any records!");


        }

        public override bool FormFind()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@" + this.KeyColumnName, grdWallet.ActiveRow.Cells[this.KeyColumnName].Value));
            this.Dr = this.Data.Select(prms);

            if (this.Dr.Read())
                return true;
            else
                return false;
        }

        public override void FormToggleButtons()
        {
            tbrTop.Toolbars[0].Tools["New"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["New"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Undo"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Undo"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Delete"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Delete"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Close"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Close"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Process Wallet"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Process Wallet"].SharedProps.Enabled;
            grdWallet.Enabled = !grdWallet.Enabled;
            
        }
        public override void FormShow()
        {
            this.Showing = true;

            this.ID = DataLayer.Field2Long(this.Dr["WalletID"]);
            txtWalletID.Text = this.Dr["WalletID"].ToString();
            txtMerchantID.Text = this.Dr["MerchantID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();
            txtAchID.Text = this.Dr["AchID"].ToString().Trim();
            txtAchDescription.Text = this.Dr["Description"].ToString().Trim();
            ListHandler.ListFindItem(cboTransType , this.Dr["TransType"].ToString().Trim());
            ListHandler.ListFindItem(cboAchSecc , this.Dr["Secc"].ToString().Trim());
            txtTransRoute.Text = this.Dr["TransRoute"].ToString().Trim();
            txtAccountNo.Text = this.Dr["AccountNo"].ToString().Trim();
            txtAccountName.Text = this.Dr["NameOnAccount"].ToString().Trim();
            txtRefID.Text = this.Dr["RefID"].ToString().Trim();
            txtAmount.Text = this.Dr["Amount"].ToString().Trim();
            chkEnabled.Checked = DataLayer.Field2Bool(this.Dr["IsEnabled"]);
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

            grpRequired.Enabled = true;
            grpOptional.Enabled = true;


            chkEnabled.Checked = true;
            ListHandler.ListFindItem(cboTransType , "27");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtWalletID.Text = string.Empty;
            txtAchDescription.Text = string.Empty;
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

            SqlParameter prm = new SqlParameter("@WalletID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@IsEnabled", DataLayer.Field2Bool(chkEnabled.Checked)));
            prms.Add(new SqlParameter("@Description", txtAchDescription.Text.Trim()));
            prms.Add(new SqlParameter("@DescDate", DateTime.Now.ToString("yyMMdd")));
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

                this.LoadWallets();

                this.ID = lngID;

                ListHandler.ListFindItem(grdWallet, "WalletID", this.ID.ToString());

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

            long lngID = DataLayer.Field2Long(txtWalletID.Text);

            SqlParameter prm = new SqlParameter("@WalletID", lngID);
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@IsEnabled", DataLayer.Field2Bool(chkEnabled.Checked)));
            prms.Add(new SqlParameter("@Description", txtAchDescription.Text.Trim()));
            prms.Add(new SqlParameter("@DescDate", DateTime.Now.ToString("yyMMdd")));
            item = (AchListItem)cboTransType.SelectedItem;
            prms.Add(new SqlParameter("@TransType", item.ItemValue));
            item = (AchListItem)cboAchSecc.SelectedItem;
            prms.Add(new SqlParameter("@Secc", item.ItemValue));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text.Trim()));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text.Trim()));
            prms.Add(new SqlParameter("@NameOnAccount", txtAccountName.Text.Trim()));
            prms.Add(new SqlParameter("@RefID", txtRefID.Text.Trim()));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(txtAmount.Text)));
            prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));


            int intRows = this.Data.Update(prms);

            if (intRows > 0)
            {
                this.LoadWallets();

                ListHandler.ListFindItem(grdWallet, "WalletID", lngID.ToString());
                
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

            if (txtAchDescription.Text.Trim() == string.Empty)
                strError += "Please enter a Description.\n";

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



        private void grdWallet_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Single;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;

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

        private void grdWallet_AfterRowActivate(object sender, EventArgs e)
        {
            if (grdWallet.ActiveRow != null)
            {
                if (this.FormFind())
                    this.FormShow();
            }
        }

        
    }
}