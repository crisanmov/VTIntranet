using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VTIntranet.Utils
{
    public class _Log
    {
        private static readonly _Log _instance = new _Log();
        protected ILog monitoringLogger;
        protected static ILog debugLogger;

        private _Log()
        {
            monitoringLogger = LogManager.GetLogger("VTI-Monitoring");
            debugLogger = LogManager.GetLogger("VTI-Debug");
        }

        public static void Debug(string message)
        {
            debugLogger.Debug(message);
        }

        public static void Debug(string message, System.Exception exception)
        {
            debugLogger.Debug(message, exception);
        }

        public static void Info(string message)
        {
            _instance.monitoringLogger.Info(message);
        }

        public static void Info(string message, System.Exception exception)
        {
            _instance.monitoringLogger.Info(message, exception);
        }

        internal static void info()
        {
            throw new NotImplementedException();
        }

        public static void Warn(string message)
        {
            _instance.monitoringLogger.Warn(message);
        }

        public static void Warn(string message, System.Exception exception)
        {
            _instance.monitoringLogger.Warn(message, exception);
        }

        public static void Error(string message)
        {
            _instance.monitoringLogger.Error(message);
        }

        public static void Error(string message, System.Exception exception)
        {
            _instance.monitoringLogger.Error(message, exception);
        }

        public static void Fatal(string message)
        {
            _instance.monitoringLogger.Fatal(message);
        }

        public static void Fatal(string message, System.Exception exception)
        {
            _instance.monitoringLogger.Fatal(message, exception);
        }
    }
}