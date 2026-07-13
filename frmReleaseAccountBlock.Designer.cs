namespace ACH2005
{
    partial class frmReleaseAccountBlock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReleaseAccountBlock));
            this.label6 = new System.Windows.Forms.Label();
            this.txtTransRoute = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboTransType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAccountNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrSearch = new System.Windows.Forms.ToolStripButton();
            this.grdSearch = new System.Windows.Forms.DataGridView();
            this.tbrClear = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbrTop = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrChangeStatus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.panel1.SuspendLayout();
            this.tbrTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Trans Route:";
            // 
            // txtTransRoute
            // 
            this.txtTransRoute.Location = new System.Drawing.Point(98, 53);
            this.txtTransRoute.MaxLength = 9;
            this.txtTransRoute.Name = "txtTransRoute";
            this.txtTransRoute.Size = new System.Drawing.Size(100, 20);
            this.txtTransRoute.TabIndex = 30;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboTransType);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(513, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(208, 85);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optional Search Fields";
            // 
            // cboTransType
            // 
            this.cboTransType.FormattingEnabled = true;
            this.cboTransType.Location = new System.Drawing.Point(76, 27);
            this.cboTransType.MaxLength = 3;
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(76, 21);
            this.cboTransType.TabIndex = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Trans Type:";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Location = new System.Drawing.Point(281, 53);
            this.txtAccountNo.MaxLength = 17;
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(190, 20);
            this.txtAccountNo.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(205, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Account No.:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMerchantID);
            this.groupBox1.Controls.Add(this.txtAchID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtAccountNo);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTransRoute);
            this.groupBox1.Location = new System.Drawing.Point(16, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 85);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Required Search Fields";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Merchant ID:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.BackColor = System.Drawing.SystemColors.Window;
            this.txtMerchantID.Location = new System.Drawing.Point(98, 28);
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 20;
            // 
            // txtAchID
            // 
            this.txtAchID.Location = new System.Drawing.Point(281, 27);
            this.txtAchID.MaxLength = 12;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "ACH ID:";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbrSearch
            // 
            this.tbrSearch.Image = ((System.Drawing.Image)(resources.GetObject("tbrSearch.Image")));
            this.tbrSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrSearch.Name = "tbrSearch";
            this.tbrSearch.Size = new System.Drawing.Size(60, 22);
            this.tbrSearch.Text = "Search";
            // 
            // grdSearch
            // 
            this.grdSearch.AllowUserToAddRows = false;
            this.grdSearch.AllowUserToDeleteRows = false;
            this.grdSearch.AllowUserToResizeRows = false;
            this.grdSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSearch.Location = new System.Drawing.Point(0, 128);
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.ReadOnly = true;
            this.grdSearch.RowHeadersVisible = false;
            this.grdSearch.RowTemplate.Height = 19;
            this.grdSearch.Size = new System.Drawing.Size(745, 288);
            this.grdSearch.TabIndex = 70;
            // 
            // tbrClear
            // 
            this.tbrClear.Image = ((System.Drawing.Image)(resources.GetObject("tbrClear.Image")));
            this.tbrClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrClear.Name = "tbrClear";
            this.tbrClear.Size = new System.Drawing.Size(52, 22);
            this.tbrClear.Text = "Clear";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 125);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(745, 3);
            this.splitter1.TabIndex = 113;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(745, 100);
            this.panel1.TabIndex = 1;
            // 
            // tbrTop
            // 
            this.tbrTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbrSearch,
            this.toolStripSeparator2,
            this.tbrClear,
            this.toolStripSeparator1,
            this.tbrChangeStatus,
            this.toolStripSeparator3,
            this.toolStripButton1,
            this.toolStripSeparator4});
            this.tbrTop.Location = new System.Drawing.Point(0, 0);
            this.tbrTop.Name = "tbrTop";
            this.tbrTop.Size = new System.Drawing.Size(745, 25);
            this.tbrTop.TabIndex = 111;
            this.tbrTop.Text = "toolStrip1";
            this.tbrTop.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbrTop_ItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbrChangeStatus
            // 
            this.tbrChangeStatus.Image = ((System.Drawing.Image)(resources.GetObject("tbrChangeStatus.Image")));
            this.tbrChangeStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrChangeStatus.Name = "tbrChangeStatus";
            this.tbrChangeStatus.Size = new System.Drawing.Size(134, 22);
            this.tbrChangeStatus.Text = "Release Account Block";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(115, 22);
            this.toolStripButton1.Text = "Add Account Block";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // frmReleaseAccountBlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 416);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbrTop);
            this.KeyPreview = true;
            this.Name = "frmReleaseAccountBlock";
            this.Text = "Release Account Block";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmReleaseAccountBlock_KeyDown);
            this.Load += new System.EventHandler(this.frmReleaseAccountBlock_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tbrTop.ResumeLayout(false);
            this.tbrTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTransRoute;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAccountNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbrSearch;
        private System.Windows.Forms.DataGridView grdSearch;
        private System.Windows.Forms.ToolStripButton tbrClear;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip tbrTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbrChangeStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ComboBox cboTransType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}