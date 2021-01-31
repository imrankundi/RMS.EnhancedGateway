using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTAddModbusDevice : ICGRC
    {
        public string TerminalId { get; set; }
        public string Code => "NA";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public string DeviceName { get; set; }
        public int DeviceId { get; set; }
        public int FunctionCode { get; set; }
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
            if (strArray != null)
            {
                if (strArray.Length > 4)
                {
                    DeviceName = strArray[0].Replace("ADD[", "");

                    int.TryParse(strArray[1], out int deviceId);
                    DeviceId = deviceId;

                    int.TryParse(strArray[2], out int functionCode);
                    FunctionCode = functionCode;

                    int.TryParse(strArray[3], out int startingAddress);
                    StartingAddress = startingAddress;

                    int.TryParse(strArray[4], out int numberOfElements);
                    NumberOfElements = numberOfElements;

                    int.TryParse(strArray[5].TrimEnd(']'), out int pageNumber);
                    PageNumber = pageNumber;

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
