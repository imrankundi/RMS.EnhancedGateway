using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;

namespace RMS.Protocols.GT
{
    public class GTPollingAndGprsSettings : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "02";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        [JsonProperty("device1")]
        public string Device1 { get; set; }
        [JsonProperty("device2")]
        public string Device2 { get; set; }
        [JsonProperty("device3")]
        public string Device3 { get; set; }
        [JsonProperty("device4")]
        public string Device4 { get; set; }
        [JsonProperty("device5")]
        public string Device5 { get; set; }
        [JsonProperty("device6")]
        public string Device6 { get; set; }
        [JsonProperty("device7")]
        public string Device7 { get; set; }
        [JsonProperty("device8")]
        public string Device8 { get; set; }
        [JsonProperty("gsmRetryTimeOut")]
        public int GSMRetryTimeOut { get; set; }
        [JsonProperty("smsTransmissionInterval")]
        public int SMSTransmissionInterval { get; set; }
        [JsonProperty("gprsRetryTimeout")]
        public int GPRSRetryTimeout { get; set; }
        [JsonProperty("gprsRetryCount")]
        public int GPRSRetryCount { get; set; }
        [JsonProperty("pollingInterval")]
        public int PollingInterval { get; set; }
        [JsonProperty("pollingBaudRate")]
        public int PollingBaudRate { get; set; }
        [JsonProperty("maxServerIdleTime")]
        public int MaxServerIdleTime { get; set; }
        [JsonProperty("noOfDevices")]
        public int NoOfDevices { get; set; }
        public GTPollingAndGprsSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.PollingAndGprsSettings;
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
        public string CreateCommand()
        {
            Device1 = string.IsNullOrEmpty(Device1) ? "" : Device1;
            Device2 = string.IsNullOrEmpty(Device2) ? "" : Device2;
            Device3 = string.IsNullOrEmpty(Device3) ? "" : Device3;
            Device4 = string.IsNullOrEmpty(Device4) ? "" : Device4;
            Device5 = string.IsNullOrEmpty(Device5) ? "" : Device5;
            Device6 = string.IsNullOrEmpty(Device6) ? "" : Device6;
            Device7 = string.IsNullOrEmpty(Device7) ? "" : Device7;
            Device8 = string.IsNullOrEmpty(Device8) ? "" : Device8;
            return string.Format("CGRC(ID({0},N,N)N({1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},)",
                Code, Device1, Device2, Device3, Device4, Device5, Device6, Device7, Device8,
                GSMRetryTimeOut, SMSTransmissionInterval, GPRSRetryTimeout, GPRSRetryCount, PollingInterval,
                PollingBaudRate, MaxServerIdleTime, NoOfDevices);
        }
        public override string ToString()
        {
            return string.Format("{0}<{1}>", TerminalId, CreateCommand());
        }

    }
}
