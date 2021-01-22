namespace RMS.Protocols.GT
{
    public class CGRC02 : ICGRC
    {
        public string TerminalId { get; private set; }
        public string Code => "02";
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
        public CGRC02(string terminalId)
        {
            TerminalId = terminalId;
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                if (strArray.Length > 45)
                {
                    Device1 = strArray[30];
                    Device2 = strArray[31];
                    Device3 = strArray[32];
                    Device4 = strArray[33];
                    Device5 = strArray[34];
                    Device6 = strArray[35];
                    Device7 = strArray[36];
                    Device8 = strArray[37];

                    int.TryParse(strArray[38], out int gsmRetryTimeout);
                    GSMRetryTimeOut = gsmRetryTimeout;

                    
                    int.TryParse(strArray[39], out int smsTransmissionInterval);
                    SMSTransmissionInterval = smsTransmissionInterval;

                    int.TryParse(strArray[40], out int gprsRetryTimeout);
                    GPRSRetryTimeout = gprsRetryTimeout;


                    int.TryParse(strArray[41], out int gprsRetryCount);
                    GPRSRetryCount = gprsRetryCount;

                    int.TryParse(strArray[42], out int pollingInterval);
                    PollingInterval = pollingInterval;

                    int.TryParse(strArray[43], out int pollingBaudRate);
                    PollingBaudRate = pollingBaudRate;

                    int.TryParse(strArray[44], out int maxServerIdleTime);
                    MaxServerIdleTime = maxServerIdleTime;

                    int.TryParse(strArray[45], out int noOfDevices);
                    NoOfDevices = noOfDevices;

                }
            }
        }
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
            return string.Format("{0}<CGRC(ID({1},N,N)N({2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},)>",
                TerminalId, Code, Device1, Device2, Device3, Device4, Device5, Device6, Device7, Device8,
                GSMRetryTimeOut, SMSTransmissionInterval, GPRSRetryTimeout, GPRSRetryCount, PollingInterval,
                PollingBaudRate, MaxServerIdleTime, NoOfDevices);
        }

    }
}
