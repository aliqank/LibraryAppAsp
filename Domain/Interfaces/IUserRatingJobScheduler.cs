using System.Linq.Expressions;
namespace Domain.Interfaces;

public interface IUserRatingJobScheduler
{
    void UpdateUserRating(Expression<Func<Task>> job);
}