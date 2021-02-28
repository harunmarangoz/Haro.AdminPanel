using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Haro.AdminPanel.DataAccess
{
    public class AdminPanelContextFactory : IDesignTimeDbContextFactory<AdminPanelContext>
    {
        public AdminPanelContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdminPanelContext>();
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=DuallAjans;User Id=sa;Password=reallyStrongPwd123;");
            return new AdminPanelContext(optionsBuilder.Options);
        }
    }
}