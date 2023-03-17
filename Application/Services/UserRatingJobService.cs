using Application.Services.Interfaces;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces;

namespace Application.Services;

public class UserRatingJobService : IUserRatingJobService
{
    private const int OneDayLate = -1;
    private const int OneWeekLate = -7;
    private const int TwoWeeksLate = -14;
    private const int OneMonthLate = -31;
    
    private readonly IUserRatingJobScheduler _userRatingJobScheduler;
    private readonly IUserService _userService;
    private readonly IBorrowHistoryService _borrowHistoryService;

    public UserRatingJobService(IUserRatingJobScheduler userRatingJobScheduler,
        IUserService userService,
        IBorrowHistoryService borrowHistoryService)
    {
        _userRatingJobScheduler = userRatingJobScheduler;
        _userService = userService;
        _borrowHistoryService = borrowHistoryService;
    }

    public void UpdateUserRating()
    {
        _userRatingJobScheduler.UpdateUserRating( () => UserRatingUpdater());
    }

    public async Task UserRatingUpdater()
    {
        var overDueBorrows = await _borrowHistoryService.GetOverDueBorrowsAsync();

        var usersList = new List<User>();

        foreach (var borrow in overDueBorrows)
        {
            var daysDelayed = (borrow.DueDate - DateTime.UtcNow).Days;

            Console.WriteLine(daysDelayed);
            var user = borrow.User;
            if (daysDelayed is OneDayLate or OneWeekLate or TwoWeeksLate or OneMonthLate)
            {
                var userRating = user.Rating;
                var newUserRating = _borrowHistoryService.GetRatingPenalty(userRating, daysDelayed);
                user.Rating = newUserRating;
                usersList.Add(user);
            }
        }

        await _userService.UpdateRangeAsync(usersList.Distinct().ToList());
    }
}