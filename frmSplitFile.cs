using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects.Logging;

namespace AchSystem
{
    public partial class frmSplitFile : Form
    {
        private ILogging NcalSplitlog = new Log();

        Hashtable m_col = new Hashtable();
        Hashtable m_merchants = new Hashtable();
        Hashtable m_TurnKey_Merchants = new Hashtable();

        string m_BankcardFileDirectory = ConfigurationManager.AppSettings["BankcardFileDirectory"];
        string m_BankcardFileOldDirectory = ConfigurationManager.AppSettings["BankcardFileOldDirectory"];
        string m_SettlementFileDirectory = ConfigurationManager.AppSettings["SettlementFileDirectory"];


        decimal m_TodayFileTotalDebit = 0;
        decimal m_TodayFileTotalCredit = 0;

        decimal m_NDFDebit = 0;
        decimal m_NDFCredit = 0;

        decimal m_PriorDayTotalDebit = 0;
        decimal m_PriorDayTotalCredit = 0;

        decimal m_SplitFileTotalDebit = 0;
        decimal m_SplitFileTotalCredit = 0;

        DateTime m_today = DateTime.Today;
        DateTime m_priorday = DateTime.Today;
        DateTime m_sunday = DateTime.Today;

        bool m_IsTodayHoliday = false;
        DateTime m_NextBankDay = DateTime.Today;

        public frmSplitFile()
        {
            InitializeComponent();
            NcalSplitlog = new Logging(Logging.NCALSplit);
        }

        private void frmSplitFile_Load(object sender, EventArgs e)
        {
            DisplayCreditCardFiles();
            LoadMerchants();
            //ShowDates();

            string d = DataLayer.ExecuteScalar("select dbo.fn_IsTodayHoliday('" + m_today.ToShortDateString() + "')", DataLayer.ConnectStringBuild());

            if (d == "Y")
            {
                m_IsTodayHoliday = true;
            }

            m_NextBankDay = DateTime.Parse(DataLayer.ExecuteScalar("select dbo.fn_GetNextBankDay('" + m_today.ToShortDateString() + "')", DataLayer.ConnectStringBuild()));

        }

        private void ShowDates()
        {
            m_today = DateTime.Today;
            //m_today = DateTime.Parse("5/19/2015");//DateTime.Today;

            //m_today = DateTime.Parse("10/06/2014");//DateTime.Today;
            //DateTime m_today = DateTime.Parse(textBox1.Text);//DateTime.Today;

            string d = DataLayer.ExecuteScalar("select dbo.fn_GetPrevWorkingDay('" + m_today.ToShortDateString() + "')", DataLayer.ConnectStringBuild());

            m_priorday = DateTime.Parse(d);

            if (m_today.DayOfWeek == DayOfWeek.Monday)
            {
                m_sunday = m_today.AddDays(-1);
            }
            //if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
            //    priorday = priorday.AddDays(-3);
            //else
            //    priorday = priorday.AddDays(-1);

            txtToday.Text = m_BankcardFileDirectory + "CMRD3062." + m_today.ToString("MMddyyyy") + ".DAT";
            txtPriorDay.Text = m_BankcardFileDirectory + "CMRD3062." + m_priorday.ToString("MMddyyyy") + ".DAT";
            txtOutput.Text = m_SettlementFileDirectory + "CMRD3062." + m_today.ToString("MMddyyyy") + ".DAT";

            if (m_today.DayOfWeek == DayOfWeek.Monday)
            {
                txtSunday.Text = m_BankcardFileDirectory + "CMRD3062." + m_sunday.ToString("MMddyyyy") + ".DAT";
                txtSundayOutput.Text = m_SettlementFileDirectory + "CMRD3062." + m_sunday.ToString("MMddyyyy") + ".DAT";
            }
            else
            {
                txtSunday.Text = string.Empty;
                txtSundayOutput.Text = string.Empty;
            }

            ToggleDisplay();
        }

        private void ToggleDisplay()
        {
            bool show = false;

            if (m_today.DayOfWeek == DayOfWeek.Monday)
            {
                show = true;
            }

            grpWeekend.Visible = show;

        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            SendReturnEmail();
            MessageBox.Show("Email Sent!");
        }

        public void SendReturnEmail()
        {
            string strMessage = string.Empty;


            strMessage = "<p>Please process our ACH file below. Thanks.</p>";
            strMessage += "<p><table>";
            strMessage += "<tr><td><b>Total Debits:</b></td><td>$" + txtSplitFileTotalDebit.Text + "</td><td>";
            strMessage += "<tr><td><b>Total Credits:</b></td><td> $" + txtSplitFileTotalCredit.Text + "</td><td>";
            strMessage += "</p></table>";

            strMessage += "<p>Mark Nguyen<br>";
            strMessage += "Meritus Payment Solutions <br>";
            strMessage += "Vice President of IT<br>";
            strMessage += "Phone: 949-788-1010 Ext.120<br>";
            strMessage += "Fax: 949-222-0809<br>";
            strMessage += "mnguyen@merituspayment.com<br>";
            strMessage += "www.merituspayment.com<br></p>";

            DataAchProcess.InsertCommunication("ACH File (CC Settlement)", strMessage, strMessage, "Ach@merituspayment.com", "ach@nbcal.com;bankcard@nbcal.com", "", "mnguyen@merituspayment.com;rnguyen@merituspayment.com;knguyen@merituspayment.com", "CE143F92-6023-4873-87C1-6C20E1689032");


        }

        private void LoadMerchants()
        {
            DataAchProcess data = new DataAchProcess();
            SqlDataReader dr = data.SelectNextDayFundingMerchant();

            m_merchants.Clear();
            m_TurnKey_Merchants.Clear();
            lstMerchants.Items.Clear();
            while (dr.Read())
            {
                m_merchants.Add(dr["mid"].ToString(), dr["dba"].ToString());
                lstMerchants.Items.Add(dr["mid"].ToString() + " - " + dr["dba"].ToString());

                switch (dr["parentzid"].ToString()) //TurnKey merchants
                {
                    case "42013":
                    case "42012":
                        m_TurnKey_Merchants.Add(dr["mid"].ToString(), dr["dba"].ToString());
                        break;
                }
            }

            dr.Close();
        }



