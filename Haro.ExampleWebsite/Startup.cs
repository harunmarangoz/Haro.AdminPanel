using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Haro.AdminPanel;
using Haro.AdminPanel.Utilities.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Haro.ExampleWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAdminPanel(Configuration["ConnectionString"],"en");
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAdminPanel();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("tr"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture("en")
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<RequestLocalizationCustomMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/",
                    defaults: new {culture = "en"}, constraints: new {culture = "en"});
                endpoints.MapControllerRoute(name: "defaultWithCulture",
                    pattern: "{culture}/{controller=Home}/{action=Index}/");
            });
        }
    }
}