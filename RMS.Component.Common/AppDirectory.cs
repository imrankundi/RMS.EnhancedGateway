using System;
using System.Reflection;

namespace RMS.Component.Common
{
    public sealed class AppDirectory
    {
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static string ExecutionDirectory(object obj)
        {
            return System.IO.Path.GetDirectoryName(Assembly.GetAssembly(obj.GetType()).Location);
        }
    }
}
