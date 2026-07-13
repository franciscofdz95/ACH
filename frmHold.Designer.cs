namespace AchSystem
{
    partial class frmHold
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHold));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.btnMerchant = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.txtMerchantName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.lnkJournalIDOut = new System.Windows.Forms.LinkLabel();
            this.lnkJournalIDIn = new System.Windows.Forms.LinkLabel();
            this.txtPostedDate = new System.Windows.Forms.MaskedTextBox();
            this.txtPaidDate = new System.Windows.Forms.MaskedTextBox();
            this.txtReleaseDate = new System.Windows.Forms.MaskedTextBox();
            this.cboReleaseHold = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtJournalIDOut = new System.Windows.Forms.TextBox();
            this.txtJournalIDIn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.txtHoldID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtJournalID = new System.Windows.Forms.TextBox();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.pnlMain = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
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
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(17, 198);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
            this.label9.TabIndex = 90;
            this.label9.Text = "Release Hold?";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(17, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 80;
            this.label4.Text = "Release Date:";
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(122, 142);
            this.txtAmount.MaxLength = 20;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(121, 20);
            this.txtAmount.TabIndex = 70;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(17, 145);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 88;
            this.label8.Text = "Amount:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(17, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Posted Date:";
            // 
            // txtAchID
            // 
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(122, 11);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.ReadOnly = true;
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 10;
            this.txtAchID.TabStop = false;
            this.txtAchID.Leave += new System.EventHandler(this.txtAchID_Leave);
            // 
            // btnMerchant
            // 
            this.btnMerchant.Enabled = false;
            this.btnMerchant.Image = global::AchSystem.Properties.Resources.search1;
            this.btnMerchant.Location = new System.Drawing.Point(228, 10);
            this.btnMerchant.Name = "btnMerchant";
            this.btnMerchant.Size = new System.Drawing.Size(29, 21);
            this.btnMerchant.TabIndex = 20;
            this.btnMerchant.UseVisualStyleBackColor = true;
            this.btnMerchant.Click += new System.EventHandler(this.btnMerchant_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(17, 40);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(69, 13);
            this.label22.TabIndex = 184;
            this.label22.Text = "Merchant ID:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(122, 37);
            this.txtMerchantID.MaxLength = 9;
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.ReadOnly = true;
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 30;
            // 
            // txtMerchantName
            // 
            this.txtMerchantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantName.Location = new System.Drawing.Point(122, 63);
            this.txtMerchantName.MaxLength = 16;
            this.txtMerchantName.Name = "txtMerchantName";
            this.txtMerchantName.ReadOnly = true;
            this.txtMerchantName.Size = new System.Drawing.Size(350, 20);
            this.txtMerchantName.TabIndex = 40;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(16, 66);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(86, 13);
            this.label18.TabIndex = 183;
            this.label18.Text = "Merchant Name:";
            // 
            // lnkJournalIDOut
            // 
            this.lnkJournalIDOut.AutoSize = true;
            this.lnkJournalIDOut.BackColor = System.Drawing.Color.Transparent;
            this.lnkJournalIDOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkJournalIDOut.Location = new System.Drawing.Point(15, 32);
            this.lnkJournalIDOut.Name = "lnkJournalIDOut";
            this.lnkJournalIDOut.Size = new System.Drawing.Size(93, 13);
            this.lnkJournalIDOut.TabIndex = 120;
            this.lnkJournalIDOut.TabStop = true;
            this.lnkJournalIDOut.Text = "Journal ID Out:";
            this.lnkJournalIDOut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkJournalIDOut_LinkClicked);
            // 
            // lnkJournalIDIn
            // 
            this.lnkJournalIDIn.AutoSize = true;
            this.lnkJournalIDIn.BackColor = System.Drawing.Color.Transparent;
            this.lnkJournalIDIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkJournalIDIn.Location = new System.Drawing.Point(15, 6);
            this.lnkJournalIDIn.Name = "lnkJournalIDIn";
            this.lnkJournalIDIn.Size = new System.Drawing.Size(84, 13);
            this.lnkJournalIDIn.TabIndex = 100;
            this.lnkJournalIDIn.TabStop = true;
            this.lnkJournalIDIn.Text = "Journal ID In:";
            this.lnkJournalIDIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkJournalIDIn_LinkClicked);
            // 
            // txtPostedDate
            // 
            this.txtPostedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostedDate.Location = new System.Drawing.Point(122, 116);
            this.txtPostedDate.Mask = "00/00/0000 90:00:00";
            this.txtPostedDate.Name = "txtPostedDate";
            this.txtPostedDate.Size = new System.Drawing.Size(121, 20);
            this.txtPostedDate.TabIndex = 60;
            // 
            // txtPaidDate
            // 
            this.txtPaidDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPaidDate.Location = new System.Drawing.Point(120, 55);
            this.txtPaidDate.Mask = "00/00/0000 90:00:00";
            this.txtPaidDate.Name = "txtPaidDate";
            this.txtPaidDate.Size = new System.Drawing.Size(121, 20);
            this.txtPaidDate.TabIndex = 150;
            this.txtPaidDate.Enter += new System.EventHandler(this.txtPaidDate_Enter);
            // 
            // txtReleaseDate
            // 
            this.txtReleaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReleaseDate.Location = new System.Drawing.Point(122, 222);
            this.txtReleaseDate.Mask = "00/00/0000 90:00:00";
            this.txtReleaseDate.Name = "txtReleaseDate";
            this.txtReleaseDate.Size = new System.Drawing.Size(121, 20);
            this.txtReleaseDate.TabIndex = 95;
            // 
            // cboReleaseHold
            // 
            this.cboReleaseHold.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseHold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboReleaseHold.FormattingEnabled = true;
            this.cboReleaseHold.Location = new System.Drawing.Point(122, 195);
            this.cboReleaseHold.Name = "cboReleaseHold";
            this.cboReleaseHold.Size = new System.Drawing.Size(100, 21);
            this.cboReleaseHold.TabIndex = 90;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 102;
            this.label5.Text = "Paid Date:";
            // 
            // txtJournalIDOut
            // 
            this.txtJournalIDOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJournalIDOut.Location = new System.Drawing.Point(120, 29);
            this.txtJournalIDOut.Name = "txtJournalIDOut";
            this.txtJournalIDOut.Size = new System.Drawing.Size(100, 20);
            this.txtJournalIDOut.TabIndex = 130;
            // 
            // txtJournalIDIn
            // 
            this.txtJournalIDIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJournalIDIn.Location = new System.Drawing.Point(120, 3);
            this.txtJournalIDIn.Name = "txtJournalIDIn";
            this.txtJournalIDIn.Size = new System.Drawing.Size(100, 20);
            this.txtJournalIDIn.TabIndex = 110;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Hold ID:";
            // 
            // cboType
            // 
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(122, 168);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(100, 21);
            this.cboType.TabIndex = 80;
            // 
            // txtHoldID
            // 
            this.txtHoldID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHoldID.Location = new System.Drawing.Point(122, 90);
            this.txtHoldID.Name = "txtHoldID";
            this.txtHoldID.ReadOnly = true;
            this.txtHoldID.Size = new System.Drawing.Size(100, 20);
            this.txtHoldID.TabIndex = 50;
            this.txtHoldID.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(17, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 84;
            this.label6.Text = "Type:";
            // 
            // txtJournalID
            // 
            this.txtJournalID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJournalID.Location = new System.Drawing.Point(97, 56);
            this.txtJournalID.Name = "txtJournalID";
            this.txtJournalID.ReadOnly = true;
            this.txtJournalID.Size = new System.Drawing.Size(100, 20);
            this.txtJournalID.TabIndex = 77;
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.pnlMain);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(329, 133);
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.ultraExpandableGroupBox1.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 28);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(492, 275);
            this.ultraExpandableGroupBox1.TabIndex = 16;
            this.ultraExpandableGroupBox1.Text = "Required Fields";
            this.ultraExpandableGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.linkLabel1);
            this.pnlMain.Controls.Add(this.btnMerchant);
            this.pnlMain.Controls.Add(this.txtMerchantName);
            this.pnlMain.Controls.Add(this.label4);
            this.pnlMain.Controls.Add(this.txtPostedDate);
            this.pnlMain.Controls.Add(this.label22);
            this.pnlMain.Controls.Add(this.cboReleaseHold);
            this.pnlMain.Controls.Add(this.txtReleaseDate);
            this.pnlMain.Controls.Add(this.txtMerchantID);
            this.pnlMain.Controls.Add(this.txtAchID);
            this.pnlMain.Controls.Add(this.label18);
            this.pnlMain.Controls.Add(this.cboType);
            this.pnlMain.Controls.Add(this.label9);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label6);
            this.pnlMain.Controls.Add(this.txtHoldID);
            this.pnlMain.Controls.Add(this.txtAmount);
            this.pnlMain.Controls.Add(this.label8);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(2, 20);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(488, 253);
            this.pnlMain.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(17, 14);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(50, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Ach ID:";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // ultraExpandableGroupBox2
            // 
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(492, 133);
            appearance2.FontData.BoldAsString = "True";
            this.ultraExpandableGroupBox2.HeaderAppearance = appearance2;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 303);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(492, 133);
            this.ultraExpandableGroupBox2.TabIndex = 17;
            this.ultraExpandableGroupBox2.Text = "Optional Fields";
            this.ultraExpandableGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lnkJournalIDOut);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtJournalIDIn);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lnkJournalIDIn);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtPaidDate);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtJournalIDOut);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label5);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(488, 111);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // frmHold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 436);
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Controls.Add(this.ultraExpandableGroupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHold";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hold";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHold_FormClosed);
            this.Controls.SetChildIndex(this.ultraExpandableGroupBox1, 0);
            this.Controls.SetChildIndex(this.ultraExpandableGroupBox2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtJournalID;
        private System.Windows.Forms.TextBox txtHoldID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtJournalIDIn;
        private System.Windows.Forms.TextBox txtJournalIDOut;
        private System.Windows.Forms.ComboBox cboReleaseHold;
        private System.Windows.Forms.MaskedTextBox txtReleaseDate;
        private System.Windows.Forms.MaskedTextBox txtPaidDate;
        private System.Windows.Forms.MaskedTextBox txtPostedDate;
        private System.Windows.Forms.LinkLabel lnkJournalIDOut;
        private System.Windows.Forms.LinkLabel lnkJournalIDIn;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.TextBox txtMerchantName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnMerchant;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel pnlMain;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}