using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Utilities.Object
{
    public static class EnumHelper
    {
        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo property in resourceManagerProvider.GetProperties((BindingFlags) 56))
            {
                if (property.PropertyType == typeof(ResourceManager))
                    return ((ResourceManager) property.GetValue((object) null, (object[]) null)).GetString(resourceKey);
            }

            return resourceKey;
        }

        public static string ToDisplayString(this Enum value)
        {
            return Convert.ToInt32(value) == 0 ? "" : GetDisplayValue(value);
        }

        public static string GetDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes != null && descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            return descriptionAttributes == null
                ? string.Empty
                : (descriptionAttributes.Length > 0)
                    ? descriptionAttributes[0].Name
                    : value.ToString();
        }


        public static List<SelectListItem> ToSelectList(
            this List<Enum> enumerations,
            IEnumerable<long> selecteds = null)
        {
            selecteds = selecteds != null ? selecteds.ToList() : new List<long>();
            var result = new List<SelectListItem>();
            foreach (var enumeration in enumerations)
            {
                SelectListItem selectListItem =
                    new SelectListItem
                    {
                        Value = Convert.ToInt32(enumeration).ToString(),
                        Text = GetDisplayValue(enumeration),
                        Selected = selecteds.Contains(Convert.ToInt64(enumeration))
                    };
                result.Add(selectListItem);
            }

            return result;
        }

        public static List<SelectListItem> ToSelectList(
            this Enum enumeration,
            IEnumerable<long> selecteds = null)
        {
            selecteds = selecteds != null ? selecteds.ToList() : new List<long>();
            var result = new List<SelectListItem>();
            foreach (Enum item in Enum.GetValues(enumeration.GetType()))
            {
                SelectListItem selectListItem =
                    new SelectListItem
                    {
                        Value = Convert.ToInt32(item).ToString(),
                        Text = GetDisplayValue(item),
                        Selected = selecteds.Contains(Convert.ToInt64(item)) || Convert.ToInt64(enumeration) == Convert.ToInt64(item)
                    };
                result.Add(selectListItem);
            }

            return result;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}