
using RMS.Component.WebApi.Responses;

namespace RMS
{
    public interface IResponseHandler
    {
        void OnResponseReceived(BaseResponse response);
    }
}
