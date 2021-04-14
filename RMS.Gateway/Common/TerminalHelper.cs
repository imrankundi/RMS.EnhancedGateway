using RMS.Core.Common;
using System;

namespace RMS.Gateway
{
    public sealed class TerminalHelper
    {
        public const string DefaultTerminalId = "SPxxxxxx";
        public const string PONG = nameof(PONG);
        public static string TimeSync()
        {
            DateTime datetime = DateTimeHelper.CurrentUniversalTime;
            return String.Format("SPDT({0},{1})\r\n",
                datetime.ToString(DateTimeFormat.UK.Date),
                datetime.ToString(DateTimeFormat.UK.Time)); // giving problem with virtual gt
        }
    }
}
