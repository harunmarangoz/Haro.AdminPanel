namespace Haro.AdminPanel.Models.Entities
{
    public class SiteInformation : Entity
    {
        public string ProjectName { get; set; }
        public string WebProjectName { get; set; }

        public string SmtpServer { get; set; }
        public int SmtPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpEnableSsl { get; set; }
    }
}