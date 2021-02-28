using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;

namespace Haro.AdminPanel.Business.Managers
{
    public abstract class AddModelManager<T, TAddModel> : IAddModelManager<T, TAddModel>
        where T : class, IEntity, new()
        where TAddModel : T, new()
    {
        protected IEntityRepository<T> Dal { get; set; }

        public virtual IQueryable<T> BaseQuery() => Dal.ListQueryable();
        public virtual List<T> List() => BaseQuery().ToList();
        public virtual List<T> List(Expression<Func<T, bool>> expression) => BaseQuery().Where<T>(expression).ToList();
        public virtual T Add(TAddModel model) => Dal.Add(model);
        public abstract T Edit(TAddModel model);

        public virtual void Delete(long id)
        {
            Dal.Delete(GetById(id));
        }

        public virtual T Get(Expression<Func<T, bool>> expression) => BaseQuery().FirstOrDefault(expression);
        public virtual T GetById(long id) => BaseQuery().FirstOrDefault(x => x.Id == id);

        public virtual TAddModel GetAddModel() => new TAddModel();
        public abstract TAddModel GetAddModel(long id);
    }
}