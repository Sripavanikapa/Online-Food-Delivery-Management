using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                _logger.LogInformation("Handling request: " + context.Request.Path);
                await _next(context);
                _logger.LogInformation("Finished handling request.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    NotImplementedException => (int)HttpStatusCode.NotImplemented, // 501
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, // 401
                    ArgumentException => (int)HttpStatusCode.BadRequest, // 400
                    KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404
                    _ => (int)HttpStatusCode.InternalServerError // 500
                };

                context.Response.StatusCode = statusCode;

                var response = new
                {
                    Message = "An Error Occured. Please try again ",
                    StatusCode = statusCode,
                    ExceptionType = ex.GetType().Name
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
               
            }
        }
    }
}