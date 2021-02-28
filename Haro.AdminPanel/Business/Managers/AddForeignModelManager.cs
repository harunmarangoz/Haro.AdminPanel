using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Managers
{
    public abstract class AddForeignModelManager<T> where T : class, IEntity, new()
    {
        protected IEntityRepository<T> Dal { get; set; }

        public abstract T Add(long firstId, long secondId);

        public abstract void Remove(long firstId, long secondId);

        public virtual List<T> List(Expression<Func<T, bool>> func = null)
        {
            return Dal.GetList(func);
        }
    }
}