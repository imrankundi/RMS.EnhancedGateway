using RMS.Server.DataTypes.Quartz;
using System.Threading.Tasks;

namespace RMS.Server.BusinessLogic
{
    public class TaskManager
    {

        private static string CreatePrefixFormatedName(string prefix, string name)
        {
            return string.Format("{0}_{1}", prefix, name);
        }
        public static async Task StartJobScheduler()
        {

            await JobScheduler.Start();
            var queueJobName = CreatePrefixFormatedName("Job", "Queue");
            var queueJobTrigger = CreatePrefixFormatedName("Trigger", "Queue");
            var queueJobDetail = JobDetailManager.CreateQueueJob(queueJobName, JobDetailManager.Group);
            var queueTrigger = TriggerFactory.CreateSimpleTrigger(new TriggerDetail
            {
                GroupName = JobDetailManager.Group,
                Interval = 10,
                IntervalType = IntervalType.Second,
                JobName = queueJobName,
                TriggerName = queueJobTrigger,
                TriggerType = TriggerType.Simple
            });

            await JobScheduler.ScheduleJob(queueJobDetail, queueTrigger);

            var qrJobName = CreatePrefixFormatedName("Job", "Qr");
            var qrJobTrigger = CreatePrefixFormatedName("Trigger", "Qr");
            var qrJobDetail = JobDetailManager.CreateQrJob(qrJobName, JobDetailManager.Group);
            var qrTrigger = TriggerFactory.CreateCronTrigger(new TriggerDetail
            {
                GroupName = JobDetailManager.Group,
                CronExpression = "0 0/1 * * * ?",
                JobName = qrJobName,
                TriggerName = qrJobTrigger,
                TriggerType = TriggerType.Cron
            });

            await JobScheduler.ScheduleJob(qrJobDetail, qrTrigger);


        }



    }
}
