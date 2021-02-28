using System;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.Models.CustomModels;
using Haro.AdminPanel.Utilities.Attributes;
using Haro.AdminPanel.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DashboardController : Controller
    {
        private DashboardManager _dashboardManager;
        private LanguageManager _languageManager;

        public DashboardController(DashboardManager dashboardManager, LanguageManager languageManager)
        {
            _dashboardManager = dashboardManager;
            _languageManager = languageManager;
        }

        public IActionResult List(long id)
        {
            return View(_dashboardManager.GetTableListModel(id));
        }


        [HttpGet]
        public IActionResult Add(long id, long? languageId, long? languagePairId)
        {
            if (!languageId.HasValue)
            {
                return RedirectToAction("Add", new {id, languageId = App.Common.Language.Id, languagePairId});
            }

            if (languagePairId.HasValue)
            {
                var entryId = _dashboardManager.FindPairedEntries(id, languageId.Value, languagePairId.Value);
                if (entryId.HasValue) return RedirectToAction("Edit", new {id, entryId});
            }

            var result = _dashboardManager.GetTableAddRecordModel(id, null, languageId);
            result.Language = _languageManager.GetById(languageId.Value);
            result.LanguagePairId = languagePairId;
            return View("AddOrEdit", result);
        }

        [HttpPost]
        [ExceptionHandling]
        public IActionResult Add()
        {
            _dashboardManager.AddRecord(HttpContext.Request.Form);
            return Ok(new
            {
                Redirect = Url.Action("List",
                    new {id = Convert.ToInt64(HttpContext.Request.Form["TableId"].ToString())})
            });
        }

        [HttpGet]
        public IActionResult Edit(long id, long entryId)
        {
            return View("AddOrEdit", _dashboardManager.GetTableAddRecordModel(id, entryId));
        }

        [HttpPost]
        [ExceptionHandling]
        public IActionResult Edit()
        {
            _dashboardManager.EditRecord(HttpContext.Request.Form);
            return Ok(new
            {
                Redirect = Url.Action("List",
                    new {id = Convert.ToInt64(HttpContext.Request.Form["TableId"].ToString())})
            });
        }

        [HttpGet]
        public IActionResult Delete(long id, long entryId)
        {
            _dashboardManager.DeleteRecord(id, entryId);
            return RedirectToAction("List", new {id});
        }

        [HttpPost]
        [ExceptionHandling]
        public IActionResult GetGalleryModal(long ColumnId, long EntryId)
        {
            var model = _dashboardManager.GetGalleryModel(ColumnId, EntryId);
            return Ok(new
            {
                Html = this.RenderView("GalleryModal", model, true)
            });
        }

        [HttpPost]
        [ExceptionHandling]
        public IActionResult AddGalleryImage(GalleryModel model)
        {
            _dashboardManager.AddGalleryImages(model);
            return Ok(new {Redirect = "reload"});
        }

        [HttpPost]
        [ExceptionHandling]
        public IActionResult DeleteGalleryImage(long ColumnId, long EntryId)
        {
            _dashboardManager.DeleteGalelryImage(ColumnId, EntryId);
            return Ok();
        }
    }
}