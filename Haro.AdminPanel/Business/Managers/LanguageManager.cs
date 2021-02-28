using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Managers
{
    public class LanguageManager : AddModelManager<Language,AddLanguageModel>
    {
        public LanguageManager(EfLanguageDal dal) { Dal = dal; }

        public override Language Edit(AddLanguageModel model)
        {
            var entry = GetById(model.Id);

            entry.Code = model.Code;
            entry.DisplayValue = model.DisplayValue;
            entry.Image = model.Image;
            
            Dal.Update(entry);
            return entry;
        }
        public override AddLanguageModel GetAddModel(long id) { return new AddLanguageModel(GetById(id)); }
    }
}