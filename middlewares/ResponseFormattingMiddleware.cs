using System.Net;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace greenhouse_aspnet_api.Middlewares;

public class ResponseFormattingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ResponseFormattingMiddleware> _logger;

  public ResponseFormattingMiddleware(RequestDelegate next, ILogger<ResponseFormattingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    if (context.Request.Path.HasValue &&
        (context.Request.Path.Value.Contains("swagger", StringComparison.OrdinalIgnoreCase) ||
         context.Request.Path.Value.Contains("openapi", StringComparison.OrdinalIgnoreCase)))
    {
      await _next(context);
      return;
    }
    var originalBodyStream = context.Response.Body;
    using (var responseBody = new MemoryStream())
    {
      context.Response.Body = responseBody;
      await _next(context);
      context.Response.Body = originalBodyStream;
      var response = await FormatResponse(context, responseBody);
      await context.Response.WriteAsync(response);
    }
  }

  private async Task<string> FormatResponse(HttpContext context, MemoryStream responseBody)
  {
    responseBody.Seek(0, SeekOrigin.Begin);
    var responseText = await new StreamReader(responseBody).ReadToEndAsync();

    try
    {
      var jsonNode = JsonNode.Parse(responseText);
      if (jsonNode != null && jsonNode["status"] != null && jsonNode["error"] != null)
      {
        return responseText;
      }
    }
    catch { }

    object? responseData = null;
    try
    {
      responseData = JsonSerializer.Deserialize<object>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    catch
    {
      responseData = responseText;
    }

    var formattedResponse = new
    {
      status = context.Response.StatusCode,
      data = context.Response.StatusCode == (int)HttpStatusCode.OK ? responseData : null,
      message = context.Response.StatusCode == (int)HttpStatusCode.OK ? "Success" : "An error occurred",
      error = context.Response.StatusCode != (int)HttpStatusCode.OK ? responseData : null
    };

    context.Response.ContentType = "application/json";
    return JsonSerializer.Serialize(formattedResponse, new JsonSerializerOptions { WriteIndented = true });
  }
}
