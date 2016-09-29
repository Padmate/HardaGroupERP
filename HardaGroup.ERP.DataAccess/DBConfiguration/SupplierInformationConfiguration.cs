using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess.DBConfiguration
{
    public class SupplierInformationConfiguration:EntityTypeConfiguration<SupplierInformation>
    {
        internal SupplierInformationConfiguration()
        {
            this.ToTable("comWebUserMngMaster");
            this.HasKey(c => new { c.TypeId, c.CompanyId });

        }
    }
}
