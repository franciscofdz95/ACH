using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Web;

namespace ZeusWeb.Class
{
    internal class DataTablePaging
    {
        public static List<InvoiceReportRow> GetDatatableInvoice(string sortExpression, bool sortDesc, int pageSize, int currentPage, int categoryId)
        {
            List<InvoiceReportRow> rows = new List<InvoiceReportRow>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchBillableInvoice";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@CurrentPage", currentPage);
            cmd.Parameters.AddWithValue("@SortOrder", sortExpression);
            cmd.Parameters.AddWithValue("@SortDirection", sortDesc.GetHashCode());
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);

            DateTime date;
            decimal moMoney;

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    InvoiceReportRow row = new InvoiceReportRow()
                    {
                        ACHDescriptor = rdr["ACHDescriptor"].ToString(),
                        BusinessDBAName = rdr["BusinessDBAName"].ToString(),
                        FeeDescription = rdr["FeeDescription"].ToString(),
                        InvoiceQuantity = int.Parse(rdr["Quantity"].ToString()),
                        MerchantID = int.Parse(rdr["MerchantID"].ToString()),
                        SettlePlatformMID = rdr["SettlePlatformMID"].ToString(),
                        TotalRecordCount = int.Parse(rdr["TotalRecordCount"].ToString()),
                        InvoiceDetailID = long.Parse(rdr["InvoiceDetailID"].ToString()),
                        RunID = long.Parse(rdr["RunID"].ToString()),
                        BankName = rdr["BankName"].ToString(),
                        SalesPartnerDBA = rdr["SalesPartnerDBA"].ToString(),
                        SalesPartnerID = rdr["SalesPartnerID"].ToString(),
                        MerchantStatus = rdr["MerchantStatus"].ToString()
                    };

                    decimal.TryParse(rdr["InvoiceAmount"].ToString(), out moMoney);
                    row.InvAmt = string.Format("{0:0.00}", moMoney);

                    decimal.TryParse(rdr["Amount"].ToString(), out moMoney);
                    row.InvoiceAmt = string.Format("{0:0.00}", moMoney);

                    if (DateTime.TryParse(rdr["DateEnrolled"].ToString(), out date))
                    {
                        row.DateEnrolled = date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        row.DateEnrolled = "";
                    }

                    if (DateTime.TryParse(rdr["LastBillDate"].ToString(), out date))
                    {
                        row.LastBillDate = date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        row.LastBillDate = "";
                    }

                    if (DateTime.TryParse(rdr["BillDate"].ToString(), out date))
                    {
                        row.BillDate = date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        row.BillDate = "";
                    }

