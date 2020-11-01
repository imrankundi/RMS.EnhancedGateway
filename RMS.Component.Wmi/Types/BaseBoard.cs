﻿using System;
namespace RMS.Component.Wmi
{
    public class BaseBoard : WmiBaseType
    {
        public BaseBoard()
            : base(WmiClasses.Win32_BaseBoard)
        {
        }

        public string Caption { get; set; }
        public string[] ConfigOptions { get; set; }
        public string CreationClassName { get; set; }
        public Single Depth { get; set; }
        public string Description { get; set; }
        public Single Height { get; set; }
        public bool HostingBoard { get; set; }
        public bool HotSwappable { get; set; }
        public object InstallDate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string OtherIdentifyingInfo { get; set; }
        public string PartNumber { get; set; }
        public bool PoweredOn { get; set; }
        public string Product { get; set; }
        public bool Removable { get; set; }
        public bool Replaceable { get; set; }
        public string RequirementsDescription { get; set; }
        public bool RequiresDaughterBoard { get; set; }
        public string SerialNumber { get; set; }
        public string SKU { get; set; }
        public string SlotLayout { get; set; }
        public bool SpecialRequirements { get; set; }
        public string Status { get; set; }
        public string Tag { get; set; }
        public string Version { get; set; }
        public Single Weight { get; set; }
        public Single Width { get; set; }

    }
}