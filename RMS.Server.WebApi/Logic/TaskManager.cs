using Newtonsoft.Json;
using RMS.Server.DataTypes.Quartz;
using RMS.Server.WebApi;
using System.Reflection;
using System.Threading.Tasks;

namespace RMS.Server.BusinessLogic
{
    public class TaskManager
    {
        protected static string ClassName => nameof(TaskManager);
        protected static string MethodName { get; private set; }
        private static string CreatePrefixFormatedName(string prefix, string name)
        {
            return string.Format("{0}_{1}", prefix, name);
        }
        public static async Task StartJobScheduler()
        {
            #region Logging
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodName = method.Name;
            #endregion

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Staring Job Schedular");
            await JobScheduler.Start();

            /*-----------------------------------------------------------------------------------------------------------------*/
            var resendFailedPacketJobName = CreatePrefixFormatedName("Job", "ResendFailedPacket");
            var resendFailedPacketTriggerName = CreatePrefixFormatedName("Trigger", "ResendFailedPacket");
            var resendFailedPacketJobDetail = JobDetailManager.CreateResendFailedPacketJob(resendFailedPacketJobName, JobDetailManager.Group);

            var resendFailedPacketTriggerDetail = new TriggerDetail
            {
                GroupName = JobDetailManager.Group,
                CronExpression = "0 0/30 0 ? * * *",
                JobName = resendFailedPacketJobName,
                TriggerName = resendFailedPacketTriggerName,
                TriggerType = TriggerType.Cron
            };

            WebServer.WebApiLogger?.Debug(ClassName, MethodName, string.Format("Trigger Details:\n{0}", JsonConvert.SerializeObject(resendFailedPacketTriggerDetail, Formatting.Indented)));

            var resendFailedPacketTrigger = TriggerFactory.CreateCronTrigger(resendFailedPacketTriggerDetail);

            WebServer.WebApiLogger?.Debug(ClassName, MethodName, string.Format("Trigger:\n{0}", JsonConvert.SerializeObject(resendFailedPacketTrigger, Formatting.Indented)));

            await JobScheduler.ScheduleJob(resendFailedPacketJobDetail, resendFailedPacketTrigger);
            /*-----------------------------------------------------------------------------------------------------------------*/


            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }

        public static async Task StopJobScheduler()
        {
            #region Logging
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodName = method.Name;
            #endregion

            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "Start");
            WebServer.WebApiLogger?.Information(ClassName, MethodName, "Stopping Job Schedular");
            await JobScheduler.Stop();
            WebServer.WebApiLogger?.Verbose(ClassName, MethodName, "End");
        }
    }
}
