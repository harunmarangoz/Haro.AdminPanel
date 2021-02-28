using System.Collections.Generic;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddColumnModel : Column
    {
        public AddColumnModel()
        {
        }

        public AddColumnModel(Column model)
        {
            PropertyCopy.Copy(model, this);
        }

        public List<SelectListItem> ColumnTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> TargetTableList { get; set; } = new List<SelectListItem>();
        
    }
}