using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess.DBConfiguration
{
    public class UserTypeConfiguration:EntityTypeConfiguration<UserType>
    {
        internal UserTypeConfiguration()
        {
            this.ToTable("WebERPUserType");
            this.HasKey(ut=>ut.Id);
            this.HasMany(u => u.Users)
                .WithRequired(u => u.UserType)
                .HasForeignKey(u=>u.WebERPUserTypeId);
        }
    }
}
