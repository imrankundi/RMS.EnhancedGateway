namespace RMS.Component.Wmi
{
    public class Bios : WmiBaseType
    {
        public Bios()
            : base(WmiClasses.Win32_BIOS)
        {
        }
        public ushort[] BiosCharacteristics { get; set; }
        public string[] BIOSVersion { get; set; }
        public string BuildNumber { get; set; }
        public string Caption { get; set; }
        public string CodeSet { get; set; }
        public string CurrentLanguage { get; set; }
        public string Description { get; set; }
        public byte EmbeddedControllerMajorVersion { get; set; }
        public byte EmbeddedControllerMinorVersion { get; set; }
        public string IdentificationCode { get; set; }
        public ushort InstallableLanguages { get; set; }
        public object InstallDate { get; set; }
        public string LanguageEdition { get; set; }
        public string[] ListOfLanguages { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string OtherTargetOS { get; set; }
        public bool PrimaryBIOS { get; set; }
        public object ReleaseDate { get; set; }
        public string SerialNumber { get; set; }
        public string SMBIOSBIOSVersion { get; set; }
        public ushort SMBIOSMajorVersion { get; set; }
        public ushort SMBIOSMinorVersion { get; set; }
        public bool SMBIOSPresent { get; set; }
        public string SoftwareElementID { get; set; }
        public ushort SoftwareElementState { get; set; }
        public string Status { get; set; }
        public byte SystemBiosMajorVersion { get; set; }
        public byte SystemBiosMinorVersion { get; set; }
        public ushort TargetOperatingSystem { get; set; }
        public string Version { get; set; }

    }
}
