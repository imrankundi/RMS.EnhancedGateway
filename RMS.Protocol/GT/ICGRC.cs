using Newtonsoft.Json;
using RMS.Server.DataTypes.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Protocols.GT
{
    public interface ICGRC
    {
        [JsonIgnore]
        string Code { get; }
        [JsonIgnore]
        string TerminalId { get; }
        GTCommandType CommandType { get; set; }
        string CommandTypeDescription { get; }
        void Parse(string[] strArray);
    }
}
