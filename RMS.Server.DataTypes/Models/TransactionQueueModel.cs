using System.Collections.Generic;

namespace RMS.Server.DataTypes
{
    public class TransactionQueueModel : BaseServerModel
    {
        public long QueueId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string AccountInfo { get; set; }
        public long QrId { get; set; }
        public string Token { get; set; }
        public Dictionary<string, object> AdditionalData { get; set; }

    }
}
