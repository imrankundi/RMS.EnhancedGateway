using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Component.Common
{
	public class SmtpSettings
	{
		public long Id { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string EmailAddress { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public bool Ssl { get; set; }
		public bool UseCredential { get; set; }
	}
}
