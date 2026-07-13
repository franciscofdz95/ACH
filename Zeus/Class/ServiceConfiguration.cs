using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeusWeb.Class
{

    public class CustomerVault
    {
        public string status { get; set; }
    }

    public class Ispyfraud
    {
        public string status { get; set; }
    }

    public class PayerAuthentication
    {
        public string status { get; set; }
        public List<string> payment_types_allowed { get; set; }
    }

    public class PayerAuthentication2
    {
        public string status { get; set; }
        public List<string> payment_types_allowed { get; set; }
    }

    public class Centinel2
    {
        public string status { get; set; }
        public List<string> payment_types_allowed { get; set; }
        public string org_unit_id { get; set; }
        public string api_identifier { get; set; }
        public string apiKey { get; set; }
        public string symmetric_key { get; set; }
    }

    public class AirlineIndustry
    {
        public string status { get; set; }
    }

    public class Certifypci
    {
        public string status { get; set; }
        public bool insurance { get; set; }
    }

    public class EncryptedDevices
    {
        public string status { get; set; }
    }

    public class EnhancedData
    {
        public string status { get; set; }
    }

    public class Invoicing
    {
        public string status { get; set; }
    }

    public class MobilePayments
    {
        public string status { get; set; }
    }

    public class QuickbooksSyncpay
    {
        public string status { get; set; }
    }

    public class ServiceConfiguration
    {

        public CustomerVault customer_vault { get; set; }
        public Ispyfraud ispyfraud { get; set; }
        public PayerAuthentication payer_authentication { get; set; }
        public PayerAuthentication2 payer_authentication_2 { get; set; }
        public Centinel2 centinel_2 { get; set; }
        public AirlineIndustry airline_industry { get; set; }
        public Certifypci certifypci { get; set; }
        public EncryptedDevices encrypted_devices { get; set; }
        public EnhancedData enhanced_data { get; set; }
        public Invoicing invoicing { get; set; }
        public MobilePayments mobile_payments { get; set; }
        public QuickbooksSyncpay quickbooks_syncpay { get; set; }
    }

    public class NMIApiResponse
    {
        public NMIApiResponse()
        {
            IsError = true;
        }
        public bool IsError { get; set; }
        public string type { get; set; }
        public string message { get; set; }
    }
}

