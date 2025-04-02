using greenhouse_aspnet_api.db.Repositories;

namespace greenhouse_aspnet_api.db;

public interface IUnitOfWork : IDisposable
{
    IDeviceRepository Devices { get; }
    Task<int> CompleteAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
