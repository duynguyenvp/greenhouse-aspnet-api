using greenhouse_aspnet_api.db.Models;
using Microsoft.EntityFrameworkCore;

namespace greenhouse_aspnet_api.db.Repositories;

public class DeviceRepository : GenericRepository<Device>, IDeviceRepository
{
  public DeviceRepository(GreenhouseDbContext context) : base(context) { }

  public async Task<IEnumerable<Device>> GetAll()
  {
    return await _dbSet.ToListAsync();
  }

  public async Task<IEnumerable<Device>> GetByTypeAsync(DeviceType type)
  {
    return await _dbSet.Where(d => d.Type == type).ToListAsync();
  }
}
