using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public static IEnumerable<FileSystemInfo> GetFilesOrderByCreationDateAscending(string directoryPath, string searchPattern)
        {
            var fileInfo = new DirectoryInfo(directoryPath).GetFileSystemInfos(searchPattern)
                .OrderBy(fi => fi.CreationTime);
            return fileInfo;
        }
        public static IEnumerable<FileSystemInfo> GetFilesOrderByCreationDateDescending(string directoryPath, string searchPattern)
        {
            var fileInfo = new DirectoryInfo(directoryPath).GetFileSystemInfos(searchPattern)
                .OrderBy(fi => fi.CreationTime);
            return fileInfo;
        }
        public static FileSystemInfo GetOldestFile(string directoryPath, string searchPattern)
        {
            FileSystemInfo fileInfo = new DirectoryInfo(directoryPath).GetFileSystemInfos(searchPattern)
                .OrderByDescending(fi => fi.CreationTime).FirstOrDefault();
            return fileInfo;
        }

        public static FileSystemInfo GetNewestFile(string directoryPath, string searchPattern)
        {
            FileSystemInfo fileInfo = new DirectoryInfo(directoryPath).GetFileSystemInfos(searchPattern)
                .OrderByDescending(fi => fi.CreationTime).FirstOrDefault();
            return fileInfo;
        }

        public static IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            string[] files = Directory.GetFiles(directory, searchPattern);
            return files;
        }

        public static void GetNewestDirectory(string directoryPath)
        {
            DateTime lastHigh = DateTime.MinValue;
            string highestDirectory;
            foreach (string subDirectory in Directory.GetDirectories(directoryPath))
            {
                DirectoryInfo fi1 = new DirectoryInfo(subDirectory);
                DateTime lastModified = fi1.LastWriteTime;

                if (lastModified > lastHigh)
                {
                    highestDirectory = subDirectory;
                    lastHigh = lastModified;
                }
            }
        }
        public static string GetOldestDirectory(string directoryPath)
        {
            DateTime lastLow = DateTime.Now;
            string lowestDirectory = string.Empty;
            foreach (string subDirectory in Directory.GetDirectories(directoryPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(subDirectory);
                DateTime lastModified = directoryInfo.LastWriteTime;

                if (lastModified < lastLow)
                {
                    lowestDirectory = subDirectory;
                    lastLow = lastModified;
                }
            }
            return lowestDirectory;
        }

        public static bool RenameFile(string oldFileName, string newFiledName)
        {
            try
            {
                File.Move(oldFileName, newFiledName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static void DeleteDirectory(string directory)
        {

            // Get an object repesenting the directory path below
            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            // Traverse all of the child directors in the root; get to the lowest child
            // and delte all files, working our way back up to the top.  All files
            // must be deleted in the directory, before the directory itself can be deleted.
            foreach (DirectoryInfo diChild in directoryInfo.GetDirectories())
                TraverseDirectoryAndDeleteAllFiles(diChild);

            // Finally, clean all of the files directly in the root directory
            DeleteAllFilesInDirectory(directoryInfo);

            directoryInfo.Refresh();
            if (directoryInfo.GetFiles().Count() == 0)
                directoryInfo.Delete();

        }
        public static void TraverseDirectoryAndDeleteAllFiles(DirectoryInfo directoryInfo)
        {

            // If the current directory has more child directories, then continure
            // to traverse down until we are at the lowest level.  At that point all of the
            // files will be deleted.
            foreach (DirectoryInfo diChild in directoryInfo.GetDirectories())
                TraverseDirectoryAndDeleteAllFiles(diChild);

            // Now that we have no more child directories to traverse, delete all of the files
            // in the current directory, and then delete the directory itself.
            DeleteAllFilesInDirectory(directoryInfo);


            // The containing directory can only be deleted if the directory
            // is now completely empty and all files previously within
            // were deleted.
            directoryInfo.Refresh();
            if (directoryInfo.GetFiles().Count() == 0)
                directoryInfo.Delete();
        }
        public static void DeleteAllFilesInDirectory(DirectoryInfo directory)
        {
            foreach (FileInfo fi in directory.GetFiles())
            {
                // The following code is NOT required, but shows how some logic can be wrapped
                // around the deletion of files.  For example, only delete files with
                // a creation date older than 1 hour from the current time.  If you
                // always want to delete all of the files regardless, just remove
                // the next 'If' statement.
                if (fi.CreationTime < DateTime.Now.Subtract(new TimeSpan(0, 0, 1)))
                {
                    // Read only files can not be deleted, so mark the attribute as 'IsReadOnly = False'
                    fi.IsReadOnly = false;
                    fi.Delete();

                    // On a rare occasion, files being deleted might be slower than program execution, and upon returning
                    // from this call, attempting to delete the directory will throw an exception stating it is not yet
                    // empty, even though a fraction of a second later it actually is.  Therefore the 'Optional' code below
                    // can stall the process just long enough to ensure the file is deleted before proceeding. The value
                    // can be adjusted as needed from testing and running the process repeatedly.
                    Thread.Sleep(50);  // 50 millisecond stall (0.05 Seconds)
                }
            }

        }

    }
}
