using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Haro.AdminPanel.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Haro.AdminPanel.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (var context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public TEntity Add(TEntity entity)
        {
            using (var context = new TContext())
            {
                EntityEntry<TEntity> entityEntry = context.Entry(entity);
                entityEntry.State = EntityState.Added;
                context.SaveChanges();
                return entityEntry.Entity;
            }
        }

        public void Update(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(TEntity entity)
        {
            using (var context = new TContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            TContext context = new TContext();
            try
            {
                return filter == null
                    ? context.Set<TEntity>().Count()
                    : context.Set<TEntity>().Where(filter).Count();
            }
            finally
            {
                ((IDisposable) context).Dispose();
            }
        }

        public IQueryable<TEntity> ListQueryable()
        {
            return new TContext().Set<TEntity>().AsQueryable<TEntity>();
        }
    }
}