        private void btnBrowse_Click(object sender, EventArgs e)
        {
            ofdSelect.ShowDialog();

            if (!string.IsNullOrEmpty(ofdSelect.FileName))
                txtToday.Text = ofdSelect.FileName;
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            
            //this.ShowDates();

            //if (!File.Exists(txtPriorDay.Text))
            //{
            //    MessageBox.Show("Prior day's file does not exists", "File Not Found", MessageBoxButtons.OK);
            //    return;
            //}

            //if (!File.Exists(txtToday.Text))
            //{
            //    MessageBox.Show("Today's file does not exists", "File Not Found", MessageBoxButtons.OK);
            //    return;
            //}

            NcalSplitlog.Info("NCAL Split start");

            Bank bank = new BankFDR("FDR", 14, NcalSplitlog);
            NachaFile today = null;
            NachaFile todayNew = null;
            
            bool perform = false;

            try
            {


                this.Cursor = Cursors.WaitCursor;


                if (lstFiles.Items.Count > 0)
                {
                    for (int i = 0; i < lstFiles.Items.Count; i++)
                    {
                        FileInfo file = new FileInfo(lstFiles.Items[i].ToString());
                        m_today = DateTime.Parse(file.Name.Substring(9, 2) + "/" + file.Name.Substring(11, 2) + "/" + file.Name.Substring(13, 4));
                        txtOutput.Text = m_SettlementFileDirectory + "CMRD3062." + m_today.ToString("MMddyyyy") + ".DAT";
                        Logging.NCALSplit.InfoFormat("Process NCALSplit for the File: {0} ", file.FullName);
                        perform = bank.ParseSettlementFile(ref today, file);
                        perform = SplitFileNew(today, txtOutput.Text);

                        if (perform)
                        {
                            FileInfo fileNew = new FileInfo(txtOutput.Text);

                            bank.ParseSettlementFile(ref todayNew, fileNew);

                            if (!(today.TotalDebit == todayNew.TotalDebit && today.TotalCredit == todayNew.TotalCredit))
                            {
                                Logging.NCALSplit.ErrorFormat("ERROR: File totals do not match. SourceTotalDebit: {0}, OutPutTotalDebit: {1}, SourceTotalCredit: {3}, OutPutTotalCredit: {4} ", today.TotalDebit, todayNew.TotalDebit, today.TotalCredit, todayNew.TotalCredit);
                                MessageBox.Show("ERROR: File totals do not match. Please contact IT.");
                                perform = false;
                            }
                        }
                       
                        if (perform)
                        {
                            if (!File.Exists(m_BankcardFileOldDirectory + file.Name))
                                file.MoveTo(m_BankcardFileOldDirectory + file.Name);
                            else
                                file.MoveTo(m_BankcardFileOldDirectory + file.Name + "_" + DateTime.Now.ToString("MMddyyyy HHmmss"));
                        }
                    }

                    //this.SendPReturnEmail();
                }

                if (perform)
                {
                    MessageBox.Show("Process completed successfully.");
                }
                this.DisplayCreditCardFiles();
                this.Cursor = Cursors.Default;


                

                //if (!string.IsNullOrEmpty(txtToday.Text))
                //{
                //    FileInfo fiToday = null;
                //    FileInfo fiPriorday = null;
                //    FileInfo fiSunday = null;

                //    if (!string.IsNullOrEmpty(txtToday.Text))
                //    {
                //        fiToday = new FileInfo(txtToday.Text);
                //        perform = bank.ParseSettlementFile(ref today, fiToday);

                //        SplitFileNew(today, txtOutput.Text);
                //    }

                    //if (!string.IsNullOrEmpty(txtPriorDay.Text))
                    //{
                    //    fiPriorday = new FileInfo(txtPriorDay.Text);
                    //    perform = bank.ParseSettlementFile(ref priorday, fiPriorday);
                    //}

                    //if (!string.IsNullOrEmpty(txtSunday.Text))
                    //{
                    //    fiSunday = new FileInfo(txtSunday.Text);
                    //    perform = bank.ParseSettlementFile(ref sunday, fiSunday);
                    //}

                   

                    //if (perform)
                    //{
                    //    perform = SplitFile(today, priorday);

                    //    if (m_today.DayOfWeek == DayOfWeek.Monday)
                    //    {
                    //        perform = SplitSundayFile(sunday);
                    //    }
                    //}

                    ///* today's file */
                    //txtTodayFileTotalDebit.Text = m_TodayFileTotalDebit.ToString("###,##0.00");
                    //txtTodayFileTotalCredit.Text = m_TodayFileTotalCredit.ToString("###,##0.00");
                    //txtTodayFileTotal.Text = Convert.ToDecimal(m_TodayFileTotalDebit - m_TodayFileTotalCredit).ToString("###,##0.00");

                    //txtNDFDebit.Text = m_NDFDebit.ToString("###,##0.00");
                    //txtNDFCredit.Text = m_NDFCredit.ToString("###,##0.00");
                    //txtNDFNet.Text = Convert.ToDecimal(m_NDFDebit - m_NDFCredit).ToString("###,##0.00");

                    //txtPriorDayTotalDebit.Text = m_PriorDayTotalDebit.ToString("###,##0.00");
                    //txtPriorTotalCredit.Text = m_PriorDayTotalCredit.ToString("###,##0.00");
                    //txtPriorTotalNet.Text = Convert.ToDecimal(m_PriorDayTotalDebit - m_PriorDayTotalCredit).ToString("###,##0.00");

                    //txtSplitFileTotalDebit.Text = m_SplitFileTotalDebit.ToString("###,##0.00");
                    //txtSplitFileTotalCredit.Text = m_SplitFileTotalCredit.ToString("###,##0.00");

                    ///* sunday's file */
                    //txtSundayFileTotalDebit.Text = m_TodayFileTotalDebit.ToString("###,##0.00");
                    //txtSundayFileTotalCredit.Text = m_TodayFileTotalCredit.ToString("###,##0.00");
                    //txtSundayFileTotal.Text = Convert.ToDecimal(m_TodayFileTotalDebit - m_TodayFileTotalCredit).ToString("###,##0.00");

                    //if (perform)
                    //{
                    //    MessageBox.Show("Process completed successfully for Weekday file!\n Path: " + txtOutput.Text);

                    //    if (m_today.DayOfWeek == DayOfWeek.Monday)
                    //    {
                    //        MessageBox.Show("Process completed successfully for Weekend file!\n Path: " + txtSundayOutput.Text);
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("No next funding merchants in today's file.");
                    //}

                //}



            }
            catch (Exception exc)
            {
                MessageBox.Show("Error - btnSplit_Click: " + exc.Message);
            }

        }

        private NachaFile CreateNachaFile(NachaFile file)
        {
            Bank bank = new BankFDR("FDR", 14);
            NachaFile s1 = new SettlementFile(bank);

            bank.OriginatingTransRoute = "122039360";
            bank.OriginatingAccountNo = "2549980";


            s1.FileBatchNumber = 1;
            s1.ImmediateDestination = " 122043482";
            s1.Immediate_Origin = "1262437777";
            s1.FileCreationDate = DateTime.Now.ToString("yyMMdd");
            s1.FileCreationTime = DateTime.Now.ToString("HHmm");
            s1.DestinationName = "NBCal";
            s1.OriginName = "MERITUS BANKCARD";
            s1.ReferenceCode = "DLY MRCH";

            return s1;

        }

        private bool TurnKeySplitCheck(string mid, decimal BatchNetAmount, decimal TransSum, decimal TransCustom1Sum, decimal TransCustom2Sum, string TurnKey_ABA, string TurnKey_DDA
            , string DistributorABA, string DistributorDDA, SettlementDetail d, DateTime date)
        {
            DataAchProcess data = new DataAchProcess();
            bool perform = false;

            DateTime[] dates = new DateTime[]
                                {
                                    date.AddDays(-1),
                                    date
                                };

            foreach (DateTime dt in dates)
            {


                ArrayList prms = new ArrayList();
                prms.Add(new SqlParameter("@Mid", mid));
                prms.Add(new SqlParameter("@Today", date));

                DataSet ds = data.SelectBatchTurnKey(prms);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
                    TransSum = DataLayer.Field2Dec(dr["TransSum"]);
                    TransCustom1Sum = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
                    TransCustom2Sum = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
                    TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
                    TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                    DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
                    DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
                }

                //only split if there are values in customfield1 or customfield2
                if (TransCustom1Sum != 0 || TransCustom2Sum != 0)
                {
                    //make batch, transaction total, and ach amount matches
                    bool AmountsEqual = d.Amount == BatchNetAmount && d.Amount == TransSum;

                    //make sure split amounts are not greater than ach amount
                    if (AmountsEqual && BatchNetAmount > (TransCustom1Sum + TransCustom2Sum))
                    {
                        perform = true;
                    }
                    else
                    {
                        perform = false;
                    }
                }

            } //foreach

            if (!perform)
            {
                MessageBox.Show("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
            }

            return perform;
        }
        //private bool SplitFile(NachaFile today, NachaFile priorday)
        //{
        //    DataAchProcess data = new DataAchProcess();
        //    NachaFile s1 = null;
        //    NachaBatch batch = null;
        //    NachaDetail detail = null;
        //    SettlementAddenda addenda = null;
        //    StreamWriter sw = null;
        //    int remainder = 0;
        //    bool createOffset = false;

        //    m_TodayFileTotalDebit = 0;
        //    m_TodayFileTotalCredit = 0;


        //    m_NDFDebit = 0;
        //    m_NDFCredit = 0;

        //    m_PriorDayTotalDebit = 0;
        //    m_PriorDayTotalCredit = 0;



        //    m_SplitFileTotalDebit = 0;
        //    m_SplitFileTotalCredit = 0;

        //    /* Get last tracenumber and batchid */
        //    int DetailCount = 0;

        //    s1 = CreateNachaFile(today);

        //    /* Get last batchid */
        //    s1.FileBatchNumber = data.SelectLastBatchID(14);

        //    /* Extract next day funding merchants out of today's file */
        //    foreach (SettlementBatch b in today.Batches)
        //    {
        //        if (createOffset)
        //        {
        //            NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
        //            createOffset = false;

        //            //if (r != null)
        //            //{
        //            //    if (r.TransType.Substring(1, 1) == "7")
        //            //        m_NDFDebit += r.Amount;
        //            //    else
        //            //        m_NDFCredit += r.Amount;
        //            //}
        //        }


        //        batch = null;


        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            string mid = d.IdentificationNumber;

        //            //if mid is next day funding

