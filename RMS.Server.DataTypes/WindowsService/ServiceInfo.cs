namespace RMS.Server.DataTypes.WindowsService
{
    public class ServiceInfo
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public ServiceStatus ServiceStatus { get; set; }
    }
}
