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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new SupplierInformationConfiguration());


        }

        public static DBContext Create()
        {
            return new DBContext();
        }
    }
}
