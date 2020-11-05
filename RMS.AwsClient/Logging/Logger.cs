using RMS.Component.Common;

namespace RMS.AWS.Logging
{
    public class Logger
    {
        private static readonly Logger instance = new Logger();
        private const string logFileName = "packet.log";

        public ILog Log { get; set; }

        public static Logger Instance => instance;

        private Logger()
        {
            string folder = string.Format(@"{0}\Logs\", AppDirectory.BaseDirectory);
            Log = LoggingFactory.CreateLogger(folder, logFileName);
        }
    }

}
