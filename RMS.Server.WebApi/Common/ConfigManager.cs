using RMS.Protocols.Configuration;

namespace RMS.Server.WebApi.Common
{
    public class ConfigManager
    {
        public static ConfigProtocol GetConfigProtocol(string protocolHeader)
        {
            var protocol = new ConfigProtocol();
            protocol.Device = "SGRC";
            protocol.ProtocolHeader = protocolHeader;
            ConfigSection section = new ConfigSection();
            section.SectionKey = "N";
            section.Name = "Numeric";
            section.Parameters.Add(new ConfigParameter
            {
                ConfigParameterControlType = ConfigParameterControlType.Series,
                DataType = Core.Enumerations.DataType.Double,
                Format = "00.0",
                MaxValue = 10.9,
                MinValue = 0.1,
                StepSize = 0.1,
                Name = "Test 1"
            });
            section.Parameters.Add(new ConfigParameter
            {
                ConfigParameterControlType = ConfigParameterControlType.ListOfValues,
                DataType = Core.Enumerations.DataType.Double,
                Format = "00.0",
                MaxValue = 10.9,
                MinValue = 0.1,
                StepSize = 0.1,
                Values = { 10, 12 },
                Name = "Test 2"
            });
            protocol.ConfigSections.Add(section);

            return protocol;
        }
    }
}
