using RMS.Component.Logging.Models;
using System.IO;

namespace RMS.Component.Logging
{

    public class LoggingFactory
    {
        public static ILog CreateLogger(string directory, string file, LogLevel level)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            file = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
            return new SerilogWrapper(file, level);
        }

        public static ILog CreateLogger(string directory, string file, string level)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            file = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
            return new SerilogWrapper(file, level);
        }
    }

}
