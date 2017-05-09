using HardaGroup.ERP.DataAccess;
using HardaGroup.ERP.Entities;
using HardaGroup.ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.ERP.Service
{
    public class B_MonthCostProduction
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <returns></returns>
        public List<M_MonthCostProduction> GetPageData(M_MonthCostProduction search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            MonthCostProduction searchModel = new MonthCostProduction()
            {
                MonthId = search.MonthId,
                ProdId = search.ProdId,
                ProdName = search.ProdName
            };

            var offset = search.offset;
            var limit = search.limit;


            var pageResult = dProduction.GetMonthCostProductionPageData(searchModel, offset, limit);
            
            //获取所有的月成本数据记录
            //var allMonthCostProductions = dProduction.GetAllMonthCostProduction(search.MonthId);
            //获取所有的月成本分析记录
            //var allMonthPassMatUses = dProduction.GetAllMonthPassMatUse(search.MonthId);
            //循环每一条月成本数据
            foreach (var monthCostData in pageResult)
            {
                //获取所有物料列表的数据
                monthCostData.MoneyDetails = dProduction.GetMoneyDetails(monthCostData.ProdId, search.MonthId);
                monthCostData.Cost = monthCostData.MoneyDetails.Sum(m => m.TotalCost);
                monthCostData.Money = monthCostData.Cost * monthCostData.Quantity;
                
            }


            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        public List<M_MonthCostProduction> GetLevel1PageData(M_MonthCostProduction search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            MonthCostProduction searchModel = new MonthCostProduction()
            {
                MonthId = search.MonthId,
                ProdId = search.ProdId,
                ProdName = search.ProdName
            };

            var offset = search.offset;
            var limit = search.limit;


            var pageResult = dProduction.GetMonthCostProductionPageData(searchModel, offset, limit);

            //循环每一条月成本数据
            foreach (var monthCostData in pageResult)
            {
                //获取一级物料数据
                monthCostData.MoneyDetails = dProduction.GetMoneyLevel1Details(monthCostData.ProdId, search.MonthId);
                monthCostData.Money = monthCostData.MoneyDetails.Sum(m=>m.Money);
                monthCostData.Cost = 0;
                if (monthCostData.Quantity > 0) monthCostData.Cost = monthCostData.Money / monthCostData.Quantity;
                monthCostData.DetailQuantity = monthCostData.MoneyDetails.Sum(m=>m.Quantity);
            }


            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        public int GetPageDataTotalCount(M_MonthCostProduction search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            MonthCostProduction searchModel = new MonthCostProduction()
            {
                MonthId = search.MonthId,
                ProdId = search.ProdId,
                ProdName = search.ProdName

            };


            var totalCount = dProduction.GetMonthCostProductionPageDataCount(searchModel);

            return totalCount;
        }

        /// <summary>
        /// 实际成本
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<M_MonthCostProduction> GetActPageData(M_MonthCostProduction search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            MonthCostProduction searchModel = new MonthCostProduction()
            {
                MonthId = search.MonthId,
                ProdId = search.ProdId,
                ProdName = search.ProdName
            };

            var offset = search.offset;
            var limit = search.limit;


            var pageResult = dProduction.GetActMonthCostProductionPageData(searchModel, offset, limit);

            List<M_MonthCostProduction> result = new List<M_MonthCostProduction>();
            //按物料编号分组
            var groupData = pageResult.GroupBy(v=>v.ProdId.ToUpper());
            foreach(var eachData in groupData){

                List<M_MoneyDetail> moneyDetails = new List<M_MoneyDetail>();
                var costItemGroupData = eachData.GroupBy(v=>v.CostItemId);
                foreach (var costItemData in costItemGroupData)
                {
                    M_MoneyDetail moneyDetail = new M_MoneyDetail();
                    moneyDetail.DefCostItemId = costItemData.First().CostItemId.Trim();
                    moneyDetail.Money = costItemData.Sum(v=>v.Cost);
                    moneyDetails.Add(moneyDetail);
                }
                M_MonthCostProduction prod = new M_MonthCostProduction();
                prod.ProdId = eachData.First().ProdId;
                prod.ProdName = eachData.First().ProdName;
                prod.ProdSpec = eachData.First().ProdSpec;
                prod.UnitName = eachData.First().UnitName;
                prod.Cost = moneyDetails.Sum(v=>v.Money);  //单位成本
                prod.MoneyDetails = moneyDetails;
                prod.Quantity = eachData.First().Quantity;
                prod.Money = prod.Quantity * prod.Cost; //总成本



                result.Add(prod);
            }

            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <returns></returns>
        public List<M_BomDetail> GetBomDetailPageData(M_BomDetail search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            var searchModel = new BomDetail()
            {
                BomId = search.BomId,
                MonthId = search.MonthId
            };

            var offset = search.offset;
            var limit = search.limit;


            var pageResult = dProduction.GetBomDetailPageData(searchModel, offset, limit);

            var result = pageResult.Select(a => ConverBomDetailEntityToModel(a)).ToList();

            return result;
        }


        public int GetBomDetailPageDataTotalCount(M_BomDetail search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            var searchModel = new BomDetail()
            {
                BomId = search.BomId,
                MonthId = search.MonthId

            };


            var totalCount = dProduction.GetBomDetailPageDataTotalCount(searchModel);

            return totalCount;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <returns></returns>
        public List<M_BomDetail> GetBomDetailLevel1PageData(M_BomDetail search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            var searchModel = new BomDetail()
            {
                BomId = search.BomId,
                MonthId = search.MonthId
            };

            var offset = search.offset;
            var limit = search.limit;


            var pageResult = dProduction.GetBomDetailLevel1PageData(searchModel, offset, limit);

            var result = pageResult.Select(a => ConverBomDetailEntityToModel(a)).ToList();

            return result;
        }


        public int GetBomDetailLevel1PageDataTotalCount(M_BomDetail search)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();

            var searchModel = new BomDetail()
            {
                BomId = search.BomId,
                MonthId = search.MonthId

            };


            var totalCount = dProduction.GetBomDetailLevel1PageDataTotalCount(searchModel);

            return totalCount;
        }

        /// <summary>
        /// 根据月成本数据获取其成本分析列表
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <param name="allMonthCostProductions">当月所有的成本记录</param>
        /// <param name="allPassMatUses">当月所有的成本分析记录</param>
        /// <returns></returns>
        public List<PassMatUse> GetPassMatUseByMonthCostProduction(MonthCostProduction search,
            List<MonthCostProduction> allMonthCostProductions,List<PassMatUse> allPassMatUses)
        {
            D_MonthCostProduction dProduction = new D_MonthCostProduction();
            List<PassMatUse> allPMU = new List<PassMatUse>();

            //var pmuList = dProduction.GetPassMatUseByMonthCostProduction(search);
            var pmuList = allPassMatUses.Where(a=>(string.IsNullOrEmpty(a.ParentBomId)?"": a.ParentBomId.ToUpper()) == search.ProdId.ToUpper() 
                && a.TypeId == search.MkTypeId).ToList(); ;

            //循环每一条成分析数据
            foreach (var pmuData in pmuList)
            {
                //如果根据该数据的物料编号在月成本表中能够查询到记录，则继续往下查询
                if (pmuData.ParentRecordNum>0)
                {
                    var parentMonthCosts = allMonthCostProductions.Where(a => a.ProdId.ToUpper() == pmuData.ProdId.ToUpper()).ToList();
                    //默认取第一条数据
                    var defaultParentMonthCost = parentMonthCosts.FirstOrDefault();
                    //如果该条数据在成本合计中存在两条记录，则取“制令类型”为 “SelfMade”的数据                    
                    if(parentMonthCosts.Count() >1)
                    {
                        defaultParentMonthCost = parentMonthCosts.Where(a=>a.MkTypeId=="SelfMade").FirstOrDefault();
                    }

                    //根据父级月成本数据再取成本分析数据
                    MonthCostProduction pumSearch = new MonthCostProduction() { 
                        ProdId = defaultParentMonthCost.ProdId,
                        MonthId = defaultParentMonthCost.MonthId,
                        MkTypeId = defaultParentMonthCost.MkTypeId
                    };
                    var pmulist = GetPassMatUseByMonthCostProduction(pumSearch, allMonthCostProductions, allPassMatUses);
                    allPMU.AddRange(pmulist);
                }
                else
                {
                    allPMU.Add(pmuData);
                }
            }
            return allPMU;
        }

        private M_MonthCostProduction ConverEntityToModel(MonthCostProduction entity)
        {
            if (entity == null) return null;

            var model = new M_MonthCostProduction()
            {
                ProdId = entity.ProdId,
                MkTypeId = entity.MkTypeId,
                MonthId = entity.MonthId,
                Cost = entity.Cost,
                Money = entity.Money,
                ProdName = entity.ProdName,
                ProdSpec = entity.ProdSpec,
                Quantity = entity.Quantity,
                DetailQuantity = entity.DetailQuantity,
                UnitName = entity.UnitName,
                MoneyDetails = entity.MoneyDetails.Select(m => new M_MoneyDetail() { 
                    DefCostItemId = m.DefCostItemId,
                    TotalCost = m.TotalCost,
                    Quantity = m.Quantity,
                    Money = m.Money
                }).ToList()

            };
            return model;
        }


        private M_BomDetail ConverBomDetailEntityToModel(BomDetail entity)
        {
            if (entity == null) return null;

            var model = new M_BomDetail()
            {
                RowId = entity.RowId,
                ProdId = entity.ProdId,
                BomId = entity.BomId,
                ProdName = entity.ProdName,
                ProdSpec = entity.ProdSpec,
                UnitName = entity.UnitName,
                BaseNumber = entity.BaseNumber,
                StdUseQty = entity.StdUseQty,
                LossRate = entity.LossRate,
                RoundingPre = entity.RoundingPre,
                Accumulate = entity.Accumulate,
                UnitCost = entity.UnitCost,
                Cost = entity.Cost,
                DefCostItemId = entity.DefCostItemId,
                TypeId = entity.TypeId,
                BillNo = entity.BillNo,
                LevelCost = entity.LevelCost,
                LevelMoney = entity.LevelMoney,
                LevelQuantity = entity.LevelQuantity
            };
            return model;
        }
    }
}
