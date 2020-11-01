namespace RMS.Server.DataTypes.Quartz
{

    public class CronTriggerDetail
    {
        public string GroupName { get; set; }
        public string TriggerName { get; set; }
        public string JobName { get; set; }
        public string TaskId { get; set; }
        public string CronExpression { get; set; }

    }

}
