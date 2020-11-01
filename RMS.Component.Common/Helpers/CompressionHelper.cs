using System;
using System.IO;
using System.IO.Compression;

namespace RMS.Component.Common.Helpers
{
    public static class Guard
    {
        /// <summary>
        /// Throws ArgumentNullException for null parameter
        /// </summary>
        /// <param name="parameter">Value to check against</param>
        /// <param name="message">Exception message</param>
        public static void CheckNull(object parameter, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(message);
        }

        /// <summary>
        /// First check for Null and throws ArgumentNullException otherwise throws ArgumentException for empty string
        /// </summary>
        /// <param name="parameter">Value to check against</param>
        /// <param name="message">Exception message</param>
        public static void CheckNullOrEmpty(string parameter, string message)
        {
            Guard.CheckNull(parameter, message);

            if (parameter.Length == 0)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// First check for Null and throws ArgumentNullException otherwise throws ArgumentException for white space or empty string
        /// </summary>
        /// <param name="parameter">Value to check against</param>
        /// <param name="message">Exception message</param>
        public static void CheckNullOrTrimEmpty(string parameter, string message)
        {
            Guard.CheckNull(parameter, message);

            if (parameter.Trim().Length == 0)
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Check using string.IsNullOrWhiteSpace and throws ArgumentException
        /// </summary>
        /// <param name="parameter">Value to check against</param>
        /// <param name="message">Exception message</param>
        public static void CheckNullOrWhiteSpace(string parameter, string message)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                throw new ArgumentException(message);
        }

        /// <summary>
        /// Throws ArgumentNullException for null array
        /// </summary>
        /// <param name="parameter">Value to check against</param>
        /// <param name="message">Exception message</param>
        public static void CheckNull(Array parameter, string message)
        {
            if (parameter == null)
                throw new ArgumentNullException(message);
        }

        /// <summary>
        /// First check for Null and throws ArgumentNullException otherwise throws ArgumentException for empty array
        /// </summary>
        /// <param name="parameter">Value to check against</param>
        /// <param name="message">Exception message</param>
        public static void CheckNullOrEmpty(Array parameter, string message)
        {
            Guard.CheckNull(parameter, message);

            if (parameter.Length == 0)
                throw new ArgumentException(message);
        }
    }
    /// <summary>
    /// Compress files into .gz extension and decompress files from .gz extension
    /// </summary>
    public static class GunZip
    {
        /// <summary>
        /// Compress file into GunZip format in same location with .gz extension
        /// </summary>
        /// <param name="filePath">Path of original file</param>
        /// <returns>Compress file path</returns>
        public static string Compress(string filePath)
        {
            Guard.CheckNullOrWhiteSpace(filePath, "Compress(filePath)");

            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
                throw new ArgumentException(string.Format("File doesn't exists: '{0}'", filePath));

            if (fileInfo.Extension == ".gz")
                throw new ArgumentException(string.Format("File is already in compress format: '{0}'", filePath));

            var compressFilePath = filePath + ".gz";
            using (var inputStream = fileInfo.OpenRead())
            {
                using (var outputStream = File.OpenWrite(compressFilePath))
                {
                    using (var compressStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        inputStream.CopyTo(compressStream);
                        return compressFilePath;
                    }
                }
            }
        }

        /// <summary>
        /// Decompress file with .gz extension in same location
        /// </summary>
        /// <param name="compressFilePath">Compress GunZip file path</param>
        /// <returns>Decompressed file path</returns>
        public static string Decompress(string compressFilePath)
        {
            Guard.CheckNullOrWhiteSpace(compressFilePath, "Compress(compressFilePath)");

            var compressFileInfo = new FileInfo(compressFilePath);
            if (!compressFileInfo.Exists)
                throw new ArgumentException(string.Format("Compress file doesn't exists: '{0}'", compressFilePath));

            if (compressFileInfo.Extension != ".gz")
                throw new ArgumentException(string.Format("Compress file extension should be .gz: '{0}'", compressFilePath));

            var filePath = Path.GetFileNameWithoutExtension(compressFilePath);
            using (var inputStream = compressFileInfo.OpenRead())
            {
                using (var outputStream = File.OpenWrite(filePath))
                {
                    using (var decompressStream = new GZipStream(outputStream, CompressionMode.Decompress))
                    {
                        inputStream.CopyTo(decompressStream);
                        return filePath;
                    }
                }
            }
        }
    }
}
