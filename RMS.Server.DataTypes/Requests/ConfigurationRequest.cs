using RMS.Component.WebApi.Requests;
using System.Collections.Generic;

namespace RMS.Server.DataTypes.Requests
{
    public class ConfigurationRequest : Request
    {
        public ConfigurationRequest()
        {
            RequestType = ServerRequestType.Configuration;
        }

        public string TerminalId { get; set; }
        public IList<string> Packets { get; set; }
    }
}
