namespace RMS.Core.Logging
{
    public class LoggingFactory
    {

        private const string ErrorLog = nameof(ErrorLog);
        private const string InfoLog = nameof(InfoLog);

        public static Logger CreateLogger(string directory)
        {
            return new Logger(directory);
        }

        public static Logger CreateErrorLog()
        {
            return CreateLogger(ErrorLog);
        }

        public static Logger CreateInfoLog()
        {
            return CreateLogger(InfoLog);
        }
    }
}
