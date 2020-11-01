namespace RMS.Component.Logging
{
    public interface ILog
    {
        void Debug(string className, string methodName, string text);
        void Information(string className, string methodName, string text);
        void Verbose(string className, string methodName, string text);
        void Warning(string className, string methodName, string text);
        void Error(string className, string methodName, string text);
        void Fatal(string className, string methodName, string text);
        void Information(string text);
    }
}
