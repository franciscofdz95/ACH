using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

public class wucBaseSearch : System.Web.UI.UserControl
{
    public object SearchParameters
    {
        get
        {
            if (HttpContext.Current.Session[this.ClientID + "_SearchParamters"] == null)
                return null;
            else
                return HttpContext.Current.Session[this.ClientID + "_SearchParamters"];
        }
        set { HttpContext.Current.Session[this.ClientID + "_SearchParamters"] = value; }
    }

    public string SortOrder
    {
        get
        {
            if (ViewState[this.ClientID + "_SortOrder"] == null)
                return string.Empty;
            else
                return ViewState[this.ClientID + "_SortOrder"].ToString();
        }
        set { ViewState[this.ClientID + "_SortOrder"] = value; }
    }

    public SortDirection SortDirectionSearch
    {
        get
        {
            if (ViewState[this.ClientID + "_SortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)ViewState[this.ClientID + "_SortDirectionSearch"];
        }
        set { ViewState[this.ClientID + "_SortDirectionSearch"] = value; }
    }

    public int CurrentPage
    {
        get
        {
            if (ViewState[this.ClientID + "_CurrentPage"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState[this.ClientID + "_CurrentPage"]);
        }
        set { ViewState[this.ClientID + "_CurrentPage"] = value; }
    }

    public int PageSize
    {
        get
        {

            if (ViewState[this.ClientID + "_PageSize"] == null)
                return 10;
            else
                return Convert.ToInt32(ViewState[this.ClientID + "_PageSize"]);
        }
        set { ViewState[this.ClientID + "_PageSize"] = value; }
    }

    public Hashtable m_Prms
    {
        get
        {
            if (ViewState["m_Prms"] == null)
                return null;
            else
                return (Hashtable)ViewState["m_Prms"];
        }
        set { ViewState["m_Prms"] = value; }
    }

    public wucBaseSearch()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool IncludeReservesHeldAtMeritus
    {
        get { return (bool)(ViewState["IncludeReservesHeldAtMeritus"] ?? false); }
        set { ViewState["IncludeReservesHeldAtMeritus"] = value; }
    }

}

