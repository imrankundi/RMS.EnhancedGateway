using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTWatchdogSettings : ICGRC
    {
        public string TerminalId { get; set; }
        public string Code => "NA";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public int TimerInterval { get; set; }
        public GTWatchdogSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.WatchdogSettings;
        }
        public string CreateCommand()
        {
            return string.Format("WDT({0})", TimerInterval);
        }
        public override string ToString()
        {
            return string.Format("{0}<{1}>", TerminalId, CreateCommand());
        }
        public void Parse(string[] strArray)
        {
            if (strArray != null)
            {
                if (strArray.Length > 17)
                {
                    int.TryParse(strArray[5], out int timerInterval);
                    TimerInterval = timerInterval;
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
