using greenhouse_aspnet_api.db.Models;
using Microsoft.AspNetCore.Mvc;

namespace greenhouse_aspnet_api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WeatherForecastController : ControllerBase
{
  private readonly GreenhouseDbContext _dbContext;
  private static readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  private readonly ILogger<WeatherForecastController> _logger;

  public WeatherForecastController(ILogger<WeatherForecastController> logger, GreenhouseDbContext dbContext)
  {
    _dbContext = dbContext;
    _logger = logger;
  }

  [HttpGet(Name = "GetWeatherForecast")]
  public IEnumerable<WeatherForecast> Get()
  {
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    })
    .ToArray();
  }

  [Route("test")]
  [HttpGet]
  public IEnumerable<Device> GetTest()
  {
    var data = _dbContext.Devices.ToList();
    return data;
  }
}
