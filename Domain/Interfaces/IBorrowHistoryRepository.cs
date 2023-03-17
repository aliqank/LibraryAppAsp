using Domain.Entity;

namespace Domain.Interfaces;

public interface IBorrowHistoryRepository : IRepositoryBase<BorrowHistory>
{
    Task<List<BorrowHistory>> FindAllAsync();
    Task<BorrowHistory> GetByIdAsync(long id);
    Task<List<BorrowHistory>> GetOverDueBorrows();
}
