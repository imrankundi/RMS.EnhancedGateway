namespace RMS.Component.Communication.Logging
{
    public class LogWrapper : ILog
    {
        private ILogger logger;
        private string format = "{0},\n";

        public LogWrapper(string directory, string fileName)
        {
            logger = CreateLogger(directory, fileName);
        }


        private ILogger CreateLogger(string directory, string file)
        {
            return BaseLogger.CreateLogger(directory, file);
        }


        public void Write(string text)
        {
            string content = string.Format(format, text);
            logger.Write(content);
        }

    }
}
