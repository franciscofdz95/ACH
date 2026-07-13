namespace AchSystem
{
    partial class frmUser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUser));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.txtLastname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFirstname = new System.Windows.Forms.TextBox();
            this.chkAdmin = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkAdminModule = new System.Windows.Forms.CheckBox();
            this.chkLookUpModule = new System.Windows.Forms.CheckBox();
            this.chkUtilityModule = new System.Windows.Forms.CheckBox();
            this.chkReportModule = new System.Windows.Forms.CheckBox();
            this.chkSearchModule = new System.Windows.Forms.CheckBox();
            this.chkProcessModule = new System.Windows.Forms.CheckBox();
            this.chkReadOnly = new System.Windows.Forms.CheckBox();
            this.chkRiskModule = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.Images.SetKeyName(0, "filesave.gif");
            this.imgList.Images.SetKeyName(1, "delete.gif");
            this.imgList.Images.SetKeyName(2, "new.gif");
            this.imgList.Images.SetKeyName(3, "search.gif");
            this.imgList.Images.SetKeyName(4, "undo.gif");
            this.imgList.Images.SetKeyName(5, "greencheck3.gif");
            this.imgList.Images.SetKeyName(6, "Exit.gif");
            this.imgList.Images.SetKeyName(7, "close.gif");
            this.imgList.Images.SetKeyName(8, "ckbx_grn.gif");
            this.imgList.Images.SetKeyName(9, "ig_tbarHistory2.gif");
            this.imgList.Images.SetKeyName(10, "status.gif");
            this.imgList.Images.SetKeyName(11, "Credit_.GIF");
            this.imgList.Images.SetKeyName(12, "Road2.gif");
            // 
            // tbrTop
            // 
            this.tbrTop.MenuSettings.ForceSerialization = true;
            this.tbrTop.ToolbarSettings.ForceSerialization = true;
            // 
            // txtLastname
            // 
            this.txtLastname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastname.Location = new System.Drawing.Point(119, 136);
            this.txtLastname.MaxLength = 50;
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.Size = new System.Drawing.Size(330, 20);
            this.txtLastname.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 56;
            this.label2.Text = "User ID:";
            // 
            // txtUserID
            // 
            this.txtUserID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserID.Location = new System.Drawing.Point(119, 6);
            this.txtUserID.MaxLength = 10;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.ReadOnly = true;
            this.txtUserID.Size = new System.Drawing.Size(100, 20);
            this.txtUserID.TabIndex = 10;
            this.txtUserID.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(15, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 112;
            this.label1.Text = "User Name:";
            // 
            // txtLoginID
            // 
            this.txtLoginID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginID.Location = new System.Drawing.Point(119, 32);
            this.txtLoginID.MaxLength = 20;
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.Size = new System.Drawing.Size(100, 20);
            this.txtLoginID.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(15, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 58;
            this.label3.Text = "First name:";
            // 
            // txtFirstname
            // 
            this.txtFirstname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFirstname.Location = new System.Drawing.Point(119, 110);
            this.txtFirstname.MaxLength = 50;
            this.txtFirstname.Name = "txtFirstname";
            this.txtFirstname.Size = new System.Drawing.Size(330, 20);
            this.txtFirstname.TabIndex = 50;
            // 
            // chkAdmin
            // 
            this.chkAdmin.AutoSize = true;
            this.chkAdmin.BackColor = System.Drawing.Color.Transparent;
            this.chkAdmin.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdmin.Location = new System.Drawing.Point(368, 35);
            this.chkAdmin.Name = "chkAdmin";
            this.chkAdmin.Size = new System.Drawing.Size(72, 17);
            this.chkAdmin.TabIndex = 40;
            this.chkAdmin.Text = "Is Admin?";
            this.chkAdmin.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(15, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 60;
            this.label4.Text = "Last name:";
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(463, 340);
            appearance1.FontData.BoldAsString = "True";
            this.ultraExpandableGroupBox1.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 26);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(463, 340);
            this.ultraExpandableGroupBox1.TabIndex = 16;
            this.ultraExpandableGroupBox1.Text = "Required Fields";
            this.ultraExpandableGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkRiskModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label6);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtConfirm);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label5);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtPassword);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkAdminModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkLookUpModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkUtilityModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkReportModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkSearchModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkProcessModule);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkReadOnly);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label1);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label2);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtLoginID);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtLastname);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label4);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtUserID);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkAdmin);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label3);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtFirstname);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(459, 318);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(15, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 124;
            this.label6.Text = "Confirm Password:";
            // 
            // txtConfirm
            // 
            this.txtConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfirm.Location = new System.Drawing.Point(119, 84);
            this.txtConfirm.MaxLength = 20;
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.Size = new System.Drawing.Size(184, 20);
            this.txtConfirm.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(15, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 122;
            this.label5.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(119, 58);
            this.txtPassword.MaxLength = 20;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(184, 20);
            this.txtPassword.TabIndex = 25;
            // 
            // chkAdminModule
            // 
            this.chkAdminModule.AutoSize = true;
            this.chkAdminModule.BackColor = System.Drawing.Color.Transparent;
            this.chkAdminModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAdminModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdminModule.Location = new System.Drawing.Point(245, 223);
            this.chkAdminModule.Name = "chkAdminModule";
            this.chkAdminModule.Size = new System.Drawing.Size(99, 17);
            this.chkAdminModule.TabIndex = 120;
            this.chkAdminModule.Text = "Admin Module?";
            this.chkAdminModule.UseVisualStyleBackColor = false;
            // 
            // chkLookUpModule
            // 
            this.chkLookUpModule.AutoSize = true;
            this.chkLookUpModule.BackColor = System.Drawing.Color.Transparent;
            this.chkLookUpModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkLookUpModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLookUpModule.Location = new System.Drawing.Point(236, 246);
            this.chkLookUpModule.Name = "chkLookUpModule";
            this.chkLookUpModule.Size = new System.Drawing.Size(108, 17);
            this.chkLookUpModule.TabIndex = 110;
            this.chkLookUpModule.Text = "LookUp Module?";
            this.chkLookUpModule.UseVisualStyleBackColor = false;
            // 
            // chkUtilityModule
            // 
            this.chkUtilityModule.AutoSize = true;
            this.chkUtilityModule.BackColor = System.Drawing.Color.Transparent;
            this.chkUtilityModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUtilityModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUtilityModule.Location = new System.Drawing.Point(89, 223);
            this.chkUtilityModule.Name = "chkUtilityModule";
            this.chkUtilityModule.Size = new System.Drawing.Size(95, 17);
            this.chkUtilityModule.TabIndex = 100;
            this.chkUtilityModule.Text = "Utility Module?";
            this.chkUtilityModule.UseVisualStyleBackColor = false;
            // 
            // chkReportModule
            // 
            this.chkReportModule.AutoSize = true;
            this.chkReportModule.BackColor = System.Drawing.Color.Transparent;
            this.chkReportModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkReportModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkReportModule.Location = new System.Drawing.Point(242, 200);
            this.chkReportModule.Name = "chkReportModule";
            this.chkReportModule.Size = new System.Drawing.Size(102, 17);
            this.chkReportModule.TabIndex = 90;
            this.chkReportModule.Text = "Report Module?";
            this.chkReportModule.UseVisualStyleBackColor = false;
            // 
            // chkSearchModule
            // 
            this.chkSearchModule.AutoSize = true;
            this.chkSearchModule.BackColor = System.Drawing.Color.Transparent;
            this.chkSearchModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSearchModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSearchModule.Location = new System.Drawing.Point(80, 246);
            this.chkSearchModule.Name = "chkSearchModule";
            this.chkSearchModule.Size = new System.Drawing.Size(104, 17);
            this.chkSearchModule.TabIndex = 80;
            this.chkSearchModule.Text = "Search Module?";
            this.chkSearchModule.UseVisualStyleBackColor = false;
            // 
            // chkProcessModule
            // 
            this.chkProcessModule.AutoSize = true;
            this.chkProcessModule.BackColor = System.Drawing.Color.Transparent;
            this.chkProcessModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkProcessModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkProcessModule.Location = new System.Drawing.Point(76, 200);
            this.chkProcessModule.Name = "chkProcessModule";
            this.chkProcessModule.Size = new System.Drawing.Size(108, 17);
            this.chkProcessModule.TabIndex = 70;
            this.chkProcessModule.Text = "Process Module?";
            this.chkProcessModule.UseVisualStyleBackColor = false;
            // 
            // chkReadOnly
            // 
            this.chkReadOnly.AutoSize = true;
            this.chkReadOnly.BackColor = System.Drawing.Color.Transparent;
            this.chkReadOnly.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkReadOnly.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkReadOnly.Location = new System.Drawing.Point(358, 9);
            this.chkReadOnly.Name = "chkReadOnly";
            this.chkReadOnly.Size = new System.Drawing.Size(82, 17);
            this.chkReadOnly.TabIndex = 30;
            this.chkReadOnly.Text = "Read Only?";
            this.chkReadOnly.UseVisualStyleBackColor = false;
            // 
            // chkRiskModule
            // 
            this.chkRiskModule.AutoSize = true;
            this.chkRiskModule.BackColor = System.Drawing.Color.Transparent;
            this.chkRiskModule.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRiskModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRiskModule.Location = new System.Drawing.Point(253, 269);
            this.chkRiskModule.Name = "chkRiskModule";
            this.chkRiskModule.Size = new System.Drawing.Size(91, 17);
            this.chkRiskModule.TabIndex = 125;
            this.chkRiskModule.Text = "Risk Module?";
            this.chkRiskModule.UseVisualStyleBackColor = false;
            // 
            // frmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 366);
            this.Controls.Add(this.ultraExpandableGroupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUser";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUser_FormClosed);
            this.Controls.SetChildIndex(this.ultraExpandableGroupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtLastname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFirstname;
        private System.Windows.Forms.CheckBox chkAdmin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLoginID;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.CheckBox chkAdminModule;
        private System.Windows.Forms.CheckBox chkLookUpModule;
        private System.Windows.Forms.CheckBox chkUtilityModule;
        private System.Windows.Forms.CheckBox chkReportModule;
        private System.Windows.Forms.CheckBox chkSearchModule;
        private System.Windows.Forms.CheckBox chkProcessModule;
        private System.Windows.Forms.CheckBox chkReadOnly;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConfirm;
        private System.Windows.Forms.CheckBox chkRiskModule;
    }
}