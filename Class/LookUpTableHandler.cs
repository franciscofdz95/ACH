using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Threading.Tasks;

namespace AchSystem
{
    class LookUpTableHandler
    {



        public static void LoadTransactionTransType(ComboBox cbo)
        {
            DataTransType data = new DataTransType();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@Type","Transaction"));
            SqlDataReader dr = data.Select(prms);
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(DataLayer.Field2Str(dr["TransTypeID"]), DataLayer.Field2Str(dr["TransTypeID"]) + " - " + DataLayer.Field2Str(dr["TransTypeDesc"])));
            }

            dr.Close();
            data = null;
        }

        public static void LoadMerchantTest(ComboBox cbo)
        {
            DataMerchant data = new DataMerchant();
            ArrayList prms = new ArrayList();

            SqlDataReader dr = data.SelectMerchantTest();
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(DataLayer.Field2Str(dr["TestCode"]), DataLayer.Field2Str(dr["TestCode"]) + " - " + DataLayer.Field2Str(dr["TestDesc"])));
            }

            dr.Close();
            data = null;
        }

        public static void LoadReturnTransType(ComboBox cbo)
        {
            DataTransType data = new DataTransType();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@Type","Return"));
            SqlDataReader dr = data.Select(prms);

            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(DataLayer.Field2Str(dr["TransTypeID"]), DataLayer.Field2Str(dr["TransTypeID"]) + " - " + DataLayer.Field2Str(dr["TransTypeDesc"])));
            }

            dr.Close();
            data = null;
        }


        public static void LoadStatementPeriod(ComboBox cbo, int AchID)
        {
            DataJournal data = new DataJournal();
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@AchID", AchID));
            SqlDataReader dr = data.SelectStatementPeriod(prms);
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(DataLayer.Field2Str(dr["Period"]), DataLayer.Field2Str(dr["Period"]) ));
            }

            dr.Close();
            data = null;            
        }

        public static void LoadSecc(ComboBox cbo)
        {
            DataSecc data = new DataSecc();
            SqlDataReader dr = data.Select(new ArrayList());

            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(DataLayer.Field2Str(dr["Secc"]), DataLayer.Field2Str(dr["Secc"])));
            }

            dr.Close();
            data = null;
        }

        public static void LoadGroupMerchants(ComboBox cbo)
        {
            DataGroupMerchant data = new DataGroupMerchant();
            SqlDataReader dr = data.Select(new ArrayList());
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(DataLayer.Field2Str(dr["GroupID"]), DataLayer.Field2Str(dr["Group Name"])));
            }

            dr.Close();
            data = null;
        }

        public static void LoadReturnPrinted(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("Y", "Yes"));
            cbo.Items.Add(new AchListItem("N", "No"));
        }

        public static void LoadTransTypeOption(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("Transaction", "Transaction"));
            cbo.Items.Add(new AchListItem("Return", "Return"));
        }

        public static void LoadReturnType(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("CK", "CK - Checking"));
            cbo.Items.Add(new AchListItem("SA", "SA - Saving"));
        }

        public static void LoadEFTTypes(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("J", "Journal"));
        }

        public static void LoadHoldTypes(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("V", "V - Reserved"));
            cbo.Items.Add(new AchListItem("H", "H - Hold"));
        }

        public static void LoadBatchSource(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("C", "C - Transaction"));
            cbo.Items.Add(new AchListItem("P", "P - Pending"));
        }

        public static void LoadReleaseHold(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("Y", "Yes"));
            cbo.Items.Add(new AchListItem("N", "No"));
        }

        public static void LoadRefCodes(ComboBox cbo)
        {
            DataJournal data = new DataJournal();
            ArrayList prms = new ArrayList();
            SqlDataReader dr;
            dr = data.SelectRefCodes(prms);
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["RefCode"].ToString(), dr["RefCode"].ToString() + " - " + dr["Description"].ToString()));
            }

            dr.Close();
        }

        public static void LoadAllReasonCodes(ComboBox cbo)
        {
            DataReturn data = new DataReturn();
            ArrayList prms = new ArrayList();
            SqlDataReader dr;
            dr = data.SelectReasonCodes(prms);
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["ReasonCode"].ToString(), dr["ReasonCode"].ToString()));
            }

            dr.Close();
        }

        public static void LoadPRIReasonCodes(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("1", "R01"));
            cbo.Items.Add(new AchListItem("2", "R02"));
            cbo.Items.Add(new AchListItem("3", "R03"));
            cbo.Items.Add(new AchListItem("4", "R04"));
            cbo.Items.Add(new AchListItem("5", "R07"));
            cbo.Items.Add(new AchListItem("6", "R08"));
            cbo.Items.Add(new AchListItem("7", "R10"));
            cbo.Items.Add(new AchListItem("8", "R12"));
            cbo.Items.Add(new AchListItem("9", "R13"));
            cbo.Items.Add(new AchListItem("10", "R15"));
            cbo.Items.Add(new AchListItem("11", "R24"));
            cbo.Items.Add(new AchListItem("12", "R29"));
            cbo.Items.Add(new AchListItem("13", "R42"));
        }

        public static void LoadUserTransStatus(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("3", "Cancel"));
            cbo.Items.Add(new AchListItem("0", "Open"));
            cbo.Items.Add(new AchListItem("2", "Pending for Batch"));
        }

        public static void LoadMerchantType(ComboBox cbo)
        {
            DataMerchantType data = new DataMerchantType();
            SqlDataReader dr;
            dr = data.Search();
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["Type"].ToString(), dr["Type"].ToString() + " - " + dr["Description"].ToString()));
            }

            dr.Close();
        }

      

        public static void LoadUsers(ComboBox cbo)
        {
            DataUser data = new DataUser();
            SqlDataReader dr;
            dr = data.SelectUsers();
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["Login ID"].ToString(), dr["Login ID"].ToString()));
            }

            dr.Close();
        }

        public static void LoadMerchantIDs(ToolStripComboBox cbo)
        {
            DataMerchant data = new DataMerchant();
            SqlDataReader dr;
            dr = data.SelectMerchantIDs();
            data = null;

            cbo.Items.Add("      << Select Default Merchant Account >>");

            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["MerchantID"].ToString(), dr["MerchantID"].ToString() + " - " + dr["AchCoName"].ToString()));
            }

            dr.Close();

            if (cbo.Items.Count > 0)
            {
                cbo.Tag = string.Empty;
                cbo.SelectedIndex = 0;
            }
        }

        public static void LoadMerchantIDs(ComboBoxTool  cbo)
        {
            DataMerchant data = new DataMerchant();
            SqlDataReader dr;
            dr = data.SelectMerchantIDs();
            data = null;

            ValueListItem item = null;

            item = new ValueListItem();
            item.DisplayText = "      << Select Default Merchant Account >>";
            cbo.ValueList.ValueListItems.Add(item);

            while (dr.Read())
            {
                item = new ValueListItem();
                item.DisplayText = dr["MerchantID"].ToString() + " - " + dr["AchCoName"].ToString();
                item.DataValue = dr["MerchantID"].ToString();
                cbo.ValueList.ValueListItems.Add(item);
            }

            dr.Close();

            //if (cbo.Items.Count > 0)
            //{
            //    cbo.Tag = string.Empty;
            //    cbo.SelectedIndex = 0;
            //}
        }


        public static void LoadBanks(ComboBox cbo)
        {
            Bank bank = null;
            DataBankInfo data = new DataBankInfo();
            Dictionary<AchBankInfo, Func<string, int, Bank>> banksClasses = new Dictionary<AchBankInfo, Func<string, int, Bank>>
            {
                { AchBankInfo.GOLDMAN_SACHS, (name, id) => new BankGoldmanSachs(name, id) },
                { AchBankInfo.PAY_MY_BILL, (name, id) => new BankPayMyBill(name, id) },
                { AchBankInfo.QUAN_COMM, (name, id) => new BankQuanComm(name, id) },
                { AchBankInfo.NCT, (name, id) => new BankNct(name, id) },
                { AchBankInfo.NCAL, (name, id) => new BankNcal(name, id) },
                { AchBankInfo.FIFTH_THIRD, (name, id) => new BankFifthThird(name, id) },
                { AchBankInfo.KBT, (name, id) => new BankKBT(name, id) },
                { AchBankInfo.FDR, (name, id) => new BankFDR(name, id) },
                { AchBankInfo.CHASE, (name, id) => new BankChase(name, id) },
                { AchBankInfo.WELLS, (name, id) => new BankWells(name, id) },
                { AchBankInfo.MB_FINANCIAL, (name, id) => new BankMBFinancial(name, id) },
                { AchBankInfo.FPS, (name, id) => new BankFPS(name, id) },
                { AchBankInfo.APT_PAY, (name, id) => new BankAptPay(name, id) }
            };


            SqlDataReader dr = data.SelectBankInfo(new ArrayList());
            cbo.Items.Clear();
            
            while (dr.Read())
            {
                int _bankId = DataLayer.Field2Int(dr["BankID"]);
                string _bankName = DataLayer.Field2Str(dr["BankName"]);
                if (_bankId <= (int)AchBankInfo.APT_PAY) // This validation is no longer because the banksClasses dictionary will simply return false if the key is not found in the dictionary.
                {
                    Func<string, int, Bank> createBank;
                    if (banksClasses.TryGetValue((AchBankInfo)_bankId, out createBank))
                    {
                        bank = createBank(_bankName, _bankId);

                        bank.CompanyName = DataLayer.Field2Str(dr["companyname"]);
                        bank.ImmediateDestination = DataLayer.Field2Str(dr["immediatedestination"]);
                        bank.ImmediateOrigin = DataLayer.Field2Str(dr["immediateorigin"]);
                        bank.Symbol = DataLayer.Field2Str(dr["IDSymbol"]);
                        bank.DestinationName = DataLayer.Field2Str(dr["DestinationName"]);
                        bank.OriginName = DataLayer.Field2Str(dr["OriginName"]);
                        bank.OriginatingTransRoute = DataLayer.Field2Str(dr["NachaTransRoute"]);
                        bank.OriginatingAccountNo = DataLayer.Field2Str(dr["NachaAccountNo"]);
                        bank.CompanyID = DataLayer.Field2Str(dr["CompanyID"]);

                        cbo.Items.Add(bank);
                    }                    
                }
            }

            dr.Close();
            data = null;
        }


        public static void LoadPaymentStatus(ComboBox cbo)
        {
            DataTransStatus data = new DataTransStatus();
            SqlDataReader dr;
            ArrayList prms = new ArrayList();
            prms.Add(new SqlParameter("@PaymentAvailable",1));

            dr = data.Select(prms);
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["StatusID"].ToString(), dr["StatusID"].ToString() + " - " + dr["Description"].ToString()));
            }

            dr.Close();
            data = null;
        }

        public static void LoadAllTransStatus(ComboBox cbo)
        {
            DataTransStatus data = new DataTransStatus();
            SqlDataReader dr;
            dr = data.Select(new ArrayList());
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["StatusID"].ToString(), dr["StatusID"].ToString() + " - " + dr["Description"].ToString()));
            }

            dr.Close();
            data = null;
        }

        public static void LoadOrigins(ComboBox cbo)
        {
            DataOrigin data = new DataOrigin();
            SqlDataReader dr;
            dr = data.Search();
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["OriginID"].ToString(), dr["OriginID"].ToString() + " - " + dr["Description"].ToString()));
            }

            dr.Close();
        }



        public static void LoadTransSource(ComboBox cbo)
        {
            DataSource data = new DataSource();
            SqlDataReader dr;
            dr = data.Search();
            data = null;
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["Source"].ToString(), dr["Source"].ToString() + " - " + dr["Description"].ToString()));
            }

            dr.Close();
        }

        public static void LoadBankID(ComboBox cbo)
        {
            DataBankInfo data = new DataBankInfo();
            SqlDataReader dr = data.Search();
            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["BankID"].ToString(), dr["BankID"].ToString() + " - " + dr["BankName"].ToString()));
            }

            dr.Close();
            data = null;

        }

        public static void EFTType(ComboBox cbo)
        {
            cbo.Items.Clear();
            cbo.Items.Add(new AchListItem("C", "EFT FROM"));
            cbo.Items.Add(new AchListItem("P", "EFT TO"));

            cbo.SelectedIndex = 0;
        }

        public static void EFTTypeDescription(ComboBox cbo)
        {
            DataPayment data = new DataPayment();

            SqlDataReader dr = data.SelectEFTTypes();
            data = null;

            cbo.Items.Clear();
            while (dr.Read())
            {
                cbo.Items.Add(new AchListItem(dr["EFT Type Code"].ToString(), dr["EFT Type Desc"].ToString()));
            }

            dr.Close();
        }
    

          
    }
}
