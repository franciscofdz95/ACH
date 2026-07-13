using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using Nmc.Ach.Dal;

namespace BatchFileLoader
{
    public class Transaction : iTransaction
    {

        private long m_TransID = -1;
        private string m_Description = string.Empty;
        private string m_TransType = "27";
        private string m_TransRoute = string.Empty;
        private string m_AccountNo = string.Empty;
        private string m_NameOnAccount = string.Empty;
        private string m_RefID = string.Empty;
        private decimal m_Amount = 0;
        private string m_Secc = "PPD";
        private int m_StatusID = 0;
        private int m_MerchantID = -1;
        private string m_MerchantAppUID = string.Empty;
        private DateTime m_NextProcDate = DateTime.Now;
        private DateTime m_TransDate = DateTime.Now;
        private string m_Source = "C";
        private string m_CheckNumber = string.Empty;
        private int m_OriginID = 13; //Comma Delimited File Format
        private long m_UploadID = 0;
        private string m_CompanyName = string.Empty;
        private bool m_IsResubmittedTrans = false;
        long m_OriginalTransID = 0;

        string m_CustomInfo1 = string.Empty;
        string m_CustomInfo2 = string.Empty;
        string m_CustomInfo3 = string.Empty;
        string m_CustomInfo4 = string.Empty;
        string m_CustomInfo5 = string.Empty;
        string m_CustomInfo6 = string.Empty;
        string m_CustomInfo7 = string.Empty;
        string m_CustomInfo8 = string.Empty;
        string m_CustomInfo9 = string.Empty;
        string m_CustomInfo10 = string.Empty;
        string m_CustomInfo11 = string.Empty;
        string m_CustomInfo12 = string.Empty;
        string m_CustomInfo13 = string.Empty;
        string m_CustomInfo14 = string.Empty;
        string m_CustomInfo15 = string.Empty;
        string m_CustomInfo16 = string.Empty;
        string m_CustomInfo17 = string.Empty;
        string m_CustomInfo18 = string.Empty;
        string m_CustomInfo19 = string.Empty;
        string m_CustomInfo20 = string.Empty;

        public string CustomInfo1
        {
            get { return m_CustomInfo1; }
            set { if (value != null) m_CustomInfo1 = value; }
        }
        public string CustomInfo2
        {
            get { return m_CustomInfo2; }
            set { if (value != null) m_CustomInfo2 = value; }
        }
        public string CustomInfo3
        {
            get { return m_CustomInfo3; }
            set { if (value != null) m_CustomInfo3 = value; }
        }
        public string CustomInfo4 { get { return m_CustomInfo4; } set { m_CustomInfo4 = value; } }
        public string CustomInfo5 { get { return m_CustomInfo5; } set { m_CustomInfo5 = value; } }
        public string CustomInfo6 { get { return m_CustomInfo6; } set { m_CustomInfo6 = value; } }
        public string CustomInfo7 { get { return m_CustomInfo7; } set { m_CustomInfo7 = value; } }
        public string CustomInfo8 { get { return m_CustomInfo8; } set { m_CustomInfo8 = value; } }
        public string CustomInfo9 { get { return m_CustomInfo9; } set { m_CustomInfo9 = value; } }
        public string CustomInfo10 { get { return m_CustomInfo10; } set { m_CustomInfo10 = value; } }
        public string CustomInfo11 { get { return m_CustomInfo11; } set { m_CustomInfo11 = value; } }
        public string CustomInfo12 { get { return m_CustomInfo12; } set { m_CustomInfo12 = value; } }
        public string CustomInfo13 { get { return m_CustomInfo13; } set { m_CustomInfo13 = value; } }
        public string CustomInfo14 { get { return m_CustomInfo14; } set { m_CustomInfo14 = value; } }
        public string CustomInfo15 { get { return m_CustomInfo15; } set { m_CustomInfo15 = value; } }
        public string CustomInfo16 { get { return m_CustomInfo16; } set { m_CustomInfo16 = value; } }
        public string CustomInfo17 { get { return m_CustomInfo17; } set { m_CustomInfo17 = value; } }
        public string CustomInfo18 { get { return m_CustomInfo18; } set { m_CustomInfo18 = value; } }
        public string CustomInfo19 { get { return m_CustomInfo19; } set { m_CustomInfo19 = value; } }
        public string CustomInfo20 { get { return m_CustomInfo20; } set { m_CustomInfo20 = value; } }

