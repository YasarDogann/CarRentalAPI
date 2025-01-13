using CarRentalApi.WebApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace CarRentalApi.WebApi.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex) 
        {
            context.Response.ContentType = "application/json";

            // Default Status Code
            var statusCode = (int)HttpStatusCode.InternalServerError;

            //custom handling for NotFoundException
            if(ex is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                error = ex.Message,
                statusCode = statusCode
            });
            
            return context.Response.WriteAsync(result);
        }
    }
}
