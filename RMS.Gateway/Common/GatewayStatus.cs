using RMS.Core.Common;
using System;
using System.Text;

namespace RMS.Gateway
{
    class GatewayStatus
    {
        public bool Started { get; set; }
        public bool ListenerConnected { get; set; }
        public bool Restarting { get; set; }
        public bool Kicking { get; set; }
        public int TerminalClientCount { get; set; }
        public int ListenerClientCount { get; set; }
        public DateTime LastSync { get; set; }
        public DateTime LastKickCheck { get; set; }

        public TimeSpan LastSyncTimeElapsed
        {
            get
            {
                return DateTimeHelper.CurrentUniversalTime.Subtract(LastSync);
            }
        }
        public double LastSyncElapsedSeconds
        {
            get
            {
                return LastSyncTimeElapsed.TotalSeconds;
            }
        }

        public TimeSpan LastKickCheckElapsedTime
        {
            get
            {
                return DateTimeHelper.CurrentUniversalTime.Subtract(LastKickCheck);
            }
        }
        public double LastKickCheckElapsedSeconds
        {
            get
            {
                return LastKickCheckElapsedTime.TotalSeconds;
            }
        }
        public DateTime StartedOn { get; private set; }

        public TimeSpan UpTime
        {
            get
            {
                return DateTimeHelper.CurrentUniversalTime.Subtract(StartedOn);
            }
        }

        public double UpTimeSeconds
        {
            get
            {
                return UpTime.TotalSeconds;
            }
        }

        public GatewayStatus()
        {
            StartedOn = DateTimeHelper.CurrentUniversalTime;
            LastSync = DateTimeHelper.CurrentUniversalTime;
            LastKickCheck = DateTimeHelper.CurrentUniversalTime;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(KeyValueString("Started", Started));
            sb.AppendLine(KeyValueString("Listener Connected", ListenerConnected));
            sb.AppendLine(KeyValueString("Restarting", Restarting));
            sb.AppendLine(KeyValueString("Kicking", Kicking));
            sb.AppendLine(KeyValueString("Connected Client Count", TerminalClientCount));
            sb.AppendLine(KeyValueString("Last GT Synchronization", LastSync));
            sb.AppendLine(KeyValueString("Last GT Synchronization Elapsed Time", LastSyncTimeElapsed));
            sb.AppendLine(KeyValueString("Last Kick Check", LastKickCheck));
            sb.AppendLine(KeyValueString("Last Kick Check Elapsed Time", LastKickCheckElapsedTime));
            sb.AppendLine(KeyValueString("Last Kick Check Elapsed Seconds", LastKickCheckElapsedSeconds / 1000));
            sb.AppendLine(KeyValueString("UpTime", UpTime));
            sb.AppendLine(KeyValueString("Up Time Seconds", UpTimeSeconds / 1000));
            return sb.ToString();
        }

        private string KeyValueString(string key, object value)
        {
            return string.Format("{0}:{1}", key, value);
        }
    }
}
