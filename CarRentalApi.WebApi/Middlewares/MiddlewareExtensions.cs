namespace CarRentalApi.WebApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMaintenenceMode(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MaintenenceMiddleware>();
        }

        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
