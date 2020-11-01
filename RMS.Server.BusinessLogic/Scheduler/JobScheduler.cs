using Quartz;
using Quartz.Impl;
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

        public static async Task ScheduleJob(IJobDetail jobDetail, ITrigger trigger)
        {
            try
            {
                await scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (System.Exception ex)
            {

                throw;
            }


        }

        private static async Task<ITrigger> FindExistingTrigger(TriggerKey key)
        {
            var trigger = await scheduler.GetTrigger(key);
            return trigger;
        }

        public static async Task Start()
        {
            scheduler = await factory.GetScheduler();
            await scheduler.Start();
        }
        public static async Task Stop()
        {
            await scheduler.Shutdown();
        }

        public static async Task UnscheduleJob(TriggerKey triggerKey)
        {
            await scheduler.UnscheduleJob(triggerKey);
        }
    }
}
