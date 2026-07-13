using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace ACH2007
{
    public class AchProcessFacade
    {
        Bank m_Bank;

        public AchProcessFacade(Bank bank)
        {
            m_Bank = bank;
        }

        public Bank Bank
        {
            get { return m_Bank; }
            set { m_Bank = value; }
        }

        public bool CreateBatch()
        {
            try
            {
                List<Merchant> merchants = m_Bank.GetPendingBatchMerchants();

                if (merchants.Count == 0)
                {
                    FormHandler.DispalyWarningMessage("No batches to process.");
                    return false;
                }
                else
                {
                    DataAchProcess data = new DataAchProcess();
                    ArrayList prms = new ArrayList();
                    prms.Add(new SqlParameter("@sp_name","Create Batches for BankID " + m_Bank.BankID.ToString()));
                    SqlDataReader dr = data.SelectNextJobID(prms);

                    if (dr.Read())
                        m_Bank.CurrentJobID = Convert.ToInt32(dr[0]);

                    foreach (Merchant merchant in merchants)
                    {
                        m_Bank.CreateBatch(merchant);
                    }
                }
                FormHandler.DispalyInformationMessage("Batches created successfully for bank " + m_Bank.BankName + ".");

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Batch process failed.  Please contact system administrator.",exc);
                return false;
            }

        }

  

        public bool CreateJournal()
        {
            try
            {
                List<Merchant> merchants = m_Bank.GetPendingJournalMerchants();

                if (merchants.Count == 0)
                {
                    FormHandler.DispalyInformationMessage("No journals to process.");
                    return false;
                }
                else
                {
                    DataAchProcess data = new DataAchProcess();
                    ArrayList prms = new ArrayList();
                    prms.Add(new SqlParameter("@sp_name", "Create Journal for BankID " + m_Bank.BankID.ToString()));
                    SqlDataReader dr = data.SelectNextJobID(prms);
                    if (dr.Read())
                        m_Bank.CurrentJobID = Convert.ToInt32(dr[0]);

                    foreach (Merchant merchant in merchants)
                    {
                        foreach (Batch batch in merchant.Batches)
                        {
                            m_Bank.CreateJournal(batch);
                        }
                    }
                }

                m_Bank.JournalReleaseHold();

                FormHandler.DispalyInformationMessage("Journals created successfully for bank " + m_Bank.BankName + ".");

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Journal process failed.   Please contact system administrator.", exc);
                return false;
            }

        }

        public bool CreateResponseFiles()
        {
            try
            {
                List<Merchant> merchants = m_Bank.GetPendingResponseFileMerchants();

                if (merchants.Count == 0)
                {
                    FormHandler.DispalyInformationMessage("No response files to process.");
                    return false;
                }
                else
                {
                    foreach (Merchant merchant in m_Bank.GetPendingResponseFileMerchants())
                    {
                        m_Bank.WriteResponseFile(merchant);
                    }
                }

                FormHandler.DispalyInformationMessage("Response files create successfully for bank " + m_Bank.BankName + ".");

                return true;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Create response files failed.   Please contact system administrator.", exc);
                return false;
            }

        }



       

    }
}
