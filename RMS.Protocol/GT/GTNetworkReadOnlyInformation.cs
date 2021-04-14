using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;

namespace RMS.Protocols.GT
{
    public class GTNetworkReadOnlyInformation : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "NA";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        [JsonProperty("IMEI")]
        public string IMEI { get; set; }
        [JsonProperty("serviceProviderName")]
        public string ServiceProviderName { get; set; }
        [JsonProperty("cellId")]
        public string CellId { get; set; }
        [JsonProperty("areaCode")]
        public string AreaCode { get; set; }
        [JsonProperty("operationBrand")]
        public string OperationBrand { get; set; }
        [JsonProperty("rfChannel")]
        public string RfChannel { get; set; }
        [JsonProperty("receiveLevel")]
        public string ReceiveLevel { get; set; }
        [JsonProperty("receiveQuality")]
        public string ReceiveQuality { get; set; }
        [JsonProperty("mobileCountryCode")]
        public string MobileCountryCode { get; set; }
        [JsonProperty("mobileNetworkCode")]
        public string MobileNetworkCode { get; set; }
        [JsonProperty("baseStationId")]
        public string BaseStationId { get; set; }
        [JsonProperty("minimumReceiveLevel")]
        public string MinimumReceiveLevel { get; set; }
        [JsonProperty("txPowerMaximum")]
        public string TxPowerMaximum { get; set; }
        [JsonProperty("timingAdvance")]
        public string TimingAdvance { get; set; }
        public GTNetworkReadOnlyInformation(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.NetworkReadOnlyInformation;
        }
        public string CreateCommand()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return string.Format("{0}<{1}>", TerminalId, CreateCommand());
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                if (strArray.Length > 18)
                {
                    IMEI = strArray[5];
                    ServiceProviderName = strArray[6];
                    CellId = strArray[7];
                    AreaCode = strArray[8];
                    OperationBrand = strArray[9];
                    RfChannel = strArray[10];
                    ReceiveLevel = strArray[11];
                    ReceiveQuality = strArray[12];
                    MobileCountryCode = strArray[13];
                    MobileNetworkCode = strArray[14];
                    BaseStationId = strArray[15];
                    MinimumReceiveLevel = strArray[16];
                    TxPowerMaximum = strArray[17];
                    TimingAdvance = strArray[18];
                    //TimerInterval = timerInterval;
                }

            }
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public string ToFormattedJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
