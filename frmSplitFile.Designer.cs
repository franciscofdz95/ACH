namespace AchSystem
{
    partial class frmSplitFile
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ofdSelect = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.txtToday = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPriorDay = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.grpRequired = new Infragistics.Win.Misc.UltraGroupBox();
            this.grpWeekendTotals = new System.Windows.Forms.GroupBox();
            this.txtSundayFileTotal = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtSundayFileTotalCredit = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtSundayFileTotalDebit = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.grpWeekday = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.grpWeekend = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSundayOutput = new System.Windows.Forms.TextBox();
            this.txtSunday = new System.Windows.Forms.TextBox();
            this.lblSunday = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPriorTotalNet = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtNDFNet = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNDFCredit = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNDFDebit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSplitFileTotalCredit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSplitFileTotalDebit = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPriorTotalCredit = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPriorDayTotalDebit = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstMerchants = new System.Windows.Forms.ListBox();
            this.btnEmail = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtTodayFileTotal = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTodayFileTotalCredit = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTodayFileTotalDebit = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.label19 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).BeginInit();
            this.grpRequired.SuspendLayout();
            this.grpWeekendTotals.SuspendLayout();
            this.grpWeekday.SuspendLayout();
            this.grpWeekend.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdSelect
            // 
            this.ofdSelect.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Today\'s File:";
            // 
            // txtToday
            // 
            this.txtToday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToday.Location = new System.Drawing.Point(9, 94);
            this.txtToday.Name = "txtToday";
            this.txtToday.ReadOnly = true;
            this.txtToday.Size = new System.Drawing.Size(437, 20);
            this.txtToday.TabIndex = 9;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(972, 552);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "Browse File";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            // 
            // btnSplit
            // 
            this.btnSplit.Location = new System.Drawing.Point(386, 55);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(112, 23);
            this.btnSplit.TabIndex = 11;
            this.btnSplit.Text = "Split File";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Prior Day\'s File:";
            // 
            // txtPriorDay
            // 
            this.txtPriorDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriorDay.Location = new System.Drawing.Point(9, 45);
            this.txtPriorDay.Name = "txtPriorDay";
            this.txtPriorDay.ReadOnly = true;
            this.txtPriorDay.Size = new System.Drawing.Size(437, 20);
            this.txtPriorDay.TabIndex = 13;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(241, 693);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Browse File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // grpRequired
            // 
            this.grpRequired.Controls.Add(this.label19);
            this.grpRequired.Controls.Add(this.lstFiles);
            this.grpRequired.Controls.Add(this.grpWeekendTotals);
            this.grpRequired.Controls.Add(this.grpWeekday);
            this.grpRequired.Controls.Add(this.grpWeekend);
            this.grpRequired.Controls.Add(this.textBox1);
            this.grpRequired.Controls.Add(this.groupBox3);
            this.grpRequired.Controls.Add(this.groupBox2);
            this.grpRequired.Controls.Add(this.btnEmail);
            this.grpRequired.Controls.Add(this.groupBox1);
            this.grpRequired.Controls.Add(this.button2);
            this.grpRequired.Controls.Add(this.btnSplit);
            this.grpRequired.Controls.Add(this.btnBrowse);
            this.grpRequired.Dock = System.Windows.Forms.DockStyle.Fill;
            appearance1.FontData.BoldAsString = "True";
            this.grpRequired.HeaderAppearance = appearance1;
            this.grpRequired.Location = new System.Drawing.Point(0, 0);
            this.grpRequired.Name = "grpRequired";
            this.grpRequired.Size = new System.Drawing.Size(985, 652);
            this.grpRequired.TabIndex = 13;
            this.grpRequired.Text = "NCAL Split Funding Process";
            this.grpRequired.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // grpWeekendTotals
            // 
            this.grpWeekendTotals.Controls.Add(this.txtSundayFileTotal);
            this.grpWeekendTotals.Controls.Add(this.label16);
            this.grpWeekendTotals.Controls.Add(this.txtSundayFileTotalCredit);
            this.grpWeekendTotals.Controls.Add(this.label17);
            this.grpWeekendTotals.Controls.Add(this.txtSundayFileTotalDebit);
            this.grpWeekendTotals.Controls.Add(this.label18);
            this.grpWeekendTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpWeekendTotals.Location = new System.Drawing.Point(322, 424);
            this.grpWeekendTotals.Name = "grpWeekendTotals";
            this.grpWeekendTotals.Size = new System.Drawing.Size(293, 131);
            this.grpWeekendTotals.TabIndex = 34;
            this.grpWeekendTotals.TabStop = false;
            this.grpWeekendTotals.Text = "Sunday\'s File";
            this.grpWeekendTotals.Visible = false;
            // 
            // txtSundayFileTotal
            // 
            this.txtSundayFileTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSundayFileTotal.Location = new System.Drawing.Point(143, 84);
            this.txtSundayFileTotal.Name = "txtSundayFileTotal";
            this.txtSundayFileTotal.ReadOnly = true;
            this.txtSundayFileTotal.Size = new System.Drawing.Size(110, 20);
            this.txtSundayFileTotal.TabIndex = 36;
            this.txtSundayFileTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(8, 87);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(66, 13);
            this.label16.TabIndex = 35;
            this.label16.Text = "Net Amount:";
            // 
            // txtSundayFileTotalCredit
            // 
            this.txtSundayFileTotalCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSundayFileTotalCredit.Location = new System.Drawing.Point(143, 58);
            this.txtSundayFileTotalCredit.Name = "txtSundayFileTotalCredit";
            this.txtSundayFileTotalCredit.ReadOnly = true;
            this.txtSundayFileTotalCredit.Size = new System.Drawing.Size(110, 20);
            this.txtSundayFileTotalCredit.TabIndex = 34;
            this.txtSundayFileTotalCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(8, 61);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(116, 13);
            this.label17.TabIndex = 33;
            this.label17.Text = "Today Total File Credit:";
            // 
            // txtSundayFileTotalDebit
            // 
            this.txtSundayFileTotalDebit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSundayFileTotalDebit.Location = new System.Drawing.Point(143, 32);
            this.txtSundayFileTotalDebit.Name = "txtSundayFileTotalDebit";
            this.txtSundayFileTotalDebit.ReadOnly = true;
            this.txtSundayFileTotalDebit.Size = new System.Drawing.Size(110, 20);
            this.txtSundayFileTotalDebit.TabIndex = 32;
            this.txtSundayFileTotalDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(8, 35);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(114, 13);
            this.label18.TabIndex = 31;
            this.label18.Text = "Today Total File Debit:";
            // 
            // grpWeekday
            // 
            this.grpWeekday.Controls.Add(this.label2);
            this.grpWeekday.Controls.Add(this.label1);
            this.grpWeekday.Controls.Add(this.txtToday);
            this.grpWeekday.Controls.Add(this.txtPriorDay);
            this.grpWeekday.Controls.Add(this.label3);
            this.grpWeekday.Controls.Add(this.txtOutput);
            this.grpWeekday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpWeekday.Location = new System.Drawing.Point(340, 541);
            this.grpWeekday.Name = "grpWeekday";
            this.grpWeekday.Size = new System.Drawing.Size(461, 175);
            this.grpWeekday.TabIndex = 33;
            this.grpWeekday.TabStop = false;
            this.grpWeekday.Text = "Weekday File";
            this.grpWeekday.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Output File:";
            // 
            // txtOutput
            // 
            this.txtOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(9, 144);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(437, 20);
            this.txtOutput.TabIndex = 16;
            // 
            // grpWeekend
            // 
            this.grpWeekend.Controls.Add(this.label15);
            this.grpWeekend.Controls.Add(this.txtSundayOutput);
            this.grpWeekend.Controls.Add(this.txtSunday);
            this.grpWeekend.Controls.Add(this.lblSunday);
            this.grpWeekend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpWeekend.Location = new System.Drawing.Point(342, 362);
            this.grpWeekend.Name = "grpWeekend";
            this.grpWeekend.Size = new System.Drawing.Size(459, 175);
            this.grpWeekend.TabIndex = 32;
            this.grpWeekend.TabStop = false;
            this.grpWeekend.Text = "Weekend File";
            this.grpWeekend.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(11, 78);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(61, 13);
            this.label15.TabIndex = 32;
            this.label15.Text = "Output File:";
            // 
            // txtSundayOutput
            // 
            this.txtSundayOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSundayOutput.Location = new System.Drawing.Point(11, 94);
            this.txtSundayOutput.Name = "txtSundayOutput";
            this.txtSundayOutput.ReadOnly = true;
            this.txtSundayOutput.Size = new System.Drawing.Size(437, 20);
            this.txtSundayOutput.TabIndex = 33;
            // 
            // txtSunday
            // 
            this.txtSunday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSunday.Location = new System.Drawing.Point(11, 45);
            this.txtSunday.Name = "txtSunday";
            this.txtSunday.ReadOnly = true;
            this.txtSunday.Size = new System.Drawing.Size(437, 20);
            this.txtSunday.TabIndex = 31;
            // 
            // lblSunday
            // 
            this.lblSunday.AutoSize = true;
            this.lblSunday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSunday.Location = new System.Drawing.Point(13, 29);
            this.lblSunday.Name = "lblSunday";
            this.lblSunday.Size = new System.Drawing.Size(94, 13);
            this.lblSunday.TabIndex = 30;
            this.lblSunday.Text = "Sunday Day\'s File:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(342, 709);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 29;
            this.textBox1.Text = "11/15/2011";
            this.textBox1.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtPriorTotalNet);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.txtNDFNet);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtNDFCredit);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtNDFDebit);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtSplitFileTotalCredit);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtSplitFileTotalDebit);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtPriorTotalCredit);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.txtPriorDayTotalDebit);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(23, 362);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(293, 296);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Split File";
            this.groupBox3.Visible = false;
            // 
            // txtPriorTotalNet
            // 
            this.txtPriorTotalNet.Location = new System.Drawing.Point(143, 193);
            this.txtPriorTotalNet.Name = "txtPriorTotalNet";
            this.txtPriorTotalNet.ReadOnly = true;
            this.txtPriorTotalNet.Size = new System.Drawing.Size(110, 20);
            this.txtPriorTotalNet.TabIndex = 46;
            this.txtPriorTotalNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(8, 196);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 45;
            this.label14.Text = "Net Amount:";
            // 
            // txtNDFNet
            // 
            this.txtNDFNet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNDFNet.Location = new System.Drawing.Point(143, 85);
            this.txtNDFNet.Name = "txtNDFNet";
            this.txtNDFNet.ReadOnly = true;
            this.txtNDFNet.Size = new System.Drawing.Size(110, 20);
            this.txtNDFNet.TabIndex = 44;
            this.txtNDFNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Net Amount:";
            // 
            // txtNDFCredit
            // 
            this.txtNDFCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNDFCredit.Location = new System.Drawing.Point(143, 59);
            this.txtNDFCredit.Name = "txtNDFCredit";
            this.txtNDFCredit.ReadOnly = true;
            this.txtNDFCredit.Size = new System.Drawing.Size(110, 20);
            this.txtNDFCredit.TabIndex = 42;
            this.txtNDFCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 41;
            this.label6.Text = "Total Next Day Credit:";
            // 
            // txtNDFDebit
            // 
            this.txtNDFDebit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNDFDebit.Location = new System.Drawing.Point(143, 33);
            this.txtNDFDebit.Name = "txtNDFDebit";
            this.txtNDFDebit.ReadOnly = true;
            this.txtNDFDebit.Size = new System.Drawing.Size(110, 20);
            this.txtNDFDebit.TabIndex = 40;
            this.txtNDFDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Total Next Day Debit:";
            // 
            // txtSplitFileTotalCredit
            // 
            this.txtSplitFileTotalCredit.Location = new System.Drawing.Point(143, 270);
            this.txtSplitFileTotalCredit.Name = "txtSplitFileTotalCredit";
            this.txtSplitFileTotalCredit.ReadOnly = true;
            this.txtSplitFileTotalCredit.Size = new System.Drawing.Size(110, 20);
            this.txtSplitFileTotalCredit.TabIndex = 38;
            this.txtSplitFileTotalCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 273);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "File Total Credit:";
            // 
            // txtSplitFileTotalDebit
            // 
            this.txtSplitFileTotalDebit.Location = new System.Drawing.Point(143, 244);
            this.txtSplitFileTotalDebit.Name = "txtSplitFileTotalDebit";
            this.txtSplitFileTotalDebit.ReadOnly = true;
            this.txtSplitFileTotalDebit.Size = new System.Drawing.Size(110, 20);
            this.txtSplitFileTotalDebit.TabIndex = 36;
            this.txtSplitFileTotalDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(8, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "File Total Debit:";
            // 
            // txtPriorTotalCredit
            // 
            this.txtPriorTotalCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriorTotalCredit.Location = new System.Drawing.Point(143, 167);
            this.txtPriorTotalCredit.Name = "txtPriorTotalCredit";
            this.txtPriorTotalCredit.ReadOnly = true;
            this.txtPriorTotalCredit.Size = new System.Drawing.Size(110, 20);
            this.txtPriorTotalCredit.TabIndex = 34;
            this.txtPriorTotalCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 170);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(110, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Total Prior Day Credit:";
            // 
            // txtPriorDayTotalDebit
            // 
            this.txtPriorDayTotalDebit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPriorDayTotalDebit.Location = new System.Drawing.Point(143, 141);
            this.txtPriorDayTotalDebit.Name = "txtPriorDayTotalDebit";
            this.txtPriorDayTotalDebit.ReadOnly = true;
            this.txtPriorDayTotalDebit.Size = new System.Drawing.Size(110, 20);
            this.txtPriorDayTotalDebit.TabIndex = 32;
            this.txtPriorDayTotalDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 144);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "Total Prior Day Debit:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstMerchants);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(490, 362);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(459, 296);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Next Day Funding Merchants";
            this.groupBox2.Visible = false;
            // 
            // lstMerchants
            // 
            this.lstMerchants.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMerchants.FormattingEnabled = true;
            this.lstMerchants.Location = new System.Drawing.Point(6, 19);
            this.lstMerchants.Name = "lstMerchants";
            this.lstMerchants.Size = new System.Drawing.Size(442, 264);
            this.lstMerchants.TabIndex = 18;
            // 
            // btnEmail
            // 
            this.btnEmail.Location = new System.Drawing.Point(386, 84);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(112, 23);
            this.btnEmail.TabIndex = 26;
            this.btnEmail.Text = "Email NCAL";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Visible = false;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTodayFileTotal);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtTodayFileTotalCredit);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtTodayFileTotalDebit);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 310);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 131);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Today\'s File";
            this.groupBox1.Visible = false;
            // 
            // txtTodayFileTotal
            // 
            this.txtTodayFileTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTodayFileTotal.Location = new System.Drawing.Point(143, 84);
            this.txtTodayFileTotal.Name = "txtTodayFileTotal";
            this.txtTodayFileTotal.ReadOnly = true;
            this.txtTodayFileTotal.Size = new System.Drawing.Size(110, 20);
            this.txtTodayFileTotal.TabIndex = 36;
            this.txtTodayFileTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(8, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "Net Amount:";
            // 
            // txtTodayFileTotalCredit
            // 
            this.txtTodayFileTotalCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTodayFileTotalCredit.Location = new System.Drawing.Point(143, 58);
            this.txtTodayFileTotalCredit.Name = "txtTodayFileTotalCredit";
            this.txtTodayFileTotalCredit.ReadOnly = true;
            this.txtTodayFileTotalCredit.Size = new System.Drawing.Size(110, 20);
            this.txtTodayFileTotalCredit.TabIndex = 34;
            this.txtTodayFileTotalCredit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(8, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(116, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "Today Total File Credit:";
            // 
            // txtTodayFileTotalDebit
            // 
            this.txtTodayFileTotalDebit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTodayFileTotalDebit.Location = new System.Drawing.Point(143, 32);
            this.txtTodayFileTotalDebit.Name = "txtTodayFileTotalDebit";
            this.txtTodayFileTotalDebit.ReadOnly = true;
            this.txtTodayFileTotalDebit.Size = new System.Drawing.Size(110, 20);
            this.txtTodayFileTotalDebit.TabIndex = 32;
            this.txtTodayFileTotalDebit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(8, 35);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(114, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "Today Total File Debit:";
            // 
            // lstFiles
            // 
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(5, 54);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(375, 173);
            this.lstFiles.TabIndex = 135;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(5, 34);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(290, 17);
            this.label19.TabIndex = 136;
            this.label19.Text = "NCAL Credit Card Files:";
            // 
            // frmSplitFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 652);
            this.Controls.Add(this.grpRequired);
            this.Name = "frmSplitFile";
            this.Text = "frmSplitFile";
            this.Load += new System.EventHandler(this.frmSplitFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpRequired)).EndInit();
            this.grpRequired.ResumeLayout(false);
            this.grpRequired.PerformLayout();
            this.grpWeekendTotals.ResumeLayout(false);
            this.grpWeekendTotals.PerformLayout();
            this.grpWeekday.ResumeLayout(false);
            this.grpWeekday.PerformLayout();
            this.grpWeekend.ResumeLayout(false);
            this.grpWeekend.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtToday;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPriorDay;
        private System.Windows.Forms.Button button2;
        private Infragistics.Win.Misc.UltraGroupBox grpRequired;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstMerchants;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtPriorTotalCredit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPriorDayTotalDebit;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSplitFileTotalCredit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSplitFileTotalDebit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTodayFileTotal;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTodayFileTotalCredit;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTodayFileTotalDebit;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtNDFNet;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNDFCredit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNDFDebit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPriorTotalNet;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtSunday;
        private System.Windows.Forms.Label lblSunday;
        private System.Windows.Forms.GroupBox grpWeekday;
        private System.Windows.Forms.GroupBox grpWeekend;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSundayOutput;
        private System.Windows.Forms.GroupBox grpWeekendTotals;
        private System.Windows.Forms.TextBox txtSundayFileTotal;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtSundayFileTotalCredit;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtSundayFileTotalDebit;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Label label19;
    }
}