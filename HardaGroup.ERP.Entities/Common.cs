using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Entities
{
    /// <summary>
    /// 物料类别
    /// </summary>
    public class CstCostItem
    {
        /// <summary>
        /// 物料类别Id
        /// </summary>
        public string CostItemId { get; set; }

        /// <summary>
        /// 物料类别名称
        /// </summary>
        public string CostItemName { get; set; }
    }

    /// <summary>
    /// 会计区间
    /// </summary>
    public class Month
    {
        /// <summary>
        /// 区间Id
        /// </summary>
        public string MonthId { get; set; }

        /// <summary>
        /// 区间名称
        /// </summary>
        public string MonthName { get; set; }
    }
}
