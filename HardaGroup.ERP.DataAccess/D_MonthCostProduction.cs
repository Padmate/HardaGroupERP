using HardaGroup.ERP.Entities;
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
                queryable = queryable.Where(a => a.ProdId.ToLower().Contains(search.ProdId.ToLower()));
            }
            if (!string.IsNullOrEmpty(search.ProdName))
            {
                queryable = queryable.Where(a => a.ProdName.ToLower().Contains(search.ProdName.ToLower()));
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
                queryable = queryable.Where(a => a.ProdId.ToLower().Contains(search.ProdId.ToLower()));
            }
            if (!string.IsNullOrEmpty(search.ProdName))
            {
                queryable = queryable.Where(a => a.ProdName.ToLower().Contains(search.ProdName.ToLower()));
            }
            #endregion
            var result = queryable.ToList().Count();

            return result;
        }

        /// <summary>
        /// 实际成本
        /// </summary>
        /// <param name="search"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<MonthCostProduction> GetActMonthCostProductionPageData(MonthCostProduction search, int skip, int limit)
        {

            List<MonthCostProduction> result = new List<MonthCostProduction>();

            #region
            try
            {
                //删除临时表
                var dropTableSql = @"Drop Table [HEDCostItem]";
                _dbContext.Database.ExecuteSqlCommand(dropTableSql);

            }catch(Exception e){ 
                //表不存在
            }
            
            try{

                //创建临时表
                var createTableSql = @"
                            CREATE TABLE [dbo].[HEDCostItem](
	                            [MonthId] [nchar](10) NULL,
	                            [ProdId] [nchar](20) NULL,
	                            [CostItemId] [nchar](10) NULL,
	                            [Cost] [decimal](18, 6) NULL,
	                            [IsBom] [int] NULL,
	                            [LowLevelCode] [int] NULL,
	                            [Money] [decimal](18, 6) NULL
                            ) ON [PRIMARY]
                                ";
                _dbContext.Database.ExecuteSqlCommand(createTableSql);
                
                //删除当前月份数据
                var delSql = @"
                        Delete cstMakeOrderTreeDetailR
                        FROM cstMakeOrderTreeDetailR
                        WHERE MonthId=@MonthId --当前报表查询月份
                        ";
                var delArgs = new DbParameter[] {
                    new SqlParameter {ParameterName = "MonthId", Value = search.MonthId}
                };
                _dbContext.Database.ExecuteSqlCommand(delSql,delArgs);


                //插入当前月份数据
                var insertSql = @"
                            INSERT INTO cstMakeOrderTreeDetailR
                                (MonthId,ProdId,LowLevelCode,IsBom,IsReMade)
                                SELECT MonthId,ProdId,LowLevelCode,IsBom,IsReMade
                                FROM cstMakeOrderTreeDetail
                                WHERE MonthId=@MonthId--当前报表查询月份
                            ";
                var insertArgs = new DbParameter[] {
                    new SqlParameter {ParameterName = "MonthId", Value = search.MonthId}
                };
                _dbContext.Database.ExecuteSqlCommand(insertSql,insertArgs);

                //查询isbom的最高级
                var maxLevelCodeSql = @"SELECT isnull(MAX(LowLevelCode),0) maxLevelCode FROM cstMakeOrderTreeDetailR WHERE ISBOM='1'";
                var maxLevelCode = _dbContext.Database.SqlQuery<int>(maxLevelCodeSql).FirstOrDefault();

                if(maxLevelCode >0)
                {
                    #region 
                    //插入最末级成本分析临时表
                    var insertHEDSql = @"
                                --插入成本分析临时表
                                INSERT INTO HEDCostItem
                                (MonthId,ProdId,CostItemId,Cost)
                                --最末级母件成本项目
                                SELECT T0.MonthId,T0.BOMId,T0.CostItemId,T0.Money/T1.Quantity AS Cost
                                FROM 
                                (
	                                SELECT T0.MonthId,T1.BOMId,T0.CostItemId,SUM(T0.Money) AS Money
	                                FROM prdPassMatUse T0
		                                JOIN prdMakeOrder T1 ON T0.TypeId=T1.TypeId AND T0.BillNo=T1.BillNo
		                                JOIN
		                                (
			                                SELECT ProdId
			                                FROM cstMakeOrderTreeDetailR
			                                WHERE IsBom=1
				                                AND MonthId=@MonthId
				                                AND LowLevelCode=@LevelCode
		                                ) T3 ON T1.BOMId=T3.ProdId
	                                WHERE T0.MonthId=@MonthId
	                                GROUP BY T0.MonthId,T1.BOMId,T0.CostItemId
                                ) T0
                                JOIN prdPassCostCollect T1 ON T0.BOMId=T1.ProdId AND T0.MonthId=T1.MonthId
                                WHERE T1.Quantity>0
                                ";
                    var insertHEDArgs = new DbParameter[] {
                        new SqlParameter {ParameterName = "MonthId", Value = search.MonthId},
                        new SqlParameter {ParameterName = "LevelCode", Value = maxLevelCode}

                    };
                    _dbContext.Database.ExecuteSqlCommand(insertHEDSql,insertHEDArgs);

                    //插入最末级-1 成本分析临时表
                    for (int i = 0; i < maxLevelCode - 1; i++)
                    {
                        var levelCode = i;
                        insertHEDSql = @"
                                --插入成本分析临时表
                                INSERT INTO HEDCostItem
                                (MonthId,ProdId,CostItemId,Cost)
                                --最末级-1 级，母件成本项目
                                SELECT T0.MonthId,T0.BOMId,T0.CostItemId,T0.Money/T1.Quantity AS ItemCost
                                FROM 
                                (
	                                SELECT T0.MonthId,T0.BOMId,T0.CostItemId,SUM(T0.Money) AS Money
	                                FROM 
	                                (
		                                select T0.MonthId,T1.BomId,T0.ProdId,ISNULL(T4.CostItemId,T0.CostItemId) AS CostItemId,
		                                CASE WHEN T4.CostItemId IS NULl THEN T0.Money ELSE T0.Quantity*T4.Cost END AS Money	
		                                FROM prdPassMatUse T0
			                                JOIN prdMakeOrder T1 ON T0.TypeId=T1.TypeId AND T0.BillNo=T1.BillNo
			                                JOIN
			                                (
				                                SELECT ProdId
				                                FROM cstMakeOrderTreeDetailR
				                                WHERE IsBom=1
					                                AND MonthId=@MonthId
					                                AND LowLevelCode=@LevelCode
			                                ) T3 ON T1.BOMId=T3.ProdId
			                                LEFT JOIN HEDCostItem T4 ON T0.ProdId=T4.ProdId
		                                WHERE T0.MonthId=@MonthId
	                                ) T0
	                                GROUP BY T0.MonthId,T0.BOMId,T0.CostItemId
                                ) T0
                                JOIN prdPassCostCollect T1 ON T0.BOMId=T1.ProdId AND T0.MonthId=T1.MonthId
                                WHERE T1.Quantity>0
                                    ";
                        insertHEDArgs = new DbParameter[] {
                            new SqlParameter {ParameterName = "MonthId", Value = search.MonthId},
                            new SqlParameter {ParameterName = "LevelCode", Value = levelCode}

                        };
                        _dbContext.Database.ExecuteSqlCommand(insertHEDSql,insertHEDArgs);
                    }
                    #endregion


                    _dbContext.SaveChanges();

                    var searchSql = @"
                         select 
                        T.MonthId,
                        T.ProdId,
                        T.CostItemId,
                        T.Cost,
                        T1.ProdName, -- 一级物料名称
                        T1.ProdSpec, -- 一级物料规格
                        T2.UnitName,  --单位
                        T3.Quantity  --产量
                        from HEDCostItem T
                        left join comProduct T1 ON T.ProdId=T1.ProdId
                        left join comUnit T2 on T1.UnitId = T2.UnitId
                        left join prdPassCostCollect T3 on T.ProdId = T3.ProdId and t3.MonthId =@MonthId

                        ";
                    var searchArgs = new DbParameter[] {
                        new SqlParameter {ParameterName = "MonthId", Value = search.MonthId}

                    };
                    var query = _dbContext.Database.SqlQuery<MonthCostProduction>(searchSql,searchArgs);
                    var queryable = query.AsQueryable();

                    #region 查询条件
                    if (!string.IsNullOrEmpty(search.ProdId))
                    {
                        queryable = queryable.Where(a => a.ProdId.ToLower().Contains(search.ProdId.ToLower()));
                    }
                    if (!string.IsNullOrEmpty(search.ProdName))
                    {
                        queryable = queryable.Where(a => a.ProdName.ToLower().Contains(search.ProdName.ToLower()));
                    }
                    #endregion

                    result = queryable.ToList();
                }

            }catch(Exception ex){
                //
            }
            #endregion
            

            return result;
        }

        /// <summary>
        /// 产品结构明细
        /// </summary>
        /// <param name="search"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<BomDetail> GetBomDetailPageData(BomDetail search, int skip, int limit)
        {

            var queryable = BomDetailPageDataSql(search);

            var result = queryable.OrderBy(a => a.RowId)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetBomDetailPageDataTotalCount(BomDetail search)
        {
           
            var queryable = BomDetailPageDataSql(search);
            var result = queryable.ToList().Count();

            return result;
        }

        private IQueryable<BomDetail> BomDetailPageDataSql(BomDetail search)
        {
            string sql = @"WITH bomMaterials AS(
	                            SELECT 
	                            BomId,
	                            ProdId,
	                            BaseNumber,  --基数
	                            StdUseQty,   --标准用量
	                            LossRate,    --损耗率
	                            RoundingPre, --舍入精度
	                            Convert(decimal(20,6),(StdUseQty/BaseNumber)*(1+LossRate*0.01)) as UnitAccumulate, --单位累计用量
	                            0 as leval FROM prdEnBomMaterials WHERE BomId=@BomId

	                            UNION ALL
	                            SELECT 
	                            b.BomId,
	                            b.ProdId,
	                            b.BaseNumber,
	                            b.StdUseQty,
	                            b.LossRate,
	                            b.RoundingPre,
	                            Convert(decimal(20,6),((a.UnitAccumulate * b.StdUseQty)/b.BaseNumber)*(1+b.LossRate*0.01)) as UnitAccumulate,
	                            leval+1
	                             --循环bomMaterials，在prdEnBomMaterials表中查找BomId等于a.ProdId的数据
	                            FROM bomMaterials a,prdEnBomMaterials b where b.BomId = a.ProdId  
	
	
                            )
                            SELECT 
                            baseapp.*, 
							case when LTrim(RTrim(UnitName)) ='个' then  CAST(CEILING(TmpAccumulatea) AS numeric(20, 6))
							else TmpAccumulatea end as Accumulate --累计用量            
                            from 
                            (
                            select 
							app.*,
                            row_number() over(order by app.ProdId) as RowId, 
                            T1.ProdName, --物料名称
							T1.DefCostItemId, --物料类别,
							T1.ProdSpec, --物料规格
							UT.UnitName, --单位
							base.Quantity, --母件产量,
							isnull(T2.Cost,0) as Cost, --金额
							app.UnitAccumulate*isnull(T2.Cost,0) as UnitCost, --单位成本
							UnitAccumulate * isnull(base.Quantity,0) as TmpAccumulatea --累计用量
                            from 
                            (
	                             --过滤不是最后层级的数据
	                            select * from (
									select a.*,
									(select count(*) from bomMaterials b where b.BOMId = a.prodid) as childNodes  --当前数据是否含有子节点（即是不是最后一层）
									from bomMaterials a
								) temp where temp.childNodes =0

                            )app
							LEFT JOIN prdPassCostCollect base on base.ProdId =@BomId and base.MonthId =@MonthId
                            LEFT JOIN comProduct T1 ON app.ProdId=T1.ProdId
							LEFT JOIN comUnit UT ON T1.UnitId=UT.UnitId
                            --过滤找不到费用的数据
                            left JOIN PassProdCost T2 ON app.ProdId = T2.ProdId and T2.MonthId =@MonthId
                            ) baseapp";

            var args = new DbParameter[] {
                 new SqlParameter {ParameterName = "MonthId", Value = search.MonthId},
                 new SqlParameter {ParameterName = "BomId", Value = search.BomId}

            };
            var query = _dbContext.Database.SqlQuery<BomDetail>(sql, args);
            var queryable = query.AsQueryable();

            return queryable;
        }

        /// <summary>
        /// 一级物料明细
        /// </summary>
        /// <param name="search"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<BomDetail> GetBomDetailLevel1PageData(BomDetail search, int skip, int limit)
        {

            var queryable = BomDetailLevel1PageDataSql(search);

            var result = queryable.OrderBy(a => a.RowId)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetBomDetailLevel1PageDataTotalCount(BomDetail search)
        {

            var queryable = BomDetailLevel1PageDataSql(search);
            var result = queryable.ToList().Count();

            return result;
        }

        private IQueryable<BomDetail> BomDetailLevel1PageDataSql(BomDetail search)
        {
            string sql = @"with passmatuse as(
	                        Select 
	                        T2.BomId, --物料编号
	                        T3.ProdName AS BOMName, --物料名称
	                        T3.ProdSpec AS BOMSpce, --物料规格
	                        T0.ProdId, -- 一级物料编号
	                        T1.ProdName, -- 一级物料名称
	                        T1.ProdSpec, -- 一级物料规格
	                        T0.Quantity as LevelQuantity, --用量
	                        T0.Cost as LevelCost, --成本
	                        T0.Money as LevelMoney, --金额
	                        T1.DefCostItemId, --一级物料类别
	                        T0.TypeId,  --制令单据类型
	                        T0.BillNo,   --制令单据编号
                            u.UnitName  --单位
	                        FROM prdPassMatUse T0
	                         join comProduct T1 ON T0.ProdId=T1.ProdId --物料主数据取回默认成本项目
	                         join prdMakeOrder T2 ON T0.TypeId=T2.TypeId AND T0.BillNo=T2.BillNo
	                         join comProduct T3 ON T2.BOMId=T3.ProdId
                             left join comUnit u on T1.UnitId = u.UnitId
	                        where T0.MonthId=@MonthId and T2.BOMId =@BomId
                        )
                        select 
                        row_number() over(order by app.ProdId) as RowId, 
                        app.* 
                        from passmatuse app";

            var args = new DbParameter[] {
                 new SqlParameter {ParameterName = "MonthId", Value = search.MonthId},
                 new SqlParameter {ParameterName = "BomId", Value = search.BomId}

            };
            var query = _dbContext.Database.SqlQuery<BomDetail>(sql, args);
            var queryable = query.AsQueryable();

            return queryable;
        }

        private void test(int skip, int limit,string monthId)
        {
            System.Data.SqlClient.SqlParameter[] parameters = {   
                new System.Data.SqlClient.SqlParameter("@startIndex",skip),  
                new System.Data.SqlClient.SqlParameter("@endIndex",skip+limit),  
                new System.Data.SqlClient.SqlParameter("@monthId", monthId)  
                };
            //parameters[2].Direction = System.Data.ParameterDirection.Output;
            var slt = _dbContext.Database.SqlQuery<MonthCostProduction>("exec pro_page @startIndex,@endIndex,@monthId", parameters);
            var aa = slt.ToList();
            string AllCount = parameters[2].Value.ToString();  
        }

        public MoneyDetail testDetail(string bomId, string monthId)
        {
            System.Data.SqlClient.SqlParameter[] parameters = {   
                new System.Data.SqlClient.SqlParameter("@bomId",bomId),  
                new System.Data.SqlClient.SqlParameter("@monthId", monthId)  
                };
            var slt = _dbContext.Database.SqlQuery<MoneyDetail>("exec pro_pagetest @bomId,@monthId", parameters);
            var result =  slt.ToList();
            return result.First();
        }


        /// <summary>
        /// 获取Bom费用记录明细
        /// </summary>
        public List<MoneyDetail> GetMoneyDetails(string bomId, string monthId)
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
                             --case when DefCostItemId ='' then 'othermoney' else DefCostItemId end as DefCostItemId,
                            DefCostItemId,
							 sum(TotalCost) as TotalCost
                            from 

                            (
                            select 
                            T1.DefCostItemId, --物料类别
                            app.accumulate*T2.Cost as TotalCost --单位成本

                            from 
                            (
	                             --过滤不是最后层级的数据
	                            select * from (
									select a.*,
									(select count(*) from bomMaterials b where b.BOMId = a.prodid) as childNodes  --当前数据是否含有子节点（即是不是最后一层）
									from bomMaterials a
								) temp where temp.childNodes =0

                            )app
                            LEFT JOIN comProduct T1 ON app.ProdId=T1.ProdId
                            --过滤找不到费用的数据
                            Inner JOIN PassProdCost T2 ON app.ProdId = T2.ProdId and T2.MonthId = @MonthId 
                            ) baseapp group by DefCostItemId";

            var args = new DbParameter[] {
                new SqlParameter {ParameterName = "BomId", Value = bomId},
                new SqlParameter {ParameterName = "MonthId", Value = monthId}
            };
            var query = _dbContext.Database.SqlQuery<MoneyDetail>(sql, args);

            var result = query.ToList();

            return result;
        }

        public List<MoneyDetail> GetMoneyLevel1Details(string bomId, string monthId)
        {
            string sql = @"
                            with passmatuse as(
	                            Select 
	                            T2.BomId, --物料编号
	                            T3.ProdName AS BOMName, --物料名称
	                            T3.ProdSpec AS BOMSpce, --物料规格
	                            T0.ProdId, -- 一级物料编号
	                            T1.ProdName, -- 一级物料名称
	                            T1.ProdSpec, -- 一级物料规格
	                            T0.Quantity, --产量
	                            T0.Cost, --成本
	                            T0.Money, --金额
	                            T1.DefCostItemId, --一级物料类别
	                            T0.TypeId,  --制令单据类型
	                            T0.BillNo   --制令单据编号
	                            FROM prdPassMatUse T0
	                             join comProduct T1 ON T0.ProdId=T1.ProdId --物料主数据取回默认成本项目
	                             join prdMakeOrder T2 ON T0.TypeId=T2.TypeId AND T0.BillNo=T2.BillNo
	                             join comProduct T3 ON T2.BOMId=T3.ProdId
	                            where T0.MonthId=@MonthId and T2.BOMId=@BomId
                            )
                            select 
                              DefCostItemId,
                              sum(Quantity) as Quantity, --材料用量
                              sum(Money) as Money  --类别金额
                            from passmatuse group by DefCostItemId";

            var args = new DbParameter[] {
                new SqlParameter {ParameterName = "BomId", Value = bomId},
                new SqlParameter {ParameterName = "MonthId", Value = monthId}
            };
            var query = _dbContext.Database.SqlQuery<MoneyDetail>(sql, args);

            var result = query.ToList();

            return result;
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
