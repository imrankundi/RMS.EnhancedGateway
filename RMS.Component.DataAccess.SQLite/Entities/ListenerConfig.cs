namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class ListenerApiConfig
    {
        public long Id { get; set; }
        public string AuthenticationType { get; set; }
        public string Name { get; set; }
        public int UploadInterval { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string XApiKey { get; set; }
        public string Region { get; set; }
        public string Service { get; set; }
        public string EndPointUri { get; set; }
        public int MaxRecordsPerHit { get; set; }
        public int MaxRecordsToFetch { get; set; }
        public int ParallelTcpConnections { get; set; }
        public int HttpTimeoutInSeconds { get; set; }
    }
}
