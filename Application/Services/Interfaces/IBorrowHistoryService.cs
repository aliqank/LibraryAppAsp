using Application.Dto;
using Application.Dto.BorrowHistory;
using Domain.Entity;
using Domain.Enum;

namespace Application.Services.Interfaces;

public interface IBorrowHistoryService
{
    Task<List<BorrowReadDto>> GetAllAsync();
    Task<BorrowReadDto> CreateAsync(BorrowHistoryCreateDto borrowHistoryCreate);
    Task<BorrowReadDto> ReturnBookAsync(BorrowReturnDto bookReturnDto);
    RatingType GetRatingPenalty(RatingType ratingType, int dueDate);
    Task<List<BorrowHistory>> GetOverDueBorrowsAsync();
}