        //            if (mid.Substring(0, 4) != "8713")
        //            {
        //                if (m_merchants.ContainsKey(mid))
        //                {
        //                    //create batch does not exists
        //                    if (batch == null)
        //                    {
        //                        createOffset = true;
        //                        batch = s1.CreateBatch();
        //                        batch.CompanyName = b.CompanyName;
        //                        batch.DiscretionaryData = b.DiscretionaryData;
        //                        batch.CompanyIdentification = "1262437777";
        //                        batch.Secc = b.Secc;
        //                        batch.CompanyDescription = b.CompanyDescription;
        //                        batch.CompanyDescriptiveDate = b.CompanyDescriptiveDate;
        //                        batch.EntyDate = m_NextBankDay.ToString("yyMMdd"); // DataLayer.Field2Date(b.EntyDate).ToString("yyMMdd");
        //                        batch.OriginatingDFI = "122043482";//b.OriginatingDFI;
        //                        batch.BatchNumber = b.BatchNumber;
        //                        s1.AddBatch(batch);
        //                    }

        //                    //Create new detail record

        //                    //Determine if this is TurnKey merchant and apply split based on custom fields in payment xp
        //                    //TurnKey vending machines could split 3 ways.
        //                    //TurnKey split $           - CustomField1
        //                    //Operations split $        - CustomField2 
        //                    //Operations aba            - CustomField3
        //                    //Operations dda            - CustomField4

        //                    decimal BatchNetAmount = 0;
        //                    decimal TransSum = 0;
        //                    decimal TransCustom1Sum = 0; //turn key split
        //                    decimal TransCustom2Sum = 0; //distributor split
        //                    string DistributorABA = "";
        //                    string DistributorDDA = "";
        //                    string TurnKey_ABA = "";
        //                    string TurnKey_DDA = "";
        //                    bool ApplySplit = false;

        //                    //begin turnkey
        //                    if (b.CompanyDescription == "BTOT DEP" && m_TurnKey_Merchants.Contains(mid))//If Turnkey merchant. Apply splits to batch deposits only.
        //                    {

        //                        DateTime[] dates = new DateTime[]
        //                        {
        //                            m_today.AddDays(-2),
        //                            m_today.AddDays(-1),
        //                            m_today
        //                        };

        //                        foreach (DateTime dt in dates)
        //                        {


        //                            ArrayList prms = new ArrayList();
        //                            prms.Add(new SqlParameter("@Mid", mid));
        //                            prms.Add(new SqlParameter("@Today", dt));

        //                            DataSet ds = data.SelectBatchTurnKey(prms);
        //                            if (ds.Tables[0].Rows.Count > 0)
        //                            {
        //                                DataRow dr = ds.Tables[0].Rows[0];
        //                                BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
        //                                TransSum = DataLayer.Field2Dec(dr["TransSum"]);
        //                                TransCustom1Sum = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
        //                                TransCustom2Sum = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
        //                                TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
        //                                TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);
        //                            }

        //                            if (ds.Tables[1].Rows.Count > 0)
        //                            {
        //                                DataRow dr = ds.Tables[1].Rows[0];
        //                                DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
        //                                DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
        //                            }

        //                            //make batch, transaction total, and ach amount matches
        //                            bool AmountsEqual = d.Amount == BatchNetAmount && d.Amount == TransSum;

        //                            //make sure split amounts are not greater than ach amount
        //                            if (AmountsEqual && BatchNetAmount > (TransCustom1Sum + TransCustom2Sum))
        //                            {
        //                                ApplySplit = true;
        //                                break;
        //                            }
        //                            else
        //                            {
        //                                ApplySplit = false;
        //                                //MessageBox.Show("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
        //                                //return false;
        //                            }
        //                        }//fornext

        //                        //if split is stll false then try to query both saturday and sunday data
        //                        if (!ApplySplit)
        //                        {
        //                            ArrayList prms = new ArrayList();
        //                            prms.Add(new SqlParameter("@Mid", mid));
        //                            prms.Add(new SqlParameter("@Today", m_sunday));
        //                            prms.Add(new SqlParameter("@Weekend", m_sunday));

        //                            DataSet ds = data.SelectBatchTurnKey(prms);
        //                            if (ds.Tables[0].Rows.Count > 0)
        //                            {
        //                                DataRow dr = ds.Tables[0].Rows[0];
        //                                BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
        //                                TransSum = DataLayer.Field2Dec(dr["TransSum"]);
        //                                TransCustom1Sum = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
        //                                TransCustom2Sum = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
        //                                TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
        //                                TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);
        //                            }

        //                            if (ds.Tables[1].Rows.Count > 0)
        //                            {
        //                                DataRow dr = ds.Tables[1].Rows[0];
        //                                DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
        //                                DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
        //                            }

        //                            //only split if there are values in customfield1 or customfield2
        //                            //make batch, transaction total, and ach amount matches
        //                            bool AmountsEqual = d.Amount == BatchNetAmount && d.Amount == TransSum;

        //                            //make sure split amounts are not greater than ach amount
        //                            if (AmountsEqual && BatchNetAmount > (TransCustom1Sum + TransCustom2Sum))
        //                            {
        //                                ApplySplit = true;
        //                            }
        //                            else
        //                            {
        //                                ApplySplit = false;
        //                            }
        //                        }

        //                        //Problem with turnkey merchant data. contact IT
        //                        if (!ApplySplit)
        //                        {
        //                            MessageBox.Show("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
        //                            return false;
        //                        }

        //                    }//end turnkey



        //                    if (ApplySplit)
        //                    {
        //                        //Split to Operator
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = d.AccountName;
        //                        detail.AccountNo = d.AccountNo;
        //                        detail.TransRoute = d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = TransSum - TransCustom1Sum - TransCustom2Sum; //d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);

        //                        //Split to TurnKey
        //                        if (TransCustom1Sum != 0)
        //                        {
        //                            detail = batch.CreateDetail();
        //                            detail.AccountName = "TW Vending Inc";// d.AccountName;
        //                            detail.AccountNo = TurnKey_DDA;// d.AccountNo;
        //                            detail.TransRoute = TurnKey_ABA; // d.TransRoute;
        //                            detail.TransType = d.TransType;
        //                            detail.Amount = TransCustom1Sum; //d.Amount;
        //                            detail.IdentificationNumber = d.IdentificationNumber;
        //                            detail.CheckNumber = d.CheckNumber;
        //                            detail.TraceNumber = ++DetailCount;
        //                            detail.DiscretionaryData = d.DiscretionaryData;
        //                            batch.AddDetail(detail);
        //                        }

        //                        //Split to Distributor only if amount is not 0 and aba/dda exist
        //                        if (TransCustom2Sum != 0 && !string.IsNullOrEmpty(DistributorABA) && !string.IsNullOrEmpty(DistributorDDA))
        //                        {
        //                            detail = batch.CreateDetail();
        //                            detail.AccountName = d.AccountName;
        //                            detail.AccountNo = d.AccountNo;
        //                            detail.TransRoute = d.TransRoute;
        //                            detail.TransType = d.TransType;
        //                            detail.Amount = TransCustom2Sum; //d.Amount;
        //                            detail.IdentificationNumber = d.IdentificationNumber;
        //                            detail.CheckNumber = d.CheckNumber;
        //                            detail.TraceNumber = ++DetailCount;
        //                            detail.DiscretionaryData = d.DiscretionaryData;
        //                            batch.AddDetail(detail);
        //                        }


        //                    }
        //                    else
        //                    {
        //                        //do not apply split
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = d.AccountName;
        //                        detail.AccountNo = d.AccountNo;
        //                        detail.TransRoute = d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);
        //                    }


        //                    if (d.TransType.Substring(1, 1) == "7")
        //                        m_NDFDebit += d.Amount;
        //                    else
        //                        m_NDFCredit += d.Amount;

        //                    if (d.TransType.Substring(1, 1) == "7")
        //                        m_TodayFileTotalDebit += d.Amount;
        //                    else
        //                        m_TodayFileTotalCredit += d.Amount;


        //                }
        //                else
        //                {
        //                    if (d.TransType.Substring(1, 1) == "7")
        //                        m_TodayFileTotalDebit += d.Amount;
        //                    else
        //                        m_TodayFileTotalCredit += d.Amount;
        //                }

        //            }




        //        }
        //    }

        //    if (createOffset)
        //    {
        //        NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
        //        createOffset = false;

        //        //if (r != null)
        //        //{
        //        //    if (r.TransType.Substring(1, 1) == "7")
        //        //        m_NDFDebit += r.Amount;
        //        //    else
        //        //        m_NDFCredit += r.Amount;
        //        //}
        //    }

