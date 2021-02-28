using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Haro.AdminPanel.Business.Extensions;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.Models.CustomModels;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Media;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Column = Haro.AdminPanel.Models.Entities.Column;

namespace Haro.AdminPanel.Business.Managers
{
    public class DashboardManager
    {
        private TableManager _tableManager;
        private ColumnManager _columnManager;
        private SqlProvider _sqlProvider;
        private LanguageManager _languageManager;

        public DashboardManager(TableManager tableManager, ColumnManager columnManager, SqlProvider sqlProvider,
            LanguageManager languageManager)
        {
            _tableManager = tableManager;
            _columnManager = columnManager;
            _sqlProvider = sqlProvider;
            _languageManager = languageManager;
        }

        public TableListModel GetTableListModel(long id)
        {
            var result = new TableListModel();
            result.Table = _tableManager.GetById(id);
            result.Columns = _columnManager.List(x => x.TableId == id).OrderBy(x => x.ListOrder).ToList();


            var q = $"SELECT {result.Table.Name}.Id AS Id,";
            foreach (var item in result.Columns.Where(x => x.ShowInList))
            {
                switch (item.ColumnType)
                {
                    case ColumnType.SelectList:
                        q += $"{item.TargetTable}.{item.TargetTableTextColumn} AS {item.Name},";
                        break;
                    default:
                        q += $"{result.Table.Name}.{item.Name} AS {item.Name},";
                        break;
                }
            }

            q = q.Trim(',');
            q += $" FROM {result.Table.Name}\n";

            if (result.Columns.Any(x => x.ShowInList && x.ColumnType == ColumnType.SelectList))
            {
                foreach (var item in result.Columns.Where(x => x.ShowInList && x.ColumnType == ColumnType.SelectList))
                {
                    q += $"LEFT JOIN {item.TargetTable} ON {item.TargetTable}.Id={result.Table.Name}.{item.Name} \n";
                }
            }

            if (result.Table.MultipleLanguage)
            {
                q += $" WHERE {result.Table.Name}.LanguageId=" + App.Common.Language.Id;
            }

            result.Entries = new SqlProvider().List(q);
            return result;
        }

        public AddRecordModel GetTableAddRecordModel(long tableId, long? entryId = null, long? languageId = null)
        {
            var result = new AddRecordModel();
            result.Table = _tableManager.GetById(tableId);

            if (entryId.HasValue)
            {
                var getQuery = $"SELECT * FROM {result.Table.Name} WHERE Id = " + entryId.Value;
                result.Entry = _sqlProvider.Get(getQuery);
                result.Language = _languageManager.GetById(Convert.ToInt64(result.Entry["LanguageId"]));
                result.LanguagePairId = Convert.ToInt64(result.Entry["LanguagePairId"]);
            }
            else
            {
                result.Language = languageId.HasValue ? _languageManager.GetById(languageId.Value) : App.Common.Language;
            }

            foreach (var item in _columnManager.List(x => x.TableId == tableId).OrderBy(x => x.FormOrder).ToList())
            {
                var model = new ColumnInputModel();
                model.Id = item.Id;
                model.Name = item.Name;
                model.DisplayName = item.DisplayName;
                model.ColumnType = item.ColumnType;
                model.InputExtra = item.InputExtra;
                if (result.Entry != null && !(item.ColumnType == ColumnType.MultipleImage ||
                                              item.ColumnType == ColumnType.MultipleSelectList))
                {
                    model.Value = result.Entry[model.Name]?.ToString();
                }

                var table = _tableManager.GetById(item.TargetTableId);
                string q;
                switch (item.ColumnType)
                {
                    case ColumnType.SelectList:
                        q = $"SELECT Id,{item.TargetTableTextColumn} FROM {item.TargetTable}";
                        if (table.MultipleLanguage)
                        {
                            q += " WHERE LanguageId=" + App.Common.Language.Id;
                        }

                        foreach (DataRow row in _sqlProvider.List(q).Rows)
                        {
                            model.SelectListItems.Add(new SelectListItem()
                            {
                                Text = row[item.TargetTableTextColumn]?.ToString(),
                                Value = row["Id"]?.ToString(),
                                Selected = row["Id"]?.ToString() == model.Value
                            });
                        }

                        break;
                    case ColumnType.MultipleSelectList:
                        q = $"SELECT Id,{item.TargetTableTextColumn} FROM {item.TargetTable}";
                        if (table.MultipleLanguage)
                        {
                            q += " WHERE LanguageId=" + App.Common.Language.Id;
                        }

                        var currentEntries = new List<long>();
                        if (entryId.HasValue)
                        {
                            var currentEntriesTable = _sqlProvider
                                .List(
                                    $"SELECT * FROM {result.Table.Name}{item.TargetTable} WHERE {result.Table.Name}Id={entryId}");
                            foreach (DataRow row in currentEntriesTable.Rows)
                            {
                                currentEntries.Add(Convert.ToInt64(row[item.TargetTable + "Id"]));
                            }
                        }

                        foreach (DataRow row in _sqlProvider.List(q).Rows)
                        {
                            model.SelectListItems.Add(new SelectListItem()
                            {
                                Text = row[item.TargetTableTextColumn]?.ToString(),
                                Value = row["Id"].ToString(),
                                Selected = currentEntries.Contains(Convert.ToInt64(row["Id"].ToString()))
                            });
                        }

                        break;
                    case ColumnType.MultipleImage:
                        break;
                }

                result.Columns.Add(model);
            }


            return result;
        }

