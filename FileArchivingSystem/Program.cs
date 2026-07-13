using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

using CommonUtility;

namespace FileArchivingSystem
{
	class Program
	{
        int m_DaysToArchiveMerchantFolders = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToArchiveMerchantFolders"]);
        int m_DaysToArchiveOtherFolders = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToArchiveOtherFolders"]);

        string m_ArchivePath = ConfigurationManager.AppSettings["ArchivePath"];
        string m_FtpPath = ConfigurationManager.AppSettings ["FtpPath"];

        static string m_SettlementFileArchivePath = ConfigurationManager.AppSettings["SettlementFileArchivePath"];
        static string m_SettlementFileFTPPath = ConfigurationManager.AppSettings["SettlementFileFTPPath"];

        static string m_ReturnFileArchivePath = ConfigurationManager.AppSettings["ReturnFileArchivePath"];
        static string m_ReturnFileFTPPath = ConfigurationManager.AppSettings["ReturnFileFTPPath"];

        static string m_InfragisticsChartImages = ConfigurationManager.AppSettings["InfragisticsChartImages"];

		string m_To = ConfigurationManager.AppSettings ["EmailTo"];
		string m_From = ConfigurationManager.AppSettings ["EmailFrom"];

		static void Main(string[] args)
		{
            //string errormsg=string.Empty;

            //PGPHandler.Encrypt(@"C:\Book2.xls", @"C:\Book2.xls.pgp", ref errormsg);

            Program oPrgm = new Program();

            Logger.LogInfo("Begin Program");

            oPrgm.DeleteOldInfragisticsChartImages();

            oPrgm.Move_Encrypt_Response_Return_Files();
            oPrgm.Move_Encrypt_Complete_All_Files();
            oPrgm.Encrypt_Response_Return_Files_Archive_Server();
            oPrgm.Encrypt_Complete_All_Files_Archive_Server();

            oPrgm.Move_Bank_Files(m_SettlementFileFTPPath, m_SettlementFileArchivePath);
            oPrgm.Move_Bank_Files(m_ReturnFileFTPPath, m_ReturnFileArchivePath);

            oPrgm.Encrypt_Complete_Bank_Files(m_SettlementFileArchivePath);
            oPrgm.Encrypt_Complete_Bank_Files(m_ReturnFileArchivePath);

            oPrgm.Move_Old_Files();

            Logger.LogInfo("End Program");

        }

