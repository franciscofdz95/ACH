using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Reflection;


namespace Nmc.Common
{
	/// <summary>
	/// Summary description for Logger.
	/// </summary>
	public class Logger
	{
		protected static Logger loggerInstance = null;
		static FileStream fs = null;
		public Logger()
		{			
			
		}
		/// <summary>
		/// Creating LoggerInstance using singleton pattern
		/// </summary>
		public static Logger LoggerInstance
		{
			get 
			{
				if (loggerInstance == null)
				{
					loggerInstance = new Logger();
					string dirName = Assembly.GetExecutingAssembly().GetName().Name;
					string dirpath = Assembly.GetExecutingAssembly().Location;
					dirpath = dirpath.Substring(0,dirpath.Length -3) + "log";
					//if (!(Directory.Exists(dirpath)))
					//    Directory.CreateDirectory(dirpath);
					string filepath = dirpath;
					fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
				}
				return loggerInstance;
			}
			set
			{ loggerInstance = value; }
		}

		/// <summary>
		/// Writes the string, followed by the current line terminator to the log file
		/// </summary>
		/// <param name="message">message to write to the log file as a string</param>
		public void WriteLine(string message)
		{
			//Console.WriteLine(message);
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.WriteLine(DateTime.Now.ToString()+ "  " + message);
			streamWriter.Flush();
            streamWriter.Close();
		}	

		public void Write(string message)
		{
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End);
			streamWriter.WriteLine(DateTime.Now.ToString()+ "  " + message);
			streamWriter.Flush();
            streamWriter.Close();
		}

        public static void LogEmptyLine()
        {
            string path = ConfigurationManager.AppSettings["LogFilePath"] + DateTime.Today.ToString("yyyyMM") + @"\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            StreamWriter sw = new StreamWriter(path + DateTime.Today.ToString("yyyyMMdd") + ".txt", true);
            sw.WriteLine("");
            sw.Flush();
            sw.Close();
        }

        public static void LogError(string msg)
        {
            Write2File("Error", msg);
        }

        public static void LogInfo(string msg)
        {
            Write2File("Info", msg);
        }

        public static void LogDebug(string msg)
        {
            Write2File("Debug", msg);
        }


        private static void Write2File(string type, string msg)
        {
            string path = ConfigurationManager.AppSettings["LogFilePath"] + DateTime.Today.ToString("yyyyMM") + @"\";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            StreamWriter sw = new StreamWriter(path + DateTime.Today.ToString("yyyyMMdd") + ".txt", true);
            sw.WriteLine(DateTime.Now.ToString() + " " + type + " --> " + msg);
            sw.Flush();
            sw.Close();
        }

        public static void Log(Exception e)
        {
            for (int i = 0; i < 1000; i++)
            {
                Logger.LogError(e.Message);
                Logger.LogError(e.StackTrace);
                if (e.InnerException != null)
                    e = e.InnerException;
                else
                    break;
            }
        }
	}
}
