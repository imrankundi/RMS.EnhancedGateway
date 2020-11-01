namespace RMS.Core.Common
{
    public sealed class DateTimeFormatValue
    {
        public DateTimeFormatValue()
        {

        }
        public DateTimeFormatValue(string datetime, string date, string time)
        {
            DateTime = datetime;
            Date = date;
            Time = time;
        }
        public string DateTime { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
    public sealed class DateTimeFormat
    {
        public static DateTimeFormatValue ISO8601WithoutDelimeter = new DateTimeFormatValue
        {
            DateTime = "yyyyMMddHHmmss",
            Date = "yyyyMMdd",
            Time = "HHmmss"
        };

        public static DateTimeFormatValue ISO8601WithDelimeter = new DateTimeFormatValue
        {
            DateTime = "yyyy-MM-dd HH:mm:ss",
            Date = "yyyy-MM-dd",
            Time = "HH:mm:ss"
        };

        public static DateTimeFormatValue UK = new DateTimeFormatValue
        {
            DateTime = "dd/MM/yyyy HH:mm:ss",
            Date = "dd/MM/yyyy",
            Time = "HH:mm:ss"
        };

        public static DateTimeFormatValue US = new DateTimeFormatValue
        {
            DateTime = "MM/dd/yyyy HH:mm:ss",
            Date = "MM/dd/yyyy",
            Time = "HH:mm:ss"
        };

    }
}
