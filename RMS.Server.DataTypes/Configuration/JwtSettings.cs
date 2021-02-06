namespace RMS.Server.DataTypes
{
    public class JwtSettings
    {
        public int Id { get; set; }
        public string SigningKey { get; set; }
        public string Issuer { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
