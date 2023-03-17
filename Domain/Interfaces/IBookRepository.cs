using Domain.Entity;
namespace Domain.Interfaces;

public interface IBookRepository : IRepositoryBase<Book>
{ 
    Task<List<Book>> FindAllAsync();
    Task<Book> GetByIdAsync(long id);
}