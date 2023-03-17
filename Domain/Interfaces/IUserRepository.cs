using Domain.Entity;
namespace Domain.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<List<User>> FindAllAsync();
    Task<User> GetByIdAsync(long id);
    Task<List<User>> GetByIdsAsync(List<long> ids);
}
