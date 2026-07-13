namespace AchSystem
{
    partial class frmAchRecurring
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAchRecurring));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.txtEncryptAccount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAccountNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTransRoute = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboTransType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTransID = new System.Windows.Forms.TextBox();
            this.txtRefID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMerchantName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnMerchant = new System.Windows.Forms.Button();
            this.txtTransDate = new System.Windows.Forms.MaskedTextBox();
            this.cboOriginID = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.cboAchSecc = new System.Windows.Forms.ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtAchCompanyName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtDescDate = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtAchDescription = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.grpRequired = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.pnlMain = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.grpOptional = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpOptional)).BeginInit();
            this.grpOptional.SuspendLayout();
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
            // txtEncryptAccount
            // 
            this.txtEncryptAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEncryptAccount.Location = new System.Drawing.Point(346, 219);
            this.txtEncryptAccount.MaxLength = 10;
            this.txtEncryptAccount.Name = "txtEncryptAccount";
            this.txtEncryptAccount.ReadOnly = true;
            this.txtEncryptAccount.Size = new System.Drawing.Size(170, 20);
            this.txtEncryptAccount.TabIndex = 280;
            this.txtEncryptAccount.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(251, 222);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 119;
            this.label9.Text = "Encrypt Account:";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountNo.Location = new System.Drawing.Point(323, 166);
            this.txtAccountNo.MaxLength = 17;
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(194, 20);
            this.txtAccountNo.TabIndex = 140;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(239, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 113;
            this.label12.Text = "Account No:";
            // 
            // txtTransRoute
            // 
            this.txtTransRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransRoute.Location = new System.Drawing.Point(114, 166);
            this.txtTransRoute.MaxLength = 9;
            this.txtTransRoute.Name = "txtTransRoute";
            this.txtTransRoute.Size = new System.Drawing.Size(121, 20);
            this.txtTransRoute.TabIndex = 130;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(14, 169);
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
            this.cboTransType.Location = new System.Drawing.Point(115, 139);
            this.cboTransType.Name = "cboTransType";
            this.cboTransType.Size = new System.Drawing.Size(153, 21);
            this.cboTransType.TabIndex = 110;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(15, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 109;
            this.label7.Text = "Trans Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(14, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Trans ID:";
            // 
            // txtTransID
            // 
            this.txtTransID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransID.Location = new System.Drawing.Point(114, 60);
            this.txtTransID.MaxLength = 10;
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.ReadOnly = true;
            this.txtTransID.Size = new System.Drawing.Size(100, 20);
            this.txtTransID.TabIndex = 30;
            this.txtTransID.TabStop = false;
            // 
            // txtRefID
            // 
            this.txtRefID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefID.Location = new System.Drawing.Point(114, 245);
            this.txtRefID.MaxLength = 50;
            this.txtRefID.Name = "txtRefID";
            this.txtRefID.Size = new System.Drawing.Size(402, 20);
            this.txtRefID.TabIndex = 300;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(14, 248);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 165;
            this.label10.Text = "Ref ID:";
            // 
            // txtMerchantName
            // 
            this.txtMerchantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantName.Location = new System.Drawing.Point(114, 34);
            this.txtMerchantName.MaxLength = 60;
            this.txtMerchantName.Name = "txtMerchantName";
            this.txtMerchantName.ReadOnly = true;
            this.txtMerchantName.Size = new System.Drawing.Size(403, 20);
            this.txtMerchantName.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(14, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 340;
            this.label1.Text = "Merchant Name:";
            // 
            // btnMerchant
            // 
            this.btnMerchant.Enabled = false;
            this.btnMerchant.Image = global::AchSystem.Properties.Resources.search;
            this.btnMerchant.Location = new System.Drawing.Point(265, 7);
            this.btnMerchant.Name = "btnMerchant";
            this.btnMerchant.Size = new System.Drawing.Size(29, 21);
            this.btnMerchant.TabIndex = 15;
            this.btnMerchant.UseVisualStyleBackColor = true;
            this.btnMerchant.Click += new System.EventHandler(this.btnMerchant_Click);
            // 
            // txtTransDate
            // 
            this.txtTransDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransDate.Location = new System.Drawing.Point(300, 60);
            this.txtTransDate.Mask = "00/00/0000 90:00:00";
            this.txtTransDate.Name = "txtTransDate";
            this.txtTransDate.Size = new System.Drawing.Size(217, 20);
            this.txtTransDate.TabIndex = 35;
            // 
            // cboOriginID
            // 
            this.cboOriginID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOriginID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboOriginID.FormattingEnabled = true;
            this.cboOriginID.Location = new System.Drawing.Point(114, 86);
            this.cboOriginID.Name = "cboOriginID";
            this.cboOriginID.Size = new System.Drawing.Size(198, 21);
            this.cboOriginID.TabIndex = 70;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(301, 11);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(69, 13);
            this.label22.TabIndex = 172;
            this.label22.Text = "Merchant ID:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(388, 8);
            this.txtMerchantID.MaxLength = 9;
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.ReadOnly = true;
            this.txtMerchantID.Size = new System.Drawing.Size(129, 20);
            this.txtMerchantID.TabIndex = 20;
            this.txtMerchantID.Leave += new System.EventHandler(this.txtMerchantID_Leave);
            // 
            // cboAchSecc
            // 
            this.cboAchSecc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAchSecc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboAchSecc.FormattingEnabled = true;
            this.cboAchSecc.Location = new System.Drawing.Point(323, 140);
            this.cboAchSecc.Name = "cboAchSecc";
            this.cboAchSecc.Size = new System.Drawing.Size(194, 21);
            this.cboAchSecc.TabIndex = 120;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Location = new System.Drawing.Point(277, 143);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(35, 13);
            this.label19.TabIndex = 164;
            this.label19.Text = "Secc:";
            // 
            // txtAchCompanyName
            // 
            this.txtAchCompanyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchCompanyName.Location = new System.Drawing.Point(115, 113);
            this.txtAchCompanyName.MaxLength = 16;
            this.txtAchCompanyName.Name = "txtAchCompanyName";
            this.txtAchCompanyName.Size = new System.Drawing.Size(197, 20);
            this.txtAchCompanyName.TabIndex = 90;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(15, 116);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(71, 13);
            this.label18.TabIndex = 161;
            this.label18.Text = "Comp. Name:";
            // 
            // txtDescDate
            // 
            this.txtDescDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescDate.Location = new System.Drawing.Point(388, 113);
            this.txtDescDate.MaxLength = 6;
            this.txtDescDate.Name = "txtDescDate";
            this.txtDescDate.Size = new System.Drawing.Size(133, 20);
            this.txtDescDate.TabIndex = 100;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point(316, 116);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(61, 13);
            this.label17.TabIndex = 159;
            this.label17.Text = "Desc Date:";
            // 
            // txtAchDescription
            // 
            this.txtAchDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchDescription.Location = new System.Drawing.Point(388, 86);
            this.txtAchDescription.MaxLength = 10;
            this.txtAchDescription.Name = "txtAchDescription";
            this.txtAchDescription.Size = new System.Drawing.Size(132, 20);
            this.txtAchDescription.TabIndex = 80;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point(316, 89);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 13);
            this.label16.TabIndex = 157;
            this.label16.Text = "Description:";
            // 
            // txtAccountName
            // 
            this.txtAccountName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountName.Location = new System.Drawing.Point(115, 192);
            this.txtAccountName.MaxLength = 23;
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(402, 20);
            this.txtAccountName.TabIndex = 150;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Location = new System.Drawing.Point(15, 195);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 13);
            this.label15.TabIndex = 155;
            this.label15.Text = "Account Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(14, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 154;
            this.label5.Text = "Origin:";
            // 
            // txtAchID
            // 
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(114, 8);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(146, 20);
            this.txtAchID.TabIndex = 10;
            this.txtAchID.Leave += new System.EventHandler(this.txtAchID_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(220, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Trans Date:";
            // 
            // txtAmount
            // 
            this.txtAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(114, 219);
            this.txtAmount.MaxLength = 20;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(131, 20);
            this.txtAmount.TabIndex = 170;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(14, 222);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 88;
            this.label8.Text = "Amount:";
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.pnlMain);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRequired.ExpandedSize = new System.Drawing.Size(552, 331);
            this.grpRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpRequired.ForeColor = System.Drawing.Color.Black;
            appearance3.FontData.BoldAsString = "True";
            appearance3.ForeColor = System.Drawing.Color.Red;
            this.grpRequired.HeaderAppearance = appearance3;
            this.grpRequired.Location = new System.Drawing.Point(0, 26);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(551, 295);
            this.grpRequired.TabIndex = 16;
            this.grpRequired.Text = "Required Fields";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.linkLabel1);
            this.pnlMain.Controls.Add(this.txtMerchantName);
            this.pnlMain.Controls.Add(this.label9);
            this.pnlMain.Controls.Add(this.txtEncryptAccount);
            this.pnlMain.Controls.Add(this.label1);
            this.pnlMain.Controls.Add(this.label10);
            this.pnlMain.Controls.Add(this.txtRefID);
            this.pnlMain.Controls.Add(this.label8);
            this.pnlMain.Controls.Add(this.btnMerchant);
            this.pnlMain.Controls.Add(this.txtAmount);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.txtTransID);
            this.pnlMain.Controls.Add(this.txtTransDate);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.label7);
            this.pnlMain.Controls.Add(this.cboTransType);
            this.pnlMain.Controls.Add(this.label11);
            this.pnlMain.Controls.Add(this.txtTransRoute);
            this.pnlMain.Controls.Add(this.cboOriginID);
            this.pnlMain.Controls.Add(this.label12);
            this.pnlMain.Controls.Add(this.txtAccountNo);
            this.pnlMain.Controls.Add(this.txtAchID);
            this.pnlMain.Controls.Add(this.label5);
            this.pnlMain.Controls.Add(this.label15);
            this.pnlMain.Controls.Add(this.txtAccountName);
            this.pnlMain.Controls.Add(this.label16);
            this.pnlMain.Controls.Add(this.txtAchDescription);
            this.pnlMain.Controls.Add(this.label17);
            this.pnlMain.Controls.Add(this.txtDescDate);
            this.pnlMain.Controls.Add(this.label18);
            this.pnlMain.Controls.Add(this.label22);
            this.pnlMain.Controls.Add(this.txtAchCompanyName);
            this.pnlMain.Controls.Add(this.txtMerchantID);
            this.pnlMain.Controls.Add(this.label19);
            this.pnlMain.Controls.Add(this.cboAchSecc);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(2, 20);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(547, 273);
            this.pnlMain.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(16, 11);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(50, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Ach ID:";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // grpOptional
            // 
            this.grpOptional.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.grpOptional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpOptional.ExpandedSize = new System.Drawing.Size(551, 267);
            appearance4.FontData.BoldAsString = "True";
            this.grpOptional.HeaderAppearance = appearance4;
            this.grpOptional.Location = new System.Drawing.Point(0, 321);
            this.grpOptional.Name = "grpOptional";
            this.grpOptional.Size = new System.Drawing.Size(551, 267);
            this.grpOptional.TabIndex = 17;
            this.grpOptional.Text = "Optional Fields";
            this.grpOptional.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(547, 245);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // frmAchRecurring
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 588);
            this.Controls.Add(this.grpOptional);
            this.Controls.Add(this.grpRequired);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAchRecurring";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ACH Recurring";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTransaction_FormClosed);
            this.Load += new System.EventHandler(this.frmTransaction_Load);
            this.Controls.SetChildIndex(this.grpRequired, 0);
            this.Controls.SetChildIndex(this.grpOptional, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).EndInit();
            this.grpRequired.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpOptional)).EndInit();
            this.grpOptional.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtEncryptAccount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAccountNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTransRoute;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboTransType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTransID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAchCompanyName;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDescDate;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtAchDescription;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.ComboBox cboAchSecc;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtRefID;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.ComboBox cboOriginID;
        private System.Windows.Forms.MaskedTextBox txtTransDate;
        private System.Windows.Forms.Button btnMerchant;
        private System.Windows.Forms.TextBox txtMerchantName;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpRequired;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel pnlMain;
        private Infragistics.Win.Misc.UltraExpandableGroupBox grpOptional;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}