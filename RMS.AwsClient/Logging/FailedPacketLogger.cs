using RMS.Component.Common;

namespace RMS.AWS.Logging
{
    public class FailedPacketLogger
    {
        private static readonly FailedPacketLogger instance = new FailedPacketLogger();
        private const string logFileName = "failed-pushed-packets.log";

        public ILog Log { get; set; }

        public static FailedPacketLogger Instance => instance;

        private FailedPacketLogger()
        {
            string folder = string.Format(@"{0}\Logs\Failed\", AppDirectory.BaseDirectory);
            Log = LoggingFactory.CreateLogger(folder, logFileName);
        }
    }

}
