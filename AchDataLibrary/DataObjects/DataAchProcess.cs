using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;


namespace Nmc.Ach.Dal
{
    public class DataAchProcess
    {

        public DataSet Select()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Scrub_Get_Data";
            cmd.CommandType = CommandType.StoredProcedure;

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SelectScrubRejectDuplicateTrans(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Scrub_Reject_Duplicate_Transaction";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public int ExecuteUpdateCorrectionInfo()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Scrub_Correction_Info";
            cmd.CommandType = CommandType.StoredProcedure;

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader ExecuteScrubData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Scrub_Process";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public int ExecuteScrubData2()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Scrub_Process";
            cmd.CommandType = CommandType.StoredProcedure;

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int ExecuteMonthlyBilling(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Journal_Monthly_Billing";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

        }

        public int ExecuteMonthlyMinimum(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Journal_Monthly_Minimum";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

        }

        public bool TruncateStagingReturn()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Truncate_Staging";
            cmd.CommandType = CommandType.StoredProcedure;

            int rows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
            return true;
        }

        public bool InsertIntoBatchFileLog(string strFile, int AchID)
        {
            DataBatchFileLog data = new DataBatchFileLog();
            ArrayList prms = new ArrayList();

            try
            {
                SqlParameter prm = new SqlParameter("@FileID", -1);
                prm.Direction = ParameterDirection.Output;
                prms.Add(prm);
                prms.Add(new SqlParameter("@AchID", AchID));
                prms.Add(new SqlParameter("@FileName", strFile));
                prms.Add(new SqlParameter("@LoadDate", DateTime.Now));
                long lngRows = data.Insert(prms);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                data = null;
            }
        }

        public int GetNextJobID(string JobDesc)
        {
            int JobID = -1;

            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@sp_name", JobDesc));
            SqlDataReader dr = this.SelectNextJobID(prms);
            if (dr.Read())
                JobID = Convert.ToInt32(dr[0]);

            return JobID;

        }

        public int CreateAccountBlock(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Create_Account_Block";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }


        public int AutoResubmitNSF(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Transaction_Resubmit_NSF";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int ScrubAndCreateReturns(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Scrub_Create_Returns";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public int UpdateReturnPrintedFlag(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Update_Print_Flag";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectSettlementData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Get_Data_Settlement";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectLastDateSubmitted(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_SelectLastDateSubmitted";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public int SelectLastTraceNumber(int BankID)
        {

            string sql = "Select LastTraceNumber From dbo.Ach_Bank_Info Where BankID = " + BankID.ToString();

            return Convert.ToInt32(DataLayer.ExecuteScalar(sql, DataLayer.ConnectStringBuild()));
        }

        public int UpdateLastTraceNumber(int BankID, int TraceNumber)
        {
            string sql = "Update dbo.Ach_Bank_Info Set LastTraceNumber = " + TraceNumber.ToString() + " Where BankID = " + BankID.ToString();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }


        public int SelectLastBatchID(int BankID)
        {

            string sql = "Select LastBatchID From dbo.Ach_Bank_Info Where BankID = " + BankID.ToString();

            return Convert.ToInt32(DataLayer.ExecuteScalar(sql, DataLayer.ConnectStringBuild()));
        }

        public int UpdateLastBatchID(int BankID, int BatchID)
        {
            string sql = "Update dbo.Ach_Bank_Info Set LastBatchID = " + BatchID.ToString() + " Where BankID = " + BankID.ToString();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }


        public SqlDataReader SelectNextDayFundingMerchant()
        {
            //string sql = "Select MID,DBA From dbo.Ach_NextDayFunding with (nolock) Where Active = 1 Order By MID";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_SelectNextDayFunding";
            cmd.CommandType = CommandType.StoredProcedure;

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectResponseFileProcess(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Response_Process";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public int SendPReturnEmail(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Process_Email_PReturns";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SelectPendingScrubbedReturns(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Pending_Scrubbed_Returns";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        public DataSet SelectBadData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Batch_Check_Data";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SelectNoLocationID()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_KBT_NoLocationID";
            cmd.CommandType = CommandType.StoredProcedure;


            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public DataSet SelectNOCBadData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Report_NOC_Watcher";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }
        public SqlDataReader SelectResponseFileGetData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Response_Get_Data";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnFileProcess(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Process";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnFileProcessRecreate(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Process_Recreate";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnFileProcessRecreateManual(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Process_Recreate_Manual";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnEmailProcess(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Process_Email";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnFileGetData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Get_Data";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnFileGetData2(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Get_Data2";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectReturnEmailGetData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Return_Get_Data_Email";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }


        public SqlDataReader SelectPaymentData(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_File_Get_Data_Payment";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public SqlDataReader SelectNextJobID(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Select_Next_JobID";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataReader(cmd, DataLayer.ConnectStringBuild());
        }

        public long InsertStagingReturn(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_Insert_Staging_Return";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            int intRows = DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());

            if (intRows > 0)
                return Convert.ToInt64(cmd.Parameters["@ReturnID"].Value);
            else
                return -1;
        }

        /// <summary>
        /// Sync up RecurringLog with any new transaction and their return information
        /// </summary>
        public DataTable SyncRecurringLogWithAchReturns(DateTime rundate)
        {
            DataTable dt = null;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_SyncRecurringLog";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RunDate", rundate));

                dt = DataLayer.GetDataTable(cmd, DataLayer.ConnectStringBuild());
            }
            return dt;
        }

        /// <summary>
        /// Get Qualifying Transactions that can be rebilled through ACH
        /// </summary>
        /// <returns></returns>
        public DataTable GetQualifiedBillRetries()
        {
            DataTable dt = null;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_GetQualifiedBillRetries";
                cmd.CommandType = CommandType.StoredProcedure;

                dt = DataLayer.GetDataTable(cmd, DataLayer.ConnectStringBuild());
            }
            return dt;
        }

        /// <summary>
        /// Get the Count of how many times an original transaction was retried and the configured BillRetry Rule Count
        /// </summary>
        /// <returns></returns>
        public DataTable GetFailedMaxRetries()
        {
            DataTable dt = null;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_GetFailedMaxRetries";
                cmd.CommandType = CommandType.StoredProcedure;

                dt = DataLayer.GetDataTable(cmd, DataLayer.ConnectStringBuild());
            }
            return dt;
        }

        public void InsertDisabledRecurringHistory(int recur_id, long originalTransID)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_InsertDisabledRecurringHistory";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Recur_ID", recur_id));
                cmd.Parameters.Add(new SqlParameter("@OriginalTransID", originalTransID));

                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
            }
        }

