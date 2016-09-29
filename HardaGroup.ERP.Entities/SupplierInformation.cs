using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Entities
{
    /// <summary>
    /// 供应商信息表
    /// </summary>
    public class  SupplierInformation
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
