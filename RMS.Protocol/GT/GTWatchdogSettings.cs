using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTWatchdogSettings : ICGRC
    {
        public string TerminalId { get; private set; }
        public string Code => "NA";
        public GTCommandType CommandType { get; set; }
        public string CommandTypeDescription => CommandType.ToString();
        public int TimerInterval { get; set; }
        public GTWatchdogSettings(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.GeneralSettings;
        }
        public override string ToString()
        {
            return string.Format("{0}<WDT({1})>",
                TerminalId, TimerInterval);
        }
        private string GetBooleanAsString(bool value)
        {
            return (value ? "1" : "0");
        }
        private bool GetCharAsBoolean(char value)
        {
            return (value == '1' ? true : false);
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
