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

        /// <summary>
        /// 产量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 材料用量
        /// </summary>
        public decimal DetailQuantity { get; set; }

        public List<MoneyDetail> MoneyDetails { get; set; }

        public string CostItemId { get; set; }
        

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

        /// <summary>
        /// 材料用量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        
    }


    /// <summary>
    /// 产品结构明细
    /// </summary>
    public class BomDetail
    {
        public string MonthId { get; set; }

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
        public decimal BaseNumber{get;set;}

        /// <summary>
        /// 标准用量
        /// </summary>
	    public decimal StdUseQty{get;set;}

        /// <summary>
        /// 损耗率
        /// </summary>
	    public decimal LossRate{ get;set;}
        /// <summary>
        /// 舍入精度
        /// </summary>
        public int RoundingPre { get; set; }

        /// <summary>
        /// 累计用量(乘以母件用量)
        /// </summary>
        public decimal Accumulate { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// 单位成本(单位累计用量*金额)
        /// </summary>
        public decimal UnitCost { get; set; }

        /// <summary>
        /// 物料类别
        /// </summary>
        public string DefCostItemId { get; set; }

        /// <summary>
        /// 制令单据类型
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 制令单据编号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal LevelQuantity { get; set; }

        /// <summary>
        /// 成本
        /// </summary>
        public decimal LevelCost { get; set; }


        /// <summary>
        /// 金额
        /// </summary>
        public decimal LevelMoney { get; set; }
    }
}