        //    /* Merge transactions from prior day that did not get process */
        //    if (priorday != null)
        //    {
        //        foreach (SettlementBatch b in priorday.Batches)
        //        {
        //            if (createOffset)
        //            {
        //                NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
        //                createOffset = false;

        //                //if (r != null)
        //                //{
        //                //    if (r.TransType.Substring(1, 1) == "7")
        //                //        m_SplitFileTotalDebit += r.Amount;
        //                //    else
        //                //        m_SplitFileTotalCredit += r.Amount;
        //                //}
        //            }


        //            batch = null;


        //            foreach (SettlementDetail d in b.Details)
        //            {
        //                string mid = d.IdentificationNumber.Trim();

        //                //if mid is next day funding
        //                if (mid.Substring(0, 4) != "8713" && !m_merchants.ContainsKey(mid))
        //                {
        //                    //create batch does not exists
        //                    if (batch == null)
        //                    {
        //                        createOffset = true;
        //                        batch = s1.CreateBatch();
        //                        batch.CompanyName = b.CompanyName;
        //                        batch.DiscretionaryData = b.DiscretionaryData;
        //                        batch.CompanyIdentification = "1262437777";
        //                        batch.Secc = b.Secc;
        //                        batch.CompanyDescription = b.CompanyDescription;
        //                        batch.CompanyDescriptiveDate = b.CompanyDescriptiveDate;
        //                        batch.EntyDate = m_NextBankDay.ToString("yyMMdd"); // DataLayer.Field2Date(b.EntyDate).ToString("yyMMdd");
        //                        batch.OriginatingDFI = b.OriginatingDFI;
        //                        batch.BatchNumber = b.BatchNumber;
        //                        s1.AddBatch(batch);
        //                    }

        //                    decimal BatchNetAmount = 0;
        //                    decimal TransSum = 0;
        //                    decimal TransCustom1Sum = 0; //turn key split
        //                    decimal TransCustom2Sum = 0; //distributor split
        //                    string DistributorABA = "";
        //                    string DistributorDDA = "";
        //                    string TurnKey_ABA = "";
        //                    string TurnKey_DDA = "";

        //                    bool ApplySplit = false;

        //                    if (b.CompanyDescription == "BTOT DEP" && m_TurnKey_Merchants.Contains(mid))//If Turnkey merchant. Apply splits to batch deposits only.
        //                    {
        //                        ArrayList prms = new ArrayList();
        //                        prms.Add(new SqlParameter("@Mid", mid));
        //                        prms.Add(new SqlParameter("@Today", m_priorday));

        //                        DataSet ds = data.SelectBatchTurnKey(prms);
        //                        if (ds.Tables[0].Rows.Count > 0)
        //                        {
        //                            DataRow dr = ds.Tables[0].Rows[0];
        //                            BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
        //                            TransSum = DataLayer.Field2Dec(dr["TransSum"]);
        //                            TransCustom1Sum = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
        //                            TransCustom2Sum = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
        //                            TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
        //                            TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);

        //                        }

        //                        if (ds.Tables[1].Rows.Count > 0)
        //                        {
        //                            DataRow dr = ds.Tables[1].Rows[0];
        //                            DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
        //                            DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
        //                        }

        //                        //only split if there are values in customfield1 or customfield2
        //                        if (TransCustom1Sum != 0 || TransCustom2Sum != 0)
        //                        {
        //                            //make batch, transaction total, and ach amount matches
        //                            bool AmountsEqual = d.Amount == BatchNetAmount && d.Amount == TransSum;

        //                            //make sure split amounts are not greater than ach amount
        //                            if (AmountsEqual && BatchNetAmount > (TransCustom1Sum + TransCustom2Sum))
        //                            {
        //                                ApplySplit = true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
        //                                return false;
        //                            }
        //                        }
        //                    }

        //                    if (ApplySplit)
        //                    {
        //                        //Split to Operator
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = d.AccountName;
        //                        detail.AccountNo = d.AccountNo;
        //                        detail.TransRoute = d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = TransSum - TransCustom1Sum - TransCustom2Sum; //d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);

        //                        //Split to TurnKey
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = "TW Vending Inc";// d.AccountName;
        //                        detail.AccountNo = TurnKey_DDA;// d.AccountNo;
        //                        detail.TransRoute = TurnKey_ABA;// d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = TransCustom1Sum; //d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);

        //                        //Split to Distributor only if amount is not 0 and aba/dda exist
        //                        if (TransCustom2Sum != 0 && !string.IsNullOrEmpty(DistributorABA) && !string.IsNullOrEmpty(DistributorDDA))
        //                        {
        //                            detail = batch.CreateDetail();
        //                            detail.AccountName = d.AccountName;
        //                            detail.AccountNo = d.AccountNo;
        //                            detail.TransRoute = d.TransRoute;
        //                            detail.TransType = d.TransType;
        //                            detail.Amount = TransCustom2Sum; //d.Amount;
        //                            detail.IdentificationNumber = d.IdentificationNumber;
        //                            detail.CheckNumber = d.CheckNumber;
        //                            detail.TraceNumber = ++DetailCount;
        //                            detail.DiscretionaryData = d.DiscretionaryData;
        //                            batch.AddDetail(detail);
        //                        }


        //                    }
        //                    else
        //                    {
        //                        //do not apply split
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = d.AccountName;
        //                        detail.AccountNo = d.AccountNo;
        //                        detail.TransRoute = d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);
        //                    }

        //                    if (d.TransType.Substring(1, 1) == "7")
        //                        m_PriorDayTotalDebit += d.Amount;
        //                    else
        //                        m_PriorDayTotalCredit += d.Amount;

        //                }



        //            }
        //        }
        //    }

        //    if (createOffset)
        //    {
        //        NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
        //        createOffset = false;

        //        //if (r != null)
        //        //{
        //        //    if (r.TransType.Substring(1, 1) == "7")
        //        //        m_SplitFileTotalDebit += r.Amount;
        //        //    else
        //        //        m_SplitFileTotalCredit += r.Amount;
        //        //}
        //    }


        //    /* update batchid */
        //    int rows = data.UpdateLastBatchID(14, s1.FileBatchNumber);
        //    if (rows == 0)
        //        throw new System.Exception("Unable to update Last BatchID");

        //    if (s1.BatchCount == 0)
        //        return false;

        //    //Print file header
        //    sw = new StreamWriter(txtOutput.Text);
        //    sw.WriteLine(s1.GetFileHeader());

        //    //Print file
        //    foreach (SettlementBatch b in s1.Batches)
        //    {
        //        sw.WriteLine(b.GetBatchHeader());
        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            sw.WriteLine(d.GetFileDetail());

        //            if (d.TransType.Substring(1, 1) == "7")
        //                m_SplitFileTotalDebit += d.Amount;
        //            else
        //                m_SplitFileTotalCredit += d.Amount;

        //            if (d.Addendas.Count > 0)
        //                sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //        }
        //        sw.WriteLine(b.GetBatchFooter());
        //    }

        //    //Print file footer
        //    sw.WriteLine(s1.GetFileFooter());

        //    //Print padded "9"
        //    remainder = s1.TotalLines % 10;

        //    if (remainder > 0)
        //    {
        //        for (int i = 0; i < 10 - remainder; i++)
        //        {
        //            sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //        }
        //    }

        //    sw.Close();


        //    return true;

        //}


        //private bool SplitSundayFile(NachaFile sunday)
        //{
        //    DataAchProcess data = new DataAchProcess();
        //    NachaFile s1 = null;
        //    NachaBatch batch = null;
        //    NachaDetail detail = null;
        //    SettlementAddenda addenda = null;
        //    StreamWriter sw = null;
        //    int remainder = 0;
        //    bool createOffset = false;



        //    /* Get last tracenumber and batchid */
        //    int DetailCount = 0;

        //    s1 = CreateNachaFile(sunday);

        //    /* Get last batchid */
        //    s1.FileBatchNumber = data.SelectLastBatchID(14);

        //    /* Extract next day funding merchants out of today's file */
        //    foreach (SettlementBatch b in sunday.Batches)
        //    {
        //        if (createOffset)
        //        {
        //            NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
        //            createOffset = false;


        //        }


        //        batch = null;


        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            string mid = d.IdentificationNumber;

