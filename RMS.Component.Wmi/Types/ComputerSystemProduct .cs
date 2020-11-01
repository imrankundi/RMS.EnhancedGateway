namespace RMS.Component.Wmi
{
    public class ComputerSystemProduct : WmiBaseType
    {
        public ComputerSystemProduct()
            : base(WmiClasses.Win32_ComputerSystemProduct)
        {
        }

        public string Caption { get; set; }
        public string Description { get; set; }
        public string IdentifyingNumber { get; set; }
        public string Name { get; set; }
        public string SKUNumber { get; set; }
        public string Vendor { get; set; }
        public string Version { get; set; }
        public string UUID { get; set; }

    }
}