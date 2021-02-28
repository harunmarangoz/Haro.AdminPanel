using Haro.AdminPanel.DataAccess.EntityFramework;
using Haro.AdminPanel.Models.AddModels;
using Haro.AdminPanel.Models.Entities;

namespace Haro.AdminPanel.Business.Managers
{
    public class UserModuleManager : AddForeignModelManager<UserModule>
    {
        public UserModuleManager(EfUserModuleDal dal)
        {
            Dal = dal;
        }

        public override UserModule Add(long firstId, long secondId)
        {
            return Dal.Add(new UserModule() {UserId = firstId, ModuleId = secondId});
        }

        public override void Remove(long firstId, long secondId)
        {
            Dal.Delete(Dal.Get(x => x.UserId == firstId && x.ModuleId == secondId));
        }
    }
}