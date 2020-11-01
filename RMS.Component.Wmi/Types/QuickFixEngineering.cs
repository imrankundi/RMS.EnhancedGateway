namespace RMS.Component.Wmi
{
    public class QuickFixEngineering : WmiBaseType
    {
        public QuickFixEngineering()
            : base(WmiClasses.Win32_QuickFixEngineering)
        {
        }
        public string Caption { get; set; }
        public string Description { get; set; }
        public object InstallDate { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string CSName { get; set; }
        public string FixComments { get; set; }
        public string HotFixID { get; set; }
        public string InstalledBy { get; set; }
        public string InstalledOn { get; set; }
        public string ServicePackInEffect { get; set; }

    }
}
