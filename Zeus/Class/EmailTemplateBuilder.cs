using System;
using System.Collections.Generic;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for EmailTemplateBuilder
/// </summary>
public class EmailTemplateBuilder
{
    public class Constants
    {
        public static string UserGuide = @"https://www.paymentxp.com/Resources/VT_User_Guide.pdf";
        public static string VTInterfaceGuide = @"https://www.paymentxp.com/Resources/Interface_Guide.pdf";
        public static string InterfaceGuide = @"https://www.paymentxp.com/Resources/Interface_Guide.pdf";
        public static string WebHostPostingUrl = @"https://webservice.paymentxp.com/wh/WebHost.aspx";
        public static string PaymentXpUrl = @"http://www.PaymentXP.com";
        public static string ClientServicesEmailAddress = Constants.ClientServicesEmailAddress;
    }

	public static void CreateEmailFooter(ref StringBuilder sb, string productName, string privateLabelUrl, string privateLabelCustomerServiceEmail)
    {
        CreateBoldLine(ref sb,
                        "If You Have Questions");

        CreateLine(ref sb,
                        String.Format("For technical integration questions about {0} Gateway solution please visit {1} or email us at {2}"
                        , productName
                        , privateLabelUrl
                        , privateLabelCustomerServiceEmail));
    }

    public static string CreateUrl(string linkUrl)
    {
        return String.Format(@"<a href='{0}'>{0}</a>", linkUrl);
    }

    public static string CreateUrl(string linkName, string linkUrl)
    {
        return String.Format(@"<a href='{0}'>{1}</a>", linkUrl, linkName);
    }

    public static string GetStyleAttribute()
    {
        return @"style='font-size:10.0pt;font-family:Verdana;'";
    }
    public static string CreateMailTo(string address)
    {
        return String.Format(@"<a mailto='{0}'>{0}</a>", address);
    }

    public static void CreateHeaderLine(ref StringBuilder sb, string str)
    {
        sb.Append("<span style='font-size:14.5pt;font-family:Verdana;color:gray;letter-spacing:-.75pt'>" + str + "</span>");
    }

    public static void CreateLine(ref StringBuilder sb, string str)
    {
        sb.Append("<span " + GetStyleAttribute() + ">" + str + "</span><BR/>");
    }

    public static void CreateBoldLine(ref StringBuilder sb, string str)
    {
        sb.Append("<span " + GetStyleAttribute() + "><b>" + str + "</b></span><BR/>");
    }

    public static void CreateSimpleLine(ref StringBuilder sb, string html)
    {
        sb.Append(html);
    }

    public static void CreateSingleLineBreak(ref StringBuilder sb)
    {
        sb.Append("<BR/>");
    }
}