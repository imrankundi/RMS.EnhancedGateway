using System;
using System.Globalization;

namespace RMS.Component.Common
{
    public static class DateTimeHelper
    {
        public static DateTime UnixEpochStart => new DateTime(1970, 1, 1, 0, 0, 0);
        public static DateTime ThisWeekMonday
        {
            get
            {
                var today = DateTime.Now;
                return new GregorianCalendar().AddDays(today, -((int)today.DayOfWeek) + 1);
            }
        }
    }
}
