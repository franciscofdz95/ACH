using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using System.Data.SqlClient;

using Nmc.Ach.Dal;


namespace AchSystem
{
    public class SearchAchRecurring : SearchBase
    {
        public SearchAchRecurring()
        {
            m_Ctrl = new pnlSearchAchRecurring();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataRecurring data = new DataRecurring();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            switch (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Status ID"].Value))
            {
                case 0:
                case 2:
                case 3:
                    grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;
                default:
                    grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                    grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;

            }
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            //grd.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Trans Date"].Format = "MM/dd/yyyy HH:mm:ss";
            //grd.DisplayLayout.Bands[0].Columns["Next Process Date"].Format = "MM/dd/yyyy HH:mm:ss";
            //grd.DisplayLayout.Bands[0].Columns["Date Processed"].Format = "MM/dd/yyyy HH:mm:ss";

            //grd.DisplayLayout.Bands[0].Summaries.Clear();
            //grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            //UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Amount"];
            //SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            //summary.DisplayFormat = "Total = {0:c}";
            //summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty && grp.Controls["txtTransID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID, a Merchant ID, or a Trans ID.\n";
            //}

            return strError;
        }
    }

    public class SearchTransaction: SearchBase
    {
        public SearchTransaction()
        {
            m_Ctrl = new pnlSearchTransaction();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            switch (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Status ID"].Value))
            {
                case 0:
                case 2:
                case 3:
                    grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;
                default:
                    grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                    grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;

            }
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Account Number"].Hidden = true;
            grd.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Trans Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["Next Process Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["Date Processed"].Format = "MM/dd/yyyy HH:mm:ss";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True; 
            UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Amount"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            summary.DisplayFormat = "Total = {0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty && grp.Controls["txtTransID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID, a Merchant ID, or a Trans ID.\n";
            //}

            return strError;
        }
    }

    public class SearchOverTicketItems : SearchBase
    {
        public SearchOverTicketItems()
        {
            m_Ctrl = new pnlSearchOverTicketItems();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchOverTicketItems(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            switch (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Status ID"].Value))
            {
                case 0:
                case 2:
                case 3:
                    grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                    grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;
                default:
                    grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                    grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    break;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty && grp.Controls["txtTransID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID, a Merchant ID, or a Trans ID.\n";
            //}

            return strError;
        }
    }

    public class SearchAccountBlock : SearchBase
    {
        public SearchAccountBlock()
        {
            m_Ctrl = new pnlSearchAccountBlock();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataAccountBlock data = new DataAccountBlock();

            try
            {
                ds = data.Search (prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            //if (grp.Controls["txtTransRoute"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter a Trans Route Number.\n";
            //}

            //if (grp.Controls["txtAccountNo"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter an Account Number.\n";
            //}

            return strError;
        }
    }

    public class SearchPurgeBatch : SearchBase
    {

        public SearchPurgeBatch()
        {
            m_Ctrl = new pnlSearchPurgeBatch();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchUploadBatch(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            {
                strError += "Please enter either an ACH ID or a Merchant ID.\n";
            }

            return strError;
        }
    }

    public class SearchReturnRates : SearchBase
    {

        public SearchReturnRates()
        {
            m_Ctrl = new pnlSearchReturnRates();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchReturnRates(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Transaction Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Return Volume"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Transaction Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Transaction Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Transaction Volume"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Transaction Volume", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Return Count"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Count", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:n}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Return Volume"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Volume", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
    }

    public class SearchReturnSummary : SearchBase
    {

        public SearchReturnSummary()
        {
            m_Ctrl = new pnlSearchReturnSummary();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchReturnSummary(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {

            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Reason Desc"].Width = 300;
            grd.DisplayLayout.Bands[0].Columns["Return Volume"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Return Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Return Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Return Volume"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Volume", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            

        }
    }

    public class SearchNegativeBalanceReport : SearchBase
    {

        public SearchNegativeBalanceReport()
        {
            m_Ctrl = new pnlSearchNegativeBalanceReturn();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchMerchantNegativeBalanceReturn(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Sale Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Return Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Credit Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Avg Tkt"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Average Ticket"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Sale Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Sale Volume"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Volume", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Return Count"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Count", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:n0}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Return Volume"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Volume", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col5 = grd.DisplayLayout.Bands[0].Columns["Credit Count"];
            SummarySettings summary5 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Count", SummaryType.Sum, col5);
            summary5.DisplayFormat = "{0:n0}";
            summary5.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col6 = grd.DisplayLayout.Bands[0].Columns["Credit Volume"];
            SummarySettings summary6 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Volume", SummaryType.Sum, col6);
            summary6.DisplayFormat = "{0:c}";
            summary6.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
    }

    public class SearchMonthlyActivityTotals : SearchBase
    {

        public SearchMonthlyActivityTotals()
        {
            m_Ctrl = new pnlSearchMonthlyActivityTotals();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchMonthlyActivityTotals(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Sale Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Return Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Credit Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["BL Credit Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Avg Tkt"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Average Ticket"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Sale Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Sale Volume"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Volume", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Return Count"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Count", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:n0}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Return Volume"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Volume", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col5 = grd.DisplayLayout.Bands[0].Columns["Credit Count"];
            SummarySettings summary5 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Count", SummaryType.Sum, col5);
            summary5.DisplayFormat = "{0:n0}";
            summary5.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col6 = grd.DisplayLayout.Bands[0].Columns["Credit Volume"];
            SummarySettings summary6 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Volume", SummaryType.Sum, col6);
            summary6.DisplayFormat = "{0:c}";
            summary6.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col7 = grd.DisplayLayout.Bands[0].Columns["BL Credit Count"];
            SummarySettings summary7 = grd.DisplayLayout.Bands[0].Summaries.Add("BL Credit Count", SummaryType.Sum, col7);
            summary7.DisplayFormat = "{0:n0}";
            summary7.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col8 = grd.DisplayLayout.Bands[0].Columns["BL Credit Volume"];
            SummarySettings summary8 = grd.DisplayLayout.Bands[0].Summaries.Add("BL Credit Volume", SummaryType.Sum, col8);
            summary8.DisplayFormat = "{0:c}";
            summary8.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            
            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
    }

    public class SearchExceedAverageTicket : SearchBase
    {

        public SearchExceedAverageTicket()
        {
            m_Ctrl = new pnlSearchExceedAverageTicket();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchExceedAverageTicket(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Sale Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Amount Over Approved"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Avg Tkt"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Average Ticket"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Sale Volume"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Volume", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Amount Over Approved"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Amount Over Approved", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Approved Avg Tkt"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Approved Avg Tkt", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:c}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Approved Mo Vol", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col5 = grd.DisplayLayout.Bands[0].Columns["Average Ticket"];
            SummarySettings summary5 = grd.DisplayLayout.Bands[0].Summaries.Add("Average Ticket", SummaryType.Sum, col5);
            summary5.DisplayFormat = "{0:c}";
            summary5.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col6 = grd.DisplayLayout.Bands[0].Columns["Sale Count"];
            SummarySettings summary6 = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Count", SummaryType.Sum, col6);
            summary6.DisplayFormat = "{0:n0}";
            summary6.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
    }


    public class SearchExceedMonthlyVolume : SearchBase
    {

        public SearchExceedMonthlyVolume()
        {
            m_Ctrl = new pnlSearchExceedMonthlyVolume();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchExceedMonthlyVolume(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Sale Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Amount Over Approved"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Sale Volume"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Volume", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Amount Over Approved"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Amount Over Approved", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;



            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Approved Mo Vol", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

           

        }
    }

    public class SearchOTMonthlyActivityTotals : SearchBase
    {

        public SearchOTMonthlyActivityTotals()
        {
            m_Ctrl = new pnlSearchOTMonthlyActivityTotals();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchOTMonthlyActivityTotals (prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Sale Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Return Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Credit Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["BL Credit Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Avg Tkt"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Average Ticket"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Sale Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Sale Volume"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Volume", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Return Count"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Count", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:n0}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Return Volume"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Volume", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col5 = grd.DisplayLayout.Bands[0].Columns["Credit Count"];
            SummarySettings summary5 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Count", SummaryType.Sum, col5);
            summary5.DisplayFormat = "{0:n0}";
            summary5.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col6 = grd.DisplayLayout.Bands[0].Columns["Credit Volume"];
            SummarySettings summary6 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Volume", SummaryType.Sum, col6);
            summary6.DisplayFormat = "{0:c}";
            summary6.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col7 = grd.DisplayLayout.Bands[0].Columns["BL Credit Count"];
            SummarySettings summary7 = grd.DisplayLayout.Bands[0].Summaries.Add("BL Credit Count", SummaryType.Sum, col7);
            summary7.DisplayFormat = "{0:n0}";
            summary7.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col8 = grd.DisplayLayout.Bands[0].Columns["BL Credit Volume"];
            SummarySettings summary8 = grd.DisplayLayout.Bands[0].Summaries.Add("BL Credit Volume", SummaryType.Sum, col8);
            summary8.DisplayFormat = "{0:c}";
            summary8.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
    }


    public class SearchUnauthorizedReturnRates : SearchBase
    {

        public SearchUnauthorizedReturnRates()
        {
            m_Ctrl = new pnlSearchUnauthorizedReturnRates();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransaction data = new DataTransaction();

            try
            {
                ds = data.SearchUnauthorizedReturnRates(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Record Count"].Value) ==
                    Convert.ToInt32(grd.Rows[e.RowIndex].Cells["Total Open Record"].Value))
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LemonChiffon;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
            else
            {
                grd.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gainsboro;
                grd.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;

            }
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];
            //if (grp.Controls["txtAchID"].Text.Trim() == string.Empty && grp.Controls["txtMerchantID"].Text.Trim() == string.Empty)
            //{
            //    strError += "Please enter either an ACH ID or a Merchant ID.\n";
            //}

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Merchant Name"].Width = 200;
            grd.DisplayLayout.Bands[0].Columns["Sale Volume"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Return Volume"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Credit Volume"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Approved Avg Tkt"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Approved Mo Vol"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Average Ticket"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Sale Count"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Count", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:n0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Sale Volume"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Sale Volume", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col3 = grd.DisplayLayout.Bands[0].Columns["Return Count"];
            SummarySettings summary3 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Count", SummaryType.Sum, col3);
            summary3.DisplayFormat = "{0:n0}";
            summary3.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col4 = grd.DisplayLayout.Bands[0].Columns["Return Volume"];
            SummarySettings summary4 = grd.DisplayLayout.Bands[0].Summaries.Add("Return Volume", SummaryType.Sum, col4);
            summary4.DisplayFormat = "{0:c}";
            summary4.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col5 = grd.DisplayLayout.Bands[0].Columns["R05"];
            SummarySettings summary5 = grd.DisplayLayout.Bands[0].Summaries.Add("R05", SummaryType.Sum, col5);
            summary5.DisplayFormat = "{0:n0}";
            summary5.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col6 = grd.DisplayLayout.Bands[0].Columns["R07"];
            SummarySettings summary6 = grd.DisplayLayout.Bands[0].Summaries.Add("R07", SummaryType.Sum, col6);
            summary6.DisplayFormat = "{0:n0}";
            summary6.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col7 = grd.DisplayLayout.Bands[0].Columns["R10"];
            SummarySettings summary7 = grd.DisplayLayout.Bands[0].Summaries.Add("R10", SummaryType.Sum, col7);
            summary7.DisplayFormat = "{0:n0}";
            summary7.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col8 = grd.DisplayLayout.Bands[0].Columns["R29"];
            SummarySettings summary8 = grd.DisplayLayout.Bands[0].Summaries.Add("R29", SummaryType.Sum, col8);
            summary8.DisplayFormat = "{0:n0}";
            summary8.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn col9 = grd.DisplayLayout.Bands[0].Columns["R51"];
            SummarySettings summary9 = grd.DisplayLayout.Bands[0].Summaries.Add("R51", SummaryType.Sum, col9);
            summary9.DisplayFormat = "{0:n0}";
            summary9.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            //UltraGridColumn col6 = grd.DisplayLayout.Bands[0].Columns["Credit Volume"];
            //SummarySettings summary6 = grd.DisplayLayout.Bands[0].Summaries.Add("Credit Volume", SummaryType.Sum, col6);
            //summary6.DisplayFormat = "{0:c}";
            //summary6.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries.Add(SummaryType.Sum, grd.DisplayLayout.Bands[0].Columns["Amount"]);
            //grd.DisplayLayout.Bands[0].Summaries[0].Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
            //grd.DisplayLayout.Bands[0].Summaries[0].DisplayFormat = "Total = {0:c}";

        }
    }


    public class SearchBank : SearchBase
    {
        public SearchBank()
        {
            m_Ctrl = new pnlSearchBank();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataBank data = new DataBank();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            if (grp.Controls["txtBankName"].Text.Trim() == string.Empty && grp.Controls["txtTransRoute"].Text.Trim() == string.Empty)
            {
                strError += "Please enter a bank name or routing number.\n";
            }

            return strError;
        }
    }

    public class SearchJournal : SearchBase
    {
        public SearchJournal()
        {
            m_Ctrl = new pnlSearchJournal();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataJournal  data = new DataJournal();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Amount"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            summary.DisplayFormat = "Total = {0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            if (grp.Controls["txtAmount"].Text.Trim() != string.Empty)
            {
                if (!DataLayer.IsNumeric(grp.Controls["txtAmount"].Text))
                {
                    strError += "Please enter a valid Amount.\n";
                }
            }

            return strError;
        }
    }

    public class SearchHold : SearchBase
    {
        public SearchHold()
        {
            m_Ctrl = new pnlSearchHold();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataHold data = new DataHold();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            if (!grd.DisplayLayout.Bands[0].Columns.Exists("Action"))
                grd.DisplayLayout.Bands[0].Columns.Insert(0,"Action").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;

            grd.DisplayLayout.Bands[0].Columns["Action"].Header.Caption = "Action";
            grd.DisplayLayout.Bands[0].Columns["Action"].ButtonDisplayStyle = ButtonDisplayStyle.Always;

            grd.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["Release Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["Paid Date"].Format = "MM/dd/yyyy HH:mm:ss";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Amount"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            summary.DisplayFormat = "Total = {0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }

        public override void GridClickCellButton(object sender, CellEventArgs e)
        {

            if (MessageBox.Show("Do you want to release this hold/reserve?", "Hold/Reserve Release", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                UltraGrid grd = (UltraGrid)sender;
                string holdid = grd.ActiveRow.Cells["HoldID"].Text;

                ArrayList prms = new ArrayList();
                prms.Add(new SqlParameter("@HoldID", holdid));
                prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

                DataHold data = new DataHold();
                data.ReleaseHoldReserve(prms);

                
            }
        
        }

        public override void GridInitializeRow(object sender, InitializeRowEventArgs e)
        {
            UltraGrid grd = (UltraGrid) sender;

            string dt = e.Row.Cells["Paid Date"].Text;
            string type = e.Row.Cells["Type"].Text;

            if (string.IsNullOrEmpty(dt))
            {
                e.Row.Cells["Action"].ButtonAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
                e.Row.Cells["Action"].Value = "Release";
            }
            else
            {
                e.Row.Cells["Action"].Hidden = true;

            }

        }

        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            if (grp.Controls["txtAmount"].Text.Trim() != string.Empty)
            {
                if (!DataLayer.IsNumeric(grp.Controls["txtAmount"].Text))
                {
                    strError += "Please enter a valid Amount.\n";
                }
            }

            return strError;
        }
    }

    public class SearchPending : SearchBase
    {
        public SearchPending()
        {
            m_Ctrl = new pnlSearchPending();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataPayment data = new DataPayment();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["Date Processed"].Format = "MM/dd/yyyy HH:mm:ss";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Amount"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            summary.DisplayFormat = "Total = {0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            if (grp.Controls["txtAmount"].Text.Trim() != string.Empty)
            {
                if (!DataLayer.IsNumeric(grp.Controls["txtAmount"].Text))
                {
                    strError += "Please enter a valid Amount.\n";
                }
            }


            return strError;
        }
    }

    public class SearchUser : SearchBase
    {
        public SearchUser()
        {
            m_Ctrl = new pnlSearchUser();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataUser data = new DataUser();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }
    public class SearchLog : SearchBase
    {
        public SearchLog()
        {
            m_Ctrl = new pnlSearchLog();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataLog data = new DataLog();
            DataSet ds = null;

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";

        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchReturn : SearchBase
    {
        public SearchReturn()
        {
            m_Ctrl = new pnlSearchReturn();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataReturn data = new DataReturn();

            try
            {
                ds = data.Search2(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Account No"].Hidden = true;

            grd.DisplayLayout.Bands[0].Columns["Amount"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Amount"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            summary.DisplayFormat = "Total = {0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;
            //UltraGroupBox grp = (UltraGroupBox)ctrl.Controls["grpRequired"];

            //if (grp.Controls["txtAmount"].Text.Trim() != string.Empty)
            //{
            //    if (!DataLayer.IsNumeric(grp.Controls["txtAmount"].Text))
            //    {
            //        strError += "Please enter a valid Amount.\n";
            //    }
            //}

            return strError;
        }
    }

    public class SearchMerchant : SearchBase
    {
        public SearchMerchant()
        {
            m_Ctrl = new pnlSearchMerchant();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataMerchant data = new DataMerchant();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
        
    }

    public class SearchMerchantBalance : SearchBase
    {
        public SearchMerchantBalance()
        {
            m_Ctrl = new pnlSearchMerchantBalance();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataMerchant data = new DataMerchant();

            try
            {
                ds = data.SearchMerchantBalance(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Available Balance"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
            grd.DisplayLayout.Bands[0].Columns["Balance"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;

            grd.DisplayLayout.Bands[0].Columns["Available Balance"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Balance"].Format = "C";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;

            UltraGridColumn col = grd.DisplayLayout.Bands[0].Columns["Available Balance"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("Available Balance", SummaryType.Sum, col);
            summary.DisplayFormat = "{0:c0}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;


            UltraGridColumn col2 = grd.DisplayLayout.Bands[0].Columns["Balance"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("Balance", SummaryType.Sum, col2);
            summary2.DisplayFormat = "{0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;



        }
    }

    public class SearchTransStatus : SearchBase
    {
        public SearchTransStatus()
        {
            m_Ctrl = new pnlSearchTransStatus();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransStatus data = new DataTransStatus();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchRefcode : SearchBase
    {
        public SearchRefcode()
        {
            m_Ctrl = new pnlSearchRefcode();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataRefcode data = new DataRefcode();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchReasonCode : SearchBase
    {
        public SearchReasonCode()
        {
            m_Ctrl = new pnlSearchReasonCode();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataReasonCode data = new DataReasonCode();

            try
            {
                ds = data.Search (prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchBatch : SearchBase
    {
        public SearchBatch()
        {
            m_Ctrl = new pnlSearchBatch();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataBatch data = new DataBatch();

            try
            {
                ds = data.Search (prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Debit"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Credit"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["To Process Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["Processed Date"].Format = "MM/dd/yyyy HH:mm:ss";

            grd.DisplayLayout.Bands[0].Summaries.Clear();
            grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Debit"];
            SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            summary.DisplayFormat = "Total = {0:c}";
            summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            UltraGridColumn columnToSummarize2 = grd.DisplayLayout.Bands[0].Columns["Credit"];
            SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal2", SummaryType.Sum, columnToSummarize2);
            summary2.DisplayFormat = "Total = {0:c}";
            summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchReturnTotals : SearchBase
    {
        public SearchReturnTotals()
        {
            m_Ctrl = new pnlSearchReturnTotals();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataReturn data = new DataReturn();

            try
            {
                ds = data.SelectReturnTotals(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            //grd.DisplayLayout.Bands[0].Columns["Debit"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Credit"].Format = "C";
            //grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";
            //grd.DisplayLayout.Bands[0].Columns["To Process Date"].Format = "MM/dd/yyyy HH:mm:ss";
            //grd.DisplayLayout.Bands[0].Columns["Processed Date"].Format = "MM/dd/yyyy HH:mm:ss";

            //grd.DisplayLayout.Bands[0].Summaries.Clear();
            //grd.DisplayLayout.Override.AllowRowSummaries = AllowRowSummaries.True;
            //UltraGridColumn columnToSummarize = grd.DisplayLayout.Bands[0].Columns["Debit"];
            //SummarySettings summary = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal", SummaryType.Sum, columnToSummarize);
            //summary.DisplayFormat = "Total = {0:c}";
            //summary.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;

            //UltraGridColumn columnToSummarize2 = grd.DisplayLayout.Bands[0].Columns["Credit"];
            //SummarySettings summary2 = grd.DisplayLayout.Bands[0].Summaries.Add("GrandTotal2", SummaryType.Sum, columnToSummarize2);
            //summary2.DisplayFormat = "Total = {0:c}";
            //summary2.Appearance.TextHAlign = Infragistics.Win.HAlign.Right;
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchBatchDetail : SearchBase
    {
        public SearchBatchDetail()
        {
            m_Ctrl = new pnlSearchBatchDetail();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataBatchDetail data = new DataBatchDetail();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Posted Date"].Format = "MM/dd/yyyy HH:mm:ss";

        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchOrigin : SearchBase
    {
        public SearchOrigin()
        {
            m_Ctrl = new pnlSearchOrigin();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataOrigin data = new DataOrigin();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchSecc : SearchBase
    {
        public SearchSecc()
        {
            m_Ctrl = new pnlSearchSecc();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataSecc data = new DataSecc();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchSource : SearchBase
    {
        public SearchSource()
        {
            m_Ctrl = new pnlSearchSource();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataSource data = new DataSource();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchTransType : SearchBase
    {
        public SearchTransType()
        {
            m_Ctrl = new pnlSearchTransType();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataTransType data = new DataTransType();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchGroupMerchant : SearchBase
    {
        public SearchGroupMerchant()
        {
            m_Ctrl = new pnlSearchGroupMerchant();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataGroupMerchant data = new DataGroupMerchant();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchProcessLog : SearchBase
    {
        public SearchProcessLog()
        {
            m_Ctrl = new pnlSearchProcessLog();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataProcessLog data = new DataProcessLog();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchBatchFileLog : SearchBase
    {
        public SearchBatchFileLog()
        {
            m_Ctrl = new pnlSearchBatchFileLog();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataBatchFileLog data = new DataBatchFileLog();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            grd.DisplayLayout.Bands[0].Columns["Load Date"].Format = "MM/dd/yyyy HH:mm:ss";

        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchSettlementForecast : SearchBase
    {
        public SearchSettlementForecast()
        {
            m_Ctrl = new pnlSearchSettlementForecast();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataAchProcess data = new DataAchProcess();

            try
            {
                ds = data.SelectReportSettlementForecast(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override void FormFormatList(DataGridView grd, DataGridViewRowPrePaintEventArgs e)
        {
        }
        public override void GridInitializeLayout(UltraGrid grd)
        {
            //grd.DisplayLayout.Bands[0].Columns["Load Date"].Format = "MM/dd/yyyy HH:mm:ss";
            grd.DisplayLayout.Bands[0].Columns["13"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["15"].Format = "C";
            grd.DisplayLayout.Bands[0].Columns["17"].Format = "C";
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchBadReturn : SearchBase
    {
        public SearchBadReturn()
        {
            m_Ctrl = new pnlSearchBadReturn();
        }
        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataBadReturn data = new DataBadReturn();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }
        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }

    public class SearchHoliday : SearchBase
    {
        public SearchHoliday()
        {
            m_Ctrl = new pnlSearchHoliday();
        }

        public override DataSet FormSearch(ArrayList prms)
        {
            DataSet ds = null;
            DataHoliday data = new DataHoliday();

            try
            {
                ds = data.Search(prms);
            }
            catch (Exception exc)
            {
                throw exc;
            }
            return ds;
        }

        public override string FormCheck(UserControl ctrl)
        {
            string strError = string.Empty;

            return strError;
        }
    }


}
