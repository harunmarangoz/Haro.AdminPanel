using System.ComponentModel;

namespace Haro.AdminPanel.Models.CustomModels
{
    public class ContactFormModel
    {
        [DisplayName("Ad Soyad")]
        public string Name { get; set; }
        [DisplayName("Firma")]
        public string Company { get; set; }
        [DisplayName("Eposta")]
        public string Email { get; set; }
        [DisplayName("Telefon")]
        public string Phone { get; set; }
        [DisplayName("Konu")]
        public string Subject { get; set; }
        [DisplayName("Mesaj")]
        public string Message { get; set; }
    }
}