namespace AchSystem
{
    partial class pnlSearchSource
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSourceID = new System.Windows.Forms.TextBox();
            this.grpRequired = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(199, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Source:";
            // 
            // txtSource
            // 
            this.txtSource.BackColor = System.Drawing.SystemColors.Window;
            this.txtSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSource.Location = new System.Drawing.Point(256, 46);
            this.txtSource.MaxLength = 2;
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(76, 20);
            this.txtSource.TabIndex = 32;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.Window;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(414, 46);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(263, 20);
            this.txtDescription.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(339, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Description:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Source ID:";
            // 
            // txtSourceID
            // 
            this.txtSourceID.BackColor = System.Drawing.SystemColors.Window;
            this.txtSourceID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceID.Location = new System.Drawing.Point(92, 46);
            this.txtSourceID.MaxLength = 9;
            this.txtSourceID.Name = "txtSourceID";
            this.txtSourceID.Size = new System.Drawing.Size(76, 20);
            this.txtSourceID.TabIndex = 20;
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.label1);
            this.grpRequired.Controls.Add(this.txtDescription);
            this.grpRequired.Controls.Add(this.txtSource);
            this.grpRequired.Controls.Add(this.txtSourceID);
            this.grpRequired.Controls.Add(this.label4);
            this.grpRequired.Controls.Add(this.label3);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.FontData.BoldAsString = "True";
            this.grpRequired.HeaderAppearance = appearance1;
            this.grpRequired.Location = new System.Drawing.Point(3, 3);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(824, 112);
            this.grpRequired.TabIndex = 14;
            this.grpRequired.Text = "Search Source";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // pnlSearchSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpRequired);
            this.Name = "pnlSearchSource";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(830, 118);
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).EndInit();
            this.grpRequired.ResumeLayout(false);
            this.grpRequired.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSourceID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSource;
        private Infragistics.Win.Misc.UltraGroupBox grpRequired;
    }
}
