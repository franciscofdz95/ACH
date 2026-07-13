using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using HtmlAgilityPack;
using Infragistics.Web.UI.EditorControls;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;


public class WebUtil
{
    public WebUtil()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// returns the current url for the page.
    /// </summary>
    /// <returns></returns>
    public static string GetMyUrl()
    {
        string real_script_name = WebUtil.GetRealScriptName();

        return string.Format("{0}{1}{2}"
                , WebUtil.GetBaseUrl().TrimEnd(new char[] { '/' })
                , real_script_name
                , (CommonUtility.Util.if_s(HttpContext.Current.Request.ServerVariables["QUERY_STRING"], "") == "")
                        ? ""
                        : "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"]
                );
    }

    /// <summary>
    /// gets the current URL with query string modifications
    /// </summary>
    /// <param name="diParams"></param>
    /// <param name="IsOverwrite"></param>
    /// <returns></returns>
    public static string GetMyUrl(Dictionary<string, string> diParams, bool PurgeExisting)
    {
        string q = "";

        if (PurgeExisting == true)
        {
            if (diParams.Count > 0)
            {
                q = CommonUtility.Util.DictToUrl(diParams);
            }
        }
        else
        {
            // no purging. we just overwrite

            // get current query string.
            Dictionary<string, string> di = CommonUtility.Util.UrlToDict(HttpContext.Current.Request.ServerVariables["QUERY_STRING"]);

            // set new values from params
            foreach (KeyValuePair<string, string> kvp in diParams)
            {
                di[kvp.Key] = kvp.Value;
            }

            q = CommonUtility.Util.DictToUrl(di);
        }

        string real_script_name = WebUtil.GetRealScriptName();

        return string.Format("{0}{1}{2}"
                , WebUtil.GetBaseUrl().TrimEnd(new char[] { '/' })
                , real_script_name
                , (q == "") ? "" : "?" + q
                );
    }


    private static string RemoveScriptTagsFromHtml(string html)
    {
        string result = System.Text.RegularExpressions.Regex.Replace(html,
                @"<( )*script([^>])*>", "<script>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<( )*(/)( )*script( )*>)", "</script>",
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);
       result = System.Text.RegularExpressions.Regex.Replace(result,
                 @"(<script>).*(</script>)", string.Empty,
                 System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        return result;
    }

    public static string ConvertHtml(string html)
    {
        //This is a method for handling the smartness of infragistics control. 
        //Infragistics is smart enough, it is encoding the potentially unsafe code twice, instead of once.
        //This would work even if we are encoding the text from external emails (As we encode the text only once.)
        //This will eliminate the risk of binding script to the page when we are converting the html to text.
        html = RemoveScriptTagsFromHtml(HttpContext.Current.Server.HtmlDecode(html));
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(html);

        StringWriter sw = new StringWriter();
        ConvertTo(doc.DocumentNode, sw);
        sw.Flush();
        string temp = sw.ToString();
        return sw.ToString();
    }


    private static void ConvertTo(HtmlNode node, TextWriter outText)
    {
        string html;
        switch (node.NodeType)
        {
            case HtmlNodeType.Comment:
                // don't output comments
                break;

            case HtmlNodeType.Document:
                ConvertContentTo(node, outText);
                break;

            case HtmlNodeType.Text:
                // script and style must not be output
                string parentName = node.ParentNode.Name;
                if ((parentName == "script") || (parentName == "style"))
                    break;

                // get text
                html = ((HtmlTextNode)node).Text;

                // is it in fact a special closing node output as text?
                if (HtmlNode.IsOverlappedClosingElement(html))
                    break;

                // check the text is meaningful and not a bunch of whitespaces
                if (html.Trim().Length > 0)
                {
                    outText.Write(HtmlEntity.DeEntitize(html));
                }
                break;

            case HtmlNodeType.Element:
                switch (node.Name)
                {
                    case "p":
                        // treat paragraphs as crlf
                        outText.Write("\r\n");
                        break;
                    //treat div as a new line
                    case "div":
                        outText.Write("\r\n");
                        break;
                    //treat li as a new line 
                    case "li":
                        outText.Write("\r\n");
                        break;
                    //treat br as a new line
                    case "br":
                        outText.Write("\r\n");
                        break;

                }

                if (node.HasChildNodes)
                {
                    ConvertContentTo(node, outText);
                }
                break;
        }
    }


    private static void ConvertContentTo(HtmlNode node, TextWriter outText)
    {
        foreach (HtmlNode subnode in node.ChildNodes)
        {
            ConvertTo(subnode, outText);
        }
    }

    public static string GetMyUrl(string UrlParams)
    {
        string q = UrlParams.Trim();

        string real_script_name = WebUtil.GetRealScriptName();

        return string.Format("{0}{1}{2}"
                , WebUtil.GetBaseUrl().TrimEnd(new char[] { '/' })
                , real_script_name
                , (q == "") ? "" : "?" + q
                );
    }

    public static string GetRealScriptName()
    {
        string default_scriptname = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];

        string sc = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];

        if (HttpContext.Current.Request.ApplicationPath != "/")
        {
            if (sc.ToUpper().StartsWith(HttpContext.Current.Request.ApplicationPath.ToUpper()))
            {
                default_scriptname = sc.Remove(0, HttpContext.Current.Request.ApplicationPath.Length);

            }
        }

        return default_scriptname;
    }

    /// <summary>
    /// gets the current base url of the web application. always returns a trailing slash.
    /// </summary>
    /// <returns></returns>
    public static string GetBaseUrl()
    {
        // for instance:
        //      http://localhost:55296/ZeusWeb/testing.aspx ->  http://localhost:55296/ZeusWeb/
        //      http://dev.zeus/testing.aspx ->                 http://dev.zeus/

        string key_name = "MPS.GetBaseUrl";

        if (HttpContext.Current == null)
        {
            throw new Exception("This function requires HttpContext.Current");
        }

        if (!(HttpContext.Current.Items[key_name] != null
                && HttpContext.Current.Items.Contains(key_name) == true
                && HttpContext.Current.Items[key_name].ToString() != ""))
        {
            string protocol = WebUtil.GetProtocol();

            string application = HttpContext.Current.Request.ApplicationPath;

            if (!application.EndsWith("/"))
            {
                application += "/";
            }

            // if session is being passed thru the url, then we need to keep the SessionID.
            // see: http://msdn.microsoft.com/en-us/library/system.web.httpresponse.applyapppathmodifier.aspx
            application = HttpContext.Current.Response.ApplyAppPathModifier(application);

            HttpContext.Current.Items[key_name] = String.Format("{0}{1}{2}", protocol, HttpContext.Current.Request.ServerVariables["HTTP_HOST"], application);
        }

        return HttpContext.Current.Items[key_name].ToString();


    }

    /// <summary>
    /// are we using http or https?
    /// </summary>
    /// <returns></returns>
    public static string GetProtocol()
    {
        return (Convert.ToInt32(HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"]) == 1) ? "https://" : "http://";
    }

    public static string GetMimeType(string fileName)
    {
        string mimeType = "application/unknown";
        string ext = System.IO.Path.GetExtension(fileName).ToLower();
        Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (regKey != null && regKey.GetValue("Content Type") != null)
        {
            mimeType = regKey.GetValue("Content Type").ToString();
        }
        return mimeType;
    }

    /// <summary>
    /// carefully injects a GET variable. overwrites if it already exists.
    /// </summary>
    /// <param name="url">a relative or absolute url</param>
    /// <param name="mykey"></param>
    /// <param name="myval"></param>
    /// <returns></returns>
    public static string InjectParam(string url, string mykey, string myval)
    {
        string final_url = url;

        url = CommonUtility.Util.if_s(url).Trim();
        mykey = CommonUtility.Util.if_s(mykey).Trim();
        myval = CommonUtility.Util.if_s(myval).Trim();

        string[] arrQuery = url.Split(new char[] { '?' });

        if (arrQuery.Length == 1)
        {
            // no question mark, so we just append.
            final_url = string.Format("{0}?{1}={2}", url, mykey, myval);
        }
        else if (arrQuery.Length >= 2)
        {
            // has at least 1 question mark, so we solidify that first
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i < arrQuery.Length; i++)
            {
                sb.Append(arrQuery[i] + "&");
            }

            // now that we have everythign fter the question mark, we create a dictionary and insert.
            string afterQ = sb.ToString();
            Dictionary<string, string> di = CommonUtility.Util.UriQueryToDict(afterQ);
            di[mykey] = myval;
            final_url = string.Format("{0}?{1}", arrQuery[0], CommonUtility.Util.DictToUrl(di, false));
        }

        return final_url;
    }


    /// <summary>
    /// Converts PST datetime format for a given date to a user configured time zone, date and time pattern
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ConvertToUserDateTimeSettings(string Date)
    {
        //Check the Timezone of the current user and convert.
        DateTime theDate;

        if (DateTime.TryParse(Date, out theDate))
        {
            if (UserSessions.CurrentUser != null)
            {
                theDate = CommonUtility.DateTimeUtil.ConvertPSTToUserTime(theDate, UserSessions.CurrentUser.TimeZone);
                Date = theDate.ToString(UserSessions.CurrentUser.DatePattern + " " + UserSessions.CurrentUser.TimePattern);
            }
        }
        
        return Date;
    }

    /// <summary>
    /// Converts PST datetime format for a given date to a user configured time zone and date pattern short date.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ConvertToUserShortDateTimeFormat(string Date)
    {
        //Check the Timezone of the current user and convert.
        DateTime theDate;

        if (DateTime.TryParse(Date, out theDate))
        {
            if (UserSessions.CurrentUser != null)
            {
                theDate = CommonUtility.DateTimeUtil.ConvertPSTToUserTime(theDate, UserSessions.CurrentUser.TimeZone);
                Date = theDate.ToString(UserSessions.CurrentUser.DatePattern);
            }
        }
        
        return Date;
    }


    /// <summary>
    /// Converts the datetime to user configured date pattern.
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ConvertToUserDatePattern(string Date)
    {
        //Check the Timezone of the current user and convert.
        if (!String.IsNullOrEmpty(UserSessions.CurrentUser.DatePattern))
        {
            DateTime theDate;
            if (DateTime.TryParse(Date, out theDate))
            {
                Date = theDate.ToString(UserSessions.CurrentUser.DatePattern);
            }
        }

        return Date;
    }

    public static string CovertToUserDateTimePattern(DateTime dt)
    {
        if (dt == null)
            throw new ArgumentNullException("DateTime parameter cannot be null.");

        //Check the Timezone of the current user and convert.
        if (!String.IsNullOrEmpty(UserSessions.CurrentUser.DatePattern) && !String.IsNullOrEmpty(UserSessions.CurrentUser.TimePattern))
        {
            return dt.ToString(UserSessions.CurrentUser.DatePattern + " " + UserSessions.CurrentUser.TimePattern);
        }

        return dt.ToString();
    }

    public static WebDatePicker SetUserSpecificDisplayMode(WebDatePicker webDatePicker)
    {
        WebDatePicker wdp = webDatePicker;

        if (wdp != null)
        {
            wdp.DisplayModeFormat = UserSessions.CurrentUser.DatePattern;
            wdp.EditModeFormat = UserSessions.CurrentUser.DatePattern;
        }

        return webDatePicker;
    }


    public static string BuildTicketTooltip(DataRow dr)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<table class='mGrid'>");

        if (dr.Table.Columns.Contains("Department"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Department", dr["Department"].ToString());
        }

        if (dr.Table.Columns.Contains("CategoryID"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Category", TicketNotification.GetTicketParentCategory(CommonUtility.Util.if_i(dr["CategoryID"].ToString(), 0)));
        }

        if (dr.Table.Columns.Contains("Category"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "SubCategory", dr["Category"].ToString());
        }

        if (dr.Table.Columns.Contains("FMAID"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "FMA ID", dr["FMAID"].ToString());
        }

        if (dr.Table.Columns.Contains("Office"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Office", dr["Office"].ToString());
        }

        if (dr.Table.Columns.Contains("Tags"))
        {
            sb.AppendFormat("<tr><th>{0}</th><td>{1}</td></tr>", "Tags", dr["Tags"].ToString());
        }

        sb.Append("</table>");

        return sb.ToString();
    }

    /// <summary>
    /// code changes for PXP-6927 by koshlendra
    /// </summary>
    /// <param name="docType"></param>
    /// <returns></returns>
    public static string GetDocIconUrl(string docType)
    {
        string imagePath = "~/images/document_view.png";
        switch (docType)
        {

            case "PDF":
                imagePath = "~/images/pdf.png";
                break;
            case "DOC":
            case "DOCX":
                // System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
                imagePath = "~/images/word.jpg";
                break;
            case "XLS":
            case "XLSX":
                imagePath = "~/images/excel.jpg";
                break;
            case "MSG":
                imagePath = "~/images/outlook.jpg";
                break;
            case "ZIP":
                imagePath = "~/images/zip.JPG";
                break;
            // images
            case "GIF":
            case "PNG":
            case "TIF":
            case "TIFF":
            case "BMP":
            case "JPG":
            case "JPEG":
                imagePath = "~/images/photo.jpg";
                break;
            default:
                imagePath = "~/images/document_view.png";
                break;


        }
        return imagePath;
    }

    //DM-7233 -- Nisha Magnani
    public static bool IsTilledStatus(MerchantApp app)
    {
        return app != null ? app.IsTilled : false;
    }

}
