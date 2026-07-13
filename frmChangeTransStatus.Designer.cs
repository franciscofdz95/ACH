namespace ACH2005
{
    partial class frmChangeTransStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangeTransStatus));
            this.tbrTop = new System.Windows.Forms.ToolStrip();
            this.tbrSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrChangeStatus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrChangeProcessDate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.txtRefID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTransID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.txtAccountNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTransEndDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTransBeginDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.grdSearch = new System.Windows.Forms.DataGridView();
            this.tbrTop.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.SuspendLayout();
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
            this.tbrChangeProcessDate,
            this.toolStripSeparator4});
            this.tbrTop.Location = new System.Drawing.Point(0, 0);
            this.tbrTop.Name = "tbrTop";
            this.tbrTop.Size = new System.Drawing.Size(1011, 25);
            this.tbrTop.TabIndex = 0;
            this.tbrTop.Text = "toolStrip1";
            this.tbrTop.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbrTop_ItemClicked);
            // 
            // tbrSearch
            // 
            this.tbrSearch.Image = ((System.Drawing.Image)(resources.GetObject("tbrSearch.Image")));
            this.tbrSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrSearch.Name = "tbrSearch";
            this.tbrSearch.Size = new System.Drawing.Size(60, 22);
            this.tbrSearch.Text = "Search";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbrClear
            // 
            this.tbrClear.Image = ((System.Drawing.Image)(resources.GetObject("tbrClear.Image")));
            this.tbrClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrClear.Name = "tbrClear";
            this.tbrClear.Size = new System.Drawing.Size(52, 22);
            this.tbrClear.Text = "Clear";
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
            this.tbrChangeStatus.Size = new System.Drawing.Size(98, 22);
            this.tbrChangeStatus.Text = "Change Status";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tbrChangeProcessDate
            // 
            this.tbrChangeProcessDate.Image = ((System.Drawing.Image)(resources.GetObject("tbrChangeProcessDate.Image")));
            this.tbrChangeProcessDate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrChangeProcessDate.Name = "tbrChangeProcessDate";
            this.tbrChangeProcessDate.Size = new System.Drawing.Size(130, 22);
            this.tbrChangeProcessDate.Text = "Change Process Date";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // txtAchID
            // 
            this.txtAchID.BackColor = System.Drawing.SystemColors.Window;
            this.txtAchID.Location = new System.Drawing.Point(325, 27);
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 30;
            // 
            // txtRefID
            // 
            this.txtRefID.Location = new System.Drawing.Point(262, 53);
            this.txtRefID.Name = "txtRefID";
            this.txtRefID.Size = new System.Drawing.Size(100, 20);
            this.txtRefID.TabIndex = 100;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ref ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "ACH ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(205, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Trans ID:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtRefID);
            this.groupBox2.Controls.Add(this.txtTransID);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtAccountName);
            this.groupBox2.Controls.Add(this.txtAccountNo);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(461, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 85);
            this.groupBox2.TabIndex = 60;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Optional Search Fields";
            // 
            // txtTransID
            // 
            this.txtTransID.Location = new System.Drawing.Point(262, 27);
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.Size = new System.Drawing.Size(100, 20);
            this.txtTransID.TabIndex = 80;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Account Name:";
            // 
            // txtAccountName
            // 
            this.txtAccountName.Location = new System.Drawing.Point(97, 53);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(100, 20);
            this.txtAccountName.TabIndex = 90;
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Location = new System.Drawing.Point(97, 27);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(100, 20);
            this.txtAccountNo.TabIndex = 70;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Account No.:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.txtTransEndDate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTransBeginDate);
            this.groupBox1.Controls.Add(this.txtAchID);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtMerchantID);
            this.groupBox1.Location = new System.Drawing.Point(16, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(439, 85);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Required Search Fields";
            // 
            // txtTransEndDate
            // 
            this.txtTransEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtTransEndDate.Location = new System.Drawing.Point(325, 52);
            this.txtTransEndDate.Name = "txtTransEndDate";
            this.txtTransEndDate.Size = new System.Drawing.Size(100, 20);
            this.txtTransEndDate.TabIndex = 50;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Merchant ID:";
            // 
            // txtTransBeginDate
            // 
            this.txtTransBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtTransBeginDate.Location = new System.Drawing.Point(127, 52);
            this.txtTransBeginDate.Name = "txtTransBeginDate";
            this.txtTransBeginDate.Size = new System.Drawing.Size(100, 20);
            this.txtTransBeginDate.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(234, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Trans End Date:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Trans Begin Date:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.BackColor = System.Drawing.SystemColors.Window;
            this.txtMerchantID.Location = new System.Drawing.Point(127, 28);
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 20;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 125);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1011, 3);
            this.splitter1.TabIndex = 14;
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
            this.panel1.Size = new System.Drawing.Size(1011, 100);
            this.panel1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label11);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 633);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1011, 31);
            this.panel4.TabIndex = 114;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(143, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 13);
            this.label11.TabIndex = 47;
            this.label11.Text = "Unable to update";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Gainsboro;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(122, 7);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(15, 15);
            this.panel5.TabIndex = 46;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(26, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "Able to update";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.LemonChiffon;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Location = new System.Drawing.Point(5, 7);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(15, 15);
            this.panel6.TabIndex = 44;
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
            this.grdSearch.Size = new System.Drawing.Size(1011, 505);
            this.grdSearch.TabIndex = 115;
            this.grdSearch.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.grdSearch_RowPrePaint);
            // 
            // frmChangeTransStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 664);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbrTop);
            this.KeyPreview = true;
            this.Name = "frmChangeTransStatus";
            this.Text = "Change Transaction Status";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmChangeTransStatus_KeyDown);
            this.Load += new System.EventHandler(this.frmChangeTransStatus_Load);
            this.tbrTop.ResumeLayout(false);
            this.tbrTop.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tbrTop;
        private System.Windows.Forms.ToolStripButton tbrSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbrClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbrChangeStatus;
        private System.Windows.Forms.ToolStripButton tbrChangeProcessDate;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.TextBox txtRefID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTransID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.TextBox txtAccountNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker txtTransEndDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker txtTransBeginDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridView grdSearch;

    }
}