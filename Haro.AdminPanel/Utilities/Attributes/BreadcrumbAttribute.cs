using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Haro.AdminPanel.Utilities.Attributes
{
    public class BreadcrumbAttribute : ActionFilterAttribute
    {
        private readonly string _name;

        public BreadcrumbAttribute(string name)
        {
            _name = name;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            base.OnActionExecuting(context);
            if (!(context.Controller is Controller controller))
                return;
            if (!string.IsNullOrEmpty(_name))
            {
                controller.ViewBag.PageTitle = _name;
            }
        }
    }
}