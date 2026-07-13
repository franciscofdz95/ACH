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
    public partial class wucUploadReserve : System.Web.UI.UserControl
    {
        public delegate void EventClickUploadComplete(eRDBReserveUploadTypeID eUT, int upload_id, int total_imported);
        public event EventClickUploadComplete event_click_uploadcomplete;

        protected void Page_Load(object sender, EventArgs e)
        {
            wucUploadGrid1.UploadType = PaymentXP.BusinessObjects.eRDBReserveUploadTypeID.ReservesHeldAtMeritus;
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
                    if(FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.') + 1).ToUpper() == "XLSX" || FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('.') + 1).ToUpper() == "XLS") 
                    {

                        int current_row = 0;
                        int import_count = 0;
                        List<string> liReportDate = new List<string>();

                        RDBUpload objU = new RDBUpload()
                        {
                            Filename = FileUpload1.FileName,
                            UploadedBy = UserSessions.CurrentUser.UserName,
                            UploadTypeID = eRDBReserveUploadTypeID.ReservesHeldAtMeritus
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

                                        int start_row = 2;
                                        current_row = start_row;
                                        bool IsValidRow = false;

                                        int col_reportdate = 8;
                                        int col_zid = 3;
                                        int col_amount = 7;
                                        int col_notes_type = 9;
                                        int col_notes_bank = 10;
                                        int col_notes_transid = 11;
                                        int col_notes_description = 12;

                                        do
                                        {
                                            if (ws.Cells[current_row, 1].Value != null && !string.IsNullOrWhiteSpace(ws.Cells[current_row, 1].Value.ToString()))
                                            {
                                                //long serialDate = long.Parse(ws.Cells[current_row, col_reportdate].Value.ToString());
                                                //DateTime dtReportDate = DateTime.FromOADate(serialDate);

                                                DateTime dtReportDate;

                                                string zid = CommonUtility.Util.if_s(ws.Cells[current_row, col_zid].Value);
                                                bool date_good = DateTime.TryParse(ws.Cells[current_row, col_reportdate].Value.ToString(), out dtReportDate);
                                                decimal amount = CommonUtility.Util.if_dec(ws.Cells[current_row, col_amount].Value, 0);
                                                string bank_notes = string.Format("Type:{0}, Bank Acct:{1}, TransID/RefNo:{2}, Desc:{3}",
                                                            CommonUtility.Util.if_s(ws.Cells[current_row, col_notes_type].Value),
                                                            CommonUtility.Util.if_s(ws.Cells[current_row, col_notes_bank].Value),
                                                            CommonUtility.Util.if_s(ws.Cells[current_row, col_notes_transid].Value),
                                                            CommonUtility.Util.if_s(ws.Cells[current_row, col_notes_description].Value));

                                                if (!string.IsNullOrWhiteSpace(zid) && date_good)
                                                {
                                                    IsValidRow = true;

                                                    MerchantApp objMA = DataMerchantApp.GetInstance().FillMerchantApp(new Hashtable() { { "ID", zid } });


                                                    /// TEST THIS!!!!!!!!!!!!!!!!!!

                                                    // maybe we should add upload ID to RDBReserve, RDBRelease, RDBReject. create no ADR060 table?


                                                    if (amount > 0)
                                                    {
                                                        // deposit

                                                        RDBReserve item = new RDBReserve();
                                                        item.ReportDate = dtReportDate;
                                                        item.ZID = CommonUtility.Util.if_i(objMA.ID, 0);
                                                        item.Amount = amount;
                                                        item.Notes = bank_notes;
                                                        item.BankID = eRDBBank.ReservesHeldAtMeritus;
                                                        item.Reserve = amount;
                                                        item.ReserveSourceID = eRDBReserveSourceID.ReserveDeposit;
                                                        item.MethodID = eRDBTransactionMethod.ACH;      // assumption. not always an ach. sometimes the merchant will wire. the description will have it.

                                                        // insert it into the journal as well.
                                                        if (DataReserve.InsertRDBReserve(item) > 0 && item.ReserveID > 0)
                                                        {
                                                            // insert into journal
                                                            DataReserve.InsertRDBJournalReserveTrans(item.ReserveID, item.ZID, CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0), DateTime.MinValue);

                                                            // commit it
                                                            DataReserve.InsertRDBJournalReserveTrans(item.ReserveID, item.ZID, CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0), DateTime.Now);
                                                            import_count++;

                                                            if (!liReportDate.Contains(dtReportDate.ToShortDateString()))
                                                            {
                                                                liReportDate.Add(dtReportDate.ToShortDateString());
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // release

                                                        RDBRelease objRel = new RDBRelease();
                                                        objRel.Amount = amount;
                                                        objRel.BankID = eRDBBank.ReservesHeldAtMeritus;
                                                        objRel.BankNotes = bank_notes;
                                                        objRel.InternalNotes = bank_notes;
                                                        objRel.MethodID = eRDBTransactionMethod.NotSet;
                                                        objRel.ZID = CommonUtility.Util.if_i(zid, 0);
                                                        objRel.ReportDate = dtReportDate;
                                                        objRel.TransTypeID = eRDBTransactionType.ReservesHeldAtMeritus;
                                                        objRel.ReserveTypeID = eRDBReserveType.Reserve;
                                                        objRel.MethodID = eRDBTransactionMethod.ACH;        // assumption
                                                        objRel.CreatedUserID = CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0);

                                                        DataReserve.InsertRDBRelease(objRel);

                                                        if (objRel.ReleaseID > 0)
                                                        {
                                                            // automatically approve.
                                                            objRel.ApprovedUserID = CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0);
                                                            objRel.DateApproved = DateTime.Now;
                                                            DataReserve.UpdateRDBRelease(objRel);

                                                            // insert into journal
                                                            DataReserve.InsertRDBJournalReleaseTrans(objRel, DateTime.MinValue);

                                                            // commit it.
                                                            DataReserve.InsertRDBJournalReleaseTrans(objRel, DateTime.Now);
                                                            import_count++;

                                                            if (!liReportDate.Contains(dtReportDate.ToShortDateString()))
                                                            {
                                                                liReportDate.Add(dtReportDate.ToShortDateString());
                                                            }
                                                        }


                                                    }

                                                }

                                            }
                                            else
                                            {
                                                IsValidRow = false;
                                            }

                                            current_row++;

                                        } while (IsValidRow);


                                    }
                                }

                            }

                            objU.TotalImported = import_count;
                            objU.ReportDates = string.Join(",", liReportDate);

                            DataReserve.UpdateRDBUpload(objU);

                            if (this.event_click_uploadcomplete != null && objU.UploadID > 0)
                            {
                                this.event_click_uploadcomplete(eRDBReserveUploadTypeID.ReservesHeldAtMeritus, objU.UploadID, import_count);
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