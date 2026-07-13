using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmReport : Form
    {
        
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {

            //Add code

        }
        public void ShowBatchReview(int BankID)
        {

            rptBatchReview rpt = new rptBatchReview();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Review";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));

            
            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_Batch_Review";

            rpt.SetDataSource(ds);

            this.crvMain.ReportSource = rpt;
            this.crvMain.DisplayGroupTree = false;
            this.Text = "Batch Review";

        }

        public void ShowBatchPreview(int BankID)
        {

            rptBatchPreview rpt = new rptBatchPreview();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Preview";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_Batch_Preview";

            rpt.SetDataSource(ds);

            this.crvMain.ReportSource = rpt;
            this.crvMain.DisplayGroupTree = false;
            this.Text = "Batch Preview";

        }

        public void ShowSettlementFile(int BankID, DateTime date)
        {

            rptSettlementFileReport rpt = new rptSettlementFileReport();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Recon_BankFile_Detail";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));
            cmd.Parameters.Add(new SqlParameter("@Date", date));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_File_Recon_BankFile_Detail";

            rpt.SetDataSource(ds);

            SetSettlementFileBatchOffsetsSubreport(BankID, date, rpt);
            SetSettlementFileSubreport(BankID, date, rpt);
            SetSettlementFileTotalSubreport(BankID, date, rpt);
            

            this.crvMain.ReportSource = rpt;
            this.crvMain.DisplayGroupTree = false;
            this.Text = "Settlement File";

        }
        
        public void SetSettlementFileSubreport(int BankID, DateTime date, rptSettlementFileReport rpt)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Recon_Journal";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));
            cmd.Parameters.Add(new SqlParameter("@Date", date));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_File_Recon_Journal";

            rpt.Subreports["rptJournalReconReport.rpt"].SetDataSource(ds);

        }

        public void SetSettlementFileBatchOffsetsSubreport(int BankID, DateTime date, rptSettlementFileReport rpt)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Recon_BatchOffsets";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));
            cmd.Parameters.Add(new SqlParameter("@Date", date));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_File_Recon_BatchOffsets";

            rpt.Subreports["rptSettlementFileBatchOffsets.rpt"].SetDataSource(ds);

        }

        public void SetSettlementFileTotalSubreport(int BankID, DateTime date, rptSettlementFileReport rpt)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Recon_FileTotal";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));
            cmd.Parameters.Add(new SqlParameter("@Date", date));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_File_Recon_FileTotal";

            rpt.Subreports["rptSettlementFileTotal.rpt"].SetDataSource(ds);

        }
    }
}