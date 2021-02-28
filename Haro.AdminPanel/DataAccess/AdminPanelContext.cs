using System.Collections.Generic;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Haro.AdminPanel.DataAccess
{
    public class AdminPanelContext : DbContext
    {
        public AdminPanelContext()
        {
        }

        public AdminPanelContext(DbContextOptions<AdminPanelContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(App.ConnectionString ?? "Server=127.0.0.1,1433;Database=DuallAjans;User Id=sa;Password=reallyStrongPwd123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Table>()
                .HasIndex(u => u.Name)
                .IsUnique();

        }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<SiteInformation> SiteInformations { get; set; }
    }
}