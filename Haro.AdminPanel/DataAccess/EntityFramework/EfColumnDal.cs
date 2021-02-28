using System;
using System.Linq;
using Haro.AdminPanel.Business.Extensions;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.DataAccess.EntityFramework
{
    public class EfColumnDal : EfEntityRepositoryBase<Column, AdminPanelContext>
    {
        public void CreateColumn(long id)
        {
            using (var context = new AdminPanelContext())
            {
                var entry = context.Columns.AsQueryable()
                    .Include(x => x.Table)
                    .GetById(id);
                string q;
                switch (entry.ColumnType)
                {
                    case ColumnType.Text:
                    case ColumnType.TextArea:
                    case ColumnType.Image:
                    case ColumnType.Editor:
                    case ColumnType.Number:
                    case ColumnType.Bool:
                    case ColumnType.Password:
                    case ColumnType.Slug:
                    case ColumnType.Hidden:
                        q = $"ALTER TABLE {entry.Table.Name} ADD {entry.Name} {ColumnTypeToStr(entry.ColumnType)};";
                        break;
                    case ColumnType.SelectList:
                        q =
                            $"ALTER TABLE {entry.Table.Name} ADD {entry.Name} BIGINT CONSTRAINT FK_{entry.Table.Name}_{entry.TargetTable} FOREIGN KEY ([{entry.Name}]) REFERENCES {entry.TargetTable}(Id)";
                        break;
                    case ColumnType.MultipleSelectList:
                        q = $"CREATE TABLE {entry.Table.Name}{entry.TargetTable} (" +
                            $"Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY," +
                            $"{entry.Table.Name}Id BIGINT CONSTRAINT FK_{entry.Table.Name}_{entry.TargetTable} FOREIGN KEY ({entry.Table.Name}Id) REFERENCES {entry.Table.Name}(Id)," +
                            $"{entry.TargetTable}Id BIGINT CONSTRAINT FK_{entry.TargetTable}_{entry.Table.Name} FOREIGN KEY ({entry.TargetTable}Id) REFERENCES {entry.TargetTable}(Id)" +
                            $")";
                        break;
                    case ColumnType.MultipleImage:
                        q = $"CREATE TABLE {entry.Table.Name}{entry.Name}Image(" +
                            $"Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY," +
                            $"Image VARCHAR(250)," +
                            $"OrderNumber INT," +
                            $"{entry.Table.Name}Id BIGINT NOT NULL" +
                            $")";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                context.Database.ExecuteSqlRaw(q);
            }
        }

        public static string ColumnTypeToStr(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Text:
                case ColumnType.Image:
                case ColumnType.Number:
                case ColumnType.Slug:
                    return "VARCHAR(MAX)";
                case ColumnType.TextArea:
                case ColumnType.Editor:
                    return "TEXT";
                case ColumnType.SelectList:
                    return "BIGINT";
                case ColumnType.Bool:
                    return "BIT";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void DeleteColumn(long id)
        {
            using (var context = new AdminPanelContext())
            {
                var entry = context.Columns.AsQueryable()
                    .Include(x => x.Table)
                    .GetById(id);
                string q;
                switch (entry.ColumnType)
                {
                    case ColumnType.Text:
                    case ColumnType.TextArea:
                    case ColumnType.Editor:
                    case ColumnType.Image:
                    case ColumnType.Password:
                    case ColumnType.Bool:
                    case ColumnType.Number:
                    case ColumnType.Slug:
                        q = $"ALTER TABLE {entry.Table.Name} DROP COLUMN {entry.Name} ";
                        break;
                    case ColumnType.SelectList:
                        q =
                            $"ALTER TABLE {entry.Table.Name} DROP CONSTRAINT FK_{entry.Table.Name}_{entry.TargetTable} " +
                            $"ALTER TABLE {entry.Table.Name} DROP COLUMN {entry.Name} ";
                        break;
                    case ColumnType.MultipleSelectList:
                        q =
                            $"ALTER TABLE {entry.Table.Name}{entry.TargetTable} DROP CONSTRAINT FK_{entry.Table.Name}_{entry.TargetTable} " +
                            $"ALTER TABLE {entry.Table.Name}{entry.TargetTable} DROP CONSTRAINT FK_{entry.TargetTable}_{entry.Table.Name} " +
                            $"DROP TABLE {entry.Table.Name}{entry.TargetTable} ";
                        break;
                    case ColumnType.MultipleImage:
                        q =
                            $"ALTER TABLE {entry.Table.Name}{entry.Name}Image DROP CONSTRAINT FK_{entry.Table.Name}_{entry.Table.Name}{entry.Name}Image " +
                            $"DROP TABLE {entry.Table.Name}{entry.Name}Image ";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                context.Database.ExecuteSqlRaw(q);
            }
        }
    }
}