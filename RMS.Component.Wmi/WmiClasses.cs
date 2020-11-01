namespace RMS.Component.Wmi
{
    public sealed class WmiClasses
    {
        public static WmiClassInfo Win32_OperatingSystem = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_OperatingSystem));
        public static WmiClassInfo Win32_ComputerSystemProduct = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_ComputerSystemProduct));
        public static WmiClassInfo Win32_BaseBoard = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_BaseBoard));
        public static WmiClassInfo Win32_BIOS = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_BIOS));
        public static WmiClassInfo Win32_Processor = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_Processor));
        public static WmiClassInfo Win32_QuickFixEngineering = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_QuickFixEngineering));
        public static WmiClassInfo Win32_Process = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_Process));
        public static WmiClassInfo Win32_SessionProcess = new WmiClassInfo(WmiNamespaces.cimv2, nameof(Win32_SessionProcess));
    }
}
