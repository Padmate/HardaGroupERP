using HardaGroup.ERP.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace HardaGroup.ERP.Web.Filters
{
    public class AuthorizationAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        /// <summary>
        /// 执行前进行session校验
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string returnUrl = filterContext.HttpContext.Request.RawUrl;
            if (!CheckSession())
            {
                var strContent = "<html xmlns='http://www.w3.org/1999/xhtml' >";
                strContent += "<head runat='server'>";
                strContent += "<title>登录超时</title>";
                strContent += "</head>";
                strContent += "<body>";
                strContent += "<script>";
                strContent += "alert('对不起，登录超时，请重新登录！');";
                strContent += "top.window.location='/';";

                strContent += "</script>";
                strContent += "</body>";
                strContent += "</html>";

                filterContext.HttpContext.Response.Write(strContent);
                filterContext.HttpContext.Response.End();

                // 返回一个空的Result，否则在完成输出之前
                filterContext.Result = new EmptyResult();

            }
        }

        #region 检查Session是否过期

        /// <summary>
        /// 检查Session是否过期
        /// </summary>
        /// <returns></returns>
        public bool CheckSession()
        {
            HttpSessionState Session = HttpContext.Current.Session;
            if (Session[Common.SessionLoginUser] != null)
            {
                return true;
            }

            return false;
        }


        #endregion

    }
}