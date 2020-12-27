using System.Collections.Generic;

namespace RMS.Server.DataTypes
{
    public class SiteInfo
    {
        public string TerminalId { get; set; }
        public string Name { get; set; }
        public TimeOffset TimeOffset { get; set; }
    }
    public class TimeOffset
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
    }
    public class Sites
    {
        public Dictionary<string, SiteInfo> SiteList { get; set; }
    }
}
