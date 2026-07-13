using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace BatchFileLoader
{
    public static class BatchLog
    {
        private static ILog _Root = log4net.LogManager.GetLogger("Batch.Root");
        private static ILog _Batch = log4net.LogManager.GetLogger("Batch.Processing");

        static BatchLog()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public static ILog Root
        {
            get { return _Root; }
        }

        public static ILog Batch
        {
            get { return _Batch; }
        }
    }
}
