using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

using Nmc.Ach.Dal;
using Nmc.Common;

namespace AchQuickbooks
{
    public class Program
    {
        static DateTime m_Date = Convert.ToDateTime("07/12/2007");

        static void Main(string[] args)
        {
            WriteSettlementFile();
            WriteResubmittalFile();
            WriteReturnFile();
        }

        private static void WriteSettlementFile()
        {
            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@PostedDate",m_Date));
            SqlDataReader dr = data.SelectQuickbooksSettlements(prms);

            if (!dr.HasRows)
                return;

            StreamWriter sw = new StreamWriter(@"C:\" + m_Date.ToString("yyyyMMdd") + "_Settlments.iif");
            string line = string.Empty;

            sw.WriteLine("!TRNS\tTRNSID\tTRNSTYPE\tDATE\tACCNT\tCLASS\tAMOUNT\tDOCNUM\tMEMO");
            sw.WriteLine("!SPL\tSPLID\tTRNSTYPE\tDATE\tACCNT\tCLASS\tAMOUNT\tDOCNUM\tMEMO");
            sw.WriteLine("!ENDTRNS");

            while (dr.Read())
            {
                //Credit Revenue
                line = "TRNS\t\tACH\t" + m_Date.ToString("MM/dd/yyyy") + "\t" + dr["AcctNumber"].ToString() + "\t\t";
                line += Convert.ToString(DataLayer.Field2Dec(dr["Amount"]) * -1) + "\t\t" + dr["TransactionType"].ToString();
                sw.WriteLine(line);

                //Debit Cash
                line = "SPL\t\tACH\t" + m_Date.ToString("MM/dd/yyyy") + "\t" + "1-10600" + "\t\t";
                line += dr["Amount"].ToString() + "\t\t" + dr["TransactionType"].ToString();
                sw.WriteLine(line);

                //Terminate Transaction
                sw.WriteLine("ENDTRNS");
                

            }

            sw.Close();
        }

        private static void WriteResubmittalFile()
        {
            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@PostedDate", m_Date));
            SqlDataReader dr = data.SelectQuickbooksResubmittals(prms);

            if (!dr.HasRows)
                return;

            StreamWriter sw = new StreamWriter(@"C:\" + m_Date.ToString("yyyyMMdd") + "_Resubmittals.iif");
            string line = string.Empty;

            sw.WriteLine("!TRNS\tTRNSID\tTRNSTYPE\tDATE\tACCNT\tCLASS\tAMOUNT\tDOCNUM\tMEMO");
            sw.WriteLine("!SPL\tSPLID\tTRNSTYPE\tDATE\tACCNT\tCLASS\tAMOUNT\tDOCNUM\tMEMO");
            sw.WriteLine("!ENDTRNS");

            while (dr.Read())
            {
                //Credit Accounts Receivable
                line = "TRNS\t\tACH\t" + m_Date.ToString("MM/dd/yyyy") + "\t" + "1-11000" + "\t\t";
                line += Convert.ToString(DataLayer.Field2Dec(dr["Amount"]) * -1) + "\t\t" + dr["TransactionType"].ToString();
                sw.WriteLine(line);

                //Debit Cash
                line = "SPL\t\tACH\t" + m_Date.ToString("MM/dd/yyyy") + "\t" + "1-10600" + "\t\t";
                line += dr["Amount"].ToString() + "\t\t" + dr["TransactionType"].ToString();
                sw.WriteLine(line);

                //Terminate Transaction
                sw.WriteLine("ENDTRNS");


            }

            sw.Close();
        }

        private static void WriteReturnFile()
        {
            DataTransaction data = new DataTransaction();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@PostedDate", m_Date));
            SqlDataReader dr = data.SelectQuickbooksReturns(prms);

            if (!dr.HasRows)
                return;

            StreamWriter sw = new StreamWriter(@"C:\" + m_Date.ToString("yyyyMMdd") + "_Returns.iif");
            string line = string.Empty;


            sw.WriteLine("!TRNS\tTRNSID\tTRNSTYPE\tDATE\tACCNT\tCLASS\tAMOUNT\tDOCNUM\tMEMO\tName");
            sw.WriteLine("!SPL\tSPLID\tTRNSTYPE\tDATE\tACCNT\tCLASS\tAMOUNT\tDOCNUM\tMEMO\tName");
            sw.WriteLine("!ENDTRNS");

            while (dr.Read())
            {
                //Debit Accounts Receivable
                line = "TRNS\t\tACH\t" + m_Date.ToString("MM/dd/yyyy") + "\t" + "1-12750" + "\t\t";
                line += dr["Amount"].ToString() + "\t\t" + dr["TransactionType"].ToString();
                sw.WriteLine(line);

                //Debit Cash
                line = "SPL\t\tACH\t" + m_Date.ToString("MM/dd/yyyy") + "\t" + "1-10602" + "\t\t";
                line += Convert.ToString(DataLayer.Field2Dec(dr["Amount"]) * -1) + "\t\t" + dr["TransactionType"].ToString() ;
                sw.WriteLine(line);


                //Terminate Transaction
                sw.WriteLine("ENDTRNS");


            }

            sw.Close();
        }
    }
}
