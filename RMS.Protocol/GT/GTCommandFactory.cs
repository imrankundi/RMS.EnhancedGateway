using Newtonsoft.Json.Linq;
using RMS.Parser;
using RMS.Server.DataTypes.Requests;
using System.Collections.Generic;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTCommandFactory
    {
        public static Dictionary<string, ICGRC> GetConfiguration(ReceivedPacket packet)
        {
            Dictionary<string, ICGRC> list = new Dictionary<string, ICGRC>();
            GTGeneralSettings cgrc00 = new GTGeneralSettings(packet.TerminalId);
            GTSimAndServerSettings cgrc01 = new GTSimAndServerSettings(packet.TerminalId);
            GTPollingAndGprsSettings cgrc02 = new GTPollingAndGprsSettings(packet.TerminalId);
            list.Add(nameof(GTGeneralSettings), cgrc00);
            list.Add(nameof(GTSimAndServerSettings), cgrc01);
            list.Add(nameof(GTPollingAndGprsSettings), cgrc02);
            //string raw = "SP333444<CGRC(ID(00,17/01/2021,07:51:07)N(+923333451191,+923468220229,+923333404763,,,,,,,,SR117R00BL01R00,13-Jan-16,16:27:21,+923,internet,,,+923,internet,,,333444,+923333451191,67.23.248.114,30004,SPMX00DT,SPMX01DT,,,,,,,1,15,2,4,30,9600,15,2)L(1000101101000000000001000000000000))\r>";
            string[] strArray = SplitPacket(packet.Data);

            cgrc00.Parse(strArray);
            cgrc01.Parse(strArray);
            cgrc02.Parse(strArray);

            return list;
        }

        public static ICGRC GetConfiguration(ReceivedPacket packet, GTCommandType commandType)
        {
            ICGRC cgrc = null;
            string[] strArray = null;
            switch (commandType)
            {
                case GTCommandType.GeneralSettings:
                    strArray = SplitPacket(packet.Data);
                    cgrc = new GTGeneralSettings(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                case GTCommandType.SimAndServerSettings:
                    strArray = SplitPacket(packet.Data);
                    cgrc = new GTSimAndServerSettings(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                case GTCommandType.PollingAndGprsSettings:
                    strArray = SplitPacket(packet.Data);
                    cgrc = new GTPollingAndGprsSettings(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                case GTCommandType.WatchdogSettings:
                    strArray = SplitPacket(packet.Data);
                    cgrc = new GTWatchdogSettings(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                case GTCommandType.Reset:
                    cgrc = new GTReset(packet.TerminalId);
                    break;
                case GTCommandType.ResetRom:
                    cgrc = new GTResetRom(packet.TerminalId);
                    break;
                case GTCommandType.ExtendedConfigurationSettings:
                    strArray = SplitPacket(packet.Data);
                    cgrc = new GTExtendedConfigurationSettings(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                case GTCommandType.GetModbusDevice:
                    strArray = SplitPacket(packet.Data.Replace("GET[","").TrimEnd(']'));
                    cgrc = new GTGetModbusDevice(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                case GTCommandType.GetMultipleModbusDevices:
                    strArray = SplitPacket(packet.Data, ';');
                    cgrc = new GTGetModbusDeviceCollection(packet.TerminalId);
                    cgrc.Parse(strArray);
                    break;
                default:
                    break;
            }
            return cgrc;
        }
        public static string CreateGetCommand(string terminalId, GTCommandType commandType)
        {
            string command = string.Empty;

            switch(commandType)
            {
                case GTCommandType.GeneralSettings:
                case GTCommandType.SimAndServerSettings:
                case GTCommandType.PollingAndGprsSettings:
                    command = string.Format("{0}<SGRC00ST>", terminalId);
                    break;
                case GTCommandType.WatchdogSettings:
                    command = string.Format("{0}<SGRC01ST>", terminalId);
                    break;
                case GTCommandType.Reset:
                    command = "RESETGDT";
                    break;
                case GTCommandType.ResetRom:
                    command = "RESETROM";
                    break;
                case GTCommandType.ExtendedConfigurationSettings:
                    command = string.Format("{0}<SGRC03ST>", terminalId);
                    break;
                default:
                    command = "";
                    break;
            }

            return command;
        }
        public static string CreateGetModbusDeviceCommand(string terminalId, int startIndex, int numberOfDevices = 1)
        {
            string command;

            StringBuilder sb = new StringBuilder();
            for(int ii = startIndex; ii < (startIndex + numberOfDevices); ii++)
            {
                sb.AppendFormat("GET[{0}];", ii);
            }
            command = string.Format("{0}<CMOD({1})>", terminalId, sb.ToString().TrimEnd(';'));
            return command;
        }
        public static string CreateSetCommand(string terminalId, object data, GTCommandType commandType)
        {
            JObject jsonObject = (JObject)data;
            ICGRC cgrc = new GTNullCommand(terminalId);
            switch(commandType)
            {
                case GTCommandType.GeneralSettings:
                    cgrc = jsonObject.ToObject<GTGeneralSettings>();
                    break;
                case GTCommandType.PollingAndGprsSettings:
                    cgrc = jsonObject.ToObject<GTPollingAndGprsSettings>();
                    break;
                case GTCommandType.SimAndServerSettings:
                    cgrc = jsonObject.ToObject<GTSimAndServerSettings>();
                    break;
                case GTCommandType.Reset:
                    cgrc = jsonObject.ToObject<GTReset>();
                    break;
                case GTCommandType.ResetRom:
                    cgrc = jsonObject.ToObject<GTResetRom>();
                    break;
                case GTCommandType.ExtendedConfigurationSettings:
                    cgrc = jsonObject.ToObject<GTExtendedConfigurationSettings>();
                    break;
                case GTCommandType.WatchdogSettings:
                    cgrc = jsonObject.ToObject<GTWatchdogSettings>();
                    break;
                case GTCommandType.AddMultipleModbusDevices:
                    cgrc = jsonObject.ToObject<GTAddModbusDeviceCollection>();
                    break;
                case GTCommandType.AddModbusDevice:
                    cgrc = jsonObject.ToObject<GTAddModbusDevice>();
                    break;
                default:
                    break;
            }

            return cgrc.ToString();
        }
        private static string[] SplitPacket(string rawPacket, char separator = ',')
        {
            if (!string.IsNullOrEmpty(rawPacket))
            {
                string[] strArray = rawPacket.Split(separator);
                return strArray;
            }

            return new string[] { };
        }
    }
}
