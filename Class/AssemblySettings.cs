using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Xml;
using System.Configuration;
namespace ACH2005
{
	/// <summary>
	/// Summary description for AssemblySettings.
	/// </summary>
	class AssemblySettings
	{
		private static AssemblySettings asmSettingsInstance = null;
		private IDictionary settings;

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected AssemblySettings(): this(Assembly.GetCallingAssembly())
		{
		}

		protected AssemblySettings( Assembly asm )
		{
			settings = GetConfig(asm);
		}

		/// <summary>
		/// Creating LoggerInstance using singleton pattern
		/// </summary>
		public static AssemblySettings AsmSettingsInstance()
		{
			if (asmSettingsInstance == null)
			{
				asmSettingsInstance = new AssemblySettings();
			}
			return asmSettingsInstance;
		}

		public string this[ string key ]
		{
			get
			{
				string settingValue = null;

				if( settings != null )
				{
					settingValue = settings[key] as string;
				}

				return(settingValue == null ? "" : settingValue);
			}
		}

		private static IDictionary GetConfig()
		{
			return GetConfig(Assembly.GetCallingAssembly());
		}

		private static IDictionary GetConfig( Assembly asm )
		{
			// Open and parse configuration file for specified
			// assembly, returning collection to caller for future
			// use outside of this class.
			//
			try
			{
				string cfgFile = asm.CodeBase + ".Xml";
				const string nodeName = "assemblySettings";

				XmlDocument doc = new XmlDocument();
				doc.Load(new XmlTextReader(cfgFile));
            
				XmlNodeList nodes = doc.GetElementsByTagName(nodeName);

				foreach( XmlNode node in nodes )
				{
					if( node.LocalName == nodeName )
					{
						DictionarySectionHandler handler = new DictionarySectionHandler();
						return (IDictionary)handler.Create(null, null, node);
					}
				}
			}
			catch
			{
			}	

			return(null);
		}
	}
}