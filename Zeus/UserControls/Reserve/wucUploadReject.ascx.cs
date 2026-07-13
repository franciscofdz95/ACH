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
    public partial class wucUploadReject : System.Web.UI.UserControl
    {

        public delegate void EventClickUploadComplete(eRDBReserveUploadTypeID eUT, int upload_id, int total_imported);
        public event EventClickUploadComplete event_click_uploadcomplete;

        protected void Page_Load(object sender, EventArgs e)
        {
            wucUploadGrid1.UploadType = PaymentXP.BusinessObjects.eRDBReserveUploadTypeID.ACHRejects;
        }


        private MerchantApp GetMerchantAppByMID(string mid)
        {
            Hashtable prms = new Hashtable();
            prms.Add("@SettlePlatformMid", mid);
            return DataMerchantApp.GetInstance().FillMerchantApp(prms);
        }



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

                        List<string> liReportDate = new List<string>();

                        RDBUpload objU = new RDBUpload()
                        {
                            Filename = FileUpload1.FileName,
                            UploadedBy = UserSessions.CurrentUser.UserName,
                            UploadTypeID = eRDBReserveUploadTypeID.ACHRejects
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

                                        int col_reportdate = 3;
                                        int col_merchantid = 2;
                                        //int col_amount = 4;
                                        int col_aba = 4;
                                        int col_dda = 5;
                                        int col_code = 11;
                                        int col_memo = 12;

                                        int col_debit = 14;
                                        int col_credit = 15;
                                        int col_status = 18;

                                        do
                                        {
                                            if (ws.Cells[current_row, 1].Value != null && !string.IsNullOrWhiteSpace(ws.Cells[current_row, 1].Value.ToString()))
                                            {

                                                if (ws.Cells[current_row, col_status].Value.ToString().ToLower() == "accepted to cars")
                                                {
                                                    //long serialDate = long.Parse(ws.Cells[current_row, col_reportdate].Value.ToString());

                                                    DateTime dtReportDate;
                                                    if (DateTime.TryParse(ws.Cells[current_row, col_reportdate].Value.ToString(), out dtReportDate))
                                                    {
                                                        IsValidRow = true;

                                                        string mid = CommonUtility.Util.if_s(ws.Cells[current_row, col_merchantid].Value).Replace("'", "").Trim();

                                                        MerchantApp objMA = this.GetMerchantAppByMID(mid);

                                                        if (objMA != null)
                                                        {
                                                            RDBReject item = new RDBReject();

                                                            item.ReportDate = dtReportDate;
                                                            item.ZID = CommonUtility.Util.if_i(objMA.ID, 0);
                                                            item.DDA = CommonUtility.Util.if_s(ws.Cells[current_row, col_dda].Value);
                                                            item.ABA = CommonUtility.Util.if_s(ws.Cells[current_row, col_aba].Value);
                                                            item.Amount = (CommonUtility.Util.if_dec(ws.Cells[current_row, col_debit].Value, 0) - CommonUtility.Util.if_dec(ws.Cells[current_row, col_credit].Value, 0)) * -1;
                                                            item.Code = CommonUtility.Util.if_s(ws.Cells[current_row, col_code].Value);
                                                            item.Memo = CommonUtility.Util.if_s(ws.Cells[current_row, col_memo].Value);


                                                            // insert it into the journal as well.
                                                            if (DataReserve.InsertRDBReject(item) > 0 && item.RejectID > 0)
                                                            {
                                                                DataReserve.InsertRDBJournalRejectTrans(item, DateTime.Now);
                                                                insert_count++;
                                                                HasInsertedOnce = true;

                                                                if (!liReportDate.Contains(dtReportDate.ToShortDateString()))
                                                                {
                                                                    liReportDate.Add(dtReportDate.ToShortDateString());
                                                                }
                                                            }
                                                        }

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

                            objU.TotalImported = insert_count;
                            objU.ReportDates = string.Join(",", liReportDate);

                            DataReserve.UpdateRDBUpload(objU);

                            if (this.event_click_uploadcomplete != null && objU.UploadID > 0)
                            {
                                this.event_click_uploadcomplete(eRDBReserveUploadTypeID.ACHRejects, objU.UploadID, insert_count);
                            }
                        }

                    }
                    else
                    {
                        pnlWarning.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                pnlWarning.Visible = true;
            }
        }

        public void RefreshGrid()
        {
            wucUploadGrid1.BindGrid();
        }
    }
}