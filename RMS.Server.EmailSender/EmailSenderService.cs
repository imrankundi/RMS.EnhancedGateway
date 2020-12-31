using Newtonsoft.Json;
using RMS.Component.Common.Helpers;
using RMS.Component.Logging;
using RMS.Component.Logging.Models;
using RMS.Server.DataTypes.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
                log = LoggingFactory.CreateLogger(@"C:\RMS\EmailSender", "EmailSender", LogLevel.Verbose);

                timer = new Timer();
                timer.Interval = 10 * 1000;
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
            var files = DirectoryHelper.FindFiles(@"C:\RMS\EmailSender", new string[] { "json" });
            foreach (var file in files)
            {
                try
                {
                    var text = FileHelper.ReadAllTextWithRetries(file);
                    var emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(text);
                    Console.WriteLine("Sending Email => {0}", file);
                    var emailSent = EmailManager.Instance.SendEmail(log, emailTemplate);
                    if(emailSent)
                    {
                        FileHelper.DeleteFileWithRetries(file);
                    }
                }
                catch (Exception ex)
                {
                    log?.Error(className, methodName, ex.ToString());
                }
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
