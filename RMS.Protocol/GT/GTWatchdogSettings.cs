using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;

namespace RMS.Protocols.GT
{
    public class GTWatchdogSettings : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "NA";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        [JsonProperty("timerInterval")]
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
