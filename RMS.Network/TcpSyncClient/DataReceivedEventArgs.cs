using System;

namespace RMS.Network.Client
{
    public class DataReceivedEventArgs : EventArgs
    {
        public string ReceivedData { get; set; }
    }
}
