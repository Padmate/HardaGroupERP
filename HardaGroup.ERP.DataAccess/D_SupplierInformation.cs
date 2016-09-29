using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess
{
    public class D_SupplierInformation
    {
        DBContext _dbContext = new DBContext();

        public List<SupplierInformation> GetAll()
        {

            var supplierInfos = _dbContext.SupplierInformations
                .ToList();

            return supplierInfos;
        }
    }
}
