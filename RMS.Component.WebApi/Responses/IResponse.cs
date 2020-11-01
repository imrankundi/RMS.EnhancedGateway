namespace RMS.Component.WebApi.Responses
{
    public interface IResponse
    {
        ResponseType ResponseType { get; set; }
        string ToJson();
    }
}
