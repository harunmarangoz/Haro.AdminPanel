using System.Linq;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.Areas.Admin.ViewComponents
{
    [ViewComponent]
    public class SideMenuViewComponent : ViewComponent
    {
        private ModuleManager _moduleManager;

        public SideMenuViewComponent(ModuleManager moduleManager)
        {
            _moduleManager = moduleManager;
        }

        public IViewComponentResult Invoke()
        {
            var query = _moduleManager.BaseQuery()
                .Include(x => x.Tables).AsQueryable();
            if (App.Common.User.Role == Role.LimitedUser)
            {
                var moduleIds = App.Common.User.Modules.Select(x => x.ModuleId).ToList();
                query = query.Where(x => moduleIds.Contains(x.Id));
            }
            query = query.OrderBy(x => x.DisplayOrder);
            return View(query.ToList());
        }
    }
}