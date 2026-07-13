using System;
using System.IO;
using System.Diagnostics;


public class Logger
{
	private static BooleanSwitch eSwitch = new BooleanSwitch("Error", "Error Tracing");
	private static BooleanSwitch wSwitch = new BooleanSwitch("Warning", "Warning Tracing");
	private static BooleanSwitch dSwitch = new BooleanSwitch("Debug", "Debug Tracing");
	private static BooleanSwitch iSwitch = new BooleanSwitch("Info", "Info Tracing");
	public static void LogInfo(string msg)
	{
		Trace.WriteLineIf(iSwitch.Enabled,"Info --> "+msg);
	}
	public static void LogError(string msg)
	{
		Trace.WriteLineIf(eSwitch.Enabled,"Error --> "+msg);
	}
	public static void LogDebug(string msg)
	{
		Trace.WriteLineIf(dSwitch.Enabled,"Debug --> "+msg);
	}
	public static void LogWarning(string msg)
	{
		Trace.WriteLineIf(wSwitch.Enabled,"Warning --> "+msg);
	}
	public static void Log(Exception e)
	{
		for(int i=0; i< 1000; i++)
		{
			Logger.LogError(e.Message);
			Logger.LogError(e.StackTrace);
			if(e.InnerException != null)
				e = e.InnerException;
			else
				break;
		}
	}
}

