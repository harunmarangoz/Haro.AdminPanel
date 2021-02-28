using System;
using Haro.AdminPanel.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.DataAccess.EntityFramework
{
    public class EfTableDal : EfEntityRepositoryBase<Table, AdminPanelContext>
    {
        public void CreateTable(string entryName)
        {
            using (var context = new AdminPanelContext())
            {
                var q = $"CREATE TABLE {entryName} (" +
                        $"Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY," +
                        $"LanguageId BIGINT CONSTRAINT FK_{entryName}_Languages FOREIGN KEY ([LanguageId]) REFERENCES Languages(Id)," +
                        $"LanguagePairId BIGINT)";
                context.Database.ExecuteSqlRaw(q);
            }
        }
    }
}