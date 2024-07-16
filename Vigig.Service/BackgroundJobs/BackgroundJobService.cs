using System.Linq.Expressions;
using Hangfire;
using Vigig.Service.BackgroundJobs.Interfaces;

namespace Vigig.Service.BackgroundJobs;

public class BackgroundJobService : IBackgroundJobService
{
    public void EnqueueFireAndForgetJob(Action job)
    {
        BackgroundJob.Enqueue(() => job());
    }

    public void EnqueueFireAndForgetJob<T>(Action<T> job, T parameter)
    {
        BackgroundJob.Enqueue(() => job(parameter));
    }

    public void ScheduleDelayedJob(Expression<Action> methodCall, TimeSpan delay)
    {
        BackgroundJob.Schedule(methodCall, delay);
    }

    public void ScheduleDelayedJob<T>(Action<T> job, T parameter, TimeSpan delay)
    {
        BackgroundJob.Schedule(() => job(parameter), delay);
    }
}