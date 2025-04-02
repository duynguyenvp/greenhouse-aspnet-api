using greenhouse_aspnet_api.db.Models;

namespace greenhouse_aspnet_api.db.Repositories;

public interface IDeviceRepository : IGenericRepository<Device>
{
    Task<IEnumerable<Device>> GetByTypeAsync(DeviceType type);
    Task<IEnumerable<Device>> GetAll();
}
