using Newtonsoft.Json.Linq;
using RMS.Server.DataTypes.Requests;
using System;
using System.Collections.Concurrent;

namespace RMS.Server.WebApi
{
    [Obsolete("This method is no longer in use")]
    public class TcpRequestManager
    {
        private static ConcurrentDictionary<string, TcpRequestContext> requests = new ConcurrentDictionary<string, TcpRequestContext>();
        public static void AddRequest(object request)
        {
            var jsonObject = JObject.FromObject(request);
            Request req = jsonObject.ToObject<Request>();
            if (requests.ContainsKey(req.RequestId))
            {
                CheckAndAddValidRequest(req);
            }
            else
            {
                requests.TryAdd(req.RequestId, new TcpRequestContext
                {
                    Request = req,
                    IsResponseReceived = false
                });
            }
        }

        public static void RemoveRequest(Request request)
        {
            if (requests.ContainsKey(request.RequestId))
            {
                requests.TryRemove(request.RequestId, out TcpRequestContext context);
            }
        }

        public static TcpRequestContext FindRequestContext(string requestId)
        {
            if (requests.ContainsKey(requestId))
            {
                return requests[requestId];
            }
            return null;
        }
        private static void CheckAndAddValidRequest(Request request)
        {

        }
    }
}
