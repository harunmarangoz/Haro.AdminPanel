using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddLanguageModel : Language
    {
        public AddLanguageModel()
        {
        }

        public AddLanguageModel(Language model)
        {
            PropertyCopy.Copy(model, this);
        }
    }
}