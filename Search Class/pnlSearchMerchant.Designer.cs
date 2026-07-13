namespace AchSystem
{
    partial class pnlSearchMerchant
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.cboBankID = new System.Windows.Forms.ComboBox();
            this.cboMerchantType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMerchantName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.grpRequired = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboBankID
            // 
            this.cboBankID.FormattingEnabled = true;
            this.cboBankID.Location = new System.Drawing.Point(254, 59);
            this.cboBankID.Name = "cboBankID";
            this.cboBankID.Size = new System.Drawing.Size(121, 21);
            this.cboBankID.TabIndex = 50;
            // 
            // cboMerchantType
            // 
            this.cboMerchantType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMerchantType.FormattingEnabled = true;
            this.cboMerchantType.Location = new System.Drawing.Point(469, 58);
            this.cboMerchantType.Name = "cboMerchantType";
            this.cboMerchantType.Size = new System.Drawing.Size(121, 21);
            this.cboMerchantType.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(381, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 125;
            this.label2.Text = "Merchant Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(199, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 123;
            this.label1.Text = "Bank ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 122;
            this.label7.Text = "Merchant Name:";
            // 
            // txtMerchantName
            // 
            this.txtMerchantName.BackColor = System.Drawing.SystemColors.Window;
            this.txtMerchantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantName.Location = new System.Drawing.Point(93, 59);
            this.txtMerchantName.MaxLength = 40;
            this.txtMerchantName.Name = "txtMerchantName";
            this.txtMerchantName.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantName.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Merchant ID:";
            // 
            // txtAchID
            // 
            this.txtAchID.BackColor = System.Drawing.SystemColors.Window;
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(254, 33);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(121, 20);
            this.txtAchID.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(199, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "ACH ID:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.BackColor = System.Drawing.SystemColors.Window;
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(93, 33);
            this.txtMerchantID.MaxLength = 9;
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 20;
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.cboBankID);
            this.grpRequired.Controls.Add(this.label7);
            this.grpRequired.Controls.Add(this.cboMerchantType);
            this.grpRequired.Controls.Add(this.txtMerchantID);
            this.grpRequired.Controls.Add(this.label2);
            this.grpRequired.Controls.Add(this.label3);
            this.grpRequired.Controls.Add(this.label1);
            this.grpRequired.Controls.Add(this.txtAchID);
            this.grpRequired.Controls.Add(this.label4);
            this.grpRequired.Controls.Add(this.txtMerchantName);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.FontData.BoldAsString = "True";
            this.grpRequired.HeaderAppearance = appearance1;
            this.grpRequired.Location = new System.Drawing.Point(3, 3);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(824, 112);
            this.grpRequired.SupportThemes = false;
            this.grpRequired.TabIndex = 12;
            this.grpRequired.Text = "Search Merchant";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // pnlSearchMerchant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRequired);
            this.Name = "pnlSearchMerchant";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(830, 118);
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).EndInit();
            this.grpRequired.ResumeLayout(false);
            this.grpRequired.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMerchantName;
        private System.Windows.Forms.ComboBox cboMerchantType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBankID;
        private Infragistics.Win.Misc.UltraGroupBox grpRequired;
    }
}
