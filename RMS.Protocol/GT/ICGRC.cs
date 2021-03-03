using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;

namespace RMS.Protocols.GT
{
    public interface ICGRC
    {
        [JsonIgnore]
        string Code { get; }
        [JsonIgnore]
        string TerminalId { get; set; }
        GTCommandType CommandType { get; set; }
        string CommandTypeDescription { get; }
        void Parse(string[] strArray);
        string CreateCommand();
    }
}
