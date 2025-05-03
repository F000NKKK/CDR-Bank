using CDR_Bank.Libs.API.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace CDR_Bank.Libs.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                ValidationException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            var problem = new ErrorResponse
            {
                Type = $"https://httpstatuses.com/{(int)statusCode}",
                Title = exception.Message,
                Status = (int)statusCode,
                Instance = context.Request.Path,
                ErrorCode = exception.GetType().Name,
                AdditionalData = exception is ValidationException ve
                    ? new Dictionary<string, object>
                        {
                            ["ValidationMessage"] = ve.ValidationResult?.ErrorMessage ?? string.Empty
                        }
                    : new Dictionary<string, object>()
            };

            var json = JsonSerializer.Serialize(problem);
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(json);
        }
    }
}
