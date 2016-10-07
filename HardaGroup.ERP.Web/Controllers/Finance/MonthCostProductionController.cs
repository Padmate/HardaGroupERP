﻿using HardaGroup.ERP.Models;
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

            return View();
        }

        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_MonthCostProduction model = JsonHandler.UnJson<M_MonthCostProduction>(strReqStream);

            B_MonthCostProduction bMonthCostProduction = new B_MonthCostProduction();
            var pageData = bMonthCostProduction.GetPageData(model);
            var totalCount = bMonthCostProduction.GetPageDataTotalCount(model);

            PageResult<M_MonthCostProduction> pageResult = new PageResult<M_MonthCostProduction>(totalCount, pageData);
            return Json(pageResult);
        }

    }
}