        //            {
        //                //create batch does not exists
        //                if (batch == null)
        //                {
        //                    createOffset = true;
        //                    batch = s1.CreateBatch();
        //                    batch.CompanyName = b.CompanyName;
        //                    batch.DiscretionaryData = b.DiscretionaryData;
        //                    batch.CompanyIdentification = "1262437777";
        //                    batch.Secc = b.Secc;
        //                    batch.CompanyDescription = b.CompanyDescription;
        //                    batch.CompanyDescriptiveDate = b.CompanyDescriptiveDate;
        //                    batch.EntyDate = m_NextBankDay.ToString("yyMMdd"); // DataLayer.Field2Date(b.EntyDate).ToString("yyMMdd");
        //                    batch.OriginatingDFI = "122043482";//b.OriginatingDFI;
        //                    batch.BatchNumber = b.BatchNumber;
        //                    s1.AddBatch(batch);
        //                }

        //                //Create new detail record

        //                //Determine if this is TurnKey merchant and apply split based on custom fields in payment xp
        //                //TurnKey vending machines could split 3 ways.
        //                //TurnKey split $           - CustomField1
        //                //Operations split $        - CustomField2 
        //                //Operations aba            - CustomField3
        //                //Operations dda            - CustomField4

        //                decimal BatchNetAmount = 0;
        //                decimal TransSum = 0;
        //                decimal TransCustom1Sum = 0; //turn key split
        //                decimal TransCustom2Sum = 0; //distributor split
        //                string DistributorABA = "";
        //                string DistributorDDA = "";
        //                string TurnKey_ABA = "";
        //                string TurnKey_DDA = "";

        //                bool ApplySplit = false;

        //                if (b.CompanyDescription == "BTOT DEP" && m_TurnKey_Merchants.Contains(mid))//If Turnkey merchant. Apply splits to batch deposits only.
        //                {
        //                    DateTime[] dates = new DateTime[]
        //                    {
        //                        m_sunday.AddDays(-2),
        //                        m_sunday.AddDays(-1),
        //                        m_sunday
        //                    };

        //                    //int ii = 0;
        //                    //if (mid == "513484010801785")
        //                    //    ii = 1;

        //                    foreach (DateTime dt in dates)
        //                    {
        //                        ArrayList prms = new ArrayList();
        //                        prms.Add(new SqlParameter("@Mid", mid));
        //                        prms.Add(new SqlParameter("@Today", dt));

        //                        DataSet ds = data.SelectBatchTurnKey(prms);
        //                        if (ds.Tables[0].Rows.Count > 0)
        //                        {
        //                            DataRow dr = ds.Tables[0].Rows[0];
        //                            BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
        //                            TransSum = DataLayer.Field2Dec(dr["TransSum"]);
        //                            TransCustom1Sum = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
        //                            TransCustom2Sum = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
        //                            TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
        //                            TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);

        //                        }

        //                        if (ds.Tables[1].Rows.Count > 0)
        //                        {
        //                            DataRow dr = ds.Tables[1].Rows[0];
        //                            DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
        //                            DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
        //                        }

        //                        //only split if there are values in customfield1 or customfield2
        //                        //make batch, transaction total, and ach amount matches
        //                        bool AmountsEqual = d.Amount == BatchNetAmount && d.Amount == TransSum;

        //                        //make sure split amounts are not greater than ach amount
        //                        if (AmountsEqual && BatchNetAmount > (TransCustom1Sum + TransCustom2Sum))
        //                        {
        //                            ApplySplit = true;
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            ApplySplit = false;
        //                        }

        //                    }

        //                    //if split is stll false then try to query both saturday and sunday data
        //                    if (!ApplySplit)
        //                    {
        //                        ArrayList prms = new ArrayList();
        //                        prms.Add(new SqlParameter("@Mid", mid));
        //                        prms.Add(new SqlParameter("@Today", m_sunday));
        //                        prms.Add(new SqlParameter("@Weekend", m_sunday));

        //                        DataSet ds = data.SelectBatchTurnKey(prms);
        //                        if (ds.Tables[0].Rows.Count > 0)
        //                        {
        //                            DataRow dr = ds.Tables[0].Rows[0];
        //                            BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
        //                            TransSum = DataLayer.Field2Dec(dr["TransSum"]);
        //                            TransCustom1Sum = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
        //                            TransCustom2Sum = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
        //                            TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
        //                            TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);
        //                        }

        //                        if (ds.Tables[1].Rows.Count > 0)
        //                        {
        //                            DataRow dr = ds.Tables[1].Rows[0];
        //                            DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
        //                            DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
        //                        }

        //                        //only split if there are values in customfield1 or customfield2
        //                        //make batch, transaction total, and ach amount matches
        //                        bool AmountsEqual = d.Amount == BatchNetAmount && d.Amount == TransSum;

        //                        //make sure split amounts are not greater than ach amount
        //                        if (AmountsEqual && BatchNetAmount > (TransCustom1Sum + TransCustom2Sum))
        //                        {
        //                            ApplySplit = true;
        //                        }
        //                        else
        //                        {
        //                            ApplySplit = false;
        //                        }
        //                    }


        //                    //Problem with turnkey merchant data. contact IT
        //                    if (!ApplySplit)
        //                    {
        //                        MessageBox.Show("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
        //                        return false;
        //                    }


        //                }


        //                if (ApplySplit)
        //                {
        //                    //Split to Operator
        //                    detail = batch.CreateDetail();
        //                    detail.AccountName = d.AccountName;
        //                    detail.AccountNo = d.AccountNo;
        //                    detail.TransRoute = d.TransRoute;
        //                    detail.TransType = d.TransType;
        //                    detail.Amount = TransSum - TransCustom1Sum - TransCustom2Sum; //d.Amount;
        //                    detail.IdentificationNumber = d.IdentificationNumber;
        //                    detail.CheckNumber = d.CheckNumber;
        //                    detail.TraceNumber = ++DetailCount;
        //                    detail.DiscretionaryData = d.DiscretionaryData;
        //                    batch.AddDetail(detail);

        //                    //Split to TurnKey
        //                    if (TransCustom1Sum != 0)
        //                    {
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = "TW Vending Inc";// d.AccountName;
        //                        detail.AccountNo = TurnKey_DDA;// d.AccountNo;
        //                        detail.TransRoute = TurnKey_ABA;// d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = TransCustom1Sum; //d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);
        //                    }

        //                    //Split to Distributor only if amount is not 0 and aba/dda exist
        //                    if (TransCustom2Sum != 0 && !string.IsNullOrEmpty(DistributorABA) && !string.IsNullOrEmpty(DistributorDDA))
        //                    {
        //                        detail = batch.CreateDetail();
        //                        detail.AccountName = d.AccountName;
        //                        detail.AccountNo = d.AccountNo;
        //                        detail.TransRoute = d.TransRoute;
        //                        detail.TransType = d.TransType;
        //                        detail.Amount = TransCustom2Sum; //d.Amount;
        //                        detail.IdentificationNumber = d.IdentificationNumber;
        //                        detail.CheckNumber = d.CheckNumber;
        //                        detail.TraceNumber = ++DetailCount;
        //                        detail.DiscretionaryData = d.DiscretionaryData;
        //                        batch.AddDetail(detail);
        //                    }


        //                }
        //                else
        //                {
        //                    //do not apply split
        //                    detail = batch.CreateDetail();
        //                    detail.AccountName = d.AccountName;
        //                    detail.AccountNo = d.AccountNo;
        //                    detail.TransRoute = d.TransRoute;
        //                    detail.TransType = d.TransType;
        //                    detail.Amount = d.Amount;
        //                    detail.IdentificationNumber = d.IdentificationNumber;
        //                    detail.CheckNumber = d.CheckNumber;
        //                    detail.TraceNumber = ++DetailCount;
        //                    detail.DiscretionaryData = d.DiscretionaryData;
        //                    batch.AddDetail(detail);
        //                }



        //            }





        //        }
        //    }

        //    if (createOffset)
        //    {
        //        NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
        //        createOffset = false;

        //    }





        //    /* update batchid */
        //    int rows = data.UpdateLastBatchID(14, s1.FileBatchNumber);
        //    if (rows == 0)
        //        throw new System.Exception("Unable to update Last BatchID");

        //    if (s1.BatchCount == 0)
        //        return false;

        //    //Print file header
        //    sw = new StreamWriter(txtSundayOutput.Text);
        //    sw.WriteLine(s1.GetFileHeader());

        //    //Print file
        //    foreach (SettlementBatch b in s1.Batches)
        //    {
        //        sw.WriteLine(b.GetBatchHeader());
        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            sw.WriteLine(d.GetFileDetail());

