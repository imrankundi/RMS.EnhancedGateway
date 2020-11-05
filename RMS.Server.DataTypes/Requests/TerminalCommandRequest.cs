namespace RMS.Server.DataTypes.Requests
{
    public class TerminalCommandRequest : Request
    {
        public TerminalCommandRequest()
        {
            RequestType = ServerRequestType.TerminalCommand;
        }

        public string TerminalId { get; set; }
        public string Data { get; set; }
        //public IList<string> Packets { get; set; }
    }
}
