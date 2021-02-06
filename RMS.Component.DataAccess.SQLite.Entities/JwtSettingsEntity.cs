namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class JwtSettingsEntity
    {
        public int Id { get; set; }
        public string SigningKey { get; set; }
        public string Issuer { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
