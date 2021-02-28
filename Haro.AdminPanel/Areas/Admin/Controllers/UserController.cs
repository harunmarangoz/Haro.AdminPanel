using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.CustomModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Breadcrumb("Kullanıcılar")]
    public class UserController : AddModelController<User, AddUserModel>
    {
        private UserManager _userManager;

        public UserController(UserManager dal)
        {
            Dal = dal;
            _userManager = dal;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl) => View(new LoginModel(){ReturnUrl = ReturnUrl});

        [HttpPost]
        [ExceptionHandling]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = _userManager.Login(loginModel.Email, loginModel.Password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            var userIdentity = new ClaimsIdentity(claims, "login");
            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            await HttpContext.SignInAsync(principal);
            if (string.IsNullOrEmpty(loginModel.ReturnUrl))
                loginModel.ReturnUrl = Url.Action("Index", "Home", new {Area = "Admin"});
            return Ok(new {Redirect = loginModel.ReturnUrl});
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}