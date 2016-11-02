using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Models
{
    public class M_MonthCostProduction:BaseModel
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

        public List<M_MoneyDetail> MoneyDetails { get; set; }

        

    }

    public class M_MoneyDetail
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

    public class M_BomDetail:BaseModel
    {
        public Int64 RowId { get; set; }
        public string BomId { get; set; }
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
        /// 基数
        /// </summary>
        public decimal BaseNumber { get; set; }

        /// <summary>
        /// 标准用量
        /// </summary>
        public decimal StdUseQty { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate { get; set; }
        /// <summary>
        /// 舍入精度
        /// </summary>
        public int RoundingPre { get; set; }

        /// <summary>
        /// 累计用量
        /// </summary>
        public decimal Accumulate { get;set; }

        /// <summary>
        /// 单位成本
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// 物料类别
        /// </summary>
        public string DefCostItemId { get; set; }
    }
}
