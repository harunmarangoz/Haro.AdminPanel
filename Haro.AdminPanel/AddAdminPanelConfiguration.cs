using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.DataAccess;
using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Utilities.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace Haro.AdminPanel
{
    public static class AddAdminPanelConfiguration
    {
        public static void AddAdminPanel(this IServiceCollection services, string connectionString,
            string culture = "tr")
        {
            var assembly = Assembly.Load("Haro.AdminPanel");
            services.AddControllersWithViews()
                .AddApplicationPart(assembly)
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("Haro.AdminPanel");
            });

            services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(assembly));
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { options.LoginPath = "/Admin/User/Login/"; });

            services.AddSingleton<EfLanguageDal>();
            services.AddSingleton<LanguageManager>();
            services.AddSingleton<EfUserDal>();
            services.AddSingleton<UserManager>();
            services.AddSingleton<EfUserModuleDal>();
            services.AddSingleton<UserModuleManager>();
            services.AddSingleton<EfModuleDal>();
            services.AddSingleton<ModuleManager>();
            services.AddSingleton<EfTableDal>();
            services.AddSingleton<TableManager>();
            services.AddSingleton<EfColumnDal>();
            services.AddSingleton<ColumnManager>();
            services.AddSingleton<EfSiteInformationDal>();
            services.AddSingleton<SiteInformationManager>();

            services.AddSingleton<DashboardManager>();
            services.AddSingleton<ExportManager>();
            services.AddSingleton<SqlProvider>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<WebCommon>();
            App.Common = services.BuildServiceProvider().GetService<WebCommon>();
            App.ConnectionString = connectionString;

            var cultureInfo = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }

        public static void UseAdminPanel(this IApplicationBuilder app)
        {
            app.UseWhen(x => x.Request.Path.Value.StartsWith("/Admin/"), builder =>
            {
                app.UseMiddleware<BackEndRequestMiddleware>();
                builder.UseExceptionHandler("/Admin/Home/Error");
            });
        }
    }
}