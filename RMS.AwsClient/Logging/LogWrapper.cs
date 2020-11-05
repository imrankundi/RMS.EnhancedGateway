namespace RMS.AWS.Logging
{
    public class LogWrapper : ILog
    {
        private ILogger logger;
        private string format = "{0}|{1}\n";

        public LogWrapper(string directory, string fileName)
        {
            logger = CreateLogger(directory, fileName);
        }


        private ILogger CreateLogger(string directory, string file)
        {
            return BaseLogger.CreateLogger(directory, file);
        }


        public void Write(string id, string text)
        {
            string content = string.Format(format, id, text);
            logger.Write(content);
        }

    }
}
