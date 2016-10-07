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

        #region 金额列
        /// <summary>
        /// 差异分配额
        /// </summary>
        public decimal CYFPMoney { get; set; }

        /// <summary>
        /// 直接材料
        /// </summary>
        public decimal ZJCLMoney { get; set; }

        /// <summary>
        /// 色粉
        /// </summary>
        public decimal SFMoney { get; set; }

        /// <summary>
        /// 配套
        /// </summary>
        public decimal PTMoney { get; set; }

        /// <summary>
        /// 包装
        /// </summary>
        public decimal BZMoney { get; set; }

        /// <summary>
        /// 制造费用
        /// </summary>
        public decimal ZZMoney { get; set; }

        /// <summary>
        /// 直接人工费用
        /// </summary>
        public decimal ZJRGMoney { get; set; }

        /// <summary>
        /// 模具费分摊
        /// </summary>
        public decimal MJFFTMoney { get; set; }
        #endregion

    }
}
