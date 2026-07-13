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
    public partial class frmLog : AchSystem.frmBase 
    {
        public frmLog()
        {
            InitializeComponent();

            tbrTop.Toolbars[0].Tools["New"].InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False;
            tbrTop.Toolbars[0].Tools["Save"].InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False;
            tbrTop.Toolbars[0].Tools["Delete"].InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False;
            tbrTop.Toolbars[0].Tools["Undo"].InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False; 

            this.Data = new DataLog();
            this.KeyColumnName = "LogID";
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

            txtLogID.Text = this.Dr["Log ID"].ToString().Trim();
            txtID.Text = this.Dr["ID"].ToString().Trim();
            txtTableName.Text = this.Dr["Table Name"].ToString().Trim();
            txtPostedDate.Text = this.Dr["Posted Date"].ToString().Trim();
            txtUser.Text = this.Dr["Login ID"].ToString().Trim();
            txtNote.Text = this.Dr["Note"].ToString().Trim();
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
                this.Close();
            }
        }

        public override void FormNew()
        {
        }

        public override void FormClear()
        {
            this.Showing = true;

            this.Showing = false;
        }

        public override bool FormAdd()
        {
            return false;
        }

        public override bool FormUpdate()
        {
            return false;          
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



            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

    }
}