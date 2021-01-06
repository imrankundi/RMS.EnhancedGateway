using Newtonsoft.Json;
using RMS.Component.Common.Helpers;
using RMS.Component.Logging;
using RMS.Server.DataTypes.Email;
using System;
using System.Reflection;
using System.Timers;

namespace RMS.Server.EmailSender
{
    public class EmailSenderService
    {
        string className = nameof(EmailSenderService);
        ILog log;
        Timer timer;

        public void Start()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                log = LoggingFactory.CreateLogger(EmailManager.Instance.Configurations.LogPath,
                    "EmailSender", EmailManager.Instance.Configurations.LogLevel);

                timer = new Timer();
                timer.Interval = EmailManager.Instance.Configurations.FolderMonitoringIntervalInSeconds * 1000;
                timer.Elapsed += Timer_Elapsed;

                timer.Start();
            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
            }

        }

        private void FindFiles()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                var files = DirectoryHelper.FindFiles(EmailManager.Instance.Configurations.EmailFolderPath,
                    new string[] { "json" });
                foreach (var file in files)
                {
                    try
                    {
                        var text = FileHelper.ReadAllTextWithRetries(file);
                        var emailTemplate = JsonConvert.DeserializeObject<EmailFileTemplate>(text);
                        Console.WriteLine("Sending Email => {0}", file);
                        var emailSent = EmailManager.Instance.SendEmail(log, emailTemplate);
                        if (emailSent)
                        {
                            FileHelper.DeleteFileWithRetries(file);
                        }
                        log?.Information(className, methodName, string.Format("Email Sent: {0}", emailSent));
                    }
                    catch (Exception ex)
                    {
                        log?.Error(className, methodName, ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            try
            {
                //log?.Verbose(className, methodName, "Sending Email");
                timer.Enabled = false;

                FindFiles();
            }
            catch (Exception ex)
            {
                log?.Error(className, methodName, ex.ToString());
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        public void Stop()
        {


        }
    }
}
