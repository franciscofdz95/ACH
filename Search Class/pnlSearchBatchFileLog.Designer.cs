namespace AchSystem
{
    partial class pnlSearchBatchFileLog
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.txtPostEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtPostBeginDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.grpRequired = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(487, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "File Name:";
            // 
            // txtFileName
            // 
            this.txtFileName.BackColor = System.Drawing.SystemColors.Window;
            this.txtFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.Location = new System.Drawing.Point(550, 47);
            this.txtFileName.MaxLength = 100;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(100, 20);
            this.txtFileName.TabIndex = 40;
            // 
            // txtPostEndDate
            // 
            this.txtPostEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtPostEndDate.Location = new System.Drawing.Point(232, 46);
            this.txtPostEndDate.Name = "txtPostEndDate";
            this.txtPostEndDate.Size = new System.Drawing.Size(100, 20);
            this.txtPostEndDate.TabIndex = 20;
            // 
            // txtPostBeginDate
            // 
            this.txtPostBeginDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostBeginDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtPostBeginDate.Location = new System.Drawing.Point(98, 46);
            this.txtPostBeginDate.Name = "txtPostBeginDate";
            this.txtPostBeginDate.Size = new System.Drawing.Size(100, 20);
            this.txtPostBeginDate.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(204, 50);
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
            this.label7.Location = new System.Drawing.Point(11, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Posted Date:";
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.txtAchID);
            this.grpRequired.Controls.Add(this.label3);
            this.grpRequired.Controls.Add(this.txtFileName);
            this.grpRequired.Controls.Add(this.label7);
            this.grpRequired.Controls.Add(this.label2);
            this.grpRequired.Controls.Add(this.label8);
            this.grpRequired.Controls.Add(this.txtPostBeginDate);
            this.grpRequired.Controls.Add(this.txtPostEndDate);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.FontData.BoldAsString = "True";
            this.grpRequired.HeaderAppearance = appearance1;
            this.grpRequired.Location = new System.Drawing.Point(3, 3);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(824, 112);
            this.grpRequired.TabIndex = 12;
            this.grpRequired.Text = "Search Process Log";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // txtAchID
            // 
            this.txtAchID.BackColor = System.Drawing.SystemColors.Window;
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(392, 47);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(87, 20);
            this.txtAchID.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(340, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "ACH ID:";
            // 
            // pnlSearchBatchFileLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRequired);
            this.Name = "pnlSearchBatchFileLog";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(830, 118);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFileName;
        private Infragistics.Win.Misc.UltraGroupBox grpRequired;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
    }
}
