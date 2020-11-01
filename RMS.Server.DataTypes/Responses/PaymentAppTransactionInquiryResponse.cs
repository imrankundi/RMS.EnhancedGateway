using Newtonsoft.Json;
using RMS.Component.WebApi.Responses;
using System.Collections.Generic;

namespace RMS.Server.DataTypes.Responses
{
    public class PaymentAppTransactionInquiryResponse : BaseResponse
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
        [JsonProperty("qrCode")]
        public string QrCode { get; set; }
        [JsonProperty("transactionName")]
        public string TransactionName { get; set; }
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("accountInfo")]
        public string AccountInfo { get; set; }
        [JsonProperty("secureToken")]
        public string SecureToken { get; set; }
        [JsonProperty("additionalData")]
        public Dictionary<string, object> AdditionalData { get; set; }
    }
}
