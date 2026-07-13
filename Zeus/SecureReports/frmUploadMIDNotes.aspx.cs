using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Threading;
using System.Data.OleDb;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.Facade;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Notify;

public partial class SecureReports_frmUploadMIDNotes : frmBaseSearch
{
    private int m_TotalRowCount = 0;
    Hashtable prms = new Hashtable();
    private string strErrorMsg = string.Empty;
    private string strSrcFileName = "ZeusMerchantNotes.xls";
    public int TotalRowCount
    {
        get { return m_TotalRowCount; }
        set { m_TotalRowCount = value; }
    }

  protected void Page_Load(object sender, EventArgs e)
  {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!Page.IsPostBack)
      {
          lnkDownload.NavigateUrl = "~/PDF/ZeusMerchantNotes.xls";
      }
      else
      {
          //lblMsg.Text = string.Empty;
      }
  }

  protected void btnUpload_Click(object sender, EventArgs e)
  {
      btnUpload.Enabled = false;
      System.Data.DataTable dt = null;
      
      int intRowsCnt = 0;

      lblMsg.Text = string.Empty;
      lblMsg2.Text = string.Empty;
      lblProcess.Text = string.Empty;

      try
      {
          
          if (FileUpload1.HasFile)
          {
              string strTempFile = string.Empty;
              int RowNumber = 0;
              int ZID = 0;
              int NoteID = 0;
              //int AgentID = 0;
              int RowCount = 0;
              string NoteText = string.Empty;
              string value = null;

              List<int> lstNonProcessedRows = new List<int>();

              if (ParseInputFile(out strTempFile))
              {
                  if (strTempFile.Trim() != string.Empty)
                  {
                      dt = GetExcelData(strTempFile, "InputData");
                      
                      if (dt != null)
                      {
                          ClearEmptyRows(dt);

                          RowCount = dt.Rows.Count;
                          if (RowCount > 300)
                          {
                              lblProcess.Text = "File contains more than 300 rows. Remove additional rows before processing.";
                          }
                          else if (RowCount > 0)
                          {

                              prms = new Hashtable();

                              foreach (DataRow dr in dt.Rows)
                              {
                                  RowNumber++;                      //For Current Row Number 
                                  if (dr.HasErrors)                 //Check if Data Row has any error
                                  {
                                      lstNonProcessedRows.Add(RowNumber);
                                      continue;
                                  }
                                  else if (!IsValidRow(dr))         //Check if any column has empty value
                                  {
                                      lstNonProcessedRows.Add(RowNumber);
                                      continue;
                                  }
                                

                                  //Check if Column *NoteCodeID does exist
                                  int intNoteID = 0;
                                  try
                                  {
                                      if (dr["*NoteCodeID"] != null)
                                          value = dr["*NoteCodeID"].ToString();
                                      else
                                      {
                                          lstNonProcessedRows.Add(RowNumber);
                                          continue;
                                      }
                                      //Check if Column *NoteCodeID has valid number 
                                     
                                      if (int.TryParse(value, out intNoteID))
                                      {
                                          NoteID = intNoteID;
                                      }
                                      else
                                      {
                                          lstNonProcessedRows.Add(RowNumber);
                                          continue;
                                      }
                                  }
                                  catch (Exception)
                                  {
                                      lblProcess.Text = "Invalid Column name(s), Can't process this file !!!";
                                      break;
                                  }

                                  //Check if Column *ZID does exist
                                  int intZID =0;
                                  try
                                  {
                                      if (dr["*ZID"] != null)
                                          value = dr["*ZID"].ToString();
                                      else
                                      {
                                          lstNonProcessedRows.Add(RowNumber);
                                          continue;
                                      }
                                      //Check if Column *ZID has valid number 

                                      if (int.TryParse(value, out intZID))
                                      {
                                          ZID = intZID;
                                      }
                                      else
                                      {
                                          lstNonProcessedRows.Add(RowNumber);
                                          continue;
                                      }
                                  }
                                  catch (Exception)
                                  {
                                      lblProcess.Text = "Invalid Column name(s), Can't process this file !!!";
                                      break;
                                  }
                                  
                                  //check if Note column does exist and should not have text length more than 25 chars
                                  try
                                  {
                                      if ((dr["*Note"] != null) && dr["*Note"].ToString().Length > 255)
                                      {
                                          lstNonProcessedRows.Add(RowNumber);
                                          continue;
                                      }
                                      else
                                        NoteText = dr["*Note"].ToString();
                                  }
                                  catch (Exception)
                                  {
                                      lblProcess.Text = "Invalid Column name(s), Can't process this file !!!";
                                      break;
                                  }

                             
                                  prms.Remove("@NoteID");
                                  prms.Remove("@ZID");
                                  prms.Clear();

                                  prms.Add("@NoteID", NoteID);
                                  prms.Add("@ZID", ZID);

                                  if (AddNotes(prms, NoteText))
                                  {
                                      intRowsCnt++;                         //Record Counter for Processed Row counter
                                  }
                                  else
                                  {
                                      lstNonProcessedRows.Add(RowNumber);   //Record Counter for Un Processed Records
                                  }
                              }
                          }
                      }
                      else
                      {
                          lblProcess.Text = "Uploaded Data sheet has data error or data format error, Please correct data errors and Upload again !!";
                      }
                   
                  }
              }

              lblMsg.Text = string.Format("Total Records in Data sheet = {0} And Total Number of Records Processed Successfully = {1}", RowCount, intRowsCnt);
              
              if(lstNonProcessedRows.Count > 0)
                  lblMsg2.Text = string.Format("Total number of Un-Processed Records = {0} & Row numbers are {1}  ", lstNonProcessedRows.Count, String.Join(",", lstNonProcessedRows.Select(i => i.ToString()).ToArray()));
              
          }
      }
      catch (Exception ex)
      {
          ZeusWeb.Logging.ErrorLog.ErrorFormat("Message:{0}", ex.Message);
          ZeusWeb.Logging.ErrorLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
          ZeusWeb.Logging.ErrorLog.ErrorFormat("TargetSite:{0}", ex.TargetSite);
          ZeusWeb.Logging.EmailLog.ErrorFormat("Error in side ParseInputFile method");
          ZeusWeb.Logging.EmailLog.ErrorFormat("Message:{0}", ex.Message);
          ZeusWeb.Logging.EmailLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
      }
      finally
      {
          dt = null;
          prms = null;
      }
      btnUpload.Enabled = true;
  }


  private bool IsValidRow(DataRow dr)
  {
      bool blnResult = false;
      try
      {
          int emptyCount = 0;
          int itemArrayCount = dr.ItemArray.Length;
          foreach (var i in dr.ItemArray) if (string.IsNullOrWhiteSpace(i.ToString())) emptyCount++;
          if(emptyCount > 0)
              blnResult = false;
          else
              blnResult = true;
      }
      catch (Exception)
      {
          blnResult = false;
      }
      return blnResult;
  }


  private DataTable ClearEmptyRows(DataTable dt)
  {
      try
      {
          List<int> rowIndexesToBeDeleted = new List<int>();
          int indexCount = 0;
          foreach (DataRow dr in dt.Rows)
          {
              int emptyCount = 0;
              int itemArrayCount = dr.ItemArray.Length;
              foreach (var i in dr.ItemArray) if (string.IsNullOrWhiteSpace(i.ToString())) emptyCount++;

              if (emptyCount == itemArrayCount) rowIndexesToBeDeleted.Add(indexCount);

              indexCount++;
          }
          int count = 0;
          foreach (var i in rowIndexesToBeDeleted)
          {
              dt.Rows.RemoveAt(i - count);
              count++;
          }

          return dt;
      }
      catch (Exception ex)
      {
          strErrorMsg = ex.Message;
          return null;
      }
  }

  private bool AddNotes(Hashtable prms, string NoteText)
  {
      bool blnResult = false;
      bool blnIsValid = false;
      string Subject = string.Empty;
      string Notes = string.Empty;
      int AgentID = 0;
      string Department = string.Empty;
      string MerchantAppUID = new Guid().ToString();
      DataSet ds = null;
      MerchantNotes ObjMerchantNotes = null;

      try
      {
          ds = DataMerchantAppPaging.GetMerchantNoteObjects(prms);
          if (ds != null)
          {
              if (ds.Tables[0] != null && ds.Tables[1] != null)
              {
                  
                  if(ds.Tables[0].Rows.Count > 0)
                  {
                      MerchantAppUID = ds.Tables[0].Rows[0]["UID"].ToString();
                      AgentID = (int)ds.Tables[0].Rows[0]["AgentID"];
                  }
                  if(ds.Tables[1].Rows.Count > 0)
                  {
                      Notes = NoteText;
                      Subject = ds.Tables[1].Rows[0]["Description"].ToString();
                      Department = ds.Tables[1].Rows[0]["TypeDesc"].ToString();
                      blnIsValid = true;
                  }

                  if (!blnIsValid)
                      return false;

                  User user = UserSessions.CurrentUser; 
                  ObjMerchantNotes = new MerchantNotes();

                  if (SaveMerchantNote(ObjMerchantNotes, MerchantAppUID, Subject, Notes, user.UserName,AgentID,Department))
                  {
                      blnResult = true;
                  }
                 
              }
              else
                  blnResult = false;
          }
      }
      catch (Exception ex)
      {
          strErrorMsg = ex.Message;
          blnResult = true;
      }
      finally
      {
          ds = null;
          ObjMerchantNotes = null;
      }
      return blnResult;
  }

  private bool SaveMerchantNote(MerchantNotes ObjMerchantNotes, string MerchantAppUID, string Subject, string Notes, string UserName, int AgentID, string Department)
  {
      bool blnResult = false;
      bool Email_Agent = true;
      try
      {
          ObjMerchantNotes.MerchantAppUID = MerchantAppUID;
          ObjMerchantNotes.Subject = Subject;
          ObjMerchantNotes.Notes = Notes;
          ObjMerchantNotes.View_Agent = true;
          ObjMerchantNotes.View_Bank = true;
          ObjMerchantNotes.View_MPSAll = true;
          ObjMerchantNotes.Email_Agent = Email_Agent;
          ObjMerchantNotes.UserCreated = UserName;

          DataAccess.DataMerchantAppDao.InsertMerchantNotes(ObjMerchantNotes);

          if (ObjMerchantNotes.Email_Agent)
          {
              SendNotification(ObjMerchantNotes,AgentID,Department);
          }

          blnResult = true;
      }
      catch (Exception)
      {
          blnResult = false;
      }
      return blnResult;
  }
  private void SendNotification(MerchantNotes ObjMerchantNotes, int AgentID, string Department)
  {
      AgentNotification notification = null;
      MerchantFacade facade = null;

      try
      {
          string AlertName = string.Empty;
          string AlertNote = string.Empty;
          string deptType = string.Empty;
          string From = string.Empty;
          string AlertTypeName = string.Empty;

          From = UserSessions.CurrentUser.Email;

          AlertName = ObjMerchantNotes.Subject;

          string notes = Server.HtmlEncode(Server.HtmlEncode(ObjMerchantNotes.Notes.Trim()));

          if (Department == "Risk")
          {
              notification = NotificationService.GetAgentNotification(AgentID, NotificationType.RiskUpdate, string.Empty);
          }
          else if (Department == "Collections")
          {
              notification = NotificationService.GetAgentNotification(AgentID, NotificationType.Collections, string.Empty);
          }
          if (notification != null && notification.Enabled)
          {
              if ((Department == "Risk") || (Department == "Collections"))
              {
                  User firstTeamRep = DataMerchantApp.GetInstance().GetMerchantUser(ObjMerchantNotes.MerchantAppUID, Constants.ROLE_FIRSTTEAM);

                  if (firstTeamRep != null)
                  {
                      notification.AddBccRecipient(firstTeamRep.Email);
                  }
                  notification.UserName = UserSessions.CurrentUser.UserName;
                  facade = new MerchantFacade();
                  MerchantApp app = facade.GetMerchantAppZeus(ObjMerchantNotes.MerchantAppUID);
                  AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "", notification.Name, CommonUtility.Formatting.nl2br(Server.HtmlDecode(notes)), AlertTypeName, Portal: ePortals.ZEUS);
              }
          }

      }
      catch (Exception ex)
      {
          strErrorMsg = ex.Message;
      }
      finally
      {
          facade = null;
          notification = null;
      }


  }

  private bool ParseInputFile(out string strTempFile)
  {
      bool blnResult = false;
      string strError = string.Empty;
      string strLine = string.Empty;
      string Qualifier = string.Empty;
      string strTempFilePath = string.Empty;

      strTempFile = string.Empty;
      HttpPostedFile fi = null;
      FileInfo file = null;
      try
      {

          fi = this.FileUpload1.PostedFile;
          String filepath = Server.MapPath("UploadedFiles");

          // create the directory if it does not exists!
          if (!Directory.Exists(filepath))
          {
              Directory.CreateDirectory(filepath);
          }

          strTempFile = filepath + "\\" + string.Concat(DateTime.Now.Ticks.ToString(), FileUpload1.PostedFile.FileName);
          file = new FileInfo(strTempFile);

          fi.SaveAs(strTempFile);

          blnResult = true;

      }
      catch (Exception ex)
      {
          //strErrorMsg = ex.Message;
          ZeusWeb.Logging.ErrorLog.ErrorFormat("Message:{0}", ex.Message);
          ZeusWeb.Logging.ErrorLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
          ZeusWeb.Logging.ErrorLog.ErrorFormat("TargetSite:{0}", ex.TargetSite);
          ZeusWeb.Logging.EmailLog.ErrorFormat("Error in side ParseInputFile method");
          ZeusWeb.Logging.EmailLog.ErrorFormat("Message:{0}", ex.Message);
          ZeusWeb.Logging.EmailLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
          blnResult = false;
      }
      finally
      {
          fi = null;
          file = null;
      }
      return blnResult;

  }

  private static Mutex mutex = new Mutex();
    public static System.Data.DataTable GetExcelData(string ExcelFilePath, string WorkSheetName)
    {
        try
        {
            ZeusWeb.Logging.ErrorLog.ErrorFormat("GetExcelData start File Paht:{0}", ExcelFilePath);

            DataTable dt = CommonUtility.ExcelHandling.GetExcelData(ExcelFilePath, WorkSheetName, null);
           
            if (dt.Rows.Count > 0)
            {
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field == null | object.ReferenceEquals(field, DBNull.Value) | field.Equals(""))).CopyToDataTable();
            }

            return dt;
        }
        catch (Exception ex)
        {
            ZeusWeb.Logging.ErrorLog.ErrorFormat("Message:{0}", ex.Message);
            ZeusWeb.Logging.ErrorLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
            ZeusWeb.Logging.ErrorLog.ErrorFormat("TargetSite:{0}", ex.TargetSite);
            ZeusWeb.Logging.EmailLog.ErrorFormat("Error in side GetExcelData method");
            ZeusWeb.Logging.EmailLog.ErrorFormat("Message:{0}", ex.Message);
            ZeusWeb.Logging.EmailLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
            return null;
        }
        finally
        {
            //mutex.ReleaseMutex();    // Release the Mutex.
        }
    }
    //public static System.Data.DataTable GetExcelData(string ExcelFilePath, string WorkSheetName)
    //{
    //    string OledbConnectionString = string.Empty;
    //    OleDbConnection objConn = null;
    //    try
    //    {
    //        ZeusWeb.Logging.ErrorLog.ErrorFormat("GetExcelData start File Paht:{0}", ExcelFilePath);
    //        //OledbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
    //        OledbConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFilePath + ";Extended Properties=Excel 8.0;";
    //        objConn = new OleDbConnection(OledbConnectionString);

    //        if (objConn.State == ConnectionState.Closed)
    //        {
    //            objConn.Open();
    //        }

    //        OleDbCommand objCmdSelect = new OleDbCommand("Select * from [" + WorkSheetName + "]", objConn);
    //        OleDbDataAdapter objAdapter = new OleDbDataAdapter();
    //        objAdapter.SelectCommand = objCmdSelect;
    //        DataSet objDataset = new DataSet();
    //        objAdapter.Fill(objDataset, "ExcelDataTable");
    //        objConn.Close();

    //        DataTable dt = new DataTable();
    //        if (objDataset.Tables[0].Rows.Count > 0)
    //        {
    //            dt = objDataset.Tables[0].Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field == null | object.ReferenceEquals(field, DBNull.Value) | field.Equals(""))).CopyToDataTable();
    //        }
    //        else
    //        {
    //            dt = objDataset.Tables[0];
    //        }

    //        return dt;
    //    }
    //    catch(Exception ex)
    //    {
    //        ZeusWeb.Logging.ErrorLog.ErrorFormat("Message:{0}", ex.Message);
    //        ZeusWeb.Logging.ErrorLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
    //        ZeusWeb.Logging.ErrorLog.ErrorFormat("TargetSite:{0}", ex.TargetSite);
    //        ZeusWeb.Logging.EmailLog.ErrorFormat("Error in side GetExcelData method");
    //        ZeusWeb.Logging.EmailLog.ErrorFormat("Message:{0}", ex.Message);
    //        ZeusWeb.Logging.EmailLog.ErrorFormat("StackTrace:{0}", ex.StackTrace);
    //        return null;
    //    }
    //    finally
    //    {
    //        //mutex.ReleaseMutex();    // Release the Mutex.
    //    }
    //}
}
