using Quartz;
using RMS.Jobs;
using System.Reflection;
namespace RMS.Server.BusinessLogic
{
    public class JobDetailManager
    {
        public const string Group = "Gateway";
        protected static string ClassName => nameof(JobDetailManager);
        protected static string MethodName { get; private set; }

        public static IJobDetail CreateResendFailedPacketJob(string name, string group, int jobId = -1)
        {
            #region Logging
            MethodBase method = MethodBase.GetCurrentMethod();
            MethodName = method.Name;
            #endregion

            //Logger.Instance.Log.Information(ClassName, MethodName, string.Format("Creating Job, Name: {0}, Group: {1}, Job ID: {2}", name, group, jobId));
            IJobDetail jobDetail = JobBuilder.Create<ResendFailedPacketJob>()
                .WithIdentity(name, group)
                .UsingJobData("id", jobId)
                .Build();
            return jobDetail;
        }

    }
}
