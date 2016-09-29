using HardaGroup.ERP.Models;
using HardaGroup.ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.ERP.Web.Controllers
{
    public class BaseController:Controller
    {
        /// <summary>
        /// 获取当前登录的用户信息
        /// </summary>
        /// <returns></returns>
        public LoginUser GetCurrentUser()
        {
            LoginUser user = null;
            if (Session != null)
            {
                user = (LoginUser)Session[Common.SessionLoginUser];
            }

            return user;
        }
    }
}