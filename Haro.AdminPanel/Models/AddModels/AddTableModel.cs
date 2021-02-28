using System.Collections.Generic;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddTableModel : Table
    {
        public AddTableModel()
        {
        }

        public AddTableModel(Table model)
        {
            PropertyCopy.Copy(model, this);
        }

        public List<SelectListItem> ModuleList { get; set; }
    }
}