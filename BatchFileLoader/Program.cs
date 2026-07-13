using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Nmc.Ach.Dal;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using CommonUtility;

namespace BatchFileLoader
{
    public class Program
    {
        internal static string m_ToError = ConfigurationManager.AppSettings["EmailToError"];
        internal static string m_To = ConfigurationManager.AppSettings["EmailTo"];
        internal static string m_From = ConfigurationManager.AppSettings["EmailFrom"];


        //we're going to make ach batch smarter by reading from both datacenter sites and 
        //writing to each archived shared folders on both datacenter sites
        private static string _SavArchivePath = ConfigurationManager.AppSettings["SavvisArchivePath"];
        private static string _LatArchivePath = ConfigurationManager.AppSettings["LatisysArchivePath"];

        private static string _SavFtpPath = ConfigurationManager.AppSettings["SavvisFtpPath"];
        private static string _LatFtpPath = ConfigurationManager.AppSettings["LatisysFtpPath"];

        private static string _LocalTempDir = ConfigurationManager.AppSettings["LocalTempPath"];
        private static string _LocalErrorDir = ConfigurationManager.AppSettings["LocalErrorPath"];

        private static List<iBatchFile> _BatchFiles = new List<iBatchFile>();

        internal static string ERROR_SUBJECT = Environment.MachineName + " - ACH Batch Uploader";

        static void Main(string[] args)
        {
            BatchLog.Root.InfoFormat("ACH Batch processing started!");

            BatchLog.Root.InfoFormat("**************************************Configuration**************************************");
            BatchLog.Root.InfoFormat("Email Error: {0}", m_ToError);
            BatchLog.Root.InfoFormat("Email To: {0}", m_To);
            BatchLog.Root.InfoFormat("Email From: {0}", m_From);
            BatchLog.Root.InfoFormat("Latisys FTP Folder: {0}", _LatFtpPath);
            BatchLog.Root.InfoFormat("Latisys Archive Folder: {0}", _LatArchivePath);
            BatchLog.Root.InfoFormat("Savvis FTP Folder: {0}", _SavFtpPath);
            BatchLog.Root.InfoFormat("Savvis Archive Folder: {0}", _SavArchivePath);
            BatchLog.Root.InfoFormat("Local Temp Directory: {0}", _LocalTempDir);
            BatchLog.Root.InfoFormat("Local Error Directory: {0}", _LocalErrorDir);
            BatchLog.Root.InfoFormat("*****************************************************************************************");

            //load files from latisys first
            if (!string.IsNullOrWhiteSpace(_LatFtpPath))
            {
                LoadBatchFiles(_LatFtpPath); 
            }

            //now load files from savvis
            if (!string.IsNullOrEmpty(_SavFtpPath))
            {
                LoadBatchFiles(_SavFtpPath);
            }

            //only process batch files if we found any new ones
            if (Program._BatchFiles.Count > 0)
            {
                ProcessBatchFiles(Program._BatchFiles);
            }
            else
            {
                BatchLog.Root.InfoFormat("No batch files to process.");
            }

            BatchLog.Root.InfoFormat("ACH Batch process complete.");
        }