        //Billing Info
        string m_BillingFirstName = string.Empty;
        string m_BillingLastName = string.Empty;
        string m_BillingAddress = string.Empty;
        string m_BillingCity = string.Empty;
        string m_BillingState = string.Empty;
        string m_BillingZip = string.Empty;
        string m_BillingCountry = string.Empty;
        string m_BillingPhone = string.Empty;
        string m_BillingFax = string.Empty;
        string m_BillingEmail = string.Empty;
        string m_ClientID = string.Empty;


        public string BillingFirstName
        {
            get
            {
                return m_BillingFirstName;
            }
            set
            {
                m_BillingFirstName = value;
            }
        }
        public string BillingLastName
        {
            get
            {
                return m_BillingLastName;
            }
            set
            {
                m_BillingLastName = value;
            }
        }
        public string BillingAddress
        {
            get
            {
                return m_BillingAddress;
            }
            set
            {

                m_BillingAddress = value;
            }
        }
        public string BillingCity
        {
            get
            {
                return m_BillingCity;
            }
            set
            {
                m_BillingCity = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string BillingState
        {
            get
            {
                return m_BillingState;
            }
            set
            {
                m_BillingState = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string BillingZip
        {
            get
            {
                return m_BillingZip;
            }
            set
            {
                //m_BillingZip = m_FDRutil.MakeIntoNumeric(value);
                m_BillingZip = value;
            }
        }
        public string BillingCountry
        {
            get
            {
                return m_BillingCountry;
            }
            set
            {
                m_BillingCountry = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string BillingPhone
        {
            get
            {
                return m_BillingPhone;
            }
            set
            {
                m_BillingPhone = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string BillingFax
        {
            get
            {
                return m_BillingFax;
            }
            set
            {
                m_BillingFax = value;
            }
        }
        public string BillingEmail
        {
            get
            {
                return m_BillingEmail;
            }
            set
            {
                m_BillingEmail = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string ClientID
        {
            get
            {
                return m_ClientID;
            }
            set
            {
                m_ClientID = value;
            }
        }



        //Shipping Info
        string m_ShippingFirstName = string.Empty;
        string m_ShippingLastName = string.Empty;
        string m_ShippingAddress = string.Empty;
        string m_ShippingAddress2 = string.Empty;
        string m_ShippingCity = string.Empty;
        string m_ShippingState = string.Empty;
        string m_ShippingZip = string.Empty;
        string m_ShippingCountry = string.Empty;


        public string ShippingFirstName
        {
            get
            {
                return m_ShippingFirstName;
            }
            set
            {
                m_ShippingFirstName = value;
            }
        }
        public string ShippingLastName
        {
            get
            {
                return m_ShippingLastName;
            }
            set
            {
                m_ShippingLastName = value;
            }
        }
        public string ShippingAddress
        {
            get
            {
                return m_ShippingAddress;
            }
            set
            {
                m_ShippingAddress = value;
            }
        }
        public string ShippingAddress2
        {
            get
            {
                return m_ShippingAddress2;
            }
            set
            {
                m_ShippingAddress2 = value;
            }
        }
        public string ShippingCity
        {
            get
            {
                return m_ShippingCity;
            }
            set
            {
                m_ShippingCity = value;
            }
        }
        public string ShippingState
        {
            get
            {
                return m_ShippingState;
            }
            set
            {
                m_ShippingState = value;
            }
        }
        public string ShippingZip
        {
            get
            {
                return m_ShippingZip;
            }
            set
            {
                m_ShippingZip = value;
            }
        }
        public string ShippingCountry
        {
            get
            {
                return m_ShippingCountry;
            }
            set
            {
                m_ShippingCountry = value;
            }
        }

        public long TransID
        {
            get { return m_TransID; }
            set { m_TransID = value; }
        }

        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        public string TransType
        {
            get { return m_TransType; }
            set { m_TransType = value; }
        }

        public string TransRoute
        {
            get { return m_TransRoute; }
            set { m_TransRoute = value; }
        }

        public string AccountNo
        {
            get { return m_AccountNo; }
            set { m_AccountNo = value; }
        }

        public string NameOnAccount
        {
            get { return m_NameOnAccount; }
            set { m_NameOnAccount = value; }
        }

        public string RefID
        {
            get { return m_RefID; }
            set { m_RefID = value; }
        }

        public decimal Amount
        {
            get { return m_Amount; }
            set { m_Amount = value; }
        }

        public string Secc
        {
            get { return m_Secc; }
            set { m_Secc = value; }
        }

        public int StatusID
        {
            get { return m_StatusID; }
            set { m_StatusID = value; }
        }

        public int MerchantID
        {
            get { return m_MerchantID; }
            set { m_MerchantID = value; }
        }

        public string MerchantAppUID
        {
            get { return m_MerchantAppUID; }
            set { m_MerchantAppUID = value; }
        }
        
        public DateTime NextProcDate
        {
            get { return m_NextProcDate; }
            set { m_NextProcDate = value; }
        }

        public DateTime TransDate
        {
            get { return m_TransDate; }
            set { m_TransDate = value; }
        }

        public string Source
        {
            get { return m_Source; }
            set { m_Source = value; }
        }

        public string CheckNumber
        {
            get { return m_CheckNumber; }
            set { m_CheckNumber = value; }
        }

        public int OriginID
        {
            get { return m_OriginID; }
            set { m_OriginID = value; }
        }

        public long UploadID
        {
            get { return m_UploadID; }
            set { m_UploadID = value; }
        }

        public string CompanyName
        {
            get { return m_CompanyName; }
            set { m_CompanyName = value; }
        }

        public bool IsResubmittedTrans
        {
            get { return m_IsResubmittedTrans; }
            set { m_IsResubmittedTrans = value; }
        }
        public long OriginalTransID
        {
            get { return m_OriginalTransID; }
            set { if (value > 0) m_OriginalTransID = value; }
        }

        public long SaveTransaction()
        {
            ArrayList prms = new ArrayList();
            SqlParameter prm = new SqlParameter("@TransID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@Description", this.Description));
            prms.Add(new SqlParameter("@TransType", this.TransType));
            prms.Add(new SqlParameter("@TransRoute", this.TransRoute));
            prms.Add(new SqlParameter("@AccountNo", this.AccountNo));
            prms.Add(new SqlParameter("@NameOnAccount", this.NameOnAccount));
            prms.Add(new SqlParameter("@RefID", this.RefID));
            prms.Add(new SqlParameter("@Amount", DataLayer.Decimal2Field(this.Amount)));
            prms.Add(new SqlParameter("@Secc", this.Secc));
            prms.Add(new SqlParameter("@StatusID", DataLayer.Int2Field(this.StatusID)));
            prms.Add(new SqlParameter("@MerchantID", DataLayer.Int2Field(MerchantID)));
            prms.Add(new SqlParameter("@NextProcDate", this.NextProcDate));
            prms.Add(new SqlParameter("@TransDate", this.TransDate));
            prms.Add(new SqlParameter("@Source", this.Source));
            prms.Add(new SqlParameter("@CheckNumber", this.CheckNumber));
            prms.Add(new SqlParameter("@OriginID", this.OriginID)); //13 = Comma Delimited File Format
            prms.Add(new SqlParameter("@UploadID", this.UploadID));
            prms.Add(new SqlParameter("@CompanyName", this.CompanyName));
            prms.Add(new SqlParameter("@IsResubmittedTrans", this.IsResubmittedTrans));

            prms.Add(new SqlParameter("@BillingAddress", this.BillingAddress));
            prms.Add(new SqlParameter("@BillingCity", this.BillingCity));
            prms.Add(new SqlParameter("@BillingState", this.BillingState));
            prms.Add(new SqlParameter("@BillingZip", this.BillingZip));
            prms.Add(new SqlParameter("@BillingCountry", this.BillingCountry));
            prms.Add(new SqlParameter("@BillingEmail", this.BillingEmail));
            prms.Add(new SqlParameter("@BillingPhone", this.BillingPhone));
            prms.Add(new SqlParameter("@ClientID", this.ClientID));
            //prms.Add(new SqlParameter("@OrigTransID", this.OriginalTransID));
            prms.Add(new SqlParameter("@ShippingAddress1", this.ShippingAddress));
            prms.Add(new SqlParameter("@ShippingAddress2", this.ShippingAddress2));
            prms.Add(new SqlParameter("@ShippingCity", this.ShippingCity));
            prms.Add(new SqlParameter("@ShippingState", this.ShippingState));
            prms.Add(new SqlParameter("@ShippingZip", this.ShippingZip));
            prms.Add(new SqlParameter("@ShippingCountry", this.ShippingCountry));
            prms.Add(new SqlParameter("@CustomInfo1", this.CustomInfo1));
            prms.Add(new SqlParameter("@CustomInfo2", this.CustomInfo2));
            prms.Add(new SqlParameter("@CustomInfo3", this.CustomInfo3));



            DataTransaction data = new DataTransaction();
            long lngID = data.InsertTransactionWS(prms);
            this.TransID = lngID;
            data = null;
            return lngID;

        }


        public bool SaveTransactionDetails()
        {
            bool perform = false;
            ArrayList prms = new ArrayList();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Ach_InsertTransactionDetails";
            cmd.CommandType = CommandType.StoredProcedure;

            prms.Add(new SqlParameter("@MerchantID", this.MerchantID));
            prms.Add(new SqlParameter("@MerchantAppUID", this.MerchantAppUID));
            prms.Add(new SqlParameter("@TransID", this.TransID));


            /* IMPORTANT NOTE!!! -- transactiondetail will ONLY be inserted if there is data in custom fields, iovation, or maxmind */

            //custom fields
            StringBuilder sb = new StringBuilder();
            sb.Append(this.CustomInfo4.Trim());
            sb.Append(this.CustomInfo5.Trim());
            sb.Append(this.CustomInfo6.Trim());
            sb.Append(this.CustomInfo7.Trim());
            sb.Append(this.CustomInfo8.Trim());
            sb.Append(this.CustomInfo9.Trim());
            sb.Append(this.CustomInfo10.Trim());
            sb.Append(this.CustomInfo11.Trim());
            sb.Append(this.CustomInfo12.Trim());
            sb.Append(this.CustomInfo13.Trim());
            sb.Append(this.CustomInfo14.Trim());
            sb.Append(this.CustomInfo15.Trim());
            sb.Append(this.CustomInfo16.Trim());
            sb.Append(this.CustomInfo17.Trim());
            sb.Append(this.CustomInfo18.Trim());
            sb.Append(this.CustomInfo19.Trim());
            sb.Append(this.CustomInfo20.Trim());

            //custom fields
            if (sb.Length > 0)
            {
                prms.Add(new SqlParameter("@CustomInfo4", this.CustomInfo4));
                prms.Add(new SqlParameter("@CustomInfo5", this.CustomInfo5));
                prms.Add(new SqlParameter("@CustomInfo6", this.CustomInfo6));
                prms.Add(new SqlParameter("@CustomInfo7", this.CustomInfo7));
                prms.Add(new SqlParameter("@CustomInfo8", this.CustomInfo8));
                prms.Add(new SqlParameter("@CustomInfo9", this.CustomInfo9));
                prms.Add(new SqlParameter("@CustomInfo10", this.CustomInfo10));
                prms.Add(new SqlParameter("@CustomInfo11", this.CustomInfo11));
                prms.Add(new SqlParameter("@CustomInfo12", this.CustomInfo12));
                prms.Add(new SqlParameter("@CustomInfo13", this.CustomInfo13));
                prms.Add(new SqlParameter("@CustomInfo14", this.CustomInfo14));
                prms.Add(new SqlParameter("@CustomInfo15", this.CustomInfo15));
                prms.Add(new SqlParameter("@CustomInfo16", this.CustomInfo16));
                prms.Add(new SqlParameter("@CustomInfo17", this.CustomInfo17));
                prms.Add(new SqlParameter("@CustomInfo18", this.CustomInfo18));
                prms.Add(new SqlParameter("@CustomInfo19", this.CustomInfo19));
                prms.Add(new SqlParameter("@CustomInfo20", this.CustomInfo20));

            }


            /* IMPORTANT NOTE!!! -- transactiondetail will ONLY be inserted if there is data in custom fields, iovation, or maxmind */
            if (prms.Count > 3)
            {
                DataLayer.AppendParamters(cmd, prms);
                int rows = DataLayer.ExecuteSQL(cmd,PaymentXP.DataObjects.DataLayer.ConnectStringACHBuild());

                if (rows > 0)
                {
                    perform = true;
                }
            }
            else
                perform = true;

            return perform;
        }

    }
}
