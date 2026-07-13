using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using ZeusWeb.Class;
using PaymentXP.DataObjects;

namespace ZeusWeb.ajax
{
    public partial class InvoiceFees : System.Web.UI.Page
    {

        //max json length for serializing data (64MB)
        private const int MAX_JSON_LEN = 67108864;

        protected void Page_Load(object sender, EventArgs e)
        {
            string response = "";

            try
            {
                string command = Request["command"].ToString();

                switch (command)
                {
                    case "invoicereport":
                        this.GetInvoiceReport();
                        break;

                    case "processach":
                        this.ProcessInvoiceACH();
                        break;

                    case "exportreport":
                        this.ExportInvoiceReport();
                        break;

                    case "getinvoicedetail":
                        this.GetMerchantInvoiceDetails();
                        break;

                    case"updateinvoicefee":
                        this.UpdateMerchantInvoiceDetails();
                        break;

                    case "invoicesearch":
                        this.SearchInvoiceHistory();
                        break;

                    case "exporthistory":
                        this.ExportInvoiceHistory();
                        break;

                    case "invoiceconfirm":
                        this.SearchInvoiceConfirmation();
                        break;

                    case "exportconfirmation":
                        this.ExportInvoiceConfirmation();
                        break;

                    default:
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                //suppress ThreadAbortException because it will be raised for export to excel features where
                //response.end gets called. we suppress the exception because it's really not an error
                //and is expected to get thrown when Response.End gets called
            }
            catch (Exception ex)
            {
                response = @"{""error"": ""Your request cannot be executed: A server error has occurred, please contact IT.""}";

                Response.ContentType = "application/json";
                Response.Write(response);
            }

            Response.End();
        }

        #region Invoice Report Methods

        private void GetInvoiceReport()
        {
            StringBuilder sb = new StringBuilder();

            int categoryId = int.Parse(Request["categoryId"].ToString());

            if (categoryId == -1)
            {
                sb.Append(@"{""sEcho"":0,""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[]}");
            }
            else
            {
                List<InvoiceReportRow> data = DataTablePaging.GetDatatableInvoice("", false, -1, 1, categoryId);

                int totalDisplay = data.Count;
                if (totalDisplay == 0)
                {
                    sb.AppendFormat(@"{{""sEcho"":0,""iTotalRecords"":{0},""iTotalDisplayRecords"":{0},""aaData"":[]}}", totalDisplay);
                }
                else
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    jss.MaxJsonLength = MAX_JSON_LEN;
                    string output = jss.Serialize(data);

                    sb.AppendFormat(@"{{""sEcho"":0, ""iTotalRecords"":{0},""iTotalDisplayRecords"":{0}, ""billDate"": ""{2}"", ""aaData"":{1}}}", data[0].TotalRecordCount, output, data[0].BillDate);
                }
            }

            Response.ContentType = "application/json";
            Response.Write(sb.ToString());
        }

        private void ExportInvoiceReport()
        {
            int categoryId = int.Parse(Request["categoryId"].ToString());

            DataSet ds = DataTablePaging.GetDSInvoiceReport("", false, categoryId);

            this.grdInvoice.DataSource = ds;
            this.grdInvoice.DataBind();

            FormHandler.Export2Excel("InvoiceReport.xls", this.grdInvoice);
        }

        private void GetMerchantInvoiceDetails()
        {
            string response = "";

            try
            {
                long invoiceDetailId = long.Parse(Request["invoicdetailid"].ToString());

                response = DataTablePaging.GetMerchantInvoiceDetails(invoiceDetailId);
            }
            catch (Exception)
            {
                response = @"{""success"": ""false"", ""error"": ""Your request cannot be executed: A server error has occurred, please contact IT.""}";
            }

            Response.ContentType = "application/json";
            Response.Write(response);
        }

