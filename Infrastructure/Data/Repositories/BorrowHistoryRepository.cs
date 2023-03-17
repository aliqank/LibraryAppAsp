using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class BorrowHistoryRepository : RepositoryBase<BorrowHistory>,IBorrowHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public BorrowHistoryRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<List<BorrowHistory>> FindAllAsync()
    {
        return await _context.BorrowHistories
            .ToListAsync();
    }

    public async Task<BorrowHistory> GetByIdAsync(long id)
    {
         return await _context.BorrowHistories.FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<List<BorrowHistory>> GetOverDueBorrows()
    {
        return await _context.BorrowHistories
            .Where(bh => bh.DueDate < DateTime.UtcNow && !bh.IsReturned)
            .Include(u=>u.User)
            .ToListAsync();
    }
}
