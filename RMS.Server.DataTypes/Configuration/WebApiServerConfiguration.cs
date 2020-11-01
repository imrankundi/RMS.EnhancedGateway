using RMS.Component.Configuration;

namespace RMS.Server.WebApi.Configuration
{
    public class WebApiServerConfiguration : BaseConfiguration
    {
        public string Url { get; set; }
        public bool EnableTcpServer { get; set; }
        public bool EnableSimulation { get; set; }
    }
}
