using greenhouse_aspnet_api.db.Models;
using Microsoft.EntityFrameworkCore;

namespace greenhouse_aspnet_api.db.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly GreenhouseDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(GreenhouseDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
