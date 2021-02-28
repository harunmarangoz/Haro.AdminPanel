using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Extensions;
using Haro.AdminPanel.Utilities.Attributes;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Breadcrumb("Kolonlar")]
    public class ColumnController : AddModelController<Column, AddColumnModel>
    {
        public ColumnController(ColumnManager dal)
        {
            Dal = dal;
        }

        public override IActionResult Add(AddColumnModel model)
        {
            var entry = this.Dal.Add(model);
            return Ok(new
            {
                Redirect = Url.Action("Edit", "Table", new {id = entry.TableId})
            });
        }

        [HttpPost]
        [ExceptionHandling]
        public override IActionResult Edit(AddColumnModel model)
        {
            var entry = Dal.Edit(model);
            return Ok(new
            {
                Redirect = Url.Action("Edit", "Table", new {id = entry.TableId})
            });
        }

        [HttpPost]
        [ExceptionHandling]
        public IActionResult GetForm(long? Id) =>
            Ok(new {Html = this.RenderView("Form", Id.HasValue ? Dal.GetAddModel(Id.Value) : Dal.GetAddModel(), true)});

        public override IActionResult Delete(long id)
        {
            var entryId = Dal.GetById(id).TableId;
            Dal.Delete(id);
            return RedirectToAction("Edit", "Table", new {id = entryId});
        }

        public IActionResult GetColumnsByTableId(long id)
        {
            return Ok(Dal.List(x => x.TableId == id).ToSelectListItem());
        }
    }
}