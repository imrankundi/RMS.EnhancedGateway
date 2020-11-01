namespace RMS.Server.WebApi.Common
{
    public static class Constant
    {
        public static class Compnay
        {
            public const string Name = "NCR Pakistan";
        }
        public static class Product
        {
            public const string ServiceName = "NCR TMCW Server Service";
            public const string ServiceDescription = "NCR TMCW Server Service";
            public const string Name = "NCR TMCW Server Service";
            public const string Version = "1.0.0.2";
            public static string Information => string.Format("{0} ver {1}", Name, Version);
        }

        public static class Headers
        {
            public const string Product = nameof(Product);
            public const string MachineName = nameof(MachineName);
            public const string CompanyName = nameof(CompanyName);
        }
    }
}
