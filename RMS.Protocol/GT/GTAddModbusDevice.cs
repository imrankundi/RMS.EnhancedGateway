using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTAddModbusDevice : ICGRC
    {
        public string TerminalId { get; private set; }
        public string Code => "NA";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public int FunctionCode { get; private set; }
        public int StartingAddress { get; set; }
        public int NumberOfElements { get; set; }
        public int PageNumber { get; set; }

        public GTAddModbusDevice(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.AddModbusDevice;
            FunctionCode = 3;
        }
        public string CreateCommand()
        {
            return string.Format("ADD[{0},{1},{2},{3},{4},{5}]", DeviceName,
                DeviceId, FunctionCode, StartingAddress, NumberOfElements, PageNumber);
        }
        public override string ToString()
        {
            return string.Format("{0}<CMOD({1})>", TerminalId, CreateCommand());
        }
        public void Parse(string[] strArray)
        {
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
