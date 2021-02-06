namespace RMS.Server.WebApi.Common
{
    public static class Constant
    {
        public static class Compnay
        {
            public const string Name = "SalTec Powerlink";
        }
        public static class Product
        {
            public const string ServiceName = "SalTec Powerlink Redirect Gateway";
            public const string ServiceDescription = "SalTec Powerlink Redirect Gateway";
            public const string Name = "Redirect Gateway";
            public const string Version = "2.0.0.1";
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
