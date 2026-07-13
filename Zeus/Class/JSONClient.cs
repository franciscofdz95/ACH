using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace ZeusWeb.Class
{
    public static class JSONClient
    {
        public static JavaScriptSerializer Serializer = new JavaScriptSerializer();

        public static string PostRestService(string json, string endPointURL)
        {
            //Logging.LogFile = Logging.LoggerName.Zeus3DE_ServiceRequest;
            //Logging.Info("PostJson() json");
            string jsonresponse = string.Empty;

            try
            {
                //Required for SSL certificate validation.
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                // string EndPoint = ConfigurationManager.AppSettings["Zeus3dePostRequest"];
                //TO DO: Move to COnfig when the token is final.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPointURL);
                request.Method = "POST";
                request.ContentType = "application/json ; charset=utf-8";

                // Chandra: For PXP-2982 TLS1.2: Zeus
                ServicePointManager.SecurityProtocol = System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //SecurityProtocolType.Tls12;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.ASCII))
                            {
                                jsonresponse = reader.ReadToEnd();
                                jsonresponse = jsonresponse.TrimStart('"');
                                jsonresponse = jsonresponse.TrimEnd('"');
                                //I will change the response from service to a Json and get more details onto Zeus.
                                //That is the best way instead of making a call to the database as we are spending time anyways to call the service.
                                //jsonresponse = "Application accepted and is being processed";
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    WebResponse errorResponse = ex.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream);
                        jsonresponse = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                //Logging.Exception("PostJson() json", ex);
            }
            return jsonresponse.Replace(@"\","");
        }
    }
}