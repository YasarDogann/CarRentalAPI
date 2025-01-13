using CarRentalApi.Business.Operations.Setting;

namespace CarRentalApi.WebApi.Middlewares
{
    public class MaintenenceMiddleware
    {
        private readonly RequestDelegate _next;
        public MaintenenceMiddleware(RequestDelegate next)
        {
            _next = next;
            
        }

        public async Task Invoke(HttpContext context)
        {
            var settingService = context.RequestServices.GetRequiredService<ISettingService>(); // property injection
            bool maintenenceMode = settingService.GetMaintenenceState();

            if(context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments ("/api/settings"))
            {
                await _next(context);
                return;
            }

            if (maintenenceMode)
            {
                await context.Response.WriteAsync("Şu anda hizmet verememekteyiz.");
            }
            else
            {
                await _next(context);
            }
        }
    }
}