        //            if (d.Addendas.Count > 0)
        //                sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //        }
        //        sw.WriteLine(b.GetBatchFooter());
        //    }

        //    //Print file footer
        //    sw.WriteLine(s1.GetFileFooter());

        //    //Print padded "9"
        //    remainder = s1.TotalLines % 10;

        //    if (remainder > 0)
        //    {
        //        for (int i = 0; i < 10 - remainder; i++)
        //        {
        //            sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //        }
        //    }

        //    sw.Close();


        //    return true;

        //}

        private bool SplitFileNew(NachaFile file, string OutputFile)
        {
            DataAchProcess data = new DataAchProcess();
            NachaFile s1 = null;
            NachaBatch batch = null;
            NachaDetail detail = null;
            SettlementAddenda addenda = null;
            StreamWriter sw = null;
            int remainder = 0;
            bool createOffset = false;
            decimal TotalTurnKeySplitAmount = 0;
            decimal TotalTurnKeyDiscountAmount = 0;

            /* Get last tracenumber and batchid */
            int DetailCount = 0;

            Logging.NCALSplit.Info("Create NACHA file");
            s1 = CreateNachaFile(file);

            /* Get last batchid */
            s1.FileBatchNumber = data.SelectLastBatchID(14);
            Logging.NCALSplit.InfoFormat("Last BatchID", s1.FileBatchNumber);


            /* Extract next day funding merchants out of today's file */
            foreach (SettlementBatch b in file.Batches)
            {
                if (createOffset)
                {
                    Logging.NCALSplit.InfoFormat("Create offset record for Batch: {0}, TraceNumber: {1}", batch.BatchID, batch.BatchOffsetTraceNumber);
                    NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
                    createOffset = false;
                }


                batch = null;


                foreach (SettlementDetail d in b.Details)
                {
                    string mid = d.IdentificationNumber;

                    Logging.NCALSplit.InfoFormat("Process detail record for MID: {0}, TraceNumber: {1}", mid, d.TraceNumber);
                    
                    if (mid.Substring(0, 4) != "8713")
                    {
                        //create batch if it does not exists
                        if (batch == null)
                        {
                            createOffset = true;
                            batch = s1.CreateBatch();
                            batch.CompanyName = b.CompanyName;
                            batch.DiscretionaryData = b.DiscretionaryData;
                            batch.CompanyIdentification = "1262437777";
                            batch.Secc = b.Secc;
                            batch.CompanyDescription = b.CompanyDescription;
                            batch.CompanyDescriptiveDate = b.CompanyDescriptiveDate;
                            batch.EntyDate = m_NextBankDay.ToString("yyMMdd"); // DataLayer.Field2Date(b.EntyDate).ToString("yyMMdd");
                            batch.OriginatingDFI = "122043482";//b.OriginatingDFI;
                            batch.BatchNumber = b.BatchNumber;
                            Logging.NCALSplit.InfoFormat("Add new Batch, Batch Number: {0} to the file.", b.BatchNumber);
                            s1.AddBatch(batch);
                        }

                        //Create new detail record

                        //Determine if this is TurnKey merchant and apply split based on custom fields in payment xp
                        //TurnKey vending machines could split 3 ways.
                        //TurnKey split $           - CustomField1
                        //Operations split $        - CustomField2 
                        //Operations aba            - CustomField3
                        //Operations dda            - CustomField4

                        //decimal BatchNetAmount = 0;
                        //decimal TransSum = 0;
                        //decimal TransCustom1Sum = 0; //turn key split
                        //decimal TransCustom2Sum = 0; //distributor split
                        //string DistributorABA = "";
                        //string DistributorDDA = "";
                        //string TurnKey_ABA = "";
                        //string TurnKey_DDA = "";

                        bool ApplySplit = false;
                        TurnKey tk = new TurnKey();
                        if ((b.CompanyDescription == "BTOT DEP" || b.CompanyDescription == "MTOT DEP") && m_TurnKey_Merchants.Contains(mid))//If Turnkey merchant. Apply splits to batch deposits only.
                        {
                            Logging.NCALSplit.InfoFormat("MID: {0}, is a valid Turnkey Merchant, Check for splits", mid);

                            ApplySplit = this.CheckTurnKeySplit(tk, mid, d, false);

                            //if split is stll false then try to query both saturday and sunday data
                            if (!ApplySplit)
                            {
                                Logging.NCALSplit.InfoFormat("Check previous two days TurnKeySplit for MID: {0},", mid);
                                ApplySplit = this.CheckTurnKeySplit(tk, mid, d, true);   
                            }


                            //Problem with turnkey merchant data. contact IT
                            if (!ApplySplit)
                            {
                                Logging.NCALSplit.Error("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
                                MessageBox.Show("Error: TurnKey deposit amount does not match batch amount. Please contact IT.");
                                return false;
                            }


                        }//TurnKey end if


                        if (ApplySplit)
                        {
                            //Split to Operator

                            Logging.NCALSplit.Info("Split to Operator");
                            detail = batch.CreateDetail();
                            detail.AccountName = d.AccountName;
                            detail.AccountNo = d.AccountNo;
                            detail.TransRoute = d.TransRoute;
                            detail.TransType = d.TransType;
                            detail.Amount = tk.TransactionTotal - tk.TurnKeySplitTotal - tk.DistributorSplitTotal; //d.Amount;
                            detail.IdentificationNumber = d.IdentificationNumber;
                            detail.CheckNumber = d.CheckNumber;
                            detail.TraceNumber = ++DetailCount;
                            detail.DiscretionaryData = d.DiscretionaryData;
                            batch.AddDetail(detail);

                            //Split to TurnKey
                            if (tk.TurnKeySplitTotal != 0)
                            {
                                Logging.NCALSplit.InfoFormat("Split to Turnkey for MID: {0}, TurnKeySplitTotal: {1}", mid, tk.TurnKeySplitTotal);
                                switch (d.TransType)
                                {
                                    case "27":
                                    case "37":
                                    case "28":
                                    case "38":
                                        TotalTurnKeySplitAmount += tk.TurnKeySplitTotal;
                                        break;
                                    default:
                                        TotalTurnKeySplitAmount -= tk.TurnKeySplitTotal;
                                        break;
                                }
                               
                            }

                            //Split to Distributor only if amount is not 0 and aba/dda exist
                            if (tk.DistributorSplitTotal != 0 && !string.IsNullOrEmpty(tk.DistributorABA) && !string.IsNullOrEmpty(tk.DistributorDDA))
                            {
                                Logging.NCALSplit.InfoFormat("Split to Distributor for MID: {0}, DistributorSplitTotal: {1}", mid, tk.DistributorSplitTotal);
                                detail = batch.CreateDetail();
                                detail.AccountName = d.AccountName;
                                detail.AccountNo = d.AccountNo;
                                detail.TransRoute = d.TransRoute;
                                detail.TransType = d.TransType;
                                detail.Amount = tk.DistributorSplitTotal; //d.Amount;
                                detail.IdentificationNumber = d.IdentificationNumber;
                                detail.CheckNumber = d.CheckNumber;
                                detail.TraceNumber = ++DetailCount;
                                detail.DiscretionaryData = d.DiscretionaryData;
                                batch.AddDetail(detail);
                            }


                        }
                        else
                        {
                            Logging.NCALSplit.InfoFormat("MID: {0}, does not have TurnKey Splits, Check if it is DISCOUNT. ");
                            //aggregate discount charges
                            if (b.CompanyDescription == "DISCOUNT" && m_TurnKey_Merchants.Contains(mid))
                            {
                                Logging.NCALSplit.InfoFormat("Aggregate discount charges for MID: {0}, Distributor Total: {1}", mid, tk.DistributorSplitTotal);
                                switch (d.TransType)
                                {
                                    case "27":
                                    case "37":
                                    case "28":
                                    case "38":
                                        TotalTurnKeyDiscountAmount += d.Amount;
                                        break;
                                    default:
                                        TotalTurnKeyDiscountAmount -= d.Amount;
                                        break;
                                }
                                
                            }
                            else
                            {
                                Logging.NCALSplit.InfoFormat("MID: {0}, does not have TurnKey Splits/Turnkey DISCOUNT, Create detail ");
                                //do not apply split
                                detail = batch.CreateDetail();
                                detail.AccountName = d.AccountName;
                                detail.AccountNo = d.AccountNo;
                                detail.TransRoute = d.TransRoute;
                                detail.TransType = d.TransType;
                                detail.Amount = d.Amount;
                                detail.IdentificationNumber = d.IdentificationNumber;
                                detail.CheckNumber = d.CheckNumber;
                                detail.TraceNumber = ++DetailCount;
                                detail.DiscretionaryData = d.DiscretionaryData;
                                batch.AddDetail(detail);
                            }
                        }



                    }//next detail





                }
            }

            if (createOffset)
            {
                NachaDetail r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
                createOffset = false;


                //Turnkey only. This code was added to create 1 deposit for all turnkey splits.
                if (TotalTurnKeySplitAmount != 0)
                {
                    Logging.NCALSplit.InfoFormat("Turnkey only. Create 1 deposit batch for all turnkey splits. Total TurnKeySplit Amount: {0}", TotalTurnKeySplitAmount);

                    batch = s1.CreateBatch();
                    batch.CompanyName = "MERITUS";
                    batch.DiscretionaryData = "";
                    batch.CompanyIdentification = "1262437777";
                    batch.Secc = "CCD";
                    batch.CompanyDescription = "BTOT DEP  ";
                    batch.CompanyDescriptiveDate = m_NextBankDay.ToString("yyMMdd");
                    batch.EntyDate = m_NextBankDay.ToString("yyMMdd");
                    batch.OriginatingDFI = "122043482";
                    batch.BatchNumber = 0;
                    s1.AddBatch(batch);

                    detail = batch.CreateDetail();
                    detail.AccountName = "TW Vending Inc";
                    detail.AccountNo = "80040488";
                    detail.TransRoute = "091900193";

                    if (TotalTurnKeySplitAmount < 0)
                        detail.TransType = "22";
                    else
                        detail.TransType = "27";

                    detail.Amount = Math.Abs(TotalTurnKeySplitAmount);
                    detail.IdentificationNumber = "513484010801637"; //Turnkey Corrections (NCAL Headquarter Account) ZID 41926
                    detail.CheckNumber = "";
                    detail.TraceNumber = ++DetailCount;
                    detail.DiscretionaryData = "0 ";
                    batch.AddDetail(detail);

                    r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);
                }

                if (TotalTurnKeyDiscountAmount != 0)
                {

                    Logging.NCALSplit.InfoFormat("Turnkey only. Create 1 deposit batch for all turnkey Discount. Total TurnKey Discount Amount: {0}", TotalTurnKeyDiscountAmount);
                    batch = s1.CreateBatch();
                    batch.CompanyName = "MERITUS";
                    batch.DiscretionaryData = "";
                    batch.CompanyIdentification = "1262437777";
                    batch.Secc = "CCD";
                    batch.CompanyDescription = "DISCOUNT";
                    batch.CompanyDescriptiveDate = m_NextBankDay.ToString("yyMMdd");
                    batch.EntyDate = m_NextBankDay.ToString("yyMMdd");
                    batch.OriginatingDFI = "122043482";
                    batch.BatchNumber = 0;
                    s1.AddBatch(batch);

                    detail = batch.CreateDetail();
                    detail.AccountName = "TW Vending Inc";
                    detail.AccountNo = "80040488";
                    detail.TransRoute = "091900193";

                    if (TotalTurnKeyDiscountAmount < 0)
                        detail.TransType = "22";
                    else
                        detail.TransType = "27";

                    detail.Amount = Math.Abs(TotalTurnKeyDiscountAmount);
                    detail.IdentificationNumber = "513484010801637"; //Turnkey Corrections (NCAL Headquarter Account) ZID 41926
                    detail.CheckNumber = "";
                    detail.TraceNumber = ++DetailCount;
                    detail.DiscretionaryData = "0 ";
                    batch.AddDetail(detail);

                    r = s1.Bank.GetBatchOffsetDetailRecord(batch, ref DetailCount);

                }
                
            }





            /* update batchid */
            ////int rows = data.UpdateLastBatchID(14, s1.FileBatchNumber);
            //if (rows == 0)
            //    throw new System.Exception("Unable to update Last BatchID");

            if (s1.BatchCount == 0)
                return false;

            //Print file header
            sw = new StreamWriter(OutputFile);
            sw.WriteLine(s1.GetFileHeader());

            //Print file
            foreach (SettlementBatch b in s1.Batches)
            {
                sw.WriteLine(b.GetBatchHeader());
                foreach (SettlementDetail d in b.Details)
                {
                    sw.WriteLine(d.GetFileDetail());

                    if (d.Addendas.Count > 0)
                        sw.WriteLine(d.Addendas[0].GetFileAddenda());
                }
                sw.WriteLine(b.GetBatchFooter());
            }

            //Print file footer
            sw.WriteLine(s1.GetFileFooter());

            //Print padded "9"
            remainder = s1.TotalLines % 10;

            if (remainder > 0)
            {
                for (int i = 0; i < 10 - remainder; i++)
                {
                    sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
                }
            }

            sw.Close();


            return true;

        }

