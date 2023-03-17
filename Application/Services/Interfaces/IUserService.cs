using Application.Dto.User;
using Domain.Entity;
using Domain.Enum;

namespace Application.Services.Interfaces;

public interface IUserService
{
    Task<List<UserReadDto>> GetAllAsync();
    Task<User> GetByIdAsync(long id);
    Task<List<UserReadDto>> GetByIdsAsync(List<long> ids);
    Task<UserReadDto> CreateAsync(UserCreateDto userCreateDto);
    Task<UserReadDto> UpdateAsync(UserUpdateDto userUpdateDto);
    Task<UserReadDto> UpdateUserRating(long id, RatingType newRating);
    Task<List<User>> UpdateRangeAsync(List<User> users);
}