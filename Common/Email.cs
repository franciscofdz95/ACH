using System;
using System.Collections.Generic;
using System.Text;
//using System.Web.Mail;
using System.Configuration;
using System.Collections;

using System.IO;
using System.Reflection;
using System.Net.Mail;

namespace Nmc.Common
{
    public class Email
    {

        

        public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                // Recipients
                string[] recipients = strTo.Split(new char[] { ';' });
                foreach (string recipient in recipients)
                    message.To.Add(recipient);

                // Body
                message.Body = strBody;

                // Subject
                message.Subject = strSubject;

                // From
                if (strFrom == null || strFrom == string.Empty)
                    message.From = new MailAddress("mnguyen@merituspayment.com", "Client Services");
                else
                {
                    message.From = new MailAddress(strFrom);
                }

                


                //SmtpClient client = new SmtpClient("192.168.1.6");
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());

                string user = ConfigurationManager.AppSettings["SMTPUser"].ToString();
                if (user != string.Empty)
                    client.Credentials = new System.Net.NetworkCredential(user, ConfigurationManager.AppSettings["SMTPPassword"].ToString());

                string port = ConfigurationManager.AppSettings["SMTPPort"].ToString();
                if (port != string.Empty)
                    client.Port = Convert.ToInt32(port);

                //client.UseDefaultCredentials = true;
                client.Send(message);

                client = null;
                message = null;
                return true;
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return false;
            }
        }
        

        public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo, string strBcc)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                // Recipients
                string[] recipients = strTo.Split(new char[] { ';' });
                foreach (string recipient in recipients)
                    message.To.Add(recipient);

                // Body
                message.Body = strBody;

                // Subject
                message.Subject = strSubject;

                // From
                if (strFrom == null || strFrom == string.Empty)
                    message.From = new MailAddress("clientservices@merituspayment.com", "Client Services");
                else
                {
                    message.From = new MailAddress(strFrom);
                }

                // Carbon copy
                //if (cc != null)
                //    foreach (string carboncopy in cc)
                //        message.CC.Add(carboncopy);

                // Blind copy
                if (strBcc != null)
                    message.Bcc.Add(strBcc);

                // Attachments
                //if (attachments != null)
                //    foreach (object attachment in attachments)
                //        message.Attachments.Add(new Attachment(attachment.ToString()));


                //SmtpClient client = new SmtpClient("192.168.1.6");
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
                client.UseDefaultCredentials = true;
                client.Send(message);

                client = null;
                message = null;
                return true;
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return false;
            }
        }

        public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo, ArrayList attachments)
        {
            try
            {

                MailMessage message = new MailMessage();
                message.Priority = MailPriority.Normal;
                message.IsBodyHtml = true;

                // Recipients
                string[] recipients = strTo.Split(new char[] { ';' });
                foreach (string recipient in recipients)
                    message.To.Add(recipient);

                // Body
                message.Body = strBody;

                // Subject
                message.Subject = strSubject;

                // From
                if (strFrom == null || strFrom == string.Empty)
                    message.From = new MailAddress("clientservices@merituspayment.com", "Client Services");
                else
                {
                    message.From = new MailAddress(strFrom);
                }

                // Carbon copy
                //if (cc != null)
                //    foreach (string carboncopy in cc)
                //        message.CC.Add(carboncopy);

                // Blind copy
                //if (bcc != null)
                //    foreach (string blindcopy in bcc)
                //        message.Bcc.Add(blindcopy);

                // Attachments
                if (attachments != null)
                    foreach (object attachment in attachments)
                        message.Attachments.Add(new Attachment(attachment.ToString()));


                //SmtpClient client = new SmtpClient("192.168.1.6");
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
                client.UseDefaultCredentials = true;
                client.Send(message);

                client = null;
                message = null;
                return true;
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return false;
            }
        }



        //public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo)
        //{
        //    try
        //    {

        //        MailMessage message = new MailMessage();
        //        message.Priority = MailPriority.Normal;
        //        message.IsBodyHtml = true;

        //        // Recipients
        //        string[] recipients = strTo.Split(new char[] { ';' });
        //        foreach (string recipient in recipients)
        //            message.To.Add(recipient);

        //        // Body
        //        message.Body = strBody;

        //        // Subject
        //        message.Subject = strSubject;

        //        // From
        //        if (strFrom == null || strFrom == string.Empty)
        //            message.From = new MailAddress("mnguyen@paymentxp.com", "Client Services");
        //        else
        //        {
        //            message.From = new MailAddress(strFrom);
        //        }

        //        // Carbon copy
        //        //if (cc != null)
        //        //    foreach (string carboncopy in cc)
        //        //        message.CC.Add(carboncopy);

        //        // Blind copy
        //        //if (bcc != null)
        //        //    foreach (string blindcopy in bcc)
        //        //        message.Bcc.Add(blindcopy);

        //        // Attachments
        //        //if (attachments != null)
        //        //    foreach (object attachment in attachments)
        //        //        message.Attachments.Add(new Attachment(attachment.ToString()));


        //        //SmtpClient client = new SmtpClient("192.168.1.6");
        //        SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
        //        client.UseDefaultCredentials = true;
        //        client.Send(message);

        //        client = null;
        //        message = null;
        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.LoggerInstance.WriteLine("Error: " + exc.Message + ", StackTrace: " + exc.StackTrace);
        //        return false;
        //    }
        //}

        //public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo)
        //{
        //    try
        //    {
        //        MailMessage oMsg = new MailMessage();

        //        oMsg.From = strFrom;
        //        oMsg.To = strTo;
        //        oMsg.Subject = strSubject;
        //        oMsg.BodyFormat = MailFormat.Html;
        //        oMsg.Body = strBody;
        //        SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
        //        SmtpMail.Send(oMsg);

        //        oMsg = null;

        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.LoggerInstance.WriteLine("Error: " + exc.Message + ", StackTrace: " + exc.StackTrace);
        //        return false;
        //    }
        //}

        //public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo, string strBcc)
        //{
        //    try
        //    {
        //        MailMessage oMsg = new MailMessage();

        //        oMsg.From = strFrom;
        //        oMsg.To = strTo;
        //        oMsg.Bcc = strBcc;
        //        oMsg.Subject = strSubject;
        //        oMsg.BodyFormat = MailFormat.Html;
        //        oMsg.Body = strBody;
        //        SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
        //        SmtpMail.Send(oMsg);

        //        oMsg = null;

        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.LoggerInstance.WriteLine("Error: " + exc.Message + ", StackTrace: " + exc.StackTrace);
        //        return false;
        //    }
        //}

        //public static bool SendEmail(string strSubject, string strBody, string strFrom, string strTo,ArrayList attachments)
        //{
        //    try
        //    {
        //        MailMessage oMsg = new MailMessage();

        //        oMsg.From = strFrom;
        //        oMsg.To = strTo;
        //        oMsg.Subject = strSubject;
        //        oMsg.BodyFormat = MailFormat.Html;
        //        oMsg.Body = strBody;

        //        foreach(object obj in attachments)
        //        {
        //            oMsg.Attachments.Add(new MailAttachment(obj.ToString()));
        //        }

                
        //        SmtpMail.SmtpServer = ConfigurationManager.AppSettings["SMTPServer"].ToString();
        //        SmtpMail.Send(oMsg);

        //        oMsg = null;

        //        return true;
        //    }
        //    catch (Exception exc)
        //    {
        //        Logger.LoggerInstance.WriteLine("Error: " + exc.Message + ", StackTrace: " + exc.StackTrace);
        //        return false;
        //    }
        //}
    }
}
