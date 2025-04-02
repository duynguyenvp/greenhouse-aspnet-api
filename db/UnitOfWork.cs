using greenhouse_aspnet_api.db.Models;
using greenhouse_aspnet_api.db.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace greenhouse_aspnet_api.db;

public class UnitOfWork : IUnitOfWork
{
    private readonly GreenhouseDbContext _context;
    private IDbContextTransaction? _transaction;

    public IDeviceRepository Devices { get; }

    public UnitOfWork(GreenhouseDbContext context)
    {
        _context = context;
        Devices = new DeviceRepository(context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Begin transaction
    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }

    // Commit transaction
    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Rollback transaction
    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    // Dispose
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
