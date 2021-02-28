using System.Collections.Generic;

namespace Haro.AdminPanel.Models.Entities
{
    public class Table : Entity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public bool MultipleLanguage { get; set; }
        public bool Show { get; set; }

        public Module Module { get; set; }
        public long ModuleId { get; set; }
        
        public List<Column> Columns { get; set; } = new List<Column>();
    }
}