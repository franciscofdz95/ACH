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
    public partial class frmRefcode : AchSystem.frmBase 
    {

        public frmRefcode()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            this.Data = new DataRefcode();
            this.KeyColumnName = "RefcodeID";
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

            txtRefcodeID.Text = this.Dr["RefcodeID"].ToString().Trim();
            txtRefcode.Text = this.Dr["Refcode"].ToString().Trim();
            txtDescription.Text = this.Dr["Description"].ToString().Trim();

            if (this.Dr["Include In Balance"].ToString() == "1")
                chkIncludeInBalance.Checked = true;
            else
                chkIncludeInBalance.Checked = false;

            if (this.Dr["Include In Available Balance"].ToString() == "1")
                chkIncludeInAvailableBalance.Checked = true;
            else
                chkIncludeInAvailableBalance.Checked = false;


            //chkIncludeInAvailableBalance.Checked = DataLayer.Field2Bool(this.Dr["Include In Available Balance"]);
            chkIsFeeCode.Checked = DataLayer.Field2Bool(this.Dr["Is Fee Code?"]);

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

            txtRefcode.ReadOnly = false;

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtRefcodeID.Text = string.Empty;
            txtRefcode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            chkIncludeInBalance.Checked = false;
            chkIncludeInAvailableBalance.Checked = false;
            chkIsFeeCode.Checked = false;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@RefcodeID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@Refcode", txtRefcode.Text));
            prms.Add(new SqlParameter("@Description", txtDescription.Text));
            prms.Add(new SqlParameter("@IncludeInBalance", DataLayer.Bool2Field(chkIncludeInBalance.Checked)));
            prms.Add(new SqlParameter("@IncludeInAvailableBalance", DataLayer.Bool2Field(chkIncludeInAvailableBalance.Checked)));
            prms.Add(new SqlParameter("@IsFeeCode", DataLayer.Bool2Field(chkIsFeeCode.Checked)));

            long lngID = this.Data.Insert (prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                this.ID = lngID;
                txtRefcode.ReadOnly = true;
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

            prms.Add(new SqlParameter("@RefcodeID", DataLayer.Int2Field(txtRefcodeID.Text)));
            prms.Add(new SqlParameter("@Refcode", txtRefcode.Text));
            prms.Add(new SqlParameter("@Description", txtDescription.Text));
            prms.Add(new SqlParameter("@IncludeInBalance", DataLayer.Bool2Field(chkIncludeInBalance.Checked)));
            prms.Add(new SqlParameter("@IncludeInAvailableBalance", DataLayer.Bool2Field(chkIncludeInAvailableBalance.Checked)));
            prms.Add(new SqlParameter("@IsFeeCode", DataLayer.Bool2Field(chkIsFeeCode.Checked)));

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

            txtRefcode.ReadOnly = true;

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

            if (txtRefcode.Text.Trim() == string.Empty)
                strError += "Please enter a Refcode.\n";

            if (txtDescription.Text.Trim() == string.Empty)
                strError += "Please enter a Description.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmRefcode_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            

        }

        
    }
}