using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Models.CustomModels
{
    public class SetupModel
    {
        public string ProjectName { get; set; } = "Duall Ajans";
        public string WebProjectName { get; set; } = "ModulpanWebsite.WebApp";

        public string SmtpServer { get; set; } = "smtp.yandex.ru";
        public int SmtPort { get; set; } = 465;
        public string SmtpUserName { get; set; } = "noreply@duallajans.com";
        public string SmtpPassword { get; set; } = "duallipa1234?";
        public bool SmtpEnableSsl { get; set; } = true;

        public List<string> SelectedLanguages { get; set; }


        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public List<SelectListItem> LanguageList { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("Türkçe", "tr"),
            new SelectListItem("English", "en"),
        };
    }
}