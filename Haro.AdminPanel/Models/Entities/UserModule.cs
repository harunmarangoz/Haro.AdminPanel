namespace Haro.AdminPanel.Models.Entities
{
    public class UserModule : Entity
    {
        public User User { get; set; }
        public long UserId { get; set; }

        public Module Module { get; set; }
        public long ModuleId { get; set; }
    }
}