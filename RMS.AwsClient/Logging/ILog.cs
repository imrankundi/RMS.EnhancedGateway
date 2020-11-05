namespace RMS.AWS.Logging
{
    public interface ILog
    {
        void Write(string id, string text);
    }
}
