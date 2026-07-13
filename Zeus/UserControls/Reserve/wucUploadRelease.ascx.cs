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
    public partial class wucUploadRelease : System.Web.UI.UserControl
    {

        public delegate void EventClickUploadComplete(eRDBReserveUploadTypeID eUT, int upload_id, int total_imported);
        public event EventClickUploadComplete event_click_uploadcomplete;

        protected void Page_Load(object sender, EventArgs e)
        {
            wucUploadGrid1.UploadType = eRDBReserveUploadTypeID.Releases;
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
                            UploadTypeID = eRDBReserveUploadTypeID.Releases
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

                                        int start_row = 4;
                                        current_row = start_row;
                                        bool IsValidRow = false;

                                        int col_merchantid = 3;
                                        int col_banknotes = 6;
                                        int col_relamount_ops = 7;  // release to Meritus
                                        int col_relamount_merchant = 8;  // release to Meritus
                                        do
                                        {
                                        
                                            if (ws.Cells[current_row, 1].Value != null && !string.IsNullOrWhiteSpace(ws.Cells[current_row, 1].Value.ToString()))
                                            {
                                                long serialDate = long.Parse(ws.Cells[current_row, 1].Value.ToString());
                                                DateTime dt = DateTime.FromOADate(serialDate);
                                                IsValidRow = true;

                                                string mid = CommonUtility.Util.if_s(ws.Cells[current_row, col_merchantid].Value);
                                                string rel_amount_ops = CommonUtility.Util.if_s(ws.Cells[current_row, col_relamount_ops].Value);
                                                string rel_amount_merchant = CommonUtility.Util.if_s(ws.Cells[current_row, col_relamount_merchant].Value);
                                                string bank_notes = CommonUtility.Util.if_s(ws.Cells[current_row, col_banknotes].Value);

                                                // we're going to be either releasing to merchant or meritus ops.
                                                string release_amount = "";
                                                eRDBTransactionType eTT = eRDBTransactionType.NotSet;
                                                if (!string.IsNullOrWhiteSpace(rel_amount_merchant) && string.IsNullOrWhiteSpace(rel_amount_ops))
                                                {
                                                    // release to Merchant
                                                    release_amount = rel_amount_merchant;
                                                    eTT = eRDBTransactionType.Merchant;
                                                }
                                                else if (string.IsNullOrWhiteSpace(rel_amount_merchant) && !string.IsNullOrWhiteSpace(rel_amount_ops))
                                                {
                                                    // release to Meritus Ops account
                                                    release_amount = rel_amount_ops;
                                                    eTT = eRDBTransactionType.Meritus;
                                                }

                                                // get the bank type
                                                eRDBBank eB = LookupTableHandler.GetBankByMid(mid.Trim());

                                                if (eTT != eRDBTransactionType.NotSet && eB != eRDBBank.NotSet)
                                                {
                                                    // a bank, and a release type must be set.

                                                    MerchantApp objMA = this.GetMerchantAppByMID(mid);

                                                    if (objMA != null)
                                                    {
                                                        RDBRelease item = new RDBRelease();
                                                        item.ReportDate = dt;
                                                        item.ZID = Convert.ToInt32(objMA.ID);
                                                        item.Amount = CommonUtility.Util.ConvertCurrencyToDecimal(release_amount) * -1; // we negate because it's a release!!
                                                        item.TransTypeID = eTT;
                                                        item.BankNotes = bank_notes;
                                                        item.InternalNotes = "Imported from worksheet: " + bank_notes;
                                                        item.ReserveTypeID = eRDBReserveType.Reserve;
                                                        item.BankID = eB;
                                                        item.MethodID = eRDBTransactionMethod.ACH;
                                                        item.CreatedUserID = Convert.ToInt32(UserSessions.CurrentUser.UserID);

                                                        // insert it into the journal as well.
                                                        if (DataReserve.InsertRDBRelease(item) > 0 && item.ReleaseID > 0)
                                                        {
                                                            DataReserve.InsertRDBJournalReleaseTrans(item, DateTime.MinValue);
                                                            import_count++;

                                                            if (!liReportDate.Contains(dt.ToShortDateString()))
                                                            {
                                                                liReportDate.Add(dt.ToShortDateString());
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
                                this.event_click_uploadcomplete(eRDBReserveUploadTypeID.Releases, objU.UploadID, import_count);
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

        private MerchantApp GetMerchantAppByMID(string mid)
        {
            Hashtable prms = new Hashtable();
            prms.Add("@SettlePlatformMid", mid);
            return DataMerchantApp.GetInstance().FillMerchantApp(prms);
        }




        public void RefreshGrid()
        {
            wucUploadGrid1.BindGrid();
        }
    }
}