        private void DeleteOldInfragisticsChartImages()
        {
            string strDest = string.Empty;
            string strError = string.Empty;

            DateTime dtDeleteDate = DateTime.Now.AddDays(-2);

            try
            {

                string[] dirs = m_InfragisticsChartImages.Split(new char[] {';'});

                foreach (string dir in dirs)
                {
                    string[] files = Directory.GetFiles(dir);

                    foreach (string file in files)
                    {
                        FileInfo fi = new FileInfo(file);

                        if (fi.LastWriteTime <= dtDeleteDate)
                            fi.Delete();

                    }
                }


            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to DeleteOldInfragisticsChartImages";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Move_Encrypt_Response_Return_Files()
        { 
            string[] strDirs;
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;
            string strFileExtension = string.Empty;

            DirectoryInfo oDir;
            FileInfo oFile;
            DateTime dtMoveDate;

            try
            {
                strDirs = Directory.GetDirectories(m_FtpPath);
                
                foreach (string strDir in strDirs)
                {

                    oDir = new DirectoryInfo(strDir);
                    if (DataLayer.IsNumeric(oDir.Name.Substring(0, 1)))
                        dtMoveDate = DateTime.Now.AddDays(m_DaysToArchiveMerchantFolders);
                    else
                        dtMoveDate = DateTime.Now.AddDays(m_DaysToArchiveOtherFolders);

                    strFiles = Directory.GetFiles(strDir);
                    foreach (string strFile in strFiles)
                    {
                        oFile = new FileInfo(strFile);

                        //Move already encrypted file
                        strFileExtension = strFile.Substring(strFile.Length - 7, 7).ToUpper();
                        switch (strFileExtension)
                        {
                            case "RSP.PGP":
                            case "RTN.PGP":
                            case "XLS.PGP":
                                if (oFile.LastWriteTime <= dtMoveDate)
                                {
                                    if (File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name))
                                        strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name.Substring(0, oFile.Name.Length - 4) + " " + DateTime.Now.ToString("yyMMddhhmmss") + "." + strFileExtension;
                                    else
                                        strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name;

                                    oFile.MoveTo(strDest);
                                }
                                break;
                        }

                        strFileExtension = strFile.Substring(strFile.Length - 3, 3).ToUpper();
                        switch (strFileExtension)
                        {
                            case "RSP":
                            case "RTN":
                                if (File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name) || File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name + ".PGP"))
                                    strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name.Substring(0, oFile.Name.Length - 4) + " " + DateTime.Now.ToString("yyMMddhhmmss") + "." + strFileExtension; 
                                else
                                    strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name; 
                                
                                if (oFile.LastWriteTime <= dtMoveDate)
                                {
                                    oFile.MoveTo(strDest);
                                    strError = string.Empty;
                                    if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                    {
                                        File.Delete(strDest);
                                        Console.WriteLine("Successfully moved and encrypted file " + strDest);
                                    }
                                    else
                                        Console.WriteLine("Failed to move and encrypt file " + strDest);
                                }
                                break;
                        }

                    }
                    //}
                }
            }
            catch(Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Destination Directory: " + strDest + "\n";
                strMessage += "Failed to Move_Encrypt_Response_Return_Files";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Move_Encrypt_Complete_All_Files()
        {
            string[] strDirs;
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;

            DirectoryInfo oDir;
            FileInfo oFile;
            DateTime dtMoveDate;

            try
            {
                strDirs = Directory.GetDirectories(m_FtpPath);

                foreach (string strDir in strDirs)
                {
                    oDir = new DirectoryInfo(strDir);

                    if (DataLayer.IsNumeric(oDir.Name.Substring(0, 1)))
                        dtMoveDate = DateTime.Now.AddDays(m_DaysToArchiveMerchantFolders);
                    else
                        dtMoveDate = DateTime.Now.AddDays(m_DaysToArchiveOtherFolders);


                    if (Directory.Exists(strDir + @"\Complete\"))
                    {
                        strFiles = Directory.GetFiles(strDir + @"\Complete\");
                        foreach (string strFile in strFiles)
                        {
                            oFile = new FileInfo(strFile);

                            //Move already encrypted file
                            switch (strFile.Substring(strFile.Length - 7, 7).ToUpper())
                            {
                                case "RSP.PGP":
                                case "RTN.PGP":
                                    if (File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name))
                                        oFile.Delete();
                                    else
                                        oFile.MoveTo(m_ArchivePath + oDir.Name + @"\" + oFile.Name);

                                    break;
                            }

                            switch (strFile.Substring(strFile.Length - 3, 3).ToUpper())
                            {
                                case "PGP":
                                case "EXE":
                                case "BAT":
                                    break;
                                case "RSP":
                                case "RTN":
                                    if (File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name))
                                        oFile.Delete(); //If file exists on ArchivePath, delete it from the FTPPath
                                    else
                                    {
                                        if (oFile.LastWriteTime <= dtMoveDate)
                                        {
                                            strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name;
                                            oFile.MoveTo(strDest);
                                            strError = string.Empty;
                                            if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                            {
                                                File.Delete(strDest);
                                                Console.WriteLine("Successfully moved and encrypted file " + oFile);
                                            }
                                            else
                                                Console.WriteLine("Failed to move and encrypt file " + oFile);
                                        }
                                    }
                                    break;
                                default:
                                    strDest = m_ArchivePath + oDir.Name + @"\Complete\" + oFile.Name;
                                    if (File.Exists(strDest))
                                        oFile.Delete(); //If file exists on ArchivePath, delete it from the FTPPath
                                    else
                                    {
                                        if (oFile.LastWriteTime <= dtMoveDate)
                                        {
                                            oFile.MoveTo(strDest);
                                            strError = string.Empty;
                                            if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                            {
                                                File.Delete(strDest);
                                                Console.WriteLine("Successfully moved and encrypted file " + oFile);
                                            }
                                            else
                                                Console.WriteLine("Failed to move and encrypt file " + oFile);
                                        }
                                    }
                                    break;

                            }
                        }
                    }


                }
            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to Move_Encrypt_Complete_All_Files";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Encrypt_Response_Return_Files_Archive_Server()
        {
            string[] strDirs;
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;

            DirectoryInfo oDir;
            FileInfo oFile;
            DateTime dtMoveDate = DateTime.Now.AddDays(-1);

            try
            {
                strDirs = Directory.GetDirectories(m_ArchivePath);

                foreach (string strDir in strDirs)
                {
                    oDir = new DirectoryInfo(strDir);
                    if (DataLayer.IsNumeric(oDir.Name.Substring(0, 1)))
                    {
                        strFiles = Directory.GetFiles(strDir);
                        foreach (string strFile in strFiles)
                        {
                            oFile = new FileInfo(strFile);

                            switch (strFile.Substring(strFile.Length - 3, 3).ToUpper())
                            {
                                case "ZIP":
                                case "RSP":
                                case "RTN":
                                    if (File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name + ".PGP"))
                                        oFile.Delete(); //If file exists on ArchivePath, delete it from the FTPPath
                                    else
                                    {
                                        strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name;
                                        strError = string.Empty;
                                        if (oFile.LastWriteTime <= dtMoveDate)
                                        {
                                            if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                            {
                                                oFile.Delete();
                                                Console.WriteLine("Successfully moved and encrypted file " + oFile);
                                            }
                                            else
                                                Console.WriteLine("Failed to move and encrypt file " + oFile);
                                        }
                                    }
                                    break;
                            }

                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to Encrypt_Response_Return_Files_Brown";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Encrypt_Complete_All_Files_Archive_Server()
        {
            string[] strDirs;
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;

            DirectoryInfo oDir;
            FileInfo oFile;
            DateTime dtMoveDate = DateTime.Now.AddDays(-1);

            try
            {
                strDirs = Directory.GetDirectories(m_ArchivePath);

                foreach (string strDir in strDirs)
                {

                    oDir = new DirectoryInfo(strDir);

                    if (DataLayer.IsNumeric(oDir.Name.Substring(0, 1)))
                    {
                        if (Directory.Exists(strDir + @"\Complete\"))
                        {
                            strFiles = Directory.GetFiles(strDir + @"\Complete\");

                            foreach (string strFile in strFiles)
                            {
                                oFile = new FileInfo(strFile);

                                switch (strFile.Substring(strFile.Length - 3, 3).ToUpper())
                                {
                                    case "PGP":
                                    case "EXE":
                                    case "BAT":
                                        break;
                                    default:
                                        strDest = m_ArchivePath + oDir.Name + @"\Complete\" + oFile.Name;

                                        if (!File.Exists(strDest + ".PGP"))
                                        {
                                            if (oFile.LastWriteTime <= dtMoveDate)
                                            {
                                                strError = string.Empty;

                                                if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                                {
                                                    File.Delete(strDest);
                                                    Console.WriteLine("Successfully encrypted file " + oFile);
                                                }
                                                else
                                                    Console.WriteLine("Failed to encrypt file " + oFile);
                                            }
                                        }
                                        else
                                        {
                                            File.Delete(strDest);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to Encrypt_Complete_All_Files_Archive_Server";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Move_Bank_Files(string strFTPDir,string strCCKDir)
        {
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;
            DateTime dtMoveDate = DateTime.Now.AddHours(-12);
            string strFileExtension = string.Empty;

            FileInfo oFile;

            try
            {

                    strFiles = Directory.GetFiles(strFTPDir);

                    foreach (string strFile in strFiles)
                    {
                        oFile = new FileInfo(strFile);

                        //Move already encrypted file
                        strFileExtension = strFile.Substring(strFile.Length - 7, 7).ToUpper();
                        switch (strFileExtension)
                        {
                            case "FED.PGP":
                            case "ACH.PGP":
                            case "INC.PGP":
                            case "CCK.PGP":
                            case "001.PGP":
                            case ".IB.PGP":
                            case "CSV.PGP":
                                if (File.Exists(strCCKDir + oFile.Name))
                                    oFile.Delete(); //If file exists on ArchivePath, delete it from the FTPPath
                                else
                                    oFile.MoveTo(strCCKDir + oFile.Name);

                                break;
                            case "RTN.PGP":
                                break;
                        }

                        strFileExtension = strFile.Substring(strFile.Length - 3, 3).ToUpper();
                        switch (strFileExtension)
                        {
                            case "ZIP":
                            case "ACH":
                            case "INC":
                            case "CCK":
                            case "FED":
                            case "001":
                            case ".IB":
                            case "CSV":
                                if (File.Exists(strCCKDir + oFile.Name) || File.Exists(strCCKDir + oFile.Name + ".PGP"))
                                    strDest = strCCKDir + oFile.Name.Substring(0, oFile.Name.Length - 4) + " " + DateTime.Now.ToString("yyMMddhhmmss") + "." + strFileExtension; 
                                else
                                    strDest = strCCKDir + oFile.Name;

                                //strDest = strCCKDir + oFile.Name;
                                if (oFile.LastWriteTime <= dtMoveDate)
                                {
                                    oFile.MoveTo(strDest);
                                    strError = string.Empty;
                                    if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                    {
                                        File.Delete(strDest);
                                        Console.WriteLine("Successfully moved and encrypted file " + oFile);
                                    }
                                    else
                                        Console.WriteLine("Failed to move and encrypt file " + oFile);
                                }

                                break;
                            case "RTN":
                                break;

                        }

                    
                }
            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to Move_Bank_Files";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Encrypt_Complete_Bank_Files(string strCCKDir)
        {
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;
            DateTime dtMoveDate = DateTime.Now.AddHours(-12);
            string strFileExtension = string.Empty;

            FileInfo oFile;

            try
            {

                strFiles = Directory.GetFiles(strCCKDir);

                foreach (string strFile in strFiles)
                {
                    oFile = new FileInfo(strFile);

                    strFileExtension = strFile.Substring(strFile.Length - 3, 3).ToUpper();
                    switch (strFileExtension)
                    {
                        case "FED":
                        case "001":
                        case ".IB":
                        case "RTN":
                        case "CSV":
                            if (File.Exists(strCCKDir + oFile.Name + ".PGP"))
                            {
                                strDest = strCCKDir + oFile.Name.Substring(0, oFile.Name.Length - 4) + " " + DateTime.Now.ToString("yyMMddhhmmss") + "." + strFileExtension;
                                File.Move(oFile.FullName, strDest);
                            }
                            else
                                strDest = strCCKDir + oFile.Name;

                            if (oFile.LastWriteTime <= dtMoveDate)
                            {
                                strError = string.Empty;
                                if (PGPHandler.Encrypt(strDest, strDest + ".PGP", ref strError))
                                {
                                    File.Delete(strDest);
                                    Console.WriteLine("Successfully encrypted file " + oFile );
                                }
                                else
                                    Console.WriteLine("Failed to encrypt file " + oFile);
                            }
                            
                            break;
                    }
                }

            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to Encrypt_Complete_Bank_Files";
                Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }

        private void Move_Old_Files()
        {
            string[] strDirs;
            string[] strFiles;
            string strDest = string.Empty;
            string strError = string.Empty;
            string strFileExtension = string.Empty;

            DirectoryInfo oDir;
            FileInfo oFile;
            DateTime dtMoveDate;

            try
            {
                strDirs = Directory.GetDirectories(m_FtpPath);

                foreach (string strDir in strDirs)
                {

                    oDir = new DirectoryInfo(strDir);
                    
                    strFiles = Directory.GetFiles(strDir);
                    foreach (string strFile in strFiles)
                    {
                        oFile = new FileInfo(strFile);
                        
                        //Move  file 
                        dtMoveDate=DateTime.Today.AddMonths(-1);

                        if (oFile.LastWriteTime <= dtMoveDate)
                        {
                            if (!Directory.Exists(m_ArchivePath + oDir.Name + @"\"))
                            {
                                Directory.CreateDirectory(m_ArchivePath + oDir.Name + @"\");
                                Directory.CreateDirectory(m_ArchivePath + oDir.Name + @"\Complete\");
                            }

                            if (File.Exists(m_ArchivePath + oDir.Name + @"\" + oFile.Name))
                                strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name.Replace(oFile.Extension, "") + " " + DateTime.Now.ToString("yyMMddhhmmss") + "." + oFile.Extension;
                            else
                                strDest = m_ArchivePath + oDir.Name + @"\" + oFile.Name;


                            oFile.MoveTo(strDest);
                        }
                        

                    }
                    //}
                }
            }
            catch (Exception exc)
            {
                string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
                strMessage += "Failed to Move_Old_Files";
                Email.SendEmail("File Archiving System Failed - Move_Old_Files", strMessage, m_From, m_To);
                Logger.Log(exc);
            }

        }
 

        //private DateTime GetMoveDate()
        //{
        //    SqlCommand cmd;
        //    SqlDataReader dr;
        //    DateTime dtMoveDate = DateTime.Today.AddDays(-1);
        //    //DateTime dtMoveDate = DateTime.Today.AddDays(-2);

        //    try
        //    {

        //        int j = 0;

        //        while (j == 0)
        //        {
        //            if (Convert.ToInt32(dtMoveDate.DayOfWeek ) == 0 || Convert.ToInt32(dtMoveDate.DayOfWeek) == 6)
        //            {
        //                dtMoveDate = dtMoveDate.AddDays(-1);
        //                j = 0;
        //            }
        //            else
        //            {
        //                cmd = new SqlCommand();
        //                cmd.CommandText = "GetHoliday";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@Date",dtMoveDate.ToString("MM/dd/yy")));
        //                dr = DataLayer.GetDataReader(cmd, m_ConnectionString);
        //                //dr = DataLayer.GetDataReader("Select Holiday From Holidays Where Holiday = '" + dtMoveDate.ToString("MM/dd/yy") + "'", m_ConnectionString);
        //                if (dr.Read())
        //                {
        //                    dtMoveDate = dtMoveDate.AddDays(-1);
        //                    j = 0;
        //                }
        //                else
        //                    j = 1;
                        
        //                dr.Close();
        //                cmd = null;
        //            }
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string strMessage = "ErrorMessage: " + exc.Message + " : " + exc.StackTrace + "\n";
        //        strMessage += "Failed to GetMoveDate";
        //        Email.SendEmail("File Archiving System Failed", strMessage, m_From, m_To);
        //        Logger.Log(strMessage);
        //    }
        //    finally 
        //    {
        //        dr = null;
        //    }

        //    return dtMoveDate;
        //}

	}

}
