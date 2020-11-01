using System;

namespace RMS.Component.Common
{
    public sealed class Machine
    {

        public static string Name => Environment.MachineName;
        public static OperatingSystem OS => Environment.OSVersion;
        public static OperatingSystemPlatform Platform => Environment.Is64BitOperatingSystem ?
            OperatingSystemPlatform.x64 : OperatingSystemPlatform.x86;
    }
}
