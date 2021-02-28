using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Haro.AdminPanel.Utilities.Extensions
{
    public static class SqlExtensions
    {
        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                return objT;
            }).ToList();
        }

        public static T ConvertToEntry<T>(this DataRow dr)
        {
            var columnNames = dr.Table.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            var objT = Activator.CreateInstance<T>();
            foreach (var pro in properties)
            {
                if (columnNames.Contains(pro.Name.ToLower()))
                {
                    try
                    {
                        pro.SetValue(objT, dr[pro.Name]);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            return objT;
        }
    }
}