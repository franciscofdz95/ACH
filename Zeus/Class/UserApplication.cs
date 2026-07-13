using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class UserApplication
{

    /// <summary>
    /// merchantappuid ->user, datetime
    /// </summary>
    public static Dictionary<string, Dictionary<string, DateTime>> diMerchantAccess
    {
        get
        {
            if (HttpContext.Current.Application["diMerchantAccess"] == null)
                return null;
            else
                return (Dictionary<string, Dictionary<string, DateTime>>)HttpContext.Current.Application["diMerchantAccess"];
        }
        set { HttpContext.Current.Application["diMerchantAccess"] = value; }
    }

    public static void AddViewLogMerchant(string merchantappuid, string username)
    {
        if (CommonUtility.Util.IsValidGuid(merchantappuid) && !string.IsNullOrEmpty(username))
        {
            merchantappuid = merchantappuid.ToUpper();

            if (diMerchantAccess == null)
            {
                diMerchantAccess = new Dictionary<string, Dictionary<string, DateTime>>();
            }

            if (!diMerchantAccess.ContainsKey(merchantappuid))
            {
                diMerchantAccess[merchantappuid] = new Dictionary<string, DateTime>();
            }
            
            diMerchantAccess[merchantappuid][username] = DateTime.Now;
        }

    }



    /// <summary>
    /// TicketUID ->user, datetime
    /// </summary>
    public static Dictionary<string, Dictionary<string, DateTime>> diTicketAccess
    {
        get
        {
            if (HttpContext.Current.Application["diTicketAccess"] == null)
                return null;
            else
                return (Dictionary<string, Dictionary<string, DateTime>>)HttpContext.Current.Application["diTicketAccess"];
        }
        set { HttpContext.Current.Application["diTicketAccess"] = value; }
    }

    public static void AddViewLogTicket(string TicketUID, string username)
    {
        if (CommonUtility.Util.IsValidGuid(TicketUID) && !string.IsNullOrEmpty(username))
        {
            TicketUID = TicketUID.ToUpper();

            if (diTicketAccess == null)
            {
                diTicketAccess = new Dictionary<string, Dictionary<string, DateTime>>();
            }

            if (!diTicketAccess.ContainsKey(TicketUID))
            {
                diTicketAccess[TicketUID] = new Dictionary<string, DateTime>();
            }

            diTicketAccess[TicketUID][username] = DateTime.Now;
        }

    }

}
