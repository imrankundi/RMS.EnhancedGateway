using System;
using System.Globalization;

namespace RMS.Network
{
    public sealed class DateTimeHelper
    {
        public static DateTime GetDateTime()
        {
            return DateTime.UtcNow;
        }

        public static string SynchronizeGT()
        {
            DateTime datetime = GetDateTime();
            return String.Format("SPDT({0},{1})",
                datetime.ToString("dd/MM/yyyy"),
                datetime.ToString("HH:mm:ss")); // giving problem with virtual gt
        }

        public static string GetIsoDateString()
        {
            return GetDateTime().ToString("yyyyMMdd");
        }

        public static DateTime ConvertFromUKDateTime(string date, string time)
        {
            DateTime datetime = DateTime.Parse(string.Format("{0} {1}", date, time), CultureInfo.GetCultureInfo("en-GB"));
            return datetime;
        }

    }
}
