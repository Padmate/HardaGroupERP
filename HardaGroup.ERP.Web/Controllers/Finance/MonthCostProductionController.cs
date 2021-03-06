﻿using HardaGroup.ERP.DataAccess;
using HardaGroup.ERP.Models;
using HardaGroup.ERP.Service;
using HardaGroup.ERP.Utility;
using HardaGroup.ERP.Web.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.ERP.Web.Controllers.Finance
{
    [Authorization]
    public class MonthCostProductionController:BaseController
    {
        /// <summary>
        /// 按BOM查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            D_Common dCommon = new D_Common();
            //查找所有的物料类别
            var allCostItems = dCommon.GetAllCostItem();
            ViewData["jsonAllCostItems"] = JsonHandler.ToJson(allCostItems);

            //查询会计区间
            var months = dCommon.GetAllMonth();
            ViewData["month"] = months;


            return View();
        }

        /// <summary>
        /// 一级查询
        /// </summary>
        /// <returns></returns>

        public ActionResult IndexLevel1()
        {
            D_Common dCommon = new D_Common();
            //查找所有的物料类别
            var allCostItems = dCommon.GetAllCostItem();
            ViewData["jsonAllCostItems"] = JsonHandler.ToJson(allCostItems);

            //查询会计区间
            var months = dCommon.GetAllMonth();
            ViewData["month"] = months;


            return View();
        }

        /// <summary>
        /// 实际成本
        /// </summary>
        /// <returns></returns>

        public ActionResult IndexAct()
        {
            D_Common dCommon = new D_Common();
            //查找所有的物料类别
            var allCostItems = dCommon.GetAllCostItem();
            ViewData["jsonAllCostItems"] = JsonHandler.ToJson(allCostItems);

            //查询会计区间
            var months = dCommon.GetAllMonth();
            ViewData["month"] = months;


            return View();
        }


        public ActionResult Detail()
        {
            D_Common dCommon = new D_Common();
            //查找所有的物料类别
            var allCostItems = dCommon.GetAllCostItem();
            ViewData["jsonAllCostItems"] = JsonHandler.ToJson(allCostItems);

            return View();
        }

        public ActionResult DetailLevel1()
        {
            D_Common dCommon = new D_Common();
            //查找所有的物料类别
            var allCostItems = dCommon.GetAllCostItem();
            ViewData["jsonAllCostItems"] = JsonHandler.ToJson(allCostItems);

            return View();
        }

        [HttpPost]
        public ActionResult GetTreeDetailData(string json)
        {
            
            return Json(json);
        }

        [HttpPost]
        public ActionResult GetDetailPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_BomDetail model = JsonHandler.UnJson<M_BomDetail>(strReqStream);

            B_MonthCostProduction bMonthCostProduction = new B_MonthCostProduction();
            var pageData = bMonthCostProduction.GetBomDetailPageData(model);
            var totalCount = bMonthCostProduction.GetBomDetailPageDataTotalCount(model);

            PageResult<M_BomDetail> pageResult = new PageResult<M_BomDetail>(totalCount, pageData);
            return Json(pageResult);
        }

        [HttpPost]
        public ActionResult GetDetailLevel1PageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_BomDetail model = JsonHandler.UnJson<M_BomDetail>(strReqStream);

            B_MonthCostProduction bMonthCostProduction = new B_MonthCostProduction();
            var pageData = bMonthCostProduction.GetBomDetailLevel1PageData(model);
            var totalCount = bMonthCostProduction.GetBomDetailLevel1PageDataTotalCount(model);

            PageResult<M_BomDetail> pageResult = new PageResult<M_BomDetail>(totalCount, pageData);
            return Json(pageResult);
        }


        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_MonthCostProduction model = JsonHandler.UnJson<M_MonthCostProduction>(strReqStream);

            PageResult<M_MonthCostProduction> pageResult = new PageResult<M_MonthCostProduction>(0,new List<M_MonthCostProduction>());
            if(string.IsNullOrEmpty(model.MonthId))
            {
                return Json(pageResult);
            }
            B_MonthCostProduction bMonthCostProduction = new B_MonthCostProduction();
            var pageData = bMonthCostProduction.GetPageData(model);
            var totalCount = bMonthCostProduction.GetPageDataTotalCount(model);

            pageResult = new PageResult<M_MonthCostProduction>(totalCount, pageData);
            return Json(pageResult);
        }

        /// <summary>
        /// 一级物料分页数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageDataForLevel1()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_MonthCostProduction model = JsonHandler.UnJson<M_MonthCostProduction>(strReqStream);

            PageResult<M_MonthCostProduction> pageResult = new PageResult<M_MonthCostProduction>(0, new List<M_MonthCostProduction>());
            if (string.IsNullOrEmpty(model.MonthId))
            {
                return Json(pageResult);
            }
            B_MonthCostProduction bMonthCostProduction = new B_MonthCostProduction();
            var pageData = bMonthCostProduction.GetLevel1PageData(model);
            var totalCount = bMonthCostProduction.GetPageDataTotalCount(model);

            pageResult = new PageResult<M_MonthCostProduction>(totalCount, pageData);
            return Json(pageResult);
        }


        /// <summary>
        /// 实际成本分页数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageDataForAct()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_MonthCostProduction model = JsonHandler.UnJson<M_MonthCostProduction>(strReqStream);

            PageResult<M_MonthCostProduction> pageResult = new PageResult<M_MonthCostProduction>(0, new List<M_MonthCostProduction>());
            if (string.IsNullOrEmpty(model.MonthId))
            {
                return Json(pageResult);
            }
            B_MonthCostProduction bMonthCostProduction = new B_MonthCostProduction();
            var pageData = bMonthCostProduction.GetActPageData(model);
            var totalCount = pageData.Count;

            pageData = pageData.OrderBy(v=>v.ProdId).ToList();
            pageResult = new PageResult<M_MonthCostProduction>(totalCount, pageData);
            return Json(pageResult);
        }

    }
}