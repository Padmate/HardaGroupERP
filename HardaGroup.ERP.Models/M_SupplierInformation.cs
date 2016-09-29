using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class M_SupplierInformation:BaseModel
    {
        public string TypeId { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CreditCode { get; set; }
        public bool IsChecked { get; set; }

        public bool IsCanceled { get; set; }
        public DateTime CancelTime { get; set; }
        public string Remark { get; set; }
    }
}
