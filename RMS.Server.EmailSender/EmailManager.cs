using Newtonsoft.Json;
using RMS.Component.Common;
using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Component.Logging;
using RMS.Server.DataTypes.Email;
using System;
using System.Reflection;

namespace RMS.Server.EmailSender
{
    public class EmailManager
    {
        string className = nameof(EmailManager);
        private EmailManager()
        {
            Configurations = LoadConfiguration();
            if (Configurations == null)
                IsConfigurationLoaded = false;
            else
                IsConfigurationLoaded = true;


            
        }
        static EmailManager() { }
        public static EmailManager Instance { get; } = new EmailManager();
        public bool IsConfigurationLoaded { get; } = false;
        public SmtpSettings Configurations { get; private set; }
        private SmtpSettings LoadConfiguration()
        {
            var repo = new EmailConfigRepository();
            var config = repo.ReadSmtpConfiguration();
            var configuration = Component.Mappers.ConfigurationMapper.Map(config);
            return configuration;
        }

        public bool SendEmail(ILog log, EmailTemplate emailTemplate)
        {
            
            bool result = false;
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                log?.Information(className, methodName, string.Format("Configuration:\n{0}", JsonConvert.SerializeObject(Configurations, Formatting.Indented)));
                EmailHelper emailHelper = new EmailHelper(Configurations);
                emailHelper.Log = log;
                result = emailHelper.Initialize();
                if(result)
                {
                    Email email = new Email();
                    email.FromEmail = Configurations.EmailAddress;
                    email.FromName = Configurations.Name;
                    email.To.AddRange(emailTemplate.ToEmailAddresses);
                    if(emailTemplate.BccEmailAddresses != null)
                    {
                        if(emailTemplate.BccEmailAddresses.Count > 0)
                        {
                            email.BCC.AddRange(emailTemplate.BccEmailAddresses);
                        }
                    }
                    if (emailTemplate.CcEmailAddresses != null)
                    {
                        if (emailTemplate.CcEmailAddresses.Count > 0)
                        {
                            email.BCC.AddRange(emailTemplate.CcEmailAddresses);
                        }
                    }
                    email.Body = emailTemplate.EmailMessage;
                    email.Subject = emailTemplate.EmailSubject;
                    result = emailHelper.SendEmail(email);
                }
                emailHelper.Dispose();
                
            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
                result = false;
            }
            return result;
        }
    }
}
