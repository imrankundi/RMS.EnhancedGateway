using RMS.Component.WebApi.Responses;

namespace RMS.Simulator.Logic
{
    public class TransactionAdviceManager
    {
        public IRequestListener RequestListener { get; set; }
        public TransactionAdviceManager(IRequestListener requestListener)
        {
            RequestListener = requestListener;
        }
        public BaseResponse PostTransactionAdvice(object request)
        {
            RequestListener?.NotifyRequest(request);
            BaseResponse response = new BaseResponse
            {
                ResponseStatus = ResponseStatus.Success
            };

            return response;
        }
    }
}
