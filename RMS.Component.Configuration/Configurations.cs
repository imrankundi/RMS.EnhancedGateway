using PKPSAssets.Component.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPSAssets.Component.Configuration
{
    public class Configurations
    {
        public AgentConfiguration Agent { get; set; }
        public string LogPath { get; set; }
        public LogLevel LogLevel { get; set; }
        public List<TerminalConfiguration> TerminalAgents { get; set; }
        /// <summary>
        /// in seconds
        /// </summary>
        public int TransactionExpiry { get; set; }
    }
}
