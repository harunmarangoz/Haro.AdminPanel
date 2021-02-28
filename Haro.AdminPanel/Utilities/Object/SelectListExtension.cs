using System;
using System.Collections.Generic;
using System.Linq;
using Haro.AdminPanel.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haro.AdminPanel.Utilities.Object
{
    public static class SelectListExtension
    {
        public static List<SelectListItem> ToSelectListItem(this IEnumerable<IEntity> list) =>
            list.ToSelectListItem(new long?());

        public static List<SelectListItem> ToSelectListItem(this IEnumerable<IEntity> list, long? selectedValue)
            => list.ToSelectListItem(selectedValue.HasValue ? new List<long>() {selectedValue.Value} : null);

        public static List<SelectListItem> ToSelectListItem(this IEnumerable<IEntity> list,
            IEnumerable<long> selectedValues, bool addFirstNull = true)
        {
            if (selectedValues == null) selectedValues = new List<long>();
            var items = list.ToList();
            if (items.Count == 0) return new List<SelectListItem>();

            string propertyName = "";
            Type type = items.First().GetType();
            if (!Equals(type.GetProperty("Name"), null)) propertyName = "Name";
            else if (!Equals(type.GetProperty("Title"), null)) propertyName = "Title";

            var result = items.Select(x =>
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = x.GetType().GetProperty(propertyName)?.GetValue(x, null)?.ToString(),
                    Value = x.Id.ToString(),
                    Selected = selectedValues.Contains(x.Id)
                };
                return selectListItem;
            }).ToList().OrderBy(x => x.Text).ToList();
            if (addFirstNull)
            {
                var selectListItem = new SelectListItem {Text = "Seçiniz", Value = ""};
                result.Insert(0, selectListItem);
            }

            return result;
        }
    }
}