        private bool CheckTurnKeySplit(TurnKey tk,string mid, SettlementDetail d, bool Query2Days)
        {
            DataAchProcess data = new DataAchProcess();
            bool perform = false;


            DateTime[] dates = new DateTime[]
                            {
                                m_today.AddDays(-2),
                                m_today.AddDays(-1),
                                m_today
                            };


            foreach (DateTime dt in dates)
            {
                ArrayList prms = new ArrayList();

                if (Query2Days)
                {
                    prms.Add(new SqlParameter("@Mid", mid));
                    prms.Add(new SqlParameter("@Today", dt));
                    prms.Add(new SqlParameter("@Weekend", dt));
                }
                else
                {
                    prms.Add(new SqlParameter("@Mid", mid));
                    prms.Add(new SqlParameter("@Today", dt));
                }

                DataSet ds = data.SelectBatchTurnKey(prms);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    tk.BatchNetAmount = DataLayer.Field2Dec(dr["BatchNetAmount"]);
                    tk.TransactionTotal = DataLayer.Field2Dec(dr["TransSum"]);
                    tk.TurnKeySplitTotal = DataLayer.Field2Dec(dr["TransCustom1Sum"]);
                    tk.DistributorSplitTotal = DataLayer.Field2Dec(dr["TransCustom2Sum"]);
                    tk.TurnKey_ABA = DataLayer.Field2Str(dr["TurnKey_ABA"]);
                    tk.TurnKey_DDA = DataLayer.Field2Str(dr["TurnKey_DDA"]);


                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                    tk.DistributorABA = DataLayer.Field2Str(dr["DistributorABA"]);
                    tk.DistributorDDA = DataLayer.Field2Str(dr["DistributorDDA"]);
                }

                //only split if there are values in customfield1 or customfield2
                //make batch, transaction total, and ach amount matches
                bool AmountsEqual = ((d.Amount == tk.BatchNetAmount) && (d.Amount == tk.TransactionTotal));

                //make sure split amounts are not greater than ach amount
                if (AmountsEqual && (tk.BatchNetAmount > (tk.TurnKeySplitTotal + tk.DistributorSplitTotal)))
                {
                    perform = true;
                    break;
                }
                else
                {
                    Logging.NCALSplit.ErrorFormat("Turnkey Spli failed for MID: {0}, TurnKey BatchNetAmount:{1}, TurnKey TransactionTotal: {2}, Batch Amount: {3}", mid, tk.BatchNetAmount, tk.TransactionTotal, d.Amount );
                    perform = false;
                }

            }//for Next Date Loop


            Logging.NCALSplit.InfoFormat("TransactionTotal : {0}", tk.TransactionTotal);
            Logging.NCALSplit.InfoFormat("TurnKeySplitTotal: {0}", tk.TurnKeySplitTotal);
            Logging.NCALSplit.InfoFormat("DistributorSplitTotal : {0}", tk.DistributorSplitTotal);
            Logging.NCALSplit.InfoFormat("TurnKey_ABA : {0}", tk.TurnKey_ABA);
            Logging.NCALSplit.InfoFormat("TurnKey_DDA: {0}", tk.TurnKey_DDA);
            Logging.NCALSplit.InfoFormat("TurnKey_DDA: {0}", tk.TurnKey_DDA);
            Logging.NCALSplit.InfoFormat("DistributorABA: {0}", tk.DistributorABA);
            Logging.NCALSplit.InfoFormat("DistributorDDA: {0}", tk.DistributorDDA);

            return perform;
        }

        private void DisplayCreditCardFiles()
        {
            string directory = ConfigurationManager.AppSettings["BankcardFileDirectory"];

            string[] files = Directory.GetFiles(directory);


            lstFiles.Items.Clear();
            foreach (string file in files)
            {

                FileInfo fi = new FileInfo(file);
                lstFiles.Items.Add(file);

                
            }
        }


