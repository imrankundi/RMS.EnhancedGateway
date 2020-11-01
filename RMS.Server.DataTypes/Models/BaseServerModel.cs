namespace RMS.Server.DataTypes
{
    public class BaseServerModel
    {
        public bool HasError { get; set; }
        public ServerErrorModel ErrorDetails { get; set; }
    }
}
