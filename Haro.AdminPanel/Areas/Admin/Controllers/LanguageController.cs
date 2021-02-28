using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Breadcrumb("Diller")]
    public class LanguageController : AddModelController<Language, AddLanguageModel>
    {
        public LanguageController(LanguageManager dal)
        {
            Dal = dal;
        }
    }
}