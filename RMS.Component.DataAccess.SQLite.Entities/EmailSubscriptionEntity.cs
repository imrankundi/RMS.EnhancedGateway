namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class EmailSubscriptionEntity
    {
        public long Id { get; set; }
        public string SubscriberName { get; set; }
        public string EmailAddress { get; set; }
    }
}
