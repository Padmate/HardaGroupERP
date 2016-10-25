using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess
{
    public class D_Common
    {
        DBContext _dbContext = new DBContext();

        /// <summary>
        /// 获取所有的物料类别
        /// </summary>
        /// <returns></returns>
        public List<CstCostItem> GetAllCostItem()
        {
            string sql = @"select CostItemId,max(CostItemName) as CostItemName from cstCostItem group by CostItemId";

            var query = _dbContext.Database.SqlQuery<CstCostItem>(sql);


            var result = query
            .ToList();

            return result;
        }

        /// <summary>
        /// 获取会计区间
        /// </summary>
        /// <returns></returns>
        public List<Month> GetAllMonth()
        {
            string sql = @"select MonthId,MonthName from comFiscalMonth order by MonthId desc";

            var query = _dbContext.Database.SqlQuery<Month>(sql);


            var result = query
            .ToList();

            return result;
        }
    }
}
