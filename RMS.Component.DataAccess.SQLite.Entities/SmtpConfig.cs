namespace RMS.Component.DataAccess.SQLite.Entities
{
    public class SmtpConfig
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public bool Ssl { get; set; }
		public bool UseCredential { get; set; }
	}
}
