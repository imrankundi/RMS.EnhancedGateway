using System;
using System.IO;
using System.Text;
using System.Threading;

namespace RMS.Component.Common.Helpers
{
    public class FileHelper
    {
        static ReaderWriterLock locker = new ReaderWriterLock();
        public static void WriteAllText(string path, string text)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                File.WriteAllText(path, text);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }

        public static void WriteAllText(string path, string text, Encoding encoding)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                File.WriteAllText(path, text, encoding);
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }

        public static string ReadAllText(string path)
        {
            string text = string.Empty;
            try
            {
                locker.AcquireReaderLock(int.MaxValue);
                text = File.ReadAllText(path);
            }
            finally
            {
                locker.ReleaseReaderLock();
            }

            return text;
        }

        public static string ReadAllTextWithRetries(string path, int retries = 10, int waitInMilliseconds = 100)
        {
            string text = string.Empty;
            while (retries > 0)
            {
                try
                {
                    text = File.ReadAllText(path);
                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(waitInMilliseconds);
                }
            }

            return text;
        }

        public static bool DeleteFileWithRetries(string path, int retries = 10, int waitInMilliseconds = 100)
        {
            bool result = false;
            while (retries > 0)
            {
                try
                {
                    File.Delete(path);
                    result = true;
                    break;
                }
                catch (Exception)
                {
                    retries--;
                    Thread.Sleep(waitInMilliseconds);
                }
            }
            return result;
        }

        public static bool DeleteFile(string file)
        {
            try
            {
                File.Delete(file);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
