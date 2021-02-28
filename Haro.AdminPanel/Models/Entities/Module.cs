using System.Collections.Generic;

namespace Haro.AdminPanel.Models.Entities
{
    public class Module : Entity
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }

        public List<Table> Tables { get; set; }
        public List<UserModule> Users { get; set; }
    }
}