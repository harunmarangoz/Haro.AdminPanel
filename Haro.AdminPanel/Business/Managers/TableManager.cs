using System;
using System.Linq;
using Haro.AdminPanel.Business.Extensions;
using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.Business.Managers
{
    public class TableManager : AddModelManager<Table, AddTableModel>
    {
        private EfTableDal _dal;
        private ModuleManager _moduleManager;

        public TableManager(EfTableDal dal, ModuleManager moduleManager)
        {
            Dal = dal;
            _dal = dal;
            _moduleManager = moduleManager;
        }

        public override IQueryable<Table> BaseQuery()
        {
            return base.BaseQuery().Include(x => x.Module);
        }

        public override Table Add(AddTableModel model)
        {
            if (string.IsNullOrEmpty(model.Name)) throw new Exception("Tablo ismi boş olamaz");
            var controlTableEntry = _moduleManager.Get(x => x.Id == model.ModuleId);
            if (controlTableEntry == null) throw new Exception("Modül bulunamadı.");
            var entry = base.Add(model);

            _dal.CreateTable(entry.Name);

            return entry;
        }

        public override Table Edit(AddTableModel model)
        {
            var entry = GetById(model.Id);

            entry.DisplayName = model.DisplayName;
            entry.ModuleId = model.ModuleId;
            entry.Show = model.Show;
            entry.MultipleLanguage = model.MultipleLanguage;

            Dal.Update(entry);
            return entry;
        }

        public override AddTableModel GetAddModel()
        {
            return new AddTableModel
            {
                ModuleList = _moduleManager.List().ToSelectListItem(),
            };
        }

        public override AddTableModel GetAddModel(long id)
        {
            var entry = BaseQuery().Include(x => x.Columns).GetById(id);
            return new AddTableModel(entry)
            {
                ModuleList = _moduleManager.List().ToSelectListItem(entry.ModuleId),
            };
        }
    }
}