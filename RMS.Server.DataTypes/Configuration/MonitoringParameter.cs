using System;

namespace RMS.Server.DataTypes
{
    public class MonitoringParameter
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public int ServiceState { get; set; }
        public DateTime LastStateChange { get; set; }
        public bool TakeActionOnStop { get; set; }
        public bool TakeActionOnStart { get; set; }
        public bool TakeActionOnNotInstalled { get; set; }
        public int LastActionTaken { get; set; }
    }
}