        private void UpdateMerchantInvoiceDetails()
        {
            string response = "";

            try
            {
                long invoiceDetailId;
                int merchantId;
                decimal billAmt;
                decimal feeAmt;
                int quantity;

                if (!long.TryParse(Request["invoicdetailid"].ToString(), out invoiceDetailId))
                {
                    throw new FormatException("Invalid invoice detail ID provided.");
                }

                if (!int.TryParse(Request["merchantid"].ToString(), out merchantId))
                {
                    throw new FormatException("Invalid merchant ID provided.");
                }

                if (!int.TryParse(Request["quantity"].ToString(), out quantity))
                {
                    throw new FormatException("Invalid invoice quantity provided.");
                }

                if (quantity < 0)
                {
                    throw new FormatException("Invoice quantity must be a non-negative value.");
                }

                if (!decimal.TryParse(Request["billAmount"].ToString(), out billAmt))
                {
                    throw new FormatException("Invalid Invoice Fee Amount provided, fee must be a numeric value.");
                }

                if (!decimal.TryParse(Request["feeAmount"].ToString(), out feeAmt))
                {
                    throw new FormatException("Invalid Product Rate provided, fee must be a numeric value.");
                }

                if (feeAmt < 0)
                {
                    throw new FormatException("Product Rate must be a non-negative value.");
                }

                DataTablePaging.UpdateMerchantInvoiceDetails(invoiceDetailId, merchantId, billAmt, feeAmt, quantity);

                response = @"{""success"": ""true"", ""message"": ""Merchant product and invoice fee updated.""}";
            }
            catch (FormatException ex)
            {
                response = string.Format(@"{{""success"": ""false"", ""message"": ""{0}""}}", ex.Message);
            }
            catch (Exception ex)
            {
                response = @"{""success"": ""false"", ""message"": ""Your request cannot be executed: A server error has occurred, please contact IT.""}";
            }

            Response.ContentType = "application/json";
            Response.Write(response);
        }

        private void ProcessInvoiceACH()
        {
            string response = "";

            try
            {
                string achInvoices = Request["invoiceach"].ToString();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var invoices = serializer.Deserialize<List<InvoiceACH>>(achInvoices);

                DataTable table = new DataTable();
                table.Columns.Add("InvoiceDetailID", typeof(long));
                table.Columns.Add("RunID", typeof(long));
                table.Columns.Add("InvoiceStatusID", typeof(int));
                table.Columns.Add("BillDate", typeof(DateTime));
                table.Columns.Add("InvoiceAmount", typeof(decimal));

                foreach (var obj in invoices)
                {
                    DataRow row = table.NewRow();

                    row["InvoiceDetailID"] = obj.InvoiceDetailID;
                    row["RunID"] = obj.RunID;
                    row["InvoiceStatusID"] = obj.InvoiceStatusID;
                    row["BillDate"] = obj.BillDate;
                    row["InvoiceAmount"] = obj.Amount;

                    table.Rows.Add(row);
                }

                DataTablePaging.ACHInvoice(table, UserSessions.CurrentUser.UserName);

                response = @"{""success"": ""true"", ""message"":""ACH for invoices successfully processed.""}";
            }
            catch (Exception)
            {
                response = @"{""success"": ""false"", ""message"":""ACH for invoices failed to process.""}";
            }

            Response.ContentType = "application/json";
            Response.Write(response);
        }

        internal class InvoiceACH
        {
            public InvoiceACH()
            {

            }

            public string InvoiceDetailID
            {
                get;
                set;
            }

            public string RunID
            {
                get;
                set;
            }

            public string InvoiceStatusID
            {
                get;
                set;
            }

            public string BillDate
            {
                get;
                set;
            }

            public string InvoiceAmount
            {
                get;
                set;
            }

            public decimal Amount
            {
                get
                {
                    return decimal.Parse(this.InvoiceAmount, NumberStyles.AllowCurrencySymbol | NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat);
                }
            }
        }

        #endregion

        #region Invoice Search Methods

