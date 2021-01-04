namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class MontioringParameterConfig
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public int ServiceState { get; set; }
        public bool TakeActionOnStop { get; set; }
        public bool TakeActionOnStart { get; set; }
        public bool TakeActionOnNotInstalled { get; set; }
        public int LastActionTaken { get; set; }
    }
}