        private static void MoveDecryptFiles(string archivePath, string ftpPath)
        {
            string[] strDirs;
            string[] strFiles;
            FileInfo oFile;
            string strFileExtension = string.Empty;
            string Dest = string.Empty;
            string strMessage = string.Empty;
            string strErrorMessage = string.Empty;
            bool perform = false;

            try
            {
                strDirs = Directory.GetDirectories(ftpPath);

                foreach (string strDir in strDirs)
                {
                    strFiles = Directory.GetFiles(strDir);

                    foreach (string strFile in strFiles)
                    {
                        oFile = new FileInfo(strFile);

                        //the target directory we're moving batch files to for processing
                        string targetDir = archivePath + oFile.Directory.Name;

                        //create the target directory if it does not exist for the merchant
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                            Directory.CreateDirectory(targetDir + @"\Complete");
                        }

                        strFileExtension = strFile.Substring(strFile.Length - 3, 3).ToUpper();
                        switch (strFileExtension)
                        {
                            case "ACH":
                            case "NAC":
                            case "NMC":
                                Dest = targetDir + @"\" + oFile.Name;
                                oFile.MoveTo(Dest);
                                break;
                        }

                        strFileExtension = strFile.Substring(strFile.Length - 7, 7).ToUpper();
                        switch (strFileExtension)
                        {
                            case "ACH.PGP":
                            case "NAC.PGP":
                            case "NMC.PGP":
                                Dest = targetDir + @"\" + oFile.Name;
                                oFile.MoveTo(Dest);

                                perform = PGPHandler.Decrypt(Dest, Dest.Substring(0, Dest.Length - 4), ref strErrorMessage);

                                oFile = new FileInfo(Dest);
                                Dest = archivePath + oFile.Directory.Name + @"\Complete\" + oFile.Name;
                                oFile.MoveTo(Dest);

                                if (perform)
                                {
                                    strMessage = "Decrypted file " + strFile + " successfully for merchant " + oFile.Directory.Name;
                                    Email.SendEmail("RE: PGP Decrypt Succeeded - " + oFile.Directory.Name, strMessage, m_From, m_To);
                                    CommonUtility.Logger.LogError(strMessage);
                                    Console.WriteLine(strMessage);
                                }
                                else
                                {
                                    strMessage = "Failed to decrypt file " + strFile + " for merchant " + oFile.Directory.Name + "," + strErrorMessage;
                                    Email.SendEmail("RE: PGP Decrypt Failed - " + oFile.Directory.Name, strMessage, m_From, m_ToError);

                                    CommonUtility.Logger.LogError(strMessage);
                                    Console.WriteLine(strMessage);
                                }

                                break;
                        }

                        oFile = null;

                    }
                }
            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg = "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("RE: Load Batch Failed", msg, Program.m_From, Program.m_To);
                CommonUtility.Logger.LogError(msg);
            }

        }

