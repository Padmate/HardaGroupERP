﻿using HardaGroup.ERP.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.DataAccess
{
    public class D_MonthCostProduction
    {
        DBContext _dbContext = new DBContext();

        public List<MonthCostProduction> GetAllMonthCostProduction(string monthId)
        {
            string sql = @"select 
                        app.*,
                        p.ProdName,
                        p.ProdSpec,
                        u.UnitName
                        from prdPassCostCollect app
                        left join comProduct p on app.ProdId = p.ProdId
                        left join comUnit u on p.UnitId = u.UnitId
                        where MonthId =@MonthId";

            var args = new DbParameter[] {
                 new SqlParameter {ParameterName = "MonthId", Value = monthId}
            };
            var query = _dbContext.Database.SqlQuery<MonthCostProduction>(sql, args);


            var result = query
            .ToList();

            return result;
        }

        public List<MonthCostProduction> GetMonthCostProductionPageData(MonthCostProduction search,int skip,int limit)
        {

            string sql = @"select 
                        app.*,
                        p.ProdName,
                        p.ProdSpec,
                        u.UnitName
                        from prdPassCostCollect app
                        left join comProduct p on app.ProdId = p.ProdId
                        left join comUnit u on p.UnitId = u.UnitId
                        where MonthId =@MonthId";

            var args = new DbParameter[] {
                 new SqlParameter {ParameterName = "MonthId", Value = search.MonthId}
            };
            var query = _dbContext.Database.SqlQuery<MonthCostProduction>(sql, args);

            var queryable = query.AsQueryable();
            #region 查询条件
            if (!string.IsNullOrEmpty(search.ProdId))
            {
                queryable = queryable.Where(a => a.ProdId == search.ProdId);
            }
            #endregion

            var result = queryable.OrderByDescending(a => a.ProdId)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetMonthCostProductionPageDataCount(MonthCostProduction search)
        {
            string sql = @"select 
                        app.*,
                        p.ProdName,
                        p.ProdSpec,
                        u.UnitName
                        from prdPassCostCollect app
                        left join comProduct p on app.ProdId = p.ProdId
                        left join comUnit u on p.UnitId = u.UnitId
                        where MonthId =@MonthId";

            var args = new DbParameter[] {
                 new SqlParameter {ParameterName = "MonthId", Value = search.MonthId}
            };
            var query = _dbContext.Database.SqlQuery<MonthCostProduction>(sql, args);

            var queryable = query.AsQueryable();
            #region 查询条件
            if (!string.IsNullOrEmpty(search.ProdId))
            {
                queryable = queryable.Where(a => a.ProdId == search.ProdId);
            }
            #endregion
            var result = queryable.ToList().Count();

            return result;
        }

        /// <summary>
        /// 获取Bom费用记录明细
        /// </summary>
        public MoneyDetail GetMoneyDetail(string bomId, string monthId)
        {
            string sql = @"
                            WITH bomMaterials AS(
	                            SELECT 
	                            BomId,
	                            ProdId,
	                            BaseNumber,  --基数
	                            StdUseQty,   --标准用量
	                            LossRate,    --损耗率
	                            RoundingPre, --舍入精度
	                            Convert(decimal(20,6),(StdUseQty/BaseNumber)*(1+LossRate*0.01)) as accumulate, --累计用量
	                            0 as leval FROM prdEnBomMaterials WHERE BomId=@BomId

	                            UNION ALL
	                            SELECT 
	                            b.BomId,
	                            b.ProdId,
	                            b.BaseNumber,
	                            b.StdUseQty,
	                            b.LossRate,
	                            b.RoundingPre,
	                            Convert(decimal(20,6),((a.accumulate * b.StdUseQty)/b.BaseNumber)*(1+b.LossRate*0.01)) as accumulate,
	                            leval+1
	                             --循环bomMaterials，在prdEnBomMaterials表中查找BomId等于a.ProdId的数据
	                            FROM bomMaterials a,prdEnBomMaterials b where b.BomId = a.ProdId  
	
	
                            )
                            SELECT 
                             sum (case when DefCostItemId = '001' then TotalCost else 0 end) ZJCLMoney,  --直接材料
                            sum (case when DefCostItemId = '002' then TotalCost else 0 end) SFMoney,--色粉
                            sum (case when DefCostItemId = '003' then TotalCost else 0 end) CYFPMoney,--差异分配额
                            sum (case when DefCostItemId = '004' then TotalCost else 0 end) PTMoney,--配套
                            sum (case when DefCostItemId = '005' then TotalCost else 0 end) BZMoney,--包装
                            sum (case when DefCostItemId = '006' then TotalCost else 0 end) ZZMoney,--制造费用
                            sum (case when DefCostItemId = '007' then TotalCost else 0 end) ZJRGMoney,--直接人工费
                            sum (case when DefCostItemId = '008' then TotalCost else 0 end) MJFFTMoney --模具费分摊

                            from 

                            (
                            select app.*,
                            IsNull(T1.ProdName, '') AS ProdName,
                            IsNull(T1.ProdSpec, '') AS ProdSpec,
                            T1.DefCostItemId, --物料类别
                            T2.Cost,
                            app.accumulate*T2.Cost as TotalCost

                            from 
                            (
	                            select * from bomMaterials where ProdId not in 
	                            --过滤不是最终层级的数据
	                            (select ProdId from bomMaterials where ProdId in (select BomId from bomMaterials))
                            )app
                            LEFT JOIN comProduct T1 ON app.ProdId=T1.ProdId
                            --过滤找不到费用的数据
                            Inner JOIN PassProdCost T2 ON app.ProdId = T2.ProdId and T2.MonthId = @MonthId 
                            ) baseapp";

            var args = new DbParameter[] {
                new SqlParameter {ParameterName = "BomId", Value = bomId},
                new SqlParameter {ParameterName = "MonthId", Value = monthId}
            };
            var query = _dbContext.Database.SqlQuery<MoneyDetail>(sql, args);

            var result = query.ToList();

            return result.First();
        }

        public List<PassMatUse> GetAllMonthPassMatUse(string monthId)
        {
            string sql = @"select * from 
                        (
	                        select 
	                        PMU.*,
	                        (select count(*) from prdPassCostCollect where ProdId = PMU.ProdId AND MonthId=@MonthId) ParentRecordNum, --当前数据的物料编号在成本合计中是否存在记录
	                        
	                        ISNULL( ISNULL(A5.BOMId, A10.BOMId), A15.BOMId) AS [ParentBomId],
	                        A22.ClassId   --类别ID
	                        from prdPassMatUse PMU 
	                        LEFT OUTER JOIN prdMakeOrder A5 ON PMU.TypeId='SelfMade' AND PMU.BillNo=A5.BillNo AND (A5.TypeId='SelfMade') AND PMU.TypeId=A5.TypeId
	                        LEFT OUTER JOIN prdMakeOrder A10 ON PMU.TypeId='Remade' AND PMU.BillNo=A10.BillNo AND (A10.TypeId='Remade') AND PMU.TypeId=A10.TypeId
	                        LEFT OUTER JOIN prdMakeOrder A15 ON PMU.TypeId='OutSource' AND PMU.BillNo=A15.BillNo AND (A15.TypeId='OutSource') AND PMU.TypeId=A15.TypeId
	                        LEFT OUTER JOIN comProduct A22 ON PMU.ProdId=A22.ProdId
	 
                        )pmulist where pmulist.MonthId=@MonthId";

            var args = new DbParameter[] {
                new SqlParameter {ParameterName = "MonthId", Value = monthId}
            };
            var query = _dbContext.Database.SqlQuery<PassMatUse>(sql, args);

            var result = query.ToList();

            return result;
        }

        /// <summary>
        /// 根据月成本数据获取其对应的成本分析数据
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<PassMatUse> GetPassMatUseByMonthCostProduction(MonthCostProduction search)
        {

            string sql = @"select * from 
                        (
	                        select 
	                        PMU.*,
	                        (select count(*) from prdPassCostCollect where ProdId = PMU.ProdId AND MonthId=@MonthId) ParentRecordNum, --当前数据的物料编号在成本合计中是否存在记录
	                        
	                        ISNULL( ISNULL(A5.BOMId, A10.BOMId), A15.BOMId) AS [ParentBomId],
	                        A22.ClassId   --类别ID
	                        from prdPassMatUse PMU 
	                        LEFT OUTER JOIN prdMakeOrder A5 ON PMU.TypeId='SelfMade' AND PMU.BillNo=A5.BillNo AND (A5.TypeId='SelfMade') AND PMU.TypeId=A5.TypeId
	                        LEFT OUTER JOIN prdMakeOrder A10 ON PMU.TypeId='Remade' AND PMU.BillNo=A10.BillNo AND (A10.TypeId='Remade') AND PMU.TypeId=A10.TypeId
	                        LEFT OUTER JOIN prdMakeOrder A15 ON PMU.TypeId='OutSource' AND PMU.BillNo=A15.BillNo AND (A15.TypeId='OutSource') AND PMU.TypeId=A15.TypeId
	                        LEFT OUTER JOIN comProduct A22 ON PMU.ProdId=A22.ProdId
	 
                        )pmulist where pmulist.ParentBomId = @ProdId AND pmulist.TypeId=@TypeId AND pmulist.MonthId=@MonthId";

            var args = new DbParameter[] {
                new SqlParameter {ParameterName = "ProdId", Value = search.ProdId},
                new SqlParameter {ParameterName = "TypeId", Value = search.MkTypeId},
                new SqlParameter {ParameterName = "MonthId", Value = search.MonthId}
            };
            var query = _dbContext.Database.SqlQuery<PassMatUse>(sql, args);

            var result = query.ToList();

            return result;
        }
    }
}