using RMS.Server.DataTypes.Requests;
using System;

namespace RMS.Server.WebApi
{
    public class TcpRequestContext
    {
        public Request Request { get; set; }
        public DateTime RequestReceivedOn { get; private set; }
        public double RequestTimeoutInSeconds => 10.0;
        public bool IsResponseReceived { get; set; }
        public DateTime? ResponseReceivedOn { get; set; }
        public bool IsTimedOut => RequestReceivedOn.AddSeconds(RequestTimeoutInSeconds) < DateTime.UtcNow ? false : true;

        public TcpRequestContext()
        {
            RequestReceivedOn = DateTime.UtcNow;
        }

    }
}
