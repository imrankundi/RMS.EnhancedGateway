using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using RMS.Server.WebApi;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace RMS.Server.BusinessLogic
{
    public class JobScheduler
    {
        static NameValueCollection props = new NameValueCollection
        {
            { "quartz.serializer.type", "binary" }
        };
        static StdSchedulerFactory factory = new StdSchedulerFactory(props);
        static IScheduler scheduler;

        protected static string ClassName => nameof(JobScheduler);
        protected static string MethodName { get; private set; }
        public static async Task ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            #region Logging
            MethodName = nameof(ScheduleJob);
            #endregion

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            try
            {
                WebServer.WebApiLogger?.Verbose(ClassName, MethodName, string.Format("Job Detail:\n{0}", JsonConvert.SerializeObject(jobDetail, Formatting.Indented)));
                WebServer.WebApiLogger?.Verbose(ClassName, MethodName, string.Format("Trigger:\n{0}", JsonConvert.SerializeObject(trigger, Formatting.Indented)));
                WebServer.WebApiLogger?.Information(ClassName, MethodName, "Scheduling Job");
                await scheduler.ScheduleJob(jobDetail, trigger);
                WebServer.WebApiLogger?.Information(ClassName, MethodName, "Job Sechduled");
            }
            catch (Exception ex)
            {
                WebServer.WebApiLogger?.Error(ClassName, MethodName, ex.ToString());
            }

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }

        private static async Task<ITrigger> FindExistingTrigger(TriggerKey key)
        {
            #region Logging
            MethodName = nameof(FindExistingTrigger);
            #endregion

            var trigger = await scheduler.GetTrigger(key);
            return trigger;
        }

        public static async Task Start()
        {
            #region Logging
            MethodName = nameof(Start);
            #endregion

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Getting scheduler from SchedulerFactory");
            scheduler = await factory.GetScheduler();
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Starting Schedular");
            await scheduler.Start();
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Schedular Started");
            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }
        public static async Task Stop()
        {
            #region Logging
            MethodName = nameof(Stop);
            #endregion

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Shutting down Schedular");
            await scheduler.Shutdown();
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Schedular Shutdown");
            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }

        public static async Task UnscheduleJob(TriggerKey triggerKey)
        {
            #region Logging
            MethodName = nameof(UnscheduleJob);
            #endregion

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Unscheduling Job");
            await scheduler.UnscheduleJob(triggerKey);
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Job Unscheduled");
            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }
    }
}
