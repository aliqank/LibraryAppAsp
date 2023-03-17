using Application.Dto.User;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IMapper mapper,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<List<UserReadDto>> GetAllAsync()
    {
        var users = await _userRepository.FindAllAsync();
        return _mapper.Map<List<UserReadDto>>(users);
    }

    public async Task<UserReadDto> CreateAsync(UserCreateDto userCreateDto)
    {
        if (userCreateDto == null)
        {
            throw new ArgumentNullException($"Data is null");
        }

        var user = _mapper.Map<User>(userCreateDto);
        var newUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<UserReadDto>(newUser);
    }

    public async Task<User> GetByIdAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ArgumentNullException($"{id}", $"user not found by {id} id");
        }

        return user;
    }

    public async Task<List<UserReadDto>> GetByIdsAsync(List<long> ids)
    {
        var users = await _userRepository.GetByIdsAsync(ids);
        return _mapper.Map<List<UserReadDto>>(users);
    }

    public async Task<UserReadDto> UpdateUserRating(long id, RatingType newRating)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ArgumentNullException($"{id}", $"user not found by {id} id");
        }

        user.Rating = newRating;
        var newUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserReadDto>(newUser);
    }

    public async Task<UserReadDto> UpdateAsync(UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.GetByIdAsync(userUpdateDto.Id);
        if (user == null)
        {
            throw new NullReferenceException($"user not found by {userUpdateDto.Id} id");
        }

        var newUser = _mapper.Map(userUpdateDto, user);
        var updatedUser = await _userRepository.UpdateAsync(newUser);
        return _mapper.Map<UserReadDto>(updatedUser);
    }

    public async Task<List<User>> UpdateRangeAsync(List<User> users)
    {
        return await _userRepository.UpdateRangeAsync(users);
    }
}