        private static void ProcessBatchFiles(string archivePath)
        {
            string[] strDirs;
            //string[] strFiles;
            FileInfo oFile;
            string strFileExtension = string.Empty;
            string Dest = string.Empty;
            string strMessage = string.Empty;

            bool perform = false;
            BatchFileFactory factory = new BatchFileFactory();
            BatchFile batchfile = null;
            

            try
            {
                strDirs = Directory.GetDirectories(archivePath);

                foreach (string strDir in strDirs)
                {
                    //strFiles = Directory.GetFiles(strDir, "*.nac");
                    IEnumerable<string> strFiles = Directory.EnumerateFiles(strDir, "*.*").Where(s => s.ToLower().EndsWith(".nac") || s.ToLower().EndsWith(".ach"));

                    foreach (string strFile in strFiles)
                    {
                        oFile = new FileInfo(strFile);

                        batchfile = factory.GetBatchFile(strFile);

                        int merchantId;

                        if (batchfile != null && int.TryParse(oFile.Directory.Name, out merchantId))
                        {
                            try
                            {
                                strMessage = string.Empty;
                                batchfile.MerchantID = Convert.ToInt32(oFile.Directory.Name);

                                strMessage = batchfile.ParseFile();

                                MerchantApp app = DataAccess.DataMerchantAppDao.GetMerchantApp(merchantId);
                                batchfile.MerchantAppUID = app.MerchantAppUID;
                                batchfile.Merchant = app;
                                

                                perform = strMessage == string.Empty ? true : false;

                                if (perform)
                                {
                                    perform = batchfile.PassDuplicateFile(oFile);

                                    if (!perform)
                                        strMessage = "File has already been loaded.";
                                }

                                if (perform)
                                {
                                    batchfile.UploadID = batchfile.LogBatchFile(oFile);

                                    if (batchfile.UploadID == -1)
                                        perform = false;
                                    else
                                        perform = true;
                                }

                                if (perform)
                                    perform = batchfile.ImportFile();

                                if (perform)
                                {
                                    DataSet ds = DataAccess.DataAchTransactionDao.GetBatchfileTotals(app.ID, Convert.ToInt32(batchfile.UploadID));

                                    decimal debit = 0;
                                    int debitcount = 0;
                                    decimal credit = 0;
                                    int creditcount = 0;

                                    if (ds.Tables[0].Rows.Count == 1)
                                    {
                                        debit = (decimal)ds.Tables[0].Rows[0]["debit"];
                                        debitcount = Convert.ToInt32(ds.Tables[0].Rows[0]["debitcount"]);
                                        credit = (decimal)ds.Tables[0].Rows[0]["credit"];
                                        creditcount = Convert.ToInt32(ds.Tables[0].Rows[0]["creditcount"]);
                                    }

                                    string subject = string.Format("{0} Batch Upload - SUCCESS", app.BusinessDBAName);
                                    string body = string.Format("MerchantID: {2}<br>Merchant: {0}<br>Type: ACH Batch<br>Status: Successful File Received<br>Filename: {1}<br>Debit: {3}<br>Debit Count: {4}<br>Credit: {5}<br>Credit Count: {6}<br>"
                                        , app.BusinessDBAName
                                        , oFile.Name
                                        , app.ID
                                        , debit.ToString("N")
                                        , debitcount.ToString()
                                        , credit.ToString("N")
                                        , creditcount.ToString()
                                        );

                                    MerchantFacade.SendEmail(subject,
                                            string.Empty,
                                            body,
                                            Constants.ADMIN_PAYMENTXP,
                                             app.NotificationEmails,
                                             string.Empty,
                                             Constants.DEVELOPERS_EMAIL,
                                             null,
                                             app.MerchantAppUID
                                             , "System");

                                    //Email.SendEmail("RE: BatchFileLoader - " + oFile.Directory.Name, "File loaded successfully - " + oFile.FullName, m_From, m_To);

                                }
                                else
                                {
                                    string subject = string.Format("{0} Batch Upload - FAILED", app.BusinessDBAName);
                                    string body = string.Format("MerchantID: {3}<br>Merchant: {0}<br>Type: ACH Batch<br>Status: {2}<br>Filename: {1}"
                                        , app.BusinessDBAName
                                        , oFile.Name
                                        , strMessage
                                        , app.ID);

                                    MerchantFacade.SendEmail(subject,
                                            string.Empty,
                                            body,
                                            Constants.ADMIN_PAYMENTXP,
                                             app.NotificationEmails,
                                             string.Empty,
                                             Constants.DEVELOPERS_EMAIL,
                                             null,
                                             app.MerchantAppUID
                                             , "System");
                                }
                            }
                            catch (BatchFileException)
                            {
                                //suppress batch file exception for now
                            }
                            finally
                            {

                                Dest = archivePath + oFile.Directory.Name + @"\Complete\" + oFile.Name;

                                if (File.Exists(Dest))
                                    Dest = archivePath + oFile.Directory.Name + @"\Complete\" + oFile.Name.Substring(0, oFile.Name.Length - 4) + "_" + DateTime.Now.ToString("yyMMddhhmmss") + "." + strFileExtension;

                                oFile.MoveTo(Dest);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string msg = string.Empty;
                msg = "Error Message: " + exc.Message + "\n";
                msg += "Error Trace: " + exc.StackTrace;

                Email.SendEmail("RE: Load Batch Failed", msg, Program.m_From, Program.m_To);
                CommonUtility.Logger.LogError(msg);
            }

        }

        private static void LoadBatchFiles(string ftpPath)
        {
            BatchLog.Root.InfoFormat("Loading batch files from '{0}'......", ftpPath);

            try
            {
                //regex for merchant IDs
                string[] strDirs = Directory.GetDirectories(ftpPath);

                BatchLog.Root.InfoFormat("{0} directories found.", strDirs.Length);

                foreach (string strDir in strDirs)
                {
                    BatchLog.Root.InfoFormat("Validating merchant directory '{0}'......", strDir);
                    DirectoryInfo dir = new DirectoryInfo(strDir);

                    int zid;

                    if (!int.TryParse(dir.Name, out zid))
                    {
                        BatchLog.Root.InfoFormat("Ignoring directory '{0}': Invalid merchant directory", strDir);
                        continue;
                    }

                    BatchLog.Root.InfoFormat("Merchant directory valid, retrieving files......");

                    //regex for supporting file extensions
                    string[] strFiles = Directory.GetFiles(strDir);

                    BatchLog.Root.InfoFormat("{0} files retrieved.", strFiles.Length);

                    foreach (string strFile in strFiles)
                    {
                        iBatchFile file = BatchFileFactory.GetBatchFile(strFile, zid);

                        if(file != null)
                        {
                            Program._BatchFiles.Add(file);
                            BatchLog.Root.InfoFormat("Batch file '{0}' added for processing", strFile);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("IOException Error Message: {0}", ex.Message).AppendLine();
                sb.AppendFormat("Error Trace: {0}", ex.StackTrace).AppendLine();

                BatchLog.Root.ErrorFormat("An IOException occurred while loading batch files: {0}", ex.Message);
                BatchLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                Email.SendEmail(ERROR_SUBJECT, sb.ToString(), Program.m_From, Program.m_To);
            }
            catch (Exception exc)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Error Message: {0}", exc.Message).AppendLine();
                sb.AppendFormat("Error Trace: {0}", exc.StackTrace).AppendLine();

                BatchLog.Root.ErrorFormat("A {0} occurred while loading batch files: {1}", exc.GetType().Name, exc.Message);
                BatchLog.Root.ErrorFormat("Stack Trace: {0}", exc.StackTrace);

                Email.SendEmail(ERROR_SUBJECT, sb.ToString(), Program.m_From, Program.m_To);
            }
        }

        private static void ProcessBatchFiles(List<iBatchFile> batchFiles)
        {
            BatchLog.Root.InfoFormat("Processing {0} batch files......", batchFiles.Count);
            string strErrorMessage = string.Empty;

            foreach (iBatchFile batchFile in batchFiles)
            {
                FileInfo bFile = null;
                string localFilePath = "";

                try
                {
                    //parse, dup check, log batch file, process file

                    BatchLog.Root.InfoFormat("Processing '{0}'......", batchFile.FileName);

                    bFile = new FileInfo(batchFile.FileName);

                    //copy file from FTP to local directory
                    localFilePath = MoveDecryptFile(batchFile);

                    //parse batch file
                    batchFile.ParseFile(localFilePath);

                    //check for duplicate files
                    bool dupFile = batchFile.PassDuplicateFile(bFile);

                    if (!dupFile)
                    {
                        //log batch file
                        batchFile.LogBatchFile(bFile);

                        //process parsed transaction one by one
                        batchFile.ImportFile();

                        //send merchant notification of successful batch processing
                        SendMerchantNotification(batchFile, bFile);
                    }
                    else
                    {
                        throw new BatchFileException("Duplicate ACH batch file", BatchFileErrorCode.DuplicateBatchFile);
                    }
                }
                catch (BatchFileException ex)
                {
                    BatchLog.Root.ErrorFormat("A BatchFileException has occurred while processing batch file '{0}': {1}", batchFile.FileName, ex.Message);
                    BatchLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                    //notify developers and merchant and move on to the next batch process
                    string subject = string.Format("{0} Batch Upload - FAILED", batchFile.Merchant.BusinessDBAName);
                    string body = string.Format("Merchant: {0}<br>Type: ACH Batch<br>Filename: {1}<br>Status: {2}", batchFile.Merchant.BusinessDBAName, bFile.Name, strErrorMessage);

                    BatchLog.Root.InfoFormat("Sending ACH Batch Error notification to merchant email: '{0}'", batchFile.Merchant.NotificationEmails);

                    MerchantFacade.SendEmail(subject, string.Empty, body, Constants.GLOBAL_EMAIL_FROM_ADDRESS, batchFile.Merchant.NotificationEmails, string.Empty, string.Empty, null, batchFile.Merchant.MerchantAppUID, "System");

                    BatchLog.Root.InfoFormat("Merchant notification email sent.");

                    Email.SendEmail(ERROR_SUBJECT, string.Format("Error Code [{0}]: {1}", ex.ErrorCode.GetHashCode(), body), Program.m_From, Program.m_To);

                    BatchLog.Root.InfoFormat("Developer notification email sent.");
                }
                catch (Exception ex)
                {
                    BatchLog.Root.ErrorFormat("A {2} has occurred while processing batch file '{0}': {1}", batchFile.FileName, ex.Message, ex.GetType().Name);
                    BatchLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                    Email.SendEmail(ERROR_SUBJECT, string.Format("A {2} has occurred while processing batch file '{0}': {1}", batchFile.FileName, ex.Message, ex.GetType().Name), Program.m_From, Program.m_To);
                }
                finally
                {
                    //copy archived file to both datacenter shared folders
                    EncryptArchiveBatchFile(localFilePath, batchFile);
                }
            }
        }


        /// <summary>
        /// move and decrypts (if batch file is encrypted) batch file to local directory and returns local file path
        /// </summary>
        /// <param name="batchFile"></param>
        /// <returns>local file path of batch file</returns>
        private static string MoveDecryptFile(iBatchFile batchFile)
        {
            bool decrypted = false;
            string strErrorMessage = string.Empty;

            FileInfo bFile = new FileInfo(batchFile.FileName);

            string localFileDir = string.Format(@"{0}{1}", _LocalTempDir, batchFile.MerchantID);

            if (!Directory.Exists(localFileDir))
            {
                BatchLog.Root.InfoFormat("Creating directory '{0}'......", localFileDir);
                Directory.CreateDirectory(localFileDir);
                BatchLog.Root.InfoFormat("Directory '{0}' created.", localFileDir);
            }

            //copy file to local directory
            string localFilePath = string.Format(@"{0}\{1}", localFileDir, bFile.Name);

            BatchLog.Root.InfoFormat("Moving batch file '{0}' to '{1}'......", batchFile.FileName, localFilePath);

            if (!batchFile.Encrypted)
            {
                //file not encrypted, just move it to local directory
                bFile.MoveTo(localFilePath);

                BatchLog.Root.InfoFormat("Batch file moved.");
            }
            else
            {
                localFilePath += batchFile.FileExtension;

                BatchLog.Root.InfoFormat("Batch file is encrypted, renaming local batch file to '{0}'......", localFilePath);

                //decrypt file to new local file path
                BatchLog.Root.InfoFormat("Decrypting batch file '{0}' to '{1}'......", batchFile.FileName, localFileDir);

                decrypted = CommonUtility.PGPHandler.Decrypt(batchFile.FileName, localFilePath, ref strErrorMessage);

                if (decrypted)
                {
                    BatchLog.Root.InfoFormat("Batch file successfully decrypted to '{0}'", localFilePath);

                    //delete file from FTP site
                    BatchLog.Root.InfoFormat("Deleting batch file from '{0}'......", batchFile.FileName);

                    try
                    {
                        File.Delete(batchFile.FileName);
                        BatchLog.Root.InfoFormat("File deleted.");
                    }
                    catch (Exception ex)
                    {
                        //file deletion failed, log exception and notify developers.
                        //let the batch processing proceed becus we don;t want to fail
                        //it just becus we couldn't delete the dercypted file. we'll have
                        //the devs manually delete this file when they get the notification
                        BatchLog.Root.ErrorFormat("Failed to delete FTP file '{0}': {1}", batchFile.FileName, ex.Message);
                        BatchLog.Root.ErrorFormat("Stack Trace: {0}", batchFile.FileName, ex.StackTrace);

                        Email.SendEmail(ERROR_SUBJECT, string.Format("Failed to delete FTP file '{0}'", batchFile.FileName), Program.m_From, Program.m_To);
                    }
                }
                else
                {
                    BatchLog.Root.ErrorFormat("Batch file decryption to '{0}' failed: {1}", localFileDir, strErrorMessage);

                    throw new BatchFileException("File decryption failed.", BatchFileErrorCode.DecryptionFailed);
                }
            }

            return localFilePath;
        }

        private static void SendMerchantNotification(iBatchFile batchFile, FileInfo bFile)
        {
            BatchLog.Root.InfoFormat("Sending email notification to '{0}'......", batchFile.Merchant.NotificationEmails);

            string subject = string.Format("{0} Batch Upload - SUCCESS", batchFile.Merchant.BusinessDBAName);
            string body = string.Format("MerchantID:{2}<br>Merchant: {0}<br>Type: ACH Batch<br>Filename: {1}<br>Status: Successful File Received<br>Count: {3}<br>", batchFile.Merchant.BusinessDBAName, bFile.Name, batchFile.Merchant.ID, batchFile.TotalTransCount);

            MerchantFacade.SendEmail(subject, string.Empty, body, Constants.GLOBAL_EMAIL_FROM_ADDRESS, batchFile.Merchant.NotificationEmails, string.Empty, string.Empty, null, batchFile.Merchant.MerchantAppUID, "System");

            BatchLog.Root.InfoFormat("Email notification to sent.");
        }

        private static void EncryptArchiveBatchFile(string localFilePath, iBatchFile batchFile)
        {
            if (string.IsNullOrWhiteSpace(localFilePath) || batchFile == null)
                return;

            //first encrypt file to temporary location
            BatchLog.Root.InfoFormat("Encrypting file '{0}'......", localFilePath);
            string localTemp = _LocalTempDir + batchFile.MerchantID;

            FileInfo info = new FileInfo(localFilePath);

            try
            {
                if (!Directory.Exists(localTemp))
                {
                    BatchLog.Root.InfoFormat("Creating directory '{0}'......", localTemp);
                    Directory.CreateDirectory(localTemp);
                    BatchLog.Root.InfoFormat("Directory '{0}' created.", localTemp);
                }

                localTemp += @"\Complete\";

                if (!Directory.Exists(localTemp))
                {
                    BatchLog.Root.InfoFormat("Creating directory '{0}'......", localTemp);
                    Directory.CreateDirectory(localTemp);
                    BatchLog.Root.InfoFormat("Directory '{0}' created.", localTemp);
                }

                localTemp += string.Format("{0}_{1}.pgp", DateTime.Now.ToString("yyyyMMddHHmmss"), info.Name);

                string strErrorMessage = "";

                BatchLog.Root.InfoFormat("Encrypting file to '{0}'.......", localTemp);

                if (CommonUtility.PGPHandler.Encrypt(localFilePath, localTemp, ref strErrorMessage))
                {
                    BatchLog.Root.InfoFormat("File '{0}' succesfully encrypted!", localTemp);
                    File.Delete(localFilePath);
                    BatchLog.Root.InfoFormat("File '{0}' succesfully deleted!", localFilePath);

                    //archive file to latisys shared folders
                    ArchiveBatchFile(_LatArchivePath, localTemp, batchFile.MerchantID);

                    //archive file to savvis shared folders
                    ArchiveBatchFile(_SavArchivePath, localTemp, batchFile.MerchantID);

                    //delete local encrypted file
                    File.Delete(localTemp);
                }
                else
                {
                    BatchLog.Root.ErrorFormat("Failed to encrypt file '{0}': {1}", localTemp, strErrorMessage);
                }
            }
            catch (BatchFileException ex)
            {
                Email.SendEmail(ERROR_SUBJECT, string.Format("Error Code [{0}]: {1}", ex.ErrorCode, ex.Message), Program.m_From, Program.m_To);
            }
            catch (Exception ex)
            {
                BatchLog.Root.ErrorFormat("A {2} has occurred while encrypting and archiving file '{0}': {1}", localFilePath, ex.Message, ex.GetType().Name);
                BatchLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                Email.SendEmail(ERROR_SUBJECT, string.Format("Failed to encrypt and archive file '{0}': {1}", localFilePath, ex.Message), Program.m_From, Program.m_To);
            }
        }


        private static void ArchiveBatchFile(string destDir, string encryptedFilePath, int zid)
        {
            if (Directory.Exists(destDir) && File.Exists(encryptedFilePath))
            {
                try
                {
                    BatchLog.Root.InfoFormat("Archiving file '{0}'......", encryptedFilePath);
                    FileInfo encryptedFile = new FileInfo(encryptedFilePath);

                    //pgp encrypt file
                    string archivePath = destDir + zid;

                    if (!Directory.Exists(archivePath))
                    {
                        BatchLog.Root.InfoFormat("Creating directory '{0}'......", archivePath);
                        Directory.CreateDirectory(archivePath);
                        BatchLog.Root.InfoFormat("Directory '{0}' created.", archivePath);
                    }

                    archivePath += @"\Complete\";

                    if (!Directory.Exists(archivePath))
                    {
                        BatchLog.Root.InfoFormat("Creating directory '{0}'......", archivePath);
                        Directory.CreateDirectory(archivePath);
                        BatchLog.Root.InfoFormat("Directory '{0}' created.", archivePath);
                    }

                    archivePath += encryptedFile.Name;

                    BatchLog.Root.InfoFormat("Archiving path: '{0}'", archivePath);

                    File.Copy(encryptedFilePath, archivePath);

                    BatchLog.Root.InfoFormat("File successfully archived.");
                }
                catch (Exception ex)
                {
                    BatchLog.Root.ErrorFormat("A {2} has occurred while archiving file '{0}' to '{3}': {1}", encryptedFilePath, ex.Message, ex.GetType().Name, destDir);
                    BatchLog.Root.ErrorFormat("Stack Trace: {0}", ex.StackTrace);

                    throw new BatchFileException(string.Format("Failed to archive encrypted file '{0}'", encryptedFilePath), BatchFileErrorCode.ArchiveFailed);
                }
            }
            else
            {
                BatchLog.Root.ErrorFormat("Failed to archive '{0}' to '{1}': Either destination directory or source file does not exist.", encryptedFilePath, destDir);
            }
        }



    }

}