        private void SearchInvoiceHistory()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (Request["sEcho"] == "1")
                {
                    Response.ContentType = "application/json";
                    Response.Write(@"{""sEcho"":1,""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[],""sortDir"": ""asc"", ""sortExpr"":""""}");

                    return;
                }

                Hashtable prms = this.GetInvoiceHistorySearchParams();
                string sortOrder = this.GetInvoiceHistorySortExpr();

                prms["@DisplayStartIndex"] = Request["iDisplayStart"].ToString();
                prms["@PageSize"] = Request["iDisplayLength"].ToString();
                prms["@SortOrder"] = sortOrder;
                prms["@SortDirection"] = Request["sSortDir_0"].ToString() == "asc" ? 1 : 0;

                List<InvoiceHistoryRow> data = DataTablePaging.GetDatatableInvoiceHistory(prms);

                int totalDisplay = data.Count;
                if (totalDisplay == 0)
                {
                    sb.AppendFormat(@"{{""sEcho"":{1},""iTotalRecords"":{0},""iTotalDisplayRecords"":{0},""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}}", totalDisplay, Request["sEcho"]);
                }
                else
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string output = jss.Serialize(data);

                    sb.AppendFormat(@"{{""sEcho"":{2}, ""iTotalRecords"":{0},""iTotalDisplayRecords"":{0}, ""aaData"":{1}, ""sortDir"": ""{3}"", ""sortExpr"":""{4}""}}", data[0].TotalRecordCount, output, Request["sEcho"], Request["sSortDir_0"], sortOrder);
                }
            }
            catch (FormatException ex)
            {
                sb.AppendFormat(@"{{""success"": ""false"", ""message"":""{0}"", ""sEcho"":{1},""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}}", ex.Message, Request["sEcho"]);
            }
            catch (Exception)
            {
                sb.AppendFormat(@"{{""success"": ""false"", ""message"":""An error has occurred while querying invoice history. Please notify IT if errors persist."", ""sEcho"":{0},""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}}", Request["sEcho"]);
            }

            Response.ContentType = "application/json";
            Response.Write(sb.ToString());
        }

        private void ExportInvoiceHistory()
        {
            Hashtable prms = this.GetInvoiceHistorySearchParams();
            
            prms["@PageSize"] = -1;
            prms["@SortOrder"] = Request["sortExpr"];
            prms["@SortDirection"] = Request["sortDir"].ToString() == "asc" ? 1 : 0;

            DataSet ds = DataTablePaging.GetDSInvoiceHistory(prms);

            this.grdInvoiceHistory.DataSource = ds;
            this.grdInvoiceHistory.DataBind();

            FormHandler.Export2Excel("InvoiceBillingHistory.xls", this.grdInvoiceHistory);
        }

        private string GetInvoiceHistorySortExpr()
        {
            int sortCol = int.Parse(Request["iSortCol_0"].ToString());

            switch (sortCol)
            {
                case 0:
                    return "MERCHANTID";

                case 1:
                    return "SETTLEPLATFORMMID";

                case 2:
                    return "BUSINESSDBANAME";

                case 3:
                    return "SALESPARTNERDBA";

                case 4:
                    return "SALESPARTNERID";

                case 5:
                    return "BANKNAME";

                case 6:
                    return "FEEDESCRIPTION";

                case 7:
                    return "ACHTRANSID";

                case 8:
                    return "ACHDESCRIPTOR";

                case 9:
                    return "ACHSTATUS";

                case 10:
                    return "ACHDATE";

                case 11:
                    return "INVOICEAMOUNT";

                default:
                    return "";
            }
        }

        private Hashtable GetInvoiceHistorySearchParams()
        {
            Hashtable prms = new Hashtable();

            DateTime dt;
            int id;
            decimal dec;
            string sValue = Request["startDate"].ToString();

            if (!string.IsNullOrWhiteSpace(sValue))
            {
                if (DateTime.TryParse(sValue, out dt))
                {
                    prms["@BillStartDate"] = dt.ToString("MM/dd/yyyy");
                }
                else
                {
                    throw new FormatException("Invalid start billing date format.");
                }
            }


            sValue = Request["endDate"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue))
            {
                if (DateTime.TryParse(sValue, out dt))
                {
                    prms["@BillEndDate"] = dt.ToString("MM/dd/yyyy");
                }
                else
                {
                    throw new FormatException("Invalid end billing date format.");
                }
            }

            sValue = Request["merchantId"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue))
            {
                if (int.TryParse(sValue, out id))
                {
                    prms["@MerchantID"] = id;
                }
                else
                {
                    throw new FormatException("Merchant ID must be a number.");
                }
            }

            sValue = Request["mid"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue))
            {
                prms["@MID"] = sValue;
            }

            int.TryParse(Request["amountOperator"].ToString(), out id);
            prms["@AmountOperator"] = id;

            sValue = Request["amount"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue) && decimal.TryParse(sValue, out dec))
            {
                if (decimal.TryParse(sValue, out dec))
                {
                    prms["@Amount"] = dec;
                }
                else
                {
                    throw new FormatException("Amount must be a number.");
                }
            }


            sValue = Request["achDescriptor"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue))
            {
                prms["@Descriptor"] = sValue;
            }

            if (int.TryParse(Request["productId"].ToString(), out id) && id > 0)
            {
                prms["@CategoryID"] = id;
            }

            if (int.TryParse(Request["achStatus"].ToString(), out id) && id > 0)
            {
                prms["@ACHStatusID"] = id;
            }

            return prms;
        }

        #endregion

        #region Invoice Confirmation Methods

        private void SearchInvoiceConfirmation()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (Request["sEcho"] == "1")
                {
                    Response.ContentType = "application/json";
                    Response.Write(@"{""sEcho"":1,""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}");
                    return;
                }

                DateTime startDate = DateTime.MinValue;
                DateTime endDate = DateTime.MaxValue;

                string sValue = Request["startDate"].ToString();
                if (!string.IsNullOrWhiteSpace(sValue))
                {
                    if (!DateTime.TryParse(sValue, out startDate))
                    {
                        throw new FormatException("Invalid starting date created format.");
                    }
                }

                sValue = Request["endDate"].ToString();
                if (!string.IsNullOrWhiteSpace(sValue))
                {
                    if (!DateTime.TryParse(sValue, out endDate))
                    {
                        throw new FormatException("Invalid ending date created format.");
                    }
                }

                int curIndex = int.Parse(Request["iDisplayStart"].ToString());
                int displayLength = int.Parse(Request["iDisplayLength"].ToString());
                string sortOrder = this.GetInvoiceConfSortExpr();
                bool sortdesc = Request["sSortDir_0"].ToString() != "asc";

                List<InvoiceConfirmRow> data = DataTablePaging.GetDatatableInvoiceConfirm(sortOrder, sortdesc, displayLength, curIndex, startDate, endDate);

                int totalDisplay = data.Count;
                if (totalDisplay == 0)
                {
                    sb.AppendFormat(@"{{""sEcho"":{1},""iTotalRecords"":{0},""iTotalDisplayRecords"":{0},""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}}", totalDisplay, Request["sEcho"]);
                }
                else
                {
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    string output = jss.Serialize(data);

                    sb.AppendFormat(@"{{""sEcho"":{2}, ""iTotalRecords"":{0},""iTotalDisplayRecords"":{0}, ""aaData"":{1}, ""sortDir"": ""{3}"", ""sortExpr"":""{4}""}}", data[0].TotalRecordCount, output, Request["sEcho"], Request["sSortDir_0"], sortOrder);
                }
            }
            catch (FormatException ex)
            {
                sb.AppendFormat(@"{{""success"": ""false"", ""message"":""{0}"", ""sEcho"":{1},""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}}", ex.Message, Request["sEcho"]);
            }
            catch (Exception ex)
            {
                sb.AppendFormat(@"{{""success"": ""false"", ""message"":""An error has occurred while querying invoice confirmation. Please notify IT if errors persist."", ""sEcho"":{0},""iTotalRecords"":0,""iTotalDisplayRecords"":0,""aaData"":[], ""sortDir"": ""asc"", ""sortExpr"":""""}}", Request["sEcho"]);
            }

