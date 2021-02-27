using System;
using System.Net;

namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class PushApiEntity
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int ServerId { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}
