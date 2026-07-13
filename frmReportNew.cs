using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmReportNew : Form
    {
        private int m_BankID;
        private DateTime m_Date;
        public DateTime ProccessDate { get; set; }
        public DateTime CuttoffTime { get; set; }

        public frmReportNew()
        {
            InitializeComponent();
        }

        private void frmReportNew_Load(object sender, EventArgs e)
        {

            this.rptViewer.RefreshReport();
        }


        public void ShowBatchPreview(int BankID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Preview";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_Batch_Preview";


            
            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.DataSources.Add(new ReportDataSource("MerchantCentralDataSet_Ach_Batch_Preview", ds.Tables[0]));
            rptViewer.LocalReport.ReportPath = Application.StartupPath + @"\rptBatchPreview.rdlc";
            rptViewer.LocalReport.Refresh();

            this.Text = "Batch Preview Report";

        }

        public void ShowBatchReview(int BankID)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Review";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_Batch_Review";



            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.DataSources.Add(new ReportDataSource("MerchantCentralDataSet_Ach_Batch_Review", ds.Tables[0]));
            rptViewer.LocalReport.ReportPath = Application.StartupPath + @"\rptBatchReview.rdlc";
            rptViewer.LocalReport.Refresh();

            this.Text = "Batch Review Report";

        }

        public void ShowSettlementFile(int BankID, DateTime date)
        {
            m_BankID = BankID;
            m_Date = date;

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Recon_BankFile_Detail";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", BankID));
            cmd.Parameters.Add(new SqlParameter("@Date", date));
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                cmd.Parameters.Add(new SqlParameter("@ProcessDate", ProccessDate));
                cmd.Parameters.Add(new SqlParameter("@CuttOffTime", CuttoffTime));
            }

            DataSet ds = new DataSet();
            ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            ds.Tables[0].TableName = "Ach_File_Recon_BankFile_Detail";


            
            rptViewer.LocalReport.DataSources.Clear();
            rptViewer.LocalReport.DataSources.Add(new ReportDataSource("MerchantCentralDataSet_Ach_File_Recon_BankFile_Detail", ds.Tables[0]));
            rptViewer.LocalReport.ReportPath = Application.StartupPath + @"\rptSettlementFileReport.rdlc";
            rptViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SubreportProcessingEventHandler);
            rptViewer.LocalReport.Refresh();

            this.Text = "Settlement File Report";
            
        }

        void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            string reportPath = string.Empty;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@BankID", m_BankID));
            cmd.Parameters.Add(new SqlParameter("@Date", m_Date));
            if (m_Date.DayOfWeek == DayOfWeek.Monday)
            {
                cmd.Parameters.Add(new SqlParameter("@ProcessDate", ProccessDate));
                cmd.Parameters.Add(new SqlParameter("@CuttOffTime", CuttoffTime));
            }

            DataSet ds = null;            

            switch (e.ReportPath)
            {
                case "rptJournalReconReport":
                    cmd.CommandText = "Ach_File_Recon_Journal";
                    if (m_Date.DayOfWeek == DayOfWeek.Monday)
                    {
                        cmd.Parameters.RemoveAt("@ProcessDate");
                        cmd.Parameters.RemoveAt("@CuttOffTime");
                    }
                    
                    ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                    e.DataSources.Clear();
                    e.DataSources.Add(new ReportDataSource("MerchantCentralDataSet_Ach_File_Recon_Journal", ds.Tables[0]));
                    break;

                case "rptSettlementFileBatchOffsets":
                    cmd.CommandText = "Ach_File_Recon_BatchOffsets";
                    ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                    e.DataSources.Clear();
                    e.DataSources.Add(new ReportDataSource("MerchantCentralDataSet_Ach_File_Recon_BatchOffsets", ds.Tables[0]));
                    break;
                case "rptSettlementFileTotal":
                    cmd.CommandText = "Ach_File_Recon_FileTotal";
                    ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
                    e.DataSources.Clear();
                    e.DataSources.Add(new ReportDataSource("MerchantCentralDataSet_Ach_File_Recon_FileTotal", ds.Tables[0]));
                    break;
            }

        }

    }
}