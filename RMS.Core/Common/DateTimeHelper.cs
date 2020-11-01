using System;
using System.Globalization;

namespace RMS.Core.Common
{
    public sealed class DateTimeHelper
    {

        public static DateTime CurrentUniversalTime
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public static DateTime OffsetTime(double offset)
        {
            return CurrentUniversalTime.AddHours(offset);
        }

        public static DateTime ConvertFromUkFormat(string datetimeString)
        {
            DateTime datetime = DateTime.Parse(datetimeString, CultureInfo.GetCultureInfo("en-GB"));
            return datetime;

        }

        public static DateTime Parse(string datetime, string format)
        {
            return DateTime.ParseExact(datetime, format, CultureInfo.InvariantCulture);
        }

        public static DateTime FromEpoch(double epochSeconds)
        {
            return Epoch().AddSeconds(epochSeconds);
        }

        public static DateTime Epoch()
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

    }
}