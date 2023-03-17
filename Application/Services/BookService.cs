using Application.Dto.Book;
using Application.Dto.User;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces;

namespace Application.Services;

public class BookService : IBookService
{ 
    private const int IncreaseToOne = 1;
    
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public BookService(
        IMapper mapper,
        IBookRepository bookRepository,
        IUserService userService)
    {
        _mapper = mapper;
        _bookRepository = bookRepository;
        _userService = userService;
    }

    public async Task<List<BookReadDto>> GetAllAsync()
    {
        var books = await _bookRepository.FindAllAsync();
        return _mapper.Map<List<BookReadDto>>(books);
    }

    public async Task<BookReadDto> CreateAsync(BookCreateDto bookCreateDto)
    {
        var book = _mapper.Map<Book>(bookCreateDto);
        
        var userId = book.UserId;
        var user = await _userService.GetByIdAsync(userId);

        user.Limit += IncreaseToOne;
        
        await _userService.UpdateAsync(_mapper.Map<UserUpdateDto>(user));
        var newBook = await _bookRepository.CreateAsync(book);
        return _mapper.Map<BookReadDto>(newBook);
    }

    public async Task<Book> GetByIdAsync(long id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }
    
    public async Task<BookReadDto> UpdateAsync(BookUpdateDto bookUpdateDto)
    {
        var book = await _bookRepository.GetByIdAsync(bookUpdateDto.Id);
        var newBook = _mapper.Map(bookUpdateDto, book);
        var updatedBook = await _bookRepository.UpdateAsync(newBook);
        return _mapper.Map<BookReadDto>(updatedBook);
    }
}