        public void AddRecord(IFormCollection form)
        {
            long tableId = Convert.ToInt64(form["TableId"].ToString());
            var table = _tableManager.GetById(tableId);
            var parameters = new List<ArrayList>();
            var columns = _columnManager.List(x => x.TableId == tableId);
            foreach (var item in columns)
            {
                if (!(item.ColumnType == ColumnType.Image || item.ColumnType == ColumnType.Bool) &&
                    !form.ContainsKey(item.Name)) continue;
                var arrayList = ProcessColumn(item, form);
                if (arrayList == null) continue;
                parameters.Add(arrayList);
            }

            parameters.Add(new ArrayList() {"LanguageId", form["LanguageId"]});
            if (form.ContainsKey("LanguagePairId"))
            {
                parameters.Add(new ArrayList() {"LanguagePairId", form["LanguagePairId"]});
            }

            var entryId = _sqlProvider.Save(table.Name, parameters);
            if (!form.ContainsKey("LanguagePairId"))
            {
                _sqlProvider.Update(table.Name, new List<ArrayList>()
                    {
                        new ArrayList() {"LanguagePairId", entryId}
                    },
                    new List<ArrayList>()
                    {
                        new ArrayList() {"Id", entryId}
                    });
            }

            foreach (var item in columns.Where(x => x.ColumnType == ColumnType.MultipleSelectList))
            {
                switch (item.ColumnType)
                {
                    case ColumnType.MultipleSelectList:

                        var selectedEntriesStr = form[item.Name].ToString();
                        var selectedEntries = string.IsNullOrEmpty(selectedEntriesStr)
                            ? new List<long>()
                            : selectedEntriesStr.Split(',').Select(x => Convert.ToInt64(x))
                                .ToList();

                        selectedEntries.ToList().ForEach(x =>
                            _sqlProvider.Save(table.Name + item.TargetTable, new List<ArrayList>()
                            {
                                new ArrayList() {table.Name + "Id", entryId},
                                new ArrayList() {item.TargetTable + "Id", x},
                            })
                        );
                        break;
                }
            }
        }

        public void EditRecord(IFormCollection form)
        {
            long tableId = Convert.ToInt64(form["TableId"].ToString());
            long entryId = Convert.ToInt64(form["Id"].ToString());
            var parameters = new List<ArrayList>();
            var columns = _columnManager.List(x => x.TableId == tableId);
            foreach (var item in columns)
            {
                if (!(item.ColumnType == ColumnType.Image || item.ColumnType == ColumnType.Bool) &&
                    !form.ContainsKey(item.Name)) continue;
                var arrayList = ProcessColumn(item, form);
                if (arrayList == null) continue;
                parameters.Add(arrayList);
            }

            var table = _tableManager.GetById(tableId);
            _sqlProvider.Update(table.Name, parameters, new List<ArrayList>() {new ArrayList() {"Id", entryId}});

            foreach (var item in columns.Where(x => x.ColumnType == ColumnType.MultipleSelectList))
            {
                var currentEntries = new List<long>();
                switch (item.ColumnType)
                {
                    case ColumnType.MultipleSelectList:

                        foreach (DataRow row in _sqlProvider
                            .List($"SELECT * FROM {table.Name}{item.TargetTable} WHERE {table.Name}Id={entryId}").Rows)
                        {
                            currentEntries.Add(Convert.ToInt64(row[item.TargetTable + "Id"]));
                        }

                        var selectedEntriesStr = form[item.Name].ToString();
                        var selectedEntries = string.IsNullOrEmpty(selectedEntriesStr)
                            ? new List<long>()
                            : selectedEntriesStr.Split(',').Select(x => Convert.ToInt64(x))
                                .ToList();

                        if (UnorderedListEqualHelper.UnorderedEqual(currentEntries, selectedEntries)) continue;

                        currentEntries.Except(selectedEntries).ToList().ForEach(x =>
                            _sqlProvider.Delete(table.Name + item.TargetTable, new List<ArrayList>()
                            {
                                new ArrayList() {table.Name + "Id", entryId},
                                new ArrayList() {item.TargetTable + "Id", x},
                            })
                        );
                        selectedEntries.Except(currentEntries).ToList().ForEach(x =>
                            _sqlProvider.Save(table.Name + item.TargetTable, new List<ArrayList>()
                            {
                                new ArrayList() {table.Name + "Id", entryId},
                                new ArrayList() {item.TargetTable + "Id", x},
                            })
                        );
                        break;
                }
            }
        }

