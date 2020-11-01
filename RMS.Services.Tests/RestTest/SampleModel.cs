using RMS.Component.RestHelper;
using System.Threading.Tasks;

namespace RMS.Services.Tests.RestTest
{
    public class SampleModel
    {
        public static async Task<SampleResponse> StartTouchlessTransaction(SampleRequest request)
        {
            SampleResponse response = new SampleResponse();
            RestClientFactory client = new RestClientFactory("AgentApi");

            response = await client.PostCallAsync<SampleResponse, SampleRequest>(client.apiConfiguration.Apis["StartTouchlessTransaction"],
                request);

            return response;
        }
    }

    public class SampleRequest
    {
        public int RequestType { get; set; }
    }

    public class SampleResponse
    {
        public int ResponseType { get; set; }
        public string Message { get; set; }
    }
}
