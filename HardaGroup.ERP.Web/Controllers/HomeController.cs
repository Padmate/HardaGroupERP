using HardaGroup.ERP.Models;
using HardaGroup.ERP.Service;
using HardaGroup.ERP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.ERP.Web.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Default()
        {

            return View();
        }

        [Authorization]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载树形菜单
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public ActionResult LoadTreeData()
        {
            //当前登录用户
            var user = GetCurrentUser();
            //根据当前用户的角色查找菜单
            string userType = "admin";
            var treeData = TreeConfig.Init.TreeDatas(userType);


            return Json(treeData.children);
        }

        /// <summary>
        /// 通知公告
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public ActionResult Announcement()
        {

            return View();
        }


    }
}