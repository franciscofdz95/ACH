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
    public partial class frmOrigin : AchSystem.frmBase 
    {
        public frmOrigin()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            txtOriginID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

            this.Data = new DataOrigin();
            this.KeyColumnName = "OriginID";
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

            txtOriginID.Text = this.Dr["OriginID"].ToString().Trim();
            txtDescription.Text = this.Dr["Description"].ToString().Trim();
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

            txtOriginID.Text = string.Empty;
            txtDescription.Text = string.Empty;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@OriginID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@Description", txtDescription.Text));

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

            prms.Add(new SqlParameter("@OriginID", DataLayer.Int2Field(txtOriginID.Text) ));
            prms.Add(new SqlParameter("@Description", txtDescription.Text));

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

        private void frmOrigin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

        
    }
}