using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmAccountBlock : AchSystem.frmBase 
    {

        private Image m_ReleaseForm = null;

        public frmAccountBlock()
        {
            InitializeComponent();
            
            FormHandler.AddControlChangedEvent(this);

            cboTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboNewTransType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);


            LookUpTableHandler.LoadTransactionTransType(cboTransType);
            LookUpTableHandler.LoadTransactionTransType(cboNewTransType);

            this.Data = new DataAccountBlock();
            this.KeyColumnName = "EntryID";
            //FormHandler.SetSecurity(this);

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
            this.Showing  = true;

            txtEntryID.Text = this.Dr["EntryID"].ToString().Trim();
            txtAchID.Text = this.Dr["AchID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();

            txtTransRoute.Text = this.Dr["TransRoute"].ToString().Trim();
            txtAccountNo.Text = this.Dr["AccountNo"].ToString().Trim();
            ListHandler.ListFindItem(cboTransType, this.Dr["TransType"].ToString().Trim());
            txtNewTransRoute.Text = this.Dr["NewTR"].ToString().Trim();
            txtNewAccountNo.Text = this.Dr["NewAcct"].ToString().Trim();
            ListHandler.ListFindItem(cboNewTransType, this.Dr["NewTranType"].ToString().Trim());

            txtR01.Text = this.Dr["R01"].ToString().Trim();
            txtR02.Text = this.Dr["R02"].ToString().Trim();
            txtR03.Text = this.Dr["R03"].ToString().Trim();
            txtR04.Text = this.Dr["R04"].ToString().Trim();
            txtR05.Text = this.Dr["R05"].ToString().Trim();
            txtR07.Text = this.Dr["R07"].ToString().Trim();
            txtR08.Text = this.Dr["R08"].ToString().Trim();
            txtR10.Text = this.Dr["R10"].ToString().Trim();
            txtR12.Text = this.Dr["R12"].ToString().Trim();
            txtR13.Text = this.Dr["R13"].ToString().Trim();
            txtR15.Text = this.Dr["R15"].ToString().Trim();
            txtR24.Text = this.Dr["R24"].ToString().Trim();
            txtR29.Text = this.Dr["R29"].ToString().Trim();
            txtR42.Text = this.Dr["R42"].ToString().Trim();

            if (this.Dr["ReleaseForm"] != DBNull.Value)
            {
                byte[] b = (byte[])this.Dr["ReleaseForm"];
                if (b.Length > 0)
                {
                    m_ReleaseForm = System.Drawing.Image.FromStream(new System.IO.MemoryStream(b));
                    pic.Image = ImageHandler.ResizeImage(m_ReleaseForm, pic.Width, pic.Height);
                }
            }
            else
            {
                m_ReleaseForm = null;
                pic.Image = null;
            }

            TextBoxTool txt = (TextBoxTool)tbrReleaseForm.Toolbars[0].Tools["txtReleaseForm"];
            txt.Text = string.Empty;


            FormHandler.PopulateControlTag(this);

            this.Showing = false;
        }

        public override void FormOpen(UltraGridRow dr)
        {
            if (dr != null)
            {
                m_lngID = Convert.ToInt64(dr.Cells[this.KeyColumnName].Value);
                if (this.FormFind())
                    this.FormShow();

                this.ShowDialog();
            }
            else
            {
                if (m_lngID == -1)
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

            FormHandler.PopulateNewRecord(this.SearchForm, this);
 
            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtEntryID.Text = string.Empty;
            txtAchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            txtMerchantName.Text = string.Empty;

            txtTransRoute.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            cboTransType.SelectedIndex = -1;
            txtNewTransRoute.Text = string.Empty;
            txtNewAccountNo.Text = string.Empty;
            cboNewTransType.SelectedIndex = -1;

            txtR01.Text = "0";
            txtR02.Text = "0";
            txtR03.Text = "0";
            txtR04.Text = "0";
            txtR07.Text = "0";
            txtR08.Text = "0";
            txtR10.Text = "0";
            txtR12.Text = "0";
            txtR13.Text = "0";
            txtR15.Text = "0";
            txtR24.Text = "0";
            txtR29.Text = "0";
            txtR42.Text = "0";

            TextBoxTool txt = (TextBoxTool)tbrReleaseForm.Toolbars[0].Tools["txtReleaseForm"];
            txt.Text = string.Empty;
            pic.Image = null;

            this.Showing = false;
        }

        public override bool FormAdd()
        {
            if (!this.DuplicateCheck())
            {
                FormHandler.DispalyWarningMessage("Account block already exists.");

                return false;
            }

            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@EntryID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            if (cboTransType.SelectedIndex == -1)
                prms.Add(new SqlParameter("@TransType", DBNull.Value ));
            else
                prms.Add(new SqlParameter("@TransType", cboTransType.Text));


            prms.Add(new SqlParameter("@NewTR", txtNewTransRoute.Text));
            prms.Add(new SqlParameter("@NEWACCT", txtNewAccountNo.Text));
            if (cboNewTransType.SelectedIndex == -1)
                prms.Add(new SqlParameter("@NewTranType", DBNull.Value));
            else
                prms.Add(new SqlParameter("@NewTranType", cboNewTransType.Text));

            prms.Add(new SqlParameter("@R01", DataLayer.Int2Field(txtR01.Text)));
            prms.Add(new SqlParameter("@R02", DataLayer.Int2Field(txtR02.Text)));
            prms.Add(new SqlParameter("@R03", DataLayer.Int2Field(txtR03.Text)));
            prms.Add(new SqlParameter("@R04", DataLayer.Int2Field(txtR04.Text)));
            prms.Add(new SqlParameter("@R05", DataLayer.Int2Field(txtR05.Text)));
            prms.Add(new SqlParameter("@R07", DataLayer.Int2Field(txtR07.Text)));
            prms.Add(new SqlParameter("@R08", DataLayer.Int2Field(txtR08.Text)));
            prms.Add(new SqlParameter("@R10", DataLayer.Int2Field(txtR10.Text)));
            prms.Add(new SqlParameter("@R12", DataLayer.Int2Field(txtR12.Text)));
            prms.Add(new SqlParameter("@R13", DataLayer.Int2Field(txtR13.Text)));
            prms.Add(new SqlParameter("@R15", DataLayer.Int2Field(txtR15.Text)));
            prms.Add(new SqlParameter("@R24", DataLayer.Int2Field(txtR24.Text)));
            prms.Add(new SqlParameter("@R29", DataLayer.Int2Field(txtR29.Text)));
            prms.Add(new SqlParameter("@R42", DataLayer.Int2Field(txtR42.Text)));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                txtAchID.ReadOnly = true;
                m_lngID = lngID;
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

            prms.Add(new SqlParameter("@EntryID", txtEntryID.Text));
            prms.Add(new SqlParameter("@AchID", DataLayer.Int2Field(txtAchID.Text)));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            if (cboTransType.SelectedIndex == -1)
                prms.Add(new SqlParameter("@TransType", DBNull.Value ));
            else
                prms.Add(new SqlParameter("@TransType", cboTransType.Text));


            prms.Add(new SqlParameter("@NewTR", txtNewTransRoute.Text));
            prms.Add(new SqlParameter("@NEWACCT", txtNewAccountNo.Text));
            if (cboNewTransType.SelectedIndex == -1)
                prms.Add(new SqlParameter("@NewTranType", DBNull.Value));
            else
                prms.Add(new SqlParameter("@NewTranType", cboNewTransType.Text));

            prms.Add(new SqlParameter("@R01", DataLayer.Int2Field(txtR01.Text)));
            prms.Add(new SqlParameter("@R02", DataLayer.Int2Field(txtR02.Text)));
            prms.Add(new SqlParameter("@R03", DataLayer.Int2Field(txtR03.Text)));
            prms.Add(new SqlParameter("@R04", DataLayer.Int2Field(txtR04.Text)));
            prms.Add(new SqlParameter("@R05", DataLayer.Int2Field(txtR05.Text)));
            prms.Add(new SqlParameter("@R07", DataLayer.Int2Field(txtR07.Text)));
            prms.Add(new SqlParameter("@R08", DataLayer.Int2Field(txtR08.Text)));
            prms.Add(new SqlParameter("@R10", DataLayer.Int2Field(txtR10.Text)));
            prms.Add(new SqlParameter("@R12", DataLayer.Int2Field(txtR12.Text)));
            prms.Add(new SqlParameter("@R13", DataLayer.Int2Field(txtR13.Text)));
            prms.Add(new SqlParameter("@R15", DataLayer.Int2Field(txtR15.Text)));
            prms.Add(new SqlParameter("@R24", DataLayer.Int2Field(txtR24.Text)));
            prms.Add(new SqlParameter("@R29", DataLayer.Int2Field(txtR29.Text)));
            prms.Add(new SqlParameter("@R42", DataLayer.Int2Field(txtR42.Text)));
            prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));

            //Save Image
   
            byte[] bytData = ImageHandler.GetBytesFromImage((Image)m_ReleaseForm );
            prms.Add( new SqlParameter("@ReleaseForm", SqlDbType.VarBinary,
            bytData.Length, ParameterDirection.Input, false,
            0, 0, null, DataRowVersion.Current, bytData));

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

            if (txtTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a Trans Route.\n";

            if (txtAccountNo.Text.Trim() == string.Empty)
                strError += "Please enter an Account No.\n";

            if (cboTransType.Text.Trim() == string.Empty)
                strError += "Please select a Trans Type.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmAccountBlock_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr  != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            

        }

    

        private bool DuplicateCheck()
        {
            ArrayList prms = new ArrayList();
            DataSet ds = null;

            prms.Add(new SqlParameter("@AchID", Convert.ToInt32(txtAchID.Text)));
            prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            prms.Add(new SqlParameter("@TransType", cboTransType.Text));

            try
            {
                ds = this.Data.Search(prms);

                if (ds.Tables[0].Rows.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception  exc)
            {
                throw exc;
            }
            finally
            {
                ds = null;
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtAchID.Text.Trim() == string.Empty)
                return;

            FormHandler.OpenDataForm(new frmMerchant(), DataLayer.Int2Field(txtAchID.Text));
        }

        private void pic_DoubleClick(object sender, EventArgs e)
        {
            frmPicture frm = new frmPicture((Image)m_ReleaseForm);
            frm.ShowDialog();
            frm = null;
        }

        private void tbrReleaseForm_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            { 
                case "Add Release Form":
                    ofdMain.ShowDialog();
                    TextBoxTool txt = (TextBoxTool)tbrReleaseForm.Toolbars[0].Tools["txtReleaseForm"];
                    if (ofdMain.FileName != string.Empty)
                    {
                        txt.Text  = ofdMain.FileName;
                        Image img = ImageHandler.GetImageFromFile(txt.Text);
                        m_ReleaseForm = img;
                        pic.Image = ImageHandler.ResizeImage(img, pic.Width, pic.Height);
                        this.Text_TextChanged(pic, new EventArgs());
                    }
                    break;
                case "Clear Release Form":
                    m_ReleaseForm = null;
                    pic.Image = null;
                    this.Text_TextChanged(pic, new EventArgs());

                    break;
                
            }
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            FormHandler.PopulateMerchantInfo(row, pnlMain);
        }

        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            FormHandler.PopulateMerchantInfo(row, pnlMain);
        }

     
        

    }
}