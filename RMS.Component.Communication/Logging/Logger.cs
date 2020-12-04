using RMS.Component.Common;

namespace RMS.Component.Communication.Logging
{
    public class Logger
    {
        private static readonly Logger instance = new Logger();
        private const string logFileName = "received-packets.log";

        public ILog Log { get; set; }

        public static Logger Instance => instance;

        private Logger()
        {
            string folder = string.Format(@"{0}\Logs\", AppDirectory.BaseDirectory);
            Log = LoggingFactory.CreateLogger(folder, logFileName);
        }
    }

}
