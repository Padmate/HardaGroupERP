using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Entities
{
    public class MonthCostProduction
    {
        /// <summary>
        /// 生产类型
        /// </summary>
        public string MkTypeId { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ProdId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ProdName { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public string ProdSpec { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 区间ID
        /// </summary>
        public string MonthId { get; set; }

        /// <summary>
        /// 成本单价
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        public decimal Money { get; set; }

        public decimal Quantity { get; set; }

        public List<MoneyDetail> MoneyDetails { get; set; }
        

    }

    public class PassMatUse
    {
        /// <summary>
        /// 物料编号
        /// </summary>

        private string prodId;
        public string ProdId { get; set; }

        public string TypeId { get; set; }

        public string ParentBomId { get; set; }
        /// <summary>
        /// 父级数据条数
        /// </summary>
        public int ParentRecordNum { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

       

    }


    public class MoneyDetail
    {
        /// <summary>
        /// 物料类别Id
        /// </summary>
        public string DefCostItemId { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal TotalCost { get; set; }
        
    }
}
