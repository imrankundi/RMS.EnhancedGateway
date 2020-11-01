using Quartz;
using RMS.Jobs;

namespace RMS.Server.BusinessLogic
{
    public class JobDetailManager
    {
        public const string Group = "MCW";

        public static IJobDetail CreateQueueJob(string name, string group, int jobId = -1)
        {
            IJobDetail jobDetail = JobBuilder.Create<QueueJob>()
                .WithIdentity(name, group)
                .UsingJobData("id", jobId)
                .Build();
            return jobDetail;
        }

        public static IJobDetail CreateQrJob(string name, string group, int jobId = -1)
        {
            IJobDetail jobDetail = JobBuilder.Create<QrJob>()
                .WithIdentity(name, group)
                .UsingJobData("id", jobId)
                .Build();
            return jobDetail;
        }

    }
}
