using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTClearModbusDevices : ICGRC
    {
        private const string deviceResponseMessage = "CLEARED ALL MODBUS STRINGS";
        private const string failureResponseMessage = "FAILED TO CLEAR MODBUS STRINGS";
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "NA";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        public string Message { get; set; }
        //public string DeviceName { get; set; }
        //public int DeviceId { get; set; }
        //public int FunctionCode { get; private set; }
        //public int StartingAddress { get; set; }
        //public int NumberOfElements { get; set; }
        //public int PageNumber { get; set; }

        public GTClearModbusDevices(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.ClearAllModbusDevices;
        }
        public string CreateCommand()
        {
            return "CLR";
        }
        public override string ToString()
        {
            return string.Format("{0}<CMOD({1})>", TerminalId, CreateCommand());
        }
        public void Parse(string[] strArray)
        {
            if(strArray != null)
            {
                if(strArray.Length > 0)
                {
                    if(strArray[0].Equals(deviceResponseMessage))
                    {
                        Message = deviceResponseMessage;
                    }
                    else
                    {
                        Message = failureResponseMessage;
                    }
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
