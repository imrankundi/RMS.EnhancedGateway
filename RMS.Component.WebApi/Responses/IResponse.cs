namespace RMS.Component.WebApi.Responses
{
    public interface IResponse
    {
        ResponseStatus ResponseStatus { get; set; }
        string ToJson();
    }
}
