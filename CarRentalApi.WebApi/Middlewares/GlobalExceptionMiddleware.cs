using System.Net;
using System.Text.Json;
using CarRentalApi.Business.Excepions;
using Microsoft.Extensions.Logging;

namespace CarRentalApi.WebApi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                var errorResponse = new ErrorResponse
                {
                    Success = false,
                    StatusCode = error switch
                    {
                        CustomException e => e.StatusCode,
                        KeyNotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };

                switch (error)
                {
                    case CustomException e:
                        errorResponse.Message = e.Message;
                        break;
                    case KeyNotFoundException:
                        errorResponse.Message = "Kaynak bulunamadı";
                        break;
                    default:
                        errorResponse.Message = "Beklenmeyen bir hata oluştu";
                        if (_env.IsDevelopment())
                        {
                            errorResponse.Detail = error.Message;
                        }
                        break;
                }

                var result = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                response.StatusCode = errorResponse.StatusCode;
                await response.WriteAsync(result);
            }
        }
    }

    public class ErrorResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
