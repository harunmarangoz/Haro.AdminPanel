using System.Collections.Generic;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Managers
{
    public interface IAddModelManager<T, TAddModel>
        where T : class, IEntity, new()
        where TAddModel : T
    {
        List<T> List();
        T Add(TAddModel model);
        T Edit(TAddModel model);
        void Delete(long id);
        T GetById(long id);
        TAddModel GetAddModel();
        TAddModel GetAddModel(long id);
    }
}