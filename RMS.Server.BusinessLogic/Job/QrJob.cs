using Quartz;
using System;
using System.Threading.Tasks;

namespace RMS.Jobs
{
    public class QrJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await context.Scheduler.PauseJob(context.Trigger.JobKey);
                Console.WriteLine("{0} -> QR Job Triggered", DateTime.Now);

                await context.Scheduler.ResumeJob(context.Trigger.JobKey);

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }


        }
    }
}

