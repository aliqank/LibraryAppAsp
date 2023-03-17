using System.Collections;
using Application.Dto.User;
using Application.Services.Interfaces;
using Domain.Entity;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<UserReadDto>>> GetAll()
    {
        try
        {
            return await _userService.GetAllAsync();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);

        }
    }

    [HttpPost]
    public async Task<ActionResult<UserReadDto>> CreateAsync(UserCreateDto userCreateDto)
    {
        try
        {
            return await _userService.CreateAsync(userCreateDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<UserReadDto>> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        try
        {
            return await _userService.UpdateAsync(userUpdateDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}