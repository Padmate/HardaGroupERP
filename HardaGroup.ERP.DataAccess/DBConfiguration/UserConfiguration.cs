using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess.DBConfiguration
{
    public class UserConfiguration:EntityTypeConfiguration<User>
    {
        internal UserConfiguration()
        {
            this.ToTable("WebERPUser");
            this.HasKey(u=>u.Id);
        }
    }
}
