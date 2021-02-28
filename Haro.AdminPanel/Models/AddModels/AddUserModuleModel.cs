using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddUserModuleModel : UserModule
    {
        public AddUserModuleModel()
        {
        }

        public AddUserModuleModel(UserModule model)
        {
            PropertyCopy.Copy(model, this);
        }
    }
}