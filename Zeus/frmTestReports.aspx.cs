using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;
using System.Collections;
using System.Data;
using PaymentXP.Facade;
using System.Text;

namespace MerchantWeb.web.SecureReportForms
{
    public partial class frmTestReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Click(object sender, EventArgs e)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            Hashtable prms = new Hashtable();
            DataRow dr = null; StringBuilder sb = null;

            decimal amount = 0.0M;
            string DBAName = string.Empty, MerEmail = string.Empty, AgentUID = string.Empty, PLUID = string.Empty;

            prms.Add("@FromDate", DateTime.Today.AddDays(-1));
            prms.Add("@ToDate", DateTime.Today.AddDays(-1));

            DataTable dt = data.GetBatchDepositMerchants(prms);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb = new StringBuilder();
                dr = dt.Rows[i];
                AgentUID = dr["AgentUID"].ToString();
                MerEmail = "";// dr["MerchantEmail"].ToString();
                PLUID = dr["PrivateLabelUID"].ToString();

                amount = DataLayer.Field2Dec(dr["DepAmt"]) + DataLayer.Field2Dec(dr["DisAmt"]) + DataLayer.Field2Dec(dr["AdjAmt"]);

                sb.Append("<span style='font-size:10.0pt;font-family:Verdana;'>Thank you for choosing %%CompanyName%%. This a report of your batch deposit activity. For greater details, please visit your web portal at %%MerUrl%%.</span><p>");
                sb.Append("<table border='1'><tr style='font:bold;'><td>Report Date</td><td>Deposit Amount</td><td>Discount Amount</td><td>Adjustment Amount</td></tr><tr><td>" + dr["ReportDate"].ToString() + "</td><td>" + dr["DepAmt"].ToString() + "</td><td>" + dr["DisAmt"].ToString() + "</td><td>" + dr["AdjAmt"].ToString() + "</td></tr></table></p>");


                if (sb.Length > 0 && amount > 0.0M)
                    AlertNotification.SendMerchantAlertReminder("clientservices@paysafe.com", DBAName, "Daily Batch Deposit", "", "wnguy@paysafe.com"/* "rm@paysafe.com;underwriting@paysafe.com"*/, sb.ToString(), "", AgentUID, PLUID, string.Empty);
            }
        }

        protected void btnChargeback_Click(object sender, EventArgs e)
        {
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            Hashtable prms = new Hashtable();
            DataRow dr = null;

            DataTable dt = data.GetChargebackCaseAuto(prms);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                // AlertNotification.SendMerchantAlertReminder("clientservices@paysafe.com", DBAName, "Daily Batch Deposit", MerEmail, "ckumbaji@zeus.paysafe.com"/* "rm@zeus.paysafe.com;underwriting@zeus.paysafe.com"*/, sb.ToString(), "", AgentUID, PLUID, string.Empty);
                bool perform = MerchantFacade.SendEmail(dr["Subject"].ToString(), "", dr["HTMLBody"].ToString(), dr["From"].ToString(), ""/*dr["To"].ToString()*/,""/* "rm@zeus.paysafe.com," + dr["CC"].ToString()*/, "wnguy@zeus.paysafe.com;ckumbaji@paysafe.com", new Hashtable(), dr["MerchantAppUID"].ToString(), string.Empty, null);
                if(perform)
                    ZeusWeb.Logging.EmailLog.InfoFormat("Sending Chargeback Reports for: {0} Sending Email from: {1}", dr["MerchantAppUID"].ToString(), dr["From"].ToString());
                else
                    ZeusWeb.Logging.EmailLog.InfoFormat("Error while Sending Chargeback Reports for: {0} Sending Email from: {1}", dr["MerchantAppUID"].ToString(), dr["From"].ToString());
            }
        }
    }

}