using System;

namespace RMS.AWS
{
    public class ServerInfo : IEquatable<ServerInfo>
    {
        public int Id { get; set; }
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
        public int ParallelTcpConn { get; set; }
        public int HttpTimeoutSecs { get; set; }

        public bool Equals(ServerInfo other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
