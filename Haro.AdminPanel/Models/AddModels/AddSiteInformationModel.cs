using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Models.AddModels
{
    public class AddSiteInformationModel : SiteInformation
    {
        public AddSiteInformationModel()
        {
        }

        public AddSiteInformationModel(SiteInformation model)
        {
            PropertyCopy.Copy(model, this);
        }
    }
}