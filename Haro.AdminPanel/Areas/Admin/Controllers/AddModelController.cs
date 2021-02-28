using System;
using System.Collections.Generic;
using System.Reflection;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Extensions;
using Haro.AdminPanel.Utilities.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Haro.AdminPanel.Areas.Admin.Controllers
{
    public abstract class AddModelController<T, TAddModel> : Controller
        where T : class, IEntity, new()
        where TAddModel : T, new()
    {
        protected AddModelManager<T, TAddModel> Dal { get; set; }

        public IActionResult Index() => RedirectToAction("List");

        public virtual IActionResult List() => base.View(Dal.List());

        public virtual IActionResult Add()
        {
            TAddModel addModel = Dal.GetAddModel();
            Type type = addModel.GetType();
            foreach (KeyValuePair<string, StringValues> keyValuePair in this.HttpContext.Request.Query)
            {
                PropertyInfo property = type.GetProperty(keyValuePair.Key);
                if (!Equals(property, null))
                    property.SetValue(addModel, ControllerExtensions.Parse(keyValuePair.Value, property.PropertyType),
                        null);
            }

            return base.View("AddOrEdit", addModel);
        }

        [ExceptionHandling]
        [HttpPost]
        public virtual IActionResult Add(TAddModel model)
        {
            var obj = this.Dal.Add(model);
            // ReSharper disable once Mvc.ActionNotResolved
            return Ok(new
            {
                Redirect = Url.Action("Edit", new {id = obj.Id}),
            });
        }

        public virtual IActionResult Edit(long id)
        {
            return base.View("AddOrEdit", Dal.GetAddModel(id));
        }

        [ExceptionHandling]
        [HttpPost]
        public virtual IActionResult Edit(TAddModel model)
        {
            Dal.Edit(model);
            // ReSharper disable once Mvc.ActionNotResolved
            return Ok(new
            {
                Redirect = Url.Action(nameof(Edit), new {id = model.Id})
            });
        }

        public virtual IActionResult Delete(long id)
        {
            Dal.Delete(id);
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction("List");
        }

        public override ViewResult View(object model)
        {
            if (HttpContext.Request.Query.ContainsKey("X-Requested-With"))
            {
                ViewBag.RenderPartial = true;
            }

            return base.View(model);
        }

        public override ViewResult View(string viewName)
        {
            if (HttpContext.Request.Query.ContainsKey("X-Requested-With"))
            {
                ViewBag.RenderPartial = true;
            }

            return base.View(viewName);
        }

        public override ViewResult View(string viewName, object model)
        {
            if (HttpContext.Request.Query.ContainsKey("X-Requested-With"))
            {
                ViewBag.RenderPartial = true;
            }

            return base.View(viewName, model);
        }
    }
}