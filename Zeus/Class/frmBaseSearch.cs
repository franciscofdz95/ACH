using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Caching;

/// <summary>
/// Summary description for frmBaseSearch
/// </summary>
public class frmBaseSearch : System.Web.UI.Page
{
    private string _ViewStateKey = string.Empty;
    protected bool StoreViewStateOnServer { get; set; }

    override protected void OnInit(EventArgs e)
    {
        _ViewStateKey = Session.SessionID + GetPageName();

        base.OnInit(e);

        if (Context.Session != null && UserSessions.CurrentUser == null)
        {
            if (Session.IsNewSession)
            {
                string szCookieHeader = Request.Headers["Cookie"];
                if ((null != szCookieHeader) && (szCookieHeader.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    Response.Redirect("~/frmLogin.aspx");
                }
            }
        }

        if (!this.IsPostBack)
        {
            FormHandler.SetSecurity(this);
        }
    }

    public object SearchParameters
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_SearchParamters"] == null)
                return null;
            else
                return HttpContext.Current.Session[this.Page.ToString() + "_SearchParamters"];
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_SearchParamters"] = value; }
    }

    public string SortOrder
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_SortOrder"] == null)
                return string.Empty;
            else
                return HttpContext.Current.Session[this.Page.ToString() + "_SortOrder"].ToString();
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_SortOrder"] = value; }
    }

    public SortDirection SortDirectionSearch
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_SortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)HttpContext.Current.Session[this.Page.ToString() + "_SortDirectionSearch"];
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_SortDirectionSearch"] = value; }
    }

    public int CurrentPage
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_CurrentPage"] == null)
                return 1;
            else
                return Convert.ToInt32(HttpContext.Current.Session[this.Page.ToString() + "_CurrentPage"]);
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_CurrentPage"] = value; }
    }

    public int PageSize
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_PageSize"] == null)
                return 10;
            else
                return Convert.ToInt32(HttpContext.Current.Session[this.Page.ToString() + "_PageSize"]);
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_PageSize"] = value; }
    }

    public virtual void Search(bool IsOnLoad) { ;}
    
    //protected override object LoadPageStateFromPersistenceMedium()
    //{
    //    object result = null;

    //    if (StoreViewStateOnServer)
    //    {
    //        try
    //        {


    //            string str = Request.Form["__VIEWSTATE_KEY"];

    //            if (!str.StartsWith("VIEWSTATE_"))
    //            {

    //                throw new Exception("Invalid viewstate key:" + str);

    //            }

    //            //retrieve from cache
    //            //return Cache[str];

    //            //retrieve from session
    //            return Session[str];

    //            // Retrieve from cache
    //            //result = Cache.Get(_ViewStateKey);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new HttpException("Invalid view state", ex);
    //        }

    //        return result;
    //    }
    //    else
    //        return base.LoadPageStateFromPersistenceMedium();
    //}
       
    //protected override void SavePageStateToPersistenceMedium(object state)
    //{
    //    string cacheKey = string.Empty;

    //    if (StoreViewStateOnServer)
    //    {

    //        string key = GetKey();

    //        // Store into session
    //        Session[key] = state;
    //        RegisterHiddenField("__VIEWSTATE_KEY", key);
    //        RegisterHiddenField("__VIEWSTATE", "");

    //        // Store into Cache
    //        //Cache.Add(key, state, null, DateTime.Now.AddMinutes(Session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
    //        //RegisterHiddenField("__VIEWSTATE_KEY", key);
    //        //RegisterHiddenField("__VIEWSTATE", "");


    //    }
    //    else
    //        base.SavePageStateToPersistenceMedium(state);
    //}
       
    protected string GetKey()
    {
        string clientIp = CommonUtility.WebUtil.GetTrueClientIP(this.Request);

        string str = "VIEWSTATE_" + clientIp + "_" + DateTime.Now.Ticks.ToString();


        return str;
    }
       
    protected string GetPageName()
    {
        string page = string.Empty;
        page = HttpContext.Current.Request.Url.LocalPath;
        if (!string.IsNullOrEmpty(page))
        {
            if (page.Contains("/"))
            {
                page = page.Substring(page.LastIndexOf("/") + 1);
            }
        }

        return "VIEWSTATE_" + page;
    }
    

}
