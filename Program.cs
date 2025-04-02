using greenhouse_aspnet_api.Middlewares;
using Serilog;
using Serilog.Formatting.Json;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff tt}] {Level:u4}: {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(new JsonFormatter(), "logs/log.json", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

try
{
  Log.Information("Starting up");

  var builder = WebApplication.CreateBuilder(args);
  // Add services to the container.
  builder.Services.AddSerilog();
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
  builder.Services.AddControllers();
  // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
  builder.Services.AddOpenApi();

  var app = builder.Build();

  // Configure the HTTP request pipeline.
  app.UseMiddleware<ResponseFormattingMiddleware>();
  app.UseMiddleware<GlobalExceptionMiddleware>();
  app.UseHttpsRedirection();
  app.UseAuthorization();
  app.MapControllers();
  if (app.Environment.IsDevelopment())
  {
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test"); });

  }
  app.Run();
}
catch (Exception error)
{
  Log.Error(error, "Server terminated unexpectedly.");
}
finally
{
  Log.CloseAndFlush();
}

