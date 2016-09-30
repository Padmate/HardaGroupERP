using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess.DBConfiguration
{
    class CompanyInfoConfiguration:EntityTypeConfiguration<CompanyInfo>
    {
        internal CompanyInfoConfiguration()
        {
            this.ToTable("WebERPCompanyInfo");
            this.HasKey(ut=>ut.Id);
            this.HasMany(u => u.Users)
                .WithRequired(u => u.CompanyInfo)
                .HasForeignKey(u=>u.WebERPCompanyInfoId);
        }
    }
}
