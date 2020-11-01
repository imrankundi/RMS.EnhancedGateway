using Newtonsoft.Json.Linq;
using RMS.Component.WebApi.Responses;
using RMS.Server.DataTypes;
using RMS.Server.DataTypes.Requests;
using RMS.Server.WebApi.Logging;
using System;

namespace RMS.Server.BusinessLogic
{
    public class RequestHandler
    {
        public static BaseResponse HandleRequest(object request, CommunicationContext context)
        {
            try
            {
                //SendToSimulator(request);
                var jsonObject = JObject.FromObject(request);
                Logger.Instance.Log.Debug("RequestHelper", "HandleRequest", jsonObject.ToString());
                Request req = jsonObject.ToObject<Request>();
                switch (req.RequestType)
                {
                    case ServerRequestType.Configuration:
                        return Configure(jsonObject.ToObject<ConfigurationRequest>(), context);
                    default:
                        return UnsupportedRequestType();
                }

            }
            catch (Exception ex)
            {
                BaseResponse response = new ErrorResponse
                {

                    ErrorDetails = ex.ToString(),
                    ErrorMessage = ex.Message,
                    Message = ex.Message,
                    ResponseType = ResponseType.Failed
                };

                return response;
            }
        }

        private static BaseResponse Configure(ConfigurationRequest request, CommunicationContext context)
        {
            BaseResponse response = new ErrorResponse
            {

                ErrorDetails = "Not Implemented",
                ErrorMessage = "Not Implemented",
                Message = "Not Implemented",
                ResponseType = ResponseType.Failed
            };
            return response;
        }

        private static BaseResponse CreateErrorResponse(Exception ex)
        {
            BaseResponse response = new ErrorResponse
            {

                ErrorDetails = ex.ToString(),
                ErrorMessage = ex.Message,
                Message = ex.Message,
                ResponseType = ResponseType.Failed
            };
            return response;
        }
        private static BaseResponse UnsupportedRequestType()
        {
            BaseResponse response = new ErrorResponse
            {

                ErrorDetails = "The api doesnot support this request type. Kindly contact sytem administrator.",
                ErrorMessage = "Unsupported request type",
                Message = "Unsupported request type",
                ResponseType = ResponseType.Failed
            };

            return response;
        }

    }
}
