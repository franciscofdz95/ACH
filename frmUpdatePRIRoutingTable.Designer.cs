namespace AchSystem
{
    partial class frmUpdatePRIRoutingTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdatePRIRoutingTable));
            this.label1 = new System.Windows.Forms.Label();
            this.cboReasonCode2 = new System.Windows.Forms.ComboBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabReleaseAccountBlock = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.tabAddAccoutBlock = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cboReasonCode = new System.Windows.Forms.ComboBox();
            this.cboTransType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAccountNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTransRoute = new System.Windows.Forms.TextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tbrTop = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabMain.SuspendLayout();
            this.tabReleaseAccountBlock.SuspendLayout();
            this.tabAddAccoutBlock.SuspendLayout();
            this.tbrTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Reason Code:";
            // 
            // cboReasonCode2
            // 
            this.cboReasonCode2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReasonCode2.FormattingEnabled = true;
            this.cboReasonCode2.Location = new System.Drawing.Point(108, 70);
            this.cboReasonCode2.MaxLength = 3;
            this.cboReasonCode2.Name = "cboReasonCode2";
            this.cboReasonCode2.Size = new System.Drawing.Size(136, 21);
            this.cboReasonCode2.TabIndex = 10;
            // 
            // tabMain
            // 
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMain.Controls.Add(this.tabReleaseAccountBlock);
            this.tabMain.Controls.Add(this.tabAddAccoutBlock);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 25);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(337, 252);
            this.tabMain.TabIndex = 1;
            // 
            // tabReleaseAccountBlock
            // 
            this.tabReleaseAccountBlock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabReleaseAccountBlock.Controls.Add(this.label4);
            this.tabReleaseAccountBlock.Controls.Add(this.label1);
            this.tabReleaseAccountBlock.Controls.Add(this.cboReasonCode2);
            this.tabReleaseAccountBlock.Location = new System.Drawing.Point(4, 25);
            this.tabReleaseAccountBlock.Name = "tabReleaseAccountBlock";
            this.tabReleaseAccountBlock.Padding = new System.Windows.Forms.Padding(3);
            this.tabReleaseAccountBlock.Size = new System.Drawing.Size(329, 223);
            this.tabReleaseAccountBlock.TabIndex = 0;
            this.tabReleaseAccountBlock.Text = "Release Account Block";
            this.tabReleaseAccountBlock.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(6, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(297, 51);
            this.label4.TabIndex = 2;
            this.label4.Text = "Note: When the save button is pressed, selected reason code will be released.";
            // 
            // tabAddAccoutBlock
            // 
            this.tabAddAccoutBlock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabAddAccoutBlock.Controls.Add(this.label7);
            this.tabAddAccoutBlock.Controls.Add(this.cboReasonCode);
            this.tabAddAccoutBlock.Controls.Add(this.cboTransType);
            this.tabAddAccoutBlock.Controls.Add(this.label2);
            this.tabAddAccoutBlock.Controls.Add(this.txtAchID);
            this.tabAddAccoutBlock.Controls.Add(this.label3);
            this.tabAddAccoutBlock.Controls.Add(this.txtAccountNo);
            this.tabAddAccoutBlock.Controls.Add(this.label6);
            this.tabAddAccoutBlock.Controls.Add(this.label5);
            this.tabAddAccoutBlock.Controls.Add(this.txtTransRoute);
            this.tabAddAccoutBlock.Location = new System.Drawing.Point(4, 25);
            this.tabAddAccoutBlock.Name = "tabAddAccoutBlock";
            this.tabAddAccoutBlock.Padding = new System.Windows.Forms.Padding(3);
            this.tabAddAccoutBlock.Size = new System.Drawing.Size(329, 223);
            this.tabAddAccoutBlock.TabIndex = 1;
            this.tabAddAccoutBlock.Text = "Add Account Block";
            this.tabAddAccoutBlock.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 64;
            this.label7.Text = "Reason Code:";
            // 
            // cboReasonCode
            // 
            this.cboReasonCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReasonCode.FormattingEnabled = true;
            this.cboReasonCode.Location = new System.Drawing.Point(94, 144);
            this.cboReasonCode.MaxLength = 3;
            this.cboReasonCode.Name = "cboReasonCode";
            this.cboReasonCode.Size = new System.Drawing.Size(136, 21);
            this.cboReasonCode.TabIndex = 60;
            // 
            // cboTransType
            // 
            this.cboTransType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTransType.FormattingEnabled = true;
            this.cboTransType.Location = new System.Drawing.Point(94, 117);
            this.cboTransType.MaxLength = 2;
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(71, 21);
            this.cboTransType.TabIndex = 50;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Trans Type:";
            // 
            // txtAchID
            // 
            this.txtAchID.Location = new System.Drawing.Point(94, 39);
            this.txtAchID.MaxLength = 12;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 41;
            this.label3.Text = "ACH ID:";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Location = new System.Drawing.Point(94, 91);
            this.txtAccountNo.MaxLength = 17;
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(190, 20);
            this.txtAccountNo.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 43;
            this.label6.Text = "Trans Route:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 42;
            this.label5.Text = "Account No.:";
            // 
            // txtTransRoute
            // 
            this.txtTransRoute.Location = new System.Drawing.Point(94, 65);
            this.txtTransRoute.MaxLength = 9;
            this.txtTransRoute.Name = "txtTransRoute";
            this.txtTransRoute.Size = new System.Drawing.Size(100, 20);
            this.txtTransRoute.TabIndex = 30;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton1.Text = "&Save";
            // 
            // tbrTop
            // 
            this.tbrTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator2});
            this.tbrTop.Location = new System.Drawing.Point(0, 0);
            this.tbrTop.Name = "tbrTop";
            this.tbrTop.Size = new System.Drawing.Size(337, 25);
            this.tbrTop.TabIndex = 2;
            this.tbrTop.Text = "toolStrip1";
            this.tbrTop.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbrTop_ItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton2.Text = "&Close";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // frmUpdatePRIRoutingTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 277);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.tbrTop);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdatePRIRoutingTable";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update PRI Routing Table";
            this.Load += new System.EventHandler(this.frmUpdatePRIRoutingTable_Load);
            this.tabMain.ResumeLayout(false);
            this.tabReleaseAccountBlock.ResumeLayout(false);
            this.tabReleaseAccountBlock.PerformLayout();
            this.tabAddAccoutBlock.ResumeLayout(false);
            this.tabAddAccoutBlock.PerformLayout();
            this.tbrTop.ResumeLayout(false);
            this.tbrTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboReasonCode2;
        public System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabReleaseAccountBlock;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip tbrTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TabPage tabAddAccoutBlock;
        public System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox txtAccountNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox txtTransRoute;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cboTransType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox cboReasonCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}