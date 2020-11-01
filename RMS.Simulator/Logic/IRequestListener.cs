namespace RMS.Simulator.Logic
{
    public interface IRequestListener
    {
        IRequestHandler GetRequestHandler();
        void NotifyRequest(object request);
    }
}
