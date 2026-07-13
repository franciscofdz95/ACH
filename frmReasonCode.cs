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
    public partial class frmReturnReasonCode : AchSystem.frmBase 
    {
        public frmReturnReasonCode()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtReasonID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtOffsetBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

            this.Data = new DataReasonCode();
            this.KeyColumnName = "ReasonID";
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

            txtReasonID.Text = this.Dr["ReasonID"].ToString().Trim();
            txtReasonCode.Text = this.Dr["Reason Code"].ToString().Trim();
            txtReason.Text = this.Dr["ReasonDesc"].ToString().Trim();
            if (Convert.ToBoolean(this.Dr["Resubmitable"]))
                chkResubmitable.Checked = true;
            else
                chkResubmitable.Checked = false;

            txtOffsetBalance.Text = this.Dr["Offset Balance"].ToString().Trim();

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

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtReasonID.Text = string.Empty;
            txtReasonCode.Text = string.Empty;
            txtReason.Text = string.Empty;
            chkResubmitable.Checked = true;
            txtOffsetBalance.Text = string.Empty;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@ReasonID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@ReasonCode", txtReasonCode.Text));
            prms.Add(new SqlParameter("@ReasonDesc", txtReason.Text));
            prms.Add(new SqlParameter("@OffsetBalance", DataLayer.Int2Field(txtOffsetBalance.Text)));
            if (chkResubmitable.Checked)
                prms.Add(new SqlParameter("@Resubmitable", true));
            else
                prms.Add(new SqlParameter("@Resubmitable", false));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
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

            prms.Add(new SqlParameter("@ReasonID", DataLayer.Int2Field (txtReasonID.Text)));
            prms.Add(new SqlParameter("@ReasonCode", txtReasonCode.Text));
            prms.Add(new SqlParameter("@ReasonDesc", txtReason.Text));
            prms.Add(new SqlParameter("@OffsetBalance", DataLayer.Int2Field(txtOffsetBalance.Text)));
            if (chkResubmitable.Checked)
                prms.Add(new SqlParameter("@Resubmitable",true));
            else
                prms.Add(new SqlParameter("@Resubmitable", false));

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

            if (txtReasonCode.Text.Trim() == string.Empty)
                strError += "Please enter a Reason Code.\n";

            if (txtReason.Text.Trim() == string.Empty)
                strError += "Please enter a ReasonDesc.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmReasonCode_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

        
    }
}