        public void DeleteRecord(long tableId, long entryId)
        {
            var table = _tableManager.GetById(tableId);
            _sqlProvider.Delete(table.Name, new List<ArrayList>() {new ArrayList() {"Id", entryId}});
        }

        private ArrayList ProcessColumn(Column column, IFormCollection form)
        {
            switch (column.ColumnType)
            {
                case ColumnType.Text:
                case ColumnType.TextArea:
                case ColumnType.Editor:
                case ColumnType.Password:
                case ColumnType.SelectList:
                case ColumnType.Number:
                case ColumnType.Slug:
                    return new ArrayList() {column.Name, form[column.Name]};
                case ColumnType.MultipleSelectList:
                    return null;
                case ColumnType.Image:
                    var file = form.Files[column.Name];
                    if (file != null)
                    {
                        return new ArrayList() {column.Name, "".Media(file)};
                    }

                    return null;
                case ColumnType.MultipleImage:
                    return null;
                case ColumnType.Bool:
                    return new ArrayList() {column.Name, form.ContainsKey(column.Name)};
                case ColumnType.Hidden:
                    return null;
                default:
                    throw new Exception("bilinmeyen");
            }
        }

        public GalleryModel GetGalleryModel(long columnId, long entryId)
        {
            var model = new GalleryModel();
            model.Column = _columnManager.BaseQuery().Include(x => x.Table).GetById(columnId);
            if (model.Column.ColumnType != ColumnType.MultipleImage) throw new Exception("Kolon tipi uygun değil.");
            model.ColumnId = model.Column.Id;
            model.EntryId = entryId;
            model.Name = model.Column.Table.DisplayName + ", " + model.Column.DisplayName;

            foreach (DataRow row in _sqlProvider
                .List(
                    $"SELECT * FROM {model.Column.Table.Name}{model.Column.Name}Image WHERE {model.Column.Table.Name}Id={entryId}")
                .Rows)
            {
                model.GalleryImages.Add(new GalleryImage()
                {
                    Id = Convert.ToInt64(row["Id"]),
                    Image = row["Image"].ToString(),
                });
            }

            return model;
        }

        public void AddGalleryImages(GalleryModel model)
        {
            var column = _columnManager.BaseQuery().Include(x => x.Table).GetById(model.ColumnId);

            model.ImageFiles.ForEach(x =>
            {
                _sqlProvider.Save($"{column.Table.Name}{column.Name}Image", new List<ArrayList>()
                {
                    new ArrayList() {"Image", model.Name.Media(x)},
                    new ArrayList() {$"{column.Table.Name}Id", model.EntryId},
                    new ArrayList() {"OrderNumber", "0"}
                });
            });
        }

        public void DeleteGalelryImage(long columnId, long entryId)
        {
            var column = _columnManager.BaseQuery().Include(x => x.Table).GetById(columnId);

            _sqlProvider.Delete($"{column.Table.Name}{column.Name}Image",
                new List<ArrayList>() {new ArrayList() {"Id", entryId}});
        }

        public long? FindPairedEntries(long tableId, long languageId, long languagePairId)
        {
            var table = _tableManager.GetById(tableId);

            var sql = $"SELECT * FROM {table.Name} WHERE LanguageId={languageId} AND LanguagePairId={languagePairId}";
            var row = _sqlProvider.Get(sql);
            if (row == null) return null;

            return Convert.ToInt64(row["Id"]);
        }
    }
}