using RMS.Core.Common;
using System;
using System.Text;

namespace RMS.Core.Logging
{
    public class Log
    {
        private string separator = "------------------------------------------------------";

        public string Separator => separator;

        public DateTime GeneratedOn { get; private set; }
        public LogType LogType { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }

        public Log()
        {
            this.GeneratedOn = DateTimeHelper.CurrentUniversalTime;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("Type:");
            sb.AppendLine(LogType.ToString());
            sb.AppendLine(separator);
            sb.Append("DateTime:");
            sb.AppendLine(GeneratedOn.ToLongTimeString());
            sb.AppendLine(separator);
            sb.Append("Message:");
            sb.AppendLine(Message);
            sb.AppendLine(separator);
            sb.Append("Detail:");
            sb.AppendLine(Detail);
            sb.AppendLine(separator);
            return sb.ToString();
        }
    }
}
