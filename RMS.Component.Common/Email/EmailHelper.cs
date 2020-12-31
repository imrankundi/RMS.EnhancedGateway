using RMS.Component.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Reflection;

namespace RMS.Component.Common
{
	public class EmailHelper
	{
		private SmtpClient smtp;
		private SmtpSettings settings;
		private string className = nameof(EmailHelper);

        public ILog Log { get; set; }

        public EmailHelper(SmtpSettings settings)
		{
			this.settings = settings;
		}

		public bool IsInitialized { get; private set; }
		public bool Initialize()
		{
			string methoName = MethodBase.GetCurrentMethod().Name;
			try
			{
				smtp = new SmtpClient();
                
               
                smtp.Host = settings.Host;
				smtp.Port = settings.Port;
				smtp.EnableSsl = settings.Ssl;
                smtp.Timeout = 20 * 1000;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
				if (settings.UseCredential)
					smtp.Credentials = new System.Net.NetworkCredential(settings.EmailAddress, settings.Password);


				IsInitialized = true;
			}
			catch (Exception ex)
			{
				Log?.Error(className, methoName, ex.ToString());
                IsInitialized = false;
			}

			return IsInitialized;
		}

		private void AddToEmailTo(MailMessage mail, IEnumerable<string> list)
		{
			if (list != null)
			{
				foreach (string s in list)
				{
					mail.To.Add(s);
				}
			}
		}

		private void AddToEmailBcc(MailMessage mail, IEnumerable<string> list)
		{
			if (list != null)
			{
				foreach (string s in list)
				{
					mail.Bcc.Add(s);
				}
			}
		}

		private void AddToEmailCC(MailMessage mail, IEnumerable<string> list)
		{
			if (list != null)
			{
				foreach (string s in list)
				{
					mail.CC.Add(s);
				}
			}
		}

		public bool SendEmail(Email email)
		{
			string methoName = MethodBase.GetCurrentMethod().Name;
			bool isSent = false;
			try
			{
				using (MailMessage mail = new MailMessage())
				{
					mail.Subject = email.Subject;
					mail.Body = email.Body;
					mail.From = new MailAddress(email.FromEmail, email.FromName);
					AddToEmailTo(mail, email.To);
					AddToEmailCC(mail, email.CC);
					AddToEmailBcc(mail, email.BCC);
					smtp.Send(mail);
					isSent = true;
				}
				
			}
			catch(Exception ex)
			{
				Log?.Error(className, methoName, ex.ToString());
				isSent = false;
			}
			
			return isSent;
		}

		public void Dispose()
		{
			if (smtp == null)
				return;

			smtp.Dispose();
		}

	}
}