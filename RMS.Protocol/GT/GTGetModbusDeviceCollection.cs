using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System.Collections.Generic;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTGetModbusDeviceCollection : ICGRC
    {
        public string TerminalId { get; private set; }
        public string Code => "NA";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public ICollection<GTGetModbusDevice> Devices { get; set; }

        public GTGetModbusDeviceCollection(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.GetMultipleModbusDevices;
            Devices = new List<GTGetModbusDevice>();
        }
        public string CreateCommand()
        {
            string command = string.Empty;
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
            if(strArray != null)
            {
                foreach(var str in strArray)
                {
                    GTGetModbusDevice device = new GTGetModbusDevice(TerminalId);
                    var arr = str.Replace("GET[", "").TrimEnd(']').Split(',');
                    device.Parse(arr);
                    Devices.Add(device);
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
