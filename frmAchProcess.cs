using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace ACH2007
{
	/// <summary>
	/// Summary description for frmAchProcess.
	/// </summary>
	public class frmProcessingEngine : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button13;
		private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Button button12;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Button button9;
		private System.Windows.Forms.Button btnAddAll;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnRemoveAll;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListBox lstSelected;
        private System.Windows.Forms.ListBox lstAvailable;
		private System.Windows.Forms.Button btnPrintBatch;
		private System.Windows.Forms.Button btnPreviewBatch;
		private System.Windows.Forms.Button btnCreateJournal;
		private System.Windows.Forms.Button btnCreateSettlementFile;
		private System.Windows.Forms.Button btnCreateBatch;
        private ProgressBar pbrBatch;
        private Label lblMessage;
		private System.ComponentModel.IContainer components;

		public frmProcessingEngine()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.pbrBatch = new System.Windows.Forms.ProgressBar();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.btnPrintBatch = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnPreviewBatch = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstSelected = new System.Windows.Forms.ListBox();
            this.button9 = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstAvailable = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCreateJournal = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateSettlementFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.btnCreateBatch = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Controls.Add(this.pbrBatch);
            this.panel1.Controls.Add(this.button13);
            this.panel1.Controls.Add(this.button14);
            this.panel1.Controls.Add(this.button12);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.groupBox10);
            this.panel1.Controls.Add(this.btnPrintBatch);
            this.panel1.Controls.Add(this.groupBox9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnPreviewBatch);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.button9);
            this.panel1.Controls.Add(this.btnAddAll);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnRemoveAll);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.btnCreateJournal);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnCreateSettlementFile);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.btnCreateBatch);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1284, 520);
            this.panel1.TabIndex = 0;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(296, 282);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(32, 13);
            this.lblMessage.TabIndex = 63;
            this.lblMessage.Text = "xxxxx";
            // 
            // pbrBatch
            // 
            this.pbrBatch.Location = new System.Drawing.Point(166, 253);
            this.pbrBatch.Name = "pbrBatch";
            this.pbrBatch.Size = new System.Drawing.Size(422, 19);
            this.pbrBatch.TabIndex = 62;
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.SystemColors.Control;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button13.Location = new System.Drawing.Point(244, 490);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 61;
            this.button13.Text = "Print";
            this.button13.UseVisualStyleBackColor = false;
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.SystemColors.Control;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button14.Location = new System.Drawing.Point(164, 490);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 60;
            this.button14.Text = "Preview";
            this.button14.UseVisualStyleBackColor = false;
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.SystemColors.Control;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button12.Location = new System.Drawing.Point(16, 490);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(144, 23);
            this.button12.TabIndex = 59;
            this.button12.Text = "Verify Outbound";
            this.button12.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label7.Location = new System.Drawing.Point(8, 459);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(580, 16);
            this.label7.TabIndex = 58;
            this.label7.Text = "Step 7 - Create Verify Outbound and Print File Verification Report";
            // 
            // groupBox10
            // 
            this.groupBox10.Location = new System.Drawing.Point(8, 474);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(580, 8);
            this.groupBox10.TabIndex = 57;
            this.groupBox10.TabStop = false;
            // 
            // btnPrintBatch
            // 
            this.btnPrintBatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrintBatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrintBatch.Location = new System.Drawing.Point(96, 310);
            this.btnPrintBatch.Name = "btnPrintBatch";
            this.btnPrintBatch.Size = new System.Drawing.Size(75, 23);
            this.btnPrintBatch.TabIndex = 56;
            this.btnPrintBatch.Text = "Print";
            this.btnPrintBatch.UseVisualStyleBackColor = false;
            // 
            // groupBox9
            // 
            this.groupBox9.Location = new System.Drawing.Point(8, 294);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(576, 8);
            this.groupBox9.TabIndex = 53;
            this.groupBox9.TabStop = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label6.Location = new System.Drawing.Point(8, 279);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(576, 16);
            this.label6.TabIndex = 54;
            this.label6.Text = "Step 4 - Print Batch Review";
            // 
            // btnPreviewBatch
            // 
            this.btnPreviewBatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnPreviewBatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPreviewBatch.Location = new System.Drawing.Point(16, 310);
            this.btnPreviewBatch.Name = "btnPreviewBatch";
            this.btnPreviewBatch.Size = new System.Drawing.Size(75, 23);
            this.btnPreviewBatch.TabIndex = 55;
            this.btnPreviewBatch.Text = "Preview";
            this.btnPreviewBatch.UseVisualStyleBackColor = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstSelected);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox3.Location = new System.Drawing.Point(352, 34);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 120);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Selected Bank";
            // 
            // lstSelected
            // 
            this.lstSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSelected.FormattingEnabled = true;
            this.lstSelected.Location = new System.Drawing.Point(3, 16);
            this.lstSelected.Name = "lstSelected";
            this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSelected.Size = new System.Drawing.Size(226, 95);
            this.lstSelected.TabIndex = 0;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.Control;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button9.Location = new System.Drawing.Point(96, 190);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 52;
            this.button9.Text = "Print";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // btnAddAll
            // 
            this.btnAddAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAddAll.Location = new System.Drawing.Point(256, 70);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(75, 23);
            this.btnAddAll.TabIndex = 35;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = false;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(256, 42);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 36;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRemove.Location = new System.Drawing.Point(256, 98);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 38;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRemoveAll.Location = new System.Drawing.Point(256, 126);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveAll.TabIndex = 37;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = false;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(8, 174);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(576, 8);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(8, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(576, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "Step 3 - Create Batch";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstAvailable);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(8, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 120);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available Bank";
            // 
            // lstAvailable
            // 
            this.lstAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAvailable.FormattingEnabled = true;
            this.lstAvailable.Location = new System.Drawing.Point(3, 16);
            this.lstAvailable.Name = "lstAvailable";
            this.lstAvailable.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstAvailable.Size = new System.Drawing.Size(226, 95);
            this.lstAvailable.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(8, 234);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(576, 8);
            this.groupBox4.TabIndex = 41;
            this.groupBox4.TabStop = false;
            // 
            // btnCreateJournal
            // 
            this.btnCreateJournal.BackColor = System.Drawing.SystemColors.Control;
            this.btnCreateJournal.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreateJournal.Location = new System.Drawing.Point(16, 370);
            this.btnCreateJournal.Name = "btnCreateJournal";
            this.btnCreateJournal.Size = new System.Drawing.Size(144, 23);
            this.btnCreateJournal.TabIndex = 46;
            this.btnCreateJournal.Text = "Create Journal";
            this.btnCreateJournal.UseVisualStyleBackColor = false;
            this.btnCreateJournal.Click += new System.EventHandler(this.btnCreateJournal_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(8, 339);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(576, 16);
            this.label3.TabIndex = 45;
            this.label3.Text = "Step 5 - Create Journal";
            // 
            // btnCreateSettlementFile
            // 
            this.btnCreateSettlementFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnCreateSettlementFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreateSettlementFile.Location = new System.Drawing.Point(16, 430);
            this.btnCreateSettlementFile.Name = "btnCreateSettlementFile";
            this.btnCreateSettlementFile.Size = new System.Drawing.Size(144, 23);
            this.btnCreateSettlementFile.TabIndex = 49;
            this.btnCreateSettlementFile.Text = "Create Settlement File";
            this.btnCreateSettlementFile.UseVisualStyleBackColor = false;
            this.btnCreateSettlementFile.Click += new System.EventHandler(this.btnCreateSettlementFile_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(8, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(580, 16);
            this.label4.TabIndex = 48;
            this.label4.Text = "Step 6 - Create Settlement File";
            // 
            // groupBox6
            // 
            this.groupBox6.Location = new System.Drawing.Point(8, 414);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(580, 8);
            this.groupBox6.TabIndex = 47;
            this.groupBox6.TabStop = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label5.Location = new System.Drawing.Point(8, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(576, 16);
            this.label5.TabIndex = 51;
            this.label5.Text = "Step 1 - Select Bank";
            // 
            // groupBox7
            // 
            this.groupBox7.Location = new System.Drawing.Point(8, 22);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(576, 10);
            this.groupBox7.TabIndex = 50;
            this.groupBox7.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label1.Location = new System.Drawing.Point(8, 159);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(576, 16);
            this.label1.TabIndex = 39;
            this.label1.Text = "Step 2 - Print Incoming Trans Preview";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Control;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Location = new System.Drawing.Point(16, 190);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 40;
            this.button5.Text = "Preview";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // btnCreateBatch
            // 
            this.btnCreateBatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnCreateBatch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreateBatch.Location = new System.Drawing.Point(16, 250);
            this.btnCreateBatch.Name = "btnCreateBatch";
            this.btnCreateBatch.Size = new System.Drawing.Size(144, 23);
            this.btnCreateBatch.TabIndex = 43;
            this.btnCreateBatch.Text = "Create Batch";
            this.btnCreateBatch.UseVisualStyleBackColor = false;
            this.btnCreateBatch.Click += new System.EventHandler(this.btnCreateBatch_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Location = new System.Drawing.Point(8, 354);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(576, 8);
            this.groupBox5.TabIndex = 44;
            this.groupBox5.TabStop = false;
            // 
            // frmProcessingEngine
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1284, 678);
            this.Controls.Add(this.panel1);
            this.Name = "frmProcessingEngine";
            this.Text = "Ach Processing";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmAchProcess_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmAchProcess_Load(object sender, System.EventArgs e)
		{
            //Bank bnk = new FifthThirdBank("Fifth Third", 12);
            //bnk.WriteResponseFiles();

			this.LoadBank();
			this.LoadLog();
		}
		private void LoadBank()
		{
			Bank bank = null;

            string strSQL = string.Empty;
            strSQL = "Select BankID,Co_name, Orig, Dest, Id_Symbol, Dest_Name, Ref_Code, Orig_name, Necha_Trn, Necha_acct ";
            strSQL += "From AchCoinf With (NoLock) ";
            strSQL += "Where BankID In (6,7,10,11,12,14)";

            SqlDataReader dr = DataLayer.GetDataReader(strSQL,DataLayer.ConnectStringBuild());

            while (dr.Read())
            { 
                switch (Convert.ToInt32(dr["BankID"]))
                {
                    case 6:
                        bank = new BankPayMyBillBank("Pay My Bill", 6);
                        break;
                    case 7:
                        bank = new PayMyBillBank("QuanComm", 7);
                        break;
                    case 10:
                        bank = new NctBank("NCT - VA", 10);
                        break;
                    case 11:
                        bank = new NcalBank("NCAL", 11);
                        break;
                    case 12:
                        bank = new FifthThirdBank("Fifth Third", 12);
                        break;
                    case 14:
                        bank = new NctBank("NCT - GA", 14);
                        break;
                }
                bank.CompanyName = Convert.ToString (dr["Co_name"]);
                bank.ImmediateDestination = Convert.ToString(dr["Dest"]);
                bank.ImmediateOrigin = Convert.ToString(dr["Orig"]);
                bank.Symbol = Convert.ToString(dr["Id_Symbol"]);
                bank.DestinationName = Convert.ToString(dr["Dest_Name"]);
                bank.OriginName = Convert.ToString(dr["Orig_name"]);
                bank.OriginatingTransRoute = Convert.ToString(dr["Necha_Trn"]);
                bank.OriginatingAccountNo = Convert.ToString(dr["Necha_acct"]);

                lstAvailable.Items.Add(bank);
            }

            dr.Close();
        }
		
		private void LoadLog()
		{
			DataTable tbl = new DataTable();
			DataColumn col;
			
			col = new DataColumn("Process Name");
			tbl.Columns.Add(col);
			col = new DataColumn("Status");
			tbl.Columns.Add(col);

			DataRow row;
			row = tbl.NewRow();
			row["Process Name"] = "Print Incoming Trans Preview for Fifth Third.";
			row["Status"] = "Success";
			tbl.Rows.Add(row);

			row = tbl.NewRow();
			row["Process Name"] = "Print Incoming Trans Preview for NCAL Succeeded.";
			row["Status"] = "Success";
			tbl.Rows.Add(row);

			row = tbl.NewRow();
			row["Process Name"] = "Print Incoming Trans Preview for Pay My Bill Succeeded.";
			row["Status"] = "Success";
			tbl.Rows.Add(row);

			row = tbl.NewRow();
			row["Process Name"] = "Batching for Fifth Third Succeeded.";
			row["Status"] = "Success";
			tbl.Rows.Add(row);

			row = tbl.NewRow();
			row["Process Name"] = "Batching for  for NCAL Succeeded.";
			row["Status"] = "Success";
			tbl.Rows.Add(row);

			row = tbl.NewRow();
			row["Process Name"] = "Succeeded for Pay My Bill Successfully.";
			row["Status"] = "Success";
			tbl.Rows.Add(row);


            //grdLog.DataSource = tbl;
            //grdLog.DisplayLayout.Bands[0].Columns["Process Name"].Width =  400;
            //grdLog.DisplayLayout.Bands[0].Columns["Status"].Width =  100;
		}

			private void btnAdd_Click(object sender, System.EventArgs e)
		{
            foreach(Object obj in lstAvailable.SelectedItems)
            {
                lstSelected.Items.Add(obj);
            }

            while( lstAvailable.SelectedItems.Count > 0)
            {
                lstAvailable.Items.Remove(lstAvailable.SelectedItem);
            }
    
		}

		private void btnCreateBatch_Click(object sender, System.EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            string strSource = string.Empty;

            foreach (object item in lstSelected.Items)
			{
                Bank bank = (Bank)item;
                DataSet ds = bank.GetBatchData();
                pbrBatch.Maximum = ds.Tables[0].Rows.Count ;
                pbrBatch.Value = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {                    
                    lblMessage.Text = "Creating batch for " + bank.BankName  + " bank. Processing AchID " + dr["AchID"].ToString();
                    pbrBatch.Value += 1;
                    Application.DoEvents();
                    strSource = bank.ApplyBatchRules(Convert.ToInt32(dr["AchID"]));
                    
                    switch (bank.BankID)
                    { 
                        case 6:
                        case 10:
                        case 14:
                            bank.CreateBatch(Convert.ToInt32(dr["AchID"]),strSource);
                            break;
                        case 11:
                        case 12:
                            if (strSource == "VA" || strSource == "TC")
                                bank.CreateBatch(Convert.ToInt32(dr["AchID"]), strSource);
                            break;
                    }
                }
			}

            MessageBox.Show("Done");
            this.Cursor = Cursors.Default;
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            foreach (Object obj in lstAvailable.Items)
            {
                lstSelected.Items.Add(obj);
            }

            lstAvailable.Items.Clear();

        }

        private void btnCreateJournal_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string strSource = string.Empty;
            int intBatchID; int intPriID; double dblDebit; 
            double dblCredit; int intDebitCount; int intCreditCount; 
            double dblOverDailyAmountLimitCount; double dblOverItemAmount;
            string strTranBase = string.Empty;
            Bank bank = null;
            DataSet ds = null;

            foreach (object item in lstSelected.Items)
            {
                bank = (Bank)item;
                ds = bank.GetJournalData();
                pbrBatch.Maximum = ds.Tables[0].Rows.Count;
                pbrBatch.Value = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lblMessage.Text = "Creating journal for " + bank.BankName  + " bank.  Processing AchID " + dr["AchID"].ToString();
                    pbrBatch.Value += 1;
                    Application.DoEvents();

                    intBatchID = DataLayer.Field2Int(dr["BatchID"]);
                    intPriID = DataLayer.Field2Int(dr["AchID"]);
                    dblDebit = DataLayer.Field2Dbl(dr["Debit"]);
                    dblCredit = DataLayer.Field2Dbl(dr["Credit"]);
                    intDebitCount = DataLayer.Field2Int(dr["DebitCount"]);
                    intCreditCount = DataLayer.Field2Int(dr["CreditCount"]);
                    dblOverDailyAmountLimitCount = DataLayer.Field2Dbl(dr["OverDailyAmountLimitCount"]);
                    dblOverItemAmount = DataLayer.Field2Dbl(dr["OverItemAmount"]);

                    bank.CreateJournal(intBatchID, intPriID, dblDebit, dblCredit,
                        intDebitCount, intCreditCount, dblOverDailyAmountLimitCount,
                        dblOverItemAmount);
                }

                bank.JournalReleaseHold();


                //Journal NCT Batches
                switch(bank.BankID)
                {
                    case 11:
                    case 12:
                        ds = bank.GetJournalPriData();
                        pbrBatch.Maximum = ds.Tables[0].Rows.Count;
                        pbrBatch.Value = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            lblMessage.Text = "Creating journal for " + bank.BankName + " bank.  Processing AchID " + dr["AchID"].ToString();
                            pbrBatch.Value += 1;
                            Application.DoEvents();

                            intBatchID = DataLayer.Field2Int(dr["BatchID"]);
                            intPriID = DataLayer.Field2Int(dr["AchID"]);
                            dblDebit = DataLayer.Field2Dbl(dr["Debit"]);
                            dblCredit = DataLayer.Field2Dbl(dr["Credit"]);
                            intDebitCount = DataLayer.Field2Int(dr["DebitCount"]);
                            intCreditCount = DataLayer.Field2Int(dr["CreditCount"]);
                            dblOverDailyAmountLimitCount = DataLayer.Field2Dbl(dr["OverDailyAmountLimitCount"]);
                            dblOverItemAmount = DataLayer.Field2Dbl(dr["OverItemAmount"]);
                            strTranBase = DataLayer.Field2Str(dr["TransBase"]);

                            bank.CreateJournalPri(intBatchID, intPriID, dblDebit, dblCredit,
                                intDebitCount, intCreditCount, dblOverDailyAmountLimitCount,
                                dblOverItemAmount,strTranBase );
                        }
                        break;
                }//switch
                bank.JournalReleaseHoldPri();

            }//foreach

            MessageBox.Show("Done");
            this.Cursor = Cursors.Default;

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            foreach (Object obj in lstSelected.SelectedItems)
            {
                lstAvailable.Items.Add(obj);
            }

            while (lstSelected.SelectedItems.Count > 0)
            {
                lstSelected.Items.Remove(lstSelected.SelectedItem);
            }

        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            foreach (Object obj in lstSelected.Items)
            {
                lstAvailable.Items.Add(obj);
            }

            lstSelected.Items.Clear();

        }

        private void btnCreateSettlementFile_Click(object sender, EventArgs e)
        {
            Bank bank = null;
            foreach (object item in lstSelected.Items)
            {
                bank = (Bank)item;
                bank.CreateSettlementFile();
            }

        }

	

	

		
	}
}
