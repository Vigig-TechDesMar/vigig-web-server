using System.Linq.Expressions;
using Vigig.Common.Attribute;

namespace Vigig.Service.BackgroundJobs.Interfaces;
[ServiceRegister]
public interface IBackgroundJobService
{
    void EnqueueFireAndForgetJob(Action job);
    void EnqueueFireAndForgetJob<T>(Action<T> job, T parameter);
    void ScheduleDelayedJob(Expression<Action> methodCall, TimeSpan delay);
    void ScheduleDelayedJob<T>(Action<T> job, T parameter, TimeSpan delay);
}