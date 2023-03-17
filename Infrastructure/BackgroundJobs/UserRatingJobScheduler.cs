using System.Linq.Expressions;
using Domain.Interfaces;
using Hangfire;
using Hangfire.Common;

namespace Infrastructure.BackgroundJobs;

public class UserRatingJobScheduler : IUserRatingJobScheduler
{
    private readonly IRecurringJobManager _recurringJobManager;

    public UserRatingJobScheduler(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }
    
    public void UpdateUserRating(Expression<Func<Task>> job)
    {
        _recurringJobManager.AddOrUpdate(
            "User rating updater job",
            Job.FromExpression(job),
            Cron.Daily()
        );
    }
}