using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

using Nmc.Ach.Dal;
using Nmc.Common;

namespace PRIReturnNotification
{
    public class main
    {
        static string m_ArchivePath = ConfigurationManager.AppSettings["ArchivePath"];
        static string m_FtpPath = ConfigurationManager.AppSettings["FtpPath"];

        //static ArrayList m_Merchants;

        public static string m_ToError = ConfigurationManager.AppSettings["EmailToError"];
        public static string m_To = ConfigurationManager.AppSettings["EmailTo"];
        public static string m_From = ConfigurationManager.AppSettings["EmailFrom"];


        static void Main(string[] args)
        {

            try
            {

                Console.WriteLine("Starting...");

                FTPFactory ff = new FTPFactory();
                ff.setDebug(true);
                ff.setRemoteHost("ftp1.paymentresource.com");
                ff.setRemoteUser("10161");
                ff.setRemotePass("IO5624");
                ff.login();
                //ff.chdir("incoming");

                string[] fileNames = ff.getFileList("*.*");

                SqlDataReader dr = null;
                DataBatchFileLog data = new DataBatchFileLog();
                ArrayList prms = new ArrayList();
                string msg = string.Empty;

                foreach (string filename in fileNames )
                {
                    if (filename.Trim() == string.Empty)
                        break;

                    string file = filename.Replace("\r", "");
                    string extension = file.Substring(file.Length - 3, 3).ToUpper();

                    switch (extension)
                    {
                        case "RTN":
                            prms.Clear();
                            prms.Add(new SqlParameter("@AchID", -1));
                            prms.Add(new SqlParameter("@FileName", file));
                            dr = data.Select(prms);

                            if (!dr.Read())
                                msg += "File " + file + " not loaded. \n\r";

                            dr.Close();

                            break;
                        //case "CSV":
                        //    ff.download(file, m_ArchivePath + @"241606001\" + file);
                        //    break;
                    }
                }

                if (msg != string.Empty)
                    Email.SendEmail("RE: Return File Not Loaded", msg, m_From, m_To);


                //ff.setBinaryMode(true);
                //ff.upload(@"c:\test.txt");
                ff.close();

            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg += "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("RE: PRI Return File Watcher - Error", msg, m_From, m_To);
                Logger.LoggerInstance.WriteLine(msg);
            }
        }
    }
}
