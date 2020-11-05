using RMS.Core.Common;
using System;

namespace RMS.Gateway
{
    public sealed class ListenerHelper
    {
        public const string DefaultTerminalId = "SPxxxxxx";
        public static string TimeSync()
        {
            DateTime datetime = DateTimeHelper.CurrentUniversalTime;
            return String.Format("SPDT({0},{1})",
                datetime.ToString(DateTimeFormat.UK.Date),
                datetime.ToString(DateTimeFormat.UK.Time)); // giving problem with virtual gt
        }
    }
}
