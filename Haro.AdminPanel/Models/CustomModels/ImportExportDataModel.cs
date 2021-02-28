using System.Collections.Generic;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Models.CustomModels
{
    public class ImportExportDataModel
    {
        public List<Language> Languages { get; set; }
        public List<Module> Modules { get; set; }
        public List<Table> Tables { get; set; }
        public List<Column> Columns { get; set; }
        public List<SiteInformation> SiteInformations { get; set; }
        public List<User> Users { get; set; }
        public List<UserModule> UserModules { get; set; }
    }
}