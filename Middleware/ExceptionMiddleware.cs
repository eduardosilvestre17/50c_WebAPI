using Sage50c.WebAPI.Models;
using System.Net;
using System.Text.Json;

namespace Sage50c.WebAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Ocorreu uma exceção não tratada: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new ApiResponseDto<object>
            {
                Success = false
            };

            switch (exception)
            {
                case ArgumentException argEx:
                    response.Message = $"Argumento inválido: {argEx.Message}";
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                    
                case UnauthorizedAccessException:
                    response.Message = "Acesso não autorizado";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                    
                case FileNotFoundException:
                    response.Message = "Recurso não encontrado";
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                    
                case TimeoutException:
                    response.Message = "Timeout na operação";
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    break;
                    
                default:
                    response.Message = "Erro interno do servidor";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            // Em desenvolvimento, incluir detalhes da exceção
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
            {
                response.Data = new
                {
                    ExceptionType = exception.GetType().Name,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    InnerException = exception.InnerException?.Message
                };
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}