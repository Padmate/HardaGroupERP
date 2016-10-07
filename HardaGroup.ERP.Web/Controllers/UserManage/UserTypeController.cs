using HardaGroup.ERP.Models;
using HardaGroup.ERP.Service;
using HardaGroup.ERP.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.ERP.Web.Controllers.UserManage
{
    public class UserTypeController:BaseController
    {
        #region 
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_UserType model = JsonHandler.UnJson<M_UserType>(strReqStream);

            B_UserType bUserType = new B_UserType();
            var pageData = bUserType.GetPageData(model);
            var totalCount = bUserType.GetPageDataTotalCount(model);

            PageResult<M_UserType> pageResult = new PageResult<M_UserType>(totalCount, pageData);
            return Json(pageResult);
        }

        public ActionResult Add()
        {
            return View();
        }

        // POST:
        [HttpPost]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_UserType model = JsonHandler.UnJson<M_UserType>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_UserType bUserType = new B_UserType();
            //判断是否已存在该
            var exist = bUserType.GetByCode(model.Code.Trim());
            if (exist != null)
            {
                message.Success = false;
                message.Content = "已存在列别代码为：" + model.Code + "的数据，不能重复添加。";
                return Json(message);
            }

            message = bUserType.AddUserType(model);

            return Json(message);
        }


        public ActionResult Edit(string id)
        {
            B_UserType bUserType = new B_UserType();

            Int32 userTypeId = System.Convert.ToInt32(id);
            var userType = bUserType.GetById(userTypeId);

            ViewData["UserType"] = userType;

            return View();
        }

        // POST:
        [HttpPost]
        public ActionResult SaveEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_UserType model = JsonHandler.UnJson<M_UserType>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_UserType bUserType = new B_UserType();
            //修改前判断当前将要更新的是否已存在系统中
            var exist = bUserType.GetByCode(model.Code.Trim());
            if (exist != null && exist.Id != model.Id)
            {
                message.Success = false;
                message.Content = "已存在类别代码为：" + model.Code + "的数据。";
                return Json(message);

            }

            message = bUserType.EditUserType(model);

            return Json(message);
        }

        [HttpPost]
        public ActionResult DeleteById(string Id)
        {
            Message message = new Message();

            var intId = System.Convert.ToInt32(Id);
            B_UserType bUserType = new B_UserType();
            message = bUserType.DeleteById(intId);
            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> userTypeIds = JsonHandler.UnJson<List<string>>(strReqStream);

            List<int> ids = new List<int>();
            foreach (var userTypeid in userTypeIds)
            {
                ids.Add(System.Convert.ToInt32(userTypeid));
            }
            Message message = new Message();
            B_UserType bUserType = new B_UserType();
            message = bUserType.BatchDeleteByIds(ids);
            return Json(message);
        }

        #endregion
    }
}