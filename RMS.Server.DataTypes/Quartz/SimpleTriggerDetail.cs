namespace RMS.Server.DataTypes.Quartz
{
    public class SimpleTriggerDetail
    {
        public string GroupName { get; set; }
        public string TriggerName { get; set; }
        public string JobName { get; set; }
        public int Interval { get; set; }
        public IntervalType IntervalType { get; set; }
    }

}
