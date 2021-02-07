using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTAddModbusDeviceCollection : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "NA";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        [JsonProperty("devices")]
        public ICollection<GTAddModbusDevice> Devices { get; set; }

        public GTAddModbusDeviceCollection()
        {

        }
        public GTAddModbusDeviceCollection(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.AddMultipleModbusDevices;
            Devices = new List<GTAddModbusDevice>();
        }
        public string CreateCommand()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var device in Devices)
            {
                sb.AppendFormat("{0};", device.CreateCommand());
            }
            return sb.ToString().TrimEnd(';');
        }
        public override string ToString()
        {
            return string.Format("{0}<CMOD({1})>", TerminalId, CreateCommand());
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                foreach (var str in strArray)
                {
                    if (str.StartsWith("ADD"))
                    {
                        GTAddModbusDevice device = new GTAddModbusDevice(TerminalId);
                        var arr = str.Replace("ADD[", "").TrimEnd(']').Split(',');
                        device.Parse(arr);
                        Devices.Add(device);
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
