using Domain.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;

    private readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _context.Entry(entity).State = EntityState.Modified;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            await _context.SaveChangesAsync();

            return entity;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message, e.InnerException);
        }
    }
    
    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entities)
    {
        await _context.BulkUpdateAsync(entities);
        await _context.SaveChangesAsync();

        return entities;
    }
}