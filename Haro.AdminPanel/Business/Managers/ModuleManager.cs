using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Managers
{
    public class ModuleManager : AddModelManager<Module, AddModuleModel>
    {
        public ModuleManager(EfModuleDal dal)
        {
            Dal = dal;
        }

        public override Module Edit(AddModuleModel model)
        {
            var entry = GetById(model.Id);
            entry.Name = model.Name;
            entry.DisplayOrder = model.DisplayOrder;
            Dal.Update(entry);
            return entry;
        }

        public override AddModuleModel GetAddModel(long id)
        {
            return new AddModuleModel(GetById(id));
        }
    }
}