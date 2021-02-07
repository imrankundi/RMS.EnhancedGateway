using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTReset : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "NA";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        public GTReset(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.Reset;
        }
        public string CreateCommand()
        {
            return "RESETGDT";
        }
        public override string ToString()
        {
            return CreateCommand();
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
