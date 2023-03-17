using Application.Dto.Book;
using Domain.Entity;

namespace Application.Services.Interfaces;

public interface IBookService
{
    Task<List<BookReadDto>> GetAllAsync();
    Task<BookReadDto> CreateAsync(BookCreateDto book);
    Task<Book> GetByIdAsync(long id);
    Task<BookReadDto> UpdateAsync(BookUpdateDto bookUpdateDto);

}