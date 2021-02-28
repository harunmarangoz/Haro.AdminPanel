using System;
using System.Collections.Generic;
using System.Linq;
using Haro.AdminPanel.Business.Managers;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Business.Extensions
{
    public static class ForeigModelExtensions
    {
        public static void Update<T>(
            this AddForeignModelManager<T> manager,
            long entryId,
            List<long> currentEntries,
            List<long> selectedEntries)
            where T : class, IEntity, new()
        {
            if (UnorderedListEqualHelper.UnorderedEqual(currentEntries, selectedEntries))
                return;
            currentEntries.Except(selectedEntries).ToList().ForEach(x => manager.Remove(entryId, x));
            selectedEntries.Except(currentEntries).ToList().ForEach(x => manager.Add(entryId, x));
        }
    }
}