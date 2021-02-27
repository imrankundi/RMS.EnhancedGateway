using Newtonsoft.Json;
using Quartz;
using RMS.AWS;
using RMS.Component.Common;
using RMS.Component.Common.Helpers;
using RMS.Component.Communication.Tcp.Server;
using RMS.Component.DataAccess.SQLite.Entities;
using RMS.Server.WebApi;
using RMS.Server.WebApi.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RMS.Jobs
{
    public class ResendFailedPacketJob : IJob
    {
        protected string ClassName => nameof(ResendFailedPacketJob);
        protected string MethodName { get; private set; }
        public async Task Execute(IJobExecutionContext context)
        {
            #region Logging
            MethodName = nameof(Execute);
            #endregion
            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");

            try
            {
                WebServer.WebApiLogger?.Information(ClassName, MethodName, "Resend Failed Packet Job Triggerred");
                WebServer.WebApiLogger?.Verbose(ClassName, MethodName, string.Format("Pausing Job, JobKey: {0}", context.Trigger.JobKey));
                await context.Scheduler.PauseJob(context.Trigger.JobKey);

                string directory = string.Format(@"{0}\Logs\Failed\", AppDirectory.BaseDirectory);
                string oldestDirectory = FileHelper.GetOldestDirectory(directory);
                if (!string.IsNullOrEmpty(oldestDirectory))
                {
                    var fileSystemInfoList = FileHelper.GetFilesOrderByCreationDateAscending(oldestDirectory, "*.log");
                    if (fileSystemInfoList != null && fileSystemInfoList.Count() > 0)
                    {
                        foreach (var fileSystemInfo in fileSystemInfoList)
                        {
                            if (fileSystemInfo.CreationTime < DateTime.Now.Subtract(new TimeSpan(1, 0, 0)))
                            {
                                PushToServer(fileSystemInfo);
                            }
                        }
                    }
                    else
                    {
                        FileHelper.DeleteDirectory(oldestDirectory);
                    }

                }

                WebServer.WebApiLogger?.Verbose(ClassName, MethodName, string.Format("Job Paused, JobKye: {0}", context.Trigger.JobKey));


            }
            catch (Exception ex)
            {
                WebServer.WebApiLogger?.Information(ClassName, MethodName, ex.ToString());
            }
            finally
            {
                WebServer.WebApiLogger?.Verbose(ClassName, MethodName, string.Format("Resuming Job, JobKey: {0}", context.Trigger.JobKey));
                await context.Scheduler.ResumeJob(context.Trigger.JobKey);
                WebServer.WebApiLogger?.Verbose(ClassName, MethodName, string.Format("Job Resumed, JobKye: {0}", context.Trigger.JobKey));
            }

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }
        public void PushToServer(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo != null)
            {
                var newFileName = string.Format("{0}.working", fileSystemInfo.FullName);

                bool fileRenamed = FileHelper.RenameFile(fileSystemInfo.FullName, string.Format("{0}.working", fileSystemInfo.FullName));
                if (fileRenamed)
                {
                    var text = FileHelper.ReadAllTextWithRetries(newFileName);
                    text = "[" + text.TrimEnd(',') + "]";
                    var packets = JsonConvert.DeserializeObject<IEnumerable<PushApiEntity>>(text);
                    PushToServer(packets);
                }
            }
        }
        private void PushToServer(IEnumerable<PushApiEntity> packets)
        {
            string MethodName = nameof(PushToServer);
            try
            {
                var listeners = ServerChannelConfigurationManager.Instance.Configurations.Listeners;

                foreach (var listener in listeners)
                {
                    try
                    {
                        AWS4Client client = new AWS4Client(listener);

                        int logLevel = (int)WebApiServerConfigurationManager.Instance.Configurations.LogLevel;

                        client.LogPacketOnFailure = true;

                        if (logLevel >= (int)Component.Logging.Models.LogLevel.Debug)
                            client.LogPacketOnSuccess = false;

                        foreach (var packet in packets)
                        {
                            try
                            {
                                var res = client.PostData(packet.Request);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WebServer.WebApiLogger?.Error(ClassName, MethodName, ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                WebServer.WebApiLogger?.Error(ClassName, MethodName, ex.ToString());
            }

        }
    }
}

