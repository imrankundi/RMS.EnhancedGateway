using Common.Logging;
using Common.Logging.Simple;

namespace RMS.Server.WebApi
{
    class Program
    {
        static readonly string className = nameof(Program);
        static void Main()
        {

            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter { Level = LogLevel.Info };
            ConfigureService.Configure();
            //
        }
    }
}
