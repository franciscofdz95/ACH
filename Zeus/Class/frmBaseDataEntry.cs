using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Caching;



/// <summary>
/// Summary description for frmBaseDataEntry
/// </summary>
public abstract class frmBaseDataEntry : System.Web.UI.Page
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
            FormHandler.LogUserAccess();

            FormHandler.SetSecurity(this);
        }

    }

    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["EditMode"]);
        }
        set { ViewState["EditMode"] = value; }

    }

    public bool Adding
    {
        get { return Convert.ToBoolean(ViewState["Adding"]); }
        set { ViewState["Adding"] = Convert.ToBoolean(value); }
    }

    //public string UIDName
    //{
    //    get { return Convert.ToString(ViewState["UIDName"]); }
    //    set { ViewState["UIDName"] = Convert.ToString(value); }
    //}

    public int RecordIndex
    {
        get 
        {
            if (ViewState["RecordIndex"] == null)
                return -1;
            else
                return Convert.ToInt32(ViewState["RecordIndex"]); 
        }
        set { ViewState["RecordIndex"] = Convert.ToInt32(value); }
    }

    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }

    public abstract void FormShow(string ID);

    public abstract void FormClear();

    public abstract bool FormSave();

    public abstract void FormNew();

    public abstract bool FormDelete();

    public abstract bool FormDataCheck();

    public abstract void FormCancel();

    public abstract void ToggleButtons();


    protected string SortOrder
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

    protected SortDirection SortDirectionSearch
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

    protected int CurrentPage
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

    protected int PageSize
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


    protected Hashtable m_Prms
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

    protected object SearchParameters
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

  
    protected override object LoadPageStateFromPersistenceMedium()
    {
        object result = null;

        if (StoreViewStateOnServer)
        {
            try
            {


                string str = Request.Form["__VIEWSTATE_KEY"];

                if (!str.StartsWith("VIEWSTATE_"))
                {

                    throw new Exception("Invalid viewstate key:" + str);

                }

                //retrieve from cache
                //return Cache[str];

                //retrieve from session
                return Session[str];

                // Retrieve from cache
                //result = Cache.Get(_ViewStateKey);
            }
            catch (Exception ex)
            {
                throw new HttpException("Invalid view state", ex);
            }

            return result;
        }
        else
            return base.LoadPageStateFromPersistenceMedium();
    }
  

    
    
    protected override void SavePageStateToPersistenceMedium(object state)
    {
        string cacheKey = string.Empty;

        if (StoreViewStateOnServer)
        {

            string key = GetKey();

            if (!string.IsNullOrEmpty(this.UID))
            {
                key = GetUID();
            }

            // Store into session
            Session[key] = state;
            RegisterHiddenField("__VIEWSTATE_KEY", key);
            RegisterHiddenField("__VIEWSTATE", "");

            // Store into Cache
            //Cache.Add(key, state, null, DateTime.Now.AddMinutes(Session.Timeout), TimeSpan.Zero, CacheItemPriority.Default, null);
            //RegisterHiddenField("__VIEWSTATE_KEY", key);
            //RegisterHiddenField("__VIEWSTATE", "");


        }
        else
            base.SavePageStateToPersistenceMedium(state);
    }
   


   
    protected string GetKey()
    {
        string clientIp = CommonUtility.WebUtil.GetTrueClientIP(this.Request);

        string str = "VIEWSTATE_" + clientIp + "_" + DateTime.Now.Ticks.ToString();
        

        return str;
    }

    protected string GetUID()
    {

        //return "VIEWSTATE_" + Request.UserHostAddress + "_" + this.UID;
        return GetPageName() + "_" + this.UID;
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

        string clientIp = CommonUtility.WebUtil.GetTrueClientIP(this.Request);

        return "VIEWSTATE_" + clientIp + "_" + page;
        
    }
    

}
