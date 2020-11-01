using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Parser.Configuration
{
    public class ConfigurationQueueResult
    {
        public ConfigurationQueueStatus Status { get; set; }
        public ConfigurationPacket Packet { get; set; }
        public string Message { get; set; }
    }
}
