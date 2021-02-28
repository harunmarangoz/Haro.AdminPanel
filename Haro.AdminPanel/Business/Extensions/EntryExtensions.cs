using System.Collections;
using System.Collections.Generic;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Extensions;

namespace Haro.AdminPanel.Business.Extensions
{
    public static class EntryExtensions
    {
        public static List<T> List<T>(int? max = null, string condition = "")
            where T : class
        {
            return List<T>(typeof(T).Name, max, condition);
        }

        public static List<T> List<T>(string tableName, int? max = null, string condition = "")
            where T : class
        {
            var q = $"SELECT ";
            if (max.HasValue)
            {
                q += $"TOP {max.Value}";
            }

            q += $" * FROM {tableName}";

            if (!string.IsNullOrEmpty(condition))
            {
                q += $" WHERE {condition}";
            }

            return new SqlProvider().List(q).ConvertToList<T>();
        }

        public static T GetById<T>(long id)
        {
            return new SqlProvider().Get($"SELECT * FROM {typeof(T).Name} WHERE Id={id}").ConvertToEntry<T>();
        }

        public static T GetByProperty<T>(string column, object value)
        {
            return new SqlProvider().Get($"{typeof(T).Name}", new List<ArrayList>()
            {
                new ArrayList() {column, value},
            }).ConvertToEntry<T>();
        }
    }
}