using RMS.Component.RestHelper;
using RMS.Server.DataTypes.Requests;
using RMS.Server.DataTypes.Responses;
using System.Threading.Tasks;

namespace RMS
{
    public class RequestManager
    {
        public static async Task PerformTerminalCommandRequest(IResponseHandler handler, TerminalCommandRequest request)
        {
            RestClientFactory factory = new RestClientFactory("RMS");
            var response = await factory.PostCallAsync<TerminalCommandResponse, TerminalCommandRequest>
                (factory.apiConfiguration.Apis["TerminalCommand"], request);

            handler.OnResponseReceived(response);
        }
    }
}
