using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class BookRepository : RepositoryBase<Book>,IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<List<Book>> FindAllAsync()
    {
        return await _context.Books
            .ToListAsync();
    }

    public async Task<Book> GetByIdAsync(long id)
    {
        return await _context.Books.FirstOrDefaultAsync(u => u.Id == id);
    }
}
