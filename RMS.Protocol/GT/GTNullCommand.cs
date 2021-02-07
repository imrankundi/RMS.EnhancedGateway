using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Text;

namespace RMS.Protocols.GT
{
    public class GTNullCommand : ICGRC
    {
        [JsonProperty("terminalId")]
        public string TerminalId { get; set; }
        [JsonProperty("code")]
        public string Code => "NA";
        [JsonProperty("commandType")]
        public GTCommandType CommandType { get; set; }
        [JsonProperty("commandTypeDescription")]
        public string CommandTypeDescription => CommandType.ToString();
        public GTNullCommand(string terminalId)
        {
            TerminalId = terminalId;
            CommandType = GTCommandType.Unknown;
        }
        public string CreateCommand()
        {
            return ToString();
        }
        public override string ToString()
        {
            return "";
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
