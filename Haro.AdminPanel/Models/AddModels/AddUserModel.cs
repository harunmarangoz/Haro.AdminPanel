using System.Collections.Generic;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddUserModel : User
    {
        public AddUserModel()
        {
        }

        public AddUserModel(User model)
        {
            PropertyCopy.Copy(model, this);
        }

        public string Password { get; set; }
        public string PasswordControl { get; set; }
        public List<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ModuleList { get; set; } = new List<SelectListItem>();
        public List<long> SelectedModules { get; set; } = new List<long>();
    }
}