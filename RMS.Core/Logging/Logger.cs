using RMS.Core.Common;
using System;
using System.IO;
using System.Threading;

namespace RMS.Core.Logging
{
    public class Logger
    {
        private string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string directory;

        public Logger(string directory)
        {
            this.directory = string.Format(@"{0}\{1}", applicationDirectory, directory);
        }

        private static ReaderWriterLock locker = new ReaderWriterLock();

        public void Write(Log log)
        {
            Write(log.ToString());
        }

        public void Write(string text)
        {
            string file = string.Format(@"{0}\{1}.log", directory,
                DateTimeHelper.CurrentUniversalTime.ToString(DateTimeFormat
                .ISO8601WithoutDelimeter.Date));

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                File.AppendAllLines(file, new[] { text });
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}
