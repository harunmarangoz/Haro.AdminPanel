using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Haro.AdminPanel.Utilities.Attributes
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            string str = context.Exception.InnerException?.Message ?? context.Exception.Message;

            var st = new StackTrace(context.Exception, true);
            var frame = st.GetFrame(0);
            int line = 0;
            string file = "";
            if (frame != null)
            {
                line = frame.GetFileLineNumber();
                file = frame.GetFileName();
            }

            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 500;
            context.Result = (new JsonResult(new
            {
                Status = 400,
                Message = str,
                Source = $"{file}:{line}"
            }));
            context.ExceptionHandled = true;
        }
    }
}