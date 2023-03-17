namespace Domain.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities);
    Task Delete(TEntity entity);
}