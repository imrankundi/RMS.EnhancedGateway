using RMS.Component.Configuration;

namespace RMS.Simulator.Configuration
{
    public class SimulatorConfig : BaseConfiguration
    {
        public string ConnectionStringName { get; set; }
        public string Url { get; set; }
    }
}
