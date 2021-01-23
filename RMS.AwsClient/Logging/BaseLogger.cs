using System;
using System.IO;
using System.Threading;

namespace RMS.AWS.Logging
{
    public class BaseLogger : ILogger
    {
        private string directory = AppDomain.CurrentDomain.BaseDirectory;

        private static ReaderWriterLock locker = new ReaderWriterLock();

        private string file;
        private string fileExtension;
        private string fileName;
        private BaseLogger(string directory, string file)
        {
            if (!string.IsNullOrEmpty(directory))
            {
                this.directory = directory;
            }

            this.file = file;
            try
            {
                string[] strArray = this.file.Split('.');
                if (strArray.Length > 0)
                {
                    string tempFileName = string.Empty;
                    for (int ii = 0; ii < strArray.Length - 1; ii++)
                    {
                        tempFileName += strArray[ii];
                    }
                    fileExtension = strArray[strArray.Length - 1];
                    fileName = tempFileName;
                }
                else
                {
                    fileName = file;
                }
            }
            catch (Exception)
            {

            }
        }


        public static ILogger CreateLogger(string directory, string file)
        {
            return new BaseLogger(directory, file);
        }

        public void Write(string text)
        {
            string dir = string.Format("{0}\\{1}", this.directory, DateTime.UtcNow.ToString("yyyyMMdd"));

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string file = string.Format("{0}\\{1}-{2}.{3}", dir, this.fileName,
                DateTime.UtcNow.ToString("yyyyMMddHH"), this.fileExtension);
            //string content = string.Format("{0} ({1}) [{2}] | {3}\n",
            //    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"),
            //    Thread.CurrentThread.ManagedThreadId.ToString("000"),
            //    level.ToString().Substring(0, 3).ToUpper(),
            //    text);

            try
            {
                BaseLogger.locker.AcquireWriterLock(int.MaxValue);
                File.AppendAllText(file, text);
            }
            finally
            {
                BaseLogger.locker.ReleaseWriterLock();
            }
        }
    }
}
