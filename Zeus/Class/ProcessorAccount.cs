using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeusWeb.Class
{
    class ProcessorAccount
    {
        public string platform { get; set; }
        public string default_industry_classification { get; set; }
        public string max_transaction_amount { get; set; }
        public string max_monthly_volume { get; set; }
        public bool enable_duplicate_checking { get; set; }
        public bool allow_duplicate_checking_override { get; set; }
        public int duplicate_checking_seconds { get; set; }
        public string processor_description { get; set; }
        public int mcc { get; set; }
        public string currencies { get; set; }
        public string payment_types { get; set; }
        public string required_fields { get; set; }
        public string processor_config_1 { get; set; }
        public string processor_config_2 { get; set; }
        //PXP-13342 by Satyajit
        public bool? processor_config_3 { get; set; }
        public bool? processor_config_4 { get; set; }
        public bool? processor_config_5 { get; set; }
        public bool? processor_config_6 { get; set; }
        public bool? processor_config_7 { get; set; }
        public string settlement_time { get; set; }
    }

    class ServiceTerms
    {
        public bool agree_to_tos { get; set; }
        public bool agree_to_billing_auth { get; set; }
    }
}
