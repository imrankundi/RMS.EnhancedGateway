using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Component.Common
{
	public class Email
	{
		public string FromEmail { get; set; }
		public string FromName { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public List<string> To { get; set; }
		public List<string> CC { get; set; }
		public List<string> BCC { get; set; }
		public List<Attachment> Attachments { get; set; }

		public Email()
		{
			To = new List<string>();
			CC = new List<string>();
			BCC = new List<string>();
			Attachments = new List<Attachment>();
		}
	}
}
