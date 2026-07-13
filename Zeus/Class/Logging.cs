using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;

namespace ZeusWeb
{
    public static class Logging
    {
        private static ILog _ExperianLog = log4net.LogManager.GetLogger("Zeus.Experian");
       

        static Logging()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static ILog ExperianLog
        {
            get { return _ExperianLog; }
        }

        private static ILog _ErrorLog = log4net.LogManager.GetLogger("Zeus.Error");
        public static ILog ErrorLog
        {
            get { return _ErrorLog; }
        }

        //Added by Abarua PXP-8896
        private static ILog _ConsentAPILog = log4net.LogManager.GetLogger("Zeus.ConsentAPI");
        public static ILog ConsentAPILog
        {
            get { return _ConsentAPILog; }
        }
        //Added by Abarua PXP-7866 and PXP-9122
        private static ILog _NMIMerchantOnboardingAPILog = log4net.LogManager.GetLogger("Zeus.NMIMerchantOnboardingAPI");
        public static ILog NMIMerchantOnboardingAPIog
        {
            get { return _NMIMerchantOnboardingAPILog; }
        }
        //Code added for PXP-12181
        private static ILog _EmailLog = log4net.LogManager.GetLogger("Zeus.EmailLog");
        public static ILog EmailLog
        {
            get { return _EmailLog; }
        }

    }
}