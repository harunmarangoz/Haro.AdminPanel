using System.Collections.Generic;
using Haro.AdminPanel.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Haro.AdminPanel.Models.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }

        public List<UserModule> Modules { get; set; }
    }
}