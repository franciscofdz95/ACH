using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucUploadADR060 : System.Web.UI.UserControl
    {
        public delegate void EventClickUploadComplete(eRDBReserveUploadTypeID eUT, int upload_id, int total_imported);
        public event EventClickUploadComplete event_click_uploadcomplete;

        protected void Page_Load(object sender, EventArgs e)
        {
            wucUploadGrid1.UploadType = PaymentXP.BusinessObjects.eRDBReserveUploadTypeID.ADR060;

            this.PreRender += wucUploadADR060_PreRender;

            if (!IsPostBack)
            {
                wucUploadGrid1.UploadGrid.Columns[2].HeaderText = "MD050 Imported";
            }
        }

        protected void wucUploadADR060_PreRender(object sender, EventArgs e)
        {
            // display a warning if today's date does not match yesterday's report date.
            DateTime dt = DataReserve.GetDivertLastReportDate();
            pnlWarning.Visible = (DateTime.Today.AddDays(-1).ToShortDateString() != dt.ToShortDateString()) ? true : false;

            litDivertLastReportDate.Text = dt.AddDays(1).ToShortDateString();

            
        }

        //private MerchantApp GetMerchantAppByMID(string mid)
        //{
        //    Hashtable prms = new Hashtable();
        //    prms.Add("@SettlePlatformMid", mid);
        //    return DataMerchantApp.GetInstance().FillMerchantApp(prms);
        //}

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //code updated by Ksingh for handle error while uploading non excel and wrong template file
            try
            {

                if (FileUpload1.HasFile && !string.IsNullOrWhiteSpace(FileUpload1.FileName) && FileUpload1.FileBytes.Length > 0)
                {

                    if (FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.') + 1).ToUpper() == "XLSX" || FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.') + 1).ToUpper() == "XLS")
                    {
                        int current_row = 0;
                        int insert_count = 0;
                        int bad_row_count = 0;
                        int max_bad_row = 4;
                        int md050_row = 0;

                        // if we encounter a report date, we add it here.
                        List<string> liReportDate = new List<string>();

                        RDBUpload objU = new RDBUpload()
                        {
                            Filename = FileUpload1.FileName,
                            UploadedBy = UserSessions.CurrentUser.UserName,
                            UploadTypeID = eRDBReserveUploadTypeID.ADR060
                        };

                        objU = DataReserve.InsertRDBUpload(objU);

                        if (objU != null && objU.UploadID > 0)
                        {

                            using (var package = new ExcelPackage(FileUpload1.FileContent))
                            {
                                ExcelWorkbook workBook = package.Workbook;

                                if (workBook != null)
                                {

                                    if (workBook.Worksheets.Count > 0)
                                    {
                                        ExcelWorksheet ws = workBook.Worksheets.First();

                                        int start_row = 1;
                                        current_row = start_row;
                                        bool IsValidRow = false;
                                        bool HasInsertedOnce = false;

                                        int col_MerchantName = 1;
                                        int col_MerchantNum = 2;
                                        int col_FeedDate = 3;
                                        int col_Portfolio = 4;
                                        int col_ISOName = 5;
                                        int col_Source = 6;
                                        int col_System = 7;
                                        int col_Prin = 8;
                                        int col_DebitAmt = 9;
                                        int col_CreditAmt = 10;
                                        int col_RejectedDebit = 11;
                                        int col_RejectedCredit = 12;
                                        int col_Status = 13;

                                        do
                                        {
                                            if (ws.Cells[current_row, 1].Value != null)
                                            {
                                                string first = CommonUtility.Util.if_s(ws.Cells[current_row, 1].Value);

                                                // from observation, the first row is blank.
                                                // the 2nd row is the header.
                                                // and the 3rd row is the start of the data.
                                                if (!string.IsNullOrWhiteSpace(first) && first != "Merchant Name")
                                                {

                                                    // TODO: wrap this around a try/catch and send an email if an import exception occurs.


                                                    DateTime dtRD = new DateTime();

                                                    if (DateTime.TryParse(CommonUtility.Util.if_s(ws.Cells[current_row, col_FeedDate].Value), out dtRD))
                                                    {
                                                        string source = CommonUtility.Util.if_s(ws.Cells[current_row, col_Source].Value);

                                                        Hashtable prms = new Hashtable()
                                                {
                                                    {"@DBA", CommonUtility.Util.if_s(ws.Cells[current_row, col_MerchantName].Value) }           
                                                    ,{"@MID",CommonUtility.Util.if_s(ws.Cells[current_row, col_MerchantNum].Value).Replace("'", "") }                       
                                                    ,{"@ReportDate", dtRD.ToShortDateString()}                
                                                    ,{"@Portfolio", CommonUtility.Util.if_s(ws.Cells[current_row, col_Portfolio].Value)}                 
                                                    ,{"@ISOName", CommonUtility.Util.if_s(ws.Cells[current_row, col_ISOName].Value)}                   
                                                    ,{"@Source", source}                
                                                    ,{"@Sys", CommonUtility.Util.if_s(ws.Cells[current_row, col_System].Value).Replace("'", "")}                       
                                                    ,{"@Prin", CommonUtility.Util.if_s(ws.Cells[current_row, col_Prin].Value).Replace("'", "")}                      
                                                    ,{"@DebitAmt",CommonUtility.Util.if_s(ws.Cells[current_row, col_DebitAmt].Value) }                  
                                                    ,{"@CreditAmt", CommonUtility.Util.if_s(ws.Cells[current_row, col_CreditAmt].Value)}                 
                                                    ,{"@RejectedDebit",CommonUtility.Util.if_s(ws.Cells[current_row, col_RejectedDebit].Value) }             
                                                    ,{"@RejectedCredit",CommonUtility.Util.if_s(ws.Cells[current_row, col_RejectedCredit].Value) }            
                                                    ,{"@Status",CommonUtility.Util.if_s(ws.Cells[current_row, col_Status].Value) }                    
                                                    ,{"@UploadFilename", FileUpload1.FileName }                    
                                                };

                                                        if (DataReserve.InsertADR060(prms) > 0)
                                                        {
                                                            insert_count++;
                                                            HasInsertedOnce = true;

                                                            if (!liReportDate.Contains(dtRD.ToShortDateString()))
                                                            {
                                                                liReportDate.Add(dtRD.ToShortDateString());
                                                            }

                                                            if (source.Trim().ToUpper().Equals("MD050"))
                                                            {
                                                                md050_row++;
                                                            }
                                                        }

                                                        IsValidRow = true;
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                bad_row_count++;

                                                IsValidRow = false;

                                                if (insert_count == 0 && HasInsertedOnce == false)
                                                {
                                                    IsValidRow = true;
                                                }
                                            }

                                            current_row++;

                                        } while (IsValidRow && bad_row_count < max_bad_row);
                                    }
                                }
                            }

                            //eluxa 10/07/2014: do not import diverts into RDB database for uploads. doing so would defeat the 
                            //purpose of the import button validation on the reserve queue. only write records to the AD060 table
                            //if (liReportDate.Count > 0)
                            //{
                            //    foreach (string strRD in liReportDate)
                            //    {
                            //        DataReserve.ImportRDB_Divert(DateTime.Parse(strRD));
                            //    }
                            //}

                            objU.TotalImported = insert_count;
                            objU.MD050Imported = md050_row;
                            objU.ReportDates = string.Join(",", liReportDate);

                            DataReserve.UpdateRDBUpload(objU);

                            if (this.event_click_uploadcomplete != null && objU.UploadID > 0)
                            {
                                this.event_click_uploadcomplete(eRDBReserveUploadTypeID.ADR060, objU.UploadID, insert_count);
                            }
                        }
                    }
                    else
                    {
                        pnlWarningFiletype.Visible = true;
                    }
                }
            }
            catch (Exception )
            {
                pnlWarningFiletype.Visible = true;
            }
        }

        public void RefreshGrid()
        {
            wucUploadGrid1.BindGrid();
            pnlWarning.Visible = (wucUploadGrid1.LastUploadedDate.ToShortDateString() != DateTime.Now.ToShortDateString()) ? true : false;
        }
    }
}