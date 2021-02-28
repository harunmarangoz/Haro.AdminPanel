using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Haro.AdminPanel.Utilities.Middlewares
{
    public class RequestLocalizationCustomMiddleware
    {
        RequestDelegate _next;

        public RequestLocalizationCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var culture = context.Request.RouteValues["culture"]?.ToString() ?? "en";
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }
}