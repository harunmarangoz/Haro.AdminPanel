using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Haro.AdminPanel.Business.Extensions;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Haro.AdminPanel.Utilities.Object;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.Business.Managers
{
    public class UserManager : AddModelManager<User, AddUserModel>
    {
        private UserModuleManager _userModuleManager;
        private ModuleManager _moduleManager;

        public UserManager(EfUserDal dal, UserModuleManager userModuleManager, ModuleManager moduleManager)
        {
            _userModuleManager = userModuleManager;
            _moduleManager = moduleManager;
            Dal = dal;
        }

        public override User Add(AddUserModel model)
        {
            ControlPassword(model.Password, model.PasswordControl);
            model.PasswordHash = HashPassword(model.Password);

            var entry = base.Add(model);
            model.SelectedModules.ForEach(x => _userModuleManager.Add(entry.Id, x));
            return entry;
        }

        public override User Edit(AddUserModel model)
        {
            var entry = BaseQuery().Include(x => x.Modules).GetById(model.Id);

            if (!string.IsNullOrEmpty(model.Password))
            {
                ControlPassword(model.Password, model.PasswordControl);
                entry.PasswordHash = HashPassword(model.Password);
            }

            entry.Email = model.Email;
            entry.Name = model.Name;
            entry.Role = model.Role;
            Dal.Update(entry);

            _userModuleManager.Update(entry.Id,
                entry.Modules.Select(x => x.ModuleId).ToList(),
                model.SelectedModules);

            return entry;
        }

        public override AddUserModel GetAddModel()
        {
            var moduleList = _moduleManager.List().ToSelectListItem();
            if (moduleList.Any()) moduleList.RemoveAt(0);
            var roleList = ((Role) 0).ToSelectList();
            if (App.Common.User.Role != Role.Support) roleList.Remove(roleList.First(x => x.Value == "4"));
            return new AddUserModel()
            {
                ModuleList = moduleList,
                RoleList = roleList,
            };
        }

        public override AddUserModel GetAddModel(long id)
        {
            var entry = BaseQuery().Include(x => x.Modules).GetById(id);
            var moduleList = _moduleManager.List().ToSelectListItem(entry.Modules.Select(x => x.ModuleId));
            if (moduleList.Any()) moduleList.RemoveAt(0);
            var roleList = entry.Role.ToSelectList();
            if (App.Common.User.Role != Role.Support) roleList.Remove(roleList.First(x => x.Value == "4"));
            return new AddUserModel(entry)
            {
                ModuleList = moduleList,
                RoleList = roleList
            };
        }

        public static string HashPassword(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return ToMD5Hash(Encoding.ASCII.GetBytes(str));
        }

        public static string ToMD5Hash(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            using (var md5 = MD5.Create())
            {
                return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }

        private static void ControlPassword(string password, string passwordControl)
        {
            if (password != passwordControl) throw new Exception("Şifreler uyuşmuyor.");
        }

        public User Login(string email, string password)
        {
            var passwordHash = HashPassword(password);
            var user = Dal.Get(x => x.Email == email);
            if (user == null) throw new Exception("Bu eposta ile kullanıcı bulunamadı.");
            if (user.PasswordHash != passwordHash) throw new Exception("Giriş yapılamadı.");
            return user;
        }
    }
}