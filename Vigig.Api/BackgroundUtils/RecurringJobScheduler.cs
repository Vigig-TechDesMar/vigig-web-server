using Hangfire;
using Vigig.Service.BackgroundJobs.Interfaces;

namespace Vigig.Api.BackgroundUtils;

public class RecurringJobScheduler
{
    public static void ScheduleJobs()
    {
        RecurringJob.AddOrUpdate(() => Console.WriteLine("Start hangfire server"),Cron.Daily);
        
        RecurringJob.AddOrUpdate<IExpirationService>(
            service => service.ValidateEventExpiration(), Cron.Daily);
    }
}