using System;
using System.Text;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Utilities.Attributes;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Breadcrumb("Dışa Aktar")]
    public class ExportController : Controller
    {
        private ExportManager _exportManager;
        private IWebHostEnvironment _env;

        public ExportController(ExportManager exportManager, IWebHostEnvironment env)
        {
            _exportManager = exportManager;
            _env = env;
        }

        public IActionResult Index()
        {
            if(!_env.IsDevelopment()) throw new Exception("Dışarı çıkartma sadece geliştirme modunda kullanılabilir.");
            return View();
        }
        [ExceptionHandling]
        [HttpPost]
        public IActionResult Export()
        {
            _exportManager.Export();
            return Ok(new { Message = "Başarıyla çıkartıldı."});
        }
        
        
        public IActionResult ExportSchema()
        {
            var result = JsonConvert.SerializeObject(_exportManager.ExportSchema());
            var encoded =  Encoding.UTF8.GetBytes(result);
            return File(encoded , "application/json", "file.json");
        }
        
        public ActionResult ExportSchemaData()
        {
            var data = "YourDataHere";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            var output = new FileContentResult(bytes, "application/octet-stream");
            output.FileDownloadName = "download.txt";

            return output;
        }
    }
}