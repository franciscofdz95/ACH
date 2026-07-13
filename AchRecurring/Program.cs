using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using Nmc.Ach.Dal;
using CommonUtility;
//using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;

namespace AchRecurring
{
    public class Program
    {
        private static DataTransaction m_DataTransaction = null;
        private static DataRecurring m_DataRecurring = null;

        private static string m_ToError = ConfigurationManager.AppSettings["EmailToError"];
        private static string m_To = ConfigurationManager.AppSettings["EmailTo"];
        private static string m_From = ConfigurationManager.AppSettings["EmailFrom"];

        static bool programCompletedWithNoErrors = true;

        static void Main(string[] args)
        {
            DateTime current_time = DateTime.Now;
            DateTime dateOverride;

            if (args.Length == 1 && DateTime.TryParse(args[0], out dateOverride))
            {
                current_time = dateOverride;
            }

            RecurLog.Root.InfoFormat("Executing recurring ACH on {0}......", current_time);
            RecurLog.Root.InfoFormat("Email errors to: {0}", m_ToError);
            RecurLog.Root.InfoFormat("Email recurring status to: {0}", m_To);
            RecurLog.Root.InfoFormat("From email address: {0}", m_From);

            m_DataTransaction = new DataTransaction();
            m_DataRecurring = new DataRecurring();

            ProcessAchRecurring(current_time);

            DateTime dtRunDate = DateTime.Now;
           
            if (DateTime.Now.Hour == 18)
            {
                ProcessDeclineRecycler();

                if (programCompletedWithNoErrors)
                {
                    SendAlerts(current_time);
                }

                try
                {
                    ProcessAchRecurring_Notifications(dtRunDate);
                }
                catch (Exception ex)
                {

                    string mysubject = "Error: Upcoming Recurring Notifications (ACH)";
                    string myerror = string.Format(@"Upcoming Recurring Notifications <br>
                                                   MESSAGE: {0}<br />
                                                   TIME: {1}<br />
                                                   STACKTRACE: {2}", ex.Message,
                                                                     DateTime.Now.ToString(),
                                                                     ex.StackTrace);
                    string myfrom = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["EmailFrom"], "no-reply@merituspayment.com");
                    string myto = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["EmailToError"], "developers@merituspayment.com");

                    RecurLog.Root.ErrorFormat("A {0} has occurred while processing upcoming recurring notifications: {1}", ex.GetType().Name, ex.Message);
                    RecurLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                    CommonUtility.Email.SendEmail(mysubject, myerror, myfrom, myto);
                }
            }
        }


        private static void ProcessAchRecurring(DateTime runDate)
        {
            string CurrentDate = runDate.ToString("MM/dd/yyyy");

            RecurLog.Root.InfoFormat("Processing ACH recurring, current date: {0}", CurrentDate);

            ArrayList prms = new ArrayList();
            int RecurID = -1;
            int RecordCount = 0;

            try
            {
                RecurLog.Root.Info("Retrieving ACH recurring rules......");

                prms.Add(new SqlParameter("@Current_Date", CurrentDate));

                using (SqlDataReader dr = m_DataRecurring.SelectDuePayments(prms))
                {
                    AchTransactionFacade facade = new AchTransactionFacade();

                    while (dr.Read())
                    {
                        RecurID = PaymentXP.DataObjects.DataLayer.Field2Int(dr["Recur_ID"]);

                        RecurLog.Root.InfoFormat("Processing ACH recurring [ID={0}]......", RecurID);

                        long transID = AddTranaction(dr);

                        //Get next schedule date
                        if (transID > 0)
                        {
                            RecurLog.Root.InfoFormat("Setting next scheduled date for recurring [ID={0}]......", RecurID);

                            AchTransaction trans = facade.GetRecurringAchTransaction(dr["MerchantID"].ToString(), DataLayer.Field2Int(dr["Recur_ID"]));

                            DateTime NextDate = SetNextSchedulePayment(trans);

                            RecurLog.Root.InfoFormat("Recur_ID: {0} Calculated Next Schedule Date: {1}", dr["Recur_ID"], NextDate);

                            //Update recurring payment schedule
                            UpdateSchedule(DataLayer.Field2Int(dr["Recur_ID"]), NextDate);

                            RecordCount++;

                            try
                            {
                                string response = GetSilentPostString(transID, DataLayer.Field2Str(dr["RefID"]), DataLayer.Field2Dec(dr["Amount"]));

                                RecurLog.Root.InfoFormat("Inserting ACH silent post for trans ID={0}......", transID);

                                m_DataRecurring.InsertACHSilentPost(transID, DataLayer.Field2Int(dr["MerchantID"]), response);

                                RecurLog.Root.InfoFormat("ACH silent post for trans ID={0} inserted.", transID);
                            }
                            catch (Exception exc)
                            {
                                RecurLog.Root.ErrorFormat("A {4} has occurred, Failed to write silent post for Recurring ACH transaction [RecurID={0}], [TransID={1}], [Merchant ID={2}]: {3}", RecurID, transID, dr["MerchantID"], exc.Message, exc.GetType().Name);
                                RecurLog.Root.ErrorFormat("Stack Trace: {0}", exc.StackTrace);
                            }
                        }
                    }

                    dr.Close();
                }

                RecurLog.Root.InfoFormat("ACH Recurring processed {0} transactions.", RecordCount);

                //Email.SendEmail("ACH Recurring Process", "ACH Recurring processed " + RecordCount.ToString() + " transactions.", m_From, m_To);
            }
            catch (Exception exc)
            {
                programCompletedWithNoErrors = false;

                RecurLog.Root.ErrorFormat("A {0} has occurred while prcessing ACH recurring transactions: {1}", exc.GetType().Name, exc.Message);
                RecurLog.Root.ErrorFormat("Stack Trace: {0}", exc.StackTrace);

                string msg = string.Empty;
                msg = "Date: " + DateTime.Now.ToString() + "\n";
                msg += "RecurID: " + RecurID.ToString() + "\n";
                msg += "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("Process ACH Recurring Failed", msg, m_From, m_ToError);
            }
        }

        private static long AddTranaction(SqlDataReader dr)
        {
            ArrayList prms = new ArrayList();

            SqlParameter prm = new SqlParameter("@TransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@AchID", DataLayer.Field2Int(dr["AchID"])));
            prms.Add(new SqlParameter("@TransDate", DateTime.Now));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Field2Int(dr["MerchantID"])));
            prms.Add(new SqlParameter("@BatchID", DBNull.Value));
            prms.Add(new SqlParameter("@OriginID", 26)); //Ach Recurring
            prms.Add(new SqlParameter("@Description", DataLayer.Field2Str(dr["Ach_Description"])));
            prms.Add(new SqlParameter("@DescDate", DataLayer.Field2Str(dr["DescDate"])));
            prms.Add(new SqlParameter("@CompanyName", DataLayer.Field2Str(dr["CompanyName"])));
            prms.Add(new SqlParameter("@TransType", DataLayer.Field2Str(dr["TransType"])));
            prms.Add(new SqlParameter("@Secc", DataLayer.Field2Str(dr["Secc"])));
            prms.Add(new SqlParameter("@TransRoute", DataLayer.Field2Str(dr["TransRoute"])));
            prms.Add(new SqlParameter("@AccountNo", DataLayer.Field2Str(dr["AccountNo"])));
            prms.Add(new SqlParameter("@NameOnAccount", DataLayer.Field2Str(dr["NameOnAccount"])));
            prms.Add(new SqlParameter("@RefID", DataLayer.Field2Str(dr["RefID"])));
            prms.Add(new SqlParameter("@Amount", DataLayer.Field2Dec(dr["Amount"])));
            prms.Add(new SqlParameter("@StatusID", DataLayer.Int2Field(0)));
            prms.Add(new SqlParameter("@DateProcessed", DBNull.Value));
            prms.Add(new SqlParameter("@NextProcessDate", DateTime.Now));
            prms.Add(new SqlParameter("@ResubmitCount", DataLayer.Int2Field(0)));
            prms.Add(new SqlParameter("@CreditID", DBNull.Value));
            prms.Add(new SqlParameter("@InvoiceID", DBNull.Value));
            prms.Add(new SqlParameter("@UploadID", DBNull.Value));
            prms.Add(new SqlParameter("@UserID", DBNull.Value));
            prms.Add(new SqlParameter("@Source", "C"));
            prms.Add(new SqlParameter("@Note", DBNull.Value));
            prms.Add(new SqlParameter("@ReturnID", DBNull.Value));
            prms.Add(new SqlParameter("@ClientID", DBNull.Value));
            prms.Add(new SqlParameter("@WalletID", DBNull.Value));
            prms.Add(new SqlParameter("@AddedBy", DataLayer.Int2Field(1)));
            prms.Add(new SqlParameter("@Recur_ID", DataLayer.Field2Int(dr["Recur_ID"])));
            prms.Add(new SqlParameter("@Email", DataLayer.Field2Str(dr["Email"])));
            prms.Add(new SqlParameter("@CustomerID", DataLayer.Field2Str(dr["CustomerID"])));
            prms.Add(new SqlParameter("@CustomerName", DataLayer.Field2Str(dr["CustomerName"])));

            RecurLog.Root.InfoFormat("Adding transaction for ACH recurring [ID={0}]......", dr["Recur_ID"]);

            long lngID = m_DataTransaction.Insert(prms);

            if (lngID != -1)
            {
                RecurLog.Root.InfoFormat("Recur_ID: {0} payment created. TransID: {1}", dr["Recur_ID"], lngID);
            }
            else
            {
                RecurLog.Root.ErrorFormat("Recur_ID: {0} failed to create.", dr["Recur_ID"]);
            }

            return lngID;
        }

        private static void ProcessDeclineRecycler()
        {
            RecurLog.Root.Info("Executing decline recycler......");

            int iCount = 0;

            string _criticalCode = "Started Method";

            try
            {
                //set up variables
                DateTime currentDateTime = DateTime.Now; //snapshot begin of process
                List<PaymentXP.BusinessObjects.MerchantApp> merchantAppBillRetryActionList = null;

                //set up data layer objects
                PaymentXP.DataObjects.DataAchTransaction dataAchTransaction = new PaymentXP.DataObjects.DataAchTransaction();
                DataAchProcess dataAchProcess = new DataAchProcess();

                RecurLog.Root.Info("Retrieving bill retry configuration......");

                //Get all configured Final Actions
                _criticalCode = "GetMerchantAppsBillRetryActionConfiguration";
                merchantAppBillRetryActionList = PaymentXP.DataObjects.DataBillRetry.GetMerchantAppsBillRetryActionConfiguration();

                RecurLog.Root.Info("Synchronizing recurring log......");

                //1. Catch up RecurringLog with latest data (we check back about 7 days - see stored procedure)
                _criticalCode = "SyncRecurringLogWithAchReturns";
                DataTable dtSyncResults = dataAchProcess.SyncRecurringLogWithAchReturns(DateTime.Today);

                //2. Handle Failed Retries that have reached maximum retry count
                _criticalCode = "HandleMaxeddFailedRetries";
                Dictionary<long, int> maximumTriedOriginalTransactions = HandleMaxedFailedRetries(ref merchantAppBillRetryActionList);

                //3. Retry Transactions
                _criticalCode = "maximumTriedOriginalTransactions";
                iCount = RetryBilling(dataAchProcess, maximumTriedOriginalTransactions);
            }
            catch (System.Exception x)
            {
                programCompletedWithNoErrors = false;

                //send email
                string message = String.Format("{2} Occurred: Critical Code Step: {0} ~ Exception: {1}", _criticalCode, x.Message, x.GetType().Name);

                RecurLog.Root.Error(message);
                RecurLog.Root.ErrorFormat("Stack Trace: {0}", x.StackTrace);

                Email.SendEmail("ACH Decline Recycler Process: Exception", message, m_From, m_ToError);
            }

            RecurLog.Root.InfoFormat("ACH Decline Recycler processed {0} transactions.", iCount);

            //Send Email with *iCount*
            //Email.SendEmail("ACH Decline Recycler Process", "ACH Decline Recycler processed " + iCount.ToString() + " transactions.", m_From, m_To);
        }

        private static Dictionary<long, int> HandleMaxedFailedRetries(ref List<PaymentXP.BusinessObjects.MerchantApp> merchantAppBillRetryActionList)
        {
            Dictionary<long, int> maximumTriedOriginalTransactions = new Dictionary<long, int>(); //Dictionary<OriginalTransID, MerchantID>

            PaymentXP.DataObjects.DataAchTransaction dataAchTransaction = new PaymentXP.DataObjects.DataAchTransaction();

            RecurLog.Root.Info("Retrieving failed max retries......");

            DataAchProcess dataAchProcess = new DataAchProcess();
            DataTable retryCounts = dataAchProcess.GetFailedMaxRetries();

            if (retryCounts != null && retryCounts.Rows.Count > 0)
            {
                RecurLog.Root.InfoFormat("Failed max retries retrieved: {0}", retryCounts.Rows.Count);

                RecurLog.Root.Info("Retrieving bill retry action configuration......");

                merchantAppBillRetryActionList = PaymentXP.DataObjects.DataBillRetry.GetMerchantAppsBillRetryActionConfiguration();

                foreach (DataRow dr in retryCounts.Rows)
                {
                    //************************************************************************************
                    //If Transaction Response is Failed, then handle any merchant configured Final Actions
                    //************************************************************************************
                    maximumTriedOriginalTransactions.Add(DataLayer.Field2Long(dr["OriginalTransID"]), DataLayer.Field2Int(dr["MerchantID"]));

                    RecurLog.Root.InfoFormat("Finalizing max retry for recur ID={0}......", dr["Recur_ID"]);

                    // If we exhausted all currently set retry rules, mark the Retries as done (i.e. set a FinalRetryDate)
                    dataAchProcess.UpdateRecurringLogFinalRetryDate(DataLayer.Field2Int(dr["Recur_ID"]));

                    RecurLog.Root.InfoFormat("Max retry for recur ID={0} finalized.", dr["Recur_ID"]);

                    if (merchantAppBillRetryActionList != null)
                    {
                        List<PaymentXP.BusinessObjects.BillRetryAction> billRetryActionList = GetBillRetryActionList(DataLayer.Field2Int(dr["MerchantID"]).ToString(), merchantAppBillRetryActionList);
                        if (billRetryActionList != null)
                        {
                            foreach (PaymentXP.BusinessObjects.BillRetryAction billRetryAction in billRetryActionList)
                            {
                                switch (billRetryAction.Key)
                                {
                                    case PaymentXP.BusinessObjects.BillRetryActionTypes.DeactivateRecurring:
                                        dataAchTransaction.UpdateRecurringStatus(DataLayer.Field2Int(dr["Recur_ID"]), false); //turn off recurring
                                        dataAchProcess.InsertDisabledRecurringHistory(DataLayer.Field2Int(dr["Recur_ID"]), DataLayer.Field2Long(dr["OriginalTransID"])); //log that we turned off recurring schedule

                                        
                                        //debug: temporarily send an email if we turn off Recurring - we don't have history yet
                                        string message = String.Format("Turned off Recurring for RecurID: {0}", DataLayer.Field2Int(dr["Recur_ID"]));

                                        RecurLog.Root.Info(message);
                                        Email.SendEmail("ACH Decline Recycler Process: Information", message, m_From, m_To);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            RecurLog.Root.InfoFormat("Maximum tried original trans: {0}", maximumTriedOriginalTransactions.Count);

            return maximumTriedOriginalTransactions;
        }

        private static int RetryBilling(DataAchProcess dataAchProcess, Dictionary<long, int> maximumTriedOriginalTransactions)
        {
            RecurLog.Root.Info("Retrieving qualified bill retries......");

            DataTable dataTable = dataAchProcess.GetQualifiedBillRetries(); //get qualifying declined transactions's
            int iCount = 0;

            if (dataTable == null) { return iCount; }
            if (dataTable.Rows.Count == 0) { return iCount; }

            RecurLog.Root.InfoFormat("Qualified bill retries retrieved: {0}", dataTable.Rows.Count);

            foreach (DataRow dr in dataTable.Rows)
            {
                try
                {
                    if (maximumTriedOriginalTransactions.ContainsKey(DataLayer.Field2Long(dr["OriginalTransID"]))) { continue; }

                    RecurLog.Root.Info("Creating new rebill ACH transaction......");

                    ArrayList prms = new ArrayList();
                    SqlParameter prm = new SqlParameter("@TransID", -1);
                    prm.Direction = ParameterDirection.Output;
                    prms.Add(prm);

                    prms.Add(new SqlParameter("@AchID", DataLayer.Field2Int(dr["AchID"])));
                    prms.Add(new SqlParameter("@TransDate", DateTime.Now));
                    prms.Add(new SqlParameter("@MerchantID", DataLayer.Field2Int(dr["MerchantID"])));
                    prms.Add(new SqlParameter("@BatchID", DBNull.Value));
                    prms.Add(new SqlParameter("@OriginID", 26)); //Ach Recurring                            // this does not come from "sp_GetAchDeclinedRecurringTransactions"
                    prms.Add(new SqlParameter("@Description", DataLayer.Field2Str(dr["Ach_Description"])));
                    prms.Add(new SqlParameter("@DescDate", DataLayer.Field2Str(dr["DescDate"])));
                    prms.Add(new SqlParameter("@CompanyName", DataLayer.Field2Str(dr["CompanyName"])));
                    prms.Add(new SqlParameter("@TransType", DataLayer.Field2Str(dr["TransType"])));
                    prms.Add(new SqlParameter("@Secc", DataLayer.Field2Str(dr["Secc"])));
                    prms.Add(new SqlParameter("@TransRoute", DataLayer.Field2Str(dr["TransRoute"])));
                    prms.Add(new SqlParameter("@AccountNo", DataLayer.Field2Str(dr["AccountNo"])));
                    prms.Add(new SqlParameter("@NameOnAccount", DataLayer.Field2Str(dr["NameOnAccount"])));
                    prms.Add(new SqlParameter("@RefID", DataLayer.Field2Str(dr["RefID"])));
                    prms.Add(new SqlParameter("@Amount", DataLayer.Field2Dec(dr["Amount"])));
                    prms.Add(new SqlParameter("@StatusID", DataLayer.Int2Field(0)));                        // this does not come from "sp_GetAchDeclinedRecurringTransactions"
                    prms.Add(new SqlParameter("@DateProcessed", DBNull.Value));
                    prms.Add(new SqlParameter("@NextProcessDate", DateTime.Now));
                    prms.Add(new SqlParameter("@ResubmitCount", DataLayer.Int2Field(0)));
                    prms.Add(new SqlParameter("@CreditID", DBNull.Value));
                    prms.Add(new SqlParameter("@InvoiceID", DBNull.Value));
                    prms.Add(new SqlParameter("@UploadID", DBNull.Value));
                    prms.Add(new SqlParameter("@UserID", DBNull.Value));
                    prms.Add(new SqlParameter("@Source", "C"));
                    prms.Add(new SqlParameter("@Note", DBNull.Value));
                    prms.Add(new SqlParameter("@ReturnID", DBNull.Value));
                    prms.Add(new SqlParameter("@ClientID", DBNull.Value));
                    prms.Add(new SqlParameter("@AddedBy", DataLayer.Int2Field(1)));
                    prms.Add(new SqlParameter("@Recur_ID", DataLayer.Field2Int(dr["Recur_ID"])));
                    prms.Add(new SqlParameter("@Email", DataLayer.Field2Str(dr["Email"])));
                    prms.Add(new SqlParameter("@CustomerID", DataLayer.Field2Str(dr["CustomerID"])));
                    prms.Add(new SqlParameter("@CustomerName", DataLayer.Field2Str(dr["CustomerName"])));
                    prms.Add(new SqlParameter("@OriginalTransID", DataLayer.Field2Str(dr["OriginalTransID"])));
                    prms.Add(new SqlParameter("@OriginalAmount", DataLayer.Field2Str(dr["OriginalAmount"])));
                    prms.Add(new SqlParameter("@BillRetryID", DataLayer.Field2Str(dr["BillRetryID"])));

                    m_DataTransaction.InsertBillRetryTransaction(prms);

                    RecurLog.Root.Info("Rebill ACH transaction created.");

                    iCount++;
                }
                catch (Exception ex)  // catch exception because we do not want it to fail because of 1 bad RecurringPayment.
                {
                    programCompletedWithNoErrors = false;

                    //send email
                    string message = String.Format("{3} Occurred: Original TransID: {0} ~ RecurID: {1} ~ Exception Detail: {2}",
                                                                                    DataLayer.Field2Str(dr["OriginalTransID"]),
                                                                                    DataLayer.Field2Int(dr["Recur_ID"]),
                                                                                    ex.Message,
                                                                                    ex.GetType().Name);

                    RecurLog.Root.Error(message);
                    RecurLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                    Email.SendEmail("ACH Decline Recycler Process: Exception", message, m_From, m_ToError);
                    
                }
            }

            return iCount;
        }

        private static void SendAlerts(DateTime currentDateTime)
        {
            try
            {
                RecurLog.Root.Info("Sending recurring notifications, retrieving merchant alert configuration......");

                //Responsible to get all contacts that subscribed to this alert
                Dictionary<int, PaymentXP.BusinessObjects.MerchantAlert> merchantAlertConfiguration = PaymentXP.DataObjects.DataAlert.GetMerchantAlerts(PaymentXP.BusinessObjects.Alerts.RecurringTransactionReport, PaymentXP.BusinessObjects.Constants.PAYMENTXP_PORTAL);

                if (merchantAlertConfiguration == null) 
                {
                    RecurLog.Root.Info("No merchant alert configuration retrieved.");
                    return; 
                }

                RecurLog.Root.InfoFormat("Merchant alert configuration retrieved: {0}", merchantAlertConfiguration.Count);
                RecurLog.Root.Info("Retrieving daily recurring stats......");

                //Responsible to get list of Daily Statistics for Merchants that processed today - this is the core data to fill the email template
                DataTable dtProcessed = PaymentXP.DataObjects.DataBillRetry.GetDailyStatsReportACH(currentDateTime);

                RecurLog.Root.Info("Daily recurring stats retrieved.");
                RecurLog.Root.Info("Retrieving private labeled merchants......");

                //Responsible to get list of private labeled merchants - this is so we can private label the email template
                DataTable dtPrivateLabeledMerchants = PaymentXP.DataObjects.DataMerchantApp.GetInstance().GetPrivateLabeledMerchantList();

                RecurLog.Root.Info("Private labeled merchants retrieved.");
                RecurLog.Root.Info("Retrieving subscribed merchant list......");

                //Responsible to get a list of MerchantID's that have configured Bill Retries - this is important because we show or hide a section on the EmailTemplate depending on whether Merchant has subscribed to Bill Retries
                Hashtable hashBillRetryMerchantIDs = PaymentXP.DataObjects.DataBillRetry.GetSubscribedMerchantList();

                RecurLog.Root.Info("Subscribed merchant list retrieved.");
                RecurLog.Root.Info("Retrieving email template for recurring transactions report......");

                //Responsible to supply email template
                PaymentXP.BusinessObjects.MPSEmailTemplate emailTemplate = PaymentXP.DataObjects.DataMPSEmailTemplate.Instance.GetTemplate(PaymentXP.DataObjects.EmailTemplateTypes.RecurringTransactionsReport.ToString());

                if (emailTemplate == null) 
                {
                    RecurLog.Root.Error("No Email template for recurring transactions report retrieved");
                    throw new System.Exception("{Alert} Email Template Not Found"); 
                }

                RecurLog.Root.Info("Email template for recurring transactions report retrieved");

                //set up template variables
                string dateTime = currentDateTime.ToShortDateString();
                string SEND_ON_BEHALF_OF_USER = "RECURRING_PAYMENTS";

                int merchantID = int.MinValue;

                string fromEmailAddress = string.Empty;
                string pLVTName = string.Empty;
                string plProductName = string.Empty;
                string pLVTUrl = string.Empty;
                string pLVTPhoneNumber = string.Empty;

                string originalTransactionCount = string.Empty;
                string originalTransactionApprovedCount = string.Empty;
                string originalTransactionApprovedAmount = string.Empty;

                string billRetryCount = string.Empty;
                string billRetryApprovedCount = string.Empty;
                string billRetryApprovedAmount = string.Empty;

                string subject = "Alert: Recurring Transaction Declined Report";

                string emailContent = emailTemplate.Content;

                if (dtProcessed != null)
                {
                    try
                    {
                        RecurLog.Root.InfoFormat("Daily recurring stats to send: {0}", dtProcessed.Rows.Count);

                        //go through GetDailyStatsReport which has one row per merchant
                        foreach (DataRow row in dtProcessed.Rows)
                        {
                            merchantID = int.Parse(row["MerchantID"].ToString()); //get MerchantID

                            if (merchantAlertConfiguration.ContainsKey(merchantID)) //if Merchant has subscribers to this alert
                            {
                                if (merchantAlertConfiguration[merchantID].ContactList == null) 
                                {
                                    RecurLog.Root.InfoFormat("ZID [{0}] is does not have a contact list for daily recurring report.", merchantID);
                                    continue; 
                                } 

                                //reset to default values
                                pLVTName = "Meritus Payment Solutions";
                                plProductName = "Payment XP";
                                pLVTPhoneNumber = "888-851-7558";
                                fromEmailAddress = "doNotReply@PaymentXP.com";
                                pLVTUrl = @"https://www.paymentxp.com";

                                DataRow plMerchantRow = GetMerchantRow(merchantID, dtPrivateLabeledMerchants);

                                //if it's a privated labeled Merchant, override variables with private label values
                                if (plMerchantRow != null)
                                {
                                    RecurLog.Root.InfoFormat("ZID [{0}] is private labeled: {1}", merchantID, plMerchantRow["PLCompanyName"]);

                                    pLVTName = DataLayer.Field2Str(plMerchantRow["PLCompanyName"]);
                                    plProductName = DataLayer.Field2Str(plMerchantRow["plProductName"]);
                                    pLVTUrl = "http://" + DataLayer.Field2Str(plMerchantRow["VTURL"]).Replace("http://", "");
                                    
                                    if (DataLayer.Field2Str(plMerchantRow["PLEmail"]).Length > 0)
                                    {
                                        if (DataLayer.Field2Str(plMerchantRow["PLEmail"]).IndexOf("@") > 0)
                                        {
                                            fromEmailAddress = String.Format(@"doNotReply{0}",
                                                    DataLayer.Field2Str(plMerchantRow["PLEmail"]).Substring(DataLayer.Field2Str(plMerchantRow["PLEmail"]).IndexOf("@")));
                                        }
                                    }

                                    pLVTPhoneNumber = DataLayer.Field2Str(plMerchantRow["PLPhone"].ToString());
                                }

                                //get original transaction values from report
                                originalTransactionCount = DataLayer.Field2Str(row["OriginalTransactionCount"]);
                                originalTransactionApprovedCount = DataLayer.Field2Str(row["OriginalTransactionApprovedCount"]);
                                originalTransactionApprovedAmount = DataLayer.Field2Str(row["OriginalTransactionApprovedAmount"]);

                                //get bill retry values from report
                                billRetryCount = DataLayer.Field2Str(row["BillRetryCount"]);
                                billRetryApprovedCount = DataLayer.Field2Str(row["BillRetryApprovedCount"]);
                                billRetryApprovedAmount = DataLayer.Field2Str(row["BillRetryApprovedAmount"]);

                                //format subject
                                subject = String.Format("{0} {1} Alert: Recurring Transaction Report - {2}", pLVTName, plProductName, merchantAlertConfiguration[merchantID].BusinessDBAName);

                                //handle dynamic content show/hide section
                                string dynamicContentStartTag = @"<!--<DynamicStart>-->";//default is to pull these out
                                string dynamicContentEndTag = @"<!--<DynamicEnd>-->";

                                if (!hashBillRetryMerchantIDs.Contains(merchantID))
                                {
                                    //if merchant is not bill retry subscriber, turn tags into comment section
                                    dynamicContentStartTag = @"<!--";
                                    dynamicContentEndTag = @"-->";
                                }

                                RecurLog.Root.InfoFormat("Generating daily recurring report email content for ZID [{0}]......", merchantID);

                                //replace variables in email template
                                emailContent = emailTemplate.Content
                                    .Replace("[$Date$]", dateTime)
                                    .Replace("[$PLVTName$]", plProductName)
                                    .Replace("[$PLVTUrl$]", pLVTUrl)
                                    .Replace("[$PLVTPhoneNumber$]", pLVTPhoneNumber)
                                    .Replace("[$OriginalTransactionApprovedCount$]", originalTransactionApprovedCount)
                                    .Replace("[$OriginalTransactionCount$]", originalTransactionCount)
                                    .Replace("[$OriginalTransactionApprovedAmount$]", String.Format("{0:c}", decimal.Parse(originalTransactionApprovedAmount)))
                                    .Replace("[$BillRetryApprovedCount$]", billRetryApprovedCount)
                                    .Replace("[$BillRetryCount$]", billRetryCount)
                                    .Replace("[$BillRetryApprovedAmount$]", String.Format("{0:c}", decimal.Parse(billRetryApprovedAmount)))
                                    .Replace("<!--<DynamicStart>-->", dynamicContentStartTag)
                                    .Replace("<!--<DynamicEnd>-->", dynamicContentEndTag);

                                RecurLog.Root.Info("Daily recurring report email content generated.");

                                //send email for each contact that subscribed
                                foreach (PaymentXP.BusinessObjects.Contact contact in merchantAlertConfiguration[merchantID].ContactList)
                                {
                                    RecurLog.Root.InfoFormat("Sending daily recurring report to '{0}'......", contact.EmailAddress);
                                    PaymentXP.Facade.MerchantFacade.SendEmail(subject, emailContent, emailContent, PaymentXP.BusinessObjects.Constants.GLOBAL_EMAIL_FROM_ADDRESS, contact.EmailAddress , string.Empty, string.Empty, null, merchantAlertConfiguration[merchantID].MerchantUID, SEND_ON_BEHALF_OF_USER);
                                    RecurLog.Root.InfoFormat("Daily recurring report sent to '{0}'.", contact.EmailAddress);
                                }
                            }
                            else
                            {
                                RecurLog.Root.InfoFormat("ZID [{0}] is does not have any subscribers for daily recurring report.", merchantID);
                            }
                        }
                    }
                    catch (System.Exception x)
                    {
                        string error = String.Format("Error in Sending Alert to Merchant ID {0}: {1}", merchantID, x.Message);

                        RecurLog.Root.ErrorFormat("A {0} has occurred while sending daily recurring report: {1}", x.GetType().Name, x.Message);
                        RecurLog.Root.ErrorFormat("Stack Trace: {0}", x.StackTrace);

                        CommonUtility.Email.SendEmail("Error occurred while processing an Alert: ", error, ConfigurationManager.AppSettings.Get("EmailFrom"), ConfigurationManager.AppSettings.Get("EmailTo"));
                    }
                }
            }
            catch (System.Exception ex)
            {
                string error = String.Format("Error in Sending Alerts: {0}", ex.Message);

                RecurLog.Root.ErrorFormat("A {0} has occurred while processing daily recurring report: {1}", ex.GetType().Name, ex.Message);
                RecurLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                CommonUtility.Email.SendEmail("Error occurred while processing Alerts: ", error, ConfigurationManager.AppSettings.Get("EmailFrom"), m_ToError);
            }

            RecurLog.Root.Info("Recurring notifications complete.");
        }

        private static DataRow GetMerchantRow(int merchantID, DataTable dtPrivateLabeledMerchants)
        {
            foreach (DataRow row in dtPrivateLabeledMerchants.Rows)
            {
                if (merchantID == int.Parse(row["MerchantID"].ToString()))
                {
                    return row;
                }
            }
            return null;
        }

        private static List<PaymentXP.BusinessObjects.BillRetryAction> GetBillRetryActionList(string merchantID, List<PaymentXP.BusinessObjects.MerchantApp> merchantAppList)
        {
            foreach (PaymentXP.BusinessObjects.MerchantApp merchantApp in merchantAppList)
            {
                if (merchantApp.ID == merchantID)
                {
                    return merchantApp.BillRetryActionList;
                }
            }
            return null;
        }

        private static bool UpdateSchedule(long Recur_ID, DateTime scheduleDate)
        {
            ArrayList prms = new ArrayList();

            RecurLog.Root.InfoFormat("Updating next scheduled run for ACH recurring [ID={0}]......", Recur_ID);

            prms.Add(new SqlParameter("@Option", Constants.UPDATESCHEDULE));
            prms.Add(new SqlParameter("@Recur_ID", Recur_ID));

            if (scheduleDate == DateTime.MaxValue)
                prms.Add(new SqlParameter("@ScheduleDate", DBNull.Value));
            else
                prms.Add(new SqlParameter("@ScheduleDate", scheduleDate));


            int rows = m_DataRecurring.UpdatePaymentSchedule(prms);

            if (rows > 0)
            {
                RecurLog.Root.InfoFormat("Recur_ID: {0} updated on {1}", Recur_ID, DateTime.Now.ToString());
                return true;
            }
            else
            {
                RecurLog.Root.ErrorFormat("Recur_ID: {0} failed to updated on {1}", Recur_ID, DateTime.Now.ToString());
                return false;
            }
        }

        private static DateTime SetNextSchedulePayment(int recurID, DateTime Schedule_Date, DateTime endDate, bool isEndDate, int occurID, int monthlyOption,
                                                int dayNum, int monthNum, int weekDay, int weekNum)
        {

            // Set the next scheduled date for this recurring payment.

            DateTime ScheduleDate = Schedule_Date;
            string NewScheduleDate = string.Empty;

            switch (occurID)
            {
                case (int)OccurenceOptions.Immediately:
                    break;
                case (int)OccurenceOptions.Daily:
                    ScheduleDate = ScheduleDate.AddDays(dayNum);
                    break;
                case (int)OccurenceOptions.Monthly:
                    if (monthlyOption == (int)MontlyOptions.Day)	// Day x of every y month
                    {
                        ScheduleDate = ScheduleDate.AddMonths(monthNum);
                        NewScheduleDate = ScheduleDate.Month.ToString() + "/" + dayNum.ToString() + "/" + ScheduleDate.Year.ToString();
                        if (DataLayer.IsDate(NewScheduleDate))
                            ScheduleDate = Convert.ToDateTime(NewScheduleDate);

                    }
                    else		//The x week y weekday of every z month
                    {
                        ScheduleDate = ScheduleDate.AddMonths(monthNum);

                        int month = ScheduleDate.Month;
                        int year = ScheduleDate.Year;

                        //Select 1 day of the next schedule month & year
                        ScheduleDate = new DateTime(ScheduleDate.Year, ScheduleDate.Month, 1);

                        int ReqWeekDay = weekDay;
                        int ScheduleDay, ActualWeekDay;

                        if (weekNum < Constants.LASTWEEK)
                        {
                            //Find the weekday of 1st of scheduled month
                            ActualWeekDay = Convert.ToInt32(ScheduleDate.DayOfWeek);

                            if (ActualWeekDay == 0)		//If Sunday, reset it to 7
                                ActualWeekDay = Constants.MAXWEEKDAY;

                            //Algorith, to find the day - In Reference to first day of the month
                            if (ReqWeekDay == ActualWeekDay)
                                ScheduleDay = (weekNum - 1) * Constants.MAXWEEKDAY + Constants.FIRSTDAY;
                            else if (ReqWeekDay > ActualWeekDay)
                                ScheduleDay = (ReqWeekDay - ActualWeekDay) + ((weekNum - 1) * Constants.MAXWEEKDAY) + Constants.FIRSTDAY;
                            else			//If reqWeekDay is less then ACtualWeekDay
                                ScheduleDay = (ReqWeekDay - ActualWeekDay) + ((weekNum) * Constants.MAXWEEKDAY) + Constants.FIRSTDAY;
                        }
                        else		//Find the selected day with reference to last day of the month
                        {
                            DateTime tempDate;
                            int LastDay = ScheduleDate.AddMonths(1).AddSeconds(-10).Day;	//Get the last day of the month
                            tempDate = new DateTime(ScheduleDate.Year, ScheduleDate.Month, LastDay);
                            ActualWeekDay = Convert.ToInt32(tempDate.DayOfWeek);

                            if (ActualWeekDay == 0)		//If Sunday, reset it to 7
                                ActualWeekDay = Constants.MAXWEEKDAY;

                            //Algorithm to find the day - In Reference to the last day of the month
                            if (ReqWeekDay == ActualWeekDay)
                                ScheduleDay = LastDay;
                            else if (ReqWeekDay > ActualWeekDay)
                                ScheduleDay = LastDay - Constants.MAXWEEKDAY + (ReqWeekDay - ActualWeekDay);
                            else			//If reqWeekDay is less then ActualWeekDay
                                ScheduleDay = LastDay - (ActualWeekDay - ReqWeekDay);
                        }

                        ScheduleDate = new DateTime(ScheduleDate.Year, ScheduleDate.Month, ScheduleDay);
                    }
                    break;
            }

            if (occurID != Constants.IMMEDIATELY)
            {
                if (isEndDate)
                {
                    if (ScheduleDate > endDate)
                    {
                        //SendToClient(true, nmc_id);
                        //utilObj.WriteLog("Next Schedule date  " + ScheduleDate.ToShortDateString() + " is greater then end date " + endDate.ToShortDateString() + " for schedule Id " + recur_id + " ! Pls inform NMC for future action (stop payment/new end date) for this payment.", Constants.ERROR);
                        //SendToClient(false, "0");
                        ScheduleDate = DateTime.MaxValue;
                    }
                }
            }

            //Update the schedule for this payment
            UpdateSchedule(recurID, ScheduleDate);

            return ScheduleDate;
        }

        private static DateTime SetNextSchedulePayment(AchTransaction recur)
        {
            // Set the next scheduled date for this recurring payment.

            RecurLog.Root.InfoFormat("Calculating next scheduled run for ACH recurring [ID={0}]......", recur.TransID);
            RecurLog.Root.InfoFormat("Current Schedule Date: {0}", recur.Schedule.ScheduleDate);
            RecurLog.Root.InfoFormat("Occurence: {0}", recur.Schedule.OccurenceOption);

            DateTime ScheduleDate = DateTime.Parse(recur.Schedule.ScheduleDate);
            string NewScheduleDate = string.Empty;

            switch (recur.Schedule.OccurenceOption)
            {
                case PaymentXP.BusinessObjects.OccurenceOptions.None:
                    break;
                
                case PaymentXP.BusinessObjects.OccurenceOptions.Daily:

                    RecurLog.Root.InfoFormat("Daily Option: {0}", recur.Schedule.DailyOption);

                    if (recur.Schedule.DailyOption == DailyOptions.WeekDays)
                    {
                        //figure out the next scheduled date. since the recurring rule ran already we'll add a day
                        ScheduleDate = ScheduleDate.AddDays(1.0);
                        DateTime endDate = DateTime.Parse(recur.Schedule.EndDate);

                        //need to figure out the billing cycle. all we care about is whether or not the cycle is fixed or has an enddate
                        BillingCycles cycle = BillingCycles.Auto;

                        RecurLog.Root.InfoFormat("Number of Installments: {0}", recur.Schedule.NumOfInstallments);
                        RecurLog.Root.InfoFormat("End Date: {0}", recur.Schedule.EndDate);

                        if(recur.Schedule.NumOfInstallments > 0 && string.IsNullOrWhiteSpace(recur.Schedule.EndDate))
                        {
                            cycle = BillingCycles.Fixed;

                            RecurLog.Root.InfoFormat("ACH Recurring billing cycle set as Fixed.");
                        }

                        if (ScheduleDate <= endDate)
                        ScheduleDate = Schedule.GetNextScheduledWeekDay(ScheduleDate, recur.Schedule, cycle);
                    }
                    else
                    {
                        ScheduleDate = ScheduleDate.AddDays(recur.Schedule.DayOfMonthOption.GetHashCode());
                    }

                    break;

                case PaymentXP.BusinessObjects.OccurenceOptions.Monthly:

                    RecurLog.Root.InfoFormat("Monthly Option: {0}", recur.Schedule.MontlyOption);

                    if (recur.Schedule.MontlyOption == PaymentXP.BusinessObjects.MontlyOptions.Day)	// Day x of every y month
                    {
                        RecurLog.Root.InfoFormat("Monthly of Year Option: {0}", recur.Schedule.MonthOfYearOption);
                        RecurLog.Root.InfoFormat("Day of Month Option: {0}", recur.Schedule.DayOfMonthOption);

                        ScheduleDate = ScheduleDate.AddMonths(recur.Schedule.MonthOfYearOption.GetHashCode());
                        NewScheduleDate = ScheduleDate.Month.ToString() + "/" + recur.Schedule.DayOfMonthOption.GetHashCode().ToString() + "/" + ScheduleDate.Year.ToString();

                        if (DataLayer.IsDate(NewScheduleDate))
                            ScheduleDate = Convert.ToDateTime(NewScheduleDate);

                    }
                    else		//The x week y weekday of every z month
                    {
                        RecurLog.Root.InfoFormat("Month of Year Option: {0}", recur.Schedule.MonthOfYearOption);
                        RecurLog.Root.InfoFormat("Weekday Option: {0}", recur.Schedule.WeekdayOption);
                        RecurLog.Root.InfoFormat("WeekNUM Option: {0}", recur.Schedule.WeekOption);

                        ScheduleDate = ScheduleDate.AddMonths(recur.Schedule.MonthOfYearOption.GetHashCode());

                        int month = ScheduleDate.Month;
                        int year = ScheduleDate.Year;

                        //Select 1 day of the next schedule month & year
                        ScheduleDate = new DateTime(ScheduleDate.Year, ScheduleDate.Month, 1);

                        int ReqWeekDay = recur.Schedule.WeekdayOption.GetHashCode();
                        int ScheduleDay;
                        int ActualWeekDay;
                        int weekNum = recur.Schedule.WeekOption.GetHashCode();

                        if (weekNum < Constants.LASTWEEK)
                        {
                            //Find the weekday of 1st of scheduled month
                            ActualWeekDay = Convert.ToInt32(ScheduleDate.DayOfWeek);

                            if (ActualWeekDay == 0)		//If Sunday, reset it to 7
                                ActualWeekDay = Constants.MAXWEEKDAY;

                            //Algorith, to find the day - In Reference to first day of the month
                            if (ReqWeekDay == ActualWeekDay)
                                ScheduleDay = (weekNum - 1) * Constants.MAXWEEKDAY + Constants.FIRSTDAY;
                            else if (ReqWeekDay > ActualWeekDay)
                                ScheduleDay = (ReqWeekDay - ActualWeekDay) + ((weekNum - 1) * Constants.MAXWEEKDAY) + Constants.FIRSTDAY;
                            else			//If reqWeekDay is less then ACtualWeekDay
                                ScheduleDay = (ReqWeekDay - ActualWeekDay) + ((weekNum) * Constants.MAXWEEKDAY) + Constants.FIRSTDAY;
                        }
                        else		//Find the selected day with reference to last day of the month
                        {
                            DateTime tempDate;
                            int LastDay = ScheduleDate.AddMonths(1).AddSeconds(-10).Day;	//Get the last day of the month
                            tempDate = new DateTime(ScheduleDate.Year, ScheduleDate.Month, LastDay);
                            ActualWeekDay = Convert.ToInt32(tempDate.DayOfWeek);

                            if (ActualWeekDay == 0)		//If Sunday, reset it to 7
                                ActualWeekDay = Constants.MAXWEEKDAY;

                            //Algorithm to find the day - In Reference to the last day of the month
                            if (ReqWeekDay == ActualWeekDay)
                                ScheduleDay = LastDay;
                            else if (ReqWeekDay > ActualWeekDay)
                                ScheduleDay = LastDay - Constants.MAXWEEKDAY + (ReqWeekDay - ActualWeekDay);
                            else			//If reqWeekDay is less then ActualWeekDay
                                ScheduleDay = LastDay - (ActualWeekDay - ReqWeekDay);
                        }

                        ScheduleDate = new DateTime(ScheduleDate.Year, ScheduleDate.Month, ScheduleDay);
                    }

                    break;
            }

            if (recur.Schedule.OccurenceOption != PaymentXP.BusinessObjects.OccurenceOptions.None)
            {
                RecurLog.Root.InfoFormat("Has End Date: {0}", recur.Schedule.IsEndDate);

                if (recur.Schedule.IsEndDate)
                {
                    RecurLog.Root.InfoFormat("End Date: {0}", recur.Schedule.EndDate);

                    DateTime endDate = DateTime.Parse(recur.Schedule.EndDate);

                    if (ScheduleDate > endDate)
                    {
                        RecurLog.Root.InfoFormat("Next Schedule Date [{0}] exceeds End Date [{1}]: Recurring rule will no longer run", ScheduleDate.ToShortDateString(), endDate.ToShortDateString());
                        ScheduleDate = DateTime.MaxValue;
                    }
                }
            }

            RecurLog.Root.InfoFormat("Next Schedule Date: {0}", ScheduleDate);

            return ScheduleDate;
        }

        private static string GetSilentPostString(long transId, string refNumber, decimal amount)
        {
            RecurLog.Root.InfoFormat("Creating ACH silent post for trans ID={0}, ref number [{1}]......", transId, refNumber);

            StringBuilder sb = new StringBuilder();
            //Transaction=ACH&TransactionStatusID=0&ReferenceNumber=4136955&TransactionAmount=54.00&TransactionID=1881735&Reason=&AddendaInformation=

            sb.Append("Transaction=ACH");
            sb.Append("&TransactionStatusID=0");
            sb.AppendFormat("&ReferenceNumber={0}", refNumber.Replace("&", "%26").Replace("=", "%3D"));
            sb.AppendFormat("&TransactionAmount={0}", amount.ToString("#.##"));
            sb.AppendFormat("&TransactionID={0}", transId);
            sb.Append("&Reason=&AddendaInformation=");

            RecurLog.Root.InfoFormat("ACH silent post for trans ID={0}: {1}", transId, sb.ToString());

            return sb.ToString();
        }

        #region Recurring_Notification


        private static void ProcessAchRecurring_Notifications(DateTime rundate)
        {
            RecurLog.Root.InfoFormat("Retrieving upcoming ACH charge notifications on rundate: {0}", rundate.ToString("yyyy/MM/dd"));

            DataRecurring objDR = new DataRecurring();

            DataTable dt = objDR.GetScheduleRecurringPayments_Notifications(rundate);

            if (dt != null && dt.Rows.Count > 0)
            {
                RecurLog.Root.InfoFormat("Upcoming ACH charge notification retrieved: {0}", dt.Rows.Count);

                // handle single customer emails
                HandleSingleCustomerEmails(dt);

                // handle merchant digest emails
                HandleMerchantDigestEmails(dt);

                // we're not going to do this now
                //HandleMerchantSilentPosts(dt);
            }
            else
            {
                RecurLog.Root.InfoFormat("No Upcoming ACH charge notifications retrieved.");
            }
        }

        private static void HandleMerchantSilentPosts(DataTable dt)
        {
            List<DataRow> li = GetMerchantList(dt);

            string url_silent_post = null;
            string post_string = null;

            foreach (DataRow dr in li)
            {
                url_silent_post = CommonUtility.Util.if_s(dr["URLSilentPost"]);

                if (!string.IsNullOrWhiteSpace(url_silent_post))
                {
                    post_string = GetMerchantSilentPostString(dr);
                    CommonUtility.HttpHandler.HttpPost(url_silent_post, post_string, null, "Post");
                }
            }


        }

        private static string GetMerchantSilentPostString(DataRow dr)
        {
            Dictionary<string, string> di = new Dictionary<string, string>();

            di.Add("NotificationID", dr["NotificationID"].ToString());
            di.Add("Recur_ID", dr["Recur_ID"].ToString());
            di.Add("CustomerFirstName", dr["CustomerFirstName"].ToString());
            di.Add("CustomerLastName", dr["CustomerLastName"].ToString());
            di.Add("Last4", dr["Last4"].ToString());
            di.Add("Amount", dr["Amount"].ToString());
            di.Add("FutureBillingDate", dr["FutureBillingDate"].ToString());
            di.Add("ProductDesc", dr["ProductDesc"].ToString());

            return CommonUtility.Util.DictToUrl(di);
        }

        private static void HandleMerchantDigestEmails(DataTable dt)
        {
            RecurLog.Root.Info("Processing upcoming recurring MERCHANT emails......");

            List<DataRow> li = GetMerchantList(dt);

            string subject_merchant = "Payment Notification. Upcoming ACH Charges on Customers";
            MPSEmailTemplate etMerchant = PaymentXP.DataObjects.DataMPSEmailTemplate.Instance.GetTemplate("UpcomingRecurring_Merchant");
            StringBuilder sb = new StringBuilder();
            Dictionary<string, List<DataRow>> diMerchant = new Dictionary<string, List<DataRow>>();
            Dictionary<string, string> myDict = new Dictionary<string, string>();

            string table_middle = @"
<tr>
    <td>{0}</td>
    <td>{1}</td>
    <td>{2:C2}</td>
    <td>{3}</td>
    <td>{4}</td>
    <td>{5}</td>
</tr>";
            

            // group each recurring transaction by merchant.
            foreach (DataRow dr in li)
            {
                string merchantAppUid = CommonUtility.Util.if_s(dr["merchantappuid"]);

                if (!diMerchant.ContainsKey(merchantAppUid))
                {
                    diMerchant.Add(merchantAppUid, new List<DataRow>());
                }

                diMerchant[merchantAppUid].Add(dr);
            }

            // foreach each merchant, build the list, and send the email.            

            foreach (KeyValuePair<string, List<DataRow>> kvp in diMerchant)
            {
                // each merchant

                string merchantAppUid = kvp.Key;

                RecurLog.Root.InfoFormat("Generating upcoming recurring MERCHANT email for MerchantAppUID={0}......", merchantAppUid);

                string merchant_email = CommonUtility.Util.if_s(kvp.Value[0]["MerchantEmail"]);
                string MerchantFromEmail = CommonUtility.Util.if_s(kvp.Value[0]["MerchantFromEmail"]);
                string NoReplyEmail = string.Format("no-reply@{0}", CommonUtility.Email.GetDomainFromEmail(MerchantFromEmail));

                RecurLog.Root.InfoFormat("Merchant Email: {0}", merchant_email);

                myDict = new Dictionary<string, string>();
                myDict = FillTemplateVars(kvp.Value[0]);

                sb.Clear();

                RecurLog.Root.InfoFormat("Upcoming recurring charges for MerchantAppUID={0}: {1}", merchantAppUid, kvp.Value.Count);
                
                foreach (DataRow dr in kvp.Value)
                {
                    sb.AppendFormat(table_middle
                        , dr["Recur_ID"].ToString()
                        , dr["Last4"].ToString()
                        , CommonUtility.Util.if_dec(dr["Amount"], 0)
                        , DateTime.Parse(dr["FutureBillingDate"].ToString()).ToShortDateString()
                        , dr["CustomerFirstName"].ToString() + " " + dr["CustomerLastName"].ToString()
                        , dr["ScheduleDescription"].ToString()
                        );
                }
                myDict["mybody"] = sb.ToString();

                SendRecurringNotification(subject_merchant, myDict, etMerchant, merchant_email, NoReplyEmail, merchantAppUid);

            }

            RecurLog.Root.Info("Upcoming recurring MERCHANT emails processed.");
        }

        private static void HandleSingleCustomerEmails(DataTable dt)
        {
            List<DataRow> drCustomer = GetCustomerList(dt);

            MPSEmailTemplate etCustomer = PaymentXP.DataObjects.DataMPSEmailTemplate.Instance.GetTemplate("UpcomingRecurring_Customer");

            foreach (DataRow dr in drCustomer)
            {
                int notification_id = CommonUtility.Util.if_i(dr["NotificationID"], 0);
                string customer_email = CommonUtility.Util.if_s(dr["CustomerEmail"]);
                int recurId = CommonUtility.Util.if_i(dr["Recur_ID"], 0);

                RecurLog.Root.InfoFormat("Generating upcoming recurring charge CUSTOMER email [RecurID={1}] for '{0}'......", customer_email, recurId);

                // private label aware
                string MerchantFromEmail = CommonUtility.Util.if_s(dr["MerchantFromEmail"]);
                string NoReplyEmail = string.Format("no-reply@{0}", CommonUtility.Email.GetDomainFromEmail(MerchantFromEmail));

                string subject_customer = "Upcoming ACH Charge";

                Dictionary<string, string> myDict = FillTemplateVars(dr);

                string merchantappUID = CommonUtility.Util.if_s(dr["merchantappuid"]);

                RecurLog.Root.Info("Upcoming recurring charge CUSTOMER email generated.");

                try
                {
                    SendRecurringNotification(subject_customer, myDict, etCustomer, customer_email, MerchantFromEmail, merchantappUID);
                }
                catch (Exception ex)
                {
                    RecurLog.Root.ErrorFormat("A {0} has occurred while sending CUSTOMER Upcoming Recurring notification: {1}", ex.GetType().Name, ex.Message);
                    RecurLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                    CommonUtility.Email.SendEmail("Error: Recurring Payment Notification", ex.Message, PaymentXP.BusinessObjects.Constants.DONOT_REPLYMERITUS_PAYMENT, PaymentXP.BusinessObjects.Constants.DEVELOPERS_EMAIL);
                }
            }
        }

        private static List<DataRow> GetCustomerList(DataTable dt)
        {
            List<DataRow> li = new List<DataRow>();

            RecurLog.Root.InfoFormat("Retreiving customer email list......");

            foreach (DataRow dr in dt.Rows)
            {
                RecurringNotificationType rnt = (RecurringNotificationType)CommonUtility.Util.if_i(dr["NotificationID"], 0);

                if (rnt == RecurringNotificationType.Customer || rnt == RecurringNotificationType.MerchantAndCustomer)
                {
                    li.Add(dr);
                }
            }

            RecurLog.Root.InfoFormat("Customer email list retrieved: {0}", li.Count);

            return li;
        }

        private static List<DataRow> GetMerchantList(DataTable dt)
        {
            List<DataRow> li = new List<DataRow>();

            RecurLog.Root.InfoFormat("Retreiving merchant email list......");

            foreach (DataRow dr in dt.Rows)
            {
                RecurringNotificationType rnt = (RecurringNotificationType)CommonUtility.Util.if_i(dr["NotificationID"], 0);

                if (rnt == RecurringNotificationType.Merchant || rnt == RecurringNotificationType.MerchantAndCustomer)
                {
                    li.Add(dr);
                }
            }

            RecurLog.Root.InfoFormat("Merchant email list retrieved: {0}", li.Count);

            return li;
        }

        private static void SendRecurringNotification(string mysubject, Dictionary<string, string> myDict, MPSEmailTemplate et, string myto, string myfrom, string merchantAppUid)
        {
            string mybody = string.Empty;

            CommonUtility.Template t = new Template();

            RecurLog.Root.Info("Filling upcoming recurring email template......");

            if (t.getTemplateAndFill_FromString(et.Content, myDict, ref mybody))
            {
                RecurLog.Root.InfoFormat("Upcoming recurring email template filled, sending email to '{0}'......", myto);

                MerchantFacade.SendEmail(mysubject, mybody, mybody, myfrom, myto, string.Empty, string.Empty, null, merchantAppUid, "RECURRING_PAYMENTS");

                RecurLog.Root.Info("Upcoming recurring email notification sent!");
            }
            else
            {
                throw new Exception("Failed to send Upcoming Recurring notification to '" + myto + "'.");
            }
        }

        private static Dictionary<string, string> FillTemplateVars(DataRow dr)
        {
            Dictionary<string, string> di = null;

            if (dr != null)
            {
                di = new Dictionary<string, string>();
                di["CustomerFirstName"] = CommonUtility.Util.if_s(dr["CustomerFirstName"]);
                di["CustomerLastName"] = CommonUtility.Util.if_s(dr["CustomerLastName"]);
                di["Amount"] = string.Format("{0:C2}", CommonUtility.Util.if_dec(dr["Amount"], 0));
                di["FutureChargeDate"] = DateTime.Parse(CommonUtility.Util.if_s(dr["FutureBillingDate"])).ToShortDateString();
                di["BusinessDBAName"] = CommonUtility.Util.if_s(dr["BusinessDBAName"]);
                di["Last4"] = CommonUtility.Util.if_s(dr["Last4"]);
                di["Description"] = CommonUtility.Util.if_s(dr["ProductDesc"]);
                di["MID"] = CommonUtility.Util.if_s(dr["MID"]);

                di["BusinessAddress"] = CommonUtility.Formatting.FormatAddress("", CommonUtility.Util.if_s(dr["BusinessAddress"]), CommonUtility.Util.if_s(dr["BusinessAddress2"]), CommonUtility.Util.if_s(dr["BusinessCity"]), CommonUtility.Util.if_s(dr["BusinessState"]), CommonUtility.Util.if_s(dr["BusinessZip"]), Formatting.eAddressFormatBreaks.htmlbr);
                di["BusinessPhone"] = CommonUtility.Util.if_s(dr["BusinessDBAPhone"]);

                di["PrivateLabelName"] = CommonUtility.Util.if_s(dr["PrivateLabelName"]);
                di["PrivateLabelPhoneNumber"] = CommonUtility.Util.if_s(dr["PrivateLabelPhoneNumber"]);

                di["ScheduleDescription"] = dr["ScheduleDescription"].ToString();
                di["CustomerFirstName"] = dr["CustomerFirstName"].ToString();
                di["CustomerLastName"] = dr["CustomerFirstName"].ToString();
                di["ReferenceNumber"] = dr["ReferenceNumber"].ToString();
                di["PONumber"] = dr["PONumber"].ToString();
                di["BillingInfo"] = CommonUtility.Formatting.FormatAddress("", CommonUtility.Util.if_s(dr["BillingAddress"]), "", CommonUtility.Util.if_s(dr["BillingCity"]), CommonUtility.Util.if_s(dr["BillingState"]), CommonUtility.Util.if_s(dr["BillingZip"]), Formatting.eAddressFormatBreaks.htmlbr);
                di["ShippingInfo"] = CommonUtility.Formatting.FormatAddress("", CommonUtility.Util.if_s(dr["ShippingAddress1"]), CommonUtility.Util.if_s(dr["ShippingAddress2"]), CommonUtility.Util.if_s(dr["ShippingCity"]), CommonUtility.Util.if_s(dr["ShippingState"]), CommonUtility.Util.if_s(dr["ShippingZip"]), Formatting.eAddressFormatBreaks.htmlbr);

                
                // if merchant has logo, then display it, otherwise dont.
                MPSLogo merchantlogo = PaymentXP.DataObjects.DataMPSLogo.Instance.GetLogo(new Hashtable() { { "@ForeignID", CommonUtility.Util.if_i(dr["ZID"], 0) } });
                if (merchantlogo != null && merchantlogo.ForeignID != -1)
                {
                    di["headerimage"] = "https://www.paymentxp.com/NotLoggedIn/frmLoadLogo.aspx?MerchantID=" + CommonUtility.Util.if_i(dr["ZID"], 0).ToString();
                    di["headertext"] = CommonUtility.Util.if_s(dr["BusinessDBAName"]);
                }

            }

            return di;
        }

        #endregion
    }
}
