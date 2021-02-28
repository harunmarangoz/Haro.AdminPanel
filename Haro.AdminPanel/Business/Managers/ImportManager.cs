using System.Linq;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.CustomModels;

namespace Haro.AdminPanel.Business.Managers
{
    public class ImportManager
    {
        private TableManager _tableManager;
        private ModuleManager _moduleManager;
        private ColumnManager _columnManager;
        private LanguageManager _languageManager;
        private SiteInformationManager _siteInformationManager;
        private UserManager _userManager;
        private UserModuleManager _userModuleManager;

        public ImportManager(TableManager tableManager, ModuleManager moduleManager, ColumnManager columnManager, LanguageManager languageManager, SiteInformationManager siteInformationManager, UserManager userManager, UserModuleManager userModuleManager)
        {
            _tableManager = tableManager;
            _moduleManager = moduleManager;
            _columnManager = columnManager;
            _languageManager = languageManager;
            _siteInformationManager = siteInformationManager;
            _userManager = userManager;
            _userModuleManager = userModuleManager;
        }
    }
}