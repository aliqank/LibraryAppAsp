using Application.Dto.Book;
using Application.Services.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BookReadDto>>> GetAll()
    {
        try
        {
            return await _bookService.GetAllAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<BookReadDto>> CreateAsync(BookCreateDto book)
    {
        try
        {
            return await _bookService.CreateAsync(book);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}