using System.Threading.Tasks;
using Haro.AdminPanel.Common;
using Microsoft.AspNetCore.Http;

namespace Haro.AdminPanel.Utilities.Middlewares
{
    public class BackEndRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public BackEndRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Do tasks before other middleware here, aka 'BeginRequest'
            if(App.Common != null)
            {
                App.Common.Clear();
            }

            // Let the middleware pipeline run
            await _next(context);

            // Do tasks after middleware here, aka 'EndRequest'
            // ...
        }
    }
}