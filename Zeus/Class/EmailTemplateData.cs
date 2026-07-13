using System;
using System.Collections.Generic;
using System.Web;
using PaymentXP.BusinessObjects;

/// <summary>
/// Summary description for EmailValue
/// </summary>
public class EmailTemplateData
{
	public EmailTemplateData() 
    {
        CompanyName = "Paysafe Solutions";
        ProductName = "Payment XP";
        ProductUrl = "https://www.paymentxp.com";
        ClientServicePhoneNumber = "1-888-851-7558";
        ClientServiceEmailAddress = Constants.IRVINE_INFO;
    }

    public EmailTemplateData(PrivateLabel privateLabel)
    {
        if (privateLabel != null)
        {
            CompanyName = privateLabel.PLCompanyName;
            ProductName = privateLabel.PLVTProductName;
            ProductUrl = privateLabel.VTURL;
            ClientServicePhoneNumber = privateLabel.PLPhone;
            ClientServiceEmailAddress = privateLabel.PLEmail;
        }
    }

    private string companyName = string.Empty;
    public string CompanyName 
    {
        get
        {
            return companyName;
        }
        set
        {
            companyName = value;
        }
    }

    private string productName = string.Empty;
    public string ProductName
    {
        get
        {
            return productName;
        }
        set
        {
            productName = value;
        }
    }

    private string productUrl = string.Empty;
    public string ProductUrl
    {
        get
        {
            return productUrl;
        }
        set
        {
            productUrl = value;
        }
    }

    private string clientServicePhoneNumber = string.Empty;
    public string ClientServicePhoneNumber
    {
        get
        {
            return clientServicePhoneNumber;
        }
        set
        {
            clientServicePhoneNumber = value;
        }
    }

    private string clientServiceEmailAddress = string.Empty;
    public string ClientServiceEmailAddress
    {
        get
        {
            return clientServiceEmailAddress;
        }
        set
        {
            clientServiceEmailAddress = value;
        }
    }
}