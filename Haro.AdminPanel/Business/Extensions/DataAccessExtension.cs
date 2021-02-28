using System.Linq;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Extensions
{
    public static class DataAccessExtension
    {
        public static T GetById<T>(this IQueryable<T> query, long id)
            where T : class, IEntity, new()
        {
            return query.FirstOrDefault<T>(x => x.Id == id);
        }
    }
}