using System;

namespace RMS.Core.Logging
{
    public class LoggingManager
    {
        private static Logger errorLogger = LoggingFactory.CreateErrorLog();

        public static void Log(Exception exception)
        {
            ErrorLog log = new ErrorLog(exception);
            errorLogger.Write(log);
        }

    }
}