                    rows.Add(row);
                }

                rdr.Close();
            }

            return rows;
        }

        #region BILLING REPORTS

        /// <summary>
        /// DM-3994 Design - Add new billing processes to Accounting tab in Zeus
        /// </summary>
        /// <returns><code>List of AnnualFeesRow</code></returns>
        public static List<AnnualFeesRow> GetAnnualFees()
        {
            List<AnnualFeesRow> rows = new List<AnnualFeesRow>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchAnnualBilling";
            cmd.CommandType = CommandType.StoredProcedure;

            decimal moMoney;

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    AnnualFeesRow row = new AnnualFeesRow()
                    {
                        DBA = rdr["DBA"].ToString(),
                        ZID = int.Parse(rdr["ZID"].ToString()),
                        PartnerID = int.Parse(rdr["PartnerID"].ToString()),
                        PartnerName = rdr["PartnerName"].ToString(),
                        Status = rdr["Status"].ToString(),
                        Bank = rdr["Bank"].ToString(),
                        Month = rdr["Month"].ToString()
                    };

                    decimal.TryParse(rdr["AnnualFee"].ToString(), out moMoney);
                    row.AnnualFee = string.Format("{0:0.00}", moMoney);

                    rows.Add(row);
                }

                rdr.Close();
            }

            return rows;
        }

        /// <summary>
        /// DM-4445 Dev Only -- Add PCI NAF billing process to Accounting tab in Zeus
        /// </summary>
        /// <returns><code>List of PCINAFRow</code></returns>
        public static List<PCINAFRow> GetPCINAFBilling()
        {
            List<PCINAFRow> rows = new List<PCINAFRow>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchPCINAFBilling";
            cmd.CommandType = CommandType.StoredProcedure;

            DateTime date;
            decimal moMoney;

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    PCINAFRow row = new PCINAFRow()
                    {
                        Zid = Convert.ToInt64(rdr["zid"].ToString()),
                        Mid = rdr["mid"].ToString(),
                        DBA = rdr["dba"].ToString(),
                        OfficeName = rdr["OfficeName"].ToString(),
                        Partner = rdr["partner"].ToString(),
                        MerchantLevel = int.Parse(rdr["merchant level"].ToString()),
                        TransactionCount = Convert.ToInt32(rdr["transaction count"].ToString()),
                        ProcessingVolume = Convert.ToDecimal(rdr["processing volume"].ToString()),
                        CP_CNP = rdr["CP/CNP"].ToString(),
                        Bank = rdr["bank"].ToString(),
                        NumberOfTimeBilled = int.Parse(rdr["# of times billed"].ToString()),
                        Status = rdr["status"].ToString(),
                        Month = rdr["Month"].ToString()
                    };

                    decimal.TryParse(rdr["amount"].ToString(), out moMoney);
                    row.Amount = string.Format("{0:0.00}", moMoney);

                    if (DateTime.TryParse(rdr["last billing date"].ToString(), out date))
                        row.LastBillingDate = date.ToString("MM/dd/yyyy");
                    else
                        row.LastBillingDate = string.Empty;

                    rows.Add(row);
                }

                rdr.Close();
            }

            return rows;
        }

        /// <summary>
        /// DM-4446 Dev Only -- Add Annual PCI billing process to Accounting tab in Zeus
        /// </summary>
        /// <returns><code>List of PCIAnnualRow</code></returns>
        public static List<PCIAnnualRow> GetPCIAnnualBilling()
        {
            List<PCIAnnualRow> rows = new List<PCIAnnualRow>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchPCIAnnualBilling";
            cmd.CommandType = CommandType.StoredProcedure;

            DateTime date;
            decimal moMoney;

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    PCIAnnualRow row = new PCIAnnualRow()
                    {
                        ClientID = rdr["ClientID"].ToString(),
                        MID = rdr["MID"].ToString(),
                        DBA = rdr["dba"].ToString(),
                        PartnerID = Convert.ToInt64(rdr["PartnerID"].ToString()),
                        PartnerDBA = rdr["partnerDBA"].ToString(),
                        SpecialRequest = rdr["SpecialRequest"].ToString(),
                        AchID = int.Parse(rdr["AchID"].ToString()),
                        MerchantID = int.Parse(rdr["MerchantID"].ToString()),
                        SECC = rdr["SECC"].ToString(),
                        TransType = rdr["TransType"].ToString(),
                        AccountName = rdr["AccountName"].ToString(),
                        Description = rdr["Description"].ToString(),
                        RefID = rdr["RefID"].ToString(),
                        BusinessLegalName = rdr["BusinessLegalName"].ToString(),
                        Status = rdr["status"].ToString(),
                        RoutingNumber = rdr["RoutingNumber"].ToString(),
                        AccountNumber = rdr["AccountNumber"].ToString(),
                        Bank = rdr["bank"].ToString(),
                        CUApprovedDateCC = rdr["CUApprovedDateCC"].ToString(),
                        CUApprovedDateAch = rdr["CUApprovedDateAch"].ToString()
                    };

                    decimal.TryParse(rdr["Amount"].ToString(), out moMoney);
                    row.Amount = string.Format("{0:0.00}", moMoney);

                    if (DateTime.TryParse(rdr["DescDate"].ToString(), out date))
                        row.DescDate = date.ToString("MM/dd/yyyy");
                    else
                        row.DescDate = string.Empty;

                    if (DateTime.TryParse(rdr["TransDate"].ToString(), out date))
                        row.TransDate = date.ToString("MM/dd/yyyy");
                    else
                        row.TransDate = string.Empty;

                    if (DateTime.TryParse(rdr["DateAdded"].ToString(), out date))
                        row.DateAdded = date.ToString("MM/dd/yyyy");
                    else
                        row.DateAdded = string.Empty;

                    if (DateTime.TryParse(rdr["NextProcessDate"].ToString(), out date))
                        row.NextProcessDate = date.ToString("MM/dd/yyyy");
                    else
                        row.NextProcessDate = string.Empty;

                    rows.Add(row);
                }

                rdr.Close();
            }

            return rows;
        }

        /// <summary>
        /// DM-3994 Design - Add new billing processes to Accounting tab in Zeus
        /// </summary>
        /// <param name="dt"><code>DataTable</code> with ZIDs</param>
        /// <param name="billBy">UserSessions.CurrentUser.UserName</param>
        public static void InsertACHAnnualFees(DataTable dt, string billBy)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_InsertAnnualBilling";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ZidTable", dt);
            cmd.Parameters.AddWithValue("@AddedBy", billBy);

            DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        /// <summary>
        /// DM-4445 Dev Only -- Add PCI NAF billing process to Accounting tab in Zeus
        /// </summary>
        /// <param name="dt"><code>DataTable</code> with ZIDs</param>
        /// <param name="billBy">UserSessions.CurrentUser.UserName</param>
        public static void InsertACHPCINAFFees(DataTable dt, string billBy)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_InsertPCINAFBilling";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ZidTable", dt);
            cmd.Parameters.AddWithValue("@AddedBy", billBy);

            DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        /// <summary>
        /// DM-4446 Dev Only -- Add Annual PCI billing process to Accounting tab in Zeus
        /// </summary>
        /// <param name="dt"><code>DataTable</code> with ZIDs</param>
        /// <param name="billBy">UserSessions.CurrentUser.UserName</param>
        public static void InsertACHPCIAnnualFees(DataTable dt, string billBy)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_InsertPCIAnnualBilling";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ZidTable", dt);
            cmd.Parameters.AddWithValue("@AddedBy", billBy);

            DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        #endregion

        public static DataSet GetDSInvoiceReport(string sortExpression, bool sortDesc, int categoryId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchBillableInvoice";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PageSize", -1);
            cmd.Parameters.AddWithValue("@SortOrder", sortExpression);
            cmd.Parameters.AddWithValue("@SortDirection", sortDesc.GetHashCode());
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            return ds;
        }

        public static void ACHInvoice(DataTable dt, string billBy)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_InvoiceBillACH";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceTable", dt);
            cmd.Parameters.AddWithValue("@BillBy", billBy);

            DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public static string GetMerchantInvoiceDetails(long invoiceDetailId)
        {
            StringBuilder json = new StringBuilder();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SelectMerchantInvoiceDetail";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceDetailID", invoiceDetailId);

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    json.AppendFormat(@"{{ ""MerchantID"": {0}, ""BusinessDBAName"": ""{1}"", ""FeeDescription"": ""{2}"",", rdr["MerchantID"], rdr["BusinessDBAName"], rdr["FeeDescription"]);
                    json.AppendFormat(@"""MerchantFee"": {0}, ""InvAmt"" : {1}, ""Quantity"" : {2}, ""Waived"": {3}, ""success"": ""true""}}", rdr["MerchantProductFee"], rdr["InvoiceAmount"], rdr["Quantity"], rdr["WaiveProductFee"]);
                }

                rdr.Close();
            }

            return json.ToString();
        }

        public static void UpdateMerchantInvoiceDetails(long invoiceDetailId, int merchantId, decimal billAmt, decimal feeAmt, int quantity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_UpdateMerchantInvoiceDetail";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InvoiceDetailID", invoiceDetailId);
            cmd.Parameters.AddWithValue("@MerchantID", merchantId);
            cmd.Parameters.AddWithValue("@MerchantProductFee", feeAmt);
            cmd.Parameters.AddWithValue("@InvoiceAmount", billAmt);
            cmd.Parameters.AddWithValue("@Quantity", quantity);

            DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public static List<InvoiceHistoryRow> GetDatatableInvoiceHistory(Hashtable prms)
        {
            List<InvoiceHistoryRow> rows = new List<InvoiceHistoryRow>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchInvoiceHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DateTime date;
            decimal moMoney;

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    InvoiceHistoryRow row = new InvoiceHistoryRow()
                    {
                        ACHDescriptor = rdr["ACHDescriptor"].ToString(),
                        ACHStatus = rdr["ACHStatus"].ToString(),
                        BankName = rdr["BankName"].ToString(),
                        BusinessDBAName = rdr["BusinessDBAName"].ToString(),
                        FeeDescription = rdr["FeeDescription"].ToString(),
                        SalesPartnerDBA = rdr["SalesPartnerDBA"].ToString(),
                        SettlePlatformMID = rdr["SettlePlatformMID"].ToString(),
                        AchTransID = rdr["AchTransID"].ToString(),
                        SalesPartnerID = int.Parse(rdr["SalesPartnerID"].ToString()),
                        MerchantID = int.Parse(rdr["MerchantID"].ToString()),
                        TotalRecordCount = int.Parse(rdr["TotalRecordCount"].ToString())
                    };

                    decimal.TryParse(rdr["InvoiceAmount"].ToString(), out moMoney);
                    row.InvAmt = string.Format("{0:0.00}", moMoney);

                    if (DateTime.TryParse(rdr["ACHBillDate"].ToString(), out date))
                    {
                        row.BillDate = date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        row.BillDate = "";
                    }

                    rows.Add(row);
                }

                rdr.Close();
            }

            return rows;
        }

        public static DataSet GetDSInvoiceHistory(Hashtable prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchInvoiceHistory";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            return ds;
        }

        public static List<InvoiceConfirmRow> GetDatatableInvoiceConfirm(string sortExpression, bool sortDesc, int pageSize, int curDisplayIndex, DateTime startDate, DateTime endDate)
        {
            List<InvoiceConfirmRow> rows = new List<InvoiceConfirmRow>();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchInvoiceConfirmation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@DisplayStartIndex", curDisplayIndex);
            cmd.Parameters.AddWithValue("@SortOrder", sortExpression);
            cmd.Parameters.AddWithValue("@SortDirection", sortDesc.GetHashCode());

            if (startDate != DateTime.MinValue)
            {
                cmd.Parameters.AddWithValue("@CreatedStartDate", startDate);
            }

            cmd.Parameters.AddWithValue("@CreatedEndDate", endDate);

            DateTime date;
            decimal dec;

            using (SqlDataReader rdr = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild()))
            {
                while (rdr.Read())
                {
                    InvoiceConfirmRow row = new InvoiceConfirmRow()
                    {
                        CreatedBy = rdr["BillBy"].ToString(),
                        TotalRecordCount = int.Parse(rdr["TotalRecordCount"].ToString()),
                        Product = rdr["Product"].ToString(),
                        AmountCollected = rdr["PercentClosed"].ToString(),
                        MonthCreated = rdr["ACHMonthDate"].ToString()
                    };

                    decimal.TryParse(rdr["TotalClosed"].ToString(), out dec);
                    row.AmountClosed = string.Format("{0:0.00}", dec);

                    decimal.TryParse(rdr["TotalRejected"].ToString(), out dec);
                    row.AmountRejected = string.Format("{0:0.00}", dec);

                    decimal.TryParse(rdr["TotalAmount"].ToString(), out dec);
                    row.Amount = string.Format("{0:0.00}", dec);

                    if (DateTime.TryParse(rdr["ACHDate"].ToString(), out date))
                    {
                        row.DateCreated = date.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        row.DateCreated = "";
                    }

                    rows.Add(row);
                }

                rdr.Close();
            }

            return rows;
        }

        public static DataSet GetDSInvoiceConfirm(string sortExpression, bool sortDesc, DateTime startDate, DateTime endDate)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_SearchInvoiceConfirmation";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PageSize", -1);
            cmd.Parameters.AddWithValue("@DisplayStartIndex", 0);
            cmd.Parameters.AddWithValue("@SortOrder", sortExpression);
            cmd.Parameters.AddWithValue("@SortDirection", sortDesc.GetHashCode());

            if (startDate != DateTime.MinValue)
            {
                cmd.Parameters.AddWithValue("@CreatedStartDate", startDate);
            }

            cmd.Parameters.AddWithValue("@CreatedEndDate", endDate);

            DataSet ds = DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());

            return ds;
        }
    }

    internal class InvoiceReportRow
    {
        public InvoiceReportRow()
        {

        }

        public int MerchantID
        {
            get;
            set;
        }

        public string SettlePlatformMID
        {
            get;
            set;
        }

        public string BusinessDBAName
        {
            get;
            set;
        }

        public string DateEnrolled
        {
            get;
            set;
        }

        public string LastBillDate
        {
            get;
            set;
        }

        public string FeeDescription
        {
            get;
            set;
        }

        public string ACHDescriptor
        {
            get;
            set;
        }

        public string BillDate
        {
            get;
            set;
        }

        public string InvoiceAmt
        {
            get;
            set;
        }

        public int InvoiceQuantity
        {
            get;
            set;
        }

        public string InvAmt
        {
            get;
            set;
        }

        public int TotalRecordCount
        {
            get;
            set;
        }

        public long RunID
        {
            get;
            set;
        }

        public long InvoiceDetailID
        {
            get;
            set;
        }

        public string SalesPartnerDBA
        {
            get;
            set;
        }

        public string SalesPartnerID
        {
            get;
            set;
        }

        public string BankName
        {
            get;
            set;
        }

        public string MerchantStatus
        {
            get;
            set;
        }
    }

    internal class AnnualFeesRow
    {
        public Int64 ZID { get; set; }
        public string DBA { get; set; }
        public string AnnualFee { get; set; }
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public string Status { get; set; }
        public string Bank { get; set; }
        public string Month { get; set; }
        public int TotalRecordCount { get; set; }
    }

    internal class PCINAFRow
    {
        public Int64 Zid { get; set; }
        public string Mid { get; set; }
        public string DBA { get; set; }
        public string OfficeName { get; set; }
        public string Amount { get; set; }
        public string Partner { get; set; }
        public int MerchantLevel { get; set; }
        public int TransactionCount { get; set; }
        public decimal ProcessingVolume { get; set; }
        public string CP_CNP { get; set; }
        public string Bank { get; set; }        
        public int NumberOfTimeBilled { get; set; }
        public string LastBillingDate { get; set; }
        public string Status { get; set; }
        public string Month { get; set; }
    }

    internal class PCIAnnualRow
    {
        public string ClientID { get; set; }
        public string MID { get; set; }
        public string DBA { get; set; }
        public Int64 PartnerID { get; set; }                
        public string PartnerDBA { get; set; }
        public string SpecialRequest { get; set; }
        public int AchID { get; set; }
        public int MerchantID { get; set; }
        public string DescDate { get; set; }
        public string SECC { get; set; }
        public string TransDate { get; set; }
        public string Amount { get; set; }
        public string TransType { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public string DateAdded { get; set; }
        public string NextProcessDate { get; set; }
        public string RefID { get; set; }
        public string BusinessLegalName { get; set; }
        public string Status { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public string CUApprovedDateCC { get; set; }
        public string CUApprovedDateAch { get; set; }
    }

    internal class InvoiceHistoryRow
    {
        public InvoiceHistoryRow()
        {

        }

        public int MerchantID
        {
            get;
            set;
        }

        public string SettlePlatformMID
        {
            get;
            set;
        }

        public string BusinessDBAName
        {
            get;
            set;
        }

        public string FeeDescription
        {
            get;
            set;
        }

        public string AchTransID
        {
            get;
            set;
        }

        public string ACHDescriptor
        {
            get;
            set;
        }

        public string ACHStatus
        {
            get;
            set;
        }

        public string BillDate
        {
            get;
            set;
        }

        public string InvAmt
        {
            get;
            set;
        }

        public int TotalRecordCount
        {
            get;
            set;
        }

        public string SalesPartnerDBA
        {
            get;
            set;
        }

        public int SalesPartnerID
        {
            get;
            set;
        }

        public string BankName
        {
            get;
            set;
        }


    }

    internal class InvoiceConfirmRow
    {
        public InvoiceConfirmRow()
        {

        }

        public string DateCreated
        {
            get;
            set;
        }

        public string MonthCreated
        {
            get;
            set;
        }

        public string Product
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public string Amount
        {
            get;
            set;
        }

        public string AmountClosed
        {
            get;
            set;
        }

        public string AmountRejected
        {
            get;
            set;
        }

        public string AmountCollected
        {
            get;
            set;
        }

        public int TotalRecordCount
        {
            get;
            set;
        }
    }
}