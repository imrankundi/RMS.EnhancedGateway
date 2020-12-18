namespace RMS.Protocols.GT
{
    public class CGRC02
    {
        public string Device1 { get; set; }
        public string Device2 { get; set; }
        public string Device3 { get; set; }
        public string Device4 { get; set; }
        public string Device5 { get; set; }
        public string Device6 { get; set; }
        public string Device7 { get; set; }
        public string Device8 { get; set; }
        public int GSMRetryTimeOut { get; set; }
        public int SMSTransmissionInterval { get; set; }
        public int GPRSRetryTimeout { get; set; }
        public int GPRSRetryCount { get; set; }
        public int PollingInterval { get; set; }
        public int PollingBaudRate { get; set; }
        public int MaxServerIdleTime { get; set; }
        public int NoOfDevices { get; set; }

        public override string ToString()
        {
            Device1 = string.IsNullOrEmpty(Device1) ? "" : Device1;
            Device2 = string.IsNullOrEmpty(Device2) ? "" : Device2;
            Device3 = string.IsNullOrEmpty(Device3) ? "" : Device3;
            Device4 = string.IsNullOrEmpty(Device4) ? "" : Device4;
            Device5 = string.IsNullOrEmpty(Device5) ? "" : Device5;
            Device6 = string.IsNullOrEmpty(Device6) ? "" : Device6;
            Device7 = string.IsNullOrEmpty(Device7) ? "" : Device7;
            Device8 = string.IsNullOrEmpty(Device8) ? "" : Device8;
            return string.Format("CGRC(ID(02,N,N)N({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},)",
                Device1, Device2, Device3, Device4, Device5, Device6, Device7, Device8,
                GSMRetryTimeOut, SMSTransmissionInterval, GPRSRetryTimeout, GPRSRetryCount, PollingInterval,
                PollingBaudRate, MaxServerIdleTime, NoOfDevices);
        }
    }
}
