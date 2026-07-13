namespace AchSystem
{
    partial class frmEFT
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEFT));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.txtJournalIDIn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTransID = new System.Windows.Forms.TextBox();
            this.txtDateProcessed = new System.Windows.Forms.MaskedTextBox();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnMerchant = new System.Windows.Forms.Button();
            this.cboEFTDescription = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.txtMerchantName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.lnkJournalIDIn = new System.Windows.Forms.LinkLabel();
            this.txtPostedDate = new System.Windows.Forms.MaskedTextBox();
            this.txtEncryptAccount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAchAccountName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtAchAccountNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtAchTransRoute = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboTransType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.pnlMain = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.lnkBatchID = new System.Windows.Forms.LinkLabel();
            this.txtBatchID = new System.Windows.Forms.TextBox();
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
            // txtJournalIDIn
            // 
            this.txtJournalIDIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJournalIDIn.Location = new System.Drawing.Point(109, 38);
            this.txtJournalIDIn.MaxLength = 10;
            this.txtJournalIDIn.Name = "txtJournalIDIn";
            this.txtJournalIDIn.Size = new System.Drawing.Size(121, 20);
            this.txtJournalIDIn.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Trans ID:";
            // 
            // txtTransID
            // 
            this.txtTransID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransID.Location = new System.Drawing.Point(111, 88);
            this.txtTransID.MaxLength = 10;
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.ReadOnly = true;
            this.txtTransID.Size = new System.Drawing.Size(100, 20);
            this.txtTransID.TabIndex = 35;
            this.txtTransID.TabStop = false;
            // 
            // txtDateProcessed
            // 
            this.txtDateProcessed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDateProcessed.Location = new System.Drawing.Point(109, 13);
            this.txtDateProcessed.Mask = "00/00/0000 90:00:00";
            this.txtDateProcessed.Name = "txtDateProcessed";
            this.txtDateProcessed.Size = new System.Drawing.Size(121, 20);
            this.txtDateProcessed.TabIndex = 50;
            // 
            // txtAchID
            // 
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(111, 10);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.ReadOnly = true;
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 10;
            this.txtAchID.TabStop = false;
            this.txtAchID.Leave += new System.EventHandler(this.txtAchID_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(11, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Posted Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 80;
            this.label4.Text = "Date Processed:";
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(111, 140);
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
            this.label8.Location = new System.Drawing.Point(11, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 88;
            this.label8.Text = "Amount:";
            // 
            // btnMerchant
            // 
            this.btnMerchant.Enabled = false;
            this.btnMerchant.Image = global::AchSystem.Properties.Resources.search;
            this.btnMerchant.Location = new System.Drawing.Point(217, 10);
            this.btnMerchant.Name = "btnMerchant";
            this.btnMerchant.Size = new System.Drawing.Size(29, 21);
            this.btnMerchant.TabIndex = 15;
            this.btnMerchant.UseVisualStyleBackColor = true;
            this.btnMerchant.Click += new System.EventHandler(this.btnAchID_Click);
            // 
            // cboEFTDescription
            // 
            this.cboEFTDescription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEFTDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboEFTDescription.FormattingEnabled = true;
            this.cboEFTDescription.Location = new System.Drawing.Point(111, 297);
            this.cboEFTDescription.Name = "cboEFTDescription";
            this.cboEFTDescription.Size = new System.Drawing.Size(129, 21);
            this.cboEFTDescription.TabIndex = 140;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(11, 39);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(69, 13);
            this.label22.TabIndex = 180;
            this.label22.Text = "Merchant ID:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(111, 36);
            this.txtMerchantID.MaxLength = 9;
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.ReadOnly = true;
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 20;
            // 
            // txtMerchantName
            // 
            this.txtMerchantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantName.Location = new System.Drawing.Point(111, 62);
            this.txtMerchantName.MaxLength = 16;
            this.txtMerchantName.Name = "txtMerchantName";
            this.txtMerchantName.ReadOnly = true;
            this.txtMerchantName.Size = new System.Drawing.Size(350, 20);
            this.txtMerchantName.TabIndex = 30;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(11, 65);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(86, 13);
            this.label18.TabIndex = 179;
            this.label18.Text = "Merchant Name:";
            // 
            // lnkJournalIDIn
            // 
            this.lnkJournalIDIn.AutoSize = true;
            this.lnkJournalIDIn.BackColor = System.Drawing.Color.Transparent;
            this.lnkJournalIDIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkJournalIDIn.Location = new System.Drawing.Point(9, 41);
            this.lnkJournalIDIn.Name = "lnkJournalIDIn";
            this.lnkJournalIDIn.Size = new System.Drawing.Size(84, 13);
            this.lnkJournalIDIn.TabIndex = 55;
            this.lnkJournalIDIn.TabStop = true;
            this.lnkJournalIDIn.Text = "Journal ID In:";
            this.lnkJournalIDIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkJournalIDIn_LinkClicked);
            // 
            // txtPostedDate
            // 
            this.txtPostedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostedDate.Location = new System.Drawing.Point(111, 114);
            this.txtPostedDate.Mask = "00/00/0000 90:00:00";
            this.txtPostedDate.Name = "txtPostedDate";
            this.txtPostedDate.Size = new System.Drawing.Size(121, 20);
            this.txtPostedDate.TabIndex = 40;
            // 
            // txtEncryptAccount
            // 
            this.txtEncryptAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEncryptAccount.Location = new System.Drawing.Point(109, 90);
            this.txtEncryptAccount.MaxLength = 10;
            this.txtEncryptAccount.Name = "txtEncryptAccount";
            this.txtEncryptAccount.ReadOnly = true;
            this.txtEncryptAccount.Size = new System.Drawing.Size(189, 20);
            this.txtEncryptAccount.TabIndex = 150;
            this.txtEncryptAccount.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 119;
            this.label9.Text = "Encrypt Account:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Location = new System.Drawing.Point(11, 301);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 13);
            this.label14.TabIndex = 117;
            this.label14.Text = "Description:";
            // 
            // txtAchAccountName
            // 
            this.txtAchAccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchAccountName.Location = new System.Drawing.Point(111, 272);
            this.txtAchAccountName.MaxLength = 25;
            this.txtAchAccountName.Name = "txtAchAccountName";
            this.txtAchAccountName.Size = new System.Drawing.Size(334, 20);
            this.txtAchAccountName.TabIndex = 130;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(11, 275);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 115;
            this.label13.Text = "Account Name:";
            // 
            // txtAchAccountNo
            // 
            this.txtAchAccountNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchAccountNo.Location = new System.Drawing.Point(111, 246);
            this.txtAchAccountNo.MaxLength = 17;
            this.txtAchAccountNo.Name = "txtAchAccountNo";
            this.txtAchAccountNo.Size = new System.Drawing.Size(121, 20);
            this.txtAchAccountNo.TabIndex = 120;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(11, 249);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 113;
            this.label12.Text = "Account No:";
            // 
            // txtAchTransRoute
            // 
            this.txtAchTransRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchTransRoute.Location = new System.Drawing.Point(111, 220);
            this.txtAchTransRoute.MaxLength = 9;
            this.txtAchTransRoute.Name = "txtAchTransRoute";
            this.txtAchTransRoute.Size = new System.Drawing.Size(121, 20);
            this.txtAchTransRoute.TabIndex = 110;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(11, 223);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 13);
            this.label11.TabIndex = 111;
            this.label11.Text = "Trans Route:";
            // 
            // cboTransType
            // 
            this.cboTransType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTransType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTransType.FormattingEnabled = true;
            this.cboTransType.Location = new System.Drawing.Point(111, 166);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(121, 21);
            this.cboTransType.TabIndex = 90;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(11, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 109;
            this.label7.Text = "Trans Type:";
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.pnlMain);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(383, 192);
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.ultraExpandableGroupBox1.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 27);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(511, 377);
            this.ultraExpandableGroupBox1.TabIndex = 16;
            this.ultraExpandableGroupBox1.Text = "Required Fields";
            this.ultraExpandableGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.cboSource);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.label6);
            this.pnlMain.Controls.Add(this.cboStatus);
            this.pnlMain.Controls.Add(this.linkLabel1);
            this.pnlMain.Controls.Add(this.cboEFTDescription);
            this.pnlMain.Controls.Add(this.btnMerchant);
            this.pnlMain.Controls.Add(this.txtTransID);
            this.pnlMain.Controls.Add(this.txtPostedDate);
            this.pnlMain.Controls.Add(this.label14);
            this.pnlMain.Controls.Add(this.label22);
            this.pnlMain.Controls.Add(this.txtAchAccountName);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label13);
            this.pnlMain.Controls.Add(this.txtMerchantID);
            this.pnlMain.Controls.Add(this.txtAchAccountNo);
            this.pnlMain.Controls.Add(this.txtAchID);
            this.pnlMain.Controls.Add(this.label12);
            this.pnlMain.Controls.Add(this.txtMerchantName);
            this.pnlMain.Controls.Add(this.txtAchTransRoute);
            this.pnlMain.Controls.Add(this.label18);
            this.pnlMain.Controls.Add(this.label11);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.cboTransType);
            this.pnlMain.Controls.Add(this.txtAmount);
            this.pnlMain.Controls.Add(this.label7);
            this.pnlMain.Controls.Add(this.label8);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(2, 20);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(507, 355);
            this.pnlMain.TabIndex = 0;
            // 
            // cboSource
            // 
            this.cboSource.Enabled = false;
            this.cboSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(111, 324);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(131, 21);
            this.cboSource.TabIndex = 145;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(11, 327);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 191;
            this.label1.Text = "Source:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(11, 196);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 181;
            this.label6.Text = "Status ID:";
            // 
            // cboStatus
            // 
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(111, 193);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(195, 21);
            this.cboStatus.TabIndex = 95;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(11, 13);
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
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(470, 173);
            appearance2.FontData.BoldAsString = "True";
            this.ultraExpandableGroupBox2.HeaderAppearance = appearance2;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 404);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(511, 162);
            this.ultraExpandableGroupBox2.TabIndex = 17;
            this.ultraExpandableGroupBox2.Text = "Optional Fields";
            this.ultraExpandableGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lnkBatchID);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtBatchID);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtEncryptAccount);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label9);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lnkJournalIDIn);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtDateProcessed);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label4);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtJournalIDIn);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(507, 140);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // lnkBatchID
            // 
            this.lnkBatchID.AutoSize = true;
            this.lnkBatchID.BackColor = System.Drawing.Color.Transparent;
            this.lnkBatchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkBatchID.Location = new System.Drawing.Point(9, 67);
            this.lnkBatchID.Name = "lnkBatchID";
            this.lnkBatchID.Size = new System.Drawing.Size(61, 13);
            this.lnkBatchID.TabIndex = 65;
            this.lnkBatchID.TabStop = true;
            this.lnkBatchID.Text = "Batch ID:";
            // 
            // txtBatchID
            // 
            this.txtBatchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatchID.Location = new System.Drawing.Point(109, 64);
            this.txtBatchID.MaxLength = 9;
            this.txtBatchID.Name = "txtBatchID";
            this.txtBatchID.Size = new System.Drawing.Size(121, 20);
            this.txtBatchID.TabIndex = 66;
            // 
            // frmEFT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 566);
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Controls.Add(this.ultraExpandableGroupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEFT";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EFT";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmEFT_FormClosed);
            this.Load += new System.EventHandler(this.frmEFT_Load);
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

        private System.Windows.Forms.TextBox txtJournalIDIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTransID;
        private System.Windows.Forms.MaskedTextBox txtDateProcessed;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboTransType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtAchAccountName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtAchAccountNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtAchTransRoute;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtEncryptAccount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox txtPostedDate;
        private System.Windows.Forms.LinkLabel lnkJournalIDIn;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.TextBox txtMerchantName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cboEFTDescription;
        private System.Windows.Forms.Button btnMerchant;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel pnlMain;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lnkBatchID;
        private System.Windows.Forms.TextBox txtBatchID;
    }
}