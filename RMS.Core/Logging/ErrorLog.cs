using System;
using System.Text;

namespace RMS.Core.Logging
{
    public class ErrorLog : Log
    {
        public ErrorLog(Exception exception)
        {
            this.Message = exception.Message;
            this.Detail = ErrorDetail(exception);
            this.LogType = LogType.Error;
        }

        private string ErrorDetail(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Inner Exception:");
            sb.AppendLine(exception.InnerException == null ? "null" :
                exception.InnerException.ToString());
            sb.AppendLine(Separator);
            sb.AppendLine("Stack Trace:");
            sb.AppendLine(exception.StackTrace);
            sb.AppendLine(Separator);
            return sb.ToString();
        }


    }
}
