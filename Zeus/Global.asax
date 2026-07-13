<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    {

            /* */
            //get reference to the source of the exception chain


            if (Server.GetLastError() == null) return;

            Exception ex = Server.GetLastError().GetBaseException();

            if (ex.Message == "File does not exist.")
            {
                string str = string.Format("{0} {1}", ex.Message, HttpContext.Current.Request.Url.ToString());
                return;
            }


            string User = string.Empty;

            if (UserSessions.CurrentUser != null)
                User = UserSessions.CurrentUser.UserName;
           // else
                //Response.Redirect("~/frmLogin.aspx");
        
            //Code modified for PXP-12181
            //PXP-16780: Code Changes:Start
            try
            {
                ZeusWeb.Logging.ErrorLog.InfoFormat("Zues Application Error : {0}", ex.Message.ToString());
                ZeusWeb.Logging.ErrorLog.ErrorFormat("StackTrace : {0}", ex.StackTrace);
                ZeusWeb.Logging.ErrorLog.ErrorFormat("TargetSite : {0}", ex.TargetSite);
                ZeusWeb.Logging.ErrorLog.InfoFormat("SERVER:{0} ,TIME:{1}", HttpContext.Current.Server.MachineName, DateTime.Now.ToString());
                ZeusWeb.Logging.ErrorLog.InfoFormat("UserName:{0} ,BROWSER:{1}", User, Request.Browser.Browser);
                ZeusWeb.Logging.ErrorLog.InfoFormat("FORM:{0} ,QUERYSTRING:{1}", Request.Form.ToString(), Request.QueryString.ToString());
                
            }
            catch (Exception ex1)
            {
                ZeusWeb.Logging.ErrorLog.ErrorFormat("Message :: {0}", ex1.Message);
                ZeusWeb.Logging.ErrorLog.ErrorFormat("StackTrace :: {0}", ex1.StackTrace);
                ZeusWeb.Logging.ErrorLog.ErrorFormat("TargetSite :: {0}", ex1.TargetSite);
            }
            //PXP-16780: Code Changes:End
            Server.ClearError();
            Response.Redirect("~/frmError.aspx");
            //Server.Execute("~/frmError.aspx");


    }

    void Session_Start(object sender, EventArgs e) 
    {
         
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        //Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message  
        if (UserSessions.CurrentUser != null && UserSessions.CurrentMerchantApp!=null)
        PaymentXP.Facade.MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
        /******** End of PXP-2206 **************/
           
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
    
       
</script>
