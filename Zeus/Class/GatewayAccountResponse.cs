using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeusWeb.Class
{
    class GatewayAccountResponse
    {
        public int id { get; set; }
        public string company { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal { get; set; }
        public string country { get; set; }
        public string url { get; set; }
        public int timezone_id { get; set; }
        public string contact_first_name { get; set; }
        public string contact_last_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_email { get; set; }
        public string account_number { get; set; }
        public string routing_number { get; set; }
        public string username { get; set; }
        public int fee_plan { get; set; }
        public string external_identifier { get; set; }
        public string locale { get; set; }
        public string account_type { get; set; }
        public string account_holder_type { get; set; }
        public string security_key { get; set; }

    }
}
