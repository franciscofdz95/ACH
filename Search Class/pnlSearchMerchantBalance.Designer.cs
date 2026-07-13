namespace AchSystem
{
    partial class pnlSearchMerchantBalance
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
            this.grpRequired = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtTransBeginDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.txtTransBeginDate);
            this.grpRequired.Controls.Add(this.label7);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.FontData.BoldAsString = "True";
            this.grpRequired.HeaderAppearance = appearance1;
            this.grpRequired.Location = new System.Drawing.Point(3, 3);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(824, 81);
            this.grpRequired.TabIndex = 12;
            this.grpRequired.Text = "Merchant Balance";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // txtTransBeginDate
            // 
            this.txtTransBeginDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtTransBeginDate.Location = new System.Drawing.Point(126, 35);
            this.txtTransBeginDate.Name = "txtTransBeginDate";
            this.txtTransBeginDate.Size = new System.Drawing.Size(100, 20);
            this.txtTransBeginDate.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(20, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Balance as of Date:";
            // 
            // pnlSearchMerchantBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRequired);
            this.Name = "pnlSearchMerchantBalance";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(830, 87);
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).EndInit();
            this.grpRequired.ResumeLayout(false);
            this.grpRequired.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox grpRequired;
        private System.Windows.Forms.DateTimePicker txtTransBeginDate;
        private System.Windows.Forms.Label label7;
    }
}
