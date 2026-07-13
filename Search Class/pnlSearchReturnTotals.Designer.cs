namespace AchSystem
{
    partial class pnlSearchReturnTotals
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
            this.txtPostEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtPostBeginDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.grpRequired = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPostEndDate
            // 
            this.txtPostEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtPostEndDate.Location = new System.Drawing.Point(220, 46);
            this.txtPostEndDate.Name = "txtPostEndDate";
            this.txtPostEndDate.Size = new System.Drawing.Size(100, 20);
            this.txtPostEndDate.TabIndex = 30;
            // 
            // txtPostBeginDate
            // 
            this.txtPostBeginDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtPostBeginDate.Location = new System.Drawing.Point(86, 46);
            this.txtPostBeginDate.Name = "txtPostBeginDate";
            this.txtPostBeginDate.Size = new System.Drawing.Size(100, 20);
            this.txtPostBeginDate.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(192, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "To";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Posted Date:";
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.txtPostBeginDate);
            this.grpRequired.Controls.Add(this.txtPostEndDate);
            this.grpRequired.Controls.Add(this.label8);
            this.grpRequired.Controls.Add(this.label7);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.FontData.BoldAsString = "True";
            this.grpRequired.HeaderAppearance = appearance1;
            this.grpRequired.Location = new System.Drawing.Point(3, 3);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(836, 112);
            this.grpRequired.TabIndex = 12;
            this.grpRequired.Text = "Search Return Totals";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // pnlSearchReturnTotals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRequired);
            this.Name = "pnlSearchReturnTotals";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(842, 118);
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).EndInit();
            this.grpRequired.ResumeLayout(false);
            this.grpRequired.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker txtPostEndDate;
        private System.Windows.Forms.DateTimePicker txtPostBeginDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private Infragistics.Win.Misc.UltraGroupBox grpRequired;
    }
}
