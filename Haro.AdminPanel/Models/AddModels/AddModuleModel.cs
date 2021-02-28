using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddModuleModel : Module
    {
        public AddModuleModel()
        {
        }

        public AddModuleModel(Module model)
        {
            PropertyCopy.Copy(model, this);
        }
    }
}