using RMS.Component.WebApi.Responses;
using RMS.Simulator.Logic;
using System;

namespace RMS.Simulator.Requests
{
    public class RequestHandler : IRequestHandler
    {
        public IRequestListener RequestListener { get; set; }
        public RequestHandler(IRequestListener requestListener)
        {
            RequestListener = requestListener;
        }
        public BaseResponse HandleRequest(object request)
        {
            try
            {

                RequestListener?.NotifyRequest(request);
                BaseResponse response = new BaseResponse
                {
                    ResponseStatus = ResponseStatus.Success
                };
                return response;

            }
            catch (Exception ex)
            {
                BaseResponse response = new ErrorResponse
                {
                    ErrorDetails = ex.ToString(),
                    ErrorMessage = ex.Message,
                    Message = ex.Message,
                    ResponseStatus = ResponseStatus.Failed
                };
                return response;
            }
        }


    }
}
