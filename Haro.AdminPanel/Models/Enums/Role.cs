using System.ComponentModel.DataAnnotations;

namespace Haro.AdminPanel.Models.Enums
{
    public enum Role
    {
        [Display(Name = "Süper Admin")] SuperAdmin = 1,
        [Display(Name = "Admin")] Admin = 2,
        [Display(Name = "Limitli Rol")] LimitedUser = 3,
        [Display(Name = "Teknik Destek")] Support = 4,
    }
}