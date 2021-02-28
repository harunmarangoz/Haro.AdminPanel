namespace Haro.AdminPanel.Models.Entities
{
    public class Language : Entity
    {
        public string Code { get; set; }
        public string DisplayValue { get; set; }
        public string Image { get; set; }
        public bool IsDefault { get; set; }
    }
}