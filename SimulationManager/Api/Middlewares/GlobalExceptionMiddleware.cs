using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SimulationManager.Application.DTOs.Common;
using System.Threading.Tasks;

namespace SimulationManager.Api.Middlewares
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Unhandled exception: {message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = ex switch
            {
                KeyNotFoundException => new ErrorResponseDto(
                    StatusCodes.Status404NotFound,
                    ex.Message
                ),
                ArgumentException => new ErrorResponseDto(
                    StatusCodes.Status400BadRequest,
                    ex.Message
                ),
                _ => new ErrorResponseDto(
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred."
                )
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
