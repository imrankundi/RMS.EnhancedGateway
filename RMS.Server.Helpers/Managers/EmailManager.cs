using Newtonsoft.Json;
using RMS.Component.Common;
using RMS.Component.Common.Helpers;
using RMS.Component.DataAccess.SQLite.Repositories;
using RMS.Component.Logging;
using RMS.Server.DataTypes.Email;
using RMS.Server.DataTypes.WindowsService;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RMS.Server.EmailSender
{
    public class EmailManager
    {
        public const string NO_CHANNEL_CONNECTED = nameof(NO_CHANNEL_CONNECTED);
        public const string SERVICE_STATUS = nameof(SERVICE_STATUS);

        static string className = nameof(EmailManager);
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
        public EmailServiceConfiguration Configurations { get; private set; }
        private EmailServiceConfiguration LoadConfiguration()
        {
            var repo = new EmailConfigRepository();
            var config = repo.ReadEmailServiceConfiguration();
            var configuration = Component.Mappers.ConfigurationMapper.Map(config);
            return configuration;
        }

        public bool SendEmail(ILog log, EmailFileTemplate emailTemplate)
        {

            bool result = false;
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                log?.Information(className, methodName, string.Format("Configuration:\n{0}", JsonConvert.SerializeObject(Configurations, Formatting.Indented)));
                log?.Information(className, methodName, string.Format("EmailTemplate:\n{0}", JsonConvert.SerializeObject(emailTemplate, Formatting.Indented)));
                EmailHelper emailHelper = new EmailHelper(Configurations.SmtpSettings);
                emailHelper.Log = log;
                result = emailHelper.Initialize();
                if (result)
                {
                    Email email = new Email();
                    email.FromEmail = Configurations.SmtpSettings.EmailAddress;
                    email.FromName = Configurations.SmtpSettings.Name;
                    email.IsHtml = emailTemplate.IsHtml;
                    if (emailTemplate.ToEmailAddresses != null)
                    {
                        if (emailTemplate.ToEmailAddresses.Count > 0)
                        {
                            email.To.AddRange(emailTemplate.ToEmailAddresses);
                        }
                    }

                    if (emailTemplate.BccEmailAddresses != null)
                    {
                        if (emailTemplate.BccEmailAddresses.Count > 0)
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
        public static bool CreateNoClientSocketConnectedEmail(ILog log = null)
        {
            bool result = false;
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var repo = new EmailConfigRepository();
                var template = repo.GetEmailTemplate(NO_CHANNEL_CONNECTED);
                var list = new List<string>();
                if (template != null)
                {
                    if (template.Subscribers != null)
                    {
                        foreach (var subscriber in template.Subscribers)
                        {
                            if (!string.IsNullOrEmpty(subscriber.EmailAddress))
                            {
                                list.Add(subscriber.EmailAddress);
                            }
                        }
                    }

                    EmailFileTemplate fileTemplate = new EmailFileTemplate();
                    fileTemplate.EmailSubject = template.TemplateSubject;
                    fileTemplate.EmailMessage = template.TemplateMessage.Replace("\\n", "\n")
                        .Replace("{{DATETIME}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
                    fileTemplate.BccEmailAddresses = list;

                    if (fileTemplate != null)
                    {
                        var json = JsonConvert.SerializeObject(fileTemplate);
                        var file = string.Format(@"{0}\Gateway_{1}.json", EmailManager.Instance.Configurations.EmailFolderPath,
                            DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

                        FileHelper.WriteAllText(file, json);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
                result = false;
            }

            return result;
        }
        public static bool CreateServiceStatusEmail(ServiceInfo info, ILog log = null)
        {
            bool result = false;
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var repo = new EmailConfigRepository();
                var template = repo.GetEmailTemplate(SERVICE_STATUS);
                var list = new List<string>();
                if (template != null)
                {
                    if (template.Subscribers != null)
                    {
                        foreach (var subscriber in template.Subscribers)
                        {
                            if (!string.IsNullOrEmpty(subscriber.EmailAddress))
                            {
                                list.Add(subscriber.EmailAddress);
                            }
                        }
                    }
                    EmailFileTemplate fileTemplate = new EmailFileTemplate();

                    fileTemplate.EmailSubject = template.TemplateSubject
                        .Replace("{{SERVICE_NAME}}", info.ServiceName)
                        .Replace("{{SERVICE_STATUS}}", info.ServiceStatus.ToString());

                    fileTemplate.EmailMessage = template.TemplateMessage.Replace("\\n", "\n")
                        .Replace("{{DATETIME}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"))
                        .Replace("{{SERVICE_NAME}}", info.ServiceName)
                        .Replace("{{SERVICE_STATUS}}", info.ServiceStatus.ToString());

                    fileTemplate.BccEmailAddresses = list;

                    if (fileTemplate != null)
                    {
                        var json = JsonConvert.SerializeObject(fileTemplate);
                        var file = string.Format(@"{0}\Gateway_{1}.json", EmailManager.Instance.Configurations.EmailFolderPath,
                            DateTime.UtcNow.ToString("yyyyMMddHHmmss"));

                        FileHelper.WriteAllText(file, json);
                        result = true;
                    }
                }
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
