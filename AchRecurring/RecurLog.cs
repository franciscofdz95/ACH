using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace AchRecurring
{
    internal class RecurLog
    {
        private static ILog _Root = log4net.LogManager.GetLogger("RecurCC.Root");

        static RecurLog()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static ILog Root
        {
            get { return _Root; }
        }
    }
}
