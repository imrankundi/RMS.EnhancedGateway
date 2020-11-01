using System;

namespace RMS.Network.Server
{
    public class ServerErrorEventArgs : EventArgs
    {
        public string Exception { get; set; }
        public ServerErrorEventArgs(string exception)
        {
            this.Exception = exception;
        }
    }
}
