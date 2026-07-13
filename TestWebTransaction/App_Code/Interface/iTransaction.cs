using System;
using System.Collections.Generic;
using System.Text;


interface iTransaction
{
    string MerchantID { get; set;}
    string MerchantKey { get; set;}

    long TransID { get; set;}
    string AccountNumber { get; set; }
    decimal Amount { get; set;}
    string ReferenceNumber { get; set;}
    string Description { get; set;}
    string CompanyName { get; set;}
    TransactionType TransactionType { get; set;}
    TransactionResponse TransactionResponse { get; set;}

    //Billing Info
    string CustomerID { get; set;}
    string BillingFirstName { get; set;}
    string BillingLastName { get; set;}
    string BillingAddress { get; set;}
    string BillingCity { get; set;}
    string BillingState { get; set;}
    string BillingZipcode { get; set;}
    string BillingCountry { get; set;}
    string BillingPhone { get; set;}
    string BillingFax { get; set;}
    string BillingEmail { get; set;}

    //Shipping Info
    bool SameAsBilling { get; set;}
    string ShippingFirstName { get; set;}
    string ShippingLastName { get; set;}
    string ShippingAddress { get; set;}
    string ShippingCity { get; set;}
    string ShippingState { get; set;}
    string ShippingZipcode { get; set;}
    string ShippingCountry { get; set;}
    

    //Methods
    bool Validate();
    bool ValidateRequiredFields();
   
}

