using Application.Dto.Book;
using Application.Dto.BorrowHistory;
using Application.Dto.User;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces;

namespace Application.Services;

public class BorrowHistoryService : IBorrowHistoryService
{
    private const int HalfYearInDays = 180;
    private const int TwoMonthInDays = 61;
    private const int OneMonthInDays = 31;
    private const int ZeroDays = 0;
    private const int OneDayLate = -1;
    private const int OneWeekLate = -7;
    private const int TwoWeeksLate = -14;
    private const int OneMonthLate = -31;
    private const int DecreaseToOne = 1;

    private readonly IBorrowHistoryRepository _borrowHistoryRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    private readonly IBookService _bookService;

    public BorrowHistoryService(IMapper mapper,
        IBorrowHistoryRepository borrowHistoryRepository,
        IUserService userService,
        IBookService bookService)
    {
        _mapper = mapper;
        _borrowHistoryRepository = borrowHistoryRepository;
        _userService = userService;
        _bookService = bookService;
    }

    public async Task<List<BorrowReadDto>> GetAllAsync()
    {
        var borrowHistories = await _borrowHistoryRepository.FindAllAsync();
        return _mapper.Map<List<BorrowReadDto>>(borrowHistories);
    }

    public async Task<BorrowReadDto> CreateAsync(BorrowHistoryCreateDto borrowHistoryCreate)
    {
        var borrowHistory = _mapper.Map<BorrowHistory>(borrowHistoryCreate);

        var userId = borrowHistory.UserId;
        var bookId = borrowHistory.BookId;

        var user = await _userService.GetByIdAsync(userId);
        var book = await _bookService.GetByIdAsync(bookId);

        if (book.IsAvailable == false)
        {
            throw new InvalidOperationException("Book isn't available");
        }

        if (borrowHistoryCreate.BorrowDurationInDays > GetLimit(user.Rating))
        {
            throw new InvalidOperationException("User rating is not enough");
        }

        if (user.Limit <= 0)
        {
            throw new InvalidOperationException("User limit is 0");
        }
        
        book.IsAvailable = false;
        user.Limit -= DecreaseToOne;
        
        await _bookService.UpdateAsync(_mapper.Map<BookUpdateDto>(book));
        await _userService.UpdateAsync(_mapper.Map<UserUpdateDto>(user));
        var newBorrowHistory = await _borrowHistoryRepository.CreateAsync(borrowHistory);
        return _mapper.Map<BorrowReadDto>(newBorrowHistory);
    }

    public async Task<BorrowReadDto> ReturnBookAsync(BorrowReturnDto borrowReturnDto)
    {
        var borrowHistory = await _borrowHistoryRepository.GetByIdAsync(borrowReturnDto.Id);

        if (borrowHistory == null)
        {
            throw new NullReferenceException("Model is null");
        }

        if (borrowHistory.IsReturned)
        {
            throw new InvalidOperationException("User already returned this book");
        }

        var userId = borrowReturnDto.UserId;
        var bookId = borrowHistory.BookId;
        
        var user = await _userService.GetByIdAsync(userId);
        var book = await _bookService.GetByIdAsync(bookId);

        borrowHistory.IsReturned = true;
        borrowHistory.ReturnedDate = DateTime.UtcNow;

        var currentRating = user.Rating;
        
        var daysDelayed = CalculateDaysDelayed(borrowHistory);

        var newUserRating = GetRatingPenalty(currentRating, daysDelayed);

        user.Rating = newUserRating;
        
        await _borrowHistoryRepository.UpdateAsync(borrowHistory);
        await _userService.UpdateAsync(_mapper.Map<UserUpdateDto>(user));
        await _bookService.UpdateAsync(_mapper.Map<BookUpdateDto>(book));
        
        return _mapper.Map<BorrowReadDto>(borrowHistory);
    }
    
    private int CalculateDaysDelayed(BorrowHistory borrowHistory)
    {
        if (borrowHistory.ReturnedDate < borrowHistory.DueDate)
        {
            return 0;
        }

        var actualBorrowDurationInDays = (borrowHistory.BorrowDate - borrowHistory.BorrowDate).Days;
        return actualBorrowDurationInDays + borrowHistory.BorrowDurationInDays;
    }

    public RatingType GetRatingPenalty(RatingType ratingType, int dueDate)
    {
        var rating = dueDate switch
        {
            >= ZeroDays => ratingType + 1,
            >= OneDayLate => ratingType - 1,
            >= OneWeekLate => ratingType - 2,
            >= TwoWeeksLate => ratingType - 3,
            _ => RatingType.VeryBad
        };
        
        return rating < RatingType.VeryBad ? RatingType.VeryBad : rating;
    }

    private int GetLimit(RatingType rating)
    {
        switch (rating)
        {
            case RatingType.Excellent:
                return HalfYearInDays;
            case RatingType.Good:
                return TwoMonthInDays;
            case RatingType.Neutral:
                return OneMonthInDays;
            case RatingType.Bad:
            case RatingType.VeryBad:
            default:
                return ZeroDays;
        }
    }
    
    public Task<List<BorrowHistory>> GetOverDueBorrowsAsync()
    {
        return _borrowHistoryRepository.GetOverDueBorrows();
    }
}