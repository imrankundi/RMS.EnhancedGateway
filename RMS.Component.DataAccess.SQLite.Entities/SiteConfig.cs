namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class SiteConfig
    {
        public int Id { get; set; }
        public string TerminalId { get; set; }
        public string Name { get; set; }
        public TimeOffsetConfig TimeOffset { get; set; }
    }
}
