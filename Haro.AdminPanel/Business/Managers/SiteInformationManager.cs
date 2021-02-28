using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Managers
{
    public class SiteInformationManager : AddModelManager<SiteInformation,AddSiteInformationModel>
    {
        public SiteInformationManager(EfSiteInformationDal dal) { Dal = dal; }

        public override SiteInformation Edit(AddSiteInformationModel model)
        {
            var entry = GetById(model.Id);
            
            entry.ProjectName = model.ProjectName;
            entry.WebProjectName = model.WebProjectName;
            entry.SmtpServer = model.SmtpServer;
            entry.SmtPort = model.SmtPort;
            entry.SmtpUserName = model.SmtpUserName;
            entry.SmtpPassword = model.SmtpPassword;
            entry.SmtpEnableSsl = model.SmtpEnableSsl;
            
            Dal.Update(entry);
            return entry;
        }
        public override AddSiteInformationModel GetAddModel(long id) { return new AddSiteInformationModel(GetById(id)); }
    }
}