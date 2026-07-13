using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;

using Nmc.Common;

namespace FileMonitoring
{
	class Program
	{
        static string m_ArchivePath = ConfigurationManager.AppSettings["ArchivePath"];
        static string m_FtpPath = ConfigurationManager.AppSettings["FtpPath"];

        //static ArrayList m_Merchants;

        string m_ToError = ConfigurationManager.AppSettings["EmailToError"];
        string m_To = ConfigurationManager.AppSettings["EmailTo"];
        string m_From = ConfigurationManager.AppSettings["EmailFrom"];
        static string m_WatchFileExtension = ConfigurationManager.AppSettings["WatchFileExtensions"];
        static string[] m_FileExtensions;

        //static string[] m_FileExtensions;

        const int HowDeepToScan = 1;

		static void Main(string[] args)
		{
            Program oPrgm = new Program();

            m_FileExtensions = m_WatchFileExtension.Split(new char[] { ',' });

            oPrgm.Monitor(m_FtpPath);
            oPrgm.Monitor(m_ArchivePath);
		}


        public Program()
        {
            //m_Merchants = new ArrayList();
        }



        private void Monitor(string WatchPath)
        {
            string[] Dirs;
            string[] Files;
            DirectoryInfo oDir;
            FileInfo oFile;
            string FileExtension = string.Empty;
            string Dest = string.Empty;
            string strErrorMessage = string.Empty;
            string strMessage = string.Empty;

            try
            {
                Dirs = Directory.GetDirectories(WatchPath);
                foreach (string strDir in Dirs)
                {
                    strMessage = string.Empty;
                    oDir = new DirectoryInfo(strDir);
                    Files = Directory.GetFiles(strDir);

                    foreach (string strFile in Files)
                    {
                        oFile = new FileInfo(strFile);
                        FileExtension = strFile.Substring(strFile.Length - 7, 7).ToUpper();

                        foreach (string ext in m_FileExtensions)
                        {
                            if (FileExtension == ext)
                                strMessage += this.GetEmailMessage(oFile, oDir);
                        }

                        FileExtension = strFile.Substring(strFile.Length - 3, 3).ToUpper();

                        foreach (string ext in m_FileExtensions)
                        {
                            if (FileExtension == ext)
                                strMessage += this.GetEmailMessage(oFile, oDir);
                        }

                        oFile = null;
                    }

                    if (strMessage != string.Empty)
                        Email.SendEmail("File Monitoring Program", strMessage, m_From, m_To);

                    oDir = null;
                }
            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg = "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("File Monitoring Program Error", msg, m_From, m_To);
            }


        }

        private string GetEmailMessage(FileInfo oFile, DirectoryInfo oDir)
        {
            TimeSpan ts;

            ts = DateTime.Now - oFile.LastWriteTime;
            if (ts.TotalHours > 1)
            {
                return "File " + oFile.FullName + " has not processed in over " + Math.Round(ts.TotalHours,2) + " hour(s).\n";
            }
            else
                return string.Empty;
        }


	}

}