        private NachaFile GetFile(NachaBatch batch)
        {
            NachaFile file = null;

            foreach (SettlementDetail d in batch.Details)
            {
                if (d.AccountName.Contains("OFFSET"))
                {
                    string settlementaccount = string.Empty;

                    settlementaccount = d.TransRoute + d.AccountNo;
                    switch (settlementaccount)
                    {
                        case "122039360002556774":
                        case "1220393602549980":
                            file = (NachaFile)m_col[settlementaccount];

                            break;
                    }

                }
            }

            return file;
        }

        private bool SplitRequired(NachaFile file)
        {
            string settlementaccount = string.Empty;


            foreach (SettlementBatch b in file.Batches)
            {

                foreach (SettlementDetail d in b.Details)
                {
                    string mid = d.IdentificationNumber.Trim();

                    if (m_merchants.ContainsKey(mid))
                        return true;

                }
            }

            return false;
        }










        //private void SplitFile(NachaFile today, NachaFile priorday)
        //{
        //    DataAchProcess data = new DataAchProcess();
        //    NachaFile s1 = null;
        //    NachaBatch batch = null;
        //    NachaDetail detail = null;
        //    SettlementAddenda addenda = null;
        //    StreamWriter sw = null;
        //    int remainder = 0;
        //    bool createOffset = false;
        //    /* Get last tracenumber and batchid */
        //    int DetailCount = 0;

        //    s1 = CreateNachaFile(today);

        //    /* Get last batchid */
        //    s1.FileBatchNumber = data.SelectLastBatchID(14);

        //    foreach (SettlementBatch b in today.Batches)
        //    {
        //        if (createOffset)
        //        {
        //            s1.Bank.CreateBatchOffsetDetailRecord(batch, ref DetailCount);
        //            createOffset = false;
        //        }


        //        batch = null;


        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            string mid = d.IdentificationNumber;

        //            //if mid is next day funding
        //            if (mid == "513484010100006")
        //            {
        //                //create batch does not exists
        //                if (batch == null)
        //                {
        //                    createOffset = true;
        //                    batch = s1.CreateBatch();
        //                    batch.CompanyName = b.CompanyName;
        //                    batch.DiscretionaryData = b.DiscretionaryData;
        //                    batch.CompanyIdentification = "1262437777";
        //                    batch.Secc = b.Secc;
        //                    batch.CompanyDescription = b.CompanyDescription;
        //                    batch.CompanyDescriptiveDate = b.CompanyDescriptiveDate;
        //                    batch.EntyDate = DataLayer.Field2Date(b.EntyDate).ToString("yyMMdd");
        //                    batch.OriginatingDFI = "122043482";//b.OriginatingDFI;
        //                    batch.BatchNumber = b.BatchNumber;
        //                    s1.AddBatch(batch);
        //                }

        //                //Create new detail record
        //                detail = batch.CreateDetail();
        //                detail.AccountName = d.AccountName;
        //                detail.AccountNo = d.AccountNo;
        //                detail.TransRoute = d.TransRoute;
        //                detail.TransType = d.TransType;
        //                detail.Amount = d.Amount;
        //                detail.IdentificationNumber = d.IdentificationNumber;
        //                detail.CheckNumber = d.CheckNumber;
        //                detail.TraceNumber = ++DetailCount;
        //                detail.DiscretionaryData = d.DiscretionaryData;
        //                batch.AddDetail(detail);

        //            }



        //        }
        //    }

        //    if (createOffset)
        //        s1.Bank.CreateBatchOffsetDetailRecord(batch, ref DetailCount);


        //    int rows = data.UpdateLastBatchID(14, s1.FileBatchNumber);
        //    if (rows == 0)
        //        throw new System.Exception("Unable to update Last BatchID");

        //    //Print file header
        //    sw = new StreamWriter(@"C:\temp\ncal\test\s2.txt");
        //    sw.WriteLine(s1.GetFileHeader());

        //    //Print batch offsets
        //    foreach (SettlementBatch b in s1.Batches)
        //    {
        //        sw.WriteLine(b.GetBatchHeader());
        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            sw.WriteLine(d.GetFileDetail());
        //            if (d.Addendas.Count > 0)
        //                sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //        }
        //        sw.WriteLine(b.GetBatchFooter());
        //    }

        //    //Print file footer
        //    sw.WriteLine(s1.GetFileFooter());

        //    //Print padded "9"
        //    remainder = s1.TotalLines % 10;

        //    if (remainder > 0)
        //    {
        //        for (int i = 0; i < 10 - remainder; i++)
        //        {
        //            sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //        }
        //    }

        //    sw.Close();

        //    return;
        //    foreach (SettlementBatch b in file.Batches)
        //    {
        //        s1 = GetFile(b);

        //        batch = s1.CreateBatch();
        //        batch.CompanyName = b.CompanyName;
        //        batch.DiscretionaryData = b.DiscretionaryData;
        //        batch.CompanyIdentification = "1262437777";
        //        batch.Secc = b.Secc;
        //        batch.CompanyDescription = b.CompanyDescription;
        //        batch.CompanyDescriptiveDate = b.CompanyDescriptiveDate;
        //        batch.EntyDate = DataLayer.Field2Date(b.EntyDate).ToString("yyMMdd");
        //        batch.OriginatingDFI = "122043482";//b.OriginatingDFI;
        //        batch.BatchNumber = b.BatchNumber;
        //        s1.AddBatch(batch);

        //        foreach (SettlementDetail d in b.Details)
        //        {
        //            //Create new detail record
        //            detail = batch.CreateDetail();
        //            detail.AccountName = d.AccountName;
        //            detail.AccountNo = d.AccountNo;
        //            detail.TransRoute = d.TransRoute;
        //            detail.TransType = d.TransType;
        //            detail.Amount = d.Amount;
        //            detail.IdentificationNumber = d.IdentificationNumber;
        //            detail.CheckNumber = d.CheckNumber;
        //            detail.TraceNumber = ++DetailCount;
        //            batch.AddDetail(detail);
        //        }

        //    }

        //    /* print file 1 */
        //    if (m_col.ContainsKey("122039360002556774"))
        //    {
        //        s1 = (NachaFile)m_col["122039360002556774"];
        //        //Print file header
        //        sw = new StreamWriter(@"C:\temp\s1.txt");
        //        sw.WriteLine(s1.GetFileHeader());

        //        //Print batch offsets
        //        foreach (SettlementBatch b in s1.Batches)
        //        {
        //            sw.WriteLine(b.GetBatchHeader());
        //            foreach (SettlementDetail d in b.Details)
        //            {
        //                sw.WriteLine(d.GetFileDetail());
        //                if (d.Addendas.Count > 0)
        //                    sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //            }
        //            sw.WriteLine(b.GetBatchFooter());
        //        }

        //        //Print file footer
        //        sw.WriteLine(s1.GetFileFooter());

        //        //Print padded "9"
        //        remainder = s1.TotalLines % 10;

        //        if (remainder > 0)
        //        {
        //            for (int i = 0; i < 10 - remainder; i++)
        //            {
        //                sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //            }
        //        }

        //        sw.Close();
        //    }

        //    /* print file 2 */
        //    if (m_col.ContainsKey("1220393602549980"))
        //    {

        //        s1 = (NachaFile)m_col["1220393602549980"];
        //        //Print file header
        //        sw = new StreamWriter(@"C:\temp\s2.txt");
        //        sw.WriteLine(s1.GetFileHeader());

        //        //Print batch offsets
        //        foreach (SettlementBatch b in s1.Batches)
        //        {
        //            sw.WriteLine(b.GetBatchHeader());
        //            foreach (SettlementDetail d in b.Details)
        //            {
        //                sw.WriteLine(d.GetFileDetail());
        //                if (d.Addendas.Count > 0)
        //                    sw.WriteLine(d.Addendas[0].GetFileAddenda());
        //            }
        //            sw.WriteLine(b.GetBatchFooter());
        //        }

        //        //Print file footer
        //        sw.WriteLine(s1.GetFileFooter());

        //        //Print padded "9"
        //        remainder = s1.TotalLines % 10;

        //        if (remainder > 0)
        //        {
        //            for (int i = 0; i < 10 - remainder; i++)
        //            {
        //                sw.WriteLine("9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
        //            }
        //        }

        //        sw.Close();
        //    }

        //}

    }
}