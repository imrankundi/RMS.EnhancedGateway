using System.IO;

namespace RMS.Component.Communication.Logging
{

    public class LoggingFactory
    {
        public static ILog CreateLogger(string directory, string file)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string filePath = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
            return new LogWrapper(directory, file);
        }

    }



}
