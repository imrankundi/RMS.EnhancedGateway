using RMS.Component.WebApi.Requests;

namespace RMS.Simulator.Requests
{
    public class Request : BaseRequest
    {
        public SimulatorRequestType RequestType { get; set; }
    }
}
