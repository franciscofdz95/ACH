using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using System.Collections;
using System.Configuration;

using Nmc.Ach.Dal;
using CommonUtility;
namespace AchSystem
{
    public partial class frmScrubData : Form
    {
        public frmScrubData()
        {
            InitializeComponent();
        }

        private void btnScrubData_Click(object sender, EventArgs e)
        {
            if (!DataCheck())
                return;

            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Are you sure you want to start scrub process?");

            if (result == DialogResult.No)
                return;

            this.Cursor = Cursors.WaitCursor;

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
            SqlDataReader dr = data.ExecuteScrubData(prms);
            //int i = data.ExecuteScrubData2();

            dr.Read();

            if (dr[0].ToString() == "1")
            {
                prms.Clear();
                prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));
                DataSet ds = data.SelectScrubRejectDuplicateTrans(prms);

                if (ds.Tables[0].Rows.Count > 0)
                    Email.SendEmail("Scrubbed process detected duplicate transactions", "Please contact the ACH operator.", "AchProcess@merituspayment.com", ConfigurationManager.AppSettings["DuplicateTransEmail"]);

                try
                {
                    data.ExecuteUpdateCorrectionInfo();
                }
                catch (Exception exc)
                {
                    FormHandler.DispalyInformationMessage("Update Correction Information Fail! Error: " + exc.Message);
                }

                FormHandler.DispalyInformationMessage("Scrub process completed successfully.");
            }
            else
                FormHandler.DispalyErrorMessage("Scrub process failed.  Please contact system administrator.");

            this.ScrubPreview();
            this.Cursor = Cursors.Default;
        }

        private bool DataCheck()
        {
            bool perform = false;
            StringBuilder sb = new StringBuilder();

            string sql = "select count(*) from Ach_returns with (Nolock) where ";
            sql += "PostedDate >= '" + DateTime.Today.ToString("MM/dd/yyyy") + "' ";
            string rows = DataLayer.ExecuteScalar(sql, DataLayer.ConnectStringBuild());

            if (int.Parse(rows) == 0)
            {
                sb.Append("Returns have not been loadeded.  Please load returns.\n");

                perform = false;
            }


            sql = "select [dbo].[fn_IsTodayHoliday] ('" + DateTime.Today.ToString("MM/dd/yyyy") + "')";
            rows = DataLayer.ExecuteScalar(sql, DataLayer.ConnectStringBuild());

            if (rows.ToUpper() == "Y")
            {
                sb.Append("Today is a holiday. Returns cannot be loaded on a holiday.\n");

                perform = false;
            }

            if (sb.Length > 0)
            {
                FormHandler.DispalyErrorMessage(sb.ToString());
            }
            else
            {
                perform = true;
            }


            return perform;

        }

        private void btnScrubPreview_Click(object sender, EventArgs e)
        {
            this.ScrubPreview();
        }

        private void ScrubPreview()
        {
            this.Cursor = Cursors.WaitCursor;

            DataAchProcess data = new DataAchProcess();
            DataSet ds = data.Select();

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables[0];
            grd.DataSource = bs;

            grd.DisplayLayout.Bands[0].Columns["Debit Count"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Debit Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Credit Count"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Credit Amount"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grd.DisplayLayout.Bands[0].Columns["Debit Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Credit Amount"].Format = "C";


            this.Cursor = Cursors.Default;

        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            FormHandler.ExportGridToExcel(grd);
        }

        private void grd_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {

            //grd.DisplayLayout.Bands[0].Columns.Insert(0, "NCAL Allow").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grd.DisplayLayout.Bands[0].Columns["NCAL Allow"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
            grd.DisplayLayout.Bands[0].Columns["NCAL Allow"].Header.Caption = "Action";
            grd.DisplayLayout.Bands[0].Columns["NCAL Allow"].ButtonDisplayStyle = ButtonDisplayStyle.Always;


            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Extended;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;

            e.Layout.Bands[0].Columns["Merchant Name"].Width = 200;
            e.Layout.Bands[0].Columns["Debit Amount"].Format = "C";
            e.Layout.Bands[0].Columns["Credit Amount"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Debit Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Debit Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Debit Amount"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Debit Amount", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Credit Count"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Count", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:n0}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Credit Amount"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Amount", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;




        }

        private void grd_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            UltraGrid grd = (UltraGrid)sender;

            string allow = e.Row.Cells["NCAL Allow"].Text;

            if (allow.ToUpper() != "ALLOW")
            {
                e.Row.Cells["NCAL Allow"].ButtonAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                e.Row.Cells["NCAL Allow"].Value = "Allow";
            }
            else
            {
                e.Row.Cells["NCAL Allow"].Hidden = true;

            }
        }

        private void grd_ClickCellButton(object sender, CellEventArgs e)
        {
            if (MessageBox.Show("Add ACH ID to allow list?", "Allow List", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                UltraGrid grd = (UltraGrid)sender;
                string achid = grd.ActiveRow.Cells["Ach ID"].Text;

                string sql = "Insert Into dbo.NcalMerchants(AchID) Values (" + achid + ")";

                DataLayer.ExecuteSQL(sql, DataLayer.ConnectStringBuild());

                this.ScrubPreview();
            }
        }


    }
}