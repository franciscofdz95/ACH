using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;

namespace ZeusWeb.SecureRiskForms
{
    public partial class frmReserveUpload : frmBaseSearch
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

            wucUploadRelease1.event_click_uploadcomplete += wucUploadRelease1_event_click_uploadcomplete;
            wucUploadReject1.event_click_uploadcomplete += wucUploadReject1_event_click_uploadcomplete;
            wucUploadReserve1.event_click_uploadcomplete += wucUploadReserve1_event_click_uploadcomplete;
            wucUploadADR0601.event_click_uploadcomplete += wucUploadADR0601_event_click_uploadcomplete;

            if (!IsPostBack)
            {
                DateTime ReportDate = DateTime.Now.AddDays(-1).Date;
                this.ReportDate.Text = ReportDate.ToString("MM/dd/yyy");
                
                BindImportGrid(ReportDate);
            }
        }

        public void wucUploadADR0601_event_click_uploadcomplete(PaymentXP.BusinessObjects.eRDBReserveUploadTypeID eUT, int upload_id, int total_imported)
        {
            wucMessage1.AddMessageStatus("ADR060 File successfully uploaded. Item Count: " + total_imported.ToString());
            wucUploadADR0601.RefreshGrid();
        }

        public void wucUploadReserve1_event_click_uploadcomplete(PaymentXP.BusinessObjects.eRDBReserveUploadTypeID eUT, int upload_id, int total_imported)
        {
            wucMessage1.AddMessageStatus("Reserve file successfully uploaded. Item Count: " + total_imported.ToString());
            wucUploadReserve1.RefreshGrid();
        }

        public void wucUploadReject1_event_click_uploadcomplete(PaymentXP.BusinessObjects.eRDBReserveUploadTypeID eUT, int upload_id, int total_imported)
        {
            wucMessage1.AddMessageStatus("Reject file successfully uploaded. Item Count: " + total_imported.ToString());
            wucUploadReject1.RefreshGrid();
        }

        public void wucUploadRelease1_event_click_uploadcomplete(PaymentXP.BusinessObjects.eRDBReserveUploadTypeID eUT, int upload_id, int total_imported)
        {
            wucMessage1.AddMessageStatus("Release file successfully uploaded. Item Count: " + total_imported.ToString());
            wucUploadRelease1.RefreshGrid();
        }

        protected void ApplyReportDate_Click(object sender, EventArgs e)
        {
            DateTime res;

            if (!DateTime.TryParse(this.ReportDate.Text, out res))
            {
                res = DateTime.Now;
            }

            BindImportGrid(res);
        }

        private void BindImportGrid(DateTime uploadDate)
        {
            UploadSummary.DataSource = DataReserve.GetImportSummary(uploadDate);
            UploadSummary.DataBind();
        }
    }
}