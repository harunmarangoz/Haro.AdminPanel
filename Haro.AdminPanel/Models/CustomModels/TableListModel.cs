using System.Collections.Generic;
using System.Data;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Models.CustomModels
{
    public class TableListModel
    {
        public List<Column> Columns { get; set; }

        public DataTable Entries { get; set; }
        public Table Table { get; set; }
    }

    public class AddRecordModel
    {
        public List<ColumnInputModel> Columns { get; set; } = new List<ColumnInputModel>();
        public Table Table { get; set; }
        public DataRow Entry { get; set; }
        public Language Language { get; set; }
        public long? LanguagePairId { get; set; }
    }

    public class ColumnInputModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string InputExtra { get; set; }
        public ColumnType ColumnType { get; set; }
        public List<SelectListItem> SelectListItems { get; set; } = new List<SelectListItem>();
    }
}