        /// <summary>
        /// Mark FinalRetryDate for all transactions in log that were tied to this recur_id (aka: the schedule's Id)
        /// </summary>
        /// <param name="recur_id"></param>
        public void UpdateRecurringLogFinalRetryDate(int recur_id)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_UpdateRecurringLogFinalRetryDate";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Recur_ID", recur_id));

                DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
            }
        }

        public static int InsertCommunication(string strSubject, string strBody, string strBodyHTML, string strFrom, string strTo, string strCC, string strBCC, string MerchantAppUID)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_InsertCommunication";
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter prm = new SqlParameter("@UID", SqlDbType.UniqueIdentifier);
            prm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(prm);
            cmd.Parameters.Add(new SqlParameter("@MerchantAppUID", MerchantAppUID));
            cmd.Parameters.Add(new SqlParameter("@LeadsUID", null));
            cmd.Parameters.Add(new SqlParameter("@Subject", strSubject));
            cmd.Parameters.Add(new SqlParameter("@To", strTo));
            cmd.Parameters.Add(new SqlParameter("@From", strFrom));
            cmd.Parameters.Add(new SqlParameter("@Cc", strCC));
            cmd.Parameters.Add(new SqlParameter("@Bcc", strBCC));
            cmd.Parameters.Add(new SqlParameter("@Body", strBody));
            cmd.Parameters.Add(new SqlParameter("@HTMLBody", strBodyHTML));
            cmd.Parameters.Add(new SqlParameter("@IsEmail", true));
            cmd.Parameters.Add(new SqlParameter("@UserCreated", "System"));
            cmd.Parameters.Add(new SqlParameter("@AgentUID", null));
            cmd.Parameters.Add(new SqlParameter("@EmailBlasterUID", null));
            cmd.Parameters.Add(new SqlParameter("@HasAttachments", null));
            //cmd.Parameters.Add(new SqlParameter("@Timesent", DataLayer.Date2Field(comm.TimeSent)));

            return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringTransDB());


        }

        public DataSet SelectBatchTurnKey(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_Batch_TurnKey";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);
            

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringTransDB());
        }

        public DataSet SelectReportSettlementForecast(ArrayList prms)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "sp_ReportSettlementForecast";
            cmd.CommandType = CommandType.StoredProcedure;
            DataLayer.AppendParamters(cmd, prms);

            return DataLayer.GetDataSet(cmd, DataLayer.ConnectStringBuild());
        }

        //Ani: DM-6867
        public int InsertBankFiles(int bankID, string fileName, decimal totalDebitAmount, decimal totalCreditAmount, int userCreated, DateTime dateCreated, int userModified, DateTime dateModified)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_Insert_BankFiles";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BankId", bankID));
                cmd.Parameters.Add(new SqlParameter("@Name", fileName));
                cmd.Parameters.Add(new SqlParameter("@UserCreated", userCreated));
                cmd.Parameters.Add(new SqlParameter("@DateCreated", dateCreated));
                cmd.Parameters.Add(new SqlParameter("@TotalDebitAmount", totalDebitAmount));
                cmd.Parameters.Add(new SqlParameter("@TotalCreditAmount", totalCreditAmount));
                cmd.Parameters.Add(new SqlParameter("@UserModified", userModified));
                cmd.Parameters.Add(new SqlParameter("@DateModified", dateModified));

                object identityObject =  DataLayer.ExecuteScalar(cmd, DataLayer.ConnectStringBuild());

                if (identityObject != null && identityObject != DBNull.Value)
                {
                    return Convert.ToInt32(identityObject);
                }
                else
                    return -1;
            }
        }

        //Ani: DM-6867
        public int InsertTransactionsToBankFileDetail(int bankFileID, int transactionID)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Ach_Insert_BankFileDetail";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@BankFileID", bankFileID));
                cmd.Parameters.Add(new SqlParameter("@TransactionID", transactionID));

               return DataLayer.ExecuteSQL(cmd, DataLayer.ConnectStringBuild());
            }
        }
    }
}
