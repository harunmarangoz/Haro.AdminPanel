using Haro.AdminPanel.Models.Enums;

namespace Haro.AdminPanel.Models.Entities
{
    public class Column : Entity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ColumnType ColumnType { get; set; } = ColumnType.Text;
        public string ColumnSize { get; set; } = "12";
        public int FormOrder { get; set; }
        public bool ShowInList { get; set; }
        public int ListOrder { get; set; }

        public string TargetTable { get; set; }
        public string TargetTableTextColumn { get; set; }
        public long TargetTableId { get; set; }
        public long TargetColumnId { get; set; }


        public int Min { get; set; }
        public int Max { get; set; }

        public string InputExtra { get; set; }
        
        public Table Table { get; set; }
        public long TableId { get; set; }
    }
}