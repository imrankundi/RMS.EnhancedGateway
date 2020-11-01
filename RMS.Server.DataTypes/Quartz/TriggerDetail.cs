namespace RMS.Server.DataTypes.Quartz
{
    public class TriggerDetail
    {
        public string GroupName { get; set; }
        public string TriggerName { get; set; }
        public string JobName { get; set; }
        public int Interval { get; set; }
        public int JobId { get; set; }
        public TriggerType TriggerType { get; set; }
        public IntervalType IntervalType { get; set; }
        public string CronExpression { get; set; }

    }

}
