using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using PaymentXP.BusinessObjects.Logging;

namespace AchSystem
{
    public class Logging: ILogging
    {

        private static ILog _NCALSplit = log4net.LogManager.GetLogger("ACH.NCALSplits");


        static Logging()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        public static ILog NCALSplit
        {
            get
            {
                return _NCALSplit;
            }
        }

        private ILog _log;

        public Logging(ILog log)
        {
            _log = log;  
        }

        public void Debug(object log)
        {
            this._log.Debug(log);
        }

        public void DebugFormat(string log, params object[] args)
        {
            this._log.DebugFormat(log, args);

        }

        public void Error(object log)
        {
            this._log.Error(log);
        }

        public void ErrorFormat(string log, params object[] args)
        {
            this._log.ErrorFormat(log,args);
        }

        public void Fatal(object log)
        {
            this._log.Fatal(log);
        }

        public void FatalFormat(string log, params object[] args)
        {
            this._log.FatalFormat(log,args);
        }

        public void Info(object log)
        {
            this._log.Info(log);
        }

        public void InfoFormat(string log, params object[] args)
        {
            this._log.InfoFormat(log,args);
        }

        public void Warn(object log)
        {
            this._log.Warn(log);
        }

        public void WarnFormat(string log, params object[] args)
        {
            this._log.WarnFormat(log);
        }
    }
}
