using RMS.Core.Common;
using System;

namespace RMS.Gateway
{
    public class ClientStatus
    {
        public string TerminalId { get; set; }
        public DateTime ConnectedOn { get; set; }
        public bool IsValid { get; set; }
        public ClientType ClientType { get; set; }
        public DateTime LastDataReceived { get; set; }


        public TimeSpan ConnectionElapsedTime
        {
            get
            {
                return DateTimeHelper.CurrentUniversalTime.Subtract(ConnectedOn);
            }
        }

        public double ConnectionElapsedSeconds
        {
            get
            {
                return ConnectionElapsedTime.TotalSeconds;
            }
        }

        public TimeSpan LastDataReceivedElapsedTime
        {
            get
            {
                return DateTimeHelper.CurrentUniversalTime.Subtract(LastDataReceived);
            }
        }

        public double LastDataReceivedElapsedSeconds
        {
            get
            {
                return LastDataReceivedElapsedTime.TotalSeconds;
            }
        }
    }
}
