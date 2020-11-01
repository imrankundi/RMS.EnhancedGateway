namespace RMS.Server.WebApi
{
    class Program
    {
        static readonly string className = nameof(Program);
        static void Main()
        {

            ConfigureService.Configure();
        }
    }
}
