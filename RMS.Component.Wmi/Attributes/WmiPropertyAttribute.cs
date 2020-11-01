using System;

namespace RMS.Component.Wmi
{
    public class WmiPropertyAttribute : Attribute
    {
        public string Name { get; set; }
        public bool Ignore { get; set; }
    }
}
