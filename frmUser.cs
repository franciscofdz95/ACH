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
    public partial class frmUser : AchSystem.frmBase 
    {
        public frmUser()
        {
            InitializeComponent();

            FormHandler.AddControlChangedEvent(this);

            this.Data = new DataUser();
            this.KeyColumnName = "UserID";
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

            txtUserID.Text = this.Dr["User ID"].ToString().Trim();
            txtLoginID.Text = this.Dr["Login ID"].ToString().Trim();
            txtFirstname.Text = this.Dr["First name"].ToString().Trim();
            txtLastname.Text = this.Dr["Last name"].ToString().Trim();
            txtPassword.Text = this.Dr["Password"].ToString().Trim();
            txtConfirm.Text = this.Dr["Password"].ToString().Trim();
            chkAdmin.Checked = DataLayer.Field2Bool(this.Dr["Is Admin"]);
            chkReadOnly.Checked = DataLayer.Field2Bool(this.Dr["Is ReadOnly"]);
            chkProcessModule.Checked = DataLayer.Field2Bool(this.Dr["Process Module"]);
            chkSearchModule.Checked = DataLayer.Field2Bool(this.Dr["Search Module"]);
            chkReportModule.Checked = DataLayer.Field2Bool(this.Dr["Report Module"]);
            chkUtilityModule.Checked = DataLayer.Field2Bool(this.Dr["Utility Module"]);
            chkLookUpModule.Checked = DataLayer.Field2Bool(this.Dr["LookUp Module"]);
            chkAdminModule.Checked = DataLayer.Field2Bool(this.Dr["Admin Module"]);
            chkRiskModule.Checked = DataLayer.Field2Bool(this.Dr["Risk Module"]);

            FormHandler.PopulateControlTag(this);

            this.Showing = false;
        }

        public override void FormOpen(UltraGridRow dr)
        {
            if (dr != null)
            {
                this.ID = Convert.ToInt64(dr.Cells["UserID"].Value);
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
            txtLoginID.Focus();

            txtPassword.Text = "password";
            txtConfirm.Text = "password";

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtUserID.Text = string.Empty;
            txtLoginID.Text = string.Empty;
            txtFirstname.Text = string.Empty;
            txtLastname.Text = string.Empty;
            chkAdmin.Checked = false;
            chkReadOnly.Checked = false;
            chkAdminModule.Checked = false;
            chkLookUpModule.Checked = false;
            chkProcessModule.Checked = false;
            chkReportModule.Checked = false;
            chkSearchModule.Checked = false;
            chkUtilityModule.Checked = false;

            this.Showing = false;
        }

        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@UserID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@Firstname", txtFirstname.Text));
            prms.Add(new SqlParameter("@Lastname", txtLastname.Text));
            prms.Add(new SqlParameter("@LoginID", txtLoginID.Text));
            prms.Add(new SqlParameter("@Password", txtPassword.Text));
            prms.Add(new SqlParameter("@IsAdmin", chkAdmin.Checked));
            prms.Add(new SqlParameter("@IsReadOnly", chkReadOnly.Checked));
            prms.Add(new SqlParameter("@ProcessModule", chkProcessModule.Checked));
            prms.Add(new SqlParameter("@SearchModule", chkSearchModule.Checked));
            prms.Add(new SqlParameter("@ReportModule", chkReportModule.Checked));
            prms.Add(new SqlParameter("@UtilityModule", chkUtilityModule.Checked));
            prms.Add(new SqlParameter("@LookUpModule", chkLookUpModule.Checked));
            prms.Add(new SqlParameter("@AdminModule", chkAdminModule.Checked));
            prms.Add(new SqlParameter("@RiskModule", chkRiskModule.Checked));

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

            prms.Add(new SqlParameter("@UserID", DataLayer.Int2Field(txtUserID.Text)));
            prms.Add(new SqlParameter("@Firstname", txtFirstname.Text ));
            prms.Add(new SqlParameter("@Lastname", txtLastname.Text ));
            prms.Add(new SqlParameter("@LoginID", txtLoginID.Text));
            prms.Add(new SqlParameter("@Password", txtPassword.Text));
            prms.Add(new SqlParameter("@IsAdmin", chkAdmin.Checked));
            prms.Add(new SqlParameter("@IsReadOnly", chkReadOnly.Checked));
            prms.Add(new SqlParameter("@ProcessModule", chkProcessModule.Checked));
            prms.Add(new SqlParameter("@SearchModule", chkSearchModule.Checked));
            prms.Add(new SqlParameter("@ReportModule", chkReportModule.Checked));
            prms.Add(new SqlParameter("@UtilityModule", chkUtilityModule.Checked));
            prms.Add(new SqlParameter("@LookUpModule", chkLookUpModule.Checked));
            prms.Add(new SqlParameter("@AdminModule", chkAdminModule.Checked));
            prms.Add(new SqlParameter("@RiskModule", chkRiskModule.Checked));


            int intRows = this.Data.Update(prms);

            if (intRows > 0)
            {
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();

                //if (txtUserID.Text == main.g_User.UserID.ToString())
                //{
                //    main.g_frmMain.GetUser();
                //    main.g_frmMain.SetFormSecurity();
                //}

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

            if (txtLoginID.Text.Trim() == string.Empty)
                strError += "Please enter a Login ID.\n";

            if (txtLastname.Text.Trim() == string.Empty)
                strError += "Please enter a Last name.\n";

            if (txtFirstname.Text.Trim() == string.Empty)
                strError += "Please enter a First name.\n";

            if (txtPassword.Text.Trim() != txtConfirm.Text.Trim())
                strError += "Password and confirmation does not match.  Please re-enter.\n";

            if (strError == string.Empty)
                return true;
            else
            {
                                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        public iFormData CloneDataForm()
        {
            frmUser frm = new frmUser();
            frm.MdiParent = main.g_frmMain;
            frm.WindowState = FormWindowState.Normal;
            return frm;
        }
       
        private void frmUser_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }

            
        }
    }
}