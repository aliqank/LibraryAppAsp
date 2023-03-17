using Application.Dto.BorrowHistory;
using Application.Services.Interfaces;
using Domain.Entity;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

[ApiController]
[Route("[controller]")]
public class BorrowHistoryController : ControllerBase
{
    private readonly IBorrowHistoryService _borrowHistoryService;
    private readonly IUserRatingJobService _ratingJobService;

    public BorrowHistoryController(
        IBorrowHistoryService borrowHistoryService,
        IUserRatingJobService userRatingJobScheduler)
    {
        _borrowHistoryService = borrowHistoryService;
        _ratingJobService = userRatingJobScheduler;
    }

    [HttpGet]
    public async Task<ActionResult<List<BorrowReadDto>>> GetAll()
    {
        try
        {
            return await _borrowHistoryService.GetAllAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<BorrowReadDto>> CreateAsync(BorrowHistoryCreateDto borrowHistory)
    {
        try
        {
            return await _borrowHistoryService.CreateAsync(borrowHistory);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("/Return")]
    public async Task<ActionResult<BorrowReadDto>> ReturnAsync(BorrowReturnDto borrowReturnDto)
    {
        try
        {
            return await _borrowHistoryService.ReturnBookAsync(borrowReturnDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("/RatingJob")]
    public void RatingJob()
    {
        _ratingJobService.UpdateUserRating();
    }
    
    [HttpGet]
    [Route("/Overdues1")]
    public async Task<ActionResult<List<BorrowHistory>>> GetOverDueBorrows()
    {
        try
        {
            return await _borrowHistoryService.GetOverDueBorrowsAsync();

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}