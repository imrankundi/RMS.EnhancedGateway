using RMS.Component.Logging.Models;
using System.IO;

namespace RMS.Component.Logging
{

    public class LoggingFactory
    {
        public static ILog CreateLogger(string directory, string file, LogLevel level)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            file = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
            return new SerilogWrapper(file, level);
        }

        public static ILog CreateLogger(string directory, string file, string level)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            file = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
            return new SerilogWrapper(file, level);
        }

        #region Old Code

        //public static ILog CreateLogger(string directory, string file, LogLevel level)
        //{
        //    if (!Directory.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }

        //    file = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
        //    return new SerilogWrapper(file, level);
        //}

        //public static ILog CreateLogger(string directory, string file, string level)
        //{
        //    if (!Directory.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }

        //    file = string.Format(@"{0}\{1}", directory.TrimEnd('\\'), file);
        //    return new SerilogWrapper(file, level);
        //}

        #endregion
    }

    #region Comment Old Code
    //public class Logger
    //{
    //    private static Logger instance = new Logger();
    //    private const string logFileName = "EdsSslChannel.log";
    //    public ILog Log { get; set; }


    //    //private List<object> list = new List<object>();


    //    public static Logger Instance { get { return instance; } }

    //    private Logger()
    //    {
    //        try
    //        {
    //            //Log = LoggingFactory.CreateLogger(ConfigurationHelper.Instance.Configurations.LogPath, logFileName, ConfigurationHelper.Instance.Configurations.LogLevel);
    //        }
    //        catch (Exception ex)
    //        {
    //            Log = LoggingFactory.CreateLogger(@"C:\Cxp\Logs\", logFileName, LogLevel.Verbose);

    //        }
    //    }
    //}
    //public interface ILog
    //{
    //    void Debug(string className, string methodName, string text);
    //    void Information(string className, string methodName, string text);
    //    void Verbose(string className, string methodName, string text);
    //    void Warning(string className, string methodName, string text);
    //    void Error(string className, string methodName, string text);
    //    void Fatal(string className, string methodName, string text);
    //    void Information(string text);
    //}

    //public class SerilogWrapper : ILog
    //{
    //    private ILogger logger;
    //    private string file;

    //    private string format = "| {0,-30} | {1,-30} | {2,-30}";

    //    public SerilogWrapper(string file, LogLevel level)
    //    {
    //        this.file = file;
    //        logger = CreateLogger(level);
    //    }

    //    public SerilogWrapper(string file, string level)
    //    {
    //        this.file = file;
    //        logger = CreateLogger(level);
    //    }

    //    private ILogger CreateLogger(LogLevel level)
    //    {
    //        return CreateLogger(level.ToString());
    //    }

    //    private ILogger CreateLogger(string level)
    //    {
    //        switch (level.ToLower())
    //        {
    //            case "debug":
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Debug);
    //            case "error":
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Error);
    //            case "fatal":
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Fatal);
    //            case "information":
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Information);
    //            case "verbose":
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Verbose);
    //            case "warning":
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Warning);
    //            default:
    //                return CreateLogger(this.file, Serilog.Events.LogEventLevel.Verbose);

    //        }
    //    }

    //    private ILogger CreateLogger(string file, Serilog.Events.LogEventLevel level)
    //    {
    //        string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message}{NewLine}{Exception}";

    //        var log = new LoggerConfiguration()
    //            .MinimumLevel
    //            .Is(level)
    //            .WriteTo
    //            .RollingFile(file, retainedFileCountLimit: null, outputTemplate: outputTemplate)
    //            .CreateLogger();
    //        return log;
    //    }


    //    public void Debug(string className, string methodName, string text)
    //    {
    //        logger.Debug(string.Format(format, className, methodName, text));
    //    }

    //    public void Information(string className, string methodName, string text)
    //    {
    //        logger.Information(string.Format(format, className, methodName, text));
    //    }

    //    public void Verbose(string className, string methodName, string text)
    //    {
    //        logger.Verbose(string.Format(format, className, methodName, text));
    //    }

    //    public void Warning(string className, string methodName, string text)
    //    {
    //        logger.Warning(string.Format(format, className, methodName, text));
    //    }

    //    public void Error(string className, string methodName, string text)
    //    {
    //        logger.Error(string.Format(format, className, methodName, text));
    //    }

    //    public void Fatal(string className, string methodName, string text)
    //    {
    //        logger.Fatal(string.Format(format, className, methodName, text));
    //    }

    //    public void Information(string text)
    //    {
    //        logger.Information(text);
    //    }
    //}
    #endregion

}
