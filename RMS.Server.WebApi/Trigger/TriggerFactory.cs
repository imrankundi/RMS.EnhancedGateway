using Quartz;
using RMS.Server.DataTypes.Quartz;
using System;

namespace RMS.Server.BusinessLogic
{
    public class TriggerFactory
    {
        public static ITrigger CreateTrigger(TriggerDetail triggerDetail)
        {
            switch (triggerDetail.TriggerType)
            {
                case TriggerType.Cron:
                    return CreateCronTrigger(triggerDetail);
                default:
                    return CreateSimpleTrigger(triggerDetail);

            }
        }
        public static ITrigger CreateSimpleTrigger(TriggerDetail triggerDetail)
        {
            switch (triggerDetail.IntervalType)
            {
                case IntervalType.Second:
                    return CreateSimpleTriggerWithIntervalInSeconds(triggerDetail);
                case IntervalType.Minute:
                    return CreateSimpleTriggerWithIntervalInMinutes(triggerDetail);
                case IntervalType.Hour:
                    return CreateSimpleTriggerWithIntervalInHours(triggerDetail);
                default:
                    return CreateSimpleTriggerWithIntervalInSeconds(triggerDetail);
            }
        }

        public static ITrigger CreateCronTrigger(TriggerDetail triggerDetail)
        {
            // cron schedule "0 1 * * * ?"
            return TriggerBuilder.Create()
                                .WithIdentity(triggerDetail.TriggerName, triggerDetail.GroupName)
                                .WithCronSchedule(triggerDetail.CronExpression)
                                .ForJob(triggerDetail.JobName, triggerDetail.GroupName)
                                .Build();
        }

        private static ITrigger CreateSimpleTriggerWithIntervalInSeconds(TriggerDetail triggerDetail)
        {
            return TriggerBuilder.Create()
                                 .WithIdentity(new TriggerKey(triggerDetail.TriggerName, triggerDetail.GroupName))
                                 .ForJob(triggerDetail.JobName, triggerDetail.GroupName)
                                 .StartAt(DateTime.UtcNow.AddSeconds(triggerDetail.Interval))
                                 .WithSimpleSchedule(x => x
                                     .WithIntervalInSeconds(triggerDetail.Interval)
                                     .RepeatForever())
                                 .Build();
        }

        private static ITrigger CreateSimpleTriggerWithIntervalInMinutes(TriggerDetail triggerDetail)
        {
            return TriggerBuilder.Create()
                                 .WithIdentity(triggerDetail.TriggerName, triggerDetail.GroupName)
                                 .ForJob(triggerDetail.JobName, triggerDetail.GroupName)
                                 .StartAt(DateTime.UtcNow.AddMinutes(triggerDetail.Interval))
                                 .WithSimpleSchedule(x => x
                                     .WithIntervalInMinutes(triggerDetail.Interval)
                                     .RepeatForever())
                                 .Build();
        }

        private static ITrigger CreateSimpleTriggerWithIntervalInHours(TriggerDetail triggerDetail)
        {
            return TriggerBuilder.Create()
                                 .WithIdentity(triggerDetail.TriggerName, triggerDetail.GroupName)
                                 .ForJob(triggerDetail.JobName, triggerDetail.GroupName)
                                 .StartAt(DateTime.UtcNow.AddHours(triggerDetail.Interval))
                                 .WithSimpleSchedule(x => x
                                     .WithIntervalInHours(triggerDetail.Interval)
                                     .RepeatForever())
                                 .Build();
        }



    }
}
