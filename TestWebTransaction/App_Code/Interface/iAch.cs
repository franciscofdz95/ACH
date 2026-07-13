using System;
using System.Collections.Generic;
using System.Text;


interface iAch
{
    string RoutingNumber { get; set; }
    string AccountName { get; set; }
    BankAccountType BankAccountType { get; set; }
}

