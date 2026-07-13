using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


public class CreditCardTransaction : Transaction, iCreditCard
{
    string m_ExpirationDate;

    public CreditCardTransaction()
    { }

    public string ExpirationDate
    {
        get
        {
            return m_ExpirationDate;
        }
        set
        {
            m_ExpirationDate = value;
        }
    }

    public override bool Validate()
    {
        return false;
    }


    public override bool ValidateRequiredFields()
    {
        return false;
    }

    public override void SaveTransaction()
    {


    }
}