            Response.ContentType = "application/json";
            Response.Write(sb.ToString());
        }

        private void ExportInvoiceConfirmation()
        {
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;

            string sValue = Request["startDate"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue))
            {
                if (!DateTime.TryParse(sValue, out startDate))
                {
                    throw new FormatException("Invalid starting date created format.");
                }
            }

            sValue = Request["endDate"].ToString();
            if (!string.IsNullOrWhiteSpace(sValue))
            {
                if (!DateTime.TryParse(sValue, out endDate))
                {
                    throw new FormatException("Invalid ending date created format.");
                }
            }

            DataSet ds = DataTablePaging.GetDSInvoiceConfirm(Request["sortExpr"].ToString(), Request["sortDir"].ToString() == "desc", startDate, endDate);

            this.grdInvoiceConfirm.DataSource = ds;
            this.grdInvoiceConfirm.DataBind();

            FormHandler.Export2Excel("InvoiceConfirmation.xls", this.grdInvoiceConfirm);
        }

        private string GetInvoiceConfSortExpr()
        {
            int sortCol = int.Parse(Request["iSortCol_0"].ToString());

            switch (sortCol)
            {
                case 0:
                    return "DATECREATED";

                case 1:
                    return "DATECREATED";

                case 2:
                    return "PRODUCTNAME";

                case 3:
                    return "CREATEDBY";

                case 4:
                    return "AMOUNT";

                case 5:
                    return "AMOUNTCLOSED";

                case 6:
                    return "AMOUNTREJECTED";

                case 7:
                    return "AMOUNTCOLLECTED";

                default:
                    return "";
            }
        }

        #endregion

        //the commented out code below was made for testing product invoicing and should not be uncommented unless
        //we want to test the product invoice sps. DO NOT DEPLOY THIS CODE TO PRODUCTION
        //private void ExecInvoiceAction()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    int actionId = int.Parse(Request["actionId"].ToString());

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand();

        //        switch (actionId)
        //        {
        //            case 1: //CBMS

        //                cmd.CommandText = "sp_Invoice_CBMS";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""CBMS invoices generated.""}"); 

        //                break;

        //            case 2: //CBMS Plus

        //                cmd.CommandText = "sp_Invoice_CBMSPlus";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""CBMS+ invoices generated""}"); 

        //                break;

        //            case 3: //FraudXP

        //                cmd.CommandText = "sp_Invoice_FraudXP";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""FraudXP invoice generated""}");
 
        //                break;

        //            case 4: //PXP

        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""PaymentXP invoice generated""}"); 

        //                break;

        //            case 5: //Advanced Reporting

        //                cmd.CommandText = "sp_Invoice_AdvReports";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""Advanced Reporting invoice generated""}"); 

        //                break;

        //            case 6: //Quick Books Plugin

        //                cmd.CommandText = "sp_Invoice_QuickBooks";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""Quick Books Plugin invoice generated""}"); 

        //                break;

        //            case 0: //Delete Invoices

        //                cmd.CommandText = "delete from invoicedetail where invoiceid > 34949" + Environment.NewLine + " delete from invoice where invoiceid > 34949";
        //                cmd.CommandType = CommandType.Text;
        //                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""Invoices deleted.""}"); 
        //                break;

        //            default:
        //                sb.AppendLine(@"{""success"": ""true"", ""message"": ""No invoice action to execute.""}"); 
        //                break;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        sb.AppendLine(@"{""success"": ""false"", ""message"": ""Your request cannot be executed: A server error has occurred, please contact IT.""}"); 
        //    }

        //    Response.ContentType = "application/json";
        //    Response.Write(sb.ToString());
        //}
    }
}