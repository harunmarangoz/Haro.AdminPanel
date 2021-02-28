using Haro.AdminPanel.Models.CustomModels;

namespace Haro.AdminPanel.Utilities.Extensions
{
    public static class ContactFormExtensions
    {
        public static string CreateMailBody(this ContactFormModel model)
        {
            var body = "";
            body += MailLine(model.Name, "Ad Soyad");
            body += MailLine(model.Company, "Firma");
            body += MailLine(model.Email, "Eposta");
            body += MailLine(model.Phone, "Telefon");
            body += MailLine(model.Subject, "Konu");
            body += MailLine(model.Message, "Mesaj");
            return body;
        }
        private static string MailLine(string str, string title)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return $"<p><b>{title} : </b>{str}</p>";
            }
            return "";
        }
    }
}