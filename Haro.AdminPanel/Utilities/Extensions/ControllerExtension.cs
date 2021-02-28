using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Haro.AdminPanel.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;

namespace Haro.AdminPanel.Utilities.Extensions
{
    public static class ControllerExtensions
    {
        public static string RenderView(
            this Controller controller,
            string viewName,
            object model,
            bool partial = false)
        {
            controller.ViewBag.RenderPartial = partial;
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            controller.ViewData.Model = model;
            using (StringWriter stringWriter = new StringWriter())
            {
                IViewEngine service =
                    controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                        ICompositeViewEngine;
                if (service != null)
                {
                    ViewEngineResult view = service.FindView(controller.ControllerContext, viewName, !partial);
                    if (!view.Success)
                        return "A view with the name " + viewName + " could not be found";
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, view.View,
                        controller.ViewData, controller.TempData, stringWriter, new HtmlHelperOptions());
                    controller.ViewBag.RenderPartial = partial;

                    view.View.RenderAsync(viewContext).Wait();
                }

                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public static object Parse(string valueToConvert, Type dataType)
        {
            return TypeDescriptor.GetConverter(dataType)
                .ConvertFromString(null, CultureInfo.InvariantCulture, valueToConvert);
        }
    }
}