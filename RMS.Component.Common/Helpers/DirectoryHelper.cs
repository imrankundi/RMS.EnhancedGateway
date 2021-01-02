using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RMS.Component.Common.Helpers
{
    public class DirectoryHelper
    {
        public static IEnumerable<string> FindFiles(string directory, IEnumerable<string> extensions)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var files = Directory
                .EnumerateFiles(directory, "*.*", SearchOption.AllDirectories)
                .Where(s => extensions.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));

            return files;
        }
    }
}
