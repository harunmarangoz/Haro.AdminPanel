using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.DataAccess;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.CustomModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private SiteInformationManager _siteInformationManager;
        private UserManager _userManager;
        private LanguageManager _languageManager;

        public HomeController(SiteInformationManager siteInformationManager, UserManager userManager,
            LanguageManager languageManager)
        {
            _siteInformationManager = siteInformationManager;
            _userManager = userManager;
            _languageManager = languageManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            using (var context = new AdminPanelContext())
            {
                if (!context.Database.GetService<IRelationalDatabaseCreator>().Exists())
                {
                    return RedirectToAction("Setup");
                }
            }

            var user = App.Common.User;
            if (user == null) return RedirectToAction("Login", "User");
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var st = new StackTrace(exceptionHandlerPathFeature.Error, true);
            var frame = st.GetFrame(0);
            int line = 0;
            string file = "";
            if (frame != null)
            {
                line = frame.GetFileLineNumber();
                file = frame.GetFileName();
            }

            return View(new ExceptionPageModel()
            {
                Description = exceptionHandlerPathFeature.Error.Message,
                Source = $"{file}:{line}"
            });
        }

        [AllowAnonymous]
        public IActionResult Setup() => View(new SetupModel());

        [AllowAnonymous]
        [ExceptionHandling]
        [HttpPost]
        public IActionResult Setup(SetupModel model)
        {
            using (var context = new AdminPanelContext())
            {
                context.Database.Migrate();
            }

            _siteInformationManager.Add(new AddSiteInformationModel()
            {
                ProjectName = model.ProjectName,
                SmtPort = model.SmtPort,
                SmtpPassword = model.SmtpPassword,
                SmtpServer = model.SmtpServer,
                SmtpEnableSsl = model.SmtpEnableSsl,
                SmtpUserName = model.SmtpUserName,
            });
            model.SelectedLanguages.ForEach(x => { _languageManager.Add(GetLang(x)); });
            _userManager.Add(new AddUserModel()
            {
                Email = model.UserEmail,
                Name = model.UserName,
                Password = model.UserPassword,
                PasswordControl = model.UserPassword,
                Role = Role.Support,
            });
            return Ok(new {Redirect = Url.Action("Index")});
        }

        [HttpGet]
        public IActionResult SetLanguage(long id)
        {
            var returnUrl = Request.Headers["Referer"];
            var lang = _languageManager.GetById(id);
            var cultureInfo = new CultureInfo(lang.Code);
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            return string.IsNullOrEmpty(returnUrl) ? (IActionResult) RedirectToAction("Index") : Redirect(returnUrl);
        }

        public IActionResult UpdateContext()
        {
            using (var context = new AdminPanelContext())
            {
                context.Database.Migrate();
            }

            return RedirectToAction("Index");
        }

        private AddLanguageModel GetLang(string str)
        {
            switch (str)
            {
                case "tr":
                    return new AddLanguageModel()
                    {
                        Code = "tr",
                        DisplayValue = "Türkçe",
                        Image = "https://flagcdn.com/160x120/tr.png"
                    };
                case "en":
                    return new AddLanguageModel()
                    {
                        Code = "en",
                        DisplayValue = "English",
                        Image = "https://flagcdn.com/160x120/gb.png"
                    };
                default:
                    throw new Exception("language");
            }
        }
    }
}