using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Network.Client
{
    public class DataReceivedEventArgs : EventArgs
    {
        public string ReceivedData { get; set; }
    }
}
