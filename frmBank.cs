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
    public partial class frmBank : AchSystem.frmBase 
    {
        public frmBank()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtNewTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

            this.Data = new DataBank();
            this.KeyColumnName = "BankID";
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

            txtBankID.Text = this.Dr["Bank ID"].ToString().Trim();
            txtTransRoute.Text = this.Dr["Trans Route"].ToString().Trim();
            txtNewTransRoute.Text = this.Dr["New Trans Route"].ToString().Trim();
            txtName.Text = this.Dr["Bank Name"].ToString().Trim();
            txtAddress.Text = this.Dr["Address"].ToString().Trim();
            txtCity.Text = this.Dr["City"].ToString().Trim();
            txtState.Text = this.Dr["State"].ToString().Trim();
            txtZipCode.Text = this.Dr["Zip Code"].ToString().Trim();
            txtSource.Text = this.Dr["Source"].ToString().Trim();

            if (this.Dr["Ok To Ach"].ToString().Trim() == "Y")
                chkAch.Checked = true;
            else
                chkAch.Checked = false;

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

                //this.Text = this.Text + " - " + dr.Cells["Bank Name"].Value.ToString().Trim() + "(" + this.ID.ToString() + ")";
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

            txtSource.Text = "F";

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtBankID.Text = string.Empty;
            txtTransRoute.Text = string.Empty;
            txtNewTransRoute.Text = "000000000";
            txtName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            txtSource.Text = string.Empty;
            chkAch.Checked = true;

            this.Showing = false;
        }


        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@BankID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@RoutineNumber", txtTransRoute.Text));
            prms.Add(new SqlParameter("@NewRoutineNo", txtNewTransRoute.Text));
            prms.Add(new SqlParameter("@BankName", txtName.Text));
            prms.Add(new SqlParameter("@BankAddress", txtAddress.Text));
            prms.Add(new SqlParameter("@BankCity", txtCity.Text));
            prms.Add(new SqlParameter("@BankState", txtState.Text));
            prms.Add(new SqlParameter("@BankZip", txtZipCode.Text));

            if (chkAch.Checked)
                prms.Add(new SqlParameter("@OkToAch", "Y"));
            else
                prms.Add(new SqlParameter("@OkToAch", "N"));

            prms.Add(new SqlParameter("@Source", txtSource.Text));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID ));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false ;
                this.IsDirty = true;
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

            prms.Add(new SqlParameter("@BankID",txtBankID.Text));
            prms.Add(new SqlParameter("@RoutineNumber", txtTransRoute.Text));
            prms.Add(new SqlParameter("@NewRoutineNo", txtNewTransRoute.Text));
            prms.Add(new SqlParameter("@BankName", txtName.Text));
            prms.Add(new SqlParameter("@BankAddress", txtAddress.Text));
            prms.Add(new SqlParameter("@BankCity", txtCity.Text));
            prms.Add(new SqlParameter("@BankState", txtState.Text));
            prms.Add(new SqlParameter("@BankZip", txtZipCode.Text));

            if (chkAch.Checked)
                prms.Add(new SqlParameter("@OkToAch","Y"));
            else
                prms.Add(new SqlParameter("@OkToAch", "N"));

            prms.Add(new SqlParameter("@Source", txtSource.Text));
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

            if (txtTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a Trans Route.\n";

            if (txtNewTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a New Trans Route.  Use '000000000' for blank value.\n";

            if (txtName.Text.Trim() == string.Empty)
                strError += "Please enter a Bank Name.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmBank_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

    


    }
}