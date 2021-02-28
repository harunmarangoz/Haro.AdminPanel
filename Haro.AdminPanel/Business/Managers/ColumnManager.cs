using System;
using System.Linq;
using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Object;

namespace Haro.AdminPanel.Business.Managers
{
    public class ColumnManager : AddModelManager<Column, AddColumnModel>
    {
        private EfColumnDal _dal;
        private TableManager _tableManager;

        public ColumnManager(EfColumnDal dal, TableManager tableManager)
        {
            Dal = dal;
            _dal = dal;
            _tableManager = tableManager;
        }

        public override Column Add(AddColumnModel model)
        {
            if (string.IsNullOrEmpty(model.Name)) throw new Exception("Kolon ismi boş olamaz.");
            var columnControlEntry = Dal.Get(x => x.Name == model.Name && x.TableId == model.TableId);
            if (columnControlEntry != null) throw new Exception("Bu kolon isminden daha önce oluşturulmuş.");
            switch (model.ColumnType)
            {
                case ColumnType.MultipleSelectList:
                case ColumnType.SelectList:
                    var targetTable = _tableManager.GetById(model.TargetTableId);
                    var targetColumn = GetById(model.TargetColumnId);
                    model.TargetTable = targetTable.Name;
                    model.TargetTableTextColumn = targetColumn.Name;
                    break;
            }

            if (model.ColumnType == ColumnType.MultipleImage || model.ColumnType == ColumnType.MultipleSelectList ||
                model.ColumnType == ColumnType.TextArea || model.ColumnType == ColumnType.Editor)
            {
                model.ShowInList = false;
            }

            var entry = base.Add(model);
            _dal.CreateColumn(entry.Id);
            return entry;
        }

        public override Column Edit(AddColumnModel model)
        {
            var entry = GetById(model.Id);

            entry.Max = model.Max;
            entry.Max = model.Max;
            entry.ColumnSize = model.ColumnSize;
            entry.DisplayName = model.DisplayName;
            entry.FormOrder = model.FormOrder;
            entry.ShowInList = model.ShowInList;

            Dal.Update(entry);
            return entry;
        }

        public override AddColumnModel GetAddModel()
        {
            var columnTypeList = ((ColumnType) 0).ToSelectList();
            columnTypeList.RemoveAll(x => x.Value == "10" || x.Value == "11");
            return new AddColumnModel()
            {
                ColumnTypeList = columnTypeList,
                TargetTableList = _tableManager.List().ToSelectListItem(),
            };
        }

        public override AddColumnModel GetAddModel(long id)
        {
            var entry = GetById(id);
            var tableList = _tableManager.List(x => x.Id != entry.TableId).ToSelectListItem();
            var columnTypeList = entry.ColumnType.ToSelectList();
            columnTypeList.RemoveAll(x => x.Value == "10" || x.Value == "11");
            return new AddColumnModel(entry)
            {
                ColumnTypeList = columnTypeList,
                TargetTableList = tableList,
            };
        }

        public override void Delete(long id)
        {
            _dal.DeleteColumn(id);
            base.Delete(id);
        }
    }
}