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
    public partial class frmBase : Form, iFormData
    {
        private bool m_Adding = false;
        private bool m_Show = false;
        public long m_lngID = -1;
        private bool m_IsDirty = false;
        private SqlDataReader m_Dr = null;
        private string m_ChangeValue = string.Empty;
        private iData m_Data;
        private frmSearch m_frmSearch = null;
        private string m_KeyColumnName = string.Empty;

        public frmBase()
        {
            InitializeComponent();

        }

        public iData Data
        {
            get { return m_Data; }
            set { m_Data = value; }
        }

        public SqlDataReader Dr
        {
            get { return m_Dr; }
            set { m_Dr = value; }
        }

        public frmSearch SearchForm
        {
            get { return m_frmSearch; }
            set { m_frmSearch = value; }
        }

        public string ChangeValue
        {
            get { return m_ChangeValue; }
            set { m_ChangeValue = value; }
        }

        public string KeyColumnName
        {
            get { return m_KeyColumnName; }
            set { m_KeyColumnName = value; }
        }

        public long ID
        {
            get { return m_lngID; }
            set { m_lngID = value; }
        }

        public bool Showing
        {
            get { return m_Show; }
            set { m_Show = value; }
        }

        public bool Adding
        {
            get { return m_Adding; }
            set { m_Adding = value; }
        }

        public bool IsDirty
        {
            get { return m_IsDirty; }
            set { m_IsDirty = value; }
        }

        public Form GetForm()
        {
            return this;
        }

        public virtual void FormOpen(UltraGridRow dr) { }
        public virtual bool FormFind(){return false;}
        public virtual void FormShow() { }
        public virtual void FormNew() { }
        public virtual void FormClear() { }
        public virtual void FormUndo() { }

        public virtual bool FormDelete(UltraGridRow dr, bool Prompt) 
        {
            this.ID = Convert.ToInt64(dr.Cells[this.KeyColumnName].Value);

            if (Prompt)
            {
                DialogResult result;
                result = FormHandler.DispalyQuestionMessage("Do you want to delete " + this.KeyColumnName + " " + this.ID.ToString() + ".");

                if (result == DialogResult.No)
                    return false;
            }

            if (!this.Visible)
                if (this.FormFind())
                    this.FormShow();

            this.ChangeValue = "DELETE - \n" + FormHandler.GetControlTextValue(this);

            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@" + this.KeyColumnName, this.ID));
            int intRows = this.Data.Delete(prms);

            if (intRows > 0)
            {
                this.FormLogChange();
                this.IsDirty = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool FormDelete(long lngID, bool Prompt)
        {
            this.ID = lngID;

            if (Prompt)
            {

                DialogResult result;
                result = FormHandler.DispalyQuestionMessage("Do you want to delete " + this.KeyColumnName + " " + this.ID.ToString() + ".");

                if (result == DialogResult.No)
                    return false;
            }

            if (!this.Visible)
                if (this.FormFind())
                    this.FormShow();

            this.ChangeValue = "DELETE - \n" + FormHandler.GetControlTextValue(this);

            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@" + this.KeyColumnName, this.ID));
            int intRows = this.Data.Delete(prms);

            if (intRows > 0)
            {
                this.FormLogChange();
                this.IsDirty = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool FormAdd() { return false; }
        public bool FormSave()
        {
            if (!this.FormDataCheck())
                return false;

            if (m_Adding)
                m_ChangeValue = "INSERT - \n" + FormHandler.GetControlChangedValue(this);
            else
                m_ChangeValue = "UPDATE - \n" + FormHandler.GetControlChangedValue(this);

            bool perform;

            if (m_Adding)
                perform = this.FormAdd();
            else
                perform = this.FormUpdate();

            if (perform)
            {
                this.IsDirty = true;
                this.FormLogChange();
            }

            return perform;
        }

        //private UltraGridRow MakeCurrentGridRow()
        //{
        //    UltraGrid grd = new UltraGrid();
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add(new DataColumn(this.KeyColumnName));
        //    DataRow dr = dt.NewRow();
        //    dr[this.KeyColumnName] = this.ID;
        //    dt.Rows.Add(dr);
        //    BindingSource bs = new BindingSource();
        //    bs.DataSource = dt;
        //    grd.DataSource = bs;
            
        //    if (grd.Rows.Count > 0)
        //        grd.Rows[0].Selected = true;

        //    return grd.ActiveRow;
        //}

        public virtual bool FormUpdate() { return false; }
        public void FormControlChanged()
        {
            if (!m_Show)
                if (!tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled)
                    this.FormToggleButtons();
        }
        public virtual void FormToggleButtons()
        {
            tbrTop.Toolbars[0].Tools["New"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["New"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Undo"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Undo"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Delete"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Delete"].SharedProps.Enabled;
            tbrTop.Toolbars[0].Tools["Close"].SharedProps.Enabled = !tbrTop.Toolbars[0].Tools["Close"].SharedProps.Enabled;


        }

        public virtual bool FormDataCheck() { return false; }
        public void FormLogChange()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@LogID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@ID", m_lngID));
            prms.Add(new SqlParameter("@TableName", this.Text));
            prms.Add(new SqlParameter("@Note", m_ChangeValue));
            prms.Add(new SqlParameter("@UserID", main.Current_User));

            DataLog data = new DataLog();

            data.Insert(prms);
            data = null;
        }

        public void Text_TextChanged(object sender, EventArgs e)
        {
            this.FormControlChanged();
        }

 
      
        private void frmBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_lngID = -1;
        }

        private void frmBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled)
            {
                DialogResult result;
                result = FormHandler.DispalyQuestionMessage("Data has not been saved.  Do you want to save data?");

                switch (result)
                {
                    case DialogResult.Yes:
                        e.Cancel = !this.FormSave();
                        break;
                    case DialogResult.No:
                        this.FormUndo();
                        e.Cancel = false;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }

            }

        }

        private void frmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

        }

        private void tbrTop_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "New":
                    this.FormNew();
                    break;
                case "Save":
                    this.FormSave();
                    break;
                case "Undo":
                    this.FormUndo();
                    break;
                case "Delete":
                    if (this.FormDelete(this.ID, true))
                        this.Close();
                    break;
                case "Close":
                    this.Close();
                    break;
            }
        }

  
    }
}