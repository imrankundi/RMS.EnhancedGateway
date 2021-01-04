using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Server.DataTypes.WindowsService
{
    public class ServiceInfo
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
    }
}
