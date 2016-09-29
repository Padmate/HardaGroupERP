using HardaGroup.ERP.Models;
using HardaGroup.ERP.Utility;
using HardaGroup.ERP.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HardaGroup.ERP.Web.Controllers
{
    public class AccountController:BaseController
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            Message message = new Message();
            message = model.validate();
            if (!message.Success)
            {
                return Json(message);
            }
            //校验系统中是否存在该用户

            //校验密码
            //对客户端密码进行再次Hash
            var loginHashPassword = PasswordHash.CreatePasswordAndSaltHash(model.Password);

            //判断登录密码是否正确 admin/123456
            if (!PasswordHash.ValidatePassword(model.Password, "MTAwMDpKOHBCUjFUUGdQdWR6c2lzYnhyQmNUbHFyKzVORWUxVjpKanZYVW9YcDQyUVJyZUkwNlNuaEExdG82c0wyRE1reg=="))
            {
                message.Success = false;
                message.Content = "用户名或密码不正确";
                return Json(message);
            }

            //如果用户登录成功，则将用户信息写入session
            LoginUser user = new LoginUser();
            user.UserName = model.UserName;
            //设置过期时间为4H
            Session[Common.SessionLoginUser] = user;
            Session.Timeout = 240;


            return Json(message);
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            ClearUserSessionInfo();
            return RedirectToAction("Default", "Home");
        }


        [Authorization]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            Message message = new Message();
            message = model.validate();
            if (!message.Success) return Json(message);
            if (model.NewPassword != model.ConfirmPassword)
            {
                message.Success = false;
                message.Content = "新密码与确认密码不匹配";
                return Json(message);
            }

            //获取当前登录的用户信息
            var loginUser = this.GetCurrentUser();

            //根据用户名从数据库中取出该条用户数据
            var databasePassword = "";


            //如果当前数据中密码为空，则构造系统默认密码:123456进行比较
            if (string.IsNullOrEmpty(databasePassword))
            {
                var defaultPassword = PasswordHash.CreatePasswordHash(loginUser.UserName, "123456");
                databasePassword = PasswordHash.CreatePasswordAndSaltHash(defaultPassword);
            }

            //判断登录密码是否正确
            if (!PasswordHash.ValidatePassword(model.Password, databasePassword))
            {
                message.Success = false;
                message.Content = "当前密码不正确";
                return Json(message);
            }

            //保存新密码
            var clientHashNewPassword = PasswordHash.CreatePasswordHash(loginUser.UserName, model.NewPassword);
            var hashNewPassword = PasswordHash.CreatePasswordAndSaltHash(clientHashNewPassword);

            //清楚Session
            ClearUserSessionInfo();


            return Json(message);
        }

        private void ClearUserSessionInfo()
        {
            if (Session != null)
            {
                try
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                }
                catch { }
            }
        }
    }
}