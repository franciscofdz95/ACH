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
    public partial class frmTransType : AchSystem.frmBase 
    {

        public frmTransType()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            this.Data = new DataTransType();
            this.KeyColumnName = "TransTypeID";
            FormHandler.SetSecurity(this);

            LookUpTableHandler.LoadTransTypeOption(cboType);
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

            txtTransTypID.Text = this.Dr["TransTypeID"].ToString().Trim();
            txtDescription.Text = this.Dr["TransTypeDesc"].ToString().Trim();
            ListHandler.ListFindItem(cboType, this.Dr["Type"].ToString().Trim());

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

            txtTransTypID.Text = string.Empty;
            cboType.SelectedIndex = -1;
            txtDescription.Text = string.Empty;

            this.Showing = false;
        }

 
        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@TransTypeID", DataLayer.Int2Field(txtTransTypID.Text)));
            prms.Add(new SqlParameter("@TransTypeDesc", txtDescription.Text));
            AchListItem item = (AchListItem)cboType.SelectedItem;
            prms.Add(new SqlParameter("@Type", item.ItemValue));

            long lngID = this.Data.Insert (prms);

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

            prms.Add(new SqlParameter("@TransTypeID", DataLayer.Int2Field(txtTransTypID.Text)));
            prms.Add(new SqlParameter("@TransTypeDesc", txtDescription.Text));
            AchListItem item = (AchListItem)cboType.SelectedItem;
            prms.Add(new SqlParameter("@Type", item.ItemValue));

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

            if (cboType.SelectedIndex == -1)
                strError += "Please select a type.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmTransType_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            

        }

        
    }
}