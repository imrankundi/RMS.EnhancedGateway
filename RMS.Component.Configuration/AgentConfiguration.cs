using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKPSAssets.Component.Configuration
{
    public class AgentConfiguration
    {
        public string Url { get; set; }
        public IList<string> ServerIps { get; set; }
    }
}
