using HardaGroup.ERP.DataAccess;
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

    }
}