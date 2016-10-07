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
                ProdId = search.ProdId
            };

            var offset = search.offset;
            var limit = search.limit;


            var pageResult = dProduction.GetMonthCostProductionPageData(searchModel, offset, limit);
            
            //获取所有的月成本数据记录
            var allMonthCostProductions = dProduction.GetAllMonthCostProduction(search.MonthId);
            //获取所有的月成本分析记录
            var allMonthPassMatUses = dProduction.GetAllMonthPassMatUse(search.MonthId);
            //循环每一条月成本数据
            foreach (var monthCostData in pageResult)
            {
                //MonthCostProduction pmuSearch = new MonthCostProduction()
                //{
                //    MonthId = search.MonthId,
                //    ProdId = monthCostData.ProdId,
                //    MkTypeId = monthCostData.MkTypeId
                //};
                //var allpmuList = GetPassMatUseByMonthCostProduction(pmuSearch, allMonthCostProductions, allMonthPassMatUses);
                var moneyDetail = dProduction.GetMoneyDetail(monthCostData.ProdId, search.MonthId);
                monthCostData.ZJCLMoney = moneyDetail.ZJCLMoney;
                monthCostData.SFMoney = moneyDetail.SFMoney;
                monthCostData.PTMoney = moneyDetail.PTMoney;
                monthCostData.BZMoney = moneyDetail.BZMoney;
                monthCostData.ZZMoney = moneyDetail.ZZMoney;
                monthCostData.ZJRGMoney = moneyDetail.ZJRGMoney;
                monthCostData.MJFFTMoney = moneyDetail.MJFFTMoney;


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
                ProdId = search.ProdId
            };


            var totalCount = dProduction.GetMonthCostProductionPageDataCount(searchModel);

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
                UnitName = entity.UnitName,
                BZMoney = entity.BZMoney,
                CYFPMoney = entity.CYFPMoney,
                MJFFTMoney = entity.MJFFTMoney,
                PTMoney = entity.PTMoney,
                SFMoney = entity.SFMoney,
                ZJCLMoney = entity.ZJCLMoney,
                ZJRGMoney = entity.ZJRGMoney,
                ZZMoney = entity.ZZMoney
               
            };
            return model;
        }
    }
}
