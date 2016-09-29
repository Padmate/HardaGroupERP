using HardaGroup.ERP.DataAccess;
using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Service
{
    public class B_SupplierInformation
    {
        public List<SupplierInformation> GetAll()
        {
            D_SupplierInformation dSupplierInformation = new D_SupplierInformation();
            var result = dSupplierInformation.GetAll();

            return result;
        }
    }
}
