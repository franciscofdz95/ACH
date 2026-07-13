using System;
using System.Windows.Forms;
using System.Configuration;
using System.Security.Principal;
using System.Threading;
using Nmc.Ach.Dal;


namespace AchSystem
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class main
	{
        public static string Current_User = string.Empty;
        public static frmMain g_frmMain = null;
        public static frmStatus g_frmStatus = new frmStatus();
        public static User g_User = new User();

		[STAThread]
		static void Main() 
		{

            Current_User = WindowsIdentity.GetCurrent().Name;
            int intPos = Current_User.IndexOf("\\");
            Current_User = Current_User.Substring(intPos + 1, Current_User.Length - intPos - 1);

            //Current_User = System.Environment.UserName;

            g_frmMain = new frmMain();
             Application.Run(g_frmMain);
		}



      	public main()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
