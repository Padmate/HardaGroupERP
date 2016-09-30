using HardaGroup.ERP.DataAccess.DBConfiguration;
using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess
{
    public class DBContext:DbContext
    {
        public DBContext()
            : base("DefaultConnection")
        {

        }

        public DbSet<SupplierInformation> SupplierInformations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<CompanyInfo> CompanyInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new SupplierInformationConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserTypeConfiguration());
            modelBuilder.Configurations.Add(new CompanyInfoConfiguration());


        }

        public static DBContext Create()
        {
            return new DBContext();
        }
    }
}
