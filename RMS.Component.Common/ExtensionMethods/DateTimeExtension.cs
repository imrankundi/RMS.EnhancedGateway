using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RMS.Component.Common
{

    public static class DateTimeExtension
    {
        public static DateTime FirstDayOfMonth(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }
        public static DateTime LastDayOfMonth(this DateTime datetime)
        {
            int noOfDaysInMonth = DateTime.DaysInMonth(datetime.Year, datetime.Month);
            return new DateTime(datetime.Year, datetime.Month, noOfDaysInMonth);
        }
        public static DateTime StartOfTheDay(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);
        }
        public static DateTime EndOfTheDay(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, datetime.Day, 23, 59, 59, 999);
        }
        public static DateTime AddWorkdays(this DateTime datetime, int days)
        {
            // start from a weekday
            while (datetime.DayOfWeek.IsWeekday()) datetime = datetime.AddDays(1.0);
            for (int i = 0; i < days; ++i)
            {
                datetime = datetime.AddDays(1.0);
                while (datetime.DayOfWeek.IsWeekday()) datetime = datetime.AddDays(1.0);
            }
            return datetime;
        }
        public static DateTime FromUnixEpoch(this double epochElapsedSeconds)
        {
            return DateTimeHelper.UnixEpochStart.AddSeconds(epochElapsedSeconds);
        }
        public static DateTime StartOfWeek(this DateTime datetime, DayOfWeek startOfWeek)
        {
            int diff = datetime.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return datetime.AddDays(-1 * diff).Date;
        }
        public static IEnumerable<DateTime> GetDateRangeTo(this DateTime self, DateTime toDate)
        {
            var range = Enumerable.Range(0, new TimeSpan(toDate.Ticks - self.Ticks).Days);

            return from p in range
                   select self.Date.AddDays(p);
        }
        public static bool IsWeekend(this DateTime datetime)
        {
            return (datetime.DayOfWeek == DayOfWeek.Sunday || datetime.DayOfWeek == DayOfWeek.Saturday);
        }
        public static bool IsBetween(this DateTime datetime, DateTime startDate, DateTime endDate, bool compareTime = false)
        {
            return compareTime ?
               datetime >= startDate && datetime <= endDate :
               datetime.Date >= startDate.Date && datetime.Date <= endDate.Date;
        }
        public static bool IsLeapYear(this DateTime datetime)
        {
            return (DateTime.DaysInMonth(datetime.Year, 2) == 29);
        }
        public static bool IsWeekend(this DayOfWeek dayOfWeek)
        {
            return !dayOfWeek.IsWeekday();
        }
        public static bool IsWeekday(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday: return false;

                default: return true;
            }
        }
        public static long ToUnixEpoch(this DateTime datetime)
        {
            TimeSpan unixTimeSpan = datetime - DateTimeHelper.UnixEpochStart;
            return (long)unixTimeSpan.TotalSeconds;
        }
        public static TimeSpan TimeElapsed(this DateTime datetime)
        {
            return DateTime.Now.Subtract(datetime);
        }
        public static DateTime NextSunday(this DateTime datetime)
        {
            return new GregorianCalendar().AddDays(datetime, -((int)datetime.DayOfWeek) + 7);
        }
        public static double ElapsedSeconds(this DateTime datetime)
        {
            return datetime.TimeElapsed().TotalSeconds;
        }
        static public DateTime NextAnniversary(this DateTime datetime, DateTime eventDate, bool preserveMonth = false)
        {
            DateTime calcDate;

            if (datetime.Date < eventDate.Date) // Return the original event date if it occurs later than initial input date.
                return new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 0, 0, 0, datetime.Kind);

            calcDate = new DateTime(datetime.Year + (datetime.Month < eventDate.Month || datetime.Month == eventDate.Month && datetime.Day < eventDate.Day ? 0 : 1), eventDate.Month, 1, 0, 0, 0, datetime.Kind).AddDays(eventDate.Day - 1);

            if (eventDate.Month == calcDate.Month || !preserveMonth)
                return calcDate;
            else
                return calcDate.AddYears(datetime.Month == 2 && datetime.Day == 28 ? 1 : 0).AddDays(-1);
        }
        static public DateTime NextAnniversary(this DateTime datetime, int eventMonth, int eventDay, bool preserveMonth = false)
        {
            DateTime calcDate;

            if (eventDay > 31 || eventDay < 1 || eventMonth > 12 || eventMonth < 1 ||
               ((eventMonth == 4 || eventMonth == 6 || eventMonth == 9 || eventMonth == 11) && eventDay > 30) ||
               (eventMonth == 2 && eventDay > 29))
                throw new Exception("Invalid combination of Event Year and Event Month.");

            calcDate = new DateTime(datetime.Year + (datetime.Month < eventMonth || datetime.Month == eventMonth && datetime.Day < eventDay ? 0 : 1), eventMonth, 1, 0, 0, 0, datetime.Kind).AddDays(eventDay - 1);

            if (eventMonth == calcDate.Month || !preserveMonth)
                return calcDate;
            else
                return calcDate.AddYears(datetime.Month == 2 && datetime.Day == 28 ? 1 : 0).AddDays(-1);
        }
        public static int Age(this DateTime datetime)
        {
            return (int)Math.Floor((DateTime.Today - datetime).TotalDays / 365.25);
        }
        public static IEnumerable<DateTime> DateRange(this DateTime start, DateTime end)
        {
            int days = new TimeSpan(end.Ticks - start.Ticks).Days;
            return start.DateRange(days);
        }
        public static IEnumerable<DateTime> DateRange(this DateTime datetime, int days)
        {
            var range = Enumerable.Range(0, days);
            return from p in range
                   select datetime.Date.AddDays(p);
        }
        public static DateTime? ToDateTime(this string datetimeString)
        {
            DateTime datetime;
            return DateTime.TryParse(datetimeString, out datetime) ? datetime : new DateTime();
        }
        public static string ToFriendlyDateString(this DateTime datetime)
        {
            string FormattedDate = "";
            if (datetime.Date == DateTime.Today)
            {
                FormattedDate = "Today";
            }
            else if (datetime.Date == DateTime.Today.AddDays(-1))
            {
                FormattedDate = "Yesterday";
            }
            else if (datetime.Date > DateTime.Today.AddDays(-6))
            {
                // *** Show the Day of the week
                FormattedDate = datetime.ToString("dddd").ToString();
            }
            else
            {
                FormattedDate = datetime.ToString("MMMM dd, yyyy");
            }

            //append the time portion to the output
            FormattedDate += " @ " + datetime.ToString("t").ToLower();
            return FormattedDate;
        }
    }
}
