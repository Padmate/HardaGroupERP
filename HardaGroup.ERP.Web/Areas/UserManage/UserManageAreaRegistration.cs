using System.Web.Mvc;

namespace HardaGroup.ERP.Web.Areas.UserManage
{
    public class UserManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "UserManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "UserManage_default",
                "UserManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "HardaGroup.ERP.Web.Controllers.UserManage" }
            );
        }
    }
}